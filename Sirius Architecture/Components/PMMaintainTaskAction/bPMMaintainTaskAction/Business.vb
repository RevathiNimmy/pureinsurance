Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
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
    ' Name: GetDocumentTemplates
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-06-2003 : workflow
    ' ***************************************************************** '
    Public Function GetDocumentTemplates(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetDocumentTemplates"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=ACGetDocumentTemplatesSQL, sSQLName:=ACGetDocumentTemplatesName, bStoredProcedure:=True, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to retrieve document templates", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
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
    ' Name: AddTaskActionType
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-06-2003 : workflow
    ' ***************************************************************** '
    Public Function AddTaskActionType(ByRef r_lId As Integer, ByVal v_sCode As String, ByVal v_sDescription As String, ByVal v_bIsDeleted As Boolean, ByVal v_dtEffectiveDate As Date, ByVal v_lDueDays As Integer, ByVal v_sDocumentTemplateCode As String, ByVal v_bOutcomeNotEditable As Boolean) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddTaskActionType"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' add required parameters

            ' Id - this is an output param.
            m_lReturn = CType(AddInputParameter(v_sName:="PMWrk_Task_Action_Type_id", v_vValue:=0, v_iType:=gPMConstants.PMEDataType.PMString, v_iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput), gPMConstants.PMEReturnCode)

            ' Code
            m_lReturn = CType(AddInputParameter(v_sName:="code", v_vValue:=v_sCode, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            ' Description
            m_lReturn = CType(AddInputParameter(v_sName:="description", v_vValue:=v_sDescription, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            ' IsDeleted
            m_lReturn = CType(AddInputParameter(v_sName:="isdeleted", v_vValue:=v_bIsDeleted, v_iType:=gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)

            ' EffectiveDate
            m_lReturn = CType(AddInputParameter(v_sName:="effective_date", v_vValue:=v_dtEffectiveDate, v_iType:=gPMConstants.PMEDataType.PMDate), gPMConstants.PMEReturnCode)

            ' DueDays
            m_lReturn = CType(AddInputParameter(v_sName:="due_days", v_vValue:=v_lDueDays, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' DocumentTemplateCode
            m_lReturn = CType(AddInputParameter(v_sName:="document_template_code", v_vValue:=v_sDocumentTemplateCode, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            ' Outcome Not Editables
            m_lReturn = CType(AddInputParameter(v_sName:="outcome_not_editable", v_vValue:=v_bOutcomeNotEditable, v_iType:=gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)

            '0,'test','test',1,'01-01-2003',10,'DOC01',1

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=ACAddTaskActionTypeSQL, sSQLName:=ACAddTaskActionTypeName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("r_lId", r_lId)
                oDict.Add("v_sCode", v_sCode)
                oDict.Add("v_dtEffectiveDate", v_dtEffectiveDate)
                oDict.Add("v_sDocumentTemplateCode", v_sDocumentTemplateCode)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create new pmwrk_task_action_type", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
                '******************************
            Else
                ' retrieve new task action type id
                r_lId = m_oDatabase.Parameters.Item("PMWrk_Task_Action_Type_id").Value
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("r_lId", r_lId)
            oDict.Add("v_sCode", v_sCode)
            oDict.Add("v_dtEffectiveDate", v_dtEffectiveDate)
            oDict.Add("v_sDocumentTemplateCode", v_sDocumentTemplateCode)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************


            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateTaskActionType
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-06-2003 : workflow
    ' ***************************************************************** '
    Public Function UpdateTaskActionType(ByVal v_lId As Integer, ByVal v_sDescription As String, ByVal v_bIsDeleted As Boolean, ByVal v_dtEffectiveDate As Date, ByVal v_lDueDays As Integer, ByVal v_sDocumentTemplateCode As String, ByVal v_bOutcomeNotEditable As Boolean) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "UpdateTaskActionType"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' add required parameters

            ' PMWrk_Task_Action_Type_id
            m_lReturn = CType(AddInputParameter(v_sName:="PMWrk_Task_Action_Type_id", v_vValue:=v_lId, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            ' Description
            m_lReturn = CType(AddInputParameter(v_sName:="description", v_vValue:=v_sDescription, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            ' IsDeleted
            m_lReturn = CType(AddInputParameter(v_sName:="isdeleted", v_vValue:=v_bIsDeleted, v_iType:=gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)

            ' EffectiveDate
            m_lReturn = CType(AddInputParameter(v_sName:="effective_date", v_vValue:=v_dtEffectiveDate, v_iType:=gPMConstants.PMEDataType.PMDate), gPMConstants.PMEReturnCode)

            ' DueDays
            m_lReturn = CType(AddInputParameter(v_sName:="due_days", v_vValue:=v_lDueDays, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' DocumentTemplateCode
            m_lReturn = CType(AddInputParameter(v_sName:="document_template_code", v_vValue:=v_sDocumentTemplateCode, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            ' Outcome Not Editables
            m_lReturn = CType(AddInputParameter(v_sName:="outcome_not_editable", v_vValue:=v_bOutcomeNotEditable, v_iType:=gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=ACUpdateTaskActionTypeSQL, sSQLName:=ACUpdateTaskActionTypeName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lId", v_lId)
                oDict.Add("v_dtEffectiveDate", v_dtEffectiveDate)
                oDict.Add("v_sDocumentTemplateCode", v_sDocumentTemplateCode)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to update pmwrk_task_action_type :" & CStr(v_lId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lId", v_lId)
            oDict.Add("v_dtEffectiveDate", v_dtEffectiveDate)
            oDict.Add("v_sDocumentTemplateCode", v_sDocumentTemplateCode)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetMaintainData
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-06-2003 : workflow
    ' ***************************************************************** '
    Public Function GetMaintainData(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetMaintainData"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=ACGetMaintainDataSQL, sSQLName:=ACGetMaintainDataName, bStoredProcedure:=True, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
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
    ' Name: GetTaskOutcomes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-06-2003 : workflow
    ' ***************************************************************** '
    Public Function GetTaskOutcomes(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetTaskOutcomes"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=ACGetTaskOutcomesSQL, sSQLName:=ACGetTaskOutcomesName, bStoredProcedure:=True, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to get task_outcome records", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
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
    ' Name:GetTaskActionTypeOutcomes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-06-2003 : workflow
    ' ***************************************************************** '
    Public Function GetTaskActionTypeOutcomes(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetTaskActionTypeOutcomes"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=ACGetTaskActionTypeOutcomesSQL, sSQLName:=ACGetTaskActionTypeOutcomesName, bStoredProcedure:=True, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to get Pmwrk_task_Action_type_Outcome records", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
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


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateTaskActionTypeOutcomes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-06-2003 : workflow
    ' ***************************************************************** '
    Public Function UpdateTaskActionTypeOutcomes(ByVal v_lTaskActionTypeId As Integer, ByVal v_sTaskActionTypeOutcomeIds As String) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "UpdateTaskActionTypeOutcomes"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="PMWrk_Task_Action_Type_Id", v_vValue:=v_lTaskActionTypeId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            m_lReturn = CType(AddInputParameter(v_sName:="Task_Outcome_Ids", v_vValue:=v_sTaskActionTypeOutcomeIds, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=ACUpdateTaskActionTypeOutcomesSQL, sSQLName:=ACUpdateTaskActionTypeOutcomesName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lTaskActionTypeId", v_lTaskActionTypeId)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lTaskActionTypeId", v_lTaskActionTypeId)
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

