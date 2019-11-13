using System.Threading.Tasks;
using JomMalaysia.Presentation.Gateways.Workflows;
using Microsoft.AspNetCore.Mvc;

namespace JomMalaysia.Presentation.Views.Workflow.Components.AllWorkflow
{
    public class AllWorkflowViewComponent : ViewComponent
    {
        private readonly IWorkflowGateway _gateway;
        public AllWorkflowViewComponent(IWorkflowGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _gateway.GetAll().ConfigureAwait(false);
            
            return View(items);
        }


    }
}
