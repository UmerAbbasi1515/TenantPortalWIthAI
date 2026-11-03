using System.Text.Json.Serialization;



public class BuildingStatusFilter
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public int[]? PropertyGroup { get; set; }
    public int[] Landlord { get; set; } = new int[] { LabelsLandlordPortal.landlordID };
    public int[]? Emirate { get; set; }
    public int[]? Property { get; set; }
    public string? UnitRefNo { get; set; }
    public int LoginUserID { get; set; } = LabelsLandlordPortal.UserID;
    public string? ReportType { get; set; }
    public bool IsZeroBalanceChecked { get; set; }
     public string? CallingPortalName { get; set; } = LabelsLandlordPortal.portalName;
}

public class BuildingStatusVM
{
    [JsonPropertyName("data")]
    public IEnumerable<BuildingStatusDetailVM>? Data { get; set; }
}
public class BuildingStatusDetailVM
{
    [JsonPropertyName("landlord")]
    public string? Landlord { get; set; }

    [JsonPropertyName("emirate")]
    public string? Emirate { get; set; }

    [JsonPropertyName("property")]
    public string? Property { get; set; }

    [JsonPropertyName("totalRecord")]
    public int TotalRecord { get; set; }

    [JsonPropertyName("tenantName")]
    public string? TenantName { get; set; }

    [JsonPropertyName("contractNo")]
    public string? ContractNo { get; set; }

    [JsonPropertyName("unitType")]
    public string? UnitType { get; set; }

    [JsonPropertyName("unitTypeAR")]
    public string? UnitTypeAR { get; set; }

    [JsonPropertyName("unitCategory")]
    public string? UnitCategory { get; set; }

    [JsonPropertyName("contractStatus")]
    public string? ContractStatus { get; set; }

    [JsonPropertyName("rent")]
    public decimal Rent { get; set; }

    [JsonPropertyName("recived")]
    public decimal Recived { get; set; }

    [JsonPropertyName("balance")]
    public decimal Balance { get; set; }

    [JsonPropertyName("contractAmount")]
    public decimal ContractAmount { get; set; }

    [JsonPropertyName("issueDate")]
    public string? IssueDate { get; set; }

    [JsonPropertyName("startDate")]
    public string? StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public string? EndDate { get; set; }

    [JsonPropertyName("propertyRef")]
    public string? PropertyRef { get; set; }

    [JsonPropertyName("mobile")]
    public string? Mobile { get; set; }

    [JsonPropertyName("propertyStatus")]
    public string? PropertyStatus { get; set; }

    [JsonPropertyName("tenantKind")]
    public string? TenantKind { get; set; }

    [JsonPropertyName("unitRef")]
    public string? UnitRef { get; set; }

    [JsonPropertyName("areasize")]
    public string? Areasize { get; set; }


}

