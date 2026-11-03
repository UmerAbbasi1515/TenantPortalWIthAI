using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PmsRepositories.Repositories.HttpClientHelper;
using PmsRepositories.ViewModels.UnitReport;


public interface ICaseReportRepository
{
    Task<CommonResponseVM> GetLegalCaseData(LegalCaseFilter filter);
    Task<FileDownloadResponse> GetLegalCaseReport(LegalCaseFilter filter);

}
public class CaseReportRepository : ICaseReportRepository
{
    private readonly HttpClientService _httpClientService;

    //apiConfig apiConfig = apiConfig.Instance;
    private readonly UBSLandlordApiConfig apiConfig;
    public CaseReportRepository(HttpClientService httpClientService, IConfiguration configuration, IOptions<UBSLandlordApiConfig> _apiConfig)
    {
        _httpClientService = httpClientService;
        apiConfig = _apiConfig.Value;
    }


    public async Task<CommonResponseVM> GetLegalCaseData(LegalCaseFilter filters)
    {
        CommonResponseVM propertyVM = new CommonResponseVM();

        try
        {
            Console.WriteLine($"Legal Case Report Filter ... {filters}");
            string ApiUrl = apiConfig.GetLegalCaseData ?? "";
            // string ApiUrl = "LegalCase/GetLegalCaseData";
            Console.WriteLine($"Legal Case Report URL ... {ApiUrl}");
            propertyVM = await _httpClientService.PostAsync<CommonResponseVM>(ApiUrl, filters);
        }
        catch (Exception)
        {
        }
        return propertyVM;
    }

    public async Task<FileDownloadResponse> GetLegalCaseReport(LegalCaseFilter filters)
    {
        FileDownloadResponse fileDownload = new FileDownloadResponse();

        try
        {
            Console.WriteLine($"Legal Case Report Filter ... {filters}");
            string ApiUrl = apiConfig.GetLegalCaseReport ?? "";
            // string ApiUrl = "LegalCase/GetLegalCaseReport";
            Console.WriteLine($"Legal Case Report URL ... {ApiUrl}");
            fileDownload = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filters);
        }
        catch (Exception)
        {
        }
        return fileDownload;
    }

}

