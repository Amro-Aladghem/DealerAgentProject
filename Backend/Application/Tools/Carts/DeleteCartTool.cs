using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Carts
{
    public class DeleteCartTool : BaseTool, IAgentTool
    {
        private readonly CartService _cartService;

        public DeleteCartTool(CartService cartService)
        {
            _cartService = cartService;
        }

        public string Name => "DeleteCartTool";
        public Type ArgType => typeof(DeleteCartToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            DeleteCartToolArg deleteCartToolArg = (DeleteCartToolArg)arg;

            return await ExecuteTool<bool>(() =>
            _cartService.DeleteCart(deleteCartToolArg.storeId, deleteCartToolArg.id), Name);
        }
    }

    public class DeleteCartToolArg()
    {
        public Guid storeId { get; set; }
        public Guid id { get; set; }
    }
}
