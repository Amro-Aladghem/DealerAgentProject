using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.AI.Interfaces
{
    public interface IAiEmbeddingService
    {
        public Task<List<float>> GetEmbeddingTextResult(string text);
    }
}
