
public class VacancyRentalossFilter
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public int[]? Landlord { get; set; } = new int[] { LabelsLandlordPortal.landlordID };
    public int[]? Emirate { get; set; }
    public int[]? Property { get; set; }
    public int[]? PropertyGroup { get; set; }
    public int[]? UnitStatus { get; set; }
    public string? UnitRefNo { get; set; }
    public string? ReportType { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public int[]? Bedrooms { get; set; }
    public int LoginMemberId { get; set; } = LabelsLandlordPortal.UserID;
    public int[]? UnitType { get; set; }
    public string? CallingPortalName { get; set; } = LabelsLandlordPortal.portalName;
}