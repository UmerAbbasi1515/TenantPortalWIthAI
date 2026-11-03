

public class SapArAgningFilter
{
    public string? company { get; set; } =  "";
    public string? asonDate { get; set; } =  "";
    public string? reportType { get; set; } =  "";
    public string? ageFromDueDate { get; set; } =  "";
    //public int[]? Landlord { get; set; } = new int[] { LabelsLandlordPortal.landlordID };
    public string? callingPortalName { get; set; } = LabelsLandlordPortal.portalName;
    public string loginUserID { get; set; } = LabelsLandlordPortal.UserID.ToString();
}



public class SapTrailBalanceFilter
{
    public string? TbPeriod { get; set; } = "";
    public string? TbType { get; set; } = "";
    //public int[]? Landlord { get; set; } = new int[] { LabelsLandlordPortal.landlordID };
    public string? callingPortalName { get; set; } = LabelsLandlordPortal.portalName;
    public string loginUserID { get; set; } = LabelsLandlordPortal.UserID.ToString();
    public string? reportType { get; set; } = "";
}
