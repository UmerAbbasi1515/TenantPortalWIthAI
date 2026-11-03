using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


public class ChequeRegisterVM
{
    [JsonPropertyName("data")]
    public List<List<Dictionary<string, object>>>? data { get; set; }
}
  public class ChequeRegisterFilter
    {
        public string? ReportType { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string? ContractNo { get; set; }
        public int[]? PropertyGroup { get; set; }
        public int[]? Landlord { get; set; } = new int[] { LabelsLandlordPortal.landlordID };
        public int[]? Emirate { get; set; }
        public int[]? Property { get; set; }
        public int[]? ChequeStatus { get; set; }
        public int[]? ReasonOfPayment { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string? FilterBy { get; set; }

        public int LoginMemberId { get; set; } = LabelsLandlordPortal.UserID;
    public string? CallingPortalName { get; set; } = LabelsLandlordPortal.portalName;
    }

