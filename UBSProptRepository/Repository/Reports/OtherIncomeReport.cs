using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PmsRepositories.Repositories.HttpClientHelper;
using PmsRepositories.ViewModels.UnitReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IOtherIncomeRepository
{
    Task<OtherIncomeVM> GetOtherIncome(OtherIncomeFilter filter);
    Task<FileDownloadResponse> GenerateOtherIncomeReport(OtherIncomeFilter filter);
}

public class OtherIncomeRepository : IOtherIncomeRepository
{
    private readonly HttpClientService _httpClientService;
    string ApiUrl = string.Empty;

    private readonly UBSLandlordApiConfig apiConfig;
    public OtherIncomeRepository(HttpClientService httpClientService, IConfiguration configuration, IOptions<UBSLandlordApiConfig> _apiConfig)
    {
        _httpClientService = httpClientService;
        apiConfig = _apiConfig.Value;
    }

    public async Task<OtherIncomeVM> GetOtherIncome(OtherIncomeFilter filters)
    {
        OtherIncomeVM otherIncomeVM = new OtherIncomeVM();
        try
        {
            filters.ReportType = "Filter";
            Console.WriteLine($"Other Income Report Filter ... {filters}");
            ApiUrl = apiConfig.GetOtherIncome ?? "";
            Console.WriteLine($"Other Income Report URL ... {ApiUrl}");
            // ApiUrl = "OtherIncome/GetOtherIncome";
            otherIncomeVM = await _httpClientService.PostAsync<OtherIncomeVM>(ApiUrl, filters);
        }
        catch (Exception ex)
        {
            return otherIncomeVM;
        }
        return otherIncomeVM;
    }

    public async Task<FileDownloadResponse> GenerateOtherIncomeReport(OtherIncomeFilter filters)
    {
        FileDownloadResponse fileDownloadResponse = new FileDownloadResponse();

        try
        {
            Console.WriteLine($"Other Income Report Filter ... {filters}");
            ApiUrl = apiConfig.GenerateOtherIncomeReport ?? "";
            Console.WriteLine($"Other Income Report URL ... {ApiUrl}");
            // ApiUrl = "OtherIncome/GenerateOtherIncomeReport";
            fileDownloadResponse = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filters);
        }
        catch (Exception)
        {
            return fileDownloadResponse;
        }
        return fileDownloadResponse;
    }
}
