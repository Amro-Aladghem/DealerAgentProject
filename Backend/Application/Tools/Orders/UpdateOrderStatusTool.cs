using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Orders
{
    public class UpdateOrderStatusTool : BaseTool, IAgentTool
    {
        private readonly OrderService _orderService;

        public UpdateOrderStatusTool(OrderService orderService)
        {
            _orderService = orderService;
        }

        public string Name => "UpdateOrderStatusTool";
        public Type ArgType => typeof(UpdateOrderStatusToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            UpdateOrderStatusToolArg updateOrderStatusToolArg = (UpdateOrderStatusToolArg)arg;

            return await ExecuteTool<OrderDto?>(() =>
            _orderService.UpdateOrderStatus(updateOrderStatusToolArg.storeId, updateOrderStatusToolArg.id, updateOrderStatusToolArg.status), Name);
        }
    }

    public class UpdateOrderStatusToolArg()
    {
        public Guid storeId { get; set; }
        public Guid id { get; set; }
        public eOrderStatus status { get; set; }
    }
}
