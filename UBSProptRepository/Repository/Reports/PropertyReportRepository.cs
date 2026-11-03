using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PmsRepositories.Repositories.HttpClientHelper;

using PmsRepositories.ViewModels.UnitReport;


public interface IPropertyReportRepository
{
    #region Property Status Report
    Task<CommonResponseVM> GetBuildingStatusData(BuildingStatusFilter propertyFilter);
    Task<FileDownloadResponse> GetBuildingStatusReport(BuildingStatusFilter propertyFilter);
    #endregion

    #region Vacany Ratio Report
    Task<CommonResponseVM> GetVacancyRatioData(VacancyRatioReportFilter propertyFilter);
    Task<FileDownloadResponse> GetVacancyRatioReport(VacancyRatioReportFilter propertyFilter);
    #endregion
}

public class PropertyReportRepository : IPropertyReportRepository
{
    private readonly HttpClientService _httpClientService;

    string ApiUrl = string.Empty;
    private readonly UBSLandlordApiConfig apiConfig;
    public PropertyReportRepository(HttpClientService httpClientService, IConfiguration configuration, IOptions<UBSLandlordApiConfig> _apiConfig)
    {
        _httpClientService = httpClientService;
        apiConfig = _apiConfig.Value;
    }

    #region Property Status Report
    public async Task<CommonResponseVM> GetBuildingStatusData(BuildingStatusFilter filters)
    {
        CommonResponseVM commonResponse = new CommonResponseVM();
        try
        {
            Console.WriteLine($"Property Report filters ... {filters}");
            ApiUrl = apiConfig.GetBuildingStatusData ?? "";
            Console.WriteLine($"Property Report URL ... {ApiUrl}");
            // ApiUrl = "Property/GetBuildingStatusData";
            commonResponse = await _httpClientService.PostAsync<CommonResponseVM>(ApiUrl, filters);
        }
        catch (Exception)
        {
            return commonResponse;
        }
        return commonResponse;
    }

    public async Task<FileDownloadResponse> GetBuildingStatusReport(BuildingStatusFilter filters)
    {
        FileDownloadResponse propertyVM = new FileDownloadResponse();
        try
        {
            Console.WriteLine($"Property Report filters ... {filters}");
            ApiUrl = apiConfig.GetBuildingStatusReport ?? "";
            Console.WriteLine($"Property Report URL ... {ApiUrl}");
            // ApiUrl = "Property/GetBuildingStatusReport";
            propertyVM = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filters);
        }
        catch (Exception)
        {
            return propertyVM;
        }
        return propertyVM;
    }
    #endregion

    #region Vacany Ratio Report
    public async Task<CommonResponseVM> GetVacancyRatioData(VacancyRatioReportFilter filter)
    {
        CommonResponseVM commonResponse = new CommonResponseVM();
        try
        {
            ApiUrl = apiConfig.GetVacancyRatioData ?? "";
            commonResponse = await _httpClientService.PostAsync<CommonResponseVM>(ApiUrl, filter);
        }
        catch (Exception)
        {
            return commonResponse;
        }
        return commonResponse;
    }

    public async Task<FileDownloadResponse> GetVacancyRatioReport(VacancyRatioReportFilter filter)
    {
        FileDownloadResponse propertyVM = new FileDownloadResponse();
        try
        {
            ApiUrl = apiConfig.GetVacancyRatioReport ?? "";
            propertyVM = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filter);
        }
        catch (Exception)
        {
            return propertyVM;
        }
        return propertyVM;
    }
    #endregion
}


