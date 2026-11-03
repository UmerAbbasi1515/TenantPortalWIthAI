using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Data;
using System;
using static Google.Apis.Requests.BatchRequest;
using UBSProptTenantPortal.Models;
using Microsoft.AspNetCore.Http.HttpResults;


namespace UBSProptTenantPortal.Areas.Dashboard.Controllers
{
    [Area("Users")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly UBSAuthRepository authRepo;
        public AuthController(Microsoft.Extensions.Logging.ILogger<AuthController> logger, UBSAuthRepository _authRepo)
        {
            _logger = logger;
            authRepo = _authRepo;
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("AuthScreen", "Auth");
        }

        // Validtator Start
        // EmailValidation
        public IActionResult IsValidEmailAjax([FromBody] EmailValidatorModel emailRequest)
        {
            // Validate email using the Regex method
            bool isValid = isValidEmail(emailRequest.Email ?? "");

            // Return a response
            return Json(new { isValid });
        }
        public static bool isValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
        // Validator End
        public IActionResult AuthScreen()
        {
            ViewBag.errorAuthScreen = null;
            return View();
            // try
            // {
            //     var data = new { };

            //     ViewBag.errorAuthScreen = null;
            //     var response = await authRepo.GetLanguagesAsync(data);
            //     if (response is String)
            //     {
            //         if (AuthLabelsPortal.isErrorFunction(response))
            //         {
            //             Console.WriteLine("Redirecting to AuthScreen...");
            //             ViewBag.errorAuthScreen = response;
            //             return View();
            //         }
            //         else
            //         {
            //             ViewBag.errorAuthScreen = AuthLabelsPortal.AnyError;
            //             return View();
            //         }
            //     }
            //     else
            //     {
            //         ViewBag.errorAuthScreen = null;
            //         ViewBag.language = response.language;
            //         return View(response);
            //     }

            // }
            // catch (HttpRequestException e)
            // {
            //     ViewBag.errorAuthScreen = e.Message;
            //     Console.WriteLine($"Request error: {e.Message}");
            //     return View(null);
            // }
        }
        public JsonResult getTokenForLoadFunction()
        {
            var token = HttpContext.Session.GetString("JWTToken");
            var jsonResponse = new
            {
                token = token
            };
            return Json(jsonResponse);
        }

        public async Task<JsonResult> GenerateFirbaseUserToken([FromBody] GenerateFirbaseUserTokenParameter data)
        {
            try
            {
                ViewBag.errorAuthScreen = null;
                var response = await authRepo.GenerateFirbaseUserToken(data);
                if (response is String)
                {
                    if (AuthLabelsPortal.isErrorFunction(response))
                    {
                        Console.WriteLine("Redirecting to AuthScreen...");
                        ViewBag.errorAuthScreen = response;
                        return Json("");
                    }
                    else
                    {
                        ViewBag.errorAuthScreen = AuthLabelsPortal.AnyError;
                        return Json("");
                    }
                }
                else
                {
                    ViewBag.generateFirbaseUserData = response;
                    AuthLabelsPortal.jwtToken = response.token;
                    AuthLabelsPortal.userNameEng = response.user.name;
                    AuthLabelsPortal.userNameAR = response.user.fullNameAr;
                    AuthLabelsPortal.mobile = response.user.mobile;
                    AuthLabelsPortal.email = response.user.email;

                    HttpContext.Session.SetString("JWTToken", $"{response.token.ToString()}");
                    //HttpContext.Session.SetString("LandlordID", $"{response.landLordId.ToString()}");
                    HttpContext.Session.SetString("CompanyName", $"UBSPropt");
                    HttpContext.Session.SetString("userNameEng", $"{response.user.name.ToString()}");
                    HttpContext.Session.SetString("userNameAR", $"{response.user.fullNameAr.ToString()}");
                    HttpContext.Session.SetString("mobile", $"{response.user.mobile.ToString()}");
                    HttpContext.Session.SetString("email", $"{response.user.email.ToString()}");
                    HttpContext.Session.SetString("userId", $"{response.user.userId.ToString()}");
                    var token = HttpContext.Session.GetString("JWTToken");
                    //var landlordID = HttpContext.Session.GetString("LandlordID");
                    var CompanyName = HttpContext.Session.GetString("CompanyName");
                    var userNameEng = HttpContext.Session.GetString("userNameEng");
                    var userNameAR = HttpContext.Session.GetString("userNameAR");
                    var mobile = HttpContext.Session.GetString("mobile");
                    var mpinSet = response.user.mpinSet.ToString();
                    var jsonResponse = new
                    {
                        statusCode = response.statusCode, // Assuming this is in your response
                        user = response.user,
                        token = response.token,
                        mpinSet = mpinSet
                    };
                    Console.WriteLine($"jsonResponse:::::::{jsonResponse.statusCode}");
                    return Json(jsonResponse);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorAuthScreen = e.Message;
                return Json(new
                {
                    statusCode = "", // Assuming this is in your response
                    user = "",
                    token = "",
                    mpinSet = ""
                });
            }
        }

        public async Task<JsonResult> SaveNewMpin([FromBody] ResetMpinVerification data)
        {
            try
            {
                ViewBag.errorAuthScreen = null;
                var response = await authRepo.SaveNewMpin(data);
                if (response is string)
                {
                    if (AuthLabelsPortal.isErrorFunction(response))
                    {
                        Console.WriteLine("Redirecting to AuthScreen...");
                        ViewBag.errorAuthScreen = response;
                        return Json("");
                    }
                    else
                    {
                        ViewBag.errorAuthScreen = AuthLabelsPortal.AnyError;
                        return Json("");
                    }
                }
                else
                {
                    return Json(new
                    {
                        statusVal = response.status?.ToString().Trim().ToLower() ?? "",
                        message = response.message,
                        token = AuthLabelsPortal.jwtToken
                    });

                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorAuthScreen = e.Message;
                return Json(new
                {
                    statusVal = "NotOK", // Assuming this is in your response
                    message = e.Message,
                    token = AuthLabelsPortal.jwtToken
                });
            }
        }
        //public async Task<JsonResult> RefreshToken([FromBody] RefreshTokenParameter data)
        public async Task<JsonResult> RefreshToken()
        {
            try
            {
                ViewBag.errorAuthScreen = null;
                var data = new
                {
                    mobile = "+971566778893"
                };
                var response = await authRepo.RefreshToken(data);
                if (response is String)
                {
                    if (AuthLabelsPortal.isErrorFunction(response))
                    {
                        Console.WriteLine("Redirecting to AuthScreen...");
                        ViewBag.errorAuthScreen = response;
                        return Json("");
                    }
                    else
                    {
                        ViewBag.errorAuthScreen = AuthLabelsPortal.AnyError;
                        return Json("");
                    }
                }
                else
                {
                    AuthLabelsPortal.jwtToken = response.token;
                    AuthLabelsPortal.userNameEng = response.user.name;
                    AuthLabelsPortal.userNameAR = response.user.fullNameAr;
                    AuthLabelsPortal.mobile = response.user.mobile;
                    AuthLabelsPortal.email = response.user.email;

                    HttpContext.Session.SetString("JWTToken", $"{response.token.ToString()}");
                    HttpContext.Session.SetString("CompanyName", $"UBSPropt");
                    HttpContext.Session.SetString("userNameEng", $"{response.user.name.ToString()}");
                    HttpContext.Session.SetString("userNameAR", $"{response.user.fullNameAr.ToString()}");
                    HttpContext.Session.SetString("mobile", $"{response.user.mobile.ToString()}");
                    HttpContext.Session.SetString("email", $"{response.user.email.ToString()}");
                    HttpContext.Session.SetString("userId", $"{response.user.userId.ToString()}");
                  
                    var token = HttpContext.Session.GetString("JWTToken");
                    AuthLabelsPortal.jwtToken = token ?? "";
                    ViewBag.refreshTokenData = response;
                    var jsonResponse = new
                    {
                        statusCode = response.statusCode, // Assuming this is in your response
                        user = response.user,
                        token = response.token
                    };
                    return Json(jsonResponse);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorAuthScreen = e.Message;
                return Json(new
                {
                    statusCode = "", // Assuming this is in your response
                    user = "",
                    token = ""
                });
            }
        }
        public async Task<JsonResult> GetUserRoles([FromBody] dynamic data1)
        {
            try
            {
                ViewBag.errorAuthScreen = null;
                var data11 = new { };
                var response = await authRepo.GetUserRoles(data11);
                if (response is String)
                {
                    if (AuthLabelsPortal.isErrorFunction(response))
                    {
                        Console.WriteLine("Redirecting to AuthScreen...");
                        ViewBag.errorAuthScreen = response;
                        return Json("");
                    }
                    else
                    {
                        ViewBag.errorAuthScreen = AuthLabelsPortal.AnyError;
                        return Json("");
                    }
                }
                else
                {
                    ViewBag.getUserRolesData = response.data;
                    List<Role> roles = new List<Role>();

                    for (int i = 0; i < response.data.Count; i++)
                    {
                        var icon = "";
                        var id = Convert.ToInt32(response.data[i]["roleId"]);
                        if (id == 1)
                        {
                            icon = "fa-home";
                        }
                        else if (id == 3)
                        {
                            icon = "fa-briefcase";
                        }
                        else if (id == 2)
                        {
                            icon = "fa-building";
                        }
                        else
                        {
                            icon = "fa-globe";
                        }

                        //Role r = new Role
                        //{
                        //    RoleId = Convert.ToInt32(response.data[i]["roleId"]),
                        //    RoleName = response.data[i]["role"].ToString(),
                        //    RoleAR = response.data[i]["roleAR"].ToString(),
                        //    UserType = response.data[i]["userType"]?.ToString(),
                        //    RoleIcon = icon
                        //};

                        if (id == 1)
                        {
                            Role r = new Role
                            {
                                RoleId = Convert.ToInt32(response.data[i]["roleId"]),
                                RoleName = response.data[i]["role"].ToString(),
                                RoleAR = response.data[i]["roleAR"].ToString(),
                                UserType = response.data[i]["userType"]?.ToString(),
                                RoleIcon = icon
                            };

                            roles.Add(r);
                        }
                        else
                        {
                        }


                        //roles.Add(r);
                    }

                    var name = HttpContext.Session.GetString("userNameEng");
                    var jsonResponse = new
                    {
                        statustCode = response.statustCode, // Assuming this is in your response
                        status = response.status,
                        data = roles,
                        userName = name,
                    };


                    return Json(jsonResponse);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorAuthScreen = e.Message;
                return Json(new
                {
                    statustCode = "", // Assuming this is in your response
                    status = "",
                    data = ""
                });
            }
        }


        public async Task<JsonResult> ProceedToLogin([FromBody] ProceedToLoginParameter data)
        {
            try
            {
                Console.WriteLine(data);
                ViewBag.errorAuthScreen = null;
                var response = await authRepo.ProccedToLogin(data);
                if (response is String)
                {
                    if (AuthLabelsPortal.isErrorFunction(response))
                    {
                        Console.WriteLine("Redirecting to AuthScreen...");
                        ViewBag.errorAuthScreen = response;
                        return Json("");
                    }
                    else
                    {
                        ViewBag.errorAuthScreen = AuthLabelsPortal.AnyError;
                        return Json("");
                    }
                }
                else
                {
                    ViewBag.getUserRolesData = response.data;
                    Console.WriteLine($"response:::::::{response.statusCode}");
                    Console.WriteLine($"response:::::::1 {ViewBag.getUserRolesData}");
                    var jsonResponse = new
                    {
                        statustCode = response.statustCode, // Assuming this is in your response
                        status = response.status,
                        data = response.data
                    };
                    Console.WriteLine($"jsonResponse:::::::{jsonResponse.statustCode}");
                    return Json(jsonResponse);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorAuthScreen = e.Message;
                return Json(new
                {
                    statustCode = "", // Assuming this is in your response
                    status = "",
                    data = ""
                });
            }
        }

        public async Task<JsonResult> ValidateRoleByMpin([FromBody] ValidateRoleByMPinParameter data)
        {
            try
            {
                ViewBag.errorAuthScreen = null;
                var response = await authRepo.ValidateRoleByMPin(data);
                if (response is String)
                {
                    if (AuthLabelsPortal.isErrorFunction(response))
                    {
                        Console.WriteLine("Redirecting to AuthScreen...");
                        ViewBag.errorAuthScreen = response;
                        return Json("");
                    }
                    else
                    {
                        ViewBag.errorAuthScreen = AuthLabelsPortal.AnyError;
                        return Json("");
                    }
                }
                else
                {
                    var jsonResponse = new
                    {
                        statustCode = response.statustCode, // Assuming this is in your response
                        status = response.status,
                        data = response.data
                    };

                    HttpContext.Session.SetString("JWTToken", $"{response.token.ToString()}");
                    var token = HttpContext.Session.GetString("JWTToken");
                    Console.WriteLine($"jsonResponse:::::::{token}");
                    AuthLabelsPortal.jwtToken = token ?? "";
                    Console.WriteLine($"jsonResponse:::::::{jsonResponse.statustCode}");
                    return Json(jsonResponse);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorAuthScreen = e.Message;
                return Json(new
                {
                    statustCode = "", // Assuming this is in your response
                    status = "",
                    data = ""
                });
            }
        }

        public IActionResult LoginWithJWT(string tokenParam,string companyName,string userNameEng,string email)
        {
            if (string.IsNullOrEmpty(tokenParam))
            {
                return RedirectToAction("AuthScreen", "Auth", new { area = "Users" });
            }

            // Store JWT in session
            HttpContext.Session.SetString("isLoginFromAuth", "True");
            HttpContext.Session.SetString("JWTToken", tokenParam);
            HttpContext.Session.SetString("CompanyName", companyName);
            HttpContext.Session.SetString("userNameEng", userNameEng);
            HttpContext.Session.SetString("email", email);

            var token = HttpContext.Session.GetString("JWTToken");
            Console.WriteLine($"jsonResponse:::::::{token}");

            AuthLabelsPortal.jwtToken = token ?? "";

            // ✅ Proper server-side redirect
            return RedirectToAction(
                "MainDashboard",
                "TenantDashboard",
                new { area = "Dashboard" }
            );
        }


        // Functions By Email
        public async Task<JsonResult> validateUserByEmail([FromBody] ValidateEmail data)
        {
            try
            {
                ViewBag.errorAuthScreen = null;
                var response = await authRepo.validateUserByEmail(data);
                if (response is String)
                {
                    if (AuthLabelsPortal.isErrorFunction(response))
                    {
                        Console.WriteLine("Redirecting to AuthScreen...");
                        ViewBag.errorAuthScreen = response;
                        return Json("");
                    }
                    else
                    {
                        ViewBag.errorAuthScreen = AuthLabelsPortal.AnyError;
                        return Json("");
                    }
                }
                else
                {
                    var jsonResponse = new
                    {
                        status = response.status.ToString(), // Assuming this is in your response
                        message = response.message.ToString(),
                        statusCode = response.statusCode.ToString(),
                        isPasswordSet = response.isPasswordSet.ToString()
                    };
                    Console.WriteLine($"jsonResponse:::::::{jsonResponse.statusCode}");
                    return Json(jsonResponse);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorAuthScreen = e.Message;
                return Json(new
                {
                    status = "", // Assuming this is in your response
                    message = "",
                    statusCode = ""
                });
            }
        }
        public async Task<JsonResult> verifyOTPByEmail([FromBody] VerifyOTPByEmail data)
        {
            try
            {
                ViewBag.errorAuthScreen = null;
                var response = await authRepo.verifyOTPByEmail(data);
                if (response is String)
                {
                    if (AuthLabelsPortal.isErrorFunction(response))
                    {
                        Console.WriteLine("Redirecting to AuthScreen...");
                        ViewBag.errorAuthScreen = response;
                        return Json("");
                    }
                    else
                    {
                        ViewBag.errorAuthScreen = AuthLabelsPortal.AnyError;
                        return Json("");
                    }
                }
                else
                {
                    ViewBag.generateFirbaseUserData = response;
                    var jsonResponse = new
                    {
                        statusCode = response.statusCode.ToString(), // Assuming this is in your response
                        message = response.message.ToString(), // Assuming this is in your response
                        user = response.user,
                        token = response.token.ToString()
                    };
                    Console.WriteLine($"jsonResponse:::::::{jsonResponse.statusCode}");
                    return Json(jsonResponse);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorAuthScreen = e.Message;
                return Json(new
                {
                    statusCode = "", // Assuming this is in your response
                    user = "",
                    token = ""
                });
            }
        }
        public async Task<JsonResult> savePasswordByEmail([FromBody] SavePasswordByEmail data)
        {
            try
            {
                ViewBag.errorAuthScreen = null;
                var response = await authRepo.savePasswordByEmail(data);
                if (response is String)
                {
                    if (AuthLabelsPortal.isErrorFunction(response))
                    {
                        Console.WriteLine("Redirecting to AuthScreen...");
                        ViewBag.errorAuthScreen = response;
                        return Json("");
                    }
                    else
                    {
                        ViewBag.errorAuthScreen = AuthLabelsPortal.AnyError;
                        return Json("");
                    }
                }
                else
                {
                    var jsonResponse = new
                    {
                        statusCode = response.statusCode.ToString(), // Assuming this is in your response
                        status = response.status.ToString(),
                        message = response.message.ToString()
                    };
                    Console.WriteLine($"jsonResponse:::::::{jsonResponse.message}");
                    return Json(jsonResponse);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorAuthScreen = e.Message;
                return Json(new
                {
                    statusCode = "", // Assuming this is in your response
                    user = "",
                    token = ""
                });
            }
        }
        public async Task<JsonResult> validatePasswordByEmail([FromBody] ValidatePasswordByEmail data)
        {
            try
            {
                ViewBag.errorAuthScreen = null;
                var response = await authRepo.validatePasswordByEmail(data);
                if (response is String)
                {
                    if (AuthLabelsPortal.isErrorFunction(response))
                    {
                        Console.WriteLine("Redirecting to AuthScreen...");
                        ViewBag.errorAuthScreen = response;
                        return Json("");
                    }
                    else
                    {
                        ViewBag.errorAuthScreen = AuthLabelsPortal.AnyError;
                        return Json("");
                    }
                }
                else
                {
                    ViewBag.generateFirbaseUserData = response;
                    var jsonResponse = new
                    {
                        statusCode = response.statusCode.ToString(), // Assuming this is in your response
                        user = response.user,
                        token = response.token.ToString(),
                        message = response.message.ToString()
                    };
                    HttpContext.Session.SetString("JWTToken", $"{response.token.ToString()}");
                    HttpContext.Session.SetString("LandlordID", $"{response.landLordId.ToString()}");
                    HttpContext.Session.SetString("CompanyName", $"UBSPropt");
                    var token = HttpContext.Session.GetString("JWTToken");
                    var landlordID = HttpContext.Session.GetString("LandlordID");
                    Console.WriteLine($"jsonResponse:::::::{token}");
                    Console.WriteLine($"jsonResponse:::::::{landlordID}");
                    AuthLabelsPortal.jwtToken = token ?? "";
                    return Json(jsonResponse);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorAuthScreen = e.Message;
                return Json(new
                {
                    statusCode = "", // Assuming this is in your response
                    user = "",
                    token = ""
                });
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class VerifyOtpRequest
    {
        public string? VerificationId
        { get; set; }
        public string? Code { get; set; } // This is the OTP entered by the user
    }


    public class Role
    {
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? RoleAR { get; set; }
        public string? UserType { get; set; }  // optional
        public string? RoleIcon { get; set; }
    }
}