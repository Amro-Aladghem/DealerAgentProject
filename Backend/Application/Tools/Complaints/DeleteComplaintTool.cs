using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Complaints
{
    public class DeleteComplaintTool : BaseTool, IAgentTool
    {
        private readonly ComplaintService _complaintService;

        public DeleteComplaintTool(ComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        public string Name => "DeleteComplaintTool";
        public Type ArgType => typeof(DeleteComplaintToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            DeleteComplaintToolArg deleteComplaintToolArg = (DeleteComplaintToolArg)arg;

            return await ExecuteTool<bool>(() =>
            _complaintService.DeleteComplaint(deleteComplaintToolArg.storeId, deleteComplaintToolArg.id), Name);
        }
    }

    public class DeleteComplaintToolArg()
    {
        public Guid storeId { get; set; }
        public Guid id { get; set; }
    }
}
