Imports System.Security.Claims
Imports System.Security.Cryptography
Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Microsoft.Owin.Security
Imports Microsoft.Owin.Security.OpenIdConnect
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Library.Config
Imports Nexus.Utils.FuncSecurity

Namespace Nexus

	Partial Class LogIn : Inherits Frontend.clsCMSPage

		Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
			Dim providerSection As IdentityProvider = CType(ConfigurationManager.GetSection("IdentityProvider"), IdentityProvider)
			Dim identityProvider = providerSection.DefaultIdentity.ToUpper
			Dim authManager = HttpContext.Current.GetOwinContext().Authentication

			If Equals(identityProvider, "SSO") OrElse Equals(identityProvider, "KEYCLOAK") Then
				If Not Context.User.Identity.IsAuthenticated Then

					Dim authProperties = New AuthenticationProperties() With
					{
					.RedirectUri = ConfigurationManager.AppSettings("webroot") + "Login.aspx"
					}

					authManager.Challenge(authProperties, OpenIdConnectAuthenticationDefaults.AuthenticationType)
				ElseIf Context.User.Identity.IsAuthenticated Then
					HttpContext.Current.User = authManager.User
					SetSessionValues()
					Throw New Exception("Invalid configurations")
				End If
			Else
				If Context.User.Identity.IsAuthenticated Then
					Dim sAgentStartPage As String = CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AgentStartPage
					HttpContext.Current.Response.Redirect(sAgentStartPage)
				Else
					Throw New Exception("Invalid configurations")
				End If
			End If
		End Sub

		Private Sub SetSessionValues()
			If Context.User.Identity.IsAuthenticated Then

				'in this case then we must get user details before we proceed
				'remove any session values left over (shouldn't be any really)
				HttpContext.Current.Session.Remove(CNAgentDetails)

				Dim oWebService As NexusProvider.ProviderBase
				Dim oUserDetails As NexusProvider.UserDetails
				Dim oUserGroup, oUserGroupLoop As New NexusProvider.UserGroup
				Dim UserRoles As String

				oWebService = New NexusProvider.ProviderManager().Provider

				'Retrieve the user details and authenticate
				Try
					oUserDetails = oWebService.GetUserDetails(HttpContext.Current.User.Identity.Name)
					Membership.DeleteUser(HttpContext.Current.User.Identity.Name, True)

					'check the Usergroups Agent belongs to and add all the roles he has been assigned
					If Not oUserDetails.AvailableUsergroups Is Nothing Then
						For Each oUserGroup In oUserDetails.AvailableUsergroups
							'check if role exists. if it doesn't then create it
							If Not Roles.RoleExists(oUserGroup.Code.Trim) Then
								Roles.CreateRole(oUserGroup.Code)
							End If

							'check if user is already in role. if not then add them
							If Not Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, oUserGroup.Code.Trim) Then
								Try
									Roles.AddUserToRole(HttpContext.Current.User.Identity.Name, oUserGroup.Code.Trim)
								Catch
									'if same user will try to login at same time then AddUserToRole can thow error
									'Nothing to do with this error
								End Try

							End If
						Next
					End If

					UserRoles = CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AllowRole
					HttpContext.Current.Session.Add(CNUserIdentityName, stripDomainPrefix(HttpContext.Current.User.Identity.Name))
					HttpContext.Current.Session.Add(CNAgentDetails, oUserDetails)
					HttpContext.Current.Session.Add(CNLoginType, LoginType.Agent)
					HttpContext.Current.Session.Add(CNLoginName, Trim(oUserDetails.PureUsername))
					HttpContext.Current.Session.Add(CNUserId, oUserDetails.UserId)
					HttpContext.Current.Session(CNClaimFlag) = "OFF"
					HttpContext.Current.Session.Add(CNEncryptedSessionKey, GenerateEncryptedHashKey())

					'Dim providerSection As IdentityProvider = CType(ConfigurationManager.GetSection("IdentityProvider"), IdentityProvider)
					'Dim providerSettings As ProviderSettings = providerSection.Identity(providerSection.DefaultIdentity)
					'Dim identityProvider = providerSection.DefaultIdentity.ToUpper
					'If Equals(identityProvider, "SSO") Then
					'	Dim aadInstance = providerSettings.Parameters("AADInstance")
					'	Dim tenantID = providerSettings.Parameters("TenantIDv")
					'	Dim tokenUrl = aadInstance + "/" + tenantID + "/oauth2/v2.0/token"
					'	HttpContext.Current.Session.Add("tokenUrl", tokenUrl)
					'ElseIf Equals(identityProvider, "KEYCLOAK") Then
					'	Dim autority = providerSettings.Parameters("Authority")
					'	Dim tokenUrl = autority + "/protocol/openid-connect/token"
					'	HttpContext.Current.Session.Add("tokenUrl", tokenUrl)
					'End If

					'Dim principal As ClaimsPrincipal = TryCast(HttpContext.Current.User, ClaimsPrincipal)
					'If principal IsNot Nothing Then
					'	HttpContext.Current.Session.Add("expires_at", principal.FindFirst("expires_at").Value)
					'	HttpContext.Current.Session.Add("access_token", principal.FindFirst("access_token").Value)
					'	HttpContext.Current.Session.Add("refresh_token", principal.FindFirst("refresh_token").Value)
					'End If

					'Find that Enable Branch Selection At logon is enabled or not
					Dim bEnableBranchSelectionAtLogin As Boolean = False
					If CType(HttpContext.Current.Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches.Count > 1 Then
						Dim oOptionType As New NexusProvider.OptionTypeSetting
						oOptionType = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 37)
						If oOptionType.OptionValue = "1" Then
							bEnableBranchSelectionAtLogin = True
						End If
					End If


					If CType(HttpContext.Current.Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches.Count > 1 And bEnableBranchSelectionAtLogin = True Then
						'if an agent has more than 1 branches and option set to force a choice to 
						'be made on login then go to select branch
						HttpContext.Current.Response.Redirect("~/SelectBranch.aspx")
					Else
						'set first branch as default
						HttpContext.Current.Session(CNBranchCode) = CType(HttpContext.Current.Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches(0).Code
						'redirect to start page
						Dim sAgentStartPage As String = CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AgentStartPage
						HttpContext.Current.Response.Redirect(sAgentStartPage)
					End If

				Catch ex As System.Web.Services.Protocols.SoapHeaderException
					'Current user is not set up for access to BO / SAM so abandon session and throw an exception
					'By abandoning the session we force this code to be run again if another request is made
					Session.Abandon()
					Throw New Exception("Authentication failed")
				Finally
					oWebService = Nothing
					oUserDetails = Nothing
				End Try
			Else
				If IsWindowsAuthentication() Then
					Throw New Exception("Invalid user credentials.")
				End If
				'HttpContext.Current.Session.Add(CNRequestedUrl, HttpContext.Current.Request.Url())
				HttpContext.Current.Response.Redirect("~/SessionExpired.aspx")
			End If
		End Sub
		Private Function GenerateEncryptedHashKey() As String

			Dim loggedUserName As String = HttpContext.Current.Session(CNUserIdentityName).ToString()
			Dim sbEncryptKey As StringBuilder = New StringBuilder()
			sbEncryptKey.Append(HttpContext.Current.Request.Browser.Browser)
			sbEncryptKey.Append(HttpContext.Current.Request.Browser.Platform)
			sbEncryptKey.Append(HttpContext.Current.Request.Browser.MajorVersion)
			sbEncryptKey.Append(HttpContext.Current.Request.Browser.MinorVersion)
			sbEncryptKey.Append(Request.ServerVariables("REMOTE_ADDR").ToString())
			sbEncryptKey.Append(loggedUserName)

			Dim shaKey As SHA1 = New SHA1CryptoServiceProvider()
			Dim encryptedHashKey As Byte() = shaKey.ComputeHash(Encoding.UTF8.GetBytes(sbEncryptKey.ToString()))
			Return Convert.ToBase64String(encryptedHashKey)
		End Function
		Public Function stripDomainPrefix(FQDUser As String) As String
			Dim sUserPath As String() = FQDUser.Split(New Char() {"\"c})
			stripDomainPrefix = sUserPath((sUserPath.Length - 1))
		End Function
	End Class

End Namespace
