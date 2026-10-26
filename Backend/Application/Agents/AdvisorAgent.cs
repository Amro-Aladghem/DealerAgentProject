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
    public class AdvisorAgent : BaseAgent, IAgent
    {
        public AdvisorAgent(AgentChatCoordinator agentChatCoordinator, ToolChatCoordinator toolChatCoordinator, IConfiguration configuration
            , AdvisorAgentChat advisorAgentChat, NotificationService notificationService)
            : base(agentChatCoordinator, toolChatCoordinator, configuration, advisorAgentChat, notificationService) { }

        protected override eAgentType _AgentType => eAgentType.AdvisorAgent;
    }
}
