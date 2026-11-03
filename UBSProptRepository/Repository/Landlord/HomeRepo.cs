using System;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

public class HomeRepository
{
    //private readonly ApiHelper apiHelper;
    //public HomeRepository(ApiHelper _apiHelper)
    //{
    //    apiHelper = _apiHelper;
    //}
    //apiConfig apiConfig = apiConfig.Instance;

    private readonly UBSApiHelper apiHelper;
    private readonly UBSLandlordApiConfig apiConfig;
    public HomeRepository(UBSApiHelper _apiHelper, IOptions<UBSLandlordApiConfig> _apiConfig)
    {
        apiHelper = _apiHelper;
        apiConfig = _apiConfig.Value;
    }

#region Home
    public async Task<dynamic> GetData(dynamic data)
    {
        var postUrl = apiConfig.LandlordDashboardGetData;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl, data, token);
        return response;
    }

    public async Task<dynamic> GetProperties(dynamic data)
    {
        var postUrl = apiConfig.GetLandlordProperties;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl ?? "", data, token);
        return response;
    }
    public async Task<dynamic> GetInvoices(dynamic data)
    {
        var postUrl = apiConfig.LandlordInvoices;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl, data, token);
        return response;
    }
    public async Task<dynamic> GetContracts(dynamic data)
    {
        var postUrl = apiConfig.GetLandlordContracts;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl, data, token);
        return response;
    }
    public async Task<dynamic> ProccedToLogin(dynamic data)
    {
        var postUrl = "";// apiConfig.proceedToLogin;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl, data, token);
        LabelsLandlordPortal.token = response.token;
        return response;
    }
    public async Task<dynamic> ValidateRoleByMPin(dynamic data)
    {
        var postUrl = "";// apiConfig.validateRoleByMpin;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl, data, token);
        LabelsLandlordPortal.token = response.token;
        return response;
    }
#endregion

#region Operational
     public async Task<dynamic> GetOperationalDBData(dynamic data)
    {
        var postUrl = apiConfig.getOperationalDashboardGraphs;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.PostAsyncWithheaderWithoutEncrypt<dynamic>(postUrl, data, token);
        return response;
    }
#endregion
#region Finaincial
     public async Task<dynamic> GetFinancialBData(dynamic data)
    {
        var postUrl = apiConfig.getFinancialDashboardGraphs;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.PostAsyncWithheaderWithoutEncrypt<dynamic>(postUrl, data, token);
        return response;
    }
#endregion
}
