
using Infrastructure.AI.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace Services.ExternalServices.QdrantServices
{
    public class QdrantSearchService
    {
        private readonly IAiEmbeddingService _aiEmbeddingService;
        private readonly IConfiguration _configuration;

        public QdrantSearchService(IAiEmbeddingService aiEmbeddingService,IConfiguration configuration)
        {
            _aiEmbeddingService = aiEmbeddingService;
            _configuration = configuration;
        }

        public async Task<string> SearchInQdrantAsync(float[] vector)
        {
            using var client = new HttpClient();
            var url = $"{_configuration["Qdrant:Url"]}/collections/{_configuration["Qdrant:CollectionName"]}/points/search";

            client.DefaultRequestHeaders.Add("api-key", _configuration["Qdrant:ApiKey"]);

            var requestBody = new
            {
                vector = vector,
                limit = 15,
                with_payload = false
            };

            var response = await client.PostAsJsonAsync(url, requestBody);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Qdrant Error: {await response.Content.ReadAsStringAsync()}");

            var qdrantData = await response.Content.ReadFromJsonAsync<QdrantResponse>();

            return JsonSerializer.Serialize(qdrantData);    
        }

        private string? GetString(Dictionary<string, JsonElement> payload, string key) =>
            payload.TryGetValue(key, out var el) ? el.GetString() : null;


        public async Task<string> HandleGetSearchTasksResults(string text)
        {
            var embeddingList = await _aiEmbeddingService.GetEmbeddingTextResult(text);
            var embedding = embeddingList.ToArray();

            return await SearchInQdrantAsync(embedding);
        }
    }
}