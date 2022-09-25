using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Misy_API.Classes.System
{
    class MISYAPI
    {
        //Secret Token
        private string? token;
        //API Post Method
        ArrayList type = new ArrayList() { "fetch", "update", "insert", "delete", "login", "register" };
        private string questType = "";
        //API Post Server
        private const string url = "https://localhost/api/misy?";
        private const string baseUrl = "https://localhost/";
        private const string selfUrl = "https://localhost/api/misy";
        //API User Id
        private int userId;
        //Auth Status
        private int statusCode;
        private string? statusMessage;

        //Auth
        private string? username, password;

        //Set
        public string SetMisyApiType(string type) => questType = type.ToLower();
        public int SetMisyApiUid(int uid) => userId = Convert.ToInt32(uid);
        public string SetMisyApiToken(string secretToken) => token = secretToken;

        //Get
        public string GetMisyApiToken() => token;
        public string GetMisyApiHostname() => url;
        public string GetMisyApiSelfHostname() => selfUrl;
        public string GetMisyApiBaseHostname() => baseUrl;
        public int GetMisyApiUid() => userId;
        public string GetMisyApiType() => questType;
        public ArrayList GetMisyApiAcceptType() => type;

        //Full
        public string GetMisyQuestParameters() => $"misy?id={GetMisyApiUid()}&token={GetMisyApiToken()}&type={GetMisyApiType()}";
        public string GetMisySelfQuestParameters() => $"id={GetMisyApiUid()}&token={GetMisyApiToken()}&type={GetMisyApiType()}";
        public string GetMisyFullQuest() => $"{GetMisyApiHostname()}id={GetMisyApiUid()}&token={GetMisyApiToken()}&type={GetMisyApiType()}";

        //Auth
        public string MisyApiLoginQuery() => $"misy?username={GetMisyAuthUsername()}&password={GetMisyAuthPassword()}&token={GetMisyApiToken()}&type=login";
        public int GetMisyApiAuthStatusCode() => statusCode;
        public string GetMisyAuthUsername() => username;
        public string GetMisyAuthPassword() => password;
        public string GetMisyAuthMessage() => statusMessage;
        public string MisyApiRegisterQuery() => $"misy?username={GetMisyAuthUsername()}&password={GetMisyAuthPassword()}&token={GetMisyApiToken()}&type=register";

        public void MisyApiLogin(string customUsername, string customPassword)
        {
            username = customUsername;
            password = customPassword;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(GetMisyApiSelfHostname());

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            string parameters = MisyApiLoginQuery();

            // List data response.
            HttpResponseMessage response = client.GetAsync(parameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var dataObjects = response.Content.ReadAsStringAsync().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll

                JObject jObject = JObject.Parse(dataObjects);
                JObject jobj = jObject;

                statusCode = Convert.ToInt16(jobj["statusCode"]);
                statusMessage = (string)jobj["message"]; 
            }
            else
            {
                statusCode = Convert.ToInt16(response.StatusCode);
            }

            //Make any other calls using HttpClient here.
            //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            client.Dispose();
        }

        public void MisyApiRegister(string customUsername, string customPassword)
        {
            username = customUsername;
            password = customPassword;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(GetMisyApiSelfHostname());

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            string parameters = MisyApiRegisterQuery();

            // List data response.
            HttpResponseMessage response = client.GetAsync(parameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var dataObjects = response.Content.ReadAsStringAsync().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll

                JObject jObject = JObject.Parse(dataObjects);
                JObject jobj = jObject;

                statusCode = Convert.ToInt16(jobj["statusCode"]);
                statusMessage = (string)jobj["message"];
            }
            else
            {
                statusCode = Convert.ToInt16(response.StatusCode);
            }

            //Make any other calls using HttpClient here.
            //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            client.Dispose();

        }
    }
}