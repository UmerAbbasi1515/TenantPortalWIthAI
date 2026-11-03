using System;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

public class RenewalFlowRepo
{

    private readonly UBSApiHelper apiHelper;
    public RenewalFlowRepo(UBSApiHelper _apiHelper)
    {
        apiHelper = _apiHelper;
    }

    public async Task<dynamic> GetRenewalContracts(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }

    public async Task<dynamic> ContractRenew(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> ContractSubmitRenew(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> ContractExtend(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> ContractSubmitExtend(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> ContractTerminate(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> ContractSubmitTerminate(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> MakePayment(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> MakePaymentUploadCheque(dynamic data, string url, string token, string filePath)
    {
        var PaymentModeId = data.PaymentModeId;
        var PaymentSettingId = data.PaymentSettingId;
        var ChequeNo = data.ChequeNo;
        var Address = data.Address;
        var response = await apiHelper.UploadMakePaymentCheque(PaymentModeId, PaymentSettingId, ChequeNo, Address, url, token, filePath);
        return response!;
    }
    public async Task<dynamic> MakePaymentDownloadCheque(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> MakePaymentRemoveCheque(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> UpdateArmaxAdress(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> getOnlinePayment(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> getOnlinePolicyRefundable(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> RegisterPayment(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> ContractOffer(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> DownloadContract(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
  
    public async Task<dynamic?> SaveSignature(string caseID, string url, string token, string path)
    {
        var response = await apiHelper.UploadFileAsync(url ?? "",  token, caseID, path);
        return response;
    }
    public async Task<dynamic> MuniciplityApproval(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> UpdateRenewalContractStage(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> GetServiceRequestReportFileUpload(string caseNo, string attachmentType, string expireDate, string DocumentTypeId, string url, string token, string filePath)
    {
        var response = await apiHelper.UploadServiceRequestVenReportType(caseNo, attachmentType, expireDate, DocumentTypeId, url, token, filePath);
        return response!;
    }

}
