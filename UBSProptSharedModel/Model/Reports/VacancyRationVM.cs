

public class VacancyRatioReportFilter
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public string? CallingPortalName { get; set; } = LabelsLandlordPortal.portalName;
    public int[]? PropertyGroup { get; set; }
    public int[]? Landlord { get; set; } = new int[] { LabelsLandlordPortal.landlordID };
    public int[]? Emirate { get; set; }
    public int[]? Property { get; set; }
    public int[]? UnitType { get; set; }
    public string? UnitRefNo { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public bool SummaryReport { get; set; }
    public int[]? BedRooms { get; set; }
    public string? ReportType { get; set; }
    public int LoginUserID { get; set; } = LabelsLandlordPortal.UserID;
}