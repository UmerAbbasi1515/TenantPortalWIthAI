


public class StatementofAccountFilterRequest
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public string? ReportType { get; set; }
    public int LoginMemberId { get; set; } = LabelsLandlordPortal.UserID;
    public int[]? PropertyGroup { get; set; }
    public int[]? Landlord { get; set; } = new int[] { LabelsLandlordPortal.landlordID };
    public int[]? Emirate { get; set; }
    public int[]? Property { get; set; }
    public int[]? TenantType { get; set; }
    public string? Tenant { get; set; }
    public string? UnitNo { get; set; }
    public string? ToDate { get; set; }
    public string? FromDate { get; set; }
    public string? CallingPortalName { get; set; } = LabelsLandlordPortal.portalName;
}
