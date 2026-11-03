

using System.ComponentModel.DataAnnotations;

public class ContractStatusFilter
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; } = 1;
    public int[]? PropertyGroup { get; set; }
    public int[]? Landlord { get; set; } = new int[] { LabelsLandlordPortal.landlordID };
    public int[]? Property { get; set; }
    public int[]? ContractStatus { get; set; }
    public int[]? TenantType { get; set; }
    public int[]? RentPaidBy { get; set; }
    public int[]? Emirate { get; set; }
    public int[]? ContractAction { get; set; }
    public int? ContractPosition { get; set; }
    public string? UnitRefNo { get; set; }
    public bool ContarctExpiry { get; set; }
    
    [Required(ErrorMessage = "From Date is required.")]
    public string? FromDate { get; set; }
    [Required(ErrorMessage = "To Date is required.")]
    public string? ToDate { get; set; }
    public int? DateBy { get; set; } = 1;
    public int? TenantID { get; set; }
    public int? LoginUserID { get; set; } = LabelsLandlordPortal.UserID;
    public string? ReportType { get; set; }
    public string? CallingPortalName { get; set; } = LabelsLandlordPortal.portalName;

}