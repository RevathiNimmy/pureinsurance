Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date:
    '
    ' Description: Main interface Class.
    '
    ' Edit History:
    ' RAM20050826 : Bug fix for PN 23018 - Added OriginalInsuranceFileCnt
    '                                      Installments Tab enable / disable bug fix
    ' ***************************************************************** '

    Private Const ACClass As String = "Interface"

    ' Process mode variables
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_sCallingAppName As String = ""

    Private m_lPMAuthorityLevel As Integer

    ' Stores the exit status of the interface.
    Private m_lStatus As Integer

    ' Party Count
    Private m_lPartyCnt As Integer
    ' Short Name
    Private m_sShortName As String = ""
    ' Insurance stuff
    Private m_iSpecifiedTab As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_lRiskId As Integer

    Private m_lReturn As Integer
    Private m_sTaskGroupCode As String = ""
    Private m_lPFPremFinanceCnt As Integer
    Private m_lPFPremFinanceVersion As Integer
    Private m_sPaymentTerms As String = ""
    Private m_sQuoteStatus As String = ""
    Private m_iMode As Integer
    Private m_sOldPolicyNumber As String = ""
    Private m_sNewPolicyNumber As String = ""
    Private m_bRoadmapDisablesInstalments As Boolean

    Private m_lOriginalInsuranceFileCnt As Integer
    Private m_iMTAType As Integer

    'Float Balance and Pre-Payment (RC)
    Private m_lPaymentAccountID As Integer
    Private m_iDebitAgainst As Integer
    Private m_vCreditTransactions As Object
    Private m_vDebitTransactions As Object
    Private m_lCashListID As Integer
    Private m_lCashListItemID As Integer
    Private m_lTransactionID As Integer
    Private m_cTransactionAmount As Decimal

    'Notation for New Business (No Transaction)
    Private m_iNewBusinessNoTrans As Integer

    Private m_bIsBackdatedMTARequired As Boolean
    Private m_sSelectedPolicyStatus As String = ""
    Private m_bBackDatedMTAsAllowed As Boolean
    Private m_bProcessSettleTransactions As Boolean
    Private m_bIsTrueMonthlypolicyandNextInstalmentRenewal As Boolean

    Dim m_iPolicyRenewalStatus As Integer
    Dim m_iPolicyMakeLiveStatus As Integer
    Private m_cRoundOffAmount As Decimal
    Private m_iRenewalProcessMode As Integer

    Private m_bIsReadyToAccept As Boolean
    Private m_bBackdatedEditing As Boolean
    Private m_bIsAllRiskQuoted As Boolean
    Private m_bProcessWithAmend As Boolean
    Private m_sApplyMTATaxRatesonRen As String = ""

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

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 07/06/2000 DC - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 07/06/00 DC - Created.
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
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: Start
    '
    ' Description:
    '
    ' History: 07/06/00 DC - Created.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = ProcessInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' GetKeys
    ''' </summary>
    ''' <param name="vKeyArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Start (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx)
            'Increasing the size of the array to pass policy make live status
            ' Return the insurance file/folder counts
            'Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
            ReDim vKeyArray(1, 20)
            'End (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx)

            ' Insurance File Cnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "risk_id"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lRiskId

            ' payment terms

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = gSIRLibrary.SIRLookupPaymentMethod

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_sPaymentTerms

            ' pfprem finance cnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameFinancePlanCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lPFPremFinanceCnt

            ' pfprem finance version

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameFinancePlanVersion

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_lPFPremFinanceVersion


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = "quote_status"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_sQuoteStatus


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.ACTKeyNameMTAType

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_iMTAType

            'Float Balance and Pre-Payment (RC)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = "Payment Account ID"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_lPaymentAccountID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = "Debit Against"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = m_iDebitAgainst


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = "Credit Transactions"


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = m_vCreditTransactions


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = "Cash List ID"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = m_lCashListID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = "Cash ListItem ID"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = m_lCashListItemID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 11) = "TransactionID"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 11) = m_lTransactionID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 12) = "TransactionAmount"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 12) = m_cTransactionAmount


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 13) = PMNavKeyConst.PMKeyNameQuoteRef

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 13) = m_sOldPolicyNumber


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 14) = PMNavKeyConst.PMKeyNameIsOutOfSequence

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 14) = m_bIsBackdatedMTARequired


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 15) = PMNavKeyConst.PMKeyNameDebitTransactions


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 15) = m_vDebitTransactions


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 16) = PMNavKeyConst.PMKeyNameSettleTransactions

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 16) = m_bProcessSettleTransactions

            'Start (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 17) = PMNavKeyConst.PMKeyNamePolicyMakeLiveStaus

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 17) = m_iPolicyMakeLiveStatus
            'Start (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx)

            'Start - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 18) = PMNavKeyConst.PMKeyNameRoundOffAmount

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 18) = m_cRoundOffAmount
            'End - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 19) = "Is_Ready_To_Accept"
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 19) = m_bIsReadyToAccept

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 20) = "Is_All_Risks_Quoted"
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 20) = m_bIsAllRiskQuoted

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetKeys
    '
    ' Description:
    '
    ' History: 07/06/00 DC - Created.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sLocalProductCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(vKeyArray) Then
                Return result
            End If


            m_vKeyArray = VB6.CopyArray(vKeyArray)

            For iLoop As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)


                Select Case vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop)
                    Case PMNavKeyConst.PMKeyNamePartyCnt

                        m_lPartyCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))

                    Case PMNavKeyConst.PMKeyNameShortName

                        m_sShortName = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))

                    Case PMNavKeyConst.PMKeyNameInsFileCnt

                        m_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))

                    Case PMNavKeyConst.PMKeyNameInsFolderCnt

                        m_lInsuranceFolderCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))

                    Case PMNavKeyConst.PMKeyNameTaskGroupCode 'aka "Product_code"
                        sLocalProductCode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop)).Trim().ToUpper()
                        Select Case sLocalProductCode
                            Case "R", "REN"
                                m_sTaskGroupCode = "REN"
                            Case "N", "NB", "NEW BUSINESS"
                                m_sTaskGroupCode = "NB"
                            Case "M", "MTA"
                                m_sTaskGroupCode = "MTA"
                            Case Else
                        End Select

                    Case PMNavKeyConst.PMKeyNameOperateMode

                        m_iMode = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))

                    Case "RoadmapDisablesInstalments"

                        m_bRoadmapDisablesInstalments = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))
                    Case PMNavKeyConst.PMKeyNameSpecifiedTab

                        m_iSpecifiedTab = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))

                        ' RAM20050825 - PN 23018
                    Case PMNavKeyConst.PMKeyNameOriginalInsuranceFileCnt

                        m_lOriginalInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))
                        'PN 43045 --SUR
                    Case PMNavKeyConst.PMKeyNameNewBusinessNoTrans

                        m_iNewBusinessNoTrans = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))
                        'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
                    Case "SelectedPolicyStatus"

                        m_sSelectedPolicyStatus = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))
                    Case "BackDatedMTAsAllowed"

                        m_bBackDatedMTAsAllowed = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))
                        'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
                        'Start (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)
                    Case "IsTrueMonthlypolicyandNextInstalmentRenewal"
                        m_bIsTrueMonthlypolicyandNextInstalmentRenewal = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))
                        ' Get policy renewal status if provided
                    Case PMNavKeyConst.PMKeyNamePolicyRenewalStatus
                        m_iPolicyRenewalStatus = gPMFunctions.ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop), 0)
                        'End (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)
                    Case PMNavKeyConst.PMKeyNameRenewalProcessMode
                        m_iRenewalProcessMode = gPMFunctions.ToSafeLong(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop), 0)
                    Case PMNavKeyConst.PMKeyNameOutOfSequenceEditing
                        m_bBackdatedEditing = CBool(vKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, iLoop))
                    Case "processwithamend"
                        m_bProcessWithAmend = CBool(vKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, iLoop))
                    Case "ApplyMTATaxRatesonRen"
                        m_sApplyMTATaxRatesonRen = CStr(vKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, iLoop))
                End Select

            Next iLoop

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetSummary
    '
    ' Description:
    '
    ' History: 07/06/00 DC - Created.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sOldPolicyNumber.Trim() = m_sNewPolicyNumber.Trim() Then
                Return result
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ReDim vSummaryArray(2, 0)

            ' Assign the key array with the parameter members.

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, 0) = "Insurance_Ref"

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0) = m_sNewPolicyNumber

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' History: 07/06/00 DC - Created.
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

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' ProcessInterface
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProcessInterface() As Integer

        Dim nResult As Integer = 0
        Dim oInterface As frmInterface

        nResult = gPMConstants.PMEReturnCode.PMTrue

        ' Create a new instance of the form
        oInterface = New frmInterface()

        ' Set the party count
        With oInterface
            .PartyCnt = m_lPartyCnt
            .ShortName = m_sShortName
            .InsuranceFolderCnt = m_lInsuranceFolderCnt
            .InsuranceFileCnt = m_lInsuranceFileCnt
            .Task = m_iTask
            .TransactionType = m_sTransactionType
            If m_sTransactionType = "REN" Then
                .InstalmentProductCode = "REN"
            ElseIf m_sTransactionType = "MTR" Then
                .InstalmentProductCode = "NB"
            Else
                .InstalmentProductCode = m_sTaskGroupCode
            End If
            .SetSpecifiedTab = m_iSpecifiedTab
            .OriginalInsuranceFileCnt = m_lOriginalInsuranceFileCnt ' RAM20050825 - PN 23018
            .NewBusinessNoTrans = m_iNewBusinessNoTrans ' PN 43045 --SUR
            .SelectedPolicyStatus = m_sSelectedPolicyStatus
            .BackDatedMTAsAllowed = m_bBackDatedMTAsAllowed
            .IsTrueMonthlypolicyandNextInstalmentRenewal = m_bIsTrueMonthlypolicyandNextInstalmentRenewal
            ' Pass the policy renewal status to interface
            .PolicyRenewalStatus = m_iPolicyRenewalStatus
            .RenewalProcessMode = m_iRenewalProcessMode
            .BackdatedEditing = m_bBackdatedEditing
            .ProcessWithAmend = m_bProcessWithAmend
            .ApplyMTATaxRatesonRen = m_sApplyMTATaxRatesonRen
        End With

        ' Show the form
        oInterface.ShowDialog()

        ' Get the insurance file cnt
        With oInterface
            m_lRiskId = .RiskId
            m_lStatus = .Status
            m_lPFPremFinanceCnt = .PFPremFinanceCnt
            m_lPFPremFinanceVersion = .PFPremFinanceVersion
            m_sPaymentTerms = .PaymentTerms
            m_sQuoteStatus = .QuoteStatus
            m_sNewPolicyNumber = .NewPolicyNumber
            m_sOldPolicyNumber = .OldPolicyNumber
            m_iMTAType = .MTAType
            'Float Balance and Pre-Payment (RC)
            m_lPaymentAccountID = .PaymentAccountID
            m_iDebitAgainst = .DebitAgainst
            m_vCreditTransactions = .CreditTransactions
            m_vDebitTransactions = .DebitTransactions
            m_lCashListID = .CashListID
            m_lCashListItemID = .CashListItemID
            m_lTransactionID = .TransactionID
            m_cTransactionAmount = .TransactionAmount
            m_bIsBackdatedMTARequired = .IsBackDatedMTA
            m_bProcessSettleTransactions = .ProcessSettleTransactions
            m_iPolicyMakeLiveStatus = .PolicyMakeLiveStatus
            m_cRoundOffAmount = .RoundOffAmount
            m_bIsReadyToAccept = .IsReadyToAccept
            m_bIsAllRiskQuoted = .IsAllRiskQuoted
        End With

        ' Remove the instance
        oInterface.Close()
        oInterface = Nothing

        Return nResult

    End Function
End Class

