using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Carts
{
    public class GetCartsForStoreTool : BaseTool, IAgentTool
    {
        private readonly CartService _cartService;

        public GetCartsForStoreTool(CartService cartService)
        {
            _cartService = cartService;
        }

        public string Name => "GetCartsForStoreTool";
        public Type ArgType => typeof(GetCartsForStoreToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetCartsForStoreToolArg getCartsForStoreToolArg = (GetCartsForStoreToolArg)arg;

            return await ExecuteTool<List<CartDto>>(() =>
            _cartService.GetCartsForStore(getCartsForStoreToolArg.storeId), Name);
        }
    }

    public class GetCartsForStoreToolArg()
    {
        public Guid storeId { get; set; }
    }
}
