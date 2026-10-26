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
    public class ProductsSearchAgent : BaseAgent, IAgent
    {
        public ProductsSearchAgent(AgentChatCoordinator agentChatCoordinator, ToolChatCoordinator toolChatCoordinator, IConfiguration configuration
            , ProductsSearchAgentChat productSearchAgentChat, NotificationService notificationService)
            : base(agentChatCoordinator, toolChatCoordinator, configuration, productSearchAgentChat, notificationService) { }

        protected override eAgentType _AgentType => eAgentType.ProductsSearchAgent;
    }
}
