Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

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


    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date:
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required.
    '
    '
    ' History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    Private m_oDatabase As dPMDAO.Database
    Private m_oArcDatabase As dPMDAO.Database
    Private m_bCloseDatabase As Boolean
    Private m_bCloseArcDatabase As Boolean
    Private m_lCurrentRecord As Integer
    Private m_lReturn As gPMConstants.PMEReturnCode

    '*************************
    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    '*************************

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    ' ***************************************************************** '
    ' Name: GetTaskGroupTaskDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-06-2003 : workflow
    ' ***************************************************************** '
    Public Function GetTaskGroupTaskDetails(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetTaskGroupTaskDetails"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=ACGetTaskGroupTaskDetailsSQL, sSQLName:=ACGetTaskGroupTaskDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to retrive PMWrk_Task_Group_Task details", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTaskGroupTaskActionTypes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-06-2003 : workflow
    ' ***************************************************************** '
    Public Function GetTaskGroupTaskActionTypes(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetTaskGroupTaskActionTypes"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=ACGetTaskGroupActionTypesSQL, sSQLName:=ACGetTaskGroupActionTypesName, bStoredProcedure:=True, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to get PMWrk_Task_Group_Task_Action_Type records", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTaskActionTypes
    '
    ' Parameters: n/a
    '
    ' Description: Returns all non deleted records from
    '                   pmwrk_task_action_type
    '
    ' History:
    '           Created : MEvans : 25-06-2003 : workflow
    ' ***************************************************************** '
    Public Function GetTaskActionTypes(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetTaskActionTypes"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=ACGetTaskActionTypeSQL, sSQLName:=ACGetTaskActionTypeSQL, bStoredProcedure:=True, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to retrieve non delete pmwrk_task_action_types", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '******************************


            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateTaskGroupTaskActions
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-06-2003 : workflow
    ' ***************************************************************** '
    Public Function UpdateTaskGroupTaskActions(ByVal v_lTaskGroupId As Integer, ByVal v_lTaskId As Integer, ByVal v_sTaskActionTypeIds As String) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "UpdateTaskGroupTaskActions"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="PMWrk_Task_Group_Id", v_vValue:=v_lTaskGroupId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            m_lReturn = CType(AddInputParameter(v_sName:="PMWrk_Task_Id", v_vValue:=v_lTaskId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            m_lReturn = CType(AddInputParameter(v_sName:="Task_Action_Ids", v_vValue:=v_sTaskActionTypeIds, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=ACUpdateTaskGroupTaskActionsSQL, sSQLName:=ACUpdateTaskGroupTaskActionsSQL, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lTaskGroupId", v_lTaskGroupId)
                oDict.Add("v_lTaskId", v_lTaskId)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lTaskGroupId", v_lTaskGroupId)
            oDict.Add("v_lTaskId", v_lTaskId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************

            Return result

        End Try
    End Function







































    ' ***************************************************************** '
    ' Name: AddInputParameter
    '
    ' Parameters: v_sName   : Parameter Name
    '             v_vValue  : Parameter Value
    '             v_iType   : Parameter DataType
    '
    ' Description: Adds an input parameter to the database parameters
    '
    ' History:
    '           Created : MEvans : 18-12-2002 : 202
    ' ***************************************************************** '
    Private Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer, Optional ByRef v_iDirection As Integer = gPMConstants.PMEParameterDirection.PMParamInput) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddInputParameter"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object

        If m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), idirection:=v_iDirection, iDataType:=v_iType) <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' STANDARD METHODS
    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise




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



            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("iUserID", iUserID)
            oDict.Add("iSourceID", iSourceID)
            oDict.Add("iLanguageID", iLanguageID)
            oDict.Add("iCurrencyID", iCurrencyID)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep, oDicParms:=oDict)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CType(CInt(vProcessMode), gPMConstants.PMEProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("vEffectiveDate", vEffectiveDate)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", excep:=excep, oDicParms:=oDict)

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
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Public Function BeginTrans() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "BeginTrans"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Terminate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Public Function CommitTrans() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "CommitTrans"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Terminate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Public Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "RollbackTrans"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Terminate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' END STANDARD METHODS
    ' ***************************************************************** '
End Class

