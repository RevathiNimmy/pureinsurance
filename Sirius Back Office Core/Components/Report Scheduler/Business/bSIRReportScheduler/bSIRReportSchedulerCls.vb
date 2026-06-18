Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System

'Developer Guide No.: 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 22/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' Database Class (Private)

    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lPMAuthorityLevel As Integer

    ' ***************************************************************** '
    '                        PUBLIC PROPERTIES
    ' ***************************************************************** '
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this object.
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise




        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
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
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

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
    ' Description: Entry point for any termination code for this object.
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
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Retrieve Scheduled Reports
    ' ***************************************************************** '

    Public Function GetScheduledReports(ByRef r_vScheduledReports(,) As Object, Optional ByVal v_lReportSchedulerID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetScheduledReports"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we were given an report scheduler id add it, else select all
            If v_lReportSchedulerID > 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "report_scheduler_id", v_lReportSchedulerID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            Else

                m_oDatabase.Parameters.Clear()
            End If

            ' Call sql

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectScheduledReportsSQL, sSQLName:=ACSelectScheduledReportsName, bStoredProcedure:=ACSelectScheduledReportsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vScheduledReports)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to get Schedule Report")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Adds a Report Scheduler 8.5
    ' ***************************************************************** '
    Public Function AddSchedulerReport(ByVal v_vParameters(,) As Object, ByVal v_sReportName As String, ByVal v_sFrequency As String, ByVal v_sReportPath As String) As Integer

        Dim result As Integer = 0
        Dim bTrans As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim v_ireport_scheduler_id As Integer

        Dim v_sName, v_sValue, v_sType, v_sPrompt, v_sCurrentIdValue, v_sPartySearch, v_sEmpty As String

        Const kMethodName As String = "AddSchedulerReport"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Start a transaction

            lReturn = m_oDatabase.SQLBeginTrans
            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                bTrans = True
            Else
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans", "Unable to create SQL transaction")
            End If


            m_oDatabase.Parameters.Clear()
            'Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "report_name", v_sReportName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "frequency", v_sFrequency, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "reportpath", v_sReportPath, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "report_scheduler_id", 0, gPMConstants.PMEParameterDirection.PMParamInputOutput, gPMConstants.PMEDataType.PMInteger)

            ' Call sql

            lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertScheduledReportsSQL, sSQLName:=ACInsertScheduledReportsName, bStoredProcedure:=ACInsertScheduledReportsStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to add Scheduler Report details")
            End If


            v_ireport_scheduler_id = m_oDatabase.Parameters.Item("report_scheduler_id").value

            If Information.IsArray(v_vParameters) And v_ireport_scheduler_id > 0 Then

                For iCount As Integer = v_vParameters.GetLowerBound(0) To v_vParameters.GetUpperBound(0)

                    v_sName = CStr(v_vParameters(iCount, 0))

                    v_sValue = CStr(v_vParameters(iCount, 1))

                    v_sType = CStr(v_vParameters(iCount, 2))

                    v_sPrompt = CStr(v_vParameters(iCount, 3))

                    v_sCurrentIdValue = CStr(v_vParameters(iCount, 4))

                    v_sPartySearch = CStr(v_vParameters(iCount, 5))

                    v_sEmpty = CStr(v_vParameters(iCount, 6))
                    ' Update Scheduled Report Details
                    lReturn = CType(AddSchedulerReportDetails(v_ireport_scheduler_id:=v_ireport_scheduler_id, v_sName:=v_sName, v_sValue:=v_sValue, v_sType:=v_sType, v_sPrompt:=v_sPrompt, v_sCurrentIdValue:=v_sCurrentIdValue, v_sPartySearch:=v_sPartySearch, v_sEmpty:=v_sEmpty), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("AddSchedulerReportDetails", "Unable to add Scheduled Report Details")
                    End If
                Next
            End If

            ' Commit the transaction

            lReturn = m_oDatabase.SQLCommitTrans
            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                bTrans = False
            Else
                gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans", "Unable to commit SQL transaction")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            ' If we are in a transaction when we get here roll it back!
            If bTrans Then

                m_oDatabase.SQLRollbackTrans()
            End If

            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    Public Function AddSchedulerReportDetails(ByVal v_ireport_scheduler_id As Integer, ByVal v_sName As String, ByVal v_sValue As String, ByVal v_sType As String, ByVal v_sPrompt As String, ByVal v_sCurrentIdValue As String, ByVal v_sPartySearch As String, ByVal v_sEmpty As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "AddSchedulerReportDetails"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()

            'Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "report_scheduler_id", v_ireport_scheduler_id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "parameter_name", v_sName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "default_value", v_sValue, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "data_type", v_sType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "prompt", v_sPrompt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "currentid_value", v_sCurrentIdValue, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "party_search", v_sPartySearch, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "empty", v_sEmpty, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            ' Call sql

            lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertScheduledReportsDetailsSQL, sSQLName:=ACInsertScheduledReportsDetailsName, bStoredProcedure:=ACInsertScheduledReportsDetailsStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to add Scheduler Report details")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            ' If we are in a transaction when we get here roll it back!

            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    Public Function DigScheduler(ByVal v_sReportName As String, ByRef r_iReportSchedulerId As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim r_vScheduledReportsDig(,) As Object = Nothing

        Const kMethodName As String = "DigScheduler"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "report_name", v_sReportName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True)

            ' Call sql

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectScheduledReportsDigSQL, sSQLName:=ACSelectScheduledReportsDigName, bStoredProcedure:=ACSelectScheduledReportsDigStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vScheduledReportsDig)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to get ri model")
            End If

            If Information.IsArray(r_vScheduledReportsDig) Then
                r_iReportSchedulerId = gPMFunctions.ToSafeInteger(r_vScheduledReportsDig(0, 0))
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Retrieve all report scheduler detail
    ' ***************************************************************** '
    Public Function GetReportSchedulerDetail(ByVal v_iReportSchedulerId As Integer, ByRef r_vReportSchedulerDetail(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetReportSchedulerDetail"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "report_scheduler_id", CStr(v_iReportSchedulerId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)

            ' Call sql

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectScheduledReportsDetailSQL, sSQLName:=ACSelectScheduledReportsDetailName, bStoredProcedure:=ACSelectScheduledReportsDetailStored, vResultArray:=r_vReportSchedulerDetail)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to get scheduled report detail")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    Public Function UpdateReportScheduler(ByVal v_iReportSchedulerId As Integer, ByVal v_vParameters(,) As Object, ByVal v_sFrequency As String, ByVal v_iExportToPDF As Integer, ByVal v_iArchieveToPDF As Integer, ByVal v_iExportToCSV As Integer, ByVal v_sSeprateBy As String) As Integer

        Dim result As Integer = 0
        Dim bTrans As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim iReportParameterId, iIsAutomatic As Integer

        Const kMethodName As String = "UpdateReportScheduler"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Start a transaction
            lReturn = m_oDatabase.SQLBeginTrans
            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                bTrans = True
            Else
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans", "Unable to create SQL transaction")
            End If

            m_oDatabase.Parameters.Clear()
            'Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "report_scheduler_id", v_iReportSchedulerId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "frequency", v_sFrequency, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "export_pdf", v_iExportToPDF, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "archieve_pdf", v_iArchieveToPDF, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "export_csv", v_iExportToCSV, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "seprateby", v_sSeprateBy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            ' Call sql

            lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateScheduledReportSQL, sSQLName:=ACUpdateScheduledReportName, bStoredProcedure:=ACUpdateScheduledReportStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to Update Scheduler Report details")
            End If

            If Information.IsArray(v_vParameters) Then
                For iCount As Integer = v_vParameters.GetLowerBound(0) To v_vParameters.GetUpperBound(0)
                    m_oDatabase.Parameters.Clear()

                    iReportParameterId = CInt(v_vParameters(iCount, 0))
                    iIsAutomatic = CInt(v_vParameters(iCount, 1))

                    bPMAddParameter.AddParameterLite(m_oDatabase, "report_scheduler_parameter_id", iReportParameterId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "is_automatic", iIsAutomatic, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                    ' Update Scheduled Report Details
                    lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateScheduledReportSQL, sSQLName:=ACUpdateScheduledReportName, bStoredProcedure:=ACUpdateScheduledReportStored)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to Update Scheduler Report details")
                    End If

                Next
            End If

            ' Commit the transaction
            lReturn = m_oDatabase.SQLCommitTrans
            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                bTrans = False
            Else
                gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans", "Unable to commit SQL transaction")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            ' If we are in a transaction when we get here roll it back!
            If bTrans Then

                m_oDatabase.SQLRollbackTrans()
            End If

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Delete an report scheduler detail
    ' ***************************************************************** '
    Public Function DeleteReportSchedulerDetail(ByVal v_iReportSchedulerId As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "DeleteReportSchedulerDetail"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "report_scheduler_id", v_iReportSchedulerId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)

            ' Call sql

            lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteScheduledReportsSQL, sSQLName:=ACDeleteScheduledReportsName, bStoredProcedure:=ACDeleteScheduledReportsStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to delete report scheduler detail")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    'Get Report Code corresponding to Report ID from Report Table. 
    Public Function GetCodeFromReportID(ByVal ReportId As Integer, ByRef r_vReportCode(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetCodeFromReportID"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "ReportID", ReportId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)

            ' Call sql
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectReportCodeSQL, sSQLName:=ACSSelectReportCodeName, bStoredProcedure:=ACSelectReportCodeStored, vResultArray:=r_vReportCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to get Report Code for ReportID")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '                           CLASS EVENTS
    ' ***************************************************************** '
    Public Sub New()
        MyBase.New()
        Exit Sub
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub
End Class
