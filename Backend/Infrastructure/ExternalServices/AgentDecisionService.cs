using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Infrastructure.ExternalServices
{
    public class AgentDecisionService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AgentDecisionService(HttpClient httpClient,IConfiguration configuration)
        {  
            _httpClient = httpClient; 
            _configuration = configuration;
        }

        public async Task<string> GetAgentTypeAsync(string message,
            CancellationToken cancellationToken = default)
        {
            var apiKey = _configuration["OpenRouter:ApiKey"]
               ?? throw new InvalidOperationException(
                   "OpenRouter API key is missing.");


            using var request = new HttpRequestMessage(HttpMethod.Post,
                "https://openrouter.ai/api/alpha/decisions");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            request.Content = JsonContent.Create(new
            {
                model = "microsoft/microsoft-decision-1",

                state = message,

                questions = new
                {
                    agent = new
                    {
                      
                    }
                }
            });

            var response = await _httpClient.SendAsync(request, cancellationToken);

            response.EnsureSuccessStatusCode();

            using var document = await JsonDocument.ParseAsync(
               await response.Content.ReadAsStreamAsync(
                   cancellationToken),
               cancellationToken: cancellationToken);

            var agentName = document.RootElement
                .GetProperty("answers")
                .GetProperty("agent")
                .GetProperty("choice")
                .GetString();

            return agentName switch
            {
                _ => throw new InvalidOperationException(
                    $"Unknown agent returned by decision model: {agentName}")
            };
        }
    }
}
