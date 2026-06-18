Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("FormClass_NET.FormClass")> _
Public NotInheritable Class FormClass
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: FormClass
    '
    ' Date: 26/10/1998
    '
    ' Description:
    '
    '
    ' Edit History:
    '
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 18/09/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "FormClass"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lError As gPMConstants.PMEReturnCode
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

        End Get
    End Property

    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise

        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Initialisation Code.

            ' Set Username and Password

            ' Set User ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            ' Check the Supplied Database.
            '    Set oCS = New sPMServerCS.PMServerBusinessCS
            '    m_lError = oCS.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID,  _
            'v_lPMProductFamily:=PMProductFamily, _
            'r_bNewInstanceCreated:=m_bCloseDatabase, _
            'r_oCheckedDatabase:=m_oDatabase, _
            'v_vDatabase:=vDatabase)

            m_lError = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            '    Set oCS = Nothing

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: AddLogEntry
    '
    ' Description: Adds a New Log Entry into the Databaase.
    '
    '
    ' ***************************************************************** '
    Public Function AddLogEntry(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_dtDateCreated As Date, ByVal v_sText As String, ByVal v_iCreatedByID As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lRecordsAffected As Integer
        Dim sString As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sString = v_sText

            lReturn = CType(GenApostrophes(sString:=sString), gPMConstants.PMEReturnCode)

            With m_oDatabase

                .Parameters.Clear()

                lReturn = .Parameters.Add(sName:="pmwrk_task_instance_cnt", vValue:=CStr(v_lPMWrkTaskInstanceCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMLong)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Developer Guide no. 40
                'lReturn = .Parameters.Add(sName:="date_created", vValue:=DateTimeHelper.ToString(v_dtDateCreated), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMDate)
                lReturn = .Parameters.Add(sName:="date_created", vValue:=v_dtDateCreated, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                lReturn = .Parameters.Add(sName:="text", vValue:=sString, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMString)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                lReturn = .Parameters.Add(sName:="created_by_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMInteger)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                lReturn = .SQLAction(sSql:=ACAddLogEntrySQL, ssqlname:=ACAddLogEntryName, bstoredprocedure:=ACAddLogEntryStored, lRecordsAffected:=lRecordsAffected)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If lRecordsAffected <> 1 Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddLogEntryFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddLogEntry", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTaskInstLogEntries
    '
    ' Description: This method gets all of the Log Entries for a single
    '              Task Instance.
    '
    ' ***************************************************************** '
    'developers guid no 101
    'Public Function GetTaskInstLogEntries(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByRef r_vLogEntriesArray As String) As Integer
    Public Function GetTaskInstLogEntries(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByRef r_vLogEntriesArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                lReturn = .Parameters.Add(sName:="pmwrk_task_instance_cnt", vValue:=CStr(v_lPMWrkTaskInstanceCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMLong)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                lReturn = .SQLSelect(sSql:=ACSelAllDescSQL, ssqlname:=ACSelAllDescName, bstoredprocedure:=ACSelAllDescStored, lnumberrecords:=gPMConstants.PMAllRecords, vresultarray:=r_vLogEntriesArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_vLogEntriesArray = Nothing
                    Return result
                End If

            End With

            If Not Information.IsArray(r_vLogEntriesArray) Then
                r_vLogEntriesArray = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTaskInstLogEntriesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskInstLogEntries", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)
    '******************************************************************************
    ' GenApostrophes
    '
    ' Take a string and replace ' with ''
    '
    '******************************************************************************
    Private Function GenApostrophes(ByRef sString As String) As Integer

        Dim result As Integer = 0
        Dim i As Integer
        Dim sTemp As New StringBuilder



        result = gPMConstants.PMEReturnCode.PMTrue

        If sString.Length = 0 Then
            Return result
        End If

        sTemp = New StringBuilder("")

        Do
            i = (sString.IndexOf("'"c) + 1)

            If i = 0 Then
                sTemp.Append(sString)
                Exit Do
            End If

            sTemp.Append(sString.Substring(0, i - 1) & "''")
            sString = sString.Substring(i)
        Loop

        sString = sTemp.ToString()

        Return result

    End Function

    ' PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

