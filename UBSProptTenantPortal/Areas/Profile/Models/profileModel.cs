public class TenantProfileApiResponse
{
    public string? status { get; set; }
    public string? statuCode { get; set; } // typo from API?
    public ProfileModel? profile { get; set; }
    public string? message { get; set; }
}
public class ProfileViewModelResponse
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public string? isProfileUpdate { get; set; }
    public List<ProfileModel>? Data { get; set; }
}

public class ProfileModel
{
    public string? Mobile { get; set; }
    public string? name { get; set; }
    public string? nameAR { get; set; }
    public string? LandlordID { get; set; }
    public string? Email { get; set; }
    public string? Nationality { get; set; }
    public string? Fax { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? AddressAR { get; set; }
    public string? TermsAndConditions { get; set; }
    public string? PhotoUrl { get; set; }
}
