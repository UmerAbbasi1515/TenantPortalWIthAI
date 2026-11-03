using System;

    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    public class PaymentsRepository
    {
        private readonly UBSApiHelper apiHelper;
        private readonly UBSTenantApiConfig apiConfig;

        public PaymentsRepository(UBSApiHelper _apiHelper, IOptions<UBSTenantApiConfig> _apiConfig)
        {
            apiHelper = _apiHelper;
            apiConfig = _apiConfig.Value;
        }

        #region Payments
        public async Task<dynamic> GetPayments(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
            return response;
        }
        #endregion

        #region Payments Detail

        public async Task<dynamic> GetPaymentsCheques(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
        public async Task<dynamic> PaymentDetailDownloadReceipt(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
        #endregion
    }
