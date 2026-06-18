Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'Developer Guide no. 129
Imports SharedFiles
Friend NotInheritable Class Business
    Implements IDisposable
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

    Private m_oDatabase As Object
    Private m_oArcDatabase As Object
    Private m_bCloseDatabase As Boolean
    Private m_bCloseArcDatabase As Boolean
    Private m_lCurrentRecord As Integer

    '*************************
    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    '*************************

    ' ************************************************
    ' Added to replace global variables 18/09/2003
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    ' ***************************************************************** '
    ' Name: IsBranchValid
    '
    ' Parameters: n/a
    '
    ' Description: Validates the specified branch code exists in the
    '               source table in the sirius db
    '
    ' History:
    '           Created : MEvans : 10-02-2005 : Credit Control RetroFit
    ' ***************************************************************** '
    Public Function IsBranchValid(ByVal v_sBranchCode As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "IsBranchValid"

        Dim lReturn As Integer
        Dim vResults(,) As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            lReturn = AddInputParameter(v_sName:="Code", v_vValue:=v_sBranchCode, v_iType:=gPMConstants.PMEDataType.PMString)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kIsBranchValidSQL, sSQLName:=kIsBranchValidName, bStoredProcedure:=True, vResultArray:=vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kIsBranchValidSQL, "Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            ' if there are no results this isnt a valid branch
            If Not Information.IsArray(vResults) Then
                gPMFunctions.RaiseError(kMethodName, kIsBranchValidSQL & " returned no results", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result

            '		Resume 
            '		Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: CreditControlProcessing
    '
    ' Parameters: n/a
    '
    ' Description: Calls out to Finance Spoke to process credit
    '               control items for the specified params
    '
    ' History:
    '           Created : MEvans : 10-02-2005 : Credit Control RetroFit
    ' ***************************************************************** '
    Public Function CreditControlProcessing(ByVal v_sBranchCode As String, ByVal v_dtDate As Date, ByVal v_bSpool As Boolean, ByVal v_bArchive As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreditControlProcessing"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sTitle, sMessage, sStatusCode As String
        Dim vHeaderData(1) As Object
        Dim vHeaderDetail(12) As Object
        Dim oFinanceSpoke As bACTFinanceSpoke.Business

        Try



            result = gPMConstants.PMEReturnCode.PMTrue



            oFinanceSpoke = New bACTFinanceSpoke.Business
            lReturn = oFinanceSpoke.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "gPMComponentServices.CreateBusinessObject bACTFinanceSpoke.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set the vHeaderData Elements

            vHeaderData(0) = "SIRIUS"

            vHeaderData(1) = vHeaderDetail

            vHeaderData(1)(kbHDBranch) = v_sBranchCode

            vHeaderData(1)(kbHDAsOfDate) = DateTime.Parse(v_dtDate).ToString("d")

            vHeaderData(1)(kbHDSpoolDoc) = v_bSpool

            vHeaderData(1)(kbHDArchiveDoc) = v_bArchive

            ' Set the status and message variables
            sStatusCode = ACSpokeStatusCode
            sMessage = ACSpokeMessage

            'Process using m_obusiness object

            lReturn = oFinanceSpoke.Export(v_sInterfaceCode:=ACSpokeInterfaceCode, r_sBatchRef:=ACSpokeBatch, r_sStatusCode:=sStatusCode, r_sMessage:=sMessage, r_sHeaderXML:=ACSpokeHeaderXML, r_vHeaderData:=vHeaderData, r_vDetailData:=ACSpokeDetailData)

            If lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMNotFound

            ElseIf lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bFinanceSpoke.Export Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CreateProcessFailedTask
    '
    ' Parameters: n/a
    '
    ' Description: Creates a MEMO task against the SYSADMIN user group
    '               indicating that the batch credit control processing
    '                task has failed and why it failed.
    '
    ' History:
    '           Created : MEvans : 10-02-2005 : Credit Control RetroFit
    ' ***************************************************************** '
    Public Function CreateProcessFailedTask(ByVal v_sBranchCode As String, ByVal v_sDate As String, ByVal v_sDescription As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateProcessFailedTask"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oWrkMgrTaskControl As bPMWrkTaskInstance.TaskControl
        Dim lTaskInstanceCnt As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' create an instance of work task instance component


            oWrkMgrTaskControl = New bPMWrkTaskInstance.TaskControl
            lReturn = oWrkMgrTaskControl.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "gPMComponentServices.CreateBusinessObject bPMWrkTaskInstance.TaskControl Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' create a memo task to indicate that the batch process failed...
            ' hardcoded task values for now..

            lReturn = oWrkMgrTaskControl.CreateNew(v_lPMWrkTaskGroupID:=1, v_lPMWrkTaskID:=18, v_sCustomer:="Batch Process Failed", v_dtTaskDueDate:=DateTime.Now, v_lPMUserGroupID:=1, v_sDescription:="Batch Call to Credit Control Processing" & _
                      " Failed at :- " & DateTimeHelper.ToString(DateTime.Now) & _
                      ". Branch =" & v_sBranchCode & _
                      ". Date:=" & v_sDate & _
                  ". Reason:=" & v_sDescription, v_iTaskStatus:=0, v_iIsUrgent:=1, r_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt, v_iIsVisible:=gPMConstants.PMEReturnCode.PMTrue)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bPMWrkTaskInstance.TaskControl.CreateNew Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
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
    ' ***************************************************************** '
    Private Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddInputParameter"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object

        If m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType) <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error.
            result = gPMConstants.PMEReturnCode.PMFalse


            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to add parameter name:" & v_sName & _
                                              ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))

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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel



            lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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
    ' END STANDARD METHODS
    ' ***************************************************************** '
End Class

