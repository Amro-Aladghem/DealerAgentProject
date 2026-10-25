using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Stores
{
    public class GetStoreByIdTool : BaseTool, IAgentTool
    {
        private readonly StoreService _storeService;

        public GetStoreByIdTool(StoreService storeService)
        {
            _storeService = storeService;
        }

        public string Name => "GetStoreByIdTool";
        public Type ArgType => typeof(GetStoreByIdToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetStoreByIdToolArg getStoreByIdToolArg = (GetStoreByIdToolArg)arg;

            return await ExecuteTool<StoreDto?>(() =>
            _storeService.GetStoreById(getStoreByIdToolArg.id), Name);
        }
    }

    public class GetStoreByIdToolArg()
    {
        public Guid id { get; set; }
    }
}
