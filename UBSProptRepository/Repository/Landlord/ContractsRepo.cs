using System;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

public class ContractsRepository
{
    private readonly UBSApiHelper apiHelper;
    private readonly UBSLandlordApiConfig apiConfig;
    public ContractsRepository(UBSApiHelper _apiHelper, IOptions<UBSLandlordApiConfig> _apiConfig)
    {
        apiConfig = _apiConfig.Value;
        apiHelper = _apiHelper;
    }
    //apiConfig apiConfig = apiConfig.Instance;


    public async Task<dynamic> GetContractDetails(dynamic data)
    {
        var postUrl = apiConfig.GetLandlordContractDetails;
        String token = LabelsLandlordPortal.token;
        // var data1 = new ContractDetailparameter
        // {
        //     ContractId = "254950",
        // };
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl ?? "", data, token);
        return response ?? "";
    }

    public async Task<dynamic> GetContractUnites(dynamic data)
    {
        var postUrl = apiConfig.GetLandlordContractUnits;
        // Static Data Passing
        // var data1 = new ContractDetailparameter
        // {
        //     ContractId = "254583",
        // };
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl ?? "", data, token);
        return response ?? "";
    }
    public async Task<dynamic> GetContractPayments(dynamic data)
    {
        var postUrl = apiConfig.GetLandlordContractPayments;
        // var data1 = new ContractDetailparameter
        // {
        //     ContractId = "254950",
        // };
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl ?? "", data, token);
        return response ?? "";
    }
    public async Task<dynamic> GetContractPaymentsCheques(dynamic data)
    {
        var postUrl = apiConfig.GetLandlordContractCheques;
        // var data1 = new ContractDetailChequeparameter
        // {
        //     TransactionID = "3448202",
        // };
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl ?? "", data, token);
        return response ?? "";
    }
    public async Task<dynamic> GetContractCharges(dynamic data)
    {
        var postUrl = apiConfig.GetLandlordContractCharges;
        // var data1 = new ContractDetailparameter
        // {
        //     ContractId = "254950",
        // };
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl ?? "", data, token);
        return response ?? "";
    }
}
