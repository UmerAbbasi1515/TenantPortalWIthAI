using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PmsRepositories.Repositories.HttpClientHelper;
using PmsRepositories.ViewModels.UnitReport;

    public interface IMontlyRentRecognitionReportRepository
    {
        Task<CommonResponseVM> GetMonthlyRentRecognitionData(MontlyRentRecognitionFilter propertyFilter);
        Task<FileDownloadResponse> GetMonthlyRentRecognitionReport(MontlyRentRecognitionFilter propertyFilter);
    }
    public class MontlyRentRecognitionReportRepository : IMontlyRentRecognitionReportRepository
    {

        private readonly HttpClientService _httpClientService;
    private readonly UBSLandlordApiConfig apiConfig;
    public MontlyRentRecognitionReportRepository(HttpClientService httpClientService, IConfiguration configuration, IOptions<UBSLandlordApiConfig> _apiConfig)
    {
        _httpClientService = httpClientService;
        apiConfig = _apiConfig.Value;
    }

    public async Task<CommonResponseVM> GetMonthlyRentRecognitionData(MontlyRentRecognitionFilter filters)
        {
            CommonResponseVM propertyVM = new CommonResponseVM();

            try
            {
                Console.WriteLine($"Monthly Rent R Report Filter ... {filters}");
                string ApiUrl = apiConfig.GetMonthlyRentRecognitionData ?? "";
                Console.WriteLine($"Monthly Rent R Report Filter ... {ApiUrl}");
                // string ApiUrl = "MonthlyRentRecognition/GetMonthlyRentRecognitionData";
                propertyVM = await _httpClientService.PostAsync<CommonResponseVM>(ApiUrl, filters);
            }
            catch (Exception)
            {
            }
            return propertyVM;
        }

        public async Task<FileDownloadResponse> GetMonthlyRentRecognitionReport(MontlyRentRecognitionFilter filters)
        {
            FileDownloadResponse fileDownload = new FileDownloadResponse();

            try
            {
                Console.WriteLine($"Monthly Rent R Report Filter ... {filters}");
                string ApiUrl = apiConfig.GetMonthlyRentRecognitionReport ?? "";
                Console.WriteLine($"Monthly Rent R Report Filter ... {ApiUrl}");
                // string ApiUrl = "MonthlyRentRecognition/GetMonthlyRentRecognitionReport";
                fileDownload = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filters);
            }
            catch (Exception)
            {
            }
            return fileDownload;
        }

    }

