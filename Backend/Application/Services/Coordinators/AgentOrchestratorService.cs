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
        private readonly DealerAgent _carDealerAgent;
        private readonly AdvisorAgent _advisorAgent;
        private readonly ProductsSearchAgent _productsSearchAgent;
        private readonly ComplaintAgent _complaintAgent;

        private IAgent? _primaryRequestAgent;
        private IAgent? _currentWorkingAgent;

        public AgentOrchestratorService(DealerAgent carDealerAgent, AdvisorAgent advisorAgent, ProductsSearchAgent productsSearchAgent,
            ComplaintAgent complaintAgent)
        {
            _carDealerAgent = carDealerAgent;
            _advisorAgent = advisorAgent;
            _productsSearchAgent = productsSearchAgent;
            _complaintAgent = complaintAgent;
        }

        public async Task<AgentRunResultDto> ExecuteAgentic(AgentRequestDto agentRequestDto)
        {
            InitPrimaryAndCurrentAgent(agentRequestDto.AgentType!);

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
                    agentRunResultDto.IsFinished = false;
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
                DealerAgent => _carDealerAgent,
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
                "DealerAgent" => _carDealerAgent,
                "AdvisorAgent" => _advisorAgent,
                "ProductsSearchAgent"=>_productsSearchAgent,
                _ =>throw new Exception($"There is no sub agent with name: ${agentName}")
            };
        }

        private void InitPrimaryAndCurrentAgent(string agentName)
        {
            _primaryRequestAgent = agentName switch
            {
                "DealerAgent" => _carDealerAgent,
                "AdvisorAgent" => _advisorAgent,
                "ProductsSearchAgent" => _productsSearchAgent,
                "ComplaintAgent" => _complaintAgent,
                _ => throw new Exception($"There is no sub agent with name: ${agentName}")
            };
            _currentWorkingAgent = _primaryRequestAgent;
        }
        
        private bool IsCurrentEqualPrimary() 
            => _primaryRequestAgent == _currentWorkingAgent;
    }
}
