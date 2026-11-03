using System;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UBSProptTenantPortal.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using UBSProptRepository.Repository;
using System.Reflection;
using static Google.Apis.Requests.BatchRequest;
using System.Globalization;
using System.Net;
using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using UBSProptSharedModel.Constant;
using System.Security.Cryptography.Xml;
using FirebaseAdmin.Messaging;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace UBSProptTenantPortal.Areas.RenewalFlow.Controllers
{
    [Area("RenewalFlow")]
    public class RenewalController : BaseController
    {
        private readonly ILogger<RenewalController> _logger;
        private readonly RenewalFlowRepo renewalRepo;
        private readonly UBSTenantApiConfig apiConfig;
        new string? token = "";
        public RenewalController(ILogger<RenewalController> logger, RenewalFlowRepo _renewalRepo, IOptions<UBSTenantApiConfig> _apiConfig)
        {
            _logger = logger;
            renewalRepo = _renewalRepo;
            apiConfig = _apiConfig.Value;
            token = AuthLabelsPortal.jwtToken ?? "";
        }

        #region Contract Flow

        public async Task<IActionResult> ContractAction(string tab)
        {
            ContractRenwalViewModelDemo viewModel = new ContractRenwalViewModelDemo();
            ViewBag.TabName = tab;
            try
            {
                viewModel.renwalContract = await GetRenewalContracts();
                for (int i = 0; i < viewModel?.renwalContract!.Contracts?.Count; i++)
                {
                    viewModel!.renwalContract.Contracts[i].tabName = "RCF";
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Error retrieving Renwal Contract data: {ex.Message}");
            }

            try
            {
                viewModel!.newContract = await GetNewContracts();

                for (int i = 0; i < viewModel?.newContract?.Contracts?.Count; i++)
                {
                    viewModel!.newContract.Contracts[i].tabName = "NCF";
                }
            }
            catch (Exception ex) { Console.WriteLine($"Error retrieving New Contract data: {ex.Message}"); }

            return View(viewModel);
        }
        public async Task<ContractWithDueActionResponse> GetRenewalContracts()
        {
            try
            {
                
                ViewBag.errorRenewalContract = "";
                var postUrl = apiConfig.contractRenewalActions;
                var data = new { }; // empty object for POST body
                var response = await renewalRepo.GetRenewalContracts(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorRenewalContract = response;
                    ContractWithDueActionResponse model1 = new ContractWithDueActionResponse
                    {
                        Status = "",
                        StatusCode = "",
                        Contracts = null
                    };
                    return model1;
                }
                ContractWithDueActionResponse apiResponse = JsonConvert.DeserializeObject<ContractWithDueActionResponse>(response.ToString());
                List<ContractWithDueAction> dataList = response.record.ToObject<List<ContractWithDueAction>>();
                if (dataList.Count == 0)
                {
                    ViewBag.errorRenewalContract = "No contract found for Renewal";
                }

                //for (int i = 0; i < dataList.Count; i++)
                //{
                //    dataList[i].StageId = 6;
                //}

                ContractWithDueActionResponse model = new ContractWithDueActionResponse
                {
                    Status = apiResponse.Status,
                    StatusCode = apiResponse.StatusCode,
                    Contracts = dataList
                };
                Console.WriteLine(response);
                return model;
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorRenewalContract = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                ContractWithDueActionResponse model = new ContractWithDueActionResponse();
                return model;
            }
        }
        public async Task<ContractWithDueActionResponse> GetNewContracts()
        {
            try
            {
                
                ViewBag.errorRenewalContract = "";
                var postUrl = apiConfig.newContractActions;
                var data = new { }; // empty object for POST body
                var response = await renewalRepo.GetRenewalContracts(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorRenewalContract = response;
                    ContractWithDueActionResponse model1 = new ContractWithDueActionResponse
                    {
                        Status = "",
                        StatusCode = "",
                        Contracts = null
                    };
                    return model1;
                }
                ContractWithDueActionResponse apiResponse = JsonConvert.DeserializeObject<ContractWithDueActionResponse>(response.ToString());
                List<ContractWithDueAction> dataList = response.record.ToObject<List<ContractWithDueAction>>();
                if (dataList.Count == 0)
                {
                    ViewBag.errorRenewalContract = "No contract found for Renewal";
                }

                

                ContractWithDueActionResponse model = new ContractWithDueActionResponse
                {
                    Status = apiResponse.Status,
                    StatusCode = apiResponse.StatusCode,
                    Contracts = dataList
                };
                Console.WriteLine(response);
                return model;
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorRenewalContract = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                ContractWithDueActionResponse model = new ContractWithDueActionResponse();
                return model;
            }
        }

        #endregion

        #region Renwal Flow

        #region Main function
        public async Task<IActionResult> RenewalContracts() {
            ContractRenwalResponse contractRenwalResponse = new ContractRenwalResponse();
            contractRenwalResponse.ContractDueAction = await GetRenewalContracts();
            return View(contractRenwalResponse);
        }
        #endregion

        #region Get Contracts list
       #endregion

        #region Renewal,Vacat,Extend,Offerletter

        #region Renew
       
        public IActionResult ContractRenew()
        {
            ViewBag.errorContractRenew = "";
            var json = HttpContext.Session.GetString("ContractRenewModel");
            if (string.IsNullOrEmpty(json))
            {
                ViewBag.errorContractRenew = "No data found.";
                return View();
            }
            var model = JsonConvert.DeserializeObject<ContractRenewInfo>(json!);
            var contractNo = HttpContext.Session.GetString("ContractNo");
            var dueActionID = HttpContext.Session.GetString("DueActionID");
            var contractID = HttpContext.Session.GetString("ContractID");
            ViewBag.ContractID = contractID;
            ViewBag.ContractNo = contractNo;
            ViewBag.DueActionID = dueActionID;
            return View(model);
        }

        public async Task<IActionResult> ContractRenewFunction([FromBody] ContractRenewParam param )
        {
            try
            {
                ViewBag.errorContractRenew = "";
                var postUrl = apiConfig.getContractRenew;
                HttpContext.Session.SetString("ContractNo", param?.ContractNo ?? "");
                HttpContext.Session.SetString("DueActionID", param?.DueActionID ?? "");
                HttpContext.Session.SetString("ContractID", param?.ContractId ?? "");
                var data = new ContractDetailparameter
                {
                    ContractId = param?.ContractId,
                };
                var response = await renewalRepo.ContractRenew(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorContractRenew = response;
                    return Json(new
                    {
                        success = false,
                        redirectUrl = Url.Action("")
                    });
                }
                ContractRenewInfo apiResponse = JsonConvert.DeserializeObject<ContractRenewInfo>(response.ToString());
                ContractRenewInfo model = new ContractRenewInfo
                {
                    Status = apiResponse.Status,
                    Record = apiResponse.Record,
                    Link = apiResponse.Link
                };
                string jsonData = JsonConvert.SerializeObject(apiResponse);
                HttpContext.Session.SetString("ContractRenewModel", jsonData);
                var jsonRes = HttpContext.Session.GetString("ContractRenewModel");
                Console.WriteLine(jsonRes);
                var js = Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("ContractRenew") // or "ContractRenew"
                });
                return js;
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorContractRenew = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                return Json(new
                {
                    success = false,
                    redirectUrl = Url.Action("")
                });
            }
        }

        public async Task<IActionResult> SubmitContractRenewAsync([FromBody] ContractSubmitRenewExtendParam data)
        {
            try
            {
                ViewBag.errorContractRenew = "";
                var postUrl = apiConfig.addRenewalRequest;
                var data1 = data;
                var response = await renewalRepo.ContractSubmitRenew(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorContractRenew = response;
                    var jss = Json(new
                    {
                        success = false,
                        message = response,
                        caseno = 0
                    });
                    return jss;
                }
                var apiResult = JsonConvert.DeserializeObject<StageOneApiResponse>(response.ToString());
                if (apiResult.status?.ToLower().Trim() == "200" ||
                    apiResult.status?.ToLower().Trim() == "true" ||
                    apiResult.status?.ToLower().Trim() == "success" ||
                    apiResult.status?.ToLower().Trim() == "ok")
                {
                    var js = Json(new
                    {
                        success = true,
                        message = "Stage updated Successfully",
                        caseno = apiResult.addServiceRequest.caseNo
                    });
                    return js;
                }
                else {
                    var js = Json(new
                    {
                        success = false,
                        message = "Stage updated Failed",
                        caseno = 0
                    });
                    return js;
                }
                 
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorContractRenew = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                var js = Json(new
                {
                    success = false,
                    message = e.Message,
                    caseno = 0
                });
                return js;
            }
        }

        #endregion
        #region Extend
        public IActionResult ContractExtend()
        {
            ViewBag.errorContractExtend = "";
            var json = HttpContext.Session.GetString("ContractExtendModel");
            if (string.IsNullOrEmpty(json))
            {
                ViewBag.errorContractExtend = "No data found.";
                return View();
            }
            var model = JsonConvert.DeserializeObject<GetExtensionPeriodModel>(json!);
            var contractNo = HttpContext.Session.GetString("ContractNo");
            var dueActionID = HttpContext.Session.GetString("DueActionID");
            ViewBag.ContractNo = contractNo;
            ViewBag.DueActionID = dueActionID;
            return View(model);
        }
        public async Task<IActionResult> ContractExtendFunction([FromBody] ContractRenewParam param)
        {
            try
            {
                ViewBag.errorContractExtend = "";
                var postUrl = apiConfig.getExtensionPeriod;
                HttpContext.Session.SetString("ContractNo", param?.ContractNo ?? "");
                HttpContext.Session.SetString("DueActionID", param?.DueActionID ?? "");
                var data = new ContractDetailparameter
                {
                    ContractId = param?.ContractId,
                };
                var response = await renewalRepo.ContractExtend(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorContractExtend = response;
                    return Json(new
                    {
                        success = false,
                        redirectUrl = Url.Action("")
                    });
                }
                GetExtensionPeriodModel apiResponse = JsonConvert.DeserializeObject<GetExtensionPeriodModel>(response.ToString());
                List<ExtensionPeriod> dataList = response.extensionPeriod.ToObject<List<ExtensionPeriod>>();

                string jsonData = JsonConvert.SerializeObject(apiResponse);
                HttpContext.Session.SetString("ContractExtendModel", jsonData);
                var jsonRes = HttpContext.Session.GetString("ContractExtendModel");
                Console.WriteLine(jsonRes);
                GetExtensionPeriodModel model = new GetExtensionPeriodModel
                {
                    Status = apiResponse.Status,
                    ExtensionPeriod = dataList,
                };
                Console.WriteLine(response);
                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("ContractExtend")
                });
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorContractExtend = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                return Json(new
                {
                    success = false,
                    redirectUrl = Url.Action("")
                });
            }
        }

        public async Task<IActionResult> SubmitContractExtendAsync([FromBody] ContractSubmitRenewExtendParam data)
        {
            try
            {
                ViewBag.errorContractRenew = "";
                var postUrl = apiConfig.addExtensionRequest;
                var data1 = data;
                var response = await renewalRepo.ContractSubmitExtend(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorContractRenew = response;
                    var jss = Json(new
                    {
                        success = false,
                        message = "Something went wrong",
                        caseno = 0
                    });
                    return jss;
                }
                var apiResult = JsonConvert.DeserializeObject<StageOneApiResponse>(response.ToString());
                if (apiResult.status?.ToLower().Trim() == "200" ||
                    apiResult.status?.ToLower().Trim() == "true" ||
                    apiResult.status?.ToLower().Trim() == "success" ||
                    apiResult.status?.ToLower().Trim() == "ok")
                {
                    var js = Json(new
                    {
                        success = true,
                        message = "Stage updated Successfully",
                        caseno = apiResult.addServiceRequest.caseNo
                    });
                    return js;
                }
                else
                {
                    var js = Json(new
                    {
                        success = false,
                        message = "Stage updated Failed",
                        caseno = 0
                    });
                    return js;
                }
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorContractRenew = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                var js = Json(new
                {
                    success = false,
                    message = e.Message,
                    caseno = 0
                });
                return js;
            }
        }


        #endregion
        #region Vacat
        public IActionResult ContractVacate()
        {
            ViewBag.errorContractExtend = "";
            var json = HttpContext.Session.GetString("ContractVacateModel");
            if (string.IsNullOrEmpty(json))
            {
                ViewBag.errorContractExtend = "No data found.";
                return View();
            }
            var model = JsonConvert.DeserializeObject<GetVacatingReasons>(json!);
            var contractNo = HttpContext.Session.GetString("ContractNo");
            var ContractId = HttpContext.Session.GetString("ContractId");
            var dueActionID = HttpContext.Session.GetString("DueActionID");
            ViewBag.ContractNo = contractNo;
            ViewBag.ContractId = ContractId;
            ViewBag.DueActionID = dueActionID;
            return View(model);
        }
        public async Task<IActionResult> ContractVacateFunction([FromBody] ContractRenewParam param)
        {
            try
            {
                ViewBag.errorContractVacate = "";
                var postUrl = apiConfig.getVacatingList;
                HttpContext.Session.SetString("ContractNo", param?.ContractNo ?? "");
                HttpContext.Session.SetString("DueActionID", param?.DueActionID ?? "");
                HttpContext.Session.SetString("ContractId", param?.ContractId ?? "");
                var data = new ContractDetailparameter
                {
                    ContractId = param?.ContractId,
                };
                var response = await renewalRepo.ContractRenew(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorContractVacate = response;
                    return Json(new
                    {
                        success = false,
                        redirectUrl = Url.Action("")
                    });
                }
                GetVacatingReasons apiResponse = JsonConvert.DeserializeObject<GetVacatingReasons>(response.ToString());
                List<RecordTermin> dataList = response.record.ToObject<List<RecordTermin>>();


                string jsonData = JsonConvert.SerializeObject(apiResponse);
                HttpContext.Session.SetString("ContractVacateModel", jsonData);
                var jsonRes = HttpContext.Session.GetString("ContractVacateModel");
                Console.WriteLine(jsonRes);
                GetVacatingReasons model = new GetVacatingReasons
                {
                    Status = apiResponse.Status,
                    Record = dataList,
                    Note = apiResponse.Note
                };
                Console.WriteLine(response);
                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("ContractVacate")
                });
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorContractVacate = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                return Json(new
                {
                    success = false,
                    redirectUrl = Url.Action("")
                });
            }
        }

        public async Task<IActionResult> SubmitContractVacateAsync([FromBody] ContractSubmitVacateParam data)
        {
            try
            {
                ViewBag.errorContractRenew = "";
                var postUrl = apiConfig.addTerminateRequest;
                var data1 = data;
                var response = await renewalRepo.ContractSubmitTerminate(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorContractRenew = response;
                    var jss = Json(new
                    {
                        success = false,
                        message = response,
                        caseno = 0
                    });
                    return jss;
                }
                var apiResult = JsonConvert.DeserializeObject<StageOneApiResponse>(response.ToString());
                if (apiResult.status?.ToLower().Trim() == "200" ||
                    apiResult.status?.ToLower().Trim() == "true" ||
                    apiResult.status?.ToLower().Trim() == "success" ||
                    apiResult.status?.ToLower().Trim() == "ok")
                {
                    var js = Json(new
                    {
                        success = true,
                        message = "Stage updated Successfully",
                        caseno = apiResult.addServiceRequest.caseNo
                    });
                    return js;
                }
                else
                {
                    var js = Json(new
                    {
                        success = false,
                        message = "Stage updated Failed",
                        caseno = 0
                    });
                    return js;
                }
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorContractRenew = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                var js = Json(new
                {
                    success = false,
                    message = e.Message,
                    caseno = 0
                });
                return js;
            }
        }
        #endregion
        #region Offer letter
        public async Task<OfferLetterDownload> OfferLetterDownload([FromBody] ContractDetailparameter data)
        {
            try
            {
                ViewBag.errorContractOfferLetter = "";
                var postUrl = apiConfig.downloadOfferLetter;
                var data1 = data;
                var response = await renewalRepo.ContractOffer(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorContractOfferLetter = response;
                    var model1 = new OfferLetterDownload
                    {
                        status = "",
                        path = "",
                    };
                    return model1;
                }
                Console.WriteLine(response);
                OfferLetterDownload apiResponse = JsonConvert.DeserializeObject<OfferLetterDownload>(response.ToString());

                var model = new OfferLetterDownload
                {
                    status = apiResponse.status,
                    path = apiResponse.path,
                };
                return model;
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorContractOfferLetter = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                var model1 = new OfferLetterDownload
                {
                    status = "",
                    path = "",
                };
                return model1;
            }
        }
        #endregion

        #endregion

        #region Make Payment
       
        public async Task<IActionResult> OutStandingPayments(string ContractId, string ContractNo, string callingFrom)
        {
            try
            {
                ViewBag.errorContractMakePayment = "";
                var postUrl = apiConfig.getOutstandingPayments;
                ViewBag.ContractNo = ContractNo;
                ViewBag.ContractID = ContractId;
                ViewBag.callingFrom = callingFrom;
                var data = new ContractDetailparameter
                {
                    ContractId = ContractId,
                };
                var response = await renewalRepo.MakePayment(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorContractMakePayment = response;
                    var model1 = new OutstandingPaymentsModel();
                    return View(model1);
                }
                var s = response.status;
                OutstandingPaymentsModel model = new OutstandingPaymentsModel();
                if (response.status != "Invalid") {
                    Console.WriteLine(response);
                    OutstandingPaymentsModel apiResponse = JsonConvert.DeserializeObject<OutstandingPaymentsModel>(response.ToString());
                    List<RecordOutStanding> recordList = response.record.ToObject<List<RecordOutStanding>>();
                   

                    RemoveZeroBalance(apiResponse);
                    var paymenySymmary = await SumPaymentsAsync(apiResponse);
                    var paymenyToShow = InsertInPaymentsToShow(apiResponse, paymenySymmary);
                    var isShowAddress = ShouldShowAddressField(apiResponse);
                    var isGoOnlinePayments = ShouldGoToOnlinePayments(apiResponse);
                    model = new OutstandingPaymentsModel
                    {
                        Status = response.status,
                        Record = recordList,
                        PaymentSummary = paymenySymmary,
                        PaymentsToShow = paymenyToShow.PaymentsToShow,
                        ChequesToShowAddress = paymenyToShow.ChequesToShowAddressList,
                        ShowDeliveryOptionsTestForButton = paymenyToShow.ShowDeliveryOptionsTestForButtonResult,
                        ShowDeliveryOptions = isShowAddress.ShowDeliveryOptions,
                        ShowDeliveryOptionsTest = isShowAddress.ShowDeliveryOptions,
                        ChequeDeliveryOption = isShowAddress.ChequeDeliveryOption,
                        GotoOnlinePayments = isGoOnlinePayments,

                    };
                    if (paymenySymmary.SumOfAllPayments == "0.00")
                    {
                        ViewBag.errorContractPayable = "No Data Found";
                    }

                    if (model.Record != null && model.Record.Any() && model.Record.First().Confirmed == 0)
                    {
                        ViewBag.isPaymentConfirmed = 0;
                    }
                    else
                    {
                        ViewBag.isPaymentConfirmed = 1;
                    }

                    Console.WriteLine("---------------GetContractDetails----------------");
                    Console.WriteLine(model.Status);
                    Console.WriteLine(model.Record);
                    Console.WriteLine(model.PaymentSummary);
                    Console.WriteLine(model.GotoOnlinePayments);
                    Console.WriteLine(model.ShowDeliveryOptions);
                    Console.WriteLine(model.ShowDeliveryOptionsTestForButton);
                    Console.WriteLine(model.ShowDeliveryOptionsTest);
                    Console.WriteLine(model.ChequeDeliveryOption);
                    Console.WriteLine(model.PaymentsToShow);
                    Console.WriteLine(model.ChequesToShowAddress);
                    Console.WriteLine("---------------GetContractDetails----------------");
                }
                return View(model);
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorContractMakePayment = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                var model1 = new OutstandingPaymentsModel();
                return View(model1);
            }
        }

        public void RemoveZeroBalance(OutstandingPaymentsModel payments)
        {
            if (payments?.Record != null)
                payments?.Record.RemoveAll(x => x.Amount == 0);

            if (payments?.Record != null)
                payments?.Record.RemoveAll(x => x.Amount == 0);

            if (payments?.Record != null)
                payments?.Record.RemoveAll(x => x.Amount == 0);

            if (payments?.Record != null)
                payments?.Record.RemoveAll(x => x.Amount == 0);
        }

        public async Task<PaymentSummaryModel> SumPaymentsAsync(OutstandingPaymentsModel outstandingPayments)
        {
            double rentalSum = 0;
            double additionalSum = 0;
            double vatRentSum = 0;
            double vatChargesSum = 0;

            if (outstandingPayments?.Record != null)
            {
                foreach (var item in outstandingPayments.Record)
                {
                    var type = item.Type?.ToLowerInvariant();
                    var amount = item.Amount ?? 0;

                    if (type == "contract payable")
                    {
                        rentalSum += amount;
                    }
                    else if (type == "additional charges")
                    {
                        additionalSum += amount;
                    }
                    else if (type == "vat on rent")
                    {
                        vatRentSum += amount;
                    }
                    else if (type == "vat on charges")
                    {
                        vatChargesSum += amount;
                    }

                    // Handle rejected
                    if (item.IsRejected == true)
                    {
                        item.FilePath = null;
                    }
                }
            }

            var amountFormat = new CultureInfo("ar-SA");

            var result = new PaymentSummaryModel
            {
                TotalRentalPayment = rentalSum.ToString("N2", amountFormat),
                TotalAdditionalCharges = additionalSum.ToString("N2", amountFormat),
                TotalVatOnRent = vatRentSum.ToString("N2", amountFormat),
                TotalVatOnCharges = vatChargesSum.ToString("N2", amountFormat),
                SumOfAllPayments = (rentalSum + additionalSum + vatRentSum + vatChargesSum).ToString("N2", amountFormat)
            };


            return await Task.FromResult(result); // you can also just do: return result;
        }

        public PaymentToShowChequeToAddress InsertInPaymentsToShow(OutstandingPaymentsModel payments, PaymentSummaryModel paymentSummary)
        {
            payments?.PaymentsToShow?.Clear();
            payments?.ChequesToShowAddress?.Clear();
            payments!.ShowDeliveryOptionsTestForButton = false;
            PaymentToShowChequeToAddress model = new PaymentToShowChequeToAddress();

            List<dynamic>? PaymentsToShow = new List<dynamic> { };
            List<string>? ChequeAddress = new List<string> { };
            if (paymentSummary?.TotalRentalPayment != "0٫00")
            {
                PaymentsToShow?.Add("Rental Payments");
                ChequeAddress?.Add("false");

                foreach (var payable in payments?.Record!)
                {
                    if (string.Equals(payable.Type, "Contract Payable", StringComparison.OrdinalIgnoreCase))
                    {
                        if (payable.DefaultPaymentMethodType == 2)
                        {
                            ChequeAddress?.Add("true");
                            payments!.ShowDeliveryOptionsTestForButton = true;
                        }
                        else
                        {
                            ChequeAddress?.Add("false");
                        }

                        PaymentsToShow?.Add(payable);
                    }
                }
            }

            if (paymentSummary?.TotalAdditionalCharges != "0٫00")
            {
                PaymentsToShow?.Add("Additional Charges");
                ChequeAddress?.Add("false");

                foreach (var payable in payments?.Record!)
                {
                    if (string.Equals(payable.Type, "Additional Charges", StringComparison.OrdinalIgnoreCase))
                    {
                        PaymentsToShow?.Add(payable);
                        if (payable.DefaultPaymentMethodType == 2)
                        {
                            ChequeAddress?.Add("true");
                            payments!.ShowDeliveryOptionsTestForButton = true;
                        }
                        else
                        {
                            ChequeAddress?.Add("false");
                        }
                    }
                }
            }

            if (paymentSummary?.TotalVatOnRent != "0٫00")
            {
                PaymentsToShow?.Add("VAT on Rent");
                ChequeAddress?.Add("false");

                foreach (var payable in payments?.Record!)
                {
                    if (string.Equals(payable.Type, "Vat On Rent", StringComparison.OrdinalIgnoreCase))
                    {
                        PaymentsToShow?.Add(payable);
                        if (payable.DefaultPaymentMethodType == 2)
                        {
                            ChequeAddress?.Add("true");
                            payments!.ShowDeliveryOptionsTestForButton = true;
                        }
                        else
                        {
                            ChequeAddress?.Add("false");
                        }
                    }
                }
            }

            if (paymentSummary?.TotalVatOnCharges != "0٫00")
            {
                PaymentsToShow?.Add("VAT on Charges");
                ChequeAddress?.Add("false");

                foreach (var payable in payments?.Record!)
                {
                    if (string.Equals(payable.Type, "Vat On Charges", StringComparison.OrdinalIgnoreCase))
                    {
                        if (payable.DefaultPaymentMethodType == 2)
                        {
                            ChequeAddress?.Add("true");
                            payments!.ShowDeliveryOptionsTestForButton = true;
                        }
                        else
                        {
                            ChequeAddress?.Add("false");
                        }

                        PaymentsToShow?.Add(payable);
                    }
                }
            }
            model.PaymentsToShow = PaymentsToShow;
            model.ChequesToShowAddressList = ChequeAddress;
            model.ShowDeliveryOptionsTestForButtonResult = payments?.ShowDeliveryOptionsTestForButton;
            return model;

        }

        public ShowDeveliveryAddress ShouldShowAddressField(OutstandingPaymentsModel payments)
        {
            foreach (var payable in payments?.Record!)
            {
                if (payable.DefaultPaymentMethodType == 2)
                {
                    payments. ShowDeliveryOptions = true;
                    payments.ShowDeliveryOptionsTest = true;
                }

                Console.WriteLine($"Self Delivery :::::: {payable?.SelfDelivery}");

                if (payable?.SelfDelivery == "2")
                {
                    payments.ChequeDeliveryOption = 2;
                    ViewBag.ChequeDeliveryOption = 2;
                    ViewBag.ChequeDeliveryAddress = payable.AramexAddress??"";
                }
                else if (payable?.SelfDelivery == "1")
                {
                    payments.ChequeDeliveryOption = 1;
                    ViewBag.ChequeDeliveryOption = 1;
                    ViewBag.ChequeDeliveryAddress = payable.AramexAddress ?? "";
                }
                else
                {
                    Console.WriteLine($"Self Delivery if Null  :::::: {payable?.SelfDelivery}");
                    payments.ChequeDeliveryOption = 2;
                }
            }
            ShowDeveliveryAddress model = new ShowDeveliveryAddress();
           
            if (payments.ShowDeliveryOptions == true && payments.ShowDeliveryOptionsTest == true)
            {
                model.ShowDeliveryOptions = true;
            }
            else {
                model.ShowDeliveryOptions = false;
            }
            model.ChequeDeliveryOption = payments.ChequeDeliveryOption;
            return model;
        }

        public bool ShouldGoToOnlinePayments(OutstandingPaymentsModel payments)
        {
            payments.GotoOnlinePayments = false;

            foreach (var payable in payments?.Record!)
            {
                if (payable.DefaultPaymentMethodType == 1 || payable.DefaultPaymentMethodType == 3)
                {
                    payments.GotoOnlinePayments = true;
                    break;
                }
            }

            return (bool)payments!.GotoOnlinePayments;
        }

        public async Task<DownloadCheque> ChequeDownload([FromBody] DownloadChequeParameter param)
        {
            try
            {
                var postUrl = apiConfig.downloadCheque;
                var data = new DownloadChequeParameter
                {
                    PaymentSettingId = param.PaymentSettingId,
                };
                var response = await renewalRepo.MakePaymentDownloadCheque(data, postUrl, token ?? "");

                if (response is string)
                {
                    ViewBag.errorDocumentDownloadDocPhotos = response;
                    return new DownloadCheque();
                }

                if (response?.status == "200")
                {
                    Console.WriteLine(response);
                   
                    DownloadCheque apiResponse = JsonConvert.DeserializeObject<DownloadCheque>(response.ToString());

                    var model = new DownloadCheque
                    {
                        status = apiResponse.status,
                        cheque = apiResponse.cheque,
                    };
                    return model;
                }
                else
                {
                    ViewBag.errorDocumentDownloadDocPhotos = response?["responseMessage"];
                    return new DownloadCheque();
                }
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorDocumentDownloadDocPhotos = e.Message;
                return new DownloadCheque();
            }
        }

        public async Task<RemoveDoc> MakePaymentRemoveCheque([FromBody] DownloadChequeParameter param)
        {
            try
            {

                var postUrl = apiConfig.removeCheque;
                var data = new DownloadChequeParameter
                {
                    PaymentSettingId = param.PaymentSettingId,
                };
                var response = await renewalRepo.MakePaymentRemoveCheque(data, postUrl, token ?? "");

                if (response is string)
                {
                    ViewBag.errorremoveSvcReqPhoto = response;
                    return new RemoveDoc();
                }
                if (response?.status == "200" && response?.message == "Cheque Deleted Successfully")
                {
                    Console.WriteLine(response);
                    RemoveDoc apiResponse = JsonConvert.DeserializeObject<RemoveDoc>(response?.ToString());
                    var model = new RemoveDoc
                    {
                        status = apiResponse.status,
                    };
                    return model;
                }
                else
                {
                    ViewBag.errorremoveSvcReqPhoto = response?["responseMessage"];
                    return new RemoveDoc();
                }
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorremoveSvcReqPhoto = e.Message;
                return new RemoveDoc();
            }
        }



        public async Task<bool> UpdateArmaxAdress([FromBody] UpdateArmaxAdressParam param)
        {
            try
            {
                HttpContext.Session.SetString("ContractId", param?.ContractId ?? "");
                ViewBag.errorUpdateContractPaymentAddress = "";
                var postUrl = apiConfig.updateContractPaymentAddress;
                var data = new UpdateArmaxAdressParam
                {
                    ContractId = param?.ContractId,
                    Address = param?.Address,
                    SelfDelivery = param?.SelfDelivery,
                };
                var response = await renewalRepo.UpdateArmaxAdress(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorUpdateContractPaymentAddress = response;
                    return false;
                }

                if (response.status == "ok")
                {
                    return true;
                }
                else {
                    return false;
                }

                
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorUpdateContractPaymentAddress = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                return false;
            }
        }


      
        public async Task<IActionResult> UploadCheque(string PaymentModeId, string PaymentSettingId, string ChequeNo, string Address,   IFormFile file)
        {
            try
            {
                ViewBag.lastUploadedCheque = ChequeNo;
                ViewBag.errorUploadCheque = "";
                var data = new MakePaymentChequeUpload
                {
                    PaymentModeId = PaymentModeId,
                    PaymentSettingId = PaymentSettingId,
                    ChequeNo = ChequeNo,
                    Address = Address,
                };
                
                string? filePath = await SaveAttachmentAsync(file);
                var postUrl = apiConfig.updateContractPaymentMethod;
                var response = await renewalRepo.MakePaymentUploadCheque(data, postUrl, token ?? "", filePath??"");
                if (response is string)
                {
                    ViewBag.errorUploadCheque = response;
                    var json = Json(new
                    {
                        success = false,
                        status = false,
                    });
                    return json;
                }

                if (response.status == "ok" &&    response.statusCode == "200")
                {
                    Console.WriteLine(response);
                    //var name = filePath?.Split('/').Last();
                    var json = Json(new
                    {
                        success = true,
                        status = response.status,
                    });
                    return json;
                }
                else
                {
                    ViewBag.errorUploadCheque = response.message;
                    var json = Json(new
                    {
                        success = false,
                        status = response.status
                    });
                    return json;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorUploadCheque = e.Message;
                var json = Json(new
                {
                    success = false,
                    status = false
                });
                return json;
            }

        }


        #endregion

        #region Online Payments
        public async Task<IActionResult> OnlinePayments(string contractNo,string contractID, string callerFrom)
        {
            try
            {
                var contractId = HttpContext.Session.GetString("ContractId");
                var postUrl = apiConfig.onlinePayables;
                ViewBag.ContractNo = contractNo;
                ViewBag.ContractID = contractId;
                ViewBag.callerFrom = callerFrom;
                var data = new GotoOnlinePaymentsParam
                {
                    ContractId = contractId
                };
                var response = await renewalRepo.getOnlinePayment(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorGetPopUp = response;
                    GoToOnlinePaymentsModel model1 = new GoToOnlinePaymentsModel
                    {
                        Status = "",
                        Record = null
                    };
                    return View(model1);
                }
                GoToOnlinePaymentsModel apiResponse = JsonConvert.DeserializeObject<GoToOnlinePaymentsModel>(response.ToString());
                List<RecordOutStanding> dataList = response.record.ToObject<List<RecordOutStanding>>();

                var onlinePaymentsSum = await SumPaymentsOnlineAsync(dataList);
                RemoveZeroBalanceOnline(dataList);
                List<int> cardPaymentsCount = new List<int>();
                List<int> bankTransferPaymentsCount = new List<int>();
                bool isRadioButton = false;
                foreach (var record in dataList)
                {
                    if (record.PaymentMode == "Card Payment" || record.PaymentMethodId == 1)
                    {
                        cardPaymentsCount.Add(record.PaymentMethodId ?? 0);
                    }
                    if (record.PaymentMode == "Bank Transfer" || record.PaymentMethodId == 3)
                    {
                        bankTransferPaymentsCount.Add(record.PaymentMethodId ?? 0);
                    }

                    if (record.PaymentMode == "Card Payment" || record.PaymentMode == "Bank Transfer" || record.PaymentMethodId == 1 || record.PaymentMethodId == 3)
                    {
                        isRadioButton = true;
                    }
                    else
                    {
                        isRadioButton = false;
                    }
                }
                GoToOnlinePaymentsModel result = new GoToOnlinePaymentsModel
                {
                    Status = apiResponse.Status,
                    Record = dataList,
                    OnlinePaymentSummary = onlinePaymentsSum,
                    CardPaymentListLength = cardPaymentsCount,
                    BankTransferListLength = bankTransferPaymentsCount,
                    IsRadioButtonShow = isRadioButton
                };
                ViewBag.CardPaymentListLength = cardPaymentsCount;
                ViewBag.BankTransferListLength = bankTransferPaymentsCount;
                return View(result);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                GoToOnlinePaymentsModel model = new GoToOnlinePaymentsModel();
                return View(model);

            }
        }


        public void RemoveZeroBalanceOnline(List<RecordOutStanding> onlinePayments)
        {
            onlinePayments!.RemoveAll(r => (r.Amount ?? 0) == 0);
            var list = onlinePayments;
        }

        public async Task<OnlinePaymentSumModel> SumPaymentsOnlineAsync(List<RecordOutStanding> onlinePayments)
        {
            double RentalSum = 0;
            double AdditionalSum = 0;
            double VatRentSum = 0;
            double VatChargesSum = 0;

            foreach (var record in onlinePayments)
            {
                if (string.Equals(record.Type, "contract payable", StringComparison.OrdinalIgnoreCase))
                {
                    RentalSum += record.Amount ?? 0.0;
                }
                else if (string.Equals(record.Type, "additional charges", StringComparison.OrdinalIgnoreCase))
                {
                    AdditionalSum += record.Amount ?? 0.0;
                    record.IsChecked = true;
                }
                else if (string.Equals(record.Type, "vat on rent", StringComparison.OrdinalIgnoreCase))
                {
                    VatRentSum += record.Amount ?? 0.0;
                    record.IsChecked = true;
                }
                else if (string.Equals(record.Type, "vat on charges", StringComparison.OrdinalIgnoreCase))
                {
                    VatChargesSum += record.Amount ?? 0.0;
                    record.IsChecked = true;
                }
            }

            var amounts = SumSelectedPayments(onlinePayments!);
            var amountFormat = new CultureInfo("en-US");
            var result = new OnlinePaymentSumModel
            {
                TotalRentalPayment = RentalSum.ToString("N2", amountFormat),
                TotalAdditionalCharges = AdditionalSum.ToString("N2", amountFormat),
                TotalVatOnRent = VatRentSum.ToString("N2", amountFormat),
                TotalVatOnCharges = VatChargesSum.ToString("N2", amountFormat),
                SumOfAllPayments = amounts,
            };
            return await Task.FromResult(result);
        }

        public List<string> SumSelectedPayments(List<RecordOutStanding> onlinePayments)
        {
            double sum = 0;
            double sum1 = 0;
            List<string> amounts = new List<string> { };

            foreach (var payment in onlinePayments!.Where(r => r.IsChecked))
            {
                if (payment.PaymentMode == "Card Payment")
                {
                    sum += payment.Amount ?? 0.0;
                }
                else
                {
                    sum1 += payment.Amount ?? 0.0;
                }
            }
            var amountFormat = new CultureInfo("ar-SA");
            var amount1 = sum.ToString("N2", amountFormat);
            var amount2 = sum1.ToString("N2", amountFormat);
            amounts.Add(amount1);
            amounts.Add(amount2);
            return amounts;
        }
        #endregion

        #region Terms&Conditions & Refund/Return Policy
        public async Task<IActionResult> TermsAndConditions(string ContractId, string ContractNo, string caller, string dataType)
        {
            try
                {
                    ViewBag.errorTermsAndConditions = "";
                    var postUrl = apiConfig.getPolicyData;
                if (caller != "New")
                {
                    if (dataType == "RefundPolicy")
                    {
                        dataType = "RefundPolicy";
                    }
                    else
                    {
                        dataType = "PrivacyPolicy";
                    }
                }
                else
                {
                    if (dataType == "RefundPolicyNewContract")
                    {
                        dataType = "RefundPolicyNewContract";
                    }
                    else
                    {
                        dataType = "PrivacyPolicyNewContract";
                    }
                }
                var data = new PolicyRefundparameter
                    {
                        Key = dataType,
                    };
                    var response = await renewalRepo.getOnlinePolicyRefundable(data, postUrl, token ?? "");
                    if (response is string)
                    {
                        ViewBag.errorTermsAndConditions = response;
                        var model1 = new TermsAndConditiosResponseModel();
                        return View(model1);
                    }
                    Console.WriteLine(response);
                    TermsAndConditiosResponseModel apiResponse = JsonConvert.DeserializeObject<TermsAndConditiosResponseModel>(response.ToString());
                    TermsAndConditiosResponse record = response.data.ToObject<TermsAndConditiosResponse>();
                    TermsAndConditiosResponseModel model = new TermsAndConditiosResponseModel
                    {
                        Status = response.status,
                        Message = response.message,
                        Data = record
                    };
                return View(model);
                }
                catch (HttpRequestException e)
                {
                    ViewBag.errorTermsAndConditions = e.Message;
                    Console.WriteLine($"Request error: {e.Message}");
                    var model1 = new TermsAndConditiosResponseModel();
                    return View(model1);
                }
            }
        public async Task<IActionResult> RefundAndReturnPolicy(string ContractId, string ContractNo)
        {
            try
            {
                ViewBag.errorRefundAndReturnPolicy = "";
                var postUrl = apiConfig.getPolicyData;
                var data = new PolicyRefundparameter
                {
                    Key = "RefundPolicy",
                };
                var response = await renewalRepo.getOnlinePolicyRefundable(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorRefundAndReturnPolicy = response;
                    var model1 = new TermsAndConditiosResponseModel();
                    return View(model1);
                }
                Console.WriteLine(response);
                TermsAndConditiosResponseModel apiResponse = JsonConvert.DeserializeObject<TermsAndConditiosResponseModel>(response.ToString());
                TermsAndConditiosResponse record = response.data.ToObject<TermsAndConditiosResponse>();
                TermsAndConditiosResponseModel model = new TermsAndConditiosResponseModel
                {
                    Status = response.status,
                    Message = response.message,
                    Data = record
                };
                return View(model);
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorRefundAndReturnPolicy = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                var model1 = new TermsAndConditiosResponseModel();
                return View(model1);
            }
        }

        #endregion

        #region Register Payment
        public async Task<RegisterPaymentResponse> RegisterPayment([FromBody] RegisterPaymentParam param)
        {
            try
            {
                ViewBag.errorRegisterPayment = "";
                var postUrl = apiConfig.registerPayment;
                var userID = HttpContext.Session.GetString("userId");
                var data = new RegisterPaymentParam
                        {
                    totalAmount = param?.totalAmount,
                    contractId = param?.contractId,
                    contractNo = param?.contractNo,
                    userId = userID,
                    detailList = param?.detailList,
                };
                var response = await renewalRepo.RegisterPayment(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorRegisterPayment = response;
                    RegisterPaymentResponse model1 = new RegisterPaymentResponse
                    {
                        OrderId = 0,
                        TransactionId = "",
                        Url = "",
                        Status = response,
                    };

                    return model1;
                }
                RegisterPaymentResponse apiResponse = JsonConvert.DeserializeObject<RegisterPaymentResponse>(response.ToString());
                RegisterPaymentResponse model = new RegisterPaymentResponse
                {
                    Status = apiResponse.Status,
                    OrderId = apiResponse.OrderId,
                    TransactionId = apiResponse.TransactionId,
                    Url = apiResponse.Url,
                };
                Console.WriteLine(response);
                return model;
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorRegisterPayment = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                RegisterPaymentResponse model1 = new RegisterPaymentResponse
                {
                    OrderId = 0,
                    TransactionId = "",
                    Url = "",
                    Status = e.Message,
                };

                return model1;
            }
        }

        #endregion

        #region Sign Contract
        public IActionResult AuthenticateContract()
        {
            ViewBag.errorDownloadContract = "";
            var json = HttpContext.Session.GetString("ContractDownloadModel");
            if (string.IsNullOrEmpty(json))
            {
                ViewBag.errorDownloadContract = "No data found.";
                return View();
            }
            var model = JsonConvert.DeserializeObject<OfferLetterDownload>(json!);
            var contractNo = HttpContext.Session.GetString("ContractNo");
            var contractID = HttpContext.Session.GetString("ContractID");
            var dueActionID = HttpContext.Session.GetString("DueActionID");
            var CaseNo = HttpContext.Session.GetString("CaseNo");
            var StageID = HttpContext.Session.GetString("StageID");
            var callingFrom = HttpContext.Session.GetString("callingFrom");

            ViewBag.ContractNo = contractNo;
            ViewBag.ContractID = contractID;
            ViewBag.DueActionID = dueActionID;
            ViewBag.CaseNO = CaseNo;
            ViewBag.StageID = StageID;
            ViewBag.callingFrom = callingFrom;
            return View(model);
        }

        public async Task<IActionResult> ContractDownload([FromBody] ContractRenewParamWithCaseNO param)
        {
            try
            {
                ViewBag.errorDownloadContract = "";
                var postUrl = apiConfig.contractDownload;
                HttpContext.Session.SetString("ContractNo", param?.ContractNo ??"");
                HttpContext.Session.SetString("ContractID", param?.ContractId ??"");
                HttpContext.Session.SetString("DueActionID", param?.DueActionID ?? "");
                HttpContext.Session.SetString("CaseNo", param?.CaseNo ?? "");
                HttpContext.Session.SetString("StageID", param?.StageID ?? ""); 
                HttpContext.Session.SetString("callingFrom", param?.callingFrom ?? ""); 
                var data = new ContractDetailparameter {
                ContractId = param?.ContractId
                    };
                var response = await renewalRepo.DownloadContract(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorContractRenew = response;
                    return Json(new
                    {
                        success = false,
                        redirectUrl = Url.Action("")
                    });
                }
                OfferLetterDownload apiResponse = JsonConvert.DeserializeObject<OfferLetterDownload>(response.ToString());
               
                string jsonData = JsonConvert.SerializeObject(apiResponse);
                HttpContext.Session.SetString("ContractDownloadModel", jsonData);
                var jsonRes = HttpContext.Session.GetString("ContractDownloadModel");
                Console.WriteLine(jsonRes);
                var js = Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("AuthenticateContract") // or "ContractRenew"
                });
                return js;
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorDownloadContract = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                var model1 = Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("") // or "ContractRenew"
                });
                return model1;
            }
        }

        public async Task<OfferLetterDownload> getContractTerms([FromBody] ContractDetailparameter data)
        {
            try
            {
                ViewBag.errorContractTerms = "";
                var postUrl = "";
                if (data.callerFrom == "New") {
                     postUrl = apiConfig.downloadContractTermsNew;
                } else {
                     postUrl = apiConfig.downloadContractTerms;
                }
               
                
                var data1 = data;
                var response = await renewalRepo.ContractOffer(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorContractTerms = response;
                    var model1 = new OfferLetterDownload
                    {
                        status = "",
                        path = "",
                    };
                    return model1;
                }
                Console.WriteLine(response);
                OfferLetterDownload apiResponse = JsonConvert.DeserializeObject<OfferLetterDownload>(response.ToString());

                var model = new OfferLetterDownload
                {
                    status = apiResponse.status,
                    path = apiResponse.path,
                };
                return model;
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorContractTerms = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                var model1 = new OfferLetterDownload
                {
                    status = "",
                    path = "",
                };
                return model1;
            }
        }

        public async Task<IActionResult> SaveSignature(
            string caseNo,
            string expiry,
            string attachementType,
            string documentTypeId,
            IFormFile file)
        {
            try
            {
                // If expiry not provided, set default
                if (string.IsNullOrEmpty(expiry))
                {
                    expiry = DateTime.UtcNow.ToString("dd-MM-yyyy");
                }

                // Save file temporarily to disk
                string? filePath = await SaveAttachmentAsync(file);

                // Prepare API URL and token
                var postUrl = apiConfig.UploadServiceRequestFiles;
                var tokenValue = token ?? "";

                // Call repository method
                var response = await renewalRepo.GetServiceRequestReportFileUpload(
                    caseNo, attachementType, expiry, documentTypeId, postUrl, tokenValue, filePath ?? ""
                );

                // Handle string response (usually an error message)
                if (response is string)
                {
                    ViewBag.errorDocumentUploadDocPhotos = response;
                    return Json(new
                    {
                        success = false,
                        message = response
                    });
                }
                // Handle valid response object
                if (response?.status == "file Uploaded")
                {
                    return Json(new
                    {
                        success = true,
                        message = response?.status,
                        PhotoId = response?.photoId // you can make this dynamic if needed
                    });
                }
                else
                {
                    ViewBag.errorDocumentUploadDocPhotos = response?["responseMessage"];
                    return Json(new
                    {
                        success = false,
                        message = response?["responseMessage"] ?? "Unknown server error"
                    });
                }
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorDocumentUploadDocPhotos = e.Message;
                return Json(new
                {
                    success = false,
                    message = e.Message
                });
            }
            catch (Exception ex)
            {
                ViewBag.errorDocumentUploadDocPhotos = ex.Message;
                return Json(new
                {
                    success = false,
                    message = "Unexpected error: " + ex.Message
                });
            }
        }

        public async Task<JsonResult> UpdateSignedContractStage(string DueActionid, string StageId)
        {
            try
            {

                var postUrl = apiConfig.updateContractStage;
                var data = new UpdateContractStage
                {
                    DueActionid = DueActionid?.ToString(),
                    StageId = StageId?.ToString()
                };
                var response = await renewalRepo.UpdateRenewalContractStage(data, postUrl, token ?? "");

                if (response is string)
                {
                    ViewBag.errorremoveSvcReqPhoto = response;
                    return Json(new { success = false, message = response });
                }

                if (response?["statusCode"] == "200")
                {
                    Console.WriteLine(response);
                    return Json(new { success = true, message = "Stage Updated Successfully" , emirateId = response?["emirateId"] });
                }
                else
                {
                    ViewBag.errorremoveSvcReqPhoto = response?["responseMessage"];
                    return Json(new { success = false, message = response?["responseMessage"] , emirateId = response?["emirateId"] });
                }
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorremoveSvcReqPhoto = e.Message;
                return Json(new { success = false, message = e.Message, emirateId = -1 });
            }
        }



        #endregion

        #region Municiplity Approval
        public async Task<IActionResult> MunicipalityApproval(string ContractId, string ContractNo, string DueActionID, string callingFrom)
        {
            try
            {
                ViewBag.errorMuniciplityApproval = "";
                var postUrl = apiConfig.getApproveMunicipal;
                ViewBag.ContractNo = ContractNo.ToString();
                ViewBag.ContractID = ContractId.ToString();
                ViewBag.DueActionID = DueActionID.ToString();
                ViewBag.callingFrom = callingFrom;
                var data = new ContractDetailparameter
                {
                    ContractId = ContractId
                };
                var response = await renewalRepo.MuniciplityApproval(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorMuniciplityApproval = response;
                    var model = new MunicipalInstructions();
                    return View(model);
                }
                MunicipalInstructions apiResponse = JsonConvert.DeserializeObject<MunicipalInstructions>(response.ToString());
                var model1 = new MunicipalInstructions {
                    Record = apiResponse.Record,
                    Status = apiResponse.Status,
                    StatusCode = apiResponse.StatusCode,
                    Message = apiResponse.Message
                };
                return View(model1);
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorMuniciplityApproval = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                var model1 = new MunicipalInstructions();
                return View(model1);
            }
        }

        public async Task<JsonResult> UpdateRenewalContractStage(string DueActionid, string StageId)
        {
            try
            {

                var postUrl = apiConfig.updateContractDocumentStage;
                var data = new UpdateContractStage
                {
                    DueActionid = DueActionid?.ToString(),
                    StageId = StageId?.ToString()
                };
                var response = await renewalRepo.UpdateRenewalContractStage(data, postUrl, token ?? "");

                if (response is string)
                {
                    ViewBag.errorremoveSvcReqPhoto = response;
                    return Json(new { success = false, message = response });
                }

                if (response?["statusCode"] == "200")
                {
                    Console.WriteLine(response);
                    return Json(new { success = true, message = "Stage Updated Successfully" });
                }
                else
                {
                    ViewBag.errorremoveSvcReqPhoto = response?["responseMessage"];
                    return Json(new { success = false, message = response?["responseMessage"] });
                }
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorremoveSvcReqPhoto = e.Message;
                return Json(new { success = false, message = e.Message });
            }
        }


        #endregion
        #endregion

        #region New Contract
        
        #region Make Payment

        public async Task<IActionResult> OutStandingPaymentsNew(string ContractId, string ContractNo, string callingFrom)
        {
            try
            {
                ViewBag.errorContractMakePayment = "";
                var postUrl = apiConfig.getOutstandingPayments;
                ViewBag.ContractNo = ContractNo;
                ViewBag.ContractID = ContractId;
                ViewBag.callingFrom = callingFrom;
                var data = new ContractDetailparameter
                {
                    ContractId = ContractId,
                };
                var response = await renewalRepo.MakePayment(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorContractMakePayment = response;
                    var model1 = new OutstandingPaymentsModel();
                    return View(model1);
                }
                var s = response.status;
                OutstandingPaymentsModel model = new OutstandingPaymentsModel();
                if (response.status != "Invalid")
                {
                    Console.WriteLine(response);
                    OutstandingPaymentsModel apiResponse = JsonConvert.DeserializeObject<OutstandingPaymentsModel>(response.ToString());
                    List<RecordOutStanding> recordList = response.record.ToObject<List<RecordOutStanding>>();


                    RemoveZeroBalanceNew(apiResponse);
                    var paymenySymmary = await SumPaymentsAsyncNew(apiResponse);
                    var paymenyToShow = InsertInPaymentsToShowNew(apiResponse, paymenySymmary);
                    var isShowAddress = ShouldShowAddressFieldNew(apiResponse);
                    var isGoOnlinePayments = ShouldGoToOnlinePaymentsNew(apiResponse);
                    model = new OutstandingPaymentsModel
                    {
                        Status = response.status,
                        Record = recordList,
                        PaymentSummary = paymenySymmary,
                        PaymentsToShow = paymenyToShow.PaymentsToShow,
                        ChequesToShowAddress = paymenyToShow.ChequesToShowAddressList,
                        ShowDeliveryOptionsTestForButton = paymenyToShow.ShowDeliveryOptionsTestForButtonResult,
                        ShowDeliveryOptions = isShowAddress.ShowDeliveryOptions,
                        ShowDeliveryOptionsTest = isShowAddress.ShowDeliveryOptions,
                        ChequeDeliveryOption = isShowAddress.ChequeDeliveryOption,
                        GotoOnlinePayments = isGoOnlinePayments,

                    };
                    if (paymenySymmary.SumOfAllPayments == "0.00")
                    {
                        ViewBag.errorContractPayable = "No Data Found";
                    }

                    if (model.Record != null && model.Record.Any() && model.Record.First().Confirmed == 0)
                    {
                        ViewBag.isPaymentConfirmed = 0;
                    }
                    else
                    {
                        ViewBag.isPaymentConfirmed = 1;
                    }

                    Console.WriteLine("---------------GetContractDetails----------------");
                    Console.WriteLine(model.Status);
                    Console.WriteLine(model.Record);
                    Console.WriteLine(model.PaymentSummary);
                    Console.WriteLine(model.GotoOnlinePayments);
                    Console.WriteLine(model.ShowDeliveryOptions);
                    Console.WriteLine(model.ShowDeliveryOptionsTestForButton);
                    Console.WriteLine(model.ShowDeliveryOptionsTest);
                    Console.WriteLine(model.ChequeDeliveryOption);
                    Console.WriteLine(model.PaymentsToShow);
                    Console.WriteLine(model.ChequesToShowAddress);
                    Console.WriteLine("---------------GetContractDetails----------------");
                }
                return View(model);
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorContractMakePayment = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                var model1 = new OutstandingPaymentsModel();
                return View(model1);
            }
        }

        public void RemoveZeroBalanceNew(OutstandingPaymentsModel payments)
        {
            if (payments?.Record != null)
                payments?.Record.RemoveAll(x => x.Amount == 0);

            if (payments?.Record != null)
                payments?.Record.RemoveAll(x => x.Amount == 0);

            if (payments?.Record != null)
                payments?.Record.RemoveAll(x => x.Amount == 0);

            if (payments?.Record != null)
                payments?.Record.RemoveAll(x => x.Amount == 0);
        }

        public async Task<PaymentSummaryModel> SumPaymentsAsyncNew(OutstandingPaymentsModel outstandingPayments)
        {
            double rentalSum = 0;
            double additionalSum = 0;
            double vatRentSum = 0;
            double vatChargesSum = 0;

            if (outstandingPayments?.Record != null)
            {
                foreach (var item in outstandingPayments.Record)
                {
                    var type = item.Type?.ToLowerInvariant();
                    var amount = item.Amount ?? 0;

                    if (type == "contract payable")
                    {
                        rentalSum += amount;
                    }
                    else if (type == "additional charges")
                    {
                        additionalSum += amount;
                    }
                    else if (type == "vat on rent")
                    {
                        vatRentSum += amount;
                    }
                    else if (type == "vat on charges")
                    {
                        vatChargesSum += amount;
                    }

                    // Handle rejected
                    if (item.IsRejected == true)
                    {
                        item.FilePath = null;
                    }
                }
            }

            var amountFormat = new CultureInfo("ar-SA");

            var result = new PaymentSummaryModel
            {
                TotalRentalPayment = rentalSum.ToString("N2", amountFormat),
                TotalAdditionalCharges = additionalSum.ToString("N2", amountFormat),
                TotalVatOnRent = vatRentSum.ToString("N2", amountFormat),
                TotalVatOnCharges = vatChargesSum.ToString("N2", amountFormat),
                SumOfAllPayments = (rentalSum + additionalSum + vatRentSum + vatChargesSum).ToString("N2", amountFormat)
            };


            return await Task.FromResult(result); // you can also just do: return result;
        }

        public PaymentToShowChequeToAddress InsertInPaymentsToShowNew(OutstandingPaymentsModel payments, PaymentSummaryModel paymentSummary)
        {
            payments?.PaymentsToShow?.Clear();
            payments?.ChequesToShowAddress?.Clear();
            payments!.ShowDeliveryOptionsTestForButton = false;
            PaymentToShowChequeToAddress model = new PaymentToShowChequeToAddress();

            List<dynamic>? PaymentsToShow = new List<dynamic> { };
            List<string>? ChequeAddress = new List<string> { };
            if (paymentSummary?.TotalRentalPayment != "0٫00")
            {
                PaymentsToShow?.Add("Rental Payments");
                ChequeAddress?.Add("false");

                foreach (var payable in payments?.Record!)
                {
                    if (string.Equals(payable.Type, "Contract Payable", StringComparison.OrdinalIgnoreCase))
                    {
                        if (payable.DefaultPaymentMethodType == 2)
                        {
                            ChequeAddress?.Add("true");
                            payments!.ShowDeliveryOptionsTestForButton = true;
                        }
                        else
                        {
                            ChequeAddress?.Add("false");
                        }

                        PaymentsToShow?.Add(payable);
                    }
                }
            }

            if (paymentSummary?.TotalAdditionalCharges != "0٫00")
            {
                PaymentsToShow?.Add("Additional Charges");
                ChequeAddress?.Add("false");

                foreach (var payable in payments?.Record!)
                {
                    if (string.Equals(payable.Type, "Additional Charges", StringComparison.OrdinalIgnoreCase))
                    {
                        PaymentsToShow?.Add(payable);
                        if (payable.DefaultPaymentMethodType == 2)
                        {
                            ChequeAddress?.Add("true");
                            payments!.ShowDeliveryOptionsTestForButton = true;
                        }
                        else
                        {
                            ChequeAddress?.Add("false");
                        }
                    }
                }
            }

            if (paymentSummary?.TotalVatOnRent != "0٫00")
            {
                PaymentsToShow?.Add("VAT on Rent");
                ChequeAddress?.Add("false");

                foreach (var payable in payments?.Record!)
                {
                    if (string.Equals(payable.Type, "Vat On Rent", StringComparison.OrdinalIgnoreCase))
                    {
                        PaymentsToShow?.Add(payable);
                        if (payable.DefaultPaymentMethodType == 2)
                        {
                            ChequeAddress?.Add("true");
                            payments!.ShowDeliveryOptionsTestForButton = true;
                        }
                        else
                        {
                            ChequeAddress?.Add("false");
                        }
                    }
                }
            }

            if (paymentSummary?.TotalVatOnCharges != "0٫00")
            {
                PaymentsToShow?.Add("VAT on Charges");
                ChequeAddress?.Add("false");

                foreach (var payable in payments?.Record!)
                {
                    if (string.Equals(payable.Type, "Vat On Charges", StringComparison.OrdinalIgnoreCase))
                    {
                        if (payable.DefaultPaymentMethodType == 2)
                        {
                            ChequeAddress?.Add("true");
                            payments!.ShowDeliveryOptionsTestForButton = true;
                        }
                        else
                        {
                            ChequeAddress?.Add("false");
                        }

                        PaymentsToShow?.Add(payable);
                    }
                }
            }
            model.PaymentsToShow = PaymentsToShow;
            model.ChequesToShowAddressList = ChequeAddress;
            model.ShowDeliveryOptionsTestForButtonResult = payments?.ShowDeliveryOptionsTestForButton;
            return model;

        }

        public ShowDeveliveryAddress ShouldShowAddressFieldNew(OutstandingPaymentsModel payments)
        {
            foreach (var payable in payments?.Record!)
            {
                if (payable.DefaultPaymentMethodType == 2)
                {
                    payments.ShowDeliveryOptions = true;
                    payments.ShowDeliveryOptionsTest = true;
                }

                Console.WriteLine($"Self Delivery :::::: {payable?.SelfDelivery}");

                if (payable?.SelfDelivery == "2")
                {
                    payments.ChequeDeliveryOption = 2;
                    ViewBag.ChequeDeliveryOption = 2;
                    ViewBag.ChequeDeliveryAddress = payable.AramexAddress ?? "";
                }
                else if (payable?.SelfDelivery == "1")
                {
                    payments.ChequeDeliveryOption = 1;
                    ViewBag.ChequeDeliveryOption = 1;
                    ViewBag.ChequeDeliveryAddress = payable.AramexAddress ?? "";
                }
                else
                {
                    Console.WriteLine($"Self Delivery if Null  :::::: {payable?.SelfDelivery}");
                    payments.ChequeDeliveryOption = 2;
                }
            }
            ShowDeveliveryAddress model = new ShowDeveliveryAddress();

            if (payments.ShowDeliveryOptions == true && payments.ShowDeliveryOptionsTest == true)
            {
                model.ShowDeliveryOptions = true;
            }
            else
            {
                model.ShowDeliveryOptions = false;
            }
            model.ChequeDeliveryOption = payments.ChequeDeliveryOption;
            return model;
        }

        public bool ShouldGoToOnlinePaymentsNew(OutstandingPaymentsModel payments)
        {
            payments.GotoOnlinePayments = false;

            foreach (var payable in payments?.Record!)
            {
                if (payable.DefaultPaymentMethodType == 1 || payable.DefaultPaymentMethodType == 3)
                {
                    payments.GotoOnlinePayments = true;
                    break;
                }
            }

            return (bool)payments!.GotoOnlinePayments;
        }

       #endregion

        #region Online Payments
        public async Task<IActionResult> OnlinePaymentsNew(string contractNo, string contractID)
        {
            try
            {
                var contractId = HttpContext.Session.GetString("ContractId");
                var postUrl = apiConfig.onlinePayables;
                ViewBag.ContractNo = contractNo;
                ViewBag.ContractID = contractId;
                var data = new GotoOnlinePaymentsParam
                {
                    ContractId = contractId
                };
                var response = await renewalRepo.getOnlinePayment(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorGetPopUp = response;
                    GoToOnlinePaymentsModel model1 = new GoToOnlinePaymentsModel
                    {
                        Status = "",
                        Record = null
                    };
                    return View(model1);
                }
                GoToOnlinePaymentsModel apiResponse = JsonConvert.DeserializeObject<GoToOnlinePaymentsModel>(response.ToString());
                List<RecordOutStanding> dataList = response.record.ToObject<List<RecordOutStanding>>();

                var onlinePaymentsSum = await SumPaymentsOnlineAsyncNew(dataList);
                RemoveZeroBalanceOnlineNew(dataList);
                List<int> cardPaymentsCount = new List<int>();
                List<int> bankTransferPaymentsCount = new List<int>();
                bool isRadioButton = false;
                foreach (var record in dataList)
                {
                    if (record.PaymentMode == "Card Payment")
                    {
                        cardPaymentsCount.Add(record.DefaultPaymentMethodType ?? 0);
                    }
                    if (record.PaymentMode == "Cheque")
                    {
                        bankTransferPaymentsCount.Add(record.DefaultPaymentMethodType ?? 0);
                    }

                    if (record.PaymentMode == "Card Payment" || record.PaymentMode == "Cheque")
                    {
                        isRadioButton = true;
                    }
                    else
                    {
                        isRadioButton = false;
                    }
                }
                GoToOnlinePaymentsModel result = new GoToOnlinePaymentsModel
                {
                    Status = apiResponse.Status,
                    Record = dataList,
                    OnlinePaymentSummary = onlinePaymentsSum,
                    CardPaymentListLength = cardPaymentsCount,
                    BankTransferListLength = bankTransferPaymentsCount,
                    IsRadioButtonShow = isRadioButton
                };
                ViewBag.CardPaymentListLength = cardPaymentsCount;
                ViewBag.BankTransferListLength = bankTransferPaymentsCount;
                return View(result);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                GoToOnlinePaymentsModel model = new GoToOnlinePaymentsModel();
                return View(model);

            }
        }
        public void RemoveZeroBalanceOnlineNew(List<RecordOutStanding> onlinePayments)
        {
            onlinePayments!.RemoveAll(r => (r.Amount ?? 0) == 0);
            var list = onlinePayments;
        }

        public async Task<OnlinePaymentSumModel> SumPaymentsOnlineAsyncNew(List<RecordOutStanding> onlinePayments)
        {
            double RentalSum = 0;
            double AdditionalSum = 0;
            double VatRentSum = 0;
            double VatChargesSum = 0;

            foreach (var record in onlinePayments)
            {
                if (string.Equals(record.Type, "contract payable", StringComparison.OrdinalIgnoreCase))
                {
                    RentalSum += record.Amount ?? 0.0;
                }
                else if (string.Equals(record.Type, "additional charges", StringComparison.OrdinalIgnoreCase))
                {
                    AdditionalSum += record.Amount ?? 0.0;
                    record.IsChecked = true;
                }
                else if (string.Equals(record.Type, "vat on rent", StringComparison.OrdinalIgnoreCase))
                {
                    VatRentSum += record.Amount ?? 0.0;
                    record.IsChecked = true;
                }
                else if (string.Equals(record.Type, "vat on charges", StringComparison.OrdinalIgnoreCase))
                {
                    VatChargesSum += record.Amount ?? 0.0;
                    record.IsChecked = true;
                }
            }

            var amounts = SumSelectedPaymentsNew(onlinePayments!);
            var amountFormat = new CultureInfo("en-US");
            var result = new OnlinePaymentSumModel
            {
                TotalRentalPayment = RentalSum.ToString("N2", amountFormat),
                TotalAdditionalCharges = AdditionalSum.ToString("N2", amountFormat),
                TotalVatOnRent = VatRentSum.ToString("N2", amountFormat),
                TotalVatOnCharges = VatChargesSum.ToString("N2", amountFormat),
                SumOfAllPayments = amounts,
            };
            return await Task.FromResult(result);
        }

        public List<string> SumSelectedPaymentsNew(List<RecordOutStanding> onlinePayments)
        {
            double sum = 0;
            double sum1 = 0;
            List<string> amounts = new List<string> { };

            foreach (var payment in onlinePayments!.Where(r => r.IsChecked))
            {
                if (payment.PaymentMode == "Card Payment")
                {
                    sum += payment.Amount ?? 0.0;
                }
                else
                {
                    sum1 += payment.Amount ?? 0.0;
                }
            }
            var amountFormat = new CultureInfo("ar-SA");
            var amount1 = sum.ToString("N2", amountFormat);
            var amount2 = sum1.ToString("N2", amountFormat);
            amounts.Add(amount1);
            amounts.Add(amount2);
            return amounts;
        }
        #endregion

        #region Terms&Conditions & Refund/Return Policy
        public async Task<IActionResult> TermsAndConditionsNew(string ContractId, string ContractNo)
        {
            try
            {
                ViewBag.errorTermsAndConditions = "";
                var postUrl = apiConfig.getPolicyData;
                var data = new PolicyRefundparameter
                {
                    Key = "PrivacyPolicy",
                };
                var response = await renewalRepo.getOnlinePolicyRefundable(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorTermsAndConditions = response;
                    var model1 = new TermsAndConditiosResponseModel();
                    return View(model1);
                }
                Console.WriteLine(response);
                TermsAndConditiosResponseModel apiResponse = JsonConvert.DeserializeObject<TermsAndConditiosResponseModel>(response.ToString());
                TermsAndConditiosResponse record = response.data.ToObject<TermsAndConditiosResponse>();
                TermsAndConditiosResponseModel model = new TermsAndConditiosResponseModel
                {
                    Status = response.status,
                    Message = response.message,
                    Data = record
                };
                return View(model);
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorTermsAndConditions = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                var model1 = new TermsAndConditiosResponseModel();
                return View(model1);
            }
        }
        public async Task<IActionResult> RefundAndReturnPolicyNew(string ContractId, string ContractNo)
        {
            try
            {
                ViewBag.errorRefundAndReturnPolicy = "";
                var postUrl = apiConfig.getPolicyData;
                var data = new PolicyRefundparameter
                {
                    Key = "RefundPolicy",
                };
                var response = await renewalRepo.getOnlinePolicyRefundable(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorRefundAndReturnPolicy = response;
                    var model1 = new TermsAndConditiosResponseModel();
                    return View(model1);
                }
                Console.WriteLine(response);
                TermsAndConditiosResponseModel apiResponse = JsonConvert.DeserializeObject<TermsAndConditiosResponseModel>(response.ToString());
                TermsAndConditiosResponse record = response.data.ToObject<TermsAndConditiosResponse>();
                TermsAndConditiosResponseModel model = new TermsAndConditiosResponseModel
                {
                    Status = response.status,
                    Message = response.message,
                    Data = record
                };
                return View(model);
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorRefundAndReturnPolicy = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                var model1 = new TermsAndConditiosResponseModel();
                return View(model1);
            }
        }

        #endregion

        #region Sign Contract
        public IActionResult AuthenticateContractNew()
        {
            ViewBag.errorDownloadContract = "";
            var json = HttpContext.Session.GetString("ContractDownloadModel");
            if (string.IsNullOrEmpty(json))
            {
                ViewBag.errorDownloadContract = "No data found.";
                return View();
            }
            var model = JsonConvert.DeserializeObject<OfferLetterDownload>(json!);
            var contractNo = HttpContext.Session.GetString("ContractNo");
            var contractID = HttpContext.Session.GetString("ContractID");
            var dueActionID = HttpContext.Session.GetString("DueActionID");
            var CaseNo = HttpContext.Session.GetString("CaseNo");
            var StageID = HttpContext.Session.GetString("StageID");

            ViewBag.ContractNo = contractNo;
            ViewBag.ContractID = contractID;
            ViewBag.DueActionID = dueActionID;
            ViewBag.CaseNO = CaseNo;
            ViewBag.StageID = StageID;
            return View(model);
        }

        public async Task<IActionResult> ContractDownloadNew([FromBody] ContractRenewParamWithCaseNO param)
        {
            try
            {
                ViewBag.errorDownloadContract = "";
                var postUrl = apiConfig.contractDownload;
                HttpContext.Session.SetString("ContractNo", param?.ContractNo ?? "");
                HttpContext.Session.SetString("ContractID", param?.ContractId ?? "");
                HttpContext.Session.SetString("DueActionID", param?.DueActionID ?? "");
                HttpContext.Session.SetString("CaseNo", param?.CaseNo ?? "");
                HttpContext.Session.SetString("StageID", param?.StageID ?? "");
                var data = new ContractDetailparameter
                {
                    ContractId = param?.ContractId
                };
                var response = await renewalRepo.DownloadContract(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorContractRenew = response;
                    return Json(new
                    {
                        success = false,
                        redirectUrl = Url.Action("")
                    });
                }
                OfferLetterDownload apiResponse = JsonConvert.DeserializeObject<OfferLetterDownload>(response.ToString());

                string jsonData = JsonConvert.SerializeObject(apiResponse);
                HttpContext.Session.SetString("ContractDownloadModel", jsonData);
                var jsonRes = HttpContext.Session.GetString("ContractDownloadModel");
                Console.WriteLine(jsonRes);
                var js = Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("AuthenticateContractNew") // or "ContractRenew"
                });
                return js;
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorDownloadContract = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                var model1 = Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("") // or "ContractRenew"
                });
                return model1;
            }
        }

        public async Task<OfferLetterDownload> getContractTermsNew([FromBody] ContractDetailparameter data)
        {
            try
            {
                ViewBag.errorContractTerms = "";
                var postUrl = apiConfig.downloadContractTerms;
                var data1 = data;
                var response = await renewalRepo.ContractOffer(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorContractTerms = response;
                    var model1 = new OfferLetterDownload
                    {
                        status = "",
                        path = "",
                    };
                    return model1;
                }
                Console.WriteLine(response);
                OfferLetterDownload apiResponse = JsonConvert.DeserializeObject<OfferLetterDownload>(response.ToString());

                var model = new OfferLetterDownload
                {
                    status = apiResponse.status,
                    path = apiResponse.path,
                };
                return model;
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorContractTerms = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                var model1 = new OfferLetterDownload
                {
                    status = "",
                    path = "",
                };
                return model1;
            }
        }

        #endregion

        #region Municiplity Approval
        public async Task<IActionResult> MunicipalityApprovalNew(string ContractId, string ContractNo, string DueActionID)
        {
            try
            {
                ViewBag.errorMuniciplityApproval = "";
                var postUrl = apiConfig.getApproveMunicipal;
                ViewBag.ContractNo = ContractNo.ToString();
                ViewBag.ContractID = ContractId.ToString();
                ViewBag.DueActionID = DueActionID.ToString();
                var data = new ContractDetailparameter
                {
                    ContractId = ContractId
                };
                var response = await renewalRepo.MuniciplityApproval(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorMuniciplityApproval = response;
                    var model = new MunicipalInstructions();
                    return View(model);
                }
                MunicipalInstructions apiResponse = JsonConvert.DeserializeObject<MunicipalInstructions>(response.ToString());
                var model1 = new MunicipalInstructions
                {
                    Record = apiResponse.Record,
                    Status = apiResponse.Status,
                    StatusCode = apiResponse.StatusCode,
                    Message = apiResponse.Message
                };
                return View(model1);
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorMuniciplityApproval = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                var model1 = new MunicipalInstructions();
                return View(model1);
            }
        }


        #endregion
        #endregion

        public async Task<string?> SaveAttachmentAsync(IFormFile attachment)
        {
            if (attachment == null || attachment.Length == 0)
            { return null; }

            // Example: save inside wwwroot/Uploads/
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");

            // Create folder if not exists
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Generate unique file name
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(attachment.FileName);

            // Full file path
            string filePath = Path.Combine(uploadsFolder, fileName);

            // Save file to path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await attachment.CopyToAsync(stream);
            }

            return filePath; // This is your file physical path
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}

