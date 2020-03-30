using System.Threading.Tasks;
using JomMalaysia.Framework.Interfaces;

namespace JomMalaysia.Presentation.Gateways.Indexes
{
    public interface IIndexGateway
    {
        Task<IWebServiceResponse> BatchIndex();
    }
}