using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PmsRepositories.Repositories.HttpClientHelper;
using PmsRepositories.ViewModels.UnitReport;

public interface IBouncedChequesReportRepository
    {
        Task<CommonResponseVM> GetBouncedChequesData(BouncedChequesFiltercs filter);
        Task<FileDownloadResponse> GetBouncedChequesReport(BouncedChequesFiltercs filter);
    }

    public class BouncedChequesReportRepository : IBouncedChequesReportRepository
    {
        private readonly HttpClientService _httpClientService;
        string ApiUrl = string.Empty;
    private readonly UBSLandlordApiConfig apiConfig;
    public BouncedChequesReportRepository(HttpClientService httpClientService, IConfiguration configuration, IOptions<UBSLandlordApiConfig> _apiConfig)
        {
            _httpClientService = httpClientService;
        apiConfig = _apiConfig.Value;
        }
        public async Task<CommonResponseVM> GetBouncedChequesData(BouncedChequesFiltercs filters)
        {
            CommonResponseVM contractStatus = new CommonResponseVM();
            try
            {
                filters.ReportType = "Filter";
                Console.WriteLine($"Bounced Cheque Report Filter ... {filters}");
                // ApiUrl = "BouncedChequesSummary/GetBouncedChequesData";
                ApiUrl = apiConfig.GetBouncedChequesData ?? "";
                Console.WriteLine($"Bounced Cheque Report URL ... {ApiUrl}");
                contractStatus = await _httpClientService.PostAsync<CommonResponseVM>(ApiUrl, filters);
            }
            catch (Exception ex)
            {
                return contractStatus;
            }
            return contractStatus;
        }

        public async Task<FileDownloadResponse> GetBouncedChequesReport(BouncedChequesFiltercs filters)
        {
            FileDownloadResponse fileDownloadResponse = new FileDownloadResponse();

            try
            {
                // ApiUrl = "BouncedChequesSummary/GetBouncedChequesReport";
                Console.WriteLine($"Bounced Cheque Report Filter ... {filters}");
                ApiUrl = apiConfig.GetBouncedChequesReport ?? " ";
                Console.WriteLine($"Bounced Cheque Report URL ... {ApiUrl}");
                fileDownloadResponse = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filters);
            }
            catch (Exception)
            {
                return fileDownloadResponse;
            }
            return fileDownloadResponse;
        }
    }
