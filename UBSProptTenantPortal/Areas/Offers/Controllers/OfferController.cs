using System;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UBSProptTenantPortal.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using static Google.Apis.Requests.BatchRequest;
using System.Globalization;
using System.Xml.Linq;

namespace UBSProptTenantPortal.Areas.Faqs.Controllers
{
    [Area("Offers")]
    public class OfferController : BaseController
    {
        private readonly ILogger<OfferController> _logger;
        private readonly OffersRepository offerRepo;
        private readonly UBSTenantApiConfig apiConfig;
        new string? token = "";

        public OfferController(ILogger<OfferController> logger, OffersRepository _offerRepo, IOptions<UBSTenantApiConfig> _apiConfig)
        {
            _logger = logger;
            offerRepo = _offerRepo;
            apiConfig = _apiConfig.Value;
            token = AuthLabelsPortal.jwtToken ?? "";
        }

        #region Old offers (unchanged)

        public async Task<IActionResult> Offers()
        {
            try
            {
                ViewBag.errorGetOffer = "";
                var postUrl = apiConfig.GetOffer;
                var data = new { }; // empty object for POST body
                var response = await offerRepo.GetOffers(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorGetOffer = response;
                    OfferResponse model1 = new OfferResponse();
                    return View(model1);
                }

                OfferResponse apiResponse = JsonConvert.DeserializeObject<OfferResponse>(response.ToString());
                var dataList = apiResponse.Record ?? new List<OfferRecord>();
                if (dataList.Count == 0) { ViewBag.errorGetOffer = "No Data Found"; }
                var viewModel = new OfferResponse
                {
                    Statsus = apiResponse.Statsus ?? "Ok",
                    Record = apiResponse.Record ?? new List<OfferRecord>(),
                };

                return View(viewModel);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorGetOffer = e.Message;
                var viewModel = new OfferResponse
                {
                    Statsus = "500",
                    Record = null
                };

                return View(viewModel);
            }
        }

        public async Task<IActionResult> OffersDetail(string offerId)
        {
            try
            {
                ViewBag.errorGetOfferDetail = "";
                var postUrl = apiConfig.GetOfferDetail;
                var data = new GetOfferDetailparameter { OfferId = "1" };
                Console.WriteLine($"OfferId: {offerId}");
                var response = await offerRepo.GetOfferDetail(data, postUrl, token ?? "");

                if (response is string)
                {
                    ViewBag.errorGetOfferDetail = response;
                    GetOfferDetailparameter model1 = new GetOfferDetailparameter();
                    return View(model1);
                }

                OfferDetailResponse apiResponse = JsonConvert.DeserializeObject<OfferDetailResponse>(response.ToString());

                var viewModel = new OfferDetailResponse
                {
                    Statsus = apiResponse.Statsus,
                    Record = apiResponse?.Record,
                    OfferProperties = apiResponse?.OfferProperties
                };
                return View(viewModel);
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorGetOfferDetail = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                OfferDetailResponse viewModel = new OfferDetailResponse
                {
                    Statsus = "500",
                    Record = null,
                    OfferProperties = null
                };
                return View(viewModel);
            }
        }

        #endregion

        #region Offers (new model)

        // Shared loader: calls the API once and returns the model.
        // Sets ViewBag.errorGetOffer when something is wrong or the list is empty.
        [NonAction]
        private async Task<OffersNResponseModel> LoadOffersAsync()
        {
            ViewBag.errorGetOffer = "";
            var model = new OffersNResponseModel { Offers = new List<OfferModel>() };

            try
            {
                var postUrl = apiConfig.GetOffersFromUbsPropT;
                var response = await offerRepo.GetOffersN(new { }, postUrl, token ?? "");

                if (response is string)
                {
                    ViewBag.errorGetOffer = (string)response;
                    model.Status = "Error";
                    return model;
                }

                var api = JsonConvert.DeserializeObject<OffersNResponseModel>(response.ToString());
                model.Status = api?.Status ?? "";
                model.Message = api?.Message ?? "";
                model.Offers = api?.Offers ?? new List<OfferModel>();

                if (model.Offers.Count == 0)
                    ViewBag.errorGetOffer = "No Data Found";
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorGetOffer = e.Message;
                model.Status = "500";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                ViewBag.errorGetOffer = "Unexpected error: " + ex.Message;
                model.Status = "500";
            }
            return model;
        }

        // LIST page -> Areas/Offers/Views/Offer/OffersN.cshtml
        public async Task<IActionResult> OffersN()
        {
            var model = await LoadOffersAsync();
            return View(model);
        }

        // DETAIL page (3 tabs) -> Areas/Offers/Views/Offer/OffersDetailN.cshtml
        // The API has no "get one offer" call, so we load the list and pick the clicked offer.
        public async Task<IActionResult> OffersDetailN(int offerId,string offerName)
        {
            ViewBag.errorOverView = "";
            ViewBag.offerId = offerId; 
            ViewBag.offerName = offerName;
            var list = await LoadOffersAsync();
            var offer = list.Offers?.FirstOrDefault(o => o.OfferID == offerId);

            if (offer == null)
            {
                var apiError = ViewBag.errorGetOffer as string;
                ViewBag.errorOverView = string.IsNullOrEmpty(apiError) ? "Offer not found" : apiError;
                offer = new OfferModel();
            }

            return View(offer);   // @model OfferModel
        }

        #endregion

        #region Document

        public async Task<OfferDocsResponse> OffersNDocuments(string OfferId)
        {
            try
            {
                ViewBag.errorDocuments = "";
                var data = new GetOfferDetailparameter
                {
                    OfferId = OfferId,
                };
                var postUrl = apiConfig.GetOfferDocuments;
                var response = await offerRepo.GetOffersDocs(data, postUrl, token ?? "");

                if (response is string)
                {
                    ViewBag.errorDocuments = response;
                    return new OfferDocsResponse();
                }

                if (response != null && response?.status == "200")
                {
                    Console.WriteLine(response);

                    List<OfferDocRecord> dataList = response!.record.ToObject<List<OfferDocRecord>>();

                   
                    var result = new OfferDocsResponse
                    {
                        status = response?.status,
                        record = dataList,
                    };
                    return result;
                }
                else
                {
                    ViewBag.errorDocuments = response?["responseMessage"] ?? "Unable to load documents";
                    return new OfferDocsResponse {};
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorDocuments = e.Message;
                return new OfferDocsResponse { };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OffersNDocuments error: {ex}");
                ViewBag.errorDocuments = "Unexpected error: " + ex.Message;
                return new OfferDocsResponse {};
            }
        }

        [HttpGet]
        public async Task<IActionResult> OffersNDocumentsPartial(string offerId, string offerName)
        {
            var model = await OffersNDocuments(offerId);
            ViewBag.OfferId = offerId;
            ViewBag.offerName = offerName;
            return PartialView("_DocumentPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadOfferDocument(
                [FromForm(Name = "MasterID")] string offerId,
                [FromForm(Name = "OriginalFiles")] IFormFile file,
                [FromForm] int index,
                [FromForm] string? title,
                [FromForm] DateTime? expiryDate)
        {
            string? filePath = null;
            try
            {
                if (file == null || file.Length == 0)
                {
                    return Json(new { success = false, message = "No file received." });
                }
                if (string.IsNullOrWhiteSpace(offerId))
                {
                    return Json(new { success = false, message = "Offer ID is missing." });
                }

                filePath = await SaveAttachmentAsync(file);

                var postUrl = apiConfig.UploadOfferFiles;
                var tokenValue = token ?? "";
                var expiry = DateTime.UtcNow;
                var response = await offerRepo.UploadOffersFile(
                 addedBy: User.Identity?.Name ?? "",   // or from session
                 expiry: expiry,                   // DateTime? from the form, or null
                 masterID: offerId,
                 title: title ?? "",
                 titleAR: title ?? "",
                 documentType: "offer",
                 suspend: "0",
                 filePath: filePath ?? "",
                 url: postUrl,
                 token: tokenValue
             );

                // String response is usually an error message
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
                    string? photoId = response?.photoId?.ToString();
                    return Json(new
                    {
                        success = true,
                        message = response?.status,
                        PhotoId = photoId,
                        index = index
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
            finally
            {
                // Remove the temporary copy so wwwroot/Uploads does not keep growing
                try
                {
                    if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }
                catch { /* ignore cleanup errors */ }
            }
        }

        [HttpPost]
        public async Task<DownloadDoc> DownloadOfferDocuments([FromBody] DownloadOfferDetailparameter param)
        {
            try
            {
                var postUrl = apiConfig.DownloadOfferDocuments;
                var data = new DownloadOfferDetailparameter
                {
                    OfferId = param.OfferId,
                    DocumentId = param.DocumentId,
                };
                var response = await offerRepo.DownloadOffersFiles(data, postUrl, token ?? "");

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

        #region Communication

        [NonAction]
        public async Task<CommunicationModel> GetOfferTicketReplies(string offerId)
        {
            var model = new CommunicationModel();
            try
            {
                var data = new GetOfferDetailparameter { OfferId = offerId };
                var postUrl = apiConfig.GetOfferTicketReplies;
                var response = await offerRepo.GetOffersTickekrtReplies(data, postUrl, token ?? "");

                if (response is string)
                {
                    model.errorMessage = (string)response;      // the partial shows this
                    return model;
                }

                if (response?.status == "Ok")
                {
                    model.Status = response.status;
                    CommunicationModel apiResponse = JsonConvert.DeserializeObject<CommunicationModel>(response.ToString());
                    model.TicketReply = apiResponse?.TicketReply ?? new List<TicketReply>();
                    model.Message = response.message;
                    return model;
                }

                return model;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                model.errorMessage = e.Message;
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                model.errorMessage = "Unexpected error: " + ex.Message;
                return model;
            }
        }

        [HttpGet]
        public async Task<IActionResult> InvoiceCommunicationsPartial(string offerId)
        {
            var model = await GetOfferTicketReplies(offerId);
            ViewBag.offerId = offerId;
            // ViewBag.status = "closed" hides the message box. Set it here if you know the offer is closed.
            return PartialView("_CommunicationPartial", model);
        }

        [NonAction]
        public async Task<string?> SaveAttachmentAsync(IFormFile? attachment)
        {
            if (attachment == null || attachment.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(attachment.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await attachment.CopyToAsync(stream);
            }
            return filePath;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string? Message, string offerId, IFormFile? Attachment)
        {
            string? filePath = null;
            try
            {
                if (string.IsNullOrWhiteSpace(offerId))
                    return Json(new { success = false, status = "Error", message = "Case number is missing." });

                if (string.IsNullOrWhiteSpace(Message) && (Attachment == null || Attachment.Length == 0))
                    return Json(new { success = false, status = "Error", message = "Type a message or attach a file." });

                var postUrl = apiConfig.AddOfferTicketReply;
                filePath = await SaveAttachmentAsync(Attachment);

                var response = await offerRepo.OffersddTickeketReplies(
                    offerId, Message ?? "", postUrl, token ?? "", filePath ?? "");

                if (response is string)
                    return Json(new { success = false, status = "Error", message = (string)response });

                if (response?.status == "Ok")
                    return Json(new { success = true, status = "Ok", message = (string?)response.message });

                return Json(new
                {
                    success = false,
                    status = "Error",
                    message = (string?)response?.message ?? "Message could not be sent"
                });
            }
            catch (HttpRequestException e)
            {
                return Json(new { success = false, status = "Error", message = e.Message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, status = "Error", message = "Unexpected error: " + ex.Message });
            }
            finally
            {
                // delete the temp copy so wwwroot/Uploads does not keep growing
                try
                {
                    if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }
                catch { /* ignore cleanup errors */ }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTikcketReplyOfferDownloadDocuments(string ticketReplyID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ticketReplyID))
                    return Json(new { status = "Error", path = "", message = "Attachment ID is missing." });

                var postUrl = apiConfig.DownloadOfferTicketFile;
                var data = new ServiceReqDetailTicketDocDownloadparameter { TicketReplyId = ticketReplyID };
                var response = await offerRepo.DownloadOffersFiles(data, postUrl, token ?? "");

                if (response is string)
                    return Json(new { status = "Error", path = "", message = (string)response });

                if (response?.status == "200")
                {
                    DownloadDoc apiResponse = JsonConvert.DeserializeObject<DownloadDoc>(response.ToString());
                    return Json(new { status = apiResponse.status, path = apiResponse.path });
                }

                return Json(new
                {
                    status = "Error",
                    path = "",
                    message = (string?)response?["responseMessage"] ?? "Download failed"
                });
            }
            catch (HttpRequestException e)
            {
                return Json(new { status = "Error", path = "", message = e.Message });
            }
            catch (Exception ex)
            {
                return Json(new { status = "Error", path = "", message = "Unexpected error: " + ex.Message });
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
