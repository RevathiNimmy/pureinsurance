<%@ Application Language="VB" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Nexus.Utils" %>
<%@ Import Namespace="Nexus.Utils.FuncSecurity" %>
<%@ Import Namespace="Nexus.Constants.Constant" %>
<%@ Import Namespace="Nexus.Constants.Session" %>
<%@ Import Namespace="Nexus.Library.Config" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Security.Authentication" %>
<%@ Import Namespace="System.Web.Caching" %>
<%@ Import Namespace="System.Configuration" %>
<%@ Import Namespace="System.Web.Security" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Web.Configuration.WebConfigurationManager" %>
<%@ Import Namespace="System.Web.Configuration" %>
<%@ Import Namespace="Microsoft.Practices.EnterpriseLibrary.Logging" %>
<%@ Import Namespace="System.Diagnostics" %>
<%@ Import Namespace="System.IdentityModel.Services" %>
<%@ Import Namespace="System.IdentityModel.Services.Configuration" %>
<%@ Import Namespace="System.Security.Cryptography" %>
<script RunAt="server">
	Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
		Dim strAddedContent As String = String.Empty
		Dim requestedUrl As String = Request.Url.ToString().ToLower()
		Dim invalidUrl As String() = requestedUrl.Split(New String() {".aspx"}, StringSplitOptions.None)
		Dim strCrossSiteRegex As String = System.Configuration.ConfigurationManager.AppSettings("CrossSiteRegex")
		Dim invalidCharArray() As String = strCrossSiteRegex.Split("|")
		If invalidUrl.Length > 1 Then
			strAddedContent = invalidUrl(1)
			For Each value As String In invalidCharArray
				If (value = "/" AndAlso strAddedContent.Contains(value)) Then
					If (Not strAddedContent.ToUpper().Contains("RETURNURL") AndAlso Not strAddedContent.ToUpper().Contains("DATE") AndAlso Not strAddedContent.ToUpper().Contains("SELECTRECORD") AndAlso Not strAddedContent.ToUpper().Contains("FOLDERNUM")) Then
						Response.Write("Request url have been tampered.Please use valid session or login again to continue.")
						Response.End()
					End If
				End If
			Next
		End If
		' Check for specific patterns and block them
		If (Not requestedUrl.ToLower().Contains("ashx")) Then
			For Each value As String In invalidCharArray
				If (value <> "/" AndAlso requestedUrl.Contains(value)) Then
					If (strAddedContent <> String.Empty AndAlso Not strAddedContent.ToUpper().Contains("RETURNURL") AndAlso Not strAddedContent.ToUpper().Contains("DATE") AndAlso Not strAddedContent.ToUpper().Contains("SELECTRECORD") AndAlso Not strAddedContent.ToUpper().Contains("FOLDERNUM") AndAlso Not strAddedContent.ToUpper().Contains("DOCPATH")) Then
						Response.Write("Request url have been tampered.Please use valid session or login again to continue.")
						Response.End()
					End If
				End If
			Next
		End If

		HttpContext.Current.Response.Headers.Remove("X-Powered-By")
	End Sub
	Sub Application_PreSendRequestHeaders(ByVal sender As Object, ByVal e As EventArgs)
		Response.Headers.Remove("Server")
	End Sub

	Sub Application_EndRequest(ByVal sender As Object, ByVal e As EventArgs)
		Dim cookieName As String = String.Empty
		Dim authCookie As HttpCookie = Nothing
		cookieName = FormsAuthentication.FormsCookieName
		authCookie = Context.Response.Cookies(cookieName)
		If authCookie Is Nothing Then
			Return
		End If
		authCookie.SameSite = SameSiteMode.Lax
		authCookie.HttpOnly = True
		authCookie.Secure = True
	End Sub
	Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)

		'This code prevents multiple login if AllowMultipleLogins set to false in the web.config file.
		'It looks for session id of the logged in user in the database and if it doesnt exist as the user
		'has logged in from an alternate location and thereby session id has been updated, force logout occurs.  
		If Not IsWindowsAuthentication() Then
			Dim cookieName As String = String.Empty
			Dim authCookie As HttpCookie = Nothing
			If CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AllowMultipleLogins = False Then
				cookieName = FormsAuthentication.FormsCookieName
				authCookie = Context.Request.Cookies(cookieName)
				If authCookie Is Nothing Then
					Return
				End If
				authCookie.SameSite = SameSiteMode.Lax
				authCookie.Secure = True
				authCookie.HttpOnly = True
				Dim command As New SqlCommand("usp_ValidateSessionInfo")

				Dim authTicket As FormsAuthenticationTicket = Nothing
				authTicket = FormsAuthentication.Decrypt(authCookie.Value)
				If authTicket Is Nothing Then
					Return
				End If

				With command.Parameters
					.Add("@vSessionid", SqlDbType.VarChar, 400).Value = authCookie.Value
				End With

				Dim param As SqlParameter = New SqlParameter("@status", SqlDbType.Int, 8)
				param.Direction = ParameterDirection.Output
				command.Parameters.Add(param)

				Dim onStatDs As Int32 = 0
				onStatDs = Convert.ToInt32(funcDB.ExecScalar(command, "CMS"))
				onStatDs = CInt(param.Value.ToString())
				If onStatDs = 1 Then
					FormsAuthentication.SignOut()
					Throw New ApplicationException("Forced logout has occured as the user has logged in from an alternate location !!!")
					Response.Redirect("~/Default.aspx")
				End If
			Else
				Dim providerSection As IdentityProvider = CType(ConfigurationManager.GetSection("IdentityProvider"), IdentityProvider)
				Dim identityProvider = providerSection.DefaultIdentity.ToUpper
				If Not Equals(identityProvider, "SSO") AndAlso Not Equals(identityProvider, "KEYCLOAK") Then
					If Request.Cookies(".ASPXANONYMOUS") IsNot Nothing Then
						Dim reqAuthCookie As HttpCookie = HttpContext.Current.Request.Cookies(".ASPXANONYMOUS")

						authCookie = Context.Response.Cookies(".ASPXANONYMOUS")
						' authCookie.SameSite = SameSiteMode.Lax
						authCookie.Secure = True
						reqAuthCookie.Secure = True
						authCookie.HttpOnly = True

					End If
				End If
			End If

		End If
	End Sub

	Sub Application_AcquireRequestState(ByVal sender As Object, ByVal e As EventArgs)

		If HttpContext.Current.Session IsNot Nothing Then
			Dim providerSection As IdentityProvider = CType(ConfigurationManager.GetSection("IdentityProvider"), IdentityProvider)
			Dim identityProvider = providerSection.DefaultIdentity.ToUpper
			If Equals(identityProvider, "SSO") OrElse Equals(identityProvider, "KEYCLOAK") Then
				If HttpContext.Current.Session(CNAgentDetails) IsNot Nothing AndAlso HttpContext.Current.Session("access_token") Is Nothing Then
					If Not Request.Url.AbsolutePath.ToLower().Contains("sessionexpired.aspx") AndAlso Not Request.Url.AbsolutePath.ToLower().Contains("login.aspx") Then
						Response.Redirect("~/SessionExpired.aspx", False)
						Context.ApplicationInstance.CompleteRequest()
					End If
				End If
			End If

			'This is windows authentication so first check to see if the config allows for multiple sessions 
			If CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AllowMultipleLogins = True Then

				'Now check to see if the config section has switched off cookies so the sessionid is on the URL 
				If GetSection("system.web/sessionState").cookieless.ToString() = "UseUri" Then

					'Now get the users ad details less the domain prefix
					Dim strADUser As String = stripDomainPrefix(HttpContext.Current.User.Identity.Name)
					If HttpContext.Current.Session(CNEncryptedSessionKey) IsNot Nothing Then
						Dim strNewSessionID As String = HttpContext.Current.Session(CNEncryptedSessionKey)
						If strNewSessionID <> GenerateEncryptedHashKey() Then
							Response.Write("The current user is not authorized for this session.  Please use valid session to continue!")
							Response.End()
						End If
					End If
				End If
			End If
		End If
	End Sub

	Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
		' Code that runs on application startup
		System.Net.ServicePointManager.DefaultConnectionLimit = 5000
	End Sub

	Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
		' Code that runs on application shutdown
	End Sub

	Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
		' Code that runs when an unhandled error occurs
		Dim sErrorRef As String = Guid.NewGuid().ToString()
		'Dim sErrorRefN As String = Guid.NewGuid().ToString()
		Dim bNestedError As Boolean
		Dim oCompilationSection As CompilationSection = CType(System.Web.Configuration.WebConfigurationManager.GetSection("system.web/compilation"), CompilationSection)
		Dim objError As Exception = Nothing
		Dim strSessionId As String = String.Empty
		If Server.GetLastError IsNot Nothing AndAlso Server.GetLastError.InnerException IsNot Nothing AndAlso TryCast(Server.GetLastError(), System.Web.HttpUnhandledException).InnerException IsNot Nothing Then
			objError = TryCast(Server.GetLastError(), System.Web.HttpUnhandledException).InnerException
		Else
			objError = Server.GetLastError()
		End If

		If objError IsNot Nothing AndAlso objError.Message.ToString.Contains("potentially dangerous Request.Form") Then
			If Request.UrlReferrer IsNot Nothing Then
				strSessionId = Request.UrlReferrer.Segments(2)
				If strSessionId.Contains("(S(") Then 'cookieless session
					Response.Redirect(String.Format(AppSettings("WebRoot") & strSessionId & "/PotentiallyDangerous.aspx?aspxerrorpath={0}&ERef={1}", HttpUtility.UrlEncode(Request.RawUrl), sErrorRef), True)
				Else
					Response.Redirect(String.Format(AppSettings("WebRoot") & "PotentiallyDangerous.aspx?aspxerrorpath={0}&ERef={1}", HttpUtility.UrlEncode(Request.RawUrl), sErrorRef), True)
				End If
			Else
				Response.Redirect(String.Format(AppSettings("WebRoot") & "PotentiallyDangerous.aspx?aspxerrorpath={0}&ERef={1}", HttpUtility.UrlEncode(Request.RawUrl), sErrorRef), True)
			End If
		End If

		Dim sErrorId As String = ""
		Dim sErrorMessage As String = ""

		Dim oWebService As NexusProvider.ProviderBase
		oWebService = New NexusProvider.ProviderManager().Provider

		If objError IsNot Nothing AndAlso objError.InnerException IsNot Nothing Then
			If TryCast(objError, System.ServiceModel.FaultException(Of System.ServiceModel.ExceptionDetail)) IsNot Nothing Then
				If DirectCast(objError, System.ServiceModel.FaultException(Of System.ServiceModel.ExceptionDetail)).Detail.HelpLink.ToString().Contains("SSP - ERROR REF") = True Then
					sErrorId = TryCast(objError, System.ServiceModel.FaultException(Of System.ServiceModel.ExceptionDetail)).Detail.HelpLink
				End If
			End If
		Else
			If TryCast(objError, System.ServiceModel.FaultException(Of System.ServiceModel.ExceptionDetail)) IsNot Nothing Then
				sErrorId = TryCast(objError, System.ServiceModel.FaultException(Of System.ServiceModel.ExceptionDetail)).Detail.HelpLink
			Else
				If TryCast(objError, NexusProvider.NexusException) IsNot Nothing Then
					If DirectCast(objError, NexusProvider.NexusException).Errors(0).Detail.ToString().Contains("SSP - ERROR REF") = True Then
						sErrorId = DirectCast(objError, NexusProvider.NexusException).Errors(0).Detail
					End If
				End If
			End If
		End If

		If Server.GetLastError IsNot Nothing AndAlso Server.GetLastError.InnerException IsNot Nothing Then
			sErrorMessage = sErrorId & vbCrLf & Server.GetLastError.InnerException.ToString & vbCrLf & "StackTrace : " & Server.GetLastError().StackTrace.ToString
		ElseIf Server.GetLastError IsNot Nothing Then
			sErrorMessage = sErrorId & vbCrLf & "StackTrace : " & Server.GetLastError().StackTrace.ToString
		Else
			sErrorMessage = sErrorId & vbCrLf
		End If
		'Application.Add(sErrorRef, sErrorMessage)
		If objError IsNot Nothing Then
			objError.HelpLink = sErrorId
			Application.Add(sErrorRef, objError)
		End If

		Dim logEntry As New LogEntry()
		logEntry.Priority = NexusProvider.Priority.Normal
		logEntry.Severity = TraceEventType.Error
		logEntry.Message = sErrorMessage
		logEntry.ExtendedProperties = Nothing
		Logger.Write(logEntry)

		'If TypeOf (Server.GetLastError.InnerException) Is NexusProvider.NexusException Then
		'    Application.Add(sErrorRefN, Server.GetLastError)
		'Else
		'    Application.Add(sErrorRefN, Nothing)
		'End If

		'If oCompilationSection.Debug Then
		'    'log the error details according to the logging configuration
		'    'Logger.Write(NexusProvider.ErrorFormatter.FormatErrorAsText(Server.GetLastError()), "Unhandled Exception", 1, 1, TraceEventType.Critical)

		'    Dim logEntry As New LogEntry()
		'    logEntry.Priority = NexusProvider.Priority.Normal
		'    logEntry.Severity = TraceEventType.Error
		'    logEntry.Message = sErrorMessage
		'    logEntry.ExtendedProperties = Nothing
		'    Logger.Write(logEntry)
		'Else
		'    'log the error details according to the logging configuration
		'    'Logger.Write(NexusProvider.ErrorFormatter.FormatErrorAsText(Server.GetLastError()), NexusProvider.Category.CriticalError, NexusProvider.Priority.Highest, 1, TraceEventType.Critical)
		'    Dim logEntry As New LogEntry()
		'    logEntry.Priority = NexusProvider.Priority.Highest
		'    logEntry.Severity = TraceEventType.Error
		'    logEntry.Message = sErrorMessage
		'    logEntry.ExtendedProperties = Nothing
		'    Logger.Write(logEntry)
		'End If

		Server.ClearError()
		'Redirect to error page
		If HttpContext.Current.Session Is Nothing Then
			'find session id from request url for cookieless session
			If (Request.UrlReferrer IsNot Nothing) Then
				strSessionId = Request.UrlReferrer.Segments(2)
				If strSessionId.Contains("(S(") Then 'cookieless session
					If bNestedError = False Then
						Response.Redirect(String.Format(AppSettings("WebRoot") & strSessionId & "/Error.aspx?aspxerrorpath={0}&ERef={1}", HttpUtility.UrlEncode(Request.RawUrl), sErrorRef), True)
					Else
						Response.Redirect(String.Format(AppSettings("WebRoot") & strSessionId & "/Error.aspx?aspxerrorpath={0}&ERef={1}&NestedError=1", HttpUtility.UrlEncode(Request.RawUrl), sErrorRef), True)
					End If

				Else
					If bNestedError = False Then
						Response.Redirect(String.Format(AppSettings("WebRoot") & "Error.aspx?aspxerrorpath={0}&ERef={1}", HttpUtility.UrlEncode(Request.RawUrl), sErrorRef), True)
					Else
						Response.Redirect(String.Format(AppSettings("WebRoot") & "Error.aspx?aspxerrorpath={0}&ERef={1}&NestedError=1", HttpUtility.UrlEncode(Request.RawUrl), sErrorRef), True)
					End If

				End If
			End If
		Else
			If HttpContext.Current.Session.IsCookieless Then
				If bNestedError = False Then
					Response.Redirect(String.Format(AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Error.aspx?aspxerrorpath={0}&ERef={1}", HttpUtility.UrlEncode(Request.RawUrl), sErrorRef), True)
				Else
					Response.Redirect(String.Format(AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Error.aspx?aspxerrorpath={0}&ERef={1}&NestedError=1", HttpUtility.UrlEncode(Request.RawUrl), sErrorRef), True)
				End If

			Else
				If bNestedError = False Then
					Response.Redirect(String.Format(AppSettings("WebRoot") & "Error.aspx?aspxerrorpath={0}&ERef={1}", HttpUtility.UrlEncode(Request.RawUrl), sErrorRef), True)
				Else
					Response.Redirect(String.Format(AppSettings("WebRoot") & "Error.aspx?aspxerrorpath={0}&ERef={1}&NestedError=1", HttpUtility.UrlEncode(Request.RawUrl), sErrorRef), True)
				End If

			End If
		End If

		If bNestedError = False Then
			Response.Redirect(String.Format("~/Error.aspx?aspxerrorpath={0}&ERef={1}", HttpUtility.UrlEncode(Request.RawUrl), sErrorRef), True)
		Else
			Response.Redirect(String.Format("~/Error.aspx?aspxerrorpath={0}&ERef={1}&NestedError=1", HttpUtility.UrlEncode(Request.RawUrl), sErrorRef), True)
		End If

	End Sub

	Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)

		If (Context.User.Identity.AuthenticationType = "Federation" OrElse Context.User.Identity.AuthenticationType = "AuthenticationTypes.Federation") Then
			Dim oFederationConfiguration As FederationConfiguration = FederatedAuthentication.FederationConfiguration
			Dim oFederatedAuthentication = FederatedAuthentication.WSFederationAuthenticationModule
			If HttpContext.Current.Request.Url.AbsoluteUri.ToUpper.Contains("SESSIONEXPIRED.ASPX") Then
				Exit Sub
			End If
		End If

		' To avoid Url rewriting for all the .axd Resources only when session's cookieless property is set to "UseUri" (means use session key in the URL)
		If HttpContext.Current.Session.IsCookieless AndAlso HttpContext.Current.Request.Url.AbsoluteUri.ToUpper.Contains(".AXD") Then
			Exit Sub
		End If

		Dim providerSection As IdentityProvider = CType(ConfigurationManager.GetSection("IdentityProvider"), IdentityProvider)
		Dim identityProvider = providerSection.DefaultIdentity.ToUpper
		If Not Equals(identityProvider, "SSO") AndAlso Not Equals(identityProvider, "KEYCLOAK") Then

			If Context.User.Identity.IsAuthenticated Then

				'in this case then we must get user details before we proceed
				'remove any session values left over (shouldn't be any really)
				HttpContext.Current.Session.Remove(CNAgentDetails)

				If Request.Cookies("SessionTimeout") Is Nothing OrElse String.IsNullOrEmpty(Request.Cookies("SessionTimeout").Value) Then
					Dim stCookie As HttpCookie = New HttpCookie("SessionTimeout", Session.SessionID + "|" + Date.Now)
					stCookie.HttpOnly = False
					Response.Cookies.Add(stCookie)
				Else
					If Request.Cookies("SessionTimeout").Value.Split("|")(0) = Session.SessionID Then
						Dim timeDifference = Date.Now - Convert.ToDateTime(Request.Cookies("SessionTimeout").Value.Split("|")(1).ToString)
						If timeDifference.Minutes > Context.Session.Timeout Then
							Request.Cookies("SessionTimeout").Expires = DateTime.Now.AddDays(-1)
							Response.Cookies.Add(Request.Cookies("SessionTimeout"))
						Else
							Request.Cookies("SessionTimeout").Value = Session.SessionID + "|" + Date.Now
							Response.Cookies.Add(Request.Cookies("SessionTimeout"))
						End If
					End If
				End If

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

					'Find that Enable Branch Selection At logon is enabled or not
					Dim bEnableBranchSelectionAtLogin As Boolean = False
					If CType(HttpContext.Current.Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches.Count > 1 Then
						Dim oOptionType As New NexusProvider.OptionTypeSetting
						oOptionType = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 37)
						If oOptionType.OptionValue = "1" Then
							bEnableBranchSelectionAtLogin = True
						End If
					End If


					If Request.UrlReferrer Is Nothing And CType(HttpContext.Current.Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches.Count > 1 And bEnableBranchSelectionAtLogin = True Then
						'if an agent has more than 1 branches and option set to force a choice to 
						'be made on login then go to select branch
						HttpContext.Current.Response.Redirect("~/SelectBranch.aspx")
					Else
						'set first branch as default
						HttpContext.Current.Session(CNBranchCode) = CType(HttpContext.Current.Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches(0).Code
						'redirect to start page
						Dim sAgentStartPage As String = CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AgentStartPage
						'HttpContext.Current.Response.Redirect(sAgentStartPage)
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
			End If
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

	Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
		' Code that runs when a session ends. 
		' Note: The Session_End event is raised only when the sessionstate mode
		' is set to InProc in the Web.config file. If session mode is set to StateServer 
		' or SQLServer, the event is not raised.

	End Sub

</script>