using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PmsRepositories.Repositories.HttpClientHelper;

using PmsRepositories.ViewModels.UnitReport;

public interface ITenantReportRepository
{
    Task<CommonResponseVM> GetRegister(RegisterFilterRequest filter);
    Task<FileDownloadResponse> GetRegisterReport(RegisterFilterRequest filter);

    Task<CommonResponseVM> GetStatementofAccount(StatementofAccountFilterRequest filter);
    Task<FileDownloadResponse> GetStatementofAccountReport(StatementofAccountFilterRequest filter);

    
    Task<FileDownloadResponse> GenerateTenancyScheduleReport(TenancyScheduleFilterRequest filter);
}
public class TenantReportRepository : ITenantReportRepository
{
    private readonly HttpClientService _httpClientService;

    string ApiUrl = string.Empty;
    private readonly UBSLandlordApiConfig apiConfig;
    public TenantReportRepository(HttpClientService httpClientService, IConfiguration configuration, IOptions<UBSLandlordApiConfig> _apiConfig)
    {
        _httpClientService = httpClientService;
        apiConfig = _apiConfig.Value;
    }

    public async Task<CommonResponseVM> GetRegister(RegisterFilterRequest filter)
    {
        CommonResponseVM propertyVM = new CommonResponseVM();
        try
        {
            Console.WriteLine($"Tenant Report filter ... {filter}");
            string ApiUrl = apiConfig.GetRegisterReport ?? "";
            Console.WriteLine($"Tenant Report URL ... {ApiUrl}");
            // string ApiUrl = "Tenant/GetRegisterReport";
            propertyVM = await _httpClientService.PostAsync<CommonResponseVM>(ApiUrl, filter);
        }
        catch (Exception)
        {
        }
        return propertyVM;
    }

    public async Task<FileDownloadResponse> GetRegisterReport(RegisterFilterRequest filter)
    {
        FileDownloadResponse fileDownloadResponse = new FileDownloadResponse();

        try
        {
            Console.WriteLine($"Tenant Report filter ... {filter}");
            ApiUrl = apiConfig.GenerateRegisterReport ?? "";
            Console.WriteLine($"Tenant Report URL ... {ApiUrl}");
            // ApiUrl = "Tenant/GenerateRegisterReport";
            fileDownloadResponse = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filter);
        }
        catch (Exception)
        {
            return fileDownloadResponse;
        }
        return fileDownloadResponse;
    }

    /////////////////////
    public async Task<CommonResponseVM> GetStatementofAccount(StatementofAccountFilterRequest filter)
    {
        CommonResponseVM propertyVM = new CommonResponseVM();
        try
        {
            Console.WriteLine($"Tenant Report filter ... {filter}");
            string ApiUrl = apiConfig.GetStatementofAccountData ?? "";
            Console.WriteLine($"Tenant Report URL ... {ApiUrl}");
            // string ApiUrl = "Tenant/GetStatementofAccountData";
            propertyVM = await _httpClientService.PostAsync<CommonResponseVM>(ApiUrl, filter);
        }
        catch (Exception)
        {
        }
        return propertyVM;
    }

    public async Task<FileDownloadResponse> GetStatementofAccountReport(StatementofAccountFilterRequest filter)
    {
        FileDownloadResponse fileDownloadResponse = new FileDownloadResponse();

        try
        {
            Console.WriteLine($"Tenant Report filter ... {filter}");
            ApiUrl = apiConfig.GenerateStatementofAccountReport ?? "";
            Console.WriteLine($"Tenant Report URL ... {ApiUrl}");
            // ApiUrl = "Tenant/GenerateStatementofAccountReport";
            fileDownloadResponse = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filter);
        }
        catch (Exception)
        {
            return fileDownloadResponse;
        }
        return fileDownloadResponse;
    }

    public async Task<FileDownloadResponse> GenerateTenancyScheduleReport(TenancyScheduleFilterRequest filter)
    {
        FileDownloadResponse fileDownloadResponse = new FileDownloadResponse();

        try
        {
            Console.WriteLine($"Tenant Report filter ... {filter}");
            ApiUrl = apiConfig.GenerateTenancyScheduleReport ?? "";
            Console.WriteLine($"Tenant Report URL ... {ApiUrl}");
            // ApiUrl = "Tenant/GenerateStatementofAccountReport";
            fileDownloadResponse = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filter);
        }
        catch (Exception)
        {
            return fileDownloadResponse;
        }
        return fileDownloadResponse;
    }

}
