﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Models;
using JomMalaysia.Presentation.Models.Categories;
using Microsoft.AspNetCore.Mvc;

namespace JomMalaysia.Presentation.Gateways.Category
{
    public interface ICategoryGateway
    {
        Task<List<CategoryViewModel>> GetCategories();
        Task<IWebServiceResponse> CreateCategory(CategoryViewModel vm);
        Task<IWebServiceResponse> EditCategory(CategoryViewModel vm);
        Task<IWebServiceResponse> Delete(string CategoryId);
    }
}
