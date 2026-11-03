using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text.Json;

public class UBSApiHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly HttpClient _httpClient;

    public UBSApiHelper(IHttpContextAccessor httpContextAccessor)
    {
        HttpClientHandler handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
        _httpClient = new HttpClient(handler);
        // Set default headers, if needed
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<T?> GetAsync<T>(string url, string token)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        try
        {

            // ✅ Auto-fetch token from session if not provided
            if (string.IsNullOrEmpty(token))
            {
                var context = _httpContextAccessor.HttpContext;
                token = context?.Session.GetString("JWTToken") ?? "";
            }
            Console.WriteLine($"Requested URL: {url}");
            Console.WriteLine($"Token ::::::: {token}");
            string requestJson = JsonConvert.SerializeObject(Formatting.Indented);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            // Clear previous headers
            _httpClient.DefaultRequestHeaders.Clear();
            // _httpClient.Timeout = TimeSpan.FromSeconds(30);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Add("ApiKey", "UZ5QdWg2dLm1sop09082AccIa03DxupsIWDt9rqfeM");
            _httpClient.DefaultRequestHeaders.Add("raw", "datawithoutfile");
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            // var jsonResponse = await response.Content.ReadAsStringAsync();
            // return JsonConvert.DeserializeObject<T>(jsonResponse);
            dynamic data = new { };
            return await GetResponse(response, url, data, _httpContextAccessor);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error:::===>>>>>: {e}");
            // RedirectToErrorPage(url, "Unauthorized: Please login again.");
            // return default;
            if (e.Message.Contains("Response status code does not indicate success: 401 (Unauthorized)"))
            {
                return (T)(object)RedirectToErrorPage(url, "Response status code does not indicate success: 401 (Unauthorized)");
                // return (T)(object)"Unauthorized: Please login again."; // Return an error message
            }
            else if (e.Message.Contains("Response status code does not indicate success: 503 (Service Unavailable)."))
            {
                return (T)(object)RedirectToErrorPage(url, "Response status code does not indicate success: 503 (Service Unavailable).");
                // return (T)(object)"Response status code does not indicate success: 503 (Service Unavailable).";
            }
            else if (e.Message.Contains("Response status code does not indicate success: 500 (Internal Server Error)"))
            {
                return (T)(object)RedirectToErrorPage(url, "Response status code does not indicate success: 500 (Internal Server Error)");
                // return (T)(object)LabelsRepository.AnyError;
            }
            else
            {
                return default; // Return default when no specific error is found
            }
        }
    }

    private string RedirectToErrorPage(string url, string errorMessage)
    {
        // Construct a query string with the error and URL

        string redirectUrl = $"/ErrorPage?message={HttpUtility.UrlEncode(errorMessage)}&url={HttpUtility.UrlEncode(url)}";
        return redirectUrl;  // This URL can be used to redirect in the calling page/controller
    }

    // Update with latest
    public async Task<dynamic?> PostAsyncWithheader<T>(string url, object data, string token)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        try
        {
            // ✅ Auto-fetch token from session if not provided
            if (string.IsNullOrEmpty(token))
            {
                var context = _httpContextAccessor.HttpContext;
                token = context?.Session.GetString("JWTToken") ?? "";
            }
           
            string requestJson11 = JsonConvert.SerializeObject(data, Formatting.Indented);
            Console.WriteLine($"Data11:::: {requestJson11},");
            string encrypted11 = await AesEncryptionHelper.EncryptDataAsync(data);
            string encryptedData11 = $"{{'requestBody': '{encrypted11}'}}";
            Console.WriteLine($"formattedResult Data: {encryptedData11}");
            Console.WriteLine($"Requested URL: {url}");
            string requestJson = JsonConvert.SerializeObject(data, Formatting.Indented);
            Console.WriteLine($"Data:::: {requestJson},");
            string encrypted = await AesEncryptionHelper.EncryptDataAsync(data);
            string encryptedData = $"{{'requestBody': '{encrypted}'}}";
            Console.WriteLine($"formattedResult Data: {encryptedData}");
            Console.WriteLine($"Token ::::::: {token}");
            var content = new StringContent(encryptedData, Encoding.UTF8, "application/json");
            // Clear previous headers
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Add("ApiKey", "UZ5QdWg2dLm1sop09082AccIa03DxupsIWDt9rqfeM");
            _httpClient.DefaultRequestHeaders.Add("raw", "datawithoutfile");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
            var response = await _httpClient.PostAsync(url, content);
            //response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var json = JsonDocument.Parse(body);
                    // Try to get the message property first
                    if (json.RootElement.TryGetProperty("message", out var message))
                    {
                        return message.GetString();
                    }
                    // Fallback to exceptionMessage if message doesn't exist
                    else if (json.RootElement.TryGetProperty("exceptionMessage", out var exceptionMessage))
                    {
                        return exceptionMessage.GetString();
                    }
                    // Handle case like: {"status":"Data not found","fgpCorrectionId":"","proposedRemedy":"","description":""}
                    else if (json.RootElement.TryGetProperty("status", out var status))
                    {
                        return status.GetString();
                    }
                    else
                    {
                        // Default fallback
                        return "Unknown error occurred";
                    }
                }
                catch
                {
                    // JSON parse failed, fallback to body
                    return "An error occurred while processing your request";
                }

                //return "Error: " + body;
            }
            return await GetResponse(response, url, data, _httpContextAccessor);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine($"Request error e.Message : {e.Message}");
            if (e.Message.Contains("Response status code does not indicate success: 401 (Unauthorized)"))
            {
                return AuthLabelsPortal.Unauthorized;
            }
            //else if (e.Message.Contains("Response status code does not indicate success: 503 (Service Unavailable).")) { }
            else if (e.Message.Contains("Response status code does not indicate success: 503 (Service Unavailable)."))
            {
                return "Response status code does not indicate success: 503 (Service Unavailable).";
            }
            else if (e.Message.Contains("Response status code does not indicate success: 500 (Internal Server Error)"))
            {
                return AuthLabelsPortal.internalServerError;
            }
            else if (e.Message.Contains("Response status code does not indicate success: 404 (Not Found)."))
            {
                return AuthLabelsPortal.NoDataFound;
            }
            else
            {
                return e.Message;
            }
        }
    }


    //public async Task<dynamic?> PostAsyncWithheader<T>(string url, object data, string token)
    //{
    //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

    //    try
    //    {
    //        //var data11 = new { OfferId = "1" };
    //        //string requestJson11 = JsonConvert.SerializeObject(data11, Formatting.Indented);
    //        //Console.WriteLine($"Data11:::: {requestJson11},");
    //        //string encrypted11 = await AesEncryptionHelper.EncryptDataAsync(data11);
    //        //string encryptedData11 = $"{{'requestBody': '{encrypted11}'}}";
    //        //Console.WriteLine($"formattedResult Data: {encryptedData11}");
    //        Console.WriteLine($"Requested URL: {url}");
    //        string requestJson = JsonConvert.SerializeObject(data, Formatting.Indented);
    //        Console.WriteLine($"Data:::: {requestJson},");
    //        string encrypted = await AesEncryptionHelper.EncryptDataAsync(data);
    //        string encryptedData = $"{{'requestBody': '{encrypted}'}}";
    //        Console.WriteLine($"formattedResult Data: {encryptedData}");
    //        Console.WriteLine($"Token ::::::: {token}");
    //        var content = new StringContent(encryptedData, Encoding.UTF8, "application/json");
    //        // Clear previous headers
    //        _httpClient.DefaultRequestHeaders.Clear();
    //        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    //        _httpClient.DefaultRequestHeaders.Add("ApiKey", "UZ5QdWg2dLm1sop09082AccIa03DxupsIWDt9rqfeM");
    //        _httpClient.DefaultRequestHeaders.Add("raw", "datawithoutfile");
    //        var response = await _httpClient.PostAsync(url, content);
    //        response.EnsureSuccessStatusCode();
    //        return await GetResponse(response, url, data, _httpContextAccessor);
    //    }
    //    catch (HttpRequestException e)
    //    {
    //        Console.WriteLine(e.Message);
    //        Console.WriteLine($"Request error e.Message : {e.Message}");
    //        if (e.Message.Contains("Response status code does not indicate success: 401 (Unauthorized)"))
    //        {
    //            // var session = _httpContextAccessor.HttpContext.Session;
    //            // session.SetString("JWTToken", "");
    //            // session.SetString("LandlordID", "");
    //            return AuthLabelsPortal.Unauthorized;
    //        }
    //        else if (e.Message.Contains("Response status code does not indicate success: 503 (Service Unavailable)."))
    //        {
    //            return "Response status code does not indicate success: 503 (Service Unavailable).";
    //        }
    //        else if (e.Message.Contains("Response status code does not indicate success: 500 (Internal Server Error)"))
    //        {
    //            return AuthLabelsPortal.AnyError;
    //        }
    //        else if (e.Message.Contains("Response status code does not indicate success: 404 (Not Found)."))
    //        {
    //            return AuthLabelsPortal.NoDataFound;
    //        }
    //        else
    //        {
    //            return e.Message;
    //        }
    //    }
    //}

    public async Task<dynamic?> PostAsyncWithheaderWithoutEncrypt<T>(string url, object data, string token)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        try
        {

            // ✅ Auto-fetch token from session if not provided
            if (string.IsNullOrEmpty(token))
            {
                var context = _httpContextAccessor.HttpContext;
                token = context?.Session.GetString("JWTToken") ?? "";
            }
            Console.WriteLine($"Requested URL: {url}");
            string requestJson = JsonConvert.SerializeObject(data, Formatting.Indented);
            Console.WriteLine($"Data:::: {requestJson},");
            Console.WriteLine($"Token ::::::: {token}");
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            // Clear previous headers
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Add("ApiKey", "UZ5QdWg2dLm1sop09082AccIa03DxupsIWDt9rqfeM");
            _httpClient.DefaultRequestHeaders.Add("raw", "datawithoutfile");
            //var response = await _httpClient.GetAsync(url);
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await GetResponse(response, url, data, _httpContextAccessor);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine($"Request error e.Message : {e.Message}");
            if (e.Message.Contains("Response status code does not indicate success: 401 (Unauthorized)"))
            {
                // var session = _httpContextAccessor.HttpContext.Session;
                // session.SetString("JWTToken", "");
                // session.SetString("LandlordID", "");
                return AuthLabelsPortal.Unauthorized;
            }
            else if (e.Message.Contains("Response status code does not indicate success: 503 (Service Unavailable)."))
            {
                return "Response status code does not indicate success: 503 (Service Unavailable).";
            }
            else if (e.Message.Contains("Response status code does not indicate success: 500 (Internal Server Error)"))
            {
                return AuthLabelsPortal.AnyError;
            }
            else
            {
                return e.Message;
            }
        }
    }

    public static async Task<dynamic> GetResponse(HttpResponseMessage response, string url, dynamic data, IHttpContextAccessor httpContextAccessor)
    {

        Console.WriteLine($"Response Body :::: inside GetResponse:: {url} ::: {data}  ::: {await response.Content.ReadAsStringAsync()}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            if (responseBody.Contains("HTML") || responseBody.Contains("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN"))
            {
                Console.WriteLine(responseBody);
                // Handle the response accordingly
                // e.g., navigate to a specific UI page
            }
        }
        var body = await response.Content.ReadAsStringAsync();
        if (body.Contains("BadRequestExecution Timeout Expired"))
        {
            Console.WriteLine($"Response in helper :: {body}");
            return AuthLabelsPortal.ConnectionTimedOut;
        }
        Console.WriteLine($"Response in helper :: {response.StatusCode}");

        // var session = httpContextAccessor.HttpContext.Session;
        switch (response.StatusCode)
        {
            case System.Net.HttpStatusCode.OK:

                return JsonConvert.DeserializeObject<dynamic>(body)!;
            case System.Net.HttpStatusCode.BadRequest:
                return AuthLabelsPortal.BadRequest;
            case System.Net.HttpStatusCode.Unauthorized:
                //httpContextAccessor?.HttpContext?.Response.Redirect("/Users/Auth/AuthScreen");

                return AuthLabelsPortal.Unauthorized;
            case System.Net.HttpStatusCode.Forbidden:
                //httpContextAccessor?.HttpContext?.Response.Redirect("/Users/Auth/AuthScreen");

                Console.WriteLine($"Response in helper Forbidden:: {AuthLabelsPortal.Unauthorized}");
                return AuthLabelsPortal.Unauthorized;
            case System.Net.HttpStatusCode.NotFound:
                return JsonConvert.DeserializeObject<dynamic>(body)!;
            //return AuthLabelsPortal.NoDataFound;
            case System.Net.HttpStatusCode.InternalServerError:
                return AuthLabelsPortal.AnyError;
            case System.Net.HttpStatusCode.NotImplemented:
                return AuthLabelsPortal.ProcessError;
            case System.Net.HttpStatusCode.ServiceUnavailable:
                return "Service Unavailable";
            default:
                return AuthLabelsPortal.CouldNotConnectToServer;
        }
    }

    public void ClearSession()
    {
        // Access the HttpContext and clear session values
        var session = _httpContextAccessor?.HttpContext?.Session;
        session?.SetString("JWTToken", "");
        session?.SetString("LandlordID", "");
    }



    // public static async Task<dynamic> GetResponse(HttpResponseMessage response, string url, dynamic data)
    // {
    //     // Console.WriteLine($"Response Body :::: inside GetResponse:: {url} ::: {data} ::: {await response.Content.ReadAsStringAsync()}");

    //     if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
    //     {
    //         var responseBody = await response.Content.ReadAsStringAsync();
    //         if (responseBody.Contains("HTML") || responseBody.Contains("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN"))
    //         {
    //             Console.WriteLine(responseBody);
    //             // Handle the response accordingly
    //             // e.g., navigate to a specific UI page
    //         }
    //     }


    //     var body = await response.Content.ReadAsStringAsync();
    //     if (body.Contains("BadRequestExecution Timeout Expired"))
    //     {
    //         Console.WriteLine($"Response in helper :: {body}");

    //         return LabelsRepository.ConnectionTimedOut;
    //     }

    //     Console.WriteLine($"Response in helper :: {System.Net.HttpStatusCode.OK}");
    //     switch (response.StatusCode)
    //     {
    //         case System.Net.HttpStatusCode.OK:
    //             return LabelsRepository.Unauthorized;
    //         // case System.Net.HttpStatusCode.OK:
    //         //     return JsonConvert.DeserializeObject<dynamic>(body)!;

    //         case System.Net.HttpStatusCode.BadRequest:

    //         case System.Net.HttpStatusCode.Unauthorized:
    //             return LabelsRepository.Unauthorized;

    //         case System.Net.HttpStatusCode.Forbidden:
    //             return LabelsRepository.Unauthorized;

    //         case System.Net.HttpStatusCode.NotFound:
    //             return LabelsRepository.NoDataFound;

    //         case System.Net.HttpStatusCode.InternalServerError:
    //             Console.WriteLine(body);
    //             return LabelsRepository.AnyError;

    //         case System.Net.HttpStatusCode.NotImplemented:
    //             Console.WriteLine(body);
    //             // Show snackbar or message
    //             return LabelsRepository.NoDataFound;

    //         default:
    //             // Handle other cases
    //             return LabelsRepository.CouldNotConnectToServer;
    //     }
    // }


    // Latest after update issue Mode Security
    //CaseNo,AttachmentType,ExpireDate,DocumentTypeId,filePath
    public async Task<dynamic?> UploadServiceRequestVenReportType(
    string caseNo, string attachmentType, string expireDate, string DocumentTypeId, string url, string token, string filePath)
    {
        HttpClientHandler handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        HttpClient client = new HttpClient(handler);


        if (string.IsNullOrEmpty(token))
        {
            var context = _httpContextAccessor.HttpContext;
            token = context?.Session.GetString("JWTToken") ?? "";
        }
        Console.WriteLine($"Requested URL: {url}");
        Console.WriteLine($"Token ::::::: {token}");

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        client.DefaultRequestHeaders.Add("ApiKey", "UZ5QdWg2dLm1sop09082AccIa03DxupsIWDt9rqfeM");
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
        var tempCaseNO = AesEncryptionHelper.EncryptSingleField(caseNo);
        var tempAttachmentType = AesEncryptionHelper.EncryptSingleField(attachmentType);
        var tempExpiryDate = AesEncryptionHelper.EncryptSingleField(expireDate);
        var tempDocumentTypeId = AesEncryptionHelper.EncryptSingleField(DocumentTypeId);

        using var form = new MultipartFormDataContent();
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(caseNo)), "CaseNo");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(attachmentType)), "AttachmentType");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(expireDate)), "ExpireDate");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(DocumentTypeId)), "DocumentTypeId");


     
        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
        {
            var fileBytes = File.ReadAllBytes(filePath);
            var fileContent = new ByteArrayContent(fileBytes);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            form.Add(fileContent, "File", Path.GetFileName(filePath));
        }



        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        try
        {
            Console.WriteLine(url);

            var response = await client.PostAsync(url, form);
            Console.WriteLine(response);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("Unauthorized: Token may be expired");
                return response;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (responseContent.Contains("<!DOCTYPE HTML") || responseContent.Contains("HTML"))
                {
                    Console.WriteLine("404 with HTML content:\n" + responseContent);
                    return response;
                }
            }

            string json1 = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<dynamic>(json1);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception while uploading: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                ReasonPhrase = ex.Message
            };
        }
    }

    // Latest after update issue Mode Security
    public async Task<dynamic?> UploadServiceRequestEIDReportType(string caseNo, string attachmentType, string expireDate, string DocumentTypeId, string url, string token, string filePath, string emirateIdNumber, string nationality,
               string nameEng, string nameAr, string dOB)
    {

        HttpClientHandler handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        HttpClient client = new HttpClient(handler);


        // ✅ Auto-fetch token from session if not provided
        if (string.IsNullOrEmpty(token))
        {
            var context = _httpContextAccessor.HttpContext;
            token = context?.Session.GetString("JWTToken") ?? "";
        }
        Console.WriteLine($"Requested URL: {url}");
        Console.WriteLine($"Token ::::::: {token}");
        // Clear previous headers
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        client.DefaultRequestHeaders.Add("ApiKey", "UZ5QdWg2dLm1sop09082AccIa03DxupsIWDt9rqfeM");
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");

        using var form = new MultipartFormDataContent();
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(caseNo)), "CaseNo");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(attachmentType)), "AttachmentType");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(expireDate)), "ExpireDate");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(DocumentTypeId)), "DocumentTypeId");

        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(emirateIdNumber)), "EmirateIdNumber");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(nationality)), "Nationality");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(nameEng)), "NameEng");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(nameAr)), "NameAr");

        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField("")), "IssueDate");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(dOB)), "DOB");


        // 📎 File Upload (like Postman)
        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
        {
            var fileBytes = File.ReadAllBytes(filePath);
            var fileContent = new ByteArrayContent(fileBytes);

            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            // "File" should match the key name in Postman
            form.Add(fileContent, "File", Path.GetFileName(filePath));
        }



        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        try
        {
            Console.WriteLine(url);

            var response = await client.PostAsync(url, form);
            Console.WriteLine(response);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("Unauthorized: Token may be expired");
                return response;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (responseContent.Contains("<!DOCTYPE HTML") || responseContent.Contains("HTML"))
                {
                    Console.WriteLine("404 with HTML content:\n" + responseContent);
                    return response;
                }
            }

            string json1 = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<dynamic>(json1);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception while uploading: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                ReasonPhrase = ex.Message
            };
        }
    }
    // Latest after update issue Mode Security
    public async Task<dynamic?> UpdateServiceRequestEIDReportType(string expireDate, string DocumentTypeId, string url, string token, string filePath)
    {

        HttpClientHandler handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        HttpClient client = new HttpClient(handler);


        // ✅ Auto-fetch token from session if not provided
        if (string.IsNullOrEmpty(token))
        {
            var context = _httpContextAccessor.HttpContext;
            token = context?.Session.GetString("JWTToken") ?? "";
        }
        Console.WriteLine($"Requested URL: {url}");
        Console.WriteLine($"Token ::::::: {token}");
        // Clear previous headers
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        client.DefaultRequestHeaders.Add("ApiKey", "UZ5QdWg2dLm1sop09082AccIa03DxupsIWDt9rqfeM");
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");

        var tempAttachmentID = AesEncryptionHelper.EncryptSingleField(DocumentTypeId);
        var tempExpireDate = AesEncryptionHelper.EncryptSingleField(expireDate);


        using var form = new MultipartFormDataContent();
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(DocumentTypeId)), "AttachmentId");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(expireDate)), "ExpireDate");


        // 📎 File Upload (like Postman)
        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
        {
            var fileBytes = File.ReadAllBytes(filePath);
            var fileContent = new ByteArrayContent(fileBytes);

            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            // "File" should match the key name in Postman
            form.Add(fileContent, "File", Path.GetFileName(filePath));
        }



        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        try
        {
            Console.WriteLine(url);

            var response = await client.PostAsync(url, form);
            Console.WriteLine(response);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("Unauthorized: Token may be expired");
                return response;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (responseContent.Contains("<!DOCTYPE HTML") || responseContent.Contains("HTML"))
                {
                    Console.WriteLine("404 with HTML content:\n" + responseContent);
                    return response;
                }
            }

            string json1 = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<dynamic>(json1);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception while uploading: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                ReasonPhrase = ex.Message
            };
        }
    }

    public async Task<dynamic?> UploadInvoiceVendorDocType(
   string caseNo, string RequestId, string attachmentType, string expireDate, string DocumentTypeId, string url, string token, string filePath)
    {
        HttpClientHandler handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        HttpClient client = new HttpClient(handler);


        if (string.IsNullOrEmpty(token))
        {
            var context = _httpContextAccessor.HttpContext;
            token = context?.Session.GetString("JWTToken") ?? "";
        }
        Console.WriteLine($"Requested URL: {url}");
        Console.WriteLine($"Token ::::::: {token}");

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        client.DefaultRequestHeaders.Add("ApiKey", "UZ5QdWg2dLm1sop09082AccIa03DxupsIWDt9rqfeM");
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
        var tempCaseNO = AesEncryptionHelper.EncryptSingleField(caseNo);
        var tempAttachmentType = AesEncryptionHelper.EncryptSingleField(attachmentType);
        var tempExpiryDate = AesEncryptionHelper.EncryptSingleField(expireDate);
        var tempDocumentTypeId = AesEncryptionHelper.EncryptSingleField(DocumentTypeId);

        using var form = new MultipartFormDataContent();
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(expireDate)), "ExpireDate");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(attachmentType)), "AttachmentType");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(RequestId)), "RequestId");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(caseNo)), "CaseNo");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(DocumentTypeId)), "DocumentTypeId");



        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
        {
            var fileBytes = File.ReadAllBytes(filePath);
            var fileContent = new ByteArrayContent(fileBytes);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            form.Add(fileContent, "File", Path.GetFileName(filePath));
        }



        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        try
        {
            Console.WriteLine(url);

            var response = await client.PostAsync(url, form);
            Console.WriteLine(response);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("Unauthorized: Token may be expired");
                return response;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (responseContent.Contains("<!DOCTYPE HTML") || responseContent.Contains("HTML"))
                {
                    Console.WriteLine("404 with HTML content:\n" + responseContent);
                    return response;
                }
            }

            string json1 = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<dynamic>(json1);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception while uploading: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                ReasonPhrase = ex.Message
            };
        }
    }


    public async Task<dynamic?> UploadOfferFile(
    string addedBy, string expiry, string masterID, string title, string titleAR,
    string documentType, string suspend, string filePath, string url, string token)
    {
        var handler = new HttpClientHandler();
#if DEBUG
        handler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator; // dev only
#endif
        using var client = new HttpClient(handler);

        if (string.IsNullOrEmpty(token))
        {
            token = _httpContextAccessor.HttpContext?.Session.GetString("JWTToken") ?? "";
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        client.DefaultRequestHeaders.Add("ApiKey", "UZ5QdWg2dLm1sop09082AccIa03DxupsIWDt9rqfeM");
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");

        using var form = new MultipartFormDataContent();
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(documentType)), "DocumentType");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(masterID)), "MasterID");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(title)), "Title");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(titleAR)), "TitleAR");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(addedBy)), "AddedBy");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(suspend)), "Suspend");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(expiry)), "ExpiryDate");

        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
        {
            var fileBytes = await File.ReadAllBytesAsync(filePath);
            var fileContent = new ByteArrayContent(fileBytes);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            form.Add(fileContent, "File", Path.GetFileName(filePath));
        }

        try
        {
            var response = await client.PostAsync(url, form);
            var body = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("Unauthorized: token may be expired");
                return response;
            }

            if (response.StatusCode == HttpStatusCode.NotFound && body.Contains("<!DOCTYPE", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("404 with HTML content:\n" + body);
                return response;
            }

            return JsonConvert.DeserializeObject<dynamic>(body);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception while uploading: " + ex.Message);
            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                ReasonPhrase = ex.Message
            };
        }
    }

    // Latest after update issue Mode Security
    // caseNo , message , filePath
    public async Task<dynamic?> UploadServiceRequestFiles(
     string url,
     string caseNo,
     string message,
     string token,
     string filePath)
    {

        HttpClientHandler handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        HttpClient client = new HttpClient(handler);


        // ✅ Auto-fetch token from session if not provided
        if (string.IsNullOrEmpty(token))
        {
            var context = _httpContextAccessor.HttpContext;
            token = context?.Session.GetString("JWTToken") ?? "";
        }
        Console.WriteLine($"Requested URL: {url}");
        Console.WriteLine($"Token ::::::: {token}");
        // Clear previous headers
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        client.DefaultRequestHeaders.Add("ApiKey", "UZ5QdWg2dLm1sop09082AccIa03DxupsIWDt9rqfeM");
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");

        var tempCaseNO =  AesEncryptionHelper.EncryptSingleField(caseNo);
        var tempMessage = AesEncryptionHelper.EncryptSingleField(message);

        using var form = new MultipartFormDataContent();
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(caseNo)), "CaseId");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(message)), "Reply");

      
        // 📎 File Upload (like Postman)
        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
        {
            var fileBytes = File.ReadAllBytes(filePath);
            var fileContent = new ByteArrayContent(fileBytes);

            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            // "File" should match the key name in Postman
            form.Add(fileContent, "File", Path.GetFileName(filePath));
        }


        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        try
        {
            Console.WriteLine(url);
           
            var response = await client.PostAsync(url, form);
            Console.WriteLine(response);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("Unauthorized: Token may be expired");
                return "Unauthorized: Token may be expired." +
                    "Please Relogin ang try again";
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (responseContent.Contains("<!DOCTYPE HTML") || responseContent.Contains("HTML"))
                {
                    Console.WriteLine("404 with HTML content:\n" + responseContent);
                    return response;
                }
            }

            string json1 = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<dynamic>(json1);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception while uploading: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                ReasonPhrase = ex.Message
            };
        }
    }

    public async Task<dynamic?> UploadOfferCommunicationFiles(
    string url,
    string offerId,
    string message,
    string token,
    string filePath)
    {

        HttpClientHandler handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        HttpClient client = new HttpClient(handler);


        // ✅ Auto-fetch token from session if not provided
        if (string.IsNullOrEmpty(token))
        {
            var context = _httpContextAccessor.HttpContext;
            token = context?.Session.GetString("JWTToken") ?? "";
        }
        Console.WriteLine($"Requested URL: {url}");
        Console.WriteLine($"Token ::::::: {token}");
        // Clear previous headers
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        client.DefaultRequestHeaders.Add("ApiKey", "UZ5QdWg2dLm1sop09082AccIa03DxupsIWDt9rqfeM");
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");

        var tempCaseNO = AesEncryptionHelper.EncryptSingleField(offerId);
        var tempMessage = AesEncryptionHelper.EncryptSingleField(message);

        using var form = new MultipartFormDataContent();
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(offerId)), "OfferId");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(message)), "Reply");


        // 📎 File Upload (like Postman)
        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
        {
            var fileBytes = File.ReadAllBytes(filePath);
            var fileContent = new ByteArrayContent(fileBytes);

            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            // "File" should match the key name in Postman
            form.Add(fileContent, "File", Path.GetFileName(filePath));
        }


        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        try
        {
            Console.WriteLine(url);

            var response = await client.PostAsync(url, form);
            Console.WriteLine(response);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("Unauthorized: Token may be expired");
                return "Unauthorized: Token may be expired." +
                    "Please Relogin ang try again";
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (responseContent.Contains("<!DOCTYPE HTML") || responseContent.Contains("HTML"))
                {
                    Console.WriteLine("404 with HTML content:\n" + responseContent);
                    return response;
                }
            }

            string json1 = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<dynamic>(json1);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception while uploading: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                ReasonPhrase = ex.Message
            };
        }
    }



    public async Task<dynamic?> UploadMakePaymentCheque(
   string PaymentModeId, string PaymentSettingId, string ChequeNo, string Address, string url, string token, string filePath)
    {

        HttpClientHandler handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        HttpClient client = new HttpClient(handler);


        // ✅ Auto-fetch token from session if not provided
        if (string.IsNullOrEmpty(token))
        {
            var context = _httpContextAccessor.HttpContext;
            token = context?.Session.GetString("JWTToken") ?? "";
        }
        Console.WriteLine($"Requested URL: {url}");
        Console.WriteLine($"Token ::::::: {token}");
        // Clear previous headers
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        client.DefaultRequestHeaders.Add("ApiKey", "UZ5QdWg2dLm1sop09082AccIa03DxupsIWDt9rqfeM");
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");

        using var form = new MultipartFormDataContent();
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(PaymentModeId)), "PaymentModeId");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(PaymentSettingId)), "PaymentSettingId");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(ChequeNo)), "ChequeNo");
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(Address)), "Address");


        // 📎 File Upload (like Postman)
        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
        {
            var fileBytes = File.ReadAllBytes(filePath);
            var fileContent = new ByteArrayContent(fileBytes);

            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            // "File" should match the key name in Postman
            form.Add(fileContent, "File", Path.GetFileName(filePath));
        }



        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        try
        {
            Console.WriteLine(url);

            var response = await client.PostAsync(url, form);
            Console.WriteLine(response);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("Unauthorized: Token may be expired");
                return response;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (responseContent.Contains("<!DOCTYPE HTML") || responseContent.Contains("HTML"))
                {
                    Console.WriteLine("404 with HTML content:\n" + responseContent);
                    return response;
                }
            }

            string json1 = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<dynamic>(json1);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception while uploading: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                ReasonPhrase = ex.Message
            };
        }
    }

    // caseID, filePath
    public async Task<dynamic?> UploadFileAsync(
       
        string url,
        string token,
        string caseID,
        string filePath
        )
    {


        HttpClientHandler handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        HttpClient client = new HttpClient(handler);


        // ✅ Auto-fetch token from session if not provided
        if (string.IsNullOrEmpty(token))
        {
            var context = _httpContextAccessor.HttpContext;
            token = context?.Session.GetString("JWTToken") ?? "";
        }
        Console.WriteLine($"Requested URL: {url}");
        Console.WriteLine($"Token ::::::: {token}");
        // Clear previous headers
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        _httpClient.DefaultRequestHeaders.Add("ApiKey", "UZ5QdWg2dLm1sop09082AccIa03DxupsIWDt9rqfeM");

        var tempCaseNO = AesEncryptionHelper.EncryptSingleField(caseID);

        using var form = new MultipartFormDataContent();
        form.Add(new StringContent(AesEncryptionHelper.EncryptSingleField(caseID)), "CaseNo");


        // 📎 File Upload (like Postman)
        if (!string.IsNullOrEmpty(filePath))
        {
            // Convert "/Uploads/Signatures/file.png" → physical path
            string physicalPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                filePath.TrimStart('/')
            );

            if (System.IO.File.Exists(physicalPath))
            {
                var fileBytes = System.IO.File.ReadAllBytes(physicalPath);

                var fileContent = new ByteArrayContent(fileBytes);
                fileContent.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                form.Add(fileContent, "File", Path.GetFileName(physicalPath));
            }
        }



        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        try
        {
            Console.WriteLine(url);

            var response = await client.PostAsync(url, form);
            Console.WriteLine(response);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("Unauthorized: Token may be expired");
                return response;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (responseContent.Contains("<!DOCTYPE HTML") || responseContent.Contains("HTML"))
                {
                    Console.WriteLine("404 with HTML content:\n" + responseContent);
                    return response;
                }
            }

            string json1 = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<dynamic>(json1);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception while uploading: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                ReasonPhrase = ex.Message
            };
        }
    }
}
