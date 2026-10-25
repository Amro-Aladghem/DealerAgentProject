using Application.Tools.Interfaces;
using Services.ExternalServices.QdrantServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools
{
    public class QdrantProductSearchTool : BaseTool, IAgentTool
    {
        private readonly QdrantSearchService _qdrantSearchService;

        public QdrantProductSearchTool(QdrantSearchService qdrantSearchService)
        {
            _qdrantSearchService = qdrantSearchService;
        }

        public string Name => "QdrantProductSearchTool";

        public Type ArgType => typeof(QdrantProductSearchToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            var QdrantProductSearchToolArg = (QdrantProductSearchToolArg)arg;

            return await ExecuteTool<string>(()=>
            _qdrantSearchService.HandleGetSearchTasksResults(QdrantProductSearchToolArg.searchText),Name);
        }
    }

    public class QdrantProductSearchToolArg()
    {
        public string searchText { get; set; }
    }
}
