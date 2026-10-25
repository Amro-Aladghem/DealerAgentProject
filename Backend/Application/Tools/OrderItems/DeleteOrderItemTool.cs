using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.OrderItems
{
    public class DeleteOrderItemTool : BaseTool, IAgentTool
    {
        private readonly OrderItemService _orderItemService;

        public DeleteOrderItemTool(OrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        public string Name => "DeleteOrderItemTool";
        public Type ArgType => typeof(DeleteOrderItemToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            DeleteOrderItemToolArg deleteOrderItemToolArg = (DeleteOrderItemToolArg)arg;

            return await ExecuteTool<bool>(() =>
            _orderItemService.DeleteOrderItem(deleteOrderItemToolArg.orderId, deleteOrderItemToolArg.storeId, deleteOrderItemToolArg.id), Name);
        }
    }

    public class DeleteOrderItemToolArg()
    {
        public Guid orderId { get; set; }
        public Guid storeId { get; set; }
        public Guid id { get; set; }
    }
}
