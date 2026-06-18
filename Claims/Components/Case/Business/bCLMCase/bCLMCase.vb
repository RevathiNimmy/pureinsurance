Option Strict Off
Option Explicit On
Imports System.Text
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable


    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 18/06/2007
    '
    ' Description: Creatable Bussiness class which contains all the
    '              methods, business rules required for the
    '              bCLMCase.
    '
    ' Edit History:VB
    ' ***************************************************************** '


    ' ************************************************
    ' Module variables
    ' ************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"


    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As Integer
    Private m_lError As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_oCaseNumbering As bSIRPolicyNumMaint.Business
    Private m_oEvent As bSIREvent.Business
    Private oGis As Object = Nothing

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get
            Return m_iTask
        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get
            Return m_lNavigate
        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get
            Return m_lProcessMode
        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
    End Property


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' Date :18/06/2007
    '
    ' Edit History :VB
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


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

            ' Set Username and Password
            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now



            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' Date :15/07/2000
    '
    ' Edit History :VB
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
    '
    ' Date :15/07/2000
    '
    ' Edit History :VB
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.

            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Sub New()
        MyBase.New()

        Try

            Dim vDatabase As Object = Nothing

            ' Class Initialise
            m_oDatabase = New dPMDAO.Database()


            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    '=============================================================
    'New Code
    '=============================================================


    ' ***************************************************************** '
    ' Name: LoadCase
    ' Parameters: n/a
    ' Description:
    ' History:
    ' Created : VB : 18-06-2007
    ' ***************************************************************** '
    Public Function LoadCase(ByVal v_lCaseId As Integer, ByRef r_sCaseNumber As String, ByRef r_dtCaseOpenedDate As Date, ByRef r_lCaseProgressStatusID As Integer, ByRef r_lCaseAnalystID As Integer, ByRef r_lCaseAssistantID As Integer, ByRef r_lCaseVersion As Integer, ByRef r_lBaseCaseID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "LoadCase"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResults(,) As Object = Nothing

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "Case_ID", v_lCaseId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Execute Action Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kFindClaimCaseSQL, sSQLName:=kFindClaimCaseName, bStoredProcedure:=True, vResultArray:=vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kFindClaimCaseSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vResults) Then

                r_sCaseNumber = gPMFunctions.ToSafeString(CStr(vResults(kICaseNumber, 0)))

                r_dtCaseOpenedDate = gPMFunctions.ToSafeDate(CStr(vResults(kICaseOpenedDate, 0)))

                r_lCaseProgressStatusID = gPMFunctions.ToSafeLong(CStr(vResults(kICaseProgressStatusID, 0)))

                r_lCaseAnalystID = gPMFunctions.ToSafeLong(CStr(vResults(kICaseAnaystID, 0)))

                r_lCaseAssistantID = gPMFunctions.ToSafeLong(CStr(vResults(kICaseAssistantID, 0)))

                r_lCaseVersion = gPMFunctions.ToSafeLong(CStr(vResults(kICaseVersion, 0)))

                r_lBaseCaseID = gPMFunctions.ToSafeLong(CStr(vResults(kIBaseCaseID, 0)))
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name       : SaveCase
    ' Parameters : n/a
    ' Description:
    ' History    :
    ' Created    : 18-06-2007 (VB)
    ' ***************************************************************** '
    Public Function SaveCase(ByVal v_lCaseId As Integer, ByVal v_sCaseNumber As String, ByVal v_dtCaseOpenedDate As Date, ByVal v_lCaseProgressStatusID As Integer, ByVal v_lCaseAnalystID As Integer, ByVal v_lCaseAssistantID As Integer, ByVal v_lCaseVersion As Integer, ByRef r_lNewCaseID As Integer, ByRef r_lBaseCaseID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SaveCase"

        Dim lReturn As gPMConstants.PMEReturnCode
        'Dim lStatus As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue


            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "case_id", v_lCaseId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "case_number", v_sCaseNumber, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "case_opened_date", v_dtCaseOpenedDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "case_version", v_lCaseVersion, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "case_progress_id", v_lCaseProgressStatusID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "analyst_handler_id", v_lCaseAnalystID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            '--------
            bPMAddParameter.AddParameterLite(m_oDatabase, "admin_handler_id", v_lCaseAssistantID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            '--------
            bPMAddParameter.AddParameterLite(m_oDatabase, "base_case_id", r_lBaseCaseID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "user_id", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute Action Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kSaveCaseSQL, sSQLName:=kSaveCaseName, bStoredProcedure:=True, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kSaveCaseSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' If there are No RECORDS return NOT Found
            If m_oDatabase.Records.Count() > 0 Then
                ' Return the Record
                'developer guide no.111
                r_lNewCaseID = gPMFunctions.ToSafeLong(m_oDatabase.Records.Item(0).Fields()("new_case_id"))
                r_lBaseCaseID = gPMFunctions.ToSafeLong(m_oDatabase.Records.Item(0).Fields()("base_case_id"))
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost.
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here.

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetClientCode
    ' Description:
    ' History: VB
    ' ***************************************************************** '
    Public Function GenerateCaseCode(ByRef r_sCaseCode As String, Optional ByVal v_iclaimid As Integer = 0, Optional ByVal v_lReturnError As Integer = 0, Optional ByVal v_sFailureReason As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GenerateCaseCode"

        Dim sFailureReason As String = ""

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oCaseNumbering Is Nothing Then

                ' Create the object

                m_oCaseNumbering = New bSIRPolicyNumMaint.Business
                m_lReturn = m_oCaseNumbering.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    gPMFunctions.RaiseError(kMethodName, "gPMComponentServices.CreateBusinessObject" & " Failed", gPMConstants.PMELogLevel.PMLogError)
                    Return result
                End If
            End If


            m_lReturn = m_oCaseNumbering.GenerateCaseCode(v_iSourceID:=m_iSourceID, r_sGeneratedCaseCode:=r_sCaseCode, r_sFailureReason:=sFailureReason, v_iClaimId:=v_iclaimid)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                'Log Error.
                gPMFunctions.RaiseError(kMethodName, "m_oCaseNumbering.GenerateClientCode" & " Failed", gPMConstants.PMELogLevel.PMLogError)
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                'MessageBox.Show("Case numbering scheme is not set.", "Case Numbering", MessageBoxButtons.OK, MessageBoxIcon.Error)
                result = gPMConstants.PMEReturnCode.PMCancel
                v_lReturnError = gPMConstants.PMEReturnCode.PMNotFound
                Return result
            ElseIf sFailureReason <> "" Then
                ' MessageBox.Show(sFailureReason, "Case Numbering", MessageBoxButtons.OK, MessageBoxIcon.Error)
                result = gPMConstants.PMEReturnCode.PMCancel
                sFailureReason = sFailureReason
                Return result
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost.
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here.

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name          : GetLinks
    ' Description   :
    ' History       : VB
    ' ***************************************************************** '

    Public Function GetLinks(ByVal v_lBaseCaseID As Integer, ByRef r_vLinks(,) As Object) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "GetLinks"

        ' Dim lReturn, lStatus As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "basecaseid", v_lBaseCaseID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute Action Query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetCaseClaimLinksSQL, sSQLName:=kGetCaseClaimLinksName, bStoredProcedure:=True, vResultArray:=r_vLinks, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetCaseClaimLinksSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: LinkClaims
    ' Description:
    ' History: VB
    ' ***************************************************************** '
    Public Function LinkClaims(ByVal v_lBaseCaseID As Integer, ByVal v_vLinkArray As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "LinkClaims"

        Dim lReturn As gPMConstants.PMEReturnCode
        ' Dim lStatus,
        Dim lClaimId As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            If Informations.IsArray(v_vLinkArray) Then

                For Each v_vLinkArray_item As Object In v_vLinkArray


                    lClaimId = gPMFunctions.ToSafeLong(CStr(v_vLinkArray_item))

                    ' Base Case Id
                    bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", lClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                    ' Claim Id
                    bPMAddParameter.AddParameterLite(m_oDatabase, "base_case_id", v_lBaseCaseID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)

                    ' Execute Action Query
                    lReturn = m_oDatabase.SQLAction(sSQL:=kUpdateClaimCaseLinkSQL, sSQLName:=kUpdateClaimCaseLinkName, bStoredProcedure:=True)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, kUpdateClaimCaseLinkSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Next v_vLinkArray_item

            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: UnlinkClaims
    ' Description:
    ' History: VB
    ' ***************************************************************** '
    Public Function UnlinkClaims(ByVal v_vUnlinkArray() As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UnlinkClaims"

        Dim lReturn As gPMConstants.PMEReturnCode
        ' Dim lStatus,
        Dim lClaimId As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If Informations.IsArray(v_vUnlinkArray) Then

                For Each v_vUnlinkArray_item As Object In v_vUnlinkArray


                    lClaimId = gPMFunctions.ToSafeLong(CStr(v_vUnlinkArray_item))
                    ' Claim Id
                    bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", lClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                    ' Base Case Id

                    bPMAddParameter.AddParameterLite(m_oDatabase, "base_case_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)

                    ' Execute Action Query
                    lReturn = m_oDatabase.SQLAction(sSQL:=kUpdateClaimCaseLinkSQL, sSQLName:=kUpdateClaimCaseLinkName, bStoredProcedure:=True)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, kUpdateClaimCaseLinkSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Next v_vUnlinkArray_item
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name          : GetClaimDetail
    ' Description   :
    ' History       : VB
    ' ***************************************************************** '
    Public Function GetClaimDetail(ByVal v_lClaimID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimDetail"

        ' Dim lReturn, lStatus As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", v_lClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute Action Query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kFindClaimSQL, sSQLName:=kFindClaimName, bStoredProcedure:=True, vResultArray:=r_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetCaseClaimLinksSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name          : FindCase
    ' Description   :
    ' History       : VB
    ' ***************************************************************** '
    Public Function FindCase(ByVal v_sSQL As String,
                         ByRef r_vResultArray(,) As Object,
                         Optional ByVal v_lNumberRecords As Long = -1) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "FindCase"

        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        If v_lNumberRecords <> -1 Then
            m_lReturn = m_oDatabase.SQLSelect(
                               sSQL:=v_sSQL,
                               sSQLName:="FindCase",
                               bStoredProcedure:=False,
                               vResultArray:=r_vResultArray,
                               lNumberRecords:=v_lNumberRecords)
        Else
            ' Execute Action Query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=v_sSQL, sSQLName:="FindCase", bStoredProcedure:=False, vResultArray:=r_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: BeginTrans
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Public Function BeginTrans() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "BeginTrans"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = m_oDatabase.SQLBeginTrans()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SQLBeginTrans Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function




    ' ***************************************************************** '
    ' Name: RollbackTrans
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Public Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RollbackTrans"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = m_oDatabase.SQLRollbackTrans()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SQLRollbackTrans Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: CommitTrans
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Public Function CommitTrans() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CommitTrans"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = m_oDatabase.SQLCommitTrans()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function




    ' ***************************************************************** '
    ' Name          : GetClaimDetail
    ' Description   :
    ' History       : VB
    ' ***************************************************************** '
    Public Function GetCaseHistory(ByVal v_lBaseCaseID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCaseHistory"

        ' Dim lReturn, lStatus As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "basecaseid", v_lBaseCaseID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute Action Query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kCaseHistorySQL, sSQLName:=kCaseHistoryName, bStoredProcedure:=True, vResultArray:=r_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kCaseHistorySQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CloseCase
    ' Parameters: n/a
    ' Description:
    ' History:
    ' Created : VB : 18-06-2007
    ' ***************************************************************** '
    Public Function CloseCase(ByVal v_lCaseId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CloseCase"

        Dim lReturn As gPMConstants.PMEReturnCode
        ' Dim vResults As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "case_id", v_lCaseId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Execute Action Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kCloseCaseSQL, sSQLName:=kCloseCaseName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kFindClaimCaseSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: CreateEvent (Private)
    '
    ' Description: Create an event record.
    '
    ' History: Made function Public, to enable it to be called directly from GII wrapper
    '
    ' ***************************************************************** '
    Public Function CreateEvent(ByVal v_lCaseId As Integer, ByVal v_sEventTypeCode As String, ByVal v_dtEventDate As Date, ByVal v_vDescription As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateEvent"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vArray(,) As Object = Nothing
        Dim lEventTypeId As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oEvent Is Nothing Then
                m_oEvent = New bSIREvent.Business()

                lReturn = m_oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                ' Check for errors

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, " Failed to initialise the event object", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If


            bPMAddParameter.AddParameterLite(m_oDatabase, "eventtypecode", v_sEventTypeCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True)

            ' Execute Action Query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetEventTypeSQL, sSQLName:=kGetEventTypeName, bStoredProcedure:=True, vResultArray:=vArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetCaseClaimLinksSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vArray) Then

                lEventTypeId = gPMFunctions.ToSafeLong(CStr(vArray(0, 0)))
            End If

            ' Directly add the event
            lReturn = m_oEvent.DirectAdd(vCaseID:=v_lCaseId, vEventType:=lEventTypeId, vPartyCnt:=0, vUserId:=m_iUserID, vEventDate:=v_dtEventDate, vDescription:=v_vDescription)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, " Failed to add the event", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GenerateSQL
    ' Parameters:
    ' Description:
    ' History:
    ' Created :
    ' ***************************************************************** '
    'developer guide no.101
    Public Function GenerateSQL(ByRef r_sSQL As String, Optional ByVal v_sClaimNumber As String = "", Optional ByVal v_sCaseNumber As String = "", Optional ByVal v_lRiskTypeID As Integer = 0, Optional ByVal v_lProgressStatusID As Integer = 0, Optional ByVal v_vCaseOpenDate As Object = "", Optional ByVal v_vSearchFields As Object = Nothing, Optional ByVal v_vAgentKey As Object = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GenerateSQL"

        Dim lReturn As Integer
        Dim oBusiness As bGISMaintainDataDictionary.Business = Nothing
        Dim sSQL = " ", sSQLWhere = "", sSQLJoin As String = ""
        ' sWhere, sJoin,
        Dim bIsWhere As Boolean
        ' Dim cSearchFields As ArrayList 

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            If Not Informations.IsNothing(v_vSearchFields) Then
                ' Get an instance of the business object
                ' the public object manager.

                oBusiness = New bGISMaintainDataDictionary.Business
                lReturn = oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    gPMFunctions.RaiseError(kMethodName, "gPMComponentServices.CreateBusinessObject" & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If


                lReturn = oBusiness.GetSearchFieldsSQL(cSearchFields:=v_vSearchFields, r_sSQLJoins:=sSQLJoin, r_sSQLWhere:=sSQLWhere)

                ' Check for errors
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetSearchFieldsSQL Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            sSQL = "SELECT DISTINCT TOP 500 c.case_id, c.case_number, c.case_opened_date, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "(SELECT description FROM handler WHERE handler_id = c.analyst_handler_id ) 'analyst' , " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "(SELECT description FROM handler WHERE handler_id = c.admin_handler_id) 'assistant', " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "cap.description, " & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " ISNULL((SELECT SUM((r.initial_reserve + r.revised_reserve) - r.paid_to_date ) FROM " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " (SELECT  base_claim_id, MAX(claim_id) ClaimVer_Claim_id " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "      From claim " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "      Where c.base_case_id = claim.base_case_id And is_dirty = 0 " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "      GROUP BY base_case_id,base_claim_id) claim_version INNER JOIN " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Claim_Peril cp ON cp.claim_id=claim_version.ClaimVer_Claim_id INNER JOIN " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " reserve r ON r.claim_peril_id=cp.claim_peril_id INNER JOIN " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " reserve_type rt ON r.reserve_type_id = rt.reserve_type_id " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " AND rt.is_indemnity=1),0) 'total_indemnity', " & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " ISNULL((SELECT SUM((r.initial_reserve + r.revised_reserve) - r.paid_to_date ) FROM " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "(SELECT  base_claim_id, MAX(claim_id) ClaimVer_Claim_id " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "      From claim " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "      Where c.base_case_id = claim.base_case_id And is_dirty = 0 " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "      GROUP BY base_case_id,base_claim_id) claim_version INNER JOIN " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Claim_Peril cp ON cp.claim_id=claim_version.ClaimVer_Claim_id INNER JOIN " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " reserve r ON r.claim_peril_id=cp.claim_peril_id INNER JOIN " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " reserve_type rt ON r.reserve_type_id = rt.reserve_type_id " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " AND rt.is_expense = 1),0) 'total_expense', " & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " ISNULL((SELECT SUM((r.initial_reserve + r.revised_reserve) - r.paid_to_date ) FROM " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " (SELECT  base_claim_id, MAX(claim_id) ClaimVer_Claim_id " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "     From claim " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "     Where c.base_case_id = claim.base_case_id And is_dirty = 0 " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "     GROUP BY base_case_id,base_claim_id) claim_version INNER JOIN " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Claim_Peril cp ON cp.claim_id=claim_version.ClaimVer_Claim_id INNER JOIN " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " reserve r ON r.claim_peril_id=cp.claim_peril_id INNER JOIN " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " reserve_type rt ON r.reserve_type_id = rt.reserve_type_id " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " AND rt.is_excess=1),0) 'total_excess', " & Strings.ChrW(13) & Strings.ChrW(10)


            sSQL = sSQL & "c.base_case_id," & Strings.ChrW(13) & Strings.ChrW(10)
            ''62125
            sSQL = sSQL & "0" & Strings.ChrW(13) & Strings.ChrW(10)

            'FROM
            sSQL = sSQL & "FROM [case]  c" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN case_progress  cap" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON c.case_progress_id = cap.case_progress_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "INNER JOIN" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "(SELECT MAX(case_version) as case_version,MAX(case_id) as case_id, base_case_id FROM [case]" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE is_dirty_case=0 GROUP BY base_case_id ) case_version" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON c.case_id = case_version.case_id" & Strings.ChrW(13) & Strings.ChrW(10)
            ''62125
            sSQL = sSQL & " LEFT OUTER JOIN CLAIM ON c.base_case_id = CLAIM.base_case_id And is_dirty = 0 " &
                   " LEFT OUTER JOIN INSURANCE_FILE ON CLAIM.Policy_id = INSURANCE_FILE.Insurance_file_cnt " & Strings.ChrW(13) & Strings.ChrW(10)


            If Not Informations.IsNothing(v_vSearchFields) Then
                'Join
                sSQL = sSQL & sSQLJoin & Strings.ChrW(13) & Strings.ChrW(10)

                If sSQLWhere.ToUpper().IndexOf("WHERE") >= 0 Then
                    bIsWhere = True
                    'Where
                    sSQL = sSQL & sSQLWhere & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            Else
                'Where Claim
                If v_sClaimNumber.Trim() <> "" Or v_lRiskTypeID > 0 Then
                    sSQL = sSQL & "JOIN claim  cl1" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "ON cl1.base_case_id = c.base_case_id" & Strings.ChrW(13) & Strings.ChrW(10)
                End If

                'risk_type
                If v_lRiskTypeID > 0 Then
                    sSQL = sSQL & "JOIN risk rkt" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "ON cl1.risk_type_id=rkt.risk_cnt " & Strings.ChrW(13) & Strings.ChrW(10)
                End If

                m_lReturn = CType(bPMFunc.ValidateSQL(v_sCaseNumber), gPMConstants.PMEReturnCode)

                If v_sCaseNumber.Trim() <> "" Then
                    sSQL = sSQL & "WHERE "
                    bIsWhere = True
                    sSQL = sSQL & "c.case_number LIKE '" & v_sCaseNumber.Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                End If
                If gPMFunctions.ToSafeString(v_vCaseOpenDate) = "00:00:00" Then
                    v_vCaseOpenDate = ""
                End If
                If gPMFunctions.ToSafeString(v_vCaseOpenDate) <> "" Then
                    If bIsWhere Then
                        sSQL = sSQL & "AND " & Strings.ChrW(13) & Strings.ChrW(10)
                    Else
                        sSQL = sSQL & "WHERE " & Strings.ChrW(13) & Strings.ChrW(10)
                        bIsWhere = True
                    End If
                    sSQL = sSQL & "c.case_opened_date = '" & gPMFunctions.ToSafeDate(v_vCaseOpenDate).ToString("yyyy-MM-dd") & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                End If

                If v_sClaimNumber.Trim() <> "" Then
                    If bIsWhere Then
                        sSQL = sSQL & "AND " & Strings.ChrW(13) & Strings.ChrW(10)
                    Else
                        sSQL = sSQL & "WHERE " & Strings.ChrW(13) & Strings.ChrW(10)
                        bIsWhere = True
                    End If
                    sSQL = sSQL & "cl1.claim_number LIKE '" & v_sClaimNumber.Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                End If

                If v_lProgressStatusID > 0 Then
                    If bIsWhere Then
                        sSQL = sSQL & "AND " & Strings.ChrW(13) & Strings.ChrW(10)
                    Else
                        sSQL = sSQL & "WHERE " & Strings.ChrW(13) & Strings.ChrW(10)
                        bIsWhere = True
                    End If
                    sSQL = sSQL & "cap.case_progress_id = " & CStr(v_lProgressStatusID) & Strings.ChrW(13) & Strings.ChrW(10)
                End If

                If v_lRiskTypeID > 0 Then
                    If bIsWhere Then
                        sSQL = sSQL & "AND " & Strings.ChrW(13) & Strings.ChrW(10)
                    Else
                        sSQL = sSQL & "WHERE " & Strings.ChrW(13) & Strings.ChrW(10)
                        bIsWhere = True
                    End If
                    sSQL = sSQL & "rkt.risk_type_id = " & CStr(v_lRiskTypeID) & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If
            If v_vAgentKey <> 0 Then
                If bIsWhere Then
                    sSQL = sSQL & "AND " & Strings.ChrW(13) & Strings.ChrW(10)
                Else
                    sSQL = sSQL & "WHERE " & Strings.ChrW(13) & Strings.ChrW(10)
                    bIsWhere = True
                End If
                sSQL = sSQL & "(c.user_id = ( select top 1 user_id from PMUser where party_cnt = (" & CStr(v_vAgentKey) & "))) " & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            If bIsWhere Then
                sSQL = sSQL & "AND " & Strings.ChrW(13) & Strings.ChrW(10)
            Else
                sSQL = sSQL & "WHERE " & Strings.ChrW(13) & Strings.ChrW(10)
                bIsWhere = True
            End If

            sSQL = sSQL & " is_dirty_case=0"

            r_sSQL = sSQL


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
        Finally
            If Not (oBusiness Is Nothing) Then

                oBusiness.Dispose()
            End If
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetPolicyCase
    ' Parameters:
    ' Description:
    ' History:
    ' Created :
    ' ***************************************************************** '
    'developer guide no.101
    Public Function GetPolicyCase(ByRef vInputData(,) As Object, ByRef vOutputData(,) As Object, ByRef v_vSiriusProduct As Object, Optional ByVal v_vClaimNumber As Object = Nothing, Optional ByVal v_vClientName As Object = Nothing, Optional ByVal v_vPolicyNumber As Object = Nothing, Optional ByVal v_vRegNumber As Object = Nothing, Optional ByVal v_vLossFromdate As Object = Nothing, Optional ByVal v_vLossToDate As Object = Nothing, Optional ByVal v_vClaimStatus As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim NoofFields, iMaxRow, iFromRow As Integer
        Dim sSQL As New StringBuilder
        Dim vTempData = Nothing, vResultData(,) As Object = Nothing
        Dim iParamCount As Integer
        'developer guide no.(As per VB Code)
        Dim vInsuranceFileCnt As Object
        Dim vPropertyValue As Object
        Dim vGISPolicyLink As Object
        Dim vClaimId As Object

        'DC250501 used for searching for apostrophes
        Dim sShortname As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not Informations.IsNothing(v_vLossFromdate) Then
                If v_vLossFromdate <> "" Then
                    If Not Informations.IsDate(v_vLossFromdate) Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If


            If Not Informations.IsNothing(v_vLossToDate) Then
                If v_vLossToDate <> "" Then
                    If Not Informations.IsDate(v_vLossToDate) Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            If Informations.IsArray(vInputData) Then

                ' Get all related data
                For iCounter As Integer = vInputData.GetLowerBound(1) To vInputData.GetUpperBound(1)

                    ' Initialise the search Criteria Variables

                    'developer guide no.101
                    'start change
                    vInsuranceFileCnt = vInputData(0, iCounter)

                    vGISPolicyLink = vInputData(1, iCounter)

                    vPropertyValue = vInputData(4, iCounter)

                    vClaimId = vInputData(6, iCounter)
                    'end change
                    ' Build the SQL select statement according to the parameters passed
                    ' Select statement to select all details relating to values entered
                    sSQL = New StringBuilder("")
                    sSQL.Append("SELECT Claim.Policy_id,Claim.Claim_id,Claim.Description,Claim.Claim_Number," & Strings.ChrW(13) & Strings.ChrW(10))


                    'RiskIndex and Product Code are required for UnderWriting


                    sSQL.Append("Claim.Policy_Number,Claim.client_short_name," & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("(" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("SELECT  prd.description" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("FROM    product prd," & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("insurance_file ifi" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("Where ifi.insurance_file_cnt = claim.Policy_id" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("AND ifi.product_id = prd.product_id" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(") product, " & Strings.ChrW(13) & Strings.ChrW(10))


                    ' recoded following to make more sense + added selection of
                    '           primary cause, secondary cause & progress status
                    sSQL.Append("Claim.loss_from_date,Claim.client_name," & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("Claim.claim_status_id, " & Strings.ChrW(13) & Strings.ChrW(10))

                    sSQL.Append("( " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" SELECT ISNULL(Handler.description,'') " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" FROM Handler " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" WHERE handler_id = Claim.handler_id " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(") Handler, " & Strings.ChrW(13) & Strings.ChrW(10))


                    sSQL.Append("Claim.insurer_claim_number, Claim.client_claim_number, " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("Claim.client_tel_no, Claim.client_tel_no_off, " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("( " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" SELECT ISNULL(Primary_Cause.description,'') " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" FROM Primary_Cause " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" WHERE primary_cause_id = Claim.primary_cause_id " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(") Primary_Cause, " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("( " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" SELECT ISNULL(Secondary_Cause.description,'') " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" FROM Secondary_Cause " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" WHERE secondary_cause_id = Claim.secondary_cause_id " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(") Secondary_Cause, " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("( " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" SELECT ISNULL(Progress_Status.description,'') " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" FROM Progress_Status " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" WHERE progress_status_id = Claim.progress_status_id " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(") Progress_Status, " & Strings.ChrW(13) & Strings.ChrW(10))

                    '       Corrected potential bug. Changes made in May 2004 meant that if a search was performed by "risk index" the
                    '       find claim step would crash when the matching claim was selected and the "OK" button clicked...5 additional
                    '       columns are required to be returned...payments, reserve, iso_code (currency), is_deleted (source), closed_allow_claims (source)
                    sSQL.Append("isnull(" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("(" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("select sum(isnull(r.paid_to_date,0))" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("from reserve r join claim_peril cp on r.claim_peril_id = cp.claim_peril_id and claim_id=claim.claim_id" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(")" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(",0) Payments," & Strings.ChrW(13) & Strings.ChrW(10))

                    sSQL.Append("(" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("(select isnull(sum(isnull(r.revised_reserve,0)),0) + isnull(sum(isnull(r.initial_reserve,0)),0) from reserve r join claim_peril cp on r.claim_peril_id = cp.claim_peril_id and claim_id=claim.claim_id)" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(") Reserve," & Strings.ChrW(13) & Strings.ChrW(10))

                    sSQL.Append("(" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("select  cu.iso_code" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("from    currency cu" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("Where cu.currency_id = claim.currency_id" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(") iso_code," & Strings.ChrW(13) & Strings.ChrW(10))

                    sSQL.Append("(" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("select s.is_deleted" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("from source s" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("inner join insurance_file ifi ON ifi.source_id = s.source_id" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("Where ifi.insurance_file_cnt = claim.Policy_id" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(") is_deleted," & Strings.ChrW(13) & Strings.ChrW(10))

                    sSQL.Append("(" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("select s.closed_allow_claims" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("from source s" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("inner join insurance_file ifi ON ifi.source_id = s.source_id" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("Where ifi.insurance_file_cnt = claim.Policy_id" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(") closed_allow_claims" & Strings.ChrW(13) & Strings.ChrW(10))

                    sSQL.Append("FROM Claim" & Strings.ChrW(13) & Strings.ChrW(10))

                    'append the parameters to the where clause
                    iParamCount = 0

                    'if the field value ClaimNumber supplied

                    If Not Informations.IsNothing(v_vClaimNumber) Then
                        If v_vClaimNumber <> "" Then
                            'increase the parameter count by 1
                            iParamCount += 1

                            If iParamCount > 1 Then
                                sSQL.Append(" AND")
                            Else
                                sSQL.Append(" WHERE")
                            End If
                            sSQL.Append(" Claim.Claim_Number Like '" & v_vClaimNumber.Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10))

                        End If
                    End If

                    'if the field value ShortName supplied

                    If Not Informations.IsNothing(v_vClientName) Then
                        If v_vClientName <> "" Then

                            sShortname = v_vClientName
                            m_lReturn = Apostrophes(sShortname)

                            'increase the parameter count by 1
                            iParamCount += 1

                            If iParamCount > 1 Then
                                sSQL.Append(" AND")
                            Else
                                sSQL.Append(" WHERE")
                            End If

                            sSQL.Append(" Claim.Client_Short_Name Like '" & sShortname.Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10))

                        End If
                    End If


                    'if the field value LossFromDate supplied

                    If Not Informations.IsNothing(v_vLossFromdate) Then
                        If v_vLossFromdate <> "" Then


                            'increase the parameter count by 1
                            iParamCount += 1

                            If iParamCount > 1 Then
                                sSQL.Append(" AND")
                            Else
                                sSQL.Append(" WHERE")
                            End If
                            'sSQL = sSQL & " claim.loss_from_date Between CONVERT(DATETIME, '" & Trim$(CStr(Format(DateAdd("d", -1, v_vLossFromdate), "d/m/yyyy"))) & "',103) " & _
                            '"And CONVERT(DATETIME, '" & Trim(CStr(Format(DateAdd("d", 1, v_vLossFromdate), "d/m/yyyy"))) & "',103)" & vbCrLf

                            sSQL.Append(" convert(char(12),claim.loss_from_date,103)=CONVERT(char(12), '" & StringsHelper.Format(v_vLossFromdate, ACDateConversion).Trim() & "',103)" & Strings.ChrW(13) & Strings.ChrW(10))
                        End If
                    End If

                    'if the field value IS supplied

                    If Not Informations.IsNothing(v_vLossToDate) Then
                        If v_vLossToDate <> "" Then

                            'therefore, Address Cnt is present
                            'increase the parameter count by 1
                            iParamCount += 1

                            If iParamCount > 1 Then
                                sSQL.Append(" AND")
                            Else
                                sSQL.Append(" WHERE")
                            End If
                            'sSQL = sSQL & " claim.loss_to_date Between CONVERT(DATETIME, '" & Trim$(CStr(Format(DateAdd("d", -1, v_vLossToDate), "d/m/yyyy"))) & "',103) " & _
                            '"And CONVERT(DATETIME, '" & Trim(CStr(Format(DateAdd("d", 1, v_vLossToDate), "d/m/yyyy"))) & "',103)" & vbCrLf

                            sSQL.Append(" convert(char(12),claim.loss_to_date,103)=CONVERT(char(12), '" & StringsHelper.Format(v_vLossToDate, ACDateConversion).Trim() & "',103)" & Strings.ChrW(13) & Strings.ChrW(10))

                        End If
                    End If

                    'if the field value IS supplied

                    If Not Informations.IsNothing(v_vClaimStatus) Then
                        If v_vClaimStatus Then

                            'therefore, Address Cnt is present
                            'increase the parameter count by 1
                            iParamCount += 1

                            If iParamCount > 1 Then
                                sSQL.Append(" AND")
                            Else
                                sSQL.Append(" WHERE")
                            End If
                            '                sSQL = sSQL & "(Claim.Claim_Status_id  = " & Trim$(CInt(CLMProvisionalOpenClaim)) & vbCrLf
                            '                sSQL = sSQL & " OR Claim.Claim_Status_id = " & Trim$(CInt(CLMLiveOpenClaim)) & vbCrLf
                            '                sSQL = sSQL & " OR Claim.Claim_Status_id = " & Trim$(CInt(CLMReOpened)) & ")" & vbCrLf

                        End If
                    End If



                    If vInsuranceFileCnt <> 0 Then
                        'increase the parameter count by 1
                        iParamCount += 1

                        If iParamCount > 1 Then
                            sSQL.Append(" AND")
                        Else
                            sSQL.Append(" WHERE")
                        End If
                        sSQL.Append(" Claim.Policy_id = " & vInsuranceFileCnt & Strings.ChrW(13) & Strings.ChrW(10))
                    End If

                    If vClaimId <> 0 Then
                        'increase the parameter count by 1
                        iParamCount += 1

                        If iParamCount > 1 Then
                            sSQL.Append(" AND")
                        Else
                            sSQL.Append(" WHERE")
                        End If
                        sSQL.Append(" Claim.Claim_id= " & vClaimId & Strings.ChrW(13) & Strings.ChrW(10))
                    End If


                    If iParamCount = 0 Then
                        'no parameters passed so query cannot be executed
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Execute SQL Statement - use array for speed
                    With m_oDatabase

                        .Parameters.Clear()

                        m_lError = .SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="Case", bStoredProcedure:=False, vResultArray:=vTempData, bKeepNulls:=True)

                        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimDetails")

                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End With

                    '        ' If NO records were found return PMFalse
                    '        If (IsArray(vTempData) = False) Then
                    '            GetPolicyCase = PMNotFound
                    '            Exit Function
                    '        End If


                    If Informations.IsArray(vTempData) Then
                        ' We have some search results for this Insurance cnt.
                        ' So merge the result Array

                        ' Get the no of fields selected

                        NoofFields = vTempData.GetUpperBound(0)

                        If Not Informations.IsArray(vResultData) Then


                            vResultData = vTempData
                        Else
                            ' We alreay have some data and we have to merge it with new data

                            iFromRow = vResultData.GetUpperBound(1)


                            iMaxRow = vResultData.GetUpperBound(1) + vTempData.GetUpperBound(1) + 1
                            ReDim Preserve vResultData(NoofFields, iMaxRow)


                            For iCounter1 As Integer = vTempData.GetLowerBound(1) To vTempData.GetUpperBound(1)
                                iFromRow += 1
                                For iCounter2 As Integer = 0 To NoofFields


                                    vResultData(iCounter2, iFromRow) = vTempData(iCounter2, iCounter1)
                                Next iCounter2
                            Next iCounter1
                        End If
                    End If


                Next iCounter


            End If
            ' If NO records were found return PMFalse
            If Not Informations.IsArray(vResultData) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Set the return Value

            vOutputData = vResultData

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUWPolicyByGISSearchIndex Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUWPolicyByGISSearchIndex", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function


    'Routine to search for apostrophes and double for searching purposes
    '******************************************************************************
    ' Apostrophes
    '
    ' Take a string and replace ' with ''
    '
    '******************************************************************************
    Private Function Apostrophes(ByRef sString As String) As Integer

        Dim result As Integer = 0
        Dim i As Integer
        Dim sTemp As New StringBuilder



        result = gPMConstants.PMEReturnCode.PMTrue

        If sString.Length = 0 Then
            Return result
        End If

        sTemp = New StringBuilder("")

        Do While True
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


    ' **************************************************************************** '
    ' Name       : GetPreviousDataModel
    ' Description: Returns previous data model Id if screen data model has changed
    '              Returns GIS Policy Link Id if there is any
    ' **************************************************************************** '
    Public Function GetPreviousDataModel(ByVal v_lCaseId As Integer, ByRef r_lPreviousDataModelId As Integer, ByRef r_lGISPolicyLinkID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPreviousDataModel"

        ' Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()
                .Parameters.Add("case_id", CStr(v_lCaseId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'developer guide no. 149
                .Parameters.Add("previous_data_model_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

                'developer guide no. 149
                .Parameters.Add("gis_policy_link_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLAction(sSQL:=ACIsScreenDataModelChangedSQL, sSQLName:=ACIsScreenDataModelChangedName, bStoredProcedure:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(CStr(m_lReturn), "Stored procedure " & ACIsScreenDataModelChangedName & " failed.")
                End If

                r_lPreviousDataModelId = gPMFunctions.ToSafeLong(.Parameters.Item("previous_data_model_id").Value)
                r_lGISPolicyLinkID = gPMFunctions.ToSafeLong(.Parameters.Item("gis_policy_link_id").Value)

            End With


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function


    ' **************************************************************************** '
    ' Name: DeleteCustomData
    ' Description: Deletes all corresponding GIS data for a GIS Policy Link Id
    ' **************************************************************************** '
    Public Function DeleteCustomData(ByVal v_lGisPolicyLinkId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DeleteCustomData"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = BeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(CStr(m_lReturn), "Begin Trans Failed.")
            End If

            With m_oDatabase

                .Parameters.Clear()
                .Parameters.Add("gis_policy_link_id", CStr(v_lGisPolicyLinkId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLAction(sSQL:=ACDeleteCustomDataSQL, sSQLName:=ACDeleteCustomDataName, bStoredProcedure:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(CStr(m_lReturn), "Stored procedure " & ACDeleteCustomDataName & " failed.")
                End If

            End With

            lReturn = CommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(CStr(m_lReturn), "Commit Trans Failed.")
            End If


        Catch ex As Exception

            'DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            lReturn = RollbackTrans()

        Finally



        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name          : GetCaseDetail
    ' Description   :
    ' History       : VB
    ' ***************************************************************** '
    Public Function GetCaseDetails(ByVal v_lCaseId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCaseDetails"

        ' Dim lReturn, lStatus As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue


            bPMAddParameter.AddParameterLite(m_oDatabase, "case_id", v_lCaseId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Execute Action Query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetCaseDetailsSQL, sSQLName:=kGetCaseDetailsName, bStoredProcedure:=True, vResultArray:=r_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetCaseDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CleanUpDirtyCase
    ' History:
    '           Created : Amit Kumar : 23-09-2008 : Case Versioning Changes
    ' ***************************************************************** '
    Public Function CleanUpDirtyCase(ByVal v_lCaseId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CleanUpDirtyCase"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "case_id", v_lCaseId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute Action Query
            m_lReturn = m_oDatabase.SQLAction(sSQL:=kCleanUpDirtyCaseSQL, sSQLName:=kCleanUpDirtyCaseName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kCleanUpDirtyCaseSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lCaseId", v_lCaseId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, excep:=ex, oDicParms:=oDict)

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessCopyCase
    ' History:
    '           Created : Amit Kumar : 23-09-2008 : Case Versioning Changes
    ' ***************************************************************** '
    Public Function ProcessCopyCase(ByVal v_lCaseId As Integer, ByRef r_lCopyCaseId As Integer, Optional ByVal iUserId As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessCopyCase"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin transaction
            m_oDatabase.SQLBeginTrans()

            If iUserId <> 0 Then
                m_iUserID = iUserId
            End If

            ' copy the Case related details
            m_lReturn = CopyCase(v_lCaseId:=v_lCaseId, r_lCopyCaseId:=r_lCopyCaseId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CopyCase Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If r_lCopyCaseId <> 0 Then
                ' copy the gis data as well
                m_lReturn = CopyCaseToGIS(v_lCaseId, r_lCopyCaseId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_oDatabase.SQLRollbackTrans()
                    gPMFunctions.RaiseError(kMethodName, "CopyCaseToWorkGIS Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                'End If
            End If

            ' Commit transaction after all processing is complete
            m_oDatabase.SQLCommitTrans()


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Rollback Transaction
            m_oDatabase.SQLRollbackTrans()

            ' Log Error.

            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lCaseId", v_lCaseId)
            oDict.Add("r_lCopyCaseId", r_lCopyCaseId)
            oDict.Add("iUserId", iUserId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, excep:=ex, oDicParms:=oDict)

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: CopyCase
    ' History:
    '           Created : Amit Kumar : 23-09-2008 : Case Versioning Changes
    ' ***************************************************************** '
    Public Function CopyCase(ByVal v_lCaseId As Integer, ByRef r_lCopyCaseId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CopyCase"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "case_id", v_lCaseId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "user_id", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "copy_Case_id", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "version_id", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            ' Execute Action Query
            m_lReturn = m_oDatabase.SQLAction(sSQL:=kCopyCaseSQL, sSQLName:=kCopyCaseName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kCopyCaseSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            r_lCopyCaseId = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("copy_Case_id").Value, 0)

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.


            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lCaseId", v_lCaseId)
            oDict.Add("r_lCopyCaseId", r_lCopyCaseId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, excep:=ex, oDicParms:=oDict)

        Finally

        End Try

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CopyCaseToGIS
    ' History:
    '           Created : Amit Kumar : 23-09-2008 : Case Versioning Changes
    ' ***************************************************************** '
    Private Function CopyCaseToGIS(ByVal v_lCaseId As Integer, ByVal v_lWorkCaseId As Integer) As Integer

        Dim result As Integer = 0
        ' Dim lStatus As Integer
        'Dim sSQL,
        Dim sDataModelCode As String = ""
        Dim vGISPolicyLink As Object = Nothing
        'Dim sSQLCopyDataSet, sSQLCopyCase As String
        Dim lGisPolicyLinkId, lNewGisPolicyLinkId As Integer
        Dim bCommitTrans, bTransOpen As Boolean
        Dim sQuoteRefPassword As String = ""

        Const kMethodName As String = "CopyCaseToGIS"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            '*****************
            ' Updated version should create an entry in the gis_policy_link table
            ' using spg_DATAMODEL_copydataset
            ' and then update gis_policy_link with the work_Case_id
            '*****************

            bCommitTrans = False

            ' so first thing we need to know is the gis_policy_link_id for
            ' the current Case_id...
            If GetGisPolicyLinkDetails(v_lCaseId:=v_lCaseId, r_vResults:=vGISPolicyLink) = gPMConstants.PMEReturnCode.PMTrue Then

                ' if the details have been retrieved
                If Informations.IsArray(vGISPolicyLink) Then

                    ' get the gis policy link id

                    lGisPolicyLinkId = CInt(vGISPolicyLink(0, 0))

                    sQuoteRefPassword = CStr(vGISPolicyLink(2, 0))

                    ' attempt to get the relevant datamodel code for this Case
                    If GetCaseDataModelCode(v_lCaseId:=v_lCaseId, r_sDataModelCode:=sDataModelCode) = gPMConstants.PMEReturnCode.PMTrue Then

                        ' start transaction
                        m_lReturn = m_oDatabase.SQLBeginTrans()
                        bTransOpen = True

                        ' Create the new gis policy link and copy the original datasets
                        ' data to it
                        If CopyGISDataSet(v_sDataModelCode:=sDataModelCode, v_lOriginalGisPolicyLinkId:=lGisPolicyLinkId, r_lNewGisPolicyLinkId:=lNewGisPolicyLinkId) = gPMConstants.PMEReturnCode.PMTrue Then

                            ' update the quote reference fields on the new gis policy link..
                            If UpdateGisPolicyLinkQuoteRef(v_sQuoteRefPassword:=sQuoteRefPassword, v_sDataModelCode:=sDataModelCode, v_lGisPolicyLinkId:=lNewGisPolicyLinkId) = gPMConstants.PMEReturnCode.PMTrue Then

                                ' Associated the work Case with the new gis policy link
                                If UpdateGisPolicyLinkDetails(v_lCopyCaseId:=v_lWorkCaseId, v_lGisPolicyLinkId:=lNewGisPolicyLinkId) = gPMConstants.PMEReturnCode.PMTrue Then
                                    bCommitTrans = True
                                Else
                                    ' Log Error
                                    result = gPMConstants.PMEReturnCode.PMFalse

                                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                                    oDict.Add("v_lCaseId", v_lCaseId)
                                    oDict.Add("v_lWorkCaseId", v_lWorkCaseId)
                                    gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed - " & " Failed to copy gis Case details for Case:" & CStr(v_lCaseId) & " datamodel:" & sDataModelCode, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, excep:=New Exception(Informations.Err().Description), oDicParms:=oDict)

                                End If
                            Else
                                ' Log Error
                                result = gPMConstants.PMEReturnCode.PMFalse

                                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                                oDict.Add("v_lCaseId", v_lCaseId)
                                oDict.Add("v_lWorkCaseId", v_lWorkCaseId)
                                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed - Failed to update quote reference fields for gis policy link:" & CStr(lNewGisPolicyLinkId), vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, excep:=New Exception(Informations.Err().Description), oDicParms:=oDict)
                            End If

                        Else
                            ' Log Error
                            result = gPMConstants.PMEReturnCode.PMFalse


                            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                            oDict.Add("v_lCaseId", v_lCaseId)
                            oDict.Add("v_lWorkCaseId", v_lWorkCaseId)
                            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed to copy gis dataset for gis policy link id:" & CStr(lGisPolicyLinkId), vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, oDicParms:=oDict)
                        End If

                        ' commit the transaction or roll it back as appropriate
                        If bCommitTrans Then
                            m_lReturn = m_oDatabase.SQLCommitTrans()
                            bTransOpen = False
                        Else
                            m_lReturn = m_oDatabase.SQLRollbackTrans()
                            bTransOpen = False
                        End If

                    Else
                        ' Log Error
                        result = gPMConstants.PMEReturnCode.PMFalse


                        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                        oDict.Add("v_lCaseId", v_lCaseId)
                        oDict.Add("v_lWorkCaseId", v_lWorkCaseId)
                        gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed to get the Case data model for Case:" & CStr(v_lWorkCaseId), vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, oDicParms:=oDict)
                    End If

                End If

            Else
                ' Log Error
                result = gPMConstants.PMEReturnCode.PMFalse

                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lCaseId", v_lCaseId)
                oDict.Add("v_lWorkCaseId", v_lWorkCaseId)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, excep:=New Exception(Informations.Err().Description), oDicParms:=oDict)
            End If

        Catch ex As Exception

            ' rollback transaction if one is still open
            If bTransOpen Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
            End If

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.



            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lCaseId", v_lCaseId)
            oDict.Add("v_lWorkCaseId", v_lWorkCaseId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, excep:=ex, oDicParms:=oDict)
        Finally
        End Try
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetGisPolicyLinkDetails
    ' History:
    '           Created : Amit Kumar : 23-09-2008 : Case Versioning Changes
    ' ***************************************************************** '
    Public Function GetGisPolicyLinkDetails(ByVal v_lCaseId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetGisPolicyLinkDetails"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "Case_id", v_lCaseId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Execute selection Query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetGisPolicyLinkDetailsSQL, sSQLName:=ACGetGisPolicyLinkDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.

                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lCaseId", v_lCaseId)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, excep:=New Exception(Informations.Err().Description), oDicParms:=oDict)
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.


            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lCaseId", v_lCaseId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, excep:=ex, oDicParms:=oDict)

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetCaseDataModelCode
    ' History:
    '           Created : Amit Kumar : 23-09-2008 : Case Versioning Changes
    ' ***************************************************************** '
    Private Function GetCaseDataModelCode(ByVal v_lCaseId As Integer, ByRef r_sDataModelCode As String) As Integer

        Dim result As Integer = 0
        ' Const kMethodName As String = "CopyCaseToGIS"


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="case_id", vValue:=CStr(v_lCaseId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Datamodel_Code", vValue:="", iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetDatamodeCodeforCaseSQL, sSQLName:=ACGetDatamodeCodeforCaseName, bStoredProcedure:=ACGetDatamodeCodeforCaseStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        r_sDataModelCode = gPMFunctions.NullToString(m_oDatabase.Parameters.Item("Datamodel_Code").Value).Trim()



        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CopyGISDataSet
    ' History:
    '           Created : Amit Kumar : 23-09-2008 : Case Versioning Changes
    ' ***************************************************************** '

    Private Function CopyGISDataSet(ByVal v_sDataModelCode As String, ByVal v_lOriginalGisPolicyLinkId As Integer, ByRef r_lNewGisPolicyLinkId As Integer) As Integer

        Dim result As Integer = 0
        Dim sCopyGISDataSetSQL As String = ""

        Const kMethodName As String = "CopyCaseToGIS"




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Required Stored Procedure Parameters

        ' old gis policy link id
        bPMAddParameter.AddParameterLite(m_oDatabase, "old_gis_policy_link_id", v_lOriginalGisPolicyLinkId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

        ' copy quotes - its a required param but doesnt do anything so just default to zero...
        bPMAddParameter.AddParameterLite(m_oDatabase, "copy_quotes", 0, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

        ' new gis policy link id
        bPMAddParameter.AddParameterLite(m_oDatabase, "new_gis_policy_link_id", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

        ' Generate Required Stored Procedure Name
        sCopyGISDataSetSQL = ACGISCopyDatasetStart & v_sDataModelCode & ACGISCopyDatasetEnd

        ' Execute Action Query
        If m_oDatabase.SQLAction(sSQL:=sCopyGISDataSetSQL, sSQLName:="Copy GIS DataSet", bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            '******************************
            ' Log Error.

            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_sDataModelCode", v_sDataModelCode)
            oDict.Add("v_lOriginalGisPolicyLinkId", v_lOriginalGisPolicyLinkId)
            oDict.Add("r_lNewGisPolicyLinkId", r_lNewGisPolicyLinkId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed to copydataset " & sCopyGISDataSetSQL & " for gis_policy_link_id :" & CStr(v_lOriginalGisPolicyLinkId), vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, oDicParms:=oDict)
            '******************************

        Else
            r_lNewGisPolicyLinkId = m_oDatabase.Parameters.Item("new_gis_policy_link_id").Value
        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdateGisPolicyLinkQuoteRef
    ' History:
    '           Created : Amit Kumar : 23-09-2008 : Case Versioning Changes
    ' ***************************************************************** '

    Private Function UpdateGisPolicyLinkQuoteRef(ByVal v_sQuoteRefPassword As String, ByVal v_sDataModelCode As String, ByVal v_lGisPolicyLinkId As Integer) As Integer

        Dim result As Integer = 0
        Dim oGis As bGIS.Application
        Dim sQuoteRef As Object = ""
        ', sQuoteRefPassword 

        Const kMethodName As String = "UpdateGisPolicyLinkQuoteRef"
        Try

            oGis = New bGIS.Application

            result = gPMConstants.PMEReturnCode.PMTrue

            ' create business object
            If oGis.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=DirectCast(m_oDatabase, dPMDAO.Database)) = gPMConstants.PMEReturnCode.PMTrue Then

                ' generate quote reference from the gis policy link id

                If oGis.GenerateQuoteRef(v_lGisPolicyLinkId:=ToSafeInteger(v_lGisPolicyLinkId), r_sQuoteRef:=sQuoteRef, v_sGisDataModelCode:=ToSafeString(v_sDataModelCode)) = gPMConstants.PMEReturnCode.PMTrue Then

                    ' Update the Quote Ref and Password

                    If oGis.UpdateQuoteRef(v_lGisPolicyLinkId:=ToSafeInteger(v_lGisPolicyLinkId), v_sQuoteRef:=ToSafeString(sQuoteRef), v_sQuoteRefPassword:=ToSafeString(v_sQuoteRefPassword), v_sGisDataModelCode:=ToSafeString(v_sDataModelCode)) <> gPMConstants.PMEReturnCode.PMTrue Then

                        ' Log Error.
                        result = gPMConstants.PMEReturnCode.PMFalse


                        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                        oDict.Add("v_sDataModelCode", v_sDataModelCode)
                        oDict.Add("v_lGisPolicyLinkId", v_lGisPolicyLinkId)
                        gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed to Update quote ref for gis policy link" & CStr(v_lGisPolicyLinkId), vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, oDicParms:=oDict)
                    End If

                Else
                    ' Log Error.
                    result = gPMConstants.PMEReturnCode.PMFalse


                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("v_sDataModelCode", v_sDataModelCode)
                    oDict.Add("v_lGisPolicyLinkId", v_lGisPolicyLinkId)
                    gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed to Generate Quote Ref for gis policy link:" & CStr(v_lGisPolicyLinkId), vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, oDicParms:=oDict)

                End If

            Else

                ' Log Error.

                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_sDataModelCode", v_sDataModelCode)
                oDict.Add("v_lGisPolicyLinkId", v_lGisPolicyLinkId)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed to create instance of bGIS.application", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, oDicParms:=oDict)

            End If



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.



            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_sDataModelCode", v_sDataModelCode)
            oDict.Add("v_lGisPolicyLinkId", v_lGisPolicyLinkId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, excep:=ex, oDicParms:=oDict)

        Finally

            ' destroy instance of gis object
            oGis = Nothing

            'Return result
            'Resume
        End Try
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateGisPolicyLinkDetails
    ' History:
    '           Created : Amit Kumar : 23-09-2008 : Case Versioning Changes
    ' ***************************************************************** '

    Private Function UpdateGisPolicyLinkDetails(ByVal v_lCopyCaseId As Integer, ByVal v_lGisPolicyLinkId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateGisPolicyLinkDetails"


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Required Stored Procedure Parameters

        ' Gis Policy Link Id
        bPMAddParameter.AddParameterLite(m_oDatabase, "gis_policy_link_id", v_lGisPolicyLinkId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
        ' Work Case Id
        bPMAddParameter.AddParameterLite(m_oDatabase, "Case_id", v_lCopyCaseId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)

        ' Execute Action Query
        If m_oDatabase.SQLAction(sSQL:=ACUpdateGisPolicyLinkDetailsSQL, sSQLName:=ACUpdateGisPolicyLinkDetailsName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.

            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lCopyCaseId", v_lCopyCaseId)
            oDict.Add("v_lGisPolicyLinkId", v_lGisPolicyLinkId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed to update gis_policy_link:" & CStr(v_lGisPolicyLinkId) & " with work Case id " & CStr(v_lCopyCaseId), vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, oDicParms:=oDict)

        End If

        Return result
    End Function
End Class
