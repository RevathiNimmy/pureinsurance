using Newtonsoft.Json;
using SSP.Pure.UsersSync.Contracts;

namespace SSP.Pure.UsersSync.Models;

public class KeycloakToken : ErrorMessage
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; } = string.Empty;
    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; } = string.Empty;
}

