

public class ManagmentFeeFilterReport
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int[]? PropertyGroup { get; set; }
    public int[]? Landlord { get; set; } = new int[] { LabelsLandlordPortal.landlordID };
    public int[]? Emirate { get; set; }
    public int[]? Property { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public string? UnitRefNo { get; set; }
    public int LoginMemberId { get; set; } = LabelsLandlordPortal.UserID;
    public string? ReportType { get; set; }
    public string? CallingPortalName { get; set; } = LabelsLandlordPortal.portalName;
}
public class ManagementFeeViewModel
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalRecord { get; set; }
    public int? PropertyId { get; set; }
    public int? LandlordId { get; set; } = LabelsLandlordPortal.landlordID;
    public int? ManagementFeeMethodId { get; set; }
    public bool? PreviousUnInvoiced { get; set; }
    public bool? UnInvoiced { get; set; }
    public string? ContractNo { get; set; }
    public string? InvoiceNo { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? Todate { get; set; }
    public List<ManagementFeeDetailViewModel>? managementFeeDetailList { get; set; }
    public List<GenerateInvoiceViewModel>? generateInvoiceVM { get; set; }
    public List<ManagementFeeInvoicesViewModel>? managementFeeInvoices { get; set; }
    public Dictionary<string, bool>? Permission { get; set; }
}
public class ManagementFeeDetailViewModel
{
    public int ManagementFeeID { get; set; }
    public string? ManagementFeeNo { get; set; }
    public string? ContractNo { get; set; }
    public int ContractID { get; set; }
    public decimal Fees { get; set; }
    public int Month { get; set; }
    public int Days { get; set; }
    public int Year { get; set; }
    public bool Suspend { get; set; }
    public DateTime? ExportedDate { get; set; }
    public int ExportedStatus { get; set; }
    public string? MFeePercent { get; set; }
    public bool IsPaid { get; set; }
    public string? MFeeDate { get; set; }
    public string? PaymentStatus { get; set; }
    public string? TableName { get; set; }
    public int PKId { get; set; }
    public string? Action { get; set; }
    public string? ManagementFeeMethod { get; set; }
    public string? PropertyName { get; set; }
    public int PropertyId { get; set; }
    public string? InvoiceNo { get; set; } = string.Empty;
    public string? ManagementFeeOn { get; set; } = string.Empty;

    public int TotalRecord { get; set; }
    public bool IsSelected { get; set; }
}
public class GenerateInvoiceViewModel
{
    public int PropertyId { get; set; }
    public string? PropertyName { get; set; }
    public decimal Fees { get; set; }
    public string? ManagementFeeIds { get; set; }
    public bool IsSelected { get; set; }
}
public class ManagementFeeInvoicesViewModel
{
    public int VATInvoiceID { get; set; }
    public int PropertyID { get; set; }
    public string? BuildingNumber { get; set; } = string.Empty;
    public int ReasonofPaymentID { get; set; }
    public string? InvoiceNo { get; set; } = string.Empty;
    public string? PropertyName { get; set; } = string.Empty;
    public string? InvoiceDate { get; set; } = string.Empty;
    public string? PaymentFor { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal VATAmount { get; set; }
    public string? InvoiceFromDate { get; set; } = string.Empty;
    public string? InvoiceToDate { get; set; } = string.Empty;
    public string? AddedDate { get; set; } = string.Empty;
    public int TotalRecord { get; set; }

}

