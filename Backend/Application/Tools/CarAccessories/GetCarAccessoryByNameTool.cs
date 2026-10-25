using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.CarAccessories
{
    public class GetCarAccessoryByNameTool : BaseTool, IAgentTool
    {
        private readonly CarAccessoryService _carAccessoryService;

        public GetCarAccessoryByNameTool(CarAccessoryService carAccessoryService)
        {
            _carAccessoryService = carAccessoryService;
        }

        public string Name => "GetCarAccessoryByNameTool";
        public Type ArgType => typeof(GetCarAccessoryByNameToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetCarAccessoryByNameToolArg getCarAccessoryByNameToolArg = (GetCarAccessoryByNameToolArg)arg;

            return await ExecuteTool<CarAccessoryDto?>(() =>
            _carAccessoryService.GetCarAccessoryByName(getCarAccessoryByNameToolArg.storeId, getCarAccessoryByNameToolArg.name), Name);
        }
    }

    public class GetCarAccessoryByNameToolArg()
    {
        public Guid storeId { get; set; }
        public string name { get; set; } = null!;
    }
}
