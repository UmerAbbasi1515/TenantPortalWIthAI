



// New
using System.Text.Json.Serialization;
using UBSProptSharedModel.Model;

public class ContractResponse
{
    public string? Status { get; set; }
    public List<Contract>? data { get; set; }
    public string? Message { get; set; }
    public ContractsFilter? contractFilter { get; set; }
}

public class Contract
{
    public int ContractID { get; set; }
    public int ContractTypeID { get; set; }
    public int ContractPosition { get; set; }
    public string? ContractPositionName { get; set; }
    public string? PreviousContactNo { get; set; }
    public int ContractStatusID { get; set; }
    public int PropertyTypeID { get; set; }
    public int PropertyStatusID { get; set; }
    public string? Contractno { get; set; }
    public string? Address { get; set; }
    public string? AddressAR { get; set; }
    public string? EmirateName { get; set; }
    public string? ContractDate { get; set; }
    public string? ContractStartDate { get; set; }
    public string? ContractStatusAR { get; set; }
    public string? ContractEndDate { get; set; }
    public int Rentforstay { get; set; }
    public int NoOfDays { get; set; }
    public int GracePeriod { get; set; }
    public int NoOfContractYears { get; set; }
    public int Installments { get; set; }
    public string? Retention { get; set; }
    public int OtherCharges { get; set; }
    public int VatCharges { get; set; }
    public int VatAmount { get; set; }
    public string? PropertyName { get; set; }
    public string? PropertyNameAR { get; set; }
    public string? PropertyImage { get; set; }
    public string? UnitType { get; set; }
    public string? UnitTypeAR { get; set; }
    public string? UnitNo { get; set; }
    public string? UnitRefNo { get; set; }
    public int Total { get; set; }
    public int Paid { get; set; }
    public string? ContractStatus { get; set; }
    public int IsExpire { get; set; }
    public int IsreadyToExpire { get; set; }
}

// Contract Detail > Main Info

    public class ContractDetailsResponse
{
        public string? Status { get; set; }
        public ContractDetails? Contract { get; set; }
        public string? FormattedAmount { get; set; }
        public string? Message { get; set; }
        public int? DaysPassed { get; set; }
        public double? DaysPassedPercentage { get; set; }
        public TenantContractPayableModel? ContratPayable { get; set;
    }
}

    public class ContractDetails
    {
        public int ContractPosition { get; set; }
        public string? ContractPositionName { get; set; }
        public int PropertyTypeID { get; set; }
        public int PropertyStatusID { get; set; }
        public int IsExpire { get; set; }
        public int IsReadyToExpire { get; set; }
        public int ContractID { get; set; }
        public string? ContractNo { get; set; }
        public string? Address { get; set; }
        public string? AddressAR { get; set; }
        public string? ContractDate { get; set; } // You might want to use DateTime instead
        public string? ContractStartDate { get; set; } // You might want to use DateTime instead
        public string? ContractEndDate { get; set; } // You might want to use DateTime instead
        public decimal? RentForStay { get; set; }
        public int NoOfDays { get; set; }
        public int GracePeriod { get; set; }
        public int NoOfContractYears { get; set; }
        public int Installments { get; set; }
        public string? Retention { get; set; }
        public decimal OtherCharges { get; set; }
        public decimal VatCharges { get; set; }
        public decimal VatAmount { get; set; }
        public string? UnitName { get; set; }
        public string? PropertyName { get; set; }
        public string? UnitNameAR { get; set; }
        public string? UnitPicture { get; set; }
        public string? UnitType { get; set; }
        public string? UnitTypeAR { get; set; }
        public string? ContractStatus { get; set; }
        public int ContractTypeID { get; set; }
        public string? ContractStatusAR { get; set; }
        public string? UnitNo { get; set; }
        public PaymentInfo? Payment { get; set; }
    }

    public class PaymentInfo
    {
        public decimal Total { get; set; }
        public decimal Paid { get; set; }
    }

// Unit Info

    public class ContractDetailsUnitsResponse
    {
        public string? Status { get; set; }
        public string? StatusCode { get; set; }
        public List<ContractUnit>? ContractUnits { get; set; }
        public string? FormatedAmount { get; set; }
        public string? Message { get; set; }
    }

    public class ContractUnit
    {
        public string? PropertyImage { get; set; }
        public string? PropertyName { get; set; }
        public string? PropertyNameAR { get; set; }
        public decimal Amount { get; set; }
        public string? UnitRefNo { get; set; }
        public int UnitID { get; set; }
        public string? PropertyType { get; set; }
        public string? PropertyTypeAR { get; set; }
        public string? UnitType { get; set; }
        public string? UnitTypeAR { get; set; }
    }

    // Payment Info

    public class ContractDetailsPaymentResponse
    {
        public string? Status { get; set; }
        public string? StatusCode { get; set; }
        public List<Payment>? Payments { get; set; }
        public string? Message { get; set; }
    }

    public class Payment
    {
        public int TransactionId { get; set; }
        public int ContractID { get; set; }
        public string? PaymentDate { get; set; } // Consider using DateTime
        public string? PropertyName { get; set; }
        public string? PropertyNameAR { get; set; }
        public string? PropertyNumber { get; set; }
        public string? ReceiptNo { get; set; }
        public string? AccountNo { get; set; }
        public string? AccountTitle { get; set; }
        public double Amount { get; set; } // Use decimal for currency
        public string? FormatedAmount { get; set; }
        public string? PaymentType { get; set; }
        public string? PaymentTypeAR { get; set; }
        public string? ContractDate { get; set; } // Consider using DateTime
        public string? Tenant { get; set; }
        public string? TenantAR { get; set; }
        public string? UnitNo { get; set; }
        public string? ContractNo { get; set; }
        public string? City { get; set; }
        public string? CityAR { get; set; }
        public string? PaymentFor { get; set; }
        public string? PaymentForAR { get; set; }
        public string? LandLord { get; set; }
    }

    // Cheques
 
    public class PaymentChequeResponse
    {
        public string? Status { get; set; }
        public string? StatusCode { get; set; }
        public List<TransactionCheque>? TransactionCheque { get; set; }
        public string? Message { get; set; }
    }

    public class TransactionCheque
    {
        public int TransactionID { get; set; }
        public string? ChequeNo { get; set; }
        public string? ChequeDate { get; set; } // Consider using DateTime for date handling
        public string? BankName { get; set; }
        public decimal ChequeAmount { get; set; } // Use decimal for currency
        public string? ChequeStatus { get; set; }
        public string? BankNameAR { get; set; } // Consider making nullable if it can be empty
    }





    // Charges Info
    public class ContractDetailChargesResponse
    {
        public int ContractId { get; set; }
        public string? Status { get; set; }
        public string? StatusCode { get; set; }
        public List<ContractCharge>? ContractCharges { get; set; }
        public double TotalCharges { get; set; }
        public string? FormatedTotalCharges { get; set; }
        public double PaidCharges { get; set; }
        public string? FormatedPaidCharges { get; set; }
        public double OutstandingCharges { get; set; }
        public string? FormatedOutstandingCharges { get; set; }
        public string? Message { get; set; }
    }

    public class ContractCharge
    {
        public int ChargeNo { get; set; }
        public int ChargesTypeID { get; set; }
        public string? ChargeName { get; set; } // Consider making nullable if it can be empty
        public int TenancyCodeNumber { get; set; }
        public string? FormatedAmount { get; set; }
        public double Amount { get; set; }
        public int ContractID { get; set; }
        public string? FormatedVatAmount { get; set; }
        public double VatAmount { get; set; }
        public string? FormattedTotalAmount { get; set; }
        public double TotalAmount { get; set; }
        public string? CreatedOn { get; set; } // Consider using DateTime
        public string? ChargesType { get; set; }
        public string? ChargesTypeAR { get; set; } // Consider making nullable if it can be null
    }

public class TenantContractPayableModel
{
    public string? StatusCode { get; set; }
    public string? Status { get; set; }
    public string? Message { get; set; }
    public List<AdditionalCharge>? ContractPayable { get; set; }
    public List<AdditionalCharge>? AdditionalCharges { get; set; }
    public List<AdditionalCharge>? VatCharges { get; set; }
    public List<AdditionalCharge>? VatOnRent { get; set; }
    public PaymentSummaryModel? PaymentSummary { get; set; }
}
public class PaymentSummaryModel
{
    public string SumOfAllPayments { get; set; } = "0.00";
    public string TotalRentalPayment { get; set; } = "0.00";
    public string TotalAdditionalCharges { get; set; } = "0.00";
    public string TotalVatOnRent { get; set; } = "0.00";
    public string TotalVatOnCharges { get; set; } = "0.00";
}
public class GoToOnlinePaymentsModel
{
    public string? Status { get; set; }
    public List<RecordOutStanding>? Record { get; set; }
    public OnlinePaymentSumModel? OnlinePaymentSummary { get; set; }

    public List<dynamic>? PaymentsToShow { get; set; }
    public List<string>? ChequesToShowAddress { get; set; }


    public List<int> CardPaymentListLength { get; set; } = new List<int>();
    public List<int> BankTransferListLength { get; set; } = new List<int>();
    public bool IsRadioButtonShow { get; set; }
}
public class OnlinePaymentSumModel
{
    public List<string> SumOfAllPayments { get; set; } = new List<string>();
    public string TotalRentalPayment { get; set; } = "0.00";
    public string TotalAdditionalCharges { get; set; } = "0.00";
    public string TotalVatOnRent { get; set; } = "0.00";
    public string TotalVatOnCharges { get; set; } = "0.00";
}

public class AdditionalCharge
{
    public int? ContractId { get; set; }
    public string? ContractNo { get; set; }
    public double? Payment { get; set; }
    public object? PaidAmount { get; set; } // you can replace with double? if it's always numeric
    public string? PaymentNo { get; set; }
    public string? Type { get; set; }
    public int? Installments { get; set; }
    public int? ContractChargeId { get; set; }
    public int? PaymentId { get; set; }
    public double? Balance { get; set; }
    public string? BalanceFormatted { get; set; }
    public string? PaymentDate { get; set; }

    // UI-related properties (optional or handled separately)
    [JsonIgnore] public bool IsChecked { get; set; } = false;
    [JsonIgnore] public int PaymentMethod { get; set; } = 1;
    [JsonIgnore] public bool ChequeSelected { get; set; } = false;
    [JsonIgnore] public byte[]? ChequeFile { get; set; }
    [JsonIgnore] public string? FilePath { get; set; }
}




public class ContractDetailViewModel
{
    public ContractDetailsResponse? ContractDetail { get; set; }
    public ContractDetailsUnitsResponse? ContractUnits { get; set; }
    public ContractDetailsPaymentResponse? ContractPayments { get; set; }
    public ContractDetailChargesResponse? ContractCharges { get; set; }

}

