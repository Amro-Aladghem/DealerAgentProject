using Application.Agents;
using Application.Dtos;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Coordinators
{
    public class AgentExecutionService
    {
        private readonly AgentOrchestratorService _agentOrchestratorService;

        public AgentExecutionService(AgentOrchestratorService agentOrchestratorService)
        {
            _agentOrchestratorService = agentOrchestratorService;
        }
        public async Task<AgentRunResultDto> ExecutAgent(AgentRequestDto agentRequestDto,string agentType)
        {
            return await _agentOrchestratorService.ExecuteAgentic(agentRequestDto);
        }
    }
}
