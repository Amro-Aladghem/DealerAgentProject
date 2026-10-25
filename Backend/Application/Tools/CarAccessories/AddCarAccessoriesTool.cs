using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.CarAccessories
{
    public class AddCarAccessoriesTool : BaseTool, IAgentTool
    {
        private readonly CarAccessoryService _carAccessoryService;

        public AddCarAccessoriesTool(CarAccessoryService carAccessoryService)
        {
            _carAccessoryService = carAccessoryService;
        }

        public string Name => "AddCarAccessoriesTool";
        public Type ArgType => typeof(AddCarAccessoriesToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            AddCarAccessoriesToolArg addCarAccessoriesToolArg = (AddCarAccessoriesToolArg)arg;

            return await ExecuteTool<List<CarAccessoryDto>>(() =>
            _carAccessoryService.AddCarAccessories(addCarAccessoriesToolArg.storeId, addCarAccessoriesToolArg.carAccessoryDtos), Name);
        }
    }

    public class AddCarAccessoriesToolArg()
    {
        public Guid storeId { get; set; }
        public List<CarAccessoryDto> carAccessoryDtos { get; set; } = null!;
    }
}
