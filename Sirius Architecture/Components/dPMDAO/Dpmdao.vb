Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  12 July 1996
    '
    ' Description: Main Module.
    ' ***************************************************************** '

    ' Main global constant for all functions
    ' to identify which application this is.


    ' Transaction Nest Level Start
    Public Const ACTransNestLevelStart As Integer = 0

    ' Open Count Start
    Public Const ACOpenCountStart As Integer = 0

    ' Default Max Number of Rows for Selects
    Public Const ACDefaultMaxRows As Integer = 500

    ' A long value cannot hold any more than this value
    Public Const ACMaxMaxRows As Integer = 2147483647

    ' Default Login User and Password (for Sirius)
    Public Const ACDefaultUser As String = "SIRIUS"
    Public Const ACDefaultPassword As String = "$1R1U5"

    ' RAM20050414 - Added the following constant to support Swift Database
    ' Default Login User and Password (for Swift)
    Public Const ACDefaultSwiftUser As String = "Swift"
    Public Const ACDefaultSwiftPassword As String = "hy4u8hv5495tyc92y637dx45t5c46y"

    ' Default Login Timeout in seconds
    Public Const ACDefaultLoginTimeout As Integer = 15

    ' Default Query Timeout in Seconds
    ' Note: Can be altered by the Query Timeout Property
    Public Const ACDefaultQueryTimeout As Integer = 30
    Public Const ACMaxQueryTimeout As Integer = 7200
    Public Const ACMinQueryTimeout As Integer = 5

    ' RFC05031998 Allow for Different Data Formats By DB Type
    Public Const ACMSAccessDateDelimiter As String = "#"
    'RFC080799 - Change Access Date Format to use Long Date Style
    Public Const ACMSAccessDateFormat As String = "dd/MMMM/yyyy hh:mm:ss"
    'CDH06022002 - Now uses the proper ODBC escape sequence for timestamps
    ' developer guide no. 
    'Public Const ACDefaultDateDelimiter As String = ""
    Public Const ACDefaultDateDelimiter As String = "'"
    'developer guide no. 13
    'Public Const ACDefaultDateFormat As String = "{\t\s'yyyy\-mm\-dd hh\:nn\:ss'}"
    'TODO: This ignores AM \ PM from the time. hence needs a fix
    'Public Const ACDefaultDateFormat As String = "{0: yyyy-MM-dd hh:mm:ss}"
    Public Const ACDefaultDateFormat As String = "{0: yyyy-MM-dd HH:mm:ss}"

    ' WARNING : This is the OLE/DB error number for a deadlock NOT the ADO numder (it doesn't have one.)
    '           So if we ever switched to a different database and/or used a different provider then
    '           this number may change.
    Public Const ACDeadlock As Integer = -2147467259

    ' Used to Identify Boolean Fields in the Voyager DB
    'Private Const ACVoyBoolFieldName As String = "_ind"
    'Private Const ACVoyBoolFieldNameLen As Integer = 4

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"


    'Public Declare Function GetTickCount Lib "kernel32" () As Long

    ' ***************************************************************** '
    ' Name: LogDatabaseError
    '
    ' Description: Strings the RDO Errors Collection properties with
    '              the input string. Used when logging database errors
    '              to the Sirius Log File.
    '
    ' ***************************************************************** '
    Public Sub LogDatabaseError(ByRef sSiriusUsername As String, ByRef sCallingAppName As String, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef bConnectionPooling As Boolean, ByRef iType As Integer, ByRef sMsg As String, Optional ByRef vApp As String = "", Optional ByRef vClass As String = "", Optional ByRef vMethod As String = "", Optional ByRef vErrNo As Object = Nothing, Optional ByRef vErrDesc As String = "", Optional ByRef vErrSource As Object = Nothing, Optional ByRef oCon As SqlConnection = Nothing, Optional ByRef oCmd As SqlCommand = Nothing, Optional ByRef lError As Integer = 0)

        Dim iSub As Integer
        Dim lPos As Integer
        Dim sParams As String = ""

        ' This isn't the return value from this function, but is intended to work out what type of database
        ' error happened. We are particularly interested in Deadlocks so that we can return to the calling
        ' business object that a deadlock has occurred and they can then take an appropriate action.
        ' Just set to general error for now, we will check for Deadlocks later.
        lError = PMConstants.PMEReturnCode.PMError

        ' If input string already has something in it add CR/LF.
        If sMsg = "" Then
            ' No newline required
            sMsg = "********************************************************************" & Strings.ChrW(13) & StringCHR10()
        Else
            ' Start on a new line
            sMsg = "********************************************************************" & Strings.ChrW(13) & StringCHR10() & sMsg
        End If

        ' Output the Parameters
        LogParameters(oCmd, sParams)

        ' Add on the Paramaters
        If sParams <> "" Then
            sMsg = sMsg & sParams & Strings.ChrW(13) & StringCHR10()
        End If

        ' Process
        'only use err if it contains something sensible
        'If Information.Err().Number Then
        '    sMsg = sMsg & StringCHR13 & StringCHR10() & "VB Error # " & Conversion.Str(Information.Err().Number)
        '    sMsg = sMsg & StringCHR13 & StringCHR10() & "   Generated by " & Information.Err().Source
        '    sMsg = sMsg & StringCHR13 & StringCHR10() & "   Description  " & Information.Err().Description
        'End If

        If Not (oCon Is Nothing) Then
            iSub = 1
        End If

        ' Find the Password
        lPos = (sMsg.IndexOf("Password=", StringComparison.CurrentCultureIgnoreCase) + 1)

        ' Replace Any occurrences of the Password with stars
        'If lPos > 0 Then
        '    lPos += ("Password=").Length
        '    Do While lPos < sMsg.Length And sMsg.Substring(lPos - 1, 1) <> ";"
        '        Mid(sMsg, lPos) = "*"

        '        lPos += 1
        '    Loop
        'End If
        sMsg = sMsg.Replace("Password=", "*********")

        'sMsg = sMsg & StringCHR13() & StringCHR10() & "PMDAO VERSION = (" & CStr(My.Application.Info.Version.Major) & "." & CStr(My.Application.Info.Version.Minor) & "." & CStr(My.Application.Info.Version.Revision) & ")"

        'sMsg = sMsg & StringCHR13() & StringCHR10() & "EXE = (" & My.Application.Info.DirectoryPath & My.Application.Info.AssemblyName & ")"
        'sMsg = sMsg & StringCHR13() & StringCHR10() & "Comments = (" & My.Application.Info.Description & ")"
        ' RDC 27062002
        sMsg = sMsg & " Sirius username: (" & sSiriusUsername & ")"
        sMsg = sMsg & " Calling App Name: (" & sCallingAppName & ")"
        sMsg = sMsg & " SourceID: (" & CStr(iSourceID) & ")"
        sMsg = sMsg & " LanguageID: (" & CStr(iLanguageID) & ")"
        sMsg = sMsg & " Connection Pooling/COM+ Mode: (" & CStr(bConnectionPooling) & ")"
        sMsg = sMsg & StringCHR13() & StringCHR10()


        'If Not Information.Nothing(vErrSource) Then

        '    vErrDesc = vErrDesc & StringCHR13() & StringCHR10() & "Source          : " & CStr(vErrSource)
        'End If

        ' Log Error Message
        LogDatabaseMessage(iType, sMsg, vApp, vClass, vMethod, vErrNo, vErrDesc)

        Exit Sub
Err_LogDatabaseError:


        ' Log Error Message
        LogDatabaseMessage(PMConstants.PMELogLevel.PMLogOnError, "LogDatabaseError failed.", "dPMDAO", "Database", "LogDatabaseError")

        Exit Sub
    End Sub

    ' ***************************************************************** '
    ' Name: LogDatabaseMessage
    '
    ' Description: Wrapper function to the log message method of the
    '              message object.
    '
    ' ***************************************************************** '
    Public Sub LogDatabaseMessage(ByRef iType As Integer, ByRef sMsg As String, Optional ByRef vApp As Object = Nothing, Optional ByRef vClass As Object = Nothing, Optional ByRef vMethod As Object = Nothing, Optional ByRef vErrNo As Object = Nothing, Optional ByRef vErrDesc As Object = Nothing)
        Try
            PMFunctions.LogMessageToFile(sUsername:=ACApp, iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod), excep:=New Exception(CStr(vErrDesc)), sErrUniqueId:=vErrNo)
        Catch
            Exit Sub
        End Try
    End Sub

    ''' <summary>
    ''' Outputs the Stored procedure's expected paraters
    ''' versus the actual parameters being passed into it.
    ''' Under ADO (without a .Prepare) these must match.
    ''' </summary>
    ''' <param name="r_oCmd"></param>
    ''' <param name="r_sMsg"></param>
    ''' <remarks></remarks>
    Public Sub LogParameters(ByRef r_oCmd As SqlCommand, ByRef r_sMsg As String)

        Dim iIndex As Integer
        Dim sText1 As New StringBuilder
        Dim bDifferent As Boolean
        Dim sADOType As String
        Dim vValue As String = ""

        Try

            r_sMsg = ""

            If r_oCmd Is Nothing Then
                Exit Sub
            End If

            If r_oCmd.Parameters.Count <= 0 Then
                Exit Sub
            End If

            iIndex = 1

            bDifferent = False

            For Each oADOParameter As SqlParameter In r_oCmd.Parameters

                r_sMsg = r_sMsg & Environment.NewLine

                sText1 = New StringBuilder("Parameter : " & oADOParameter.ParameterName)

                Select Case oADOParameter.DbType
                    Case DbType.Boolean
                        sADOType = "Boolean"
                    Case DbType.Currency
                        sADOType = "Currency"
                    Case DbType.String
                        sADOType = "String (char)"
                    Case DbType.String
                        sADOType = "String (varchar)"
                    Case DbType.Date, DbType.DateTime
                        sADOType = "DateTime (" & oADOParameter.DbType & ")"
                    Case DbType.Int32
                        sADOType = "Numeric"
                    Case DbType.Int32
                        sADOType = "Integer"
                    Case DbType.Decimal
                        sADOType = "Decimal"
                    Case DbType.Single
                        sADOType = "Single"
                    Case DbType.Int16
                        sADOType = "SmallInt"
                    Case DbType.UInt16
                        sADOType = "Unsigned TinyInt"
                    Case DbType.Int16
                        sADOType = "TinyInt"
                    Case DbType.Double
                        sADOType = "Double"
                    Case Else
                        sADOType = "Unknown (" & oADOParameter.DbType & ")"

                End Select
                Try
                    vValue = oADOParameter.Value
                    vValue = vValue
                    sText1.Append(" Value : (" & vValue & ") ")

                    sText1.Append("ADO Type : " & sADOType)

                    ' Is it an output parameter?
                    Select Case oADOParameter.Direction
                        Case ParameterDirection.Input
                            sText1.Append(" INPUT")
                        Case ParameterDirection.InputOutput
                            sText1.Append(" INPUTOUTPUT")
                        Case ParameterDirection.Output
                            sText1.Append(" OUTPUT")
                        Case ParameterDirection.ReturnValue
                            sText1.Append(" RETURN")
                        Case Else
                            sText1.Append(" UNKNOWN")
                    End Select

                    iIndex += 1

                    r_sMsg = r_sMsg & sText1.ToString()
                Catch ex As Exception

                End Try
            Next oADOParameter

            r_sMsg = r_sMsg & StringCHR13() & StringCHR10()

        Catch ex As Exception
            ' Log Error Message
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="LogParameters Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LogParameters")

        End Try

    End Sub
    Private Function StringCHR() As String
        Return ""
    End Function
    Private Function StringCHR10() As String
        Return ""
    End Function
    Private Function StringCHR13() As String
        Return ""
    End Function
End Module
