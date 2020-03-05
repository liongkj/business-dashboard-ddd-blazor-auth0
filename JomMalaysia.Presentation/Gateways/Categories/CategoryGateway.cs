using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using JomMalaysia.Framework.CacheKeys;
using JomMalaysia.Framework.Constant;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Interfaces;
using JomMalaysia.Presentation.Manager;
using JomMalaysia.Presentation.Models.Categories;
using JomMalaysia.Presentation.ViewModels.Categories;
using Microsoft.Extensions.Caching.Memory;
using RestSharp;

namespace JomMalaysia.Presentation.Gateways.Categories
{
    public class CategoryGateway : ICategoryGateway
    {
        private readonly IWebServiceExecutor _webServiceExecutor;
        private readonly IApiBuilder _apiBuilder;
        private IMemoryCache _cache;


        public CategoryGateway(IWebServiceExecutor webServiceExecutor,
            IApiBuilder apiBuilder, IMemoryCache cache)
        {
            _webServiceExecutor = webServiceExecutor;
            _apiBuilder = apiBuilder;
            _cache = cache;
        }

        public async Task<Category> GetCategory(string CategoryId)
        {
            Category result = null;
            if (!_cache.TryGetValue(CacheKeys<Category>.EntryItem+CategoryId, out result))
            {
                try
                {
                    var req = _apiBuilder.GetApi(APIConstant.API.Path.CategoryWithId, CategoryId);
                    const Method method = Method.GET;
                    IWebServiceResponse<ViewModel<Category>> response = await _webServiceExecutor
                        .ExecuteRequestAsync<ViewModel<Category>>(req, method)
                        .ConfigureAwait(false);
                    if (response.StatusCode != HttpStatusCode.OK) return null;
                    result = response.Data.Data;
                }
                catch (GatewayException ex)
                {
                    throw ex;
                }
                _cache.Set(CacheKeys<Category>.EntryItem+CategoryId, result, CacheKeys<Category>.cacheEntryOptions);
            }

            return result;
        }

        public async Task<IWebServiceResponse> CreateCategory(NewCategoryViewModel vm, string categoryID)
        {
            IWebServiceResponse<Category> response;
            try
            {
                var req = string.IsNullOrEmpty(categoryID)
                    ? _apiBuilder.GetApi(APIConstant.API.Path.Category)
                    : _apiBuilder.GetApi(APIConstant.API.Path.NewSubcategory, categoryID);

                const Method method = Method.POST;
                response = await _webServiceExecutor
                    .ExecuteRequestAsync<Category>(req, method, vm)
                    .ConfigureAwait(false);
            }
            catch (GatewayException ex)
            {
                throw ex;
            }

            return response;
        }

        public async Task<IWebServiceResponse> EditCategory(NewCategoryViewModel vm, string categoryId)
        {
            IWebServiceResponse<Category> response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.CategoryWithId, categoryId);

                const Method method = Method.PUT;
                response = await _webServiceExecutor
                    .ExecuteRequestAsync<Category>(req, method, vm)
                    .ConfigureAwait(false);
            }
            catch (GatewayException ex)
            {
                throw ex;
            }

            return response;


            //handle exception
        }

        public async Task<List<Category>> GetCategories()
        {
            List<Category> result;
            IWebServiceResponse<ListViewModel<Category>> response;
            if (!_cache.TryGetValue(CacheKeys<Category>.Entry, out result))
            {
                try
                {
                    var req = _apiBuilder.GetApi(APIConstant.API.Path.Category);
                    const Method method = Method.GET;
                    response = await _webServiceExecutor
                        .ExecuteRequestAsync<ListViewModel<Category>>(req, method)
                        .ConfigureAwait(false);
                }
                catch (GatewayException ex)
                {
                    throw ex;
                }

                if (response.StatusCode != HttpStatusCode.OK) return result;
                var categories = response.Data.Data;
                result = categories;
                // Set cache options.
                 
                _cache.Set(CacheKeys<Category>.Entry, result, CacheKeys<Category>.cacheEntryOptions);
            }

            
            //handle exception
            return result;
        }

        public async Task<IWebServiceResponse> Delete(string categoryId)
        {
            IWebServiceResponse<Category> response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.CategoryWithId, categoryId);

                const Method method = Method.DELETE;
                response = await _webServiceExecutor
                    .ExecuteRequestAsync<Category>(req, method)
                    .ConfigureAwait(false);
            }
            catch (GatewayException ex)
            {
                throw ex;
            }

            return response;


            //handle exception
        }
    }
}