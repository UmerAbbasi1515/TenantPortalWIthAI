namespace UBSProptTenantPortal.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}

public class ErrorViewModelN
{
    public string? ErrorMessage { get; set; }
    public string? ErrorUrl { get; set; }
}