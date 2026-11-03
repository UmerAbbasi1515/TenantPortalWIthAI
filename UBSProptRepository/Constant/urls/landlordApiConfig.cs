using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;

public class UBSLandlordApiConfig
{
    public string? environment { get; set; }
    
    // Auth
    private readonly string baseUrlAuthDEV = "https://auth.api.ubspropt.com/api/";
    private readonly string baseUrlAuthUAT = "https://10.103.102.4/Auth/api/";
    private readonly string baseUrlAuthPROD = "http://40.80.86.229:9024/api/";
    private string baseUrlAuth => environment switch
    {
        "DEV" => baseUrlAuthDEV,
        "UAT" => baseUrlAuthUAT,
        "PROD" => baseUrlAuthPROD,
        _ => baseUrlAuthDEV
    };
    
    // Reports
    private readonly string baseUrlReportsDEV = "https://proptapi.ubspropt.com/api/";
    private readonly string baseUrlReportsUAT = "http://10.103.102.4/ASTECO_TRANSITION_ReportApi/api/";
    private readonly string baseUrlReportsPROD = "http://10.103.102.4:80/ASTECO_TRANSITION_ReportApi/api/";
    private string baseUrlReports => environment switch
    {
        "DEV" => baseUrlReportsDEV,
        "UAT" => baseUrlReportsUAT,
        "PROD" => baseUrlReportsPROD,
        _ => baseUrlReportsUAT
    };

    // Landlord
    private readonly string baseUrlLandlordDEV = "https://landlord.api.ubspropt.com/api/";
    private readonly string baseUrlLandlordUAT = "https://10.103.102.4/Lanldord/api/";
    private readonly string baseUrlLandlordPROD = "https://provisuat.uaenorth.cloudapp.azure.com/fabplandlord01/api/";
    private string baseUrlLandlord => environment switch
    {
        "DEV" => baseUrlLandlordDEV,
        "UAT" => baseUrlLandlordUAT,
        "PROD" => baseUrlLandlordPROD,
        _ => baseUrlLandlordDEV
    };

    

    // Auth by Email
    public string validateUserbyEmail => $"{baseUrlAuth}AuthEmail/ValidateUserbyEmail";
    public string verifyUserOtpbyEmail => $"{baseUrlAuth}AuthEmail/VerifyUserOtp";
    public string saveUserPasswordbyEmail => $"{baseUrlAuth}AuthEmail/SaveUserPassword";
    public string validatePasswordforEmail => $"{baseUrlAuth}AuthEmail/ValidatePasswordforEmail";
    public string? forgetPasswordByEmail => $"{baseUrlAuth}AuthEmail/ForgetPasswordByEmail";

    // Auth by Phone
    public string verifyUserOtpByPhone => $"{baseUrlAuth}Auth/GenerateFirbaseUserToken";
    public string getUserRoles => $"{baseUrlAuth}Account/GetUserRoles";
    public string proceedToLogin => $"{baseUrlAuth}ProceedToLogin";
    public string validateRoleByMpin => $"{baseUrlAuth}Account/ValidateRolebyMpin";
    public string validatePublicRole => $"{baseUrlAuth}Auth/ValidatePublicRole";
    public string compareDeviceToken => $"{baseUrlAuth}Account/CompareToken";
    public string getNewToken => $"{baseUrlAuth}Auth/RefreshToken";

    // Dropdown Filters
    public string emirateGetDropdown => $"{baseUrlReports}Common/Dropdown/EmirateGetDropdown?landLordId=";
    public string unitStatusGetDropdown => $"{baseUrlReports}Common/Dropdown/UnitStatusGetDropdown?landLordId=";
    public string propertyDropdown => $"{baseUrlReports}Common/Dropdown/PropertyDropdown?landLordId=";
    public string GetLandLordProperty => $"{baseUrlReports}Common/Dropdown/GetLandLordProperty/";
    
    public string companyDropdown => $"{baseUrlReports}Common/Dropdown/CompanyDropdown?landLordId=";
    public string unitTypeDropdown => $"{baseUrlReports}Common/Dropdown/UnitTypesGetDropdown?landLordId=";
    public string chequeStatusDropdown => $"{baseUrlReports}Common/Dropdown/GetChequeStatusDropdown?landLordId=";
    public string reasonOfPaymentsDropdown => $"{baseUrlReports}Common/Dropdown/ReasonOfPaymentsGetDropdown?landLordId=";
    public string tenantTypeDrp => $"{baseUrlReports}Common/Dropdown/TenantTypeDrp?landLordId=";
    
    public string contractStatusDrp => $"{baseUrlReports}Common/Dropdown/ContractStatusDrp?landLordId=";
    public string contractActionDropDown => $"{baseUrlReports}Common/Dropdown/ContractActionDrp";
    //public string contractActionDropDown => $"{baseUrlReports}Common/Dropdown/ContractActionDrp?landLordId=";
    public string getPaymentKindDropdown => $"{baseUrlReports}Common/Dropdown/GetPaymentKindDropdown?landLordId=";
    public string refundableReasonOfPaymentsGetDropdown => $"{baseUrlReports}Common/Dropdown/RefundableReasonOfPaymentsGetDropdown?landLordId=";

    // Reports
    public string GetBouncedChequesData => $"{baseUrlReports}Pms/BouncedChequesSummary/GetBouncedChequesData";
    public string GetBouncedChequesReport => $"{baseUrlReports}Pms/BouncedChequesSummary/GetBouncedChequesReport";
    public string GetLegalCaseData => $"{baseUrlReports}Pms/LegalCase/GetLegalCaseData";
    public string GetLegalCaseReport => $"{baseUrlReports}Pms/LegalCase/GetLegalCaseReport";
    public string GetArAgningData => $"{baseUrlReports}Pms/SapReport/GetSapArAgningData";
    public string GetArAgningReport => $"{baseUrlReports}Pms/SapReport/GetSapArAgeingReport";
    public string GetSapTrailBalanceData => $"{baseUrlReports}Pms/SapReport/GetSapTrialBalanceData";
    public string GetSapTrailBalanceReport => $"{baseUrlReports}Pms/SapReport/GetSapTrialBalanceReport";
    public string GetRegisterCheque => $"{baseUrlReports}Pms/ChequeRegister/GetRegisterCheque";
    public string GenerateChequeRegisterReport => $"{baseUrlReports}Pms/ChequeRegister/GenerateChequeRegisterReport";
    public string GetContractStatusData => $"{baseUrlReports}Pms/Contracts/GetContractStatusData";
    public string GenerateContractStatusReport => $"{baseUrlReports}Pms/Contracts/GenerateContractStatusReport";
    public string GetContractDueDate => $"{baseUrlReports}Pms/Contracts/GetContractDueDate";
    public string GetContractDueReport => $"{baseUrlReports}Pms/Contracts/GetContractDueReport";
    public string GetDepositedCheque => $"{baseUrlReports}Pms/DepositCheque/GetDepositedCheque";
    public string GetDepositedChequeReport => $"{baseUrlReports}Pms/DepositCheque/GetDepositedChequeReport";
    public string GetRefundableDeposits => $"{baseUrlReports}Pms/DepositReports/GetRefundableDeposits";
    public string GeneratePrintRefundableDepositsReport => $"{baseUrlReports}Pms/DepositReports/GeneratePrintRefundableDepositsReport";
    public string GenerateSecurityDepositsReport => $"{baseUrlReports}Pms/DepositReports/GenerateSecurityDepositsReport";
    
    public string GetReceiptRegisterData => $"{baseUrlReports}Pms/ReceiptRegister/GetReceiptRegisterData";
    public string GetReceiptRegisterReport => $"{baseUrlReports}Pms/ReceiptRegister/GetReceiptRegisterReport";
    public string GetMonthlyRentRecognitionData => $"{baseUrlReports}Pms/MonthlyRentRecognition/GetMonthlyRentRecognitionData";
    public string GetMonthlyRentRecognitionReport => $"{baseUrlReports}Pms/MonthlyRentRecognition/GetMonthlyRentRecognitionReport";
    public string GetManagementFeeData => $"{baseUrlReports}Pms/ManagmentFee/GetManagementFeeData";
    public string GetManagementFeeReport => $"{baseUrlReports}Pms/ManagmentFee/GetManagementFeeReport";
    public string GetOtherIncome => $"{baseUrlReports}Pms/OtherIncome/GetOtherIncome";
    public string GenerateOtherIncomeReport => $"{baseUrlReports}Pms/OtherIncome/GenerateOtherIncomeReport";
    public string GetPrintVATInvoice => $"{baseUrlReports}Pms/PrintVATInvoice/GetPrintVATInvoice";
    public string GeneratePrintVATInvoiceReport => $"{baseUrlReports}Pms/PrintVATInvoice/GeneratePrintVATInvoiceReport";
    public string GetBuildingStatusData => $"{baseUrlReports}Pms/Property/GetBuildingStatusData";
    public string GetBuildingStatusReport => $"{baseUrlReports}Pms/Property/GetBuildingStatusReport";
    public string GetVacancyRatioData => $"{baseUrlReports}Pms/Property/GetVacancyRatioData";
    public string GetVacancyRatioReport => $"{baseUrlReports}Pms/Property/GetVacancyRatioReport";
    public string GetRegisterReport => $"{baseUrlReports}Pms/Tenant/GetRegisterReport";
    public string GenerateRegisterReport => $"{baseUrlReports}Pms/Tenant/GenerateRegisterReport";
    public string getTenantbySearch => $"{baseUrlReports}Pms/Tenant/SearchTenant?";
    public string GetStatementofAccountData => $"{baseUrlReports}Pms/Tenant/GetStatementofAccountData";
    //public string GenerateStatementofAccountReport => $"{baseUrlReports}Pms/Tenant/GenerateTenancyScheduleReport";
    public string GenerateStatementofAccountReport => $"{baseUrlReports}Pms/Tenant/GenerateStatementofAccountReport";
    public string GenerateTenancyScheduleReport => $"{baseUrlReports}Pms/Tenant/GenerateTenancyScheduleReport";
    public string GetVacancyUnit => $"{baseUrlReports}Pms/Unit/GetVacancyUnit";
    public string GetUnitVacancyReport => $"{baseUrlReports}Pms/Unit/GetUnitVacancyReport";
    public string GetUnitStatusData => $"{baseUrlReports}Pms/Unit/GetUnitStatusData";
    public string GetUnitStatusReport => $"{baseUrlReports}Pms/Unit/GetUnitStatusReport";
    public string GetVacancyRentalosstData => $"{baseUrlReports}Pms/Unit/GetVacancyRentalosstData";
    public string GetVacancyRentalosstReport => $"{baseUrlReports}Pms/Unit/GetVacancyRentalosstReport";
    public string GetVatInvoiceData => $"{baseUrlReports}Pms/VAT/GetVatInvoiceData";
    public string GenerateVatInvoiceReport => $"{baseUrlReports}Pms/VAT/GenerateVatInvoiceReport";

    // Operational and Financial Dashboard
    public string getOperationalDashboardGraphs => $"{baseUrlReports}Pms/Dashboard/GetOperationalDashboard";
    public string getFinancialDashboardGraphs => $"{baseUrlReports}Pms/Dashboard/GetFinancialDashboardChartData";

    // Example Landlord API usage
    public string getLandlordNotifications => $"{baseUrlLandlord}Notification/Get";
    public string landlordReadNotifications => $"{baseUrlLandlord}Notification/Archived";
    public string landlordArchiveNotifications => $"{baseUrlLandlord}Notification/Read";
    public string landlordNotificationsDetails => $"{baseUrlLandlord}Notification/GetDetails";
    public string landlordDownloadNotificationFile => $"{baseUrlLandlord}Notification/DownloadNotificationFile";
    public string landlordGetNotificationFiles => $"{baseUrlLandlord}Notification/GetNotificationFiles";

    public string GetLandlordProfile => $"{baseUrlLandlord}Landlord/Profile/GetProfile";
    public string CanEditLandlordProfile => $"{baseUrlLandlord}Landlord/Profile/CanEditProfile";
    public string UpdateLandlordProfile => $"{baseUrlLandlord}Landlord/Profile/UpdateProfileRequest";
    public string GetLandlordFaqsCatg => $"{baseUrlLandlord}Faq/GetFaqCategories";
    public string GetLandlordFaqs => $"{baseUrlLandlord}Faq/GetFaqs";

    public string GetLandlordContracts => $"{baseUrlLandlord}Landlord/Contract/GetContracts";
    public string GetLandlordContractsWithFilter => $"{baseUrlLandlord}Landlord/Contract/GetContractsWithFilter";
    public string GetLandlordContractStatus => $"{baseUrlLandlord}Landlord/Contract/GetContractStatus";
    public string GetLandlordPropTypes => $"{baseUrlLandlord}Landlord/Property/GetPropertyTypes";
    public string GetLandlordEmirate => $"{baseUrlLandlord}Landlord/Property/GetPropertyEmirate";
    public string GetLandlordCategory => $"{baseUrlLandlord}Landlord/Property/GetPropertyCategory";
    public string GetPropertyWithFilter => $"{baseUrlLandlord}Landlord/Property/GetPropertyWithFilter";
    public string GetLandlordContractDetails => $"{baseUrlLandlord}Landlord/Contract/GetContractDetails";
    public string GetLandlordContractUnits => $"{baseUrlLandlord}Landlord/Contract/GetContractUnits";
    public string GetLandlordUnverifiedPayments => $"{baseUrlLandlord}Landlord/Contract/GetContractPayments";
    public string GetLandlordContractPayments => $"{baseUrlLandlord}Landlord/Contract/GetContractPayments";
    public string GetLandlordContractCharges => $"{baseUrlLandlord}Landlord/Contract/GetContractCharges";
    public string GetLandlordContractCheques => $"{baseUrlLandlord}Landlord/Contract/GetContractCheques";
    public string LandlordDashboardGetData => $"{baseUrlLandlord}Landlord/Property/GetData";
    public string GetLandlordProperties => $"{baseUrlLandlord}Landlord/Property/GetProperty";
    public string GetLandlordPropertyUnits => $"{baseUrlLandlord}Landlord/Property/GetPropertyUnits";
    public string GetLandlordUnitImages => $"{baseUrlLandlord}api/Public/Property/GetUnitImage";
    public string GetLandlordPropertyDetails => $"{baseUrlLandlord}Landlord/Property/GetPropertyDetails";
    public string GetLandlordPropertyUnitDetails => $"{baseUrlLandlord}Landlord/Property/GetPropertyUnitDetails";
    public string GetLandlordPayments => $"{baseUrlLandlord}Landlord/Payments/Get";
    public string GetLandlordContractChargeReceipts => $"{baseUrlLandlord}Landlord/Contract/ContractChargeReceipts";
    public string UnverifiedLandlordContractPayment => $"{baseUrlLandlord}Landlord/Contract/UnVerifiedContractPayment";
    public string GetLandlordUnits => $"{baseUrlLandlord}Landlord/Contract/GetUnits";
    public string PaymentsDownloadReceiptLandlord => $"{baseUrlLandlord}Landlord/Payments/DownloadReceipt";
    public string GetContractChargesLandlord => $"{baseUrlLandlord}Landlord/Contract/GetContractCharges";
    public string ContractChargeReceiptsLandlord => $"{baseUrlLandlord}Landlord/Contract/ContractChargeReceipts";
    public string GetLandlordReportDropDownType => $"{baseUrlLandlord}Landlord/Report/GetDropdownByType";
    public string DownloadAMCReport => $"{baseUrlLandlord}Landlord/Report/GenerateAMCReport";
    public string GetDropDownType => $"{baseUrlLandlord}Landlord/Report/GetDropdownByType";
    public string GenerateAMCReportSummary => $"{baseUrlLandlord}Landlord/Report/GenerateAMCReportSummary";
    public string DownloadGenerateLPOReport => $"{baseUrlLandlord}Landlord/Report/GenerateLPOReport";
    public string GenerateLPOReportSummary => $"{baseUrlLandlord}Landlord/Report/GenerateLPOReportSummary";
    public string DownloadGenerateBERReport => $"{baseUrlLandlord}Landlord/Report/GenerateBERReport";
    public string DownloadGenerateVATReport => $"{baseUrlLandlord}Landlord/Report/GenerateVATReport";
    public string GenerateVATReportSummary => $"{baseUrlLandlord}Landlord/Report/GenerateVATReportSummary";
    public string DownloadGenerateChequeRegisterReport => $"{baseUrlLandlord}Landlord/Report/GenerateChequeRegisterReport";
    public string GenerateChequeRegisterReportSummary => $"{baseUrlLandlord}Landlord/Report/GenerateChequeRegisterReportSummary";
    public string DownloadGenerateLegalCaseReport => $"{baseUrlLandlord}Landlord/Report/GenerateLegalCaseReport";
    public string GenerateLegalCaseReport => $"{baseUrlLandlord}Landlord/Report/GenerateLegalCaseReport";
    public string GenerateLegalCaseReportSummary => $"{baseUrlLandlord}Landlord/Report/GenerateLegalCaseReportSummary";
    public string GenerateContractReport => $"{baseUrlLandlord}Landlord/Report/GenerateLandlordGetContract";
    public string GenerateContractReportSummary => $"{baseUrlLandlord}Landlord/Report/GenerateLandlordGetContractSummary";
    public string DownloadGenerateReceiptRegisterReport => $"{baseUrlLandlord}Landlord/Report/GenerateReceiptRegisterReport";
    public string GenerateReceiptRegisterReportSummary => $"{baseUrlLandlord}Landlord/Report/GenerateReceiptRegisterReportSummary";
    public string DownloadGenerateUnitStatusReport => $"{baseUrlLandlord}Landlord/Report/GenerateUnitStatusReport";
    public string GenerateUnitStatusReportSummary => $"{baseUrlLandlord}Landlord/Report/GenerateUnitStatusReportSummary";
    public string DownloadGenerateLandlordGetContracts => $"{baseUrlLandlord}Landlord/Report/GenerateLandlordGetContracts";
    public string DownloadGenerateBuildingStatusReport => $"{baseUrlLandlord}Landlord/Report/GenerateBuildingStatusReport";
    public string GenerateBuildingStatusReportSummary => $"{baseUrlLandlord}Landlord/Report/GenerateBuildingStatusReportSummary";
    public string DownloadOccupancyVacancyRegisterReport => $"{baseUrlLandlord}Landlord/Report/OccupancyVacancyRegister";
    public string OccupancyVacancyRegisterSummary => $"{baseUrlLandlord}Landlord/Report/OccupancyVacancyRegisterSummary";
    public string LandlordInvoices => $"{baseUrlLandlord}Landlord/Property/GetAllInvoices";
    


}
