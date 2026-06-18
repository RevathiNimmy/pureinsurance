Imports CMS.Library.Frontend
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants.Session
Imports NexusProvider
Imports Nexus.Constants.Constant
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports System.Security.Cryptography
Imports System.Text
Imports Nexus.Library.Config

Namespace Nexus
	Partial Class MasterPages_Internal_main : Inherits CMSMasterPage
		Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
			'This will expire the webpage from cache on browser back button. 
			HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
			HttpContext.Current.Response.Cache.SetNoServerCaching()
			HttpContext.Current.Response.Cache.SetNoStore()
		End Sub

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

			'This will expire the webpage from cache on browser back button. 
			HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
			HttpContext.Current.Response.Cache.SetNoServerCaching()
			HttpContext.Current.Response.Cache.SetNoStore()

			' Script initializer call to handle the busy indicator.
			If (Not Page.ClientScript.IsOnSubmitStatementRegistered(Me.GetType(), "OnSubmitScript")) Then
				Page.ClientScript.RegisterOnSubmitStatement(Me.GetType(), "OnSubmitScript", "return beforeSubmit();")
			End If

			'This will register a function for showing updatepanel errors as alert 
			'and also for showing busy progress(full screen) during postback from update panels
			If ScriptManager.GetCurrent(Me.Page) IsNot Nothing Then
				If Not (Page.ClientScript.IsStartupScriptRegistered("EndRequestHandlerForUpdatePanel")) Then
					Page.ClientScript.RegisterStartupScript(Me.GetType(), "EndRequestHandlerForUpdatePanel", "Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerForUpdatePanel);", True)
				End If
				If Not (Page.ClientScript.IsStartupScriptRegistered("StartRequestHandlerForUpdatePanel")) Then
					Page.ClientScript.RegisterStartupScript(Me.GetType(), "StartRequestHandlerForUpdatePanel", "Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandlerForUpdatePanel);", True)
				End If
			End If

			If Not Page.IsPostBack Then
				Page.ClientScript.RegisterStartupScript(Me.GetType(), "clearJQueryCookies", "clearJQueryCookies();", True)
			End If

			Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), UserDetails)
			If oUserDetails IsNot Nothing Then
				' Added to override defualt portal id as per user type       
				If oUserDetails IsNot Nothing Then
					' Added to override defualt portal id as per user type       
					If oUserDetails.Key = 0 Then
						'set PortalID for internal portal & app_themes
						Session("PortalID") = 1
					Else
						If oUserDetails.PartyType = "AG" Then
							Session("PortalID") = 2
						Else
							Session("PortalID") = 5 ' If user is of Third Party Type 
						End If
					End If
					HttpContext.Current.Cache.Insert("PortalID", Session("PortalID"), Nothing, Now.AddHours(4), TimeSpan.Zero)
				End If

				'If Session portalid set first time and agent has more than 1 branches then need to reload page to set themes
				Dim sAgentStartPage As String = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AgentStartPage
				If Request.UrlReferrer Is Nothing AndAlso Session("HasThemesSet") <> 1 Then
					If Request.CurrentExecutionFilePath.ToUpper.Contains("/SELECTBRANCH.ASPX") Then
						Session("HasThemesSet") = 1
						Response.Redirect("~/SelectBranch.aspx", False)
					Else
						Session("HasThemesSet") = 1
						Response.Redirect(sAgentStartPage, False)
					End If
				End If

				If Not Request.CurrentExecutionFilePath.ToUpper.Contains("/SELECTBRANCH.ASPX") Then
					If Session(CNBranchCode) Is Nothing Then
						Response.Redirect("~/SelectBranch.aspx", False)
					End If
				End If

				If Session(CNQuoteMode) = QuoteMode.QuickQuote Then
					Dim btnFinish As Button = Nothing
					btnFinish = Page.Master.FindControl("cntMainBody").FindControl("btnFinish")
					If btnFinish IsNot Nothing Then
						btnFinish.Visible = False
					End If
				End If

				If (Page.Request.QueryString("PostBack") IsNot Nothing AndAlso Page.Request.QueryString("PostBack") = "RibbonMenu" AndAlso
					((Page.Request.QueryString("Mode") Is Nothing) OrElse (Page.Request.QueryString("Mode") IsNot Nothing _
					AndAlso Page.Request.QueryString("Mode") <> "ManualTransfer"))) Then
					If Session(CNNoTrans) IsNot Nothing Then
						Session.Remove(CNNoTrans)
					End If
				End If
			End If

			If ((Request.QueryString("ExternalWrkMgrMsg") IsNot Nothing) AndAlso (Not Page.ClientScript.IsStartupScriptRegistered(Page.GetType(), "StartupScriptsExternalWrkMgrMsg"))) Then
				Dim sJavascript As String
				Dim sExternalWrkMgrMsg As String = Request.QueryString("ExternalWrkMgrMsg")
				sJavascript = "$(window).on('load', function() {alert('" & sExternalWrkMgrMsg & "');});"
				Page.ClientScript.RegisterStartupScript(GetType(String), "StartupScriptsExternalWrkMgrMsg", sJavascript, True)
			End If
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
		Public Sub RegisterAndPublishMsg(ByVal sOrginatingSource As String, ByVal sMode As String, ByVal sPureKey As String)
			Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
			Dim oEnableExternalWorkFlowSystem As NexusProvider.OptionTypeSetting = Nothing
			oEnableExternalWorkFlowSystem = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 110)
			If (oEnableExternalWorkFlowSystem IsNot Nothing AndAlso oEnableExternalWorkFlowSystem.OptionValue = "1") Then
				Dim sJavascript As String
				sJavascript = "window.onload = function(){publishMessageToE5('" & sOrginatingSource & "','" & sPureKey & "','" & sMode & "');};"
				'sJavascript = "publishMessageToE5('" & sOrginatingSource & "','" & sPureKey & "','" & sMode & "');"
				Page.ClientScript.RegisterStartupScript(GetType(String), "StartupScriptsPublishMsg", sJavascript, True)
			End If
		End Sub

		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
			'Uncomment line below in order to display debug information
			' Controls.Add(New LiteralControl(NexusProvider.ErrorFormatter.GetSessionAsHtml()))
			'If Current.User.Identity.IsAuthenticated Then
			'    Select Case Session.Item(CNLoginType)
			'        Case LoginType.Agent
			'            With CType(Session.Item(CNAgentDetails), NexusProvider.UserDetails)
			'                If Not IsWindowsAuthentication() Then
			'                    lbtnChangePassword.Visible = True
			'                Else
			'                    lbtnChangePassword.Visible = False
			'                End If
			'            End With
			'    End Select
			'End If
		End Sub

		Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
			hdnIsAuthenticated.Value = HttpContext.Current.User.Identity.IsAuthenticated
			Dim sGuid As String
			sGuid = System.Guid.NewGuid.ToString()
			hdnGuid.Value = sGuid

			If Session(CNParty) IsNot Nothing AndAlso Request.CurrentExecutionFilePath.ToUpper.Contains("CLIENTDETAILS") _
				Then
				If (Session(CNClientMode) IsNot Nothing) Then
					Select Case CType(Session(CNClientMode), Mode)
						Case Mode.Add
							hdnMode.Value = "NEW"
						Case Mode.Edit
							hdnMode.Value = "EDIT"
						Case Mode.View
							hdnMode.Value = "VIEW"
					End Select
				End If
				hdnPureOrginatingSource.Value = "PARTY"
			Else
				If (Session(CNClaim) IsNot Nothing) Then
					If (Session(CNMode) IsNot Nothing) Then
						Select Case CType(Session(CNMode), Mode)
							Case Mode.ViewClaim
								hdnMode.Value = "VIEW"
							Case Mode.EditClaim
								hdnMode.Value = "EDIT"
							Case Mode.PayClaim
								hdnMode.Value = "PAY"
							Case Mode.NewClaim
								hdnMode.Value = "NEW"
						End Select
					End If
					hdnPureOrginatingSource.Value = "CLAIM"
				End If
				If (Session(CNQuote) IsNot Nothing) Then
					If (Session(CNMode) IsNot Nothing) Then
						Select Case CType(Session(CNMode), Mode)
							Case Mode.Add
								hdnMode.Value = "NEW"
							Case Mode.Edit, Mode.Buy
								hdnMode.Value = "EDIT"
							Case Mode.View, Mode.Review
								hdnMode.Value = "VIEW"
						End Select
					Else
						hdnMode.Value = "NEW"
					End If
					hdnPureOrginatingSource.Value = "POLICY"
				End If
			End If

			'Pure send message to E5
			If (Session("PureKey") Is Nothing OrElse Session("OpenMode") Is Nothing) Then
				Session("PureKey") = ""
				Session("OpenMode") = ""
			End If

			' policy mode for E5
			Dim sPolicyMode As String = "QUOTE"
			If Session(CNMTAType) IsNot Nothing And Session(CNRenewal) Is Nothing Then
				Select Case CType(Session(CNMTAType), Mode)
					Case MTAType.PERMANENT, MTAType.TEMPORARY
						sPolicyMode = "MTA"
					Case MTAType.CANCELLATION
						sPolicyMode = "MTC"
					Case MTAType.REINSTATEMENT
						sPolicyMode = "MTR"
				End Select
				Session("OpenMode") = ""
			ElseIf Session(CNMTAType) Is Nothing And Session(CNRenewal) IsNot Nothing Then
				sPolicyMode = "REN"
			Else
				If (Session(CNQuote) IsNot Nothing) Then
					If (Session(CNMode) IsNot Nothing) Then
						Select Case CType(Session(CNMode), Mode)
							Case Mode.Add, Mode.Edit, Mode.Buy
								sPolicyMode = "QUOTE"
							Case Mode.View, Mode.Review
								sPolicyMode = "VIEW"
						End Select
					End If
				End If
			End If

			' claim mode for E5
			Dim sClaimMode As String = "VIEW"
			If (Session(CNClaim) IsNot Nothing) Then
				If (Session(CNMode) IsNot Nothing) Then
					Select Case CType(Session(CNMode), Mode)
						Case Mode.ViewClaim
							sClaimMode = "VIEW"
						Case Mode.EditClaim
							sClaimMode = "MAINTAIN"
						Case Mode.PayClaim
							sClaimMode = "PAYMENT"
						Case Mode.NewClaim
							sClaimMode = "OPEN"
					End Select
				End If
			End If

			Dim sPartyCode As String = String.Empty
			If Request.QueryString("Code") IsNot Nothing Then
				sPartyCode = Request.QueryString("Code").Trim()
			End If

			If _
				(Session(CNQuote) IsNot Nothing AndAlso
				 (Session("PureKey").ToString() <> DirectCast(Session(CNQuote), NexusProvider.Quote).InsuranceFileRef OrElse
				  Session("OpenMode").ToString() <> sPolicyMode)) Then
				RegisterAndPublishMsg("POLICY", sPolicyMode,
									  DirectCast(Session(CNQuote), NexusProvider.Quote).InsuranceFileRef)
				Session("PureKey") = DirectCast(Session(CNQuote), NexusProvider.Quote).InsuranceFileRef
				Session("OpenMode") = ""
			ElseIf _
				(Session(CNParty) IsNot Nothing AndAlso sPartyCode.Length > 0 AndAlso
				 (Session("PureKey").ToString() <> sPartyCode OrElse Session("OpenMode").ToString() <> hdnMode.Value)) _
				Then
				RegisterAndPublishMsg("PARTY", hdnMode.Value, Request.QueryString("Code").Trim())
				Session("PureKey") = sPartyCode
				Session("OpenMode") = hdnMode.Value
			ElseIf _
				 (Session(CNClaim) IsNot Nothing AndAlso
				 (Session("PureKey").ToString() <> DirectCast(Session(CNClaim), NexusProvider.ClaimOpen).ClaimNumber OrElse
				  Session("OpenMode").ToString() <> sClaimMode) AndAlso (Session(CNReturnURL) Is Nothing OrElse (Session(CNReturnURL) IsNot Nothing AndAlso
				  Not Session(CNReturnURL).ToString().ToUpper.Contains("/CLAIMS/CLIENTPAGES/")))) Then
				RefreshE5WorkingPanel()
				RegisterAndPublishMsg("CLAIM", sClaimMode,
									  DirectCast(Session(CNClaim), NexusProvider.ClaimOpen).ClaimNumber)
				Session("PureKey") = DirectCast(Session(CNClaim), NexusProvider.ClaimOpen).ClaimNumber
				Session("OpenMode") = sClaimMode
			ElseIf _
				(Request.QueryString("Claimno") IsNot Nothing AndAlso Request.CurrentExecutionFilePath.ToUpper.Contains("/CLAIMS/FINDCLAIM.ASPX")) Then
				RefreshE5WorkingPanel()
				RegisterAndPublishMsg("CLAIM", sClaimMode, Request.QueryString("Claimno"))
				Session("PureKey") = Request.QueryString("Claimno")
				Session("OpenMode") = sClaimMode
			Else
				If Session(CNClaim) Is Nothing Then
					RegisterAndPublishMsg("CLAIM", "VIEW", "-1")
					Session("PureKey") = "-1"
					Session("OpenMode") = "VIEW"
				Else
					RefreshE5WorkingPanel()
				End If
			End If

			If (Not Request.CurrentExecutionFilePath.ToUpper.Contains("/CLAIMS/COMPLETE.ASPX")) Then
				If (Session("PostID") IsNot Nothing) Then
					Session.Remove("PostID")
				End If
			End If

			hdnPureRef.Value = Session("PureKey").ToString()

			Dim validatorOverrideScripts As String = ("<script src='" & (ResolveClientUrl("~/App_Themes/Internal/js/validators.js") & "' type='text/javascript'></script>"))
			Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "ValidatorOverrideScripts", validatorOverrideScripts, False)
			MyBase.Render(writer)
		End Sub

		''' <summary>
		''' this method is used to refresh E5 Zone 3 for other pure tasks and pass -1 instead of the claim number
		''' </summary>        
		''' <remarks></remarks>				
		Public Sub RefreshE5WorkingPanel()
			If Request.CurrentExecutionFilePath.ToUpper.Contains("/SECURE/AGENT/PERSONALCLIENTDETAILS.ASPX") OrElse
			   Request.CurrentExecutionFilePath.ToUpper.Contains("/SECURE/AGENT/CORPORATECLIENTDETAILS.ASPX") OrElse
			   Request.CurrentExecutionFilePath.ToUpper.Contains("/SECURE/FINDFILES.ASPX") OrElse
			   Request.CurrentExecutionFilePath.ToUpper.Contains("/SECURE/SEARCHTRANSACTIONS.ASPX") OrElse
			   Request.CurrentExecutionFilePath.ToUpper.Contains("/SECURE/REPORTS.ASPX") Then
				RegisterAndPublishMsg("CLAIM", "VIEW", "-1")
				Session("PureKey") = "-1"
				Session("OpenMode") = "VIEW"
			End If
		End Sub


	End Class
End Namespace
