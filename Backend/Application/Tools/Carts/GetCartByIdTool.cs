using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Carts
{
    public class GetCartByIdTool : BaseTool, IAgentTool
    {
        private readonly CartService _cartService;

        public GetCartByIdTool(CartService cartService)
        {
            _cartService = cartService;
        }

        public string Name => "GetCartByIdTool";
        public Type ArgType => typeof(GetCartByIdToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetCartByIdToolArg getCartByIdToolArg = (GetCartByIdToolArg)arg;

            return await ExecuteTool<CartDto?>(() =>
            _cartService.GetCartById(getCartByIdToolArg.storeId, getCartByIdToolArg.id), Name);
        }
    }

    public class GetCartByIdToolArg()
    {
        public Guid storeId { get; set; }
        public Guid id { get; set; }
    }
}
