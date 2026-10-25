using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.OrderItems
{
    public class AddOrderItemTool : BaseTool, IAgentTool
    {
        private readonly OrderItemService _orderItemService;

        public AddOrderItemTool(OrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        public string Name => "AddOrderItemTool";
        public Type ArgType => typeof(AddOrderItemToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            AddOrderItemToolArg addOrderItemToolArg = (AddOrderItemToolArg)arg;

            return await ExecuteTool<OrderItemDto?>(() =>
            _orderItemService.AddOrderItem(addOrderItemToolArg.orderId, addOrderItemToolArg.storeId, addOrderItemToolArg.orderItemDto), Name);
        }
    }

    public class AddOrderItemToolArg()
    {
        public Guid orderId { get; set; }
        public Guid storeId { get; set; }
        public OrderItemDto orderItemDto { get; set; } = null!;
    }
}
