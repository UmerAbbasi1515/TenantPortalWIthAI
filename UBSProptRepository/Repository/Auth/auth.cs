using System;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.AspNetCore.Http;
using static Google.Apis.Requests.BatchRequest;

public class UBSAuthRepository
{
    private readonly UBSApiHelper apiHelper;
    private readonly UBSAuthApiConfig apiConfig;
    public UBSAuthRepository(UBSApiHelper _apiHelper, IOptions<UBSAuthApiConfig> _apiConfig)
    {
        apiConfig = _apiConfig.Value;
        apiHelper = _apiHelper;
    }



    public async Task<dynamic> GetLanguagesAsync(dynamic data)
    {
        var postUrl = "";// apiConfig.getLanguage;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl, data, "");
        return response;
    }

    public async Task<dynamic> GenerateFirbaseUserToken(dynamic data)
    {
        var postUrl = apiConfig.verifyUserOtpByPhone;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl ?? "", data, "");
        AuthLabelsPortal.jwtToken = response.token;
        AuthLabelsPortal.userNameEng = response.user.name;
        AuthLabelsPortal.userNameAR = response.user.fullNameAr;
        AuthLabelsPortal.mobile = response.user.mobile;
        AuthLabelsPortal.email = response.user.email;
        Console.WriteLine(AuthLabelsPortal.jwtToken);
        Console.WriteLine(AuthLabelsPortal.userNameEng);
        Console.WriteLine(AuthLabelsPortal.userNameAR);
        Console.WriteLine(AuthLabelsPortal.mobile);
        Console.WriteLine(AuthLabelsPortal.email);
        return response;
    }
    public async Task<dynamic> RefreshToken(dynamic data)
    {
        var postUrl = apiConfig.getNewToken;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl, data, AuthLabelsPortal.jwtToken);
       
        AuthLabelsPortal.jwtToken = response.token;
        AuthLabelsPortal.userNameEng = response.user.name;
        AuthLabelsPortal.userNameAR = response.user.fullNameAr;
        AuthLabelsPortal.mobile = response.user.mobile;
        AuthLabelsPortal.email = response.user.email;
        Console.WriteLine(AuthLabelsPortal.jwtToken);
        Console.WriteLine(AuthLabelsPortal.userNameEng);
        Console.WriteLine(AuthLabelsPortal.userNameAR);
        Console.WriteLine(AuthLabelsPortal.mobile);
        Console.WriteLine(AuthLabelsPortal.email);
        return response;
    }
    public async Task<dynamic> GetUserRoles(dynamic data)
    {
        var postUrl = apiConfig.getUserRoles;
        String token = AuthLabelsPortal.jwtToken;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl, data, token);
        if (response is string)
        {
            return response;
        }
        return response;
    }
    public async Task<dynamic> SaveNewMpin(dynamic data)
    {
        var postUrl = apiConfig.saveMpin;
        String token = AuthLabelsPortal.jwtToken;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl, data, token);
        if (response is string)
        {
            return response;
        }
        return response;
    }
    public async Task<dynamic> ProccedToLogin(dynamic data)
    {
        var postUrl = apiConfig.proceedToLogin;
        String token = AuthLabelsPortal.jwtToken;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl, data, token);
        AuthLabelsPortal.jwtToken = response.token;
        return response;
    }
    public async Task<dynamic> ValidateRoleByMPin(dynamic data)
    {
        var postUrl = apiConfig.validateRoleByMpin;
        String token = AuthLabelsPortal.jwtToken;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl, data, token);
        var newToken = response.token;
        AuthLabelsPortal.jwtToken = newToken;
        return response;
    }
    public async Task<dynamic> validatePublicRole(dynamic data)
    {
        var postUrl = apiConfig.validatePublicRole;
        String token = AuthLabelsPortal.jwtToken;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl, data, token);
        AuthLabelsPortal.jwtToken = response.token;
        return response;
    }
    
    // Email Flow
    public async Task<dynamic> validateUserByEmail(dynamic data)
    {
        var postUrl = apiConfig.validateUserbyEmail;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl ?? "", data, "");
        return response;
    }
    public async Task<dynamic> forgotPasswordByEmail(dynamic data)
    {
        var postUrl = apiConfig.forgetPasswordByEmail;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl ?? "", data, "");
        return response;
    }
    public async Task<dynamic> verifyOTPByEmail(dynamic data)
    {
        var postUrl = apiConfig.verifyUserOtpbyEmail;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl ?? "", data, "");
        return response;
    }
    public async Task<dynamic> savePasswordByEmail(dynamic data)
    {
        var postUrl = apiConfig.saveUserPasswordbyEmail;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl ?? "", data, "");
        return response;
    }
    public async Task<dynamic> validatePasswordByEmail(dynamic data)
    {
        var postUrl = apiConfig.validatePasswordforEmail;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl ?? "", data, "");
        return response;
    }
}
