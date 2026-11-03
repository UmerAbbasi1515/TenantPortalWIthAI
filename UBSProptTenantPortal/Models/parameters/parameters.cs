using System.Security.Cryptography;
using UBSProptTenantPortal;

public class DashboardFunctionCalling
{
    public bool? getDataFunc { get; set; }
    public bool? getContractsFunc { get; set; }
    public bool? getServiceRequestFunc { get; set; }
    public bool getPaymentFunc { get; set; }
}
public class GenerateFirbaseUserTokenParameter
{
    public string? Mobile { get; set; }
    public string? OTP { get; set; }
    public string? otpCode { get; set; }
    public bool OTPStatus { get; set; }
}
public class RefreshTokenParameter
{
    public string? Mobile { get; set; }
}
public class ResetMpinVerification
{
    public string? mpin { get; set; }
}
public class ProceedToLoginParameter
{
    public string? UserRole { get; set; }
}
public class ValidateRoleByMPinParameter
{
    public string? MPIN { get; set; }
    public string? RoleId { get; set; }
}
//
public class GetPropertiesParameter
{
    public string? search { get; set; }
    public string? pageSize { get; set; }
    public string? pageNo { get; set; }
}
public class PropertyDetailparameter
{
    public string? propertyID { get; set; }
}
public class ContractDetailparameter
{
    public string? ContractId { get; set; }
    public string? callerFrom { get; set; }
}
public class ContractUploadSignatureparameter
{
    public string? base64Data { get; set; }
    public string? caseNo { get; set; }
}
public class PolicyRefundparameter
{
    public string? Key { get; set; }
}
public class ContractDetailChequeparameter
{
    public string? TransactionID { get; set; }
}

public class PropertyUnitDetailparameter
{
    public string? unitID { get; set; }
}
public class GetPaymentParameter { 
    public string? Page { get; set; }
    public string? PageSize { get; set; }
    public string? SearchParam { get; set; }
}
public class GetContractsParameter
{
    public string? page { get; set; }
    public string? pageSize { get; set; }
    public string? search { get; set; }
}

public class GetContractsFilterParameter
{
    public string? propertyName { get; set; }
    public string? propertyTypeId { get; set; }
    public string? contractStatusId { get; set; }
    public string? fromDate { get; set; }
    public string? toDate { get; set; }
}

public class GetContractsFilterParameterNew
{
    public string? PropertyName { get; set; }
    public string? PropertyTypeId { get; set; }
    public string? PropertyStausId { get; set; }
    public string? ContractDateFrom { get; set; }
    public string? ContractDateTo { get; set; }
}


//public class ServiceReqDetailparameterCaseNo
//{
//    public string? CaseNo { get; set; }
//}

public class ServiceReqDetailSurveyparameter
{
    public string? QuestionId { get; set; }
}

public class ServiceReqDetailSaveSurveyparameter
{
    public string? answerId { get; set; }
    public string? customerChoice { get; set; }
    public string? description { get; set; }
    public string? IsCompleted { get; set; }
}
public class ServiceReqDetailCaseIdparameter
{
    public string? CaseId { get; set; }
}
public class DownloadChequeParameter
{
    public string? PaymentSettingId { get; set; }
}

public class ServiceReqDetailPhotosparameter
{
    public string? CaseNo { get; set; }
    public string? RoleId { get; set; }
}
public class ServiceReqDetailDocsparameter
{
    public string? CaseNo { get; set; }
    public string? RoleId { get; set; }
    public string? Code { get; set; }
}
public class ServiceReqDetailDownloadDocsparameter
{
    public string? CaseNo { get; set; }
    public string? RoleId { get; set; }
    public string? PhotoId { get; set; }
}
public class ServiceReqDetailRemoveDocsparameter
{
    public string? PhotoId { get; set; }
}
public class ServiceReqDetailTicketDocDownloadparameter
{
    public string? TicketReplyId { get; set; }
}
public class ServiceReqDetailDocsUploadparameter
{
    public string? CaseNo { get; set; }
    public string? AttachmentType { get; set; }
    public string? ExpireDate { get; set; }
    public string? DocumentTypeId { get; set; }
}
//public class GetServiceRequestParameterN
//{
//    public string? type { get; set; }
//    public string? fromDate { get; set; }
//    public string? toDate { get; set; }
//    public string? search { get; set; }
//}

public class ServiceRequestDeletePhotoParameter
{
    public string? PhotoId { get; set; }
}
public class GetPaymentChequeParameter
{
    public string? TransactionID { get; set; }
}
public class Base64FileModel
{
    public string? path { get; set; }
}
public class GetPaymentChequeParameterLoweCase
{
    public string? transactionid { get; set; }
}
public class GetInvoicesParameter
{
    public string? search { get; set; }
    public string? pageNo { get; set; }
    public string? pageSize { get; set; }
}
public class GetFaqsDetailparameter
{
    public string? CategoryId { get; set; }
}
public class GetOfferDetailparameter
{
    public string? OfferId { get; set; }
}
public class DownloadOfferDetailparameter
{
    public string? OfferId { get; set; }
    public string? DocumentId { get; set; }
}

public class ReportsFilterparameter
{
    public string? dropdownTypeID { get; set; }
}
public class ReportsFilterparameterWithName
{
    public string? type { get; set; }
    public string? dropdownTypeID { get; set; }
    public string? name { get; set; }
}

public class Test
{
    public string Name { get; set; }
    public string NameAr { get; set; }
    public string Value { get; set; }

    public Test(string name, string nameAr, string value)
    {
        Name = name;
        NameAr = nameAr;
        Value = value;
    }
}

// Validator
public class EmailValidatorModel
{
    public string? Email { get; set; }
}
public class ValidateEmail
{
    public string? email { get; set; }
}
public class ValidatePasswordByEmail
{
    public string? Password { get; set; }
    public string? Email { get; set; }
}
public class VerifyOTPByEmail
{
    public string? OTP { get; set; }
    public string? Email { get; set; }
}
public class SavePasswordByEmail
{
    public string? Password { get; set; }
    public string? Email { get; set; }
}




///


public class PropertySummary
{
    public int PropertyID { get; set; }
    public string? Property { get; set; }
    public string? Landlord { get; set; }
    public string? PropRefNo { get; set; }
    public string? PropNo { get; set; }
}
public class ReportFilterCommonDropDown
{
    public int? landLordId { get; set; }
}

// operational dashboard
public class OperationlFinancialDashboardParameterModel
{
    public int unitTypeId { get; set; } = 0;
    public string datePeriod { get; set; } = "";
    public int landlordId { get; set; } = AuthLabelsPortal.landlordID;
    public int propertyId { get; set; } = 0;
    public int tenantId { get; set; } = 0;
    public string action { get; set; } = "";
}