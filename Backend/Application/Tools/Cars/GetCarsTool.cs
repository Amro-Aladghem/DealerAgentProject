using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Cars
{
    public class GetCarsTool : BaseTool, IAgentTool
    {
        private readonly CarService _carService;

        public GetCarsTool(CarService carService)
        {
            _carService = carService;
        }

        public string Name => "GetCarsTool";
        public Type ArgType => typeof(GetCarsToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetCarsToolArg getCarsToolArg = (GetCarsToolArg)arg;

            return await ExecuteTool<List<CarDto>>(() =>
            _carService.GetCars(getCarsToolArg.storeId), Name);
        }
    }

    public class GetCarsToolArg()
    {
        public Guid storeId { get; set; }
    }
}
