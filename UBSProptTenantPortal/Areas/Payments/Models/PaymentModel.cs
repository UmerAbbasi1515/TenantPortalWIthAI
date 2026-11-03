using System;


// payment
public class PaymentResponse
{
    public string? Status { get; set; }
    public string? StatusCode { get; set; }
    public List<PaymentDetail>? Payments { get; set; }
    public string? Message { get; set; }
}

public class PaymentDetail
{
    public dynamic? TransactionId { get; set; }
    public dynamic? ContractId { get; set; }
    public string? PaymentDate { get; set; }
    public string? PropertyName { get; set; }
    public string? PropertyNameAr { get; set; }
    public string? PropertyNumber { get; set; }
    public string? ReceiptNo { get; set; }
    public string? AccountNo { get; set; }
    public string? AccountTitle { get; set; }
    public dynamic? Amount { get; set; }
    public string? PaymentType { get; set; }
    public string? PaymentTypeAr { get; set; }
    public string? ContractDate { get; set; }
    public string? Tenant { get; set; }
    public string? TenantAr { get; set; }
    public string? UnitNo { get; set; }
    public string? ContractNo { get; set; }
    public string? City { get; set; }
    public string? CityAr { get; set; }
    public string? PaymentFor { get; set; }
    public string? PaymentForAr { get; set; }
    public string? LandLord { get; set; }
    public string? ContractStatus { get; set; }
    public string? ReceiptDate { get; set; }
    public PaymentDetailCheque? Cheque { get; set; }
}

// Cheques
public class PaymentDetailCheque
{
    public string? Status { get; set; }
    public string? StatusCode { get; set; }
    public List<PTransactionCheque>? TransactionCheque { get; set; }
    public string? Message { get; set; }
}

public class PTransactionCheque
{
    public string? TransactionId { get; set; }
    public string? ChequeNo { get; set; }
    public string? ChequeDate { get; set; }
    public string? BankName { get; set; }
    public decimal? ChequeAmount { get; set; }
    public string? ChequeStatus { get; set; }
    public string? BankNameAr { get; set; }
    public string? ChequeStatusAR { get; set; }
}

public class PaymentChequeDownload
{
    public string? path { get; set; }
    public string? status { get; set; }
}

// ServiceRequestViewModel
public class PaymentsViewModel
{
    public PaymentDetailUpdated? paymentDetail { get; set; }
    public PaymentDetailCheque? Cheques { get; set; }
}

public class PaymentDetailUpdated
{
    public dynamic? TransactionId { get; set; }
    public string? PaymentDate { get; set; }
    public string? ReceiptNo { get; set; }
    public dynamic? Amount { get; set; }
    public string? PaymentType { get; set; }
    public string? PaymentFor { get; set; }
    public string? ReceiptDate { get; set; }
}


