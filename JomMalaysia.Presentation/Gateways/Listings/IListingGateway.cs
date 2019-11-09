using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Models;
using JomMalaysia.Presentation.Models.Listings;
using JomMalaysia.Presentation.ViewModels.Listings;
using Microsoft.AspNetCore.Mvc;

namespace JomMalaysia.Presentation.Gateways.Listings
{
    public interface IListingGateway
    {
        Task<List<Listing>> GetAll();
        Task<IWebServiceResponse> Add(RegisterListingViewModel vm);
        Task<IWebServiceResponse> Publish(string ListingId,int months);
    }
}
