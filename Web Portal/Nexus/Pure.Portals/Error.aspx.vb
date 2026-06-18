Imports System.Data
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.HttpContext
Imports AjaxControlToolkit
Imports CMS.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Utils
Imports NexusProvider

Namespace Nexus

	Partial Class _Error : Inherits Frontend.clsCMSPage

		Private oException As System.Exception
		Private oExceptionN As NexusProvider.NexusException
		Dim oNexusConfig As Nexus.Library.Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork)
		Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

		''' <summary>
		''' 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
			If Request.QueryString("aspxerrorpath") IsNot Nothing AndAlso Request.QueryString("aspxerrorpath").Contains("modal=true") Then
				CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
			End If
		End Sub

		''' <summary>
		''' Show last server error
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
			If Current.Session(Nexus.Constants.CNLoginName) Is Nothing Then
				hdnIsAuthenticated.Value = "False"
			End If

			If Request.QueryString("aspxerrorpath") IsNot Nothing AndAlso Request.QueryString("aspxerrorpath").Contains("modal=true") Then
				'back button will close the current modal page
				btnBack.Attributes.Add("onclick", "self.parent.tb_remove();")
			End If

			'Show more detail section if debug=true
			Dim oCompilationSection As CompilationSection = CType(System.Web.Configuration.WebConfigurationManager.GetSection("system.web/compilation"), CompilationSection)
			If oCompilationSection.Debug Then
				pnlErrorDetailTitle.Visible = True
				pnlErrorDetailTitle.Visible = True
				pnlErrorDetail.Visible = True
			Else
				cpeErrorDetail.Enabled = False
				pnlErrorDetailTitle.Visible = False
				pnlErrorDetail.Visible = False
			End If

			'Show Email button as per portal config
			If oPortal.ShowEmailButtonForError = True Then
				btnEmailError.Visible = True
			End If

			If Page.IsPostBack Then Return
			Dim sErrorRef As String = Request.QueryString("ERef")
			'Dim sErrorRefN As String = Request.QueryString("ERefN")
			Dim sError As New StringBuilder

			'Create Exception object from application variable
			'And Remove application variable
			'oException = New Exception(HttpContext.Current.Application(sErrorRef))
			oException = HttpContext.Current.Application(sErrorRef)
			HttpContext.Current.Application.Remove(sErrorRef)

			If TypeOf (oException) Is NexusProvider.NexusException Then
				oExceptionN = CType(oException, NexusProvider.NexusException)
				If oExceptionN.Errors.Count = 1 Then
					If oExceptionN.Errors(0).NexusCode = ErrorCodes.InvalidData Or
						oExceptionN.Errors(0).NexusCode = ErrorCodes.DuplicateClaimExists Or
						oExceptionN.Errors(0).NexusCode = ErrorCodes.NoResultsFound Or
						oExceptionN.Errors(0).NexusCode = ErrorCodes.BusinessRule Then
						ShowErrorValidation(sError)
						ltError.Text = sError.ToString()
						ltErrorDetail.Text = ErrorFormatter.FormatErrorAsHtml(oExceptionN)
						Return
					End If
				End If
			Else
				ShowError(oCompilationSection, sError)
			End If

			ltError.Text = sError.ToString()
			If TryCast(oException, System.ServiceModel.FaultException(Of System.ServiceModel.ExceptionDetail)) IsNot Nothing Then
				ltErrorDetail.Text = ErrorFormatter.FormatErrorAsHtml(oException, DirectCast(oException, System.ServiceModel.FaultException(Of System.ServiceModel.ExceptionDetail)).Detail.StackTrace.ToString & vbCrLf & oException.StackTrace.ToString)
			Else
				If oException IsNot Nothing Then
					ltErrorDetail.Text = ErrorFormatter.FormatErrorAsHtml(oException)
				Else
					Dim exception As Exception = New Exception(HttpContext.Current.Application(sErrorRef))
					ltErrorDetail.Text = ErrorFormatter.FormatErrorAsHtml(exception)
				End If
			End If

		End Sub

		Protected Sub ShowErrorValidation(ByRef sError As StringBuilder)
			sError.Append("<b>Message :</b>" & oExceptionN.Message & "</br>")
			Dim sReferennceNumber As String = ""
			If oExceptionN.HelpLink IsNot Nothing AndAlso oExceptionN.HelpLink.Length > 0 Then
				sReferennceNumber = oExceptionN.HelpLink.Substring(oExceptionN.HelpLink.IndexOf(ERROR_LABEL) + ERROR_LABEL.Length, ERROR_NO_LENGTH)
				sError.Append("<b>Reference Number :</b>" & sReferennceNumber & "</br>")
			End If
			For Each oError As NexusError In oExceptionN.Errors
				If Not String.IsNullOrEmpty(oError.Code) Then
					If Not String.IsNullOrEmpty(GetLocalResourceObject(oError.Code)) Then
						'Pick error message from resource file and show to user
						sError.Append("<b>Error Detail : </b>" & GetLocalResourceObject(oError.Code))
					Else
						'all other not handled error codes will come here. We will show exact error returned from SAM 
						sError.Append("<b>Code :</b>" & oError.Code & "</br>")
						If Not String.IsNullOrEmpty(oError.NexusCode) Then
							sError.Append("<b>Nexus Code :</b>" & oError.NexusCode.ToString() & "</br>")
						End If
						sError.Append("<b>Description :</b>" & oError.Description & "</br>")
						sError.Append("<b>Detail :</b>" & oError.Detail & "</br>")
					End If
				Else
					'all other not handled error codes will come here. We will show exact error returned from SAM 
					If Not String.IsNullOrEmpty(oError.NexusCode) Then
						sError.Append("<b>Nexus Code :</b>" & oError.NexusCode.ToString() & "</br>")
					End If
					sError.Append("<b>Description :</b>" & oError.Description & "</br>")
					sError.Append("<b>Detail :</b>" & oError.Detail & "</br>")
				End If
			Next
			ltError.Text = sError.ToString()
			Return
		End Sub

		Protected Sub ShowError(ByRef oCompilationSection As CompilationSection, ByRef sError As StringBuilder)
			Dim sReferennceNumber As String = ""
			If Not oCompilationSection.Debug Then
				If oException IsNot Nothing Then
					sError.Append("<b>Message :</b>" & oException.Message & "</br>")
				Else
					Dim sErrorRef As String = Request.QueryString("ERef")
					sError.Append("<b>Message :</b>" & sErrorRef & "</br>")
				End If
			Else
				sError.Append("<b>Message :</b>An error has occurred. Please contact your system administrator quoting the reference number below.</br>")
			End If
			If oException IsNot Nothing AndAlso oException.HelpLink IsNot Nothing AndAlso oException.HelpLink.Length > 0 Then
				sReferennceNumber = oException.HelpLink.Substring(oException.HelpLink.IndexOf(ERROR_LABEL) + ERROR_LABEL.Length, ERROR_NO_LENGTH)
				sError.Append("<b>Reference Number :</b>" & sReferennceNumber & "</br>")
			End If
		End Sub

		''' <summary>
		''' It will clear all the process related sessions and will redirect an user to startup page
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
			If Request.QueryString("aspxerrorpath") IsNot Nothing AndAlso Request.QueryString("aspxerrorpath").Contains("Startup.cs") Then
				Dim strUserLoginPage As String = ConfigurationManager.AppSettings("webroot") + "logout.aspx"
				'Redirect a user to Login page
				Response.Redirect(strUserLoginPage, False)
			Else
				RedirectToStartupPage()
			End If
		End Sub

		''' <summary>
		''' TO Send formatted HTML Error as email
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub btnEmailError_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmailError.Click
			Dim strSupportEmailId As String = oPortal.SupportEmailId
			Dim bEmailResult As Boolean = True ' Default is failed
			If Not String.IsNullOrEmpty(strSupportEmailId) Then
				Dim dtEmailDetails As New DataTable
				Dim EmailTemplates As New Nexus.Library.Config.EmailTemplates
				EmailTemplates = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).EmailTemplates

				Dim sSenderAddress As String = String.Empty
				Dim sRecipient As String = strSupportEmailId
				Dim sTemplatePath As String = String.Empty

				dtEmailDetails.Columns.Add("ID")
				dtEmailDetails.Columns.Add("Code")
				dtEmailDetails.Columns.Add("Path")

				Dim drEmailDetails As DataRow
				For i As Integer = 0 To EmailTemplates.Count - 1
					drEmailDetails = dtEmailDetails.NewRow()
					With EmailTemplates.EmailTemplate(i)
						drEmailDetails(0) = .ID
						drEmailDetails(1) = .Path
						drEmailDetails(2) = .Sender
					End With

					If drEmailDetails(0) = "ApplicationError" Then
						sTemplatePath = drEmailDetails(1)
						sSenderAddress = drEmailDetails(2)
						Exit For
					End If
				Next

				'SET UP HASH TABLE FOR DATA THAT NEEDS TO BE COLLECTED FOR EMAIL
				Dim EmailDetails As New Hashtable
				EmailDetails.Add("[!PAGE_TITLE!]", "Nexus Error")
				EmailDetails.Add("[!ERROR_CONTENT!]", ltErrorDetail.Text) 'formatted Error content 


				'Clear policy sessions
				ClearQuoteCollectionSessionValues()
				ClearQuote()

				'Clear Claim sessions
				ClearCase()
				ClearClaims()


				'SEND EMAIL
				Try
					'SendEmail will return False if email send successfully
					bEmailResult = SendEmail(sSenderAddress, sRecipient, "Nexus Error", "", EmailDetails, sTemplatePath, Nothing, Nothing, Nothing, Nothing)
				Catch
					bEmailResult = True ' Email Sending Failed
					Server.ClearError()
				Finally
					Dim sMessage As String = ""
					Dim strUserStartPage As String = ""

					If bEmailResult = True Then
						sMessage = GetLocalResourceObject("lbl_EmailSendingError")
					Else
						sMessage = GetLocalResourceObject("lbl_SuccessEmailSending")
					End If

					'Get user startup page for logged in user
					If (Session(CNLoginType) = LoginType.Agent) Then
						strUserStartPage = CType(WebConfigurationManager.GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AgentStartPage
					Else
						strUserStartPage = CType(WebConfigurationManager.GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).ClientStartPage
					End If
					strUserStartPage = ResolveClientUrl(strUserStartPage)

					'Show message and redirect an user to startup page
					'Redirect to start page only if not sending email from modal page
					'In case of modal, user need to click on back button for closing the form.
					If Not Request.QueryString("aspxerrorpath").Contains("modal=true") Then
						ToolkitScriptManager.RegisterStartupScript(Me, Page.GetType(), "emailsending", "alert('" + sMessage + "');window.location.href('" + strUserStartPage + "'); ", True)
					Else
						ToolkitScriptManager.RegisterStartupScript(Me, Page.GetType(), "emailsending", "alert('" + sMessage + "');", True)
					End If
				End Try
			Else
				Dim sSupportEmailIdNotConfigured As String = GetLocalResourceObject("lbl_SupportEmailIdNotConfigured")
				ToolkitScriptManager.RegisterStartupScript(Me, Page.GetType(), "emailsending", "alert('" + sSupportEmailIdNotConfigured + "');", True)
			End If

		End Sub

		''' <summary>
		''' Clear the sessions and redirect an user to startup page
		''' </summary>
		''' <remarks></remarks>
		Private Sub RedirectToStartupPage()
			Dim strUserStartPage As String = ""
			'Get user startup page for logged in user
			If (Session(CNLoginType) = LoginType.Agent) Then
				strUserStartPage = CType(WebConfigurationManager.GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AgentStartPage
			Else
				strUserStartPage = CType(WebConfigurationManager.GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).ClientStartPage
			End If

			'Clear policy sessions
			ClearQuoteCollectionSessionValues()
			ClearQuote()

			'Clear Claim sessions
			ClearCase()
			ClearClaims()

			'Redirect a user to startup page
			Response.Redirect(strUserStartPage, False)
		End Sub
	End Class

End Namespace

