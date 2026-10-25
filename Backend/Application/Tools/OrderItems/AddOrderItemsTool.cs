using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.OrderItems
{
    public class AddOrderItemsTool : BaseTool, IAgentTool
    {
        private readonly OrderItemService _orderItemService;

        public AddOrderItemsTool(OrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        public string Name => "AddOrderItemsTool";
        public Type ArgType => typeof(AddOrderItemsToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            AddOrderItemsToolArg addOrderItemsToolArg = (AddOrderItemsToolArg)arg;

            return await ExecuteTool<List<OrderItemDto>?>(() =>
            _orderItemService.AddOrderItems(addOrderItemsToolArg.orderId, addOrderItemsToolArg.storeId, addOrderItemsToolArg.orderItemDtos), Name);
        }
    }

    public class AddOrderItemsToolArg()
    {
        public Guid orderId { get; set; }
        public Guid storeId { get; set; }
        public List<OrderItemDto> orderItemDtos { get; set; } = null!;
    }
}
