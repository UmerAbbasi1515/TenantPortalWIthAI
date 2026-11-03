using System;
using Microsoft.Extensions.Configuration;

public class UBSAuthApiConfig
{
    public string? environment { get; set; }

    private readonly string baseUrlAuth229 = "http://40.80.86.229:9024/api/";
    private readonly string baseUrlAuthDEV = "https://auth.api.ubspropt.com/api/";
    private readonly string baseUrlAuthUAT = "https://10.103.102.4/Auth/api/";
    private readonly string baseUrlAuthPROD = "https://landlord.fabproperties.ae/auth01/api/";
    private readonly string baseUrlAuthProvis = "https://provisuat.uaenorth.cloudapp.azure.com/fabpauth01/api/";
    private string BaseUrlAuth => environment switch
    {
        "229" => baseUrlAuth229,
        "DEV" => baseUrlAuthDEV,
        "UAT" => baseUrlAuthUAT,
        "PROD" => baseUrlAuthPROD,
        "PROVIS" => baseUrlAuthProvis,
        _ => baseUrlAuthDEV
    };

     public string validateUserbyEmail => $"{BaseUrlAuth}AuthEmail/ValidateUserbyEmail";
     public string? verifyUserOtpbyEmail => $"{BaseUrlAuth}AuthEmail/VerifyUserOtp";
     public string? saveUserPasswordbyEmail => $"{BaseUrlAuth}AuthEmail/SaveUserPassword";
     public string? validatePasswordforEmail => $"{BaseUrlAuth}AuthEmail/ValidatePasswordforEmail";
     public string? forgetPasswordByEmail => $"{BaseUrlAuth}AuthEmail/ForgetPasswordByEmail";
     public string? verifyUserOtpByPhone => $"{BaseUrlAuth}Auth/GenerateFirbaseUserToken";
     public string? getUserRoles => $"{BaseUrlAuth}Account/GetUserRoles";
     public string? proceedToLogin => $"{BaseUrlAuth}Account/ProceedToLogin";
     public string? validateRoleByMpin => $"{BaseUrlAuth}Account/ValidateRolebyMpin";
     public string? validatePublicRole => $"{BaseUrlAuth}Auth/ValidatePublicRole";
    public string? saveMpin => $"{BaseUrlAuth}Account/SaveMpin";
     public string? compareDeviceToken => $"{BaseUrlAuth}Account/CompareToken";
     public string? getNewToken => $"{BaseUrlAuth}Auth/RefreshToken";
}


