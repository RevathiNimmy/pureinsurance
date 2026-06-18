Imports System.Configuration.ConfigurationManager
Imports System.IdentityModel.Services
Imports System.IdentityModel.Services.Configuration
Imports System.Linq
Imports System.Security.Principal
Imports CMS.Library
Imports Microsoft.Owin.Security
Imports Microsoft.Owin.Security.Cookies
Imports Microsoft.Owin.Security.OpenIdConnect
Imports Microsoft.Owin.Security.WsFederation
Imports Nexus.Library.Config

Namespace Nexus

	Partial Class SessionExpired : Inherits Frontend.clsCMSPage

		Private Const AWS_Session_Cookie As String = "AWSELBAuthSessionCookie-0"
		Protected Sub SessionExpired_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
			Dim authResponseCookie As HttpCookie = Nothing
			Dim authRequestCookie As HttpCookie = Nothing
			Session.Abandon()
			Session.Clear()

			If Not Request.Cookies("SessionTimeout") Is Nothing Then
				Request.Cookies("SessionTimeout").Expires = DateTime.Now.AddDays(-1)
				Response.Cookies.Add(Request.Cookies("SessionTimeout"))
				authResponseCookie = HttpContext.Current.Response.Cookies("SessionTimeout")
				authRequestCookie = HttpContext.Current.Request.Cookies("SessionTimeout")
				authResponseCookie.SameSite = SameSiteMode.None
				Dim requireSsl As Boolean = authResponseCookie.Secure
				If (requireSsl) Then
					authResponseCookie.Secure = True
					authResponseCookie.HttpOnly = True
					authRequestCookie.Secure = True
				Else
					authResponseCookie.Secure = False
					authResponseCookie.HttpOnly = False
					authRequestCookie.Secure = False
				End If
			End If
			If (Context.User.Identity.AuthenticationType = "Federation") Then
				Dim oFederationConfiguration As FederationConfiguration = FederatedAuthentication.FederationConfiguration
				Dim oFederatedAuthentication = FederatedAuthentication.WSFederationAuthenticationModule
				oFederatedAuthentication.SignOut(False)
				Dim oSignOutRequestMessage = New SignOutRequestMessage(New Uri(oFederatedAuthentication.Issuer), oFederatedAuthentication.Realm)
				Response.Redirect(New SignOutRequestMessage(New Uri(oFederatedAuthentication.Issuer), oFederatedAuthentication.Realm).WriteQueryString())
			Else

				Dim cookie1 As HttpCookie = New HttpCookie(FormsAuthentication.FormsCookieName, "")
				cookie1.Expires = DateTime.Now.AddYears(-1)
				Response.Cookies.Add(cookie1)

				HttpContext.Current.Request.Cookies.Clear()

				Dim providerSection As IdentityProvider = CType(ConfigurationManager.GetSection("IdentityProvider"), IdentityProvider)
				Dim identityProvider = providerSection.DefaultIdentity
				Dim authManager = HttpContext.Current.GetOwinContext().Authentication

				If Equals(identityProvider, "SSO") Then
					Dim openIdConnect As ProviderSettings = providerSection.Identity(providerSection.DefaultIdentity)
					Dim aadInstance = openIdConnect.Parameters("AADInstance")
					Dim callbackUrl As String = Request.Url.GetLeftPart(UriPartial.Authority) + Response.ApplyAppPathModifier("~/Login.aspx")
					Dim logoutUrl As String = aadInstance + "/common/oauth2/logout?post_logout_redirect_uri=" + callbackUrl

					Dim authenticationProperties = New AuthenticationProperties With {
					.RedirectUri = logoutUrl
				}
					authManager.SignOut(authenticationProperties, OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType)
					HttpContext.Current.User = New GenericPrincipal(New GenericIdentity(String.Empty), {""})
				ElseIf Equals(identityProvider, "KEYCLOAK") Then
					Dim keyCloak As ProviderSettings = providerSection.Identity(providerSection.DefaultIdentity)
					authManager.SignOut(OpenIdConnectAuthenticationDefaults.AuthenticationType,
								CookieAuthenticationDefaults.AuthenticationType)
					Dim authority = keyCloak.Parameters("Authority")
					Dim keycloakLogoutUrl As String = authority + "/protocol/openid-connect/logout"
					Dim postLogoutRedirectUri As String = keyCloak.Parameters("LogoutRedirectUri")

					Dim idToken As String = Nothing
					Dim identity = HttpContext.Current.GetOwinContext().Authentication.User

					If identity IsNot Nothing AndAlso identity.HasClaim(Function(c) c.Type = "id_token") Then
						idToken = identity.FindFirst("id_token").Value
					End If

					Dim logoutUrl As String = keycloakLogoutUrl & "?post_logout_redirect_uri=" & HttpUtility.UrlEncode(postLogoutRedirectUri)

					If Not String.IsNullOrEmpty(idToken) Then
						logoutUrl &= "&id_token_hint=" & HttpUtility.UrlEncode(idToken)
					End If

					Response.Redirect(logoutUrl, False)
					HttpContext.Current.Request.Cookies.Clear()
					HttpContext.Current.Response.Cookies.Clear()
					Context.ApplicationInstance.CompleteRequest()
				Else
					Dim modules As HttpModuleCollection = HttpContext.Current.ApplicationInstance.Modules
					Dim authModule As IHttpModule = modules.Get("AuthHttpModule")
					If authModule IsNot Nothing Then
						Dim userClaims = TryCast(Context.User.Identity, System.Security.Claims.ClaimsIdentity)
						Dim logOutRedirectUri As String
						logOutRedirectUri = userClaims.Claims.FirstOrDefault(Function(c) c.Type = "allowed-origins").Value & AppSettings("WebRoot") & "/logout.aspx"

						Dim awsSessionCookie As HttpCookie = New HttpCookie(AWS_Session_Cookie)
						awsSessionCookie.SameSite = SameSiteMode.Lax
						awsSessionCookie.Secure = True
						awsSessionCookie.Expires = DateTime.Now.AddYears(-1)
						Response.Cookies.Remove(AWS_Session_Cookie)
						Response.Cookies.Add(awsSessionCookie)

						Dim logOutUrl As String = AppSettings("AuthDomain").ToString & "/protocol/openid-connect/logout?redirect_uri=" & logOutRedirectUri
						Response.Redirect(logOutUrl)
					End If
				End If
				Dim sScript As String = "<script language=javascript>window.top.close();</script>"
				ClientScript.RegisterClientScriptBlock(Me.GetType(), "clientScript", sScript)
			End If
		End Sub
	End Class

End Namespace
