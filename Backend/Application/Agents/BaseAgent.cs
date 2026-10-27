using Application.Dtos;
using Application.Services;
using Application.Services.Coordinators;
using Application.Tools.Coordinator;
using Domain.Enums;
using Infrastructure.AI;
using Infrastructure.AI.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Application.Agents
{
    public abstract class BaseAgent : IAgent
    {
        private static readonly JsonSerializerOptions DataJsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        private readonly AgentChatCoordinator _agentChatCoordinator;
        private readonly ToolChatCoordinator _toolChatCoordinator;
        private readonly NotificationService _notificationService;
        private readonly IAgentChat _agentChat;

        protected abstract eAgentType _AgentType { get; }

        private int _aiRetryCount;
        public BaseAgent(
            AgentChatCoordinator agentChatCoordinator, 
            ToolChatCoordinator toolChatCoordinator,
            IConfiguration configuration,
            IAgentChat agentChat,
            NotificationService notificationService
         )
        {
            _agentChatCoordinator = agentChatCoordinator;
            _toolChatCoordinator = toolChatCoordinator;
            _agentChat = agentChat;
            _aiRetryCount = configuration.GetValue<int>("aiRetryCount");
            _notificationService = notificationService; 
        }

        public async Task<AgentRunResultDto> RunAsync(AgentRequestDto agentRequestDto)
        {
            AgentResponseDto? agentResponse = new AgentResponseDto();
            AgentRunResultDto agentRunResultDto = new AgentRunResultDto();

            for(int retries=0; retries <= _aiRetryCount; retries++)
            {
                agentResponse = await _agentChatCoordinator.GenerateAgentResponse
                (
                    agentRequestDto.ChatSignture!.Value,
                    agentRequestDto.StoreId,
                    _agentChat
                );

                if (agentResponse is null)
                    continue;

                agentRunResultDto = await HandleAgentResponse
                (
                    agentResponse,
                    agentRequestDto.StoreId,
                    agentRequestDto.ChatSignture.Value
                );

                return agentRunResultDto;
            }

            agentRunResultDto.HasError = true;
            return agentRunResultDto;
        }

        private async Task<AgentRunResultDto> HandleAgentResponse(AgentResponseDto agentResponseDto, Guid inventoryId, Guid chatSignture)
        {
            AgentRunResultDto agentRunResultDto = new AgentRunResultDto();
            agentRunResultDto.Message = agentResponseDto.Message;
            agentRunResultDto.AgentTargetName = agentResponseDto.TargetAgentName;

            if (agentResponseDto.HasError)
                return await GetResultWhenHasError(
                    agentResponseDto,
                    inventoryId,
                    chatSignture
                );

            if (agentResponseDto.IsFinished)
                return await GetResultWhenIsFinished(
                     agentResponseDto,
                     inventoryId,
                     chatSignture
                );

            if (agentResponseDto.AskUser)
                return await GetResultWhenAskUser(
                    agentResponseDto,
                    inventoryId,
                    chatSignture
                );

            if (!string.IsNullOrEmpty(agentResponseDto.ToolName))
            {
                await _toolChatCoordinator.HandleToolExecuteAndSave
                (
                    agentResponseDto.ToolName,
                    agentResponseDto.ArgumentData ?? new JsonElement(),
                    chatSignture:chatSignture,
                    inventoryId:inventoryId,
                    agentType:_AgentType
                );
            }

            return agentRunResultDto;
        }

        private async Task<AgentRunResultDto> GetResultWhenHasError(AgentResponseDto agentResponseDto,Guid inventoryId, Guid chatSignture)
        {
            AgentRunResultDto agentRunResultDto = new AgentRunResultDto();
            agentRunResultDto.HasError = true;

            await _notificationService.CreateNotification(inventoryId, new NotificationDto()
            {
                ChatId = chatSignture,
                AgentType = _AgentType.ToString(),
                StoreId = inventoryId,
                NotificationStatus = eNotificationStatus.Failed.ToString(),
                Text = agentResponseDto.Message,
            });

            return agentRunResultDto;
        }

        private async Task<AgentRunResultDto> GetResultWhenIsFinished(AgentResponseDto agentResponseDto, Guid inventoryId, Guid chatSignture)
        {
            AgentRunResultDto agentRunResultDto = new AgentRunResultDto();
            agentRunResultDto.IsFinished = true;
            agentRunResultDto.Message = agentResponseDto.Message;
            agentRunResultDto.Data = agentResponseDto.Data;

            await _notificationService.CreateNotification(inventoryId, new NotificationDto()
            {
                ChatId = chatSignture,
                AgentType = _AgentType.ToString(),
                StoreId = inventoryId,
                NotificationStatus = eNotificationStatus.Completed.ToString(),
                Data= JsonSerializer.Serialize(agentResponseDto.Data, DataJsonOptions),
                Text = agentResponseDto.Message,
            });

            return agentRunResultDto;
        }

        private async Task<AgentRunResultDto> GetResultWhenAskUser(AgentResponseDto agentResponseDto, Guid inventoryId, Guid chatSignture)
        {
            AgentRunResultDto agentRunResultDto = new AgentRunResultDto();
            agentRunResultDto.AskUser = true;
            agentRunResultDto.Message = agentResponseDto.Message;
            agentRunResultDto.Data = agentResponseDto.Data;

            await _notificationService.CreateNotification(inventoryId, new NotificationDto()
            {
                ChatId = chatSignture,
                AgentType = _AgentType.ToString(),
                StoreId = inventoryId,
                NotificationStatus = eNotificationStatus.WaitingForUser.ToString(),
                Text = agentResponseDto.Message,
                Data = JsonSerializer.Serialize(agentResponseDto.Data, DataJsonOptions),
            });

            return agentRunResultDto;
        }
    }
}
