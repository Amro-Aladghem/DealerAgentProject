using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.CarAccessories
{
    public class GetCarAccessoriesTool : BaseTool, IAgentTool
    {
        private readonly CarAccessoryService _carAccessoryService;

        public GetCarAccessoriesTool(CarAccessoryService carAccessoryService)
        {
            _carAccessoryService = carAccessoryService;
        }

        public string Name => "GetCarAccessoriesTool";
        public Type ArgType => typeof(GetCarAccessoriesToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetCarAccessoriesToolArg getCarAccessoriesToolArg = (GetCarAccessoriesToolArg)arg;

            return await ExecuteTool<List<CarAccessoryDto>>(() =>
            _carAccessoryService.GetCarAccessories(getCarAccessoriesToolArg.storeId), Name);
        }
    }

    public class GetCarAccessoriesToolArg()
    {
        public Guid storeId { get; set; }
    }
}
