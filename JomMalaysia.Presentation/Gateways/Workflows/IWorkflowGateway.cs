using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Presentation.Models.Workflows;

namespace JomMalaysia.Presentation.Gateways.Workflows
{
    public interface IWorkflowGateway
    {
        Task<List<WorkflowModel>> GetAll();
        Task<WorkflowModel> Detail(string id);
    }
}
