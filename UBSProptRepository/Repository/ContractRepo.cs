using System;

    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;

    public class ContractRepository
    {
        private readonly UBSApiHelper apiHelper;
        private readonly UBSTenantApiConfig apiConfig;

        public ContractRepository(UBSApiHelper _apiHelper, IOptions<UBSTenantApiConfig> _apiConfig)
        {
            apiHelper = _apiHelper;
            apiConfig = _apiConfig.Value;
        }

        #region Contract
        public async Task<dynamic> GetContracts(dynamic data,string url, string token)
        {
            //var postUrl = apiConfig.getContracts;
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
            return response;
        }
        #endregion

        #region Contract Detail
        public async Task<dynamic> GetContractDetails(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
        public async Task<dynamic> GetContractPayables(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }

    public async Task<dynamic> GetContractUnites(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
        public async Task<dynamic> GetContractPayments(dynamic data, string url, string token)
        {;
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
        public async Task<dynamic> GetContractPaymentsCheques(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
        public async Task<dynamic> GetContractCharges(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
        public async Task<dynamic> GetContractInvoice(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
        public async Task<dynamic> GetContractFinancial(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
        public async Task<dynamic> GetContractProperties(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
    #endregion
}
