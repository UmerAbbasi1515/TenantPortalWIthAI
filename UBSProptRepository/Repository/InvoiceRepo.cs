using System;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class InvoiceRepository
{
    private readonly UBSApiHelper apiHelper;

    public InvoiceRepository(UBSApiHelper _apiHelper)
    {
        apiHelper = _apiHelper;
    }

    #region Lpo
    public async Task<dynamic> GetAllInvoices(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    #endregion

    #region Lpo Detail

    #endregion

    #region Add Invoice
    public async Task<dynamic> InvoiceLpos(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> InvoiceAMCs(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> InvoiceInstallments(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> SaveInvoiceServiceRequest(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> GetInvoiceMainInfo(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
        return response;
    }
    public async Task<dynamic> GetInvoiceDocs(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
        return response;
    }
    public async Task<dynamic> UploadInvoiceFileUpload(string caseNo, string RequestId, string attachmentType, string expireDate, string DocumentTypeId, string url, string token, string filePath)
    {
        var response = await apiHelper.UploadInvoiceVendorDocType(caseNo, RequestId, attachmentType, expireDate, DocumentTypeId, url, token, filePath);
        return response!;
    }
    public async Task<dynamic?> DownloadServiceRequestFiles(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
        return response;
    }
    public async Task<dynamic?> RemoveServiceRequestFiles(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
        return response;
    }
    public async Task<dynamic> GetServiceRequestGetTickekrtReplies(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
        return response;
    }
    public async Task<dynamic> GetServiceRequestReportFileUpload(string caseNo, string attachmentType, string expireDate, string DocumentTypeId, string url, string token, string filePath)
    {
        var response = await apiHelper.UploadServiceRequestVenReportType(caseNo, attachmentType, expireDate, DocumentTypeId, url, token, filePath);
        return response!;
    }
    public async Task<dynamic> GetServiceRequestAddTickekrtReplies(string caseNo, string message, string url, string token, string filePath)
    {
        var response = await apiHelper.UploadServiceRequestFiles(url ?? "", caseNo, message, token, filePath);
        return response!;
    }

    #endregion

}


