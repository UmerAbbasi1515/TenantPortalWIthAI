using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PmsRepositories.Repositories.HttpClientHelper;
using PmsRepositories.ViewModels.UnitReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IDepositedChequeRepository
{
    Task<CommonResponseVM> GetDepositedCheque(DepositChequeFilter filter);
    Task<FileDownloadResponse> GetDepositedChequeReport(DepositChequeFilter filter);
}

public class DepositedChequeRepository : IDepositedChequeRepository
{
    private readonly HttpClientService _httpClientService;
    string ApiUrl = string.Empty;
    //apiConfig apiConfig = apiConfig.Instance;
    private readonly UBSLandlordApiConfig apiConfig;
    public DepositedChequeRepository(HttpClientService httpClientService, IConfiguration configuration, IOptions<UBSLandlordApiConfig> _apiConfig)
    {
        _httpClientService = httpClientService;
        apiConfig = _apiConfig.Value;
    }

    public async Task<CommonResponseVM> GetDepositedCheque(DepositChequeFilter filters)
    {
        CommonResponseVM apiResponseVM = new CommonResponseVM();
        try
        {
            filters.ReportType = "Filter";
            Console.WriteLine($"Cheque Report Filter ... {filters}");

            Console.WriteLine(string.Join(", ", filters.GetType().GetProperties().Select(p => $"{p.Name}: {p.GetValue(filters)}")));
            ApiUrl = apiConfig.GetDepositedCheque ?? "";
            Console.WriteLine($"Cheque Report Filter ... {ApiUrl}");
            // ApiUrl = "DepositCheque/GetDepositedCheque";
            apiResponseVM = await _httpClientService.PostAsync<CommonResponseVM>(ApiUrl, filters);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return apiResponseVM;
        }
        return apiResponseVM;
    }

    public async Task<FileDownloadResponse> GetDepositedChequeReport(DepositChequeFilter filters)
    {
        FileDownloadResponse fileDownloadResponse = new FileDownloadResponse();

        try
        {
            Console.WriteLine($"Cheque Report Filter ... {filters}");
            Console.WriteLine(string.Join(", ", filters.GetType().GetProperties().Select(p => $"{p.Name}: {p.GetValue(filters)}")));
            ApiUrl = apiConfig.GetDepositedChequeReport ?? "";
            // ApiUrl = "DepositCheque/GetDepositedChequeReport";
            Console.WriteLine($"Cheque Report Filter ... {ApiUrl}");
            fileDownloadResponse = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filters);
        }
        catch (Exception)
        {
            return fileDownloadResponse;
        }
        return fileDownloadResponse;
    }
}

