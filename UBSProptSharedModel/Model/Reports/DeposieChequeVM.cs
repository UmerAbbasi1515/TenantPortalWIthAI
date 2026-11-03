using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DepositChequeFilter
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public string? ContractNo { get; set; }
    public int[]? PropertyGroup { get; set; }
    public int[]? Landlord { get; set; } = new int[] { LabelsLandlordPortal.landlordID };
    public int[]? Emirate { get; set; }
    public int[]? Property { get; set; }
    public int[]? ReasonOfPayment { get; set; }
    public string? DepositFromDate { get; set; }
    public string? DepositToDate { get; set; }
    public string? TransStartDate { get; set; }
    public string? TransEndDate { get; set; }
    public int LoginMemberId { get; set; } = LabelsLandlordPortal.UserID;
    public string? ReportType { get; set; }
    public string? CallingPortalName { get; set; } = LabelsLandlordPortal.portalName;
}
