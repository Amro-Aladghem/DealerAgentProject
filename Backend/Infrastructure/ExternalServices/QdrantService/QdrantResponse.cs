using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.ExternalServices.QdrantServices
{
    public class QdrantResponse
    {
        public List<QdrantResult> Result { get; set; } = new();
    }

    public class QdrantResult
    {
        public Guid Id { get; set; }
        public float Score { get; set; }
        public Dictionary<string, JsonElement>? Payload { get; set; } 
    }
}
