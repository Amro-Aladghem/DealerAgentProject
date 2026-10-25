using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Carts
{
    public class GetCartForStoreTool : BaseTool, IAgentTool
    {
        private readonly CartService _cartService;

        public GetCartForStoreTool(CartService cartService)
        {
            _cartService = cartService;
        }

        public string Name => "GetCartForStoreTool";
        public Type ArgType => typeof(GetCartForStoreToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetCartForStoreToolArg getCartForStoreToolArg = (GetCartForStoreToolArg)arg;

            return await ExecuteTool<CartDto?>(() =>
            _cartService.GetCartForStore(getCartForStoreToolArg.storeId), Name);
        }
    }

    public class GetCartForStoreToolArg()
    {
        public Guid storeId { get; set; }
    }
}
