Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System

Imports SharedFiles
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 11th July 1997
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iACTCashListItem"
    Public Const ACTKeyNameUnallocatedAmountForPost As String = "Unallocated_Amount_For_Post"
    Public Const ACTKeyNameIsUnallocatedAmountForPost As String = "Is_Unallocated_Amount_For_Post"

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons

    <ThreadStatic()> _
    Public m_ofrmList As frmList

    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101

    Public Const ACListTitle1 As Integer = 102
    Public Const ACListTitle2 As Integer = 103
    Public Const ACListTitle3 As Integer = 104
    Public Const ACListTitle4 As Integer = 105
    Public Const ACListTitle5 As Integer = 106
    'eck250800
    Public Const ACTotalLabel As Integer = 107

    ' Details Form
    Public Const ACDetailsInterfaceTitle As Integer = 120
    ' Alix - 11/11/2003
    Public Const ACDetailsTabTitle1 As Integer = 121
    Public Const ACDetailsTabTitle2 As Integer = 122
    Public Const ACDetailsTabTitle3 As Integer = 123
    Public Const ACDetailsTabTitle4 As Integer = 124
    Public Const ACDetailsTabTitle5 As Integer = 125
    Public Const ACDetailsTabTitle6 As Integer = 126

    Public Const ACDetailsTabTitle1_Brok As Integer = 400
    Public Const ACDetailsTabTitle2_Brok As Integer = 401
    Public Const ACDetailsTabTitle3_Brok As Integer = 402
    Public Const ACDetailsTabTitle4_Brok As Integer = 403
    Public Const ACDetailsTabTitle5_Brok As Integer = 404
    Public Const ACDetailsTabTitle6_Brok As Integer = 405
    ' /Alix

    ' CR/DR drop down
    Public Const ACDetailsCR As Integer = 130
    Public Const ACDetailsDR As Integer = 131

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACAddButton As Integer = 204
    Public Const ACRemoveButton As Integer = 205
    Public Const ACEditButton As Integer = 206
    Public Const ACClaimDebtButton As Integer = 207
    Public Const ACClaimSalvageButton As Integer = 208

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303
    Public Const ACShortNameErrorTitle As Integer = 304
    Public Const ACShortNameError As Integer = 305
    'DD 20/05/2002
    Public Const ACFailedValidationTitle As Integer = 306
    Public Const ACFailedValidationError As Integer = 307

    ' Menus

    ' Constants for the List data array subscripts.
    'BB

    Public Const ACSubCashListItemID As Integer = 0
    Public Const ACSubAllocationstatusID As Integer = 1
    Public Const ACSubMediatypeID As Integer = 2
    Public Const ACSubCashListID As Integer = 3
    Public Const ACSubAccountID As Integer = 4
    Public Const ACSubMediaRef As Integer = 5
    Public Const ACSubOurRef As Integer = 6
    Public Const ACSubTheirRef As Integer = 7
    Public Const ACSubAmount As Integer = 8
    Public Const ACSubContactName As Integer = 9
    Public Const ACSubAddress1 As Integer = 10
    Public Const ACSubAddress2 As Integer = 11
    Public Const ACSubAddress3 As Integer = 12
    Public Const ACSubAddress4 As Integer = 13
    Public Const ACSubPostalCode As Integer = 14
    Public Const ACSubAddressCountry As Integer = 15
    Public Const ACSubPaymentName As Integer = 16
    Public Const ACSubPaymentAccountCode As Integer = 17
    Public Const ACSubPaymentBranchCode As Integer = 18
    Public Const ACSubPaymentExpiryDate As Integer = 19
    Public Const ACSubPaymentReference1 As Integer = 20
    Public Const ACSubPaymentReference2 As Integer = 21
    'eck210501 renamed and used for transdetail _id
    'Public Const ACSubAccountShortCode = 22
    Public Const ACSubTransdetailId As Integer = 22
    Public Const ACSubRealAllocationID As Integer = 23
    'eck10070`
    Public Const ACSubLetter As Integer = 24
    'new Constants added for front office receipting SW 15-10-2002

    Public Const ACBatchid As Integer = 25
    Public Const ACPMUserid As Integer = 26
    Public Const ACTransactiondate As Integer = 27
    Public Const ACOriginalamount As Integer = 28
    Public Const ACAmounttendered As Integer = 29
    Public Const ACChange As Integer = 30
    Public Const ACvCashlistitemreceipttypeid As Integer = 31
    Public Const ACCashlistitemreceiptstatusid As Integer = 32
    Public Const ACCashlistitembankid As Integer = 33
    Public Const ACChequedate As Integer = 34
    Public Const ACCCnumber As Integer = 35
    Public Const ACCCexpirydate As Integer = 36
    Public Const ACCCstartdate As Integer = 37
    Public Const ACCCissue As Integer = 38
    Public Const ACCCpin As Integer = 39
    Public Const ACCCauthcode As Integer = 40
    Public Const ACReceiptdetails As Integer = 41
    Public Const ACCashlistitemreversepmuserid As Integer = 42
    Public Const ACCashlistitemreversereasonid As Integer = 43
    Public Const ACCashListItemPaymentTypeID As Integer = 44
    Public Const ACCashListItemPaymentMethodID As Integer = 45
    Public Const ACCashListItemPaymentStatusID As Integer = 46
    Public Const ACDatepresented As Integer = 47
    Public Const ACChequeinpossession As Integer = 48
    Public Const ACStoprequesteddate As Integer = 49
    Public Const ACStopprinteddate As Integer = 50
    Public Const ACStopconfirmationdate As Integer = 51
    Public Const ACReason As Integer = 52
    Public Const ACReplacescashlistitemid As Integer = 53
    Public Const ACInstalmentArray As Integer = 54
    Public Const ACSalvageArray As Integer = 55
    Public Const ACCLMUSRecoveryArray As Integer = 56
    Public Const ACCLMRVRecoveryArray As Integer = 57
    Public Const ACUnderwritingYearID As Integer = 58
    Public Const ACCurrencyBaseDate As Integer = 59
    Public Const ACCurrencyBaseXrate As Integer = 60
    Public Const ACAccountBaseDate As Integer = 61
    Public Const ACAccountBaseXrate As Integer = 62
    Public Const ACSystemBaseDate As Integer = 63
    Public Const ACSystemBaseXrate As Integer = 64
    Public Const ACOverrideReason As Integer = 65
    Public Const ACCCname As Integer = 66
    Public Const ACCCcustomer As Integer = 67
    Public Const ACCCmanualauthcode As Integer = 68
    Public Const ACCCtransactioncode As Integer = 69
    Public Const ACMediatype_IssuerID As Integer = 70

    'Party Bank Details
    Public Const ACBankPaymentTypeId As Integer = 71
    Public Const ACBaseAmount As Integer = 72
    Public Const ACCollectionDate As Integer = 73
    Public Const ACComments As Integer = 74
    Public Const ACBGPolicies As Integer = 75

    'WPR12- Enhancement Quote Collection Process
    Public Const ACBankLocation As Integer = 76
    Public Const ACBankBranch As Integer = 77
    Public Const ACChequeTypeId As Integer = 78
    Public Const ACCCBankId As Integer = 79
    Public Const ACCardTypeId As Integer = 80
    Public Const ACCardTransSlipNo As Integer = 81
    Public Const ACChequeClearingTypeId As Integer = 82

    ' WPR 51
    Public Const ACIsLeadAccount As Integer = 83
    Public Const ACSplitTotal As Integer = 84
    Public Const ACReceiptTypeIsInstalmentBased As Integer = 85

    Public Const kTaxBandID As Integer = 86
    Public Const kTaxAmount As Integer = 87
    Public Const kPMNavBatchKey As Integer = 88
    Public Const kBIC As Integer = 89
    Public Const kIBAN As Integer = 90
    Public Const ACInsuranceRef As Integer = 91

    'This needs to be a separate number as it is used to store the row ID.
    Public Const ACLastItem As Integer = 92

    'end of front office receipting changes

    ' WPR 51
    Public m_dSplitTotal As Decimal
    Public m_dSplitReceiptRunningTotal As Decimal
    Public m_bIsLeadAccount As Boolean
    Public m_bCollectionHasLead As Boolean
    Public m_sMediaRefLead As String
    Public m_lMediaTypeIDLead As Integer
    Public m_dChequeDateLead As Date
    Public m_sNameLead As String
    Public m_sPlanRef As String = String.Empty

    Public m_iMediaTypeID As Integer = 0
    Public m_sCCNameLead As String = ""
    Public m_sCCNumberLead As String = ""
    Public m_sCCExpiryDateLead As String = ""
    Public m_sCCissueLead As String = ""
    Public m_sCCpinLead As String = ""
    Public m_sCCstartdateLead As String = ""
    Public m_sCCauthcodeLead As String = ""
    Public m_sCCmanualauthcodeLead As String = ""
    Public m_sCCcustomerLead As String = ""
    Public m_sCCtransactioncodeLead As String = ""

    'Constants to Contain Array Subscripts for instalments
    'sw Front Office receipting 23-10-2002

    'sw added 16/12/2002, cater for new instalments functionality
    Public Const ACInstalmentsID As Integer = 0
    'increse vals by 1 16/12/2002
    Public Const ACPremFinanceCnt As Integer = 1
    Public Const ACPremFinanceVersion As Integer = 2
    Public Const ACInstalmentNumber As Integer = 3
    Public Const ACInstalmentDueDate As Integer = 4
    Public Const ACInstalmentFee As Integer = 5
    Public Const ACInstalmentAmount As Integer = 6
    Public Const ACInstalmentPlanRef As Integer = 7
    Public Const ACInstalmentFlagElement As Integer = 8
    Public Const ACPartialPaymentAmount As Integer = 9
    'AAB-08-Oct-2003 10:31 - to support displaying Deposit Instalments
    Public Const ACTransactionCode As Integer = 10
    Public Const ACReceiptDifferenceOption As Integer = 11
    Public Const ACUseTransCurrencyInPF As Integer = 12
    Public Const ACTransCurrencyCode As Integer = 13
    Public Const ACInstalmentReceiptAmount As Integer = 14
    Public Const ACInstalmentBaseAmmount As Integer = 15

    Public Const kWriteOffReasonId As Integer = 16
    ' START CHANGES - Changed By: AAB  - Changed On: 08-Oct-2003 10:32
    ' Added to use for Transactions codes
    Public Const PFTransactionCreate As Integer = 1
    Public Const PFTransactionCancel As Integer = 2
    Public Const PFTransactionFirst As Integer = 3
    Public Const PFTransactionOngoing As Integer = 4
    Public Const PFTransactionRepresent As Integer = 5
    Public Const PFTransactionLast As Integer = 6
    Public Const PFTransactionDeposit As Integer = 7
    ' END CHANGES - Changed By: AAB  - Changed On: 08-Oct-2003 10:32
    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    'SW Front Office receipting, define rtransaction type
    Public Const ACReceiptType As Integer = 2
    Public Const ACPaymentType As Integer = 1

    'sw front office receipting define Media Validation Codes
    Public Const ACMediaTypeCash As String = "CASH"
    Public Const ACMediaTypeCreditCard As String = "CC"
    Public Const ACMediaTypeBank As String = "BANK"
    Public Const ACMediaTypeBasic As String = "BASIC"
    'WPR12- Enhancement Quote Collection Process
    Public Const ACMediaTypeCheque As String = "CHEQUE"

    'sw payment maintenance 10-11-2002
    'define payment status code Issued and stopped
    Public Const ACStatusIssued As String = "ISS"
    Public Const ACStatusStopped As String = "STOPPED"
    Public Const ACStatusStopRequested As String = "STOPREQ"
    'AAB-03-Dec-2003 10:04 - to support ICB
    Public Const ACStatusPending As String = "PENDING"
    Public Const ACStatusPendingID As Integer = 7
    ' Public source and language ID's from the
    ' Object Manager.
    Public g_iSourceID As Integer
    Public g_iLanguageID As Integer
    Public g_sUserName As String = ""
    Public g_iCountryID As Integer

    Public Declare Function FindWindow Lib "user32.dll" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
    Declare Function IsWindow Lib "user32.dll" (ByVal hwnd As Integer) As Integer

    'eck070201 Extra variables for component services
    Public g_sPassword As String = ""
    Public g_iUserID As Integer
    Public g_iCurrencyID As Integer
    Public g_iLogLevel As Integer

    'jmf 11/9/2003

    <ThreadStatic()> _
    Public g_lLockedSalvagePartyId As Integer

    ' Public instance of the object manager.

    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager

    ' Instance of SolutionConfig

    <ThreadStatic()> _
    Public g_oSirConfig As bSIRSolutionConfig.Business

    <ThreadStatic()> _
    Private _g_oZipper As bPMZipper.Business = Nothing
    Public Property g_oZipper() As bPMZipper.Business
        Get
            If _g_oZipper Is Nothing Then
                _g_oZipper = New bPMZipper.Business()
            End If
            Return _g_oZipper
        End Get
        Set(ByVal Value As bPMZipper.Business)
            _g_oZipper = Value
        End Set
    End Property

    <ThreadStatic()> _
    Public g_iSolutionConfig As Integer

    'eck070201

    Private m_oSystemOption As bSIROptions.Business
    Private m_lReturn As Integer
    'eck070201

    <ThreadStatic()> _
    Public g_bChequeProduction As Boolean
    'AR20050210 - PN18698/PN18699 Was the account id passed

    <ThreadStatic()> _
    Public g_bHasAccountContext As Boolean

    Public Const ACNormalMode As Integer = 0
    Public Const ACMergeMode As Integer = 1
    ' CTAF 130600 - Modes for printing
    Public Const ACPrintMode As Integer = 2
    Public Const ACPrintSilentMode As Integer = 3

    'TN20010130 Start
    Public Const ACSpoolDocMode As Integer = 4
    Public Const ACSpoolReportMode As Integer = 5
    'TN20010130 End

    Public Const ACCreditIndexPosition As Integer = 0
    Public Const ACDebitIndexPosition As Integer = 1

    'sw front office receipting 04-11-2002, public constants used to identify
    'individual frames on the receipt tab
    Public Const ACChequeFrame As Integer = 0
    Public Const ACCreditCardFrame As Integer = 1
    Public Const ACClaimsFrame As Integer = 2
    Public Const ACReversalFrame As Integer = 3

    'sw front offfice receipting, receipt types
    Public Const ACStandardReceiptType As String = "STD"
    Public Const ACInstalmentReceiptType As String = "INST"
    Public Const ACClaimRecoveryReceiptType As String = "CLMREC"
    Public Const ACClaimSalvageReceiptType As String = "CLMSAL"
    Public Const ACMiscReceiptType As String = "MISC"
    Public Const ACReversalOfAnyType As String = "REVERSAL"

    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFOrion

    Public Const ScreenHelpID As Integer = 16000
    Public Const ScreenHelpID2 As Integer = 37000

    Public Const ACTClientCashRoadMap As String = "CLIENTCASH"

    '***************
    ' MEvans : 14-05-2003 : CQ 709
    ' Approval Types
    Public Const ACApprovalTypeClaimPayment As Integer = 1
    Public Const kApprovalTypeInsurerPayment As Integer = 2
    '***************

    Public Const kSystemOptionUWYearMandatory As Integer = 5012

    '2005 Client Manager Security

    <ThreadStatic()> _
    Public g_oUserAuthorities As Object

    <ThreadStatic()> _
    Public g_bCanPerformAllocationsAuthority As Boolean 'PN24911

    Public Const kClaimPaymentAccountDetailsClaimPaymentId As Integer = 0
    Public Const kClaimPaymentAccountDetailsTotalPaymentAmount As Integer = 1
    Public Const kClaimPaymentAccountDetailsAccountId As Integer = 2
    Public Const kClaimPaymentAccountDetailsCurrencyId As Integer = 3
    Public Const kClaimPaymentAccountDetailsMediaTypeID As Integer = 4
    Public Const kClaimPaymentAccountDetailsPayeeName As Integer = 5
    Public Const kClaimPaymentAccountDetailsBankName As Integer = 6
    Public Const kClaimPaymentAccountDetailsBankSortCode As Integer = 7
    Public Const kClaimPaymentAccountDetailsBankAccountNo As Integer = 8
    Public Const kClaimPaymentAccountDetailsPayeeCountry As Integer = 9
    Public Const kClaimPaymentAccountDetailsPayeeComments As Integer = 10
    Public Const kClaimPaymentAccountDetailsMediaRef As Integer = 11
    Public Const kClaimPaymentAccountDetailsPayeeAddress1 As Integer = 12
    Public Const kClaimPaymentAccountDetailsPayeeAddress2 As Integer = 13
    Public Const kClaimPaymentAccountDetailsPayeeAddress3 As Integer = 14
    Public Const kClaimPaymentAccountDetailsPayeeAddress4 As Integer = 15
    Public Const kClaimPaymentAccountDetailsPayeePostalCode As Integer = 16
    Public Const kClaimPaymentAccountDetailsThirdPartyReference As Integer = 17
    Public Const kClaimPaymentAccountDetailsOurReference As Integer = 20
    Public Const kClaimPaymentAccountDetailsPartyBankID As Integer = 21
    Public Const kClaimPaymentAccountDetailsBIC As Integer = 22
    Public Const kClaimPaymentAccountDetailsIBAN As Integer = 23

    ' cashlistitem receipt type lookup details
    Public Const kReceiptTypeDescription As Integer = 1
    Public Const kReceiptTypeCode As Integer = 2
    Public Const kReceiptTypeIsInstalmentBased As Integer = 3

    Public Const kBankGuaranteeColHIndexBGRef As Integer = 0
    Public Const kBankGuaranteeColHIndexDueDate As Integer = 1
    Public Const kBankGuaranteeColHIndexPolicyNo As Integer = 2
    Public Const kBankGuaranteeColHIndexPremiumAmount As Integer = 3
    Public Const kBankGuaranteeColHIndexPostedAmount As Integer = 4

    Public Enum ENBankGuarantee
        BGId
        BankNameId
        BankName
        BGRef
        DueDate
        InsuranceFileCnt
        InsuranceRef
        Amount
        OutstandingAmount
        SourceID
        SourceDescription
        ProductId
        ProductDescription
        SourceCode
        CoverStartDate
        ExpiryDate
        ProductCode
        AmtTobePosted
        LastItem = ENBankGuarantee.AmtTobePosted
    End Enum

    'Start - Sankar - PN 55288
    Public Const kCashListItemReceiptTypeCode As String = "BGDEPT"
    'End - Sankar - PN 55288
    Public Const kSysOptSingleCashReceipt = 5087
    Public Const kSysOptIncludeInsurerPaymentMultiStep As Integer = 5143
    'eck070201
    ' ***************************************************************** '
    ' Name: GetOption (Private)
    '
    ' Description: Get an option.
    '
    ' ***************************************************************** '
    Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oSystemOption Is Nothing Then

                ' Get an instance of the business object via
                ' the public object manager.
                Dim temp_m_oSystemOption As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oSystemOption, "bSIROptions.Business", vInstanceManager:="ClientManager")
                m_oSystemOption = temp_m_oSystemOption

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            m_lReturn = m_oSystemOption.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=r_sOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sOptionValue = "0"

            End If

            m_oSystemOption.Dispose()



            m_oSystemOption = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Sub Main()

    End Sub
    Sub New()
        Main()
    End Sub
    Sub JustForInvokeMain()
    End Sub
End Module