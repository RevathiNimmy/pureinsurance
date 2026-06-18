Imports System
Imports System.IdentityModel.Services
Imports System.IdentityModel.Services.Configuration
Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants.Session
Imports System.Web
Imports System.Web.UI
Imports System.Linq
Imports System.Security.Principal
Imports Microsoft.Owin.Security
Imports Microsoft.Owin.Security.Cookies
Imports Microsoft.Owin.Security.OpenIdConnect
Imports Microsoft.Owin.Security.WsFederation
Imports Nexus.Library.Config
Namespace Nexus

    Partial Class secure_logout : Inherits System.Web.UI.Page
        Private Const AWS_Session_Cookie As String = "AWSELBAuthSessionCookie-0"
        ''' <summary>
        ''' Performs Unlocking of locks and  clear the session and signout the forms authentication
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim sMessage As String = String.Empty
            Dim sScript As String = Nothing
            If Not IsPostBack Then

                Dim oWebService As NexusProvider.ProviderBase
                Dim bClearAll As Boolean = False
                Dim bLogout As Boolean = True
                Try
                    Dim oUserDetails As NexusProvider.UserDetails
                    oUserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                    If oUserDetails IsNot Nothing Then
                        Dim sBranchCode As String = oUserDetails.ListOfBranches(0).Code
                        oWebService = New NexusProvider.ProviderManager().Provider
                        oWebService.MaintainLock(Nothing, bClearAll, bLogout, sBranchCode)
                    End If
                Catch ex As System.Exception
                    sMessage = GetLocalResourceObject("err_ErrorOccured")
                End Try

            End If

            If sMessage.Length > 0 Then
                sScript = "$(document).ready(function(){logout('" & sMessage & "');});"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "claimlockederror", sScript, True)
            End If

            Session.Abandon()
            Session.Clear()
            HttpContext.Current.Cache.Remove("PortalID")
            System.Web.Security.FormsAuthentication.SignOut()

            If (Context.User.Identity.AuthenticationType = "Federation") Then
                Dim oFederationConfiguration As FederationConfiguration = FederatedAuthentication.FederationConfiguration
                Dim oFederatedAuthentication = FederatedAuthentication.WSFederationAuthenticationModule
                oFederatedAuthentication.SignOut(False)
                Dim oSignOutRequestMessage = New SignOutRequestMessage(New Uri(oFederatedAuthentication.Issuer), oFederatedAuthentication.Realm)
                HttpContext.Current.User = New GenericPrincipal(New GenericIdentity(String.Empty), {""})
                Response.Redirect(New SignOutRequestMessage(New Uri(oFederatedAuthentication.Issuer), oFederatedAuthentication.Realm).WriteQueryString())
            Else

                Dim providerSection As IdentityProvider = CType(ConfigurationManager.GetSection("IdentityProvider"), IdentityProvider)
                Dim identityProvider = providerSection.DefaultIdentity.ToUpper
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

                sScript = "<script language=javascript>window.top.close();</script>"
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "clientScript", sScript)
            End If

        End Sub

        Public Sub SignOut()
            'Dim fc As WsFederationConfiguration = FederatedAuthentication.FederationConfiguration.WsFederationConfiguration

            'Dim request As String = System.Web.HttpContext.Current.Request.Url.ToString()
            'Dim wreply As String = request.Substring(0, request.Length - 7)

            'Dim soMessage As New SignOutRequestMessage(New Uri(fc.Issuer), wreply)
            'soMessage.SetParameter("wtrealm", fc.Realm)

            'FederatedAuthentication.SessionAuthenticationModule.SignOut()
            'Response.Redirect(soMessage.WriteQueryString())
        End Sub

        Private Sub FederatedSignOut(Optional reply As String = Nothing)

            ''Dim fam As WSFederationAuthenticationModule = FederatedAuthentication.WSFederationAuthenticationModule

            ' '' Native FederatedSignOut doesn't seem to have a way for finding/registering realm for singout, get it from the FAM
            ''Dim wrealm As String = String.Format("wtrealm={0}", fam.Realm)

            ' '' Create basic url for signout (wreply is set by native FederatedSignOut)
            ''Dim signOutUrl As String = WSFederationAuthenticationModule.GetFederationPassiveSignOutUrl(fam.Issuer, Nothing, wrealm)

            ' '' Check where to return, if not set ACS will use Reply address configured for the RP
            ''Dim wreply As String = If(Not String.IsNullOrEmpty(reply), reply, (If(Not String.IsNullOrEmpty(fam.Reply), fam.Reply, Nothing)))

            ''WSFederationAuthenticationModule.FederatedSignOut(New Uri(signOutUrl), If(Not String.IsNullOrEmpty(wreply), New Uri(wreply), Nothing))

            ' Remarks! Native FederatedSignout has an option for setting signOutUrl to null, even if the documentation tells otherwise.
            ' If set to null the method will search for signoutUrl in Session token, but I couldn't find any information about how to set this. Found some Sharepoint code that use this
            ' Michele Leroux Bustamante had a code example (from 2010) that also uses this form.
            ' Other examples creates the signout url manually and calls redirect.

            ' FAM has support for wsignoutcleanup1.0 right out of the box, there is no need for code to handle this.
            ' That makes it even harder to understand why there are no complete FederatedSignOut method in FAM

            ' When using native FederatedSignOut() no events for signout will be called, if you need this use the FAM SignOut methods instead.

        End Sub

    End Class



End Namespace
