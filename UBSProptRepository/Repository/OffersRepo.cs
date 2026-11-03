using System;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using System.Globalization;

public class OffersRepository
{

    private readonly UBSApiHelper apiHelper;
    public OffersRepository(UBSApiHelper _apiHelper)
    {
        apiHelper = _apiHelper;
    }

    public async Task<dynamic> GetOffers(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> GetOfferDetail(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }

    public async Task<dynamic> GetPromotions(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> GetPromotionsDetail(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }

    #region New Offers
    public async Task<dynamic> GetOffersN(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    #endregion
    public async Task<dynamic> GetOffersDocs(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
        return response;
    }


    public async Task<dynamic> UploadOffersFile(
        string addedBy, DateTime? expiry, string masterID, string title, string titleAR,
        string documentType, string suspend, string filePath, string url, string token)
    {
        var expiryText = (expiry ?? DateTime.UtcNow)
            .ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

        var response = await apiHelper.UploadOfferFile(
            addedBy, expiryText, masterID, title, titleAR,
            documentType, suspend, filePath, url, token);

        return response!;
    }
    public async Task<dynamic?> DownloadOffersFiles(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
        return response;
    }
    #region Documents

    #endregion
    #region Communications
    public async Task<dynamic> GetOffersTickekrtReplies(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url ?? "", data, token);
        return response;
    }
    public async Task<dynamic> OffersTicketReplyFileUpload(string caseNo, string attachmentType, string expireDate, string DocumentTypeId, string url, string token, string filePath)
    {
        var response = await apiHelper.UploadServiceRequestVenReportType(caseNo, attachmentType, expireDate, DocumentTypeId, url, token, filePath);
        return response!;
    }
    public async Task<dynamic> OffersddTickeketReplies(string offerId, string message, string url, string token, string filePath)
    {
        var response = await apiHelper.UploadOfferCommunicationFiles(url ?? "", offerId, message, token, filePath);
        return response!;
    }
    #endregion

}
