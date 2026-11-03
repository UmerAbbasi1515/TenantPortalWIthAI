using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PmsRepositories.Repositories.HttpClientHelper;
using PmsRepositories.ViewModels.UnitReport;


public interface ISapArAgningReport
{
    #region Sap ArAgning Report
    Task<CommonResponseVM> GetSapArAgningData(SapArAgningFilter filter);
    Task<FileDownloadResponse> GetSapArAgningReport(SapArAgningFilter filter);
    #endregion
    #region Sap Trail balance Report
    Task<CommonResponseVM> GetSapTrailBalanceData(SapTrailBalanceFilter filter);
    Task<FileDownloadResponse> GetSapTrailBalanceReport(SapTrailBalanceFilter filter);
    #endregion
}
public class SapArAgningReportRepo : ISapArAgningReport
{
    private readonly HttpClientService _httpClientService;

    private readonly UBSLandlordApiConfig apiConfig;
    public SapArAgningReportRepo(HttpClientService httpClientService, IConfiguration configuration, IOptions<UBSLandlordApiConfig> _apiConfig)
    {
        _httpClientService = httpClientService;
        apiConfig = _apiConfig.Value;
    }

    #region Sap ArAgning Report
    public async Task<CommonResponseVM> GetSapArAgningData(SapArAgningFilter filters)
    {
        CommonResponseVM propertyVM = new CommonResponseVM();

        try
        {
            Console.WriteLine($"Ar Agning Report Filter ... {filters}");
            string ApiUrl = apiConfig.GetArAgningData ?? "";
            Console.WriteLine($"Ar Agning  Report URL ... {ApiUrl}");
            propertyVM = await _httpClientService.PostAsync<CommonResponseVM>(ApiUrl, filters);
        }
        catch (Exception)
        {
        }
        return propertyVM;
    }

    public async Task<FileDownloadResponse> GetSapArAgningReport(SapArAgningFilter filters)
    {
        FileDownloadResponse fileDownload = new FileDownloadResponse();

        try
        {
            if(filters.company == null){
                filters.company = "";
            }
            if(filters.asonDate == null){
                filters.asonDate = "";
            }
            if(filters.ageFromDueDate == null){
                filters.ageFromDueDate = "";
            }
            if(filters.reportType == null){
                filters.reportType = "";
            }
            if(filters.loginUserID == "-1"){
                filters.loginUserID = "-1";
            }
           
             string ApiUrl = apiConfig.GetArAgningReport ?? "";
            Console.WriteLine($"Ar Agning  Report URL ... {ApiUrl}");
            fileDownload = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filters);
        }
        catch (Exception)
        {
        }
        return fileDownload;
    }
    #endregion

    #region Sap Trail balance Report
    public async Task<CommonResponseVM> GetSapTrailBalanceData(SapTrailBalanceFilter filters)
    {
        CommonResponseVM propertyVM = new CommonResponseVM();

        try
        {
            Console.WriteLine($"Sap trail  Report Filter ... {filters}");
            string ApiUrl = apiConfig.GetSapTrailBalanceData ?? "";
            Console.WriteLine($"Sap trail   Report URL ... {ApiUrl}");
            propertyVM = await _httpClientService.PostAsync<CommonResponseVM>(ApiUrl, filters);
        }
        catch (Exception)
        {
        }
        return propertyVM;
    }

    public async Task<FileDownloadResponse> GetSapTrailBalanceReport(SapTrailBalanceFilter filters)
    {
        FileDownloadResponse fileDownload = new FileDownloadResponse();

        try
        {
            if (filters.TbPeriod == null)
            {
                filters.TbPeriod = "";
            }
            if (filters.TbType == null)
            {
                filters.TbType = "";
            }
            if (filters.reportType == null)
            {
                filters.reportType = "";
            }
            if (filters.loginUserID == "-1")
            {
                filters.loginUserID = "-1";
            }

            //string ApiUrl = "http://40.80.86.229:9023/api/Pms/SapReport/GetSapArAgningReport";
            string ApiUrl = apiConfig.GetSapTrailBalanceReport ?? "";
            Console.WriteLine($"Sap trail  Report URL ... {ApiUrl}");
            fileDownload = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filters);
        }
        catch (Exception)
        {
        }
        return fileDownload;
    }
    #endregion
}

