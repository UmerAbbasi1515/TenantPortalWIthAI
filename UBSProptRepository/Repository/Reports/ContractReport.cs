using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PmsRepositories.Repositories.HttpClientHelper;
using PmsRepositories.ViewModels.UnitReport;

public interface IContractReportRepository
{
    Task<CommonResponseVM> GetContractStatus(ContractStatusFilter filter);
    Task<FileDownloadResponse> GetContractStatusReport(ContractStatusFilter filter);
    #region
    Task<CommonResponseVM> GetContractDueData(ContractStatusFilter filters);
    Task<FileDownloadResponse> GetContractDueReport(ContractStatusFilter filter);
    #endregion
}

public class ContractReportRepository : IContractReportRepository
{
    private readonly HttpClientService _httpClientService;

    string ApiUrl = string.Empty;

    //apiConfig apiConfig = apiConfig.Instance;
    private readonly UBSLandlordApiConfig apiConfig;
    public ContractReportRepository(HttpClientService httpClientService, IConfiguration configuration, IOptions<UBSLandlordApiConfig> _apiConfig)
    {
        _httpClientService = httpClientService;
        apiConfig = _apiConfig.Value;
    }

    // Done
    public async Task<CommonResponseVM> GetContractStatus(ContractStatusFilter filters)
    {
        CommonResponseVM contractStatus = new CommonResponseVM();
        try
        {
            filters.ReportType = "Filter";
            Console.WriteLine($"Contract Report Filter ... {filters}");
            ApiUrl = apiConfig.GetContractStatusData ?? "";
            Console.WriteLine($"Contract Report Filter ... {ApiUrl}");
            // ApiUrl = "Contracts/GetContractStatusData";
            contractStatus = await _httpClientService.PostAsync<CommonResponseVM>(ApiUrl, filters);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return contractStatus;
        }
        return contractStatus;
    }

    public async Task<FileDownloadResponse> GetContractStatusReport(ContractStatusFilter filters)
    {
        FileDownloadResponse fileDownloadResponse = new FileDownloadResponse();

        try
        {
            Console.WriteLine($"Contract Report Filter ... {filters}");
            filters.LoginUserID = LabelsLandlordPortal.UserID;
            ApiUrl = apiConfig.GenerateContractStatusReport ?? "";
            Console.WriteLine($"Contract Report Filter ... {ApiUrl}");
            // ApiUrl = "Contracts/GenerateContractStatusReport";
            fileDownloadResponse = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filters);
        }
        catch (Exception)
        {
            return fileDownloadResponse;
        }
        return fileDownloadResponse;
    }

    public async Task<CommonResponseVM> GetContractDueData(ContractStatusFilter filters)
    {
        CommonResponseVM contractStatus = new CommonResponseVM();
        try
        {
            filters.ReportType = "Filter";
            ApiUrl = apiConfig.GetContractDueDate ?? "";
            contractStatus = await _httpClientService.PostAsync<CommonResponseVM>(ApiUrl, filters);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return contractStatus;
        }
        return contractStatus;
    }

    public async Task<FileDownloadResponse> GetContractDueReport(ContractStatusFilter contractStatus)
    {
        FileDownloadResponse fileDownloadResponse = new FileDownloadResponse();

        try
        {
            ApiUrl = apiConfig.GetContractDueReport ?? "";
            fileDownloadResponse = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, contractStatus);
        }
        catch (Exception)
        {
            return fileDownloadResponse;
        }
        return fileDownloadResponse;
    }


}
