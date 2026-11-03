using System;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class LpoRepository
{
    private readonly UBSApiHelper apiHelper;

    public LpoRepository(UBSApiHelper _apiHelper)
    {
        apiHelper = _apiHelper;
    }

    #region Lpo
    public async Task<dynamic> GetAllLpos(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    #endregion

    #region Lpo Detail
    public async Task<dynamic> GetAllLposPropeties(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> GetAllLposSerives(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> GetAllLposTerms(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> GetAllLposinvoices(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    #endregion

}


