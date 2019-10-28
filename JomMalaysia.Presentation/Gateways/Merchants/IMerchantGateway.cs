using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Models.Merchants;

namespace JomMalaysia.Presentation.Gateways.Merchants
{
    public interface IMerchantGateway
    {
        Task<List<Merchant>> GetMerchants();
        Task<IWebServiceResponse> CreateMerchant(Merchant vm);
    }
}
