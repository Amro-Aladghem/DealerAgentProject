using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Orders
{
    public class UpdateOrderTool : BaseTool, IAgentTool
    {
        private readonly OrderService _orderService;

        public UpdateOrderTool(OrderService orderService)
        {
            _orderService = orderService;
        }

        public string Name => "UpdateOrderTool";
        public Type ArgType => typeof(UpdateOrderToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            UpdateOrderToolArg updateOrderToolArg = (UpdateOrderToolArg)arg;

            return await ExecuteTool<OrderDto?>(() =>
            _orderService.UpdateOrder(updateOrderToolArg.storeId, updateOrderToolArg.id, updateOrderToolArg.orderDto), Name);
        }
    }

    public class UpdateOrderToolArg()
    {
        public Guid storeId { get; set; }
        public Guid id { get; set; }
        public OrderDto orderDto { get; set; } = null!;
    }
}
