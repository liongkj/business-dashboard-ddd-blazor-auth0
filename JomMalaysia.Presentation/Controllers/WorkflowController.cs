using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Presentation.Gateways.Workflows;
using JomMalaysia.Presentation.Models.Workflows;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JomMalaysia.Presentation.Controllers
{
    public class WorkflowController : Controller
    {
        private readonly IWorkflowGateway _gateway;
        // GET: /<controller>/

        private List<Workflow> WorkflowList { get; set; }
        private Boolean refresh = false;
        #region gateway helper
        public WorkflowController(IWorkflowGateway gateway)
        {
            _gateway = gateway;
            

            Refresh();
        }

        async void Refresh()
        {
            if (WorkflowList != null && !refresh)
                WorkflowList = await GetWorkflows().ConfigureAwait(false);
            else
            {
                WorkflowList = new List<Workflow>();
            }
        }



        // GET: Workflow
        async Task<List<Workflow>> GetWorkflows()
        {

            try
            {
                WorkflowList = await _gateway.GetAll().ConfigureAwait(false);
                return WorkflowList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        public async Task<IActionResult >Index()
        {
            var workflows = await GetWorkflows().ConfigureAwait(false);

            var pendings = workflows.Where(x => x.Status == "pending").ToList();
            var completed = workflows.Where(x => x.Status == "completed").ToList();
            var authorized = workflows.Where(x => x.Status == "Level1").ToList();

            return View(workflows);
        }
    }
}
