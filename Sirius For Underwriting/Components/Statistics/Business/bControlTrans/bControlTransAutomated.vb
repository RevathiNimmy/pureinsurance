Option Strict Off
Option Explicit On
Imports System.Data
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Automated_NET.Automated")>
Public NotInheritable Class Automated
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Automated
    '
    ' Date: 28/04/1997
    '
    ' Description: Creatable Automated class which contains all the
    '              Automated methods which can be called by Navigator.
    '
    ' Edit History: TF280497  - Created
    '               TF091297 - Import to Pinstripe bits removed
    '               PH250298 - Removed transaction_type_code parameter
    '                          from CreateStatsFolder
    ' RAW 13/11/2003 : CQ1765 : when processing an MTA ensure that the instalment plan for the original policy version is cancelled
    ' RAW 13/11/2003 : CQ1765 : when processing a cancellation ensure that the instalment plan is cancelled for the correct policy version
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 17/12/2003
    ' ***************************************************************** '
    ' Class Name: Automated
    '
    ' Date: 28/04/1997
    '
    ' Description: Creatable Automated class which contains all the
    '              Automated methods which can be called by Navigator.
    '
    ' Edit History: TF280497  - Created
    '               TF091297 - Import to Pinstripe bits removed
    '               PH250298 - Removed transaction_type_code parameter
    '                          from CreateStatsFolder
    ' RAW 13/11/2003 : CQ1765 : when processing an MTA ensure that the instalment plan for the original policy version is cancelled
    ' RAW 13/11/2003 : CQ1765 : when processing a cancellation ensure that the instalment plan is cancelled for the correct policy version
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 17/12/2003
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************
    Private m_lCompanyId As Integer

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Automated"

    Private Const ACPaymentOptionPayNow As Integer = 1
    Private Const ACPaymentOptionInvoice As Integer = 2
    Private Const ACPaymentOptionInstalments As Integer = 3
    Private Const ACLockName As String = "CashDepositAccount"


    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database
    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean
    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    ' Task
    Private m_iTask As Integer
    ' Navigate
    Private m_lNavigate As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    ' Insurance File ID
    Private m_lInsuranceFileCnt As Integer
    Private m_lOriginalInsuranceFileCnt As Integer ' RAW 13/11/2003 : CQ1765 : added
    Private m_bCancelOriginalPFPlan As Boolean ' RAW 13/11/2003 : CQ1765 : added
    Private m_lRunMode As Integer ' RAW 13/11/2003 : CQ1765 : added

    ' Party ID
    'Private m_lPartyCnt As Long
    ' Product ID
    'Private m_iProductID As Integer

    'RWH(06/06/01) Added message property, initially to return failure reasons.
    Private m_sMessage As String = ""

    'sj 03/04/2003 - start
    ' NextOrionDocRef

    'Private m_lNextOrionDocRef As Long
    Private m_sNextOrionDocRef As String = ""
    'Private m_lNextOrionDocRefForInstalment As Long
    Private m_sNextOrionDocRefForInstalment As String = ""

    Private m_bByPassLocking As Boolean

    Private m_lClonedInsuranceFileCnt As Long
    Private m_bReverseCloned As Boolean

    Private m_lPTInsuranceFileCnt As Long
    Private m_bReversePT As Boolean
    Private m_bIsCloned As Boolean
    Private m_bIsPT As Boolean
    Private m_dtTransferDate As Date
    Private m_bBackDateMTA As Boolean
    Private m_nClonedReversalDocumentID As Integer
	Private m_bReverseVoid As Boolean
    Public Property ReverseVoid() As Boolean
        Get
            Return m_bReverseVoid
        End Get
        Set(value As Boolean)
            m_bReverseVoid = value
        End Set
    End Property

    'Private g_oObjectManager As bObjectManager.ObjectManager


    Public Property ClonedInsuranceFileCnt() As Integer
        Get
            Return m_lClonedInsuranceFileCnt
        End Get
        Set(value As Integer)
            m_lClonedInsuranceFileCnt = value
        End Set
    End Property



    Public Property ReverseCloned() As Boolean
        Get
            Return m_bReverseCloned
        End Get
        Set(value As Boolean)
            m_bReverseCloned = value
        End Set
    End Property


    Public Property PTInsuranceFileCnt() As Long
        Get
            Return m_lPTInsuranceFileCnt
        End Get
        Set(value As Long)
            m_lPTInsuranceFileCnt = value
        End Set
    End Property

    Public Property ReversePT() As Boolean
        Get
            Return m_bReversePT
        End Get
        Set(value As Boolean)
            m_bReversePT = value
        End Set
    End Property





    'Float balance and Pre-Payment work
    Public Property CompanyId() As Integer
        Get
            Return m_lCompanyId
        End Get
        Set(ByVal Value As Integer)
            m_lCompanyId = Value
        End Set
    End Property

    Public Property ByPassLocking() As Boolean
        Get
            Return m_bByPassLocking
        End Get
        Set(ByVal Value As Boolean)
            m_bByPassLocking = Value
        End Set
    End Property

    Public WriteOnly Property NextOrionDocRef() As String
        Set(ByVal Value As String)
            m_sNextOrionDocRef = Value
        End Set
    End Property


    Public Property Message() As String
        Get
            Return m_sMessage
        End Get
        Set(ByVal Value As String)
            m_sMessage = Value
        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
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

    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public WriteOnly Property OriginalInsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lOriginalInsuranceFileCnt = Value
        End Set
    End Property

    Public WriteOnly Property CancelOriginalPFPlanFlag() As Boolean
        Set(ByVal Value As Boolean)
            m_bCancelOriginalPFPlan = Value
        End Set
    End Property

    Public WriteOnly Property RunMode() As Integer
        Set(ByVal Value As Integer)
            m_lRunMode = Value
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public Property IsCloned() As Boolean
        Get
            Return m_bIsCloned
        End Get
        Set(value As Boolean)
            m_bIsCloned = value
        End Set
    End Property

    Public Property IsPT() As Boolean
        Get
            Return m_bIsPT
        End Get
        Set(value As Boolean)
            m_bIsPT = value
        End Set
    End Property

    Public Property TransferDate() As DateTime
        Get
            Return m_dtTransferDate
        End Get
        Set(value As DateTime)
            m_dtTransferDate = value
        End Set
    End Property

    Public Property BackDateMTA() As Boolean
        Get
            Return m_bBackDateMTA
        End Get
        Set(value As Boolean)
            m_bBackDateMTA = value
        End Set
    End Property

    Private Function Allocate(ByVal v_lPartyCnt As Object) As Integer

        Dim result As Integer = 0

        Dim vResultArray(,) As Object = Nothing
        Dim vTransDetailsArray As Object = Nothing
        Dim oAllocationManual As bACTAllocationManual.Business
        Dim lAccountID As Integer
        Dim vKeyArray(1, 3) As Object

        result = gPMConstants.PMEReturnCode.PMTrue
        oAllocationManual = New bACTAllocationManual.Business
        If oAllocationManual.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bACTAllocationManual", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="Account_ID", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="Insurance_File_Cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="Party_Cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .SQLSelect(sSQL:=ACGetTransDetailForStatsReversalSQL, sSQLName:=ACGetTransDetailForStatsReversalName, bStoredProcedure:=ACGetTransDetailForStatsReversalStored, vResultArray:=vResultArray)

            lAccountID = m_oDatabase.Parameters.Item("Account_ID").Value

        End With

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If vResultArray.GetUpperBound(1) < 1 Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ReDim vTransDetailsArray(vResultArray.GetUpperBound(1) - 1)

        For lCount As Integer = 1 To vResultArray.GetUpperBound(1)

            vTransDetailsArray(lCount - 1) = CStr(vResultArray(0, lCount)) & "|" & CStr(vResultArray(1, lCount))
        Next

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = lAccountID

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = CStr(vResultArray(0, 0)) & "|" & CStr(vResultArray(1, 0))

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vTransDetailsArray

        m_lReturn = oAllocationManual.SetKeys(vKeyArray:=vKeyArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Allocation Manual Failed with Set Keys", vApp:=ACApp, vClass:=ACClass, vMethod:="MakeInstalmentPlanLive", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = oAllocationManual.Start()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to allocate against original NB invoice", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        oAllocationManual.Dispose()

        oAllocationManual = Nothing

        Return result

    End Function

    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceId As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceId
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

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

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Informations.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameInsFileCnt

                        m_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                End Select

            Next lRow

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vKeyArray(1, 0)

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameInsFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lInsuranceFileCnt

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function Start(Optional ByRef iPaymentAccountId As Integer = 0, Optional ByRef iDebitAgainst As Integer = 0, Optional ByRef vCreditTransactions As Object = Nothing, Optional ByRef lCashListID As Integer = 0, Optional ByRef lCashListItemId As Integer = 0, Optional ByRef lTransactionID As Integer = 0, Optional ByRef cTransactionAmount As Decimal = 0, Optional ByRef sOldPolicyNumber As String = "", Optional ByRef sPaymentMethod As String = "", Optional ByRef vDebitTransactions As Object = Nothing, Optional ByRef bProcessSettleTransactions As Boolean = False, Optional ByRef cRoundOffAmount As Decimal = 0) As Integer
        Dim nResult As Integer
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue
            Dim vResultArray(,) As Object = Nothing
            Dim iBusinessCurrency As Integer
            Dim cBaseCurrency As Decimal
            Dim iTransCurrencyID As Integer
            Dim iInsuranceFileTypeID As Integer
            Dim sFailureReason As String

            m_lReturn = GetInsuranceRef(m_lInsuranceFileCnt, vResultArray)
            If Informations.IsArray(vResultArray) Then

                iBusinessCurrency = gPMFunctions.ToSafeLong(CDbl(vResultArray(18, 0)))
                iInsuranceFileTypeID = gPMFunctions.ToSafeInteger(vResultArray(2, 0))
            End If

            If iInsuranceFileTypeID = 11 Then
                Return nResult
            End If
            If iBusinessCurrency <> m_iCurrencyID Then
                If Not Informations.IsArray(vCreditTransactions) Then
                    m_lReturn = GetBaseCurrencyAmount(lCompanyId:=m_iSourceID, lCurrencyID:=iBusinessCurrency, cBaseAmount:=cBaseCurrency, cCurrencyamount:=cTransactionAmount)
                Else
                    For iVar As Integer = 0 To vCreditTransactions.GetUpperBound(1)

                        m_lReturn = GetGetCurrencyIDFromTransDetail(CInt(vCreditTransactions(1, iVar)), iTransCurrencyID)
                        If iTransCurrencyID <> m_iCurrencyID Then

                            m_lReturn = GetBaseCurrencyAmount(lCompanyId:=m_iSourceID, lCurrencyID:=iBusinessCurrency, cBaseAmount:=cBaseCurrency, cCurrencyamount:=vCreditTransactions(2, iVar))

                            vCreditTransactions(2, iVar) = cBaseCurrency
                        End If
                    Next
                End If
            End If

            If m_sNextOrionDocRef.Trim() = "" Then
                'Get the next document ref number
                m_lReturn = GetNextOrionDocRef()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(m_lReturn.ToString() + ", Start, GetNextOrionDocRef Failed")
                End If
            End If
            sFailureReason = ""

            If iPaymentAccountId = 0 Then
                'Get Intermediary Agent Account Id
                m_lReturn = GetPolicyIntermediaryAgentAccount(lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(vResultArray(6, 0)), lPaymentAccountId:=iPaymentAccountId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(m_lReturn.ToString() + ", Start, GetIntermediaryAgentAccountId Failed")
                End If
            End If

            m_oDatabase.SQLBeginTrans()

            m_lReturn = ProcessTransactions(lPaymentAccountId:=iPaymentAccountId, iDebitAgainst:=iDebitAgainst, vCreditTransactions:=vCreditTransactions, lCashListID:=lCashListID, lCashListItemId:=lCashListItemId, lCashListTransActionID:=lTransactionID, cTransactionAmount:=cTransactionAmount, sPaymentMethod:=sPaymentMethod, vDebitTransactions:=vDebitTransactions, bProcessSettleTransactions:=bProcessSettleTransactions, cRoundOffAmount:=cRoundOffAmount)

            m_sNextOrionDocRef = ""
            m_sNextOrionDocRefForInstalment = ""

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                Throw New System.Exception(m_lReturn.ToString() + ", Start, ProcessTransactions Failed" + sFailureReason)
            End If

            m_oDatabase.SQLCommitTrans()
            Return nResult

        Catch excep As System.Exception
            m_oDatabase.SQLRollbackTrans()
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    Function RollbackPolicyStatus(ByVal sOldPolicyNumber As String) As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("insurance_file_cnt", CStr(m_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("old_policy_number", sOldPolicyNumber, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                result = .SQLAction("spu_SIR_Rollback_Policy_Status", "RollbackPolicyStatus", True)
            End With

        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackPolicyStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally
        End Try
        Return result

    End Function

    ''' <summary>
    ''' This mehod is used to  PostRoundOffAmount amount.
    ''' </summary>
    ''' <param name="v_lAccountID"></param>
    ''' <param name="v_lRoundOffAccountId"></param>
    ''' <param name="v_lCompanyId"></param>
    ''' <param name="v_lSubBranchID"></param>
    ''' <param name="v_lCurrencyID"></param>
    ''' <param name="v_lCurrencyRate"></param>
    ''' <param name="v_cRoundOffAmount"></param>
    ''' <param name="v_dtAccountingDate"></param>
    ''' <param name="v_sInsuranceRef"></param>
    ''' <param name="v_vTransactions"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PostRoundOffAmount(ByVal v_lAccountID As Object, ByVal v_lRoundOffAccountId As Object,
                                     ByVal v_lCompanyId As Object, ByVal v_lSubBranchID As Object,
                                     ByVal v_lCurrencyID As Object, ByVal v_lCurrencyRate As Object,
                                     ByVal v_cRoundOffAmount As Object,
                                     ByVal v_dtAccountingDate As Object,
                                     ByVal v_sInsuranceRef As Object,
                                     ByRef v_vTransactions(,) As Object) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim lNumberRangeID, lNumber As Integer
        Dim sDocumentRef As String = ""
        Dim lDocumentId, lTransDetailId As Integer
        Dim oAllocationManual As bACTAllocationManual.Business = New bACTAllocationManual.Business
        Dim aoAllocationTransaction(0) As Object
        Dim oPMAutoNumber As bACTAutoNumber.Business = New bACTAutoNumber.Business
        Dim oDocumentPost As bACTDocumentPost.Form = New bACTDocumentPost.Form
        Const kMethodName As String = "PostRoundOffAmount"

        Try
            m_lReturn = oPMAutoNumber.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Initilizing the componenet bACTAutoNumber.Business failed", gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If

            oDocumentPost = New bACTDocumentPost.Form
            m_lReturn = oDocumentPost.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Initilizing the componenet bACTDocumentPost.Form failed", gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If

            ' Get the number range for a Journal
            m_lReturn = oPMAutoNumber.GetNumberRange(v_sGroupCode:=ACTAutoNumberGroupCodeDocumentRef56, v_sRangeCode:=ACTAutoNumberRangeCodeSRO, r_lNumberRangeID:=lNumberRangeID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Method(oPMAutoNumber.GetNumberRange) failed", gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If

            m_lReturn = oPMAutoNumber.GenerateNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=m_iUserID, v_iCompanyID:=v_lCompanyId, r_lNumber:=lNumber)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Method(oPMAutoNumber.GenerateNumber) failed", gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If

            ' Format the number
            sDocumentRef = StringsHelper.Format(lNumber, "0000000000")
            sDocumentRef = ACTAutoNumberRangeCodeSRO & sDocumentRef

            m_lReturn = oDocumentPost.AddDocument(v_lDocumentTypeId:=ACTDocTypeRoundOff, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=v_dtAccountingDate, v_sComment:="Round Off", r_vDocumentId:=lDocumentId, r_vDocSourceID:=v_lCompanyId, v_sReason:="", r_vSubBranchId:=v_lSubBranchID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Post the Credit (remember the TransDetail for Allocation)
            m_lReturn = oDocumentPost.AddTransaction(v_lAccountID:=v_lAccountID, v_vDocumentSequence:=1, v_iCurrencyID:=v_lCurrencyID, v_cAmount:=v_cRoundOffAmount, v_cCurrencyAmount:=v_cRoundOffAmount, v_vdCurrencyBaseXRate:=v_lCurrencyRate, v_vComment:="RoundOff", r_vTransDetailId:=lTransDetailId, v_vAccountingDate:=v_dtAccountingDate, v_vSpare:="RoundOff", v_vSubBranchId:=v_lSubBranchID, v_vInsuranceRef:=v_sInsuranceRef, v_vDocSourceID:=v_lCompanyId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Method(oDocumentPost.AddTransaction) failed", gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If

            ' Post the Debit (back to the same Account)
            ' Ignore the TransDetail for this item
            m_lReturn = oDocumentPost.AddTransaction(v_lAccountID:=v_lRoundOffAccountId, v_vDocumentSequence:=2, v_iCurrencyID:=v_lCurrencyID, v_cAmount:=-v_cRoundOffAmount, v_cCurrencyAmount:=-v_cRoundOffAmount, v_vdCurrencyBaseXRate:=v_lCurrencyRate, v_vComment:="RoundOff", r_vTransDetailId:=0, v_vAccountingDate:=DateTime.Today, v_vSubBranchId:=v_lSubBranchID, v_vInsuranceRef:=v_sInsuranceRef, v_vDocSourceID:=v_lCompanyId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Method(oDocumentPost.AddTransaction) failed", gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If

            'Use the bACTAllocationManual component to do the allocation
            'oAllocationManual = New bACTAllocationManual.Business
            If oAllocationManual.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Initilizing the componenet bACTAllocationManual.Business failed", gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If
            Dim vTransdetailId As Object = Nothing
            Dim cRoundOffAmount As Decimal
            'Set keys for the AllocationManual component
            Dim vKeyArray As Array = Array.CreateInstance(GetType(Object), New Integer() {gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue - gPMConstants.PMENavLetGetKeyColPosition.PMKeyName + 1, 5}, New Integer() {gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0})

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameRoundOffAmount
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameRoundOffTransDetailId
            For lRow As Integer = 0 To v_vTransactions.GetUpperBound(1)

                If CDbl(v_vTransactions(0, lRow)) = v_lAccountID Then
                    ReDim aoAllocationTransaction(0)
                    aoAllocationTransaction(0) = CStr(v_vTransactions(1, lRow)) & "|" & CStr(-v_cRoundOffAmount)
                    vTransdetailId = CDec(v_vTransactions(1, lRow))
                    cRoundOffAmount = v_cRoundOffAmount

                    v_vTransactions(2, lRow) = CDbl(v_vTransactions(2, lRow)) + v_cRoundOffAmount
                    Exit For
                End If
            Next lRow

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lAccountID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = CStr(lTransDetailId) & "|" & CStr(v_cRoundOffAmount)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = aoAllocationTransaction(0)
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = cRoundOffAmount
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = vTransdetailId

            'Perform the allocation
            With oAllocationManual

                If .SetKeys(vKeyArray:=CType(vKeyArray, Array)) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nResult
                End If

                If .Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Method(Start()) call failed", gPMConstants.PMELogLevel.PMLogError)
                    Return nResult
                End If
            End With

            oAllocationManual.Dispose()

            oPMAutoNumber.Dispose()

            oDocumentPost = Nothing
            oPMAutoNumber = Nothing
            oAllocationManual = Nothing
            aoAllocationTransaction = Nothing

            Return nResult
        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="PostRoundOffAmount", r_lFunctionReturn:=nResult)
            Return PMEReturnCode.PMFalse
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessFolder (Public)
    '
    ' Description:  Performs the required processing to re-process the reversed stats folder
    '
    ' SMJB 12/09/03 Optional v_bAutoAllocate parameter to allow us to supress auto-allocation
    ' Set to True for backwards compatability
    '
    ' ***************************************************************** '
    Public Function ProcessFolder(ByVal v_lStatsFolderCnt As Integer, Optional ByRef v_bAutoAllocate As Boolean = True) As Integer

        Dim result As Integer = 0
        Dim lTransactionExportFolderCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CreateExport(lStatsFolderCnt:=v_lStatsFolderCnt, lTransactionExportFolderCnt:=lTransactionExportFolderCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = PostDocument(v_lTransactionExportFolderCnt:=lTransactionExportFolderCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = DeleteCreditControlItem(v_lInsuranceFileCnt:=m_lInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'SMJB 12/09/03 Auto allocate, unless we have specifically been told not to
            If v_bAutoAllocate Then
                m_lReturn = ProcessAutoAllocation(v_lTransactionExportFolderCnt:=lTransactionExportFolderCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            Return result

        Catch excep As System.Exception

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Processfolder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFolder", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' ProcessTransactions
    ''' </summary>
    ''' <param name="lPaymentAccountId"></param>
    ''' <param name="iDebitAgainst"></param>
    ''' <param name="vCreditTransactions"></param>
    ''' <param name="lCashListID"></param>
    ''' <param name="lCashListItemId"></param>
    ''' <param name="lCashListTransActionID"></param>
    ''' <param name="cTransactionAmount"></param>
    ''' <param name="sPaymentMethod"></param>
    ''' <param name="vDebitTransactions"></param>
    ''' <param name="bProcessSettleTransactions"></param>
    ''' <param name="cRoundOffAmount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProcessTransactions(Optional ByRef lPaymentAccountId As Integer = 0, Optional ByRef iDebitAgainst As Integer = 0, Optional ByRef vCreditTransactions(,) As Object = Nothing, Optional ByRef lCashListID As Integer = 0, Optional ByRef lCashListItemId As Integer = 0, Optional ByRef lCashListTransActionID As Integer = 0, Optional ByRef cTransactionAmount As Decimal = 0, Optional ByRef sPaymentMethod As String = "", Optional ByRef vDebitTransactions As Object = Nothing, Optional ByRef bProcessSettleTransactions As Boolean = False, Optional ByRef cRoundOffAmount As Decimal = 0, Optional ByRef r_sFailureReason As String = "") As Integer
        Dim result As Integer = 0

        Const kMethodName As String = "ProcessTransactions"
        Const kAuthAccountsTransOptionNo As Integer = 81

        Dim nResult As Integer
        Dim oCashListPost As bACTCashListPost.Automated
        Dim nRoundOffAccountId As Integer
        Dim sInsuranceRef As String = ""
        Dim nSubBranchID As Integer
        Dim sRoundOffAccount As String = ""
        Dim nPartyAccountId As Integer
        Dim nCurrencyID As Integer
        Dim dcurrencyBaseRate As Double
        Dim nCompanyId As Integer
        Dim dtAccountingDate As Date
        Dim nAccount_Key As Integer

        Dim nStatsFolderCnt As Decimal
        Dim nTransactionExportFolderCnt As Integer
        Dim sAuthAccountsTransOptionVal As String = ""
        Dim oBusiness As bACTImportSiriusTrans.Business
        Dim oTransactions(,) As Object = Nothing
        Dim crAmountPaid As Decimal
        Dim bHasWriteOffAuthority As Boolean
        Dim crWriteOffLimit As Decimal

        Dim bCreditControlEnabled As Boolean
        Dim sOptionValue As String = ""
        Dim nReturn As gPMConstants.PMEReturnCode

        Dim sDocumentRef As String
        Dim nWriteOffReasonID As Integer
        Dim crWriteOffAmount As Decimal
        Dim bCurrencyWriteOff As Boolean
        Dim nAllocationStatus As Integer
        Dim oResults As Object(,) = Nothing
        Dim oAllocationManual As bACTAllocationManual.Business
        Dim oTransDetail As bACTTransdetail.Form
        Dim oPremiumFinance As bSIRPremiumFinance.Business
        Dim oDepositArray As Object
        Dim oKeyArray(1, 3) As Object
        Dim nTransDetailId As Integer
        Dim nInsuranceFileCnt As Integer
        Dim crTransdetailAmount As Decimal
        Dim crCrAmount As Decimal
        'Dim bFirstRow As Boolean
        Dim nOldInsuranceFileCnt As Integer
        Dim nDocumentId As Integer
        Dim crAmount As Decimal
        Dim oResultArray(,) As Object = Nothing
        Dim bChaseCycleEnabled As Boolean
        Dim crDrAmount As Decimal


        Try

            dtAccountingDate = DateTime.Now

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_sMessage = ""
            sDocumentRef = ""
            nWriteOffReasonID = 1
            bCurrencyWriteOff = False
            crWriteOffAmount = 0

            'Find the User Authority to WroteOff
            m_oDatabase.Parameters.Clear()
            If m_oDatabase.Parameters.Add("User_Id", CStr(m_iUserID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            nReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUserWriteOffLimitSQL, sSQLName:=ACGetUserWriteOffLimitName, bStoredProcedure:=ACGetUserWriteOffLimitStored, vResultArray:=oResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get Write Off Limit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If
            If Informations.IsArray(oResults) Then

                bHasWriteOffAuthority = gPMFunctions.ToSafeBoolean(CDbl(oResults(0, 0)))
                If bHasWriteOffAuthority Then

                    crWriteOffLimit = gPMFunctions.ToSafeCurrency(CDbl(oResults(1, 0)))
                End If
            End If

            oResults = Nothing
            m_oDatabase.Parameters.Clear()
            If m_oDatabase.Parameters.Add("CashListItem_id", CStr(lCashListItemId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            nReturn = m_oDatabase.SQLSelect(sSQL:="spu_ACT_Select_CashListItem", sSQLName:="spu_ACT_Select_CashListItem", bStoredProcedure:=True, vResultArray:=oResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessTransactions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashListDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If

            If oResults IsNot Nothing AndAlso Informations.IsArray(oResults) Then
                crAmountPaid = gPMFunctions.ToSafeCurrency(gPMMaths.PMRoundupValueCurrency(CDbl(oResults(8, 0)) * CDbl(oResults(55, 0)), gPMConstants.PMECurrencyNoOfDP.pmeCurDPTwo, gPMConstants.PMERoundupFactor.pmeRFactor50Up))
            End If

            If vCreditTransactions IsNot Nothing AndAlso Informations.IsArray(vCreditTransactions) Then
                If ToSafeInteger(vCreditTransactions(1, 0), 0) <> lCashListTransActionID Then
                    lCashListItemId = 0
                End If
            End If

            If lCashListItemId <> 0 Then
                crWriteOffAmount = cTransactionAmount + cRoundOffAmount - crAmountPaid
                If bHasWriteOffAuthority And crWriteOffLimit >= Math.Abs(crWriteOffAmount) And m_sCallingAppName <> "iPMUQuoteCollectionProcess" Then

                Else
                    crWriteOffAmount = 0
                End If
            End If

            'Update Insurance File System
            If lPaymentAccountId <> 0 Then
                'Pay Now Option Selected
                m_lReturn = UpdateInsuranceFileSystem(iDebitAgainst:=iDebitAgainst, lPaymentAccountId:=lPaymentAccountId)
            Else
                m_lReturn = UpdateInsuranceFileSystem()
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                m_sMessage = "Failed to update Insurance File System"
                Return nResult
            End If

            ' Create the Stats tables
            m_lReturn = CreateStats(nStatsFolderCnt:=nStatsFolderCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                m_sMessage = "Failed to create Statistics"
                Return nResult
            End If

            m_lReturn = CreateExport(lStatsFolderCnt:=nStatsFolderCnt, lTransactionExportFolderCnt:=nTransactionExportFolderCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                m_sMessage = "Failed to create Transactions"
                Return nResult
            End If

            'get system option value
            If bPMFunc.GetSystemOption(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, v_iOptionNumber:=kAuthAccountsTransOptionNo, r_sOptionValue:=sAuthAccountsTransOptionVal) <> gPMConstants.PMEReturnCode.PMTrue Then
                sAuthAccountsTransOptionVal = CStr(gPMConstants.PMEReturnCode.PMFalse)
            End If

            'Float Balance and Pre-Payment

            If CInt(sAuthAccountsTransOptionVal) = gPMConstants.PMEReturnCode.PMFalse Then
                'Call the routine to post the transaction to the Orion
                m_lReturn = PostDocument(v_lTransactionExportFolderCnt:=nTransactionExportFolderCnt, r_vTransactions:=oTransactions, sPaymentMethod:=sPaymentMethod, r_sFailureReason:=r_sFailureReason)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    m_sMessage = "Failed to post Transactions to Orion" + r_sFailureReason
                    Return nResult
                ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If
            End If

            Dim oEnablePayNowOptionsValue As Object = Nothing

            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:="", v_sPassword:="", v_iUserID:=0, v_iMainSourceID:=0, v_iLanguageID:=0, v_iCurrencyID:=0, v_iLogLevel:=0, v_sCallingAppName:="", v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnablePayNowOptions, v_vBranch:=1, r_vUnderwriting:=oEnablePayNowOptionsValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = m_lReturn
                m_sMessage = "Failed to get product option."
                Return nResult
            End If

            Dim oMergedArray As Object = Nothing

            If iDebitAgainst <> gPMConstants.PMDebitAgainst.PMDebitAgainstCashDeposit Or (iDebitAgainst = gPMConstants.PMDebitAgainst.PMDebitAgainstCashDeposit And Informations.IsArray(vCreditTransactions)) Then

                If lPaymentAccountId <> 0 Then
                    'Float Balance and pre-payment
                    If Informations.IsArray(vCreditTransactions) And TransactionType <> "MTC" And TransactionType <> "MTA" Then
                        m_lReturn = MergeArrays(oTransactions, vCreditTransactions, oMergedArray)
                        oBusiness = New bACTImportSiriusTrans.Business
                        If oBusiness.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database)) = gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = oBusiness.AutoAllocateCancellingTransactions(vTransactions:=oMergedArray, bAllowPartialAllocation:=True, cWriteOffAmount:=crWriteOffAmount,
                                                                                     lWriteOffReasonID:=nWriteOffReasonID, bCurrencyWriteOff:=bCurrencyWriteOff, lCashListItemId:=lCashListItemId,
                                                                                     v_lPaymentAccount:=lPaymentAccountId)
                        Else
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                m_sMessage = "Failed to create Business object"
                                Return nResult
                            End If
                        End If

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            m_sMessage = "Failed to Auto Allocate Cancelling Transactions"
                            Return nResult
                        End If

                        If Not (Convert.IsDBNull(vCreditTransactions) Or Informations.IsNothing(vCreditTransactions)) Then
                            For icount As Integer = 0 To vCreditTransactions.GetUpperBound(1)
                                nTransDetailId = gPMFunctions.ToSafeLong(CDbl(vCreditTransactions(1, icount)), 0)
                                crAmount = gPMFunctions.ToSafeCurrency(CDbl(vCreditTransactions(2, icount)), 0)
                                With m_oDatabase
                                    .Parameters.Clear()
                                    m_lReturn = .Parameters.Add(sName:="Transdetail_id", vValue:=CStr(nTransDetailId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If

                                    m_lReturn = .SQLSelect(sSQL:=ACGetDocumentFromTransdetailSQL, sSQLName:=ACGetDocumentFromTransdetailName, bStoredProcedure:=ACGetDocumentFromTransdetailStored, vResultArray:=oResultArray)

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If

                                End With
                                If Informations.IsArray(oResultArray) Then

                                    nDocumentId = gPMFunctions.ToSafeLong(CDbl(oResultArray(0, 0)))
                                Else
                                    nDocumentId = 0
                                End If

                                If m_lInsuranceFileCnt > 0 And nTransDetailId > 0 Then
                                    m_lReturn = InsertInsuranceFilePaymentDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lCashListItemId:=lCashListItemId, v_lDocumentId:=nDocumentId, v_lTransdetailId:=nTransDetailId, v_cAmount:=crAmount)

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        nResult = gPMConstants.PMEReturnCode.PMFalse
                                        m_sMessage = "Unable to Insert values in Insurance_File_Payment_Details"
                                        Return nResult
                                    End If
                                End If
                            Next
                        End If
                    Else
                        If (sPaymentMethod).ToLower() = "paynow" Then

                            oCashListPost = New bACTCashListPost.Automated
                            m_lReturn = oCashListPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                m_sMessage = "Unable to create Business Object"
                                Return nResult
                            End If

                            'Create bSIRPremiumFinance.Business

                            oPremiumFinance = New bSIRPremiumFinance.Business
                            m_lReturn = oPremiumFinance.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRPremiumFinance.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                Return m_lReturn
                            End If

                            oCashListPost.CashListTransactionID = lCashListTransActionID

                            oCashListPost.CallingAppName = m_sCallingAppName

                            If m_sTransactionType = "MTC" And Not (Informations.IsNothing(vDebitTransactions)) And cTransactionAmount > 0 Then
                                'Receipt has been processed in Pay Now so we will allocate it against the SED transaction
                                If Not Informations.IsNothing(vDebitTransactions(0)) Then
                                    m_lReturn = oPremiumFinance.GetInsuranceFileCnt(v_lTransdetailId:=gPMFunctions.ToSafeLong(CDbl(vDebitTransactions(0))), r_lInsuranceFileCnt:=nInsuranceFileCnt)
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        gPMFunctions.RaiseError(kMethodName, "Failed to get insurance file", gPMConstants.PMELogLevel.PMLogError)
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If
                                End If
                            Else
                                nInsuranceFileCnt = m_lInsuranceFileCnt
                            End If

                            m_lReturn = oCashListPost.PostAllocatedCashListItem(lCashListID:=lCashListID, lCashListItemId:=lCashListItemId, lInsuranceFileCnt:=nInsuranceFileCnt, sDocumentRef:=sDocumentRef, lWriteOffReasonID:=nWriteOffReasonID, cWriteOffAmount:=crWriteOffAmount, bCurrencyWriteOff:=bCurrencyWriteOff, r_iAllocationStatus:=nAllocationStatus)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return nResult
                            End If

                            'WPR12- Enhancement Quote Collection Process
                            With m_oDatabase
                                .Parameters.Clear()
                                m_lReturn = .Parameters.Add(sName:="Transdetail_id", vValue:=CStr(lCashListTransActionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If

                                m_lReturn = .SQLSelect(sSQL:=ACGetDocumentFromTransdetailSQL, sSQLName:=ACGetDocumentFromTransdetailName, bStoredProcedure:=ACGetDocumentFromTransdetailStored, vResultArray:=oResultArray)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If
                            End With

                            If Informations.IsArray(oResultArray) Then

                                nDocumentId = gPMFunctions.ToSafeLong(CDbl(oResultArray(0, 0)))
                            Else
                                nDocumentId = 0
                            End If

                            If m_lInsuranceFileCnt > 0 And lCashListTransActionID > 0 Then
                                m_lReturn = InsertInsuranceFilePaymentDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lCashListItemId:=lCashListItemId, v_lDocumentId:=nDocumentId, v_lTransdetailId:=lCashListTransActionID, v_cAmount:=cTransactionAmount)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    nResult = gPMConstants.PMEReturnCode.PMFalse
                                    m_sMessage = "Unable to Insert values in Insurance_File_Payment_Details"
                                    Return nResult
                                End If
                            End If
                        End If
                    End If
                End If

            End If

            If Math.Round(cRoundOffAmount, 2) <> 0 Then
                m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=kSystemOptionRoundOffAccount, r_sOptionValue:=sRoundOffAccount, v_iSourceID:=m_iSourceID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to get System Option", gPMConstants.PMELogLevel.PMLogError)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = GetInsuranceFileInfo(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_sInsuranceRef:=sInsuranceRef, r_lCompanyId:=nCompanyId, r_lPartyAccountId:=nPartyAccountId, r_lCurrencyId:=nCurrencyID, r_dcurrencyBaseRate:=dcurrencyBaseRate, r_dtAccountingDate:=dtAccountingDate, r_lSubBranchId:=nSubBranchID, r_lAccount_Key:=nAccount_Key, v_lTransactionExportFolderCnt:=nTransactionExportFolderCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to get GetInsuranceFileInfo", gPMConstants.PMELogLevel.PMLogError)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = GetRoundOffAccount(v_sAccountShortCode:=sRoundOffAccount, v_lAccount_Key:=nAccount_Key, v_lSubBranchID:=nSubBranchID, v_lCurrencyID:=nCurrencyID, r_lAccountId:=nRoundOffAccountId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to Get RoundOff Account ", gPMConstants.PMELogLevel.PMLogError)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = PostRoundOffAmount(v_lAccountID:=If(iDebitAgainst = 4 Or iDebitAgainst = 3, lPaymentAccountId, nPartyAccountId), v_lRoundOffAccountId:=nRoundOffAccountId, v_lCompanyId:=nCompanyId, v_lSubBranchID:=nSubBranchID, v_lCurrencyID:=nCurrencyID, v_lCurrencyRate:=dcurrencyBaseRate, v_cRoundOffAmount:=cRoundOffAmount, v_dtAccountingDate:=dtAccountingDate, v_sInsuranceRef:=sInsuranceRef, v_vTransactions:=oTransactions)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to get PostRoundOffAmount", gPMConstants.PMELogLevel.PMLogError)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Dim sSysOptionValue As String = ""
            If bProcessSettleTransactions Then
                'get system option "Cancel Instalment Plan on Policy Cancellation"
                m_lReturn = GetSystemOptionLite(v_iOptionNumber:=5076, r_sOptionValue:=sSysOptionValue, v_iSourceID:=m_iSourceID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Could not get System Option: Cancel Instalment Plan on Policy Cancellation", gPMConstants.PMELogLevel.PMLogError)
                    Return nResult
                End If

                If gPMFunctions.ToSafeLong(CDbl(sSysOptionValue), 0) = 1 Then
                    ' Get an instance of bACTTransDetail
                    oTransDetail = New bACTTransdetail.Form
                    If oTransDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bACTTransDetail", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oAllocationManual = New bACTAllocationManual.Business
                    'Get an instance of bACTAllocationManual component to do the allocation
                    If oAllocationManual.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bACTAllocationManual", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Get the party account for allocations
                    m_lReturn = GetInsuranceFileInformation(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_lAccountId:=lPaymentAccountId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Failed to get insurance file details", gPMConstants.PMELogLevel.PMLogError)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ReDim oDepositArray(0)

                    If Informations.IsArray(oTransactions) Then
                        For lRow As Integer = oTransactions.GetLowerBound(1) To oTransactions.GetUpperBound(1)

                            If gPMFunctions.ToSafeLong(CDbl(oTransactions(0, lRow))) = lPaymentAccountId Then
                                'Set the first SEC transaction as Main Credit Transaction
                                'and put others in the allocation array

                                oDepositArray(oDepositArray.GetUpperBound(0)) = gPMFunctions.ToSafeLong(CDbl(oTransactions(1, lRow)))

                                ReDim Preserve oDepositArray(oDepositArray.GetUpperBound(0) + 1)

                                nTransDetailId = gPMFunctions.ToSafeLong(CDbl(oTransactions(1, lRow)))
                            End If
                        Next lRow
                    End If

                    If (nTransDetailId <> 0) And (Not Informations.IsNothing(vDebitTransactions)) And Not (vDebitTransactions Is Nothing) Then

                        If Informations.IsArray(vDebitTransactions) Then
                            For Each vDebitTransactions_item As Object In vDebitTransactions

                                If gPMFunctions.ToSafeLong(CDbl(vDebitTransactions_item)) <> 0 Then

                                    oDepositArray(oDepositArray.GetUpperBound(0)) = gPMFunctions.ToSafeLong(CDbl(vDebitTransactions_item))

                                    ReDim Preserve oDepositArray(oDepositArray.GetUpperBound(0) + 1)
                                End If
                            Next vDebitTransactions_item
                        End If

                        ReDim Preserve oDepositArray(oDepositArray.GetUpperBound(0) - 1)

                        crCrAmount = 0
                        crDrAmount = 0

                        Dim crCrOutstandingTotal, crDrOutstandingTotal, crAmounttoAllocate As Decimal
                        Dim lMatchRow, lMainRow As Integer
                        crAmounttoAllocate = 0


                        For lRow As Integer = oDepositArray.GetLowerBound(0) To oDepositArray.GetUpperBound(0)
                            m_lReturn = oTransDetail.GetDetails(vTransdetailID:=oDepositArray(lRow))
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "Failed to get transaction details", gPMConstants.PMELogLevel.PMLogError)
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            m_lReturn = oTransDetail.GetNext(vOSBaseAmount:=crTransdetailAmount)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "Failed to get transaction details", gPMConstants.PMELogLevel.PMLogError)
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            If crTransdetailAmount < 0 Then
                                crCrAmount += crTransdetailAmount
                            Else
                                crDrAmount += crTransdetailAmount
                            End If
                        Next lRow
                        If crCrAmount <> 0 AndAlso crDrAmount <> 0 Then

                            If Math.Abs(crDrAmount) >= Math.Abs(crCrAmount) Then
                                crCrOutstandingTotal = crCrAmount
                                crDrOutstandingTotal = Math.Abs(crCrAmount) * Math.Sign(crDrAmount)
                            Else
                                crCrOutstandingTotal = Math.Abs(crDrAmount) * Math.Sign(crCrAmount)
                                crDrOutstandingTotal = crDrAmount
                            End If

                            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID

                            For lRow As Integer = oDepositArray.GetLowerBound(0) To oDepositArray.GetUpperBound(0)

                                m_lReturn = oTransDetail.GetDetails(vTransdetailID:=oDepositArray(lRow))
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    gPMFunctions.RaiseError(kMethodName, "Failed to get transaction details", gPMConstants.PMELogLevel.PMLogError)
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If

                                m_lReturn = oTransDetail.GetNext(vOSBaseAmount:=crTransdetailAmount)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    gPMFunctions.RaiseError(kMethodName, "Failed to get transaction details", gPMConstants.PMELogLevel.PMLogError)
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If

                                'oDepositArray(lRow) = CStr(oDepositArray(lRow)) & "|" & CStr(crTransdetailAmount)
                                'crCrAmount += crTransdetailAmount
                                If Math.Sign(crCrOutstandingTotal) = Math.Sign(gPMFunctions.ToSafeDouble(crTransdetailAmount)) Then
                                    If Math.Abs(crCrOutstandingTotal) <= Math.Abs(gPMFunctions.ToSafeDouble(crTransdetailAmount)) Then
                                        If lMainRow = lRow Then
                                            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = gPMFunctions.ToSafeString(oDepositArray(lRow)) & "|" & CStr(Math.Abs(crCrOutstandingTotal) * Math.Sign(gPMFunctions.ToSafeDouble(crTransdetailAmount)))
                                        Else
                                            oDepositArray(lMatchRow) = gPMFunctions.ToSafeString(oDepositArray(lRow)) & "|" & CStr(Math.Abs(crCrOutstandingTotal) * Math.Sign(gPMFunctions.ToSafeDouble(crTransdetailAmount)))
                                            lMatchRow += 1
                                        End If
                                        crCrOutstandingTotal = 0
                                    Else
                                        If lRow <> oDepositArray.GetUpperBound(0) Then
                                            If lMainRow = lRow Then
                                                oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = gPMFunctions.ToSafeString(oDepositArray(lRow)) & "|" & gPMFunctions.ToSafeString(crTransdetailAmount, "0")
                                            Else
                                                oDepositArray(lMatchRow) = gPMFunctions.ToSafeString(oDepositArray(lRow)) & "|" & gPMFunctions.ToSafeString(crTransdetailAmount, "0")
                                                lMatchRow += 1
                                            End If
                                            crCrOutstandingTotal = (Math.Abs(crCrOutstandingTotal) - Math.Abs(Convert.ToDecimal(gPMFunctions.ToSafeString(crTransdetailAmount, "0")))) * Math.Sign(gPMFunctions.ToSafeDouble(crTransdetailAmount))
                                        Else
                                            If Math.Abs(crCrOutstandingTotal) > 0 Then
                                                crAmounttoAllocate = crCrOutstandingTotal
                                                If lMainRow = lRow Then
                                                    oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = gPMFunctions.ToSafeString(oDepositArray(lRow)) & "|" & (CStr(Math.Abs(crAmounttoAllocate) * Math.Sign(gPMFunctions.ToSafeDouble(crTransdetailAmount))))
                                                Else
                                                    oDepositArray(lMatchRow) = gPMFunctions.ToSafeString(oDepositArray(lRow)) & "|" & (CStr(Math.Abs(crAmounttoAllocate) * Math.Sign(gPMFunctions.ToSafeDouble(crTransdetailAmount))))
                                                    lMatchRow += 1
                                                End If
                                            End If
                                        End If
                                    End If
                                ElseIf Math.Sign(crDrOutstandingTotal) = Math.Sign(gPMFunctions.ToSafeDouble(crTransdetailAmount)) Then
                                    If Math.Abs(crDrOutstandingTotal) <= Math.Abs(gPMFunctions.ToSafeDouble(crTransdetailAmount)) Then
                                        If lMainRow = lRow Then
                                            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = gPMFunctions.ToSafeString(oDepositArray(lRow)) & "|" & CStr(Math.Abs(crDrOutstandingTotal) * Math.Sign(gPMFunctions.ToSafeDouble(crTransdetailAmount)))
                                        Else
                                            oDepositArray(lMatchRow) = gPMFunctions.ToSafeString(oDepositArray(lRow)) & "|" & CStr(Math.Abs(crDrOutstandingTotal) * Math.Sign(gPMFunctions.ToSafeDouble(crTransdetailAmount)))
                                            lMatchRow += 1
                                        End If
                                        crDrOutstandingTotal = 0
                                    Else
                                        If lRow <> oDepositArray.GetUpperBound(0) Then
                                            If lMainRow = lRow Then
                                                oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = gPMFunctions.ToSafeString(oDepositArray(lRow)) & "|" & gPMFunctions.ToSafeString(crTransdetailAmount, "0")
                                            Else
                                                oDepositArray(lMatchRow) = gPMFunctions.ToSafeString(oDepositArray(lRow)) & "|" & gPMFunctions.ToSafeString(crTransdetailAmount, "0")
                                                lMatchRow += 1
                                            End If
                                            crDrOutstandingTotal = (Math.Abs(crDrOutstandingTotal) - Math.Abs(Convert.ToDecimal(gPMFunctions.ToSafeString(crTransdetailAmount, "0")))) * Math.Sign(gPMFunctions.ToSafeDouble(crTransdetailAmount))
                                        Else
                                            If Math.Abs(crDrOutstandingTotal) > 0 Then
                                                crAmounttoAllocate = crDrOutstandingTotal
                                                If lMainRow = lRow Then
                                                    oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = gPMFunctions.ToSafeString(oDepositArray(lRow)) & "|" & (CStr(Math.Abs(crAmounttoAllocate) * Math.Sign(gPMFunctions.ToSafeDouble(crTransdetailAmount))))
                                                Else
                                                    oDepositArray(lMatchRow) = gPMFunctions.ToSafeString(oDepositArray(lRow)) & "|" & (CStr(Math.Abs(crAmounttoAllocate) * Math.Sign(gPMFunctions.ToSafeDouble(crTransdetailAmount))))
                                                    lMatchRow += 1
                                                End If
                                            End If
                                        End If
                                    End If
                                Else
                                    oDepositArray(lMatchRow) = gPMFunctions.ToSafeString(oDepositArray(lRow)) & "|" & "0"
                                    lMatchRow += 1
                                End If
                            Next lRow
                            ReDim Preserve oDepositArray(oDepositArray.GetUpperBound(0) - 1)

                            ' AccountID

                            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

                            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = lPaymentAccountId

                            ' Matched Transactions

                            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs

                            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = oDepositArray

                            'Perform the allocation
                            With oAllocationManual

                                If .SetKeys(vKeyArray:=oKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Allocation Manual Failed with Set Keys", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If

                                If .Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to allocate transactions", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If

                                .Dispose()
                            End With
                            oAllocationManual = Nothing
                        End If
                    End If
                End If
            End If

                ' if credit control enabled
                nReturn = CType(GetSystemOptionLite(v_iOptionNumber:=kSystemOptionCreditControlEnabled, r_sOptionValue:=sOptionValue, v_iSourceID:=1), gPMConstants.PMEReturnCode)

                If nReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    bCreditControlEnabled = (sOptionValue = "1")
                End If

            If bCreditControlEnabled Then
                ' Add a credit control item
                ' NB: if this policy is later moved onto instalments the credit control item
                ' will be deleted by the instalment process.

                Select Case m_sTransactionType
                    Case "NB", "MTA", "REN"

                        ' New Business, Mid Term Adjustment, Renewals
                        m_lReturn = AddCreditControlItem(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_sBusinessType:=m_sTransactionType)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            m_sMessage = "Failed to add Credit Control Item"
                            Return nResult
                        End If

                    Case "MTC"

                        m_lReturn = UpdateCreditControlItem(v_lInsuranceFileCnt:=m_lInsuranceFileCnt)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            m_sMessage = "Failed to Update Credit Control Item"
                            Return result
                        End If
                    Case Else

                End Select

            End If
            ' if Chase Cycle enabled
            nReturn = CType(GetSystemOptionLite(v_iOptionNumber:=kSystemOptionChaseCycleEnabled, r_sOptionValue:=sOptionValue, v_iSourceID:=1), gPMConstants.PMEReturnCode)

                If nReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    bChaseCycleEnabled = (sOptionValue = "1")
                End If

                If bChaseCycleEnabled Then
                    ' Add a Chase Cycle item
                    ' NB: if this policy is later moved onto instalments the Chase Cycle item
                    ' will be deleted by the instalment process.

                    m_lReturn = AddChaseCycleItem(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_sBusinessType:=m_sTransactionType)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        m_sMessage = "Failed to add Chase Cycle Item"
                        Return nResult
                    End If
                End If

                'Modified by Vijay Pal on 5/10/2010 11:10:05 AM Error By To do list,so i comment the if loop
                If iDebitAgainst = gPMConstants.PMDebitAgainst.PMDebitAgainstCashDeposit Then
                    m_lReturn = UpdateCashDepositPolicyLink(v_lCashDepositAccountId:=lPaymentAccountId, v_lInsuranceFileCnt:=m_lInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Failed to update policy cash deposit details", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

                If m_sTransactionType = "MTA" Then
                    m_lReturn = GetPreviousInsuranceFile(v_lNewInsuranceFileCnt:=m_lInsuranceFileCnt, r_lOldInsuranceFileCnt:=nOldInsuranceFileCnt)

                    If gPMFunctions.ToSafeLong(nOldInsuranceFileCnt) > 0 Then
                        m_oDatabase.Parameters.Clear()
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="old_insurance_file_cnt", vValue:=CStr(nOldInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="new_insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACMoveSuspendedAgentCommissionSQL, sSQLName:=ACMoveSuspendedAgentCommissionName, bStoredProcedure:=ACMoveSuspendedAgentCommissionStored, vResultArray:=oResults)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to move the Suspended Agent Commission", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If Informations.IsArray(oResults) Then

                            For ctr As Integer = 0 To oResults.GetUpperBound(1)

                                m_lReturn = AllocateDocuments(CInt(oResults(0, ctr)), CInt(oResults(1, ctr)))
                            Next ctr
                        End If

                    End If
                End If

                Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessTransactions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'Modified by Vijay Pal on 5/10/2010 11:11:05 AM Error By To do list,so i comment the if loop
            If iDebitAgainst = gPMConstants.PMDebitAgainst.PMDebitAgainstCashDeposit And Not m_bByPassLocking Then
                If UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=lPaymentAccountId, v_lUserID:=m_iUserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                    'MessageBox.Show("Failed to unlock KeyName: " & ACLockName & "for " & CStr(lPaymentAccountId), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Informations)
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unlock KeyName: " & ACLockName & "for " & CStr(lPaymentAccountId), vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
                End If
            End If
            Return nResult

        End Try
    End Function

    Private Function UpdateInsuranceFileSystem(Optional ByRef iDebitAgainst As Integer = 0, Optional ByRef lPaymentAccountId As Integer = 0) As Integer
        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type", vValue:=m_sTransactionType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'Float balance and Pre-Payment work
        If iDebitAgainst <> 0 Then
            m_lReturn = m_oDatabase.Parameters.Add(sName:="debitagainst", vValue:=CStr(iDebitAgainst), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        Else

            m_lReturn = m_oDatabase.Parameters.Add(sName:="debitagainst", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If lPaymentAccountId <> 0 Then
            m_lReturn = m_oDatabase.Parameters.Add(sName:="paymentAccountId", vValue:=CStr(lPaymentAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        Else

            m_lReturn = m_oDatabase.Parameters.Add(sName:="paymentAccountId", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="lastTransDate", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSystemSQL, sSQLName:=ACUpdateSystemName, bStoredProcedure:=ACUpdateSystemStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Createstats (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function CreateStats(ByRef nStatsFolderCnt As Integer) As Integer

        Dim nResult As Integer
        Dim nTransactionExportFolderCnt As Integer
        Dim oTransactions As Object = Nothing
        Dim bIsPT As Boolean


        nResult = gPMConstants.PMEReturnCode.PMTrue

        If m_bIsCloned Then

            m_lReturn = IsPortfolioTransferVersion(m_lInsuranceFileCnt, bIsPT, m_dtTransferDate)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bIsPT = bIsPT
            m_nClonedReversalDocumentID = 0
            If bIsPT = False Then
                m_lReturn = CreateStatsFolder(lStatsFolderCnt:=nStatsFolderCnt, IsClonedReverse:=True)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = CreateClonedStatsDetails(lStatsFolderCnt:=nStatsFolderCnt, lClonedInsuranceFileCnt:=m_lInsuranceFileCnt)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = CreateClonedExport(nStatsFolderCnt:=nStatsFolderCnt, r_nTransactionExportFolderCnt:=nTransactionExportFolderCnt, nInsuranceFileCnt:=m_lInsuranceFileCnt)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = PostDocument(v_lTransactionExportFolderCnt:=nTransactionExportFolderCnt, r_vTransactions:=oTransactions, r_nDocumentID:=m_nClonedReversalDocumentID)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Informations.IsArray(oTransactions) Then
                    m_lReturn = ProcessClonedTransactions(nStatsFolderCnt:=nStatsFolderCnt, oTransactions:=oTransactions)

                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If
        End If


        m_lReturn = CreateStatsFolder(lStatsFolderCnt:=nStatsFolderCnt)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '    If m_bIsCloned Then
        '          m_lReturn& = CreateClonedStatsDetails( _
        '                    lStatsFolderCnt:=lStatsFolderCnt, _
        '                    lClonedInsuranceFileCnt:=m_lInsuranceFileCnt)
        '
        '            If (m_lReturn& <> PMTrue) Then
        '                CreateStats = PMFalse
        '                Exit Function
        '            End If
        '
        '    End If
		If m_bReverseVoid Then
            m_lReturn = CreateReverseStatsDetails(lStatsFolderCnt:=nStatsFolderCnt, lInsuranceFileCnt:=m_lOriginalInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        Else
			If m_bReversePT Then
				m_lReturn = CreatePTStatsDetails(lStatsFolderCnt:=nStatsFolderCnt, lPTInsuranceFileCnt:=m_lPTInsuranceFileCnt)

				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If

			End If


			m_lReturn = CreateStatsDetails(lStatsFolderCnt:=nStatsFolderCnt)

			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
		End If
		
        Return m_lReturn

    End Function
	
	Private Function CreateReverseStatsDetails(lStatsFolderCnt As Long, lInsuranceFileCnt As Long) As Long

        Dim lRecordsAffected As Long


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        With m_oDatabase

            ' Add StatsFolderCnt as an INPUT param for an insert
            m_lReturn = .Parameters.Add(sName:="nInsuranceFileCnt", vValue:=lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_lReturn = .Parameters.Add(sName:="nStatsFolderCnt", vValue:=lStatsFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End With

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyStatsDetailsRevSQL, sSQLName:=ACCopyStatsDetailsRevName, bStoredProcedure:=ACCopyStatsDetailsRevStored, lRecordsAffected:=lRecordsAffected)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        Return m_lReturn
    End Function
	
    ''' <summary>
    ''' CreateStatsFolder
    ''' </summary>
    ''' <param name="lStatsFolderCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Overloads Function CreateStatsFolder(ByRef lStatsFolderCnt As Integer) As Integer
        m_lReturn = CreateStatsFolder(lStatsFolderCnt, False)
        Return m_lReturn
    End Function
    ''' <summary>
    ''' CreateStatsFolder
    ''' </summary>
    ''' <param name="lStatsFolderCnt"></param>
    ''' <param name="IsClonedReverse"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Overloads Function CreateStatsFolder(ByRef lStatsFolderCnt As Integer, ByVal IsClonedReverse As Boolean) As Integer
        Dim nResult As Integer
        Dim lRecordsAffected As Integer


        nResult = PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(0), iDirection:=PMEParameterDirection.PMParamOutput, iDataType:=PMEDataType.PMInteger)

            m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)

            m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)

            m_lReturn = .Parameters.Add(sName:="user_name", vValue:=m_sUsername, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

            m_lReturn = .Parameters.Add(sName:="next_orion_doc_ref", vValue:=m_sNextOrionDocRef, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)


            If m_bBackDateMTA = True Then
                m_lReturn = .Parameters.Add(sName:="is_out_of_sequence", vValue:=1, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
            End If

            If m_bIsPT Then
                m_lReturn = .Parameters.Add(sName:="transfer_date", vValue:=m_dtTransferDate, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMDate)
            End If

            If IsClonedReverse = True Then
                m_lReturn = .Parameters.Add(sName:="is_cloned_reverse", vValue:=1, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
            End If
        End With

        ' Execute Add Stats Folder SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddStatsFolderSQL, sSQLName:=ACAddStatsFolderName, bStoredProcedure:=ACAddStatsFolderStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        ' Get the Cnt of the record inserted
        lStatsFolderCnt = m_oDatabase.Parameters.Item("stats_folder_cnt").Value
        If lStatsFolderCnt < 1 Then
            Return PMEReturnCode.PMFalse
        End If

        Return nResult

    End Function
    ' ***************************************************************** '
    ' Name: CreateStatsDetails (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function CreateStatsDetails(ByRef lStatsFolderCnt As Integer) As Integer

        Dim nResult As Integer
        Dim nRecordsAffected As Integer



        nResult = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        With m_oDatabase

            ' Add StatsFolderCnt as an INPUT param for an insert
            m_lReturn = .Parameters.Add(sName:="stats_folder_cnt", vValue:=lStatsFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add StatsFolderCnt as an INPUT param for an insert
            m_lReturn = .Parameters.Add(sName:="only_ri", vValue:=If(m_bIsPT, 1, IIf(m_bIsCloned Or TransactionType = "DRI", 2, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        ' Execute Add Stats Details SQL Statement
        'SR 03/11/2K
        ' This SP takes more than 30 seconds.
        ' Since the timeout in PMDAO is 15 secs only, it terminates with timeout error. need checking.
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddStatsDetailsSQL, sSQLName:=ACAddStatsDetailsName, bStoredProcedure:=ACAddStatsDetailsStored, lRecordsAffected:=nRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'SR  03/11/2K - It always returns zero even if some detail records are added.
        'Needs checking
        '    If (lRecordsAffected < 1) Then
        '        CreateStatsDetails = PMFalse
        '        Exit Function
        '    End If

        Return nResult

    End Function

    ' ***************************************************************** '
    ' Name: CreateExport (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function CreateExport(ByRef lStatsFolderCnt As Integer, ByRef lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = CreateExportFolder(lStatsFolderCnt:=lStatsFolderCnt, lTransactionExportFolderCnt:=lTransactionExportFolderCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not (lStatsFolderCnt > 0) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = CreateExportDetails(lStatsFolderCnt:=lStatsFolderCnt, lTransactionExportFolderCnt:=lTransactionExportFolderCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CreateExportFolder (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function CreateExportFolder(ByRef lStatsFolderCnt As Integer, ByRef lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        With m_oDatabase

            ' Add the Input Params
            If lStatsFolderCnt = Nothing Then
                m_lReturn = .Parameters.Add(sName:="stats_folder_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="stats_folder_cnt", vValue:=lStatsFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add ExportFolderCnt as an OUTPUT param for an insert
            If lTransactionExportFolderCnt = Nothing Then
                m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=lTransactionExportFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        ' Execute Add Trans Export Folder SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddExportFolderSQL, sSQLName:=ACAddExportFolderName, bStoredProcedure:=ACAddExportFolderStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the Cnt of the record inserted
        lTransactionExportFolderCnt = m_oDatabase.Parameters.Item("transaction_export_folder_cnt").Value
        If lTransactionExportFolderCnt < 1 Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: CreateExportDetails (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function CreateExportDetails(ByRef lStatsFolderCnt As Integer, ByRef lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        With m_oDatabase

            ' Add the Input Params
            m_lReturn = .Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(lStatsFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add ExportFolderCnt as an INPUT param for an insert
            m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(lTransactionExportFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        ' Execute Add Trans Export Details SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddExportDetailsSQL, sSQLName:=ACAddExportDetailsName, bStoredProcedure:=ACAddExportDetailsStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

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

            ' Error.
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

            ' Error.
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

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    ''' <summary>
    ''' PostDocument
    ''' </summary>
    ''' <param name="v_lTransactionExportFolderCnt"></param>
    ''' <param name="r_vTransactions"></param>
    ''' <param name="sPaymentMethod"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PostDocument(ByVal v_lTransactionExportFolderCnt As Integer, Optional ByRef r_vTransactions(,) As Object = Nothing, Optional ByVal sPaymentMethod As String = "", Optional ByRef r_nDocumentID As Integer = 0, Optional ByRef r_sFailureReason As String = "") As Integer

        Dim oTransactionBusiness As bPMBTransactions.Automated
        Dim nDocumentId As Integer



        'Create an instance of PMBTransaction
        oTransactionBusiness = New bPMBTransactions.Automated()

        'Initialise the object
        m_lReturn = oTransactionBusiness.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, vDatabase:=m_oDatabase)

        'Set the process modes
        m_lReturn = oTransactionBusiness.SetProcessModes(m_iTask, m_lNavigate, m_lProcessMode, m_sTransactionType, m_dtEffectiveDate)

        'Call the function to Send the Transaction to Orion
        m_lReturn = oTransactionBusiness.SendToOrion(v_lTransactionExportFolderCnt, r_lDocumentId:=nDocumentId, r_sFailureReason:=r_sFailureReason)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
            Return gPMConstants.PMEReturnCode.PMError
        ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If
        r_nDocumentID = nDocumentId
        If String.IsNullOrEmpty(sPaymentMethod.Trim) OrElse sPaymentMethod.Trim.ToUpper = "INVOICE" OrElse sPaymentMethod.Trim.ToUpper = "AGENTCOLLECTION" Then
            Dim oEnableDebitOrder As Object = Nothing

            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:="",
                         v_sPassword:="", v_iUserID:=0,
                         v_iMainSourceID:=0, v_iLanguageID:=0,
                         v_iCurrencyID:=0, v_iLogLevel:=0,
                         v_sCallingAppName:="",
                         v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableDebitOrder,
                         v_vBranch:=1,
                         r_vUnderwriting:=oEnableDebitOrder)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_sMessage = "Failed to get product option."
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If ToSafeString(oEnableDebitOrder, "") = "1" Then
                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add("Document_Id", nDocumentId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.SQLAction(kAddTransDetailExSQL, kAddTransDetailExName, kAddTransDetailExStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        'Get an Array of TransDetailsId and Amount From lDocumentID
        Dim sSQL As String = ""
        m_oDatabase.Parameters.Clear()
        sSQL = "Select Account_id,transdetail_id , OutStanding_Amount, transdetail_type_id From TransDetail " &
               " Where Document_id=" & CStr(nDocumentId) & "  and Amount<>0"
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTransDetails", bStoredProcedure:=False, vResultArray:=r_vTransactions)

        ' Database error encountered
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMError
        End If

        Return m_lReturn

    End Function

    ''' <summary>
    ''' GetThisPremium
    ''' </summary>
    ''' <param name="r_cThisPremium"></param>
    ''' <param name="o_nRIRowsToPostCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetThisPremium(ByRef r_cThisPremium As Decimal, Optional ByRef o_nRIRowsToPostCnt As Integer = 0) As Integer
        Dim nResult As Integer = 0
        Dim sSQL As String = ""
        Dim oResultArray(,) As Object = Nothing

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_oDatabase.Parameters.Add("nInsuranceFileCnt", CStr(m_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetThisPremiumSQL, sSQLName:=kGetThisPremiumName, bStoredProcedure:=kGetThisPremiumStored, vResultArray:=oResultArray)

            ' Database error encountered
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            If Informations.IsArray(oResultArray) Then
                'Thinh Nguyen 23/01/2002 (start)
                'r_cThisPremium = vResultArray(0, 0)

                Dim auxVar As Object = oResultArray(0, 0)

                If Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar) Or CStr(oResultArray(0, 0)) = "" Then
                    r_cThisPremium = 0
                Else

                    r_cThisPremium = CDec(oResultArray(0, 0))
                    o_nRIRowsToPostCnt = CInt(oResultArray(1, 0))
                End If
                'Thinh Nguyen 23/01/2002 (end)
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetThisPremium Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetThisPremium", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPFTransactions
    '
    ' Description:  Returns the PF Transaction array for passing into
    '               the roadmap. Called by iPMUStats.
    '
    ' History: 05/02/2002 DD - Created.
    '
    ' ***************************************************************** '
    Public Function GetPFTransactions(ByRef v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set up the SQL and run it
            m_oDatabase.Parameters.Clear()
            m_oDatabase.Parameters.Add("InsuranceFileCnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(ACGetPFTransactionsSQL, ACGetPFTransactionsName, ACGetPFTransactionsStored, , r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPFTransactions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPFTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPreviousInsuranceFile
    '
    ' Description: Returns the previous live Insurance File based on the
    '              new one.
    '
    ' History: 07/02/2002 DD - Created.
    '
    ' ***************************************************************** '
    Public Function GetPreviousInsuranceFile(ByVal v_lNewInsuranceFileCnt As Object, ByRef r_lOldInsuranceFileCnt As Object) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Send the new file in
            m_oDatabase.Parameters.Clear()

            m_oDatabase.Parameters.Add("new_insurance_file_cnt", CStr(v_lNewInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Execute the SP
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPreviousFileSQL, sSQLName:=ACGetPreviousFileName, bStoredProcedure:=ACGetPreviousFileStored, vResultArray:=vResultArray)

            'Determine the result
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
            ElseIf Informations.IsArray(vResultArray) Then

                r_lOldInsuranceFileCnt = vResultArray(0, 0)
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPreviousInsuranceFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPreviousInsuranceFile", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *****************************************************************
    ' Name : GetPlanInsuranceFile
    '
    ' Desc : get insurance file count on latest version of instalment plan
    '
    ' Hist : Thinh Nguyen 01/03/2002 - created
    ' *****************************************************************
    Public Function GetPlanInsuranceFile(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lPlanInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArary(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPlanInsuranceFileSQL, sSQLName:=ACGetPlanInsuranceFileName, bStoredProcedure:=ACGetPlanInsuranceFileStored, vResultArray:=vResultArary)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArary) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            r_lPlanInsuranceFileCnt = CInt(vResultArary(0, 0))

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPlanInsuranceFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPlanInsuranceFile", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function ReverseStats(ByRef v_lInsuranceFileCnt As Integer, Optional ByRef v_bProcess As Boolean = False, Optional ByRef v_lPartyCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lStatsFolderCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()

                'SMJB CQ2155 28/08//03 Added output parameter
                m_lReturn = .Parameters.Add(sName:="new_stats_folder_cnt", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                ' KG 15/07/03
                m_lReturn = .Parameters.Add(sName:="Insurance_File_Cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                m_lReturn = .Parameters.Add(sName:="user_name", vValue:=m_sUsername, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                ' KG 15/07/03

                m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_Stats_Folder_Reverse", sSQLName:="spu_Stats_Folder_Reverse", bStoredProcedure:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to reverse Stats Folder", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseStats", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                lStatsFolderCnt = m_oDatabase.Parameters.Item("new_stats_folder_cnt").Value

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'SMJB CQ2155 28/08/03 Added functionality to process the folder without returning
            'to the calling procedure
            If v_bProcess Then
                'SMJB: We won't have populated our insurance file count yet if we've come this route
                m_lInsuranceFileCnt = v_lInsuranceFileCnt
                m_lReturn = ProcessFolder(lStatsFolderCnt, False)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process Stats Folder", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseStats", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                End If

                'SMJB 15/09/03: Call allocate function to do the allocation for us
                If v_lPartyCnt <> 0 Then
                    m_lReturn = Allocate(v_lPartyCnt:=v_lPartyCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to allocate stats reversal", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseStats", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    End If
                End If

            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReverseStats Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseStats", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetPFTransactions
    '
    ' Description:  Returns the PF Transaction array for passing into
    '               the roadmap. Called by iPMUStats.
    '
    ' History: 05/02/2002 DD - Created.
    '
    ' ***************************************************************** '
    Public Function UpdatePFCommissionTransactionID(ByRef v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set up the SQL and run it
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add("InsuranceFileCnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction("spu_PFUpdateCommissionTransID", "spu_PFUpdateCommissionTransID", True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPFTransactions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPFTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckPF
    '
    ' Description: Check if there is a PF record, and if so
    '              check for Tax and Fee accounts
    '
    ' History: 27/02/2003 PSL - Created.
    '
    ' ***************************************************************** '
    Public Function CheckPF(ByRef r_bIsPF As Boolean, ByRef r_lFeeAccount As Integer, ByRef r_lTaxAccount As Integer) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add the InsuranceFileCnt INPUT parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_Stats_CheckPFAccounts", sSQLName:="CheckPFAccounts", bStoredProcedure:=True, vResultArray:=vResultArray)

            ' Database error encountered
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            If Informations.IsArray(vResultArray) Then

                r_lFeeAccount = CInt(vResultArray(0, 0))

                r_lTaxAccount = CInt(vResultArray(1, 0))
                r_bIsPF = True
            Else
                r_bIsPF = False
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckPF Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckPF", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : AddCreditControlItem (Private)
    '
    ' Description   : Method to add an entry in the Credit Control Item Table
    '                   based for the supplied
    '                   a) insurance_file_cnt
    '                   b) business_type        (NB / MTA / REN)
    '
    ' Note          : Uses the 'spu_ACT_Add_Credit_Control_Item_InsFile' stored proc
    '
    ' Reference     : Issue No. 2687
    '
    ' Author        : Ram Chandrabose
    '
    ' Date          : 2003-03-05
    '
    ' Edit History  :
    ' RAM20030305   : Created
    ' ***************************************************************** '
    Private Function AddCreditControlItem(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sBusinessType As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            result = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = .Parameters.Add(sName:="business_type", vValue:=v_sBusinessType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = .SQLAction(sSQL:=ACAddCreditControlItemInsuranceFileSQL, sSQLName:=ACAddCreditControlItemInsuranceFileName, bStoredProcedure:=ACAddCreditControlItemInsuranceFileStored)

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name          : DeleteCreditControlItem (Private)
    '
    ' Description   : Method to Delete entries in the Credit Control Item Table
    '                   based for the supplied (i.e while cancelling policy)
    '                   a) insurance_file_cnt
    '
    ' Note          : Uses the 'spu_ACT_Del_Credit_Control_Item_InsFile' stored proc
    '
    ' Reference     : Issue No. 2687
    '
    ' Author        : Ram Chandrabose
    '
    ' Date          : 2003-03-07
    '
    ' Edit History  :
    ' RAM20030307   : Created
    ' ***************************************************************** '
    Private Function DeleteCreditControlItem(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            result = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = .SQLAction(sSQL:=ACDelCreditControlItemInsuranceFileSQL, sSQLName:=ACDelCreditControlItemInsuranceFileName, bStoredProcedure:=ACDelCreditControlItemInsuranceFileStored)

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ProcessInstalmentsDeposit (Private)
    '
    ' Description: we need to create a posting for the deposit, so we can
    '              do cashlist allocation later on.
    '
    ' PSL 07/03/2003
    ' ***************************************************************** '

    Function ProcessInstalmentsDeposit(ByRef r_lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            Dim lStatsFolderCnt, lTransactionExportFolderCnt As Integer

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the Stats tables
            m_lReturn = CreateDepositStats(lStatsFolderCnt:=lStatsFolderCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sMessage = "Failed to create Deposit Statistics"
                Return result
            End If

            'There was no PF so don't proceed (this is not an error)
            If lStatsFolderCnt = 0 Then Return result

            m_lReturn = CreateDepositExport(lStatsFolderCnt:=lStatsFolderCnt, lTransactionExportFolderCnt:=lTransactionExportFolderCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sMessage = "Failed to create Deposit Transactions"
                Return result
            End If

            'Call the routine to post the transaction to the Orion
            m_lReturn = PostDocument(v_lTransactionExportFolderCnt:=lTransactionExportFolderCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sMessage = "Failed to post deposit Transactions to Orion"
                Return result
            End If

            'Return the new folder
            r_lTransactionExportFolderCnt = lTransactionExportFolderCnt

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessInstalmentsDeposit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessInstalmentsDeposit", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CreateDepositStats (Private)
    '
    ' Description: Creates stats folder and details for deposit
    '
    ' PSL 07/03/2003
    ' ***************************************************************** '
    Private Function CreateDepositStats(ByRef lStatsFolderCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Create Folder of type journal
        m_lReturn = CreateDepositStatsFolder(lStatsFolderCnt:=lStatsFolderCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'There was no PF Record so don't bother
        If lStatsFolderCnt = 0 Then Return result

        'create details +/- of the deposit both into client account
        m_lReturn = CreateDepositStatsDetails(lStatsFolderCnt:=lStatsFolderCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return m_lReturn

    End Function

    ' ***************************************************************** '
    ' Name: CreateDepositStatsFolder (Private)
    '
    ' Description: creates a stats folder of type journal for deposit
    '
    ' PSL 07/03/2003
    ' ***************************************************************** '
    Private Function CreateDepositStatsFolder(ByRef lStatsFolderCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim lRecordsAffected As Integer


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = .Parameters.Add(sName:="user_name", vValue:=m_sUsername, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = .Parameters.Add(sName:="next_orion_doc_ref", vValue:=m_sNextOrionDocRefForInstalment, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        End With

        ' Execute Add Stats Folder deposit SQL Statement

        m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_add_stats_folder_Deposit", sSQLName:="add_stats_folder_Deposit", bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the Cnt of the record inserted
        If lRecordsAffected > 0 Then
            lStatsFolderCnt = m_oDatabase.Parameters.Item("stats_folder_cnt").Value
            If lStatsFolderCnt < 1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        Else
            lStatsFolderCnt = 0
        End If

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: CreateDepositStatsDetails (Private)
    '
    ' Description: create stats details rows, just for deposit
    '
    ' PSL 07/03/2003 OPSL
    ' ***************************************************************** '
    Private Function CreateDepositStatsDetails(ByRef lStatsFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        With m_oDatabase

            ' Add StatsFolderCnt as an INPUT param for an insert
            m_lReturn = .Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(lStatsFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        ' Execute Add Stats Details SQL Statement

        m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_Add_Stats_Details_Deposit", sSQLName:="Add_Stats_Details_Deposit", bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CreateDepositExport (Private)
    '
    ' Description: Create  transaction export info for deposit
    '
    ' PSL 07/03/2003
    ' ***************************************************************** '
    Private Function CreateDepositExport(ByRef lStatsFolderCnt As Integer, ByRef lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Same as normal export folder
        m_lReturn = CreateExportFolder(lStatsFolderCnt:=lStatsFolderCnt, lTransactionExportFolderCnt:=lTransactionExportFolderCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not (lStatsFolderCnt > 0) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'different export details for deposit
        m_lReturn = CreateDepositExportDetails(lStatsFolderCnt:=lStatsFolderCnt, lTransactionExportFolderCnt:=lTransactionExportFolderCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CreateDepositExportDetails (Private)
    '
    ' Description: create 2 journal details into and out of client account
    '
    ' PSL 07/03/2003
    ' ***************************************************************** '
    Private Function CreateDepositExportDetails(ByRef lStatsFolderCnt As Integer, ByRef lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        With m_oDatabase

            ' Add the Input Params
            m_lReturn = .Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(lStatsFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add ExportFolderCnt as an INPUT param for an insert
            m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(lTransactionExportFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        ' Execute Add Trans Export Details SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:="{ call spu_Add_trans_Details_Deposit (?,?)}", sSQLName:="Add_trans_Details_Deposit", bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    Private Function ProcessAutoAllocation(ByVal v_lTransactionExportFolderCnt As Integer) As gPMConstants.PMEReturnCode
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ProcessAutoAllocation
        ' PURPOSE: Process the Auto Allocation, when the Policy is Cancelled.
        ' AUTHOR: Ram Chandrabose
        ' DATE: 13/03/2003, 16:16
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' RAM20030313   : Created
        ' ---------------------------------------------------------------------------

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse


        ' RAM20030314 - Added the following 4 constants (from gACTLibrary.bas)
        Const ACTAllocationStatusUnallocated As Integer = 1
        'Const ACTAllocationStatusPosted As Integer = 2
        'Const ACTAllocationStatusAllocated As Integer = 3
        Const ACTAllocationStatusPartial As Integer = 4

        Dim vReturnArray(,) As Object = Nothing
        Dim oAutoAllocation As bACTAllocate.Business
        Dim lTransDetailId, lAccountID As Integer
        Dim sStatusCode As String = ""
        Dim sMessage As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            ' Clear the Database Parameters Collection
            .Parameters.Clear()

            ' Add ExportFolderCnt as an INPUT param
            m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(v_lTransactionExportFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Execute Add Trans Export Details SQL Statement
            m_lReturn = .SQLSelect(sSQL:=ACGetTransDetailIDSQL, sSQLName:=ACGetTransDetailIDName, bStoredProcedure:=ACGetTransDetailIDStored, vResultArray:=vReturnArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If Not Informations.IsArray(vReturnArray) Then
                ' We dont' have any values
                ' so report error
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to fetch TransDetail ID", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAutoAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' You will get multiple rows which contains transaction id, account id
            '         and also amount, document_ref, document_sequence (may need it in the future)

            lTransDetailId = gPMFunctions.NullToLong(vReturnArray(0, 0)) ' Only one id is enough to auto allocate
            lAccountID = gPMFunctions.NullToLong(vReturnArray(1, 0))

            If lTransDetailId > 0 And lAccountID > 0 Then

                'Create bACTAllocate.Business component to do the allocation

                oAutoAllocation = New bACTAllocate.Business
                m_lReturn = oAutoAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bACTAllocate.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAutoAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Call the Auto Allocate Method

                m_lReturn = oAutoAllocation.AutoAllocate(v_lTransDetailId:=lTransDetailId, r_sStatusCode:=sStatusCode, v_lTransAccountID:=lAccountID)
                ' Clear the objects

                oAutoAllocation.Dispose()
                oAutoAllocation = Nothing

                Select Case Val(sStatusCode)
                    Case ACTAllocationStatusUnallocated
                        sMessage = "Amount is not Allocated."
                    Case ACTAllocationStatusPartial
                        sMessage = "Amount is not fully Allocated. Only Partial amount is allocated."
                    Case Else
                        sMessage = ""
                End Select

                If sMessage.Trim().Length > 0 Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAutoAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            End If

        End With

        Return result

    End Function

    Public Function GetNextOrionDocRef() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oACTAutoNumber As bACTAutoNumber.Business

            oACTAutoNumber = New bACTAutoNumber.Business
            m_lReturn = oACTAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetNextOrionRef", "Initilizing the componenet bACTAutoNumber.Business failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            m_lReturn = oACTAutoNumber.GenerateDocumentReferenceNumber(v_sRangeCode:="SND", v_iUserID:=m_iUserID, v_iCompanyID:=m_iSourceID, r_sDocumentRef:=m_sNextOrionDocRef)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetNextOrionRef", "Method GenerateDocumentReferenceNumber failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            Return result
        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNextOrionDocRef Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextOrionDocRef", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetNextOrionDocRefForInstalment
    '
    ' Description:
    '
    ' History: 03/04/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function GetNextOrionDocRefForInstalment() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oACTAutoNumber As bACTAutoNumber.Business

            oACTAutoNumber = New bACTAutoNumber.Business
            m_lReturn = oACTAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetNextOrionDocRefForInstalment", "Initilizing the componenet bACTAutoNumber.Business failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            m_lReturn = oACTAutoNumber.GenerateDocumentReferenceNumber(v_sRangeCode:="JN", v_iUserID:=m_iUserID, v_iCompanyID:=m_iSourceID, r_sDocumentRef:=m_sNextOrionDocRefForInstalment)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetNextOrionDocRefForInstalment", "Method GenerateDocumentReferenceNumber Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNextOrionDocRefForInstalment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextOrionDocRefForInstalment", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function CheckIfInstalmentDepositRequired(ByRef r_bInstalmentDepositRequired As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object = Nothing

            r_bInstalmentDepositRequired = False

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=ACCheckIfInstalmentDepositRequiredSQL, sSQLName:=ACCheckIfInstalmentDepositRequiredName, bStoredProcedure:=ACCheckIfInstalmentDepositRequiredStored, vResultArray:=vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                r_bInstalmentDepositRequired = CDbl(vResultArray(0, 0)) = 1

            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckIfInstalmentDepositRequired Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIfInstalmentDepositRequired", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSystemOptionLite
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 15-02-2005 : Credit Control RetroFit
    ' ***************************************************************** '
    Public Function GetSystemOptionLite(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String, ByVal v_iSourceID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetSystemOptionLite"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=v_iOptionNumber, r_sOptionValue:=r_sOptionValue, v_iSourceID:=v_iSourceID), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption Failed", gPMConstants.PMELogLevel.PMLogError)
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

    Private Function MergeArrays(ByVal vTransactions(,) As Object, ByVal vCreditTransactions(,) As Object, ByRef vMergedArray(,) As Object) As Integer

        Dim result As Integer = 0
        'Const ksMyProcedureName As String = "MergeArrays"


        result = gPMConstants.PMEReturnCode.PMTrue

        'Dim iCol As Integer
        result = gPMConstants.PMEReturnCode.PMTrue

        vMergedArray = Nothing
        Dim iUpperBoundTransactions, iUpperBoundCreditTransactions As Integer

        iUpperBoundTransactions = vTransactions.GetUpperBound(1)
        iUpperBoundCreditTransactions = vCreditTransactions.GetUpperBound(0)
        ReDim Preserve vMergedArray(2, iUpperBoundTransactions + vCreditTransactions.GetUpperBound(1) + 1)
        Dim nUpper As Integer = 0
        For nCnt As Integer = 0 To 2

            Array.Copy(vTransactions, (iUpperBoundTransactions + 1) * nCnt, vMergedArray, (iUpperBoundTransactions + 1) * nCnt + nUpper, iUpperBoundTransactions + 1)
            nUpper = nUpper + vCreditTransactions.GetUpperBound(1) + 1
        Next

        For iRow As Integer = 0 To vCreditTransactions.GetUpperBound(1)

            vMergedArray(0, iUpperBoundTransactions + iRow + 1) = vCreditTransactions(0, iRow)

            vMergedArray(1, iUpperBoundTransactions + iRow + 1) = vCreditTransactions(1, iRow)

            vMergedArray(2, iUpperBoundTransactions + iRow + 1) = -1 * CDbl(vCreditTransactions(2, iRow))
        Next

        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    Public Function GetBaseCurrencyAmount(ByVal lCompanyId As Integer, ByVal lCurrencyID As Integer, ByRef cBaseAmount As Decimal, ByVal cCurrencyamount As Decimal) As Integer

        Dim result As Integer = 0
        Try
            Dim lReturnStatus As Integer
            Dim lReturn As gPMConstants.PMEReturnCode

            Const kMethodName As String = "GetBaseCurrencyAmount"

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("company_id", CStr(lCompanyId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If

            If m_oDatabase.Parameters.Add("currency_id", CStr(lCurrencyID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If

            If m_oDatabase.Parameters.Add("currency_amount_unrounded", CStr(cCurrencyamount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If

            If m_oDatabase.Parameters.Add("base_amount", CStr(cBaseAmount), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If
            If m_oDatabase.Parameters.Add("return_status", CStr(lReturnStatus), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACDoCurrencyConversionSQL, sSQLName:=ACDoCurrencyConversionName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, ACDoCurrencyConversionSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            cBaseAmount = gPMFunctions.ToSafeCurrency(m_oDatabase.Parameters.Item("base_amount").Value)


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetBaseCurrencyAmount", r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function
    Public Function GetGetCurrencyIDFromTransDetail(ByVal lTransID As Integer, ByRef iCurrencyID As Integer) As Integer

        Dim result As Integer = 0
        Try
            'Dim lReturnStatus As Integer
            Dim lReturn As gPMConstants.PMEReturnCode

            Const kMethodName As String = "GetGetCurrencyIDFromTransDetail"
            Dim r_vResults(,) As Object = Nothing

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("transdetail_id", CStr(lTransID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCurrencyIDFromTransDetailSQL, sSQLName:=ACGetCurrencyIDFromTransDetailName, bStoredProcedure:=False, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, CStr(result) & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(r_vResults) Then

                iCurrencyID = CInt(r_vResults(0, 0))
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetBaseCurrencyAmount", r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    Public Function GetInsuranceRef(ByVal v_lInsuranceFileCnt As Object, ByRef r_vResults(,) As Object) As Integer
        Dim result As Integer = 0
        Try
            Dim lReturn As gPMConstants.PMEReturnCode

            Const kMethodName As String = "GetInsuranceRef"

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("Insurance_File_Cnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If

            lReturn = m_oDatabase.SQLSelect(sSQL:="spe_Insurance_File_sel", sSQLName:="spe_Insurance_File_sel", bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Get Insurance Ref Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetInsuranceRef", r_lFunctionReturn:=result, excep:=ex)

        Finally
        End Try
        Return result

    End Function

    Private Function GetRoundOffAccount(ByVal v_sAccountShortCode As String, ByVal v_lAccount_Key As Integer, ByVal v_lSubBranchID As Integer, ByVal v_lCurrencyID As Integer, ByRef r_lAccountId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetRoundOffAccount"


        result = gPMConstants.PMEReturnCode.PMTrue

        If Not String.IsNullOrEmpty(v_sAccountShortCode.Trim()) Then
            m_lReturn = GetAccountIdWithShortCode(v_sAccountShortCode, r_lAccountId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                gPMFunctions.RaiseError(kMethodName, "Method(GetAccountIdWithShortCode) calling failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        If String.IsNullOrEmpty(v_sAccountShortCode.Trim()) Or r_lAccountId <= 0 Then
            m_lReturn = CreateSuspenseAccount(r_lAccountId:=r_lAccountId, v_sLedgerFlag:=gSIRLibrary.SIRACTNominalLedgerShortName, v_lSubBranchID:=v_lSubBranchID, v_sShortCode:=v_sAccountShortCode, v_lCurrencyID:=v_lCurrencyID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Method(CreateSuspenseAccount) calling failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        End If


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    'Note:-
    Private Function GetAccountIdWithShortCode(ByVal v_sAccountShortCode As String, ByRef v_lAccountID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Const kMethodName As String = "GetAccountIdWithShortCode"

        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="Short_code", vValue:=v_sAccountShortCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (Short_code) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = .SQLSelect(sSQL:=ACGetAccountIdFromShortcodeSQL, sSQLName:=ACGetAccountIdFromShortcodeName, bStoredProcedure:=ACGetAccountIdFromShortcodeStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SP " & ACGetAccountIdFromShortcodeSQL & " calling failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        End With

        If Not Object.Equals(vResultArray, Nothing) And Informations.IsArray(vResultArray) Then

            v_lAccountID = gPMFunctions.ToSafeLong(CDbl(vResultArray(0, 0)))
        Else
            v_lAccountID = 0
        End If


        Return result

    End Function

    Public Function GetInsuranceFileInformation(ByVal v_lInsuranceFileCnt As Integer, Optional ByRef r_sInsuranceRef As String = "", Optional ByRef r_lCompanyID As Integer = 0, Optional ByRef r_lAccountId As Integer = 0, Optional ByRef r_iCurrencyID As Integer = 0, Optional ByRef r_cPremium As Decimal = 0, Optional ByRef r_dCurrencyBaseXrate As Double = 0, Optional ByRef r_dtCurrencyBaseDate As Date = #12/30/1899#, Optional ByRef r_dAccountBaseXrate As Double = 0, Optional ByRef r_dtAccountBaseDate As Date = #12/30/1899#, Optional ByRef r_dSystemBaseXrate As Double = 0, Optional ByRef r_dtSystemBaseDate As Date = #12/30/1899#, Optional ByRef r_lRateOverrideReasonID As Integer = 0, Optional ByRef r_lSubBranchId As Integer = 0, Optional ByRef r_lAccount_Key As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetInsuranceFileInformation"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Add parameter details.
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            bPMAddParameter.AddParameterLite(m_oDatabase, "source_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "agent_account_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)

            bPMAddParameter.AddParameterLite(m_oDatabase, "premium", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)

            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_base_xrate", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "account_base_xrate", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "account_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "system_base_xrate", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "system_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "exchange_rate_override_reason_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "InsuranceRef", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMString)

            bPMAddParameter.AddParameterLite(m_oDatabase, "SubBranchId", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "Account_Key", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            'Call the stored procedure.
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetInsuranceFileInformationSQL, sSQLName:=ACGetInsuranceFileInformationName, bStoredProcedure:=ACGetInsuranceFileInformationStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", SQLAction, SQL Call failed.")
            End If

            'Get the return values.

            If Convert.IsDBNull(m_oDatabase.Parameters.Item("source_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("source_id").Value) Then
                r_lCompanyID = 0
            Else
                r_lCompanyID = m_oDatabase.Parameters.Item("source_id").Value
            End If

            If Convert.IsDBNull(m_oDatabase.Parameters.Item("agent_account_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("agent_account_id").Value) Then
                r_lAccountId = 0
            Else
                r_lAccountId = m_oDatabase.Parameters.Item("agent_account_id").Value
            End If

            If Convert.IsDBNull(m_oDatabase.Parameters.Item("currency_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("currency_id").Value) Then
                r_iCurrencyID = 0
            Else
                r_iCurrencyID = m_oDatabase.Parameters.Item("currency_id").Value
            End If

            If Convert.IsDBNull(m_oDatabase.Parameters.Item("premium").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("premium").Value) Then
                r_cPremium = 0
            Else
                r_cPremium = m_oDatabase.Parameters.Item("premium").Value
            End If

            If Convert.IsDBNull(m_oDatabase.Parameters.Item("currency_base_xrate").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("currency_base_xrate").Value) Then
                r_dCurrencyBaseXrate = 0
            Else
                r_dCurrencyBaseXrate = m_oDatabase.Parameters.Item("currency_base_xrate").Value
            End If

            If Convert.IsDBNull(m_oDatabase.Parameters.Item("currency_base_date").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("currency_base_date").Value) Then
                r_dtCurrencyBaseDate = DateTime.Now 'PN-71597
            Else
                r_dtCurrencyBaseDate = m_oDatabase.Parameters.Item("currency_base_date").Value
            End If

            If Convert.IsDBNull(m_oDatabase.Parameters.Item("account_base_xrate").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("account_base_xrate").Value) Then
                r_dAccountBaseXrate = 0
            Else
                r_dAccountBaseXrate = m_oDatabase.Parameters.Item("account_base_xrate").Value
            End If

            If Convert.IsDBNull(m_oDatabase.Parameters.Item("account_base_date").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("account_base_date").Value) Then
                r_dtAccountBaseDate = DateTime.Now 'PN-71597
            Else
                r_dtAccountBaseDate = m_oDatabase.Parameters.Item("account_base_date").Value
            End If

            If Convert.IsDBNull(m_oDatabase.Parameters.Item("system_base_xrate").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("system_base_xrate").Value) Then
                r_dSystemBaseXrate = 0
            Else
                r_dSystemBaseXrate = m_oDatabase.Parameters.Item("system_base_xrate").Value
            End If

            If Convert.IsDBNull(m_oDatabase.Parameters.Item("system_base_date").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("system_base_date").Value) Then
                r_dtSystemBaseDate = DateTime.Now 'PN-71597
            Else
                r_dtSystemBaseDate = m_oDatabase.Parameters.Item("system_base_date").Value
            End If

            If Convert.IsDBNull(m_oDatabase.Parameters.Item("exchange_rate_override_reason_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("exchange_rate_override_reason_id").Value) Then
                r_lRateOverrideReasonID = 0
            Else
                r_lRateOverrideReasonID = m_oDatabase.Parameters.Item("exchange_rate_override_reason_id").Value
            End If

            r_sInsuranceRef = gPMFunctions.ToSafeString(m_oDatabase.Parameters.Item("InsuranceRef").Value)
            r_lSubBranchId = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("SubBranchId").Value)
            r_lAccount_Key = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("Account_key").Value)


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

    Public Function CreateSuspenseAccount(ByRef r_lAccountId As Integer, ByVal v_sLedgerFlag As String, ByVal v_lSubBranchID As Integer, ByVal v_sShortCode As String, ByVal v_lCurrencyID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateSuspenseAccount"

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oOrionExplorer As bACTExplorer.Form
            Dim oOrionAccount As bACTAccount.Form
            Dim oOrionLedger As bACTLedger.Form
            Dim oLedger As bSirOrionLink.Form
            Dim lLedgerId, lAccountID As Integer
            Dim iAccountTypeID As Integer
            Dim lSalesLedgerID, lPurchaseLedgerID, lInsurerLedgerId, lAgentLedgerID, lFeeLedgerId, lCommissionLedgerID, lDiscountLedgerID, lPRemiumFinanceLedgerId, lSubAgentLedgerId, lNominalLedgerId, lOtherPartyPayLedgerId, lOtherPartyRecLedgerId, lIntroducerLedgerId, lNodeId, lElementId As Integer


            oOrionAccount = New bACTAccount.Form
            m_lReturn = oOrionAccount.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                gPMFunctions.RaiseError(kMethodName, "Failed to Initialise bACTAccount.Form", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            ' Create an instance of the Orion Ledger object

            oOrionLedger = New bACTLedger.Form
            m_lReturn = oOrionLedger.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                gPMFunctions.RaiseError(kMethodName, "Failed to Initialise bACTLedger.Form", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            ' Create an instance of the Orion Ledger object

            oLedger = New bSirOrionLink.Form
            m_lReturn = oLedger.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                gPMFunctions.RaiseError(kMethodName, "Failed to Initialise bACTLedger.Form", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            ' Create an instance of the Orion Account Explorer object

            oOrionExplorer = New bACTExplorer.Form
            m_lReturn = oOrionExplorer.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                gPMFunctions.RaiseError(kMethodName, "Failed to Initialise bACTExplorer.Form", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            m_lReturn = oLedger.GetLedgerIDs(r_lSalesLedgerID:=lSalesLedgerID, r_lPurchaseLedgerID:=lPurchaseLedgerID, r_lInsurerLedgerId:=lInsurerLedgerId, r_lAgentLedgerId:=lAgentLedgerID, r_lFeeLedgerId:=lFeeLedgerId, r_lCommissionLedgerId:=lCommissionLedgerID, r_lDiscountLedgerId:=lDiscountLedgerID, r_lPremiumFinanceLedgerId:=lPRemiumFinanceLedgerId, r_lSubAgentLedgerId:=lSubAgentLedgerId, r_lNominalLedgerID:=lNominalLedgerId, r_lOtherPartyPayLedgerID:=lOtherPartyPayLedgerId, r_lOtherPartyRecLedgerID:=lOtherPartyRecLedgerId, r_lIntroducerLedgerId:=lIntroducerLedgerId, v_lSubBranchID:=v_lSubBranchID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLedgerIDs Failed", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Select Case v_sLedgerFlag
                Case gSIRLibrary.SIRACTSalesLedgerShortName
                    iAccountTypeID = gACTLibrary.ACTAccountTypeAsset
                    lLedgerId = lSalesLedgerID
                Case gSIRLibrary.SIRACTPurchaseLedgerShortName
                    iAccountTypeID = gACTLibrary.ACTAccountTypeLiability
                    lLedgerId = lPurchaseLedgerID
                Case "I"
                    iAccountTypeID = gACTLibrary.ACTAccountTypeLiability
                    lLedgerId = lInsurerLedgerId

                Case "A"
                    iAccountTypeID = gACTLibrary.ACTAccountTypeLiability
                    lLedgerId = lAgentLedgerID

                Case "F"
                    iAccountTypeID = gACTLibrary.ACTAccountTypeLiability
                    lLedgerId = lFeeLedgerId

                Case "C"
                    iAccountTypeID = gACTLibrary.ACTAccountTypeLiability
                    lLedgerId = lCommissionLedgerID

                Case "D"
                    iAccountTypeID = gACTLibrary.ACTAccountTypeLiability
                    lLedgerId = lDiscountLedgerID

                Case "R"
                    iAccountTypeID = gACTLibrary.ACTAccountTypeLiability
                    lLedgerId = lPRemiumFinanceLedgerId
                Case "U"
                    iAccountTypeID = gACTLibrary.ACTAccountTypeAsset
                    lLedgerId = lSubAgentLedgerId

                Case "N"
                    iAccountTypeID = gACTLibrary.ACTAccountTypeLiability
                    lLedgerId = lNominalLedgerId

                Case gSIRLibrary.SIRACTOtherPartyPayLedgerShortName
                    iAccountTypeID = gACTLibrary.ACTAccountTypeLiability
                    lLedgerId = lOtherPartyPayLedgerId

                Case gSIRLibrary.SIRACTOtherPartyRecLedgerShortName
                    iAccountTypeID = gACTLibrary.ACTAccountTypeAsset
                    lLedgerId = lOtherPartyRecLedgerId

                Case "T"
                    iAccountTypeID = gACTLibrary.ACTAccountTypeLiability
                    lLedgerId = lIntroducerLedgerId

            End Select

            m_lReturn = oOrionAccount.DirectAdd(vAccountID:=lAccountID, vAccounttypeID:=iAccountTypeID, vPurgefrequencyID:=0, vCurrencyID:=v_lCurrencyID, vLedgerId:=lLedgerId, vAccountName:="", vShortCode:=v_sShortCode, vAccountStatusID:=gACTLibrary.ACTAccountStatusActive)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, "oOrionAccount.DirectAdd Failed", gPMConstants.PMELogLevel.PMLogError)

                oOrionAccount.Dispose()
                oOrionAccount = Nothing
                Return result
            End If

            r_lAccountId = lAccountID

            oOrionAccount.Dispose()
            oOrionAccount = Nothing

            m_lReturn = oOrionLedger.GetLedgerNodeId("Expense", lNodeId)

            If lNodeId = 0 Then

                m_lReturn = oOrionLedger.GetLedgerNodeId("Expenditure", lNodeId)
            End If

            oOrionLedger.Dispose()

            oOrionLedger = Nothing

            lElementId = oOrionExplorer.InsertElement(v_sShortCode)

            If lElementId > 0 Then

                lNodeId = oOrionExplorer.InsertNode(lParentNodeId:=lNodeId, lElementId:=lElementId, vAccountID:=lAccountID)
            Else
                gPMFunctions.RaiseError(kMethodName, "oOrionExplorer.InsertElement Failed ", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            oOrionExplorer.Dispose()
            oOrionExplorer = Nothing


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="CreateSuspenseAccount", r_lFunctionReturn:=result, excep:=ex)

            Return result

        Finally

        End Try
        Return result
    End Function

    Private Function InsertInsuranceFilePaymentDetails(ByVal v_lInsuranceFileCnt As Integer, ByRef v_lCashListItemId As Integer, ByRef v_lDocumentId As Integer, ByRef v_lTransdetailId As Integer, ByRef v_cAmount As Decimal) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "InsertInsuranceFilePaymentDetails"
        Dim vResultArray As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue
        m_oDatabase.Parameters.Clear()
        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="Insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (Insurance_file_cnt) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If v_lCashListItemId = 0 Or False Then

                m_lReturn = .Parameters.Add(sName:="CashListItem_Id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="CashListItem_Id", vValue:=CStr(v_lCashListItemId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (CashListItem_Id) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = .Parameters.Add(sName:="Document_id", vValue:=CStr(v_lDocumentId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (Document_id) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = .Parameters.Add(sName:="Transdetail_id", vValue:=CStr(v_lTransdetailId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (Transdetail_id) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = .Parameters.Add(sName:="Amount", vValue:=CStr(v_cAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (Amount) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertInsuranceFilePaymentDetailsSQL, sSQLName:=ACInsertInsuranceFilePaymentDetailsName, bStoredProcedure:=ACInsertInsuranceFilePaymentDetailsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SP " & ACInsertInsuranceFilePaymentDetailsSQL & " calling failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

        End With


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    Public Function UpdateCashDepositPolicyLink(ByVal v_lCashDepositAccountId As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateCashDepositPolicyLink"
        Dim lCashDepositID As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="CashDepsoit_ID", vValue:=CStr(lCashDepositID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="CashDeposit_Account_ID", vValue:=CStr(v_lCashDepositAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateCashDepositPolicyLinkSQL, sSQLName:=ACUpdateCashDepositPolicyLinkName, bStoredProcedure:=ACUpdateCashDepositPolicyLinkStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("CashDepsoit_ID").Value, 0) = 0 Then
                gPMFunctions.RaiseError(kMethodName, ACUpdateCashDepositPolicyLinkSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
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

    '************************************************************************************
    'unlock specified key
    '************************************************************************************
    Public Function UnLockKey(ByVal v_sKeyName As String, ByVal v_lKeyValue As Integer, ByVal v_lUserID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UnLockKey"
        Try
            Dim oLock As bpmlock.User

            oLock = New bpmlock.User
            If oLock.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                gPMFunctions.RaiseError(kMethodName, "Create Object for bPMLock.User Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            result = oLock.UnLockKey(sKeyName:=v_sKeyName, vKeyValue:=v_lKeyValue, iUserID:=v_lUserID)


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock KeyName: " & v_sKeyName & " KeyValue: " & CStr(v_lKeyValue), vApp:=ACApp, vClass:=ACClass, vMethod:="UnLockKey", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Private Function AllocateDocuments(ByVal lOriginalDocId As Integer, ByVal lReversedDocId As Integer) As Integer

        Dim result As Integer = 0
        'Dim lReturn As Integer

        Const kMethodName As String = "AllocateDocuments"

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vOriginalResultArray(,) As Object = Nothing
        Dim vReversedResultArray(,) As Object = Nothing
        Dim oAllocationManual As New bACTAllocationManual.Business
        'Dim lAccountID As Integer
        Dim vKeyArray(1, 3) As Object
        Dim vTrans(0) As Object
        Try

            oAllocationManual = New bACTAllocationManual.Business


            If oAllocationManual.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add(sName:="document_id", vValue:=CStr(lOriginalDocId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = .SQLSelect(sSQL:=ACGetTransDetailByDocSQL, sSQLName:=ACGetTransDetailByDocName, bStoredProcedure:=ACGetTransDetailByDocStored, vResultArray:=vOriginalResultArray)
            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add(sName:="document_id", vValue:=CStr(lReversedDocId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = .SQLSelect(sSQL:=ACGetTransDetailByDocSQL, sSQLName:=ACGetTransDetailByDocName, bStoredProcedure:=ACGetTransDetailByDocStored, vResultArray:=vReversedResultArray)
            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Informations.IsArray(vOriginalResultArray) Or Not Informations.IsArray(vReversedResultArray) Then
                ' transactions not available to allocate
                Return result
            End If

            If vOriginalResultArray.GetUpperBound(1) <> vReversedResultArray.GetUpperBound(1) Then
                ' just to ensure number account lines are same else leave alone
                Return result
            End If

            For lCount As Integer = 0 To vReversedResultArray.GetUpperBound(1)

                vTrans(0) = CStr(vReversedResultArray(0, lCount)) & "|" & CStr(vReversedResultArray(2, lCount))

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = vOriginalResultArray(1, lCount)

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = CStr(vOriginalResultArray(0, lCount)) & "|" & CStr(vOriginalResultArray(2, lCount))

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vTrans

                m_lReturn = oAllocationManual.SetKeys(vKeyArray:=vKeyArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = oAllocationManual.Start()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            Next


        Finally
            oAllocationManual.Dispose()
        End Try
        Return result
    End Function

    Private Function GetInsuranceFileInfo(ByVal v_lInsuranceFileCnt As Integer, ByRef r_sInsuranceRef As String, ByRef r_lCompanyId As Integer, ByRef r_lPartyAccountId As Integer, ByRef r_lCurrencyId As Integer, ByRef r_dcurrencyBaseRate As Double, ByRef r_dtAccountingDate As Date, ByRef r_lSubBranchId As Integer, ByRef r_lAccount_Key As Integer, Optional ByVal v_lTransactionExportFolderCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetInsuranceFileInfo"

        Dim vResultArray(,) As Object = Nothing


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (insurance_file_cnt) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = .Parameters.Add(sName:="source_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (source_id) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            m_lReturn = .Parameters.Add(sName:="agent_account_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (agent_account_id) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            m_lReturn = .Parameters.Add(sName:="currency_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (currency_id) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            m_lReturn = .Parameters.Add(sName:="premium", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (premium) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            m_lReturn = .Parameters.Add(sName:="currency_base_xrate", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (currency_base_xrate) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            m_lReturn = .Parameters.Add(sName:="currency_base_date", vValue:="", iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (currency_base_date) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            m_lReturn = .Parameters.Add(sName:="account_base_xrate", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (account_base_xrate) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            m_lReturn = .Parameters.Add(sName:="account_base_date", vValue:="", iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (account_base_date) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            m_lReturn = .Parameters.Add(sName:="system_base_xrate", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (system_base_xrate) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            m_lReturn = .Parameters.Add(sName:="system_base_date", vValue:="", iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (system_base_date) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            m_lReturn = .Parameters.Add(sName:="exchange_rate_override_reason_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (exchange_rate_override_reason_id) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            m_lReturn = .Parameters.Add(sName:="InsuranceRef", vValue:="", iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (InsuranceRef) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            m_lReturn = .Parameters.Add(sName:="SubBranchId", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (SubBranchId) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = .Parameters.Add(sName:="Account_Key", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (Account_Key) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = .Parameters.Add(sName:="cover_start_date", vValue:="", iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (cover_start_date) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = .Parameters.Add(sName:="accounting_date", vValue:="", iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (accounting_date) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(v_lTransactionExportFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (transaction_export_folder_cnt ) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = .SQLSelect(sSQL:=ACGetInsuranceFileInformationSQL, sSQLName:=ACGetInsuranceFileInformationName, bStoredProcedure:=ACGetInsuranceFileInformationStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SP " & ACGetAccountIdFromShortcodeSQL & " calling failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            Else
                r_lCompanyId = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("source_id").Value)
                r_lPartyAccountId = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("agent_account_id").Value)
                r_lCurrencyId = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("currency_id").Value)
                r_dcurrencyBaseRate = gPMFunctions.ToSafeDouble(m_oDatabase.Parameters.Item("currency_base_xrate").Value)
                If v_lTransactionExportFolderCnt = 0 Then
                    r_dtAccountingDate = gPMFunctions.ToSafeDate(m_oDatabase.Parameters.Item("cover_start_date").Value)
                Else
                    r_dtAccountingDate = gPMFunctions.ToSafeDate(m_oDatabase.Parameters.Item("accounting_date").Value)
                End If
                r_sInsuranceRef = gPMFunctions.ToSafeString(m_oDatabase.Parameters.Item("InsuranceRef").Value)
                r_lSubBranchId = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("SubBranchId").Value)
                r_lAccount_Key = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("Account_key").Value)
            End If
        End With


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    Private Function AddChaseCycleItem(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sBusinessType As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            result = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = .Parameters.Add(sName:="business_type", vValue:=v_sBusinessType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = .SQLAction(sSQL:=ACAddChaseCycleItemInsuranceFileSQL, sSQLName:=ACAddChaseCycleItemInsuranceFileName, bStoredProcedure:=ACAddChaseCycleItemInsuranceFileStored)

        End With

        Return result

    End Function
    Private Function DeleteChaseCycleItem(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            result = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = .SQLAction(sSQL:=ACDelChaseCycleItemInsuranceFileSQL, sSQLName:=ACDelChaseCycleItemInsuranceFileName, bStoredProcedure:=ACDelChaseCycleItemInsuranceFileStored)

        End With

        Return result

    End Function

    Private Function CreateClonedStatsDetails(
    lStatsFolderCnt As Long, lClonedInsuranceFileCnt As Long) As Integer

        Dim lRecordsAffected As Long



        m_lReturn = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        With m_oDatabase


            ' Add StatsFolderCnt as an INPUT param for an insert
            m_lReturn = .Parameters.Add(
                  sName:="ClonedInsuranceFileCnt",
                  vValue:=lClonedInsuranceFileCnt,
                  iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                  iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_lReturn = .Parameters.Add(
                 sName:="StatsFolderCnt",
                 vValue:=lStatsFolderCnt,
                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                 iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End With

        ' Execute Add Stats Details SQL Statement
        'SR 03/11/2K
        ' This SP takes more than 30 seconds.
        ' Since the timeout in PMDAO is 15 secs only, it terminates with timeout error. need checking.
        m_lReturn = m_oDatabase.SQLAction(
            sSQL:=ACAddClonedStatsDetailsSQL,
            sSQLName:=ACAddClonedStatsDetailsName,
            bStoredProcedure:=ACAddClonedStatsDetailsStored,
            lRecordsAffected:=lRecordsAffected)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If
        Return m_lReturn
    End Function




    Private Function CreatePTStatsDetails(
        lStatsFolderCnt As Long, lPTInsuranceFileCnt As Long) As Long

        Dim lRecordsAffected As Long


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        With m_oDatabase


            ' Add StatsFolderCnt as an INPUT param for an insert
            m_lReturn = .Parameters.Add(
                  sName:="PTInsuranceFileCnt",
                  vValue:=lPTInsuranceFileCnt,
                  iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                  iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_lReturn = .Parameters.Add(
                 sName:="StatsFolderCnt",
                 vValue:=lStatsFolderCnt,
                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                 iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End With

        ' Execute Add Stats Details SQL Statement
        'SR 03/11/2K
        ' This SP takes more than 30 seconds.
        ' Since the timeout in PMDAO is 15 secs only, it terminates with timeout error. need checking.
        m_lReturn = m_oDatabase.SQLAction(
            sSQL:=ACAddPTStatsDetailsSQL,
            sSQLName:=ACAddPTStatsDetailsName,
            bStoredProcedure:=ACAddPTStatsDetailsStored,
            lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        Return m_lReturn
    End Function

    ''''<summary>
    '''' ProcessClonedTransactions
    '''' </summary>
    '''' <param name="nStatsFolderCnt"></param>
    '''' <param name="oTransactions"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 

    Private Function GetPolicyIntermediaryAgentAccount(ByVal lInsuranceFolderCnt As Integer, ByRef lPaymentAccountId As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            Dim vResultArray(,) As Object = Nothing
            m_oDatabase.Parameters.Clear()
            With m_oDatabase
                m_lReturn = .Parameters.Add(
                    sName:="InsuranceFolderCnt",
                    vValue:=lInsuranceFolderCnt,
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                    iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
                m_lReturn = .Parameters.Add(
                   sName:="InsuranceFileCnt",
                   vValue:=m_lInsuranceFileCnt,
                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                   iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
                m_lReturn = .SQLSelect(sSQL:=ACGetPolicyIntermediaryAgentAccountSQL, sSQLName:=ACGetPolicyIntermediaryAgentAccountName, bStoredProcedure:=ACGetPolicyIntermediaryAgentAccountStored, vResultArray:=vResultArray)
            End With
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetPolicyIntermediaryAgentAccount", "Method GetPolicyIntermediaryAgentAccount failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            ElseIf vResultArray IsNot Nothing AndAlso vResultArray.Length > 0 Then
                lPaymentAccountId = gPMFunctions.ToSafeInteger(vResultArray(0, 0))
            End If
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyIntermediaryAgentAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyIntermediaryAgentAccount", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function
    Private Function ProcessClonedTransactions(nStatsFolderCnt As Integer,
                                               ByVal oTransactions(,) As Object) As Integer

        Dim oAllocationManual As Object = Nothing
        Dim iRow As Integer
        Dim oCreditTransactions(,) As Object = Nothing
        Dim oKeyArray(1, 3) As Object
        Dim iCount As Integer
        Dim oTrans(0) As Object
        Dim nResult As Integer
        Try

            nResult = PMEReturnCode.PMTrue

            'Get an instance of bACTAllocationManual component to do the allocation
            If CreateBusinessObject(
                r_oObject:=oAllocationManual,
                v_sClassName:="bACTAllocationManual.Business",
                v_sCallingAppName:=ACApp,
                v_sUsername:=m_sUsername$,
                v_sPassword:=m_sPassword$,
                v_iUserID:=m_iUserID%,
                v_iSourceID:=m_iSourceID%,
                v_iLanguageID:=m_iLanguageID%,
                v_iCurrencyID:=m_iCurrencyID%,
                v_iLogLevel:=m_iLogLevel,
                v_oDatabase:=m_oDatabase) <> PMEReturnCode.PMTrue Then

                Return PMEReturnCode.PMFalse

            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nStatsFolderCnt", vValue:=nStatsFolderCnt,
                                                      iDirection:=PMEParameterDirection.PMParamInput,
                                                      iDataType:=PMEDataType.PMInteger)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="nInsuranceFileCnt", vValue:=m_lInsuranceFileCnt,
                                                   iDirection:=PMEParameterDirection.PMParamInput,
                                                   iDataType:=PMEDataType.PMInteger)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_clm_get_transdetails_RI", sSQLName:="GetTransDetails",
                                              bStoredProcedure:=True, vResultArray:=oCreditTransactions)

            If (nStatsFolderCnt < 1) Then
                Return PMEReturnCode.PMFalse
            End If

            For iCount = 0 To oTransactions.GetUpperBound(1)

                For iRow = 0 To oCreditTransactions.GetUpperBound(1)

                    If oTransactions(0, iCount) = oCreditTransactions(0, iRow) Then
                        If Math.Abs(ToSafeDecimal(oTransactions(2, iCount))) = Math.Abs(ToSafeDecimal(oCreditTransactions(2, iRow))) AndAlso
                           oTransactions(3, iCount) = oCreditTransactions(3, iRow) Then

                            oTrans(0) = oTransactions(1, iCount) & "|" & oTransactions(2, iCount)

                            oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 0) = ACTKeyNameAccountID
                            oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 0) = oCreditTransactions(0,
                                                                                                                   iRow)

                            oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 1) = ACTKeyNameTransDetailID
                            oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 1) =
                                oCreditTransactions(1, iRow) & "|" & oCreditTransactions(2, iRow)

                            oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 2) = ACTKeyNameTransDetailIDs
                            oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 2) = oTrans

                            m_lReturn = oAllocationManual.SetKeys(vKeyArray:=CType(oKeyArray, Object))

                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                Return PMEReturnCode.PMFalse
                            End If

                            m_lReturn = oAllocationManual.Start()

                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                Return PMEReturnCode.PMFalse
                            End If
                        End If
                    End If
                Next
            Next

        Catch ex As Exception
            nResult = PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="IsPortfolioTransferVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessClonedTransaction", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)
        Finally
            oAllocationManual.Dispose()
            oAllocationManual = Nothing
        End Try
        Return nResult
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="r_bIsPT"></param>
    ''' <param name="r_dtTransferDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsPortfolioTransferVersion(ByVal nInsuranceFileCnt As Integer, ByRef r_bIsPT As Boolean,
                                               ByRef r_dtTransferDate As Date) As Integer
        Dim dtResultArray As New DataTable
        Dim result As gPMConstants.PMEReturnCode
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add the InsuranceFileCnt INPUT parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="nInsurance_file_cnt", vValue:=nInsuranceFileCnt,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:="spu_get_policy_portfolio_log",
                                                     sSQLName:="IsPortfolioTransferVersion",
                                                     bStoredProcedure:=True,
                                                     oRecordset:=dtResultArray)

            ' Database error encountered
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If dtResultArray IsNot Nothing AndAlso dtResultArray.Rows.Count > 0 Then
                r_dtTransferDate = dtResultArray.Rows(0).Item(0)
                r_bIsPT = True
            Else
                r_bIsPT = False
            End If

        Catch ex As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsPortfolioTransferVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsPortfolioTransferVersion", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)
        End Try
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nStatsFolderCnt"></param>
    ''' <param name="r_nTransactionExportFolderCnt"></param>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateClonedExport(ByRef nStatsFolderCnt As Integer, ByRef r_nTransactionExportFolderCnt As Integer,
                                       ByVal nInsuranceFileCnt As Integer) As Integer
        Dim nResult As Integer
        Dim nTransactionExportFolderCnt As Integer

        nResult = gPMConstants.PMEReturnCode.PMTrue
        m_lReturn = CreateExportFolder(lStatsFolderCnt:=nStatsFolderCnt,
                                       lTransactionExportFolderCnt:=nTransactionExportFolderCnt)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If (nTransactionExportFolderCnt > 0) = False Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = CreateClonedExportDetails(nInsuranceFileCnt:=nInsuranceFileCnt,
                                              nTransactionExportFolderCnt:=nTransactionExportFolderCnt,
                                              nStatsFolderCnt:=nStatsFolderCnt)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        r_nTransactionExportFolderCnt = nTransactionExportFolderCnt
        Return nResult
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="nTransactionExportFolderCnt"></param>
    ''' <param name="nStatsFolderCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateClonedExportDetails(ByVal nInsuranceFileCnt As Integer, ByVal nTransactionExportFolderCnt As Integer,
                                               ByVal nStatsFolderCnt As Integer) _
        As Integer

        Dim nRecordsAffected As Integer
        Dim nResult As Integer

        nResult = gPMConstants.PMEReturnCode.PMTrue
        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        With m_oDatabase

            ' Add the Input Params
            m_lReturn = .Parameters.Add(sName:="ClonedInsuranceFileCnt", vValue:=nInsuranceFileCnt,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add ExportFolderCnt as an INPUT param for an insert
            m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt",
                                        vValue:=nTransactionExportFolderCnt,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="Stats_folder_cnt",
                           vValue:=nStatsFolderCnt,
                           iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                           iDataType:=gPMConstants.PMEDataType.PMLong)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End With

        ' Execute Add Trans Export Details SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=kCopyClonedExportDetailsSQL,
                                          sSQLName:=kCopyClonedExportDetailsName,
                                          bStoredProcedure:=kCopyClonedExportDetailsStored,
                                          lRecordsAffected:=nRecordsAffected)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return nResult
    End Function

    '''' <summary>
    '''' AllocateClonedTransactions
    '''' </summary>
    '''' <param name="vTransactions"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AllocateClonedTransactions(ByVal oTransactions(,) As Object) As Integer

        Dim oAllocationManual As Object = Nothing
        Dim lRow As Long
        Dim oCreditTransactions(,) As Object = Nothing
        Dim nResult As Integer
        Dim vKeyArray(1, 3) As Object
        Dim lCount As Long
        Dim vTrans(0) As Object

        Try

            nResult = PMEReturnCode.PMTrue

            'Get an instance of bACTAllocationManual component to do the allocation
            If gPMComponentServices.CreateBusinessObject(r_oObject:=oAllocationManual, v_sClassName:="bACTAllocationManual.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bACTAllocationManual", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="nDocumentId", vValue:=CStr(m_nClonedReversalDocumentID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_clm_get_transdetails_RI", sSQLName:="GetTransDetails",
                                          bStoredProcedure:=True, vResultArray:=oCreditTransactions)

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If oCreditTransactions IsNot Nothing AndAlso Informations.IsArray(oCreditTransactions) Then
                For lCount = 0 To oTransactions.GetUpperBound(1)

                    For lRow = 0 To oCreditTransactions.GetUpperBound(1)

                        If oTransactions(0, lCount) = oCreditTransactions(0, lRow) Then
                            If Math.Abs(ToSafeDecimal(oTransactions(2, lCount), 0)) = Math.Abs(ToSafeDecimal(oCreditTransactions(2, lRow), 0)) And
                                    oTransactions(3, lCount) = oCreditTransactions(3, lRow) Then

                                vTrans(0) = oTransactions(1, lCount) & "|" & oTransactions(2, lCount)

                                vKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 0) = ACTKeyNameAccountID
                                vKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 0) = oCreditTransactions(0, lRow)

                                vKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 1) = ACTKeyNameTransDetailID
                                vKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 1) = oCreditTransactions(1, lRow) & "|" & oCreditTransactions(2, lRow)

                                vKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 2) = ACTKeyNameTransDetailIDs
                                vKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 2) = vTrans

                                m_lReturn = oAllocationManual.SetKeys(vKeyArray:=CType(vKeyArray, Object))

                                If m_lReturn <> PMEReturnCode.PMTrue Then
                                    AllocateClonedTransactions = PMEReturnCode.PMFalse
                                    Exit Function
                                End If

                                m_lReturn = oAllocationManual.Start()

                                If m_lReturn <> PMEReturnCode.PMTrue Then
                                    AllocateClonedTransactions = PMEReturnCode.PMFalse
                                    Exit Function
                                End If
                            End If
                        End If
                    Next
                Next
            End If
            Return nResult
        Catch ex As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="AllocateClonedTransactions Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:="AllocateClonedTransactions", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message,
                               excep:=ex)

            Return nResult
        Finally
            If oAllocationManual IsNot Nothing Then
                oAllocationManual.Terminate()
                oAllocationManual = Nothing
            End If
        End Try

    End Function

    Private Function UpdateCreditControlItem(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                result = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                result = .SQLAction(sSQL:=ACUpdateCreditControlItemInsuranceFileSQL, sSQLName:=ACUpdateCreditControlItemInsuranceFileName, bStoredProcedure:=ACUpdateCreditControlItemInsuranceFileStored)

            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateCreditControlItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteCreditControlItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


End Class
