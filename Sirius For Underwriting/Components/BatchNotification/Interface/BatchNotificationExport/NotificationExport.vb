Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Xml
Imports System.Data.SqlClient
Imports SSP.Shared.gPMConstants

Public NotInheritable Class NotificationExport
#Region "Application Constants"
    ' System option numbers
    Public Const ACExportPathOption As Integer = 5077
#End Region

#Region "Fields"
    Private m_sProcedureName As String
    Private m_dtStartDate As DateTime
    Private m_dtEndDate As DateTime

    Private m_sExportPath As String = String.Empty
    ' Database connection
    Protected m_oDatabase As Object = Nothing
#End Region

#Region "Creator"

    Public Sub New(ByVal v_sProcedureName As String, ByVal v_dtStartDate As DateTime, ByVal v_dtEndDate As DateTime)
        ' Connect to database
        m_sProcedureName = v_sProcedureName
        m_dtStartDate = v_dtStartDate
        m_dtEndDate = v_dtEndDate
        m_sExportPath = GetSystemOption(ACExportPathOption)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

#End Region

#Region "Private Methods"
    Private Function GenerateFullProcedureName(ByRef sProcedureName As String) As Long
        Dim iReturn As PMEReturnCode
        Dim sICCSNumber As String

        Try
            ' Add parameters
            AddParameterLite(m_oDatabase, "ICCS", System.DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong, True)

            ' Execute command
            iReturn = m_oDatabase.SQLSelect("spu_pm_iccs", "Get ICCS", True)

            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_pm_iccs'")
            End If

            ' Get batch id
            sICCSNumber = m_oDatabase.Parameters.Item("ICCS").Value
            sProcedureName = "spu_ICCS_" & sICCSNumber.ToString & "_" & sProcedureName
        Catch ex As Exception
            Throw New Exception("Unable to Generate Full ProcedureName", ex)
        End Try

    End Function

    Private Function ProcedureExists(ByVal v_sProcedureName As String) As Long
        Dim iReturn As PMEReturnCode
        Dim iExists As String

        ' Add parameters
        AddParameterLite(m_oDatabase, "sName", v_sProcedureName, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
        AddParameterLite(m_oDatabase, "o_return", System.DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)

        ' Execute command
        iReturn = m_oDatabase.SQLSelect("DDLExistsProcedure", "Exists Procedure", True)

        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'DDLExistsProcedure'")
        End If

        iExists = m_oDatabase.Parameters.Item("o_return").Value

        If iExists = 0 Then
            Throw New ApplicationException(v_sProcedureName & " does not exist in the database.")
        End If

    End Function

    Private Function GenerateFullPath(ByRef sDirectoryPath As String) As Long
        Dim sFileName As String
        ' Check it was configured
        If (sDirectoryPath.Length = 0) Then
            Throw New ApplicationException("Export directory not configured in System Options.")
        Else
            ' Check it exists
            If Not Directory.Exists(m_sExportPath) Then
                Throw New ApplicationException("Export directory does not exist. " & sDirectoryPath)
            End If
        End If
        sFileName = GenerateFileName()
        sDirectoryPath = System.IO.Path.Combine(sDirectoryPath, sFileName)

    End Function

    Private Function GenerateFileName() As String
        Return String.Format("BatchNotification_{0}.xml", Now.ToString("yyyyMMddhhmm"))
    End Function

    Private Function KeyColumnsExists(ByVal oRecordset As DataTable) As Boolean
        Dim bPartyKeyExists As Boolean
        Dim bInsuranceFileKeyExists As Boolean
        Dim bInsuranceFolderKeyExists As Boolean
        'Dim oField As DataRow

        If oRecordset.Rows.Count = 0 Then
            Throw New ApplicationException("No Records Found.")
            'Else
            '    oRecordset.MoveFirst()
        End If

        For Each oField As DataColumn In oRecordset.Columns
            'If oField.Item(0) = "PartyKey" Then
            If oField.ColumnName = "PartyKey" Then
                bPartyKeyExists = True
                'ElseIf oField.Item(0) = "InsuranceFileKey" Then
                'ElseIf oRecordset.Columns(0).ColumnName = "InsuranceFileKey" Then
            ElseIf oField.ColumnName = "InsuranceFileKey" Then
                bInsuranceFileKeyExists = True
                'ElseIf oRecordset.Columns(0).ColumnName = "InsuranceFolderKey" Then
            ElseIf oField.ColumnName = "InsuranceFolderKey" Then
                bInsuranceFolderKeyExists = True
            End If
        Next
        If Not (bPartyKeyExists And bInsuranceFileKeyExists And bInsuranceFolderKeyExists) Then
            Throw New ApplicationException("One or more of the key columns does not exist in the stored procedure output. PartyKey, InsuranceFileKey, InsuranceFolderKey (Case-sensitive).")
        End If
        Return True
    End Function

    Private Function ExportFile(ByVal oRecordset As DataTable) As Long
        Dim oXmlTextWriter As New XmlTextWriter(m_sExportPath, System.Text.Encoding.UTF8)
        'Dim oField As DataRow
        Try
            With oXmlTextWriter
                .Formatting = Formatting.Indented
                .WriteStartDocument()
                .WriteStartElement("ExportRows")
                .WriteStartAttribute("TotalRows")
                .WriteValue(oRecordset.Rows.Count)
                .WriteEndAttribute()

                For ctrRow As Integer = 0 To oRecordset.Rows.Count - 1
                    .WriteStartElement("ExportRow")
                    For ctrColumn As Integer = 0 To oRecordset.Columns.Count - 1
                        .WriteStartAttribute(oRecordset.Columns(ctrColumn).ColumnName.ToString.Trim)
                        .WriteValue(oRecordset.Rows(ctrRow)(ctrColumn).ToString.Trim())
                    Next
                    .WriteEndAttribute()
                    .WriteEndElement()
                Next

                .WriteEndElement()
                .WriteEndDocument()
                .Close()
            End With
        Catch ex As Exception
            Throw New Exception("Unable to Export File", ex)
        Finally
            oXmlTextWriter = Nothing
        End Try
    End Function

#End Region

#Region "Public Methods"

    Public Sub ProcessExport()
        Dim oRecordSet As New DataTable
        Dim oCommand As New SqlCommand
        Dim oParameter As SqlParameter
        Dim adp As New SqlDataAdapter

        Try
            'Init and open m_oDatabase
            DBConnect(m_oDatabase)

            GenerateFullProcedureName(m_sProcedureName)

            ProcedureExists(m_sProcedureName)

            GenerateFullPath(m_sExportPath)


            If Not (m_dtStartDate = Date.MinValue) Then
                oParameter = New SqlParameter
                oParameter.ParameterName = "@StartDate"
                oParameter.Value = m_dtStartDate
                oParameter.Direction = ParameterDirection.Input
                oParameter.DbType = DbType.Date
                oCommand.Parameters.Add(oParameter)
                If Not (m_dtEndDate = Date.MinValue) Then
                    oParameter = New SqlParameter
                    oParameter.ParameterName = "@EndDate"
                    oParameter.Value = m_dtEndDate
                    oParameter.Direction = ParameterDirection.Input
                    oParameter.DbType = DbType.Date
                    oCommand.Parameters.Add(oParameter)
                End If
            Else
                'Just EndDate could have been passed
                If Not (m_dtEndDate = Date.MinValue) Then
                    oParameter = New SqlParameter
                    oParameter.ParameterName = "@EndDate"
                    oParameter.Value = m_dtEndDate
                    oParameter.Direction = ParameterDirection.Input
                    oParameter.DbType = DbType.Date
                    oCommand.Parameters.Add(oParameter)
                End If
            End If
            oCommand.CommandText = m_sProcedureName
            oCommand.CommandType = CommandType.StoredProcedure
            Dim i As Integer = m_oDatabase.ExecuteDataTable(oCommand, adp, oRecordSet)

            KeyColumnsExists(oRecordSet)

            ExportFile(oRecordSet)
        Catch ex As Exception
            Throw New ApplicationException(ex.Message, ex)
        Finally
            DBDisconnect(m_oDatabase)
            oRecordSet = Nothing
        End Try
    End Sub

    Public Function GetSystemOption(ByVal iOptionNumber As Integer) As String
        Dim lResult As Integer = 0
        Dim oSystemOptions As bSIROptions.Business = Nothing
        Dim sOptionValue As String = String.Empty

        Try
            ' Create the System Options Object
            oSystemOptions = New bSIROptions.Business
            If (oSystemOptions Is Nothing) Then
                Throw New Exception("Unable to create bSIROptions.Business")
            End If

            ' Initialise
            lResult = oSystemOptions.Initialise(
                sUsername:="",
                sPassword:="",
                iUserID:=0,
                iSourceID:=1,
                iLanguageID:=1,
                iCurrencyID:=26,
                iLogLevel:=PMELogLevel.PMLogError,
                sCallingAppName:=ACApp)
            If lResult <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to initialise bSIROptions.Business")
            End If

            ' Get the system option
            lResult = oSystemOptions.GetOption(
                iOptionNumber:=iOptionNumber,
                sValue:=sOptionValue,
                v_iSourceID:=1)
            If lResult <> PMEReturnCode.PMTrue Then
                Throw New Exception(String.Format("Unable to retrieve system option '{0}'", iOptionNumber))
            End If

            ' Return the option value
            Return sOptionValue

        Catch ex As Exception
            Throw New Exception("Unable to retrieve system option", ex)

        Finally
            If Not oSystemOptions Is Nothing Then
                oSystemOptions.Dispose()
            End If
            oSystemOptions = Nothing
        End Try
    End Function

    Public Sub CloseDBConnection()
        DBDisconnect(m_oDatabase)
    End Sub
#End Region
End Class
