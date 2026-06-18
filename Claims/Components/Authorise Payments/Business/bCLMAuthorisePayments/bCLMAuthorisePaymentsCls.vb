Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 20092002
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a CLMAuthorisePayments.
    '
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 10/12/2003
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

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private lPMAuthorityLevel As Integer
    Private m_sUnderwritingOrAgency As String = ""
    Private m_bAuthorisePayments As Boolean

    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business

    Private Const PMKeyNameRealClaimID As String = "claim_id"
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            Value = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property

    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

        End Get
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
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
            m_dtEffectiveDate = DateTime.Now

            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUserName, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                    m_oLookup = Nothing
                End If
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

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Public Function BeginTrans() As Integer

        Dim result As Integer = 0
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Sub New()
        MyBase.New()

        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Public Function getUnderwritingOrAgency() As Integer
        ' ***************************************************************** '
        ' Name: UnderwritingOrAgency
        '
        ' Description:  Finds out if Underwriting or Agency business

        ' ***************************************************************** '

        Dim result As Integer = 0
        Try

            Return bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingOrAgency)

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnderwritingOrAgencyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnderwritingOrAgency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetReferredList(ByRef r_vResultArray(,) As Object, Optional ByRef other_party_id As Integer = 0) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="other_party_id", vValue:=other_party_id, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetReferredCliamsSQL, sSQLName:=ACGetReferredCliamsName, bStoredProcedure:=ACGetReferredCliamsStored, vResultArray:=r_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReferredList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReferredList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function CreateEvent(ByRef v_lEventType As Integer, ByRef v_sDescription As String, ByRef v_lClaimId As Integer, ByRef v_sOriginalUser As String, ByRef v_sMode As String) As Integer

        Dim result As Integer = 0
        Dim lPartyCnt, lInsuranceFolderCnt, lInsuranceFileCnt, lEventCnt As Integer
        Dim vArray(,) As Object = Nothing
        Dim oEvent As bSIREvent.Business
        Dim sDescription As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCountsSQL, sSQLName:=ACGetCountsName, bStoredProcedure:=ACGetCountsStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lPartyCnt = CInt(vArray(0, 0))

            lInsuranceFolderCnt = CInt(vArray(1, 0))

            lInsuranceFileCnt = CInt(vArray(2, 0))

            vArray = Nothing

            sDescription = "Payment Created by - " & v_sOriginalUser & " and "
            If v_sMode = "D" Then
                sDescription = sDescription & "Declined by - " & m_sUsername
            Else
                sDescription = sDescription & "Authorised by - " & m_sUsername
            End If
            If v_sDescription.Trim() <> "" Then
                sDescription = sDescription & ", Comments - " & v_sDescription.Trim()
            End If


            oEvent = New bSIREvent.Business
            m_lReturn = oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the event object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            m_lReturn = oEvent.DirectAdd(vEventCnt:=lEventCnt, vPartyCnt:=lPartyCnt, vInsuranceFolderCnt:=lInsuranceFolderCnt, vInsuranceFileCnt:=lInsuranceFileCnt, vClaimCnt:=v_lClaimId, vDocumentCnt:=DBNull.Value, vOldAddressCnt:=DBNull.Value, vNewAddressCnt:=DBNull.Value, vCampaignId:=DBNull.Value, vDocumentType:=DBNull.Value, vReportType:=DBNull.Value, vEventType:=v_lEventType, vUserId:=m_iUserID, vEventDate:=DateTime.Today, vDescription:=sDescription)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function

    'AK 28052003 - function to set status of payments to declined
    Public Function ProcessDecline(ByRef v_lClaimId As Integer, ByVal v_lPaymentId As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            m_oDatabase.Parameters.Clear()

            ' add claim id parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Alix - 02/02/2004
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Payment_Id", vValue:=CStr(v_lPaymentId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' run SP
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACProcessDeclineSQL, sSQLName:=ACProcessDeclineName, bStoredProcedure:=ACProcessDeclineStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessDecline Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessDecline", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'AK 06062003 - function to set status of payments to authorised
    Public Function ProcessAuthorise(ByRef v_lClaimId As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            m_oDatabase.Parameters.Clear()

            ' add claim id parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' run SP
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACProcessAuthoriseSQL, sSQLName:=ACProcessAuthoriseName, bStoredProcedure:=ACProcessAuthoriseStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAuthorise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAuthorise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'AK 06062003 - function to set status of payments to authorised
    Public Function CheckUserGroup() As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            m_oDatabase.Parameters.Clear()

            ' add username parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="username", vValue:=m_sUsername, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' add groupcode parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="group_code", vValue:="CLMSUPER", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' run SP
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckUserGroupSQL, sSQLName:=ACCheckUserGroupName, bStoredProcedure:=ACCheckUserGroupStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
            End If
            If Not Informations.IsArray(vArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If result <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMTrue

                'Check if the user is a system Admin
                m_oDatabase.Parameters.Clear()

                ' add username parameter
                m_lReturn = m_oDatabase.Parameters.Add(sName:="username", vValue:=m_sUsername, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

                ' add groupcode parameter
                m_lReturn = m_oDatabase.Parameters.Add(sName:="group_code", vValue:="SYSADMIN", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

                ' run SP
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckUserGroupSQL, sSQLName:=ACCheckUserGroupName, bStoredProcedure:=ACCheckUserGroupStored, vResultArray:=vArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                End If

                If Not Informations.IsArray(vArray) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckUserGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckUserGroup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function ProcessWTM(ByVal v_lClaimId As Integer) As Integer

        Dim result As Integer = 0
        Dim oDatabase As Object = Nothing
        Dim oWrkTaskInstance As New bPMWrkTaskInstance.TaskControl
        Try

            Dim vInstanceCnt(,) As Object = Nothing

            result = gPMConstants.PMEReturnCode.PMTrue

            'Let's get the Instance Cnt using the Key Name and the Key Value
            With m_oDatabase
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="KeyName", vValue:=PMKeyNameRealClaimID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessWTM Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWTM", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                m_lReturn = .Parameters.Add(sName:="KeyValue", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessWTM Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWTM", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                m_lReturn = .SQLSelect(sSQL:=ACGetWTMInstanceCntSQL, sSQLName:=ACGetWTMInstanceCntName, bStoredProcedure:=ACGetWTMInstanceCntStored, vResultArray:=vInstanceCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessWTM Failed to run the stored proc", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWTM", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            End With

            If Informations.IsArray(vInstanceCnt) Then
                'open architecture database

                m_lReturn = CType(gPMComponentServices.NewDatabase(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oDatabase), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessWTM Failed to Check NewDatabase", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWTM", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If


                oWrkTaskInstance = New bPMWrkTaskInstance.TaskControl
                m_lReturn = oWrkTaskInstance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessWTM Failed to Create bPMWrkTaskInstance object", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWTM", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                For lCount As Integer = 0 To vInstanceCnt.GetUpperBound(1)

                    m_lReturn = oWrkTaskInstance.SetStatusComplete(v_lPMWrkTaskInstanceCnt:=gPMFunctions.NullToLong(vInstanceCnt(0, lCount)))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessWTM Failed to Set the WTM status to complete", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWTM", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Exit For
                    End If

                    m_lReturn = oWrkTaskInstance.Delete(v_lPMWrkTaskInstanceCnt:=gPMFunctions.NullToLong(vInstanceCnt(0, lCount)))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessWTM Failed to delete WTM", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWTM", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Exit For
                    End If
                Next lCount
                If Not (oWrkTaskInstance Is Nothing) Then

                    oWrkTaskInstance.Dispose()
                    oWrkTaskInstance = Nothing
                End If
                If Not (oDatabase Is Nothing) Then

                    m_lReturn = oDatabase.CloseDatabase()
                    oDatabase = Nothing
                End If
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            If Not (oWrkTaskInstance Is Nothing) Then

                oWrkTaskInstance.Dispose()
                oWrkTaskInstance = Nothing
            End If
            If Not (oDatabase Is Nothing) Then

                m_lReturn = oDatabase.CloseDatabase()
                oDatabase = Nothing
            End If
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessWTM Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWTM", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function AddTaskToWorkManager(ByRef r_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_sDescription As String, ByVal v_dtTaskDueDate As Date, Optional ByVal v_lPMWrkTaskID As Integer = 0, Optional ByVal v_sTaskCode As String = "", Optional ByVal v_lPMWrkTaskGroupID As Integer = 0, Optional ByVal v_sTaskGroupCode As String = "", Optional ByVal v_iUserID As Integer = 0, Optional ByVal v_sUserGroupCode As String = "PURCLDGR", Optional ByVal v_vKeyArray As Object = Nothing, Optional ByVal v_iIsUrgent As Integer = 0, Optional ByVal v_iIsVisible As Integer = gPMConstants.PMEReturnCode.PMTrue, Optional ByVal v_iTaskStatus As Integer = gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew) As Integer

        Dim result As Integer = 0
        Dim oDatabase As Object = Nothing
        Dim oWrkTaskInstance As New bPMWrkTaskInstance.TaskControl
        Dim sSQL As String = ""
        Dim vResultArray As Object = Nothing
        Dim sName As String = "GetGroupIDs"
        Dim lUserGroupID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'check to see if we have task_id or task code
            If v_lPMWrkTaskID = 0 And v_sTaskCode = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Must supply either task id or task code", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'check to see if we have task_group_id or task group code
            If v_lPMWrkTaskGroupID = 0 And v_sTaskGroupCode = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Must supply either task group id or task group code", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'open architecture database

            m_lReturn = CType(gPMComponentServices.NewDatabase(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager failed to create a new Database for Architecture.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            oWrkTaskInstance = New bPMWrkTaskInstance.TaskControl
            m_lReturn = oWrkTaskInstance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager failed to create bPMWrkTaskInstance.TaskControl Object.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                If Not (oDatabase Is Nothing) Then
                    'close database

                    m_lReturn = oDatabase.CloseDatabase()
                End If

                Return result
            End If

            '*************************************************************************
            '                     GET THE GROUP ID FOR USER
            '*************************************************************************
            ' Clear the database parameters

            oDatabase.Parameters.Clear()

                If v_sUserGroupCode <> "" Then

                    sSQL = "SELECT pmuser_group_id FROM pmuser_group WHERE code = {group_code}"

                    ' Add the Group_code parameter

                    m_lReturn = oDatabase.Parameters.Add(sName:="group_code", vValue:=ToSafeString(v_sUserGroupCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager failed to the Group_Code paramater.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        If Not (oDatabase Is Nothing) Then
                            'close database

                            m_lReturn = oDatabase.CloseDatabase()
                        End If

                        If Not (oWrkTaskInstance Is Nothing) Then

                            oWrkTaskInstance.Dispose()
                            oWrkTaskInstance = Nothing
                        End If

                        Return result
                    End If

                Else

                    sSQL = "SELECT pmuser_group_id FROM pmuser_group_user WHERE user_id = {user_id}"

                    ' Add the user_id parameter

                    m_lReturn = oDatabase.Parameters.Add(sName:="user_id", vValue:=IIf(v_iUserID > 0, v_iUserID, m_iUserID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager failed to the User_ID paramater.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        If Not (oDatabase Is Nothing) Then
                            'close database

                            m_lReturn = oDatabase.CloseDatabase()
                        End If

                        If Not (oWrkTaskInstance Is Nothing) Then

                            oWrkTaskInstance.Dispose()
                            oWrkTaskInstance = Nothing
                        End If

                        Return result
                    End If

                End If

            ' Run the stored proc

            m_lReturn = oDatabase.SQLSelect(sSQL:=ToSafeString(sSQL), sSQLName:=ToSafeString(sName), bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager failed to run GetGroupIDs SQL Statement.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                If Not (oDatabase Is Nothing) Then
                    'close database

                    m_lReturn = oDatabase.CloseDatabase()
                End If

                If Not (oWrkTaskInstance Is Nothing) Then

                    oWrkTaskInstance.Dispose()
                    oWrkTaskInstance = Nothing
                End If

                Return result
            End If

            'do we have any data
            If Not Informations.IsArray(vResultArray) Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager failed to GetGroupIDs from the database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                If Not (oDatabase Is Nothing) Then
                    'close database

                    m_lReturn = oDatabase.CloseDatabase()
                End If

                If Not (oWrkTaskInstance Is Nothing) Then

                    oWrkTaskInstance.Dispose()
                    oWrkTaskInstance = Nothing
                End If

                Return result
            Else
                ' Get the user_group_id

                lUserGroupID = CInt(vResultArray(0, 0))
            End If

            '*************************************************************************
            '                     GET THE TASK ID
            '*************************************************************************
            If v_sTaskCode <> "" Then
                sSQL = "SELECT pmwrk_task_id FROM PMWrk_Task WHERE code = {task_code}"

                oDatabase.Parameters.Clear()

                m_lReturn = oDatabase.Parameters.Add(sName:="task_code", vValue:=ToSafeString(v_sTaskCode), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager failed to add Task_code paramater.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    If Not (oDatabase Is Nothing) Then
                        'close database

                        m_lReturn = oDatabase.CloseDatabase()
                    End If

                    If Not (oWrkTaskInstance Is Nothing) Then

                        oWrkTaskInstance.Dispose()
                        oWrkTaskInstance = Nothing
                    End If
                    Return result
                End If

                m_lReturn = oDatabase.SQLSelect(sSQL:=ToSafeString(sSQL), sSQLName:="GetTaskID", bStoredProcedure:=False, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager failed to run the GetTaskID SQL Statement.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    If Not (oDatabase Is Nothing) Then
                        'close database

                        m_lReturn = oDatabase.CloseDatabase()
                    End If

                    If Not (oWrkTaskInstance Is Nothing) Then

                        oWrkTaskInstance.Dispose()
                        oWrkTaskInstance = Nothing
                    End If
                    Return result
                End If

                If Not Informations.IsArray(vResultArray) Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager failed to get GetTaskID from Database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    If Not (oDatabase Is Nothing) Then
                        'close database

                        m_lReturn = oDatabase.CloseDatabase()
                    End If

                    If Not (oWrkTaskInstance Is Nothing) Then

                        oWrkTaskInstance.Dispose()
                        oWrkTaskInstance = Nothing
                    End If
                    Return result
                Else
                    'Set the TaskID

                    v_lPMWrkTaskID = CInt(vResultArray(0, 0))
                End If
            End If

            '*************************************************************************
            '                     GET THE TASK GROUP ID
            '*************************************************************************
            If v_sTaskGroupCode <> "" Then
                sSQL = "SELECT pmwrk_task_group_id FROM PMWrk_Task_group WHERE code = {task_group_code}"

                oDatabase.Parameters.Clear()

                m_lReturn = oDatabase.Parameters.Add(sName:="task_group_code", vValue:=ToSafeString(v_sTaskGroupCode), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager failed to add Task_Group_Code paramater.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    If Not (oDatabase Is Nothing) Then
                        'close database

                        m_lReturn = oDatabase.CloseDatabase()
                    End If

                    If Not (oWrkTaskInstance Is Nothing) Then

                        oWrkTaskInstance.Dispose()
                        oWrkTaskInstance = Nothing
                    End If
                    Return result
                End If

                m_lReturn = oDatabase.SQLSelect(sSQL:=ToSafeString(sSQL), sSQLName:="GetGroupTaskID", bStoredProcedure:=False, vResultArray:=vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager failed to run the GetGroupTaskID SQL Statement.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    If Not (oDatabase Is Nothing) Then
                        'close database

                        m_lReturn = oDatabase.CloseDatabase()
                    End If

                    If Not (oWrkTaskInstance Is Nothing) Then

                        oWrkTaskInstance.Dispose()
                        oWrkTaskInstance = Nothing
                    End If
                    Return result
                End If

                If Not Informations.IsArray(vResultArray) Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager failed to get GetGroupTaskID from Database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    If Not (oDatabase Is Nothing) Then
                        'close database

                        m_lReturn = oDatabase.CloseDatabase()
                    End If

                    If Not (oWrkTaskInstance Is Nothing) Then

                        oWrkTaskInstance.Dispose()
                        oWrkTaskInstance = Nothing
                    End If
                    Return result
                Else

                    v_lPMWrkTaskGroupID = CInt(vResultArray(0, 0))
                End If
            End If

            '*************************************************************************
            '                             CREATE THE TASK
            '*************************************************************************

            m_lReturn = oWrkTaskInstance.CreateNew(v_lPMWrkTaskGroupID:=v_lPMWrkTaskGroupID, v_lPMWrkTaskID:=v_lPMWrkTaskID, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_lPMUserGroupID:=lUserGroupID, v_sDescription:=v_sDescription, v_iTaskStatus:=v_iTaskStatus, v_iIsUrgent:=v_iIsUrgent, r_lPMWrkTaskInstanceCnt:=r_lPMWrkTaskInstanceCnt, v_iUserID:=v_iUserID, v_vKeyArray:=v_vKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager failed to add the Task using oWrkTaskInstance object.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                If Not (oDatabase Is Nothing) Then
                    'close database

                    m_lReturn = oDatabase.CloseDatabase()
                End If

                If Not (oWrkTaskInstance Is Nothing) Then

                    oWrkTaskInstance.Dispose()
                    oWrkTaskInstance = Nothing
                End If
                Return result
            End If

            'Clean up after ourselves
            If Not (oDatabase Is Nothing) Then
                'close database

                m_lReturn = oDatabase.CloseDatabase()
            End If

            If Not (oWrkTaskInstance Is Nothing) Then

                oWrkTaskInstance.Dispose()
                oWrkTaskInstance = Nothing
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'Clean up after ourselves
            If Not (oDatabase Is Nothing) Then
                'close database

                m_lReturn = oDatabase.CloseDatabase()
            End If

            If Not (oWrkTaskInstance Is Nothing) Then

                oWrkTaskInstance.Dispose()
                oWrkTaskInstance = Nothing
            End If

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimPaymentAccountsDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-01-2006 : Cheque Production Workflow (ATD16)
    ' ***************************************************************** '
    Public Function GetClaimPaymentAccountsDetails(ByVal v_lClaimPaymentId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimPaymentAccountsDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_payment_id", v_vValue:=v_lClaimPaymentId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetClaimPaymentAccountsDetailsSQL, sSQLName:=kGetClaimPaymentAccountsDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetClaimPaymentAccountsDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
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

    Public Function GetClaimVersionDescription(ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimVersionDescription"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetClaimVersionDescriptionSQL, sSQLName:=kGetClaimVersionDescriptionName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetClaimVersionDescriptionSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: AddInputParameter
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddInputParameter"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Parameter to database object

            lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, " Failed to add parameter name:" & v_sName &
                                        ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), gPMConstants.PMELogLevel.PMLogError)
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
    Public Function GetClaimStatus(ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimStatus"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetClaimStatusSQL, sSQLName:=kGetClaimStatusName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetClaimStatusSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
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

    Public Function GetReferredClaimStatus(ByVal v_lClaimPaymentId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetReferredClaimStatus"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_Payment_id", v_vValue:=v_lClaimPaymentId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetReferredClaimStatusSQL, sSQLName:=kGetReferredClaimStatusName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetClaimStatusSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
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

    Public Function Update_Claim_Status(ByVal iClaimid As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Update_Claim_Status"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            With m_oDatabase

                lReturn = .Parameters.Add(sName:="claim_id", vValue:=CStr(iClaimid), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                lReturn = .SQLAction(sSQL:=ACClaimUpdateStatusSQL, sSQLName:=ACClaimUpdateStatusName, bStoredProcedure:=True)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACClaimUpdateStatusSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End With


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function
    ' Get the CashListItem for the Claim Payment
    Public Function GetCashListItemClaimLinkDetails(ByVal v_lClaimPaymentId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCashListItemClaimLinkDetails"
        Dim lReturn As gPMConstants.PMEReturnCode
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_payment_id", v_vValue:=v_lClaimPaymentId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetCashListItemClaimLinkSQL, sSQLName:=kGetCashListItemClaimLinkName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetCashListItemClaimLinkSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
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

    Public Function SetClaimPaymentRecommendStatus(ByVal v_lClaimId As Integer, ByVal v_iStatus As Integer, ByVal v_iUserID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetClaimPaymentRecommendStatus"
        Dim lReturn As gPMConstants.PMEReturnCode
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claimid", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="status", v_vValue:=v_iStatus, v_iType:=gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="user_id", v_vValue:=v_iUserID, v_iType:=gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLAction(sSQL:=kSetClaimRcommendPaymentStatusSQL, sSQLName:=kSetClaimRcommendPaymentStatusName, bStoredProcedure:=True)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kSetClaimRcommendPaymentStatusSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
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

    Public Function GetReserveTotalForClaimPayment(ByVal v_lClaimPaymentId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetReserveTotalForClaimPayment"
        Dim lReturn As gPMConstants.PMEReturnCode
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_payment_id", v_vValue:=v_lClaimPaymentId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetReserveTotalForClaimPaymentSQL, sSQLName:=kGetReserveTotalForClaimPaymentName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetReserveTotalForClaimPaymentSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function GetTransDetailFromCashListItem(ByVal v_lCashListItemId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetTransDetailFromCashListItem"
        Dim lReturn As gPMConstants.PMEReturnCode
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="CashListItem_id", v_vValue:=v_lCashListItemId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetTransDetailFromCashListItemSQL, sSQLName:=kGetTransDetailFromCashListItemName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetTransDetailFromCashListItemSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
        Return result
    End Function

    Public Function GetAlreadyReferredClaimStatus(ByVal v_lClaimPaymentId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAlreadyReferredClaimStatus"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_Payment_id", v_vValue:=v_lClaimPaymentId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetAlreadyReferredClaimStatusSQL, sSQLName:=kGetAlreadyReferredClaimStatusName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetClaimStatusSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function
    'archana
    Public Function GetUserOtherParty(ByVal iUserID As Integer, ByRef r_vResultArray(,) As Object) As Long
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "userid", iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            ' Execute the stored procedure.

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUserotherpartySQL, sSQLName:=ACGetUserotherpartyName, bStoredProcedure:=ACGetUserotherpartyStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get user other party Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Getuserotherparty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class
