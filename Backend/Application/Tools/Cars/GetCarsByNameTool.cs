using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Cars
{
    public class GetCarsByNameTool : BaseTool, IAgentTool
    {
        private readonly CarService _carService;

        public GetCarsByNameTool(CarService carService)
        {
            _carService = carService;
        }

        public string Name => "GetCarsByNameTool";
        public Type ArgType => typeof(GetCarsByNameToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetCarsByNameToolArg getCarsByNameToolArg = (GetCarsByNameToolArg)arg;

            return await ExecuteTool<List<CarDto>>(() =>
            _carService.GetCarsByName(getCarsByNameToolArg.storeId, getCarsByNameToolArg.name), Name);
        }
    }

    public class GetCarsByNameToolArg()
    {
        public Guid storeId { get; set; }
        public string name { get; set; } = null!;
    }
}
