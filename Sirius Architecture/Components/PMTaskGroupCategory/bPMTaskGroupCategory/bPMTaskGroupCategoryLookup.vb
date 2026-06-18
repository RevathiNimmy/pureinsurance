Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Lookup_NET.Lookup")> _
Public NotInheritable Class Lookup
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: LookupControl
    '
    ' Date: 22nd October 1998
    '
    ' Description: This class is used by the PMTaskGroupLookup control.
    '
    ' Edit History:
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
    Private Const ACClass As String = "LookupControl"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)
    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' RFC250398 - Product Family Property Get Added.
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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer




        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oComponentServices As PMServerBusinessCS
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

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

            '    Set oComponentServices = New PMServerBusinessCS


            If Information.IsNothing(vDatabase) Then
                '        lReturn = oComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, _
                'r_bNewInstanceCreated:=m_bCloseDatabase, _
                'r_oCheckedDatabase:=m_oDatabase)
                lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            Else
                '        lReturn = oComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, _
                'r_bNewInstanceCreated:=m_bCloseDatabase, _
                'r_oCheckedDatabase:=m_oDatabase, _
                'v_vDatabase:=vDatabase)

                lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        Set oComponentServices = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    Set oComponentServices = Nothing

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
    ' Name: GetInstanceCategory
    '
    ' Description: Return the Category details for a task instance.
    '
    ' ***************************************************************** '
    Public Function GetInstanceCategory(ByVal v_lPMTaskInstanceCnt As Integer, ByRef r_lPMTaskGroupCategoryID As Integer) As Integer

        Dim result As Integer = 0
        'developer guide no.112
        'Dim oFields As ADODB.Fields
        Dim oFields As DataRow

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Parameters
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="task_instance_cnt", vValue:=CStr(v_lPMTaskInstanceCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSql:=ACTaskGroupCategoryLookupSQL, ssqlname:=ACTaskGroupCategoryLookupName, bstoredprocedure:=ACTaskGroupCategoryLookupStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            ' If there aren't any Tasks then just return an empty string
            If m_oDatabase.Records.Count() < 1 Then
                Throw New Exception()
            End If

            oFields = m_oDatabase.Records.Item(1).Fields()

            r_lPMTaskGroupCategoryID = oFields("pmwrk_task_group_category_id")

            oFields = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInstanceCategory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstanceCategory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

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
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
