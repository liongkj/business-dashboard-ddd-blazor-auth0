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
        string status)
        {
            var items = await GetItemsAsync(status).ConfigureAwait(false);
            return View(items);
        }
        private async Task<List<WorkflowModel>> GetItemsAsync(string status)
        {
            if (Enum.TryParse(status, out WorkflowModel.StatusEnum statusEnum))
            {
                List<WorkflowModel> items = await _gateway.GetAll().ConfigureAwait(false);
                List<WorkflowModel> found;
                if (statusEnum != WorkflowModel.StatusEnum.all)
                {
                    found = items.Where(x => x.Status.Equals(statusEnum)).ToList();
                    return found;
                }
                return items;
            }
            else
            {
                throw new ArgumentException("Invalid status");
            }
        }
    }
}
