using System;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UBSProptTenantPortal.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using static Google.Apis.Requests.BatchRequest;

namespace UBSProptTenantPortal.Areas.Faqs.Controllers
{
    [Area("Faqs")]
    public class FaqController : BaseController
    {
        private readonly ILogger<FaqController> _logger;
        private readonly FaqsRepository faqsRepo;
        private readonly UBSTenantApiConfig apiConfig;
        new string? token = "";
        public FaqController(ILogger<FaqController> logger, FaqsRepository _faqsRepo, IOptions<UBSTenantApiConfig> _apiConfig)
        {
            _logger = logger;
            faqsRepo = _faqsRepo;
            apiConfig = _apiConfig.Value;
            token = AuthLabelsPortal.jwtToken ?? "";
        }


        public async Task<IActionResult> Faqs()
        {
            try
            {
                ViewBag.errorGetFaqs = "";
                var postUrl = apiConfig.GetFaqs;
                var data = new { }; // empty object for POST body
                var response = await faqsRepo.GetFaqCategories(data, postUrl, token??"");

                if (response is string)
                {
                    ViewBag.errorGetFaqs = response;
                    FaqsViewModelResponse model1 = new FaqsViewModelResponse();
                    return View(model1);
                }
                // Safely convert response data to list
                // Deserialize into the new response DTO
                FaqApiResponse apiResponse = JsonConvert.DeserializeObject<FaqApiResponse>(response.ToString());
                var dataList = apiResponse.FaqCategories ?? new List<QuestionData>();
                if (dataList.Count == 0) { ViewBag.errorGetFaqs = "No Data Found"; }
                var viewModel = new FaqsViewModelResponse
                {
                    StatusCode = 200,
                    Data = apiResponse.FaqCategories ?? new List<QuestionData>(),
                    Message = apiResponse.Message,
                };

                return View(viewModel);
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorGetFaqs = e.Message;
                FaqsViewModelResponse model1 = new FaqsViewModelResponse();
                return View(model1);
            }
        }

        public async Task<IActionResult> FaqsDetail(string CategoryId)
        {
            try
            {
                ViewBag.errorGetFaqsDetail = "";
                var postUrl = apiConfig.GetFaqsCategories;
                var data = new GetFaqsDetailparameter { CategoryId = CategoryId };
                Console.WriteLine($"CategoryId: {CategoryId}");
                var response = await faqsRepo.GetFaqsDetail(data, postUrl,token??"");
                if (response is string)
                {
                    ViewBag.errorGetFaqsDetail = response;
                    GetFaqsDetailparameter model1 = new GetFaqsDetailparameter();
                    return View(model1);
                }
                // Convert JArray to List<DataModel>
                var apiResponse = JsonConvert.DeserializeObject<FaqDetailApiResponse>(response.ToString());

                var viewModel = new FaqsDetailViewModelResponse
                {
                    StatusCode = 200,
                    Data = apiResponse?.Faq ?? new List<FaqData>(),
                    Message = apiResponse?.Message
                };
                return View(viewModel);
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorGetFaqsDetail = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                GetFaqsDetailparameter viewModel = new GetFaqsDetailparameter
                {

                };
                return View(viewModel);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}

