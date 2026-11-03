using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using UBSProptTenantPortal.Models;
using UBSProptRepository.Repository;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Options;
using System.Net.NetworkInformation;
using System.Reflection;
using static Google.Apis.Requests.BatchRequest;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace UBSProptTenantPortal.Areas.Contracts.Controllers
{
    [Area("ServiceRequests")]
    public class ServiceRequestController : BaseController
    {
        private readonly ServiceRequestRepository serviceReqRepo;
        private readonly UBSTenantApiConfig apiConfig;
        new string? token = "";

        public ServiceRequestController(ServiceRequestRepository _serviceReqRepo, IOptions<UBSTenantApiConfig> _apiConfig)
        {
            serviceReqRepo = _serviceReqRepo;
            apiConfig = _apiConfig.Value;
            token = AuthLabelsPortal.jwtToken ?? "";
        }


        public async Task<IActionResult> ServiceRequests(string tab)
        {
            ServiceRequestViewModel viewModel = new ServiceRequestViewModel();
            ViewBag.TabName = tab;
            try {
                viewModel.fmRequest = await GetServiceRequestFM("", "");
                for (int i = 0; i < viewModel?.fmRequest?.ServiceRequests?.Count; i++) {
                    viewModel.fmRequest.ServiceRequests[i].tabName = "FM";
                }
            }
            catch (Exception ex) { Console.WriteLine($"Error retrieving FM data: {ex.Message}"); }

            try
            {
                viewModel!.pmRequest = await GetServiceRequestPM("", "");

                for (int i = 0; i < viewModel?.pmRequest?.ServiceRequests?.Count; i++)
                {
                    viewModel.pmRequest.ServiceRequests[i].tabName = "PM";
                }
            }
            catch (Exception ex) { Console.WriteLine($"Error retrieving PM data: {ex.Message}"); }

            return View(viewModel);
        }

        public async Task<IActionResult> ServiceRequestsFilter(string? fromDate, string? toDate, string type)
        {
            try
            {
                ViewBag.TabName = type;
                if (type == "FM")
                {
                    var fm = await GetServiceRequestFM(fromDate ?? "", toDate ?? "");
                    return PartialView("_ServiceRequestTable", fm?.ServiceRequests);
                }
                else
                {
                    var pm = await GetServiceRequestPM(fromDate ?? "", toDate ?? "");
                    return PartialView("_ServiceRequestTable", pm?.ServiceRequests);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving filtered data: {ex.Message}");
                return Content($@"
                    <div class='card-header pb-0' style='flex-shrink: 0;'>
                        <center style='padding-top: 50px;'>
                            <h4>Service Request</h4>
                            <p style='padding-top: 5px;'>{ex.Message}</p>
                        </center>
                        <div style='height:20px'></div>
                    </div>", "text/html");
             }
        }

        public async Task<ServiceRequestsResponse> GetServiceRequestFM(string fromDate,string toDate)
        {
            try
            {
                ViewBag.errorGetServiceReqPMFM = "";
                fromDate = fromDate.Replace('/', '-');
                toDate = toDate.Replace('/', '-');
                var data = new GetServiceRequestParameterN
                {
                    type = "FM",
                    fromDate = fromDate,
                    toDate = toDate,
                    search = ""
                };
                var postUrl = apiConfig.getServiceRequests;
                var response = await serviceReqRepo.GetServiceRequests(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorGetServiceReqPMFM = response;
                    ServiceRequestsResponse model1 = new ServiceRequestsResponse
                    {
                        Status = "",
                        ServiceRequests = null,
                        Message = ""
                    };
                    return model1;
                }
                Console.WriteLine(response);
                ServiceRequestsResponse apiResponse = JsonConvert.DeserializeObject<ServiceRequestsResponse>(response.ToString());
                var dataList = apiResponse.ServiceRequests ?? new List<ServiceRequest>();
                if (dataList.Count == 0) { ViewBag.errorGetServiceReqPMFM = "No Data Found"; }
               
               
                var model = new ServiceRequestsResponse
                {
                    Status = apiResponse.Status,
                    ServiceRequests = dataList, //apiResponse.ServiceRequests ?? new List<ServiceRequest>(),
                    Message = apiResponse.Message,
                };
                Console.WriteLine(model.Status);
                Console.WriteLine(model.Message);
                Console.WriteLine(model.ServiceRequests);
                return model;
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorGetServiceReqPMFM = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                return new ServiceRequestsResponse();
            }
        }


        public async Task<ServiceRequestsResponse> GetServiceRequestPM(string fromDate, string toDate)
        {
            try
            {
                ViewBag.errorGetServiceReqPMFM = "";
                fromDate = fromDate.Replace('/', '-');
                toDate = toDate.Replace('/', '-');
                var data = new GetServiceRequestParameterN
                {
                    type = "PM",
                    fromDate = fromDate,
                    toDate = toDate,
                    search = ""
                };
                var postUrl = apiConfig.getServiceRequests;
                var response = await serviceReqRepo.GetServiceRequests(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorGetServiceReqPMFM = response;
                    ServiceRequestsResponse model1 = new ServiceRequestsResponse
                    {
                        Status = "",
                        ServiceRequests = null,
                        Message = ""
                    };
                    return model1;
                }
                Console.WriteLine(response);
                ServiceRequestsResponse apiResponse = JsonConvert.DeserializeObject<ServiceRequestsResponse>(response.ToString());
                var dataList = apiResponse.ServiceRequests ?? new List<ServiceRequest>();
                if (dataList.Count == 0) { ViewBag.errorGetServiceReqPMFM = "No Data Found"; }
               
                var model = new ServiceRequestsResponse
                {
                    Status = apiResponse.Status,
                    ServiceRequests = dataList,
                    Message = apiResponse.Message,
                };
                Console.WriteLine(model.Status);
                Console.WriteLine(model.Message);
                Console.WriteLine(model.ServiceRequests);
                return model;
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorGetServiceReqPMFM = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                return new ServiceRequestsResponse();
            }
        }






        #region Detail
        public async Task<IActionResult> ServiceRequestDetailN(string caseNo, string tab, string callingFrom)
        {
            ViewBag.errorMainInfo = "";
            //ViewBag.errorDocuments = "";
            //ViewBag.errorCommunications = "";
            ViewBag.CallingFrom = callingFrom;
            var tabName = tab ?? "";
            ViewBag.TabName = tabName;

            ViewData["ServiceReqNoTitle"] = caseNo;
            ViewData["SelectedTab"] = tab;
            SrReqDetailMainDocumentCommunication viewModel = new SrReqDetailMainDocumentCommunication();
            viewModel.mainInfo = await ServiceRequestDetailFunc(caseNo);
            //viewModel.uploadDocResponse = await ServiceRequestDocumentsN(caseNo);
            //viewModel.CoomunicationResponse = await ServiceRequestGetTicketReplies(caseNo);
            return View(viewModel);
        }
        public async Task<IActionResult> _ServiceRequestMainInfo(string caseNo)
        {
            var docs = await ServiceRequestDetailFunc(caseNo);
            return PartialView("_MainInfoPartial", docs);
        }
        public async Task<IActionResult> _ServiceRequestDocuments(string caseNo)
        {
            ViewBag.ServiceRequestNo = caseNo;
            var docs = await ServiceRequestDocumentsN(caseNo);
            return PartialView("_DocumentPartial", docs);
        }
        public async Task<IActionResult> _ServiceRequestCommunications(string caseNo)
        {
            //var comms = await ServiceRequestGetTicketReplies(caseNo);
            //return PartialView("_CommunicationPartial", comms);
            try
            {
                var comms = await ServiceRequestGetTicketReplies(caseNo);
                return PartialView("_CommunicationPartial", comms);
            }
            catch (Exception ex)
            {
                var errorModel = new CommunicationModel
                {
                    errorMessage = "Failed to load communications: " + ex.Message
                };
                return PartialView("_CommunicationPartial", errorModel);
            }
        }

        //public async Task<IActionResult> ServiceRequestDetailN(string caseNo, int tab, string callingFrom)
        //{
        //    ViewBag.errorMainInfo = "";
        //    ViewBag.errorDocuments = "";
        //    ViewBag.errorCommunications = "";
        //    ViewBag.CallingFrom = callingFrom;
        //    ViewData["ServiceReqNoTitle"] = caseNo;
        //    ViewData["SelectedTab"] = tab;
        //    SrReqDetailMainDocumentCommunication viewModel = new SrReqDetailMainDocumentCommunication();
        //    viewModel.mainInfo = await ServiceRequestDetailFunc(caseNo);
        //    viewModel.uploadDocResponse = await ServiceRequestDocumentsN(caseNo);
        //    viewModel.CoomunicationResponse = await ServiceRequestGetTicketReplies(caseNo);
        //    return View(viewModel);
        //}
        public async Task<IActionResult> ServiceRequestDetail(string caseNo, int tab,string callingFrom)
        {
            ViewBag.errorMainInfo = "";
            ViewBag.errorDocuments = "";
            ViewBag.errorCommunications = "";
            ViewBag.CallingFrom = callingFrom;
            ViewData["ServiceReqNoTitle"] = caseNo;
            ViewData["SelectedTab"] = tab;
            SrReqDetailMainDocumentCommunication viewModel = new SrReqDetailMainDocumentCommunication();
            viewModel.mainInfo = await ServiceRequestDetailFunc(caseNo);
            viewModel.uploadDocResponse = await ServiceRequestDocuments(caseNo); 
            viewModel.CoomunicationResponse = await ServiceRequestGetTicketReplies(caseNo);
            return View(viewModel);
        }

        #endregion

        #region MainInfo
        public async Task<ServiceRequestDetailsTModel> ServiceRequestDetailFunc(string caseNo)
        {
            try
            {
                ViewBag.errorMainInfo = "";
                var data = new ServiceReqDetailparameterCaseNo
                {
                    CaseNo = caseNo,
                };
                var postUrl = apiConfig.getServiceRequestsDetail;
                var response = await serviceReqRepo.GetServiceRequestsDetails(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorMainInfo = response;
                    ServiceRequestDetailsTModel model = new ServiceRequestDetailsTModel();
                    return model;
                }

                if (response.status == "Ok")
                {
                    Console.WriteLine(response);

                    // photos
                    List<PhotoFileTen?> photos = new List<PhotoFileTen?>();
                    if (response?.detail?.RequestType != "FM") {
                      photos =  await ServiceRequestDetailGetPhotos(caseNo);
                    }
                    // Survey
                    SurveyQuestions surveyModel = new SurveyQuestions();
                    if (response?.statusInfo?.canTakeSurvey == true)
                    {
                        surveyModel = await ServiceRequestDetailGetSurveyQuestions(caseNo);
                        
                    }
                    // Feedback
                    TenantGetSrFeedback feedbackModel = new TenantGetSrFeedback();
                    if (response?.detail?.caseFeedback != 0)
                    {
                        feedbackModel = await ServiceRequestDetailGetFeedBack(caseNo);
                    }



                    List<DocFile> reqReport = await ServiceRequestDetailGetFiles(caseNo);

                    ServiceRequestDetailsTModel model = new ServiceRequestDetailsTModel
                    {
                        Status = response?.status,
                        Detail = ((JObject)response?.detail!).ToObject<Detail>(),
                        StatusInfo = ((JObject)response.statusInfo).ToObject<StatusInfo>(),
                        Message = response.message,
                        ContractInfo = ((JObject)response.contractInfo).ToObject<ContractInfo>(),
                        StageInfo = ((JObject)response.stageInfo).ToObject<StageInfo>(),
                        reqPhotos = photos!,
                        surveyQAs = surveyModel,
                        feedback = feedbackModel,
                        reqReport = reqReport
                    };
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine(model?.Status);
                    Console.WriteLine(model?.Message);
                    Console.WriteLine("-------------------------------");
                    return model!;
                }
                else
                {
                    ViewBag.errorMainInfo = response.message;
                    ServiceRequestDetailsTModel model = new ServiceRequestDetailsTModel();
                    return model;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorMainInfo = e.Message;
                ServiceRequestDetailsTModel model = new ServiceRequestDetailsTModel();
                return model;
            }

        }

        #region Photos
        public async Task<IActionResult> UploadPhoto(string caseNo)
        {
            try
            {
                ViewBag.errorMainInfoPhotos = "";
                ViewBag.caseNo = caseNo;
                var data = new ServiceReqDetailPhotosparameter
                {
                    CaseNo = caseNo,
                    RoleId = "1"
                };
                var postUrl = apiConfig.getServiceRequestThumbnailList;
                var response = await serviceReqRepo.GetServiceRequestsDetailsPhotos(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorMainInfoPhotos = response;
                    List<PhotoFileTen?> model = new List<PhotoFileTen?>();

                    return View(null);
                }

                if (response.status == "200")
                {
                    Console.WriteLine(response);
                    var dataArray = response["data"] as JArray;
                    if (dataArray != null)
                    {
                        List<PhotoFileTen?> photos = dataArray
                            .Select(x => x?.ToObject<PhotoFileTen>())
                            .ToList();

                        if (photos.Any())
                        {
                            return View(photos); // or return Ok(photos) if you're in an API controller
                        }
                        else
                        {
                            List<PhotoFileTen?> model = new List<PhotoFileTen?>();
                            return View(photos);
                        }
                    }
                    else
                    {
                        List<PhotoFileTen?> model = new List<PhotoFileTen?>();
                        return View(model);
                    }
                }
                else
                {
                    ViewBag.errorMainInfoPhotos = response.message;
                    List<PhotoFileTen?> model = new List<PhotoFileTen?>();
                    return View(model);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorMainInfoPhotos = e.Message;
                List<PhotoFileTen?> model = new List<PhotoFileTen?>();
                return View(model);
            }

        }


        public async Task<List<PhotoFileTen?>> ServiceRequestDetailGetPhotos(string caseNo)
        {
            try
            {
                ViewBag.errorMainInfoPhotos = "";
                var data = new ServiceReqDetailPhotosparameter
                {
                    CaseNo = caseNo,
                    RoleId = "1"
                };
                var postUrl = apiConfig.getServiceRequestThumbnailList;
                var response = await serviceReqRepo.GetServiceRequestsDetailsPhotos(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorMainInfoPhotos = response;
                    List<PhotoFileTen?> model = new List<PhotoFileTen?>();

                    return model;
                }

                if (response.status == "200")
                {
                    Console.WriteLine(response);
                    var dataArray = response["data"] as JArray;
                    if (dataArray != null)
                    {
                        List<PhotoFileTen?> photos = dataArray
                            .Select(x => x?.ToObject<PhotoFileTen>())
                            .ToList();

                        if (photos.Any())
                        {
                            return photos; // or return Ok(photos) if you're in an API controller
                        }
                        else
                        {
                            List<PhotoFileTen?> model = new List<PhotoFileTen?>();
                            return model;
                        }
                    }
                    else
                    {
                        List<PhotoFileTen?> model = new List<PhotoFileTen?>();
                        return model;
                    }
                }
                else
                {
                    ViewBag.errorMainInfoPhotos = response.message;
                    List<PhotoFileTen?> model = new List<PhotoFileTen?>();
                    return model;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorMainInfoPhotos = e.Message;
                List<PhotoFileTen?> model = new List<PhotoFileTen?>();
                return model;
            }

        }
        [HttpPost]
        public async Task<IActionResult> ServiceRequestUploadReportDoc(
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
                expiry = expiry.Trim().Replace('/', '-');
                // Save file temporarily to disk
                string ? filePath = await SaveAttachmentAsync(file);

                // Prepare API URL and token
                var postUrl = apiConfig.UploadServiceRequestFiles;
                var tokenValue = token ?? "";

                // Call repository method
                var response = await serviceReqRepo.GetServiceRequestReportFileUpload(
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

        //public async Task<IActionResult> ServiceRequestUpdateReportDoc(
        //               string caseNo,
        //               string expiry,
        //               string attachementType,
        //               string documentTypeId,
        //               IFormFile file)
        //{
        //    try
        //    {
        //        // If expiry not provided, set default
        //        if (string.IsNullOrEmpty(expiry))
        //        {
        //            expiry = DateTime.UtcNow.ToString("dd-MM-yyyy");

        //        }
        //        expiry = expiry.Trim().Replace('/', '-');
        //        // Save file temporarily to disk
        //        string? filePath = await SaveAttachmentAsync(file);

        //        // Prepare API URL and token
        //        var postUrl = apiConfig.UpdateServiceRequestFiles;
        //        var tokenValue = token ?? "";

        //        // Call repository method
        //        var response = await serviceReqRepo.GetServiceRequestReportFileUpload(
        //            caseNo, attachementType, expiry, documentTypeId, postUrl, tokenValue, filePath ?? ""
        //        );

        //        // Handle string response (usually an error message)
        //        if (response is string)
        //        {
        //            ViewBag.errorDocumentUploadDocPhotos = response;
        //            return Json(new
        //            {
        //                success = false,
        //                message = response
        //            });
        //        }

        //        // Handle valid response object
        //        if (response?.status == "file Uploaded")
        //        {
        //            return Json(new
        //            {
        //                success = true,
        //                message = response?.status,
        //                PhotoId = response?.photoId // you can make this dynamic if needed
        //            });
        //        }
        //        else
        //        {
        //            ViewBag.errorDocumentUploadDocPhotos = response?["responseMessage"];
        //            return Json(new
        //            {
        //                success = false,
        //                message = response?["responseMessage"] ?? "Unknown server error"
        //            });
        //        }
        //    }
        //    catch (HttpRequestException e)
        //    {
        //        ViewBag.errorDocumentUploadDocPhotos = e.Message;
        //        return Json(new
        //        {
        //            success = false,
        //            message = e.Message
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.errorDocumentUploadDocPhotos = ex.Message;
        //        return Json(new
        //        {
        //            success = false,
        //            message = "Unexpected error: " + ex.Message
        //        });
        //    }
        //}

        public async Task<IActionResult> ServiceRequestUpdateReportDoc(
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
                expiry = expiry.Trim().Replace('/', '-');
                // Save file temporarily to disk
                string? filePath = await SaveAttachmentAsync(file);

                // Prepare API URL and token
                var postUrl = apiConfig.UpdateServiceRequestFiles;
                var tokenValue = token ?? "";

                // Call repository method
                var response = await serviceReqRepo.GetServiceRequestReportFileUpload(
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


        public async Task<IActionResult> ServiceRequestDeleteReportDoc(string photoId)
        {
            try
            {
                // Prepare API URL and token
                var postUrl = apiConfig.removeSvcReqPhoto;
                var tokenValue = token ?? "";
                ViewBag.errorGetServiceReqPMFM = "";
                var data = new ServiceRequestDeletePhotoParameter
                {
                    PhotoId = photoId,
                };
                var response = await serviceReqRepo.GetServiceRequests(data, postUrl, token ?? "");
                if (response is string)
                {
                    return Json(new
                    {
                        success = false,
                        message = response
                    });
                }
                Console.WriteLine(response);
                Console.WriteLine(response.status);

                return Json(new
                {
                    success = true,
                    message = response.status
                });
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return Json(new
                {
                    success = false,
                    message = e.Message
                });
            }
        }


        #endregion

        #region Survey Q/A
        public async Task<IActionResult> LoadSurveyPartialAsync(string caseNo)
        {
            SurveyQuestions surveyModel = new SurveyQuestions();
            ViewBag.caseNo = caseNo;
            surveyModel = await ServiceRequestDetailGetSurveyQuestions(caseNo);
            // You can load data here
            return PartialView("_SurveyPartial", surveyModel);
        }
        public async Task<SurveyQuestions> ServiceRequestDetailGetSurveyQuestions(string caseNo)
        {
            SurveyQuestions surveyQuestions = new SurveyQuestions();
            try
            {
                ViewBag.errorMainInfoSurvey = "";
                var data = new ServiceReqDetailparameterCaseNo
                {
                    CaseNo = caseNo,
                };
                var postUrl = apiConfig.getSurveyQuestions;
                var response = await serviceReqRepo.GetSurveyQuestions(data, postUrl, token ?? "");
             
                if (response is string)
                {
                    ViewBag.errorMainInfoSurvey = response;
                    return surveyQuestions;
                }
                if (response.status == "Ok")
                {
                    Console.WriteLine(response);
                    List<FaqQuestion> dataList = response.faqQuestion.ToObject<List<FaqQuestion>>();
                    //var dataList = new List<FaqQuestion>
                    //{
                    //    new FaqQuestion { QuestionId = 2, Title = "What is your age?", TitleAr = "ما هو عمرك؟" , AnswerId = 1 },
                    //    new FaqQuestion { QuestionId = 3, Title = "Do you agree with our terms?", TitleAr = "هل توافق على الشروط؟", AnswerId = 2 },
                    //    new FaqQuestion { QuestionId = 4, Title = "Rate our service", TitleAr = "قيّم خدمتنا", AnswerId = 3 }
                    //};

                    if (!dataList.Any() || dataList == null) {
                       ViewBag.errorMainInfoSurvey = "No Survey Question/Answer Found";
                        return surveyQuestions;
                    } else {

                        surveyQuestions.FaqQuestion = dataList!;
                        SurveyQuestionAnswers answers = new SurveyQuestionAnswers();
                        var qIDFirst = dataList.FirstOrDefault()?.QuestionId ?? 0;

                        HttpContext.Session.SetInt32("currentQuestion", 0);
                        var currentQuestion = HttpContext.Session.GetInt32("currentQuestion") ?? 0;
                        surveyQuestions.currentQuestion = currentQuestion;

                        HttpContext.Session.SetInt32("totalQuestions", dataList.Count);
                        var totalQuestions = HttpContext.Session.GetInt32("totalQuestions") ?? 0;

                        var json = JsonConvert.SerializeObject(surveyQuestions);
                        HttpContext.Session.SetString("SurveyQuestions", json);
                        var getjson = HttpContext.Session.GetString("SurveyQuestions");


                        answers =  await ServiceRequestDetailGetSurveyAnswer(dataList[0]?.QuestionId ?? -1);

                        answers.Progress = 1.0 / dataList.Count;

                      
                        surveyQuestions!.FaqQuestion![currentQuestion]!.Answers = answers;

                        return surveyQuestions;
                    }
                }
                else
                {
                    ViewBag.errorMainInfoSurvey = response.message;
                    return surveyQuestions;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorMainInfoSurvey = e.Message;
                return surveyQuestions;
            }
        }

        public async Task<SurveyQuestionAnswers> ServiceRequestDetailGetSurveyAnswer(int questionID)
        {
            SurveyQuestionAnswers surveyAnswers = new SurveyQuestionAnswers();
            try
            {
                ViewBag.errorMainInfoSurvey = "";
                var data = new ServiceReqDetailSurveyparameter
                {
                    QuestionId = questionID.ToString(),
                };
                var postUrl = apiConfig.getSurveyQuestionAnswers;
                var response = await serviceReqRepo.GetSurveyAnswers(data, postUrl, token ?? "");

                if (response is string)
                {
                    ViewBag.errorMainInfoSurvey = response;
                    return surveyAnswers;
                }
                if (response.status == "Ok")
                {
                    Console.WriteLine(response);
                    List<FaqOption> dataList = response.faqOptions.ToObject<List<FaqOption>>();
                   

                    if (dataList.Count == 0 || dataList == null)
                    {
                        ViewBag.errorMainInfoSurvey = "No Survey Found";
                        return surveyAnswers;
                    }
                    else
                    {
                        // Update progress
                        var newCurrentQuestion = HttpContext.Session.GetInt32("currentQuestion") ?? 0;
                        var newTotalQuestions = HttpContext.Session.GetInt32("totalQuestions") ?? 0;
                        surveyAnswers.Progress = (newCurrentQuestion + 1.0) / newTotalQuestions;

                        surveyAnswers.FaqOptions = dataList;
                        return surveyAnswers;
                    }
                }
                else
                {
                    ViewBag.errorMainInfoSurvey = response.message;
                    return surveyAnswers;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorMainInfoSurvey = e.Message;
                return surveyAnswers;
            }
        }

        public async Task<IActionResult> NextQuestion([FromBody] SurveyQASaveParam surveyQuestions)
        {
            if (await ServiceRequestDetailSaveAnswer(surveyQuestions))
            {
                var currentQuestion = HttpContext.Session.GetInt32("currentQuestion") ?? 0;
                var totalQuestions = HttpContext.Session.GetInt32("totalQuestions") ?? 0;

                if (currentQuestion < totalQuestions - 1)
                {
                    currentQuestion++;
                    HttpContext.Session.SetInt32("currentQuestion", currentQuestion);
                    var newCurrentQuestion = HttpContext.Session.GetInt32("currentQuestion") ?? 0;
                    

                   var json = HttpContext.Session.GetString("SurveyQuestions");
                    SurveyQuestions surveyQuestionsN = new SurveyQuestions();
                    if (!string.IsNullOrEmpty(json))
                    {
                        surveyQuestionsN = JsonConvert.DeserializeObject<SurveyQuestions>(json)!;
                    }

                    surveyQuestionsN.currentQuestion = newCurrentQuestion;
                    var qId = surveyQuestions.QuestionId ?? -1;
                    var updatedSurveyModel=   await ServiceRequestDetailGetSurveyAnswer(qId);

                   
                    surveyQuestionsN.FaqQuestion![newCurrentQuestion]!.Answers = updatedSurveyModel;

                    // Return fresh partial view
                    return PartialView("_SurveyPartial", surveyQuestionsN);
                }
                else
                {
                    ViewBag.errorMainInfoSurvey = "No more questions";
                    // No more questions
                    SurveyQuestions surveyQuestionsNN = new SurveyQuestions();
                    return PartialView("_SurveyPartial", surveyQuestionsNN);
                }
            }

            ViewBag.errorMainInfoSurvey = "Answer saved failed";
            SurveyQuestions surveyQuestionsNNN = new SurveyQuestions();
            return PartialView("_SurveyPartial", surveyQuestionsNNN);
        }

        public async Task<bool> ServiceRequestDetailSaveAnswer(SurveyQASaveParam surveyQuestions)
        {
            SurveyQuestionAnswers surveyAnswers = new SurveyQuestionAnswers();
            try
            {
                ViewBag.errorMainInfoSurvey = "";
                var data = new ServiceReqDetailSaveSurveyparameter
                { 
                    answerId = surveyQuestions?.answerId?.ToString(),
                    customerChoice = surveyQuestions?.customerChoice?.ToString(),
                    description = surveyQuestions?.description?.ToString(),
                    IsCompleted = surveyQuestions?.IsCompleted?.ToString(),
                };
                var postUrl = apiConfig.saveSurveyAnswer;
                var response = await serviceReqRepo.SaveSurveyAnswers(data, postUrl, token ?? "");

                if (response is string)
                {
                    ViewBag.errorMainInfoSurvey = response;
                    return false;
                }
                if (response.status == "Ok")
                {
                    return true;
                }
                else
                {
                    ViewBag.errorMainInfoSurvey = response.message;
                    return false;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorMainInfoSurvey = e.Message;
                return false;
            }
        }
        #endregion

        #region feedback
        public async Task<TenantGetSrFeedback> ServiceRequestDetailGetFeedBack(string caseNo)
        {

            TenantGetSrFeedback feedbackResp = new TenantGetSrFeedback();
            try
            {
                ViewBag.errorMainInfoFeedback = "";
                var data = new ServiceReqDetailparameterCaseNo
                {
                    CaseNo = caseNo,
                };
                var postUrl = apiConfig.getTenantFeedback;
                var response = await serviceReqRepo.GetServiceRequestsDetailsFeedback(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorMainInfoFeedback = response;
                    return feedbackResp;
                }

                if (response.status == "Ok")
                {
                    Console.WriteLine(response);
                    var feedback1 = response.feedback;

                    if (feedback1 != null)
                    {
                        feedbackResp = JsonConvert.DeserializeObject<TenantGetSrFeedback>(response.ToString());
                        return feedbackResp;
                    }
                    else
                    {
                        ViewBag.errorMainInfoFeedback = "Not Found";
                        return feedbackResp; // return empty model
                    }
                }
                else
                {
                    //var feedback11 = new Feedback
                    //{
                    //    CaseNo = 123,
                    //    Description = "Good work",
                    //    Rating = 4.5
                    //};

                    //feedback.Feedback = feedback11;

                    //return feedback;
                    ViewBag.errorMainInfoFeedback = response.message;
                    return feedbackResp;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorMainInfoFeedback = e.Message;
                return feedbackResp;
            }
        }

        public async Task<IActionResult> SubmitFeedBack([FromBody] SubmitFeedback feedbackM)
        {
            var postUrl = apiConfig.saveTenantFeedback;
            var data = feedbackM;
            var response = await serviceReqRepo.GetServiceRequestsSubmitFeedback(data, postUrl, token ?? "");
            if (response is string)
            {
                ViewBag.errorMainInfoFeedback = response;
                return Json(new
                {
                    success = false,
                    message = "Feedback submitted failed"
                });
            }
            else {
                return Json(new
                {
                    success = true,
                    message = "Feedback submitted Successfully"
                });
            }
        }

        #endregion

        #region Report
        public async Task<List<DocFile?>> ServiceRequestDetailGetFiles(string caseNo)
        {
            try
            {
                ViewBag.errorMainInfoFiles = "";
                var data = new ServiceReqDetailPhotosparameter
                {
                    CaseNo = caseNo,
                    RoleId = "3"
                };
                var postUrl = apiConfig.getServiceRequestDocumentsPdf;
                var response = await serviceReqRepo.GetServiceRequestsDetailsPhotos(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorMainInfoFiles = response;
                    List<DocFile?> model = new List<DocFile?>();
                    return model;
                }

                if (response.status == "200")
                {
                    Console.WriteLine(response);
                    var resp = response;
                    List<DocFile?> reqFiles = new List<DocFile?>();

                    if (resp["path"] != null && resp["path"].HasValues)
                    {
                        var byteDocs = resp["path"].ToObject<List<string>>();
                        var docIds = resp["photoId"].ToObject<List<int>>(); // Assuming these are numbers
                        var attachmentType = resp["attachmentType"].ToObject<List<int>>();
                        for (int i = 0; i < byteDocs.Count; i++)
                        {
                            var base64String = byteDocs[i].Replace("\n", "");
                            byte[] fileBytes = Convert.FromBase64String(base64String);
                            int docId = docIds[i];

                            var docFile = new DocFile
                            {
                                File = fileBytes,
                                Id = docId.ToString(),
                                Name = $"doc{docId}"
                                // You can set additional properties here if available
                            };

                            reqFiles.Add(docFile);
                        }
                        return reqFiles;
                    }
                    else
                    {
                        var list = reqFiles;
                        return list;
                    }

                }
                else
                {
                    ViewBag.errorMainInfoFiles = response.message;
                    List<DocFile?> model = new List<DocFile?>();
                    return model;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorMainInfoFiles = e.Message;
                List<DocFile?> model = new List<DocFile?>();
                return model;
            }

        }


        #endregion

        #region Cancel Service Request
        public async Task<IActionResult> CancelServiceRequestDetail(string caseNo)
        {
            try
            {
                ViewBag.errorReqAcknowledge = "";
                var data = new ServiceReqDetailparameterCaseNo
                {
                    CaseNo = caseNo,
                };
                var postUrl = apiConfig.cancelRequest;
                var response = await serviceReqRepo.GetServiceRequestCancel(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorReqAcknowledge = response;
                    return Json(new
                    {
                        success = false,
                        message = response
                    });
                }

                if (response.status == "Ok")
                {
                    return Json(new
                    {
                        success = true,
                        message = "Cancelled Successfully"
                    });
                }
                else
                {
                    ViewBag.errorReqAcknowledge = response.message;
                    return Json(new
                    {
                        success = false,
                        message = response.message
                    });
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorReqAcknowledge = e.Message;
                return Json(new
                {
                    success = false,
                    message = e.Message
                });
            }
        }
        #endregion

        #endregion

        #region Documnet
        public async Task<UploadDocResponse> ServiceRequestDocumentsN(string caseNo)
        {




            try
            {
                ViewBag.errorDocuments = "";
                var data = new ServiceReqDetailDocsparameter
                {
                    CaseNo = caseNo,
                    RoleId = "1",
                    Code = "41"
                };
                var postUrl = apiConfig.getServiceRequestsDocsByType;
                var response = await serviceReqRepo.GetServiceRequestDocs(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorDocuments = response;
                    UploadDocResponse model = new UploadDocResponse();
                    return model;
                }

                if (response.status == "200")
                {

                    Console.WriteLine(response);
                    List<DocFile> dataList = response.record.ToObject<List<DocFile>>();
                    for (int i = 0; i < dataList.Count; i++)
                    {
                        if (dataList[i].AttachmentUrl == null)
                        {
                            dataList[i].Id = null;
                        }
                        else
                        {
                            dataList[i].Id = dataList[i].AttachmentUrl;
                        }
                    }
                    var result = new UploadDocResponse
                    {
                        ResponseMessage = response.responseMessage,
                        CaseStageInfo = new CaseStageInfo
                        {
                            DueActionId = response.caseStageInfo?.dueActionid?.ToString(),
                            StageId = response.caseStageInfo?.stageId?.ToString()
                        },
                        Docs = dataList,
                    };
                    return result;
                }
                else
                {
                    ViewBag.errorDocuments = response["responseMessage"];
                    UploadDocResponse model = new UploadDocResponse();
                    return model;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorDocuments = e.Message;
                UploadDocResponse model = new UploadDocResponse();
                return model;
            }
        }
        public async Task<UploadDocResponse> ServiceRequestDocuments(string caseNo) {

            try
            {
                ViewBag.errorDocuments = "";
                var data = new ServiceReqDetailDocsparameter
                {
                    CaseNo = caseNo,
                    RoleId = "1",
                    Code = "41"
                };
                var postUrl = apiConfig.getServiceRequestsDocsByType;
                var response = await serviceReqRepo.GetServiceRequestDocs(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorDocuments = response;
                    UploadDocResponse model = new UploadDocResponse();
                    return model;
                }

                if (response.status == "200")
                {
                  
                   Console.WriteLine(response);
                   List<DocFile> dataList = response.record.ToObject<List<DocFile>>();
                    for (int i = 0; i < dataList.Count; i++) {
                        if (dataList[i].AttachmentUrl == null)
                        {
                            dataList[i].Id = null;
                        }
                        else {
                            dataList[i].Id = dataList[i].AttachmentUrl;
                        }
                    }
                   var result = new UploadDocResponse
                        {
                            ResponseMessage = response.responseMessage,
                            CaseStageInfo = new CaseStageInfo
                            {
                                DueActionId = response.caseStageInfo?.dueActionid?.ToString(),
                                StageId = response.caseStageInfo?.stageId?.ToString()
                            },
                            Docs = dataList,
                        };
                    return result;
                    }
                    else
                    {
                        ViewBag.errorDocuments = response["responseMessage"];
                        UploadDocResponse model = new UploadDocResponse();
                        return model;
                    }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorDocuments = e.Message;
                UploadDocResponse model = new UploadDocResponse();
                return model;
            }
        }

        public async Task<DownloadDoc> ServiceRequestDownloadDocuments([FromBody] ServiceReqDetailDownloadDocsparameter param)
        {
            try
            {
                var postUrl = apiConfig.tenantDownloadDoc;
                var data = new ServiceReqDetailDownloadDocsparameter
                {
                    CaseNo = param.CaseNo,
                    RoleId = param.RoleId,
                    PhotoId = param.PhotoId,
                };
                var response = await serviceReqRepo.DownloadServiceRequestFiles(data, postUrl, token ?? "");

                if (response is string)
                {
                    ViewBag.errorDocumentDownloadDocPhotos = response;
                    return new DownloadDoc();
                }

                if (response?.status == "200")
                {
                    Console.WriteLine(response);
                    DownloadDoc apiResponse = JsonConvert.DeserializeObject<DownloadDoc>(response.ToString());

                    var model = new DownloadDoc
                    {
                        status = apiResponse.status,
                        path = apiResponse.path,
                    };
                    return model;
                }
                else
                {
                    ViewBag.errorDocumentDownloadDocPhotos = response?["responseMessage"];
                    return new DownloadDoc();
                }
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorDocumentDownloadDocPhotos = e.Message;
                return new DownloadDoc();
            }
        }

        public async Task<RemoveDoc> ServiceRequestRemoveDocuments([FromBody] ServiceReqDetailRemoveDocsparameter param)
        {
            try
            {

                var postUrl = apiConfig.removeSvcReqPhoto;
                var data = new ServiceReqDetailRemoveDocsparameter
                {
                    PhotoId = param.PhotoId,
                };
                var response = await serviceReqRepo.RemoveServiceRequestFiles(data, postUrl, token ?? "");

                if (response is string)
                {
                    ViewBag.errorremoveSvcReqPhoto = response;
                    return new RemoveDoc();
                }

                if (response?.status == "file Deleted Successfully")
                {
                    Console.WriteLine(response);
                    RemoveDoc apiResponse = JsonConvert.DeserializeObject<RemoveDoc>(response.ToString());
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

      
        public async Task<JsonResult> ServiceRequestUpdateDocumentsStage([FromBody] UpdateDocStage param)
        {
            try
            {

                var postUrl = apiConfig.updateContractDocumentStage;
                var data = new UpdateDocStage
                {
                    DueActionId = param.DueActionId?.ToString(),
                };
                var response = await serviceReqRepo.UpdateServiceRequestDocStage(data, postUrl, token ?? "");

                if (response is string)
                {
                    ViewBag.errorremoveSvcReqPhoto = response;
                     return Json(new { success = false, message = response });
                }

                if (response?["statusCode"] == "200")
                {
                    Console.WriteLine(response);
                    return Json(new { success = true, message = "Stage Updated Successfully"});
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

        public async Task<IActionResult> ServiceRequestUpdateDocument(
             string expiry,
              string attachementId,
             IFormFile file)
        {
            try
            {
                // If expiry not provided, set default
                if (string.IsNullOrEmpty(expiry))
                {
                    expiry = DateTime.UtcNow.ToString("dd-MM-yyyy");

                }
                if (expiry.Contains('/'))
                {
                    expiry = expiry.Replace('/', '-');
                }
                // Save file temporarily to disk
                string? filePath = await SaveAttachmentAsync(file);

                // Prepare API URL and token
                var postUrl = apiConfig.UpdateServiceRequestFiles;
                var tokenValue = token ?? "";

                // Call repository method
                var response = await serviceReqRepo.GetServiceRequestReportEIDFileUpdate(
                    attachementId, expiry, postUrl, tokenValue, filePath ?? ""
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
        #region EmirateID


        public EmirateIDInfo ParseFront(string text)
        {
            var info = new EmirateIDInfo();
           

            // 🔹 Normalize text (remove double spaces, strange chars)
            text = Regex.Replace(text, @"\s+", " ");

            // -------------------------------------------
            // 1. Extract ID Number (handles broken OCR)
            // -------------------------------------------
            // Track what we found
            //bool hasName = false;
            //bool hasNationality = false;
            //bool hasIdNumber = false;
            var idPattern = new Regex(@"\b(\d{3}[- ]?\d{4}[- ]?\d{7}[- ]?\d)\b");
            var idMatch = idPattern.Match(text);
            if (idMatch.Success)
            {
                info.IDNumber = idMatch.Value.Replace(" ", "").Trim();
                //hasIdNumber = true;
            }


            // -------------------------------------------
            // 2. Extract Name (OCR has "Name:" correctly)
            // -------------------------------------------
            var namePattern = new Regex(@"Name[:\- ]+([A-Za-z ]+)", RegexOptions.IgnoreCase);
            var nameMatch = namePattern.Match(text);
            if (nameMatch.Success)
            {  info.FullName = nameMatch.Groups[1].Value.Trim();
                //hasName = true;
            }


            // -------------------------------------------
            // 3. Extract Nationality (OCR has "Nationasty:" / "Nationality")
            // -------------------------------------------
            var nationalityPattern = new Regex(@"Nation[a-z]*[:\- ]+([A-Za-z ]+)", RegexOptions.IgnoreCase);
            var nationalityMatch = nationalityPattern.Match(text);
            if (nationalityMatch.Success)
            {
                info.Nationality = nationalityMatch.Groups[1].Value.Trim();
                //hasNationality = true;
            }



            // Extract ALL date-like patterns
            var dateMatches = Regex.Matches(text, @"\b(\d{2}/\d{2}/\d{2,4}|\d{6})\b");

            var extractedDates = new List<string>();

            foreach (Match dt in dateMatches)
            {
                string raw = dt.Value;

                // Case 1: dd/MM/yyyy or dd/MM/yy
                if (raw.Contains("/"))
                {
                    if (DateOnly.TryParseExact(raw, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly tempDate))
                    {
                        string formatted = tempDate.ToString("dd/MM/yyyy");
                        extractedDates.Add(formatted);
                    }
                }
                // Case 2: YYMMDD (6-digit)
                else if (raw.Length == 6)
                {
                    string yy = raw.Substring(0, 2);
                    string mm = raw.Substring(2, 2);
                    string dd = raw.Substring(4, 2);

                    int year = 2000 + int.Parse(yy); // Adjust if needed

                    string formatted = $"{dd}/{mm}/{year}";

                    if (DateOnly.TryParseExact(raw, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly tempDate))
                    {
                        string formattedDate = tempDate.ToString("dd/MM/yyyy");
                        extractedDates.Add(formattedDate);
                    }
                }
            }

            //bool hasDates = false;
            //bool hasSex = false;


            // Sort dates
            extractedDates = extractedDates.OrderBy(d => d).ToList();

            // Oldest = DOB
            if (extractedDates.Count > 0)
            { info.DateOfBirth = extractedDates.First();
                //hasDates = true;
            }

            // Newest = Expiry
            if (extractedDates.Count > 1)
            {
                info.IssuingDate = extractedDates.Last();
            }

            // Newest = Expiry
            if (extractedDates.Count > 2)
            {
                info.ExpiryDate = extractedDates.Last();
                info.ExpiryDateConfirmation = extractedDates.Last();
            }

            // Gender on back side
            var sexMatch = Regex.Match(text, @"Sex\s*[:\-]?\s*(M|F)", RegexOptions.IgnoreCase);
            if (sexMatch.Success)
            {
                info.Sex = sexMatch.Groups[1].Value;
                //hasSex = true;
            }

            //// Front side should have Name AND Nationality
            //if (hasName && hasNationality)
            //{
            //    info.DetectedSide = "front";
            //}
            //else if (hasIdNumber) // Has ID but not name/nationality - might be back
            //{
            //    info.DetectedSide = "unknown";
            //}
            //else
            //{
            //    info.DetectedSide = "unknown";
            //}

            return info;
        }
        public EmirateIDInfo ParseBack(string text)
        {
            var info = new EmirateIDInfo();

            // Extract ALL date-like patterns
            //var dateMatches = Regex.Matches(text, @"\b(\d{2}/\d{2}/\d{2,4}|\d{6})\b");

            //var extractedDates = new List<string>();

            //foreach (Match dt in dateMatches)
            //{
            //    string raw = dt.Value;

            //    // Case 1: dd/MM/yyyy or dd/MM/yy
            //    if (raw.Contains("/"))
            //    {
            //        if (DateOnly.TryParseExact(raw, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly tempDate))
            //        {
            //            string formatted = tempDate.ToString("dd/MM/yyyy");
            //            extractedDates.Add(formatted);
            //        }
            //    }
            //    // Case 2: YYMMDD (6-digit)
            //    else if (raw.Length == 6)
            //    {
            //        string yy = raw.Substring(0, 2);
            //        string mm = raw.Substring(2, 2);
            //        string dd = raw.Substring(4, 2);

            //        int year = 2000 + int.Parse(yy); // Adjust if needed

            //        string formatted = $"{dd}/{mm}/{year}";

            //        if (DateOnly.TryParseExact(raw, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly tempDate))
            //        {
            //            string formattedDate = tempDate.ToString("dd/MM/yyyy");
            //            extractedDates.Add(formattedDate);
            //        }
            //    }
            //}

            ////bool hasDates = false;
            ////bool hasSex = false;


            //// Sort dates
            //extractedDates = extractedDates.OrderBy(d => d).ToList();

            //// Oldest = DOB
            //if (extractedDates.Count > 0)
            //{ info.DateOfBirth = extractedDates.First();
            //    //hasDates = true;
            //}

            //// Newest = Expiry
            //if (extractedDates.Count > 1)
            //{
            //    info.ExpiryDate = extractedDates.Last();
            //    info.ExpiryDateConfirmation = extractedDates.Last();
            //}


            // Gender on back side
            //var sexMatch = Regex.Match(text, @"Sex\s*[:\-]?\s*(M|F)", RegexOptions.IgnoreCase);
            //if (sexMatch.Success)
            //{
            //    info.Sex = sexMatch.Groups[1].Value;
            //    hasSex = true;
            //}


            //// Back side should have dates OR sex field
            //if (hasDates || hasSex)
            //{
            //    info.DetectedSide = "back";
            //}
            //else
            //{
            //    info.DetectedSide = "unknown";
            //}

            return info;
        }



        //public EmirateIDInfo ParseFront(string text)
        //{
        //    var info = new EmirateIDInfo();


        //    // 🔹 Normalize text (remove double spaces, strange chars)
        //    text = Regex.Replace(text, @"\s+", " ");

        //    // -------------------------------------------
        //    // 1. Extract ID Number (handles broken OCR)
        //    // -------------------------------------------
        //    // Track what we found
        //    bool hasName = false;
        //    bool hasNationality = false;
        //    bool hasIdNumber = false;
        //    var idPattern = new Regex(@"\b(\d{3}[- ]?\d{4}[- ]?\d{7}[- ]?\d)\b");
        //    var idMatch = idPattern.Match(text);
        //    if (idMatch.Success)
        //    {
        //        info.IDNumber = idMatch.Value.Replace(" ", "").Trim();
        //        hasIdNumber = true;
        //    }


        //    // -------------------------------------------
        //    // 2. Extract Name (OCR has "Name:" correctly)
        //    // -------------------------------------------
        //    var namePattern = new Regex(@"Name[:\- ]+([A-Za-z ]+)", RegexOptions.IgnoreCase);
        //    var nameMatch = namePattern.Match(text);
        //    if (nameMatch.Success)
        //    {
        //        info.FullName = nameMatch.Groups[1].Value.Trim(); hasName = true;
        //    }


        //    // -------------------------------------------
        //    // 3. Extract Nationality (OCR has "Nationasty:" / "Nationality")
        //    // -------------------------------------------
        //    var nationalityPattern = new Regex(@"Nation[a-z]*[:\- ]+([A-Za-z ]+)", RegexOptions.IgnoreCase);
        //    var nationalityMatch = nationalityPattern.Match(text);
        //    if (nationalityMatch.Success)
        //    {
        //        info.Nationality = nationalityMatch.Groups[1].Value.Trim(); hasNationality = true;
        //    }

        //    // Front side should have Name AND Nationality
        //    if (hasName && hasNationality)
        //    {
        //        info.DetectedSide = "front";
        //    }
        //    else if (hasIdNumber) // Has ID but not name/nationality - might be back
        //    {
        //        info.DetectedSide = "unknown";
        //    }
        //    else
        //    {
        //        info.DetectedSide = "unknown";
        //    }

        //    return info;
        //}
        //public EmirateIDInfo ParseBack(string text)
        //{
        //    var info = new EmirateIDInfo();

        //    // Extract ALL date-like patterns
        //    var dateMatches = Regex.Matches(text, @"\b(\d{2}/\d{2}/\d{2,4}|\d{6})\b");

        //    var extractedDates = new List<string>();

        //    foreach (Match dt in dateMatches)
        //    {
        //        string raw = dt.Value;

        //        // Case 1: dd/MM/yyyy or dd/MM/yy
        //        if (raw.Contains("/"))
        //        {
        //            if (DateOnly.TryParseExact(raw, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly tempDate))
        //            {
        //                string formatted = tempDate.ToString("dd/MM/yyyy");
        //                extractedDates.Add(formatted);
        //            }
        //        }
        //        // Case 2: YYMMDD (6-digit)
        //        else if (raw.Length == 6)
        //        {
        //            string yy = raw.Substring(0, 2);
        //            string mm = raw.Substring(2, 2);
        //            string dd = raw.Substring(4, 2);

        //            int year = 2000 + int.Parse(yy); // Adjust if needed

        //            string formatted = $"{dd}/{mm}/{year}";

        //            if (DateOnly.TryParseExact(raw, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly tempDate))
        //            {
        //                string formattedDate = tempDate.ToString("dd/MM/yyyy");
        //                extractedDates.Add(formattedDate);
        //            }
        //        }
        //    }

        //    bool hasDates = false;
        //    bool hasSex = false;


        //    // Sort dates
        //    extractedDates = extractedDates.OrderBy(d => d).ToList();

        //    // Oldest = DOB
        //    if (extractedDates.Count > 0)
        //    { info.DateOfBirth = extractedDates.First(); hasDates = true; }

        //    // Newest = Expiry
        //    if (extractedDates.Count > 1)
        //    {
        //        info.ExpiryDate = extractedDates.Last();
        //        info.ExpiryDateConfirmation = extractedDates.Last();
        //    }


        //    // Gender on back side
        //    var sexMatch = Regex.Match(text, @"Sex\s*[:\-]?\s*(M|F)", RegexOptions.IgnoreCase);
        //    if (sexMatch.Success)
        //    {
        //        info.Sex = sexMatch.Groups[1].Value;
        //        hasSex = true;
        //    }


        //    // Back side should have dates OR sex field
        //    if (hasDates || hasSex)
        //    {
        //        info.DetectedSide = "back";
        //    }
        //    else
        //    {
        //        info.DetectedSide = "unknown";
        //    }

        //    return info;
        //}

        [HttpPost]
        public JsonResult ValidateDate(string dateString)
        {
            string[] formats = { "dd/MM/yyyy", "dd-MM-yyyy", "MM/dd/yyyy" };
            DateTime date;

            bool isValid = DateTime.TryParseExact(
                dateString,
                formats,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out date
            );

            return Json(new { isValid = isValid });
        }

        [HttpPost]
        public async Task<IActionResult> ServiceRequestUploadEIDDoc(
               string caseNo,
               string expiry,
               string attachementType,
               string documentTypeId,
               string emirateIdNumber,
               string nationality,
               string nameEng,
               string nameAr,
               string dOB,
               IFormFile file)
        {
            try
            {
                if (string.IsNullOrEmpty(expiry))
                {
                    expiry = DateTime.UtcNow.ToString("dd-MM-yyyy");
                }
                expiry = expiry.Trim().Replace('/', '-');
                dOB = dOB.Trim().Replace('/', '-');

                string? filePath = await SaveAttachmentAsync(file);

                var postUrl = apiConfig.UploadServiceRequestFiles;
                var tokenValue = token ?? "";

                var response = await serviceReqRepo.GetServiceRequestReportEIDFileUpload(
                    caseNo, attachementType, expiry, documentTypeId, postUrl, tokenValue, filePath ?? "", emirateIdNumber, nationality, nameEng, nameAr, dOB
                );

                if (response is string)
                {
                    ViewBag.errorDocumentUploadDocPhotos = response;
                    return Json(new
                    {
                        success = false,
                        message = response
                    });
                }

                if (response?.status == "file Uploaded")
                {
                    return Json(new
                    {
                        success = true,
                        message = response?.status,
                        PhotoId = response?.photoId 
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

        public IActionResult ValidateIDNumber([FromBody] IDNumberData param)
        {
            if (string.IsNullOrEmpty(param.IDNumber) || param.IDNumber.Length != 15)
            {
                return Json(new { success = false, message = "Invalid ID Number" });
            }

            return Json(new { success = true });
        }

        public class IDNumberData
        {
            public string? IDNumber { get; set; }
        }
        #endregion


        #endregion

        #region communication
        public async Task<CommunicationModel> ServiceRequestGetTicketReplies(string CaseId)
        {
            try
            {
                ViewBag.errorCommunication = "";
                var data = new ServiceReqDetailCaseIdparameter
                {
                    CaseId = CaseId,
                };
                var postUrl = apiConfig.getServiceRequestsTicketReplies;
                var response = await serviceReqRepo.GetServiceRequestGetTickekrtReplies(data, postUrl, token ?? "");
                CommunicationModel apiResponse = JsonConvert.DeserializeObject<CommunicationModel>(response.ToString());

                if (response is string)
                {
                    ViewBag.errorCommunication = response;
                    CommunicationModel model = new CommunicationModel();
                    return model;
                }

                if (response.status == "Ok")
                {
                    Console.WriteLine(response);
                    CommunicationModel model = new CommunicationModel
                    {
                        Status = response.status,
                        TicketReply = apiResponse.TicketReply ?? new List<TicketReply>(),
                        Message = response.message,
                    };
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine(model?.Status);
                    Console.WriteLine(model?.Message);
                    Console.WriteLine("-------------------------------");
                    return model!;
                }
                else
                {
                    ViewBag.errorCommunication = response.message;
                    CommunicationModel model = new CommunicationModel();
                    return model;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorCommunication = e.Message;
                CommunicationModel model = new CommunicationModel();
                return model;
            }

        }

      
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

        public async Task<CommunicationModel?> SendMessageAsync(string Message,string CaseNo, IFormFile Attachment)
        {
            try { 
            var postUrl = apiConfig.getServiceRequestsAddTicketReply;
                string? filePath = await SaveAttachmentAsync(Attachment);
                var response = await serviceReqRepo.GetServiceRequestAddTickekrtReplies(CaseNo, Message, postUrl, token ?? "", filePath??"");
            CommunicationModel apiResponse = JsonConvert.DeserializeObject<CommunicationModel>(response.ToString());

            if (response is string)
            {
                ViewBag.errorCommunication = response;
                return null;
            }

            if (response.status == "Ok")
            {
                Console.WriteLine(response);
                SrReqDetailMainDocumentCommunication viewModel = new SrReqDetailMainDocumentCommunication();
                viewModel.CoomunicationResponse = await ServiceRequestGetTicketReplies(CaseNo);
                    return apiResponse;
                }
            else
            {
                ViewBag.errorCommunication = response.message;
                    return null;
                }
        }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorCommunication = e.Message;
                return null;
            }
        }
        public async Task<DownloadDoc> GetTikcketReplyDownloadDocuments([FromBody] ServiceReqDetailTicketDocDownloadparameter param)
        {
            try
            {
                var postUrl = apiConfig.getServiceRequestsDownloadTicketFile;
                var data = new ServiceReqDetailTicketDocDownloadparameter
                {
                    TicketReplyId = param.TicketReplyId,
                };
                var response = await serviceReqRepo.DownloadServiceRequestFiles(data, postUrl, token ?? "");

                if (response is string)
                {
                    ViewBag.errorDocumentDownloadDocPhotos = response;
                    return new DownloadDoc();
                }

                if (response?.status == "200")
                {
                    Console.WriteLine(response);
                    DownloadDoc apiResponse = JsonConvert.DeserializeObject<DownloadDoc>(response.ToString());

                    var model = new DownloadDoc
                    {
                        status = apiResponse.status,
                        path = apiResponse.path,
                    };
                    return model;
                }
                else
                {
                    ViewBag.errorDocumentDownloadDocPhotos = response?["responseMessage"];
                    return new DownloadDoc();
                }
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorDocumentDownloadDocPhotos = e.Message;
                return new DownloadDoc();
            }
        }




        #endregion

        #region Add New Req
        public IActionResult AddNewRequest11()
        {
           return View();
        }
        public async Task<IActionResult> AddNewRequest()
        {
            ViewBag.errorAddNewRequest = "";
            SrReqAddNewRequest viewModel = new SrReqAddNewRequest();
            viewModel.caseCategory = await ServiceRequestCaseCategoryFunc();
            viewModel.propeties = await ServiceRequestCasePropertiesFunc();
            viewModel.contactTiming = await ServiceRequestCaseContactTimingFunc();
            return View(viewModel);
        }
        public async Task<GetCaseCategoryModel> ServiceRequestCaseCategoryFunc()
        {
            try
            {
                ViewBag.errorAddServiceRequestCategory = "";
                var data = new {};
                var postUrl = apiConfig.getServiceRequestsCaseCategory;
                var response = await serviceReqRepo.GetServiceRequestsCaseCategory(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorAddServiceRequestCategory = response;
                    GetCaseCategoryModel model = new GetCaseCategoryModel();
                    return model;
                }

                if (response.status == "Ok")
                {
                    Console.WriteLine(response);
                    List<CaseCategory> dataList = response.caseCategories.ToObject<List<CaseCategory>>();
                    GetCaseCategoryModel model = new GetCaseCategoryModel
                    {
                        Status = response.status,
                        CaseCategories = dataList,
                        Message = response.message,
                    };
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine(model?.Status);
                    Console.WriteLine(model?.Message);
                    Console.WriteLine("-------------------------------");
                    return model!;
                }
                else
                {
                    ViewBag.errorAddServiceRequestCategory = response.message;
                    GetCaseCategoryModel model = new GetCaseCategoryModel();
                    return model;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorAddServiceRequestCategory = e.Message;
                GetCaseCategoryModel model = new GetCaseCategoryModel();
                return model;
            }

        }
        public async Task<GetCaseSubCateoryModel> ServiceRequestCaseSubCategoryFunc(string catId)
        {
            try
            {
                ViewBag.errorAddServiceRequestSubCategory = "";
                var data = new ServiceRequestsCaseSubCategoryParam
                {
                    CategoryId = catId,
                };
                var postUrl = apiConfig.getServiceRequestsCaseSubCategory;
                var response = await serviceReqRepo.GetServiceRequestsCaseSubCategory(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorAddServiceRequestSubCategory = response;
                    GetCaseSubCateoryModel model = new GetCaseSubCateoryModel();
                    return model;
                }

                if (response.status == "Ok")
                {
                    Console.WriteLine(response);
                    List<CaseSubCategory> dataList = response.caseSubCategories.ToObject<List<CaseSubCategory>>();
                    GetCaseSubCateoryModel model = new GetCaseSubCateoryModel
                    {
                        Status = response.status,
                        CaseSubCategories = dataList,
                        Message = response.message,
                    };
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine(model?.Status);
                    Console.WriteLine(model?.Message);
                    Console.WriteLine("-------------------------------");
                    return model!;
                }
                else
                {
                    ViewBag.errorAddServiceRequestSubCategory = response.message;
                    GetCaseSubCateoryModel model = new GetCaseSubCateoryModel();
                    return model;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorAddServiceRequestSubCategory = e.Message;
                GetCaseSubCateoryModel model = new GetCaseSubCateoryModel();
                return model;
            }

        }
        public async Task<GetPropertiesModel> ServiceRequestCasePropertiesFunc()
        {
            try
            {
                ViewBag.errorAddServiceRequestProperites = "";
                var data = new ServiceRequestsCasePropertiesParam
                {
                    Tenantid = "0"
                };
               
                var postUrl = apiConfig.getServiceRequestsTenantContractUnit;
                var response = await serviceReqRepo.GetServiceRequestsCaseCategory(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorAddServiceRequestProperites = response;
                    GetPropertiesModel model = new GetPropertiesModel();
                    return model;
                }

                if (response.status == "Ok")
                {
                    Console.WriteLine(response);
                    List<Property> dataList = response.properties.ToObject<List<Property>>();
                    GetPropertiesModel model = new GetPropertiesModel
                    {
                        Status = response.status,
                        Properties = dataList,
                        Message = response.message,
                    };
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine(model?.Status);
                    Console.WriteLine(model?.Message);
                    Console.WriteLine("-------------------------------");
                    return model!;
                }
                else
                {
                    ViewBag.errorAddServiceRequestProperites = response.message;
                    GetPropertiesModel model = new GetPropertiesModel();
                    return model;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorAddServiceRequestProperites = e.Message;
                GetPropertiesModel model = new GetPropertiesModel();
                return model;
            }

        }
        public async Task<GetContactTimingModel> ServiceRequestCaseContactTimingFunc()
        {
            try
            {
                ViewBag.errorAddServiceRequestContactTiming = "";
                var data = new { };
                var postUrl = apiConfig.getServiceRequestsGetContactTiming;
                var response = await serviceReqRepo.GetServiceRequestsCaseCategory(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorAddServiceRequestContactTiming = response;
                    GetContactTimingModel model = new GetContactTimingModel();
                    return model;
                }

                if (response.status == "Ok")
                {
                    Console.WriteLine(response);
                    List<ContactTiming> dataList = response.contactTiming.ToObject<List<ContactTiming>>();
                    GetContactTimingModel model = new GetContactTimingModel
                    {
                        Status = response.status,
                        ContactTiming = dataList,
                        Message = response.message,
                    };
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine(model?.Status);
                    Console.WriteLine(model?.Message);
                    Console.WriteLine("-------------------------------");
                    return model!;
                }
                else
                {
                    ViewBag.errorAddServiceRequestContactTiming = response.message;
                    GetContactTimingModel model = new GetContactTimingModel();
                    return model;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorAddServiceRequestContactTiming = e.Message;
                GetContactTimingModel model = new GetContactTimingModel();
                return model;
            }

        }

        public async Task<dynamic> SubmitServiceRequest([FromBody] SubmitServiceRequestParam para) {
           
            try
            {
                ViewBag.errorSubmitServiceRequestCategory = "";
                var data = new SubmitServiceRequestParam
                {
                    CaseCategoryId = para.CaseCategoryId,
                    CaseSubCategoryId = para.CaseSubCategoryId,
                    ContractUnitId = para.ContractUnitId,
                    Description = para.Description,
                    OtherContactPersonName = para.OtherContactPersonName == ""?null: para.OtherContactPersonName,
                    OtherContactPersonMobile = para.OtherContactPersonMobile == "" ? null : para.OtherContactPersonMobile,
                    ContactTimeId = para.ContactTimeId == ""?null: para.ContactTimeId,
                };
                var postUrl = apiConfig.saveServiceRequestsCaseCategory;
                var response = await serviceReqRepo.SaveServiceRequestsGetContactTiming(data, postUrl, token ?? "");
                Console.WriteLine(response);
                if (response is string)
                {
                    ViewBag.errorSubmitServiceRequestCategory = response;
                    SaveServiceRequestModel model = new SaveServiceRequestModel();
                    return model;
                }

                if (response.status == "Ok")
                {
                    Console.WriteLine(response);
                    AddServiceRequestNReq addSR = response.addServiceRequest.ToObject<AddServiceRequestNReq>();
                    SaveServiceRequestModel model = new SaveServiceRequestModel
                    {
                        Status = response.status,
                        AddServiceRequest = addSR,
                        Message = response.message,
                    };
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine(model?.Status);
                    Console.WriteLine(model?.Message);
                    Console.WriteLine("-------------------------------");
                    return model!;
                }
                else
                {
                    ViewBag.errorSubmitServiceRequestCategory = response.message;
                    SaveServiceRequestModel model = new SaveServiceRequestModel();
                    return model;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorSubmitServiceRequestCategory = e.Message;
                SaveServiceRequestModel model = new SaveServiceRequestModel();
                return model;
            }
        }

        #endregion




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}

public class EmirateIDInfo
{
    public string? IDNumber { get; set; }
    public string? FullName { get; set; }
    public string? Nationality { get; set; }
    public string? Sex { get; set; }
    public string? DateOfBirth { get; set; }
    public string? ExpiryDate { get; set; }
    public string? IssuingDate { get; set; }
    public string? ExpiryDateConfirmation { get; set; }
    public string? CardNumber { get; set; }

    public string? DetectedSide { get; set; } // "front", "back", or "unknown"
}
