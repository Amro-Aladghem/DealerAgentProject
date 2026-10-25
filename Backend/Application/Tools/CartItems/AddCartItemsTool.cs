using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.CartItems
{
    public class AddCartItemsTool : BaseTool, IAgentTool
    {
        private readonly CartItemService _cartItemService;

        public AddCartItemsTool(CartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        public string Name => "AddCartItemsTool";
        public Type ArgType => typeof(AddCartItemsToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            AddCartItemsToolArg addCartItemsToolArg = (AddCartItemsToolArg)arg;

            return await ExecuteTool<List<CartItemDto>?>(() =>
            _cartItemService.AddCartItems(addCartItemsToolArg.cartId, addCartItemsToolArg.storeId, addCartItemsToolArg.cartItemDtos), Name);
        }
    }

    public class AddCartItemsToolArg()
    {
        public Guid cartId { get; set; }
        public Guid storeId { get; set; }
        public List<CartItemDto> cartItemDtos { get; set; } = null!;
    }
}
