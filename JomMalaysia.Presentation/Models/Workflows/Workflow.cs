using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Framework.Helper;

namespace JomMalaysia.Presentation.Models.Workflows
{
    public class Workflow
    {

        public string WorkflowId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public Listing Listing { get; set; }
        public User Requester { get; set; }
        public User Responder { get; set; }

        public DateTime Created { get; set; }

        public ICollection<Workflow> HistoryData { get; set; }
        
        public bool CanHandle(string userRole)
        {

            return true;
            //todo
        }
    }
}
