using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.OrderItems
{
    public class UpdateOrderItemTool : BaseTool, IAgentTool
    {
        private readonly OrderItemService _orderItemService;

        public UpdateOrderItemTool(OrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        public string Name => "UpdateOrderItemTool";
        public Type ArgType => typeof(UpdateOrderItemToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            UpdateOrderItemToolArg updateOrderItemToolArg = (UpdateOrderItemToolArg)arg;

            return await ExecuteTool<OrderItemDto?>(() =>
            _orderItemService.UpdateOrderItem(updateOrderItemToolArg.orderId, updateOrderItemToolArg.storeId, updateOrderItemToolArg.id, updateOrderItemToolArg.orderItemDto), Name);
        }
    }

    public class UpdateOrderItemToolArg()
    {
        public Guid orderId { get; set; }
        public Guid storeId { get; set; }
        public Guid id { get; set; }
        public OrderItemDto orderItemDto { get; set; } = null!;
    }
}
