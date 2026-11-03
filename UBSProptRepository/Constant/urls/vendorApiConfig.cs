using System;
using System.Diagnostics.Contracts;
using Microsoft.Extensions.Configuration;

public class UBSVendorApiConfig
{
    public string? environment { get; set; }
    private readonly string baseUrlAuthDEV = "https://auth.api.ubspropt.com/api/";
    private readonly string baseUrlAuthUAT = "https://10.103.102.4/Auth/api/";
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

    private readonly string baseUrlVendorDEV = "https://vendor.api.ubspropt.com/api/";
    private readonly string baseUrlVendorUAT = "https://10.103.102.4/Tenant/api/";
    private readonly string baseUrlVendorPROD = "https://landlord.fabproperties.ae/api/";
    private readonly string baseUrlVendorProvis = "https://provisuat.uaenorth.cloudapp.azure.com/fabpvendor01/api/";
    private string baseUrlVendor => environment switch
    {
        "DEV" => baseUrlVendorDEV,
        "UAT" => baseUrlVendorUAT,
        "PROD" => baseUrlVendorPROD,
        "PROVIS" => baseUrlVendorProvis,
        _ => baseUrlVendorDEV
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

    // Vendor
    public string getContracts => $"{baseUrlVendor}Vendor/Contracts/GetContracts"; // var data = {"pageNo": pageNo, "pageSize": '20', "search": searchtext};
    public string getContractDetails => $"{baseUrlVendor}Vendor/Contracts/GetContractDetails";
    public string getVendorContractsStatus => $"{baseUrlVendor}Vendor/Contracts/GetContractStatus";
    public string getVendorContractswithFilter => $"{baseUrlVendor}Vendor/Contracts/GetContractsWithFilter"; 
    public string getVendorContractInvoices => $"{baseUrlVendor}Vendor/Contracts/ContractInvoices"; // Map data = { "ContractId": contractId.toString() };
    public string getContractFinancialTerms => $"{baseUrlVendor}Vendor/Contracts/GetContractFinancialTerms"; // Map data={ "ContractId":contractId, "pageNo":1.toString(), "pageSize":100.toString(), };
    public string getVendorContractProperties => $"{baseUrlVendor}Vendor/Contracts/GetContractProperties";// Map data = {"ContractId": contractID};
    public string lpoInvoices => $"{baseUrlVendor}Vendor/Contracts/LpoInvoices";
    public string contractInvoices => $"{baseUrlVendor}Vendor/Contracts/ContractInvoices";
    public string getPropertyImageVendor => $"{baseUrlVendor}Vendor/Contracts/GetPropertyImage";
 
    public string getContactPersons => $"{baseUrlVendor}Vendor/ContactPersons/GetContactPersons";
    

    public string getVendorAccounts => $"{baseUrlVendor}Vendor/Accounts/GetAccountInfo";

    public string getNotifications => $"{baseUrlVendor}Vendor/Notifications/Get"; 
    public string getNotificationsDetail => $"{baseUrlVendor}Vendor/Notifications/GetDetails";
    public string getNotificationsArchived => $"{baseUrlVendor}Vendor/Notifications/Archived";
    public string getNotificationsRead => $"{baseUrlVendor}Vendor/Notifications/Read";

    public string contractorProfile => $"{baseUrlVendor}Vendor/Profile/Get"; 
    public string updateContactersons => $"{baseUrlVendor}Vendor/Profile/UpdateProfileRequest";
    public string contactPerson => $"{baseUrlVendor}Vendor/ContactPersons/GetContactPersons"; 
         public string accounts => $"{baseUrlVendor}Vendor/Accounts/GetAccountInfo";
    public string updateProfileRequest => $"{baseUrlVendor}Vendor/Profile/UpdateProfileRequest";

    public string getServiceRequests => $"{baseUrlVendor}Vendor/ServiceRequests/GetVendorServiceRequests";
    public string getServiceRequestsDetail => $"{baseUrlVendor}Vendor/ServiceRequests/GetServiceRequestDetail";
    public string vendorSaveInvoiceServiceRequest => $"{baseUrlVendor}Vendor/ServiceRequests/SaveInvoiceServiceRequest";
    public string vendorGetInvoiceServiceRequest => $"{baseUrlVendor}Vendor/ServiceRequests/GetCaseServiceRequestDetail";
    public string UploadServiceRequestFiles => $"{baseUrlVendor}Vendor/ServiceRequests/UploadServiceRequestFiles";
  
    public string RemoveServiceRequestFiles => $"{baseUrlVendor}Vendor/ServiceRequests/RemoveServiceRequestFiles";
    public string vendorUploadInvoiceServiceRequestFiles => $"{baseUrlVendor}Vendor/ServiceRequests/UploadInvoiceServiceRequestFiles";
    public string vendorUpdateServiceRequestFiles => $"{baseUrlVendor}Vendor/ServiceRequests/UpdateCaseAttachment";
    public string vendorGetServiceRequestImages => $"{baseUrlVendor}Vendor/ServiceRequests/GetServiceRequestImages";
    public string vendorGetServiceRequestDocs => $"{baseUrlVendor}Vendor/ServiceRequests/GetServiceRequestDocuments";
    public string vendorRemoveSvcReqPhoto => $"{baseUrlVendor}Vendor/ServiceRequests/RemoveServiceRequestFiles";
    public string vendorCloseRequest => $"{baseUrlVendor}Vendor/ServiceRequests/CloseServiceRequest";
    public string vendorAcknowledgeCase => $"{baseUrlVendor}Vendor/ServiceRequests/AcknowledgeCase";
    public string contractorRejected => $"{baseUrlVendor}Vendor/ServiceRequests/ContractorRejected";
    public string uploadVendorSign => $"{baseUrlVendor}Vendor/ServiceRequests/UploadVendorSign";
    public string uploadTenantSign => $"{baseUrlVendor}Vendor/ServiceRequests/UploadTenantSign";
    public string vendorGetDocsByType => $"{baseUrlVendor}Vendor/ServiceRequests/GetServiceRequestDocumentsByType";
    public string vendorDownloadDoc => $"{baseUrlVendor}Vendor/ServiceRequests/DownloadServiceRequestDocuments";
    
    public string vendorGetTickets => $"{baseUrlVendor}Vendor/ServiceRequests/GetTicketReplies";
    public string vendorAddTicket => $"{baseUrlVendor}Vendor/ServiceRequests/AddTicketReply";
    public string vendorDownloadTicketFile => $"{baseUrlVendor}Vendor/ServiceRequests/DownloadTicketFile";
    public string vendorGetSRReportDetail => $"{baseUrlVendor}Vendor/ServiceRequests/GetSRReportDetail";

    public string GetOffer => $"{baseUrlVendor}Vendor/Offer/GetOffer";
    public string GetOfferDetail => $"{baseUrlVendor}Vendor/Offer/GetOfferDetail";

    public string GetFaqsCategories => $"{baseUrlVendor}Vendor/GeneralData/GetFaqs";
    public string GetFaqs => $"{baseUrlVendor}Vendor/GeneralData/GetFaqCategories";

    public string getstats => $"{baseUrlVendor}Vendor/Dashboard/GetData"; // var data;
    public string getdashboardlpo => $"{baseUrlVendor}Vendor/Dashboard/GetLpoProperties";
    public string vendorDashboardAllInvoces => $"{baseUrlVendor}Vendor/Dashboard/GetAllInvoices";//

    public string getlpoproperties => $"{baseUrlVendor}Vendor/Lpo/GetLpoProperties";
    public string getlpos => $"{baseUrlVendor}Vendor/Lpo/GetLpoData";
    public string getlposdetail => $"{baseUrlVendor}Vendor/Lpo/GetLpoDetails";
    public string getlpoterm => $"{baseUrlVendor}Vendor/Lpo/GetTerms";
    public string getlposervies => $"{baseUrlVendor}Vendor/Lpo/GetServices";
    public string getlpostatus => $"{baseUrlVendor}Vendor/Lpo/GetLpoStatus";
    public string getLpowithFilter => $"{baseUrlVendor}Vendor/Lpo/GetAllLpowithFilter";
    public string getAllLpo => $"{baseUrlVendor}Vendor/Lpo/GetAllLpo"; //  Map data = {"pageNo": pageNoP.toString(),"pageSize": 20.toString(),"search": searchtext};
    public string getLpoDropdownForIvoice => $"{baseUrlVendor}Vendor/Lpo/LpoDropdown";
    
    public string getAMCDropdownForInvoice => $"{baseUrlVendor}Vendor/Lpo/AMCDropdown";
    public string getAmcInstallmentForInvoice => $"{baseUrlVendor}Vendor/Lpo/AmcInstallment";
    public string saveInvoiceServiceRequest => $"{baseUrlVendor}Vendor/ServiceRequests/SaveInvoiceServiceRequest";
    public string getInvoiceDocuments => $"{baseUrlVendor}Vendor/ServiceRequests/GetServiceRequestDocumentsByType";
    public string getInvoiceMainInfo => $"{baseUrlVendor}Vendor/ServiceRequests/GetCaseServiceRequestDetail";


    // Tenant
    private readonly string baseUrlTenant = "https://provisuat.uaenorth.cloudapp.azure.com/fabptenant01/api/";
    public string getServiceRequestThumbnailList => $"{baseUrlTenant}Tenant/ServiceRequests/GetServiceRequestThumbnailList";
    public string getSurveyQuestions => $"{baseUrlTenant}Tenant/Survey/GetSurveyQuestions";
    public string getSurveyQuestionAnswers => $"{baseUrlTenant}Tenant/Survey/GetSurveyQuestionAnswer";
    public string saveSurveyAnswer => $"{baseUrlTenant}Tenant/Survey/SaveSurveyAnswers";
    public string saveTenantFeedback => $"{baseUrlTenant}Tenant/ServiceRequests/SaveSRFeedback";
    public string getTenantFeedback => $"{baseUrlTenant}Tenant/ServiceRequests/GetSRFeedback";

    public string getServiceRequestsDocsByType => $"{baseUrlTenant}Tenant/ServiceRequests/GetServiceRequestDocumentsByType";
    

    public string getServiceRequestsTicketReplies => $"{baseUrlTenant}Tenant/ServiceRequests/GetTicketReplies";
    public string getServiceRequestsAddTicketReply => $"{baseUrlTenant}Tenant/ServiceRequests/AddTicketReply";
    public string getServiceRequestsDownloadTicketFile => $"{baseUrlTenant}Tenant/ServiceRequests/DownloadTicketFile";

    public string saveServiceRequestsCaseCategory => $"{baseUrlTenant}Tenant/ServiceRequests/SaveServiceRequest";
    public string getServiceRequestsCaseCategory => $"{baseUrlTenant}Tenant/ServiceRequests/CaseCategory"; // {}
    public string getServiceRequestsCaseSubCategory => $"{baseUrlTenant}Tenant/ServiceRequests/CaseSubCategory"; // var data={"CategoryId":""};
    public string getServiceRequestsTenantContractUnit => $"{baseUrlTenant}Tenant/ServiceRequests/GetTenantContractUnit"; // var data = {"Tenantid": '0'};
    public string getServiceRequestsGetContactTiming => $"{baseUrlTenant}Tenant/ServiceRequests/GetContactTiming";  // {}



    public string getContractUnits => $"{baseUrlTenant}Tenant/Contracts/GetContractUnits";
    public string getContractUnitDetails => $"{baseUrlTenant}Tenant/Contracts/GetUnitDetails";
    public string getUnitAditionalDetails => $"{baseUrlTenant}Tenant/Contracts/GetUnitAditionalDetails";
    public string getTenantNotifications => $"{baseUrlTenant}Tenant/Notifications/Get";
    public string tenantReadNotifications => $"{baseUrlTenant}Tenant/Notifications/Read";
    public string tenantArchiveNotifications => $"{baseUrlTenant}Tenant/Notifications/Archived";
    public string tenantNotificationsDetails => $"{baseUrlTenant}Tenant/Notifications/GetDetails";
    public string getContractPayments => $"{baseUrlTenant}Tenant/Contracts/GetContractPayments";
    public string getContractCharges => $"{baseUrlTenant}Tenant/Contracts/GetContractCharges";
    public string payments => $"{baseUrlTenant}Tenant/Payments";
    public string paymentsGet => $"{baseUrlTenant}Tenant/Payments/Get";
    public string getStats => $"{baseUrlTenant}Tenant/Dashboard/GetStats";
    public string dashboard => $"{baseUrlTenant}dashboard/getdata?TenantId=";

    public string getOnlinePayments => $"{baseUrlTenant}Tenant/GatewayPayment/ViewOnlinePaymant";
    public string getContractCheques => $"{baseUrlTenant}Tenant/Contracts/GetContractCheques";
    public string toBePaidIn30Days => $"{baseUrlTenant}Tenant/Dashboard/ToBePaidIn30Days";
    public string contractsExpiringIn30Days => $"{baseUrlTenant}Tenant/Contracts/ContractExpire30Days";
    public string getPropTypes => $"{baseUrlTenant}Tenant/Contracts/GetPropertyTypes";
    public string getContractStatus => $"{baseUrlTenant}Tenant/Contracts/GetContractStatus";
    public string getContractswithFilter => $"{baseUrlTenant}Tenant/Contracts/GetContractswithFilter";
    public string contractChargeReceipts => $"{baseUrlTenant}Tenant/Contracts/ContractChargeReceipts";
    public string paymentsDownloadReceipt => $"{baseUrlTenant}Tenant/Payments/DownloadReceipt";
    public string contractDownload => $"{baseUrlTenant}Tenant/Contracts/DownloadContract";
    public string contractDownloadNew => $"{baseUrlTenant}Tenant/Contracts/DownloadContractNew";
    public string downloadContractTerms => $"{baseUrlTenant}Tenant/Contracts/DownloadContractTerms";
    public string downloadContractTermsNew => $"{baseUrlTenant}Tenant/Contracts/DownloadContractTermsNew";
    public string downloadOfferLetter => $"{baseUrlTenant}Tenant/Contracts/DownloadOfferLetter";
    public string contractRenewalActions => $"{baseUrlTenant}Tenant/Contracts/ContractRenewalActions";
    public string contractNewActions => $"{baseUrlTenant}Tenant/Contracts/ContractNewActions";
    public string getOutstandingPayments => $"{baseUrlTenant}Tenant/Contracts/GetOutstandingPayments";
    public string getOutstandingPaymentsNew => $"{baseUrlTenant}Tenant/Contracts/GetOutstandingPaymentsNew";
    public string updateContractPaymentMethod => $"{baseUrlTenant}Tenant/Contracts/UpdateContractPaymentMethod";
    public string updateContractPaymentMethodNew => $"{baseUrlTenant}Tenant/Contracts/UpdateContractPaymentMethodNew";
    public string updateContractStage => $"{baseUrlTenant}Tenant/Contracts/UpdateContractStage";
    public string updateContractStageNew => $"{baseUrlTenant}Tenant/Contracts/UpdateContractStageNew";
    public string getApproveMunicipal => $"{baseUrlTenant}Tenant/Contracts/GetApproveMunicipal";
    public string getApproveMunicipalNew => $"{baseUrlTenant}Tenant/Contracts/GetApproveMunicipalNew";
    public string downloadSignedContract => $"{baseUrlTenant}Tenant/Contracts/DownloadSignedContract";
    public string canDownloadContract => $"{baseUrlTenant}Tenant/Contracts/CanDownloadContract";
    public string unVerfiedContractPayment => $"{baseUrlTenant}Tenant/GatewayPayment/UnVerfiedContractPayment";
    public string unVerfiedAllPayment => $"{baseUrlTenant}Tenant/GatewayPayment/UnVerfiedAllPayment?ContractId=";
  

    

}
