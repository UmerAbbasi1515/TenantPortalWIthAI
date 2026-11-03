using System;
using System.Diagnostics.Contracts;
using Microsoft.Extensions.Configuration;

public class UBSPublicApiConfig
{
    public string? environment { get; set; }
    private readonly string baseUrlAuthDEV = "https://auth.api.ubspropt.com/api/"; // Ali hamza
    private readonly string baseUrlAuthUAT = "https://10.103.102.4/Auth/api/"; // Rizwan 
    private readonly string baseUrlAuthPROD = "https://landlord.fabproperties.ae/api/";
    private readonly string baseUrlAuthProvis = "https://provisuat.uaenorth.cloudapp.azure.com/fabpauth01/api/";
    private string baseUrlAuth => environment switch
    {
        "DEV" => baseUrlAuthDEV,
        "UAT" => baseUrlAuthUAT,
        "PROD" => baseUrlAuthPROD,
        "PROVIS" => baseUrlAuthProvis,
        _ => baseUrlAuthDEV
    };

    private readonly string baseUrlPublicDEV = "https://public.api.ubspropt.com/api/";
    private readonly string baseUrlPublicUAT = "https://10.103.102.4/Public/api/";
    private readonly string baseUrlPublicPROD = "https://landlord.fabproperties.ae/api/";
    private readonly string baseUrlPublicProvis = "https://provisuat.uaenorth.cloudapp.azure.com/fabppublic01/api/";
    private string baseUrlPublic => environment switch
    {
        "DEV" => baseUrlPublicDEV,
        "UAT" => baseUrlPublicUAT,
        "PROD" => baseUrlPublicPROD,
        "PROVIS" => baseUrlPublicProvis,
        _ => baseUrlPublicDEV
    };


    // Auth by Email
    public string validateUserbyEmail => $"{baseUrlAuth}AuthEmail/ValidateUserbyEmail";
    public string verifyUserOtpbyEmail => $"{baseUrlAuth}AuthEmail/VerifyUserOtp";
    public string saveUserPasswordbyEmail => $"{baseUrlAuth}AuthEmail/SaveUserPassword";
    public string validatePasswordforEmail => $"{baseUrlAuth}AuthEmail/ValidatePasswordforEmail";
    public string forgetPasswordByEmail => $"{baseUrlAuth}AuthEmail/ForgetPasswordByEmail";

    // Auth by Phone
    public string verifyUserOtpByPhone => $"{baseUrlAuth}Auth/GenerateFirbaseUserToken";
    public string getUserRoles => $"{baseUrlAuth}Account/GetUserRoles";
    public string proceedToLogin => $"{baseUrlAuth}Account/ProceedToLogin";
    public string validateRoleByMpin => $"{baseUrlAuth}Account/ValidateRolebyMpin";
    public string validatePublicRole => $"{baseUrlAuth}Auth/ValidatePublicRole";
    public string compareDeviceToken => $"{baseUrlAuth}Account/CompareToken";
    public string getNewToken => $"{baseUrlAuth}Auth/RefreshToken";

    // public
    public string getEmirates => $"{baseUrlPublic}Public/Property/GetEmirate";
    public string getPropertyCategory => $"{baseUrlPublic}Public/Property/GetPropertyCategory";
    public string getUnitTypeByCategoryId => $"{baseUrlPublic}Public/Property/GetUnitTypeByCategory";
    public string getProperties => $"{baseUrlPublic}Public/Property/GetUnitProperties";
    public string getPropertyDetail => $"{baseUrlPublic}Public/Property/GetUnitPropertyDetail";
    public string getBookingReqImages => $"{baseUrlPublic}Public/Property/GetUnitImage";
    public string getBookingReqPropertyImage => $"{baseUrlPublic}Public/Property/GetPropertyImage";

    public string getPublicFaqsCatg => $"{baseUrlPublic}Public/GeneralData/GetFaqCategories";
    public string getPublicFaqsQuestionAndDescription => $"{baseUrlPublic}Public/GeneralData/GetFaqs";
    public string getPublicLocation => $"{baseUrlPublic}Public/GeneralData/GetLocations";
    public string getPublicServicesCategoriesDetails => $"{baseUrlPublic}Public/GeneralData/GetServices";
    public string getPublicServiceCategories => $"{baseUrlPublic}Public/GeneralData/GetServicesCategories";
    public string publicPropertyManagement => $"{baseUrlPublic}Public/GeneralData/GetPropertyManagement";

    public string getPublicBookingAgentList => $"{baseUrlPublic}Public/BookingRequest/GetAgentList";
    public string savePublicBookingRequest => $"{baseUrlPublic}Public/BookingRequest/SaveBookingRequest";
    public string getPublicServices => $"{baseUrlPublic}Public/BookingRequest/GetServiceRequests";
    public string publicServiceMianinfo => $"{baseUrlPublic}Public/BookingRequest/GetRequestDetails";
    public string savePublicFeedback => $"{baseUrlPublic}Public/BookingRequest/SaveBRFeedback";
    public string getPublicFeedback => $"{baseUrlPublic}Public/BookingRequest/GetBRFeedback";
    public string cancelPublicBookingreq => $"{baseUrlPublic}Public/BookingRequest/CancelRequest";
    public string publicAddTicketReply => $"{baseUrlPublic}Public/BookingRequest/AddTicketReply";
    public string publicGetTicketReplies => $"{baseUrlPublic}Public/BookingRequest/GetTicketReplies";
    public string publicGetTicketRepliesDownload => $"{baseUrlPublic}Public/BookingRequest/DownloadTicketFile";

    public string getPublicOffers => $"{baseUrlPublic}Public/Offer/GetOffer";
    public string getPublicOffersDetails => $"{baseUrlPublic}Public/Offer/GetOfferDetail";
    
    public string publicCanEditProfile => $"{baseUrlPublic}Public/Profile/CanEditProfile";
    public string publicGetProfile => $"{baseUrlPublic}Public/Profile/GetProfile";
    public string publicUpdateProfile => $"{baseUrlPublic}Public/Profile/UpdateProfileRequest";
    public string publicUpdateProfile2 => $"{baseUrlPublic}Public/Profile/UpdateProfile";

    public string getPublicNotification => $"{baseUrlPublic}Public/Notifications/Get";
    public string publicNotificationDetails => $"{baseUrlPublic}Public/Notifications/GetDetails";
    public string publicArchivedNotifications => $"{baseUrlPublic}Public/Notifications/Archived";
    public string publicReadNotifications => $"{baseUrlPublic}Public/Notifications/Read";
    public string publicCountNotifications => $"{baseUrlPublic}Public/Notifications/GetCountNotification";
}