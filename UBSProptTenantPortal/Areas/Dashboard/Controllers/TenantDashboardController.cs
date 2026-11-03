using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UBSProptRepository.Repository;
namespace UBSProptTenantPortal.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class TenantDashboardController : BaseController
    {
        private readonly DashboardRepo dahboardRepo;
        private readonly UBSTenantApiConfig apiConfig;
        private readonly ContractRepository contractRepo;
        private readonly ServiceRequestRepository serviceReqRepo;
        private readonly PaymentsRepository paymentRepo;
        string? tokenN = "";
        public TenantDashboardController(DashboardRepo _dahboardRepo, PaymentsRepository _paymentRepo,ContractRepository _contractRepo, ServiceRequestRepository _serviceReqRepo,  IOptions<UBSTenantApiConfig> _apiConfig)
        {
            dahboardRepo = _dahboardRepo;
            contractRepo = _contractRepo;
            serviceReqRepo = _serviceReqRepo;
            paymentRepo = _paymentRepo;
            apiConfig = _apiConfig.Value;
            tokenN = AuthLabelsPortal.jwtToken ?? "";
        }


        [HttpGet]
        public async Task<IActionResult> MainDashboardAsync()
        {

            var showPopUp = HttpContext.Session.GetString("ShowPopUp");
            if (string.IsNullOrEmpty(showPopUp))
            {
                ViewBag.ShowPopUp = true;
                HttpContext.Session.SetString("ShowPopUp", "true");
            }
            else
            {
                ViewBag.ShowPopUp = false;
            }

            DashboardViewModel viewModel = new DashboardViewModel();
            try { viewModel.PopUp = await GetDashboardPopUp();

            }
            catch (Exception ex)
            {
                ViewBag.errorGetPopUp = ex.Message;
                Console.WriteLine($"Error retrieving dashboard data: {ex.Message}");
            }

            try { viewModel.GetData = await GetDashboardDataAsync(); }
            catch (Exception ex) {
                ViewBag.GetDashboardData = ex.Message;
                Console.WriteLine($"Error retrieving dashboard data: {ex.Message}"); }

            try { viewModel.ServiceRequest = await GetServiceRequest();

               
            }
            catch (Exception ex) {
                ViewBag.errorGetServiceRequest = ex.Message;
                Console.WriteLine($"Error retrieving properties: {ex.Message}"); }

            try { viewModel.Contracts = await GetContracts(); }
            catch (Exception ex) {
                ViewBag.errorGetContracts = ex.Message;
                Console.WriteLine($"Error retrieving Contracts: {ex.Message}"); }

            try { viewModel.Payments = await Payments(); }
            catch (Exception ex) {
                ViewBag.errorGetPayments = ex.Message;
                Console.WriteLine($"Error retrieving Invoices: {ex.Message}"); }

            return View(viewModel);
        }

        public async Task<IActionResult> SingleFucntionCallAsync([FromBody] DashboardFunctionCalling type)
        {
            DashboardViewModel viewModel = new DashboardViewModel();

            if (type.getDataFunc == true)
            {
                try { viewModel.GetData = await GetDashboardDataAsync(); }
                catch (Exception ex) {
                    ViewBag.GetDashboardData = ex.Message;
                    Console.WriteLine($"Error retrieving dashboard data: {ex.Message}"); }
            }

            if (type.getContractsFunc == true)
            {
                try { viewModel.Contracts = await GetContracts(); }
                catch (Exception ex) {
                    ViewBag.errorGetContracts = ex.Message;
                    Console.WriteLine($"Error retrieving contracts: {ex.Message}"); }
            }

            if (type.getServiceRequestFunc == true)
            {
                try { viewModel.ServiceRequest = await GetServiceRequest(); }
                catch (Exception ex) {
                    ViewBag.errorGetServiceRequest = ex.Message;
                    Console.WriteLine($"Error retrieving service requests: {ex.Message}"); }
            }
            else {
                try { viewModel.Payments = await Payments(); }
                catch (Exception ex) {
                    ViewBag.errorGetPayments = ex.Message;
                    Console.WriteLine($"Error retrieving Invoices: {ex.Message}");
                    }
            }

            return View(viewModel);
        }

        public async Task<NotificationPopUpModel> GetDashboardPopUp() //GetDataViewModel
        {
            try
            {
                ViewBag.errorGetPopUp = "";
                var data = new { };
                var postUrl = apiConfig.tenantDashboardGetPopUp;
                var response = await dahboardRepo.GetDashboardPopUp(data, postUrl, tokenN ?? "");
                if (response is string)
                {
                    ViewBag.errorGetPopUp = response;
                    NotificationPopUpModel model1 = new NotificationPopUpModel
                    {
                        Status = "",
                        StatusCode = "",
                        Message = "",
                        Notifications =null
                    };
                    return model1;
                }
                Console.WriteLine(response);
                NotificationPopUpModel apiResponse = JsonConvert.DeserializeObject<NotificationPopUpModel>(response.ToString());
                NotificationPopUpModel model = new NotificationPopUpModel
                {
                    Status = apiResponse.Status,
                    StatusCode = apiResponse.StatusCode,
                    Message = apiResponse.Status,
                    Notifications = apiResponse.Notifications
                };
                return model;
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorGetPopUp = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                return new NotificationPopUpModel();
            }
        }

        public async Task<GetDataRepose> GetDashboardDataAsync() //GetDataViewModel
        {



            try
            {
                ViewBag.errorGetData = "";
                var data = new  { };
                var postUrl = apiConfig.tenantDashboardGetData;
                var response = await dahboardRepo.GetData(data, postUrl, tokenN ?? "");
                if (response is string)
                {
                    ViewBag.errorGetData = response;
                    GetDataRepose model1 = new GetDataRepose
                    {
                        Status = "",
                        Dashboard = null,
                        StatusCode = "",
                        Message = "",
                        UnreadNotifications = 0,
                        ContractRenewalAction = 0,
                    };
                    return model1;
                }
                Console.WriteLine(response);
                GetDataRepose apiResponse = JsonConvert.DeserializeObject<GetDataRepose>(response.ToString());
                // Use Arabic culture info for formatting (e.g., ar-SA for Saudi Arabia)
                var arabicCulture = new CultureInfo("ar-SA");
                // Rental Value
                decimal rentalVal = apiResponse.Dashboard?.RentalVal ?? 0m;
                string formattedRentalVal = rentalVal.ToString("#,##0.00", arabicCulture);
                string formattedtoBePaidCurrency = rentalVal.ToString("#,##0.00", arabicCulture);

                // RentOutStanding
                decimal rentOutstanding = apiResponse.Dashboard?.RentOutstanding ?? 0m;
                string formattedRentOutstanding = rentOutstanding.ToString("#,##0.00", arabicCulture);
                string formattedBalanceCurrency = rentOutstanding.ToString("#,##0.00", arabicCulture);

                // Payment Balance
                decimal paymentBalance = apiResponse.Dashboard?.PaymentBalance ?? 0m;
                string formattedPaymentBalance = paymentBalance.ToString("#,##0.00", arabicCulture);


                GetDataRepose model = new GetDataRepose
                {
                    Status = apiResponse.Status,
                    Dashboard = apiResponse.Dashboard,
                    StatusCode = apiResponse.StatusCode,
                    Message = apiResponse.Status,
                    UnreadNotifications = apiResponse.UnreadNotifications,
                    ContractRenewalAction = apiResponse.ContractRenewalAction,
                    contractExpiringIn30Days = apiResponse.Dashboard?.ContractExpiringIn30Days.ToString(),
                    rentalVal = rentalVal,
                    rentOutstanding = rentOutstanding,
                    paymentCurrency = formattedPaymentBalance,
                    toBePaidCurrency = formattedtoBePaidCurrency,
                    balanceCurrency = formattedBalanceCurrency,
                };
                HttpContext.Session.SetString("unReadNotification", $"{model.UnreadNotifications.ToString()}");
                return model;
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorGetData = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                return new GetDataRepose();
            }
        }

        public async Task<ContractResponse> GetContracts()
        {
            try
            {
                ViewBag.errorGetContracts = "";
                var data = new GetContractsParameter
                {
                    page = 1.ToString(),
                    pageSize = 20.ToString(),
                    search = "",
                };
                var postUrl = apiConfig.getContracts;
                var response = await contractRepo.GetContracts(data, postUrl, tokenN ?? "");
                if (response is string)
                {
                    ViewBag.errorGetContracts = response;
                    ContractResponse model1 = new ContractResponse
                    {
                        Status = "",
                        data = null,
                        Message = ""
                    };
                    return model1;
                }
                Console.WriteLine(response);
                // Convert JArray to List
                List<Contract> dataList = response.contracts.ToObject<List<Contract>>();
                if (dataList.Count == 0) {
                    ViewBag.errorGetContracts = "No Data Dound";
                }
                ContractResponse model = new ContractResponse
                {
                    Status = response.status,
                    data = dataList,
                    Message = response.message
                };
                return model;
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorGetContracts = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                return new ContractResponse();
            }
        }

        public async Task<ServiceRequestsResponse> GetServiceRequest()
        {
            try
            {
                ViewBag.errorGetServiceRequest = "";
                var dataFM = new GetServiceRequestParameterN
                {
                    type = "FM",
                    fromDate = "",
                    toDate = "",
                    search = ""
                };
                var postUrlFM = apiConfig.getServiceRequests;
                var responseFM = await serviceReqRepo.GetServiceRequests(dataFM, postUrlFM, tokenN ?? "");
                ServiceRequestsResponse apiResponseFM = JsonConvert.DeserializeObject<ServiceRequestsResponse>(responseFM.ToString());
                List<ServiceRequest> dataListFM = apiResponseFM.ServiceRequests ?? new List<ServiceRequest>();
                if (dataListFM.Count == 0)
                {
                    var dataPM = new GetServiceRequestParameterN
                    {
                        type = "PM",
                        fromDate = "",
                        toDate = "",
                        search = ""
                    };
                    var postUrlPM = apiConfig.getServiceRequests;
                    var responsePM = await serviceReqRepo.GetServiceRequests(dataPM, postUrlPM, tokenN ?? "");
                    ServiceRequestsResponse apiResponsePM = JsonConvert.DeserializeObject<ServiceRequestsResponse>(responsePM.ToString());
                    List<ServiceRequest> dataListPM = apiResponsePM.ServiceRequests ?? new List<ServiceRequest>();
                    if (responsePM is string)
                    {
                        ViewBag.errorGetServiceRequest = responsePM;
                        ServiceRequestsResponse model1 = new ServiceRequestsResponse
                        {
                            Status = "",
                            ServiceRequests = null,
                            Message = ""
                        };
                        return model1;
                    }
                    if (dataListPM.Count == 0) {
                        ViewBag.errorGetServiceRequest = "No Data Found";
                    }
                        Console.WriteLine(responsePM);

                    for (int i = 0; i < apiResponsePM?.ServiceRequests?.Count; i++)
                    {
                        apiResponsePM.ServiceRequests[i].tabName = "PM";
                    }
                    var model = new ServiceRequestsResponse
                    {
                        Status = apiResponsePM?.Status,
                        ServiceRequests = apiResponsePM?.ServiceRequests ?? new List<ServiceRequest>(),
                        Message = apiResponsePM?.Message,
                    };
                    Console.WriteLine(model.Status);
                    Console.WriteLine(model.Message);
                    Console.WriteLine(model.ServiceRequests);
                    return model;
                }
                else {
                 if (responseFM is string)
                    {
                        ViewBag.errorGetServiceRequest = responseFM;
                        ServiceRequestsResponse model1 = new ServiceRequestsResponse
                        {
                            Status = "",
                            ServiceRequests = null,
                            Message = ""
                        };
                        return model1;
                    }
                    if (dataListFM.Count == 0)
                    {
                        ViewBag.errorGetServiceRequest = "No Data Found";
                    }
                    Console.WriteLine(responseFM);
                    for (int i = 0; i < apiResponseFM?.ServiceRequests?.Count; i++)
                    {
                        apiResponseFM.ServiceRequests[i].tabName = "FM";
                    }
                    var model = new ServiceRequestsResponse
                    {
                        Status = apiResponseFM?.Status,
                        ServiceRequests = apiResponseFM?.ServiceRequests ?? new List<ServiceRequest>(),
                        Message = apiResponseFM?.Message,
                    };
                    Console.WriteLine(model.Status);
                    Console.WriteLine(model.Message);
                    Console.WriteLine(model.ServiceRequests);
                    return model;
                }
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorGetServiceRequest = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                return new ServiceRequestsResponse();
            }
        }

        public async Task<PaymentResponse> Payments()
        {
            try
            {
                ViewBag.errorGetPayments = "";

                var data = new GetPaymentParameter
                {
                    Page = "1",
                    PageSize = "20",
                    SearchParam = ""
                };

                var postUrl = apiConfig.paymentsGet;
                var response = await paymentRepo.GetPayments(data, postUrl, tokenN ?? "");

                if (response is string errorMessage)
                {
                    ViewBag.errorGetPayments = errorMessage;
                    var errorModel = new PaymentResponse
                    {
                        Status = "Error",
                        Payments = null,
                        Message = errorMessage
                    };
                    return errorModel; // Or return View(errorModel) if using Razor
                }

                // Assuming response is a JObject or strongly-typed object
                var paymentsArray = response["payments"]; // JSON field should match the API
                List<PaymentDetail> paymentList = paymentsArray.ToObject<List<PaymentDetail>>();
                if (paymentList.Count == 0)
                {
                    ViewBag.errorGetPayments = "No Data Dound";
                }
                var model = new PaymentResponse
                {
                    Status = response["status"]?.ToString(),
                    Payments = paymentList,
                    Message = response["message"]?.ToString()
                };
                return model;
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorGetPayments = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                var errorModel = new PaymentResponse
                {
                    Status = "Exception",
                    Payments = null,
                    Message = e.Message
                };
                return errorModel; // Or return View(errorModel)
            }
        }
    }
}
