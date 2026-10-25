using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.CartItems
{
    public class DeleteAllCartItemsTool : BaseTool, IAgentTool
    {
        private readonly CartItemService _cartItemService;

        public DeleteAllCartItemsTool(CartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        public string Name => "DeleteAllCartItemsTool";
        public Type ArgType => typeof(DeleteAllCartItemsToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            DeleteAllCartItemsToolArg deleteAllCartItemsToolArg = (DeleteAllCartItemsToolArg)arg;

            return await ExecuteTool<bool>(() =>
            _cartItemService.DeleteAllCartItems(deleteAllCartItemsToolArg.cartId, deleteAllCartItemsToolArg.storeId), Name);
        }
    }

    public class DeleteAllCartItemsToolArg()
    {
        public Guid cartId { get; set; }
        public Guid storeId { get; set; }
    }
}
