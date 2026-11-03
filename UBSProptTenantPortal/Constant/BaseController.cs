using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


public class BaseController : Controller
{
    public string? token { get; set; }
    public string? landlordID { get; set; }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var token = context.HttpContext.Session.GetString("JWTToken");
        var landlordID = context.HttpContext.Session.GetString("LandlordID");
        Console.WriteLine($"jsonResponse:::::::{token}");
        Console.WriteLine($"jsonResponse:::::::{landlordID}");
        token = context.HttpContext.Session.GetString("JWTToken");
        landlordID = context.HttpContext.Session.GetString("LandlordID");
        base.OnActionExecuting(context);
        
        if (string.IsNullOrWhiteSpace(context.HttpContext.Session.GetString("JWTToken")))
        {
            context.Result = new RedirectToRouteResult(new { controller = "Auth", action = "AuthScreen", Area = "Users" });
        }
        else
        {
            token = context.HttpContext.Session.GetString("JWTToken");
            landlordID = context.HttpContext.Session.GetString("LandlordID");
            base.OnActionExecuting(context);
        }
    }
}

