Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports System.Windows.Forms
Imports System.Xml

Imports SharedFiles
Public Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 06 May 1997
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    '               AMB 11/02/2003: PS220 - added insured_name, insured_account
    '               and flag columns/fields for IAG PS220 Manage Debtors
    ' RAW 15/05/2003 : CQ954 : added constants for icon images
    ' CJB 25/03/2004 : Added 'comment' (from TransDetail) to array to be be shown
    '                  in a tooltip). If there is a comment then show a notes
    '                  icon in the listview in place of the std one. Also allow
    '                  comments to be added/edited from a new right mouse button
    '                  click menu item.
    ' CJB 30/03/2004 : Added 'not_reported' (from TransDetail) to array to be
    '                  toggled on and off from a new right mouse button
    '                  click menu item. This will dictate if transactions will
    '                  appear on an agent's statement report or not.
    ' ***************************************************************** '
    'replaced iPMFunc.GetResData with GetResData in the whole document

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iACTFindTransaction"
    Public arrControlList As ArrayList
    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Username.
    Public g_sUsername As New FixedLengthString(12)

    Public g_iUserID As Integer
    ' Password.

    Public g_sPassword As New FixedLengthString(30)
    ' Calling Application
    <ThreadStatic()> _
    Public g_sCallingAppName As String = ""
    ' Company ID
    <ThreadStatic()> _
    Public g_iCompanyID As Integer
    ' Currency ID
    <ThreadStatic()> _
    Public g_iCurrencyID As Integer
    ' LogLevel
    <ThreadStatic()> _
    Public g_iLogLevel As Integer

    'DD 03/06/2003: Added for Locking
    <ThreadStatic()> _
    Public g_oAccount As Object

    ' General Icons

    ' Form
    Public Const ACInterfaceTitleRollup As Integer = 99
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle As Integer = 101
    Public Const ACTabTitleCount As Integer = 4

    ' ColumnHeaders
    Public Const ACListTitle As Integer = 110
    'eck090500
    Public Const ACListTitleCount As Integer = 18

    ' Labels
    Public Const ACDocumentRef As Integer = 130
    Public Const ACDocTypeGroup As Integer = 131
    Public Const ACDocumentType As Integer = 132
    Public Const ACPeriod As Integer = 133
    Public Const ACDateFrom As Integer = 134
    Public Const ACDateTo As Integer = 135
    Public Const ACCurrency As Integer = 136
    Public Const ACCurrencyAmount As Integer = 137
    Public Const ACTolerance As Integer = 138
    Public Const ACInsuranceRef As Integer = 139
    Public Const ACOperatorName As Integer = 140
    Public Const ACPurchaseInvoiceNo As Integer = 141
    Public Const ACPurchaseOrderNo As Integer = 142
    Public Const ACDepartment As Integer = 143
    Public Const ACSpare As Integer = 144
    Public Const ACAccountCode As Integer = 150
    Public Const ACContactName As Integer = 151
    Public Const ACTelephone As Integer = 152
    Public Const ACAccountBalance As Integer = 153
    Public Const ACAccountName As Integer = 154
    Public Const ACBalanceOption As Integer = 155
    Public Const ACInsuredAccount As Integer = 175
    Public Const ACAmountColumn As Integer = 176
    Public Const ACOutstandingColumn As Integer = 177
    Public Const ACTransactionCurrency As Integer = 178
    Public Const ACBaseCurrency As Integer = 179
    Public Const ACAccountCurrency As Integer = 180
    Public Const ACBranch As Integer = 181
    Public Const ACPolicyBalance As Integer = 182
    Public Const ACPremiumFinanceBalance As Integer = 183
    Public Const ACSelectedItemBalance As Integer = 184
    Public Const ACAccountType As Integer = 185

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACNewButton As Integer = 204
    Public Const ACEditButton As Integer = 205
    Public Const ACFindNowButton As Integer = 206
    Public Const ACNewSearchButton As Integer = 207
    Public Const ACFindAccTransButton As Integer = 208
    Public Const ACFindDocTransButton As Integer = 209
    'EK 020300
    Public Const ACMatchTransButton As Integer = 210
    Public Const ACReverseButton As Integer = 211
    '19/11/2002 - PWC - Added button for Automatic Allocation
    Public Const ACViewAllocationButton As Integer = 212

    'Added Button for Reverse and Replace Transaction
    Public Const ACReverseAndReplaceButton As Integer = 215

    ' Messages
    '19/11/2002 - PWC - Renumbered for Automatic Allocation
    Public Const ACCancelDetailsTitle As Integer = 700
    Public Const ACCancelDetails As Integer = 701
    Public Const ACBusinessFailTitle As Integer = 702
    Public Const ACBusinessFail As Integer = 703

    Public Const ACClearDetailsTitle As Integer = 704
    Public Const ACClearDetails As Integer = 705
    Public Const ACStatusSearching As Integer = 706
    Public Const ACStatusFound As Integer = 707
    Public Const ACAllTypes As Integer = 708
    Public Const ACYes As Integer = 709
    Public Const ACNo As Integer = 710
    Public Const ACPrimaryForAllocation As Integer = 711
    Public Const ACSecondaryForAllocation As Integer = 712

    '19/11/2002 - PWC - Added mesaages for Automatic Allocation
    Public Const ACNoSelectionTitle As Integer = 713
    Public Const ACNoSelectionDetails As Integer = 714
    Public Const ACNotAllocatedTitle As Integer = 715
    Public Const ACNotAllocatedDetails As Integer = 716
    Public Const ACConfirmReversalTitle As Integer = 717
    Public Const ACConfirmReversalDetails As Integer = 718
    Public Const ACConfirmReversalAndReplaceTitle As Integer = 719

    ' Messages
    ' AMB 12/02/2003: PS220 - write-off messages
    Public Const ACWriteOffNotAllowedInsurancePolicy As Integer = 719
    Public Const ACWriteOffNotAllowedPendingAccount As Integer = 720
    Public Const ACWriteOffNotAllowedTitle As Integer = 721
    Public Const ACWriteOffNotAllowedError As Integer = 765
    Public Const ACAddAuditSet As Integer = 722
    Public Const ACAddAuditSetTitle As Integer = 723
    Public Const ACSelectUserGroup As Integer = 724
    Public Const ACSelectUserGroupTitle As Integer = 725
    Public Const ACSelectAddTask As Integer = 726
    Public Const ACSelectAddTaskTitle As Integer = 727
    Public Const ACWriteOffDone As Integer = 729
    Public Const ACWriteOffDoneTitle As Integer = 728
    Public Const ACShowNoteEntry As Integer = 735
    Public Const ACShowNoteEntryTitle As Integer = 736
    Public Const ACShowRefund As Integer = 737
    Public Const ACShowRefundTitle As Integer = 738
    Public Const ACBankAccountDefaultAccountID As Integer = 739
    Public Const ACBankAccountDefaultAccountIDTitle As Integer = 740
    Public Const ACShowCashList As Integer = 741
    Public Const ACShowCashListTitle As Integer = 742
    Public Const ACShowCashListItem As Integer = 743
    Public Const ACShowCashListItemTitle As Integer = 744
    Public Const ACRefundComplete As Integer = 745
    Public Const ACRefundCompleteTitle As Integer = 746
    ' AMB 26/02/2003: PS220 - approve write off check messages
    Public Const ACApproveWriteOffExceedError As Integer = 747
    Public Const ACApproveWriteOffTitle As Integer = 748
    Public Const ACApproveWriteOffYourselfError As Integer = 749
    Public Const ACApproveWriteOffConfirm As Integer = 750
    Public Const ACApproveWriteOffConfirmTitle As Integer = 751
    Public Const ACApproveWriteOffError As Integer = 752
    Public Const ACApproveWriteOffErrorTitle As Integer = 753
    Public Const ACApproveWriteOffDone As Integer = 754
    Public Const ACApproveWriteOffDoneTitle As Integer = 755
    ' AMB 26/02/2003: PS220 - reject write-off message constants
    Public Const ACRejectWriteOffSingle As Integer = 756
    Public Const ACRejectWriteOffMultiple As Integer = 762
    Public Const ACRejectWriteOffTitle As Integer = 757
    Public Const ACRejectWriteOffError As Integer = 758
    Public Const ACRejectWriteOffErrorTitle As Integer = 759
    Public Const ACRejectWriteOffDone As Integer = 760
    Public Const ACRejectWriteOffDoneTitle As Integer = 761
    Public Const ACRejectGetUserID As Integer = 763
    Public Const ACRejectGetUserIDTitle As Integer = 764

    'TR 18/02/2003: TS220 Transfer messages
    Public Const ACConfirmTransferAmount As Integer = 730
    Public Const ACTransferAmount As Integer = 731
    Public Const ACTransfer As Integer = 732
    Public Const ACTransferAmountTooMuch As Integer = 733
    Public Const ACTransferAmountNotNumeric As Integer = 734
    ' Menus

    ' Listview Columnheaders
    Public Const ACListColBranch As Integer = 110
    Public Const ACListColAccount As Integer = 111
    Public Const ACListColDocRef As Integer = 112
    Public Const ACListColEffectiveDate As Integer = 113
    Public Const ACListColTransDate As Integer = 114
    Public Const ACListColAmount As Integer = 115
    Public Const ACListColPrimarySettled As Integer = 116
    Public Const ACListColOSAmount As Integer = 117
    Public Const ACListColPaidDate As Integer = 118
    Public Const ACListColDocType As Integer = 119
    Public Const ACListColInsuranceRef As Integer = 120
    Public Const ACListColOperatorName As Integer = 121
    Public Const ACListColPeriod As Integer = 122
    Public Const ACListColDocGroup As Integer = 123
    Public Const ACListColMediaRef As Integer = 124
    Public Const ACListColPurchaseInvoiceNo As Integer = 125
    Public Const ACListColPurchaseOrderNo As Integer = 126
    Public Const ACListColDepartment As Integer = 127
    Public Const ACListColAccountCode As Integer = 128
    Public Const ACListColClaimRef As Integer = 129
    ' Showing the ListHeader
    Public Const ACListColBalanceType As Integer = 156

    Public Const ACListColFlag As Integer = 800
    Public Const ACListColInsuredName As Integer = 801
    Public Const ACListColInsuredAccount As Integer = 802
    Public Const ACListColPayeeName As Integer = 806
    Public Const ACListColOSCredit As Integer = 807
    Public Const ACListColOSDebit As Integer = 808
    Public Const ACListColUnderwritingYear As Integer = 811
    Public Const ACListColMediaType As Integer = 812
    Public Const ACListColPaymentDueDate As Integer = 813
    Public Const ACListColRiskTransfer As Integer = 814
    Public Const ACListColAgentName As Integer = 815
    Public Const ACListColBGRef As Integer = 816
    Public Const ACListColDueDate As Integer = 817
    Public Const ACListColMatchAmount As Integer = 818
    Public Const ACListColCaseNumber = 819
    Public Const ACListColPeriodEndDate = 820
    Public Const ACListColOSAmountCredit = 821
    Public Const ACListColAmountCredit = 822

    ' Work Manager Task Description
    Public Const ACIWorkManagerTaskWriteOffDesc As Integer = 803
    Public Const ACIWorkManagerTaskRefundDesc As Integer = 804
    Public Const ACIWorkManagerTaskRejectDesc As Integer = 805

    ' AMB 19/02/2003: PS220 - refund form
    ' Refund Form
    Public Const ACRefundInterfaceTitle As Integer = 300
    Public Const ACRefundOKCaption As Integer = 200
    Public Const ACRefundCancelCaption As Integer = 201
    Public Const ACRefundPaymentMethodLabel As Integer = 301
    Public Const ACRefundRefundAmountLabel As Integer = 302
    Public Const ACRefundAmountExceeded As Integer = 303

    ' Constants for the lookup table array indexes.
    Public Const ACLDocumentType As Integer = 0
    Public Const ACLDocTypeGroup As Integer = 1
    Public Const ACLMax As Integer = 1

    ' Constants for data array indexes.
    Public Const ACIDocumentRef As Integer = 0
    Public Const ACIAccountingDate As Integer = 1
    Public Const ACIPeriodName As Integer = 2
    Public Const ACICurrencyAmount As Integer = 3
    Public Const ACIPrimarySettled As Integer = 4
    Public Const ACIMatchedCurrencyAmount As Integer = 5
    Public Const ACIDocumentTypeId As Integer = 6
    Public Const ACIDocTypeGroupId As Integer = 7
    Public Const ACIInsuranceRef As Integer = 8
    Public Const ACIOperatorName As Integer = 9
    Public Const ACIPurchaseInvoiceNo As Integer = 10
    Public Const ACIPurchaseOrderNo As Integer = 11
    Public Const ACIDepartment As Integer = 12
    Public Const ACISpare As Integer = 13
    Public Const ACIAccountShortCode As Integer = 14
    Public Const ACIAccountId As Integer = 15
    Public Const ACICurrencyID As Integer = 16
    Public Const ACITransDetailId As Integer = 17
    Public Const ACIBaseAmount As Integer = 18
    Public Const ACIDocumentSequence As Integer = 19
    Public Const ACIDocumentDate As Integer = 20
    Public Const ACISourceID As Integer = 21
    Public Const ACIMatchAmount As Integer = 22
    Public Const ACIMatchDate As Integer = 23
    Public Const ACIReason As Integer = 24
    Public Const ACIInsuredName As Integer = 25
    Public Const ACIInsuredAccount As Integer = 26
    Public Const ACIFlag As Integer = 27
    Public Const ACIDocInsuranceFileCnt As Integer = 28
    Public Const ACIDocDocumentID As Integer = 29
    Public Const ACIAuditSetID As Integer = 30
    Public Const ACIAuditSetUserID As Integer = 31
    Public Const ACITransCurrencyBaseXRate As Integer = 32
    Public Const ACIPayeeName As Integer = 33
    Public Const ACIAlternateReference As Integer = 34
    Public Const ACIPolicyTypeId As Integer = 35
    Public Const ACIComment As Integer = 36
    Public Const ACINotReported As Integer = 37
    Public Const ACIUnderwritingYear As Integer = 38
    Public Const ACIMediaType As Integer = 39
    Public Const ACICurrencyText As Integer = 40
    Public Const ACIAmountCurrencyText As Integer = 41
    Public Const ACIAmountCurrencyID As Integer = 42
    Public Const ACIAccountCurrencyID As Integer = 43
    Public Const ACIAccountAmount As Integer = 44
    Public Const ACIOutstandingBaseAmount As Integer = 45
    Public Const ACIOutstandingAccountAmount As Integer = 46
    Public Const ACIAmountUpdated As Integer = 47
    Public Const ACIOutstandingTransAmount As Integer = 48
    Public Const ACIPeriodEndDate As Integer = 49
    'S4B Claim Enhancements R&D 2005
    Public Const ACIClaimReference As Integer = 50
    Public Const ACIPaymentDueDate As Integer = 51
    'S4B FSA Risk Transfer Status
    Public Const ACIRiskTransfer As Integer = 52
    ' RDC 07112005
    Public Const ACITaxBandCode As Integer = 50
    Public Const ACITaxGroupCode As Integer = 51
    Public Const ACIAllocSequence As Integer = 52
    Public Const ACIAllocRule As Integer = 53
    Public Const ACIDetailTypeCode As Integer = 54
    '
    Public Const ACIBalanceType As Integer = 55
    Public Const ACIPartyType As Integer = 56
    Public Const ACIIsFlaotBalanceAccount As Integer = 57
    Public Const ACIIsOverdraftAccount As Integer = 58
    Public Const ACITransReference As Integer = 59
    Public Const ACIAgentName As Integer = 60
    Public Const ACIBGRef As Integer = 61


    Public Const ACCashListID As Integer = 68
    Public Const ACIsSplitReceipt As Integer = 69
    Public Const ACIsLead As Integer = 70
    Public Const ACIDueDate = 71
    'always keep the following constant the last in the array
    Public Const ACICaseNumber As Integer = 72
    Public Const ACIMTAAgentName As Integer = 73
    Public Const kPendingApproval As Integer = 74
    Public Const kIsDebitOrderTransDetail As Integer = 75
    Public Const ACIIncludeTaxInInstalments As Integer = 76
    Public Const kCashListItemID As Integer = 77
    Public Const kBankAccountID As Integer = 78
    Public Const kCashlistRef As Integer = 79
    Public Const kAccount_Key As Integer = 80
    ' RDT 09/04/2003 - Reshuffle.  Added two new constants
    ' for ACListCurrencyAmountCredit and ACListOSCurrencyAmountCredit

    ' AMB 11/02/2003: added ACIHasUnrestrictedEnquiry constant to replace a hard-coded numeric
    Public Const ACIHasUnrestrictedEnquiry As Integer = 26

    'Constants for account.ledger_short_name
    Public Const ACCOUNT_LEDGER_CLIENT As String = "Client"
    Public Const ACCOUNT_LEDGER_AGENT As String = "Agent"
    Public Const ACCOUNT_LEDGER_OTHERPARTY As String = "Introducer"
    Public Const ACCOUNT_LEDGER_SUBAGENT As String = "Sub Agent"

    ' AMB 13/02/2003: PS220 - constant for adding a work manager task for write offs and refunds
    Public Const ACTION_KEY_APPROVE_BAD_DEBT As String = "ABD"
    Public Const CREATE_TASK_WRITEOFF_TASK_CODE As String = "FINDTXN"
    Public Const CREATE_TASK_WRITEOFF_TASK_GROUP As String = "PLACS"
    Public Const CREATE_TASK_REFUND_TASK_CODE As String = "APPROVE"
    Public Const CREATE_TASK_REFUND_TASK_GROUP As String = "PLACS"
    Public Const DEBTOR_USER_GROUP_TYPE_CODE_BAD_DEBT As String = "BD"
    Public Const DEBTOR_USER_GROUP_TYPE_CODE_REFUND As String = "REF"
    ' AMB 19/02/2003: PS220 - added for pending refund
    Public Const ACPaymentTypeDebtorRefund As String = "REFUND"
    Public Const PAYMENT_STATUS_PENDING_APPROVAL As String = "PENDING"
    ' AMB 24/02/2003: PS220 - auditset type constants
    Public Const AUDITSETTYPE_RECURRING As String = "REC"
    Public Const AUDITSETTYPE_REVERSAL As String = "REV"
    Public Const AUDITSETTYPE_APPROVED_BAD_DEBT As String = "ABD"
    Public Const AUDITSETTYPE_UNNAPPROVED_BAD_DEBT As String = "UBD"
    Public Const AUDITSETTYPE_PENDING_REFUND As String = "PR"
    Public Const AUDITSETTYPE_APPROVED_REFUND As String = "AR"
    ' AMB 28/02/2003: PS220 - auditeset_type_id's
    Public Const AUDITSETTYPE_ID_RECURRING As Integer = 1
    Public Const AUDITSETTYPE_ID_REVERSAL As Integer = 2
    Public Const AUDITSETTYPE_ID_APPROVED_BAD_DEBT As Integer = 3
    Public Const AUDITSETTYPE_ID_UNNAPPROVED_BAD_DEBT As Integer = 4
    Public Const AUDITSETTYPE_ID_PENDING_REFUND As Integer = 5
    Public Const AUDITSETTYPE_ID_APPROVED_REFUND As Integer = 6

    ' AMB 26/02/2003: PS220 - memo task constants
    Public Const CREATE_TASK_MEMO_TASK_CODE As String = "MEMO"
    Public Const CREATE_TASK_MEMO_TASK_GROUP As String = "COMMON"

    ' AMB 11/02/2003: PS220 - reordered
    Public Const ACListCheckboxID As Integer = 0
    Public Const ACListSourceID As Integer = 1
    ' AMB 11/02/2003: PS220 - added 'flag'
    Public Const ACListFlag As Integer = 2
    Public Const ACListBalanceType As Integer = 3
    Public Const ACListAccountShortCode As Integer = 4
    Public Const ACListDocumentRef As Integer = 5
    Public Const ACListAltRef As Integer = 6 '(RC) QBENZ014
    Public Const ACListAccountingDate As Integer = 7
    Public Const ACListDocumentDate As Integer = 8
    Public Const ACListDueDate As Integer = 9
    Public Const ACListMediaType As Integer = 10

    Public Const ACListCurrencyAmount As Integer = 11
    Public Const ACListCurrencyAmountCredit As Integer = 12
    Public Const ACListPrimarySettled As Integer = 13
    Public Const ACListOSCurrencyAmount As Integer = 14
    Public Const ACListOSCurrencyAmountCredit As Integer = 15
    Public Const ACListMatchDate As Integer = 16
    Public Const ACListPaymentDueDate As Integer = 17
    Public Const ACListDocumentTypeId As Integer = 18
    Public Const ACListInsuranceRef As Integer = 19
    Public Const ACListOperatorName As Integer = 20
    Public Const ACListPeriodName As Integer = 21
    Public Const ACListDocTypeGroupId As Integer = 22
    Public Const ACListInsuredName As Integer = 23
    Public Const ACListInsuredAccount As Integer = 24
    Public Const ACListSpare As Integer = 25
    Public Const ACListPurchaseInvoiceNo As Integer = 26
    Public Const ACListPurchaseOrderNo As Integer = 27
    Public Const ACListDepartment As Integer = 28
    Public Const ACListMatchAmount As Integer = 29
    Public Const ACListPayeeName As Integer = 30
    Public Const ACListUnderwritingYear As Integer = 31
    Public Const ACListPeriodEndDate As Integer = 32
    'S4B Claim Enhancements R&D 2005
    Public Const ACListClaimReference As Integer = 33
    Public Const ACListRiskTransfer As Integer = 34
    Public Const ACListAgentName As Integer = 35
    Public Const ACListBGRef As Integer = 36
    Public Const ACListCaseNumber = 37
    <ThreadStatic()> _
    Public g_sYes As String = ""
    <ThreadStatic()> _
    Public g_sNo As String = ""
    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the maxiumum search details.
    Public Const ACMaxSearchDetails As Integer = 500

    ' Constant for the miniumum search length.
    Public Const ACMinSearchLength As Integer = 3

    Public Const kUserAgentCnt As Integer = 6

    ' Public source and language ID's from the Object Manager.
    <ThreadStatic()> _
    Public g_iSourceID As Integer
    <ThreadStatic()> _
    Public g_iLanguageID As Integer

    ' Public instance of the object manager.
    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager

    ' Public instances of the business objects.
    <ThreadStatic()> _
    Public g_oBusiness As Object 'FindTransaction business

    ' Payment Maintenance - adding bACTPaymentMaintenance object

    <ThreadStatic()> _
    Public m_oFindPaymentMaintenance As bACTPaymentMaintenance.Form

    ' Payment Maintenance - adding bACTCurrencyConvert object
    <ThreadStatic()> _
    Public m_obACTCurrencyConvert As Object

    '2005 Client Manager Security
    <ThreadStatic()> _
    Public g_bReverseTransactionsAuthority As Boolean
    <ThreadStatic()> _
    Public g_bReverseAllocationsAuthority As Boolean
    <ThreadStatic()> _
    Public g_bPerformAllocationsAuthority As Boolean

    'Electra M3 Gaps Phase 1 - Find Transaction Changes
    <ThreadStatic()> _
    Public g_bReverseReplaceTransactionsAuthority As Boolean

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails As Object
    Private m_vLedgers As Object

    ' Stores the return value for a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    'Product Family Name for Help
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFOrion

    Public Const ScreenHelpID As Integer = 13000

    ' CTAF 130300
    Public Const ACTFindTransBalanceOption As String = "BalanceOption"

    ' Icon
    Public Const ACIconCheck As String = "check" ' RAW 15/05/2003 : CQ954 : added
    Public Const ACIconBlank As Integer = 0 ' RAW 15/05/2003 : CQ954 : added

    ' RDT 10/04/2003
    Public Const ACShowCreditDebit As Boolean = True

    'Dimensions in a standard PM 2d array
    Public Const klColDimension As Integer = 1
    Public Const klRowDimension As Integer = 2

    ' Payment Maintenance
    Public Const ACAllowReverse As Integer = 60
    Public Const ACReverseDays As Integer = 61
    Public Const ACHasPaymentAuthority As Integer = 10
    Public Const ACPaymentCurrencyId As Integer = 16
    Public Const ACPaymentAmount As Integer = 11
    Public Const ACHasClaimAuthority As Integer = 12
    Public Const ACClaimCurrencyId As Integer = 17
    Public Const ACClaimAmount As Integer = 13
    Public Const kSysOptSingleCashReceipt = 5087

    Public Const ACCanReverseReceiptArrPos As Integer = 78

    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.2.4)
    <ThreadStatic()> _
    Public g_objDOMRootForInterfaceDisplay As XmlDocument
    <ThreadStatic()> _
    Public g_sUserConfigXMLDataset As String = ""
    'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.2.4)

    ' Global application functions

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Public Function GetLookupValues() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Gets all of the lookup values.

            ReDim m_vLookupValues(3, ACLMax)

            ' Setup Lookup Table Names

            m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, ACLDocumentType) = gACTLibrary.ACTLookupDocumentType

            m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, ACLDocTypeGroup) = gACTLibrary.ACTLookupDocTypeGroup

            ' Do not supply a key
            For i As Integer = 0 To ACLMax

                m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupKey, i) = ""
            Next i

            ' Get all of the lookup values with the correct
            ' effective date.

            m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupRow
    '
    ' Description: Converts a lookup table name to its matching row index
    '              in the table of lookup values.
    '              May be used to indirect GetLookupDetails, GetLookupDesc.
    '              Returns -1 if no match found
    '
    ' ***************************************************************** '
    Public Function GetLookupRow(ByRef sLookupTable As String) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        Try

            result = -1

            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.

                If CStr(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            If bFoundMatch Then
                result = lRow
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to match lookup table", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupRow", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    Public Function GetLookupDetails(ByRef lLookupRow As Integer, ByRef ctlLookup As ComboBox, Optional ByVal vAllTypes As Object = Nothing) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if there has been a table match.
            If lLookupRow = -1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.

            Dim sLookupDesc As String = ""
            If Not Information.IsNothing(vAllTypes) Then

                If CBool(vAllTypes) Then
                    ' First entry is all types (don't care)

                    sLookupDesc = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAllTypes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    Dim newindex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem(sLookupDesc, -1))
                    'end
                End If
            End If

            For lCntr As Integer = CInt(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupStartPos, lLookupRow)) To CInt((CDbl(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupStartPos, lLookupRow)) + CDbl(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupNumOfItems, lLookupRow))) - 1)
                ' Add the details to the control.

                Dim newindex As Integer = ctlLookup.Items.Add(New ListBoxItem(m_vLookupDetails(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lCntr), m_vLookupDetails(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lCntr)))
                'end
                ' Check if this is the selected index.

                If m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupKey, lLookupRow).Equals(m_vLookupDetails(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lCntr)) Then

                    ctlLookup.SelectedIndex = newindex
                End If
            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.

            If CStr(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupKey, lLookupRow)) = "" Then

                ctlLookup.SelectedIndex = 0

            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDesc
    '
    ' Description: Gets a description string for a given lookup set
    '              and lookup id.
    '
    ' ***************************************************************** '
    Public Function GetLookupDesc(ByRef lLookupRow As Integer, ByRef lLookupID As Integer, ByRef sLookupDesc As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if there has been a table match.
            If lLookupRow = -1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Using the lookup values, populate the lookup
            ' string from the lookup details array when the
            ' lookup ID has been matched.

            For lCntr As Integer = CInt(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupStartPos, lLookupRow)) To CInt((CDbl(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupStartPos, lLookupRow)) + CDbl(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupNumOfItems, lLookupRow))) - 1)
                ' Check for a match on the ID.

                If CInt(m_vLookupDetails(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lCntr)) = lLookupID Then
                    ' Found a match

                    ' Store the details to the lookup string.

                    sLookupDesc = CStr(m_vLookupDetails(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lCntr)).Trim()

                    Exit For
                End If
            Next lCntr

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDesc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: OnColumnClick
    '
    ' Description: Called by a form's listview column click event
    '
    ' ***************************************************************** '
    Public Sub OnColumnClick(ByVal ListView As ListView, ByVal ColumnHeader As ColumnHeader)

        Try
            Dim iDirection As SortOrder
            Dim lColumnHeaderIndex As Integer

            lColumnHeaderIndex = ColumnHeader.Index + 1 - 1

            With ListView

                ' override period name ordering with period end date ordering
                ' as period name is a string and can be anything including a date.
                If lColumnHeaderIndex = ACListPeriodName Then
                    lColumnHeaderIndex = ACListPeriodEndDate
                End If
                If ListViewHelper.GetSortOrderProperty(ListView) = 1 Then
                    iDirection = SortOrder.Descending
                Else
                    iDirection = SortOrder.Ascending
                End If
                ' If date column clicked, then sort by date sort column
                'eck010800
                Select Case lColumnHeaderIndex
                    Case ACListSourceID
                        ListViewHelper.SetSortedProperty(ListView, False)
                        'Modified
                        'ListViewHelper.SetSortOrderProperty(ListView, (ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2)
                        ListViewHelper.SetSortOrderProperty(ListView, iDirection)
                        'Use the special sort function for numerics
                        ListViewFunc.ListViewSortByValue(ListView, lColumnHeaderIndex, ListViewHelper.GetSortOrderProperty(ListView))

                        'VB 17/02/2005 PN-18705 sorting for Period Column
                    Case ACListAccountingDate
                        'If (ColumnHeader.Index - 1 = ACListAccountingDate) Then
                        'Modified
                        'm_lReturn = CType(ListViewFunc.ListViewSortByDate(v_oListView:=ListView, v_iSourceColumn:=lColumnHeaderIndex, v_iDirection:=(ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2), gPMConstants.PMEReturnCode)
                        m_lReturn = CType(ListViewFunc.ListViewSortByDate(v_oListView:=ListView, v_iSourceColumn:=lColumnHeaderIndex, v_iDirection:=iDirection), gPMConstants.PMEReturnCode)
                        'VB 08/02/2005 PN-18514 sorting for Paid Date
                    Case ACListDocumentDate, ACListMatchDate, ACListPeriodEndDate
                        'Modified
                        'm_lReturn = CType(ListViewFunc.ListViewSortByDate(v_oListView:=ListView, v_iSourceColumn:=lColumnHeaderIndex, v_iDirection:=(ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2), gPMConstants.PMEReturnCode)
                        m_lReturn = CType(ListViewFunc.ListViewSortByDate(v_oListView:=ListView, v_iSourceColumn:=lColumnHeaderIndex, v_iDirection:=iDirection), gPMConstants.PMEReturnCode)
                        'eck240800 also sort for OSAmount
                    Case ACListCurrencyAmount, ACListOSCurrencyAmount
                        'Modified
                        'm_lReturn = CType(ListViewSortByStringVal(v_oListView:=ListView, v_iSourceColumn:=lColumnHeaderIndex, v_iDirection:=(ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2), gPMConstants.PMEReturnCode)
                        m_lReturn = CType(ListViewSortByStringVal(v_oListView:=ListView, v_iSourceColumn:=lColumnHeaderIndex, v_iDirection:=iDirection), gPMConstants.PMEReturnCode)
                        '        ElseIf (ColumnHeader.Index - 1 = .SortKey) Then
                    Case ListViewHelper.GetSortKeyProperty(ListView)
                        ' Set sort order opposite of
                        ' current direction.
                        'Modified
                        'ListViewHelper.SetSortOrderProperty(ListView, (ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2)
                        ListViewHelper.SetSortOrderProperty(ListView, iDirection)
                    Case Else
                        ' Sort by this column (ascending).
                        ListViewHelper.SetSortedProperty(ListView, False)

                        ' Turn off sorting so that the list
                        ' is not sorted twice
                        ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Ascending)
                        ListViewHelper.SetSortKeyProperty(ListView, lColumnHeaderIndex)
                        ListViewHelper.SetSortedProperty(ListView, True)
                End Select

            End With

        Catch excep As System.Exception

            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="OnColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Function DisplayMessage(ByRef r_lTitleId As Integer, ByRef r_lMessageId As Integer, ByRef r_lOptions As Integer, ByVal ParamArray r_vTokens() As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: DisplayMessage
        ' PURPOSE: Displays a message based on passed resource file Ids
        ' AUTHOR: Sirius Financial Systems Plc
        ' DATE: 09 October 2002, 16:03:54
        ' RETURNS: PMTrue for success
        ' CHANGES: PWC 16/10/2002 - Added param array to enable substition of tokens
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim sTitle, sMessage As String

        Try

            'Get the title from the res file

            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=r_lTitleId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Get the message from the res file

            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=r_lMessageId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Replace Tokens in the title
            ReplaceTokens(sTitle, New Object() {r_vTokens})

            'Replace Tokens in the message
            ReplaceTokens(sMessage, New Object() {r_vTokens})

            'Now display the message to the user
            result = Interaction.MsgBox(sMessage, r_lOptions, sTitle)


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayMessage", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

            End Select

        Finally


        End Try
        Return result
    End Function

    Public Function ReplaceTokens(ByRef r_sMessage As String, ByVal ParamArray r_vTokens() As Object) As Boolean
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ReplaceTokens
        ' PURPOSE: Replace place holders with values
        '          e.g. convert |Username| to "Fred Bloggs"
        ' AUTHOR: Paul Cunnigham
        ' DATE: 21 October 2002, 10:04:52
        ' RETURNS: True for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Boolean = False
        Dim lUpper As Integer
        Dim vToken, vValue As Object

        'This routine could be called directly like...
        '    ReplaceTokens sMessage, "Usename", m sUserName
        'With the params explicitly listed

        'OR by a routine that itself accepts a ParamArray.
        '    ReplaceTokens sMessage, r_vParams

        'We need to ensure that we find the 'root' ParamArray as the second
        'calling method would pass Variant(0)(0), Variant(0)(1) into this routine
        'and we need Variant(0), Variant(1)

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Find the 'root' paramarray
            '(i.e. convert  Variant(0)(0) to Variant(0))
            Dim vParams As Object = r_vTokens

            While vParams(0).GetType().Name = "Variant()"
                If Information.Err().Number <> 0 Then
                    'No params passed at all
                    Information.Err().Clear()
                    Return result
                End If

                vParams = vParams(0)

                If vParams.GetUpperBound(0) = -1 Then ' no params
                    Return result
                End If
            End While

            'Any params actually passed?
            'We could just have a blank paramarray
            lUpper = r_vTokens.GetUpperBound(0)
            If lUpper <> -1 Then
                'Loop through the param array
                For iItem As Integer = 0 To lUpper \ 2
                    'Get the token and its replacement value

                    vToken = vParams(iItem * 2)
                    'This will bomb if developer has passed an odd number of params

                    vValue = vParams(iItem * 2 + 1)

                    'Replace the token with the value

                    r_sMessage = r_sMessage.Replace("|" & CStr(vToken) & "|", CStr(vValue))
                Next
            End If

            result = gPMConstants.PMEReturnCode.PMTrue

            GoTo Finally_Renamed

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

Catch_Renamed:

            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ReplaceTokens", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    result = gPMConstants.PMEReturnCode.PMError

                    GoTo Finally_Renamed
            End Select

Finally_Renamed:
            Return result

        Catch exc As System.Exception
        End Try

    End Function

    ' ***************************************************************** '
    '
    ' Name: UnLockTransdetailId
    '
    ' Description:
    '
    ' History: 13/05/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function UnLockTransdetailId(ByVal v_vLockedTransDetailIds(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(v_vLockedTransDetailIds) Then
                Return result
            End If

            If g_oAccount Is Nothing Then
                Dim temp_g_oAccount As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_g_oAccount, "bACTAccount.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                g_oAccount = temp_g_oAccount
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to create instance of bACTAccount.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLockTransdetailId")
                    Return result
                End If
            End If

            m_lReturn = g_oAccount.DeleteAllocationLocks(v_vOSTransactions:=v_vLockedTransDetailIds)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="bACTAccount.Form.DeleteAllocationLocks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLockTransdetailId")
                Return result
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnLockTransdetailId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLockTransdetailId", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Public Function AddSourceArrayToDestinationArray(ByRef r_vSourceArray(,) As Object, ByRef r_vDestinationArray As Array) As Integer
    Public Function AddSourceArrayToDestinationArray(ByRef r_vSourceArray(,) As Object, ByRef r_vDestinationArray(,) As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AddSourceArrayToDestinationArray
        ' PURPOSE: Adds the source array elements to the destination array
        ' AUTHOR: Paul Cunningham
        ' DATE: 28 May 2003, 15:47:44
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACMethod As String = "AddSourceArrayToDestinationArray"

        Dim lSourceLowerRow, lSourceUpperRow, lSourceRowCount As Integer

        Dim lSourceLowerCol, lSourceUpperCol As Integer

        Dim lDestinationLowerRow, lDestinationUpperRow, lDestinationLowerCol, lDestinationUpperCol, lDestinationRow As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Get the number of new items to add
            lSourceLowerRow = r_vSourceArray.GetLowerBound(klRowDimension - 1)
            lSourceUpperRow = r_vSourceArray.GetUpperBound(klRowDimension - 1)
            lSourceRowCount = lSourceUpperRow - lSourceLowerRow + 1

            'Get the columns bounds as well
            lSourceLowerCol = r_vSourceArray.GetLowerBound(klColDimension - 1)
            lSourceUpperCol = r_vSourceArray.GetUpperBound(klColDimension - 1)

            '(Re)size the destination array
            If Information.IsArray(r_vDestinationArray) Then
                lDestinationLowerRow = r_vDestinationArray.GetLowerBound(klRowDimension - 1)
                lDestinationUpperRow = r_vDestinationArray.GetUpperBound(klRowDimension - 1)
                lDestinationLowerCol = r_vDestinationArray.GetLowerBound(klColDimension - 1)
                lDestinationUpperCol = r_vDestinationArray.GetUpperBound(klColDimension - 1)

                r_vDestinationArray = ArraysHelper.RedimPreserve(Of Object(,))(r_vDestinationArray, New Integer() {lDestinationUpperCol - lDestinationLowerCol + 1, lDestinationUpperRow + lSourceRowCount - lDestinationLowerRow + 1}, New Integer() {lDestinationLowerCol, lDestinationLowerRow})

                lDestinationRow = lDestinationUpperRow + 1
            Else
                r_vDestinationArray = Array.CreateInstance(GetType(Object), New Integer() {lSourceUpperCol - lSourceLowerCol + 1, lSourceUpperRow - lSourceLowerRow + 1}, New Integer() {lSourceLowerCol, lSourceLowerRow})

                lDestinationRow = lSourceLowerCol
            End If

            'Populate the destination array with details from the source array
            For lRow As Integer = lSourceLowerRow To lSourceUpperRow
                For lCol As Integer = lSourceLowerCol To lSourceUpperCol
                    r_vDestinationArray(lCol, lDestinationRow) = ToSafeString(r_vSourceArray(lCol, lRow))
                Next lCol
                lDestinationRow += 1
            Next lRow

            result = gPMConstants.PMEReturnCode.PMTrue


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError


            End Select

        Finally


        End Try
        Return result
    End Function

    ''' <summary>
    ''' Check if transaction is linked to Instalment with a Third Party Scheme
    ''' </summary>
    ''' <param name="sDocumentRef"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckIsLinkedToThirdPartyScheme(ByVal sDocumentRef As String) As Boolean
        Dim oBAllocation As Object = Nothing
        Dim result As Boolean = False

        Try
            m_lReturn = g_oObjectManager.GetInstance(oObject:=oBAllocation, sClassName:="bACTAllocation.Form", vInstanceManager:=PMGetViaClientManager)

            result = oBAllocation.IsLinkedToThirdPartyScheme(DocumentRef:=sDocumentRef)

        Catch excep As System.Exception

            ' This will cause calling app to fail safe
            result = True
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error determining Is Linked To Third Party Scheme", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIsLinkedToThirdPartyScheme", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return result
        Finally
            oBAllocation = Nothing
        End Try

        Return result

    End Function


    ' ***************************************************************** '
    ' Author: Kevin Grandison
    '
    ' Date: 02/09/2003
    '
    ' Name: ChkDocTypeIsInstalments
    '
    ' Description: Check Document type against a list.
    '
    ' ***************************************************************** '
    Public Function ChkDocTypeIsInstalments(ByVal v_sDocType As String) As Boolean

        Dim result As Boolean = False
        Try

            Select Case v_sDocType
                Case gACTLibrary.ACTAutoNumberRangeCodeIdr, gACTLibrary.ACTAutoNumberRangeCodeIdr, gACTLibrary.ACTAutoNumberRangeCodeIcr, gACTLibrary.ACTAutoNumberRangeCodeIca, gACTLibrary.ACTAutoNumberRangeCodeIND, gACTLibrary.ACTAutoNumberRangeCodeINC, gACTLibrary.ACTAutoNumberRangeCodeIED, gACTLibrary.ACTAutoNumberRangeCodeIEC, gACTLibrary.ACTAutoNumberRangeCodeIRD, gACTLibrary.ACTAutoNumberRangeCodeIRC
                    result = True
            End Select

            Return result

        Catch excep As System.Exception

            ' This will cause calling app to fail safe
            result = True

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error determining status of Document Type", vApp:=ACApp, vClass:=ACClass, vMethod:="ChkDocTypeIsInstalments", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: ListViewSortByStringValue
    '
    ' Description: Sorts the list view based on the column passed, and
    '              the order given.
    '
    ' Note : This is the copy of the original function of iPMListViewFunc.Bas
    '        with some changes particular to the issue no 32220
    ' ***************************************************************** '
    Public Function ListViewSortByStringVal(ByVal v_oListView As ListView, ByVal v_iSourceColumn As Integer, ByVal v_iDirection As SortOrder) As Integer

        Dim result As Integer = 0
        Const ACLVTag As String = "SORT_VALUE_HIDDEN"

        Dim cValue As Decimal
        Dim sValue As String = ""
        Dim iIndex As Integer
        Dim bNegative As Boolean
        Dim iLen As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the column
            'PSL 02/10/2003 Should be zero width as well
            v_oListView.Columns.Add(ACLVTag, "Internal Sort", CInt(VB6.TwipsToPixelsX(0)))

            ' Get the index of this new column, -1 because it's a sub item
            iIndex = v_oListView.Columns.Count - 1

            ' Not sorted yet
            ListViewHelper.SetSortedProperty(v_oListView, False)

            ' Add the items
            For lLoop1 As Integer = 1 To v_oListView.Items.Count

                If v_iSourceColumn = 0 Then
                    sValue = StringsHelper.Format(v_oListView.Items.Item(lLoop1 - 1).Text, "#,##0.00")
                Else

                    'PSL 05/08/2003 Issue 5830
                    'Changed various bits, so negative numbers, and various currency formats work
                    Dim dbNumericTemp5 As Double
                    If ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.IndexOf(" "c) + 1 Then
                        Dim dbNumericTemp3 As Double
                        Dim dbNumericTemp As Double
                        If Double.TryParse(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Substring(0, ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.IndexOf(" "c) + 1), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                            sValue = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Substring(0, ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.IndexOf(" "c) + 1)
                        ElseIf Double.TryParse(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Substring(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Length - (ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Length - (ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.IndexOf(" "c) + 1))), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                            sValue = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Substring(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Length - (ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Length - (ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.IndexOf(" "c) + 1)))
                            If ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.StartsWith("-") Then
                                sValue = CStr(CDbl(sValue) * -1)
                            End If
                        Else
                            iLen = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Trim().Length

                            For iCount As Integer = iLen To 1 Step -1
                                sValue = Mid(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text, iCount, 1)
                                Dim dbNumericTemp2 As Double
                                If Double.TryParse(sValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                                    sValue = Mid(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text, 1, iCount)
                                    Exit For
                                End If
                            Next
                        End If
                    ElseIf Not Double.TryParse(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                        iLen = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Trim().Length

                        For iCount As Integer = iLen To 1 Step -1
                            sValue = Mid(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text, iCount, 1)
                            Dim dbNumericTemp4 As Double
                            If Double.TryParse(sValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                                sValue = Mid(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text, 1, iCount)
                                Exit For
                            End If
                        Next

                    Else
                        sValue = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text
                    End If
                    sValue.TrimEnd()

                    If sValue.StartsWith("-") Then
                        sValue = Mid(sValue, 2, sValue.Length - 1)
                        bNegative = True
                    Else
                        bNegative = False
                    End If
                    If sValue.Substring(0, 1) < "0" Or sValue.Substring(0, 1) > "9" Then
                        sValue = sValue.Substring(sValue.Length - (sValue.Length - 1))
                    End If
                    If bNegative Then
                        cValue = 1000000000 - CDec(sValue)
                    Else
                        cValue = CDec(sValue) + 1000000000
                    End If
                    sValue = StringsHelper.Format(cValue, "0000000000.00")

                End If
                ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), iIndex).Text = sValue

            Next lLoop1

            ' Sort now
            ListViewHelper.SetSortOrderProperty(v_oListView, v_iDirection)

            ' Set the sort key
            ListViewHelper.SetSortKeyProperty(v_oListView, iIndex)

            ListViewHelper.SetSortedProperty(v_oListView, True)

            ' Remove the column now
            v_oListView.Columns.RemoveAt(iIndex)

            ' Reset the sort key
            'eck 010800
            '    v_oListView.SortKey = v_iSourceColumn%
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListViewSortByStringVal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewSortByStringVal", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Module
