using System;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

public class CommonDropdownRepository
{

    //private readonly ApiHelper apiHelper;
    //public CommonDropdownRepository(ApiHelper _apiHelper)
    //{
    //    apiHelper = _apiHelper;
    //}
    //apiConfig apiConfig = apiConfig.Instance;

    private readonly UBSApiHelper apiHelper;
    private readonly UBSLandlordApiConfig apiConfig;
    private readonly UBSTenantApiConfig tenantApi;
    public CommonDropdownRepository(UBSApiHelper _apiHelper, IOptions<UBSTenantApiConfig> _tenantApi, IOptions<UBSLandlordApiConfig> _apiConfig)
    {
        apiHelper = _apiHelper;
        tenantApi = _tenantApi.Value;
        apiConfig = _apiConfig.Value;
    }

    public async Task<IEnumerable<DropdownModel>> PropertyDropdown(dynamic data)
    {
        var postUrl = apiConfig.GetLandLordProperty + data.landLordId;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.GetAsync<dynamic>(postUrl ?? "", token);
        // // Console.WriteLine($"response...  {response}");
        if (response == null)
        {
            return Enumerable.Empty<DropdownModel>();
        }
        var dropdownList = JsonConvert.DeserializeObject<IEnumerable<DropdownModel>>(response?.ToString());
        // Console.WriteLine($"dropdownList...  {dropdownList.Count}");
        return dropdownList;
    }
    public async Task<IEnumerable<DropdownModel>> CompanyDropdown(dynamic data)
    {
        var postUrl = apiConfig.propertyDropdown + data.landLordId;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.GetAsync<dynamic>(postUrl ?? "", token);
        // // Console.WriteLine($"response...  {response}");
        var dropdownList = JsonConvert.DeserializeObject<IEnumerable<DropdownModel>>(response?.ToString());
        // Console.WriteLine($"dropdownList...  {dropdownList.Count}");
        return dropdownList;
    }

    public async Task<IEnumerable<DropdownModel>> EmirateGetDropdown(dynamic data)
    {
        var postUrl = apiConfig.emirateGetDropdown + data.landLordId;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.GetAsync<dynamic>(postUrl ?? "", token);
        // Console.WriteLine($"response...  {response}");
        var dropdownList = JsonConvert.DeserializeObject<IEnumerable<DropdownModel>>(response?.ToString());
        Console.WriteLine($"dropdownList...  {dropdownList.Count}");
        return dropdownList;
    }
    public async Task<IEnumerable<DropdownModel>> UnitTypeGetDropdown(dynamic data)
    {
        var postUrl = apiConfig.unitTypeDropdown + data.landLordId;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.GetAsync<dynamic>(postUrl ?? "", token);
        if (response == null)
        {
            return Enumerable.Empty<DropdownModel>();
        }
        var dropdownList = JsonConvert.DeserializeObject<IEnumerable<DropdownModel>>(response?.ToString());
        Console.WriteLine($"dropdownList...  {dropdownList.Count}");
        return dropdownList;
    }
    public List<DropdownModel> BedRoomsGetDropdown(dynamic data)
    {
        var bedroomsList = new List<DropdownModel>();

        // Populate the list with 5 DropdownModel objects
        for (int i = 1; i <= 5; i++)
        {
            var model = new DropdownModel
            {
                ID = i,
                TenantTypeId = i,
                Name = $"Bed No {i}"
            };

            bedroomsList.Add(model);
        }
        return bedroomsList;
    }

    public async Task<IEnumerable<DropdownModel>> UnitStatusGetDropdown(dynamic data)
    {
        var postUrl = apiConfig.unitStatusGetDropdown + data.landLordId;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.GetAsync<dynamic>(postUrl ?? "", token);
        // Console.WriteLine($"response...  {response}");
        var dropdownList = JsonConvert.DeserializeObject<IEnumerable<DropdownModel>>(response?.ToString());
        Console.WriteLine($"dropdownList...  {dropdownList.Count}");
        return dropdownList;
    }
    public async Task<IEnumerable<DropdownModel>> GetChequeStatusDropdown(dynamic data)
    {
        var postUrl = apiConfig.chequeStatusDropdown + data.landLordId;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.GetAsync<dynamic>(postUrl ?? "", token);
        // Console.WriteLine($"response...  {response}");
        var dropdownList = JsonConvert.DeserializeObject<IEnumerable<DropdownModel>>(response?.ToString());
        Console.WriteLine($"dropdownList...  {dropdownList.Count}");
        return dropdownList;
    }

    public async Task<IEnumerable<DropdownModel>> ReasonOfPaymentsGetDropdown(dynamic data)
    {
        var postUrl = apiConfig.reasonOfPaymentsDropdown + data.landLordId;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.GetAsync<dynamic>(postUrl ?? "", token);
        // Console.WriteLine($"response...  {response}");
        var dropdownList = JsonConvert.DeserializeObject<IEnumerable<DropdownModel>>(response.ToString());
        Console.WriteLine($"dropdownList...  {dropdownList.Count}");
        return dropdownList;
    }
    public async Task<IEnumerable<DropdownModel>> TenantTypeGetDropdown(dynamic data)
    {
        var postUrl = apiConfig.tenantTypeDrp + data.landLordId;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.GetAsync<dynamic>(postUrl ?? "", token);
        if (response == null)
        {
            return Enumerable.Empty<DropdownModel>();
        }
        var dropdownList = JsonConvert.DeserializeObject<IEnumerable<DropdownModel>>(response?.ToString());
        Console.WriteLine($"dropdownList...  {dropdownList.Count}");
        return dropdownList;
    }
    public async Task<IEnumerable<DropdownModel>> ContractStatusGetDropdown(dynamic data)
    {
        var postUrl = apiConfig.contractStatusDrp + data.landLordId;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.GetAsync<dynamic>(postUrl ?? "", token);
        // Console.WriteLine($"response...  {response}");
        var dropdownList = JsonConvert.DeserializeObject<IEnumerable<DropdownModel>>(response?.ToString());
        Console.WriteLine($"dropdownList...  {dropdownList.Count}");
        return dropdownList;
    }
    public async Task<IEnumerable<DropdownModel>> ContractActionDrp(dynamic data)
    {
        var postUrl = apiConfig.contractActionDropDown;
        //var postUrl = apiConfig.contractActionDropDown + data.landLordId;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.GetAsync<dynamic>(postUrl ?? "", token);
        // Console.WriteLine($"response...  {response}");
        var dropdownList = JsonConvert.DeserializeObject<IEnumerable<DropdownModel>>(response?.ToString());
        Console.WriteLine($"dropdownList...  {dropdownList.Count}");
        return dropdownList;
    }
    public async Task<IEnumerable<DropdownModel>> GetPaymentKindDropdown(dynamic data)
    {
        var postUrl = apiConfig.getPaymentKindDropdown + data.landLordId;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.GetAsync<dynamic>(postUrl ?? "", token);
        // Console.WriteLine($"response...  {response}");
        var dropdownList = JsonConvert.DeserializeObject<IEnumerable<DropdownModel>>(response?.ToString());
        Console.WriteLine($"dropdownList...  {dropdownList.Count}");
        return dropdownList;
    }
    public async Task<IEnumerable<DropdownModel>> RefundableReasonOfPaymentsGetDropdown(dynamic data)
    {
        var postUrl = apiConfig.refundableReasonOfPaymentsGetDropdown + data.landLordId;
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.GetAsync<dynamic>(postUrl ?? "", token);
        // Console.WriteLine($"response...  {response}");
        var dropdownList = JsonConvert.DeserializeObject<IEnumerable<DropdownModel>>(response?.ToString());
        Console.WriteLine($"dropdownList...  {dropdownList.Count}");
        return dropdownList;
    }
    public async Task<IEnumerable<DropdownModel>> GetTenant(String search, int TenantTypeID, dynamic data)
    {
        var postUrl = apiConfig.getTenantbySearch + "search=" + search + "&tenantTypeID=" + TenantTypeID;

        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.GetAsync<dynamic>(postUrl ?? "", token);
        // Console.WriteLine($"response...  {response}");
        var dropdownList = JsonConvert.DeserializeObject<IEnumerable<DropdownModel>>(response?.ToString());
        Console.WriteLine($"dropdownList...  {dropdownList.Count}");
        return dropdownList;
    }
    

    public async Task<IEnumerable<PropertyTypeL>> GetPropertyTypeDropdown(string url, string token)
    {
        var postUrl = url;
        var data = new { };
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl ?? "", data, token);
        // // Console.WriteLine($"response...  {response}");
        PropertyTypesModel propType = JsonConvert.DeserializeObject<PropertyTypesModel>(response?.ToString());
        var dropdownList = propType?.PropertyTypes ?? new List<PropertyTypeL>();

        Console.WriteLine($"dropdownList...  {dropdownList.Count}");
        return dropdownList;
    }
    public async Task<IEnumerable<ContractStatus>> GetContractStatusDropdown(string url, string token)
    {
        var postUrl = url;
        var data = new { };
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl ?? "", data, token);
        // // Console.WriteLine($"response...  {response}");
        ContractStatusModel status = JsonConvert.DeserializeObject<ContractStatusModel>(response?.ToString());
        var dropdownList = status?.ContractStatus ?? new List<ContractStatus>();

        Console.WriteLine($"dropdownList...  {dropdownList.Count}");
        return dropdownList;
    }
    public async Task<IEnumerable<VendorContractStatus>> GetContractStatusDropdownVendor(string url, string token)
    {
        var postUrl = url;
        var data = new { };
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl ?? "", data, token);
        // // Console.WriteLine($"response...  {response}");
        ContractStatusVendorModel status = JsonConvert.DeserializeObject<ContractStatusVendorModel>(response?.ToString());
        var dropdownList = status?.ContractStatus ?? new List<VendorContractStatus>();

        Console.WriteLine($"dropdownList...  {dropdownList.Count}");
        return dropdownList;
    }
    public async Task<IEnumerable<LpoStatus>> GetLpoStatusDropdownVendor(string url, string token)
    {
        var postUrl = url;
        var data = new { };
        var response = await apiHelper.PostAsyncWithheader<dynamic>(postUrl ?? "", data, token);
        GetLpoStatusModel status = JsonConvert.DeserializeObject<GetLpoStatusModel>(response?.ToString());
        var dropdownList = status?.LpoStatus ?? new List<LpoStatus>();

        Console.WriteLine($"dropdownList...  {dropdownList.Count}");
        return dropdownList;
    }
}
