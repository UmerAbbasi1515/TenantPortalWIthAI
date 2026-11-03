using Microsoft.AspNetCore.Mvc.RazorPages;

public class ErrorPageModel : PageModel
{
    public string? ErrorMessage { get; set; }
    public string? UrlToRetry { get; set; }

    public void OnGet()
    {
        // Get the query parameters from the URL
        ErrorMessage = Request.Query["message"];
        UrlToRetry = Request.Query["url"];
    }
}
