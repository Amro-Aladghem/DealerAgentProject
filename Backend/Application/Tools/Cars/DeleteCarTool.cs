using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Cars
{
    public class DeleteCarTool : BaseTool, IAgentTool
    {
        private readonly CarService _carService;

        public DeleteCarTool(CarService carService)
        {
            _carService = carService;
        }

        public string Name => "DeleteCarTool";
        public Type ArgType => typeof(DeleteCarToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            DeleteCarToolArg deleteCarToolArg = (DeleteCarToolArg)arg;

            return await ExecuteTool<bool>(() =>
            _carService.DeleteCar(deleteCarToolArg.storeId, deleteCarToolArg.id), Name);
        }
    }

    public class DeleteCarToolArg()
    {
        public Guid storeId { get; set; }
        public Guid id { get; set; }
    }
}
