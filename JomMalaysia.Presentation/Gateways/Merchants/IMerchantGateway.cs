using System.Collections.Generic;
using System.Threading.Tasks;
using JomMalaysia.Framework.Interfaces;
using JomMalaysia.Presentation.Models.Merchants;
using JomMalaysia.Presentation.ViewModels.Merchants;

namespace JomMalaysia.Presentation.Gateways.Merchants
{
    public interface IMerchantGateway
    {
        Task<List<Merchant>> GetMerchants();
        Task<IWebServiceResponse> Add(RegisterMerchantViewModel vm);
        Task<Merchant> Detail(string id);
        Task<IWebServiceResponse> Edit(RegisterMerchantViewModel vm, string merchantId);
    }
}