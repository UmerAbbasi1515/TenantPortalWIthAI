using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class TenancyScheduleFilterRequest
{
    public int[]? Emirate { get; set; }
    public int[]? Landlord { get; set; } = new int[] { LabelsLandlordPortal.landlordID };
    public int[]? Property { get; set; }
    public string? ReportType { get; set; }
    public string? callingPortalName { get; set; } = LabelsLandlordPortal.portalName;

    [Required(ErrorMessage = "From Date is required.")]
    public string? FromDate { get; set; }
    [Required(ErrorMessage = "To Date is required.")]
    public string? ToDate { get; set; }
    public int? DateBy { get; set; } = 1;
    public int[]? ContractPosition { get; set; }
    public int[]? PurposeOfUse { get; set; }
    public int LoginMemberId { get; set; } = LabelsLandlordPortal.UserID;
}