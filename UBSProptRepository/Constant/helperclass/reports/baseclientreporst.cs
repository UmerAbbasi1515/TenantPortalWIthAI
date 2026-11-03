using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text;

namespace PmsRepositories.Repositories.HttpClientHelper
{
    public class HttpClientService
    {
        private readonly HttpClient _httpClient;
        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<T> GetAsync<T>(string requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);

            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseData);
        }

        //public async Task<dynamic> PostAsync<T>(string requestUri, object content)
        //{
        //    try
        //    {

        //        Console.WriteLine($"Response in the updated basecliesnt  requestUri:::::: ******** ====== > {requestUri}");

        //        var jsonContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");

        //        Console.WriteLine($"Response in the updated basecliesnt  jsonContent:::::: ******** ====== > {jsonContent}");
        //        var response = await _httpClient.PostAsync(requestUri, jsonContent);
        //        response.EnsureSuccessStatusCode();

        //        var responseData = await response.Content.ReadAsStringAsync();
        //        Console.WriteLine($"Response in the updated basecliesnt :::::: ******** ====== > {responseData}");
        //        Console.WriteLine($"Response in the updated basecliesnt ::::::11 ******** ====== > {JsonSerializer.Deserialize<T>(responseData)}");
        //        return JsonSerializer.Deserialize<T>(responseData);
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        Console.WriteLine($"Request failed: {ex.Message}");
        //        if (ex.InnerException != null)
        //            Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
        //        return null;
        //    }
        //}

        public async Task<T> PostAsync<T>(string requestUri, object content)
        {

            Console.WriteLine($"Response in the updated basecliesnt  requestUri:::::: ******** ====== > {requestUri}");

            var jsonContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");

            var contentString = await jsonContent.ReadAsStringAsync();
            Console.WriteLine($"Response in the updated basecliesnt  jsonContent:::::: ******** ====== > {contentString}");
            var response = await _httpClient.PostAsync(requestUri, jsonContent);
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response in the updated basecliesnt :::::: ******** ====== > {responseData}");
            Console.WriteLine($"Response in the updated basecliesnt ::::::11 ******** ====== > {JsonSerializer.Deserialize<T>(responseData)}");
            return JsonSerializer.Deserialize<T>(responseData);

        }

        public async Task<T> PutAsync<T>(string requestUri, object content)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(requestUri, jsonContent);

            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseData);
        }



        public async Task DownloadFileAsync(string fileName, string savePath, string requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();

                await File.WriteAllBytesAsync(savePath, fileBytes);
            }
            else
            {
                Console.WriteLine($"Failed to download file: {response.StatusCode}");
            }
        }


        public async Task DeleteAsync(string requestUri)
        {
            var response = await _httpClient.DeleteAsync(requestUri);
            response.EnsureSuccessStatusCode();
        }
    }
}
