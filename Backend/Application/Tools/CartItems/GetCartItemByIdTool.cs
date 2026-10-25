using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.CartItems
{
    public class GetCartItemByIdTool : BaseTool, IAgentTool
    {
        private readonly CartItemService _cartItemService;

        public GetCartItemByIdTool(CartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        public string Name => "GetCartItemByIdTool";
        public Type ArgType => typeof(GetCartItemByIdToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetCartItemByIdToolArg getCartItemByIdToolArg = (GetCartItemByIdToolArg)arg;

            return await ExecuteTool<CartItemDto?>(() =>
            _cartItemService.GetCartItemById(getCartItemByIdToolArg.cartId, getCartItemByIdToolArg.storeId, getCartItemByIdToolArg.id), Name);
        }
    }

    public class GetCartItemByIdToolArg()
    {
        public Guid cartId { get; set; }
        public Guid storeId { get; set; }
        public Guid id { get; set; }
    }
}
