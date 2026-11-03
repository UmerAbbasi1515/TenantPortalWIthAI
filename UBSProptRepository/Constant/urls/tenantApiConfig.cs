using System;
using System.Diagnostics.Contracts;
using Microsoft.Extensions.Configuration;

public class UBSTenantApiConfig
{
    public string? environment { get; set; }
    private readonly string baseUrlAuthDEV = "https://auth.api.ubspropt.com/api/"; // Ali hamza
    private readonly string baseUrlAuthUAT = "https://10.103.102.4/Auth/api/"; // Rizwan 
    private readonly string baseUrlAuthPROD = "https://landlord.fabproperties.ae/auth01/api/";
    private readonly string baseUrlAuthProvis = "https://provisuat.uaenorth.cloudapp.azure.com/fabpauth01/api/"; 
    private readonly string baseUrlAuthNgRok = "https://depth-evolution-making.ngrok-free.dev/api/";
    private string baseUrlAuth => environment switch
    {
        "DEV" => baseUrlAuthDEV,
        "UAT" => baseUrlAuthUAT,
        "PROD" => baseUrlAuthPROD,
        "PROVIS" => baseUrlAuthProvis,
        "NGRok" => baseUrlAuthNgRok,
        _ => baseUrlAuthDEV
    };

    private readonly string baseUrlTenantDEV = "https://tenant.api.ubspropt.com/api/";
    private readonly string baseUrlTenantUAT = "https://10.103.102.4/Tenant/api/";
    private readonly string baseUrlTenantPROD = "https://landlord.fabproperties.ae/tenant01/api/";
    private readonly string baseUrlTenantProvis = "https://provisuat.uaenorth.cloudapp.azure.com/fabptenant01/api/";
    private readonly string baseUrlTenantNgRok = "https://depth-evolution-making.ngrok-free.dev/api/";
    private string baseUrlTenant => environment switch
    {
        "DEV" => baseUrlTenantDEV,
        "UAT" => baseUrlTenantUAT,
        "PROD" => baseUrlTenantPROD,
        "PROVIS" => baseUrlTenantProvis,
        "NGRok" => baseUrlTenantNgRok,
        _ => baseUrlTenantDEV
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

    // Tenant

    public string getContracts  => $"{baseUrlTenant}Tenant/Contracts/GetContracts";
    public string getContractDetails => $"{baseUrlTenant}Tenant/Contracts/GetContractDetails";
    public string getContractPayable => $"{baseUrlTenant}Tenant/Contracts/ContractPayables";
    public string getCanCheckinContract => $"{baseUrlTenant}Tenant/Contracts/CanCheckIn";
    public string caseCheckinContract => $"{baseUrlTenant}Tenant/Contracts/CaseCheckinContract";
    public string downloadCheque => $"{baseUrlTenant}Tenant/Contracts/DownloadCheque";
    public string downloadChequeNew => $"{baseUrlTenant}Tenant/Contracts/DownloadChequeNew";
    public string getNotifications  => $"{baseUrlTenant}Tenant/Notifications/Get";
   public string downloadNotificationFile => $"{baseUrlTenant}Tenant/Notifications/DownloadNotificationFile";
    public string getNotificationFiles => $"{baseUrlTenant}Tenant/Notifications/GetNotificationFiles";
    public string getTenantProfile  => $"{baseUrlTenant}Tenant/Profile/Get";
    public string canEditTenantProfile => $"{baseUrlTenant}Tenant/Profile/CanEditProfile";
    public string getServiceRequests => $"{baseUrlTenant}Tenant/ServiceRequests/GetTenantServiceRequests";
    public string getServiceRequestsDetail => $"{baseUrlTenant}Tenant/ServiceRequests/GetServiceRequestDetails";
    public string getServiceRequestThumbnailList => $"{baseUrlTenant}Tenant/ServiceRequests/GetServiceRequestThumbnailList";
    public string cancelRequest => $"{baseUrlTenant}Tenant/ServiceRequests/CancelRequest";
    public string getSurveyQuestions => $"{baseUrlTenant}Tenant/Survey/GetSurveyQuestions";
    public string getSurveyQuestionAnswers => $"{baseUrlTenant}Tenant/Survey/GetSurveyQuestionAnswer";
    public string saveSurveyAnswer => $"{baseUrlTenant}Tenant/Survey/SaveSurveyAnswers";
    public string saveTenantFeedback => $"{baseUrlTenant}Tenant/ServiceRequests/SaveSRFeedback";
    public string getTenantFeedback => $"{baseUrlTenant}Tenant/ServiceRequests/GetSRFeedback";
    public string getServiceRequestDocumentsPdf => $"{baseUrlTenant}Tenant/ServiceRequests/GetServiceRequestDocumentsPdf";

    public string getServiceRequestsDocsByType => $"{baseUrlTenant}Tenant/ServiceRequests/GetServiceRequestDocumentsByType";
    public string UploadServiceRequestFiles => $"{baseUrlTenant}Tenant/ServiceRequests/UploadServiceRequestFiles";
    public string UpdateServiceRequestFiles => $"{baseUrlTenant}Tenant/ServiceRequests/UpdateCaseAttachment";

    public string getServiceRequestsTicketReplies => $"{baseUrlTenant}Tenant/ServiceRequests/GetTicketReplies";
    public string getServiceRequestsAddTicketReply => $"{baseUrlTenant}Tenant/ServiceRequests/AddTicketReply";
    public string getServiceRequestsDownloadTicketFile => $"{baseUrlTenant}Tenant/ServiceRequests/DownloadTicketFile";
   
    public string saveServiceRequestsCaseCategory => $"{baseUrlTenant}Tenant/ServiceRequests/SaveServiceRequestv1";
    public string getServiceRequestsCaseCategory => $"{baseUrlTenant}Tenant/ServiceRequests/CaseCategory"; // {}
    public string getServiceRequestsCaseSubCategory => $"{baseUrlTenant}Tenant/ServiceRequests/CaseSubCategory"; // var data={"CategoryId":""};
    public string getServiceRequestsTenantContractUnit => $"{baseUrlTenant}Tenant/ServiceRequests/GetTenantContractUnit"; // var data = {"Tenantid": '0'};
    public string getServiceRequestsGetContactTiming => $"{baseUrlTenant}Tenant/ServiceRequests/GetContactTiming";  // {}
    public string tenantDownloadDoc => $"{baseUrlTenant}Tenant/ServiceRequests/DownloadServiceRequestDocuments";
    public string removeSvcReqPhoto => $"{baseUrlTenant}Tenant/ServiceRequests/RemoveServiceRequestFiles";
    public string removeCheque => $"{baseUrlTenant}Tenant/Contracts/RemoveContractCheque";
    public string updateContractDocumentStage => $"{baseUrlTenant}Tenant/ServiceRequests/UpdateContractDocumentStage";
    public string getContractUnits  => $"{baseUrlTenant}Tenant/Contracts/GetContractUnits";
    public string getContractUnitDetails => $"{baseUrlTenant}Tenant/Contracts/GetUnitDetails";
    public string getUnitAditionalDetails => $"{baseUrlTenant}Tenant/Contracts/GetUnitAditionalDetails";
    public string getTenantNotifications  => $"{baseUrlTenant}Tenant/Notifications/Get";
    public string tenantReadNotifications  => $"{baseUrlTenant}Tenant/Notifications/Read";
    public string tenantArchiveNotifications => $"{baseUrlTenant}Tenant/Notifications/Archived";
    public string tenantNotificationsDetails => $"{baseUrlTenant}Tenant/Notifications/GetDetails";
    public string getContractPayments => $"{baseUrlTenant}Tenant/Contracts/GetContractPayments";
    public string getContractCharges => $"{baseUrlTenant}Tenant/Contracts/GetContractCharges";
    public string payments  => $"{baseUrlTenant}Tenant/Payments";
    public string paymentsGet  => $"{baseUrlTenant}Tenant/Payments/Get";
    public string getStats  => $"{baseUrlTenant}Tenant/Dashboard/GetStats";
    public string dashboard  => $"{baseUrlTenant}dashboard/getdata?TenantId=";
     public string tenantDashboardGetData => $"{baseUrlTenant}Tenant/Dashboard/GetData";
    public string tenantDashboardGetPopUp => $"{baseUrlTenant}Tenant/Dashboard/GetDashboardNotifications";
    public string getOnlinePayments => $"{baseUrlTenant}Tenant/GatewayPayment/ViewOnlinePaymant";
    public string getContractCheques => $"{baseUrlTenant}Tenant/Contracts/GetContractCheques";
    public string toBePaidIn30Days  => $"{baseUrlTenant}Tenant/Dashboard/ToBePaidIn30Days";
    public string contractsExpiringIn30Days => $"{baseUrlTenant}Tenant/Contracts/ContractExpire30Days";
    public string getPropTypes  => $"{baseUrlTenant}Tenant/Contracts/GetPropertyTypes";
    public string getContractStatus  => $"{baseUrlTenant}Tenant/Contracts/GetContractStatus";
    public string getContractswithFilter => $"{baseUrlTenant}Tenant/Contracts/GetContractswithFilter";
    public string contractChargeReceipts => $"{baseUrlTenant}Tenant/Contracts/ContractChargeReceipts";
    public string paymentsDownloadReceipt => $"{baseUrlTenant}Tenant/Payments/DownloadReceipt";
    public string contractDownload  => $"{baseUrlTenant}Tenant/Contracts/DownloadContract";
    public string contractDownloadNew => $"{baseUrlTenant}Tenant/Contracts/DownloadContractNew";
    public string downloadContractTerms => $"{baseUrlTenant}Tenant/Contracts/DownloadContractTerms"; 
    public string downloadContractTermsNew => $"{baseUrlTenant}Tenant/Contracts/DownloadNewContractTerms";
    public string downloadOfferLetter => $"{baseUrlTenant}Tenant/Contracts/DownloadOfferLetter"; //var data = { "ContractId":contractId.toString() };
    public string contractRenewalActions => $"{baseUrlTenant}Tenant/Contracts/ContractRenewalActions";
    public string newContractActions => $"{baseUrlTenant}Tenant/Contracts/newContractActions";

    


    public string getContractRenew => $"{baseUrlTenant}Tenant/Case/GetRenewalContractInfo"; //var data = { "ContractId":contractId.toString() };
    public string addRenewalRequest => $"{baseUrlTenant}Tenant/Case/AddRenewalRequest";   // 'contractId': contractId,'duration': '12 Month','startDate': startDate, 'endDate': endDate, 'dueActionid': dueActionid
    public string getExtensionPeriod => $"{baseUrlTenant}Tenant/Case/GetExtensionPeriod";//var data = { "ContractId":contractId.toString() };
    public string addExtensionRequest => $"{baseUrlTenant}Tenant/Case/AddExtensionRequest"; // 'contractId': contractId,'duration': '12 Month','startDate': startDate, 'endDate': endDate, 'dueActionid': dueActionid
    public string getVacatingList => $"{baseUrlTenant}Tenant/Case/GetVacatingList"; // {}
    public string addTerminateRequest => $"{baseUrlTenant}Tenant/Case/AddTerminateRequest";


    public string contractNewActions => $"{baseUrlTenant}Tenant/Contracts/ContractNewActions";
    public string getOutstandingPayments => $"{baseUrlTenant}Tenant/Contracts/GetOutstandingPayments";
    public string getOutstandingPaymentsNew => $"{baseUrlTenant}Tenant/Contracts/GetOutstandingPaymentsNew";
    public string updateContractPaymentMethod => $"{baseUrlTenant}Tenant/Contracts/UpdateContractPaymentMethod";
    public string registerPayment => $"{baseUrlTenant}Tenant/GatewayPayment/RegisterPayment";
    

    public string updateContractPaymentAddress => $"{baseUrlTenant}Tenant/Contracts/UpdateContractPaymentAddress";
    public string onlinePayables => $"{baseUrlTenant}Tenant/Contracts/OnlinePayments";
    public string getPolicyData => $"{baseUrlTenant}Tenant/GeneralData/GetConfigurationValue";
    public string updateContractPaymentMethodNew => $"{baseUrlTenant}Tenant/Contracts/UpdateContractPaymentMethodNew";
    public string updateContractStage => $"{baseUrlTenant}Tenant/Contracts/UpdateContractStage";
    public string updateContractStageNew => $"{baseUrlTenant}Tenant/Contracts/UpdateContractStageNew";
    public string getApproveMunicipal => $"{baseUrlTenant}Tenant/Contracts/GetApproveMunicipal";
    public string getApproveMunicipalNew => $"{baseUrlTenant}Tenant/Contracts/GetApproveMunicipalNew";
    public string downloadSignedContract => $"{baseUrlTenant}Tenant/Contracts/DownloadSignedContract";
    public string canDownloadContract => $"{baseUrlTenant}Tenant/Contracts/CanDownloadContract";

    public string unVerfiedContractPayment => $"{baseUrlTenant}Tenant/GatewayPayment/UnVerfiedContractPayment";
    public string unVerfiedAllPayment => $"{baseUrlTenant}Tenant/GatewayPayment/UnVerfiedAllPayment?ContractId=";
    public string GetFaqsCategories => $"{baseUrlTenant}Tenant/ServiceRequests/GetFaqs";
    public string GetFaqs => $"{baseUrlTenant}Tenant/ServiceRequests/GetFaqCategories";
    public string GetOffer => $"{baseUrlTenant}Tenant/Offer/GetOffer"; 
    public string GetOfferDetail => $"{baseUrlTenant}Tenant/Offer/GetOfferDetail";
    public string GetOffersFromUbsPropT => $"{baseUrlTenant}Tenant/Offer/GetOffersFromUbsPropT"; 
    public string GetOfferDocuments => $"{baseUrlTenant}Tenant/Offer/GetOfferDocuments"; 
    public string UploadOfferFiles => $"{baseUrlTenant}Tenant/Offer/UploadOfferFiles";
    public string DownloadOfferDocuments => $"{baseUrlTenant}Tenant/Offer/DownloadOfferDocuments";
    public string GetOfferTicketReplies => $"{baseUrlTenant}Tenant/Offer/GetOfferTicketReplies";
    public string DownloadOfferTicketFile => $"{baseUrlTenant}Tenant/Offer/DownloadOfferTicketFile";
    public string AddOfferTicketReply => $"{baseUrlTenant}Tenant/Offer/AddOfferTicketReply";
}
