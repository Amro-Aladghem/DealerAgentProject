using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Complaints
{
    public class UpdateComplaintTool : BaseTool, IAgentTool
    {
        private readonly ComplaintService _complaintService;

        public UpdateComplaintTool(ComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        public string Name => "UpdateComplaintTool";
        public Type ArgType => typeof(UpdateComplaintToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            UpdateComplaintToolArg updateComplaintToolArg = (UpdateComplaintToolArg)arg;

            return await ExecuteTool<ComplaintDto?>(() =>
            _complaintService.UpdateComplaint(updateComplaintToolArg.storeId, updateComplaintToolArg.id, updateComplaintToolArg.complaintDto), Name);
        }
    }

    public class UpdateComplaintToolArg()
    {
        public Guid storeId { get; set; }
        public Guid id { get; set; }
        public ComplaintDto complaintDto { get; set; } = null!;
    }
}
