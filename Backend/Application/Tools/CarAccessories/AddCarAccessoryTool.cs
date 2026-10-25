using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.CarAccessories
{
    public class AddCarAccessoryTool : BaseTool, IAgentTool
    {
        private readonly CarAccessoryService _carAccessoryService;

        public AddCarAccessoryTool(CarAccessoryService carAccessoryService)
        {
            _carAccessoryService = carAccessoryService;
        }

        public string Name => "AddCarAccessoryTool";
        public Type ArgType => typeof(AddCarAccessoryToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            AddCarAccessoryToolArg addCarAccessoryToolArg = (AddCarAccessoryToolArg)arg;

            return await ExecuteTool<CarAccessoryDto>(() =>
            _carAccessoryService.AddCarAccessory(addCarAccessoryToolArg.storeId, addCarAccessoryToolArg.carAccessoryDto), Name);
        }
    }

    public class AddCarAccessoryToolArg()
    {
        public Guid storeId { get; set; }
        public CarAccessoryDto carAccessoryDto { get; set; } = null!;
    }
}
