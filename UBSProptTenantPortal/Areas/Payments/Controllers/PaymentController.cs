using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using UBSProptTenantPortal.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Transactions;

namespace UBSProptTenantPortal.Areas.Payments.Controllers
{
    [Area("Payments")]
    public class PaymentController : BaseController
    {
        private readonly PaymentsRepository paymentRepo;
        private readonly UBSTenantApiConfig apiConfig;
        new string? token = "";

        public PaymentController(PaymentsRepository _paymentRepo, IOptions<UBSTenantApiConfig> _apiConfig)
        {
            paymentRepo = _paymentRepo;
            apiConfig = _apiConfig.Value;
            token = AuthLabelsPortal.jwtToken ?? "";
        }


        public async Task<IActionResult> Payments()
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
                var response = await paymentRepo.GetPayments(data, postUrl, token ?? "");

                if (response is string errorMessage)
                {
                    ViewBag.errorGetPayments = errorMessage;
                    var errorModel = new PaymentResponse
                    {
                        Status = "Error",
                        Payments = null,
                        Message = errorMessage
                    };
                    return View(errorModel); // Or return View(errorModel) if using Razor
                }

                // Assuming response is a JObject or strongly-typed object
                var paymentsArray = response["payments"]; // JSON field should match the API
                List<PaymentDetail> paymentList = paymentsArray.ToObject<List<PaymentDetail>>();
                if (paymentList.Count == 0) { ViewBag.errorGetPayments = "No Data Found"; }
                var model = new PaymentResponse
                {
                    Status = response["status"]?.ToString(),
                    Payments = paymentList,
                    Message = response["message"]?.ToString()
                };
                return View(model);
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
                return Json(errorModel); // Or return View(errorModel)
            }
        }

        public async Task<IActionResult> PaymentDetail(string values)
        {
            ViewBag.errorGetPaymentDetail = "";
            PaymentsViewModel viewModel = new PaymentsViewModel();
            var parts = values.Split(" | ");
            string amount = parts.Length > 0 ? parts[0] : "";
            string paymentFor = parts.Length > 1 ? parts[1] : "";
            string paymentDate = parts.Length > 2 ? parts[2] : "";
            string paymentType = parts.Length > 3 ? parts[3] : "";
            string transactionID = parts.Length > 4 ? parts[4] : "";
            string receiptNo = parts.Length > 5 ? parts[5] : "";
            ViewData["ReceiptNo"] = receiptNo;
            try
            {
                viewModel.paymentDetail = new PaymentDetailUpdated
                {
                    Amount = amount,
                    PaymentFor = paymentFor,
                    PaymentDate = paymentDate,
                    PaymentType = paymentType,
                    TransactionId = transactionID,
                    ReceiptNo = receiptNo,
                };

            }
            catch (Exception ex) { Console.WriteLine($"Error retrieving Payment detail data: {ex.Message}");
                ViewBag.errorGetPaymentDetail = ex.Message;
            }
            try { viewModel.Cheques = await GetPaymentDetailCheques(transactionID); }
            catch (Exception ex) {
                ViewBag.errorGetPaymentDetail = ex.Message;
                Console.WriteLine($"Error retrieving Payment detail Cheque data: {ex.Message}"); }
            var cheque = viewModel.Cheques?.TransactionCheque?[0];
            Console.WriteLine($"Amount: {cheque?.ChequeAmount}, Type: {cheque?.ChequeAmount?.GetType()?.Name ?? "null"}");
            Console.WriteLine($"Cheque No: {cheque?.ChequeNo}, Type: {cheque?.ChequeNo?.GetType()?.Name ?? "null"}");
            Console.WriteLine($"Cheque Date: {cheque?.ChequeDate}, Type: {cheque?.ChequeDate?.GetType()?.Name ?? "null"}");
            Console.WriteLine($"Cheque Issuer Bank: {cheque?.BankName}, Type: {cheque?.BankName?.GetType()?.Name ?? "null"}");
            Console.WriteLine($"Cheque Status: {cheque?.ChequeStatus}, Type: {cheque?.ChequeStatus?.GetType()?.Name ?? "null"}");

            return View(viewModel);
        }

        public async Task<PaymentDetailCheque> GetPaymentDetailCheques(string transactionID)
        {
            try
            {
                ViewBag.errorGetPaymentDetailCheques = "";
                var data = new GetPaymentChequeParameter
                {
                    TransactionID = transactionID
                };
            var postUrl = apiConfig.getContractCheques; // basically getting cheque of payments from this
                var response = await paymentRepo.GetPaymentsCheques(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorGetPaymentDetailCheques = response;
                    PaymentDetailCheque model1 = new PaymentDetailCheque
                    {
                        Status = "",
                        TransactionCheque = null,
                        Message = ""
                    };
                    return model1;
                }
                Console.WriteLine(response);
                PaymentDetailCheque apiResponse = JsonConvert.DeserializeObject<PaymentDetailCheque>(response.ToString());

                var model = new PaymentDetailCheque
                {
                    Status = apiResponse.Status,
                    TransactionCheque = apiResponse.TransactionCheque ?? new List<PTransactionCheque>(),
                    Message = apiResponse.Message,
                };
                Console.WriteLine(model.Status);
                Console.WriteLine(model.Message);
                Console.WriteLine(model.TransactionCheque);
                return model;
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorGetPaymentDetailCheques = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                return new PaymentDetailCheque();
            }
        }


        public async Task<IActionResult> PaymentDetailDownloadReceipt(
             [FromBody] GetPaymentChequeParameterLoweCase data)
        {
            try
            {
                var postUrl = apiConfig.paymentsDownloadReceipt;
                var response = await paymentRepo.PaymentDetailDownloadReceipt(data, postUrl, token ?? "");

                if (response is string)
                {
                    return BadRequest(new { status = "", path = "" });
                }

                PaymentChequeDownload apiResponse =
                    JsonConvert.DeserializeObject<PaymentChequeDownload>(response.ToString());

                return Ok(new
                {
                    status = apiResponse.status,
                    path = apiResponse.path
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }


        //public async Task<PaymentChequeDownload> PaymentDetailDownloadReceipt([FromBody] GetPaymentChequeParameterLoweCase data)
        //{
        //    try
        //    {
        //        ViewBag.errorGetPaymentDetailCheques = "";
        //        var postUrl = apiConfig.paymentsDownloadReceipt;
        //        var response = await paymentRepo.PaymentDetailDownloadReceipt(data, postUrl, token ?? "");
        //        if (response is string)
        //        {
        //            ViewBag.errorGetPaymentDetailCheques = response;
        //            PaymentChequeDownload model1 = new PaymentChequeDownload
        //            {
        //                status = "",
        //                path = ""
        //            };
        //            return model1;
        //        }
        //        Console.WriteLine(response);
        //        PaymentChequeDownload apiResponse = JsonConvert.DeserializeObject<PaymentChequeDownload>(response.ToString());

        //        var model = new PaymentChequeDownload
        //        {
        //            status = apiResponse.status,
        //            path = apiResponse.path,
        //        };
        //        TempData["base64Pdf"] = apiResponse.path;
        //        return model;
        //    }
        //    catch (HttpRequestException e)
        //    {
        //        ViewBag.errorGetPaymentDetailCheques = e.Message;
        //        Console.WriteLine($"Request error: {e.Message}");
        //        return new PaymentChequeDownload();
        //    }
        //}

        public IActionResult DownloadReceiptFile([FromBody] Base64FileModel model)
        {
      

            // Decode Base64
            byte[] fileBytes = Convert.FromBase64String(model?.path??"");

            // Set file type (change if it's not PDF)
            string contentType = "application/pdf";
            string fileName = "Receipt.pdf";

            return File(fileBytes, contentType, fileName);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}

