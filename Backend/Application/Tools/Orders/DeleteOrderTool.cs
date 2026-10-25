using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Orders
{
    public class DeleteOrderTool : BaseTool, IAgentTool
    {
        private readonly OrderService _orderService;

        public DeleteOrderTool(OrderService orderService)
        {
            _orderService = orderService;
        }

        public string Name => "DeleteOrderTool";
        public Type ArgType => typeof(DeleteOrderToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            DeleteOrderToolArg deleteOrderToolArg = (DeleteOrderToolArg)arg;

            return await ExecuteTool<bool>(() =>
            _orderService.DeleteOrder(deleteOrderToolArg.storeId, deleteOrderToolArg.id), Name);
        }
    }

    public class DeleteOrderToolArg()
    {
        public Guid storeId { get; set; }
        public Guid id { get; set; }
    }
}
