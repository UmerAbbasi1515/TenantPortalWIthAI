using System;
using Microsoft.AspNetCore.Http.Extensions;

namespace UBSProptRepository.Repository
{
	public class CompanyAndServicesRepository
	{
        private readonly UBSApiHelper apiHelper;
        public CompanyAndServicesRepository(UBSApiHelper _apiHelper)
        {
            apiHelper = _apiHelper;
        }

        public async Task<dynamic> GetCompany(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
            return response;
        }
        public async Task<dynamic> GetServices(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
            return response;
        }
        public async Task<dynamic> GetServicesDetail(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
            return response;
        }
    }
}

