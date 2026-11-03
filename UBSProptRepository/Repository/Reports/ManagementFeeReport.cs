using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PmsRepositories.Repositories.HttpClientHelper;
using PmsRepositories.ViewModels.UnitReport;
using System.Data;


public interface IManagementFeeRepository
{
    // Task<ManagementFeeViewModel> GetManagementFeeList(ManagementFeeViewModel managementFeeVM);
    // Task<int> GenerateManagementFeeInvoices(ManagementFeeViewModel managementFeeVM);
    // Task<ManagementFeeViewModel> GetManagementFeeInvoices(ManagementFeeViewModel managementFeeVM);


    #region Management Fee Report
    Task<CommonResponseVM> GetManagmentFeeData(ManagmentFeeFilterReport filter);
    Task<FileDownloadResponse> GetManagmentFeeReport(ManagmentFeeFilterReport filter);

    #endregion

    // Task<ManagementFeeViewModel> GetPropertyWiseManagementFee(ManagementFeeViewModel managementFeeVM);
}
public class ManagementFeeRepository : IManagementFeeRepository
{
    private readonly string _connectionString;
    private readonly HttpClientService _httpClientService;
    // string ApiBaseUrl = string.Empty;
    // string ApiUrl = string.Empty;
    //apiConfig apiConfig = apiConfig.Instance;
    private readonly UBSLandlordApiConfig apiConfig;
    public ManagementFeeRepository(HttpClientService httpClientService, IConfiguration configuration, IOptions<UBSLandlordApiConfig> _apiConfig)
    {
        _httpClientService = httpClientService;
        apiConfig = _apiConfig.Value;
    }

    // public async Task<int> GenerateManagementFeeInvoices(ManagementFeeViewModel managementFeeVM)
    // {
    //     try
    //     {
    //         int returnVal = 0;
    //         if (managementFeeVM.generateInvoiceVM != null && managementFeeVM.generateInvoiceVM.Count > 0)
    //         {
    //             foreach (var item in managementFeeVM.generateInvoiceVM)
    //             {
    //                 if (item.IsSelected)
    //                 {
    //                     using (var connection = new SqlConnection(_connectionString))
    //                     {
    //                         await connection.OpenAsync();
    //                         var dynamicParams = new DynamicParameters();
    //                         dynamicParams.Add("@PropertyId", item.PropertyId);
    //                         dynamicParams.Add("@TotalAmount", item.Fees);
    //                         dynamicParams.Add("@ManagementFeeIds", item.ManagementFeeIds);

    //                         returnVal = await connection.ExecuteAsync("SP_GenerateManagementFeeInvoices", dynamicParams, commandType: CommandType.StoredProcedure);
    //                     }
    //                 }
    //             }
    //         }

    //         return returnVal;
    //     }
    //     catch (Exception ex)
    //     {
    //         throw;
    //     }
    // }

    // public async Task<ManagementFeeViewModel> GetManagementFeeList(ManagementFeeViewModel managementFeeVM)
    // {
    //     if (managementFeeVM.PageSize == 0)
    //         managementFeeVM.PageSize = 10;
    //     if (managementFeeVM.PageNumber == 0)
    //         managementFeeVM.PageNumber = 1;

    //     int SkipRows = managementFeeVM.PageSize * managementFeeVM.PageNumber;
    //     try
    //     {
    //         using (var connection = new SqlConnection(_connectionString))
    //         {

    //             await connection.OpenAsync();
    //             var dynamicParams = new DynamicParameters();
    //             dynamicParams.Add("@PageSize", managementFeeVM.PageSize);
    //             dynamicParams.Add("@PageNumber", managementFeeVM.PageNumber);
    //             dynamicParams.Add("@FromDate", managementFeeVM.FromDate);
    //             dynamicParams.Add("@ToDate", managementFeeVM.Todate);
    //             dynamicParams.Add("@PropertyId", managementFeeVM.PropertyId);
    //             dynamicParams.Add("@ManagementFeeMethodId", managementFeeVM.ManagementFeeMethodId);
    //             dynamicParams.Add("@ContractNo", managementFeeVM.ContractNo);
    //             dynamicParams.Add("@PreviousUnInvoiced", managementFeeVM.PreviousUnInvoiced);
    //             dynamicParams.Add("@UnInvoiced", managementFeeVM.UnInvoiced);


    //             var result = await connection.QueryAsync<ManagementFeeDetailViewModel>("SP_GetManagementFees", dynamicParams, commandType: CommandType.StoredProcedure);
    //             managementFeeVM.managementFeeDetailList = result.ToList();
    //             if (managementFeeVM.managementFeeDetailList.Any()) managementFeeVM.TotalRecord = managementFeeVM.managementFeeDetailList.FirstOrDefault().TotalRecord;

    //             return managementFeeVM;
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex);
    //         throw;
    //     }
    // }
    // public async Task<ManagementFeeViewModel> GetManagementFeeInvoices(ManagementFeeViewModel managementFeeVM)
    // {
    //     try
    //     {
    //         if (managementFeeVM.PageNumber == 0)
    //             managementFeeVM.PageNumber = 1;
    //         using (var connection = new SqlConnection(_connectionString))
    //         {

    //             await connection.OpenAsync();
    //             var dynamicParams = new DynamicParameters();
    //             dynamicParams.Add("@PageSize", managementFeeVM.PageSize);
    //             dynamicParams.Add("@PageNumber", managementFeeVM.PageNumber);
    //             dynamicParams.Add("@FromDate", managementFeeVM.FromDate);
    //             dynamicParams.Add("@ToDate", managementFeeVM.Todate);
    //             dynamicParams.Add("@PropertyId", managementFeeVM.PropertyId);
    //             dynamicParams.Add("@LandlordId", managementFeeVM.LandlordId);
    //             dynamicParams.Add("@InvoiceNo", managementFeeVM.InvoiceNo);


    //             var result = await connection.QueryAsync<ManagementFeeInvoicesViewModel>("SP_GetManagementFeeInvoices", dynamicParams, commandType: CommandType.StoredProcedure);

    //             managementFeeVM.managementFeeInvoices = result.ToList();

    //             if (managementFeeVM.managementFeeInvoices.Any()) managementFeeVM.TotalRecord = managementFeeVM.managementFeeInvoices.FirstOrDefault().TotalRecord;

    //             return managementFeeVM;
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex);
    //         throw;
    //     }
    // }

    public async Task<CommonResponseVM> GetManagmentFeeData(ManagmentFeeFilterReport filter)
    {
        CommonResponseVM propertyVM = new CommonResponseVM();

        try
        {
            string ApiUrl = apiConfig.GetManagementFeeData ?? "";
            propertyVM = await _httpClientService.PostAsync<CommonResponseVM>(ApiUrl, filter);
        }
        catch (Exception)
        {
        }
        return propertyVM;
    }

    public async Task<FileDownloadResponse> GetManagmentFeeReport(ManagmentFeeFilterReport filter)
    {
        FileDownloadResponse fileDownload = new FileDownloadResponse();

        try
        {
            string ApiUrl = apiConfig.GetManagementFeeReport ?? "";
            fileDownload = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filter);
        }
        catch (Exception)
        {
        }
        return fileDownload;
    }

    //     public async Task<ManagementFeeViewModel> GetPropertyWiseManagementFee(ManagementFeeViewModel managementFeeVM)
    //     {
    //         try
    //         {
    //             var selectedManagementFeeIds = managementFeeVM.managementFeeDetailList
    // .Where(x => x.IsSelected)
    // .Select(x => x.ManagementFeeID)
    // .ToList();
    //             managementFeeVM = await GetManagementFeeList(managementFeeVM);
    //             var groupedMgmentFee = managementFeeVM.managementFeeDetailList.Where(x => string.IsNullOrEmpty(x.InvoiceNo) && selectedManagementFeeIds.Contains(x.ManagementFeeID))
    //                    .GroupBy(fee => new { fee.PropertyName, fee.PropertyId })
    //                    .Select(fee => new GenerateInvoiceViewModel()
    //                    {
    //                        PropertyId = fee.Key.PropertyId,
    //                        PropertyName = fee.Key.PropertyName,
    //                        Fees = fee.Sum(fee => fee.Fees),
    //                        ManagementFeeIds = string.Join(", ", fee.Select(f => f.ManagementFeeID))
    //                    }).ToList();
    //             managementFeeVM.generateInvoiceVM = groupedMgmentFee;

    //             return managementFeeVM;
    //         }
    //         catch (Exception ex)
    //         {
    //             Console.WriteLine(ex);
    //             return null;
    //         }
    //     }

}

