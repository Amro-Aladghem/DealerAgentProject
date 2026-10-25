using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.OrderItems
{
    public class GetOrderItemsForOrderTool : BaseTool, IAgentTool
    {
        private readonly OrderItemService _orderItemService;

        public GetOrderItemsForOrderTool(OrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        public string Name => "GetOrderItemsForOrderTool";
        public Type ArgType => typeof(GetOrderItemsForOrderToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetOrderItemsForOrderToolArg getOrderItemsForOrderToolArg = (GetOrderItemsForOrderToolArg)arg;

            return await ExecuteTool<List<OrderItemDto>>(() =>
            _orderItemService.GetOrderItemsForOrder(getOrderItemsForOrderToolArg.orderId, getOrderItemsForOrderToolArg.storeId), Name);
        }
    }

    public class GetOrderItemsForOrderToolArg()
    {
        public Guid orderId { get; set; }
        public Guid storeId { get; set; }
    }
}
