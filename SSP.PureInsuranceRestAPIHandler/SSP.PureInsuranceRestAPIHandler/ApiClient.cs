using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SSP.PureInsuranceRestAPIHandler
{
    public static class ApiClient
    {
        private static readonly string TokenCacheKey = "access_token";
        private static string _tokenUrl;
        private static string _apiBaseUrl;

        private static readonly HttpClient HttpClient = new HttpClient();
        private static string _accessToken;
        private static string _refreshToken;
        private static DateTime _accessTokenExpiry;
        private static readonly object _tokenLock = new object();
        public static TokenModel _tokenModel { get; set; }

        // <summary>
        /// Optional callback invoked after a successful token refresh.
        /// Register this once in ProviderSAMForInsuranceV2 to sync the new token back to Session
        /// without needing to call SyncTokenToSession() at every individual API call site.
        /// </summary>
        public static Action<TokenModel> OnTokenRefreshed { get; set; }

        public static T DeserializeJson<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default(T); // return null for reference types, default value for value types
            var settings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Include,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All
            };

            try
            {
                return JsonConvert.DeserializeObject<T>(json, settings);
            }
            catch (JsonException ex)
            {
                
                return default(T);
            }
        }
        public static string BuildQueryString(object obj)
        {
            var properties = obj.GetType().GetProperties();
            var query = HttpUtility.ParseQueryString(string.Empty);

            foreach (PropertyInfo prop in properties)
            {
                var value = prop.GetValue(obj);
                if (value != null)
                {
                    if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                    {
                        var dateValue = (DateTime)value;
                        query[prop.Name] = dateValue.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        query[prop.Name] = value.ToString();
                    }
                }
               
            }

            return query.ToString(); // Outputs: Name=John&Page=2&IsActive=True
        }
        //public static ApiClient(TokenModel tokenModel)
        //{
        //    _accessToken = _tokenModel.AccessToken ;
        //    _refreshToken = _tokenModel.RefreshToken ;
        //    _accessTokenExpiry = _tokenModel.AccessTokenExpiry;
        //    _tokenUrl = _tokenModel.TokenUrl ;
        //    _apiBaseUrl = _tokenModel.ApiBaseUrl ;
        //}
        private static void RefreshAccessTokenAsync()
        {
            var formParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", _refreshToken),
                new KeyValuePair<string, string>("client_id", _tokenModel.ClientId),
                new KeyValuePair<string, string>("client_secret", _tokenModel.ClientSecret)
            };
            
            using (var content = new FormUrlEncodedContent(formParams))
            {
                var response = HttpClient.PostAsync(_tokenUrl, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(json);

                    TokenModel snapshot;
                    lock (_tokenLock)
                    {
                        _accessToken = tokenResponse.access_token;
                        _refreshToken = tokenResponse.refresh_token;
                        _accessTokenExpiry = DateTime.Now.AddSeconds(tokenResponse.expires_in);
                        snapshot = _tokenModel;
                        if (snapshot != null)
                        {
                            snapshot.AccessToken = _accessToken;
                            snapshot.RefreshToken = _refreshToken;
                            snapshot.AccessTokenExpiry = _accessTokenExpiry;
                        }
                    }
                    // Invoke callback outside the lock using the stable snapshot
                    OnTokenRefreshed?.Invoke(snapshot);
                }
            }
        }
        /// <summary>
        /// Execute HTTP calls (GET/POST/PUT/DELETE)
        /// </summary>
        public static object CallApi(string endpoint, HttpMethod method, object body = null)
        {
            _accessToken = _tokenModel.AccessToken;
            _refreshToken = _tokenModel.RefreshToken;
            _accessTokenExpiry = _tokenModel.AccessTokenExpiry;
            _tokenUrl = _tokenModel.TokenUrl;
            _apiBaseUrl = _tokenModel.ApiBaseUrl;

            if (string.IsNullOrEmpty(_apiBaseUrl))
                throw new InvalidOperationException("Session has expired. API base URL is unavailable.");

            // Only required when a refresh is actually needed
            if ((string.IsNullOrEmpty(_accessToken) || DateTime.Now > _accessTokenExpiry) && string.IsNullOrEmpty(_tokenUrl))
                throw new InvalidOperationException("Session has expired. Token URL is unavailable.");

            //  string token = GetToken();
            if (string.IsNullOrEmpty(_accessToken) || DateTime.Now > _accessTokenExpiry)
            {
                RefreshAccessTokenAsync();
            }
            //request = new HttpRequestMessage();
            //request.Headers.Add("preferred_username", userName);// credentials.Username);
            //request.RequestUri = new Uri(client.BaseAddress + ApiUrl);
            //request.Method = HttpMethod.Get;
            //  private HttpRequestMessage request;
            HttpRequestMessage request = new HttpRequestMessage();
            using (request)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
               
                request.Method = method;
                
                if (body != null)
                {
                    if (request.Method == HttpMethod.Get)
                    {
                        endpoint = endpoint + "?" + BuildQueryString(body);
                    }
                    else
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        string json = JsonConvert.SerializeObject(body, settings);
                        request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    }
                }

                request.RequestUri = new Uri(_apiBaseUrl + endpoint);
                // Force TLS 1.2
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // Bypass cert validation (only in dev!)
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                var response = Task.Run(() => HttpClient.SendAsync(request)).Result;

                // Handle expired token by retrying
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Token may be expired - refresh it
                    RefreshAccessTokenAsync();
                    
                    // Create a new request for retry
                    using (var retryRequest = new HttpRequestMessage())
                    {
                        retryRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                        retryRequest.Method = method;
                        retryRequest.RequestUri = new Uri(_apiBaseUrl + endpoint);
                        
                        if (body != null && method != HttpMethod.Get)
                        {
                            var settings = new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                MissingMemberHandling = MissingMemberHandling.Ignore
                            };
                            string json = JsonConvert.SerializeObject(body, settings);
                            retryRequest.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                        }
                        
                        response = Task.Run(() => HttpClient.SendAsync(retryRequest)).Result;
                    }
                }
               
                response.EnsureSuccessStatusCode();
                return Task.Run(() => response.Content.ReadAsStringAsync()).Result;
            }
        }

        // Shortcut methods
        public static object Get(string endpoint,object request) => CallApi(endpoint, HttpMethod.Get,request);
        public static object Post(string endpoint, object body) => CallApi(endpoint, HttpMethod.Post, body);
        public static object Put(string endpoint, object body) => CallApi(endpoint, HttpMethod.Put, body);
        public static object Delete(string endpoint, object body) => CallApi(endpoint, HttpMethod.Delete, body);
        //public static TokenModel GetApiTokendetails()
        //{
        //    var apiTokenDetails = new TokenModel();
        //    apiTokenDetails = GenerateToken.GetJwtToken();
        //    apiTokenDetails.ApiBaseUrl = "https://localhost:7246/api"; // ConfigurationManager.AppSettings("RestAPIUrl")
        //                                                           // .AccessToken = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJCVGNINU5NY3JqWHFPS2p5RTI1Z1lNeXd3WnpyWTJJZGV1TU94YXhqMldvIn0.eyJleHAiOjE3NTM4NjM0OTAsImlhdCI6MTc1Mzg2MTY5MCwianRpIjoiYWE2NTMyZWEtMWE5ZS00YmQ5LTg2Y2YtZTA4NmM4OTY3ODc3IiwiaXNzIjoiaHR0cDovL3BzLWFsdG92YS1sczAxL3JlYWxtcy9TU1BTdGFuZGFyZCIsImF1ZCI6WyJyZWFsbS1tYW5hZ2VtZW50IiwiYWNjb3VudCJdLCJzdWIiOiIzNTA3NjBkOS1kMGUxLTQ5ZmQtOWE2Ny0yMzY0Yjc1Y2NkN2UiLCJ0eXAiOiJCZWFyZXIiLCJhenAiOiJTU1AtUmVkIiwic2lkIjoiYmNmMWIyODAtNThlMS00N2QxLWJjYzctNGEwNzI0MmI1OWM4IiwiYWNyIjoiMSIsInJlc291cmNlX2FjY2VzcyI6eyJyZWFsbS1tYW5hZ2VtZW50Ijp7InJvbGVzIjpbIm1hbmFnZS11c2VycyIsInF1ZXJ5LXJlYWxtcyIsInZpZXctY2xpZW50cyIsInZpZXctYXV0aG9yaXphdGlvbiIsInF1ZXJ5LWNsaWVudHMiLCJxdWVyeS1ncm91cHMiLCJxdWVyeS11c2VycyJdfSwiYWNjb3VudCI6eyJyb2xlcyI6WyJtYW5hZ2UtYWNjb3VudCIsIm1hbmFnZS1hY2NvdW50LWxpbmtzIl19fSwic2NvcGUiOiJvcGVuaWQgcHJvZmlsZSByb2xlcyBlbWFpbCIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwibmFtZSI6IlNpcml1cyBzaXJpdXMiLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJzaXJpdXMiLCJnaXZlbl9uYW1lIjoiU2lyaXVzIiwiZmFtaWx5X25hbWUiOiJzaXJpdXMiLCJlbWFpbCI6InNpcml1c0BzaXJpdXMuY29tIn0.RzO6p9fByvPhImRfpug9wubuzRCDn8Pe1YcTybu8jR7_WGEyNTASvdFVqk2dujIo-eano86IqFxIaH3z13vtQid1V-pAnu7SBpKOPO49gshYJJAQCrbtinGyjV-08rh6jLmbVYTR-M9aBB4O8FhLCcQhcQa1MT15KyqChBbqCWtIce-6t7ArsGeyP2FW3MSpqYkNBMqPeKZfTeEkbELMeqf9eWwi5ePBHKR3EhTWPKe1Ilb4hOb3R03GZyFdy-EKW64SPxRb_sltTfHtGguesG7loohKxbXu7otT8IBtI_FjMw7WUDWt5-GtKejQNj_ixVGJXInuYx3MJFm__2-KjA.eyJleHAiOjE3NTM3NDQ3MDIsImlhdCI6MTc1Mzc0MjkwMiwianRpIjoiZDI0YjExYTEtZTJhMy00ZWRkLWIzNDUtOWNiYWI5MjI5MzA2IiwiaXNzIjoiaHR0cDovL3BzLWFsdG92YS1sczAxL3JlYWxtcy9TU1BTdGFuZGFyZCIsImF1ZCI6WyJyZWFsbS1tYW5hZ2VtZW50IiwiYWNjb3VudCJdLCJzdWIiOiIzNTA3NjBkOS1kMGUxLTQ5ZmQtOWE2Ny0yMzY0Yjc1Y2NkN2UiLCJ0eXAiOiJCZWFyZXIiLCJhenAiOiJTU1AtUmVkIiwic2lkIjoiN2UwODcwY2EtMzFmMC00ZWYwLWI5ZDYtNjAzNzc3YjM2YWE0IiwiYWNyIjoiMSIsInJlc291cmNlX2FjY2VzcyI6eyJyZWFsbS1tYW5hZ2VtZW50Ijp7InJvbGVzIjpbIm1hbmFnZS11c2VycyIsInF1ZXJ5LXJlYWxtcyIsInZpZXctY2xpZW50cyIsInZpZXctYXV0aG9yaXphdGlvbiIsInF1ZXJ5LWNsaWVudHMiLCJxdWVyeS1ncm91cHMiLCJxdWVyeS11c2VycyJdfSwiYWNjb3VudCI6eyJyb2xlcyI6WyJtYW5hZ2UtYWNjb3VudCIsIm1hbmFnZS1hY2NvdW50LWxpbmtzIl19fSwic2NvcGUiOiJvcGVuaWQgcHJvZmlsZSByb2xlcyBlbWFpbCIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwibmFtZSI6IlNpcml1cyBzaXJpdXMiLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJzaXJpdXMiLCJnaXZlbl9uYW1lIjoiU2lyaXVzIiwiZmFtaWx5X25hbWUiOiJzaXJpdXMiLCJlbWFpbCI6InNpcml1c0BzaXJpdXMuY29tIn0.ShvDCN9mqjZa7ClysAnPRqY0L5mI6wvXss-E9fQsGRPn4zA011f-qKbWnN68R59fNqdMG0zf1TPCZXSIVmsaZ7YVwPDqPFLCsaW6lZNMRmBSZW8DU3PLevAxkhCEWCVrreC2NbKFODFpoERBqXJNtJav_0pt5JzTbBgsw2qv9esTzn-ivctkGlFwKXF4AYh3puUVRwHHhnhFe7Gag0mP_ZayadmQALSA9aT00MfRU3mMrYTuwcJGnNREKvPmIYaVfOVsDOuVPAYgK0Fimeg08_HGNWRUrWmWeX8An9u_p6lX6Kowm3vzLO-CEkyN0LRjIMjd6-LXX5qF_rHt0ZJDvw"
        //    apiTokenDetails.AccessTokenExpiry = DateTime.UtcNow.AddSeconds(1800d);
        //    // .ExpiresIn = "1800"
        //    // .RefreshToken = "eyJhbGciOiJIUzUxMiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICI2NGE2YjRjZi00MDkzLTQ4ZTItODNhMy04NWJmYjQxOTM2NDUifQ.eyJleHAiOjE3NTM3NDE2MzIsImlhdCI6MTc1MzczOTgzMiwianRpIjoiNWJjODVhMTYtODJkYy00OTE2LWJmNmQtMzljNjRmOTY0MDcwIiwiaXNzIjoiaHR0cDovL3BzLWFsdG92YS1sczAxL3JlYWxtcy9TU1BTdGFuZGFyZCIsImF1ZCI6Imh0dHA6Ly9wcy1hbHRvdmEtbHMwMS9yZWFsbXMvU1NQU3RhbmRhcmQiLCJzdWIiOiIzNTA3NjBkOS1kMGUxLTQ5ZmQtOWE2Ny0yMzY0Yjc1Y2NkN2UiLCJ0eXAiOiJSZWZyZXNoIiwiYXpwIjoiU1NQLVJlZCIsInNpZCI6IjViNDQ1ZDQxLTZiYzMtNGIwZC05OTZiLTgxZWE5ZmFkNjUyYSIsInNjb3BlIjoib3BlbmlkIHByb2ZpbGUgd2ViLW9yaWdpbnMgcm9sZXMgZW1haWwgYmFzaWMgYWNyIn0.UlVCywzoZ82MZTezY926L6zdaHe5HCRTcXL8QoVuDdf0lc-6GZS44Og6fR6naMlVmHeOlPMDYIsP9820t4OT9w"
        //    // .TokenType = "Bearer"
        //    apiTokenDetails.TokenUrl = "http://ps-altova-ls01/realms/SSPStandard/protocol/openid-connect/token";
        //    return apiTokenDetails;
        //}

        private class TokenResponse
        {
            public System.DateTime access_Token_Expiry { get; set; }

            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public string refresh_token { get; set; }
        }
    }

}
