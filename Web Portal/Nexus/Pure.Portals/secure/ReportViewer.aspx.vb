Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Microsoft.Reporting.WebForms
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports Nexus.Library.ReportParameterDataSets
Imports Nexus.Utils
Imports LocalReport = Microsoft.Reporting.WebForms.LocalReport
Imports ReportDataSource = Microsoft.Reporting.WebForms.ReportDataSource
Imports ReportParameterInfo = Microsoft.Reporting.WebForms.ReportParameterInfo

Partial Class secure_ReportViewer
	Inherits System.Web.UI.Page
	Dim sFolder As String
	Dim sFileName As String
	Dim sReportName As String
	Dim sReportDirName As String
	Dim oWebService As NexusProvider.ProviderBase
	Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
		If Not IsPostBack Then
			LoadReport()
			'LoadReportAtServer()
		End If
	End Sub
	Private Sub LoadReport()
		'set the properties for the reportsource control and then bind the viewer control to it
		sReportDirName = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Reports.Location
		sFileName = Request.QueryString("reportfile")
		sReportName = Request.QueryString("reportname")
		sReportName = HttpUtility.UrlDecode(sReportName)
		oWebService = New NexusProvider.ProviderManager().Provider
		Dim sDocumentExtractionDirectory As String = Nothing
		Dim url As String = String.Empty
		sFolder = sReportName.Split("\")(0) + "\"
		'set the extraction directory using a guid to ensure it is unique
		sReportDirName = sReportDirName & "\"
		viewerReportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local
		'viewerReportViewer.CssClass = "report-container"
		Dim localReport As New LocalReport
		localReport = viewerReportViewer.LocalReport
		localReport.ReportPath = sReportDirName + sReportName + ".rdl"
		Dim oParametersCollection As New NexusProvider.ParametersCollection
		oParametersCollection = Session("Parameters")
		Dim oReportParameterDataSet As New Nexus.Library.ReportParameterDataSets.ReportDataSets
		Dim iCount As Integer = 0
		Dim oParameter(oParametersCollection.Count - 1, 1) As Object
		For Each param As NexusProvider.Parameters In oParametersCollection
			oParameter(iCount, 0) = param.ParamNameField
			oParameter(iCount, 1) = param.ParamValueField
			iCount = iCount + 1
		Next

		oReportParameterDataSet = GetReportDataSet(sReportDirName, sReportName)

		Dim localReportParameter As New List(Of Microsoft.Reporting.WebForms.ReportParameter)
		Dim dsReport As System.Data.DataSet
		Dim orgReportParameter = localReport.GetParameters()
		Dim addBranchInLocalParameter As Boolean = orgReportParameter.Any(Function(p) p.Name.ToLower() = "branch")
		dsReport = GetReportData(oReportParameterDataSet.ReportDataSet(0), oParametersCollection, localReportParameter, addBranchInLocalParameter, orgReportParameter)
		localReport.DataSources.Clear()
		Dim reportDS As New ReportDataSource
		reportDS.Name = "DataSet1"
		reportDS.Value = dsReport.Tables(0)
		localReport.DataSources.Add(reportDS)
		AddHandler localReport.SubreportProcessing, AddressOf Me.SubreportProcessingEventHandler

		localReport.SetParameters(localReportParameter)
		viewerReportViewer.AsyncRendering = True
		HttpContext.Current.Session("KeepAlive") = DateTime.Now
		Server.ScriptTimeout = 1800
		viewerReportViewer.LocalReport.Refresh()
		viewerReportViewer.ZoomMode = Microsoft.Reporting.WebForms.ZoomMode.PageWidth
	End Sub

	Private Sub LoadReportAtServer()
		sFileName = Request.QueryString("reportfile")
		sReportName = Request.QueryString("reportname")
		sReportName = HttpUtility.UrlDecode(sReportName)

		Dim oParametersCollection As New NexusProvider.ParametersCollection
		oParametersCollection = Session("Parameters")

		viewerReportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote
		viewerReportViewer.CssClass = "report-container"
		viewerReportViewer.AsyncRendering = True
		viewerReportViewer.ZoomMode = Microsoft.Reporting.WebForms.ZoomMode.Percent
		viewerReportViewer.ShowParameterPrompts = False

		Dim reportServerURL As String = ConfigurationManager.AppSettings("ReportServerURL")
		viewerReportViewer.ServerReport.ReportServerUrl = New Uri(reportServerURL)
		viewerReportViewer.ServerReport.ReportPath = "/" & sReportName.Replace("\", "/")

		Dim orgReportParameter = viewerReportViewer.ServerReport.GetParameters()
		Dim localReportParameter As List(Of ReportParameter)
		localReportParameter = GetServerReportData(oParametersCollection, orgReportParameter)
		viewerReportViewer.ServerReport.SetParameters(localReportParameter)
		viewerReportViewer.ServerReport.Refresh()
	End Sub
	Private Function GetServerReportData(ByRef reportParameters As NexusProvider.ParametersCollection, ByVal orgReportParameters As ReportParameterInfoCollection) As List(Of ReportParameter)

		Dim addBranchInLocalParameter As Boolean = orgReportParameters.Any(Function(p) p.Name.ToLower() = "branch")
		Dim localReportParameter As List(Of ReportParameter) = New List(Of ReportParameter)()

		Dim operatorParamExists As Boolean = orgReportParameters.Any(Function(p) p.Name = "Operator")
		If operatorParamExists Then
			localReportParameter.Add(New Microsoft.Reporting.WebForms.ReportParameter("Operator", Session(Nexus.Constants.CNLoginName).ToString()))
		End If

		Dim i As Integer = 0
		Dim branchId As String = 0
		For Each param As NexusProvider.Parameters In reportParameters
			If (Not Equals(param.ParamNameField.ToString().ToLower(), "branch") Or addBranchInLocalParameter) Then
				Dim key As String = param.ParamNameField.ToString()
				Dim value As String = String.Empty
				If String.IsNullOrEmpty(param.ParamValueField) OrElse param.ParamValueField Is Nothing Then
					If (key = "TPACode") Then
						value = "null"
					Else
						value = 0
					End If
				Else
					value = param.ParamValueField.ToString()
				End If
				If key.ToLower = "branch_id" Then
					key = "branch_id"
					If value.ToLower() = "all" Then
						value = 0
						branchId = "All"
					Else
						branchId = value
					End If
				End If
				For Each p As ReportParameterInfo In orgReportParameters
					If p.Name.ToLower() = key.ToLower() Then
						localReportParameter.Add(New Microsoft.Reporting.WebForms.ReportParameter(p.Name, value))

						If addBranchInLocalParameter AndAlso key.ToLower = "branch_id" AndAlso branchId = "All" Then
							localReportParameter.Add(New ReportParameter("Branch", "All"))
						ElseIf addBranchInLocalParameter AndAlso key.ToLower = "branch_id" Then
							Dim oBranchs As NexusProvider.BranchCollection = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
							If oBranchs IsNot Nothing Then
								For Each oBranch As NexusProvider.Branch In oBranchs
									If oBranch.BranchKey.ToString = branchId Then
										localReportParameter.Add(New ReportParameter("Branch", oBranch.Description))
										Exit For
									End If
								Next
							End If
						End If

						Exit For
					End If
				Next
			End If
		Next
		Return localReportParameter
	End Function

	'Private Function MapReportParameters(ByVal ReportParameter As List(Of Microsoft.Reporting.WebForms.ReportParameter), ByVal localReport As LocalReport) As List(Of Microsoft.Reporting.WebForms.ReportParameter)
	'    Dim localReportParameter As ReportParameterInfoCollection
	'    localReportParameter = localReport.GetParameters()
	'    For Each param As Microsoft.Reporting.WebForms.ReportParameter In localReportParameter

	'    Next


	'    Dim destinationProperties As PropertyInfo() = destination.[GetType]().GetProperties()
	'    For Each destinationPi In destinationProperties
	'        Dim sourcePi As PropertyInfo = source?.[GetType]()?.GetProperty(destinationPi.Name)
	'        destinationPi.SetValue(destination, sourcePi?.GetValue(source, Nothing), Nothing)
	'    Next
	'End Function
	Protected Sub viewerReportViewer_ReportRefresh(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles viewerReportViewer.ReportRefresh
		LoadReport()
		'LoadReportAtServer()
	End Sub
	Public Function GetReportData(ByRef reportDataSet As ReportDataSet, ByRef reportParameters As NexusProvider.ParametersCollection, ByRef localReportParameter As List(Of Microsoft.Reporting.WebForms.ReportParameter), ByVal addBranchInLocalParameter As Boolean, ByVal orgReportParameters As ReportParameterInfoCollection) As Data.DataSet
		Dim dsReport As Data.DataSet = New Data.DataSet()
		Dim storedProcedureName As String = reportDataSet.SqlCommandText
		Dim sqlCommandType As String = reportDataSet.SqlCommandType
		Dim queryParameters = New Dictionary(Of String, Object)()
		localReportParameter = New List(Of Microsoft.Reporting.WebForms.ReportParameter)()

		Dim operatorParamExists As Boolean = orgReportParameters.Any(Function(p) p.Name = "Operator")
		If operatorParamExists Then
			localReportParameter.Add(New Microsoft.Reporting.WebForms.ReportParameter("Operator", Session(Nexus.Constants.CNLoginName).ToString()))
		End If

		Dim oQueryParameterCollection As New NexusProvider.ParametersCollection
		Dim i As Integer = 0
		Dim branchId As String = 0
		For Each param As NexusProvider.Parameters In reportParameters
			If (Not Equals(param.ParamNameField.ToString().ToLower(), "branch") Or addBranchInLocalParameter) Then
				Dim key As String = param.ParamNameField.ToString()
				Dim value As String = String.Empty
				If String.IsNullOrEmpty(param.ParamValueField) OrElse param.ParamValueField Is Nothing Then
					If (key = "TPACode") Then
						value = "null"
					Else
						value = 0
					End If
				Else
					value = param.ParamValueField.ToString()
				End If
				If key.ToLower = "branch_id" Then
					key = "branch_id"
					If value.ToLower() = "all" Then
						value = 0
						branchId = "All"
					Else
						branchId = value
					End If
				End If
				For Each p As ReportParameterInfo In orgReportParameters
					If p.Name.ToLower() = key.ToLower() Then
						localReportParameter.Add(New Microsoft.Reporting.WebForms.ReportParameter(p.Name, value))

						If addBranchInLocalParameter AndAlso key.ToLower = "branch_id" AndAlso branchId = "All" Then
							localReportParameter.Add(New ReportParameter("Branch", "All"))
						ElseIf addBranchInLocalParameter AndAlso key.ToLower = "branch_id" Then
							Dim oBranchs As NexusProvider.BranchCollection = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
							If oBranchs IsNot Nothing Then
								For Each oBranch As NexusProvider.Branch In oBranchs
									If oBranch.BranchKey.ToString = branchId Then
										localReportParameter.Add(New ReportParameter("Branch", oBranch.Description))
										Exit For
									End If
								Next
							End If
						End If

						Exit For
					End If
				Next
			End If

			If (reportDataSet.ReportQueryParameters IsNot Nothing) Then

				For Each keyValue As KeyValuePair(Of String, Object) In reportDataSet.ReportQueryParameters
					If keyValue.Key.ToString().StartsWith("@") Then
						Dim key = keyValue.Key.Replace("@", "").Trim()
						If Equals(key.ToLower(), param.ParamNameField.ToString().ToLower()) Then
							Dim oParam As New NexusProvider.Parameters
							oParam.ParamNameField = param.ParamNameField.ToString()
							oParam.ParamValueField = param.ParamValueField
							oQueryParameterCollection.Add(oParam)
							Exit For
						End If
					End If
				Next
			End If
		Next

		Dim objDataSet As New System.Data.DataSet

		'make SAM call with request parameters, sFileName will contain the name of the file we need to display
		objDataSet = oWebService.CallNamedStoredProcedure(reportDataSet.SqlCommandText, oQueryParameterCollection, True)
		Return objDataSet
	End Function
	Public Function GetReportDataSet(reportPath As String, reportName As String) As ReportDataSets

		Dim report As RdlReportSchema.Report = New RdlReportSchema.Report()
		Dim reportDataSets As ReportDataSets = New ReportDataSets()
		Dim reportDataSet As List(Of ReportDataSet) = New List(Of ReportDataSet)()
		If reportPath.EndsWith("\") = False Then
			reportPath = reportPath & "\"
		End If

		report = GetReportDetails(reportPath, reportName, False)
		If report.DataSets IsNot Nothing AndAlso report.DataSets.DataSet IsNot Nothing Then
			'foreach (DataSet ds in report.DataSets.DataSet)
			If True Then
				If report.DataSets.DataSet IsNot Nothing AndAlso report.DataSets.DataSet.Query IsNot Nothing Then
					Dim reportDataSet1 As ReportDataSet = New ReportDataSet()
					reportDataSet1.DataSetName = report.DataSets.DataSet.Name
					reportDataSet1.SqlCommandType = report.DataSets.DataSet.Query.CommandType
					reportDataSet1.SqlCommandText = report.DataSets.DataSet.Query.CommandText
					If report.DataSets.DataSet.Query.QueryParameters IsNot Nothing AndAlso report.DataSets.DataSet.Query.QueryParameters.QueryParameter.Count > 0 Then
						reportDataSet1.ReportQueryParameters = New Dictionary(Of String, Object)()

						For Each queryParameter As RdlReportSchema.QueryParameter In report.DataSets.DataSet.Query.QueryParameters.QueryParameter
							reportDataSet1.ReportQueryParameters.Add(queryParameter.Name, queryParameter.Value)
						Next
					End If
					reportDataSet.Add(reportDataSet1)
				End If
			End If
		End If
		reportDataSets.ReportDataSet = New List(Of ReportDataSet)()
		reportDataSets.ReportDataSet = reportDataSet
		Return reportDataSets

	End Function

	Private Sub SubreportProcessingEventHandler(ByVal sender As Object,
											   ByVal e As Microsoft.Reporting.WebForms.SubreportProcessingEventArgs)
		Dim oReportDataSets As ReportDataSets
		Dim query As New Nexus.Utils.SubReportQuery
		query = GetSubReportDetails(sReportDirName + sFolder, e.ReportPath)

		Dim storedProcedureName As String = query.CommandText.ToString()
		Dim sqlCommandType As String = query.CommandType.ToString()
		Dim dataSourceName As String = query.DataSourceName.ToString()
		Dim oParametersCollection As New NexusProvider.ParametersCollection
		Dim oParameters As Object(,)
		Dim iCount As Integer = 0
		Dim oParameter(oParametersCollection.Count - 1, 1) As Object
		If e.Parameters IsNot Nothing AndAlso e.Parameters.Count > 0 Then
			For Each param As Microsoft.Reporting.WebForms.ReportParameterInfo In e.Parameters
				If param.Name.ToLower().StartsWith("pm_sp") = False Then
					Dim oParam As New NexusProvider.Parameters
					oParam.ParamNameField = param.Name
					oParam.ParamValueField = param.Values(0)
					oParametersCollection.Add(oParam)
				End If
			Next
		End If

		Dim dsSubReport As New System.Data.DataSet
		dsSubReport = oWebService.CallNamedStoredProcedure(storedProcedureName, oParametersCollection, True)
		e.DataSources.Add(New ReportDataSource("DataSet1", dsSubReport.Tables(0)))
	End Sub


	'String urlReportServer = "http://sqlDBServer//Reportserver";
	'    rptViewer.ProcessingMode = ProcessingMode.Remote; // ProcessingMode will be Either Remote Or Local
	'    rptViewer.ServerReport.ReportServerUrl = New Uri(urlReportServer); //Set the ReportServer Url
	'    rptViewer.ServerReport.ReportPath = "/ReportName"; //Passing the Report Path                

	'    //Creating an ArrayList for combine the Parameters which will be passed into SSRS Report
	'    ArrayList reportParam = New ArrayList();
	'    reportParam = ReportDefaultPatam();

	'    ReportParameter[] param = New ReportParameter[reportParam.Count];
	'    For (int k = 0; k < reportParam.Count; k++)
	'    {
	'        param[k] = (ReportParameter)reportParam[k];
	'    }
	'    // pass crendentitilas
	'    //rptViewer.ServerReport.ReportServerCredentials = 
	'    //  New ReportServerCredentials("uName", "PassWORD", "doMain");

	'    //pass parmeters to report
	'    rptViewer.ServerReport.SetParameters(param); //Set Report Parameters
	'    rptViewer.ServerReport.Refresh();
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			'  viewerReportViewer.LocalReport.Refresh()
			ConfigureEmailButton()
		End If

	End Sub

	Private Sub ConfigureEmailButton()
		Dim sReportNameQS As String = Request.QueryString("reportname")
		If sReportNameQS Is Nothing Then Return

		' Only show for Remittance Advice Agency reports
		If Not sReportNameQS.Contains("Remittance_Advice_Agency") Then Return

		' Only show for non-broker/agent users (Req 11)
		Dim oWebSvc As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
		Dim oUser As NexusProvider.UserDetails = oWebSvc.GetUserDetails(HttpContext.Current.User.Identity.Name)
		If oUser IsNot Nothing AndAlso Not String.IsNullOrEmpty(oUser.PartyCode) Then Return

		' Show the email toolbar
		divEmailToolbar.Visible = True
	End Sub

	Protected Sub btnEmailRemittanceAdvice_Click(ByVal sender As Object, ByVal e As EventArgs)
		Dim sAccountCode As String = If(Session("AccountCode") IsNot Nothing, Session("AccountCode").ToString(), "")
		Dim iPartyKey As Integer = 0
		If Request.QueryString("PartyKey") IsNot Nothing Then
			Integer.TryParse(Request.QueryString("PartyKey"), iPartyKey)
		End If

		Dim oService As New RemittanceEmailService()

		' --- Validation 1: Recipient email ---
		Dim oRecipientResult As EmailResolutionResult = oService.ResolveRecipientEmail(iPartyKey)
		If Not oRecipientResult.Success Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "NoEmailAlert",
				"alert('" & oRecipientResult.ErrorMessage.Replace("'", "\'") & "');", True)
			' Create Work Manager task for missing email
			Try
				Dim oWebSvc As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
				Dim oParty As NexusProvider.BaseParty = oWebSvc.GetParty(iPartyKey)
				Dim sPartyName As String = ""
				If oParty IsNot Nothing Then
					If TypeOf oParty Is NexusProvider.PersonalParty Then
						sPartyName = If(oParty.ResolvedName IsNot Nothing AndAlso oParty.ResolvedName.Trim() <> "", oParty.ResolvedName.Trim(), "")
					ElseIf TypeOf oParty Is NexusProvider.CorporateParty Then
						sPartyName = If(DirectCast(oParty, NexusProvider.CorporateParty).CompanyName IsNot Nothing AndAlso DirectCast(oParty, NexusProvider.CorporateParty).CompanyName.Trim() <> "", DirectCast(oParty, NexusProvider.CorporateParty).CompanyName.Trim(), "")
					End If
				End If
				If String.IsNullOrEmpty(sPartyName) Then sPartyName = sAccountCode
				Dim sPaymentDate As String = If(Session("PaymentDate") IsNot Nothing, Session("PaymentDate").ToString(), "")
				Dim dPaymentDate As DateTime = DateTime.Now
				DateTime.TryParse(sPaymentDate, dPaymentDate)
				Dim dPaymentAmount As Decimal = 0
				If Session("PaymentAmount") IsNot Nothing Then Decimal.TryParse(Session("PaymentAmount").ToString(), dPaymentAmount)
				oService.CreateFailedEmailTask(sAccountCode, sPartyName, dPaymentDate, dPaymentAmount, oRecipientResult.ErrorMessage)
			Catch
			End Try
			Return
		End If

		' --- Validation 2: Sender email ---
		Dim iInsurerPartyKey As Integer = 0
		If Session("InsurerPartyKey") IsNot Nothing Then
			Integer.TryParse(Session("InsurerPartyKey").ToString(), iInsurerPartyKey)
		End If
		Dim oSenderResult As EmailResolutionResult = oService.ResolveSenderEmail(iInsurerPartyKey)
		If Not oSenderResult.Success Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "NoSenderAlert",
				"alert('" & oSenderResult.ErrorMessage.Replace("'", "\'") & "');", True)
			Return
		End If

		' --- Validation 3: Export report to PDF ---
		Dim sPdfPath As String = ""
		Try
			Dim warnings As Microsoft.Reporting.WebForms.Warning() = Nothing
			Dim streamIds As String() = Nothing
			Dim mimeType As String = String.Empty
			Dim encoding As String = String.Empty
			Dim extension As String = String.Empty

			Dim bytes As Byte() = viewerReportViewer.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamIds, warnings)

			If bytes Is Nothing OrElse bytes.Length = 0 Then
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "NoPdfAlert",
					"alert('The remittance advice PDF could not be generated. The email has not been sent.');", True)
				Return
			End If

			Dim sTempLocation As String = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).TempFileLocation
			Dim sUniqueDir As String = sTempLocation & "\" & Guid.NewGuid.ToString()
			If Not System.IO.Directory.Exists(sUniqueDir) Then
				System.IO.Directory.CreateDirectory(sUniqueDir)
			End If

			Dim sPaymentDatePdf As String = If(Session("PaymentDate") IsNot Nothing, Session("PaymentDate").ToString(), DateTime.Now.ToString("dd/MM/yyyy"))
			Dim sPdfFileName As String = String.Format("Remittance_Advice_{0}_{1}.pdf", sAccountCode, sPaymentDatePdf.Replace("/", ""))
			sPdfPath = sUniqueDir & "\" & sPdfFileName

			System.IO.File.WriteAllBytes(sPdfPath, bytes)
		Catch ex As Exception
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PdfErrorAlert",
				"alert('Failed to generate the remittance advice PDF. The email has not been sent.');", True)
			Return
		End Try

		' Validate PDF file was actually created
		If String.IsNullOrEmpty(sPdfPath) OrElse Not System.IO.File.Exists(sPdfPath) Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "NoPdfFileAlert",
				"alert('The remittance advice PDF attachment could not be found. The email has not been sent.');", True)
			Return
		End If

		' Clean up previous PDF
		If Session("Report") IsNot Nothing Then
			Dim sPreviousPdfPath As String = Session("Report").ToString()
			If Not String.IsNullOrEmpty(sPreviousPdfPath) AndAlso System.IO.File.Exists(sPreviousPdfPath) Then
				System.IO.File.Delete(sPreviousPdfPath)
			End If
		End If
		Session("Report") = sPdfPath

		' --- All validations passed — open SendEmail modal ---
		Dim sPaymentDateQS As String = If(Session("PaymentDate") IsNot Nothing, Session("PaymentDate").ToString(), DateTime.Now.ToString("dd/MM/yyyy"))
		Dim sPaymentAmount As String = If(Session("PaymentAmount") IsNot Nothing, Session("PaymentAmount").ToString(), "0.00")
		Dim iTransactionCount As Integer = If(Session("TransactionCount") IsNot Nothing, CInt(Session("TransactionCount")), 0)

		Dim sUrl As String = String.Format(
			"../Modal/SendEmail.aspx?EmailMode=RemittanceAdvice&AccountCode={0}&PartyKey={1}&PaymentDate={2}&PaymentAmount={3}&TransactionCount={4}",
			Server.UrlEncode(sAccountCode), iPartyKey, Server.UrlEncode(sPaymentDateQS),
			Server.UrlEncode(sPaymentAmount), iTransactionCount)

		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenEmailModal",
			String.Format("window.open('{0}', 'EmailRemittance', 'width=750,height=500,scrollbars=yes,resizable=yes');", sUrl), True)
	End Sub

	'Protected Sub viewerReportViewer_Error(ByVal source As Object, ByVal e As CrystalDecisions.Web.ErrorEventArgs) Handles viewerReportViewer.Error
	'    e.Handled = True
	'End Sub



End Class


