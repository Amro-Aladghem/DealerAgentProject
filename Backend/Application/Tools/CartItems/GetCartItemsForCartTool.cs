using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.CartItems
{
    public class GetCartItemsForCartTool : BaseTool, IAgentTool
    {
        private readonly CartItemService _cartItemService;

        public GetCartItemsForCartTool(CartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        public string Name => "GetCartItemsForCartTool";
        public Type ArgType => typeof(GetCartItemsForCartToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetCartItemsForCartToolArg getCartItemsForCartToolArg = (GetCartItemsForCartToolArg)arg;

            return await ExecuteTool<List<CartItemDto>>(() =>
            _cartItemService.GetCartItemsForCart(getCartItemsForCartToolArg.cartId, getCartItemsForCartToolArg.storeId), Name);
        }
    }

    public class GetCartItemsForCartToolArg()
    {
        public Guid cartId { get; set; }
        public Guid storeId { get; set; }
    }
}
