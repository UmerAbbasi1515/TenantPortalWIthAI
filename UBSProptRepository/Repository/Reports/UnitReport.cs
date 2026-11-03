using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PmsRepositories.Repositories.HttpClientHelper;

using PmsRepositories.ViewModels.UnitReport;

public interface IUnitReportRepository
{
    #region Unit Vacany Report
    Task<CommonResponseVM> GetUnitVacany(UnitFilterRequest propertyFilter);
    Task<FileDownloadResponse> GetUnitVacancyReport(UnitFilterRequest propertyFilter);

    #endregion

    #region Unit Status Report

    Task<CommonResponseVM> GetUnitStatusData(UnitStatusFilterRequest statusReportVM);
    Task<FileDownloadResponse> GetUnitStatusReport(UnitStatusFilterRequest statusReportVM);

    #endregion

    #region Unit Status Report

    Task<CommonResponseVM> GetVacancyRentalosstData(VacancyRentalossFilter filter);
    Task<FileDownloadResponse> GetVacancyRentalosstReport(VacancyRentalossFilter filter);

    #endregion
}

public class UnitReportRepository : IUnitReportRepository
{

    private readonly HttpClientService _httpClientService;

    string ApiUrl = string.Empty;
    private readonly UBSLandlordApiConfig apiConfig;
    public UnitReportRepository(HttpClientService httpClientService, IConfiguration configuration, IOptions<UBSLandlordApiConfig> _apiConfig)
    {
        _httpClientService = httpClientService;
        apiConfig = _apiConfig.Value;
    }


    public async Task<CommonResponseVM> GetUnitVacany(UnitFilterRequest filter)
    {
        CommonResponseVM propertyVM = new CommonResponseVM();

        try
        {
            Console.WriteLine($"Unit vacancy Report filter ... {filter}");
            string ApiUrl = apiConfig.GetVacancyUnit ?? "";
            Console.WriteLine($"Unit vacancy Report URL ... {ApiUrl}");
            // string ApiUrl = "Unit/GetVacancyUnit";
            propertyVM = await _httpClientService.PostAsync<CommonResponseVM>(ApiUrl, filter);
        }
        catch (Exception)
        {
        }
        return propertyVM;
    }

    public async Task<FileDownloadResponse> GetUnitVacancyReport(UnitFilterRequest filter)
    {
        FileDownloadResponse fileDownload = new FileDownloadResponse();

        try
        {
            Console.WriteLine($"Unit vacancy Report filter ... {filter}");
            string ApiUrl = apiConfig.GetUnitVacancyReport ?? "";
            Console.WriteLine($"Unit vacancy Report URL ... {ApiUrl}");
            // string ApiUrl = "Unit/GetUnitVacancyReport";
            fileDownload = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filter);
        }
        catch (Exception)
        {
        }
        return fileDownload;
    }

    // Status Report
    public async Task<CommonResponseVM> GetUnitStatusData(UnitStatusFilterRequest filter)
    {
        CommonResponseVM propertyVM = new CommonResponseVM();

        try
        {
            Console.WriteLine($"Unit status Report filter ... {filter}");
            string ApiUrl = apiConfig.GetUnitStatusData ?? "";
            Console.WriteLine($"Unit status Report URL ... {ApiUrl}");
            // string ApiUrl = "Unit/GetUnitStatusData";
            propertyVM = await _httpClientService.PostAsync<CommonResponseVM>(ApiUrl, filter);
        }
        catch (Exception)
        {
        }
        return propertyVM;
    }

    public async Task<FileDownloadResponse> GetUnitStatusReport(UnitStatusFilterRequest filter)
    {
        FileDownloadResponse fileDownload = new FileDownloadResponse();

        try
        {
            Console.WriteLine($"TUnit status Report filter ... {filter}");
            string ApiUrl = apiConfig.GetUnitStatusReport ?? "";
            Console.WriteLine($"TUnit status Report URL ... {ApiUrl}");
            // string ApiUrl = "Unit/GetUnitStatusReport";
            fileDownload = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filter);
        }
        catch (Exception)
        {
        }
        return fileDownload;
    }

    #region Rental Loss Report
    public async Task<CommonResponseVM> GetVacancyRentalosstData(VacancyRentalossFilter filter)
    {
        CommonResponseVM propertyVM = new CommonResponseVM();

        try
        {
            string ApiUrl = apiConfig.GetVacancyRentalosstData ?? "";
            propertyVM = await _httpClientService.PostAsync<CommonResponseVM>(ApiUrl, filter);
        }
        catch (Exception)
        {
        }
        return propertyVM;
    }

    public async Task<FileDownloadResponse> GetVacancyRentalosstReport(VacancyRentalossFilter filter)
    {
        FileDownloadResponse fileDownload = new FileDownloadResponse();

        try
        {
            string ApiUrl = apiConfig.GetVacancyRentalosstReport ?? "";
            fileDownload = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filter);
        }
        catch (Exception)
        {
        }
        return fileDownload;
    }

    #endregion
}

