using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Program {
    public class APIOperationsHandler {
        private readonly HttpClient _httpClient;
        private readonly string _privateApiKey;

        public APIOperationsHandler (HttpClient httpClient, string privateApiKey) {
            _httpClient = httpClient;
            _privateApiKey = privateApiKey;
        }

        // Util Functions
        public async Task<HttpResponseMessage> ExecuteAsync (HttpRequestMessage request, CancellationToken cancellationToken = default) {
            var response = await _httpClient.SendAsync(request, cancellationToken);
            return response;
        }
        // END: Util Functions

        public async Task<HttpResponseMessage> UploadFileAsync (string filePath, CancellationToken cancellationToken = default) {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://www.virustotal.com/api/v3/files");
            request.Headers.Add ("x-apikey", _privateApiKey);

            using var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue ("application/octet-stream");
            request.Content = fileContent;

            return await ExecuteAsync (request, cancellationToken);
        }

        public async Task<HttpResponseMessage> ScanUrlAsync (string urlToScan, CancellationToken cancellationToken = default) {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://www.virustotal.com/api/v3/urls");
            request.Headers.Add ("x-apikey", _privateApiKey);

            var formData = new MultipartFormDataContent();
            formData.Add (new StringContent (urlToScan), "url");
            request.Content = formData;

            return await ExecuteAsync (request, cancellationToken);
        }

        public async Task<HttpResponseMessage> GetScanResultsAsync (string scanId, CancellationToken cancellationToken = default) {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://www.virustotal.com/api/v3/analyses/{scanId}");
            request.Headers.Add ("x-apikey", _privateApiKey);

            return await ExecuteAsync (request, cancellationToken);
        }



    }
}
