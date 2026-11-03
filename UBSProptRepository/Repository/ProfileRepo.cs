using System;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

public class ProfileRepository
{

    private readonly UBSApiHelper apiHelper;
    public ProfileRepository(UBSApiHelper _apiHelper)
    {
        apiHelper = _apiHelper;
    }

    public async Task<dynamic> GetProfile(dynamic data,dynamic url,dynamic token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> GetProfileUpdate(dynamic data, dynamic url, dynamic token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> GetProfileDoc(dynamic data, dynamic url, dynamic token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> CanEditProfile(dynamic data, dynamic url, dynamic token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> GetProfileContractor(dynamic data, dynamic url, dynamic token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> GetProfileAccounts(dynamic data, dynamic url, dynamic token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> GetProfileContacts(dynamic data, dynamic url, dynamic token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }

}
