using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.CarAccessories
{
    public class UpdateCarAccessoryByIdTool : BaseTool, IAgentTool
    {
        private readonly CarAccessoryService _carAccessoryService;

        public UpdateCarAccessoryByIdTool(CarAccessoryService carAccessoryService)
        {
            _carAccessoryService = carAccessoryService;
        }

        public string Name => "UpdateCarAccessoryByIdTool";
        public Type ArgType => typeof(UpdateCarAccessoryByIdToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            UpdateCarAccessoryByIdToolArg updateCarAccessoryByIdToolArg = (UpdateCarAccessoryByIdToolArg)arg;

            return await ExecuteTool<CarAccessoryDto?>(() =>
            _carAccessoryService.UpdateCarAccessoryById(updateCarAccessoryByIdToolArg.storeId, updateCarAccessoryByIdToolArg.id, updateCarAccessoryByIdToolArg.carAccessoryDto), Name);
        }
    }

    public class UpdateCarAccessoryByIdToolArg()
    {
        public Guid storeId { get; set; }
        public Guid id { get; set; }
        public CarAccessoryDto carAccessoryDto { get; set; } = null!;
    }
}
