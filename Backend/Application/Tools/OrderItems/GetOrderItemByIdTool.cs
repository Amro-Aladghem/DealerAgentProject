using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.OrderItems
{
    public class GetOrderItemByIdTool : BaseTool, IAgentTool
    {
        private readonly OrderItemService _orderItemService;

        public GetOrderItemByIdTool(OrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        public string Name => "GetOrderItemByIdTool";
        public Type ArgType => typeof(GetOrderItemByIdToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetOrderItemByIdToolArg getOrderItemByIdToolArg = (GetOrderItemByIdToolArg)arg;

            return await ExecuteTool<OrderItemDto?>(() =>
            _orderItemService.GetOrderItemById(getOrderItemByIdToolArg.orderId, getOrderItemByIdToolArg.storeId, getOrderItemByIdToolArg.id), Name);
        }
    }

    public class GetOrderItemByIdToolArg()
    {
        public Guid orderId { get; set; }
        public Guid storeId { get; set; }
        public Guid id { get; set; }
    }
}
