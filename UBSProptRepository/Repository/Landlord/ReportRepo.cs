using System;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

public class ReportsRepository
{
    //private readonly ApiHelper apiHelper;
    //public ReportsRepository(ApiHelper _apiHelper)
    //{
    //    apiHelper = _apiHelper;
    //}
    //apiConfig apiConfig = apiConfig.Instance;

    private readonly UBSApiHelper apiHelper;
    private readonly UBSLandlordApiConfig apiConfig;
    public ReportsRepository(UBSApiHelper _apiHelper, IOptions<UBSLandlordApiConfig> _apiConfig)
    {
        apiHelper = _apiHelper;
        apiConfig = _apiConfig.Value;
    }

    public async Task<dynamic> GetreportFilters(dynamic data)
    {
        var url = apiConfig.GetLandlordReportDropDownType ?? "";
        Console.WriteLine($"Request type: {data} & Request URL: {url} ");
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> GetreportSummary(dynamic data, dynamic reportName)
    {
        var url = "";
        if (reportName == "LPO Report")
        {
            url = apiConfig.GenerateLPOReportSummary;
        }
        else if (reportName == "AMC Report")
        {
            url = apiConfig.GenerateAMCReportSummary;
        }
        else if (reportName == "Cheque Register Report")
        {
            url = apiConfig.GenerateChequeRegisterReportSummary;
        }
        else if (reportName == "VAT Report")
        {
            url = apiConfig.GenerateVATReportSummary;
        }
        else if (reportName == "Receipt Register Report")
        {
            url = apiConfig.GenerateReceiptRegisterReportSummary;
        }
        else if (reportName == "Unit Status Report")
        {
            url = apiConfig.GenerateUnitStatusReportSummary;
        }
        else if (reportName == "Building Status Report")
        {
            url = apiConfig.GenerateBuildingStatusReportSummary;
        }
        else if (reportName == "Occupancy Vacancy Register Report")
        {
            url = apiConfig.OccupancyVacancyRegisterSummary;
        }
        else if (reportName == "Legal Case Report")
        {
            url = apiConfig.GenerateLegalCaseReportSummary;
        }
        else if (reportName == "Tenancy Contracts Report")
        {
            url = apiConfig.GenerateContractReportSummary;
        }
        else
        {
            url = apiConfig.GenerateAMCReportSummary;
        }
        Console.WriteLine($"Request ReportName: {reportName} & Request URL: {url} ");
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
    public async Task<dynamic> DownloadReport(dynamic data, dynamic fileName)
    {
        var url = "";
        if (fileName == "Cheque Register Report")
        {
            url = apiConfig.DownloadGenerateChequeRegisterReport;
        }
        else if (fileName == "Legal Case Report")
        {
            url = apiConfig.DownloadGenerateLegalCaseReport;
        }
        else if (fileName == "Receipt Register Report")
        {
            url = apiConfig.DownloadGenerateReceiptRegisterReport;
        }
        else if (fileName == "Unit Status Report")
        {
            url = apiConfig.DownloadGenerateUnitStatusReport;
        }
        else if (fileName == "AMC Report")
        {
            url = apiConfig.DownloadAMCReport;
        }
        else if (fileName == "LPO Report")
        {
            url = apiConfig.DownloadGenerateLPOReport;
        }
        else if (fileName == "Building Expense Report")
        {
            url = apiConfig.DownloadGenerateBuildingStatusReport;
        }
        else if (fileName == "Tenancy Contracts Report")
        {
            url = apiConfig.GenerateContractReport;
        }
        else if (fileName == "Building Status Report")
        {
            url = apiConfig.DownloadGenerateBuildingStatusReport;
        }
        else if (fileName == "Occupancy Vacancy Register Report")
        {
            url = apiConfig.DownloadOccupancyVacancyRegisterReport;
        }
        else
        {
            url = apiConfig.DownloadGenerateVATReport;
        }
        Console.WriteLine($"Request type: {data} & Request URL: {url} ");
        String token = LabelsLandlordPortal.token;
        var response = await apiHelper.PostAsyncWithheader<dynamic>(url, data, token);
        return response;
    }
}
