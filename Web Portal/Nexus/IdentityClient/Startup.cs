using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using Nexus.Library.Config;
using NexusProvider;
using Owin;
using System;
using System.Configuration;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;
using Claim = System.Security.Claims.Claim;
using Task = System.Threading.Tasks.Task;

[assembly: OwinStartup(typeof(IdentityClient.Startup))]

namespace IdentityClient
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            /* Should be comment on Production
            IdentityModelEventSource.ShowPII = true;
            IdentityModelEventSource.Logger.LogLevel = (System.Diagnostics.Tracing.EventLevel)System.Diagnostics.TraceLevel.Verbose;
            */

            var webRoot = WebConfigurationManager.AppSettings["WebRoot"].ToString();
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            IdentityProvider providerSection = ConfigurationManager.GetSection("IdentityProvider") as IdentityProvider;

            // Enable Cookie Authentication (Required for session persistence)
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = "Cookies",
                ExpireTimeSpan = TimeSpan.FromMinutes(providerSection.ExpireTimeSpan),
                SlidingExpiration = true,
                CookieSecure = CookieSecureOption.Always,
                CookieHttpOnly = true,
                CookieSameSite = Microsoft.Owin.SameSiteMode.None
            });

            if (providerSection != null)
            {
                if (providerSection.DefaultIdentity == "SSO")
                {
                    ProviderSettings openIdConnect = providerSection.Identity[providerSection.DefaultIdentity];

                    string tenantId = openIdConnect.Parameters["TenantID"];
                    string clientId = openIdConnect.Parameters["ClientID"];
                    string clientSecret = openIdConnect.Parameters["ClientSecret"];
                    string aadInstance = EnsureTrailingSlash(openIdConnect.Parameters["AADInstance"]);
                    string redirectUri = openIdConnect.Parameters["RedirectUriChallenge"];
                    string authority = $"{aadInstance}/{tenantId}/v2.0";

                    app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
                    {
                        ClientId = clientId,
                        Authority = authority,
                        RedirectUri = redirectUri,
                        Scope = "openid profile offline_access",
                        PostLogoutRedirectUri = redirectUri,

                        TokenValidationParameters = new TokenValidationParameters
                        {
                            NameClaimType = "pureUsername"
                        },

                        Notifications = new OpenIdConnectAuthenticationNotifications()
                        {
                            AuthenticationFailed = (context) =>
                            {
                                var code = context.ProtocolMessage.Code;
                                var token = context.ProtocolMessage.IdToken;
                                var nonce = context.ProtocolMessage.Nonce;
                                IdentityModelEventSource.Logger.WriteError("Authentication failed: " + context.Exception.ToString());
                                context.HandleResponse();
                                string errorRef = Guid.NewGuid().ToString();
                                HttpContext.Current.Application.Add(errorRef, context.Exception);
                                context.Response.Redirect($"{webRoot}Error.aspx?aspxerrorpath=Startup.cs&ERef={errorRef}");
                                return Task.CompletedTask;
                            },
                            SecurityTokenValidated = (context) =>
                            {
                                var code = context.ProtocolMessage.Code;
                                var token = context.ProtocolMessage.IdToken;
                                var nonce = context.ProtocolMessage.Nonce;

                                var identity = context.AuthenticationTicket.Identity;

                                var name = identity.FindFirst("name").Value;
                                var email = identity.FindFirst("preferred_username").Value;
                                SetSessionValues(context);

                                var tokenUrl = $"{aadInstance}/{tenantId}/oauth2/v2.0/token";
                                if (tokenUrl != null)
                                {
                                    HttpContext.Current.Session.Add("tokenUrl", tokenUrl);
                                }
                                var userDetail = GetUserDetails(email);
                                if (String.IsNullOrEmpty(userDetail.PureUsername))
                                {
                                    context.HandleResponse();
                                    string errorRef = Guid.NewGuid().ToString();
                                    var unAuthError = new UnauthorizedAccessException("The user does not have the authority to perform this action.");
                                    HttpContext.Current.Application.Add(errorRef, unAuthError);
                                    context.Response.Redirect($"{webRoot}Error.aspx?aspxerrorpath=Startup.cs&ERef={errorRef}");
                                    return Task.CompletedTask;
                                }

                                var expiresAt = context.ProtocolMessage.ExpiresIn;
                                var expirationTime = DateTime.UtcNow.AddSeconds(Convert.ToDouble(expiresAt));

                                identity.AddClaim(new Claim("pureUsername", userDetail.PureUsername, ClaimValueTypes.String));
                                identity.AddClaim(new Claim("tenent", tenantId, ClaimValueTypes.String));
                                identity.AddClaim(new Claim("expires_at", expirationTime.ToString("o")));
                                identity.AddClaim(new Claim("id_token", context.ProtocolMessage.IdToken));

                                // Create a new ticket with the updated claims
                                context.AuthenticationTicket = new AuthenticationTicket(identity, context.AuthenticationTicket.Properties);

                                IdentityModelEventSource.Logger.WriteInformation("Security token validated successfully.");

                                return Task.CompletedTask;
                            },
                        }
                    });
                }
                if (providerSection.DefaultIdentity.ToUpper() == "KEYCLOAK")
                {
                    ProviderSettings openIdConnect = providerSection.Identity[providerSection.DefaultIdentity];

                    string clientId = openIdConnect.Parameters["ClientID"];
                    string clientSecret = openIdConnect.Parameters["ClientSecret"];
                    string redirectUri = openIdConnect.Parameters["RedirectUri"];
                    string authority = openIdConnect.Parameters["Authority"];
                    string logoutRedirectUri = openIdConnect.Parameters["LogoutRedirectUri"];
                    app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
                    {
                        Authority = authority,
                        ClientId = clientId,
                        ClientSecret = clientSecret,
                        RedirectUri = redirectUri,
                        ResponseType = "code",
                        Scope = "openid profile email",
                        SignInAsAuthenticationType = "Cookies",
                        UseTokenLifetime = false,
                        RedeemCode = true,
                        SaveTokens = true,
                        RequireHttpsMetadata = false,

                        TokenValidationParameters = new TokenValidationParameters
                        {
                            NameClaimType = "pureUsername"
                        },

                        Notifications = new OpenIdConnectAuthenticationNotifications
                        {
                            SecurityTokenValidated = (context) =>
                            {
                                var code = context.ProtocolMessage.Code;
                                var token = context.ProtocolMessage.IdToken;
                                var nonce = context.ProtocolMessage.Nonce;

                                var identity = context.AuthenticationTicket.Identity;

                                var name = identity.FindFirst("name").Value;
                                var email = identity.FindFirst("preferred_username").Value;
                                SetSessionValues(context);
                                
                                var tokenUrl = $"{authority}/protocol/openid-connect/token";
                                if (tokenUrl != null)
                                {
                                    HttpContext.Current.Session.Add("tokenUrl", tokenUrl);
                                }
                                var userDetail = GetUserDetails(email);
                                if (String.IsNullOrEmpty(userDetail.PureUsername))
                                {
                                    context.HandleResponse();
                                    string errorRef = Guid.NewGuid().ToString();
                                    var unAuthError = new UnauthorizedAccessException("The user does not have the authority to perform this action.");
                                    HttpContext.Current.Application.Add(errorRef, unAuthError);
                                    context.Response.Redirect($"{webRoot}Error.aspx?aspxerrorpath=Startup.cs&ERef={errorRef}");
                                    return Task.CompletedTask;
                                }

                                var expiresAt = context.ProtocolMessage.ExpiresIn;
                                var expirationTime = DateTime.UtcNow.AddSeconds(Convert.ToDouble(expiresAt));

                                identity.AddClaim(new Claim("pureUsername", userDetail.PureUsername, ClaimValueTypes.String));
                                identity.AddClaim(new Claim("expires_at", expirationTime.ToString("o")));
                                identity.AddClaim(new Claim("id_token", context.ProtocolMessage.IdToken));
                                identity.AddClaim(new Claim("access_token", context.ProtocolMessage.AccessToken));
                                identity.AddClaim(new Claim("refresh_token", context.ProtocolMessage.RefreshToken));

                                context.AuthenticationTicket = new AuthenticationTicket(identity,
                                                                new AuthenticationProperties
                                                                {
                                                                    IsPersistent = true
                                                                });

                                context.OwinContext.Authentication.SignIn(context.AuthenticationTicket.Identity);

                                IdentityModelEventSource.Logger.WriteInformation("Security token validated successfully.");

                                return Task.CompletedTask;
                            },
                            AuthenticationFailed = context =>
                            {
                                var code = context.ProtocolMessage.Code;
                                var token = context.ProtocolMessage.IdToken;
                                var nonce = context.ProtocolMessage.Nonce;
                                IdentityModelEventSource.Logger.WriteError("Authentication failed: " + context.Exception.ToString());
                                context.HandleResponse();
                                string errorRef = Guid.NewGuid().ToString();
                                HttpContext.Current.Application.Add(errorRef, context.Exception);
                                context.Response.Redirect($"{webRoot}Error.aspx?aspxerrorpath=Startup.cs&ERef={errorRef}");
                                return Task.CompletedTask;
                            }

                        }
                    });
                }
            }
            else
            {
                IdentityModelEventSource.Logger.WriteError("IdentityProvider section not found.");
                throw new ConfigurationErrorsException("IdentityProvider section not found.");
            }
        }


        private static string EnsureTrailingSlash(string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }

            if (!value.EndsWith("/", StringComparison.Ordinal))
            {
                return value + "/";
            }

            return value;
        }
        private UserDetails GetUserDetails(string username)
        {
            UserDetails userDetails = null;
            try
            {
                var webService = new ProviderManager().Provider;
                userDetails = webService.GetUserDetails(sUserName: username, bIsSSO: true);
            }
            catch
            {
                throw;
            }
            return userDetails;
        }
        static void SetSessionValues(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> context)
        {
            var expiresAt = context.ProtocolMessage.ExpiresIn;
            var expirationTime = DateTime.UtcNow.AddSeconds(Convert.ToDouble(expiresAt));
            if (expirationTime.ToString("o") != null) HttpContext.Current.Session.Add("expires_at", expirationTime.ToString("o"));
            if (context.ProtocolMessage.AccessToken != null) HttpContext.Current.Session.Add("access_token", context.ProtocolMessage.AccessToken);
            if (context.ProtocolMessage.RefreshToken != null) HttpContext.Current.Session.Add("refresh_token", context.ProtocolMessage.RefreshToken);

        }

    }
}