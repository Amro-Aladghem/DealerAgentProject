using Domain.Enums;
using Infrastructure.AI.Chat;
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
                        type = "choice",

                       instructions = """
                        Read the user's message and select the agent whose
                        responsibilities best match the user's primary intent.
                        Select exactly one option. Base your decision on the
                        user's actual intent, not merely on keywords in the message.
                        Route greetings, general dealership inquiries, and all
                        requests related to cars, car accessories, products, and
                        ordinary dealership operations to DealerAgent.
                        Route messages to ComplaintAgent only when the user
                        clearly expresses a complaint or reports a problem
                        they want addressed.
                        Select NoMatch when the user's request is unrelated to
                        the car dealership, its products, orders, or complaints.
                        """,

                        criteria = new Dictionary<string, string>
                        {
                           ["DealerAgent"] = """
                            Responsible for general customer interactions with
                            the car dealership.
                            Select this agent for greetings, welcome messages,
                            general dealership inquiries, car and car accessory
                            searches, product information, purchase-related
                            requests, and ordinary order inquiries or operations.
                            This is the default agent for dealership-related
                            requests unless the user clearly expresses a complaint
                            that should be handled by ComplaintAgent.
                            If the user only greets the system or starts a
                            conversation without expressing a complaint, choose
                            this agent.
                            """,

                           ["ComplaintAgent"] = """
                            Responsible for handling customer complaints and
                            problems related to dealership orders or services.
                            Select this agent when the user's message clearly
                            expresses dissatisfaction, reports a problem, or
                            explicitly requests help submitting or resolving
                            a complaint.
                            Do not select this agent for ordinary order status
                            inquiries or general product questions unless the
                            user clearly reports a problem or expresses a complaint.
                            """,

                            ["NoMatch"] =
                            """
                            The user's request is unrelated to the car dealership,
                            cars, car accessories, dealership products, orders,
                            or customer complaints.
                            Do not choose NoMatch for greetings, general
                            conversation starters, or any request related to
                            the dealership.
                            """
                        }
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
                "DealerAgent" => eAgentType.DealerAgent.ToString(),
                "ComplaintAgent" =>eAgentType.ComplaintAgent.ToString(),
                _ => throw new InvalidOperationException(
                    $"Unknown agent returned by decision model: {agentName}")
            };
        }
    }
}
