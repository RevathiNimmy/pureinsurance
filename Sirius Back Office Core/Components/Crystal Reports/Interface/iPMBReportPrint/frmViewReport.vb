Option Strict Off
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Xml.Linq
Imports System.Xml.Serialization

Imports bSIRReportPrint
Imports Microsoft.Reporting.WinForms
Imports SharedFiles
Imports ReportParameter = Microsoft.Reporting.WinForms.ReportParameter
Public Class frmviewReport
    Dim m_sReportFileName As String
    Dim m_oReportDocument As Report
    Dim m_oReportParameters As bSIRReportPrint.ReportParameters
    Public oBusiness As bSIRReportPrint.Business
    Public reportFolderName As String
    'Friend Function ViewReport(ByVal sReportName As String, ByVal sConnectionString As String, ByVal parameters As bSIRReportPrint.ReportDataSets) As Integer
    '    Dim intCounter As Integer
    '    Dim intCounter1 As Integer

    '    Dim sLoginId As String = ""
    '    Dim sPassword As String = ""
    '    Dim strParValPair() As String
    '    Dim strVal() As String
    '    Dim index As Integer

    '    Try

    '        GetUserAndPassword(sLoginId, sPassword)

    '        reportViewer.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local
    '        Dim localReport As LocalReport = reportViewer.LocalReport
    '        localReport.ReportPath = sReportName

    '        Dim reportParameters As List(Of Microsoft.Reporting.WinForms.ReportParameter) = New List(Of Microsoft.Reporting.WinForms.ReportParameter)()
    '        For Each reportDatSet As bSIRReportPrint.ReportDataSet In parameters.ReportDataSet
    '            For Each param As KeyValuePair(Of String, Object) In reportDatSet.ReportQueryParameters
    '                reportParameters.Add(New Microsoft.Reporting.WinForms.ReportParameter(param.Key, param.Value.ToString()))
    '            Next
    '        Next

    '        'Dim oReport As New bSIRReportPrintb.ReportDocument()
    '        Dim reportDS As ReportDataSource = New ReportDataSource()
    '        Dim report As String = sReportName
    '        Dim dsReport As System.Data.DataSet
    '        For Each reportDatSet As bSIRReportPrint.ReportDataSet In parameters.ReportDataSet
    '            reportDS.Name = reportDatSet.DataSetName
    '            reportDS.Value = dsReport.Tables(0)
    '            localReport.DataSources.Add(reportDS)

    '        Next


    '        ' Set the report parameters for the report  
    '        localReport.SetParameters(reportParameters)
    '        reportViewer.RefreshReport()

    '        Return True

    '    Catch ex As Exception
    '        Dim str As String = ex.GetBaseException().ToString()
    '        MsgBox(ex.GetBaseException().Message)
    '        Return False
    '    End Try
    'End Function

    Friend Sub ShowReport()
        With reportViewer
            .LocalReport.ReportPath = m_sReportFileName
            .ShowExportButton = True
            .ShowPrintButton = True
            .ShowExportButton = True
            .ShowPrintButton = True
            .ShowPageNavigationControls = True
            .ShowFindControls = True
            .ShowZoomControl = True
            .ShowStopButton = True
            .RefreshReport()
        End With
    End Sub

    Friend Function PrintReport() As Integer
        Try
            reportViewer.PrintDialog()
            Return gPMConstants.PMEReturnCode.PMTrue
        Catch ex As Exception
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    Friend WriteOnly Property ReportFileName() As String
        Set(ByVal value As String)
            m_sReportFileName = value
        End Set
    End Property

    Friend WriteOnly Property ReportDocument() As Report
        Set(ByVal value As Report)
            m_oReportDocument = value
        End Set
    End Property
    Public Property ReportParameters As bSIRReportPrint.ReportParameters
    Public Property ConnectionString As String
    Public Property ReportParametersObjects As Object(,)
    Public Property ReportPath As String
    Public Property ReportName As String

#Region "Private Methods"
    ''' <summary>
    ''' Get the User Name and Password to connect to the DataBase
    ''' Password is Stored in Encrypted Form in the Registry.
    ''' </summary>
    ''' <param name="o_sUserName"></param>
    ''' <param name="o_sPassword"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetUserAndPassword(ByRef o_sUserName As String, ByRef o_sPassword As String) As Integer
        Dim nReturn As Integer = PMEReturnCode.PMFalse
        Try
            Dim sLoginId As String = ""
            nReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                       v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                       v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon,
                       v_sSettingName:=PMSQLLoginId,
                       r_sSettingValue:=sLoginId)

            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("Failed to Retrive values from Regisrty")
            End If
            Dim sPasswordSecure As String = ""
            nReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                       v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                       v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon,
                       v_sSettingName:=PMSQLLoginPassword,
                       r_sSettingValue:=sPasswordSecure)

            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("Failed to Retrive values from Regisrty")
            End If

            Dim aKeys As Byte()
            aKeys = Encoding.ASCII.GetBytes(PMEncryptionEntropy)

            o_sUserName = Decrypt(sLoginId, aKeys)
            o_sPassword = Decrypt(sPasswordSecure, aKeys)

            Return nReturn

        Catch ex As Exception
            nReturn = PMEReturnCode.PMFalse
            Throw New ApplicationException("GetUserAndPassword Failed.", ex)
        End Try

        Return nReturn

    End Function
    Private Function Decrypt(sCipher As String, aKeys As Byte()) As String
        Const kScope As DataProtectionScope = DataProtectionScope.LocalMachine
        If sCipher Is Nothing Then
            Throw New ArgumentNullException("cipher")
        End If

        'parse base64 string
        Dim aData As Byte() = Convert.FromBase64String(sCipher)

        'decrypt data
        Dim aDecrypted As Byte() = ProtectedData.Unprotect(aData, aKeys, kScope)
        Return Encoding.Unicode.GetString(aDecrypted)
    End Function
    Public Sub SubreportProcessingEventHandler(ByVal sender As Object,
                                               ByVal e As SubreportProcessingEventArgs)

        Dim query As New bSIRReportPrint.SubReportQuery
        query = oBusiness.GetSubReportDetails(ReportPath + reportFolderName, e.ReportPath)

        Dim storedProcedureName As String = query.CommandText.ToString()
        Dim sqlCommandType As String = query.CommandType.ToString()
        Dim dataSourceName As String = query.DataSourceName.ToString()
        Dim dsSubReport As New System.Data.DataSet
        Using connection As SqlConnection = New SqlConnection(ConnectionString)
            Using command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand(storedProcedureName, connection)
                command.CommandType = If(Equals(sqlCommandType, "StoredProcedure"), CommandType.StoredProcedure, CommandType.Text)
                'connection.Close()
                If e.Parameters IsNot Nothing AndAlso e.Parameters.Count > 0 Then
                    For Each param As ReportParameterInfo In e.Parameters
                        If param.Name.ToLower().StartsWith("pm_sp") = False Then '  param.Name.ToLower().Contains(storedProcedureName.ToLower()) = False  Then
                            command.Parameters.Add(New SqlParameter(param.Name, param.Values(0)))
                        End If
                    Next
                End If
                Dim reportAdapter As Data.SqlClient.SqlDataAdapter = New Data.SqlClient.SqlDataAdapter(command)
                reportAdapter.Fill(dsSubReport, "ReportData")
            End Using
        End Using
        e.DataSources.Add(New ReportDataSource("DataSet1", dsSubReport.Tables(0)))
        'Select Case e.ReportPath
        '    Case "Subreport1"

        '        'Dim tbl As DataTable = New DataTable("TableName")
        '        'Dim Status As DataColumn = New DataColumn
        '        'Status.DataType = System.Type.GetType("System.String")
        '        'Status.ColumnName = "Status"
        '        'tbl.Columns.Add(Status)
        '        'Dim Account As DataColumn = New DataColumn
        '        'Account.DataType = System.Type.GetType("System.String")
        '        'Account.ColumnName = "Account"
        '        'tbl.Columns.Add(Account)
        '        'Dim rw As DataRow = tbl.NewRow()
        '        'rw("Status") = core.GetStatus
        '        'rw("Account") = core.Account
        '        'tbl.Rows.Add(rw)
        '        'e.DataSources.Add(New ReportDataSource("ReportDatasourceName", tbl))
        '    Case "subreport2"
        '        'core.DAL.cnStr = My.Settings.cnStr
        '        'core.DAL.LoadSchedule()
        '        'e.DataSources.Add(New ReportDataSource("ScheduledTasks",
        '        '                                   My.Forms.Mother.DAL.dsSQLCfg.tSchedule))
        '    Case "subreport3"
        '        'core.DAL.cnStr = My.Settings.cnStr
        '        'Dim dt As DataTable = core.DAL.GetNodesForDateRange(DateAdd("d",
        '        '                                                          -1 * CInt(e.Parameters("NumberOfDays").Values(0)),
        '        '                                                          Today),
        '        '                                                  Now)
        '        'e.DataSources.Add(New ReportDataSource("Summary", dt))
        'End Select
    End Sub

    Private Sub frmviewReport_Load(sender As Object, e As EventArgs) Handles Me.Load
        oBusiness = New bSIRReportPrint.Business
        reportViewer.Visible = False
        Dim localReport As New LocalReport()
        localReport.ReportPath = ReportPath + ReportName + ".rdl"
        reportViewer.LocalReport.ReportPath = ReportPath + ReportName + ".rdl"
        reportFolderName = m_sReportFileName.Split("\")(0)
        reportViewer.ProcessingMode = ProcessingMode.Local
        Dim s As ReportParameterInfoCollection = reportViewer.LocalReport.GetParameters()
        Dim addBranchInLocalParameter As Boolean = False

        For Each rp As ReportParameterInfo In reportViewer.LocalReport.GetParameters()
            If rp.Name.ToLower() = "branch" Then
                addBranchInLocalParameter = True
                Exit For
            End If
        Next
        Dim reportDataSets As bSIRReportPrint.ReportDataSets = oBusiness.GetReportDataSet(ReportPath, ReportName, ReportParametersObjects, False)

        reportViewer.LocalReport.DataSources.Clear()
        Dim localReportParameter As New List(Of ReportParameter)
        Dim dsReport As System.Data.DataSet
        dsReport = oBusiness.GetReportData(ConnectionString, reportDataSets.ReportDataSet(0), ReportParametersObjects, localReportParameter, addBranchInLocalParameter)

        If dsReport Is Nothing OrElse dsReport.Tables Is Nothing OrElse dsReport.Tables.Count = 0 OrElse dsReport.Tables(0).Rows Is Nothing OrElse dsReport.Tables(0).Rows.Count = 0 Then
            If MessageBox.Show("There is no data currently available for this report" & Strings.Chr(13) & Strings.Chr(10) &
                                           "Do you still wish to preview?", m_sReportFileName.Split("\")(1), MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then

                Me.Close()
            End If
        End If
        reportViewer.Visible = True
        reportViewer.LocalReport.DataSources.Add(New ReportDataSource() With {
.Name = "DataSet1",
.Value = dsReport.Tables(0)
})
        AddHandler reportViewer.LocalReport.SubreportProcessing, AddressOf Me.SubreportProcessingEventHandler

        reportViewer.LocalReport.SetParameters(localReportParameter)
        ' Add a handler for SubreportProcessing.
        reportViewer.LocalReport.Refresh()
        reportViewer.RefreshReport()
        reportViewer.ZoomMode = ZoomMode.PageWidth
    End Sub
    'Public Function GetReportData(connectionString As String, reportDataSet As bSIRReportPrint.ReportDataSet, reportParameters As Object(,), ByRef localReportParameters As List(Of Microsoft.Reporting.WinForms.ReportParameter), ByVal addBranchInLocalParameter As Boolean) As Data.DataSet
    '    Dim dsReport As Data.DataSet = New Data.DataSet()
    '    Dim storedProcedureName As String = reportDataSet.SqlCommandText
    '    Dim sqlCommandType As String = reportDataSet.SqlCommandType
    '    Dim queryParameters = New Dictionary(Of String, Object)()
    '    localReportParameters = New List(Of Microsoft.Reporting.WinForms.ReportParameter)()
    '    Dim i As Integer = 0
    '    For i = 0 To reportParameters.GetLength(0) - 1
    '        If (Not Equals(reportParameters(i, 0).ToString().ToLower(), "branch") Or addBranchInLocalParameter) Then
    '            Dim value As String = String.Empty
    '            If reportParameters(CInt(i), CInt(1)) Is Nothing Then
    '                value = 0
    '            Else
    '                value = reportParameters(CInt(i), CInt(1)).ToString()
    '            End If
    '            localReportParameters.Add(New Microsoft.Reporting.WinForms.ReportParameter(reportParameters(CInt(i), CInt(0)).ToString(), reportParameters(CInt(i), CInt(1)).ToString()))
    '        End If
    '        If (reportDataSet.ReportQueryParameters IsNot Nothing) Then

    '            For Each keyValue As KeyValuePair(Of String, Object) In reportDataSet.ReportQueryParameters
    '                If keyValue.Key.ToString().StartsWith("@") Then
    '                    Dim key = keyValue.Key.Replace("@", "").Trim()
    '                    If Equals(key.ToLower(), reportParameters(i, 0).ToString().ToLower()) Then
    '                        queryParameters.Add(reportParameters(i, 0).ToString(), reportParameters(i, 1))
    '                        Exit For
    '                    End If
    '                End If
    '            Next
    '        End If
    '    Next

    '    Using connection As SqlConnection = New SqlConnection(connectionString)
    '        Using command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand(storedProcedureName, connection)
    '            command.CommandType = If(Equals(sqlCommandType, "StoredProcedure"), CommandType.StoredProcedure, CommandType.Text)
    '            If (reportDataSet.ReportQueryParameters IsNot Nothing) Then

    '                For Each param As KeyValuePair(Of String, Object) In reportDataSet.ReportQueryParameters
    '                    Dim name = param.Key.Replace("@", "").Trim()
    '                    Dim value As Object = Nothing

    '                    If queryParameters.TryGetValue(name, value) Then
    '                        value = CObj(Convert.ToString(value).Replace("<", "").Replace(">", ""))
    '                        'reportDataSet1.ReportQueryParameters.Add(name, value);
    '                        Dim cmdParam As New SqlParameter()
    '                        cmdParam.ParameterName = name
    '                        cmdParam.Value = value
    '                        command.Parameters.Add(cmdParam)
    '                    End If
    '                Next
    '            End If
    '            Dim reportAdapter As Data.SqlClient.SqlDataAdapter = New Data.SqlClient.SqlDataAdapter(command)
    '            reportAdapter.Fill(dsReport, "ReportData")
    '        End Using
    '    End Using

    '    Return dsReport


    'End Function

#End Region

End Class