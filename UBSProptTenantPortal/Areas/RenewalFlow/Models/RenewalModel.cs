using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Text.Json.Serialization;
using UBSProptSharedModel.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;


public class ContractRenwalViewModel
{
    public ContractRenwalResponse? fmRequest { get; set; }
    public ContractRenwalResponse? pmRequest { get; set; }
    public ContractRenwalTabs? tabs { get; set; }
}
public class ContractRenwalTabs
{
    public List<ContractWithDueAction>? ServiceRequests { get; set; }
    public string? tabName { get; set; }
}

public class ContractRenwalViewModelDemo
{
    public ContractWithDueActionResponse? renwalContract { get; set; }
    public ContractWithDueActionResponse? newContract { get; set; }
    public ContractRenwalTabs? tabs { get; set; }
}

public class ContractRenwalResponse
{
    public ContractWithDueActionResponse? ContractDueAction { get; set; }
}
public class StageOneApiResponse
{
    public string? status { get; set; }
    public AddServiceRequest? addServiceRequest { get; set; }
    public string? message { get; set; }
}

public class AddServiceRequest
{
    public int caseNo { get; set; }
}


public class MunicipalInstructions
{
    public string? Record { get; set; }
    public string? Status { get; set; }
    public string? StatusCode { get; set; }
    public string? Message { get; set; }
}

public class ContractWithDueActionResponse
    {
        public string? Status { get; set; }
        public List<ContractWithDueAction>? Contracts { get; set; }
        public string? StatusCode { get; set; }
    }
    public class ContractWithDueAction
    {
    public string? tabName { get; set; }
    public int? DueActionId { get; set; }
        public int? ContractId { get; set; }
        public int? CaseId { get; set; }
        public int? ExpiringingDays { get; set; }
        public int? RenewalContractId { get; set; }
        public int? StageId { get; set; }
        public string? StageName { get; set; }
        public string? PreviousContractNo { get; set; }
        public string? PropertyName { get; set; }
        public string? PropertyNameAr { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string? ContractNo { get; set; }
        public int? IsCanceled { get; set; }
        public int? IsClosed { get; set; }
        public string? Status { get; set; }
        public string? StatusAr { get; set; }
        public bool? ShowExtend { get; set; }
        public int? IsAllPaid { get; set; }
        public string? EmirateName { get; set; }
        public int? ShowOfferLetter { get; set; }
    }

public class OfferLetterDownload
{
    public string? path { get; set; }
    public string? status { get; set; }
}

public class ContractRenewParam
{
    public string? ContractId { get; set; }
    public string? DueActionID { get; set; }
    public string? ContractNo { get; set; }
}

public class ContractRenewParamWithCaseNO
{
    public string? ContractId { get; set; }
    public string? DueActionID { get; set; }
    public string? ContractNo { get; set; }
    public string? CaseNo { get; set; }
    public string? StageID { get; set; }// Changed here
    public string? callingFrom { get; set; }// Changed here
}
public class UpdateArmaxAdressParam
{
    public string? ContractId { get; set; }
    public string? Address { get; set; }
    public string? SelfDelivery { get; set; }
}
public class RegisterPaymentParam
{
    public string? totalAmount { get; set; }
    public string? contractId { get; set; }
    public string? contractNo { get; set; }
    public string? userId { get; set; }
    public List<DetailList>? detailList { get; set; }
}
public class DetailList
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public double? Amount { get; set; }
    public int ChargeId { get; set; } = 0;
    public int PaymentId { get; set; } = 0;
    public int VatOnRentContractId { get; set; } = 0;
    public int VatOnChargeId { get; set; } = 0;
    public int PaymentSettingId { get; set; } = 0;
}

public class GotoOnlinePaymentsParam
{
    public string? ContractId { get; set; }
}

public class MakePaymentChequeUpload
{
    public string? PaymentModeId { get; set; }
    public string? PaymentSettingId { get; set; }
    public string? ChequeNo { get; set; }
    public string? Address { get; set; }
}
public class ContractSubmitRenewExtendParam
{
    public string? contractId { get; set; }
    public string? dueActionid { get; set; }
    public string? startDate { get; set; }
    public string? endDate { get; set; }
    public string? duration { get; set; }
}
public class ContractSubmitVacateParam
{
    public string? VacatingId { get; set; }
    public string? ContractId { get; set; }
    public string? DueActionId { get; set; }
    public string? Description { get; set; }
    public string? Duration { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public string? VacatingDate { get; set; }
}


#region renew
public class ContractRenewInfo
{
    public string? Status { get; set; }
    public Record? Record { get; set; }
    public string? Link { get; set; }
}

public class Record
{
    public int? ContractId { get; set; }
    public int? Installments { get; set; }
    public int? EmirateId { get; set; }
    public double? TotalAmount { get; set; }
    public string? Amount { get; set; }
    public string? ContractNo { get; set; }
    public string? FromDate { get; set; }
    public string? EndDate { get; set; }
    public string? AddNewDate { get; set; }
    public string? EndNewDate { get; set; }
    public string? EmirateName { get; set; }
}

#endregion

#region Extend
public class GetExtensionPeriodModel
{
    public string? Status { get; set; }
    public List<ExtensionPeriod>? ExtensionPeriod { get; set; }
}

public class ExtensionPeriod
{
    public string? Duration { get; set; }
    public string? Amount { get; set; }
    public ExtensionDetail? ExtensionDetail { get; set; }
}

public class ExtensionDetail
{
    public int? ContractId { get; set; }
    public string? Contractno { get; set; }
    public string? FromDate { get; set; }
    public string? EndDate { get; set; }
    public string? AddNewDate { get; set; }
    public string? EndNewDate { get; set; }
    public string? EmirateName { get; set; }
}

#endregion

#region Termination
public class GetVacatingReasons
{
    public string? Status { get; set; }
    public string? SelectedReason { get; set; }
    public List<RecordTermin>? Record { get; set; }
    public string? Message { get; set; }
    public string? Note { get; set; }
}

public class RecordTermin
{
    public int? VacatingId { get; set; }
    public string? Title { get; set; }
    public string? TitleAr { get; set; }
    public string? Description { get; set; }
    public string? DescriptionAr { get; set; }
}

#endregion

#region Make Payment
public class RegisterPaymentResponse
{
    public int? OrderId { get; set; }
    public string? TransactionId { get; set; }
    public string? Url { get; set; }
    public string? Status { get; set; }
}
public class TermsAndConditiosResponseModel
{
    public string? Status { get; set; }
    public TermsAndConditiosResponse? Data { get; set; }
    public string? Message { get; set; }
}
public class TermsAndConditiosResponse
{
    public int SettingId { get; set; }
    public string Name { get; set; } = "";
    public string Value { get; set; } = "";
    public string ValueAr { get; set; } = "";
    public string Title { get; set; } = "";
    public string TitleAr { get; set; } = "";
    public string Type { get; set; } = "";
}

public class OutstandingPaymentsModelResponse
{
    public OutstandingPaymentsModel? MakePayment { get; set; }
}

public class ShowDeveliveryAddress {
    public bool? ShowDeliveryOptions { get; set; }
    public int? ChequeDeliveryOption { get; set; }
}

public class PaymentToShowChequeToAddress
{
    public List<string>? ChequesToShowAddressList { get; set; }
    public List<dynamic>? PaymentsToShow { get; set; }
    public bool? ShowDeliveryOptionsTestForButtonResult { get; set; }
}

public class OutstandingPaymentsModel
{
    public string? Status { get; set; }
    public List<RecordOutStanding>? Record { get; set; }
    public PaymentSummaryModel? PaymentSummary { get; set; }
    public List<dynamic>? PaymentsToShow { get; set; }
    public bool? ShowDeliveryOptions { get; set; }
    public int? chequeDeliveryOption { get; set; }
    public bool? ShowDeliveryOptionsTestForButton { get; set; }
    public bool? ShowDeliveryOptionsTest { get; set; }
    public int? ChequeDeliveryOption { get; set; }
    public bool? GotoOnlinePayments { get; set; }

    public List<string>? ChequesToShowAddress { get; set; }
}



public class RecordOutStanding
{
    public string? Type { get; set; }
    public string? Title { get; set; }
    public string? TitleAr { get; set; }
    public string? PaymentMode { get; set; }
    public string? Cheque { get; set; }
    public double? Amount { get; set; }
    public string? AmountFormatted { get; set; }
    public string? Description { get; set; }
    public string? PaymentDate { get; set; }

    public int? PaymentSettingId { get; set; }
    public int? PaymentMethodId { get; set; }
    public int? ContractId { get; set; }
    public int? ContractChargeId { get; set; }
    public int? ContractPaymentId { get; set; }
    public int? VatOnRentContractId { get; set; }
    public int? VatOnChargeId { get; set; }

    public string? SelfDelivery { get; set; }
    public int? Confirmed { get; set; }
    public string? ChequeNo { get; set; } = "";

    public bool? IsRejected { get; set; }
    public int? AcceptPaymentType { get; set; }
    public int? DefaultPaymentMethodType { get; set; }

    public string? AramexAddress { get; set; }
    public string? FilePath { get; set; }

    public byte[]? ChequeFile { get; set; }

    // UI-only properties (if needed in frontend logic; ignored in API/DB)
    [JsonIgnore] public bool IsChecked { get; set; } = false;
    [JsonIgnore] public bool UpdatingPaymentMethod { get; set; } = false;
    [JsonIgnore] public bool ErrorUpdatingPaymentMethod { get; set; } = false;
    [JsonIgnore] public bool UploadingCheque { get; set; } = false;
    [JsonIgnore] public bool ErrorUploadingCheque { get; set; } = false;
    [JsonIgnore] public bool DownloadingCheque { get; set; } = false;
    [JsonIgnore] public bool ErrorDownloadingCheque { get; set; } = false;
    [JsonIgnore] public bool ForceUploadCheque { get; set; } = false;
    [JsonIgnore] public bool ErrorRemovingCheque { get; set; } = false;
    [JsonIgnore] public bool RemovingCheque { get; set; } = false;
    [JsonIgnore] public bool ErrorChequeNo { get; set; } = false;
}

#endregion