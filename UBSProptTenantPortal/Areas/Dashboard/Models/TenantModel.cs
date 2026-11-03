

public class DashboardViewModel
{
    public NotificationPopUpModel? PopUp { get; set; }
    public GetDataRepose? GetData { get; set; }
    public ContractResponse? Contracts { get; set; }
    public ServiceRequestsResponse? ServiceRequest { get; set; }
    public PaymentResponse? Payments { get; set; }
}

// Get Data

public class GetDataViewModel
{

    public string? Status { get; set; }
    public List<DataModel>? Data { get; set; }
    public string? Message { get; set; }
    public string? UnreadNotifications { get; set; }
}

public class DataModel
{
    public string? LandlordName { get; set; }
    public string? LandlordNameAR { get; set; }
    public int OccupiedUnits { get; set; }
    public int VacantUnits { get; set; }
    public int TotalUnits { get; set; }
    public int ActiveContracts { get; set; }
    public int ContractExpiringIn30Days { get; set; }
    public int CheckDueIn30Days { get; set; }
}

// New
public class GetDataRepose
{
    public string? Status { get; set; }
    public Dashboard? Dashboard { get; set; }
    public string? StatusCode { get; set; }
    public string? Message { get; set; }
    public int? UnreadNotifications { get; set; }
    public int? ContractRenewalAction { get; set; }

    public dynamic? contractExpiringIn30Days { get; set; }
    public dynamic? toBePaidIn30DaysLength { get; set; }
    public dynamic? paymentCurrency { get; set; }
    public dynamic? paidCurrency { get; set; }
    public dynamic? toBePaidCurrency { get; set; }
    public dynamic? balanceCurrency { get; set; }
    public dynamic? rentalVal { get; set; }
    public dynamic? rentOutstanding { get; set; }
}

public class Dashboard
{
    public int? TenantId { get; set; }
    public string? TenantName { get; set; }
    public string? TenantNameAr { get; set; }
    public decimal? PaymentBalance { get; set; }
    public int? ContractExpiringIn30Days { get; set; }
    public int? CheckDueIn30Days { get; set; }
    public decimal? RentalVal { get; set; }
    public decimal? RentOutstanding { get; set; }
    public int? ActiveContracts { get; set; }
}

