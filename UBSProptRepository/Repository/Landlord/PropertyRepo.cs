using System;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

public class PropertyRepository
{
    //private readonly ApiHelper apiHelper;
    //public PropertyRepository(ApiHelper _apiHelper)
    //{
    //    apiHelper = _apiHelper;
    //}
    //apiConfig apiConfig = apiConfig.Instance;

    private readonly UBSApiHelper apiHelper;
    private readonly UBSLandlordApiConfig apiConfig;
    public PropertyRepository(UBSApiHelper _apiHelper, IOptions<UBSLandlordApiConfig> _apiConfig)
    {
        apiHelper = _apiHelper;
        apiConfig = _apiConfig.Value;
    }

    public async Task<dynamic> GetPropertyDetails(dynamic data)
    {
        var postUrl = apiConfig.GetLandlordPropertyDetails;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl, data, token);
        return response;
    }

    public async Task<dynamic> GetPropertyUnites(dynamic data)
    {
        var postUrl = apiConfig.GetLandlordPropertyUnits;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl ?? "", data, token);
        return response;
    }
    public async Task<dynamic> GetPropertyUnitesDetail(dynamic data)
    {
        var postUrl = apiConfig.GetLandlordPropertyUnitDetails;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl, data, token);
        return response;
    }
}
