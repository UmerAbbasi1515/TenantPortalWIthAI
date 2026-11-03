using System;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

public class FaqsRepository
{

    private readonly UBSApiHelper apiHelper;
    public FaqsRepository(UBSApiHelper _apiHelper)
    {
        apiHelper = _apiHelper;
    }

    public async Task<dynamic> GetFaqCategories(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> GetFaqsDetail(dynamic data, string url, string token)
    {
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }

}
