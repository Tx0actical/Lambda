using RestSharp;
using System;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace Program {

    public class APIOperationsHandler {

        private RestRequest __request;
        private RestClient __file_upload_client;
        private readonly string __privateapikey = "a5b0377c53bb369b2dbaf846511a76a80d30b5c8d004d94950124dca4ed0acd1";

        TResponse __tresponse__ = new();

        public RestRequest Request { get { return __request; } set { __request = value; } }
        public RestResponse Response { get { return __tresponse__.__response; } set { __tresponse__.__response = value; } }
        public RestClient Client { get { return __file_upload_client; } set { __file_upload_client = value; } }
        public string PrivateAPIKey { get { return __privateapikey; } } // read-only

        public void InstantiateRESTClient () {
            __file_upload_client = new RestClient ("https://www.virustotal.com/api/v3/files");
        }

        public async Task VTAPI_Upload_File (RestRequest request) {

            this.__request = new RestRequest ()
                  .AddHeader ("x-apikey", $"{PrivateAPIKey}");

            request = this.__request;
            var response = await __file_upload_client.PostAsync<TResponse> (this.__request);
        }

        public async Task VTAPI_Upload_URL () {

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
            Console.WriteLine (body);

        }
    }

}