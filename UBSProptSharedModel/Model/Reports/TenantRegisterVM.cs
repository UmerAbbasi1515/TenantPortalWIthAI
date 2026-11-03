using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class RegisterFilterRequest
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public int[]? TenantTypeID { get; set; }
    public int[]? MasterTenantTypeID { get; set; }
    public int[]? Emirate { get; set; }
    public string? ReportType { get; set; }
    public int LoginMemberId { get; set; } = LabelsLandlordPortal.UserID;
    //public int[]? Landlord { get; set; }
    public int[]? Landlord { get; set; } = new int[] { LabelsLandlordPortal.landlordID };
    public string? CallingPortalName { get; set; } = LabelsLandlordPortal.portalName;
    public bool activeTenant { get; set; }
}
