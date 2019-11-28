using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Presentation.Gateways.Workflows;
using JomMalaysia.Presentation.Models.Workflows;
using JomMalaysia.Presentation.ViewModels.Workflows;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static JomMalaysia.Framework.Constant.EnumConstant;
using static JomMalaysia.Presentation.Models.Workflows.WorkflowModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JomMalaysia.Presentation.Controllers
{
    [Authorize]
    public class WorkflowController : Controller
    {
        private readonly IWorkflowGateway _gateway;
        // GET: /<controller>/

        private List<WorkflowModel> WorkflowList { get; set; }
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
                WorkflowList = new List<WorkflowModel>();
            }
        }



        // GET: WorkflowModel
        async Task<List<WorkflowModel>> GetWorkflows()
        {
            if (WorkflowList.Count > 0 && !refresh)
            {
                return WorkflowList;
            }
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
        [HttpGet]
        public IActionResult Detail(string id)
        {
            var vm = new WorkflowAction
            {
                WorkflowId = id
            };
            return PartialView("_Detail", vm);
        }

        public async Task<IActionResult> Index()
        {
            var workflows = await GetWorkflows().ConfigureAwait(false);
            List<WorkflowStatus> vm = new List<WorkflowStatus>();
            var total = 0;
            foreach (WorkflowStatusEnum x in Enum.GetValues(typeof(WorkflowStatusEnum)))
            {
                if (x != WorkflowStatusEnum.all)
                {
                    var status = new WorkflowStatus(x.ToString());
                    var count = 0;
                    foreach (var w in workflows)
                    {
                        if (w.Status.Equals(x))
                        {
                            count++;
                        }
                        status.Count = count;
                    }
                    total += count;
                    vm.Add(status);
                }
            }
            var allStatus = new WorkflowStatus("all")
            {
                Count = total
            };
            vm.Add(allStatus);
            return View(vm);
        }

        public IActionResult Respond()
        {
            throw new NotImplementedException();
        }
    }
}
