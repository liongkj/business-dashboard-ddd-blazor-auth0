using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static JomMalaysia.Framework.Constant.EnumConstant;

namespace JomMalaysia.Presentation.Models.Workflows
{
    public class WorkflowSummary
    {
        public WorkflowStatusEnum Status { get; set; }
        public string Level { get; set; }
        public UserSummary Responder { get; set; }
        public WorkflowActionEnum Action { get; set; }
        public DateTime Created { get; set; }
        public string Comments { get; set; }
    }
}