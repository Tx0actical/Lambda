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

        private readonly HttpClient _httpClient;
        private readonly string _privateApiKey;
        private const string BaseAPIUrl = "https://www.virustotal.com/api/v3";

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

        public async Task<ApiResponse> UploadFileAsync (string filePath, CancellationToken cancellationToken = default) {

            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseAPIUrl}/files");
            request.Headers.Add ("x-apikey", _privateApiKey);
            using var content = new MultipartFormDataContent();
            using var fileStream = File.OpenRead(filePath);
            var fileContent = new StreamContent(fileStream);

            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse ("application/octet-stream");
            content.Add (fileContent, "file", Path.GetFileName (filePath));
            request.Content = content;

            var response = await ExecuteAsync(request, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Debug
            System.Diagnostics.Debug.WriteLine ("Raw response content: " + responseContent);

            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseContent);
            
            System.Diagnostics.Debug.WriteLine ("Deserialized ApiResponse in UploadFileAsync: " + JsonSerializer.Serialize (apiResponse));
            apiResponse.IsSuccessStatusCode = response.IsSuccessStatusCode;
            apiResponse.StatusCode = (int) response.StatusCode;

            return apiResponse;
        }

        // Not yet converted to APIResponse
        public async Task<HttpResponseMessage> ScanUrlAsync (string urlToScan, CancellationToken cancellationToken = default) {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://www.virustotal.com/api/v3/urls");
            request.Headers.Add ("x-apikey", _privateApiKey);

            var formData = new MultipartFormDataContent {
                { new StringContent (urlToScan), "url" }
            };
            request.Content = formData;

            return await ExecuteAsync (request, cancellationToken);
        }

        public async Task<ScanResultsApiResponse> GetScanResultsAsync (string analysisId, CancellationToken cancellationToken = default) {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseAPIUrl}/analyses/{analysisId}");
            request.Headers.Add ("x-apikey", _privateApiKey);
            request.Headers.Add ("accept", "application/json");

            var response = await ExecuteAsync(request, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync();
            // Debug
            System.Diagnostics.Debug.WriteLine ("Raw response content in GetScanResultsAsync: " + responseContent);

            var apiResponse = JsonSerializer.Deserialize<ScanResultsApiResponse>(responseContent);
            System.Diagnostics.Debug.WriteLine ("Deserialized ApiResponse in GetScanResultsAsync: " + JsonSerializer.Serialize (apiResponse));
            apiResponse.IsSuccessStatusCode = response.IsSuccessStatusCode;
            apiResponse.StatusCode = (int) response.StatusCode;
            return apiResponse;
        }

        //// A new method to poll for the scan results until the status is no longer "queued"
        //public async Task<ScanResultsApiResponse> WaitForScanCompletionAsync (string analysisId, int pollingIntervalInSeconds = 10) {
        //    ScanResultsApiResponse scanResults;

        //    do {
        //        // Wait for the specified polling interval
        //        await Task.Delay (TimeSpan.FromSeconds (pollingIntervalInSeconds));

        //        // Fetch the scan results
        //        scanResults = await GetScanResultsAsync (analysisId);

        //    } while (scanResults.Data.Attributes.Status == "queued");

        //    return scanResults;
        //}

    }

    public class ApiResponse {
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

    public class ScanResultsApiResponse {
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
