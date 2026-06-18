using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
namespace SSP.PureInsuranceRestAPIHandler
{

    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }

    public static class GenerateToken
    {
        public static TokenModel GetJwtToken()
        {
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "client_id", "SSP-Red" },
                { "client_secret", "n3C6OlDcv9vRDkQnVzzqjpaS3FdFRk72" },
                { "username", "sirius" },
                { "password", "Password1" },
                { "scope", "openid" } // Optional, based on your server's configuration
            };

                var content = new FormUrlEncodedContent(form);
                var tokenUrl = "http://ps-altova-ls01/realms/SSPStandard/protocol/openid-connect/token";
                var response = Task.Run(async () => await client.PostAsync(tokenUrl, content));// await client.PostAsync(tokenUrl, content);

                if (response.Result.IsSuccessStatusCode)
                {
                    var json = Task.Run(async () => await response.Result.Content.ReadAsStringAsync());
                    var token = JsonConvert.DeserializeObject<TokenModel>(json.Result.ToString());
                    return token;
                }
                else
                {
                    var error = Task.Run(async () => await response.Result.Content.ReadAsStringAsync());
                    throw new Exception($"Token request failed: {response.Result.StatusCode}\n{error}");
                }
            }
        }
        public static TokenModel GetJwtTokenForBatchProcess(string ClientId, string TokenUrl)
        {
            string clientSecret = "";

            clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET",EnvironmentVariableTarget.Machine);
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", ClientId },
                { "client_secret", clientSecret},
                { "scope", "openid" } // Optional, based on your server's configuration
            };

                var content = new FormUrlEncodedContent(form);
                var tokenUrl = TokenUrl;
                var response = Task.Run(async () => await client.PostAsync(tokenUrl, content));// await client.PostAsync(tokenUrl, content);

                if (response.Result.IsSuccessStatusCode)
                {
                    var json = Task.Run(async () => await response.Result.Content.ReadAsStringAsync());
                    var token = JsonConvert.DeserializeObject<TokenModel>(json.Result.ToString());
                    return token;
                }
                else
                {
                    var error = Task.Run(async () => await response.Result.Content.ReadAsStringAsync());
                    throw new Exception($"Token request failed: {response.Result.StatusCode}\n{error}");
                }
            }
        }

    }

}
