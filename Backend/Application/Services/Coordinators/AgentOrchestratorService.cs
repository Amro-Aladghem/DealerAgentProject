using Application.Agents;
using Application.Dtos;
using Domain.Enums;
using Infrastructure.AI.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Coordinators
{
    public class AgentOrchestratorService
    {
        private readonly CarDealerAgent _carDealerAgent;
        private readonly AdvisorAgent _advisorAgent;
        private readonly ProductsSearchAgent _productsSearchAgent;
        private readonly ComplaintAgent _complaintAgent;

        private readonly IAgent _primaryRequestAgent;
        private IAgent _currentWorkingAgent;

        public AgentOrchestratorService(IAgent primaryRequestAgent, CarDealerAgent carDealerAgent, AdvisorAgent advisorAgent, ProductsSearchAgent productsSearchAgent,
            ComplaintAgent complaintAgent)
        {
            _carDealerAgent = carDealerAgent;
            _advisorAgent = advisorAgent;
            _productsSearchAgent = productsSearchAgent;
            _complaintAgent = complaintAgent;

            _primaryRequestAgent = (BaseAgent)primaryRequestAgent;
            _currentWorkingAgent = primaryRequestAgent;
        }

        public async Task<AgentRunResultDto> ExecuteAgentic(AgentRequestDto agentRequestDto)
        {
            AgentRunResultDto agentRunResultDto = new AgentRunResultDto();
            BaseAgent currentAgent;

            do
            {
                currentAgent = GetCurrentRunningAgent();

                agentRunResultDto = await currentAgent.RunAsync(agentRequestDto);

                if (agentRunResultDto.HasError
                    || (agentRunResultDto.AskUser && IsCurrentEqualPrimary())
                    || (agentRunResultDto.IsFinished && IsCurrentEqualPrimary()))
                    return agentRunResultDto;


                if (agentRunResultDto.AskUser || agentRunResultDto.IsFinished)
                {
                    _currentWorkingAgent = _primaryRequestAgent;
                    continue;
                }

                if (!string.IsNullOrEmpty(agentRunResultDto.AgentTargetName))
                    SetRequestedSubAgentByName(agentRunResultDto.AgentTargetName);

            } while (!agentRunResultDto.IsFinished || !IsCurrentEqualPrimary());

            return agentRunResultDto;
        }

        private BaseAgent GetCurrentRunningAgent()
        {
            BaseAgent currentAgent = _currentWorkingAgent switch
            {
                CarDealerAgent => _carDealerAgent,
                AdvisorAgent => _advisorAgent,
                ProductsSearchAgent => _productsSearchAgent,
                ComplaintAgent => _complaintAgent,
                _ => throw new ArgumentOutOfRangeException(nameof(_currentWorkingAgent))
            };

            return currentAgent;
        }

        private void SetRequestedSubAgentByName(string agentName)
        {
            _currentWorkingAgent = agentName switch
            {
                "CarDealerAgent" => _carDealerAgent,
                "AdvisorAgent" => _advisorAgent,
                "ProductsSearchAgent"=>_productsSearchAgent,
                _=>throw new Exception($"There is no sub agent with name: ${agentName}")
            };
        }

        private bool IsCurrentEqualPrimary() 
            => _primaryRequestAgent == _currentWorkingAgent;
    }
}
