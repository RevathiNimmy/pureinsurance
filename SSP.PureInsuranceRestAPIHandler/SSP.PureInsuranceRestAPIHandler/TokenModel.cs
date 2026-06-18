using Newtonsoft.Json;

namespace SSP.PureInsuranceRestAPIHandler
{
    public class TokenModel
    {
        [JsonProperty("access_token")]
        public string AccessToken
        {
            get;
            set;
        }
        [JsonProperty("refresh_token")]
        public string RefreshToken
        {
            get;
            set;
        }
        [JsonProperty("token_type")]
        public string TokenType
        {
            get;
            set;
        }
        [JsonProperty("expires_in")]
        public long ExpiresIn
        {
            get;
            set;
        }
        public string TokenUrl
        {
            get;
            set;
        }
        public string ApiBaseUrl
        {
            get;
            set;
        }
        public System.DateTime AccessTokenExpiry
        {
            get;
            set;
        }
        [JsonProperty("client_id")]
        public string ClientId
        {
            get;
            set;
        }
        [JsonProperty("client_secret")]
        public string ClientSecret
        {
            get;
            set;
        }
    }
}
