using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Application.Dtos
{
    public class AgentResponseDto
    {
        public bool IsFinished { get; set; }
        public string Message { get; set; }
        public string? ToolName { get; set; }
        public JsonElement? ArgumentData { get; set; }
        public bool AskUser { get; set; }
        public bool HasError { get; set; }
        public AgentResponseData Data { get; set; }
        public string? TargetAgentName { get; set; }
    }
}
