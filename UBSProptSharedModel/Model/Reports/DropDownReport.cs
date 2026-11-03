

public class DropdownModel
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public int? TenantTypeId { get; set; }
}

public class PropertyTypesModel
{
    public string? Status { get; set; }
    public string? StatusCode { get; set; }
    public List<PropertyTypeL>? PropertyTypes { get; set; }
    public string? Message { get; set; }
}

public class PropertyTypeL
{
    public string? PropertyType { get; set; }
    public int? PropertyTypeId { get; set; }
    public string? PropertyTypeAr { get; set; }
}


public class ContractStatusModel
{
    public string? Status { get; set; }
    public string? StatusCode { get; set; }
    public List<ContractStatus>? ContractStatus { get; set; }
    public string? Message { get; set; }
}
public class ContractStatus
{
    public int? ContractTypeId { get; set; }
    public string? ContractType { get; set; }
    public string? ContractTypeAr { get; set; }
}

public class ContractStatusVendorModel
{
    public string? Status { get; set; }
    public string? StatusCode { get; set; }
    public List<VendorContractStatus>? ContractStatus { get; set; }
    public string? Message { get; set; }
}

public class VendorContractStatus
{
    public int? ServiceContractStatusID { get; set; }
    public string? StatusName { get; set; }
    public string? StatusNameAr { get; set; }
}

public class GetLpoStatusModel
{
    public string? Status { get; set; }
    public string? StatusCode { get; set; }
    public List<LpoStatus> LpoStatus { get; set; } = new List<LpoStatus>(); // safe default
    public string? Message { get; set; }
}

public class LpoStatus
{
    public int? LpoStatusId { get; set; }        // dynamic → int? (nullable int)
    public string? LpoStatusName { get; set; }
    public string? LpoStatusNameAr { get; set; }
}
