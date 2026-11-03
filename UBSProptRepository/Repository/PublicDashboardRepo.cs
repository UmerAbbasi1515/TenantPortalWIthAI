using System;
namespace UBSProptRepository.Repository
{
    public class PublicDashboardRepo
    {

        private readonly UBSApiHelper apiHelper;
        public PublicDashboardRepo(UBSApiHelper _apiHelper)
        {
            apiHelper = _apiHelper;
        }

        public async Task<dynamic> GetPropertyCategoryData(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
            return response;
        }
        public async Task<dynamic> GetNotificationCount(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
            return response;
        }
        public async Task<dynamic> GetUnitType(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
            return response;
        }
        public async Task<dynamic> GetCity(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
            return response;
        }
        #region Search Properties
        public async Task<dynamic> GetSearchProperties(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
            return response;
        }

        public async Task<dynamic> GetPropertyImageAsync(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
            return response;
        }
        
        public async Task<dynamic> GetSearchPropertiesDetail(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
            return response;
        }
        public async Task<dynamic> GetUnitImagePropertiesDetail(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
            return response;
        }
        public async Task<dynamic> GetAgentBookingRequest(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
            return response;
        }
        public async Task<dynamic> submitBookingRequest(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
            return response;
        }
        #endregion

    }
}
