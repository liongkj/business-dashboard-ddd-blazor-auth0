using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Framework.Helper;

namespace JomMalaysia.Presentation.ViewModels.Workflows
{
    public class WorkflowStatus
    {
        public string Name { get; set; }
        public int Count { get; set; }

        public WorkflowStatus(string name)
        {
            Name = StringHelper.CapitalizeOrConvertNullToEmptyString(name);
        }
    }
}