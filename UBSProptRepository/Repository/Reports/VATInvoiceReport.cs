using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PmsRepositories.Repositories.HttpClientHelper;

using PmsRepositories.ViewModels.UnitReport;
public interface IVatReportRepository
{
    #region Unit Vacany Report
    Task<CommonResponseVM> GetVatInvoiceData(VATInvoiceFilter propertyFilter);
    Task<FileDownloadResponse> GetVatInvoiceReport(VATInvoiceFilter propertyFilter);

    #endregion

}
public class VatReportRepository : IVatReportRepository
{
    private readonly HttpClientService _httpClientService;
    private readonly UBSLandlordApiConfig apiConfig;
    public VatReportRepository(HttpClientService httpClientService, IConfiguration configuration, IOptions<UBSLandlordApiConfig> _apiConfig)
    {
        _httpClientService = httpClientService;
        apiConfig = _apiConfig.Value;
    }

    public async Task<CommonResponseVM> GetVatInvoiceData(VATInvoiceFilter filter)
    {
        CommonResponseVM propertyVM = new CommonResponseVM();

        try
        {
            Console.WriteLine($"VAT Invcoice Report filter ... {filter}");
            string ApiUrl = apiConfig.GetVatInvoiceData ?? "";
            Console.WriteLine($"VAT Invcoice  Report URL ... {ApiUrl}");
            propertyVM = await _httpClientService.PostAsync<CommonResponseVM>(ApiUrl, filter);
        }
        catch (Exception)
        {
        }
        return propertyVM;
    }

    public async Task<FileDownloadResponse> GetVatInvoiceReport(VATInvoiceFilter filter)
    {
        FileDownloadResponse fileDownload = new FileDownloadResponse();

        try
        {
            Console.WriteLine($"VAT Invcoice  Report filter ... {filter}");
            string ApiUrl = apiConfig.GenerateVatInvoiceReport ?? "";
            Console.WriteLine($"VAT Invcoice  Report URL ... {ApiUrl}");
            fileDownload = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filter);
        }
        catch (Exception)
        {
        }
        return fileDownload;
    }
}
