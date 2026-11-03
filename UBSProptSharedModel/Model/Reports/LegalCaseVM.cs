

public class LegalCaseFilter
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public int[]? PropertyGroup { get; set; }
    public int[]? Landlord { get; set; } = new int[] { LabelsLandlordPortal.landlordID }; 
    public int[]? Emirate { get; set; }
    public int[]? Property { get; set; }
    public string? UnitRefNo { get; set; }
    public string? AsOnDate { get; set; }
    public int LoginUserID { get; set; } = LabelsLandlordPortal.UserID;
    public string? ReportType { get; set; }
    public string? CallingPortalName { get; set; } = LabelsLandlordPortal.portalName;
}

