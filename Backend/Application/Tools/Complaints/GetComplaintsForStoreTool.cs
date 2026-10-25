using Application.Dtos;
using Application.Services;
using Application.Tools.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tools.Complaints
{
    public class GetComplaintsForStoreTool : BaseTool, IAgentTool
    {
        private readonly ComplaintService _complaintService;

        public GetComplaintsForStoreTool(ComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        public string Name => "GetComplaintsForStoreTool";
        public Type ArgType => typeof(GetComplaintsForStoreToolArg);

        public async Task<ToolResponse> ExecuteAsync(object arg)
        {
            GetComplaintsForStoreToolArg getComplaintsForStoreToolArg = (GetComplaintsForStoreToolArg)arg;

            return await ExecuteTool<List<ComplaintDto>>(() =>
            _complaintService.GetComplaintsForStore(getComplaintsForStoreToolArg.storeId), Name);
        }
    }

    public class GetComplaintsForStoreToolArg()
    {
        public Guid storeId { get; set; }
    }
}
