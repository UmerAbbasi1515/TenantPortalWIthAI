using System.Text.Json.Serialization;

namespace PmsRepositories.ViewModels.UnitReport
{
    public class FileDownloadResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("fileDownloadUrl")]
        public string? FileDownloadUrl { get; set; }
    }
}
