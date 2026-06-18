using KeycloackTest.Contracts;
using KeycloackTest.DTOMappers;
using KeycloackTest.Models;
using KeycloackTest.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SSP.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BPMUsersSync.Services;

public class PureService : IPureService
{
    static HttpClient _httpClient;
    private readonly KeyCloakConfiguration _keycloakConfiguration;
    private string _uriString;
    public PureService()
    {
    }
    public PureService(KeyCloakConfiguration keyCloakConfiguration)
    {
        _keycloakConfiguration = keyCloakConfiguration;
        _uriString = (new Uri(_keycloakConfiguration.TokenEndpoint)).GetLeftPart(UriPartial.Authority).ToString();
        var httpClientHandler = new HttpClientHandler()
        {
            UseDefaultCredentials = true,
            Credentials = CredentialCache.DefaultNetworkCredentials
        };
        Uri uri = new Uri(_keycloakConfiguration.TokenEndpoint);
        _httpClient = new HttpClient()
        {
            BaseAddress = new System.Uri(uri.GetLeftPart(UriPartial.Authority)),
            Timeout = TimeSpan.FromSeconds(5000)
        };
    }

    public async Task<IReadOnlyList<UserResponseDTO>> GetAllUsersAsync()
    {
        var users = await GetUsersAsync();

        return users.ToDTO();
    }
    public async Task<string> GetUserAsync(string userName)
    {
        var newUser = await GetUserByNameAsync(userName);

        return newUser.Id;
    }
    public async Task<AuthResponseDTO> RegisterUserAsync(UserRegisterRequestDTO request)
    {
        try
        {
            var tokenResponse = await GetAdminTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        string json;
        if (!string.IsNullOrEmpty(request.Password))
        {
            var user = new
            {
                username = request.UserName,
                email = request.Email,
                firstName = request.FirstName,
                lastName = request.LastName,
                enabled = request.Deleted == 0 ? true : false,
                credentials = new[]
                        {
            new
            {
                type = "password",
                value = request.Password,
                temporary = false
            }
        }
            };
            json = JsonConvert.SerializeObject(user);
        }
        else
        {
            var user = new
            {
                username = request.UserName,
                email = request.Email,
                firstName = request.FirstName,
                lastName = request.LastName,
                enabled = request.Deleted == 0 ? true : false
            };
            json = JsonConvert.SerializeObject(user);
        }

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var requestUrl = string.Format($"{_uriString}/admin/realms/{_keycloakConfiguration.Realm}/users", _uriString, _keycloakConfiguration.Realm);
        var response = _httpClient.PostAsync(requestUrl, content);
        response.Wait();
        if (response.Status != TaskStatus.RanToCompletion || response.Result.StatusCode != HttpStatusCode.Created)
        {
            throw new HttpRequestException($"Request failed: {response.Result.StatusCode}");
        }

        var newUser = await GetUserByNameAsync(request.UserName);
        if (!string.IsNullOrEmpty(request.AdminGroupName))
        {
            await AddUserToGroupByNameAsync(newUser.Id, request.AdminGroupName);
            var groupId = await GetGroupIdByNameAsync(request.AdminGroupName);
            await AddGroupRoleToUserAsync(newUser.Id, groupId, request.AdminGroupName);
        }
        var userToken = await GetUserTokenAsync(request.UserName, request.Password);

        return new AuthResponseDTO(newUser.ToDTO(), AccessToken: userToken.AccessToken,
            RefreshToken: userToken.RefreshToken);
    }

    public async Task<AuthResponseDTO> UpdateUserAsync(UserRegisterRequestDTO request)
    {
        var tokenResponse = await GetAdminTokenAsync();
        string json;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
        if (!string.IsNullOrEmpty(request.Password))
        {
            var user = new
            {
                email = request.Email,
                firstName = request.FirstName,
                lastName = request.LastName,
                enabled = request.Deleted == 0 ? true : false,
                credentials = new[]
                        {
                            new
                            {
                                type = "password",
                                value = request.Password,
                                temporary = false
                            }
                        },
            };
            json = JsonConvert.SerializeObject(user);
        }
        else
        {
            var user = new
            {
                email = request.Email,
                firstName = request.FirstName,
                lastName = request.LastName,
                enabled = request.Deleted == 0 ? true : false
            };
            json = JsonConvert.SerializeObject(user);
        }

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var requestUrl = string.Format($"{_uriString}/admin/realms/{_keycloakConfiguration.Realm}/users/{request.Id}", _uriString, _keycloakConfiguration.Realm, request.Id);
        var response = _httpClient.PutAsync(requestUrl, content);
        response.Wait();
        if (response.Status != TaskStatus.RanToCompletion)
        {
            bPMFunc.LogMessage(_keycloakConfiguration.LoggedInUser, _keycloakConfiguration.UserKey, $"Failed to update user: {response.Result.StatusCode}");
            return null;
        }
        var newUser = await GetUserByNameAsync(request.UserName);

        if (request.Deleted == 0)
        {
            if (!string.IsNullOrEmpty(request.AdminGroupName))
            {
                await AddUserToGroupByNameAsync(newUser.Id, request.AdminGroupName);
                var groupId = await GetGroupIdByNameAsync(request.AdminGroupName);
                await AddGroupRoleToUserAsync(newUser.Id, groupId, request.AdminGroupName);
            }
            if (!string.IsNullOrEmpty(request.Password))
            {
                var userToken = await GetUserTokenAsync(request.UserName, request.Password);

                return new AuthResponseDTO(newUser.ToDTO(), AccessToken: userToken.AccessToken,
                    RefreshToken: userToken.RefreshToken);
            }
        }
        return null;
    }

    public async Task<AuthResponseDTO> LoginUserAsync(UserLoginRequestDTO request)
    {
        var userToken = await GetUserTokenAsync(request.Email, request.Password);

        var user = await GetUserByEmailAsync(request.Email);

        return new AuthResponseDTO(user.ToDTO(), AccessToken: userToken.AccessToken,
            RefreshToken: userToken.RefreshToken);
    }
    public KeyCloakConfiguration GetKeyCloakConfiguration(string userName, string password, int userKey)
    {
        var keyCloakConfiguration = new KeyCloakConfiguration();
        keyCloakConfiguration.TokenEndpoint = GetOptionValues(5252, userName, password, userKey);
        keyCloakConfiguration.client_id = GetOptionValues(5247, userName, password, userKey);
        keyCloakConfiguration.client_secret = GetOptionValues(5248, userName, password, userKey);
        keyCloakConfiguration.grant_type = "password";
        keyCloakConfiguration.username = GetOptionValues(5249, userName, password, userKey);
        keyCloakConfiguration.Realm = GetOptionValues(5246, userName, password, userKey);
        keyCloakConfiguration.Password = bPMFunc.GetOVal(GetOptionValues(5250, userName, password, userKey));
        keyCloakConfiguration.AdminGroupName = GetOptionValues(5251, userName, password, userKey);
        keyCloakConfiguration.LoggedInUser = userName;
        keyCloakConfiguration.UserKey = userKey;
        if (keyCloakConfiguration.TokenEndpoint == string.Empty ||
        keyCloakConfiguration.client_id == string.Empty ||
        keyCloakConfiguration.client_secret == string.Empty ||
        keyCloakConfiguration.grant_type == string.Empty ||
        keyCloakConfiguration.username == string.Empty ||
        keyCloakConfiguration.Realm == string.Empty ||
        keyCloakConfiguration.Password == string.Empty ||
        keyCloakConfiguration.AdminGroupName == string.Empty)
        {
            return null;
        }

        return keyCloakConfiguration;
    }
    private string GetOptionValues(int optionNumber, string userName, string password, int userKey)
    {
        int sourceKey = 1;
        string optionValue = string.Empty;
        int result = bPMFunc.GetSystemOption(v_sUsername: userName, v_sPassword: password, v_iUserID: userKey, v_iMainSourceID: 1, v_iLanguageID: 1, v_iCurrencyID: 1, v_iLogLevel: 1, v_sCallingAppName: "GetOptionValues", v_iOptionNumber: ref optionNumber, r_sOptionValue: ref optionValue, v_iSourceID: ref sourceKey);
        if (result != (int)gPMConstants.PMEReturnCode.PMTrue)
        {
            return string.Empty;
        }
        return optionValue;
    }
    private async Task<IEnumerable<User>> GetUsersAsync()
    {
        var tokenResponse = await GetAdminTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
        var requestUrl = string.Format($"{_uriString}/admin/realms/{_keycloakConfiguration.Realm}/users", _uriString, _keycloakConfiguration.Realm);
        var response = _httpClient.GetAsync(requestUrl);
        response.Wait();
        if (response.Status != TaskStatus.RanToCompletion)
        {
            bPMFunc.LogMessage(_keycloakConfiguration.LoggedInUser, _keycloakConfiguration.UserKey, $"Failed to retrieve user details: {response.Result.StatusCode}");
            return null;
            throw new HttpRequestException($"Failed to retrieve user details: {response.Result.StatusCode}");
        }

        var jsonResponse = await response.Result.Content.ReadAsStringAsync();

        var users = JsonConvert.DeserializeObject<List<User>>(jsonResponse);

        if (users == null || users.Count == 0)
        {
            bPMFunc.LogMessage(_keycloakConfiguration.LoggedInUser, _keycloakConfiguration.UserKey, $"User Not Found");
            return null;
        }

        return users;
    }

    private async Task<User> GetUserByNameAsync(string username)
    {
        var apiUrl = string.Format($"{_uriString}/admin/realms/{_keycloakConfiguration.Realm}/users/?username={username}", _uriString, _keycloakConfiguration.Realm, _keycloakConfiguration.username);

        var tokenResponse = await GetAdminTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

        var response = _httpClient.GetAsync(apiUrl);
        response.Wait();
        if (response.Status != TaskStatus.RanToCompletion)
        {
            throw new HttpRequestException($"Failed to retrieve user details: {response.Result.StatusCode}");
        }

        var jsonResponse = await response.Result.Content.ReadAsStringAsync();

        // However, it will return a unique user.
        var users = JsonConvert.DeserializeObject<List<User>>(jsonResponse);

        if (users == null || users.Count == 0)
        {
            bPMFunc.LogMessage(_keycloakConfiguration.LoggedInUser, _keycloakConfiguration.UserKey, $"User Not Found");
            return null;
        }

        return users.First();
    }

    private async Task<User> GetUserByEmailAsync(string email)
    {
        var apiUrl = string.Format($"{_uriString}/admin/realms/{_keycloakConfiguration.Realm}/users/?email={email}", _uriString, _keycloakConfiguration.Realm, email);

        var tokenResponse = await GetAdminTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

        var response = _httpClient.GetAsync(apiUrl);
        response.Wait();
        if (response.Status != TaskStatus.RanToCompletion)
        {
            throw new HttpRequestException($"Failed to retrieve user details: {response.Result.StatusCode}");
        }

        var jsonResponse = await response.Result.Content.ReadAsStringAsync();

        // However, it will return a unique user.
        var users = JsonConvert.DeserializeObject<List<User>>(jsonResponse);

        if (users == null || users.Count == 0)
        {
            bPMFunc.LogMessage(_keycloakConfiguration.LoggedInUser, _keycloakConfiguration.UserKey, $"User Not Found");
            return null;
        }

        return users.First();
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<KeycloakToken> GetAdminTokenAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        var formData = new Dictionary<string, string>
        {
            { "client_id", _keycloakConfiguration.client_id},
            { "client_secret", _keycloakConfiguration.client_secret },
            { "grant_type", _keycloakConfiguration.grant_type },
            { "username", _keycloakConfiguration.username },
            { "password",  _keycloakConfiguration.Password }
        };

        var tokenEndpoint = _keycloakConfiguration.TokenEndpoint;
        var content = new FormUrlEncodedContent(formData);
        var response = _httpClient.PostAsync(tokenEndpoint, content);
        response.Wait();
        if (response.Status != TaskStatus.RanToCompletion)
        {
            bPMFunc.LogMessage(_keycloakConfiguration.LoggedInUser, _keycloakConfiguration.UserKey, $"Request failed: {response.Result.StatusCode}");
            return null;
        }
        var data = JObject.Parse(response.Result.Content.ReadAsStringAsync().Result);

        var tokenResponse = JsonConvert.DeserializeObject<KeycloakToken>(data.ToString());

        if (tokenResponse == null)
        {
            bPMFunc.LogMessage(_keycloakConfiguration.LoggedInUser, _keycloakConfiguration.UserKey, $"Request failed: {response.Result.StatusCode}");
            return null;
        }

        return tokenResponse;
    }

    private async Task<KeycloakToken> GetUserTokenAsync(string username, string password)
    {
        var formData = new Dictionary<string, string>
    {
        { "client_id", _keycloakConfiguration.client_id},
        { "client_secret", _keycloakConfiguration.client_secret },
        { "grant_type", "password" },
        { "username", username },
        { "password", password }
    };

        var tokenEndpoint = _keycloakConfiguration.TokenEndpoint;

        var content = new FormUrlEncodedContent(formData);
        var response = _httpClient.PostAsync(tokenEndpoint, content);
        response.Wait();
        if (response.Status != TaskStatus.RanToCompletion)
        {
            throw new HttpRequestException($"Request failed: {response.Result.StatusCode}");
        }

        var jsonResponse = await response.Result.Content.ReadAsStringAsync();
        var tokenResponse = JsonConvert.DeserializeObject<KeycloakToken>(jsonResponse);

        if (tokenResponse == null)
        {
            bPMFunc.LogMessage(_keycloakConfiguration.LoggedInUser, _keycloakConfiguration.UserKey, $"Request failed: {response.Result.StatusCode}");
            return null;
        }
        return tokenResponse;
    }

    private async Task AddUserToGroupAsync(string userId, string groupId)
    {
        var tokenResponse = await GetAdminTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

        var addGroupUrl = string.Format($"{_uriString}/admin/realms/{_keycloakConfiguration.Realm}/users/{userId}/groups/{groupId}", _uriString, _keycloakConfiguration.Realm);

        var response = _httpClient.PutAsync(addGroupUrl, null);
        response.Wait();
        if (response.Status != TaskStatus.RanToCompletion)
        {
            bPMFunc.LogMessage(_keycloakConfiguration.LoggedInUser, _keycloakConfiguration.UserKey, $"Failed to add user to group: {response.Result.StatusCode}");
            throw new HttpRequestException($"Failed to add user to group: {response.Result.StatusCode}");
        }
    }

    private async Task<string> GetGroupIdByNameAsync(string groupName)
    {
        var tokenResponse = await GetAdminTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

        var groupUrl = string.Format($"{_uriString}/admin/realms/{_keycloakConfiguration.Realm}/groups?search={groupName}", _uriString, _keycloakConfiguration.Realm);

        var response = _httpClient.GetAsync(groupUrl);
        response.Wait();
        if (response.Status != TaskStatus.RanToCompletion)
        {
            bPMFunc.LogMessage(_keycloakConfiguration.LoggedInUser, _keycloakConfiguration.UserKey, $"Failed to retrieve groups: {response.Result.StatusCode}");

            throw new HttpRequestException($"Failed to retrieve groups: {response.Result.StatusCode}");
        }

        var jsonResponse = await response.Result.Content.ReadAsStringAsync();
        var groups = JsonConvert.DeserializeObject<List<GroupResponseDTO>>(jsonResponse);

        var group = groups?.FirstOrDefault(g => g.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));

        return group?.Id;
    }

    private async Task AddUserToGroupByNameAsync(string userId, string groupName)
    {
        var groupId = await GetGroupIdByNameAsync(groupName);
        await AddUserToGroupAsync(userId, groupId);
    }

    private async Task<List<UserResponseDTO>> GetUsersInGroupAsync(string groupId)
    {
        var tokenResponse = await GetAdminTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

        var usersUrl = string.Format($"{_uriString}/admin/realms/{_keycloakConfiguration.Realm}/groups/{groupId}/members", _uriString, _keycloakConfiguration.Realm);

        var response = _httpClient.GetAsync(usersUrl);
        response.Wait();
        if (response.Status != TaskStatus.RanToCompletion)
        {
            bPMFunc.LogMessage(_keycloakConfiguration.LoggedInUser, _keycloakConfiguration.UserKey, $"Failed to retrieve users in group: {response.Result.StatusCode}");

            throw new HttpRequestException($"Failed to retrieve users in group: {response.Result.StatusCode}");
        }

        var jsonResponse = await response.Result.Content.ReadAsStringAsync();

        var users = JsonConvert.DeserializeObject<List<UserResponseDTO>>(jsonResponse);

        if (users == null)
        {
            bPMFunc.LogMessage(_keycloakConfiguration.LoggedInUser, _keycloakConfiguration.UserKey, $"Failed to retrieve users: {response.Result.StatusCode}");
        }
        return users;
    }

    private async Task<Dictionary<string, ClientMappingDTO>> GetRolesByGroupIdAsync(string groupId)
    {
        var tokenResponse = await GetAdminTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

        var roleUrl = string.Format($"{_uriString}/admin/realms/{_keycloakConfiguration.Realm}/groups/{groupId}/role-mappings", _uriString, _keycloakConfiguration.Realm);

        var response = _httpClient.GetAsync(roleUrl);
        response.Wait();
        if (response.Status != TaskStatus.RanToCompletion || response.Result.StatusCode != HttpStatusCode.OK)
        {
            bPMFunc.LogMessage(_keycloakConfiguration.LoggedInUser, _keycloakConfiguration.UserKey, $"Failed to retrieve roles for group {groupId}: {response.Result.StatusCode}");

            throw new HttpRequestException($"Failed to retrieve roles for group {groupId}: {response.Result.StatusCode}");
        }
        var jsonResponse = await response.Result.Content.ReadAsStringAsync();
        var clientMappingsResponse = JsonConvert.DeserializeObject<ClientMappingsResponseDTO>(jsonResponse);
        return clientMappingsResponse.ClientMappings;
    }
    public async Task AddGroupRoleToUserAsync(string userId, string groupId, string roleName)
    {
        var clientMappings = await GetRolesByGroupIdAsync(groupId);

        foreach (var item in clientMappings)
        {
            var mappings = item.Value;
            foreach (var itemRoles in mappings.Mappings)
            {
                RoleMappingDTO role = new RoleMappingDTO();
                role.ClientRole = itemRoles.ClientRole;
                role.ContainerId = itemRoles.ContainerId;
                role.Composite = itemRoles.Composite;
                role.Description = itemRoles.Description;
                role.Id = itemRoles.Id;
                role.Name = itemRoles.Name;
                await AddRoleToUserAsync(userId, role);
            }
        }
    }

    private async Task AddRoleToUserAsync(string userId, RoleMappingDTO role)
    {
        var tokenResponse = await GetAdminTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

        var addRoleUrl = string.Format($"{_uriString}/admin/realms/{_keycloakConfiguration.Realm}/users/{userId}/role-mappings/clients/{role.ContainerId}", _uriString, _keycloakConfiguration.Realm);
        var content = new StringContent(JsonConvert.SerializeObject(new[] { new { id = role.Id, name = role.Name } }), Encoding.UTF8, "application/json");

        var response = _httpClient.PostAsync(addRoleUrl, content);
        response.Wait();
        if (response.Status != TaskStatus.RanToCompletion)
        {
            bPMFunc.LogMessage(_keycloakConfiguration.LoggedInUser, _keycloakConfiguration.UserKey, $"Failed to add role to user: {response.Result.StatusCode}");
            throw new HttpRequestException($"Failed to add role to user: {response.Result.StatusCode}");
        }

    }
}
