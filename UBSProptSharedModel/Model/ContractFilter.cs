using System;
using System.ComponentModel.DataAnnotations;

namespace UBSProptSharedModel.Model
{

    public class ContractsFilter
    {
        public string? Property { get; set; }
        public int? PropertyType { get; set; }
        public int? ContractStatus { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }

    public class ServiceRequestFilter
    {
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }

    public class LposFilter
    {
        public string? Property { get; set; }
        public int? PropertyType { get; set; }
        public int? LpoStatusId { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }
}

