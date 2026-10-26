using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos
{
    public class AgentRunResultDto
    {
        public bool IsFinished { get; set; } = false;
        public string Message { get; set; }
        public bool AskUser { get; set; } = false;
        public bool HasError { get; set; } = false;
        public AgentResponseData Data { get; set; } 
        public string? AgentTargetName { get; set; }
    }
}
