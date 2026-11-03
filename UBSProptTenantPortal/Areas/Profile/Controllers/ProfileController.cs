using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using UBSProptTenantPortal.Models;

namespace UBSProptTenantPortal.Areas.Profile.Controllers
{
    [Area("Profile")]
    public class ProfileController : BaseController
    {
        private readonly ILogger<ProfileController> _logger;

        private readonly UBSTenantApiConfig apiConfig;
        private readonly ProfileRepository profileRepo;
        new string? token = "";
        public ProfileController(ILogger<ProfileController> logger, ProfileRepository _profileRepo, IOptions<UBSTenantApiConfig> _apiConfig)
        {
            _logger = logger;
            profileRepo = _profileRepo;
            apiConfig = _apiConfig.Value;
            token = AuthLabelsPortal.jwtToken ?? "";
        }

        public async Task<dynamic> Profile()
        {
            try
            {
                var data = new { };
                ViewBag.erroProfile = "";
                var postUrl = apiConfig.getTenantProfile;
                var response = await profileRepo.GetProfile(data, postUrl,token??"");
                if (response is String)
                {
                    ViewBag.erroProfile = response;
                    Console.WriteLine($"::::::::::::::::::: GetProfile Else {ViewBag.erroProfile}");
                    ProfileViewModelResponse viewModelEmpty = new ProfileViewModelResponse
                    {
                        StatusCode = -1,
                        Data = null,
                        Message = ""
                    };
                    return View(viewModelEmpty);

                }
                // Convert JArray to List<DataModel>
                var result = JsonConvert.DeserializeObject<TenantProfileApiResponse>(response.ToString());

                var viewModel = new ProfileViewModelResponse
                {
                    StatusCode = int.Parse(result.statuCode), // if it's a string
                    Data = new List<ProfileModel> { result.profile }, // wrap in a list if needed
                    Message = result.message
                };
                //await CanEditProfile();
                return View(viewModel);
            }
            catch (UnauthorizedAccessException)
            {
                ViewBag.erroProfile = LabelsLandlordPortal.Unauthorized;
                // Navigate to the login screen
                Console.WriteLine("Navigating to the login screen....................");
                return View();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.erroProfile = e.Message;
                ProfileViewModelResponse viewModel = new ProfileViewModelResponse
                {

                };
                return View(viewModel);
            }
        }
        public async Task<dynamic> CanEditProfile()
        {
            try
            {
                var data = new { };

                var postUrl = apiConfig.canEditTenantProfile;
                var response = await profileRepo.CanEditProfile(data,postUrl,token??"");
                ViewBag.canEditProfile = response.message;
                Console.WriteLine(ViewBag.canEditProfile);
                return View();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return View();
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}

