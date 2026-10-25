using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Orders
{
    public class CreateOrderTool : BaseTool, IAgentTool
    {
        private readonly OrderService _orderService;

        public CreateOrderTool(OrderService orderService)
        {
            _orderService = orderService;
        }

        public string Name => "CreateOrderTool";
        public Type ArgType => typeof(CreateOrderToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            CreateOrderToolArg createOrderToolArg = (CreateOrderToolArg)arg;

            return await ExecuteTool<OrderDto>(() =>
            _orderService.CreateOrder(createOrderToolArg.storeId, createOrderToolArg.orderDto), Name);
        }
    }

    public class CreateOrderToolArg()
    {
        public Guid storeId { get; set; }
        public OrderDto orderDto { get; set; } = null!;
    }
}
