using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Stores
{
    public class GetStoreByNameTool : BaseTool, IAgentTool
    {
        private readonly StoreService _storeService;

        public GetStoreByNameTool(StoreService storeService)
        {
            _storeService = storeService;
        }

        public string Name => "GetStoreByNameTool";
        public Type ArgType => typeof(GetStoreByNameToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetStoreByNameToolArg getStoreByNameToolArg = (GetStoreByNameToolArg)arg;

            return await ExecuteTool<StoreDto?>(() =>
            _storeService.GetStoreByName(getStoreByNameToolArg.name), Name);
        }
    }

    public class GetStoreByNameToolArg()
    {
        public string name { get; set; } = null!;
    }
}
