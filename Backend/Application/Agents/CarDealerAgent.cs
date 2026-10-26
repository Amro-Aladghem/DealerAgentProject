using Application.Services;
using Application.Services.Coordinators;
using Application.Tools.Coordinator;
using Domain.Enums;
using Infrastructure.AI.Chat;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Agents
{
    public class CarDealerAgent :BaseAgent,IAgent
    {
        public CarDealerAgent(AgentChatCoordinator agentChatCoordinator, ToolChatCoordinator toolChatCoordinator, IConfiguration configuration
            , DealerAgentChat dealerAgentChat, NotificationService notificationService)
            : base(agentChatCoordinator, toolChatCoordinator, configuration, dealerAgentChat, notificationService) { }

        protected override eAgentType _AgentType => eAgentType.DealerAgent;
    }
}
