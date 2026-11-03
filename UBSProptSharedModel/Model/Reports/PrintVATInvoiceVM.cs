using System.Text.Json.Serialization;


    public class PrintVATInvoiceVM
    {
        [JsonPropertyName("logoImage")]
        public string? LogoImage { get; set; }
        [JsonPropertyName("data")]
        public List<PrintVATInvoiceViewModel>? Data { get; set; }
        public PrintHeaderViewModel? PrintHeaderViewModel { get; set; }
    }
    public class PrintVATInvoiceFilter
    {
        public string? ReportType { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int? LandlordID { get; set; } = LabelsLandlordPortal.landlordID;
        public int? ReceiverTypeID { get; set; }
        public bool IsShowMFee { get; set; }
        public bool IsShowPayment { get; set; }
        public bool IsShowCreditNote { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string? InvoiceNo { get; set; }
        public string? ContractNo { get; set; }
        public string? callingPortalName { get; set; } = LabelsLandlordPortal.portalName;
        public int VATInvoiceID { get; set; }
        public int LoginMemberId { get; set; } = LabelsLandlordPortal.UserID;
}
    public class PrintVATInvoiceViewModel
    {
        [JsonPropertyName("vatInvoiceID")]
        public string? VATInvoiceID { get; set; }

        [JsonPropertyName("landlordName")]
        public string? LandlordName { get; set; }

        [JsonPropertyName("isFGP")]
        public bool IsFGP { get; set; }

        [JsonPropertyName("propertyName")]
        public string? PropertyName { get; set; }

        [JsonPropertyName("emirateName")]
        public string? EmirateName { get; set; }

        [JsonPropertyName("contractNo")]
        public string? ContractNo { get; set; }

        [JsonPropertyName("fromDate")]
        public string? FromDate { get; set; }

        [JsonPropertyName("toDate")]
        public string? ToDate { get; set; }

        [JsonPropertyName("contractUnits")]
        public string? ContractUnits { get; set; }

        [JsonPropertyName("receiverName")]
        public string? ReceiverName { get; set; }

        [JsonPropertyName("receiverAddress")]
        public string? ReceiverAddress { get; set; }

        [JsonPropertyName("receiverVATRegisteredName")]
        public string? ReceiverVATRegisteredName { get; set; }

        [JsonPropertyName("receiverVATRegNo")]
        public string? ReceiverVATRegNo { get; set; }

        [JsonPropertyName("receiverType")]
        public string? ReceiverType { get; set; }

        [JsonPropertyName("contractID")]
        public string? ContractID { get; set; }

        [JsonPropertyName("transactionNo")]
        public string? TransactionNo { get; set; }

        [JsonPropertyName("ownerID")]
        public string? OwnerID { get; set; }

        [JsonPropertyName("transactionID")]
        public string? TransactionID { get; set; }

        [JsonPropertyName("payerEmirateName")]
        public string? PayerEmirateName { get; set; }

        [JsonPropertyName("countryName")]
        public string? CountryName { get; set; }

        [JsonPropertyName("payerID")]
        public string? PayerID { get; set; }

        [JsonPropertyName("payerName")]
        public string? PayerName { get; set; }

        [JsonPropertyName("payerType")]
        public string? PayerType { get; set; }

        [JsonPropertyName("payerVATRegNo")]
        public string? PayerVATRegNo { get; set; }

        [JsonPropertyName("propertyID")]
        public string? PropertyID { get; set; }

        [JsonPropertyName("vatType")]
        public string? VATType { get; set; }

        [JsonPropertyName("invoiceNo")]
        public string? InvoiceNo { get; set; }

        [JsonPropertyName("invoiceFromDate")]
        public string? InvoiceFromDate { get; set; }

        [JsonPropertyName("invoiceToDate")]
        public string? InvoiceToDate { get; set; }

        [JsonPropertyName("invoiceDate")]
        public string? InvoiceDate { get; set; }

        [JsonPropertyName("totalAmount")]
        public decimal TotalAmount { get; set; }

        [JsonPropertyName("vatAmount")]
        public decimal VATAmount { get; set; }

        [JsonPropertyName("taxableValue")]
        public decimal TaxableValue { get; set; }

        [JsonPropertyName("chargeAmount")]
        public decimal ChargeAmount { get; set; }

        [JsonPropertyName("netAmount")]
        public decimal NetAmount { get; set; }

        [JsonPropertyName("reasonOfPayment")]
        public string? ReasonOfPayment { get; set; }

        [JsonPropertyName("vatReasonOfPaymentID")]
        public string? VATReasonOfPaymentID { get; set; }
        [JsonPropertyName("amountInWords")]
        public string? AmountInWords { get; set; }
        [JsonPropertyName("vatAmountInWords")]
        public string? VATAmountInWords { get; set; }

        [JsonPropertyName("companyName")]
        public string? CompanyName { get; set; }
        [JsonPropertyName("address1")]
        public string? Address1 { get; set; }
        [JsonPropertyName("phoneNo")]
        public string? PhoneNo { get; set; }
        [JsonPropertyName("emailID")]
        public string? EmailID { get; set; }
        [JsonPropertyName("webSite")]
        public string? WebSite { get; set; }
        [JsonPropertyName("isShowMFee")]
        public bool IsShowMFee { get; set; }
        [JsonPropertyName("isShowPayment")]
        public bool IsShowPayment { get; set; }
        [JsonPropertyName("isShowCreditNote")]
        public bool IsShowCreditNote { get; set; }

        [JsonPropertyName("receiverTenantAddess")]
        public string? ReceiverTenantAddess { get; set; }
        [JsonPropertyName("payerTenantAddess")]
        public string? PayerTenantAddess { get; set; }
    }



// using System.Text.Json.Serialization;
// using FabPropertiesLandlordPortal;


// public class PrintVATInvoiceVM
// {
//     [JsonPropertyName("logoImage")]
//     public string? LogoImage { get; set; }
//     [JsonPropertyName("data")]
//     public List<PrintVATInvoiceViewModel>? Data { get; set; }
// }
// public class PrintVATInvoiceFilter
// {
//     public string? ReportType { get; set; }
//     public int PageSize { get; set; }
//     public int PageNumber { get; set; }
//     public int? LandlordID { get; set; } = 417;
//     public int? ReceiverTypeID { get; set; }
//     public bool IsShowMFee { get; set; }
//     public bool IsShowPayment { get; set; }
//     public bool IsShowCreditNote { get; set; }
//     public string? FromDate { get; set; }
//     public string? ToDate { get; set; }
//     public string? InvoiceNo { get; set; }
//     public string? ContractNo { get; set; }
//     public int LoginMemberId { get; set; } = 0;
//     public string? CallingPortalName { get; set; } = Labels.portalName;
// }
// public class PrintVATInvoiceViewModel
// {
//     [JsonPropertyName("vatInvoiceID")]
//     public string? VATInvoiceID { get; set; }

//     [JsonPropertyName("landlordName")]
//     public string? LandlordName { get; set; }

//     [JsonPropertyName("isFGP")]
//     public bool IsFGP { get; set; }

//     [JsonPropertyName("propertyName")]
//     public string? PropertyName { get; set; }

//     [JsonPropertyName("emirateName")]
//     public string? EmirateName { get; set; }

//     [JsonPropertyName("contractNo")]
//     public string? ContractNo { get; set; }

//     [JsonPropertyName("fromDate")]
//     public string? FromDate { get; set; }

//     [JsonPropertyName("toDate")]
//     public string? ToDate { get; set; }

//     [JsonPropertyName("contractUnits")]
//     public string? ContractUnits { get; set; }

//     [JsonPropertyName("receiverName")]
//     public string? ReceiverName { get; set; }

//     [JsonPropertyName("receiverAddress")]
//     public string? ReceiverAddress { get; set; }

//     [JsonPropertyName("receiverVATRegisteredName")]
//     public string? ReceiverVATRegisteredName { get; set; }

//     [JsonPropertyName("receiverVATRegNo")]
//     public string? ReceiverVATRegNo { get; set; }

//     [JsonPropertyName("receiverType")]
//     public string? ReceiverType { get; set; }

//     [JsonPropertyName("contractID")]
//     public string? ContractID { get; set; }

//     [JsonPropertyName("transactionNo")]
//     public string? TransactionNo { get; set; }

//     [JsonPropertyName("ownerID")]
//     public string? OwnerID { get; set; }

//     [JsonPropertyName("transactionID")]
//     public string? TransactionID { get; set; }

//     [JsonPropertyName("payerEmirateName")]
//     public string? PayerEmirateName { get; set; }

//     [JsonPropertyName("countryName")]
//     public string? CountryName { get; set; }

//     [JsonPropertyName("payerID")]
//     public string? PayerID { get; set; }

//     [JsonPropertyName("payerName")]
//     public string? PayerName { get; set; }

//     [JsonPropertyName("payerType")]
//     public string? PayerType { get; set; }

//     [JsonPropertyName("payerVATRegNo")]

//     public string? PayerVATRegNo { get; set; }
//     [JsonPropertyName("payerTenantAddess")]
//     public string? PayerTenantAddess { get; set; }

//     [JsonPropertyName("propertyID")]
//     public string? PropertyID { get; set; }

//     [JsonPropertyName("vatType")]
//     public string? VATType { get; set; }

//     [JsonPropertyName("invoiceNo")]
//     public string? InvoiceNo { get; set; }

//     [JsonPropertyName("invoiceFromDate")]
//     public string? InvoiceFromDate { get; set; }

//     [JsonPropertyName("invoiceToDate")]
//     public string? InvoiceToDate { get; set; }

//     [JsonPropertyName("invoiceDate")]
//     public string? InvoiceDate { get; set; }

//     [JsonPropertyName("totalAmount")]
//     public decimal TotalAmount { get; set; }
//     [JsonPropertyName("vatAmountInWords")]
//     public string? VATAmountInWords { get; set; }

//     [JsonPropertyName("vatAmount")]
//     public decimal VATAmount { get; set; }

//     [JsonPropertyName("taxableValue")]
//     public decimal TaxableValue { get; set; }

//     [JsonPropertyName("chargeAmount")]
//     public decimal ChargeAmount { get; set; }

//     [JsonPropertyName("netAmount")]
//     public decimal NetAmount { get; set; }

//     [JsonPropertyName("reasonOfPayment")]
//     public string? ReasonOfPayment { get; set; }

//     [JsonPropertyName("vatReasonOfPaymentID")]
//     public string? VATReasonOfPaymentID { get; set; }
//     [JsonPropertyName("amountInWords")]
//     public string? AmountInWords { get; set; }

//     [JsonPropertyName("companyName")]
//     public string? CompanyName { get; set; }
//     [JsonPropertyName("address1")]
//     public string? Address1 { get; set; }
//     [JsonPropertyName("phoneNo")]
//     public string? PhoneNo { get; set; }
//     [JsonPropertyName("emailID")]
//     public string? EmailID { get; set; }
//     [JsonPropertyName("webSite")]
//     public string? WebSite { get; set; }
//     [JsonPropertyName("isShowMFee")]
//     public bool IsShowMFee { get; set; }
//     [JsonPropertyName("isShowPayment")]
//     public bool IsShowPayment { get; set; }
//     [JsonPropertyName("isShowCreditNote")]
//     public bool IsShowCreditNote { get; set; }
// }

