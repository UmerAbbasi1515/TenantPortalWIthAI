using System;

  using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
using static System.Runtime.InteropServices.JavaScript.JSType;

    public class ServiceRequestRepository
    {
        private readonly UBSApiHelper apiHelper;
        private readonly UBSTenantApiConfig apiConfig;

        public ServiceRequestRepository(UBSApiHelper _apiHelper, IOptions<UBSTenantApiConfig> _apiConfig)
        {
            apiHelper = _apiHelper;
            apiConfig = _apiConfig.Value;
        }

        #region Service Request
        public async Task<dynamic> GetServiceRequests(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
            return response;
        }
        #endregion

        #region Service Request Detail
        public async Task<dynamic> GetServiceRequestsDetails(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
        public async Task<dynamic> GetServiceRequestsDetailsPhotos(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
        public async Task<dynamic> GetSurveyQuestions(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
    public async Task<dynamic> GetSurveyAnswers(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
        return response;
    }
    public async Task<dynamic> SaveSurveyAnswers(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
        return response;
    }
    public async Task<dynamic> GetServiceRequestsDetailsFeedback(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
    public async Task<dynamic> GetServiceRequestsSubmitFeedback(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
        return response;
    }
        public async Task<dynamic> GetServiceRequestDocs(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
    public Task<dynamic?> UploadServiceRequestFiles(Dictionary<string, string> data, string url, string token, IFormFile file)
    {
        //var response = await apiHelper.UploadServiceRequestFiles(url ?? "", data, token, file);
        //return response;
        return null!;
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
    public async Task<dynamic?> UpdateServiceRequestDocStage(dynamic data, string url, string token)
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
    public async Task<dynamic> GetServiceRequestReportEIDFileUpload(
        string caseNo, string attachmentType, string expireDate, string DocumentTypeId, string url, string token, string filePath, string emirateIdNumber,string nationality,
               string nameEng,string nameAr,string dOB)
    {
        var response = await apiHelper.UploadServiceRequestEIDReportType(caseNo, attachmentType,
            expireDate, DocumentTypeId, url, token, filePath, emirateIdNumber, nationality, nameEng, nameAr, dOB
            );
        return response!;
    }
    public async Task<dynamic> GetServiceRequestReportEIDFileUpdate(
       string DocumentTypeId, string expireDate, string url, string token, string filePath)
    {
        var response = await apiHelper.UpdateServiceRequestEIDReportType( expireDate, DocumentTypeId, url, token, filePath
            );
        return response!;
    }
    public async Task<dynamic> GetServiceRequestAddTickekrtReplies(string caseNo, string message, string url, string token, string filePath)
    {
        var response = await apiHelper.UploadServiceRequestFiles(url ?? "", caseNo, message, token, filePath);
        return response!;
    }
    public async Task<dynamic> GetServiceRequestTickekrtRepliesFilesDownload(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
        public async Task<dynamic> GetServiceRequestAcknowledge(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
        public async Task<dynamic> GetServiceRequestReject(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
    public async Task<dynamic> GetServiceRequestClose(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
        return response;
    }
    public async Task<dynamic> GetServiceRequestCancel(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
        return response;
    }

    public async Task<dynamic?> SaveSignature(string caseID, string url, string token, string path)
    {
        var response = await apiHelper.UploadFileAsync(url ?? "", token, caseID, path);
        return response;
    }
    #endregion

    #region Add Service Request
    public async Task<dynamic> GetServiceRequestsCaseCategory(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
        public async Task<dynamic> GetServiceRequestsCaseSubCategory(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
        public async Task<dynamic> GetServiceRequestsTenantContractUnit(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
        public async Task<dynamic> GetServiceRequestsGetContactTiming(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
        public async Task<dynamic> SaveServiceRequestsGetContactTiming(dynamic data, string url, string token)
        {
            var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
            return response;
        }
        #endregion
    }


