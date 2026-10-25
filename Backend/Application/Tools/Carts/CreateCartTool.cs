using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Carts
{
    public class CreateCartTool : BaseTool, IAgentTool
    {
        private readonly CartService _cartService;

        public CreateCartTool(CartService cartService)
        {
            _cartService = cartService;
        }

        public string Name => "CreateCartTool";
        public Type ArgType => typeof(CreateCartToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            CreateCartToolArg createCartToolArg = (CreateCartToolArg)arg;

            return await ExecuteTool<CartDto>(() =>
            _cartService.CreateCart(createCartToolArg.storeId), Name);
        }
    }

    public class CreateCartToolArg()
    {
        public Guid storeId { get; set; }
    }
}
