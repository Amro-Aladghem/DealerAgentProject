using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.CartItems
{
    public class AddCartItemTool : BaseTool, IAgentTool
    {
        private readonly CartItemService _cartItemService;

        public AddCartItemTool(CartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        public string Name => "AddCartItemTool";
        public Type ArgType => typeof(AddCartItemToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            AddCartItemToolArg addCartItemToolArg = (AddCartItemToolArg)arg;

            return await ExecuteTool<CartItemDto?>(() =>
            _cartItemService.AddCartItem(addCartItemToolArg.cartId, addCartItemToolArg.storeId, addCartItemToolArg.cartItemDto), Name);
        }
    }

    public class AddCartItemToolArg()
    {
        public Guid cartId { get; set; }
        public Guid storeId { get; set; }
        public CartItemDto cartItemDto { get; set; } = null!;
    }
}
