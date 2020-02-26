using System.Threading.Tasks;
using JomMalaysia.Presentation.Gateways.Workflows;
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
            var item = await _gateway.Detail(id).ConfigureAwait(false);

            return View(item);
        }
    }
}