using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.CarAccessories
{
    public class DeleteCarAccessoryTool : BaseTool, IAgentTool
    {
        private readonly CarAccessoryService _carAccessoryService;

        public DeleteCarAccessoryTool(CarAccessoryService carAccessoryService)
        {
            _carAccessoryService = carAccessoryService;
        }

        public string Name => "DeleteCarAccessoryTool";
        public Type ArgType => typeof(DeleteCarAccessoryToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            DeleteCarAccessoryToolArg deleteCarAccessoryToolArg = (DeleteCarAccessoryToolArg)arg;

            return await ExecuteTool<bool>(() =>
            _carAccessoryService.DeleteCarAccessory(deleteCarAccessoryToolArg.storeId, deleteCarAccessoryToolArg.id), Name);
        }
    }

    public class DeleteCarAccessoryToolArg()
    {
        public Guid storeId { get; set; }
        public Guid id { get; set; }
    }
}
