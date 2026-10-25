using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Cars
{
    public class GetCarByNameTool : BaseTool, IAgentTool
    {
        private readonly CarService _carService;

        public GetCarByNameTool(CarService carService)
        {
            _carService = carService;
        }

        public string Name => "GetCarByNameTool";
        public Type ArgType => typeof(GetCarByNameToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetCarByNameToolArg getCarByNameToolArg = (GetCarByNameToolArg)arg;

            return await ExecuteTool<CarDto?>(() =>
            _carService.GetCarByName(getCarByNameToolArg.storeId, getCarByNameToolArg.name), Name);
        }
    }

    public class GetCarByNameToolArg()
    {
        public Guid storeId { get; set; }
        public string name { get; set; } = null!;
    }
}
