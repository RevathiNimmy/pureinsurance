Imports CMS.Library.Frontend
Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports System.Web.Configuration
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Security.Cryptography
Imports Nexus.Library.Config


Namespace Nexus
	Partial Class MasterPages_External_main : Inherits CMSMasterPage
		Dim RestrictedClientdetailbox1URLs, RestrictedClientdetailbox2URLs, RestrictedPolicyRefCleanURL As String

		Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
			Response.Cache.SetCacheability(HttpCacheability.NoCache)
		End Sub
		Public Function GenerateHashFileName(fileName As String) As String
			Dim returnHashString As String
			' Use SHA256 to hash the filename
			Using sha256 As SHA256 = SHA256.Create()
				Dim sourceBytes As Byte() = Encoding.UTF8.GetBytes(fileName)
				Dim hashBytes As Byte() = sha256.ComputeHash(sourceBytes)
				Dim sb As New StringBuilder()
				For Each b As Byte In hashBytes
					sb.Append(b.ToString("x2")) ' Convert to hex
				Next
				returnHashString = sb.ToString().Substring(0, 8) ' Return hashed filename
				Return returnHashString

			End Using
		End Function
		Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
			Dim originalFileName As String
			Dim hashedFileName As String

			originalFileName = "jquery.min.js"

			' Get hashed filename
			hashedFileName = String.Format("~/js/libs/jquery/{0}", originalFileName)
			' Dynamically reference the hashed file in the page
			ltrScriptTag.Text = "<script src='" + ResolveClientUrl(hashedFileName) + "' type='text/javascript'></script>"
			originalFileName = "bootstrap.min.js"

			hashedFileName = String.Format("~/js/libs/bootstrap/{0}", originalFileName)
			' reference the new name of the file 
			ltrBootstrapTag.Text = "<script src='" + ResolveClientUrl(hashedFileName) + "' type='text/javascript'></script>"

			Page.Header.DataBind()

			Dim providerSection As IdentityProvider = CType(ConfigurationManager.GetSection("IdentityProvider"), IdentityProvider)
			Dim identityProvider = providerSection.DefaultIdentity.ToUpper
			If Not Equals(identityProvider, "SSO") AndAlso Not Equals(identityProvider, "KEYCLOAK") Then
				If Request.Cookies("SessionTimeout") Is Nothing Then
					Response.Redirect("~/logout.aspx")
				End If
				Try
					If (Request.Cookies("SessionTimeout").Value IsNot Nothing AndAlso Request.Cookies("SessionTimeout").Value <> "") Then

						Dim startTime As DateTime
						Dim endTime As DateTime
						If DateTime.TryParse(Date.Now.ToString, startTime) AndAlso DateTime.TryParse(Request.Cookies("SessionTimeout").Value.Split("|")(1).ToString, endTime) Then

							Dim timeDifference = startTime - endTime
							If timeDifference.Minutes <= Context.Session.Timeout Then
								Request.Cookies("SessionTimeout").Value = Session.SessionID + "|" + Date.Now
								Response.Cookies.Add(Request.Cookies("SessionTimeout"))
								Dim requireSsl As Boolean = HttpContext.Current.Response.Cookies("SessionTimeout").Secure
								If (requireSsl) Then
									HttpContext.Current.Response.Cookies("SessionTimeout").Secure = True
									HttpContext.Current.Response.Cookies("SessionTimeout").HttpOnly = True
								Else
									HttpContext.Current.Response.Cookies("SessionTimeout").Secure = False
									HttpContext.Current.Response.Cookies("SessionTimeout").HttpOnly = False
								End If

							Else
								Request.Cookies("SessionTimeout").Expires = DateTime.Now.AddDays(-1)
								Response.Cookies.Add(Request.Cookies("SessionTimeout"))
								Dim requireSsl As Boolean = HttpContext.Current.Response.Cookies("SessionTimeout").Secure
								If (requireSsl) Then
									HttpContext.Current.Response.Cookies("SessionTimeout").Secure = True
									HttpContext.Current.Response.Cookies("SessionTimeout").HttpOnly = True
								Else
									HttpContext.Current.Response.Cookies("SessionTimeout").Secure = False
									HttpContext.Current.Response.Cookies("SessionTimeout").HttpOnly = False
								End If
								Response.Redirect("~/logout.aspx")
							End If
						Else
							Dim requireSsl As Boolean = HttpContext.Current.Response.Cookies("SessionTimeout").Secure
							If (requireSsl) Then
								HttpContext.Current.Response.Cookies("SessionTimeout").Secure = True
								HttpContext.Current.Response.Cookies("SessionTimeout").HttpOnly = True
							Else
								HttpContext.Current.Response.Cookies("SessionTimeout").Secure = False
								HttpContext.Current.Response.Cookies("SessionTimeout").HttpOnly = False
							End If
							Response.Cookies.Add(Request.Cookies("SessionTimeout"))
							HttpContext.Current.Response.Cookies("SessionTimeout").Secure = True
							HttpContext.Current.Response.Cookies("SessionTimeout").HttpOnly = True

						End If


					End If
				Catch ex As Exception
					Throw ex
				End Try
			End If

			'This will register a function for showing updatepanel errors as alert
			' Script initializer call to handle the busy indicator.
			If (Not Page.ClientScript.IsOnSubmitStatementRegistered(Me.GetType(), "OnSubmitScript")) Then
				Page.ClientScript.RegisterOnSubmitStatement(Me.GetType(), "OnSubmitScript", "return beforeSubmit();")
			End If

			If ScriptManager.GetCurrent(Me.Page) IsNot Nothing Then
				If Not (Page.ClientScript.IsStartupScriptRegistered("EndRequestHandlerForUpdatePanel")) Then
					Page.ClientScript.RegisterStartupScript(Me.GetType(), "EndRequestHandlerForUpdatePanel", "Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerForUpdatePanel);", True)
				End If
				If Not (Page.ClientScript.IsStartupScriptRegistered("BeginRequestHandlerForUpdatePanel")) Then
					Page.ClientScript.RegisterStartupScript(Me.GetType(), "BeginRequestHandlerForUpdatePanel", "Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandlerForUpdatePanel);", True)
				End If
			End If
			Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
			If oUserDetails IsNot Nothing Then
				' Added to override defualt portal id as per user type       
				If oUserDetails.Key = 0 Then
					'set PortalID for internal portal & app_themes
					Session("PortalID") = 1
				Else
					If oUserDetails.PartyType = "AG" Then
						Session("PortalID") = 2
					Else
						Session("PortalID") = 3 ' If user is of Third Party Type 
					End If
				End If
				HttpContext.Current.Cache.Insert("PortalID", Session("PortalID"), Nothing, Now.AddHours(4), TimeSpan.Zero)
			End If

			'If Session portalid set first time and agent has more than 1 branches then need to reload page to set themes
			Dim sLoginPageURLs As String = "login.aspx,default.aspx"
			Dim sAgentStartPage As String = CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AgentStartPage
			If Request.UrlReferrer IsNot Nothing AndAlso sLoginPageURLs.ToUpper().Contains(Request.UrlReferrer.Segments(Request.UrlReferrer.Segments.Length - 1).ToString.ToUpper) And Session("HasThemesSet") <> 1 Then
				If Request.CurrentExecutionFilePath.ToUpper.Contains("/SELECTBRANCH.ASPX") Then
					Session("HasThemesSet") = 1
					Response.Redirect("~/SelectBranch.aspx", False)
				Else
					Session("HasThemesSet") = 1
					Response.Redirect(sAgentStartPage, False)
				End If
			End If

			If oUserDetails IsNot Nothing Then
				If Not Request.CurrentExecutionFilePath.ToUpper.Contains("/SELECTBRANCH.ASPX") Then
					If Session(CNBranchCode) Is Nothing Then
						Response.Redirect("~/SelectBranch.aspx", False)
					End If
				End If
			End If
		End Sub



		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
			'Remove comment below for debugging
			'Controls.Add(New LiteralControl(NexusProvider.ErrorFormatter.GetSessionAsHtml()))
		End Sub

		Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
			Dim validatorOverrideScripts As String = ("<script src='" & (ResolveClientUrl("~/App_Themes/External/js/validators.js") & "' type='text/javascript'></script>"))
			Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "ValidatorOverrideScripts", validatorOverrideScripts, False)
			MyBase.Render(writer)
		End Sub



	End Class
End Namespace

