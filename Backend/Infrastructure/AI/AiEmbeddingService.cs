using Infrastructure.AI.Interfaces;
using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.AI
{

    public class AiEmbeddingService : IAiEmbeddingService
    {
        private readonly IConfiguration _configuration;
        private readonly GoogleAI _googleAi;
        private readonly GenerativeModel _model;

        public AiEmbeddingService(IConfiguration configuration, GoogleAI googleAI)
        {
            _configuration = configuration;
            _googleAi = googleAI;
            _model = _googleAi.GenerativeModel(model: Model.GeminiEmbedding001);
        }

        public async Task<List<float>> GetEmbeddingTextResult(string text)
        {
            EmbedContentRequest request = new EmbedContentRequest("")
            {
                OutputDimensionality = 128,
                Model = Model.GeminiEmbedding,
                Content = new ContentResponse(text),
                TaskType = TaskType.RetrievalQuery
            };

            var response = await _model.EmbedContent(request);

            return response?.Embedding?.Values ?? new List<float>();
        }
    }
}
