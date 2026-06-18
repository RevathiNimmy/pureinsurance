Option Strict Off
Option Explicit On
Imports System.Text
Imports SSP.Shared


Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Automated
    '
    ' Date: 07/04/1998
    '
    ' Description: Creatable Automated class which contains all the
    '              Automated methods which can be called by Navigator.
    '
    ' Edit History:
    ' RAW 04/03/2003 : ISS2592 : added test for no rows found in GetSchemeAccount
    ' RAW 11/03/2003 : ISS2580 : Correct the posting of transactions for a Plan
    ' RAW 13/11/2003 : CQ1765 : do not PostInstalmentDebit when cancelling a plan if the amount is 0
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 23/01/2004
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

    Private Const k_AGENT_TYPE_BROKER As Integer = 1
    Private Const k_AGENT_TYPE_SUBAGENT As Integer = 2
    Private Const k_AGENT_TYPE_COMMISSION As Integer = 3
    Private Const k_AGENT_TYPE_INTERMED As Integer = 5
    Private Const kRoundingUpto As Integer = 2
    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database
    Private m_bCloseDatabase As Boolean

    Private m_oAllocateManual As bACTAllocationManual.Business
    Private m_oDocumentPost As bACTDocumentPost.Form
    Private m_oAutoNumber As bACTAutoNumber.Business
    Private m_oTransDetail As bACTTransDetail.Form
    Private m_oCurrencyConvert As bACTCurrencyConvert.Form

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    ' Task
    Private m_iTask As Integer
    ' Navigate
    Private m_lNavigate As Integer
    ' Current Record Pointer
    Private m_lCurrentRecord As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date

    ' Calling Application name.

    ' Component Services object

    'Abort Transaction
    Private m_bAbortTrans As Boolean
    Private m_bTransStarted As Boolean
    Private m_lCompanyID As Integer
    Private m_lSubBranchID As Integer
    Private m_sBankCode As String = ""
    Private m_sFeeCode As String = ""
    Private m_lAccountID As Integer
    Private m_lAgentID As Integer
    Private m_lBankID As Integer
    Private m_lFeeID As Integer
    Private m_lSuspID As Integer
    Private m_lTaxSuspID As Integer
    Private m_lCommSuspID As Integer
    Private m_sInstalmentPlan As String = ""
    Private m_lInsuranceFileCnt As Integer ' RAW 11/03/2003 : ISS2580 : added
    Private m_vInsuranceRef As Object ' RAW 11/03/2003 : ISS2580 : added
    Private m_lFeeAccountId As Integer
    Private m_vUnderwriting As String = "" 'PN10637
    Private m_sCommissionOption As String = "" 'PN10637
    Private m_nSpreadCommission As Integer 'FSA Phase 3.2
    Private m_bAllocateDepositToPlan As Boolean 'FSA Phase 4
    Private m_sEarnCommissionOnPartPayments As String = "" 'FSA Phase 4
    Private m_lDocumentId As Integer
    Private m_bSuppressDecimalValues As Boolean

    'SP to Select a single CashListDrawer
    Private Const ACSelectCashListDrawerName As String = "SelectCashListDrawer"

    Private Const ACSelectCashListDrawerSQL As String = "spu_ACT_Select_CashListDrawer"

    Private Const ACAgentCommissionSuspenseTransaction As Integer = 2
    Private Const ACReinsuranceSuspenseTransaction As Integer = 1

    Private m_lTaxTransDetailId As Integer
    Private m_dOutstandingTaxAmount As Decimal
    Private m_lReversalTransDetailId As Integer
    Private m_dOutstandingTaxCurrencyAmount As Decimal
    Private m_nSubAgentCommSuspId As Integer
    Private m_bIsSinglePlanParty As Boolean
    'Deposit Cr Changes
    Private bIsRenewed As Boolean = False

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            m_sCallingAppName = Value

        End Set
    End Property

    'Recall Reversal

    Public Property ReversalTransDetailId() As Integer
        Get
            Return m_lReversalTransDetailId
        End Get
        Set(ByVal Value As Integer)
            m_lReversalTransDetailId = Value
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

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property
    Public WriteOnly Property BankCode() As String
        Set(ByVal Value As String)

            m_sBankCode = Value

        End Set
    End Property
    Public WriteOnly Property FeeCode() As String
        Set(ByVal Value As String)

            m_sFeeCode = Value

        End Set
    End Property
    Public WriteOnly Property BankID() As Integer
        Set(ByVal Value As Integer)
            m_lBankID = 0
        End Set
    End Property

    Public WriteOnly Property IsSinglePlanParty() As Boolean
        Set(ByVal value As Boolean)
            m_bIsSinglePlanParty = value
        End Set
    End Property

    'PN10637 Underwriting Specific Version
    Private Function CreateAccountingTransaction_SFU(ByVal v_lPremiumFinanceCnt As Integer, ByVal v_lPremiumFinanceVersion As Integer, ByVal v_lAgentCnt As Integer, ByVal v_bCommissionSpread As Boolean, ByVal v_sInsuranceRef As String, ByVal v_lInsuranceFileCnt As Integer, ByRef r_iDocSeq As Integer, ByVal v_dtAccountingDate As Date, Optional ByVal bSubAgentCommissionSpread As Boolean = Nothing, Optional ByVal oSubAgent As Object = Nothing) As Integer

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse


        Dim cCommission, cOutstandingCommission As Decimal
        Dim cCommissionBaseAmount, cOutstandingCommissionBaseAmount As Decimal
        Dim lAgentCommissionAccountID As Integer
        Dim dtDocumentDate As Date
        Dim lSuspendedTransDetailID As Integer
        Dim bRISuspendedFlag As Boolean
        Dim lRISuspenseAccount As Integer
        Dim vSuspendedParties(,) As Object = Nothing
        Dim vTransactionsToSuspend(,) As Object = Nothing
        Dim iBaseCurrencyID As Integer

        'FSA Phase 3.2
        Dim vResultArray(,) As Object = Nothing
        Dim lTransdetailTypeId As Integer
        'FSA Phase 3.2End
        Dim nCount As Integer
        Dim nSubAgentCommissionAccountID As Integer

        Dim vAgentOldTrans As Object = Nothing
        Dim lAgentTransDetailID As Long
        Dim vTransactions As Object = Nothing
        Dim nSubAgentOldTransDetailID As Object = Nothing
        Dim nSubAgentTransDetailID As Integer
        Const ACSuspendedPartyAccountID As Integer = 1
        Const ACSuspendedTransAmount As Integer = 0
        Const ACSuspendedTransOutstandingAmount As Integer = 1
        Const ACSuspendedTransSpare As Integer = 2
        Const ACSuspendedTransDetailId = 3
        bRISuspendedFlag = False
        Dim dXRate As Double = 1

        Dim nRITransDetailID As Long
        Dim bTransCurrency As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set the document date
        dtDocumentDate = DateTime.Now

        ' Get the Currency for Transacting against
        m_lReturn = GetSchemeCurrency(lPremiumFinanceCnt:=v_lPremiumFinanceCnt, lPremiumFinanceVersion:=v_lPremiumFinanceVersion, r_iCurrencyID:=iBaseCurrencyID, r_XRate:=dXRate, r_bTransCurrency:=bTransCurrency)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFail
        End If

        If v_lAgentCnt <> 0 And v_bCommissionSpread = True Then 'PN 78871
            m_lReturn = GetOutstandingCommission_SFU(v_lFinancePlanCnt:=v_lPremiumFinanceCnt, v_lFinancePlanVersion:=v_lPremiumFinanceVersion, r_cCommission:=cCommission,
                                                     r_cOutstandingCommission:=cOutstandingCommission, r_lOldCommissionAccountID:=lAgentCommissionAccountID,
                                                     v_lPartyCnt:=v_lAgentCnt, v_vTransactions:=vAgentOldTrans, r_cCommissionBaseAmount:=cCommissionBaseAmount,
                                                     r_cOutstandingCommissionBaseAmount:=cOutstandingCommissionBaseAmount)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAccountingTransaction_SFU failed to GetOutstandingCommission_SFU", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccountingTransaction_SFU", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Don't do it if some or all of the commission has been paid
            If cCommission = cOutstandingCommission And cCommission <> 0 Then
                If m_lCommSuspID = 0 Then
                    m_lReturn = GetSchemeAccount(v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, v_sWhichAccount:="COMM")
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAccountingTransaction_SFU failed to GetSchemeAccount COMM", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccountingTransaction_SFU", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If
                End If

                'Only do it if the commission IS in NOT Suspended
                If m_lCommSuspID <> lAgentCommissionAccountID Then

                    r_iDocSeq += 1

                    'FSA Phase 3.2 get Transdetail_type_id for Agent Commission
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTransdetailTypeIDSQL, sSQLName:=ACGetTransdetailTypeIDName, bStoredProcedure:=ACGetTransdetailTypeIDStored, vResultArray:=vResultArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAccountingTransaction_SFU failed to Get TransdetailTypeId", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccountingTransaction_SFU", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If
                    If Not Informations.IsArray(vResultArray) Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAccountingTransaction_SFU failed to Get TransdetailTypeId", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccountingTransaction_SFU", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    lTransdetailTypeId = CInt(vResultArray(0, 0))
                    'FSA Phase 3.2End

                    ' Add Transaction to the Agent Account the Value comes in Negative from the StoredProc

                    m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=lAgentCommissionAccountID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound(cCommission, kRoundingUpto, m_bSuppressDecimalValues), v_cCurrencyAmount:=gPMFunctions.ToSafeRound(cCommission, kRoundingUpto, m_bSuppressDecimalValues), v_vdCurrencyBaseXRate:=dXRate, v_vDocumentSequence:=r_iDocSeq, v_vInsuranceRef:=v_sInsuranceRef, v_vAccountingDate:=v_dtAccountingDate, r_vTransDetailId:=lAgentTransDetailID, v_vTransdetailTypeID:=lTransdetailTypeId, v_vSpare:="COMM")
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAccountingTransaction_SFU failed to AddTransaction", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccountingTransaction_SFU", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If
                    If Informations.IsArray(vAgentOldTrans) Then
                        ReDim vTransactions(UBound(vAgentOldTrans, 2))
                        For iCount As Integer = LBound(vAgentOldTrans, 2) To UBound(vAgentOldTrans, 2)
                            vTransactions(iCount) = vAgentOldTrans(3, iCount) & "|" & -1 * vAgentOldTrans(1, iCount)
                        Next
                    End If

                    m_lReturn = Allocate(
                           v_vTransaction:=lAgentTransDetailID & "|" & cCommission * dXRate,
                           v_vTransactions:=vTransactions)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        CreateAccountingTransaction_SFU = m_lReturn
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                sMsg:="CreateAccountingTransaction_SFU failed to AddTransaction",
                                vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccountingTransaction_SFU",
                                vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description)
                        Exit Function
                    End If
                    r_iDocSeq += 1

                    ' Add Transaction to the suspense Account from the Finance Plan Scheme.

                    m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=m_lCommSuspID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound(cCommission * dXRate *-1, kRoundingUpto, m_bSuppressDecimalValues), v_cCurrencyAmount:=gPMFunctions.ToSafeRound(cCommission * -1, kRoundingUpto, m_bSuppressDecimalValues), v_vdCurrencyBaseXRate:=dXRate, v_vDocumentSequence:=r_iDocSeq, v_vInsuranceRef:=v_sInsuranceRef, v_vAccountingDate:=v_dtAccountingDate, r_vTransDetailId:=lSuspendedTransDetailID, v_vTransdetailTypeID:=lTransdetailTypeId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAccountingTransaction_SFU failed to AddTransaction", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccountingTransaction_SFU", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    m_lReturn = m_oTransDetail.CreateSuspendedTransaction(lSuspendedTransdetailId:=lSuspendedTransDetailID, lLinkedTransdetailID:=0, dLinkedPercentage:=100, vPFPremFinanceCnt:=v_lPremiumFinanceCnt, vPFPremFinanceVersion:=v_lPremiumFinanceVersion, lInsuranceFileCnt:=0, lDestinationAccountID:=lAgentCommissionAccountID, lDocumentTypeId:=1, lTransdetailTypeID:=lTransdetailTypeId, sSpare:="")

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAccountingTransaction_SFU failed to UpdateSuspendedTransactions", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccountingTransaction_SFU", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If
                End If
            End If
        End If
        'Commission suspense Account Posting For Sub Agents
        If Informations.IsArray(oSubAgent) AndAlso bSubAgentCommissionSpread Then
            If m_nSubAgentCommSuspId = 0 Then
                m_lReturn = GetSchemeAccount(v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt,
                                             v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion,
                                             v_sWhichAccount:="SUBCOMM")
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    CreateAccountingTransaction_SFU = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="CreateAccountingTransaction_SFU failed to GetSchemeAccount COMM",
                                       vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="CreateAccountingTransaction_SFU",
                                       vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description)
                    Exit Function
                End If
            End If

            For nCount = LBound(oSubAgent, 2) To UBound(oSubAgent, 2)
                m_lReturn = GetOutstandingCommission_SFU(v_lFinancePlanCnt:=v_lPremiumFinanceCnt,
                                                         v_lFinancePlanVersion:=v_lPremiumFinanceVersion,
                                                         r_cCommission:=cCommission,
                                                         r_cOutstandingCommission:=cOutstandingCommission,
                                                         r_lOldCommissionAccountID:=nSubAgentCommissionAccountID,
                                                         v_lPartyCnt:=ToSafeLong(oSubAgent(0, nCount)),
                                                         v_vTransactions:=nSubAgentOldTransDetailID,
                                                          r_cCommissionBaseAmount:=cCommissionBaseAmount,
                         r_cOutstandingCommissionBaseAmount:=cOutstandingCommissionBaseAmount)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    CreateAccountingTransaction_SFU = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:=
                                          "CreateAccountingTransaction_SFU failed to GetOutstandingCommission_SFU",
                                       vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="CreateAccountingTransaction_SFU",
                                       vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description)
                    Exit Function
                End If

                'Don't do it if some or all of the commission has been paid
                If cCommission = cOutstandingCommission And cCommission <> 0 Then

                    If Not bTransCurrency Then
                        cCommission = cCommissionBaseAmount * -1
                        cOutstandingCommission = cOutstandingCommissionBaseAmount * -1
                    End If
                    'Only do it if the commission IS in NOT Suspended
                    If m_nSubAgentCommSuspId <> nSubAgentCommissionAccountID Then

                        r_iDocSeq = r_iDocSeq + 1

                        'FSA Phase 3.2 get Transdetail_type_id for Agent Commission
                        m_lReturn = m_oDatabase.SQLSelect(
                            sSQL:=ACGetTransdetailTypeIDSQL,
                            sSQLName:=ACGetTransdetailTypeIDName,
                            bStoredProcedure:=ACGetTransdetailTypeIDStored,
                            vResultArray:=vResultArray)

                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            CreateAccountingTransaction_SFU = m_lReturn
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                               sMsg:=
                                                  "CreateAccountingTransaction_SFU failed to Get TransdetailTypeId",
                                               vApp:=ACApp, vClass:=ACClass,
                                               vMethod:="CreateAccountingTransaction_SFU",
                                               vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description)
                            Exit Function
                        End If
                        If Not Informations.IsArray(vResultArray) Then
                            CreateAccountingTransaction_SFU = gPMConstants.PMEReturnCode.PMFalse
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                               sMsg:=
                                                  "CreateAccountingTransaction_SFU failed to Get TransdetailTypeId",
                                               vApp:=ACApp, vClass:=ACClass,
                                               vMethod:="CreateAccountingTransaction_SFU",
                                               vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description)
                            Exit Function
                        End If
                        lTransdetailTypeId = CLng(vResultArray(0, 0))
                        'FSA Phase 3.2End

                        ' Add Transaction to the SubAgent Account the Value comes in Negative from the StoredProc
                        m_lReturn =
                            m_oDocumentPost.AddTransaction(v_lAccountID:=nSubAgentCommissionAccountID,
                                                           v_iCurrencyID:=iBaseCurrencyID,
                                                           v_cAmount:=cCommission,
                                                           v_cCurrencyAmount:=cCommission,
                                                           v_vdCurrencyBaseXRate:=1,
                                                           v_vDocumentSequence:=r_iDocSeq,
                                                           v_vInsuranceRef:=v_sInsuranceRef,
                                                           v_vAccountingDate:=dtDocumentDate,
                                                           r_vTransDetailId:=nSubAgentTransDetailID,
                                                           v_vTransdetailTypeID:=lTransdetailTypeId)
                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            CreateAccountingTransaction_SFU = m_lReturn
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                               sMsg:="CreateAccountingTransaction_SFU failed to AddTransaction",
                                               vApp:=ACApp, vClass:=ACClass,
                                               vMethod:="CreateAccountingTransaction_SFU",
                                               vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description)
                            Exit Function
                        End If

                        ReDim vTransactions(0)
                        vTransactions(0) = ToSafeLong(nSubAgentOldTransDetailID(2, 0), 0) & "|" & -1 * cCommission

                        m_lReturn = Allocate(
                            v_vTransaction:=nSubAgentTransDetailID & "|" & cCommission * dXRate,
                            v_vTransactions:=vTransactions)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            CreateAccountingTransaction_SFU = m_lReturn
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                               sMsg:="CreateAccountingTransaction_SFU failed to AddTransaction",
                                               vApp:=ACApp, vClass:=ACClass,
                                               vMethod:="CreateAccountingTransaction_SFU",
                                               vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description)
                            Exit Function
                        End If

                        r_iDocSeq = r_iDocSeq + 1

                        ' Add Transaction to the suspense Account from the Finance Plan Scheme.
                        m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=m_nSubAgentCommSuspId,
                               v_iCurrencyID:=iBaseCurrencyID,
                               v_cAmount:=cCommission * dXRate * -1,
                               v_cCurrencyAmount:=cCommission * -1,
                               v_vdCurrencyBaseXRate:=dXRate,
                               v_vDocumentSequence:=r_iDocSeq,
                               v_vInsuranceRef:=v_sInsuranceRef,
                               v_vAccountingDate:=dtDocumentDate,
                               r_vTransDetailId:=lSuspendedTransDetailID,
                               v_vTransdetailTypeID:=lTransdetailTypeId)
                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            CreateAccountingTransaction_SFU = m_lReturn
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                               sMsg:="CreateAccountingTransaction_SFU failed to AddTransaction",
                                               vApp:=ACApp, vClass:=ACClass,
                                               vMethod:="CreateAccountingTransaction_SFU",
                                               vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description)
                            Exit Function
                        End If

                        m_lReturn =
            m_oTransDetail.CreateSuspendedTransaction(
                lSuspendedTransdetailId:=lSuspendedTransDetailID,
                lLinkedTransdetailID:=0,
                dLinkedPercentage:=100,
                vPFPremFinanceCnt:=v_lPremiumFinanceCnt,
                vPFPremFinanceVersion:=v_lPremiumFinanceVersion,
                lInsuranceFileCnt:=0,
                lDestinationAccountID:=nSubAgentCommissionAccountID,
                lDocumentTypeId:=1,
                lTransdetailTypeID:=lTransdetailTypeId,
                sSpare:="")


                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            CreateAccountingTransaction_SFU = m_lReturn
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                               sMsg:=
                                                  "CreateAccountingTransaction_SFU failed to UpdateSuspendedTransactions",
                                               vApp:=ACApp, vClass:=ACClass,
                                               vMethod:="CreateAccountingTransaction_SFU",
                                               vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description)
                            Exit Function
                        End If
                    End If
                End If
            Next nCount
        End If
        ' Get the RI Suspense Flag and Account from Scheme
        m_lReturn = GetRISuspenseInfo(v_lFinancePlanCnt:=v_lPremiumFinanceCnt, v_lFinancePlanVersion:=v_lPremiumFinanceVersion, r_bSuspenseFlag:=bRISuspendedFlag, r_lSuspenseAccount:=lRISuspenseAccount)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAccountingTransaction_SFU failed to GetRISuspenseInfo", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccountingTransaction_SFU", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If
        If bRISuspendedFlag Then
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="transdetail_type_code", vValue:="RISUSP", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRISUSPTransdetailTypeIDSQL, sSQLName:=ACGetRISUSPTransdetailTypeIDName, bStoredProcedure:=ACGetRISUSPTransdetailTypeIDStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAccountingTransaction_SFU failed to Get RISUSP TransdetailTypeId", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccountingTransaction_SFU", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If
            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAccountingTransaction_SFU failed to Get TransdetailTypeId", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccountingTransaction_SFU", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            lTransdetailTypeId = CInt(vResultArray(0, 0))

            ' Get the RI Parties and Account Information
            m_lReturn = GetSuspendedParties(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vSuspendedParties:=vSuspendedParties)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAccountingTransaction_SFU failed to GetSuspendedParties", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccountingTransaction_SFU", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' If the function returned a result set
            If Informations.IsArray(vSuspendedParties) Then
                ' Loop through the array and create suspended records

                For iCount As Integer = vSuspendedParties.GetLowerBound(1) To vSuspendedParties.GetUpperBound(1)
                    ' Get the outstanding commission for the party
                    m_lReturn = GetSuspendedTransactions(v_lFinancePlanCnt:=v_lPremiumFinanceCnt, v_lFinancePlanVersion:=v_lPremiumFinanceVersion, v_lAccountId:=gPMFunctions.NullToLong(vSuspendedParties(ACSuspendedPartyAccountID, iCount)), r_vTransactionToSuspend:=vTransactionsToSuspend)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAccountingTransaction_SFU failed to GetSuspendedTransactions", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccountingTransaction_SFU", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If
                    If Informations.IsArray(vTransactionsToSuspend) Then

                        For iTransCount As Integer = vTransactionsToSuspend.GetLowerBound(1) To vTransactionsToSuspend.GetUpperBound(1)
                            'Don't do it if some or all of the commission has been paid
                            If gPMFunctions.NullToCurrency(vTransactionsToSuspend(ACSuspendedTransAmount, iTransCount)) = gPMFunctions.NullToCurrency(vTransactionsToSuspend(ACSuspendedTransOutstandingAmount, iTransCount)) Then
                                'Only do it if the commission IS in the wrong account
                                If lRISuspenseAccount <> gPMFunctions.NullToLong(vSuspendedParties(ACSuspendedPartyAccountID, iCount)) Then

                                    r_iDocSeq += 1

                                    ' Add the commission transaction we need to Multiply by -1

                                    m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=gPMFunctions.NullToLong(vSuspendedParties(ACSuspendedPartyAccountID, iCount)), v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound(gPMFunctions.NullToCurrency(vTransactionsToSuspend(ACSuspendedTransAmount, iTransCount)) * -1, kRoundingUpto, m_bSuppressDecimalValues), v_cCurrencyAmount:=gPMFunctions.ToSafeRound(gPMFunctions.NullToCurrency(vTransactionsToSuspend(ACSuspendedTransAmount, iTransCount)) * -1, kRoundingUpto, m_bSuppressDecimalValues), v_vdCurrencyBaseXRate:=1, v_vDocumentSequence:=r_iDocSeq, v_vInsuranceRef:=v_sInsuranceRef, v_vAccountingDate:=dtDocumentDate, v_vTransdetailTypeID:=lTransdetailTypeId, r_vTransDetailId:=nRITransDetailID)
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        result = m_lReturn
                                        ' Log Error Message
                                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAccountingTransaction_SFU failed to AddTransaction", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccountingTransaction_SFU", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                        Return result
                                    End If
                                    Dim vRITransactions As Object
                                    ReDim vRITransactions(0)
                                    ''For iRICount As Integer = LBound(vTransactionsToSuspend, 2) To UBound(vTransactionsToSuspend, 2)
                                    vRITransactions(0) = vTransactionsToSuspend(3, iTransCount) & "|" & vTransactionsToSuspend(1, iTransCount)
                                    '' Next

                                    m_lReturn = Allocate(
                           v_vTransaction:=nRITransDetailID & "|" & ToSafeDecimal(vTransactionsToSuspend(ACSuspendedTransAmount, iTransCount)) * -1,
                           v_vTransactions:=vRITransactions)

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                                        CreateAccountingTransaction_SFU = m_lReturn
                                        ' Log Error Message
                                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                sMsg:="CreateAccountingTransaction_SFU failed to AddTransaction",
                                vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccountingTransaction_SFU",
                                vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description)
                                        Exit Function
                                    End If


                                    ReDim vTransactions(0)
                                    vTransactions(0) = vTransactionsToSuspend(ACSuspendedTransDetailId, iTransCount) & "|" & NullToCurrency(vTransactionsToSuspend(ACSuspendedTransAmount, iTransCount))

                                    m_lReturn = Allocate(
                                            v_vTransaction:=nRITransDetailID & "|" & NullToCurrency(vTransactionsToSuspend(ACSuspendedTransAmount, iTransCount)) * -1,
                                            v_vTransactions:=vTransactions)

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        CreateAccountingTransaction_SFU = m_lReturn
                                        ' Log Error Message
                                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAccountingTransaction_SFU failed to AddTransaction", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccountingTransaction_SFU", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                                        Exit Function
                                    End If

                                    r_iDocSeq += 1

                                    ' Add the suspense transaction which is the amount

                                    m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=lRISuspenseAccount, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound(gPMFunctions.NullToCurrency(vTransactionsToSuspend(ACSuspendedTransAmount, iTransCount)), kRoundingUpto, m_bSuppressDecimalValues), v_cCurrencyAmount:=gPMFunctions.ToSafeRound(gPMFunctions.NullToCurrency(vTransactionsToSuspend(ACSuspendedTransAmount, iTransCount)), kRoundingUpto, m_bSuppressDecimalValues), v_vdCurrencyBaseXRate:=1, v_vDocumentSequence:=r_iDocSeq, v_vInsuranceRef:=v_sInsuranceRef, v_vAccountingDate:=v_dtAccountingDate, r_vTransDetailId:=lSuspendedTransDetailID, v_vTransdetailTypeID:=lTransdetailTypeId)
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        result = m_lReturn
                                        ' Log Error Message
                                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAccountingTransaction_SFU failed to AddTransaction", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccountingTransaction_SFU", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                        Return result
                                    End If

                                    m_lReturn = m_oTransDetail.CreateSuspendedTransaction(lSuspendedTransdetailId:=lSuspendedTransDetailID, lLinkedTransdetailID:=0, dLinkedPercentage:=100, vPFPremFinanceCnt:=v_lPremiumFinanceCnt, vPFPremFinanceVersion:=v_lPremiumFinanceVersion, lInsuranceFileCnt:=0, lDestinationAccountID:=gPMFunctions.NullToLong(vSuspendedParties(ACSuspendedPartyAccountID, iCount)), lDocumentTypeId:=1, lTransdetailTypeID:=lTransdetailTypeId, sSpare:=gPMFunctions.NullToString(vTransactionsToSuspend(ACSuspendedTransSpare, iTransCount)))

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        result = m_lReturn
                                        ' Log Error Message
                                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAccountingTransaction_SFU failed to CreateAccountingTransaction_SFU", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateSuspendedTransaction", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                        Return result
                                    End If
                                End If
                            End If
                        Next
                    End If
                Next
            End If
        End If

        Return result

    End Function

    '******************************************************************************
    '        Function Name:  GetLastInstalmentID
    '******************************************************************************
    '           Created By:  Ahmed A. Bishtawi
    '           Created On:  18-Aug-2003
    '******************************************************************************
    '       Parameters Are:
    '                        (In)     - v_lPremFinanceCnt     - Long  -
    '                        (In)     - v_lPremFinanceVersion - Long  -
    '                        (In/Out) - r_lLastInstalmentID   - Long  -
    '
    ' Return Value Type Is:  Long -
    '******************************************************************************
    ' Function Description:  This function returns the last instalmentID for given
    '                        Plan and version
    '******************************************************************************
    Private Function GetLastInstalmentID(ByVal v_lPremFinanceCnt As Integer, ByVal v_lPremFinanceVersion As Integer, ByRef r_lLastInstalmentID As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="PremiumFinanceCnt", vValue:=CStr(v_lPremFinanceCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLastInstalmentID Failed to add Parameter to Stored Proc", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLastInstalmentID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If

            m_lReturn = .Parameters.Add(sName:="PremiumFinanceVersion", vValue:=CStr(v_lPremFinanceVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLastInstalmentID Failed to add Parameter to Stored Proc", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLastInstalmentID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If

            m_lReturn = .SQLSelect(sSQL:=ACGetLasInstalmentIDSQL, sSQLName:=ACGetLasInstalmentIDName, bStoredProcedure:=ACGetLasInstalmentIDStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLastInstalmentID Failed to process the Stored Proc", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLastInstalmentID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If

            If .Records.Count() >= 1 Then
                'Developer Guide No 162
                r_lLastInstalmentID = gPMFunctions.NullToLong(.Records.Item(0).Fields("pfinstalments_id"))
            Else
                r_lLastInstalmentID = 0
            End If

        End With

        Return result

    End Function
    '******************************************************************************
    '        Function Name:  ProcessPFAccountsForCancellation
    '******************************************************************************
    '           Created By:  Ahmed A. Bishtawi
    '           Created On:  18-Aug-2003
    '******************************************************************************
    '       Parameters Are:
    '                        (In) - v_lPremiumFinanceCnt     - Long  -
    '                        (In) - v_lPremiumFinanceVersion - Long  -
    '
    ' Return Value Type Is:  Long -
    '******************************************************************************
    ' Function Description:  This function process Dripping of reinsurance & agent
    '                        commission for cancellation
    '******************************************************************************
    Private Function ProcessPFAccountsForCancellation(ByVal v_lPremiumFinanceCnt As Integer, ByVal v_lPremiumFinanceVersion As Integer) As Integer

        Dim result As Integer = 0


        Dim lInsuranceFileCnt As Integer
        Dim sInsuranceRef As String = ""
        Dim lLastInstalmentID As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = GetInsuranceFileCntFromPF(lPremFinanceCnt:=v_lPremiumFinanceCnt, lPremFinanceVersion:=v_lPremiumFinanceVersion, r_lInsuranceFileCnt:=lInsuranceFileCnt, r_sInsuranceRef:=sInsuranceRef)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessPFAccountsForCancellation Failed to GetInsuranceFileCntFromPF", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPFAccountsForCancellation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        m_lReturn = GetLastInstalmentID(v_lPremFinanceCnt:=v_lPremiumFinanceCnt, v_lPremFinanceVersion:=v_lPremiumFinanceVersion, r_lLastInstalmentID:=lLastInstalmentID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessPFAccountsForCancellation Failed to GetLastInstalmentID", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPFAccountsForCancellation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        If lLastInstalmentID > 0 Then
            ' Process the Agent Commission first
            m_lReturn = ProcessSuspendedTransactions(v_lAllocationId:=0, v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, v_lInstalmentID:=lLastInstalmentID, v_lInsuranceFileCnt:=lInsuranceFileCnt, v_sInsuranceRef:=sInsuranceRef, v_bPartialPayment:=False, v_bCancellationProcess:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessPFAccountsForCancellation Failed to ProcessSuspendedTransactions For Agent Commission", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPFAccountsForCancellation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'FSA Phase 3.2
            ' Process the Reinsurance transactions
            '        m_lReturn& = ProcessSuspendedTransactions(v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, _
            ''                                                  v_lPremiumFinanceVersion:=v_lPremiumFinanceCnt, _
            ''                                                  v_lInstalmentID:=lLastInstalmentID, _
            ''                                                  v_lInsuranceFileCnt:=lInsuranceFileCnt, _
            ''                                                  v_sInsuranceRef:=sInsuranceRef, _
            ''                                                  v_lSuspenseType:=ACReinsuranceSuspenseTransaction, _
            ''                                                  v_bPartialPayment:=False)
            '         If m_lReturn& <> PMTrue Then
            '            ProcessPFAccountsForCancellation = m_lReturn&
            '            LogMessage m_sUsername, iType:=PMLogOnError, _
            ''                       sMsg:="ProcessPFAccountsForCancellation Failed to ProcessSuspendedTransactions For ReInsurance", _
            ''                       vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPFAccountsForCancellation", _
            ''                       vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description
            '            Exit Function
            '        End If
            'FSA Phase 3.2

            '    Else
            '        ' We could not find the last instalment raise an error
            '        'ProcessPFAccountsForCancellation = PMError
            '        LogMessage m_sUsername, iType:=PMLogOnError, _
            ''                   sMsg:="ProcessPFAccountsForCancellation Failed becauase LastInstalmentID was not found", _
            ''                   vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPFAccountsForCancellation", _
            ''                   vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description
            '        Exit Function
        End If

        Return result

    End Function

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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing, Optional ByRef vSIRDatabase As Object = Nothing) As Integer

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
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            ' Initialisation Code.

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bAbortTrans = True
            m_bTransStarted = False

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now


            m_oCurrencyConvert = New bACTCurrencyConvert.Form
            m_lReturn = m_oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_vUnderwriting = "" Or Convert.IsDBNull(m_vUnderwriting) Or Informations.IsNothing(m_vUnderwriting) Then
                m_lReturn = bPMFunc.getUnderwritingType(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_vUnderwriting)
            End If

            'Get the Decimal Suppression flag
            Dim sTempOptionValue As String = ""
            bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID,
                                          v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName,
                                          v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableDecimalsSuppression, v_vBranch:=m_iSourceID, r_vUnderwriting:=sTempOptionValue)

            If Trim(sTempOptionValue) = "1" Then
                m_bSuppressDecimalValues = True
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
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If

                If m_oAllocateManual IsNot Nothing Then
                    m_oAllocateManual.Dispose()
                    m_oAllocateManual = Nothing
                End If
                If m_oDocumentPost IsNot Nothing Then
                    m_oDocumentPost.Dispose()
                    m_oDocumentPost = Nothing
                End If
                If m_oAutoNumber IsNot Nothing Then
                    m_oAutoNumber.Dispose()
                    m_oAutoNumber = Nothing
                End If
                If m_oTransDetail IsNot Nothing Then
                    m_oTransDetail.Dispose()
                    m_oTransDetail = Nothing
                End If
                If m_oCurrencyConvert IsNot Nothing Then
                    m_oCurrencyConvert.Dispose()

                End If
                m_oCurrencyConvert = Nothing

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

            ReDim vKeyArray(0, 0)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetOutstandingTaxAmount(ByVal lDocumentId As Integer, ByVal lAccountId As Integer, ByRef dTaxAmount As Decimal, ByRef lTaxTransDetailId As Integer,
                                             ByRef dTaxCurrencyAmount As Decimal) As Integer
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse


        Dim vResultArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        bPMAddParameter.AddParameterLite(m_oDatabase, "Document_id", lDocumentId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
        bPMAddParameter.AddParameterLite(m_oDatabase, "Account_id", lAccountId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetOutstandingTaxAmount, sSQLName:=ACGetOutstandingTaxAmountName, bStoredProcedure:=True, vResultArray:=vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQL:=ACGetOutstandingTaxAmount", gPMConstants.PMELogLevel.PMLogError)
        End If

        If Informations.IsArray(vResultArray) Then

            'Developer Guide No 98
            dTaxAmount = gPMFunctions.ToSafeDecimal(vResultArray(1, 0), 0)
            dTaxCurrencyAmount = gPMFunctions.ToSafeDecimal(vResultArray(2, 0), 0)
            lTaxTransDetailId = gPMFunctions.ToSafeLong(vResultArray(0, 0), 0)
        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PostPlan (Post a new plan)
    '
    ' Description: Post a new plan
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    Public Function PostPlan(ByVal v_lPartyCnt As Integer, ByVal v_sInstalmentRef As String,
                             ByVal v_cAmount As Decimal, ByVal v_cDeposit As Decimal,
                             ByVal v_vFinanceTransIds As Object,
                             ByRef v_lPlanTransDetailID As Integer,
                             Optional ByVal v_lAgentCnt As Integer = 0,
                             Optional ByRef vDepositTransID As Object = Nothing,
                             Optional ByVal v_cTax As Decimal = 0,
                             Optional ByVal v_cFee As Decimal = 0,
                             Optional ByVal v_lPremiumFinanceCnt As Integer = 0,
                             Optional ByVal v_lPremiumFinanceVersion As Integer = 0,
                             Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_bCommissionSpread As Boolean = False, Optional ByVal v_lAgentType As Integer = 0, Optional ByVal v_bProcessDeposit As Boolean = True, Optional ByRef v_vDepositJournal As Object = Nothing, Optional ByRef v_cCommissionAmount As Decimal = 0,
                             Optional ByVal v_lAccountID As Integer = 0,
                             Optional ByVal bSubAgentCommissionSpread As Boolean = False,
                             Optional ByVal oSubAgent As Object = Nothing,
                             Optional ByVal cCurrXRate As Decimal = 1) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PostPlan"

        Dim cFinanceAmount, cDebitAmount, cInterestAmount As Decimal
        Dim lDepositTransdetailId, lCreditTransDetailId As Integer
        Dim vMatchTransDetailIds As Object = Nothing
        Dim cTransDetailOSAmount As Decimal
        Dim cTransDetailsOSAmountTransCurrency As Decimal
        Dim lClientAccountID As Integer
        Dim iBaseCurrencyID As Integer
        Dim dDepositPercentage As Double
        Dim lDepositJournalId, lCreditSuspenceTransDetailID As Integer
        Dim cCreditSuspenceAmount As Decimal
        Dim lDebitSuspenceTransDetailID As Integer
        Dim cDebitSuspenceAmount As Decimal
        Dim vSuspenceTransDetailIDs As Object
        Dim bRollback As Boolean
        Dim lDocumentTypeID As Integer
        Dim sRangeCode As String = String.Empty
        Dim sDocRef As String = String.Empty
        Dim sDocumentTypeCode As String = String.Empty
        Dim vDocumentType As Object = Nothing
        Dim lTransDetailId As Integer
        Dim dAmountNotIncludedInInstalment As Double
        Dim bResetToIRD As Boolean
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(v_vFinanceTransIds) Then
                gPMFunctions.RaiseError("IsArray(v_vFinanceTransIds)", "Transaction array is invalid", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get the first transdetail of the transactions to be financed by the instalment plan

            lTransDetailId = CInt(v_vFinanceTransIds(0))

            'Get the document type
            m_lReturn = GetTransDetailDocumentType(v_lTransDetailId:=lTransDetailId, r_vResults:=vDocumentType)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetTransDetailDocumentType", "v_lTransDetailId:=" & lTransDetailId, gPMConstants.PMELogLevel.PMLogError)
            End If
            If Informations.IsArray(vDocumentType) Then

                sDocumentTypeCode = CStr(vDocumentType(0, 0)).Trim()
            Else
                gPMFunctions.RaiseError("IsArray(vDocumentType)", "DocumentType array is invalid", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_sInstalmentPlan = v_sInstalmentRef
            m_lInsuranceFileCnt = v_lInsuranceFileCnt

            'Get the AccountIds. m_lAccountID for Credit & lClientAccountId for Debit
            m_lReturn = GetClientAccount(r_lAccountId:=lClientAccountID, v_lPartyCnt:=v_lPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetClientAccount", "v_lPartyCnt:=" & v_lPartyCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            If v_lAccountID > 0 Then
                m_lAccountID = v_lAccountID
            Else
                If Not Informations.IsNothing(v_lAgentCnt) And v_lAgentCnt <> 0 And v_lAgentType = k_AGENT_TYPE_BROKER Then
                    'Get the Account_Id from the AgentCnt passed in
                    m_lReturn = GetClientAccount(r_lAccountId:=m_lAccountID, v_lPartyCnt:=v_lAgentCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("GetClientAccount", "v_lPartyCnt:=" & v_lAgentCnt, gPMConstants.PMELogLevel.PMLogError)
                    End If
                    'Now get the Client's AccountId and hold it locally
                    m_lReturn = GetClientAccount(r_lAccountId:=lClientAccountID, v_lPartyCnt:=v_lPartyCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("GetClientAccount", "v_lPartyCnt:=" & v_lPartyCnt,
                                                gPMConstants.PMELogLevel.PMLogError)
                    End If
                ElseIf Not Informations.IsNothing(v_lAgentCnt) And v_lAgentCnt <> 0 And v_lAgentType = k_AGENT_TYPE_INTERMED Then
                    Dim vResultArray(,) As Object = Nothing
                    result = gPMConstants.PMEReturnCode.PMTrue
                    m_oDatabase.Parameters.Clear()
                    bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAccountDetailsForInsuranceFileSQL, sSQLName:=ACGetAccountDetailsForInsuranceFileName, bStoredProcedure:=ACGetAccountDetailsForInsuranceStored, vResultArray:=vResultArray)
                    If Not vResultArray.Length = 0 AndAlso Not vResultArray(0, 0).ToString = "" Then
                        'Now get the Client's AccountId and hold it locally
                        m_lAccountID = vResultArray(0, 0)
                    Else
                        m_lReturn = GetClientAccount(r_lAccountId:=m_lAccountID, v_lPartyCnt:=v_lAgentCnt)
                    End If
                Else
                    'Just Get the Account_Id from the PartyCnt passed in
                    m_lReturn = GetClientAccount(r_lAccountId:=m_lAccountID, v_lPartyCnt:=v_lPartyCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("GetClientAccount", "v_lPartyCnt:=" & v_lPartyCnt,
                                             gPMConstants.PMELogLevel.PMLogError)
                    End If
                    m_lAccountID = lClientAccountID
                End If
            End If

            m_lReturn = SetPlanSourceID(v_lPremFinanceCnt:=v_lPremiumFinanceCnt, v_lPremFinanceVersion:=v_lPremiumFinanceVersion)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetPlanSourceID", "v_lPremFinanceCnt:=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If
            Dim dXRate As Double = 1
            'Get the Currency for Transacting against
            m_lReturn = GetSchemeCurrency(lPremiumFinanceCnt:=v_lPremiumFinanceCnt, lPremiumFinanceVersion:=v_lPremiumFinanceVersion, r_iCurrencyID:=iBaseCurrencyID, r_XRate:=dXRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetSchemeCurrency", "lPremiumFinanceCnt:=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If
            If dXRate = 0 Then
                dXRate = 1
            End If

            m_lReturn = CreateTransdetail()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CreateTransdetail", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'put the transaction array together for allocation
            cDebitAmount = 0

            'If it is new business then we don't do this anymore
            If Informations.IsArray(v_vFinanceTransIds) Then
                'Moved following line to within if statement
                ReDim vMatchTransDetailIds(v_vFinanceTransIds.GetUpperBound(0))

                For iCount As Integer = 0 To v_vFinanceTransIds.GetUpperBound(0)

                    m_lReturn = m_oTransDetail.GetDetails(vTransdetailID:=v_vFinanceTransIds(iCount), vOSAmounts:=True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        gPMFunctions.RaiseError("m_oTransDetail.GetDetails", "vTransdetailID:=" & CStr(v_vFinanceTransIds(iCount)), gPMConstants.PMELogLevel.PMLogError)
                    End If

                    m_lReturn = m_oTransDetail.GetNext(vDocumentID:=m_lDocumentId, vOSBaseAmount:=cTransDetailOSAmount, vCurrencyAmount:=cTransDetailsOSAmountTransCurrency)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        gPMFunctions.RaiseError("m_oTransDetail.GetNext", "vTransdetailID:=" & CStr(v_vFinanceTransIds(iCount)), gPMConstants.PMELogLevel.PMLogError)
                    End If

                    m_lReturn = GetAmountNotIncludedInInstalment(vTransdetailID:=CInt(v_vFinanceTransIds(iCount)), r_vAmountNotIncludedInInstalment:=dAmountNotIncludedInInstalment)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If dXRate = 1 Then
                        cDebitAmount = cDebitAmount + cTransDetailOSAmount - dAmountNotIncludedInInstalment
                        vMatchTransDetailIds(iCount) = CStr(v_vFinanceTransIds(iCount)) & "|" & (CStr(cTransDetailOSAmount - dAmountNotIncludedInInstalment))
                    Else
                        cDebitAmount = cDebitAmount + cTransDetailsOSAmountTransCurrency - (dAmountNotIncludedInInstalment / dXRate)
                        vMatchTransDetailIds(iCount) = CStr(v_vFinanceTransIds(iCount)) & "|" & (CStr(cTransDetailOSAmount - dAmountNotIncludedInInstalment))
                    End If


                    ' Store the details for allocation later - allocate the full original amount to reset balance to 0

                    If cTransDetailOSAmount - dAmountNotIncludedInInstalment = 0 Then
                        'do add this in trans match detail as this is transaction is not to be included in debit amount
                    Else

                        If Not Informations.IsArray(vMatchTransDetailIds) Then
                            ReDim vMatchTransDetailIds(0)
                        Else
                            'ReDim Preserve vMatchTransDetailIds(UBound(vMatchTransDetailIds) + 1)
                        End If
                        'vMatchTransDetailIds(UBound(vMatchTransDetailIds)) = v_vFinanceTransIds(iCount) & "|" & (cTransDetailOSAmount)
                        ' vMatchTransDetailIds(iCount) = v_vFinanceTransIds(iCount) & "|" & (cTransDetailOSAmount)

                    End If
                Next
            End If

            'The new amount - the deposit
            cFinanceAmount = v_cAmount - v_cDeposit
            'The new amount - The original amount posted in New business
            cInterestAmount = v_cAmount - cDebitAmount

            m_lReturn = m_oDatabase.SQLBeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = True
            End If

            'Post Credit in Revenue Account
            m_lReturn = PostInstalmentCredit(v_cCreditAmount:=cDebitAmount,
                                                    r_lCreditTransDetailId:=lCreditTransDetailId,
                                                    v_bPostToSuspense:=True,
                                                    v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt,
                                                    v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion,
                                                    v_lAgentCnt:=v_lAgentCnt, v_bNewPlan:=True,
                                                    v_bCommissionSpread:=v_bCommissionSpread,
                                                    v_bCreateAccountingTransactions:=True,
                                                    iBaseCurrencyID:=iBaseCurrencyID,
                                                    r_lCreditSuspenceTransDetailID:=lCreditSuspenceTransDetailID,
                                                    r_cCreditSuspenceAmount:=cCreditSuspenceAmount,
                                                    v_dXRate:=dXRate, bSubAgentCommissionSpread:=bSubAgentCommissionSpread, oSubAgent:=oSubAgent)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("PostInstalmentCredit", "v_lPremiumFinanceCnt:=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If
            ' Check if plan gets cancelled(SED) and previously its on Renewal so IRD will be generated
            If v_lPremiumFinanceCnt > 0 And v_lPremiumFinanceVersion > 0 And sDocumentTypeCode = gACTLibrary.ACTAutoNumberRangeCodeSed Then
                m_lReturn = GetInsuranceFileTransactionType(nInsuranceFileCnt:=v_lInsuranceFileCnt, r_bResetToIRD:=bResetToIRD)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New ApplicationException("Method GetInsuranceFileTransactionType() failed")
                End If
            End If

            ' if the original transactions to be financed are on a document of type "renewal invoice"
            If sDocumentTypeCode = kDocumentTypeCodeRenewalInvoice OrElse bResetToIRD = True Then
                ' then instalment plan transactions should be "instalment renewal" based
                lDocumentTypeID = gACTLibrary.ACTDocTypeInstalmentRenewalDebit
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeIRD
                sDocRef = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef46
            Else
                lDocumentTypeID = gACTLibrary.ACTDocTypeInstalmentDebit
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeIND
                sDocRef = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef42
            End If

            'ADO #39472: Override for claim recovery plans (SR/TPR) — use ICD
            Dim sPostPlanTransType As String = ""
            m_lReturn = GetPlanTransType(v_lPremFinanceCnt:=v_lPremiumFinanceCnt, v_lPremFinanceVersion:=v_lPremiumFinanceVersion, r_sTransType:=sPostPlanTransType)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetPlanTransType", "v_lPremiumFinanceCnt:=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If
            If sPostPlanTransType = "SR" OrElse sPostPlanTransType = "TPR" Then
                lDocumentTypeID = gACTLibrary.ACTDocTypeInstalmentClaimDebit
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeICD
                sDocRef = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef59
            End If
            'Priya
            'm_lReturn = PostInstalmentDebit(v_lAccountId:=lClientAccountID, v_cDebitAmount:=v_cAmount, v_cInterestAmount:=cInterestAmount, r_lDebitTransdetailId:=v_lPlanTransDetailID, v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, v_cTaxAmount:=v_cTax, iBaseCurrencyID:=iBaseCurrencyID, v_lDocumentTypeID:=lDocumentTypeID, v_sRangeCode:=sRangeCode, v_sDocRef:=sDocRef, r_lDebitSuspenceTransDetailID:=lDebitSuspenceTransDetailID, r_cDebitSuspenceAmount:=cDebitSuspenceAmount)
            m_lReturn = PostInstalmentDebit(v_lAccountId:=m_lAccountID, v_cDebitAmount:=v_cAmount, v_cInterestAmount:=cInterestAmount, r_lDebitTransdetailId:=v_lPlanTransDetailID,
                                            v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, v_cTaxAmount:=v_cTax,
                                            iBaseCurrencyID:=iBaseCurrencyID, v_lDocumentTypeID:=lDocumentTypeID, v_sRangeCode:=sRangeCode, v_sDocRef:=sDocRef,
                                            r_lDebitSuspenceTransDetailID:=lDebitSuspenceTransDetailID, r_cDebitSuspenceAmount:=cDebitSuspenceAmount, v_dXRate:=dXRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("PostInstalmentDebit", "v_lAccountId:=" & lClientAccountID, gPMConstants.PMELogLevel.PMLogError)
            End If
            'Post Transaction For value of Deposit
            If v_bProcessDeposit Then
                If v_cDeposit <> 0 Then

                    If Not Informations.IsNothing(vDepositTransID) Then
                        lDepositTransdetailId = CInt(Val(vDepositTransID))
                    End If

                    m_lReturn = PostDeposit(v_cAmount:=v_cDeposit, v_lTransDetailId:=lDepositTransdetailId, v_lAccount_ID:=m_lAccountID,
                                            r_vMatchTransDetailIds:=vMatchTransDetailIds, iBaseCurrencyID:=iBaseCurrencyID, r_vDepositJournal:=v_vDepositJournal, v_dXRate:=dXRate,
                                            nPremiumFinanceCnt:=v_lPremiumFinanceCnt, nPremiumFinanceVersion:=v_lPremiumFinanceVersion, nPlanTransDetailID:=v_lPlanTransDetailID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("PostDeposit", "v_lTransDetailID:=" & lDepositTransdetailId, gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'Calculate Deposit Percentage and get TransactionId
                    dDepositPercentage = v_cDeposit / cDebitAmount
                    'Developer Guide No 292
                    If Not Informations.IsNothing(v_vDepositJournal) Then
                        lDepositJournalId = CInt(Mid(v_vDepositJournal, 1, v_vDepositJournal.IndexOf("|"c)))
                    End If

                End If
            End If
            'If we posted both transactions to the suspense account and the values match then allocate them together.
            If lDebitSuspenceTransDetailID <> 0 And lCreditSuspenceTransDetailID <> 0 And cDebitSuspenceAmount = cCreditSuspenceAmount * -1 Then

                ReDim vSuspenceTransDetailIDs(0)

                vSuspenceTransDetailIDs(0) = CStr(lDebitSuspenceTransDetailID) & "|" & CStr(cDebitSuspenceAmount * dXRate)

                m_lReturn = Allocate(v_vTransaction:=lCreditSuspenceTransDetailID & "|" & cCreditSuspenceAmount * dXRate, v_vTransactions:=vSuspenceTransDetailIDs)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("Allocate", "v_vTransaction:=" & lCreditSuspenceTransDetailID & "|" & CStr(cCreditSuspenceAmount), gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            'Update Suspended Accounts Transaction if commission should be taken when instalments are paid
            m_lReturn = GetSystemOptionValue(v_iOptionNumber:=CInt(ACCommissionMovementOptionNo), r_sValue:=m_sCommissionOption)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetSystemOptionValue", "v_iOptionNumber:=" & ACCommissionMovementOptionNo, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_sEarnCommissionOnPartPayments = "0"
            If m_sCommissionOption = ClientPayment Then
                m_lReturn = GetSystemOptionValue(v_iOptionNumber:=CInt(ACEarnCommissionOnPartPayments), r_sValue:=m_sEarnCommissionOnPartPayments)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetSystemOptionValue", "v_iOptionNumber:=" & ACEarnCommissionOnPartPayments, gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            If m_sCommissionOption = ClientPayment Then
                If v_bCommissionSpread Or m_sEarnCommissionOnPartPayments = "1" Then
                    m_lReturn = UpdateSuspendedAccountsTransactions(v_vOldTriggerTransdetailIds:=v_vFinanceTransIds, v_vNewTriggerTransdetailId:=v_lPlanTransDetailID, v_vPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_vPremiumFinanceVersion:=v_lPremiumFinanceVersion, v_vDepositTransdetailID:=lDepositJournalId, v_vDepositPercentage:=dDepositPercentage)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("UpdateSuspendedAccountsTransactions", "v_vNewTriggerTransdetailId:=" & v_lPlanTransDetailID, gPMConstants.PMELogLevel.PMLogError)
                    End If
                Else
                    m_lReturn = UpdateSuspendedAccountsTransactions(v_vOldTriggerTransdetailIds:=v_vFinanceTransIds, v_vNewTriggerTransdetailId:=v_lPlanTransDetailID, v_vDepositTransdetailID:=lDepositJournalId, v_vDepositPercentage:=dDepositPercentage)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("UpdateSuspendedAccountsTransactions", "v_vNewTriggerTransdetailId:=" & v_lPlanTransDetailID, gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            End If

            If m_dOutstandingTaxAmount <> 0 Then
                If m_lTaxTransDetailId <> 0 Then
                    'For iAccountIdCount = LBound(v_vFinanceTransIds, 1) To UBound(v_vFinanceTransIds, 1)
                    '    If v_vFinanceTransIds(iAccountIdCount) = m_lTaxTransDetailId Then
                    '        vAllocatedTax = Split(vMatchTransDetailIds(iAccountIdCount), "|")

                    '        m_lOutstandingTaxAmount = ToSafeCurrency(m_lOutstandingTaxAmount - vAllocatedTax(1), 0)
                    '    End If
                    'Next

                    ReDim Preserve vMatchTransDetailIds(vMatchTransDetailIds.GetUpperBound(0) + 1)
                    ' allocate the deposit amount only to the new plan transaction

                    vMatchTransDetailIds(vMatchTransDetailIds.GetUpperBound(0)) = CStr(m_lTaxTransDetailId) & "|" & CStr(m_dOutstandingTaxAmount)
                End If
            End If
            'Now do the allocations
            m_bAllocateDepositToPlan = True

            m_lReturn = Allocate(v_vTransaction:=lCreditTransDetailId & "|" & (cDebitAmount * dXRate) * -1, v_vTransactions:=vMatchTransDetailIds)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Allocate", "v_vTransaction:=" & lCreditTransDetailId & "|" & CStr((cDebitAmount) * -1), gPMConstants.PMELogLevel.PMLogError)
            End If

            If lDepositTransdetailId <> 0 Then
                vDepositTransID = lDepositJournalId
            End If

            'Match the journal to cash deposit if it has already been posted
            'If lDepositTransdetailId <> 0 Then
            '    vDepositTransID = lDepositTransdetailId
            '    m_bAllocateDepositToPlan = False

            '    ReDim vMatchTransDetailIds(0)

            '    vMatchTransDetailIds(0) = v_vDepositJournal

            '    m_lReturn = Allocate(v_vTransaction:=lDepositTransdetailId & "|" & v_cDeposit * -1, v_vTransactions:=vMatchTransDetailIds)
            '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '        gPMFunctions.RaiseError("Allocate", "v_vTransaction:=" & lDepositTransdetailId & "|" & CStr(v_cDeposit * -1), gPMConstants.PMELogLevel.PMLogError)
            '    End If
            'End If

            m_lReturn = m_oDatabase.SQLCommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = False
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            If bRollback Then
                m_oDatabase.SQLRollbackTrans()
            End If

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CancelPlan (Cancel plan)
    '
    ' Description: Cancel plan
    '
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    Public Function CancelPlan(ByVal v_lPlanTransDetailID As Integer, ByVal v_cAmount As Decimal, ByVal v_lPremiumFinanceCnt As Integer, ByVal v_lPremiumFinanceVersion As Integer, ByVal v_cTax As Decimal, Optional ByRef r_vCreditTransDetail As Object = Nothing, Optional ByRef r_vDebitTransDetail As Object = Nothing, Optional ByVal v_bPlanHasSinglePolicy As Boolean = False, Optional ByVal v_iIsThirdPartyPlan As Integer = 0, Optional ByVal v_lPartyCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CancelPlan"
        'Developer Guide No 98
        'Const cInsFileCnt As Integer = 0
        'Const cStatsFolderCnt As Integer = 1
        'Const cSettlementAmount As Integer = 2
        'Const cInstalmentFeeSplitByCOB As Integer = 1032

        Dim iCount As Integer
        Dim vFinanceTransIds As Object
        Dim cTransDetailOSAmount As Decimal
        Dim lCreditTransDetailId, lDebitTransDetailId As Integer
        Dim vMatchTransDetailIds As Object
        Dim m_crSettleAmount, m_crSettleRefundFee As Decimal

        Dim m_crSettleRefundTax As Object = Nothing

        Dim cCreditSuspenceAmount As Decimal
        Dim cDebitSuspenceAmount As Decimal

        Dim vTempArray As Object = Nothing
        Dim oControlTrans As Object = Nothing
        Dim sOptionVal As String = ""
        Dim iBaseCurrencyID As Integer
        Dim lDocumentTypeID As Integer
        Dim sRangeCode, sDocRef As String
        Dim bRollback As Boolean

        Dim lCreditSuspenceTransDetailID As Long
        Dim lDebitSuspenceTransDetailID As Long
        Dim cTransDetailOSCurrencyAmount As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetClientAccount(r_lAccountId:=m_lAccountID, v_lTransDetail_id:=v_lPlanTransDetailID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetClientAccount", "v_lTransDetail_id=" & v_lPlanTransDetailID, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = SetPlanSourceID(v_lPremFinanceCnt:=v_lPremiumFinanceCnt, v_lPremFinanceVersion:=v_lPremiumFinanceVersion)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetPlanSourceID", "v_lPremFinanceCnt:=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = CreateTransdetail()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CreateTransdetail", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oTransDetail.GetDetails(vTransdetailID:=v_lPlanTransDetailID, vOSAmounts:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oTransDetail.GetDetails", "vTransdetailID:=" & v_lPlanTransDetailID, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oTransDetail.GetNext(vOSBaseAmount:=cTransDetailOSAmount, vOSCurrencyAmount:=cTransDetailOSCurrencyAmount)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oTransDetail.GetNext", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLBeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = True
            End If

            'Get the Currency for Transacting against
            Dim dXRate As Double = 1
            m_lReturn = GetSchemeCurrency(lPremiumFinanceCnt:=v_lPremiumFinanceCnt, lPremiumFinanceVersion:=v_lPremiumFinanceVersion, r_iCurrencyID:=iBaseCurrencyID, r_XRate:=dXRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetSchemeCurrency", "lPremiumFinanceCnt:=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If
            If cTransDetailOSCurrencyAmount <> 0 Then
                m_lReturn = PostInstalmentCredit(v_cCreditAmount:=cTransDetailOSCurrencyAmount, r_lCreditTransDetailId:=lCreditTransDetailId,
                                                 v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, iBaseCurrencyID:=iBaseCurrencyID,
                                                 v_bPostToSuspense:=True, r_lCreditSuspenceTransDetailID:=lCreditSuspenceTransDetailID, r_cCreditSuspenceAmount:=cCreditSuspenceAmount, v_dXRate:=dXRate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("PostInstalmentCredit", "v_lPremiumFinanceCnt:=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
                End If

                r_vCreditTransDetail = lCreditTransDetailId
            End If
            If v_iIsThirdPartyPlan = gPMConstants.PMEReturnCode.PMTrue Then
                m_crSettleAmount = cTransDetailOSAmount

                m_lReturn = GetClientAccount(
                r_lAccountId:=m_lAccountID, v_lPartyCnt:=v_lPartyCnt,
                v_lTransDetail_id:=v_lPlanTransDetailID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception
                End If

            Else
                m_lReturn = SettlePlanCalculate(v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, r_crSettlement:=m_crSettleAmount, r_crRefundFee:=m_crSettleRefundFee, r_vRefundTax:=m_crSettleRefundTax)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("SettlePlanCalculate", "v_lPremiumFinanceCnt:=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            'Have we found some unpaid instalments
            If m_crSettleAmount <> 0 Or m_crSettleRefundFee <> 0 Or m_crSettleRefundTax <> 0 Then

                'If the plan has transactions from several policies on it then debit to be raised is just a journal.
                If v_bPlanHasSinglePolicy Then
                    lDocumentTypeID = gACTLibrary.ACTDocTypeEndorsementDebit
                    sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSed
                    sDocRef = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef17
                Else
                    lDocumentTypeID = gACTLibrary.ACTDocTypeJournal
                    sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeJn
                    sDocRef = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef1
                End If

                m_lReturn = PostInstalmentDebit(v_lAccountId:=m_lAccountID, v_cDebitAmount:=m_crSettleAmount, v_cInterestAmount:=m_crSettleRefundFee * -1, r_lDebitTransdetailId:=lDebitTransDetailId, v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, v_cTaxAmount:=m_crSettleRefundTax * -1, v_lDocumentTypeID:=lDocumentTypeID, v_sRangeCode:=sRangeCode, v_sDocRef:=sDocRef, iBaseCurrencyID:=iBaseCurrencyID, r_lDebitSuspenceTransDetailID:=lDebitSuspenceTransDetailID, r_cDebitSuspenceAmount:=cDebitSuspenceAmount, v_dXRate:=dXRate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("PostInstalmentDebit", "v_lAccountId:=" & m_lAccountID, gPMConstants.PMELogLevel.PMLogError)
                End If

                r_vDebitTransDetail = lDebitTransDetailId
            End If

            ' We need to process dripping of reinsurance and agent commission and taxes...
            ' Before we delete the instlaments.
            m_lReturn = ProcessPFAccountsForCancellation(v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ProcessPFAccountsForCancellation", "v_lPremiumFinanceCnt:=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Delete the unpaid instalments
            m_lReturn = DeleteCancelledInstalments(v_lFinancePlanCnt:=v_lPremiumFinanceCnt, v_lFinancePlanVersion:=v_lPremiumFinanceVersion)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("DeleteCancelledInstalments", "v_lPremiumFinanceCnt:=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Allocate Instalment Credit To Pre-cancellation OS Debit
            ReDim vFinanceTransIds(0)

            vFinanceTransIds(0) = v_lPlanTransDetailID

            ReDim vMatchTransDetailIds(0)

            vMatchTransDetailIds(iCount) = CStr(vFinanceTransIds(0)) & "|" & CStr(cTransDetailOSAmount)
            '5109- Priya


            m_lReturn = Allocate(v_vTransaction:=lCreditTransDetailId & "|" & cTransDetailOSAmount * -1, v_vTransactions:=vMatchTransDetailIds)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Allocate", "v_vTransaction:=" & lCreditTransDetailId & "|" & CStr(cTransDetailOSAmount * -1), gPMConstants.PMELogLevel.PMLogError)
                'If lCreditTransDetailId > 0 Then
                '    m_lReturn = Allocate(v_vTransaction:=lCreditTransDetailId & "|" & cTransDetailOSAmount * -1, v_vTransactions:=vMatchTransDetailIds)
                '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        gPMFunctions.RaiseError("Allocate", "v_vTransaction:=" & lCreditTransDetailId & "|" & CStr(cTransDetailOSAmount * -1), gPMConstants.PMELogLevel.PMLogError)
                '    End If
            End If

            'Allocating suspense line to sed suspense line. Sumit.Kumar PM026505(while cancelling installment plan)
            If lDebitSuspenceTransDetailID <> 0 Then
                ReDim vMatchTransDetailIds(0)

                vMatchTransDetailIds(iCount) = lDebitSuspenceTransDetailID & "|" & cDebitSuspenceAmount

                m_lReturn = Allocate(
                    v_vTransaction:=lCreditSuspenceTransDetailID & "|" & cCreditSuspenceAmount,
                    v_vTransactions:=vMatchTransDetailIds)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("Allocate", "v_vTransaction:=" & lCreditSuspenceTransDetailID & "|" & cCreditSuspenceAmount * -1, gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            'Allocate SUSP INC and SED
            If v_iIsThirdPartyPlan = gPMConstants.PMEReturnCode.PMTrue Then
                ReDim vFinanceTransIds(0)
                vFinanceTransIds(0) = lDebitSuspenceTransDetailID

                ReDim vMatchTransDetailIds(0)
                vMatchTransDetailIds(iCount) = vFinanceTransIds(0) & "|" & cTransDetailOSAmount * -1

                m_lReturn = Allocate(
                    v_vTransaction:=lCreditSuspenceTransDetailID & "|" & cTransDetailOSAmount,
                    v_vTransactions:=vMatchTransDetailIds)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("Allocate", "v_vTransaction:=" & lCreditSuspenceTransDetailID & "|" & cTransDetailOSAmount, gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            m_lReturn = m_oDatabase.SQLCommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = False
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            If bRollback Then
                m_oDatabase.SQLRollbackTrans()
            End If

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here
            If Not (oControlTrans Is Nothing) Then

                oControlTrans.Dispose()
                oControlTrans = Nothing
            End If

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PostAdjustment (Post Adjustment)
    '
    ' Description: Post Adjustment
    '
    '
    ' ***************************************************************** '
    Public Function PostAdjustment(ByRef v_lPlanTransDetailID As Integer, ByVal v_vFinanceTransIds() As Object, ByVal v_cAmount As Decimal, ByVal v_lPremiumFinanceCnt As Integer, ByVal v_lPremiumFinanceVersion As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PostAdjustment"

        Dim lOriginalPlanTransDetailId, lOriginalPlanCreditTransDetailId, lAdjustmentCreditTransDetailId As Integer
        Dim cOSPlanAmount As Decimal
        Dim vMatchTransDetailIds As Object
        Dim cTransDetailOSAmount, cOSAdjustmentAmount As Decimal
        Dim iBaseCurrencyID As Integer
        Dim bRollback As Boolean
        Dim cOSPlanCurrencyAmount As Decimal
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetClientAccount(r_lAccountId:=m_lAccountID, v_lTransDetail_id:=v_lPlanTransDetailID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetClientAccount", "v_lTransDetail_id=" & v_lPlanTransDetailID, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = SetPlanSourceID(v_lPremFinanceCnt:=v_lPremiumFinanceCnt, v_lPremFinanceVersion:=v_lPremiumFinanceVersion)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetPlanSourceID", "v_lPremFinanceCnt:=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = CreateTransdetail()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CreateTransdetail", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oTransDetail.GetDetails(vTransdetailID:=v_lPlanTransDetailID, vOSAmounts:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oTransDetail.GetDetails", "vTransdetailID:=" & v_lPlanTransDetailID, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oTransDetail.GetNext(vSpare:=m_sInstalmentPlan, vOSBaseAmount:=cOSPlanAmount, vOSCurrencyAmount:=cOSPlanCurrencyAmount)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oTransDetail.GetNext", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lOriginalPlanTransDetailId = v_lPlanTransDetailID
            Dim dXRate As Double = 1
            'Get the Currency for Transacting against
            m_lReturn = GetSchemeCurrency(lPremiumFinanceCnt:=v_lPremiumFinanceCnt, lPremiumFinanceVersion:=v_lPremiumFinanceVersion, r_iCurrencyID:=iBaseCurrencyID, r_XRate:=dXRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetSchemeCurrency", "lPremiumFinanceCnt" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If
            If dXRate = 0 Then
                dXRate = 1
            End If
            m_lReturn = m_oDatabase.SQLBeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = True
            End If

            'Post Instalment Credit for Outstanding  Financed Amount
            m_lReturn = PostInstalmentCredit(v_cCreditAmount:=cOSPlanAmount, r_lCreditTransDetailId:=lOriginalPlanCreditTransDetailId, v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt,
                                             v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, iBaseCurrencyID:=iBaseCurrencyID, v_dXRate:=dXRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("PostInstalmentCredit", "v_lPremiumFinanceCnt:=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            'put the transaction array together for allocation of New MTA Transactions
            ReDim vMatchTransDetailIds(v_vFinanceTransIds.GetUpperBound(0))

            For iCount As Integer = 0 To v_vFinanceTransIds.GetUpperBound(0)

                m_lReturn = m_oTransDetail.GetDetails(vTransdetailID:=v_vFinanceTransIds(iCount), vOSAmounts:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    gPMFunctions.RaiseError("m_oTransDetail.GetDetails", "vTransdetailID:=" & CStr(v_vFinanceTransIds(iCount)), gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = m_oTransDetail.GetNext(vOSCurrencyAmount:=cTransDetailOSAmount)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oTransDetail.GetNext", "Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                cOSAdjustmentAmount += cTransDetailOSAmount

                vMatchTransDetailIds(iCount) = CStr(v_vFinanceTransIds(iCount)) & "|" & CStr(cTransDetailOSAmount)

            Next

            'Post Instalment Credit for Adjustment Amount
            m_lReturn = PostInstalmentCredit(v_cCreditAmount:=cOSAdjustmentAmount, r_lCreditTransDetailId:=lAdjustmentCreditTransDetailId, v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, iBaseCurrencyID:=iBaseCurrencyID, v_dXRate:=dXRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("PostInstalmentCredit", "v_lPremiumFinanceCnt:=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Post Instalment Credit for Adjustment Amount
            m_lReturn = PostInstalmentDebit(v_lAccountId:=m_lAccountID, v_cDebitAmount:=(cOSAdjustmentAmount + cOSPlanAmount), v_cInterestAmount:=0, r_lDebitTransdetailId:=v_lPlanTransDetailID, v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, v_cTaxAmount:=0, iBaseCurrencyID:=iBaseCurrencyID, v_dXRate:=dXRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("PostInstalmentDebit", "v_lAccountId:=" & m_lAccountID, gPMConstants.PMELogLevel.PMLogError)
            End If

            'have to close and start transaction again otherwise allocate won't work
            m_lReturn = m_oDatabase.SQLCommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = False
            End If

            'have to close and start transaction again otherwise allocate won't work
            m_lReturn = m_oDatabase.SQLBeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = True
            End If

            'Allocate The Adjustment to the Adjustment Credit

            m_lReturn = Allocate(v_vTransaction:=lAdjustmentCreditTransDetailId & "|" & cOSAdjustmentAmount * -1, v_vTransactions:=vMatchTransDetailIds)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("v_vTransaction", "v_vTransaction:=" & lAdjustmentCreditTransDetailId & "|" & CStr(cOSAdjustmentAmount * -1), gPMConstants.PMELogLevel.PMLogError)
            End If

            'Allocate the Original Plan with its Credit
            ReDim vMatchTransDetailIds(0)

            vMatchTransDetailIds(0) = CStr(lOriginalPlanTransDetailId) & "|" & CStr(cOSPlanAmount)

            m_lReturn = Allocate(v_vTransaction:=lOriginalPlanCreditTransDetailId & "|" & cOSPlanAmount * -1, v_vTransactions:=vMatchTransDetailIds)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("v_vTransaction", "v_vTransaction:=" & lOriginalPlanCreditTransDetailId & "|" & CStr(cOSPlanAmount * -1), gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLCommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = False
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            If bRollback Then
                m_oDatabase.SQLRollbackTrans()
            End If

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name : PostUWAdjustment
    '
    ' Desc : post adjustment underwriting version
    '
    ' v_cosamount = outstanding amount
    ' v_cAdjustment = prem + ipt
    ' v_cFee = fee amount (interest + whatever)
    ' v_vFinanceTrans = adjustment premium and ipt
    '
    ' Hist : Thinh Nguyen 22/02/2002
    ' ***************************************************************** '
    Public Function PostUWAdjustment(ByRef r_lNewPlanTransID As Integer, ByVal v_lPlanTransDetailID As Integer, ByVal v_vFinanceTrans As Object, ByVal v_cAdjustment As Decimal, ByVal v_cFee As Decimal, ByVal v_cOSAmount As Decimal, ByVal v_lPremiumFinanceCnt As Integer, ByVal v_lPremiumFinanceVersion As Integer, Optional ByVal v_lLeadAgentCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PostUWAdjustment"

        Dim vMatchTransDetailIds As Object = Nothing
        Dim lDebitTransDetailId, lAdjustmentCreditTransDetailId, lCreditTransDetailId, lAccountId As Integer
        Dim iBaseCurrencyID As Integer
        Dim bRollback As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetClientAccount(r_lAccountId:=m_lAccountID, v_lTransDetail_id:=v_lPlanTransDetailID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetClientAccount", "v_lTransDetail_id=" & v_lPlanTransDetailID, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = SetPlanSourceID(v_lPremFinanceCnt:=v_lPremiumFinanceCnt, v_lPremFinanceVersion:=v_lPremiumFinanceVersion)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetPlanSourceID", "v_lPremFinanceCnt:=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If
            Dim dXRate As Double = 1
            ' Get the Currency for Transacting against
            m_lReturn = GetSchemeCurrency(lPremiumFinanceCnt:=v_lPremiumFinanceCnt, lPremiumFinanceVersion:=v_lPremiumFinanceVersion, r_iCurrencyID:=iBaseCurrencyID, r_XRate:=dXRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetSchemeCurrency", "lPremiumFinanceCnt:=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLBeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = True
            End If

            'post negative outstanding amount to client and positive to suspense
            m_lReturn = PostInstalmentCredit(v_cCreditAmount:=v_cOSAmount, r_lCreditTransDetailId:=lCreditTransDetailId, v_bPostToSuspense:=True, v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, iBaseCurrencyID:=iBaseCurrencyID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("PostInstalmentCredit", "v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt", gPMConstants.PMELogLevel.PMLogError)
            End If

            'do we have lead agent
            If v_lLeadAgentCnt <> 0 Then

                'keep track current account id
                lAccountId = m_lAccountID

                'Get agent account id
                m_lReturn = GetClientAccount(r_lAccountId:=m_lAccountID, v_lPartyCnt:=v_lLeadAgentCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetClientAccount", "v_lPartyCnt:=v_lLeadAgentCnt", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            'post positive adjustment to suspense and negative to agent if we have lead agent
            m_lReturn = PostInstalmentCredit(v_cCreditAmount:=v_cAdjustment, r_lCreditTransDetailId:=lAdjustmentCreditTransDetailId, v_bPostToSuspense:=True, v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, iBaseCurrencyID:=iBaseCurrencyID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("PostInstalmentCredit", "v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt", gPMConstants.PMELogLevel.PMLogError)
            End If

            'do we have lead agent
            If v_lLeadAgentCnt <> 0 Then
                'put the original account id back
                m_lAccountID = lAccountId
            End If

            'post positive (outstanding + adjustment + fee) to client
            'post negative (outstanding + adjustment + fee) to suspense
            'post negative fee to fee account
            m_lReturn = PostInstalmentDebit(v_lAccountId:=m_lAccountID, v_cDebitAmount:=(v_cOSAmount + v_cAdjustment + v_cFee), v_cInterestAmount:=v_cFee, r_lDebitTransdetailId:=lDebitTransDetailId, v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, v_cTaxAmount:=0, iBaseCurrencyID:=iBaseCurrencyID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("PostInstalmentDebit", "v_lAccountId:=m_lAccountId", gPMConstants.PMELogLevel.PMLogError)
            End If

            'get back new plan trans id
            r_lNewPlanTransID = lDebitTransDetailId

            'have to close and start transaction again otherwise allocate won't work
            m_lReturn = m_oDatabase.SQLCommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = False
            End If

            'have to close and start transaction again otherwise allocate won't work
            m_lReturn = m_oDatabase.SQLBeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = True
            End If

            'allocate adjustment

            m_lReturn = Allocate(v_vTransaction:=lAdjustmentCreditTransDetailId & "|" & v_cAdjustment * -1, v_vTransactions:=v_vFinanceTrans)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Allocate", "v_vTransaction:=" & lAdjustmentCreditTransDetailId & "|" & CStr(v_cAdjustment * -1), gPMConstants.PMELogLevel.PMLogError)
            End If

            'allocate original plan
            vMatchTransDetailIds(0) = CStr(v_lPlanTransDetailID) & "|" & CStr(v_cOSAmount)
            m_lReturn = Allocate(v_vTransaction:=lCreditTransDetailId & "|" & v_cOSAmount * -1, v_vTransactions:=vMatchTransDetailIds)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Allocate", "v_vTransaction:=" & lCreditTransDetailId & "|" & CStr(v_cOSAmount * -1), gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLCommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = False
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            If bRollback Then
                m_oDatabase.SQLRollbackTrans()
            End If

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    '************************************************************************************
    ' Name : GetPlanOS
    '
    ' Desc : get outstanding amount on plan
    '
    ' Hist : Thinh Nguyen 05/03/2002 (created)
    '************************************************************************************
    Public Function GetPlanOS(ByVal v_lPlanTransDetailID As Integer, ByRef r_cOS As Decimal) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CreateTransdetail()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oTransDetail.GetDetails(vTransdetailID:=v_lPlanTransDetailID, vOSAmounts:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return m_oTransDetail.GetNext(vSpare:=m_sInstalmentPlan, vOSCurrencyAmount:=r_cOS)

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPlanOS Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPlanOS", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PostInstalment (Post Instalment)
    '
    ' Description: Post Instalment
    '
    '
    ' ***************************************************************** '
    Public Function PostInstalment(ByVal v_lPlanTransDetailID As Integer,
                ByVal v_cAmount As Decimal, ByVal v_bPostAsCash As Boolean,
                ByRef v_lInstalmentTransdetailID As Integer,
                ByVal v_lInstalmentID As Integer,
                Optional ByVal v_lCashDrawerID As Integer = 0,
                Optional ByVal v_bPartialPayment As Boolean = False,
                Optional ByVal v_cPartialAmount As Decimal = 0,
                Optional ByVal v_bWriteOff As Boolean = False,
                Optional ByVal v_dtTransactionDate As Date = #12/30/1899#,
                Optional ByVal v_lInstallmentNo As Integer = 0,
                Optional ByVal v_sSpare As String = "",
                Optional ByVal v_iCashListCurrencyID As Integer = 0,
                Optional ByVal sWriteOffReason As String = "",
                Optional ByRef nWriteoffTransDetailId As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PostInstalment"




        Dim vMatchTransDetailIds As Object
        Dim lCashListItemID, lPremiumFinanceCnt, lPremiumFinanceVersion As Integer
        Dim bIsLastInstalment As Boolean
        Dim nSpreadRI As Integer
        Dim lInsuranceFileCnt As Integer
        Dim sInsuranceRef As String = ""
        Dim iBaseCurrencyID As Integer
        Dim lAllocationId As Integer
        Dim sCreditControlValue As String = ""
        Dim bCreditControlEnabled As Boolean
        Dim lCreditControlItemID As Integer
        Dim cTaxAmount As Decimal
        Dim bRollback As Boolean
        Dim lTotalInstalment As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetPFFromInstalmentsID(v_lInstalmentsID:=v_lInstalmentID, r_lPremiumFinanceCnt:=lPremiumFinanceCnt, r_lPremiumFinanceVersion:=lPremiumFinanceVersion, r_nSpreadCommission:=m_nSpreadCommission, r_nSpreadRI:=nSpreadRI, r_cTaxAmount:=cTaxAmount, r_lTotalInstalment:=lTotalInstalment)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetPFFromInstalmentsID", "v_lInstalmentsID:=" & v_lInstalmentID, gPMConstants.PMELogLevel.PMLogError)
            End If
            If lTotalInstalment = v_lInstallmentNo Then
                bIsLastInstalment = True
            Else
                bIsLastInstalment = False
            End If
            m_lReturn = GetInsuranceFileCntFromPF(lPremFinanceCnt:=lPremiumFinanceCnt, lPremFinanceVersion:=lPremiumFinanceVersion, r_lInsuranceFileCnt:=lInsuranceFileCnt, r_sInsuranceRef:=sInsuranceRef)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetPFFromInstalmentsID", "v_lInstalmentsID:=" & v_lInstalmentID, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Derive the corresponding credit control item record, if one exists
            'First check if the credit control system option is set
            m_lReturn = GetSystemOptionValue(kSystemOptionCreditControlEnabled, sCreditControlValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetSystemOptionValue", "kSystemOptionCreditControlEnabled", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Only proceed with credit control insert if system option has been set
            If String.Compare(sCreditControlValue, ACValueWhenCreditControlSet) = 0 Then
                bCreditControlEnabled = True

                'call a method to return the CCI ID
                m_lReturn = GetCreditControlItemID(lPFInstalmentsID:=v_lInstalmentID, r_lCCIID:=lCreditControlItemID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetCreditControlItemID", "lPFInstalmentsID:=" & v_lInstalmentID, gPMConstants.PMELogLevel.PMLogError)
                End If
            Else
                bCreditControlEnabled = False
            End If

            m_lReturn = GetClientAccount(r_lAccountId:=m_lAccountID, v_lTransDetail_id:=v_lPlanTransDetailID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetClientAccount", "v_lTransDetail_id:=" & v_lPlanTransDetailID, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = SetPlanSourceID(v_lPremFinanceCnt:=lPremiumFinanceCnt, v_lPremFinanceVersion:=lPremiumFinanceVersion)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetPlanSourceID", "v_lPremFinanceCnt:=" & lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get the Currency for Transacting against (change here - pk)
            Dim dXRate As Double = 1

            m_lReturn = GetSchemeCurrency(lPremiumFinanceCnt:=lPremiumFinanceCnt, lPremiumFinanceVersion:=lPremiumFinanceVersion, r_iCurrencyID:=iBaseCurrencyID, r_XRate:=dXRate)

            If v_iCashListCurrencyID > 0 Then
                m_lReturn = m_oCurrencyConvert.GetCurrencyRate(v_lCurrencyID:=v_iCashListCurrencyID, v_lCompanyId:=m_lCompanyID, v_dtConversionDate:=DateTime.Now, r_vConversionRate:=dXRate)

                If dXRate = 0 Then
                    dXRate = 1
                End If

                iBaseCurrencyID = v_iCashListCurrencyID
            End If



            m_lReturn = GetCashListItem(v_lInstalmentID:=v_lInstalmentID, r_lCashListItemID:=lCashListItemID, r_lCashTransactionID:=v_lInstalmentTransdetailID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetCashListItem", "lPremiumFinanceCnt:=" & lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            'If m_lBankID = 0 Then  
            m_lReturn = GetSchemeAccount(v_lPremiumFinanceCnt:=lPremiumFinanceCnt, v_lPremiumFinanceVersion:=lPremiumFinanceVersion, v_sWhichAccount:="BANK", v_lCashListItemId:=lCashListItemID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetSchemeAccount", "v_sWhichAccount:=BANK", gPMConstants.PMELogLevel.PMLogError)
            End If
            'End If

            m_lReturn = m_oDatabase.SQLBeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = True
            End If

            If Not v_bPostAsCash Then
                'v_sSpare = 1
                m_lReturn = PostInstalmentCredit(v_cCreditAmount:=v_cAmount, r_lCreditTransDetailId:=v_lInstalmentTransdetailID,
                                                 v_lPremiumFinanceCnt:=lPremiumFinanceCnt, v_lPremiumFinanceVersion:=lPremiumFinanceVersion,
                                                 v_lCashDrawerID:=v_lCashDrawerID, v_cPartialAmount:=v_cPartialAmount, v_bWriteOff:=v_bWriteOff,
                                                 iBaseCurrencyID:=iBaseCurrencyID, v_cTaxAmount:=cTaxAmount, v_dtTransactionDate:=v_dtTransactionDate, v_dXRate:=dXRate, v_sSpare:=v_sSpare, r_nWriteOffTransDetailId:=nWriteoffTransDetailId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("PostInstalmentCredit", "v_lPremiumFinanceCnt:=" & lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
                End If
            Else
                m_lReturn = PostInstalmentCash(v_cCreditAmount:=v_cAmount, r_lCreditTransDetailId:=v_lInstalmentTransdetailID, iBaseCurrencyID:=iBaseCurrencyID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("PostInstalmentCash", "v_cCreditAmount:=" & v_cAmount, gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            'Remove item from credit control
            If bCreditControlEnabled Then
                If lCreditControlItemID > 0 Then
                    m_lReturn = RemoveCreditControlItem(v_iCreditControlItemID:=lCreditControlItemID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("RemoveCreditControlItem", "v_iCreditControlItemID:=" & lCreditControlItemID, gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            End If

            'have to close and start transaction again otherwise allocate won't work
            m_lReturn = m_oDatabase.SQLCommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = False
            End If

            'have to close and start transaction again otherwise allocate won't work
            m_lReturn = m_oDatabase.SQLBeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = True
            End If

            'Change the amount to the correct client amount used in the transaction.
            If v_cPartialAmount < v_cAmount And v_cPartialAmount <> 0 And Not v_bWriteOff Then
                v_cAmount = v_cPartialAmount
            End If

            'Allocate Instalment Credit/Pre-cancellation OS Debit
            If Not v_bWriteOff Then
                ReDim vMatchTransDetailIds(0)

                vMatchTransDetailIds(0) = CStr(v_lPlanTransDetailID) & "|" & CStr(Math.Round(v_cAmount * dXRate, 2))

                m_sEarnCommissionOnPartPayments = "0"

                m_lReturn = Allocate(v_vTransaction:=v_lInstalmentTransdetailID & "|" & Math.Round(v_cAmount * dXRate * -1, 2), v_vTransactions:=vMatchTransDetailIds, v_vAllocationId:=lAllocationId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("Allocate", "v_vTransaction:=" & v_lInstalmentTransdetailID & "|" & CStr(v_cAmount * -1), gPMConstants.PMELogLevel.PMLogError)
                End If
            Else
                If v_cPartialAmount <> 0 And v_cAmount <> v_cPartialAmount And v_bWriteOff Then
                    ReDim vMatchTransDetailIds(1)
                    vMatchTransDetailIds(0) = v_lInstalmentTransdetailID & "|" & CStr(Math.Round(v_cPartialAmount * dXRate, 2) * -1)
                    vMatchTransDetailIds(1) = nWriteoffTransDetailId & "|" & CStr((Math.Round(v_cAmount * dXRate, 2) - Math.Round(v_cPartialAmount * dXRate, 2)) * -1)
                Else
                    ReDim vMatchTransDetailIds(0)
                    vMatchTransDetailIds(0) = v_lInstalmentTransdetailID & "|" & CStr(Math.Round(v_cAmount * dXRate, 2) * -1)
                End If
                m_sEarnCommissionOnPartPayments = "0"


                m_lReturn = GetSystemOptionValue(
                    v_iOptionNumber:=ACCommissionMovementOptionNo,
                    r_sValue:=m_sCommissionOption)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetSystemOptionValue", "v_iOptionNumber:=ACCommissionMovementOptionNo", gPMConstants.PMELogLevel.PMLogError)
                End If

                If m_sCommissionOption = ClientPayment Then
                    m_lReturn = GetSystemOptionValue(
                        v_iOptionNumber:=ACEarnCommissionOnPartPayments,
                        r_sValue:=m_sEarnCommissionOnPartPayments)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("GetSystemOptionValue", "v_iOptionNumber:=ACEarnCommissionOnPartPayments", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If


                m_lReturn = Allocate(
                    v_vTransaction:=v_lPlanTransDetailID & "|" & CStr(Math.Round(v_cAmount * dXRate, 2)),
                    v_vTransactions:=vMatchTransDetailIds,
                    v_vAllocationId:=lAllocationId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("Allocate", "v_vTransaction:=" & v_lPlanTransDetailID & "|" & v_cAmount, gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            m_lReturn = ProcessSuspendedTransactions(v_lAllocationId:=lAllocationId, v_lPremiumFinanceCnt:=lPremiumFinanceCnt, v_lPremiumFinanceVersion:=lPremiumFinanceVersion, v_lInstalmentID:=v_lInstalmentID, v_lInsuranceFileCnt:=lInsuranceFileCnt, v_sInsuranceRef:=sInsuranceRef, v_bPartialPayment:=v_bPartialPayment, v_bLastInstalment:=bIsLastInstalment)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ProcessSuspendedTransactions", "v_lAllocationId:=" & lAllocationId, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLCommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = False
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            If bRollback Then
                m_oDatabase.SQLRollbackTrans()
            End If

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: RecallInstalment (RecallInstalment)
    '
    ' Description: RecallInstalment
    '
    ' Thinh Nguyen 30/07/2003 - Danny Davis suggested calling bActDocumentReversal to do the work
    '                    that way we don't need to know whether to reverse to bank or susp account
    ' ***************************************************************** '
    Public Function RecallInstalment(ByVal v_lPlanTransDetailID As Integer, ByVal v_lInstalmentTransdetailID As Integer) As Integer

        Dim result As Integer = 0
        Dim oObject As bACTDocumentReversal.Business = Nothing
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Developer Guide No 267

            oObject = New bACTDocumentReversal.Business
            m_lReturn = oObject.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get an instance of bActDocumentReversal.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="RecallInstalment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oObject.TransDetailId = v_lInstalmentTransdetailID

            m_lReturn = oObject.Start()

            ReversalTransDetailId = gPMFunctions.ToSafeLong(oObject.ReversalTransDetailId, 0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oObject.Dispose()
                oObject = Nothing

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to reverse transaction for transdetail_id " & v_lInstalmentTransdetailID, vApp:=ACApp, vClass:=ACClass, vMethod:="RecallInstalment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception

            If Not (oObject Is Nothing) Then

                oObject.Dispose()
                oObject = Nothing
            End If

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RecallInstalment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RecallInstalment", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' RecallInstalment (RecallInstalment)
    ''' </summary>
    ''' <param name="v_lPlanTransDetailID"></param>
    ''' <param name="v_lInstalmentTransdetailID"></param>
    ''' <param name="r_bRecallInstalmentFromInstalmentMaint"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function RecallInstalment(ByVal v_lPlanTransDetailID As Integer, ByVal v_lInstalmentTransdetailID As Integer,
                                     ByVal r_bRecallInstalmentFromInstalmentMaint As Boolean) As Integer

        Dim nResult As Integer = 0
        Dim oObject As Object = Nothing
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Developer Guide No 267
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oObject,
                                                                  v_sClassName:="bACTDocumentReversal.Business",
                                                                  v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername,
                                                                  v_sPassword:=m_sPassword, v_iUserID:=m_iUserID,
                                                                  v_iSourceID:=m_iSourceID,
                                                                  v_iLanguageID:=m_iLanguageID,
                                                                  v_iCurrencyID:=m_iCurrencyID,
                                                                  v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="Failed to get an instance of bActDocumentReversal.Business", vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="RecallInstalment", vErrNo:=Informations.Err().Number,
                                   vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oObject.TransdetailID = v_lInstalmentTransdetailID
            oObject.IsRecallInstalmentFromInstalmentMaint = r_bRecallInstalmentFromInstalmentMaint

            m_lReturn = oObject.Start()

            ReversalTransDetailId = gPMFunctions.ToSafeLong(oObject.ReversalTransDetailId, 0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = oObject.Terminate()
                oObject = Nothing

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:=
                                      "Failed to reverse transaction for transdetail_id " & v_lInstalmentTransdetailID,
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="RecallInstalment",
                                   vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return nResult

        Catch excep As System.Exception

            If Not (oObject Is Nothing) Then

                m_lReturn = oObject.Terminate()
                oObject = Nothing
            End If

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="RecallInstalment Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:="RecallInstalment", vErrNo:=Informations.Err().Number,
                               vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PostDeposit (PostDeposit)
    '
    ' Description: Post Transaction for Deposit Value
    '
    ' ***************************************************************** '
    Private Function PostDeposit(ByVal v_cAmount As Decimal, ByRef v_lTransDetailId As Integer, ByVal v_lAccount_ID As Integer, ByRef r_vMatchTransDetailIds() As Object,
                                 ByVal iBaseCurrencyID As Integer, ByVal nPlanTransDetailID As Integer, Optional ByRef r_vDepositJournal As Object = Nothing,
                                 Optional ByVal v_dXRate As Double = 1,
                                 Optional ByVal nPremiumFinanceCnt As Integer = 0, Optional ByVal nPremiumFinanceVersion As Integer = 0) As Integer

        Dim result As Integer = 0

        Dim iDocSeq As Integer
        Dim lNumberRangeID As Integer
        Dim sDocumentRef As String = ""
        Dim dtDocumentDate As Date
        Dim sComment As String = ""
        Dim nPeriodId As Integer
        Dim sRangeCode As String = String.Empty
        Dim nDocumentTypeID As Integer = 0
        Dim nSchemeSuspenceTransDetailID As Integer = 0
        Dim nClientSuspenseTransDetailId As Integer = 0
        Dim vMatchTransDetailIds As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        dtDocumentDate = DateTime.Now
        m_lReturn = GetCurrentPeriodForLedger(v_nAccountId:=m_lAccountID, r_nPeriodId:=nPeriodId)

        'Get Scheme Account
        If m_lSuspID = 0 Then
            m_lReturn = GetSchemeAccount(v_lPremiumFinanceCnt:=nPremiumFinanceCnt, v_lPremiumFinanceVersion:=nPremiumFinanceVersion, v_sWhichAccount:="SUSP")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_lReturn = GetSchemeAccount", "v_sWhichAccount=SUSP", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        'Get the Insurance File Cnt for the Plan
        m_lReturn = CreateDocumentPost()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("CreateDocumentPost", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = CreateAutoNumber()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("CreateAutoNumber", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
        'Post Deposit into Revenue account and allocate to the Plan amout to reduce the amount

        'Set from the Instalment credit
        If bIsRenewed Then
            sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeIRC
            nDocumentTypeID = gACTLibrary.ACTDocTypeInstalmentRenewalCredit
        Else
            sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeINC
            nDocumentTypeID = gACTLibrary.ACTDocTypeInstalmentNBCredit
        End If

        m_lReturn = m_oAutoNumber.GetNumberRange(v_sGroupCode:=gACTLibrary.ACTAutoNumberGroupCodeDocumentRef43, v_sRangeCode:=sRangeCode, r_lNumberRangeID:=lNumberRangeID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oAutoNumber.GetNumberRange", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Generate the next number
        'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

        m_lReturn = m_oAutoNumber.GenerateDocumentReferenceNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=m_iUserID, v_iCompanyID:=m_lCompanyID, r_sDocumentRef:=sDocumentRef, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeINC)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber
            gPMFunctions.RaiseError("m_oAutoNumber.GenerateDocumentReferenceNumber", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        'Format the number
        If bIsRenewed Then
            sDocumentRef = gACTLibrary.ACTAutoNumberRangeCodeIRC & sDocumentRef
        Else
            sDocumentRef = gACTLibrary.ACTAutoNumberRangeCodeINC & sDocumentRef
        End If


        m_lReturn = m_oDocumentPost.AddDocument(v_lDocumentTypeId:=nDocumentTypeID, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=dtDocumentDate, v_sComment:="Instalment Plan Deposit " & m_sInstalmentPlan,
                                                r_vDocSourceID:=m_lCompanyID, r_vSubBranchID:=m_lSubBranchID, v_lInsuranceFileCnt:=m_lInsuranceFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDocumentPost.AddDocument", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        iDocSeq = 1

        m_oDocumentPost.PostingPeriodNumber = nPeriodId

        'Posting to Revenue account and Client account
        m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=v_lAccount_ID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound((v_cAmount * v_dXRate) * -1, kRoundingUpto, m_bSuppressDecimalValues), v_cCurrencyAmount:=gPMFunctions.ToSafeRound(v_cAmount * -1, kRoundingUpto, m_bSuppressDecimalValues),
                                                    v_vdCurrencyBaseXRate:=v_dXRate, r_vTransDetailId:=v_lTransDetailId, v_vDocumentSequence:=iDocSeq,
                                                    v_vInsuranceRef:=m_vInsuranceRef, v_vComment:="Instalment Plan Deposit ", v_vAccountingDate:=dtDocumentDate)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId:=" & v_lAccount_ID, gPMConstants.PMELogLevel.PMLogError)
        End If


        ReDim vMatchTransDetailIds(0)
        vMatchTransDetailIds(0) = CStr(nPlanTransDetailID) & "|" & CStr(v_cAmount * v_dXRate)

        'allocate Deposit INC with the Plan Amount
        If v_cAmount <> 0 Then
            m_bAllocateDepositToPlan = True
            m_lReturn = Allocate(v_vTransaction:=v_lTransDetailId & "|" & (v_cAmount * v_dXRate) * -1, v_vTransactions:=vMatchTransDetailIds)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Allocate", "v_vTransaction:=" & nPlanTransDetailID & "|" & CStr(v_cAmount * v_dXRate * -1), gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        iDocSeq += 1
        m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=m_lSuspID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound(v_cAmount * v_dXRate, kRoundingUpto, m_bSuppressDecimalValues), v_cCurrencyAmount:=gPMFunctions.ToSafeRound(v_cAmount, kRoundingUpto, m_bSuppressDecimalValues),
                                                 v_vdCurrencyBaseXRate:=v_dXRate, r_vTransDetailId:=nSchemeSuspenceTransDetailID,
                                                 v_vDocumentSequence:=iDocSeq, v_vInsuranceRef:=m_vInsuranceRef, v_vAccountingDate:=dtDocumentDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId:=" & m_lSuspID, gPMConstants.PMELogLevel.PMLogError)

        End If


        ' End Deposit INC Logic

        'Get the number range

        m_lReturn = m_oAutoNumber.GetNumberRange(v_sGroupCode:=gACTLibrary.ACTAutoNumberGroupCodeDocumentRef1, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn, r_lNumberRangeID:=lNumberRangeID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oAutoNumber.GetNumberRange", "v_sRangeCode:=" & gACTLibrary.ACTAutoNumberRangeCodeJn, gPMConstants.PMELogLevel.PMLogError)
        End If

        'Generate the next number
        'Start (Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

        m_lReturn = m_oAutoNumber.GenerateDocumentReferenceNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=m_iUserID, v_iCompanyID:=m_lCompanyID, r_sDocumentRef:=sDocumentRef, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn)
        'End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber
            gPMFunctions.RaiseError("m_oAutoNumber.GenerateDocumentReferenceNumber", "v_lNumberRangeID:=" & lNumberRangeID, gPMConstants.PMELogLevel.PMLogError)
        End If

        'Format the number
        '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        sDocumentRef = gACTLibrary.ACTAutoNumberRangeCodeJn & sDocumentRef

        'Generate document

        m_lReturn = m_oDocumentPost.AddDocument(v_lDocumentTypeId:=gACTLibrary.ACTDocTypeJournal, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=dtDocumentDate, v_sComment:="Instalment Plan Deposit " & m_sInstalmentPlan, r_vDocSourceID:=m_lCompanyID, r_vSubBranchID:=m_lSubBranchID, v_lInsuranceFileCnt:=m_lInsuranceFileCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDocumentPost.AddDocument", "v_sDocumentRef:=" & sDocumentRef, gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=v_lAccount_ID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound(v_cAmount * v_dXRate, kRoundingUpto, m_bSuppressDecimalValues), v_cCurrencyAmount:=gPMFunctions.ToSafeRound(v_cAmount, kRoundingUpto, m_bSuppressDecimalValues),
                                                    v_vdCurrencyBaseXRate:=v_dXRate, r_vTransDetailId:=v_lTransDetailId, v_vDocumentSequence:=1,
                                                    v_vInsuranceRef:=m_vInsuranceRef, v_vComment:="Deposit", v_vAccountingDate:=dtDocumentDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId:=" & v_lAccount_ID, gPMConstants.PMELogLevel.PMLogError)
        End If
        'Assign the Deposit transaction for the allocation if Cash receipt get process.
        r_vDepositJournal = CStr(v_lTransDetailId) & "|" & CStr(v_cAmount * v_dXRate)

        m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=m_lSuspID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound((v_cAmount * v_dXRate) * -1, kRoundingUpto, m_bSuppressDecimalValues),
                                                   v_cCurrencyAmount:=gPMFunctions.ToSafeRound(v_cAmount * -1, kRoundingUpto, m_bSuppressDecimalValues), v_vdCurrencyBaseXRate:=v_dXRate, r_vTransDetailId:=v_lTransDetailId,
                                                   v_vDocumentSequence:=2, v_vInsuranceRef:=m_vInsuranceRef, v_vComment:="Instalment Plan Deposit", v_vAccountingDate:=dtDocumentDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId:=" & v_lAccount_ID, gPMConstants.PMELogLevel.PMLogError)
        End If

        ReDim vMatchTransDetailIds(0)
        vMatchTransDetailIds(0) = CStr(nSchemeSuspenceTransDetailID) & "|" & CStr(v_cAmount * v_dXRate)


        'allocate Deposit INC to the JN for deposit INC
        If v_lTransDetailId <> 0 Then
            m_lReturn = Allocate(v_vTransaction:=v_lTransDetailId & "|" & (v_cAmount * v_dXRate) * -1, v_vTransactions:=vMatchTransDetailIds)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Allocate", "v_vTransaction:=" & nSchemeSuspenceTransDetailID & "|" & CStr(v_cAmount * v_dXRate * -1), gPMConstants.PMELogLevel.PMLogError)
            End If
        End If
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PostInstalmentCredit (PostInstalmentCredit)
    '
    ' Description: Post Instalment Credit For Financed Amount
    '
    ' ***************************************************************** '
    Private Function PostInstalmentCredit(ByRef v_cCreditAmount As Decimal,
                                          ByRef r_lCreditTransDetailId As Integer,
                                          ByVal v_lPremiumFinanceCnt As Integer,
                                          ByVal v_lPremiumFinanceVersion As Integer,
                                          ByVal iBaseCurrencyID As Integer,
                                          Optional ByVal v_lInsuranceFileCnt As Integer = 0,
                                          Optional ByVal v_bPostToSuspense As Object = Nothing,
                                          Optional ByVal v_lAgentCnt As Integer = 0,
                                          Optional ByVal v_bNewPlan As Object = Nothing,
                                          Optional ByVal v_cFeeAmount As Decimal = 0,
                                          Optional ByVal v_cTaxAmount As Decimal = 0,
                                          Optional ByVal v_lCashDrawerID As Integer = 0,
                                          Optional ByVal v_bCommissionSpread As Boolean = False,
                                          Optional ByVal v_bCreateAccountingTransactions As Boolean = False,
                                          Optional ByVal v_cPartialAmount As Decimal = 0,
                                          Optional ByVal v_bWriteOff As Boolean = False,
                                          Optional ByRef r_lCreditSuspenceTransDetailID As Integer = 0,
                                          Optional ByRef r_cCreditSuspenceAmount As Decimal = 0,
                                          Optional ByVal v_dtTransactionDate As Date = #12/30/1899#,
                                          Optional ByVal v_dXRate As Double = 1, Optional ByVal v_sSpare As String = "",
                                          Optional ByVal sWriteOffReason As String = "",
                                          Optional ByRef r_nWriteOffTransDetailId As Integer = 0,
                                          Optional ByVal bSubAgentCommissionSpread As Boolean = False,
                                          Optional ByVal oSubAgent As Object = Nothing) As Integer

        Dim result As Integer = 0

        Dim lNumberRangeID As Integer
        Dim iDocSeq As Integer
        Dim sDocumentRef As String = ""
        Dim dtDocumentDate As Date
        Dim sComment As String = ""
        Dim cSuspenseAmount As Decimal
        Dim lInsuranceFileCnt As Integer
        Dim sInsuranceRef As String = ""
        Dim lCDCollectionAccountID As Integer
        Dim lGainsLossAccountID, lTaxAccountID As Integer
        Dim cThisTaxAmount As Decimal
        Dim vTaxArray(,) As Object = Nothing
        Dim sTaxAccountCode As String = ""
        Dim bIsRenewed As Boolean
        Dim lDocumentTypeID As Integer
        Dim sRangeCode As String = ""
        Dim dClientPostAmount As Decimal
        Dim oValue As Object = Nothing
        Dim dtAccountingDate As Date
        Dim nPeriodId As Integer
        Dim nWriteOffAccountID As Long


        result = gPMConstants.PMEReturnCode.PMTrue

        If v_dXRate = 0 Then
            v_dXRate = 1
        End If

        'Get the Insurance File Cnt for the Plan
        m_lReturn = GetInsuranceFileCntFromPF(lPremFinanceCnt:=v_lPremiumFinanceCnt,
                                              lPremFinanceVersion:=v_lPremiumFinanceVersion,
                                              r_lInsuranceFileCnt:=lInsuranceFileCnt,
                                              r_sInsuranceRef:=sInsuranceRef,
                                              r_bIsRenewed:=bIsRenewed,
                                              r_dtCoverStartDate:=dtAccountingDate)


        If Trim(m_vInsuranceRef) = "" Then
            m_vInsuranceRef = sInsuranceRef
            m_lInsuranceFileCnt = lInsuranceFileCnt
        End If

        m_vInsuranceRef = sInsuranceRef

        m_lReturn = GetPeriodFromInsuranceFile(v_lInsuranceFileCnt:=lInsuranceFileCnt, nPeriodID:=nPeriodId)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("GetPeriodFromInsuranceFile", "v_lInsuranceFileCnt:=" & lInsuranceFileCnt, gPMConstants.PMELogLevel.PMLogError)
        End If

        If v_dtTransactionDate <> CDate(#12/30/1899#) Then
            dtDocumentDate = v_dtTransactionDate
            dtAccountingDate = v_dtTransactionDate
        Else
            dtDocumentDate = DateTime.Now
            If nPeriodId = 0 Then
                m_lReturn = GetCurrentPeriodForLedger(v_nAccountId:=m_lAccountID, r_nPeriodId:=nPeriodId)
            End If
            dtAccountingDate = If(dtAccountingDate > dtDocumentDate, dtAccountingDate, dtDocumentDate)
        End If

        'Get the Insurance File Cnt for the Plan
        m_lReturn = GetInsuranceFileCntFromPF(lPremFinanceCnt:=v_lPremiumFinanceCnt,
                                              lPremFinanceVersion:=v_lPremiumFinanceVersion,
                                                  r_lInsuranceFileCnt:=lInsuranceFileCnt,
                                              r_sInsuranceRef:=sInsuranceRef,
                                              r_bIsRenewed:=bIsRenewed)
        m_lReturn = CreateDocumentPost()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("CreateDocumentPost", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = CreateAutoNumber()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("CreateAutoNumber", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        'ADO #39472: Check if this is a claim recovery plan (SR/TPR) — use ICC
        Dim sPlanTransType As String = ""
        m_lReturn = GetPlanTransType(v_lPremFinanceCnt:=v_lPremiumFinanceCnt, v_lPremFinanceVersion:=v_lPremiumFinanceVersion, r_sTransType:=sPlanTransType)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("GetPlanTransType", "v_lPremiumFinanceCnt:=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
        End If

        If sPlanTransType = "SR" OrElse sPlanTransType = "TPR" Then
            sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeICC
            lDocumentTypeID = gACTLibrary.ACTDocTypeInstalmentClaimCredit
        ElseIf bIsRenewed Then
            sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeIRC
            lDocumentTypeID = gACTLibrary.ACTDocTypeInstalmentRenewalCredit
        Else
            sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeINC
            lDocumentTypeID = gACTLibrary.ACTDocTypeInstalmentNBCredit
        End If

        'ADO #39472: Use correct group code based on range code
        Dim sGroupCode As String = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef43
        If sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeICC Then
            sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef60
        End If

        m_lReturn = m_oAutoNumber.GetNumberRange(v_sGroupCode:=sGroupCode, v_sRangeCode:=sRangeCode, r_lNumberRangeID:=lNumberRangeID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oAutoNumber.GetNumberRange", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Generate the next number
        'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

        m_lReturn = m_oAutoNumber.GenerateDocumentReferenceNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=m_iUserID, v_iCompanyID:=m_lCompanyID, r_sDocumentRef:=sDocumentRef, v_sRangeCode:=sRangeCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber
            gPMFunctions.RaiseError("m_oAutoNumber.GenerateDocumentReferenceNumber", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        'Format the number
        sDocumentRef = sRangeCode & sDocumentRef

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("GetInsuranceFileCntFromPF", "lPremFinanceCnt:=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = m_oDocumentPost.AddDocument(v_lDocumentTypeId:=lDocumentTypeID, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=dtDocumentDate, v_sComment:="Instalment Plan " & m_sInstalmentPlan, r_vDocSourceID:=m_lCompanyID, r_vSubBranchID:=m_lSubBranchID, v_lInsuranceFileCnt:=lInsuranceFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDocumentPost.AddDocument", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        iDocSeq = 1

        If v_cPartialAmount < v_cCreditAmount And v_cPartialAmount <> 0 And Not v_bWriteOff Then
            v_cCreditAmount = v_cPartialAmount
        End If

        m_oDocumentPost.PostingPeriodNumber = nPeriodId
        m_lReturn = GetOutstandingTaxAmount(m_lDocumentId, m_lAccountID, m_dOutstandingTaxAmount, m_lTaxTransDetailId, m_dOutstandingTaxCurrencyAmount)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            gPMFunctions.RaiseError("GetOutstandingTaxAmount", "lDocumentId", gPMConstants.PMELogLevel.PMLogError)
        End If

        If v_dXRate = 1 Then
            v_cCreditAmount += m_dOutstandingTaxAmount
        Else
            v_cCreditAmount += m_dOutstandingTaxCurrencyAmount
        End If


        If oValue = "1" And v_cPartialAmount <> 0 Then
            dClientPostAmount = v_cPartialAmount * -1
        Else
            dClientPostAmount = v_cCreditAmount * -1
        End If
        m_oDocumentPost.PostingPeriodNumber = nPeriodId
        m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=m_lAccountID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound((If(v_bWriteOff, v_cPartialAmount, v_cCreditAmount) * v_dXRate) * -1, kRoundingUpto, m_bSuppressDecimalValues), v_cCurrencyAmount:=gPMFunctions.ToSafeRound(If(v_bWriteOff, v_cPartialAmount, v_cCreditAmount) * -1, kRoundingUpto, m_bSuppressDecimalValues),
                                                   v_vdCurrencyBaseXRate:=v_dXRate, r_vTransDetailId:=r_lCreditTransDetailId, v_vDocumentSequence:=iDocSeq, v_vInsuranceRef:=sInsuranceRef,
                                                   v_vAccountingDate:=dtAccountingDate, v_vSpare:=v_sSpare, v_vComment:=sWriteOffReason)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId:=" & m_lAccountID, gPMConstants.PMELogLevel.PMLogError)
        End If

        If m_lBankID = 0 Then
            m_lReturn = GetSchemeAccount(v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, v_sWhichAccount:="BANK")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_lReturn = GetSchemeAccount", "v_sWhichAccount=BANK", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        If m_lSuspID = 0 Then
            m_lReturn = GetSchemeAccount(v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, v_sWhichAccount:="SUSP")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_lReturn = GetSchemeAccount", "v_sWhichAccount=SUSP", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        If m_lTaxSuspID = 0 Then
            m_lReturn = GetSchemeAccount(v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, v_sWhichAccount:="TAX")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_lReturn = GetSchemeAccount", "v_sWhichAccount=TAX", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        iDocSeq += 1

        If v_bWriteOff And v_cPartialAmount <> 0 Then
            cSuspenseAmount = v_cPartialAmount
        Else
            cSuspenseAmount = v_cCreditAmount
        End If

        If Not Informations.IsNothing(v_cTaxAmount) Then
            cSuspenseAmount -= v_cTaxAmount
        End If

        If Not Informations.IsNothing(v_cFeeAmount) Then
            cSuspenseAmount -= v_cFeeAmount
        End If

        If Not Informations.IsNothing(v_bPostToSuspense) Then

            r_cCreditSuspenceAmount = cSuspenseAmount
            'Priya
            m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=m_lSuspID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound(cSuspenseAmount * v_dXRate, kRoundingUpto, m_bSuppressDecimalValues), v_cCurrencyAmount:=gPMFunctions.ToSafeRound(cSuspenseAmount, kRoundingUpto, m_bSuppressDecimalValues),
                                                      v_vdCurrencyBaseXRate:=v_dXRate, r_vTransDetailId:=r_lCreditSuspenceTransDetailID,
                                                      v_vDocumentSequence:=iDocSeq, v_vInsuranceRef:=sInsuranceRef, v_vAccountingDate:=dtAccountingDate, v_vSpare:=v_sSpare, v_vComment:=sWriteOffReason)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId:=" & m_lSuspID, gPMConstants.PMELogLevel.PMLogError)

            End If


        Else

            If v_lCashDrawerID <> 0 Then

                'find the collection account ID of the cash drawer
                m_lReturn = GetCollectionAccountID(v_lCashDrawerID:=v_lCashDrawerID, r_lCollectionAccountID:=lCDCollectionAccountID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or lCDCollectionAccountID = 0 Then
                    gPMFunctions.RaiseError("GetCollectionAccountID", "v_lCashDrawerID:=" & v_lCashDrawerID, gPMConstants.PMELogLevel.PMLogError)
                End If

                'post transaction to the cash drawer collection account

                m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=lCDCollectionAccountID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound(cSuspenseAmount * v_dXRate, kRoundingUpto, m_bSuppressDecimalValues), v_cCurrencyAmount:=gPMFunctions.ToSafeRound(cSuspenseAmount, kRoundingUpto, m_bSuppressDecimalValues),
                                                           v_vdCurrencyBaseXRate:=v_dXRate, v_vDocumentSequence:=iDocSeq, v_vAccountingDate:=dtAccountingDate, v_vSpare:=v_sSpare, v_vComment:=sWriteOffReason)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId:=" & lCDCollectionAccountID, gPMConstants.PMELogLevel.PMLogError)
                End If
            Else
                m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=m_lBankID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound(v_cCreditAmount * v_dXRate, kRoundingUpto, m_bSuppressDecimalValues), v_cCurrencyAmount:=gPMFunctions.ToSafeRound(v_cCreditAmount, kRoundingUpto, m_bSuppressDecimalValues),
                                                           v_vdCurrencyBaseXRate:=v_dXRate, v_vDocumentSequence:=iDocSeq, v_vInsuranceRef:=sInsuranceRef, v_vAccountingDate:=dtAccountingDate, v_vSpare:=v_sSpare, v_vComment:=sWriteOffReason)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId:=" & m_lBankID, gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
        End If

        If m_vUnderwriting = "" Or Convert.IsDBNull(m_vUnderwriting) Or Informations.IsNothing(m_vUnderwriting) Then
            m_lReturn = bPMFunc.getUnderwritingType(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_vUnderwriting)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("getUnderwritingType", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        If v_bCreateAccountingTransactions Then
            m_lReturn = CreateAccountingTransaction_SFU(v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, v_lAgentCnt:=v_lAgentCnt, v_bCommissionSpread:=v_bCommissionSpread, v_sInsuranceRef:=sInsuranceRef, v_lInsuranceFileCnt:=lInsuranceFileCnt, r_iDocSeq:=iDocSeq, v_dtAccountingDate:=dtAccountingDate, bSubAgentCommissionSpread:=bSubAgentCommissionSpread, oSubAgent:=oSubAgent)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CreateAccountingTransaction_SFU", "v_lPremiumFinanceCnt=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        If Not Informations.IsNothing(v_cFeeAmount) Then

            If v_cFeeAmount <> 0 Then

                m_lReturn = GetSchemeAccount(v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, v_sWhichAccount:="Fee")
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetSchemeAccount", "v_sWhichAccount=Fee", gPMConstants.PMELogLevel.PMLogError)
                End If

                iDocSeq += 1

                m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=m_lFeeID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound(v_cFeeAmount, kRoundingUpto, m_bSuppressDecimalValues), v_cCurrencyAmount:=gPMFunctions.ToSafeRound(v_cFeeAmount * v_dXRate, kRoundingUpto, m_bSuppressDecimalValues),
                                                           v_vdCurrencyBaseXRate:=v_dXRate, v_vDocumentSequence:=iDocSeq, v_vInsuranceRef:=sInsuranceRef, v_vAccountingDate:=dtAccountingDate, v_vSpare:=v_sSpare, v_vComment:=sWriteOffReason)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId=" & m_lFeeID, gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
        End If

        'This code is here for the future when we support the releasing of Tax per instalment
        'It is not currently executed
        If Not Informations.IsNothing(v_cTaxAmount) And m_lTaxSuspID <> 0 Then
            If v_cTaxAmount <> 0 Then
                'Get the split of Taxes by Band from the Tax Calculation table
                With m_oDatabase
                    .Parameters.Clear()
                    'Developer Guide No 98
                    .Parameters.Add("pfprem_finance_cnt", v_lPremiumFinanceCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    .Parameters.Add("pfprem_finance_version", v_lPremiumFinanceVersion, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    .Parameters.Add("tax_amount", v_cTaxAmount, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                    .SQLSelect("spu_PFInstalments_get_instalments_tax", "Get Instalments Tax", True, , vTaxArray)
                End With

                'Loop through the results
                If Informations.IsArray(vTaxArray) Then

                    For lTaxRow As Integer = vTaxArray.GetLowerBound(1) To vTaxArray.GetUpperBound(1)

                        'Developer Guide No 98
                        lTaxAccountID = gPMFunctions.ToSafeLong(vTaxArray(0, lTaxRow), 0)

                        cThisTaxAmount = gPMFunctions.ToSafeCurrency(vTaxArray(1, lTaxRow), 0)

                        sTaxAccountCode = gPMFunctions.ToSafeString(vTaxArray(1, lTaxRow)).Trim()

                        iDocSeq += 1
                        If lTaxAccountID <> 0 And cThisTaxAmount <> 0 Then

                            m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=lTaxAccountID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound(cThisTaxAmount * v_dXRate * -1, kRoundingUpto, m_bSuppressDecimalValues), v_cCurrencyAmount:=gPMFunctions.ToSafeRound(cThisTaxAmount, kRoundingUpto, m_bSuppressDecimalValues),
                                                                       v_vdCurrencyBaseXRate:=v_dXRate, v_vDocumentSequence:=iDocSeq, v_vInsuranceRef:=sInsuranceRef, v_vSpare:="TAX", v_vAccountingDate:=dtAccountingDate, v_vComment:=sWriteOffReason)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId=" & lTaxAccountID, gPMConstants.PMELogLevel.PMLogError)
                            End If

                            m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=m_lTaxSuspID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound(cThisTaxAmount * v_dXRate, kRoundingUpto, m_bSuppressDecimalValues), v_cCurrencyAmount:=gPMFunctions.ToSafeRound(cThisTaxAmount, kRoundingUpto, m_bSuppressDecimalValues),
                                                                       v_vdCurrencyBaseXRate:=v_dXRate, v_vDocumentSequence:=iDocSeq, v_vInsuranceRef:=sInsuranceRef, v_vSpare:="TAX", v_vAccountingDate:=dtAccountingDate, v_vComment:=sWriteOffReason)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId=" & m_lTaxSuspID, gPMConstants.PMELogLevel.PMLogError)
                            End If

                        ElseIf lTaxAccountID = 0 Then
                            gPMFunctions.RaiseError("lTaxAccountID = 0", "Account does not exist", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    Next lTaxRow
                End If
            End If
        End If

        'If it has a currency gain/loss then add the gain/loss line to the transaction
        If v_bWriteOff And v_cPartialAmount <> 0 Then

            m_lReturn = CreateAllocationManual()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CreateAllocationManual", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If v_cPartialAmount <> 0 AndAlso v_dXRate = 1 Then
                m_lReturn = m_oAllocateManual.GetWriteOffDiffAccount(
                            crCurrencyDifference:=v_cCreditAmount - v_cPartialAmount,
                            r_nAccountID:=nWriteOffAccountID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oAllocateManual.GetWriteOffDiffAccount", "v_cCurrencyDifference=" & v_cCreditAmount - v_cPartialAmount, gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = PostInstalmentWriteOff(nGainsLossAccountID:=nWriteOffAccountID,
                                             nBaseCurrencyID:=iBaseCurrencyID,
                                             crWriteOffAmount:=v_cCreditAmount - v_cPartialAmount,
                                             nInsuranceFileCnt:=lInsuranceFileCnt,
                                             sInsuranceRef:=sInsuranceRef,
                                             dtAccountingDate:=dtDocumentDate,
                                             sComment:=sWriteOffReason,
                                             nWriteOffTransDetailId:=r_nWriteOffTransDetailId,
                                             sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeSwd, dXRate:=v_dXRate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("PostInstalmentWriteOff ", "v_lAccountId=" & nWriteOffAccountID, gPMConstants.PMELogLevel.PMLogError)
                End If

            Else
                m_lReturn = m_oAllocateManual.GetExchangeDiffAccount(v_cCurrencyDifference:=v_cCreditAmount - v_cPartialAmount, r_lAccountID:=lGainsLossAccountID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oAllocateManual.GetExchangeDiffAccount", "v_cCurrencyDifference=" & v_cCreditAmount - v_cPartialAmount, gPMConstants.PMELogLevel.PMLogError)
                End If

                'iDocSeq += 1

                'm_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=lGainsLossAccountID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=v_cCreditAmount - v_cPartialAmount, _
                '                                           v_cCurrencyAmount:=v_cCreditAmount - v_cPartialAmount, v_vdCurrencyBaseXRate:=v_dXRate, v_vDocumentSequence:=iDocSeq, v_vInsuranceRef:=sInsuranceRef, v_vAccountingDate:=dtAccountingDate, v_vComment:=sWriteOffReason)
                'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '    gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId=" & lGainsLossAccountID, gPMConstants.PMELogLevel.PMLogError)
                'End If
                m_lReturn = PostInstalmentWriteOff(nGainsLossAccountID:=lGainsLossAccountID,
                                             nBaseCurrencyID:=iBaseCurrencyID,
                                             crWriteOffAmount:=v_cCreditAmount - v_cPartialAmount,
                                             nInsuranceFileCnt:=lInsuranceFileCnt,
                                             sInsuranceRef:=sInsuranceRef,
                                             dtAccountingDate:=dtDocumentDate,
                                             sComment:=sWriteOffReason,
                                             nWriteOffTransDetailId:=r_nWriteOffTransDetailId,
                                             sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeSCD, dXRate:=v_dXRate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("PostInstalmentWriteOff ", "v_lAccountId=" & nWriteOffAccountID, gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
        End If
        'If it is overallocated then create a credit to the clients account
        If v_cPartialAmount > v_cCreditAmount And v_cPartialAmount <> 0 And Not v_bWriteOff Then

            m_lReturn = m_oAutoNumber.GetNumberRange(v_sGroupCode:=gACTLibrary.ACTAutoNumberGroupCodeDocumentRef1, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn, r_lNumberRangeID:=lNumberRangeID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oAutoNumber.GetNumberRange", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

            m_lReturn = m_oAutoNumber.GenerateDocumentReferenceNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=m_iUserID, v_iCompanyID:=m_lCompanyID, r_sDocumentRef:=sDocumentRef, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn)
            'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber
                gPMFunctions.RaiseError("m_oAutoNumber.GenerateDocumentReferenceNumber", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Format the number
            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            sDocumentRef = gACTLibrary.ACTAutoNumberRangeCodeJn & sDocumentRef

            m_lReturn = m_oDocumentPost.AddDocument(v_lDocumentTypeId:=gACTLibrary.ACTDocTypeInstalmentNBCredit, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=dtDocumentDate, v_sComment:="Overallocation of instalment plan " & m_sInstalmentPlan, r_vDocSourceID:=m_lCompanyID, r_vSubBranchID:=m_lSubBranchID, v_lInsuranceFileCnt:=lInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDocumentPost.AddDocument", "v_sDocumentRef=" & sDocumentRef, gPMConstants.PMELogLevel.PMLogError)
            End If

            iDocSeq = 1

            'Add client side of transaction

            m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=m_lAccountID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound(v_cPartialAmount - v_cCreditAmount, kRoundingUpto, m_bSuppressDecimalValues) * -1,
                                                       v_cCurrencyAmount:=gPMFunctions.ToSafeRound(v_cPartialAmount - v_cCreditAmount, kRoundingUpto, m_bSuppressDecimalValues) * -1, v_vdCurrencyBaseXRate:=v_dXRate, v_vDocumentSequence:=iDocSeq, v_vInsuranceRef:=sInsuranceRef, v_vAccountingDate:=dtAccountingDate, v_vComment:=sWriteOffReason)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId=" & m_lAccountID, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = CreateAllocationManual()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CreateAllocationManual", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oAllocateManual.GetExchangeDiffAccount(v_cCurrencyDifference:=v_cPartialAmount - v_cCreditAmount, r_lAccountID:=lGainsLossAccountID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CreateAllocationManual", "v_cCurrencyDifference=" & v_cPartialAmount - v_cCreditAmount, gPMConstants.PMELogLevel.PMLogError)
            End If

            iDocSeq += 1

            'Add gains/loss side of transaction

            m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=lGainsLossAccountID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound(v_cPartialAmount - v_cCreditAmount, kRoundingUpto, m_bSuppressDecimalValues),
                                                       v_cCurrencyAmount:=gPMFunctions.ToSafeRound(v_cPartialAmount - v_cCreditAmount, kRoundingUpto, m_bSuppressDecimalValues), v_vdCurrencyBaseXRate:=v_dXRate, v_vDocumentSequence:=iDocSeq, v_vInsuranceRef:=sInsuranceRef, v_vAccountingDate:=dtAccountingDate, v_vComment:=sWriteOffReason)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId=" & lGainsLossAccountID, gPMConstants.PMELogLevel.PMLogError)
            End If

        End If



        Return result
    End Function

    '******************************************************************************
    '        Function Name:  GetRISuspenseInfo
    '******************************************************************************
    '           Created By:  Ahmed "Jay" Bishtawi
    '           Created On:  18-Sep-2003
    '******************************************************************************
    '       Parameters Are:
    '                        (In)     - v_lFinancePlanCnt     - Long     -
    '                        (In)     - v_lFinancePlanVersion - Long     -
    '                        (In/Out) - r_bSuspenseFlag       - Boolean  -
    '                        (In/Out) - r_lSuspenseAccount    - Long     -
    '
    ' Return Value Type Is:  Long -
    '******************************************************************************
    ' Function Description:  This procedure gets the RI Suspense Infromation from
    '                        the scheme of the supplied PFPlan Cnt & version
    '******************************************************************************
    Private Function GetRISuspenseInfo(ByVal v_lFinancePlanCnt As Integer, ByVal v_lFinancePlanVersion As Integer, ByRef r_bSuspenseFlag As Boolean, ByRef r_lSuspenseAccount As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="PremiumFinanceCnt", vValue:=CStr(v_lFinancePlanCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRISuspenseInfo Failed to add Parameter to Stored Proc", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRISuspenseInfo", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If

            m_lReturn = .Parameters.Add(sName:="PremiumFinanceVersion", vValue:=CStr(v_lFinancePlanVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRISuspenseInfo Failed to add Parameter to Stored Proc", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRISuspenseInfo", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If

            m_lReturn = .SQLSelect(sSQL:=ACGetRISuspenseInfoSQL, sSQLName:=ACGetRISuspenseInfoName, bStoredProcedure:=ACGetRISuspenseInfoStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRISuspenseInfo Failed to Process the stored proc", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRISuspenseInfo", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If

            If .Records.Count() >= 1 Then
                'Developer Guide No 162
                r_lSuspenseAccount = gPMFunctions.NullToLong(.Records.Item(0).Fields("account_id"))
                r_bSuspenseFlag = gPMFunctions.NullToBoolean(.Records.Item(0).Fields("spread_ri"))
            Else
                r_lSuspenseAccount = 0
                r_bSuspenseFlag = False
            End If

        End With

        Return result

    End Function

    '******************************************************************************
    '        Function Name:  GetSuspendedParties
    '******************************************************************************
    '           Created By:  Ahmed "Jay" Bishtawi
    '           Created On:  18-Aug-2003
    '******************************************************************************
    '       Parameters Are:
    '                        (In)     - v_lInsuranceFileCnt - Long     -
    '                        (In/Out) - r_vSuspendedParties - Variant  -
    '
    ' Return Value Type Is:  Long -
    '******************************************************************************
    ' Function Description:  Get All RI Parties on a Policy including FAC so we can
    '                        suspend all of them
    '******************************************************************************
    Private Function GetSuspendedParties(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vSuspendedParties(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'First Get all the reinsurance memeber FAC and Treaty and their Account Numbers
        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSuspendedParties Failed to add Parameter to Stored Proc", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSuspendedParties", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If

            m_lReturn = .SQLSelect(sSQL:=ACGetSuspendedPartiesSQL, sSQLName:=ACGetSuspendedPartiesName, bStoredProcedure:=ACGetSuspendedPartiesStored, vResultArray:=r_vSuspendedParties)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSuspendedParties Failed to Process the stored proc", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSuspendedParties", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: PostInstalmentCash (PostInstalmentCash)
    '
    ' Description: Post Instalment Credit For Financed Amount
    '
    ' ***************************************************************** '
    Private Function PostInstalmentCash(ByVal v_cCreditAmount As Decimal, ByRef r_lCreditTransDetailId As Integer, ByVal iBaseCurrencyID As Integer) As Integer

        Dim result As Integer = 0

        Dim lNumberRangeID As Integer
        Dim iDocSeq As Integer
        Dim sDocumentRef As String = ""
        Dim dtDocumentDate As Date
        Dim sComment As String = ""


        result = gPMConstants.PMEReturnCode.PMTrue

        dtDocumentDate = DateTime.Now

        m_lReturn = CreateDocumentPost()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("CreateDocumentPost", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = CreateAutoNumber()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("CreateAutoNumber", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        'Get the number range

        m_lReturn = m_oAutoNumber.GetNumberRange(v_sGroupCode:=gACTLibrary.ACTAutoNumberGroupCodeDocumentRef22, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeSrp, r_lNumberRangeID:=lNumberRangeID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oAutoNumber.GetNumberRange", "v_sRangeCode:=" & gACTLibrary.ACTAutoNumberRangeCodeSrp, gPMConstants.PMELogLevel.PMLogError)
        End If

        'Generate the next number
        'Start (Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

        m_lReturn = m_oAutoNumber.GenerateDocumentReferenceNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=m_iUserID, v_iCompanyID:=m_lCompanyID, r_sDocumentRef:=sDocumentRef, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeSrp)
        'End (Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber
            gPMFunctions.RaiseError("m_oAutoNumber.GenerateDocumentReferenceNumber", "v_lNumberRangeID:=" & lNumberRangeID, gPMConstants.PMELogLevel.PMLogError)
        End If

        'Format the number
        '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        sDocumentRef = gACTLibrary.ACTAutoNumberRangeCodeSrp & sDocumentRef

        'Generate document

        m_lReturn = m_oDocumentPost.AddDocument(v_lDocumentTypeId:=gACTLibrary.ACTDocTypeReceipt, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=dtDocumentDate, v_sComment:="Instalment Plan " & m_sInstalmentPlan, r_vDocSourceID:=m_lCompanyID, r_vSubBranchID:=m_lSubBranchID, v_lInsuranceFileCnt:=m_lInsuranceFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDocumentPost.AddDocument", "v_sDocumentRef:=" & sDocumentRef, gPMConstants.PMELogLevel.PMLogError)
        End If

        iDocSeq = 1

        m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=m_lAccountID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound(v_cCreditAmount, kRoundingUpto, m_bSuppressDecimalValues) * -1, v_cCurrencyAmount:=gPMFunctions.ToSafeRound(v_cCreditAmount, kRoundingUpto, m_bSuppressDecimalValues) * -1, v_vdCurrencyBaseXRate:=1, r_vTransDetailId:=r_lCreditTransDetailId, v_vDocumentSequence:=iDocSeq, v_vInsuranceRef:=m_vInsuranceRef, v_vAccountingDate:=dtDocumentDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId:=" & m_lAccountID, gPMConstants.PMELogLevel.PMLogError)
        End If

        'If m_lBankID = 0 Then  
        m_lReturn = GetBankAccount()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("GetBankAccount", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
        'End If

        iDocSeq += 1

        m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=m_lBankID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound(v_cCreditAmount, kRoundingUpto, m_bSuppressDecimalValues), v_cCurrencyAmount:=gPMFunctions.ToSafeRound(v_cCreditAmount, kRoundingUpto, m_bSuppressDecimalValues), v_vdCurrencyBaseXRate:=1, v_vDocumentSequence:=iDocSeq, v_vInsuranceRef:=m_vInsuranceRef, v_vAccountingDate:=dtDocumentDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId:=" & m_lBankID, gPMConstants.PMELogLevel.PMLogError)
        End If
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PostInstalmentDebit (PostInstalmentDebit)
    '
    ' Description: Post Instalment Debit For Financed Amount
    '
    ' ***************************************************************** '
    Private Function PostInstalmentDebit(ByVal v_lAccountId As Integer,
                                         ByVal v_cDebitAmount As Decimal,
                                         ByVal v_cInterestAmount As Decimal,
                                         ByRef r_lDebitTransdetailId As Integer,
                                         ByVal v_lPremiumFinanceCnt As Integer,
                                         ByVal v_lPremiumFinanceVersion As Integer,
                                         ByVal v_cTaxAmount As Decimal,
                                         ByVal iBaseCurrencyID As Integer,
                                         Optional ByRef v_lDocumentTypeID As Integer = gACTLibrary.ACTDocTypeInstalmentDebit,
                                         Optional ByRef v_sRangeCode As String = gACTLibrary.ACTAutoNumberRangeCodeIND,
                                         Optional ByRef v_sDocRef As Object = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef42,
                                         Optional ByRef r_lDebitSuspenceTransDetailID As Integer = 0,
                                         Optional ByRef r_cDebitSuspenceAmount As Decimal = 0, Optional ByVal v_cOldFee As Decimal = 0D,
                                         Optional ByVal v_dXRate As Double = 1,
                                         Optional ByVal v_bCancelledSupersedPlan As Boolean = False) As Integer


        Dim result As Integer = 0

        Dim lNumberRangeID As Integer
        Dim iDocSeq As Integer
        Dim sDocumentRef As String = ""
        Dim dtDocumentDate As Date
        Dim sComment As String = ""
        Dim lInsuranceFileCnt As Integer
        Dim sInsuranceRef As String = ""
        Dim vTaxArray(,) As Object = Nothing
        Dim lTaxAccountID As Integer
        Dim cThisTaxAmount As Decimal
        Dim sTaxAccountCode As String = ""
        Dim bEndorsementEntry As Boolean
        Dim dtAccountingDate As Date
        Dim nPeriodId As Integer
        Dim sDocumentComments As String = ""


        result = gPMConstants.PMEReturnCode.PMTrue

        'Get the Insurance File Cnt for the Plan
        m_lReturn = GetInsuranceFileCntFromPF(lPremFinanceCnt:=v_lPremiumFinanceCnt, lPremFinanceVersion:=v_lPremiumFinanceVersion, r_lInsuranceFileCnt:=lInsuranceFileCnt, r_sInsuranceRef:=sInsuranceRef, r_dtCoverStartDate:=dtAccountingDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("GetInsuranceFileCntFromPF", "lPremFinanceCnt:=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
        End If

        dtDocumentDate = DateTime.Now
        dtAccountingDate = If(dtAccountingDate > dtDocumentDate, dtAccountingDate, dtDocumentDate)

        m_lReturn = GetPeriodFromInsuranceFile(v_lInsuranceFileCnt:=lInsuranceFileCnt, nPeriodID:=nPeriodId)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("GetPeriodFromInsuranceFile", "v_lInsuranceFileCnt:=" & lInsuranceFileCnt, gPMConstants.PMELogLevel.PMLogError)
        End If
        If nPeriodId = 0 Then
            m_lReturn = GetCurrentPeriodForLedger(v_nAccountId:=m_lAccountID, r_nPeriodId:=nPeriodId)
        End If

        m_lReturn = CreateDocumentPost()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("CreateDocumentPost", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = CreateAutoNumber()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("CreateDocumentPost", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = m_oAutoNumber.GetNumberRange(v_sGroupCode:=v_sDocRef, v_sRangeCode:=v_sRangeCode, r_lNumberRangeID:=lNumberRangeID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oAutoNumber.GetNumberRange", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
        'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

        m_lReturn = m_oAutoNumber.GenerateDocumentReferenceNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=m_iUserID, v_iCompanyID:=m_lCompanyID, r_sDocumentRef:=sDocumentRef, v_sRangeCode:=v_sRangeCode)
        'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber
            gPMFunctions.RaiseError("m_oAutoNumber.GenerateDocumentReferenceNumber", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        'Format the number
        '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        sDocumentRef = v_sRangeCode & sDocumentRef

        'Get the Insurance File Cnt for the Plan
        m_lReturn = GetInsuranceFileCntFromPF(lPremFinanceCnt:=v_lPremiumFinanceCnt,
                                              lPremFinanceVersion:=v_lPremiumFinanceVersion,
                                              r_lInsuranceFileCnt:=lInsuranceFileCnt, r_sInsuranceRef:=sInsuranceRef)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("GetInsuranceFileCntFromPF", "lPremFinanceCnt:=" & v_lPremiumFinanceCnt,
                                    gPMConstants.PMELogLevel.PMLogError)
        End If
        If v_bCancelledSupersedPlan Then
            sDocumentComments = "Prior plan version dishonour"
        Else
            sDocumentComments = "Instalment Plan " & m_sInstalmentPlan
        End If
        m_oDocumentPost.PostingPeriodNumber = nPeriodId

        m_lReturn = m_oDocumentPost.AddDocument(v_lDocumentTypeId:=v_lDocumentTypeID, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=dtDocumentDate, v_sComment:=sDocumentComments, r_vDocSourceID:=m_lCompanyID, r_vSubBranchID:=m_lSubBranchID, v_lInsuranceFileCnt:=lInsuranceFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDocumentPost.AddDocument", "v_sDocumentRef:=" & sDocumentRef, gPMConstants.PMELogLevel.PMLogError)
        End If

        iDocSeq = 1
        If bEndorsementEntry Then
            v_cDebitAmount = v_cDebitAmount + v_cInterestAmount
        End If
        m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=v_lAccountId, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound(If(v_dXRate <> 0, v_cDebitAmount * v_dXRate, v_cDebitAmount), kRoundingUpto, m_bSuppressDecimalValues), v_cCurrencyAmount:=gPMFunctions.ToSafeRound(v_cDebitAmount, kRoundingUpto, m_bSuppressDecimalValues),
                                                   v_vdCurrencyBaseXRate:=v_dXRate, r_vTransDetailId:=r_lDebitTransdetailId, v_vDocumentSequence:=iDocSeq, v_vInsuranceRef:=sInsuranceRef, v_vAccountingDate:=dtAccountingDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId:=" & v_lAccountId, gPMConstants.PMELogLevel.PMLogError)
        End If

        If v_cInterestAmount <> 0 Then
            m_lReturn = GetSchemeAccount(v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, v_sWhichAccount:="Fee")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetSchemeAccount", "v_sWhichAccount:=Fee", gPMConstants.PMELogLevel.PMLogError)
            End If

            iDocSeq += 1

            If v_cTaxAmount >= 0 Then

                m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=m_lFeeID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound((v_cInterestAmount + v_cOldFee - v_cTaxAmount) * v_dXRate * -1, kRoundingUpto, m_bSuppressDecimalValues),
                                                       v_cCurrencyAmount:=gPMFunctions.ToSafeRound((v_cInterestAmount + v_cOldFee - v_cTaxAmount) * -1, kRoundingUpto, m_bSuppressDecimalValues), v_vdCurrencyBaseXRate:=v_dXRate, v_vDocumentSequence:=iDocSeq, v_vInsuranceRef:=sInsuranceRef, v_vAccountingDate:=dtAccountingDate)
            Else

                m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=m_lFeeID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound(v_cInterestAmount * v_dXRate, kRoundingUpto, m_bSuppressDecimalValues) * -1, v_cCurrencyAmount:=gPMFunctions.ToSafeRound(v_cInterestAmount, kRoundingUpto, m_bSuppressDecimalValues) * -1,
                                                       v_vdCurrencyBaseXRate:=v_dXRate, v_vDocumentSequence:=iDocSeq, v_vInsuranceRef:=sInsuranceRef, v_vAccountingDate:=dtAccountingDate)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId:=" & m_lFeeID, gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        m_lReturn = GetSchemeAccount(v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, v_sWhichAccount:="SUSP")
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("GetSchemeAccount", "v_sWhichAccount:=SUSP", gPMConstants.PMELogLevel.PMLogError)
        End If

        iDocSeq += 1

        'Priya


        'Normal circumstance for suspense account - nothing to do with tax
        r_cDebitSuspenceAmount = (v_cDebitAmount - (v_cInterestAmount + v_cOldFee)) * -1

        m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=m_lSuspID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound(r_cDebitSuspenceAmount * v_dXRate, kRoundingUpto, m_bSuppressDecimalValues), v_cCurrencyAmount:=gPMFunctions.ToSafeRound(r_cDebitSuspenceAmount, kRoundingUpto, m_bSuppressDecimalValues),
                                               v_vdCurrencyBaseXRate:=v_dXRate, r_vTransDetailId:=r_lDebitSuspenceTransDetailID, v_vDocumentSequence:=iDocSeq, v_vInsuranceRef:=sInsuranceRef, v_vAccountingDate:=dtAccountingDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId:=" & m_lSuspID, gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = GetSchemeAccount(v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, v_sWhichAccount:="TAX")
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("GetSchemeAccount", "v_sWhichAccount:=TAX", gPMConstants.PMELogLevel.PMLogError)
        End If

        If v_cTaxAmount > 0 And m_lTaxSuspID = 0 Then
            'Get the split of Taxes by Band from the Tax Calculation table
            With m_oDatabase
                .Parameters.Clear()
                'Developer Guide No 98
                .Parameters.Add("pfprem_finance_cnt", v_lPremiumFinanceCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("pfprem_finance_version", v_lPremiumFinanceVersion, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("tax_amount", v_cTaxAmount, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                .SQLSelect("spu_PFInstalments_get_instalments_tax", "Get Instalments Tax", True, , vTaxArray)
            End With

            'Loop through the results
            If Informations.IsArray(vTaxArray) Then

                For lTaxRow As Integer = vTaxArray.GetLowerBound(1) To vTaxArray.GetUpperBound(1)

                    'Developer Guide No 98
                    lTaxAccountID = gPMFunctions.ToSafeLong(vTaxArray(0, lTaxRow), 0)

                    cThisTaxAmount = gPMFunctions.ToSafeCurrency(vTaxArray(1, lTaxRow), 0)

                    sTaxAccountCode = gPMFunctions.ToSafeString(vTaxArray(2, lTaxRow)).Trim()

                    iDocSeq += 1
                    If lTaxAccountID <> 0 And cThisTaxAmount <> 0 Then

                        m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=lTaxAccountID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=gPMFunctions.ToSafeRound(cThisTaxAmount * v_dXRate * -1, kRoundingUpto, m_bSuppressDecimalValues),
                                                                   v_cCurrencyAmount:=gPMFunctions.ToSafeRound(cThisTaxAmount * -1, kRoundingUpto, m_bSuppressDecimalValues), v_vdCurrencyBaseXRate:=v_dXRate, v_vDocumentSequence:=iDocSeq, v_vInsuranceRef:=sInsuranceRef, v_vSpare:="TAX", v_vAccountingDate:=dtAccountingDate)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId:=" & lTaxAccountID, gPMConstants.PMELogLevel.PMLogError)
                        End If
                    ElseIf lTaxAccountID = 0 Then
                        gPMFunctions.RaiseError("lTaxAccountID = 0", "Account does not exist", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Next lTaxRow
            End If
        End If
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: Allocate(Allocate)
    '
    ' Description: Allocate the Instalment Plan credit/cash
    '              against the original debt
    ' FSA Phase 3.2 Return AllocationId if requested
    ' ***************************************************************** '
    Private Function Allocate(ByVal v_vTransaction As Object, ByVal v_vTransactions As Object, Optional ByRef v_vAllocationId As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim vKeys As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        ReDim vKeys(1, 3)
        ' AccountID

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lAccountID

        ' Main Credit Transaction ID

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = v_vTransaction

        ' Matched Transactions

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = v_vTransactions
        'FSA Phase 3.2 suppress Release of Suspended Transactions

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameAllocatingSuspense
        If m_nSpreadCommission = 0 And m_sEarnCommissionOnPartPayments = "0" And Not m_bAllocateDepositToPlan Then

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = False
        Else

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = True
        End If
        ' Create an instance of the OrionLink business object

        m_lReturn = CreateAllocationManual()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMError
        End If

        m_lReturn = m_oAllocateManual.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

        ' Set the keys

        m_lReturn = m_oAllocateManual.SetKeys(vKeyArray:=vKeys)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set navigator keys.", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Start it

        m_oAllocateManual.CompanyId = m_lCompanyID

        m_lReturn = m_oAllocateManual.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start Allocation Manual.", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'FSA Phase 3.2

        If Not Informations.IsNothing(v_vAllocationId) Then

            m_lReturn = m_oAllocateManual.GetKeys(oKeyArray:=vKeys)
            If Informations.IsArray(vKeys) Then

                v_vAllocationId = vKeys(1, 0)
            End If
        End If

        m_oAllocateManual.Dispose()


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetAmountNotIncludedInInstalment
    '
    ' Description: Gets the amount from transdetail table for fees and taxes which are not included in instalment
    '
    ' Rahul Jaiswal 04/08/2006: Created
    ' ***************************************************************** '

    Private Function GetAmountNotIncludedInInstalment(ByVal vTransdetailID As Integer, ByRef r_vAmountNotIncludedInInstalment As Decimal) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()
            m_lReturn = .Parameters.Add(sName:="transdetail_id", vValue:=CStr(vTransdetailID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="amount", vValue:=CStr(r_vAmountNotIncludedInInstalment), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .SQLAction(sSQL:=ACGetAmountNotIncludedInInstalmentSQL, sSQLName:=ACGetAmountNotIncludedInInstalmentName, bStoredProcedure:=ACGetAmountNotIncludedInInstalmentStored)
            r_vAmountNotIncludedInInstalment = gPMFunctions.ToSafeCurrency(.Parameters.Item("amount").Value, 0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetBankAccount (GetBankAccount)
    '
    ' Description: Determine Bank Account from Sort Code
    '
    ' DD 06/08/2002: Alterations for multi-branch
    ' ***************************************************************** '
    Private Function GetBankAccount() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()
            m_lReturn = .Parameters.Add(sName:="company_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Developer Guide NO 85
            m_lReturn = .Parameters.Add(sName:="sub_branch_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="shortcode", vValue:=m_sBankCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="accountid", vValue:=CStr(m_lBankID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .SQLAction(sSQL:=ACGetAccountFromShortCodeSQL, sSQLName:=ACGetAccountFromShortCodeName, bStoredProcedure:=ACGetAccountFromShortCodeStored, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lBankID = gPMFunctions.NullToLong(.Parameters.Item("accountid").Value)

        End With
        Return result

    End Function
    ' ***************************************************************** '
    ' Name: GetFeeAccount (GetFeeAccount)
    '
    ' Description: Determine Fee ACcount from Sort Code
    '
    ' DD 06/08/2002: Alterations for multi-branch
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetFeeAccount) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetFeeAccount() As Integer
    '
    'Dim result As Integer = 0
    'Dim lRecordsAffected As Integer
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'With m_oDatabase
    '
    '.Parameters.Clear()
    'm_lReturn = .Parameters.Add(sName:="company_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lReturn = .Parameters.Add(sName:="sub_branch_id", vValue:=CStr(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = .Parameters.Add(sName:="shortcode", vValue:=m_sFeeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = .Parameters.Add(sName:="accountid", vValue:=CStr(m_lFeeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    '
    'm_lReturn = .SQLAction(sSQL:=ACGetAccountFromShortCodeSQL, sSQLName:=ACGetAccountFromShortCodeName, bStoredProcedure:=ACGetAccountFromShortCodeStored, lRecordsAffected:=lRecordsAffected)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lFeeID = gPMFunctions.NullToLong(.Parameters.Item("accountid").Value)
    '
    'End With
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFeeAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFeeAccount", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetClientAccount (GetClientAccount)
    '
    ' Description: Determine Bank ACcount from Sort Code
    '
    ' History:
    '   02/09/2002 PWF - Added sub branch return field
    ' ***************************************************************** '
    Private Function GetClientAccount(ByRef r_lAccountId As Integer, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_lTransDetail_id As Object = Nothing, Optional ByVal v_iIsThirdPartyPlan As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer
        Dim vResultArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        'TR - Check if the PartyCnt is supplied.

        If v_lPartyCnt > 0 Then

            'TR - Get the Account_id from the Party_cnt
            With m_oDatabase
                .Parameters.Clear()
                'PN6169 Extra parameter for company
                m_lReturn = .Parameters.Add(sName:="companyID", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                '

                m_lReturn = .Parameters.Add(sName:="partycnt", vValue:=CStr(CInt(v_lPartyCnt)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Developer Guide No. 85
                m_lReturn = .Parameters.Add(sName:="accountid", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'developer Guide No 85
                m_lReturn = .Parameters.Add(sName:="subbranchid", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLAction(sSQL:=ACGetAccountFromPartySQL, sSQLName:=ACGetAccountFromPartyName, bStoredProcedure:=ACGetAccountFromPartyStored, lRecordsAffected:=lRecordsAffected)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                r_lAccountId = gPMFunctions.NullToLong(.Parameters.Item("accountid").Value)

            End With
        Else

            With m_oDatabase
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="transdetail_id", vValue:=CStr(CInt(v_lTransDetail_id)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="is_third_party", vValue:=CInt(v_iIsThirdPartyPlan), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=ACGetTransDetailSQL, sSQLName:=ACGetTransDetailName, bStoredProcedure:=ACGetTransDetailStored, vResultArray:=vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Informations.IsArray(vResultArray) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                r_lAccountId = CInt(vResultArray(1, 0))

            End With

        End If

        Return result

    End Function
    ' PRIVATE Methods (End)

    Private Function UpdatePFTransactions(ByVal v_lPremiumFinanceCnt As Integer, ByVal v_lPremiumFinanceVersion As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="PremiumFinanceCnt", vValue:=CStr(v_lPremiumFinanceCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePFTransactions Failed to add Parameter to Stored Proc", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePFTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If

            m_lReturn = .Parameters.Add(sName:="PremiumFinanceVersion", vValue:=CStr(v_lPremiumFinanceVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePFTransactions Failed to add Parameter to Stored Proc", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePFTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If

            m_lReturn = .SQLSelect(sSQL:=ACUpdatePFAccountsTransactionsSQL, sSQLName:=ACUpdatePFAccountsTransactionsName, bStoredProcedure:=ACUpdatePFAccountsTransactionsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePFTransactions Failed on the call to Stored Proc", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePFTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If

        End With

        Return result

    End Function

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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub
    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    'dd
    Private Function GetSchemeCurrency(ByVal lPremiumFinanceCnt As Integer, ByVal lPremiumFinanceVersion As Integer, ByRef r_iCurrencyID As Integer, ByRef r_XRate As Double, Optional ByRef r_bTransCurrency As Boolean = False) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetSchemeCurrency
        ' PURPOSE: Returns the Base Currency used by the Finance Scheme
        ' AUTHOR: Danny Davis
        ' DATE: 04 August 2004, 09:50 AM
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vResultArray As Object(,) = Nothing

        result = gPMConstants.PMEReturnCode.PMFalse
        r_XRate = 1

        With m_oDatabase

            .Parameters.Clear()
            m_lReturn = .Parameters.Add(sName:="pfprem_finance_cnt", vValue:=CStr(lPremiumFinanceCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = .Parameters.Add(sName:="pfprem_finance_version", vValue:=CStr(lPremiumFinanceVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'Developer Guide No 39
            m_lReturn = .SQLSelect(sSQL:="spu_PFScheme_GetCurrency", sSQLName:="spu_PFScheme_GetCurrency", bStoredProcedure:=True, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Informations.IsArray(vResultArray) Then
                r_iCurrencyID = ToSafeInteger(vResultArray(5, 0))

                If ToSafeInteger(vResultArray(2, 0)) = 1 Then
                    r_bTransCurrency = True
                    r_iCurrencyID = ToSafeInteger(vResultArray(0, 0))
                    r_XRate = ToSafeDouble(vResultArray(1, 0))
                End If
            End If
        End With

        result = gPMConstants.PMEReturnCode.PMTrue

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetSchemeAccount
    '
    ' Description: Get the account from the scheme table for posting Instalment Tax,Fee etc
    '
    ' PSL 03/12/2002: Created
    ' ***************************************************************** '
    Private Function GetSchemeAccount(ByVal v_lPremiumFinanceCnt As Integer, ByVal v_lPremiumFinanceVersion As Integer, ByVal v_sWhichAccount As String, Optional ByRef r_sLedgerCode As String = "", Optional ByRef r_sAccountType As String = "", Optional ByRef v_lCashListItemId As Integer = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            If v_lCashListItemId <> 0 And v_sWhichAccount = "BANK" Then
                m_lBankID = 0
                .Parameters.Clear()
                m_lReturn = .Parameters.Add(sName:="CashListItemID", vValue:=v_lCashListItemId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:="spu_GetCashListBankAccount", sSQLName:="spu_GetCashListBankAccount", bStoredProcedure:=True)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    If Not m_oDatabase.Records Is Nothing Then
                        If m_oDatabase.Records.Count > 0 Then
                            m_lBankID = NullToLong(.Records.Item(0).Fields("account_id"))
                            r_sLedgerCode = .Records.Item(0).Fields("LedgerCode")
                            r_sAccountType = .Records.Item(0).Fields("AccountType")

                            If m_lBankID > 0 Then
                                Return gPMConstants.PMEReturnCode.PMTrue
                                Exit Function
                            End If

                        End If
                    End If
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            .Parameters.Clear()
            m_lReturn = .Parameters.Add(sName:="PremiumFinanceCnt", vValue:=CStr(v_lPremiumFinanceCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="PremiumFinanceVersion", vValue:=CStr(v_lPremiumFinanceVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="WhichAccount", vValue:=v_sWhichAccount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Developer Guide No 39
            m_lReturn = .SQLSelect(sSQL:="spu_PFScheme_GetAccount", sSQLName:="spu_Scheme_GetAccount", bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TR - Protect against uninstantiated objects
            If Not (m_oDatabase.Records Is Nothing) Then
                ' RAW 04/03/2003 : ISS2592 : added
                If m_oDatabase.Records.Count() <= 0 And v_sWhichAccount <> "TAX" Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Account can be found for " & v_sWhichAccount, vApp:=ACApp, vClass:=ACClass, vMethod:="GetSchemeAccount")
                    Return result
                End If
            Else
                'TR - Same message, but without the error
                result = gPMConstants.PMEReturnCode.PMNotFound
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Account can be found for " & v_sWhichAccount, vApp:=ACApp, vClass:=ACClass, vMethod:="GetSchemeAccount")
                Return result
            End If
            ' RAW 04/03/2003 : ISS2592 : end

            If m_oDatabase.Records.Count() > 0 Then
                'Developer Guide No 162
                'Starts

                Select Case v_sWhichAccount.ToUpper()
                    Case "FEE"
                        m_lFeeID = gPMFunctions.NullToLong(.Records.Item(0).Fields("account_id"))
                    Case "SUSP"
                        m_lSuspID = gPMFunctions.NullToLong(.Records.Item(0).Fields("account_id"))
                    Case "COMM"
                        m_lCommSuspID = gPMFunctions.NullToLong(.Records.Item(0).Fields("account_id"))
                    Case "BANK"
                        m_lBankID = gPMFunctions.NullToLong(.Records.Item(0).Fields("account_id"))
                    Case "TAX"
                        m_lTaxSuspID = gPMFunctions.NullToLong(.Records.Item(0).Fields("account_id"))
                    Case "SUBCOMM"
                        m_nSubAgentCommSuspId = NullToLong(.Records.Item(0).Fields("account_id"))
                End Select
                'Ends
                'Developer Guide No 162
                r_sLedgerCode = .Records.Item(0).Fields("LedgerCode")
                r_sAccountType = .Records.Item(0).Fields("AccountType")
            End If

        End With
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetOutstandingCommission
    '
    ' Description: Check if any commission has already been paid
    ' if it has we don't want to mess about with the accounts
    '
    ' History: 16/12/2002 PSL - Created
    ' AAB-24-September-2003 - Added the optional PartyCnt parameter
    ' ***************************************************************** '
    Public Function GetOutstandingCommission(ByVal v_lFinancePlanCnt As Integer, ByVal v_lFinancePlanVersion As Integer, ByRef r_cCommission As Decimal, ByRef r_cOutstandingCommission As Decimal, ByRef r_lCommissionEarnedAccountId As Integer, ByRef r_lTransactionId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Retrieve in house status for associated scheme
                .Parameters.Clear()

                ' Finance plan cnt
                m_lReturn = .Parameters.Add(sName:="PremiumFinanceCnt", vValue:=CStr(v_lFinancePlanCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

                ' Finance plan version
                m_lReturn = .Parameters.Add(sName:="PremiumFinanceVersion", vValue:=CStr(v_lFinancePlanVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

                ' Execute the procedure
                'Developer Guide No 39
                m_lReturn = .SQLSelect(sSQL:="spu_PFGetOutstandingCommission", sSQLName:="spu_PFGetOutstandingCommission", bStoredProcedure:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

                ' Retrieve the return value
                'Developer guide no 162
                r_cCommission = .Records.Item(0).Fields("Commission")
                r_cOutstandingCommission = .Records.Item(0).Fields("OutstandingCommission")
                r_lCommissionEarnedAccountId = .Records.Item(0).Fields("AccountID")
                r_lTransactionId = .Records.Item(0).Fields("TransactionID")

            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOutstandingCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOutstandingCommission", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetOutstandingCommission_SFU
    '
    ' Description: Check if any commission has already been paid
    ' if it has we don't want to mess about with the accounts
    '
    ' History: 16/12/2002 PSL - Created
    ' AAB-24-September-2003 - Added the optional PartyCnt parameter
    ' PN10637 Underwriting Specific Version
    ' ***************************************************************** '
    Public Function GetOutstandingCommission_SFU(ByVal v_lFinancePlanCnt As Integer, ByVal v_lFinancePlanVersion As Integer, ByRef r_cCommission As Decimal,
                                                ByRef r_cOutstandingCommission As Decimal,
                                                ByRef r_lOldCommissionAccountID As Integer, Optional ByVal v_lPartyCnt As Integer = 0,
                                                Optional ByRef v_vTransactions(,) As Object = Nothing,
                                                Optional ByRef r_cCommissionBaseAmount As Decimal = 0,
                                                Optional ByRef r_cOutstandingCommissionBaseAmount As Decimal = 0, Optional ByRef r_lOldTransDetailID As Long = 0) As Integer

        Dim result As Integer = 0
        Dim iCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Retrieve in house status for associated scheme
                .Parameters.Clear()

                ' Finance plan cnt
                m_lReturn = .Parameters.Add(sName:="PremiumFinanceCnt", vValue:=CStr(v_lFinancePlanCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

                ' Finance plan version
                m_lReturn = .Parameters.Add(sName:="PremiumFinanceVersion", vValue:=CStr(v_lFinancePlanVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

                If v_lPartyCnt > 0 Then
                    ' Party Cnt
                    m_lReturn = .Parameters.Add(sName:="PartyCnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return m_lReturn
                    End If

                    ' Execute the procedure
                    'Developer Guide No 39
                    m_lReturn = .SQLSelect(sSQL:="spu_PFGetOutstandingCommission_SFU", sSQLName:="spu_PFGetOutstandingCommission_SFU", bStoredProcedure:=True, vResultArray:=v_vTransactions)
                Else

                    ' Execute the procedure
                    'Developer Guide No 39
                    m_lReturn = .SQLSelect(sSQL:="spu_PFGetOutstandingCommission_SFU", sSQLName:="spu_PFGetOutstandingCommission_SFU", bStoredProcedure:=True, vResultArray:=v_vTransactions)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

                ' Retrieve the return value
                r_cCommission = 0
                r_cOutstandingCommission = 0
                r_cCommissionBaseAmount = 0
                r_cOutstandingCommissionBaseAmount = 0
                If (Informations.IsArray(v_vTransactions)) Then
                    For iCount = v_vTransactions.GetLowerBound(1) To v_vTransactions.GetUpperBound(1)
                        If iCount = 0 Then
                            r_lOldTransDetailID = ToSafeLong(v_vTransactions(3, iCount), 0)
                        End If
                        r_cCommission = r_cCommission + ToSafeCurrency(v_vTransactions(0, iCount), 0)
                        r_cOutstandingCommission = r_cOutstandingCommission + ToSafeCurrency(v_vTransactions(1, iCount), 0)
                        r_lOldCommissionAccountID = ToSafeLong(v_vTransactions(2, iCount), 0)
                        r_cCommissionBaseAmount = r_cCommissionBaseAmount + ToSafeCurrency(v_vTransactions(4, iCount), 0)
                        r_cOutstandingCommissionBaseAmount = r_cOutstandingCommissionBaseAmount + ToSafeCurrency(v_vTransactions(5, iCount), 0)
                    Next
                End If

            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOutstandingCommission_SFU Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOutstandingCommission_SFU", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'FSA Phase 3.2 Superceded
    '******************************************************************************
    '        Function Name:  CreatePFTransactions
    '******************************************************************************
    '           Created By:  Ahmed A. Bishtawi
    '           Created On:  18-Aug-2003
    '******************************************************************************
    '       Parameters Are:
    '                        (In) - v_lFinancePlanCnt     - Long    -
    '                        (In) - v_lFinancePlanVersion - Long    -
    '                        (In) - v_lAccountId          - Long    -
    '                        (In) - v_sSpare              - String  -
    '                        (In) - v_lTransDetailID      - Long    -
    '                        (In) - v_lTransactionType    - Long    -
    '
    ' Return Value Type Is:  Long -
    '******************************************************************************
    ' Function Description:  This table create the records in PF_Accounts_Transactions
    ' PN10637 Made function public so that it can be called from Insurer Payments
    '******************************************************************************
    'Public Function CreatePFTransactions(ByVal v_lFinancePlanCnt As Long, _
    ''                                      ByVal v_lFinancePlanVersion As Long, _
    ''                                      ByVal v_lAccountId As Long, _
    ''                                      ByVal v_sSpare As String, _
    ''                                      ByVal v_lTransDetailID As Long, _
    ''                                      ByVal v_lTransactionType As Long) As Long
    '
    '
    '    On Error GoTo Err_CreatePFTransactions
    '
    '    CreatePFTransactions = PMTrue
    '
    '
    '    With m_oDatabase
    '
    '        .Parameters.Clear
    '
    '        m_lReturn& = .Parameters.Add(sName:="PremiumFinanceCnt", _
    ''                                     vValue:=v_lFinancePlanCnt, _
    ''                                     idirection:=PMParamInput, _
    ''                                     iDataType:=PMLong)
    '
    '        If m_lReturn& <> PMTrue Then
    '
    '            CreatePFTransactions = m_lReturn&
    '
    '            Call LogMessage(m_sUsername, iType:=PMLogOnError, _
    ''                            sMsg:="CreatePFTransactions Failed to add Parameter to Stored Proc", _
    ''                            vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePFTransactions", _
    ''                            vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description)
    '
    '            Exit Function
    '
    '        End If
    '
    '        m_lReturn& = .Parameters.Add(sName:="PremiumFinanceVersion", _
    ''                                     vValue:=v_lFinancePlanVersion, _
    ''                                     idirection:=PMParamInput, _
    ''                                     iDataType:=PMLong)
    '
    '        If m_lReturn& <> PMTrue Then
    '
    '            CreatePFTransactions = m_lReturn&
    '
    '            Call LogMessage(m_sUsername, iType:=PMLogOnError, _
    ''                            sMsg:="CreatePFTransactions Failed to add Parameter to Stored Proc", _
    ''                            vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePFTransactions", _
    ''                            vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description)
    '
    '            Exit Function
    '
    '        End If
    '
    '        m_lReturn& = .Parameters.Add(sName:="AccountID", _
    ''                                     vValue:=v_lAccountId, _
    ''                                     idirection:=PMParamInput, _
    ''                                     iDataType:=PMLong)
    '
    '        If m_lReturn& <> PMTrue Then
    '
    '            CreatePFTransactions = m_lReturn&
    '
    '            Call LogMessage(m_sUsername, iType:=PMLogOnError, _
    ''                            sMsg:="CreatePFTransactions Failed to add Parameter to Stored Proc", _
    ''                            vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePFTransactions", _
    ''                            vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description)
    '
    '            Exit Function
    '
    '        End If
    '
    '        m_lReturn& = .Parameters.Add(sName:="Spare", _
    ''                                     vValue:=v_sSpare, _
    ''                                     idirection:=PMParamInput, _
    ''                                     iDataType:=PMString)
    '
    '        If m_lReturn& <> PMTrue Then
    '
    '            CreatePFTransactions = m_lReturn&
    '
    '            Call LogMessage(m_sUsername, iType:=PMLogOnError, _
    ''                            sMsg:="CreatePFTransactions Failed to add Parameter to Stored Proc", _
    ''                            vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePFTransactions", _
    ''                            vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description)
    '
    '            Exit Function
    '
    '        End If
    '
    '        m_lReturn& = .Parameters.Add(sName:="TransDetailID", _
    ''                                     vValue:=v_lTransDetailID, _
    ''                                     idirection:=PMParamInput, _
    ''                                     iDataType:=PMLong)
    '
    '        If m_lReturn& <> PMTrue Then
    '
    '            CreatePFTransactions = m_lReturn&
    '
    '            Call LogMessage(m_sUsername, iType:=PMLogOnError, _
    ''                            sMsg:="CreatePFTransactions Failed to add Parameter to Stored Proc", _
    ''                            vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePFTransactions", _
    ''                            vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description)
    '
    '            Exit Function
    '
    '        End If
    '
    '        m_lReturn& = .Parameters.Add(sName:="TransactionType", _
    ''                                     vValue:=v_lTransactionType, _
    ''                                     idirection:=PMParamInput, _
    ''                                     iDataType:=PMLong)
    '
    '        If m_lReturn& <> PMTrue Then
    '
    '            CreatePFTransactions = m_lReturn&
    '
    '            Call LogMessage(m_sUsername, iType:=PMLogOnError, _
    ''                            sMsg:="CreatePFTransactions Failed to add Parameter to Stored Proc", _
    ''                            vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePFTransactions", _
    ''                            vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description)
    '
    '            Exit Function
    '
    '        End If
    '
    '        m_lReturn& = .SQLSelect(sSQL:=ACCreatePFAccountsTransactionsSQL, _
    ''                                sSQLName:=ACCreatePFAccountsTransactionsName, _
    ''                                bStoredProcedure:=ACCreatePFAccountsTransactionsStored)
    '
    '        If m_lReturn& <> PMTrue Then
    '
    '            CreatePFTransactions = m_lReturn&
    '
    '            Call LogMessage(m_sUsername, iType:=PMLogOnError, _
    ''                            sMsg:="CreatePFTransactions Failed to Process the stored proc", _
    ''                            vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePFTransactions", _
    ''                            vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description)
    '
    '            Exit Function
    '
    '        End If
    '
    '    End With
    '
    'Exit Function
    '
    'Err_CreatePFTransactions:
    '
    '    CreatePFTransactions = PMFalse
    '
    '    Call LogMessage(m_sUsername, iType:=PMLogOnError, _
    ''                    sMsg:="CreatePFTransactions Failed", _
    ''                    vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePFTransactions", _
    ''                    vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description)
    'End Function

    ' ***************************************************************** '
    ' Name: ChangeCommissionTransDetailsID
    '
    ' Description: Now we have moved the commission from one account to another,
    ' update the premium finance record accordingly
    '
    ' History: 17/12/2002 PSL - Created
    ' ***************************************************************** '
    Public Function ChangeCommissionTransDetailsID(ByVal v_lFinancePlanCnt As Integer, ByVal v_lFinancePlanVersion As Integer, ByVal v_lCommissionAccountID As Integer, ByRef v_sDocumentRef As String, ByVal v_nDocSeq As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Retrieve in house status for associated scheme
            m_oDatabase.Parameters.Clear()

            ' Finance plan cnt
            m_lReturn = m_oDatabase.Parameters.Add(sName:="PremiumFinanceCnt", vValue:=CStr(v_lFinancePlanCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Finance plan version
            m_lReturn = m_oDatabase.Parameters.Add(sName:="PremiumFinanceVersion", vValue:=CStr(v_lFinancePlanVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' commission account
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CommissionAccountID", vValue:=CStr(v_lCommissionAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="DocumentRef", vValue:=v_sDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="DocSeq", vValue:=CStr(v_nDocSeq), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Execute the procedure
            'Developer Guide no 39
            m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_PFChangeCommisionTransID", sSQLName:="PFChangeCommisionTransID", bStoredProcedure:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChangeCommissionTransDetailsID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChangeCommissionTransDetailsID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetFeeAccount
    '
    ' Description: Get the fee account on the scheme
    ' using nothing but this transactionid you see before you
    '
    ' History: 17/12/2002 PSL - Created
    ' ***************************************************************** '
    Public Function GetFeeAccountFromTransID(ByVal v_lTransactionId As Integer, ByRef r_lFeeAccountID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Retrieve in house status for associated scheme
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="TransID", vValue:=CStr(v_lTransactionId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Developer Guide No 39
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_PFgetFeeAccountFromTrans", sSQLName:="PFgetFeeAccountFromTrans", bStoredProcedure:=True)

            ' Retrieve the return value
            'developer Guide No 162
            r_lFeeAccountID = m_oDatabase.Records.Item(0).Fields("interest_account_id")
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFeeAccountFromTransID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFeeAccountFromTransID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetCashListItem
    '
    ' Description: Get the cashlist item for this instalment (If there is one)
    '
    ' History: 17/12/2002 PSL - Created
    ' ***************************************************************** '
    Public Function GetCashListItem(ByVal v_lInstalmentID As Integer, ByRef r_lCashListItemID As Integer, ByRef r_lCashTransactionID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Retrieve in house status for associated scheme
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InstalmentID", vValue:=CStr(v_lInstalmentID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
            'Developer Guide No 39
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_PFGetCashListItemFromInstalment", sSQLName:="PFGetCashListItemFromInstalment", bStoredProcedure:=True)

            'sw 23/01/2003 check function return code
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Retrieve the return value
            If m_oDatabase.Records.Count() > 0 Then
                'Developer Guide No 162
                r_lCashListItemID = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields("cashlistitem_id"))
                r_lCashTransactionID = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields("transdetail_id"))
                m_lBankID = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields("account_id"))

            Else
                r_lCashListItemID = 0
                r_lCashTransactionID = 0
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCashListItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashListItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GETPFFromInstalmentsID
    '
    ' Description: Get the Premium Finance Count and version from the Instalments ID
    '
    ' History: 18/12/2002 PSL - Created
    ' AAB-19-September-2003 - Added r_nSpreadRI for RI suspense
    ' ***************************************************************** '
    Private Function GetPFFromInstalmentsID(ByVal v_lInstalmentsID As Integer, ByRef r_lPremiumFinanceCnt As Integer, ByRef r_lPremiumFinanceVersion As Integer, ByRef r_nSpreadCommission As Integer, ByRef r_nSpreadRI As Integer, ByRef r_cTaxAmount As Decimal, ByRef r_lTotalInstalment As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Retrieve in house status for associated scheme
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="InstalmentsID", vValue:=CStr(v_lInstalmentsID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPFFromInstalmentsIDSQL, sSQLName:=ACGetPFFromInstalmentsIDName, bStoredProcedure:=ACGetPFFromInstalmentsIDStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        If m_oDatabase.Records.Count() >= 1 Then
            ' Retrieve the return value
            'Developer Guide No 162
            r_lPremiumFinanceCnt = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields("pfPrem_Finance_cnt"))
            r_lPremiumFinanceVersion = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields("pfPrem_Finance_version"))
            r_nSpreadCommission = gPMFunctions.ToSafeInteger(m_oDatabase.Records.Item(0).Fields("Spread_Commission"))
            r_nSpreadRI = gPMFunctions.ToSafeInteger(m_oDatabase.Records.Item(0).Fields("Spread_ri"))
            r_cTaxAmount = gPMFunctions.ToSafeCurrency(m_oDatabase.Records.Item(0).Fields("tax"))
            r_lTotalInstalment = gPMFunctions.ToSafeInteger(m_oDatabase.Records.Item(0).Fields("NoOfInstallments"))
        End If
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GETPFFromTransID
    '
    ' Description: Get the Premium Finance Count and version from the TRansaction ID
    '
    ' History: 17/01/2003 PSL - Re-Created (Got Lost)
    '
    ' FSA Phase 3.2 1/2005  ECk Return the Finance Plan main transaction
    ' ***************************************************************** '
    Public Function GetPFFromTransID(ByVal v_lTransactionId As Integer, ByRef r_lPremiumFinanceCnt As Integer, ByRef r_lPremiumFinanceVersion As Integer, ByRef r_nSpreadCommission As Integer, ByRef r_lPlanTransDetailID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Const ACPremFinCnt As Integer = 0
        Const ACPremFinVer As Integer = 1
        Const ACSpreadCommission As Integer = 2
        Const ACPlanTransDetailID As Integer = 3
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Retrieve in house status for associated scheme
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="TransID", vValue:=CStr(v_lTransactionId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'developer Guide No 39
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_ACT_GetPFFromTransID", sSQLName:="ACT_GetPFFromTransactionID", bStoredProcedure:=True, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Retrieve the return value

            r_lPremiumFinanceCnt = CInt(vResultArray(ACPremFinCnt, 0))

            r_lPremiumFinanceVersion = CInt(vResultArray(ACPremFinVer, 0))

            r_nSpreadCommission = CInt(vResultArray(ACSpreadCommission, 0))

            r_lPlanTransDetailID = CInt(vResultArray(ACPlanTransDetailID, 0))

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GETPFFromTransactionID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GETPFFromTransactionID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GETCommissionDetails
    '
    ' Description: Get the Premium Finance Count and version from the TRansaction ID
    '
    ' History: 18/12/2002 PSL - Created
    ' ***************************************************************** '
    Public Function GetCommissionDetails(ByVal v_lTransactionId As Integer, ByVal v_lPremiumFinanceCnt As Integer, ByVal v_lPremiumFinanceVersion As Integer, ByVal v_lInstalmentID As Integer, ByRef r_lCommissionSuspenseTransID As Integer, ByRef r_dPercentage As Double, ByRef r_bIsLastInstalment As Boolean, ByRef r_lFeeAccountID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Retrieve in house status for associated scheme
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PlanTransID", vValue:=CStr(v_lTransactionId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="PremiumFinanceCnt", vValue:=CStr(v_lPremiumFinanceCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PremiumFinanceVersion", vValue:=CStr(v_lPremiumFinanceVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="InstalmentID", vValue:=CStr(v_lInstalmentID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
            'Dveloper Gudie No 39
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_ACT_GetDetailsForCommissionPost", sSQLName:="ACT_GETCommissionDetails", bStoredProcedure:=True)

            'sw 23/01/2003. Remember to check return code
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Retrieve the return value
            '13/03/2003 - PWC - Issue (ref:2752) - Handle Nulls
            'Developer Guide No 162
            r_lCommissionSuspenseTransID = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields("CommissionSuspenseTransID"))
            r_dPercentage = gPMFunctions.NullToDouble(m_oDatabase.Records.Item(0).Fields("Percentage"))
            r_bIsLastInstalment = gPMFunctions.NullToBoolean(m_oDatabase.Records.Item(0).Fields("IsLastInstalment"))
            r_lFeeAccountID = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields("FeeAccountID"))

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GETCommissionDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GETCommissionDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ChangePFTransactionDetailsID
    '
    ' Description: Now we have moved the commission from one account to another,
    ' update the premium finance record accordingly
    '
    ' History: 17/12/2002 PSL - Created
    ' ***************************************************************** '
    Public Function ChangePFTransactionDetailsID(ByVal v_lFinancePlanCnt As Integer, ByVal v_lFinancePlanVersion As Integer, ByRef v_sDocumentRef As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Retrieve in house status for associated scheme
            m_oDatabase.Parameters.Clear()

            ' Finance plan cnt
            m_lReturn = m_oDatabase.Parameters.Add(sName:="PremiumFinanceCnt", vValue:=CStr(v_lFinancePlanCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Finance plan version
            m_lReturn = m_oDatabase.Parameters.Add(sName:="PremiumFinanceVersion", vValue:=CStr(v_lFinancePlanVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="DocumentRef", vValue:=v_sDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Execute the procedure
            'Developer Guide NO 39
            m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_ACT_ChangePFTransactionID", sSQLName:="PFChangeTransactionID", bStoredProcedure:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChangePFTransactionDetailsID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChangePFTransactionDetailsID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteCancelledInstalments
    '
    ' Description: Now we have moved the commission from one account to another,
    ' update the premium finance record accordingly
    '
    ' History: 17/12/2002 PSL - Created
    ' ***************************************************************** '
    Public Function DeleteCancelledInstalments(ByVal v_lFinancePlanCnt As Integer, ByVal v_lFinancePlanVersion As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Retrieve in house status for associated scheme
            m_oDatabase.Parameters.Clear()

            ' Finance plan cnt
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pfprem_finance_cnt", vValue:=CStr(v_lFinancePlanCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Finance plan version
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pfprem_finance_version", vValue:=CStr(v_lFinancePlanVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Execute the procedure
            'developer Guide no 39
            m_lReturn = m_oDatabase.SQLAction(sSQL:="spe_PFInstalments_del", sSQLName:="PFInstalments_del", bStoredProcedure:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteCancelledInstalments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteCancelledInstalments", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' post adjustment underwriting version
    ''' </summary>
    ''' <param name="r_lNewPlanTransID"></param>
    ''' <param name="v_lPlanTransDetailID"></param>
    ''' <param name="v_vFinanceTrans"></param>
    ''' <param name="v_cAdjustment"></param>
    ''' <param name="v_cFee"></param>
    ''' <param name="v_cTax"></param>
    ''' <param name="v_cOSAmount"></param>
    ''' <param name="v_lPremiumFinanceCnt"></param>
    ''' <param name="v_lPremiumFinanceVersion"></param>
    ''' <param name="v_cNewPlanAmount"></param>
    ''' <param name="v_lLeadAgentCnt"></param>
    ''' <param name="v_cOldFee"></param>
    ''' <param name="v_cOldTax"></param>
    ''' <param name="v_lOrigianlPlanTransDetailID"></param>
    ''' <param name="v_bCommissionSpread"></param>
    ''' <param name="v_cDeposit"></param>
    ''' <param name="v_bProcessDeposit"></param>
    ''' <param name="v_vDepositJournal"></param>
    ''' <param name="v_lPartyCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PostMTAAdjustment(ByRef r_lNewPlanTransID As Integer,
                                      ByVal v_lPlanTransDetailID As Integer,
                                      ByVal v_vFinanceTrans As Object,
                                      ByVal v_cAdjustment As Decimal,
                                      ByVal v_cFee As Decimal, ByVal v_cTax As Decimal,
                                      ByVal v_cOSAmount As Decimal, ByVal v_lPremiumFinanceCnt As Integer,
                                      ByVal v_lPremiumFinanceVersion As Integer, ByVal v_cNewPlanAmount As Decimal,
                                      Optional ByVal v_lLeadAgentCnt As Integer = 0,
                                      Optional ByVal v_cOldFee As Decimal = 0,
                                      Optional ByVal v_cOldTax As Decimal = 0,
                                      Optional ByVal v_lOrigianlPlanTransDetailID As Integer = 0,
                                      Optional ByVal v_bCommissionSpread As Boolean = False,
                                      Optional ByVal v_cDeposit As Decimal = 0,
                                      Optional ByVal v_bProcessDeposit As Boolean = True,
                                      Optional ByRef v_vDepositJournal As Object = Nothing,
                                      Optional ByVal v_lPartyCnt As Long = 0,
                                      Optional ByVal v_bLeadAgentCommissionSpread As Boolean = False,
                                      Optional ByVal v_bSubAgentCommissionSpread As Boolean = False,
                                      Optional ByVal v_vSubAgent As Object = Nothing) As Integer

        Dim nResult As Integer = 0

        Dim oMatchTransDetailIds As Object
        Dim oDepositTransDetailIds As Object
        Dim nDebitTransDetailId As Integer
        Dim nDebitSuspenceTransDetailID As Integer
        Dim nAdjustmentCreditTransDetailId As Integer
        Dim nAdjustmentCreditSuspenseTransDetailId As Integer
        Dim nCreditTransDetailId As Integer
        Dim nCreditSuspenseTransDetailId As Integer
        Dim nBaseCurrencyID As Integer
        Dim nSelectedTransDetailId As Integer
        Dim oSelectedTrans As Object
        Dim cSelectedTransactionAmount As Decimal
        Dim nUBoundSelectedTransactions As Integer
        Dim nLBoundSelectedTransactions As Integer
        Dim sInsuranceRef As String = ""
        Dim bRollback As Boolean
        Dim cOSAmount As Decimal

        Dim nDepositTransdetailId As Integer
        Dim nDepositJournalId As Integer
        Dim nClientAccountID As Integer

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
            'Get the Currency for Transacting against
            Dim dXRate As Double = 1

            m_lReturn = GetSchemeCurrency(lPremiumFinanceCnt:=v_lPremiumFinanceCnt, lPremiumFinanceVersion:=v_lPremiumFinanceVersion, r_iCurrencyID:=nBaseCurrencyID, r_XRate:=dXRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetSchemeCurrency", "lPremiumFinanceCnt:=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            If dXRate = 0 Then
                dXRate = 1
            End If
            If Informations.IsArray(v_vFinanceTrans) Then

                nLBoundSelectedTransactions = v_vFinanceTrans.GetLowerBound(0)
                nUBoundSelectedTransactions = v_vFinanceTrans.GetUpperBound(0)

                cSelectedTransactionAmount = 0
                For lSelectedTransaction As Integer = nLBoundSelectedTransactions To nUBoundSelectedTransactions

                    ' 1 - get the first selected transdetail id

                    oSelectedTrans = CStr(v_vFinanceTrans(lSelectedTransaction)).Split("|"c)

                    ' get the transdetail id from the array
                    If Informations.IsArray(oSelectedTrans) Then

                        ' get transaction detail id

                        nSelectedTransDetailId = CInt(oSelectedTrans(0))

                        ' calculate total selected transaction amount

                        cSelectedTransactionAmount += CDec(oSelectedTrans(1))

                    End If

                Next
                cSelectedTransactionAmount = cSelectedTransactionAmount / dXRate
                'get the account from the selected transactions
                m_lReturn = GetClientAccount(r_lAccountId:=m_lAccountID, v_lTransDetail_id:=nSelectedTransDetailId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetClientAccount", "v_lTransDetail_id=" & nSelectedTransDetailId, gPMConstants.PMELogLevel.PMLogError)
                End If

            Else

                m_lReturn = GetClientAccount(r_lAccountId:=m_lAccountID, v_lTransDetail_id:=v_lPlanTransDetailID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetClientAccount", "v_lTransDetail_id=" & v_lPlanTransDetailID, gPMConstants.PMELogLevel.PMLogError)
                End If

                cSelectedTransactionAmount = v_cAdjustment

            End If
            If v_lPartyCnt <> 0 Then
                m_lReturn = GetClientAccount(r_lAccountId:=nClientAccountID, v_lPartyCnt:=v_lPartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetClientAccount", "v_lPartyCnt:=" & v_lPartyCnt, gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            m_lReturn = SetPlanSourceID(v_lPremFinanceCnt:=v_lPremiumFinanceCnt, v_lPremFinanceVersion:=v_lPremiumFinanceVersion)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetPlanSourceID", "v_lPremFinanceCnt:=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLBeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = True
            End If

            ReDim oMatchTransDetailIds(0)


            'We are creating a credit for the Outstanding Amount
            m_lReturn = PostInstalmentCredit(v_cCreditAmount:=v_cOSAmount,
                                             r_lCreditTransDetailId:=nCreditTransDetailId,
                                             v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt,
                                             v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion,
                                             v_lAgentCnt:=v_lLeadAgentCnt, v_bPostToSuspense:=True, v_cTaxAmount:=v_cOldTax,
                                              v_bCommissionSpread:=v_bLeadAgentCommissionSpread,
                                              v_cFeeAmount:=v_cOldFee, iBaseCurrencyID:=nBaseCurrencyID,
                                             r_lCreditSuspenceTransDetailID:=nCreditSuspenseTransDetailId, v_dXRate:=dXRate,
                                             bSubAgentCommissionSpread:=v_bSubAgentCommissionSpread,
                                             oSubAgent:=v_vSubAgent)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("PostInstalmentCredit", "v_lPremiumFinanceCnt:= " & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            If cSelectedTransactionAmount <> 0 Then
                m_lReturn = PostInstalmentCredit(v_cCreditAmount:=cSelectedTransactionAmount, r_lCreditTransDetailId:=nAdjustmentCreditTransDetailId, v_bPostToSuspense:=True, v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, v_lAgentCnt:=v_lLeadAgentCnt, v_bCommissionSpread:=v_bCommissionSpread, v_bCreateAccountingTransactions:=True, iBaseCurrencyID:=nBaseCurrencyID, r_lCreditSuspenceTransDetailID:=nAdjustmentCreditSuspenseTransDetailId, v_dXRate:=dXRate, bSubAgentCommissionSpread:=v_bSubAgentCommissionSpread, oSubAgent:=v_vSubAgent)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("PostInstalmentCredit", "v_lPremiumFinanceCnt:=" & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
            'post positive (outstanding + adjustment + fee) to client
            'post negative (outstanding + adjustment + fee) to suspense
            'post negative fee to fee account
            Dim dDebitAmt As Double = v_cNewPlanAmount + v_cAdjustment
            If Not v_bProcessDeposit Then
                dDebitAmt = dDebitAmt + v_cDeposit
            End If

            m_lReturn = PostInstalmentDebit(v_lAccountId:=m_lAccountID, v_cDebitAmount:=(v_cNewPlanAmount + v_cDeposit),
                                            v_cInterestAmount:=v_cFee, r_lDebitTransdetailId:=nDebitTransDetailId, v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt,
                                       v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, v_cTaxAmount:=v_cTax,
                                       v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeIED, v_sDocRef:=gACTLibrary.ACTAutoNumberGroupCodeDocumentRef44,
                                       iBaseCurrencyID:=nBaseCurrencyID, r_lDebitSuspenceTransDetailID:=nDebitSuspenceTransDetailID,
                                       v_cOldFee:=v_cOldFee, v_dXRate:=dXRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("PostInstalmentDebit", "v_lAccountId:=" & m_lAccountID, gPMConstants.PMELogLevel.PMLogError)
            End If

            'get back new plan trans id
            r_lNewPlanTransID = nDebitTransDetailId

            ' We need to update the existing records with the new version.
            m_lReturn = UpdatePFTransactions(v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("UpdatePFTransactions", "v_lPremiumFinanceCnt:= " & v_lPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            'have to close and start transaction again otherwise allocate won't work
            m_lReturn = m_oDatabase.SQLCommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = False
            End If

            'have to close and start transaction again otherwise allocate won't work
            m_lReturn = m_oDatabase.SQLBeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = True
            End If

            'allocate selected amount adjustment to original selected transactions
            If cSelectedTransactionAmount <> 0 Then
                m_lReturn = Allocate(v_vTransaction:=nAdjustmentCreditTransDetailId & "|" & cSelectedTransactionAmount * dXRate * -1, v_vTransactions:=v_vFinanceTrans)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("Allocate", "v_vTransaction:=" & nAdjustmentCreditTransDetailId & "|" & CStr(cSelectedTransactionAmount * dXRate * -1), gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
            'allocate original plan

            oMatchTransDetailIds(0) = CStr(v_lOrigianlPlanTransDetailID) & "|" & CStr(v_cOSAmount * dXRate)


            m_lReturn = Allocate(v_vTransaction:=nCreditTransDetailId & "|" & v_cOSAmount * dXRate * -1, v_vTransactions:=oMatchTransDetailIds)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Allocate", "v_vTransaction:=" & nCreditTransDetailId & "|" & CStr(v_cOSAmount * -1), gPMConstants.PMELogLevel.PMLogError)
            End If

            'Allocate ISUSP amounts
            If v_cFee - v_cOldFee <> 0 Then
                cOSAmount = (v_cOSAmount - v_cOldFee) * dXRate
            Else
                cOSAmount = v_cOSAmount * dXRate
            End If

            If cSelectedTransactionAmount <> 0 Then
                ReDim oMatchTransDetailIds(1)
                oMatchTransDetailIds(0) = CStr(nCreditSuspenseTransDetailId) & "|" & CStr(cOSAmount)
                oMatchTransDetailIds(1) = CStr(nAdjustmentCreditSuspenseTransDetailId) & "|" &
                                          CStr(cSelectedTransactionAmount)
            Else
                ReDim oMatchTransDetailIds(0)
                oMatchTransDetailIds(0) = CStr(nCreditSuspenseTransDetailId) & "|" & CStr(cOSAmount)
            End If
            m_lReturn =
                Allocate(
                    v_vTransaction:=nDebitSuspenceTransDetailID & "|" & (cOSAmount + cSelectedTransactionAmount) * -1,
                    v_vTransactions:=oMatchTransDetailIds)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Allocate",
                                        "v_vTransaction:=" & nDebitSuspenceTransDetailID & "|" &
                                        CStr((cOSAmount + cSelectedTransactionAmount) * -1),
                                        gPMConstants.PMELogLevel.PMLogError)
            End If
            'Post Transaction For value of Deposit
            If v_bProcessDeposit Then
                If v_cDeposit <> 0 Then

                    ReDim oDepositTransDetailIds(0)
                    ReDim oMatchTransDetailIds(0)

                    m_lReturn = PostDeposit(v_cAmount:=v_cDeposit * dXRate, v_lTransDetailId:=nDepositTransdetailId, v_lAccount_ID:=m_lAccountID, r_vMatchTransDetailIds:=oMatchTransDetailIds,
                                            iBaseCurrencyID:=nBaseCurrencyID, r_vDepositJournal:=v_vDepositJournal, nPremiumFinanceCnt:=v_lPremiumFinanceCnt,
                                            nPremiumFinanceVersion:=v_lPremiumFinanceVersion, nPlanTransDetailID:=nDebitTransDetailId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("PostDeposit", "v_lTransDetailID:=" & nDepositTransdetailId, gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If Not Informations.IsNothing(v_vDepositJournal) Then
                        nDepositJournalId = CInt(Mid(v_vDepositJournal, 1, v_vDepositJournal.IndexOf("|"c)))
                    End If

                    oDepositTransDetailIds(0) = oMatchTransDetailIds(0)

                End If
            End If



            m_lReturn = m_oDatabase.SQLCommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = False
            End If

            ' Do any tidy up, e.g. Set x = Nothing here

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostMTAAdjustment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostMTAAdjustment", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
        ' This is for debugging only

    End Function

    'SD 09/01/2003 Start - Use system option for Credit Control changes
    ' ***************************************************************** '
    ' Name: GetSystemOptionValue
    ' Description:
    ' History: 09/01/2003 sd - Created.
    '
    ' ***************************************************************** '
    Private Function GetSystemOptionValue(ByRef v_iOptionNumber As Integer, ByRef r_sValue As String) As Integer

        Dim result As Integer = 0
        Dim oOptions As bSIROptions.Business



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get an instance of component services

        oOptions = New bSIROptions.Business
        m_lReturn = oOptions.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the value for this option

        m_lReturn = oOptions.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=r_sValue)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then

            oOptions = Nothing

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get system option", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemOptionValue")

            Return gPMConstants.PMEReturnCode.PMFalse

        End If

        'If system option not found, default to zero
        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
            r_sValue = "0"
        End If

        oOptions.Dispose()
        oOptions = Nothing

        Return result

    End Function
    'SD 09/01/2003 End

    Private Function GetInsuranceFileCntFromPF(ByRef lPremFinanceCnt As Integer, ByRef lPremFinanceVersion As Integer, ByRef r_lInsuranceFileCnt As Integer, ByRef r_sInsuranceRef As String, Optional ByRef r_bIsRenewed As Boolean = False,
                                               Optional ByRef r_dtCoverStartDate As Date = #12/30/1899#) As Integer

        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetInsuranceFileCntFromPF
        ' PURPOSE:
        ' AUTHOR: Internal Use
        ' DATE: 13/03/2003, 16:00
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            .Parameters.Clear()

            'Developer Guide No 98
            .Parameters.Add("PFPremFinanceCnt", lPremFinanceCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("PFPremFinanceVersion", lPremFinanceVersion, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("InsuranceFileCnt", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("InsuranceRef", "", gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMString)
            'Developer Guide No 98
            .Parameters.Add("IsRenewed", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)
            .Parameters.Add("CoverStartDate", "", gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDate)

            'TR - Get the Premium Finance Record
            m_lReturn = .SQLSelect(sSQL:="spu_ACT_Get_InsuranceFileFromPF", sSQLName:="spu_ACT_Get_InsuranceFileFromPF", bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the Insurance File Cnt
            r_lInsuranceFileCnt = gPMFunctions.NullToLong(.Parameters.Item("InsuranceFileCnt").Value)
            r_sInsuranceRef = gPMFunctions.NullToString(.Parameters.Item("InsuranceRef").Value).Trim()
            r_bIsRenewed = gPMFunctions.ToSafeBoolean(ToSafeDouble(gPMFunctions.NullToString(.Parameters.Item("IsRenewed").Value)) = 1)
            r_dtCoverStartDate = gPMFunctions.ToSafeDate(.Parameters.Item("CoverStartDate").Value)

        End With


        '----------------------------------------------------------------------------------------
        'Only for Debugging, the code will never execute this line

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SettlePlanCalculate
    ' Description:
    ' History: 05/10/2001 DD - Created.
    ' ***************************************************************** '
    Public Function SettlePlanCalculate(ByVal v_lPremiumFinanceCnt As Integer, ByVal v_lPremiumFinanceVersion As Integer, ByRef r_crSettlement As Decimal, ByRef r_crRefundFee As Decimal, Optional ByRef r_vRefundTax As Decimal = Nothing) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            'TR - Use SP spu_PFPremiumFinance_settlement to calculate the Amount
            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("pfprem_finance_cnt", CStr(v_lPremiumFinanceCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("pfprem_finance_version", CStr(v_lPremiumFinanceVersion), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("SettleAmount", CStr(0), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDouble)
                .Parameters.Add("RefundFee", CStr(0), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDouble)
                'PSL 18/03/2003 Issue 2993
                .Parameters.Add("RefundTax", CStr(0), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDouble)

                'developer Guide No 39
                m_lReturn = .SQLSelect("spu_PFPremiumFinance_settlement", "spu_PFPremiumFinance_settlement", True, , vResultArray)

                'TR - Now save the amounts locally

                r_crRefundFee = gPMFunctions.NullToDecimal(.Parameters.Item("RefundFee").Value)
                'PSL 18/03/2003 2993

                If Not Informations.IsNothing(r_vRefundTax) Then
                    r_vRefundTax = gPMFunctions.NullToDecimal(.Parameters.Item("RefundTax").Value)
                End If
                If v_lPremiumFinanceCnt <> 0 AndAlso v_lPremiumFinanceVersion <> 0 Then
                    m_lReturn = SettlePlanCalculateRefund(nPremiumFinanceCnt:=v_lPremiumFinanceCnt, nPremiumFinanceVersion:=v_lPremiumFinanceVersion, r_crSettlement:=r_crSettlement, r_crInterestRefund:=r_crRefundFee)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                        Throw New ApplicationException("failed to execute SettlePlanCalculateRefund() method")
                    End If
                End If
                ' Add up Deposit if not added already

                .Parameters.Clear()
                'Developer Guide No 98
                .Parameters.Add("PremiumFinanceCnt", v_lPremiumFinanceCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("PremiumFinanceVersion", v_lPremiumFinanceVersion, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'developer Guide No 39
                m_lReturn = .SQLSelect("spu_PFGetDepositInfo", "spu_PFGetDepositInfo", True, , vResultArray)

                If vResultArray IsNot Nothing AndAlso vResultArray.Length > 0 Then
                    For iCnt As Integer = 0 To vResultArray.GetUpperBound(1)
                        'Developer Guide No 248
                        If Val(vResultArray(0, iCnt)) = 1 Or Val(vResultArray(0, iCnt)) = 5 Or Val(vResultArray(0, iCnt)) = 6 Then


                            If gPMFunctions.ToSafeLong(vResultArray(1, 0)) = 1 Then

                                r_crSettlement = CDec(CDbl(r_crSettlement + CDbl(vResultArray(2, iCnt)) - CDbl(vResultArray(3, iCnt))) - CDbl(vResultArray(4, iCnt)))
                            End If
                        End If
                    Next
                End If

            End With

            'Calculate the amount of Interest

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "SettlePlanCalculate Failed", ACApp, ACClass, "SettlePlanCalculate", Informations.Err().Number, excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetCollectionAccountID
    ' Description:
    ' History: 29/04/2003 SW get the collection account id for the cash drawer
    ' ***************************************************************** '

    Public Function GetCollectionAccountID(ByVal v_lCashDrawerID As Integer, ByRef r_lCollectionAccountID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()
                'Developer Guide No 98
                m_lReturn = .Parameters.Add("cashdrawerid", v_lCashDrawerID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'developer Gudie No 39
                m_lReturn = .SQLSelect(sSQL:="spu_ACT_Get_CD_CollectionAccountID", sSQLName:="spu_ACT_Get_CD_CollectionAccountID", bStoredProcedure:=True, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vResultArray) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                r_lCollectionAccountID = CInt(vResultArray(0, 0))

            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "GetCollectionAccountID Failed", ACApp, ACClass, "GetCollectionAccountID", Informations.Err().Number, excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    '        Function Name:  ProcessSuspendedTransactions
    '******************************************************************************
    '           Created By:  Ahmed "Jay" Bishtawi
    '           Created On:  24-Sep-2003
    '******************************************************************************
    '       Parameters Are:
    '                        (In) - v_lPremiumFinanceCnt     - Long  -
    '                        (In) - v_lPremiumFinanceVersion - Long  -
    '                        (In) - v_lInstalmentID          - Long  -
    '                        (In) - v_lSuspenseType          - Long  -
    '
    ' Return Value Type Is:  Long -
    '
    '       FSA Phase 3.2   Removed Suspense Type Parameter
    '******************************************************************************
    ' Function Description:  This function is used to process all suspended trans
    '                        for a given instalment when it is received
    '******************************************************************************
    Public Function ProcessSuspendedTransactions(ByVal v_lAllocationId As Integer,
                                                 ByVal v_lPremiumFinanceCnt As Integer,
                                                 ByVal v_lPremiumFinanceVersion As Integer,
                                                 ByVal v_lInstalmentID As Integer,
                                                 ByVal v_lInsuranceFileCnt As Integer,
                                                 ByVal v_sInsuranceRef As String,
                                                 ByVal v_bPartialPayment As Boolean,
                                                 Optional ByVal v_lSuspendedTransdetailID As Integer = 0,
                                                 Optional ByVal v_bLastInstalment As Boolean = False,
                                                 Optional ByVal v_bCancellationProcess As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vSuspenseTransDetails(,) As Object = Nothing

            'FSA Phase 3.2
            Dim bLastInstalment As Boolean
            Dim nTotalPendingInstalments As Integer
            Dim lSuspendedTransDetailID As Integer
            Dim vSuspendedTransDetailID As Integer

            'vSuspenseTransDetails array structure
            Const ACSuspenseTransID As Integer = 0
            Const ACSuspenseReleasePercetage As Integer = 1
            Const ACSuspenseIsLastInstalment As Integer = 2
            'Const ACSuspenseAccountID As Integer = 3
            Const ACSuspenseReleaseAmount As Integer = 4 'FSA Phase 3.2
            Const ACTotalPendingInstalments As Integer = 5 'FSA Phase 3.2

            ' Get Suspended Transactions Detail for the suspended type

            m_lReturn = GetSuspenseDetails(v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, v_lInstalmentID:=v_lInstalmentID, r_vSuspenseTransDetails:=vSuspenseTransDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessSuspendedTransactions Failed to get the Suspense Details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSuspendedTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If Informations.IsArray(vSuspenseTransDetails) Then

                m_lReturn = CreateTransdetail()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Loop through the array and post the transactions
                If Not True Then
                    vSuspendedTransDetailID = 0
                Else

                    If Convert.IsDBNull(v_lSuspendedTransdetailID) Or Informations.IsNothing(v_lSuspendedTransdetailID) Then
                        vSuspendedTransDetailID = 0
                    Else
                        vSuspendedTransDetailID = v_lSuspendedTransdetailID
                    End If
                End If

                For iCount As Integer = vSuspenseTransDetails.GetLowerBound(1) To vSuspenseTransDetails.GetUpperBound(1)
                    'If we have passed through a suspended transdetailId only this transaction should be released
                    If vSuspendedTransDetailID = 0 Or gPMFunctions.NullToLong(vSuspenseTransDetails(ACSuspenseTransID, iCount)) = vSuspendedTransDetailID Then

                        lSuspendedTransDetailID = gPMFunctions.NullToLong(vSuspenseTransDetails(ACSuspenseTransID, iCount))
                        bLastInstalment = gPMFunctions.NullToBoolean(vSuspenseTransDetails(ACSuspenseIsLastInstalment, iCount))
                        nTotalPendingInstalments = gPMFunctions.NullToLong(vSuspenseTransDetails(ACTotalPendingInstalments, iCount))

                        If (Not v_bCancellationProcess AndAlso nTotalPendingInstalments > 1) OrElse v_bPartialPayment Then
                            m_lReturn = m_oTransDetail.ReleaseSuspendedTransactions(lAllocationId:=v_lAllocationId,
                                                                                    vPFPremiumFinanceCnt:=v_lPremiumFinanceCnt,
                                                                                    vPFPremiumFinanceVersion:=v_lPremiumFinanceVersion,
                                                                                    vPercentage:=vSuspenseTransDetails(ACSuspenseReleasePercetage, iCount),
                                                                                    vAmount:=Nothing,
                                                                                    vSuspendedTransdetailID:=lSuspendedTransDetailID,
                                                                                    vLastInstalment:=bLastInstalment,
                                                                                    v_iInstalmentID:=v_lInstalmentID)
                        Else

                            m_lReturn = m_oTransDetail.ReleaseSuspendedTransactions(lAllocationId:=v_lAllocationId,
                                                                                    vPFPremiumFinanceCnt:=v_lPremiumFinanceCnt,
                                                                                    vPFPremiumFinanceVersion:=v_lPremiumFinanceVersion,
                                                                                    vPercentage:=Nothing,
                                                                                    vAmount:=vSuspenseTransDetails(ACSuspenseReleaseAmount, iCount),
                                                                                    vSuspendedTransdetailID:=lSuspendedTransDetailID,
                                                                                    vLastInstalment:=bLastInstalment,
                                                                                    v_iInstalmentID:=v_lInstalmentID)
                        End If
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = m_lReturn
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessSuspendedTransactions Failed Post Suspeneded Transaction(s)", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSuspendedTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            Return result
                        End If
                    End If
                Next

            End If

            'FSA Phase 3.2End

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessSuspendedTransactions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSuspendedTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    '******************************************************************************
    '        Function Name:  GetSuspenseDetails
    '******************************************************************************
    '           Created By:  Ahmed "Jay" Bishtawi
    '           Created On:  24-Sep-2003
    '******************************************************************************
    '       Parameters Are:
    '                        (In)     - v_lPremiumFinanceCnt     - Long     -
    '                        (In)     - v_lPremiumFinanceVersion - Long     -
    '                        (In)     - v_lInstalmentID          - Long     -
    '                        (In)     - v_lSuspenseType          - Long     -
    '                        (In/Out) - r_vSuspenseTransDetails  - Variant  -
    '
    ' Return Value Type Is:  Long -
    '
    '  FSA Phase 3.2 Removed Suspense Type Parameter
    '
    '******************************************************************************
    ' Function Description:  This function returns all the suspended transactions
    '                        for a given Instalment
    '******************************************************************************
    Public Function GetSuspenseDetails(ByVal v_lPremiumFinanceCnt As Integer, ByVal v_lPremiumFinanceVersion As Integer, ByVal v_lInstalmentID As Integer, ByRef r_vSuspenseTransDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="PremiumFinanceCnt", vValue:=CStr(v_lPremiumFinanceCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = m_lReturn

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSuspenseDetails Failed to add Parameter to Stored Proc", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSuspenseDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result

                End If

                m_lReturn = .Parameters.Add(sName:="PremiumFinanceVersion", vValue:=CStr(v_lPremiumFinanceVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = m_lReturn

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSuspenseDetails Failed to add Parameter to Stored Proc", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSuspenseDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result

                End If

                m_lReturn = .Parameters.Add(sName:="InstalmentID", vValue:=CStr(v_lInstalmentID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = m_lReturn

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSuspenseDetails Failed to add Parameter to Stored Proc", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSuspenseDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result

                End If

                'FSA Phase 3.2
                '        m_lReturn& = .Parameters.Add(sName:="TransactionType", _
                ''                                     vValue:=v_lSuspenseType, _
                ''                                     idirection:=PMParamInput, _
                ''                                     iDataType:=PMLong)

                '        If m_lReturn& <> PMTrue Then
                '
                '            GetSuspenseDetails = m_lReturn&
                '
                '            Call LogMessage(m_sUsername, iType:=PMLogOnError, _
                ''                            sMsg:="GetSuspenseDetails Failed to add Parameter to Stored Proc", _
                ''                            vApp:=ACApp, vClass:=ACClass, vMethod:="GetSuspenseDetails", _
                ''                            vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description)
                '                           Exit Function
                '        End If

                m_lReturn = .SQLSelect(sSQL:=ACGetSuspenseDetailsSQL, sSQLName:=ACGetSuspenseDetailsName, bStoredProcedure:=ACGetSuspenseDetailsStored, vResultArray:=r_vSuspenseTransDetails)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = m_lReturn

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSuspenseDetails Failed to process the Stored Proc", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSuspenseDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result

                End If

            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSuspenseDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSuspenseDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '******************************************************************************
    '        Function Name:  GetSuspendedTransactions
    '******************************************************************************
    '           Created By:  Ahmed "Jay" Bishtawi
    '           Created On:  18-Sep-2003
    '******************************************************************************
    '       Parameters Are:
    '                        (In)     - v_lFinancePlanCnt       - Long     -
    '                        (In)     - v_lFinancePlanVersion   - Long     -
    '                        (In)     - v_lAccountId            - Long     -
    '                        (In/Out) - r_vTransactionToSuspend - Variant  -
    '
    ' Return Value Type Is:  Long -
    '******************************************************************************
    ' Function Description:  This function returns an array of the Transaction ID,
    '                        TransAmount, TransNetAmount
    '******************************************************************************
    Private Function GetSuspendedTransactions(ByVal v_lFinancePlanCnt As Integer, ByVal v_lFinancePlanVersion As Integer, ByVal v_lAccountId As Integer, ByRef r_vTransactionToSuspend(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'First Get all the reinsurance memeber FAC and Treaty and their Account Numbers
        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="PremiumFinanceCnt", vValue:=CStr(v_lFinancePlanCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSuspendedTransactions Failed to add Parameter to Stored Proc", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSuspendedTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If

            m_lReturn = .Parameters.Add(sName:="PremiumFinanceVersion", vValue:=CStr(v_lFinancePlanVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSuspendedTransactions Failed to add Parameter to Stored Proc", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSuspendedTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If

            m_lReturn = .Parameters.Add(sName:="AccountID", vValue:=CStr(v_lAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSuspendedTransactions Failed to add Parameter to Stored Proc", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSuspendedTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If
            m_lReturn = .SQLSelect(sSQL:=ACGetSuspendedTransactionsSQL, sSQLName:=ACGetSuspendedTransactionsName, bStoredProcedure:=ACGetSuspendedTransactionsStored, vResultArray:=r_vTransactionToSuspend)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSuspendedTransactions Failed to Process the stored proc", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSuspendedTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If

        End With

        Return result

    End Function

    Private Function CreateAllocationManual() As Integer
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oAllocateManual Is Nothing Then

            m_oAllocateManual = New bACTAllocationManual.Business
            m_lReturn = m_oAllocateManual.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bACTAllocationManual.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAllocationManual", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If
        Return result
    End Function
    'FSA Phase 3.2
    Public Function UpdateSuspendedAccountsTransactions(ByVal v_vOldTriggerTransdetailIds As Object, Optional ByVal v_vNewTriggerTransdetailId As Object = Nothing, Optional ByRef v_vPremiumFinanceCnt As Object = Nothing, Optional ByRef v_vPremiumFinanceVersion As Object = Nothing, Optional ByRef v_vDepositTransdetailID As Object = Nothing, Optional ByRef v_vDepositPercentage As Object = Nothing) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: UpdateSuspendedAccountsTransactions
        ' PURPOSE: Applies Finance Plan ids's to suspended transactions
        ' AUTHOR: Elaine Knott
        ' DATE: December 2004
        ' RETURNS: PMTrue for success
        ' CHANGES: FSA Phase IV Receive Deposit Transdetail & Percentage Paramaters
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CreateTransdetail()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(v_vOldTriggerTransdetailIds) Then
                For lCounter As Integer = 0 To v_vOldTriggerTransdetailIds.GetUpperBound(0)
                    'FSA Phase IV Use new method to move suspended transactions to Finance Plan
                    '    m_lReturn = m_oTransDetail.RewriteSuspendedTransactions(lOldTriggerTransdetailID:=CLng(v_vOldTriggerTransdetailIds(lCounter)), _
                    ''                                            vNewTriggerTransdetailId:=v_vNewTriggerTransdetailId, _
                    ''                                            vPFPremiumFinanceCnt:=v_vPremiumFinanceCnt, _
                    ''                                            vPFPremiumFinanceVersion:=v_vPremiumFinanceVersion)

                    m_lReturn = m_oTransDetail.FinanceSuspendedTransactions(lOldTriggerTransdetailID:=CInt(v_vOldTriggerTransdetailIds(lCounter)), vPlanTransdetailId:=v_vNewTriggerTransdetailId, vPFPremiumFinanceCnt:=v_vPremiumFinanceCnt, vPFPremiumFinanceVersion:=v_vPremiumFinanceVersion, vDepositTransdetailId:=v_vDepositTransdetailID, vDepositPercentage:=v_vDepositPercentage)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTInstalments - UpdateSuspendedAccountsTransactions - bACTTransdetail.RewriteSuspendedTransactions Failed")
                    End If

                Next
            End If


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateSuspendedAccountsTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally

        End Try
        Return result

    End Function

    '****************************************************************************
    ' Get values from specified columns (v_vReturnColumn)
    ' if v_sKeyColumn and v_sKeyValue is passed in then use it as criteria otherwise
    ' whole table is selected
    ' if v_vReturnColumn is not an array then single value is returned
    ' Note: v_lColumnType = PMLong, PMString etc
    '
    '****************************************************************************
    'Developer Guide No 33
    Public Function GetValueFromTable(ByVal v_sTableName As String, ByVal v_vReturnColumn As Object, ByVal v_sKeyColumn As String, ByVal v_sKeyValue As String, ByVal v_iDataType As Integer, ByRef r_vResult(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As New StringBuilder
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = New StringBuilder("SELECT DISTINCT ")

            If Informations.IsArray(v_vReturnColumn) Then

                For Each v_vReturnColumn_item As Object In v_vReturnColumn

                    sSQL.Append(CStr(v_vReturnColumn_item) & ",")
                Next v_vReturnColumn_item

                'get rid of last comma
                sSQL = New StringBuilder(sSQL.ToString().Substring(0, sSQL.ToString().Length - 1))

            Else
                sSQL.Append(CStr(v_vReturnColumn))
            End If

            sSQL.Append(Strings.ChrW(13) & Strings.ChrW(10) & "FROM " & v_sTableName & Strings.ChrW(13) & Strings.ChrW(10))

            If v_sKeyColumn <> "" Then
                sSQL.Append("WHERE " & v_sKeyColumn & " = {KeyValue}")

                m_oDatabase.Parameters.Clear()

                Select Case v_iDataType
                    Case gPMConstants.PMEDataType.PMString
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=v_sKeyValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)
                    Case gPMConstants.PMEDataType.PMLong
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CInt(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)
                    Case gPMConstants.PMEDataType.PMInteger
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CInt(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                    Case gPMConstants.PMEDataType.PMDouble
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CDbl(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                    Case gPMConstants.PMEDataType.PMDate
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=(v_sKeyValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                    Case gPMConstants.PMEDataType.PMBoolean
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CBool(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                    Case gPMConstants.PMEDataType.PMCurrency
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CDec(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                End Select

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="Get single value from table", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'are we returning an array or a single value?
            If Informations.IsArray(v_vReturnColumn) Then

                r_vResult = vResultArray
            Else
                If Informations.IsArray(vResultArray) Then

                    r_vResult = vResultArray(0, 0)
                Else
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get value from " & v_sTableName & "." & v_sKeyColumn & " = " & v_sKeyValue, vApp:=ACApp, vClass:=ACClass, vMethod:="GetValueFromTable", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally

        End Try

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetTransDetailDocumentType
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 13-07-2005 : PN21887
    ' ***************************************************************** '
    Private Function GetTransDetailDocumentType(ByVal v_lTransDetailId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetTransDetailDocumentType"




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = AddInputParameter(v_sName:="transdetail_id", v_vValue:=v_lTransDetailId, v_iType:=gPMConstants.PMEDataType.PMLong)

        ' Execute selection Query
        If m_oDatabase.SQLSelect(sSQL:=kGetTransDetailDocumentTypeSQL, sSQLName:=kGetTransDetailDocumentTypeName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

            gPMFunctions.RaiseError(kMethodName, kGetTransDetailDocumentTypeSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

        End If

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

    ' ***************************************************************** '
    ' Name: GetCreditControlItemId
    '
    ' Description: Retrun the Cred Control Item ID from that table for
    ' the given input parameters
    '
    ' ***************************************************************** '
    Public Function GetCreditControlItemID(ByVal lPFInstalmentsID As Integer, ByRef r_lCCIID As Integer) As Integer
        Dim result As Integer = 0
        Dim vResults(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Add the parameters to our SP call
            m_oDatabase.Parameters.Clear()

            ' Finance plan cnt
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pfinstalments_id", vValue:=CStr(lPFInstalmentsID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCCIFromInstalmentSQL, sSQLName:=ACGetCCIFromInstalmentName, bStoredProcedure:=ACGetCCIFromInstalmentStored, vResultArray:=vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vResults) Then
                r_lCCIID = 0
                Return m_lReturn
            End If

            'Get the return value

            r_lCCIID = CInt(vResults(0, 0))

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCreditControlItemId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCreditControlItemId", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RemoveCreditControlItem
    ' Description:
    ' History: 09/01/2003 sd - Created.
    '
    ' ***************************************************************** '
    Private Function RemoveCreditControlItem(ByRef v_iCreditControlItemID As Integer) As Integer

        Dim result As Integer = 0
        Dim oCredConItem As bACTCreditControlItem.Business



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get an instance of component services

        oCredConItem = New bACTCreditControlItem.Business
        m_lReturn = oCredConItem.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Do the work

        m_lReturn = oCredConItem.DirectDelete(v_lCreditControlItemId:=v_iCreditControlItemID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            oCredConItem = Nothing

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to remove a credit control item", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveCreditControlItem")

            Return gPMConstants.PMEReturnCode.PMFalse

        End If

        oCredConItem.Dispose()
        oCredConItem = Nothing

        Return result

    End Function

    Private Function CreateDocumentPost() As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oDocumentPost Is Nothing Then

            m_oDocumentPost = New bACTDocumentPost.Form
            m_lReturn = m_oDocumentPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sClassName:=bACTDocumentPost.Form", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If
        Return result
    End Function

    Private Function CreateAutoNumber() As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oAutoNumber Is Nothing Then

            m_oAutoNumber = New bACTAutoNumber.Business
            m_lReturn = m_oAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sClassName:=bACTAutoNumber.Business", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If
        Return result
    End Function

    Private Function CreateTransdetail() As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oTransDetail Is Nothing Then

            m_oTransDetail = New bACTTransDetail.Form
            m_lReturn = m_oTransDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sClassName:=bACTTransDetail.Form", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If
        m_oTransDetail.m_oDatabase = m_oDatabase
        Return result
    End Function

    Private Function SetPlanSourceID(ByVal v_lPremFinanceCnt As Integer, ByVal v_lPremFinanceVersion As Integer) As Integer

        Dim result As Integer = 0


        Dim vResultArray(,) As Object = Nothing


        result = gPMConstants.PMEReturnCode.PMTrue

        bPMAddParameter.AddParameterLite(m_oDatabase, "pfprem_finance_cnt", v_lPremFinanceCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
        bPMAddParameter.AddParameterLite(m_oDatabase, "pfprem_finance_version", v_lPremFinanceVersion, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSourceIDFromPlanSQL, sSQLName:=ACGetSourceIDFromPlanName, bStoredProcedure:=ACGetSourceIDFromPlanStored, vResultArray:=vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQL:=ACGetSourceIDFromPlanSQL", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lCompanyID = CInt(vResultArray(0, 0))

        m_lSubBranchID = CInt(vResultArray(1, 0))

        Return result
    End Function

    Private Function GetCurrentPeriodForLedger(ByVal v_nAccountId As Integer, ByRef r_nPeriodId As Integer) As Integer

        Dim nResult As Integer = 0
        Dim vResultArray(,) As Object = Nothing



        nResult = gPMConstants.PMEReturnCode.PMTrue

        bPMAddParameter.AddParameterLite(m_oDatabase, "nAccountId", v_nAccountId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
        bPMAddParameter.AddParameterLite(m_oDatabase, "nSourceId", m_iSourceID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCurrentPeriodFromLegderSQL, sSQLName:=ACGetCurrentPeriodFromLegderName, bStoredProcedure:=ACGetCurrentPeriodFromLegderStored, vResultArray:=vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQL:=ACGetCurrentPeriodFromLegderSQL", gPMConstants.PMELogLevel.PMLogError)
        End If

        If Informations.IsArray(vResultArray) Then
            r_nPeriodId = vResultArray(0, 0)
        End If

        Return nResult

    End Function

    Private Function PostInstalmentWriteOff(ByVal nGainsLossAccountID As Integer,
                                             ByVal nBaseCurrencyID As Integer,
                                             ByVal crWriteOffAmount As Decimal,
                                             ByVal nInsuranceFileCnt As Integer,
                                             ByVal sInsuranceRef As String,
                                             ByVal dtAccountingDate As Date,
                                             ByVal sComment As String,
                                             ByRef nWriteOffTransDetailId As Integer) As Integer

        Dim nNumberRangeID As Integer
        Dim nNumber As Integer
        Dim sDocumentRef As String


        Const kMethodName As String = "PostInstalmentWriteOff"

        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMTrue

        If crWriteOffAmount = 0 Then
            Return gPMConstants.PMEReturnCode.PMTrue
        End If
        Try

            nReturn = m_oAutoNumber.GetNumberRange(
                    v_sGroupCode:=gACTLibrary.ACTAutoNumberGroupCodeDocumentRef12,
                    v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeSwd,
                    r_lNumberRangeID:=nNumberRangeID)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oAutoNumber.GetNumberRange", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Generate the next number
            nReturn = m_oAutoNumber.GenerateNumber(
                v_lNumberRangeID:=nNumberRangeID,
                v_iUserID:=m_iUserID,
                v_iCompanyID:=m_lCompanyID,
                r_lNumber:=nNumber)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oAutoNumber.GenerateNumber", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Format the number
            sDocumentRef = FormatDocumentRef(gACTLibrary.ACTAutoNumberRangeCodeSwd, nNumber)

            nReturn = m_oDocumentPost.AddDocument(
                v_lDocumentTypeId:=gACTLibrary.ACTDocTypeWriteOff,
                v_sDocumentRef:=sDocumentRef,
                v_dtDocumentDate:=dtAccountingDate,
                v_sComment:=sComment,
                r_vDocSourceID:=m_lCompanyID,
                r_vSubBranchID:=m_lSubBranchID,
                v_lInsuranceFileCnt:=nInsuranceFileCnt)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTInstalments - PostInstalmentWriteOff - bACTTransdetail.PostInstalmentWriteOff Failed")
            End If

            nReturn = m_oDocumentPost.AddTransaction(
                v_lAccountID:=m_lAccountID,
                v_iCurrencyID:=nBaseCurrencyID,
                v_cAmount:=gPMFunctions.ToSafeRound(crWriteOffAmount, kRoundingUpto, m_bSuppressDecimalValues) * -1,
                v_cCurrencyAmount:=gPMFunctions.ToSafeRound(crWriteOffAmount, kRoundingUpto, m_bSuppressDecimalValues) * -1,
                v_vdCurrencyBaseXRate:=1,
                r_vTransDetailId:=nWriteOffTransDetailId,
                v_vDocumentSequence:=1,
                v_vInsuranceRef:=sInsuranceRef,
                v_vAccountingDate:=dtAccountingDate,
                v_vComment:=sComment)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId:=" & m_lAccountID, gPMConstants.PMELogLevel.PMLogError)
            End If

            nReturn = m_oDocumentPost.AddTransaction(
                v_lAccountID:=nGainsLossAccountID,
                v_iCurrencyID:=nBaseCurrencyID,
                v_cAmount:=gPMFunctions.ToSafeRound(crWriteOffAmount, kRoundingUpto, m_bSuppressDecimalValues),
                v_cCurrencyAmount:=gPMFunctions.ToSafeRound(crWriteOffAmount, kRoundingUpto, m_bSuppressDecimalValues),
                v_vdCurrencyBaseXRate:=1,
                v_vDocumentSequence:=2,
                v_vInsuranceRef:=sInsuranceRef,
                v_vAccountingDate:=dtAccountingDate,
                v_vComment:=sComment)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId=" & nGainsLossAccountID, gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch excep As System.Exception

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostInstalmentWriteOff Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
        Return nReturn
    End Function
    Private Function PostInstalmentWriteOff(ByVal nGainsLossAccountID As Integer,
                                             ByVal nBaseCurrencyID As Integer,
                                             ByVal crWriteOffAmount As Decimal,
                                             ByVal nInsuranceFileCnt As Integer,
                                             ByVal sInsuranceRef As String,
                                             ByVal dtAccountingDate As Date,
                                             ByVal sComment As String,
                                             ByRef nWriteOffTransDetailId As Integer,
                                             ByVal sRangeCode As String, ByVal dXRate As Double) As Integer

        Dim nNumberRangeID As Integer
        Dim nNumber As Integer
        Dim sDocumentRef As String = String.Empty

        Const kMethodName As String = "PostInstalmentWriteOff"

        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMTrue

        If crWriteOffAmount = 0 Then
            Return gPMConstants.PMEReturnCode.PMTrue
        End If
        Try

            nReturn = m_oAutoNumber.GetNumberRange(
                    v_sGroupCode:=gACTLibrary.ACTAutoNumberGroupCodeDocumentRef12,
                    v_sRangeCode:=sRangeCode,
                    r_lNumberRangeID:=nNumberRangeID)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oAutoNumber.GetNumberRange", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Generate the next number
            nReturn = m_oAutoNumber.GenerateNumber(
                v_lNumberRangeID:=nNumberRangeID,
                v_iUserID:=m_iUserID,
                v_iCompanyID:=m_lCompanyID,
                r_lNumber:=nNumber)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oAutoNumber.GenerateNumber", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Format the number
            sDocumentRef = FormatDocumentRef(sRangeCode, nNumber)

            nReturn = m_oDocumentPost.AddDocument(
                v_lDocumentTypeId:=gACTLibrary.ACTDocTypeWriteOff,
                v_sDocumentRef:=sDocumentRef,
                v_dtDocumentDate:=dtAccountingDate,
                v_sComment:=sComment,
                r_vDocSourceID:=m_lCompanyID,
                r_vSubBranchID:=m_lSubBranchID,
                v_lInsuranceFileCnt:=nInsuranceFileCnt)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTInstalments - PostInstalmentWriteOff - bACTTransdetail.PostInstalmentWriteOff Failed")
            End If

            nReturn = m_oDocumentPost.AddTransaction(
                v_lAccountID:=m_lAccountID,
                v_iCurrencyID:=nBaseCurrencyID,
                v_cAmount:=gPMFunctions.ToSafeRound(Math.Round(crWriteOffAmount * dXRate, 2) * -1, kRoundingUpto, m_bSuppressDecimalValues),
                v_cCurrencyAmount:=gPMFunctions.ToSafeRound(crWriteOffAmount * -1, kRoundingUpto, m_bSuppressDecimalValues),
                v_vdCurrencyBaseXRate:=dXRate,
                r_vTransDetailId:=nWriteOffTransDetailId,
                v_vDocumentSequence:=1,
                v_vInsuranceRef:=sInsuranceRef,
                v_vAccountingDate:=dtAccountingDate,
                v_vComment:=sComment)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId:=" & m_lAccountID, gPMConstants.PMELogLevel.PMLogError)
            End If

            nReturn = m_oDocumentPost.AddTransaction(
                v_lAccountID:=nGainsLossAccountID,
                v_iCurrencyID:=nBaseCurrencyID,
                v_cAmount:=gPMFunctions.ToSafeRound(Math.Round(crWriteOffAmount * dXRate, 2), kRoundingUpto, m_bSuppressDecimalValues),
                v_cCurrencyAmount:=gPMFunctions.ToSafeRound(crWriteOffAmount, kRoundingUpto, m_bSuppressDecimalValues),
                v_vdCurrencyBaseXRate:=dXRate,
                v_vDocumentSequence:=2,
                v_vInsuranceRef:=sInsuranceRef,
                v_vAccountingDate:=dtAccountingDate,
                v_vComment:=sComment)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId=" & nGainsLossAccountID, gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch excep As System.Exception

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostInstalmentWriteOff Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
        Return nReturn
    End Function

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
    ''' <summary>
    ''' Calculate Plan Refund
    ''' </summary>
    ''' <param name="nPremiumFinanceCnt"></param>
    ''' <param name="nPremiumFinanceVersion"></param>
    ''' <param name="r_crSettlement"></param>
    ''' <param name="r_crInterestRefund"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SettlePlanCalculateRefund(ByVal nPremiumFinanceCnt As Integer, ByVal nPremiumFinanceVersion As Integer, ByRef r_crSettlement As Decimal, ByRef r_crInterestRefund As Decimal) As Integer

        Dim aoResultArray(,) As Object = Nothing
        Dim crTotalInterestAmount As Decimal
        Dim crInterestPerInstalmentAmount As Decimal
        Dim nTotalInstalment As Integer
        Dim nRemaingInstalment As Integer
        Dim bIsSuppressDecimal As Boolean
        Dim crTaxAmount As Decimal
        Dim crTotalUnCollectedAmount As Decimal
        Dim crTotalInterestRefund As Decimal

        m_lReturn = gPMConstants.PMEReturnCode.PMTrue
        Try
            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("pfprem_finance_cnt", CStr(nPremiumFinanceCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("pfprem_finance_version", CStr(nPremiumFinanceVersion), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(ACSettlePlanCalculateRefundSQL, ACSettlePlanCalculateRefundFileName, True, , aoResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = gPMConstants.PMEReturnCode.PMError
                    Throw New ApplicationException("failed to execute ACSettlePlanCalculateRefundSQL")
                End If
                If Informations.IsArray(aoResultArray) Then
                    nRemaingInstalment = ToSafeInteger(aoResultArray(0, 0))
                    nTotalInstalment = ToSafeInteger(aoResultArray(1, 0))
                    crTotalInterestAmount = ToSafeDecimal(aoResultArray(2, 0))
                    crTotalUnCollectedAmount = ToSafeDecimal(aoResultArray(3, 0))
                    crTaxAmount = ToSafeDecimal(aoResultArray(4, 0))
                    bIsSuppressDecimal = ToSafeBoolean(aoResultArray(5, 0))

                    If nTotalInstalment = nRemaingInstalment Then
                        r_crSettlement = crTotalUnCollectedAmount - crTaxAmount - crTotalInterestAmount
                        r_crInterestRefund = crTotalInterestAmount
                    Else
                        If crTotalInterestAmount > 0 Then

                            If nTotalInstalment > 0 Then
                                crInterestPerInstalmentAmount = crTotalInterestAmount / nTotalInstalment
                                crTotalInterestRefund = crInterestPerInstalmentAmount * nRemaingInstalment

                                If crTotalInterestAmount >= crTotalInterestRefund Then
                                    r_crSettlement = crTotalUnCollectedAmount - crTotalInterestRefund - crTaxAmount
                                    r_crInterestRefund = crTotalInterestRefund
                                Else
                                    r_crSettlement = crTotalUnCollectedAmount - crTotalInterestAmount - crTaxAmount
                                    r_crInterestRefund = crTotalInterestAmount
                                End If
                            End If
                        Else
                            r_crSettlement = crTotalUnCollectedAmount - crTaxAmount
                            r_crInterestRefund = 0
                        End If
                    End If
                    If r_crSettlement < 0 Then
                        r_crSettlement = crTotalUnCollectedAmount - crTaxAmount
                        r_crInterestRefund = 0
                    End If

                    r_crSettlement = gPMFunctions.ToSafeRound(r_crSettlement, 2, bIsSuppressDecimal)
                    r_crInterestRefund = gPMFunctions.ToSafeRound(r_crInterestRefund, 2, bIsSuppressDecimal)

                End If
            End With
            Return m_lReturn
        Catch excep As System.Exception
            m_lReturn = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "SettlePlanCalculateRefund Failed", ACApp, ACClass, "SettlePlanCalculateRefund", excep:=excep)
        End Try
        Return m_lReturn
    End Function
    Private Function GetInsuranceFileTransactionType(ByVal nInsuranceFileCnt As Integer,
                                             ByRef r_bResetToIRD As Boolean) As Integer

        Dim aoResultArray(,) As Object = Nothing
        Dim sTransactionCode As String = String.Empty

        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("nInsurance_file_cnt", nInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            m_lReturn = .SQLSelect(KGetInsuranceFileTransactionTypeSQL, KGetInsuranceFileTransactionTypeName, True, , aoResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = gPMConstants.PMEReturnCode.PMError
                Throw New ApplicationException("failed to execute KGetInsuranceFileTransactionTypeSQL")
            End If
        End With
        If Informations.IsArray(aoResultArray) Then
            sTransactionCode = ToSafeString(aoResultArray(0, 0))
        End If
        If sTransactionCode = "REN" Then
            r_bResetToIRD = True
        End If
        Return m_lReturn
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_nPlanTransDetailID"></param>
    ''' <param name="v_nPremiumFinanceCnt"></param>
    ''' <param name="v_nPremiumFinanceVersion"></param>
    ''' <param name="v_bPlanHasSinglePolicy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SettleCancelledSupersededPlan(ByVal v_nPlanTransDetailID As Integer, ByVal v_nPremiumFinanceCnt As Integer, ByVal v_nPremiumFinanceVersion As Integer,
                                                  Optional ByRef r_vCreditTransDetail As Object = Nothing, Optional ByRef r_vDebitTransDetail As Object = Nothing, Optional ByVal v_bPlanHasSinglePolicy As Boolean = False,
                                                  Optional ByVal v_nIsThirdPartyPlan As Integer = 0, Optional ByVal v_nPartyCnt As Integer = 0) As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "SettleCancelledSupersededPlan"

        Dim nCount As Integer
        Dim oFinanceTransIds As Object
        Dim crTransDetailOSAmount As Decimal
        Dim nCreditTransDetailId, nDebitTransDetailId As Integer
        Dim oMatchTransDetailIds As Object
        Dim crSettleAmount, crSettleRefundFee As Decimal
        Dim crSettleRefundTax As Object = Nothing
        Dim sOptionVal As String = ""

        Dim nBaseCurrencyID As Integer
        Dim nDocumentTypeID As Integer
        Dim sRangeCode, sDocRef As String
        Dim bRollback As Boolean

        Dim nCreditSuspenceTransDetailID As Long
        Dim nDebitSuspenceTransDetailID As Long

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetClientAccount(r_lAccountId:=m_lAccountID, v_lTransDetail_id:=v_nPlanTransDetailID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetClientAccount", "v_lTransDetail_id=" & v_nPlanTransDetailID, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = SetPlanSourceID(v_lPremFinanceCnt:=v_nPremiumFinanceCnt, v_lPremFinanceVersion:=v_nPremiumFinanceVersion)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetPlanSourceID", "v_lPremFinanceCnt:=" & v_nPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = CreateTransdetail()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CreateTransdetail", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oTransDetail.GetDetails(vTransdetailID:=v_nPlanTransDetailID, vOSAmounts:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oTransDetail.GetDetails", "vTransdetailID:=" & v_nPlanTransDetailID, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oTransDetail.GetNext(vOSCurrencyAmount:=crTransDetailOSAmount)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oTransDetail.GetNext", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLBeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = True
            End If
            'd
            'Get the Currency for Transacting against
            m_lReturn = GetSchemeCurrency(lPremiumFinanceCnt:=v_nPremiumFinanceCnt, lPremiumFinanceVersion:=v_nPremiumFinanceVersion, r_iCurrencyID:=nBaseCurrencyID, r_XRate:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetSchemeCurrency", "lPremiumFinanceCnt:=" & v_nPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            If crTransDetailOSAmount <> 0 Then
                m_lReturn = PostInstalmentCredit(v_cCreditAmount:=crTransDetailOSAmount, r_lCreditTransDetailId:=nCreditTransDetailId, v_lPremiumFinanceCnt:=v_nPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_nPremiumFinanceVersion, iBaseCurrencyID:=nBaseCurrencyID, v_bPostToSuspense:=True, r_lCreditSuspenceTransDetailID:=nCreditSuspenceTransDetailID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("PostInstalmentCredit", "v_lPremiumFinanceCnt:=" & v_nPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
            r_vCreditTransDetail = nCreditTransDetailId

            If v_nIsThirdPartyPlan = gPMConstants.PMEReturnCode.PMTrue Then
                crSettleAmount = crTransDetailOSAmount

                m_lReturn = GetClientAccount(
                r_lAccountId:=m_lAccountID, v_lPartyCnt:=v_nPartyCnt,
                v_lTransDetail_id:=v_nPlanTransDetailID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' DO Not Call any functions before here or the error will be lost
                    bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult)
                    If bRollback Then
                        m_oDatabase.SQLRollbackTrans()
                    End If
                End If

            End If
            'If the plan has transactions from several policies on it then debit to be raised is just a journal.
            If v_bPlanHasSinglePolicy Then
                nDocumentTypeID = gACTLibrary.ACTDocTypeEndorsementDebit
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSed
                sDocRef = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef17
            Else
                nDocumentTypeID = gACTLibrary.ACTDocTypeJournal
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeJn
                sDocRef = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef1
            End If

            m_lReturn = PostInstalmentDebit(v_lAccountId:=m_lAccountID, v_cDebitAmount:=crTransDetailOSAmount, v_cInterestAmount:=crSettleRefundFee * -1, r_lDebitTransdetailId:=nDebitTransDetailId, v_lPremiumFinanceCnt:=v_nPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_nPremiumFinanceVersion, v_cTaxAmount:=crSettleRefundTax * -1, v_lDocumentTypeID:=nDocumentTypeID, v_sRangeCode:=sRangeCode, v_sDocRef:=sDocRef, iBaseCurrencyID:=nBaseCurrencyID, r_lDebitSuspenceTransDetailID:=nDebitSuspenceTransDetailID, v_bCancelledSupersedPlan:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("PostInstalmentDebit", "v_lAccountId:=" & m_lAccountID, gPMConstants.PMELogLevel.PMLogError)
            End If

            'r_vDebitTransDetail = nDebitTransDetailId

            ' We need to process dripping of reinsurance and agent commission and taxes...
            ' Before we delete the instlaments.
            m_lReturn = ProcessPFAccountsForCancellation(v_lPremiumFinanceCnt:=v_nPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_nPremiumFinanceVersion)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ProcessPFAccountsForCancellation", "v_lPremiumFinanceCnt:=" & v_nPremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Allocate Instalment Credit To Pre-cancellation OS Debit
            ReDim oFinanceTransIds(0)

            oFinanceTransIds.SetValue(ToSafeInteger(v_nPlanTransDetailID), 0)

            ReDim oMatchTransDetailIds(0)

            oMatchTransDetailIds.SetValue(CStr(oFinanceTransIds.GetValue(0)) & "|" & CStr(crTransDetailOSAmount), ToSafeInteger(nCount))
            '5109- Priya

            If nCreditTransDetailId <> 0 And crTransDetailOSAmount <> 0 Then
                m_lReturn = Allocate(v_vTransaction:=nCreditTransDetailId & "|" & crTransDetailOSAmount * -1, v_vTransactions:=oMatchTransDetailIds)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("Allocate", "v_vTransaction:=" & nCreditTransDetailId & "|" & CStr(crTransDetailOSAmount * -1), gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            'Allocate SUSP INC and SED
            If v_nIsThirdPartyPlan = gPMConstants.PMEReturnCode.PMTrue Then
                ReDim oFinanceTransIds(0)
                oFinanceTransIds(0) = nDebitSuspenceTransDetailID

                ReDim oMatchTransDetailIds(0)
                oMatchTransDetailIds(nCount) = oFinanceTransIds(0) & "|" & crTransDetailOSAmount * -1

                m_lReturn = Allocate(
                    v_vTransaction:=nCreditSuspenceTransDetailID & "|" & crTransDetailOSAmount,
                    v_vTransactions:=oMatchTransDetailIds)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("Allocate", "v_vTransaction:=" & nCreditSuspenceTransDetailID & "|" & crTransDetailOSAmount, gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            m_lReturn = m_oDatabase.SQLCommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                bRollback = False
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult)

            ' If you want to rollback a transaction or something, do it here
            If bRollback Then
                m_oDatabase.SQLRollbackTrans()
            End If

        Finally
            ' Do any tidy up, e.g. Set x = Nothing here

        End Try

        Return nResult
    End Function

    Public Function GetPeriodFromInsuranceFile(v_lInsuranceFileCnt As Integer, ByRef nPeriodID As Integer)
        Dim vResultArray As Object
        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("InsuranceFileCnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = .SQLSelect(sSQL:=ACGetPeriodFromInsuranceFileIDSQL, sSQLName:=ACGetPeriodFromInsuranceFileIDName, bStoredProcedure:=ACGetPeriodFromInsuranceFileIDStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then
                nPeriodID = vResultArray(0, 0)
            Else
                nPeriodID = 0
            End If
        End With
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    'ADO #39472: Claim Recovery — returns the transtype (NB, REN, SR, TPR, etc.) for a PF plan
    Private Function GetPlanTransType(ByVal v_lPremFinanceCnt As Integer, ByVal v_lPremFinanceVersion As Integer, ByRef r_sTransType As String) As Integer
        Dim vResultArray(,) As Object = Nothing
        r_sTransType = ""

        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("pfprem_finance_cnt", v_lPremFinanceCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("pfprem_finance_version", v_lPremFinanceVersion, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = .SQLSelect(sSQL:="SELECT LTRIM(RTRIM(transtype)) FROM PFPremiumFinance WHERE pfprem_finance_cnt = {pfprem_finance_cnt} AND pfprem_finance_version = {pfprem_finance_version}",
                                   sSQLName:="GetPlanTransType", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then
                r_sTransType = gPMFunctions.NullToString(vResultArray(0, 0)).Trim()
            End If
        End With
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

End Class
