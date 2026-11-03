using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using UBSProptTenantPortal.Models;
using Microsoft.Extensions.Options;

namespace UBSProptTenantPortal.Areas.Contracts.Controllers
{
    [Area("Contracts")]
    public class ContractController : BaseController
    {
        private readonly ContractRepository contractRepo;
        private readonly CommonDropdownRepository _commonRepo;
        private readonly UBSTenantApiConfig apiConfig;
        new string? token = "";

        public ContractController(ContractRepository _contractRepo, IOptions<UBSTenantApiConfig> _apiConfig, CommonDropdownRepository commonRepo)
        {
            contractRepo = _contractRepo;
            _commonRepo = commonRepo;
            apiConfig = _apiConfig.Value;
            token = AuthLabelsPortal.jwtToken ?? "";
        }


        public async Task<IActionResult> Contracts()
        {
            var contracts = await GetContracts();
            return View(contracts);
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
                var response = await contractRepo.GetContracts(data, postUrl, token??"");
                if (response is string) {
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
                if (dataList.Count == 0) { ViewBag.errorGetContracts = "No Data Found"; }
                    ContractResponse model = new ContractResponse
                    {
                        Status = response.status,
                        data = dataList,
                        Message = response.message
                    };
                    Console.WriteLine(model.Status);
                    Console.WriteLine(model.Message);
                    Console.WriteLine(model.data);
                    return model;
              
               
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorGetContracts = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                return new ContractResponse();
            }
        }

        // ContractsN

        private async Task FillDropdown()
        {
            var urlPropType = apiConfig.getPropTypes;
            var urlContractStatus = apiConfig.getContractStatus;
            ViewBag.Property = await _commonRepo.GetPropertyTypeDropdown(urlPropType, token ?? "");
            ViewBag.ContractStatus = await _commonRepo.GetContractStatusDropdown(urlContractStatus, token ?? "");
            
            var data = ViewBag.Property;
        }
        public async Task<IActionResult> ContractsN()
        {
            await FillDropdown();
            var contracts = await GetContractsN();
            return View(contracts);
        }
        public async Task<ContractResponse> GetContractsN()
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
                var response = await contractRepo.GetContracts(data, postUrl, token ?? "");
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
                    ViewBag.errorGetContracts = "No Data Found";
                }
                ContractResponse model = new ContractResponse
                {
                    Status = response.status,
                    data = dataList,
                    Message = response.message
                };
                Console.WriteLine(model.Status);
                Console.WriteLine(model.Message);
                Console.WriteLine(model.data);
                return model;


            }
            catch (HttpRequestException e)
            {
                ViewBag.errorGetContracts = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                return new ContractResponse();
            }
        }

        public async Task<IActionResult> ContractsNFilter(string? fromDate, string? toDate, string? propertyName, string? propertyType, string? contractStatus)
        {
            try
            {
                var contracts = await GetContractsNFilter(fromDate,toDate,propertyName,propertyType,contractStatus);
                for (int i = 0; i < contracts?.Count; i++) {
                    contracts[i].Contractno = contracts?[i].Contractno + " Hello";
                }
                return PartialView("_ContractsTable", contracts);
            }
            catch (HttpRequestException e)
            {
                return Content($@"
                    <div class='card-header pb-0' style='flex-shrink: 0;'>
                        <center style='padding-top: 50px;'>
                            <h4>Contracts</h4>
                            <p style='padding-top: 5px;'>{e.Message}</p>
                        </center>
                        <div style='height:20px'></div>
                    </div>", "text/html");
            }
        }

        public async Task<List<Contract>> GetContractsNFilter(string? fromDate, string? toDate, string? propertyName, string? propertyType, string? contractStatus)
        {
            try
            {
            ViewBag.errorGetContracts = "";
                var data = new GetContractsFilterParameterNew
                {
                    PropertyName = propertyName==null?"": propertyName,
                    PropertyTypeId = propertyType?.ToString() == null ? "-1" : propertyType,
                    PropertyStausId = contractStatus?.ToString() == null ? "-1" : contractStatus,
                    ContractDateFrom = fromDate == null ? "" : fromDate,
                    ContractDateTo = toDate == null ? "" : toDate,
                };
                var postUrl = apiConfig.getContractswithFilter;
                var response = await contractRepo.GetContracts(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorGetContracts = response;
                    List <Contract> list = new List<Contract>();
                    return list;
                }
                Console.WriteLine(response);
                // Convert JArray to List
                List<Contract> dataList = response.contracts.ToObject<List<Contract>>();

                return dataList;


            }
            catch (HttpRequestException e)
            {
                ViewBag.errorGetContracts = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                List<Contract> list = new List<Contract>();
                return list;
            }
        }


        // ContractsController Detail
        public async Task<IActionResult> ContractDetails(string contractID)
        {
            ContractDetailViewModel contractDetail = new ContractDetailViewModel();
            try
            {
                Console.WriteLine($"--------------GetContractDetails---ContractID-------------- {contractID}");
                contractDetail.ContractDetail = await GetContractDetails(contractID);

                ViewData["ContractNoTitle"] = contractDetail.ContractDetail?.Contract?.ContractNo;
                Console.WriteLine("--------------GetContractDetails-----------------");
                Console.WriteLine(contractDetail?.ContractDetail?.Status);
                Console.WriteLine(contractDetail?.ContractDetail?.Message);
                Console.WriteLine("--------------GetContractDetails-----------------");
            }
            catch (Exception ex)
            {
                ViewBag.errorContractDetail = ex.Message;
                Console.WriteLine($"Error retrieving dashboard data: {ex.Message}");
            }
            try
            {
                contractDetail!.ContractUnits = await GetContractUnites(contractID);
                Console.WriteLine("--------------GetContractUnites-----------------");
                Console.WriteLine(contractDetail.ContractUnits.Status);
                Console.WriteLine(contractDetail.ContractUnits.Message);
                Console.WriteLine("--------------GetContractUnites-----------------");
            }
            catch (Exception ex)
            {
                ViewBag.errorContractDetailUnits = ex.Message;
                Console.WriteLine($"Error retrieving properties: {ex.Message}");
            }
            try
            {
                contractDetail!.ContractPayments = await GetContractPayments(contractID);
                Console.WriteLine("--------------GetContractPayments-----------------");
                Console.WriteLine(contractDetail.ContractPayments.Status);
                Console.WriteLine(contractDetail.ContractPayments.Message);
                Console.WriteLine("--------------GetContractPayments-----------------");
            }
            catch (Exception ex)
            {
                ViewBag.errorContractDetailPayments = ex.Message;
                Console.WriteLine($"Error retrieving properties: {ex.Message}");
            }
            try
            {
                contractDetail!.ContractCharges = await GetContractCharges(contractID);
                Console.WriteLine("--------------GetContractCharges-----------------");
                Console.WriteLine(contractDetail.ContractCharges.Status);
                Console.WriteLine(contractDetail.ContractCharges.Message);
                Console.WriteLine("--------------GetContractCharges-----------------");
            }
            catch (Exception ex)
            {
                ViewBag.errorContractDetailCharges = ex.Message;
                Console.WriteLine($"Error retrieving Contracts Detail: {ex.Message}");
            }
            return View(contractDetail);
        }

        public async Task<ContractDetailsResponse> GetContractDetails(string contractID)
        {
            try
            {
                ViewBag.errorContractDetail = "";
                var data = new ContractDetailparameter
                {
                    ContractId = contractID,
                };

                var postUrl = apiConfig.getContractDetails;
                var response = await contractRepo.GetContractDetails(data, postUrl, token ?? "");
                var contractPayable = await GetContractPayables(contractID);
                if (response is string)
                {
                    ViewBag.errorContractDetail = response;
                    ContractDetailsResponse model1 = new ContractDetailsResponse
                    {
                        Status = "",
                        Contract = { },
                        Message = ""
                    };
                    return model1;
                }

                    ContractDetails contract = JsonConvert.DeserializeObject<ContractDetails>(response.contract.ToString());
                    // Formatted Amount
                    double am = (double)(contract.RentForStay ?? 0.0m);
                    CultureInfo invariantCulture = CultureInfo.InvariantCulture;
                    string formattedAmount = string.Format(invariantCulture, "{0:N2}", am);
                    formattedAmount = $"AED {formattedAmount}";
                    // ViewBag.formattedAmountt = formattedAmount;

                    // Passed Days/No of Days and percentage
                    DateTime startDate = DateTime.ParseExact(contract.ContractStartDate ?? "", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    DateTime endDate = DateTime.ParseExact(contract.ContractEndDate ?? "", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    DateTime now = DateTime.Now;
                    int daysPassed;
                    double comPtg;
                    if (now.CompareTo(startDate) <= 0)
                    {
                        daysPassed = 0;
                        comPtg = 0;
                    }
                    else if (now.CompareTo(endDate) >= 0)
                    {
                        daysPassed = contract.NoOfDays;
                        comPtg = 1;
                    }
                    else
                    {
                        daysPassed = (now - startDate).Days + 1; // Add 1 to include the start day
                        comPtg = (double)daysPassed / contract.NoOfDays; // Assuming noOfDays is not null
                    }

                    // ViewBag.daysPassedD = daysPassed;
                    // ViewBag.daysPassedPercentage = comPtg;

                    ContractDetailsResponse model = new ContractDetailsResponse
                    {
                        Status = response.status,
                        Contract = contract,
                        FormattedAmount = formattedAmount,
                        DaysPassed = daysPassed,
                        DaysPassedPercentage = comPtg,
                        Message = response.message,
                        ContratPayable = contractPayable

                    };

                    Console.WriteLine("---------------GetContractDetails----------------");
                    Console.WriteLine(model.Status);
                    Console.WriteLine(model.Message);
                    Console.WriteLine("---------------GetContractDetails----------------");
                    return model;
                
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorContractDetailUnits = e.Message;
                ContractDetailsResponse model = new ContractDetailsResponse
                {
                    Status = "",
                    Contract = { },
                    Message = ""
                };
                return model;
            }
        }

        public async Task<TenantContractPayableModel> GetContractPayables(string contractID)
        {
            try
            {
                ViewBag.errorContractPayable = "";
                var data = new ContractDetailparameter
                {
                    ContractId = contractID,
                };

                var postUrl = apiConfig.getContractPayable;
                var response = await contractRepo.GetContractPayables(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorContractPayable = "No Data Found";
                    TenantContractPayableModel model1 = new TenantContractPayableModel
                    {
                        Status = "",
                        Message = ""
                    };
                    return model1;
                }
                TenantContractPayableModel apiResponse = JsonConvert.DeserializeObject<TenantContractPayableModel>(response.ToString());

                RemoveZeroBalance(apiResponse);
                var paymenySymmary =  SumPayments(apiResponse);

                List<AdditionalCharge> contractPayableList = response.contractPayable.ToObject<List<AdditionalCharge>>();
                List<AdditionalCharge> additionalChargesList = response.additionalCharges.ToObject<List<AdditionalCharge>>();
                List<AdditionalCharge> vatChargesList = response.vatOnCharges.ToObject<List<AdditionalCharge>>();
                List<AdditionalCharge> vatOnRentList = response.vatOnRent.ToObject<List<AdditionalCharge>>();
                TenantContractPayableModel model = new TenantContractPayableModel
                {
                    Status = response.status,
                    ContractPayable = contractPayableList,
                    AdditionalCharges = additionalChargesList,
                    VatCharges = vatChargesList,
                    VatOnRent = vatOnRentList,
                    PaymentSummary = paymenySymmary,
                    Message = response.message,
                };
                if (paymenySymmary.SumOfAllPayments == "0.00") {
                    ViewBag.errorContractPayable = "No Data Found";
                }
                Console.WriteLine("---------------GetContractDetails----------------");
                Console.WriteLine(model.Status);
                Console.WriteLine(model.Message);
                Console.WriteLine("---------------GetContractDetails----------------");
                return model;

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorContractPayable = "No Data Found";
                TenantContractPayableModel model = new TenantContractPayableModel
                {
                    Status = "",
                    Message = ""
                };
                return model;
            }
        }
        public void RemoveZeroBalance(TenantContractPayableModel contractPayables)
        {
            if (contractPayables.ContractPayable != null)
                contractPayables.ContractPayable.RemoveAll(x => x.Balance == 0);

            if (contractPayables.AdditionalCharges != null)
                contractPayables.AdditionalCharges.RemoveAll(x => x.Balance == 0);

            if (contractPayables.VatCharges != null)
                contractPayables.VatCharges.RemoveAll(x => x.Balance == 0);

            if (contractPayables.VatOnRent != null)
                contractPayables.VatOnRent.RemoveAll(x => x.Balance == 0);
        }
        public PaymentSummaryModel SumPayments(TenantContractPayableModel contractPayables)
        {
            decimal rentalSum = (decimal)(contractPayables.ContractPayable?.Sum(x => x.Balance ?? 0) ?? 0);
            decimal additionalSum = (decimal)(contractPayables.AdditionalCharges?.Sum(x => x.Balance ?? 0) ?? 0);
            decimal vatChargesSum = (decimal)(contractPayables.VatCharges?.Sum(x => x.Balance ?? 0) ?? 0);
            decimal vatRentSum = (decimal)(contractPayables.VatOnRent?.Sum(x => x.Balance ?? 0) ?? 0);

            var amountFormat = new CultureInfo("ar-SA"); // 'AR' is not valid, use ar-SA or ar-AE

            return new PaymentSummaryModel
            {
                TotalRentalPayment = rentalSum.ToString("N2", amountFormat),
                TotalAdditionalCharges = additionalSum.ToString("N2", amountFormat),
                TotalVatOnCharges = vatChargesSum.ToString("N2", amountFormat),
                TotalVatOnRent = vatRentSum.ToString("N2", amountFormat),
                SumOfAllPayments = (rentalSum + additionalSum + vatChargesSum + vatRentSum).ToString("N2", amountFormat)
            };
        }


        public async Task<ContractDetailsUnitsResponse> GetContractUnites(string contractID)
        {
            try
            {
                ViewBag.errorContractDetailUnits = "";
                var data = new ContractDetailparameter
                {
                    ContractId = contractID,
                };
                var postUrl = apiConfig.getContractUnits;
                var response = await contractRepo.GetContractUnites(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorContractDetailUnits = response;
                    ContractDetailsUnitsResponse model1 = new ContractDetailsUnitsResponse
                    {
                        Status = "",
                        StatusCode = "",
                        ContractUnits = null,
                        Message = ""
                    };
                    return model1;
                }

                    Console.WriteLine(response);
                    List<ContractUnit> dataList = response.contractUnits.ToObject<List<ContractUnit>>();
                    // Formatted Amount
                    double am = (double)(response.contractUnits[0].amount ?? 0.0m);
                    CultureInfo invariantCulture = CultureInfo.InvariantCulture;
                    string formattedAmount = string.Format(invariantCulture, "{0:N2}", am);
                    formattedAmount = $"AED {formattedAmount}";
                    ContractDetailsUnitsResponse model = new ContractDetailsUnitsResponse
                    {
                        Status = response.status,
                        StatusCode = response.statusCode,
                        ContractUnits = dataList,
                        FormatedAmount = formattedAmount,
                        Message = response.message
                    };
                    ;
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine(model.Status);
                    Console.WriteLine(model.Message);
                    Console.WriteLine("-------------------------------");
                    return model;
               
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorContractDetailUnits = e.Message;
               ContractDetailsUnitsResponse model = new ContractDetailsUnitsResponse
                {
                    Status = "",
                    StatusCode = "",
                    ContractUnits = null,
                    Message = ""
                };
                return model;
            }
        }

        public async Task<ContractDetailsPaymentResponse> GetContractPayments(string contractID)
        {
            try
            {
                ViewBag.errorContractDetailPayments = "";
                var data = new ContractDetailparameter
                {
                    ContractId = contractID,
                };
                var postUrl = apiConfig.getContractPayments;
                var response = await contractRepo.GetContractPayments(data, postUrl, token ?? "");
                Console.WriteLine(response);
                if (response is string)
                {
                    ViewBag.errorContractDetailPayments = response;
                    ContractDetailsPaymentResponse model1 = new ContractDetailsPaymentResponse
                    {
                        Status = "",
                        StatusCode = "",
                        Payments = null,
                        Message = ""
                    };
                    return model1;
                }

                if (response.status == "Ok")
                {
                    // Convert JArray to List<DataModel>
                    List<Payment> dataList = response.payments.ToObject<List<Payment>>();
                    for (int i = 0; i < dataList.Count; i++)
                    {
                        // Formatted Amount
                        double am = dataList[i].Amount;
                        CultureInfo invariantCulture = CultureInfo.InvariantCulture;
                        string formattedAmount = string.Format(invariantCulture, "{0:N2}", am);
                        formattedAmount = $"AED {formattedAmount}";
                        dataList[i].FormatedAmount = formattedAmount;
                    }
                    ContractDetailsPaymentResponse model = new ContractDetailsPaymentResponse
                    {
                        Status = response.status,
                        StatusCode = response.status,
                        Payments = dataList,
                        Message = response.message
                    };
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine(model.Status);
                    Console.WriteLine(model.Message);
                    Console.WriteLine("-------------------------------");
                    return model;
                }
                else {
                    ViewBag.errorContractDetailPayments = response.status;
                    ContractDetailsPaymentResponse model = new ContractDetailsPaymentResponse
                    {
                        Status = "",
                        StatusCode = "",
                        Payments = null,
                        Message = ""
                    };
                    return model;
                }
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorContractDetailPayments = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                ContractDetailsPaymentResponse model = new ContractDetailsPaymentResponse
                {
                    Status = "",
                    StatusCode = "",
                    Payments = null,
                    Message = ""
                };
                return model;
            }
        }
        public async Task<ContractDetailChargesResponse> GetContractCharges(string contractID)
        {
            try
            {
                ViewBag.errorContractDetailCharges = "";
                var data = new ContractDetailparameter
                {
                    ContractId = contractID,
                };
                var postUrl = apiConfig.getContractCharges;
                var response = await contractRepo.GetContractCharges(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorContractDetailCharges = response;
                    ContractDetailChargesResponse model1 = new ContractDetailChargesResponse
                    {
                        Status = "",
                        StatusCode = "",
                        ContractCharges = null,
                        TotalCharges = 0,
                        PaidCharges = 0,
                        OutstandingCharges = 0,
                        Message = "",
                    };
                    return model1;
                }
                    Console.WriteLine(response);
                    List<ContractCharge> dataList = response.contractCharges.ToObject<List<ContractCharge>>();
                    CultureInfo invariantCulture = CultureInfo.InvariantCulture;

                    for (int i = 0; i < dataList.Count; i++)
                    {
                        double amount = dataList[i].Amount;
                        string formattedAmount = string.Format(invariantCulture, "{0:N2}", amount);
                        formattedAmount = $"AED {formattedAmount}";
                        dataList[i].FormatedAmount = formattedAmount;

                        double vatAmount = dataList[i].VatAmount;
                        string formattedVatAmount = string.Format(invariantCulture, "{0:N2}", vatAmount);
                        formattedVatAmount = $"AED {formattedVatAmount}";
                        dataList[i].FormatedVatAmount = formattedVatAmount;

                        double totalAmount = dataList[i].TotalAmount;
                        string formattedTotlaAmount = string.Format(invariantCulture, "{0:N2}", totalAmount);
                        formattedTotlaAmount = $"AED {formattedTotlaAmount}";
                        dataList[i].FormattedTotalAmount = formattedTotlaAmount;
                    }

                    double total = response.totalCharges ?? 0.0;
                    string formattedtotalAmount = string.Format(invariantCulture, "{0:N2}", total);
                    formattedtotalAmount = $"AED {formattedtotalAmount}";

                    double paid = response.paidCharges ?? 0.0;
                    string formattedpaidAmount = string.Format(invariantCulture, "{0:N2}", paid);
                    formattedpaidAmount = $"AED {formattedpaidAmount}";

                    double outstading = response.outstandingCharges ?? 0.0;
                    string formattedoutstadingAmount = string.Format(invariantCulture, "{0:N2}", outstading);
                    formattedoutstadingAmount = $"AED {formattedoutstadingAmount}";

                Console.WriteLine($"Paid {response.paidCharges}");
                Console.WriteLine($"outstanding {response.outstandingCharges}");
                ContractDetailChargesResponse model = new ContractDetailChargesResponse
                    {
                        Status = response.status,
                        StatusCode = response.statusCode,
                        ContractCharges = dataList,
                        FormatedTotalCharges = formattedtotalAmount,
                        TotalCharges = response.totalCharges,
                        FormatedPaidCharges = formattedpaidAmount,
                        PaidCharges = response.paidCharges,
                        FormatedOutstandingCharges = formattedoutstadingAmount,
                        OutstandingCharges = response.outstandingCharges,
                        Message = response.message,
                    };
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine(model.Status);
                    Console.WriteLine(model.Message);
                    Console.WriteLine("-------------------------------");
                    return model;
               
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorContractDetailCharges = e.Message;
               ContractDetailChargesResponse model = new ContractDetailChargesResponse
                {
                    Status = "",
                    StatusCode = "",
                    ContractCharges = null,
                    TotalCharges = 0,
                    PaidCharges = 0,
                    OutstandingCharges = 0,
                    Message = "",
                };
                return model;
            }
        }


        public async Task<PaymentChequeResponse> GetContractPaymentsCheques(string transactionID)
        {
            try
            {
                ViewBag.errorContractPaymentsCheque = "";
                var data = new ContractDetailChequeparameter
                {
                    TransactionID = "3448202",
                };
                var postUrl = apiConfig.getContractCharges;
                var response = await contractRepo.GetContractPaymentsCheques(data, postUrl, token ?? "");
                if (response is string) {
                    ViewBag.errorContractPaymentsCheque = response;
                    PaymentChequeResponse model = new PaymentChequeResponse
                    {
                        Status = "",
                        StatusCode = "",
                        TransactionCheque = null,
                        Message = "",
                    };
                    return model;
                }

                if(response.status != "NotFound") { 
                    Console.WriteLine(response);
                    List<TransactionCheque> dataList = response.TransactionCheque.ToObject<List<TransactionCheque>>();
                    PaymentChequeResponse model = new PaymentChequeResponse
                    {
                        Status = response.status,
                        StatusCode = response.statusCode,
                        TransactionCheque = dataList,
                        Message = response.message,
                    };
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine(model.Status);
                    Console.WriteLine(model.Message);
                    Console.WriteLine("-------------------------------");
                    return model;
                }
                else {
                    ViewBag.errorContractPaymentsCheque = response.status;
                    PaymentChequeResponse model = new PaymentChequeResponse
                    {
                        Status = "",
                        StatusCode = "",
                        TransactionCheque = null,
                        Message = "",
                    };
                    return model;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                ViewBag.errorContractPaymentsCheque = e.Message;
                PaymentChequeResponse model = new PaymentChequeResponse
                {
                    Status = "",
                    StatusCode = "",
                    TransactionCheque = null,
                    Message = "",
                };
                return model;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}

