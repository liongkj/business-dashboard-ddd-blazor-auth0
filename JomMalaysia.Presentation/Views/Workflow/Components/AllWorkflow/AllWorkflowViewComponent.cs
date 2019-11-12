using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Presentation.Models;
using JomMalaysia.Presentation.Gateways.Workflows;
using Microsoft.AspNetCore.Mvc;
using JomMalaysia.Presentation.Models.Workflows;

namespace JomMalaysia.Presentation.Views.Workflow.Components.AllWorkflow
{
    public class AllWorkflowViewComponent : ViewComponent
    {
        private readonly IWorkflowGateway _gateway;
        public AllWorkflowViewComponent(IWorkflowGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<IViewComponentResult> InvokeAsync(
        )
        {
            List<WorkflowModel> items = await _gateway.GetAll().ConfigureAwait(false);
            
            return View(items);
        }


    }
}
