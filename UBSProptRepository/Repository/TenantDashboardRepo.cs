using System;
namespace UBSProptRepository.Repository
{
	public class DashboardRepo
	{

        private readonly UBSApiHelper apiHelper;
        public DashboardRepo(UBSApiHelper _apiHelper)
        {
            apiHelper = _apiHelper;
        }

        public async Task<dynamic> GetDashboardPopUp(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
            return response;
        }
        public async Task<dynamic> GetData(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
            return response;
        }
        public async Task<dynamic> GetContractPopUp(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
            return response;
        }

    }
}
