using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Complaints
{
    public class GetComplaintsForCustomerEmailTool : BaseTool, IAgentTool
    {
        private readonly ComplaintService _complaintService;

        public GetComplaintsForCustomerEmailTool(ComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        public string Name => "GetComplaintsForCustomerEmailTool";
        public Type ArgType => typeof(GetComplaintsForCustomerEmailToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetComplaintsForCustomerEmailToolArg getComplaintsForCustomerEmailToolArg = (GetComplaintsForCustomerEmailToolArg)arg;

            return await ExecuteTool<List<ComplaintDto>>(() =>
            _complaintService.GetComplaintsForCustomerEmail(getComplaintsForCustomerEmailToolArg.storeId, getComplaintsForCustomerEmailToolArg.customerEmail), Name);
        }
    }

    public class GetComplaintsForCustomerEmailToolArg()
    {
        public Guid storeId { get; set; }
        public string customerEmail { get; set; } = null!;
    }
}
