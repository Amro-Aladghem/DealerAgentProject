using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Stores
{
    public class UpdateStoreTool : BaseTool, IAgentTool
    {
        private readonly StoreService _storeService;

        public UpdateStoreTool(StoreService storeService)
        {
            _storeService = storeService;
        }

        public string Name => "UpdateStoreTool";
        public Type ArgType => typeof(UpdateStoreToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            UpdateStoreToolArg updateStoreToolArg = (UpdateStoreToolArg)arg;

            return await ExecuteTool<StoreDto?>(() =>
            _storeService.UpdateStore(updateStoreToolArg.id, updateStoreToolArg.storeDto), Name);
        }
    }

    public class UpdateStoreToolArg()
    {
        public Guid id { get; set; }
        public StoreDto storeDto { get; set; } = null!;
    }
}
