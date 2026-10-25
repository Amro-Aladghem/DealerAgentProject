using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.CarAccessories
{
    public class GetCarAccessoryByIdTool : BaseTool, IAgentTool
    {
        private readonly CarAccessoryService _carAccessoryService;

        public GetCarAccessoryByIdTool(CarAccessoryService carAccessoryService)
        {
            _carAccessoryService = carAccessoryService;
        }

        public string Name => "GetCarAccessoryByIdTool";
        public Type ArgType => typeof(GetCarAccessoryByIdToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetCarAccessoryByIdToolArg getCarAccessoryByIdToolArg = (GetCarAccessoryByIdToolArg)arg;

            return await ExecuteTool<CarAccessoryDto?>(() =>
            _carAccessoryService.GetCarAccessoryById(getCarAccessoryByIdToolArg.storeId, getCarAccessoryByIdToolArg.id), Name);
        }
    }

    public class GetCarAccessoryByIdToolArg()
    {
        public Guid storeId { get; set; }
        public Guid id { get; set; }
    }
}
