using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Cars
{
    public class AddCarsTool : BaseTool, IAgentTool
    {
        private readonly CarService _carService;

        public AddCarsTool(CarService carService)
        {
            _carService = carService;
        }

        public string Name => "AddCarsTool";
        public Type ArgType => typeof(AddCarsToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            AddCarsToolArg addCarsToolArg = (AddCarsToolArg)arg;

            return await ExecuteTool<List<CarDto>>(() =>
            _carService.AddCars(addCarsToolArg.storeId, addCarsToolArg.carDtos), Name);
        }
    }

    public class AddCarsToolArg()
    {
        public Guid storeId { get; set; }
        public List<CarDto> carDtos { get; set; } = null!;
    }
}
