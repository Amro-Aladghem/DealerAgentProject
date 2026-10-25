using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Stores
{
    public class CreateStoreTool : BaseTool, IAgentTool
    {
        private readonly StoreService _storeService;

        public CreateStoreTool(StoreService storeService)
        {
            _storeService = storeService;
        }

        public string Name => "CreateStoreTool";
        public Type ArgType => typeof(CreateStoreToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            CreateStoreToolArg createStoreToolArg = (CreateStoreToolArg)arg;

            return await ExecuteTool<StoreDto>(() =>
            _storeService.CreateStore(createStoreToolArg.storeDto), Name);
        }
    }

    public class CreateStoreToolArg()
    {
        public StoreDto storeDto { get; set; } = null!;
    }
}
