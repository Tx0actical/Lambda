using RestSharp;
using System;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http;
using System.Threading;
using Windows.Media.Protection.PlayReady;

namespace Program {

    public class APIOperationsHandler {

        private RestRequest __request;
        private RestResponse __response;
        private RestClient __file_upload_client;
        private readonly string __privateapikey = Environment.GetEnvironmentVariable("LAMBDA_ACCOUNT_API_KEY");

        public RestRequest Request { get { return __request; } set { __request = value; } }
        public RestResponse Response { get { return __response; } set { __response = value; } }
        public RestClient Client { get { return __file_upload_client; } set { __file_upload_client = value; } }
        public string PrivateAPIKey { get { return __privateapikey; } } // read-only

        public void InstantiateRESTClient () {
            __file_upload_client = new RestClient ("https://www.virustotal.com/api/v3/files");
        }

        // Util Functions
        public async Task<RestResponse> ExecuteAsync (RestRequest request, CancellationToken cancellationToken = default) {
            var response = await __file_upload_client.ExecuteAsync(request, cancellationToken);
            return response;
        }

        // END: Util Functions

        public async Task<RestResponse> VTAPI_Upload_File (RestRequest request) {
            this.__request = new RestRequest ()
                .AddHeader ("content-type", "multipart/form-data; boundary=---011000010111000001101001")
                .AddHeader("Accept", "application/json")
                .AddHeader ("x-apikey", $"{__privateapikey}")
                .AddParameter ( "multipart/form-data; boundary=---011000010111000001101001", 
                                "-----011000010111000001101001\r\nContent-Disposition: form-data; name=\"file\"\r\n\r\ndata:application/octet-stream;name=Lay%20of%20the%20Land.md;base64,IyMjIEludGVybmFsIE5ldHdvcmtzCgojIyMgRGVtaWxpdGFyaXplZCBab25lIChETVopCgo=\r\n-----011000010111000001101001--\r\n\r\n", 
                                ParameterType.RequestBody);
            dynamic response = await ExecuteAsync (request);
            return response;
        }

        public async Task<dynamic> VTAPI_Upload_URL () {
            var client = new HttpClient();
            var request = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = new Uri ("https://www.virustotal.com/api/v3/urls"),
                Headers = {
                    { "accept", "application/json" },
                },
            };

            using var response = await client.SendAsync (request);
            response.EnsureSuccessStatusCode ();
            var body = await response.Content.ReadAsStringAsync ();
            return body;
        }
    }

}