using System.Text.Json.Serialization;

public class BouncedChequesFiltercs
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public int[] Landlord { get; set; } = new int[] { LabelsLandlordPortal.landlordID };
    public int[]? Emirate { get; set; }
    public int[]? Property { get; set; }
    public int[]? PropertyGroup { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public string? ReportType { get; set; }
    public int LoginMemberId { get; set; } = LabelsLandlordPortal.UserID;
    public string? CallingPortalName { get; set; } = LabelsLandlordPortal.portalName;
}
