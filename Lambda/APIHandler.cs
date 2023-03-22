using RestSharp;
using System;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net;

namespace Program {

   public class APIOperationsHandler {

       RestRequest __request;
       RestResponse __response;
       RestClient __client;

       // Create constructor
       public APIOperationsHandler (
               RestRequest request,
               RestResponse response,
               RestClient client
           ) {
           __request = request;
           __response = response;
           __client = client;
       }

       await SendRequestToVirusTotalAPI (RestClient);

       public class FileObjectAttributes {
           public int creation_date { get; set; }
           public bool downloadable { get; set; }
           public int first_submission_date { get; set; }
       }

       public class Submission {
           public int date { get; set; }
           public string _interface { get; set; }
           public string country { get; set; }
           public string city { get; set; }
           public string name { get; set; }
       }

        

       public async Task VT_API_UploadFile (RestClient client) {

           client = new RestClient ("https://www.virustotal.com/api/v3/files");
           __request = new RestRequest();

           __request.AddHeader ("accept", "application/json");
           __request.AddHeader ("x-apikey", "a5b0377c53bb369b2dbaf846511a76a80d30b5c8d004d94950124dca4ed0acd1");
           __request.AddHeader ("content-type", "multipart/form-data");

           RestResponse response = client.Execute(__request);

       }

        
   }

}
