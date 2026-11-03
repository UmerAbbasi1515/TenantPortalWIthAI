using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RefundableDepositsVM
{
    public string? LogoImage { get; set; }
    public List<List<Dictionary<string, object>>>? data { get; set; }
}
public class RefundableDepositsFilter
{
    public string? ReportType { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int[]? PropertyGroup { get; set; }
    public int[]? Landlord { get; set; } = new int[] { LabelsLandlordPortal.landlordID };
    public int[]? Emirate { get; set; }
    public int[]? Property { get; set; }
    public int? ReasonOfPaymentId { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public string? UnitRefNo { get; set; }
    public int? TenantID { get; set; }
    public string? callingPortalName { get; set; } = LabelsLandlordPortal.portalName;
    public int LoginMemberId { get; set; } = LabelsLandlordPortal.UserID;
    public string? EmirateCommaSeparated => Emirate != null && Emirate.Any() ? string.Join(",", Emirate) : null;
    public string? PropertyCommaSeparated => Property != null && Property.Any() ? string.Join(",", Property) : null;
}
public class SecurityDepositsVM
{
    public string? LogoImage { get; set; }
    public List<List<Dictionary<string, object>>>? data { get; set; }
    public string? FromDate { get; set; }
}
public class SecurityDepositsFilter
{
    //flag for calling protal .. to identity the api calling from which portal 
    public string? CallingPortalName { get; set; } = LabelsLandlordPortal.portalName;

    public string? ReportType { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int[]? PropertyGroup { get; set; }
    public int[]? Landlord { get; set; } = new int[] { LabelsLandlordPortal.landlordID };
    public int[]? Emirate { get; set; }
    public int[]? Property { get; set; }
    public int? ReasonOfPaymentId { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public string? UnitRefNo { get; set; }
    public int? Tenant { get; set; }

    public int LoginMemberId { get; set; }
    public string? LandlordCommaSeparated => Landlord != null && Landlord.Any() ? string.Join(",", Landlord) : null;
    public string? EmirateCommaSeparated => Emirate != null && Emirate.Any() ? string.Join(",", Emirate) : null;
    public string? PropertyCommaSeparated => Property != null && Property.Any() ? string.Join(",", Property) : null;
}
