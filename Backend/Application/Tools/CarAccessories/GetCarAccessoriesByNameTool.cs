using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.CarAccessories
{
    public class GetCarAccessoriesByNameTool : BaseTool, IAgentTool
    {
        private readonly CarAccessoryService _carAccessoryService;

        public GetCarAccessoriesByNameTool(CarAccessoryService carAccessoryService)
        {
            _carAccessoryService = carAccessoryService;
        }

        public string Name => "GetCarAccessoriesByNameTool";
        public Type ArgType => typeof(GetCarAccessoriesByNameToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetCarAccessoriesByNameToolArg getCarAccessoriesByNameToolArg = (GetCarAccessoriesByNameToolArg)arg;

            return await ExecuteTool<List<CarAccessoryDto>>(() =>
            _carAccessoryService.GetCarAccessoriesByName(getCarAccessoriesByNameToolArg.storeId, getCarAccessoriesByNameToolArg.name), Name);
        }
    }

    public class GetCarAccessoriesByNameToolArg()
    {
        public Guid storeId { get; set; }
        public string name { get; set; } = null!;
    }
}
