using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.CartItems
{
    public class UpdateCartItemTool : BaseTool, IAgentTool
    {
        private readonly CartItemService _cartItemService;

        public UpdateCartItemTool(CartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        public string Name => "UpdateCartItemTool";
        public Type ArgType => typeof(UpdateCartItemToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            UpdateCartItemToolArg updateCartItemToolArg = (UpdateCartItemToolArg)arg;

            return await ExecuteTool<CartItemDto?>(() =>
            _cartItemService.UpdateCartItem(updateCartItemToolArg.cartId, updateCartItemToolArg.storeId, updateCartItemToolArg.id, updateCartItemToolArg.cartItemDto), Name);
        }
    }

    public class UpdateCartItemToolArg()
    {
        public Guid cartId { get; set; }
        public Guid storeId { get; set; }
        public Guid id { get; set; }
        public CartItemDto cartItemDto { get; set; } = null!;
    }
}
