using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;


public class VATInvoiceFilter
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public int[]? PropertyGroup { get; set; }
    public int[]? Landlord { get; set; } = new int[] { LabelsLandlordPortal.landlordID };
    public int[]? Emirate { get; set; }
    public int[]? Property { get; set; }
    public string? ContactNo { get; set; }
    public int[]? ReasonOfPayment { get; set; }
    public int[]? VatType { get; set; }
    public string? ReportType { get; set; }

    [Required(ErrorMessage = "Invoice From Date is required.")]
    public string? InvoiceFrmDate { get; set; }

    [Required(ErrorMessage = "Invoice To Date is required.")]
    public string? InvoiceToDate { get; set; }
    public int LoginMemberId { get; set; } = LabelsLandlordPortal.UserID;
    public string? CallingPortalName { get; set; } = LabelsLandlordPortal.portalName;
}
