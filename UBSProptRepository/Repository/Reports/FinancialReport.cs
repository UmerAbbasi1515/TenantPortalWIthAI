using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PmsRepositories.Repositories.HttpClientHelper;
using PmsRepositories.ViewModels.UnitReport;


public interface IReceiptRegisterRepository
{
    Task<CommonResponseVM> GetReceiptData(ReceiptRegisterFilter propertyFilter);
    Task<FileDownloadResponse> GetReceiptReport(ReceiptRegisterFilter propertyFilter);
}
public class ReceiptRegisterRepository : IReceiptRegisterRepository
{

    private readonly HttpClientService _httpClientService;

    //apiConfig apiConfig = apiConfig.Instance;
    private readonly UBSLandlordApiConfig apiConfig;
    public ReceiptRegisterRepository(HttpClientService httpClientService, IConfiguration configuration, IOptions<UBSLandlordApiConfig> _apiConfig)
    {
        _httpClientService = httpClientService;
        apiConfig = _apiConfig.Value;
    }

    public async Task<CommonResponseVM> GetReceiptData(ReceiptRegisterFilter filters)
    {
        CommonResponseVM propertyVM = new CommonResponseVM();
        try
        {
            Console.WriteLine($"Recipt Register Report Filter ... {filters}");
            string ApiUrl = apiConfig.GetReceiptRegisterData ?? "";
            Console.WriteLine($"Recipt Register Report URL ... {ApiUrl}");
            // string ApiUrl = "ReceiptRegister/GetReceiptRegisterData";
            propertyVM = await _httpClientService.PostAsync<CommonResponseVM>(ApiUrl, filters);
        }
        catch (Exception)
        {
        }
        return propertyVM;
    }

    public async Task<FileDownloadResponse> GetReceiptReport(ReceiptRegisterFilter filters)
    {
        FileDownloadResponse fileDownload = new FileDownloadResponse();

        try
        {
            Console.WriteLine($"Recipt Register Report Filter ... {filters}");
            string ApiUrl = apiConfig.GetReceiptRegisterReport ?? "";
            // string ApiUrl = "ReceiptRegister/GetReceiptRegisterReport";
            Console.WriteLine($"Recipt Register Report URL ... {ApiUrl}");
            fileDownload = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filters);
        }
        catch (Exception)
        {
        }
        return fileDownload;
    }

}

