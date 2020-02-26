using System;
using System.Collections.Generic;
using static JomMalaysia.Framework.Constant.EnumConstant;

namespace JomMalaysia.Presentation.Models.Workflows
{
    public class WorkflowModel
    {
        public string WorkflowId { get; set; }
        public WorkflowTypeEnum Type { get; set; }
        public WorkflowStatusEnum Status { get; set; }
        public string Level { get; set; }
        public ListingSummary Listing { get; set; }
        public UserSummary Requester { get; set; }
        public UserSummary Responder { get; set; }

        public DateTime Created { get; set; }

        public ICollection<WorkflowSummary> HistoryData { get; set; }

        public bool IsCompleted()
        {
            return Status == WorkflowStatusEnum.rejected || Status == WorkflowStatusEnum.completed;
        }
    }
}