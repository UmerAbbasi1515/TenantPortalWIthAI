
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


public class UnitStatusFilterRequest
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public int[]? Landlord { get; set; } = new int[] { LabelsLandlordPortal.landlordID };
    public int[]? Emirate { get; set; } = null;
    public int[]? Property { get; set; } = null;
    public int[]? PropertyGroup { get; set; }
    public int[]? UnitStatus { get; set; } = null;
    public string? UnitRefNo { get; set; }
    public int[]? Bedrooms { get; set; }
    public string? ReportType { get; set; }
    public int LoginMemberId { get; set; } = LabelsLandlordPortal.UserID;
    public string? CallingPortalName { get; set; } = LabelsLandlordPortal.portalName;
}


public class UnitFilterRequest
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public int[]? PropertyGroup { get; set; }
    public int[]? Landlord { get; set; } = new int[] { LabelsLandlordPortal.landlordID };
    public int[]? Emirate { get; set; }
    public int[]? Property { get; set; }
    public int[]? UnitType { get; set; }
    public int[]? Bedrooms { get; set; }
    public string? UnitRefNo { get; set; }
    public string? ReportType { get; set; }
    public int LoginMemberId { get; set; } = LabelsLandlordPortal.UserID;
    public string? CallingPortalName { get; set; } = LabelsLandlordPortal.portalName;
}

public class UnitVacancyVM
{
    [JsonPropertyName("totalRecord")]
    public int TotalRecord { get; set; }

    [JsonPropertyName("data")]
    public IEnumerable<UnitVacancyDetailVM>? Data { get; set; }

    [JsonPropertyName("filters")]
    public FilterVM? Filters { get; set; }
}

public class FilterVM
{
    [JsonPropertyName("landlordFilter")]
    public string? LandlordFilter { get; set; }

    [JsonPropertyName("propertyFilter")]
    public string? PropertyFilter { get; set; }

    [JsonPropertyName("propertyGroupFilter")]
    public string? PropertyGroupFilter { get; set; }

    [JsonPropertyName("emiratesFilter")]
    public string? EmiratesFilter { get; set; }

    [JsonPropertyName("unitTypeFilter")]
    public string? UnitTypeFilter { get; set; }

    [JsonPropertyName("bedRoomFilter")]
    public string? BedRoomFilter { get; set; }
}

public class UnitVacancyDetailVM
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("areaSize")]
    public string? AreaSize { get; set; }

    [JsonPropertyName("areaSizeSQM")]
    public string? AreaSizeSQM { get; set; }

    [JsonPropertyName("unitCategory")]
    public string? UnitCategory { get; set; }

    [JsonPropertyName("rent")]
    public string? Rent { get; set; }

    [JsonPropertyName("totalRecord")]
    public int TotalRecord { get; set; }

    [JsonPropertyName("propertyRef")]
    public string? PropertyRef { get; set; }

    [JsonPropertyName("propertyNo")]
    public string? PropertyNo { get; set; }

    [JsonPropertyName("emirates")]
    public string? Emirates { get; set; }

    [JsonPropertyName("owner")]
    public string? Owner { get; set; }

    [JsonPropertyName("property")]
    public string? Property { get; set; }

    [JsonPropertyName("unitRef")]
    public string? UnitRef { get; set; }

    [JsonPropertyName("vacatedBy")]
    public string? VacatedBy { get; set; }

    [JsonPropertyName("vacationDate")]
    public string? VacationDate { get; set; }

    [JsonPropertyName("targetRent")]
    public string? TargetRent { get; set; }

    [JsonPropertyName("noOfDaysSinceVacating")]
    public string? NoOfDaysSinceVacating { get; set; }

    [JsonPropertyName("unitStatus")]
    public string? UnitStatus { get; set; }


    [JsonPropertyName("maintenanceStatus")]
    public string? MaintenanceStatus { get; set; }

    [JsonPropertyName("rooms")]
    public string? Rooms { get; set; }


}