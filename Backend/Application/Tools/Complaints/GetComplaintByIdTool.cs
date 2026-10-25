using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Complaints
{
    public class GetComplaintByIdTool : BaseTool, IAgentTool
    {
        private readonly ComplaintService _complaintService;

        public GetComplaintByIdTool(ComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        public string Name => "GetComplaintByIdTool";
        public Type ArgType => typeof(GetComplaintByIdToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetComplaintByIdToolArg getComplaintByIdToolArg = (GetComplaintByIdToolArg)arg;

            return await ExecuteTool<ComplaintDto?>(() =>
            _complaintService.GetComplaintById(getComplaintByIdToolArg.storeId, getComplaintByIdToolArg.id), Name);
        }
    }

    public class GetComplaintByIdToolArg()
    {
        public Guid storeId { get; set; }
        public Guid id { get; set; }
    }
}
