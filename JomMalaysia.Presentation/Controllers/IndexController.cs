using System;
using System.Threading.Tasks;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Helper;
using JomMalaysia.Framework.Interfaces;
using JomMalaysia.Presentation.Gateways.Categories;
using JomMalaysia.Presentation.Gateways.Indexes;
using JomMalaysia.Presentation.Gateways.Listings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JomMalaysia.Presentation.Controllers
{

    [Authorize]
    public class IndexController : Controller
    {


        private readonly IIndexGateway _gateway;


        public IndexController(IIndexGateway gateway)
        {
            _gateway = gateway;
        }
        
        [HttpPost]
        public async Task<Tuple<int, string>> Sync( )
        {
            IWebServiceResponse response ;
            try
            {
              response =  await _gateway.BatchIndex();
            } catch (GatewayException e)
            {
                return SweetDialogHelper.HandleStatusCode(e.StatusCode, e.Message);
            }
            
            return SweetDialogHelper.HandleResponse(response);
        }
    }
}