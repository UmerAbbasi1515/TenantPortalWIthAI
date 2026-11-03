using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PmsRepositories.Repositories.HttpClientHelper;
using PmsRepositories.ViewModels.UnitReport;

public interface IDepositReportsRepository
{
    Task<RefundableDepositsVM> GetRefundableDeposits(RefundableDepositsFilter RefundableDepositsFilter);
    Task<FileDownloadResponse> GenerateRefundableDepositsReport(RefundableDepositsFilter filter);

    Task<FileDownloadResponse> GenerateSecurityDepositsReport(SecurityDepositsFilter filter);
    
}
public class DepositReportsRepository : IDepositReportsRepository
{
    private readonly HttpClientService _httpClientService;
    //apiConfig apiConfig = apiConfig.Instance; 
    string ApiUrl = string.Empty;
    private readonly UBSLandlordApiConfig apiConfig;
    public DepositReportsRepository(HttpClientService httpClientService, IConfiguration configuration, IOptions<UBSLandlordApiConfig> _apiConfig)
    {
        _httpClientService = httpClientService;
        apiConfig = _apiConfig.Value;
    }

    public async Task<RefundableDepositsVM> GetRefundableDeposits(RefundableDepositsFilter filter)
    {
        RefundableDepositsVM RefundableDeposits = new RefundableDepositsVM();
        try
        {
            filter.ReportType = "Filter";
            ApiUrl = apiConfig.GetRefundableDeposits ?? "";
            RefundableDeposits = await _httpClientService.PostAsync<RefundableDepositsVM>(ApiUrl, filter);
            var data = RefundableDeposits?.data.Count == 0 ? null : RefundableDeposits?.data[0];
            Console.WriteLine($"Response in the updated basecliesnt GetRefundableDeposits **:::::: ******** ====== > {data}");
            Console.WriteLine($"Response in the updated basecliesnt GetRefundableDeposits **:::::: ******** ====== > {data == null}");
            if (data == null || data.Count == 0)
            {
                Console.WriteLine($"Inside IF");
            }
            else
            {
                Console.WriteLine($"Inside Else");
            }
            Console.WriteLine($"Response in the updated basecliesnt GetRefundableDeposits :::::: ******** ====== > {RefundableDeposits?.data.Count}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new RefundableDepositsVM();
            // return RefundableDeposits;
        }
        return RefundableDeposits;
    }

    public async Task<FileDownloadResponse> GenerateRefundableDepositsReport(RefundableDepositsFilter filter)
    {
        FileDownloadResponse fileDownloadResponse = new FileDownloadResponse();

        try
        {
            ApiUrl = apiConfig.GeneratePrintRefundableDepositsReport ?? "";
            fileDownloadResponse = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filter);
        }
        catch (Exception)
        {
            return fileDownloadResponse;
        }
        return fileDownloadResponse;
    }


    public async Task<FileDownloadResponse> GenerateSecurityDepositsReport(SecurityDepositsFilter filter)
    {
        FileDownloadResponse fileDownloadResponse = new FileDownloadResponse();

        try
        {
            ApiUrl = apiConfig.GenerateSecurityDepositsReport ?? "";
            fileDownloadResponse = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filter);
        }
        catch (Exception)
        {
            return fileDownloadResponse;
        }
        return fileDownloadResponse;
    }
}

