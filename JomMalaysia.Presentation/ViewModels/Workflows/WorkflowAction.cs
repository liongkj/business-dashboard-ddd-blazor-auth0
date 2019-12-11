using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JomMalaysia.Presentation.ViewModels.Workflows
{
    public class WorkflowAction
    {
        public string WorkflowId { get; set; }
        public string Comments { get; set; }
        public actionEnum Action { get; set; }
    }

    public enum actionEnum
    {
        approve,
        reject
    }
}