using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Orders
{
    public class GetOrderByIdTool : BaseTool, IAgentTool
    {
        private readonly OrderService _orderService;

        public GetOrderByIdTool(OrderService orderService)
        {
            _orderService = orderService;
        }

        public string Name => "GetOrderByIdTool";
        public Type ArgType => typeof(GetOrderByIdToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetOrderByIdToolArg getOrderByIdToolArg = (GetOrderByIdToolArg)arg;

            return await ExecuteTool<OrderDto?>(() =>
            _orderService.GetOrderById(getOrderByIdToolArg.storeId, getOrderByIdToolArg.id), Name);
        }
    }

    public class GetOrderByIdToolArg()
    {
        public Guid storeId { get; set; }
        public Guid id { get; set; }
    }
}
