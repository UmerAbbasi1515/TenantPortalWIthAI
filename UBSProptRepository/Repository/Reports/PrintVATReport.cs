using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PmsRepositories.Repositories.HttpClientHelper;
using PmsRepositories.ViewModels.UnitReport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IPrintVATInvoiceRepository
{
    Task<PrintVATInvoiceVM> GetPrintVATInvoice(PrintVATInvoiceFilter filter);
    Task<FileDownloadResponse> GetPrintVATInvoiceReport(PrintVATInvoiceFilter filter);
}

public class PrintVATInvoiceRepository : IPrintVATInvoiceRepository
{
    private readonly HttpClientService _httpClientService;
    string ApiUrl = string.Empty;
    private readonly UBSLandlordApiConfig apiConfig;
    public PrintVATInvoiceRepository(HttpClientService httpClientService, IConfiguration configuration, IOptions<UBSLandlordApiConfig> _apiConfig)
    {
        _httpClientService = httpClientService;
        apiConfig = _apiConfig.Value;
    }

    public async Task<PrintVATInvoiceVM> GetPrintVATInvoice(PrintVATInvoiceFilter filters)
    {
        PrintVATInvoiceVM printVATInvoiceVM = new PrintVATInvoiceVM();
        try
        {
            filters.ReportType = "Filter";
            Console.WriteLine($"Print VAT Invoice Report filters ... {filters}");
            ApiUrl = apiConfig.GetPrintVATInvoice ?? "";
            // ApiUrl = "PrintVATInvoice/GetPrintVATInvoice";
            Console.WriteLine($"Print VAT Invoice Report URL ... {ApiUrl}");
            printVATInvoiceVM = await _httpClientService.PostAsync<PrintVATInvoiceVM>(ApiUrl, filters);
            Console.WriteLine($"Print VAT Invoice Report try ... {printVATInvoiceVM}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Print VAT Invoice Report Catch ... {ex}");
            Console.WriteLine($"Print VAT Invoice Report Catch ... {printVATInvoiceVM}");
            return printVATInvoiceVM;
        }
        return printVATInvoiceVM;
    }

    public async Task<FileDownloadResponse> GetPrintVATInvoiceReport(PrintVATInvoiceFilter filters)
    {
        FileDownloadResponse fileDownloadResponse = new FileDownloadResponse();

        try
        {
            Console.WriteLine($"Print VAT Invoice Report filters ... {filters}");
            ApiUrl = apiConfig.GeneratePrintVATInvoiceReport ?? "";
            Console.WriteLine($"Print VAT Invoice Report URL ... {ApiUrl}");
            // ApiUrl = "PrintVATInvoice/GeneratePrintVATInvoiceReport";
            fileDownloadResponse = await _httpClientService.PostAsync<FileDownloadResponse>(ApiUrl, filters);
            Console.WriteLine($"Print VAT Invoice Report fileDownloadResponse try ... {fileDownloadResponse}");
        }
        catch (Exception)
        {
            Console.WriteLine($"Print VAT Invoice Report fileDownloadResponse Catch ... {fileDownloadResponse}");
            return fileDownloadResponse;
        }
        return fileDownloadResponse;
    }

}





