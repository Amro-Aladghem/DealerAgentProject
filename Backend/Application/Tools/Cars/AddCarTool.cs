using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Cars
{
    public class AddCarTool : BaseTool, IAgentTool
    {
        private readonly CarService _carService;

        public AddCarTool(CarService carService)
        {
            _carService = carService;
        }

        public string Name => "AddCarTool";
        public Type ArgType => typeof(AddCarToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            AddCarToolArg addCarToolArg = (AddCarToolArg)arg;

            return await ExecuteTool<CarDto>(() =>
            _carService.AddCar(addCarToolArg.storeId, addCarToolArg.carDto), Name);
        }
    }

    public class AddCarToolArg()
    {
        public Guid storeId { get; set; }
        public CarDto carDto { get; set; } = null!;
    }
}
