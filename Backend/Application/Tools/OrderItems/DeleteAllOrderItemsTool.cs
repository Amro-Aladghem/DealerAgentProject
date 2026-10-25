using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.OrderItems
{
    public class DeleteAllOrderItemsTool : BaseTool, IAgentTool
    {
        private readonly OrderItemService _orderItemService;

        public DeleteAllOrderItemsTool(OrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        public string Name => "DeleteAllOrderItemsTool";
        public Type ArgType => typeof(DeleteAllOrderItemsToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            DeleteAllOrderItemsToolArg deleteAllOrderItemsToolArg = (DeleteAllOrderItemsToolArg)arg;

            return await ExecuteTool<bool>(() =>
            _orderItemService.DeleteAllOrderItems(deleteAllOrderItemsToolArg.orderId, deleteAllOrderItemsToolArg.storeId), Name);
        }
    }

    public class DeleteAllOrderItemsToolArg()
    {
        public Guid orderId { get; set; }
        public Guid storeId { get; set; }
    }
}
