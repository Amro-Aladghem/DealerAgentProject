using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Complaints
{
    public class CreateComplaintTool : BaseTool, IAgentTool
    {
        private readonly ComplaintService _complaintService;

        public CreateComplaintTool(ComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        public string Name => "CreateComplaintTool";
        public Type ArgType => typeof(CreateComplaintToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            CreateComplaintToolArg createComplaintToolArg = (CreateComplaintToolArg)arg;

            return await ExecuteTool<ComplaintDto>(() =>
            _complaintService.CreateComplaint(createComplaintToolArg.storeId, createComplaintToolArg.complaintDto), Name);
        }
    }

    public class CreateComplaintToolArg()
    {
        public Guid storeId { get; set; }
        public ComplaintDto complaintDto { get; set; } = null!;
    }
}
