using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Program {

    public class APIOperationsHandler {

        private readonly HttpClient __HTTPClient;
        private readonly string __privateAPIKey;
        private const string __baseAPIUrl = "https://www.virustotal.com/api/v3";

        public APIOperationsHandler (HttpClient httpClient, string privateAPIKey) {
            __HTTPClient = httpClient;
            __privateAPIKey = privateAPIKey;
        }

        // Util Functions
        public async Task<HttpResponseMessage> ExecuteAsync (HttpRequestMessage request, CancellationToken cancellationToken = default) {
            var response = await __HTTPClient.SendAsync(request, cancellationToken);
            return response;
        }
        // END: Util Functions

        public async Task<APIResponse> UploadFileAsync (string filePath, CancellationToken cancellationToken = default) {

            var request = new HttpRequestMessage(HttpMethod.Post, $"{__baseAPIUrl}/files");
            request.Headers.Add ("x-apikey", __privateAPIKey);
            using var content = new MultipartFormDataContent();
            
            using var fileStream = File.OpenRead(filePath);
            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse ("application/octet-stream");
            
            content.Add (fileContent, "file", Path.GetFileName (filePath));
            request.Content = content;
            var response = await ExecuteAsync(request, cancellationToken);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine ("Raw response content: " + responseContent); // Debug
            var apiResponse = JsonSerializer.Deserialize<APIResponse>(responseContent);
            
            System.Diagnostics.Debug.WriteLine ("Deserialized ApiResponse in UploadFileAsync: " + JsonSerializer.Serialize (apiResponse));
            apiResponse.IsSuccessStatusCode = response.IsSuccessStatusCode;
            apiResponse.StatusCode = (int) response.StatusCode;

            return apiResponse;
        }

        // Not yet converted to APIResponse
        public async Task<HttpResponseMessage> ScanUrlAsync (string urlToScan, CancellationToken cancellationToken = default) {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://www.virustotal.com/api/v3/urls");
            request.Headers.Add ("x-apikey", __privateAPIKey);

            var formData = new MultipartFormDataContent {
                { new StringContent (urlToScan), "url" }
            };
            request.Content = formData;

            return await ExecuteAsync (request, cancellationToken);
        }

        public async Task<ScanResultsAPIResponse> GetScanResultsAsync (string analysisId, CancellationToken cancellationToken = default) {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{__baseAPIUrl}/analyses/{analysisId}");
            request.Headers.Add ("x-apikey", __privateAPIKey);
            request.Headers.Add ("accept", "application/json");

            var response = await ExecuteAsync(request, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            System.Diagnostics.Debug.WriteLine ("Raw response content in GetScanResultsAsync: " + responseContent); // Debug

            var apiResponse = JsonSerializer.Deserialize<ScanResultsAPIResponse>(responseContent);
            System.Diagnostics.Debug.WriteLine ("Deserialized ApiResponse in GetScanResultsAsync: " + JsonSerializer.Serialize (apiResponse));
            apiResponse.IsSuccessStatusCode = response.IsSuccessStatusCode;
            
            apiResponse.StatusCode = (int) response.StatusCode;
            return apiResponse;
        }

        // A new method to poll for the scan results until the status is no longer "queued"
        // pollingIntervalInSeconds is set to 15 to meet request/min. criteria for the analysis endpoint.
        public async Task<ScanResultsAPIResponse> WaitForScanCompletionAsync (string analysisId, int pollingIntervalInSeconds = 15) {
            ScanResultsAPIResponse scanResults;

            do {
                // Wait for the specified polling interval
                await Task.Delay (TimeSpan.FromSeconds (pollingIntervalInSeconds));
                // Fetch the scan results
                scanResults = await GetScanResultsAsync (analysisId);
            } while (scanResults.Data.Attributes.Status == "queued");

            return scanResults;
        }

    }

    public class APIResponse {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public int  StatusCode { get; set; }
    }

    public class Data {
        [JsonPropertyName ("id")]
        public string Id { get; set; }

        [JsonPropertyName ("type")]
        public string Type { get; set; }

        [JsonPropertyName ("links")]
        public Links Links { get; set; }
    }

    public class Links {
        [JsonPropertyName ("self")]
        public string Self { get; set; }
    }

    public class ScanResultsAPIResponse {
        [JsonPropertyName ("data")]
        public ScanResultsData Data { get; set; }

        public bool IsSuccessStatusCode { get; set; }
        public int StatusCode { get; set; }
    }

    public class ScanResultsData {
        [JsonPropertyName ("id")]
        public string Id { get; set; }

        [JsonPropertyName ("type")]
        public string Type { get; set; }

        // Add other properties specific to file scan results
        [JsonPropertyName ("attributes")]
        public AnalysisAttributes Attributes { get; set; }
    }

    public class AnalysisAttributes {
        [JsonPropertyName ("date")]
        public long Date { get; set; }

        [JsonPropertyName ("status")]
        public string Status { get; set; }

        [JsonPropertyName ("stats")]
        public Stats Stats { get; set; }

        [JsonPropertyName ("results")]
        public Dictionary<string, ScanResult> Results { get; set; }

    }

    public class Stats {
        [JsonPropertyName ("harmless")]
        public int Harmless { get; set; }

        [JsonPropertyName ("type-unsupported")]
        public int TypeUnsupported { get; set; }

        [JsonPropertyName ("suspicious")]
        public int Suspicious { get; set; }

        [JsonPropertyName ("confirmed-timeout")]
        public int ConfirmedTimeout { get; set; }

        [JsonPropertyName ("timeout")]
        public int Timeout { get; set; }

        [JsonPropertyName ("failure")]
        public int Failure { get; set; }

        [JsonPropertyName ("malicious")]
        public int Malicious { get; set; }

        [JsonPropertyName ("undetected")]
        public int Undetected { get; set; }
    }

    public class ScanResult {
        [JsonPropertyName ("category")]
        public string Category { get; set; }

        [JsonPropertyName ("engine_name")]
        public string EngineName { get; set; }

        [JsonPropertyName ("engine_version")]
        public string EngineVersion { get; set; }

        [JsonPropertyName ("result")]
        public string Result { get; set; }

        [JsonPropertyName ("method")]
        public string Method { get; set; }

        [JsonPropertyName ("engine_update")]
        public string EngineUpdate { get; set; }
    }
}
