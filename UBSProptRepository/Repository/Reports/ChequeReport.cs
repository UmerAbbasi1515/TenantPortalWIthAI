using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PmsRepositories.Repositories.HttpClientHelper;
using PmsRepositories.ViewModels.UnitReport;

public interface IChequeRegisterRepository
{
    Task<ChequeRegisterVM> GetChequeRegister(ChequeRegisterFilter filter);
    Task<FileDownloadResponse> GetChequeRegisterReport(ChequeRegisterFilter filter);
}

public class ChequeRegisterRepository : IChequeRegisterRepository
{
    private readonly HttpClientService _httpClientService;
    string ApiUrl = string.Empty;
    //apiConfig apiConfig = apiConfig.Instance;
    private readonly UBSLandlordApiConfig apiConfig;
    public ChequeRegisterRepository(HttpClientService httpClientService, IConfiguration configuration, IOptions<UBSLandlordApiConfig> _apiConfig)
    {
        _httpClientService = httpClientService;
        apiConfig = _apiConfig.Value;
    }

    public async Task<ChequeRegisterVM> GetChequeRegister(ChequeRegisterFilter filters)
    {
        ChequeRegisterVM contractStatus = new ChequeRegisterVM();
        try
        {
            filters.ReportType = "Filter";
            Console.WriteLine($"Cheque Report Filter ... {filters.ToString()}");
            ApiUrl = apiConfig.GetRegisterCheque ?? "";
            Console.WriteLine($"Cheque Report URL ... {ApiUrl}");
            contractStatus = await _httpClientService.PostAsync<ChequeRegisterVM>(ApiUrl, filters);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return contractStatus;
        }
        return contractStatus;
    }

    public async Task<FileDownloadResponse> GetChequeRegisterReport(ChequeRegisterFilter filters)
    {
        FileDownloadResponse fileDownloadResponse = new FileDownloadResponse();

        try
        {
            Console.WriteLine($"Cheque Report Filter ... {filters}");
            ApiUrl = apiConfig.GenerateChequeRegisterReport ?? "";
            Console.WriteLine($"Cheque Report URL ... {ApiUrl}");
            // ApiUrl = "ChequeRegister/GenerateChequeRegisterReport";
            fileDownloadResponse = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filters);
        }
        catch (Exception)
        {
            return fileDownloadResponse;
        }
        return fileDownloadResponse;
    }
}
