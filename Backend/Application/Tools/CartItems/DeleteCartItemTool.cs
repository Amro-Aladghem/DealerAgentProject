using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.CartItems
{
    public class DeleteCartItemTool : BaseTool, IAgentTool
    {
        private readonly CartItemService _cartItemService;

        public DeleteCartItemTool(CartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        public string Name => "DeleteCartItemTool";
        public Type ArgType => typeof(DeleteCartItemToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            DeleteCartItemToolArg deleteCartItemToolArg = (DeleteCartItemToolArg)arg;

            return await ExecuteTool<bool>(() =>
            _cartItemService.DeleteCartItem(deleteCartItemToolArg.cartId, deleteCartItemToolArg.storeId, deleteCartItemToolArg.id), Name);
        }
    }

    public class DeleteCartItemToolArg()
    {
        public Guid cartId { get; set; }
        public Guid storeId { get; set; }
        public Guid id { get; set; }
    }
}
