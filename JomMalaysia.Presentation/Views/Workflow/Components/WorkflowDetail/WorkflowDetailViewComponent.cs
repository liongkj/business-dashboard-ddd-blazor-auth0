using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Presentation.Gateways.Workflows;
using JomMalaysia.Presentation.Models.Workflows;
using Microsoft.AspNetCore.Mvc;

namespace JomMalaysia.Presentation.Views.Workflow.Components.WorkflowDetail
{

    public class WorkflowDetailViewComponent : ViewComponent
    {
        private readonly IWorkflowGateway _gateway;
        public WorkflowDetailViewComponent(IWorkflowGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<IViewComponentResult> InvokeAsync(
        string id)
        {
            WorkflowModel item = await _gateway.Detail(id).ConfigureAwait(false);

            return View(item);
        }


    }
}

