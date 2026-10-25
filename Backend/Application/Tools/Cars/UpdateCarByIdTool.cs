using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Cars
{
    public class UpdateCarByIdTool : BaseTool, IAgentTool
    {
        private readonly CarService _carService;

        public UpdateCarByIdTool(CarService carService)
        {
            _carService = carService;
        }

        public string Name => "UpdateCarByIdTool";
        public Type ArgType => typeof(UpdateCarByIdToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            UpdateCarByIdToolArg updateCarByIdToolArg = (UpdateCarByIdToolArg)arg;

            return await ExecuteTool<CarDto?>(() =>
            _carService.UpdateCarById(updateCarByIdToolArg.storeId, updateCarByIdToolArg.id, updateCarByIdToolArg.carDto), Name);
        }
    }

    public class UpdateCarByIdToolArg()
    {
        public Guid storeId { get; set; }
        public Guid id { get; set; }
        public CarDto carDto { get; set; } = null!;
    }
}
