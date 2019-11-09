using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Models.Categories;
using JomMalaysia.Presentation.Models.Workflows;

namespace JomMalaysia.Presentation.Gateways.Workflows
{
    public interface IWorkflowGateway
    {
        Task<List<Workflow>> GetAll();
        
    }
}
