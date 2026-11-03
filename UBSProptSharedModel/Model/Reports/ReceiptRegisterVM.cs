using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ReceiptRegisterFilter
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public int[]? PropertyGroup { get; set; }
    public int[]? Landlord { get; set; } = new int[] { LabelsLandlordPortal.landlordID };
    public int[]? Emirate { get; set; }
    public int[]? Property { get; set; }
    public string? ContractNo { get; set; }
    public int[]? ReasonOfPayment { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public int? TransactionType { get; set; }
    public bool ShowUnposted { get; set; }
    public bool ShowAccountDetails { get; set; }
    public bool IncludeVATReceipts { get; set; }
    public int LoginMemberId { get; set; } = LabelsLandlordPortal.UserID;
    public string? ReportType { get; set; }
    public string? UnitRefNo { get; set; }
    public string? CallingPortalName { get; set; } = LabelsLandlordPortal.portalName;
}
