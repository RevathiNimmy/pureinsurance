using bSIRSharepointApi.Models;
using Microsoft.Identity.Client;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace bSIRSharepointApi
{
    public class AuthorizationManager
    {
        SPContextConfiguration model;
        static HttpClient _httpClient = null;
        public AuthorizationManager(SPContextConfiguration spModel)
        {
            model = spModel;
        }

        /// <summary>
        /// Get HTTP client instance
        /// </summary>
        /// <returns></returns>
        public HttpClient GetHttpClient()
        {
            try
            {
                string formDigest = "";
                HttpClientHandler httpClientHandler = null;
                SharePointOnlineCredentials credentials = null;
                if (model.IsSharePointOnline)
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    Uri uri1 = new Uri(model.SharePointSiteURL);
                    string hostName = uri1.Host;

                    string[] segments = uri1.Segments;
                    string siteName = string.Empty;

                    // Find "sites" or "teams" and get the next segment
                    for (int i = 0; i < segments.Length; i++)
                    {
                        if (segments[i].Equals("sites/", StringComparison.OrdinalIgnoreCase) ||
                            segments[i].Equals("teams/", StringComparison.OrdinalIgnoreCase))
                        {
                            if (i + 1 < segments.Length)
                            {
                                siteName = segments[i + 1].TrimEnd('/');
                            }
                            break;
                        }
                    }
                    //Load certificate(.pfx)
                    //var cert = new X509Certificate2(
                    //    @"C:\SharePointOnlineAppCert.pfx",
                    //    "YourStrongPassword123!",
                    //    X509KeyStorageFlags.MachineKeySet);
                    var cert = new X509Certificate2(
                       model.SharePointUserName,
                       model.SharePointPassword,
                       X509KeyStorageFlags.MachineKeySet);

                    var app = ConfidentialClientApplicationBuilder.Create(model.AppClientId)
                        .WithCertificate(cert)
                        .WithAuthority(new Uri($"https://login.microsoftonline.com/{model.SharepointTenantId}"))
                        .Build();
                    string baseUrl = $"{uri1.Scheme}://{uri1.Host}";
                    string[] scopes = {  $"{uri1.Scheme}://{uri1.Host}/.default" };

                    // Acquire token
                    var result = Task.Run(async()=> await app.AcquireTokenForClient(scopes).ExecuteAsync()).Result;

                    // Call SharePoint REST API
                     _httpClient = new HttpClient();
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    return _httpClient;
                }
                else
                {
                    httpClientHandler = new HttpClientHandler()
                    {
                        UseDefaultCredentials = true,
                        Credentials = CredentialCache.DefaultNetworkCredentials
                    };

                }
                Uri uri = new Uri(model.SharePointSiteURL);
                _httpClient = new HttpClient(httpClientHandler)
                {
                    BaseAddress = new System.Uri(uri.GetLeftPart(UriPartial.Authority)),
                    Timeout = TimeSpan.FromSeconds(5000)
                };

                if (model.IsSharePointOnline) httpClientHandler.CookieContainer.SetCookies(new Uri(uri.GetLeftPart(UriPartial.Authority)), credentials.GetAuthenticationCookie(new Uri(uri.GetLeftPart(UriPartial.Authority))));

                _httpClient.DefaultRequestHeaders.Add("accept", "application/json;odata=verbose");
                _httpClient.DefaultRequestHeaders.Add("X-RequestDigest", string.IsNullOrEmpty(formDigest) ? GetFormDigestValue() : formDigest);

                return _httpClient;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Get Form Digest Value
        /// </summary>
        /// <returns></returns>
        private string GetFormDigestValue()
        {
            var contextPath = $"{model.SharePointSiteURL}/_api/contextinfo";
            var response = _httpClient
                .PostAsync(contextPath, null)
                .GetAwaiter()
                .GetResult()
               .EnsureSuccessStatusCode();

            // parse json response
            dynamic parsed = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);

            // return FormDigestValue
            return parsed.d.GetContextWebInformation.FormDigestValue; // example response: 0xF11B65FE8814D7931F89FADE490B7C43FC562E5488D9D6D9D5D96556030B98E8513C6D5F8B3ADB628A178A4B49E3D95CBFDE4AAAB1C43610F714AE7C5C7D303A,17 Sep 2021 15:11:22 -0000
        }
    }
}
