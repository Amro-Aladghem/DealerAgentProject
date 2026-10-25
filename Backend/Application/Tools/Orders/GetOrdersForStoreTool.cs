using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Orders
{
    public class GetOrdersForStoreTool : BaseTool, IAgentTool
    {
        private readonly OrderService _orderService;

        public GetOrdersForStoreTool(OrderService orderService)
        {
            _orderService = orderService;
        }

        public string Name => "GetOrdersForStoreTool";
        public Type ArgType => typeof(GetOrdersForStoreToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetOrdersForStoreToolArg getOrdersForStoreToolArg = (GetOrdersForStoreToolArg)arg;

            return await ExecuteTool<List<OrderDto>>(() =>
            _orderService.GetOrdersForStore(getOrdersForStoreToolArg.storeId), Name);
        }
    }

    public class GetOrdersForStoreToolArg()
    {
        public Guid storeId { get; set; }
    }
}
