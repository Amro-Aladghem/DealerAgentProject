using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Cars
{
    public class GetCarByIdTool : BaseTool, IAgentTool
    {
        private readonly CarService _carService;

        public GetCarByIdTool(CarService carService)
        {
            _carService = carService;
        }

        public string Name => "GetCarByIdTool";
        public Type ArgType => typeof(GetCarByIdToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetCarByIdToolArg getCarByIdToolArg = (GetCarByIdToolArg)arg;

            return await ExecuteTool<CarDto?>(() =>
            _carService.GetCarById(getCarByIdToolArg.storeId, getCarByIdToolArg.id), Name);
        }
    }

    public class GetCarByIdToolArg()
    {
        public Guid storeId { get; set; }
        public Guid id { get; set; }
    }
}
