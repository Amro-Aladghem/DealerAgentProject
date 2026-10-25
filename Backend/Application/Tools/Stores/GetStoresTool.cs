using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Stores
{
    public class GetStoresTool : BaseTool, IAgentTool
    {
        private readonly StoreService _storeService;

        public GetStoresTool(StoreService storeService)
        {
            _storeService = storeService;
        }

        public string Name => "GetStoresTool";
        public Type ArgType => typeof(GetStoresToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetStoresToolArg getStoresToolArg = (GetStoresToolArg)arg;

            return await ExecuteTool<List<StoreDto>>(() =>
            _storeService.GetStores(), Name);
        }
    }

    public class GetStoresToolArg()
    {

    }
}
