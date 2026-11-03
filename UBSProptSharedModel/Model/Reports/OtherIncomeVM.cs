using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class OtherIncomeVM
{
    public string? LogoImage { get; set; }
    public List<List<Dictionary<string, object>>>? data { get; set; }
}
public class OtherIncomeFilter
{
    public string? ReportType { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int[]? PropertyGroup { get; set; }
    public int[]? Landlord { get; set; } = new int[] { LabelsLandlordPortal.landlordID };
    public int[]? Emirate { get; set; }
    public int[]? Property { get; set; }
    public int[]? ReasonOfPayment { get; set; }
    public int[]? PaymentKinds { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public string? UnitRefNo { get; set; }
    public int? TenantID { get; set; }
    public int LoginMemberId { get; set; } = LabelsLandlordPortal.UserID;
    public string? CallingPortalName { get; set; } = LabelsLandlordPortal.portalName;
}

