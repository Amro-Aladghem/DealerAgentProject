using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Cars
{
    public class GetCarsByCarMakeTool : BaseTool, IAgentTool
    {
        private readonly CarService _carService;

        public GetCarsByCarMakeTool(CarService carService)
        {
            _carService = carService;
        }

        public string Name => "GetCarsByCarMakeTool";
        public Type ArgType => typeof(GetCarsByCarMakeToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetCarsByCarMakeToolArg getCarsByCarMakeToolArg = (GetCarsByCarMakeToolArg)arg;

            return await ExecuteTool<List<CarDto>>(() =>
            _carService.GetCarsByCarMake(getCarsByCarMakeToolArg.storeId, getCarsByCarMakeToolArg.carMakeId), Name);
        }
    }

    public class GetCarsByCarMakeToolArg()
    {
        public Guid storeId { get; set; }
        public int carMakeId { get; set; }
    }
}
