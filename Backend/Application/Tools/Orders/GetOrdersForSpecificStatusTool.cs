using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Orders
{
    public class GetOrdersForSpecificStatusTool : BaseTool, IAgentTool
    {
        private readonly OrderService _orderService;

        public GetOrdersForSpecificStatusTool(OrderService orderService)
        {
            _orderService = orderService;
        }

        public string Name => "GetOrdersForSpecificStatusTool";
        public Type ArgType => typeof(GetOrdersForSpecificStatusToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetOrdersForSpecificStatusToolArg getOrdersForSpecificStatusToolArg = (GetOrdersForSpecificStatusToolArg)arg;

            return await ExecuteTool<List<OrderDto>>(() =>
            _orderService.GetOrdersForSpecificStatus(getOrdersForSpecificStatusToolArg.storeId, getOrdersForSpecificStatusToolArg.status), Name);
        }
    }

    public class GetOrdersForSpecificStatusToolArg()
    {
        public Guid storeId { get; set; }
        public eOrderStatus status { get; set; }
    }
}
