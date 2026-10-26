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
    public class ComplaintAgent : BaseAgent , IAgent    
    {
        public ComplaintAgent(AgentChatCoordinator agentChatCoordinator, ToolChatCoordinator toolChatCoordinator, IConfiguration configuration
            , ComplaintAgentChat complaintAgentChat, NotificationService notificationService)
            : base(agentChatCoordinator, toolChatCoordinator, configuration, complaintAgentChat, notificationService) { }

        protected override eAgentType _AgentType => eAgentType.ComplaintAgent;
    }
}
