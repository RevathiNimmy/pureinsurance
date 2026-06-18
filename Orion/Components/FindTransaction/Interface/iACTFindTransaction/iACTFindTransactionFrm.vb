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
Imports System.Runtime.InteropServices

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 06 May 1997
    '
    ' Description: Main interface.
    '
    ' Edit History:
    '               AMB 11/02/2003 - added insured_name, insured_account
    '               and flag columns/fields for IAG PS220 Manage Debtors
    '               CJB 25/03/2004 - added new constant for a notes image
    '               to be used in the listview. Also a new tooltip class
    '               and current item index for tooltips. Further changes
    '               to process display of comments in tooltips and add/edit
    '               them via right mouse button click menus also done.
    '               CJB 30/03/2004 - added new right mouse button click
    '               menu to toggle not_reported flag for transactions.
    '               CJB 17/02/2006 - Changed SetInterfaceDefaults to use
    '               SetListIndex to set combo item to prevent errors. PN27383
    '***************************************************************** '
    'replaced iPMFunc.GetResData with GetResData in the whole document

    Dim frmViewAllocation As frmViewAllocation
    Dim frmRefund As frmRefund

    Private WithEvents m_oNavStart As iPMNavStart.Interface_Renamed
    Private m_bNavCompleted As Boolean
    Private m_bProcessComplete As Boolean

    Private Const vbFormCode As Integer = 0
    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const ACSecondaryImage As String = "SecondaryImage"
    Private Const ACPrimaryImage As String = "PrimaryImage"
    ' Constants to identify the type of transaction
    Private Const ACDocTypeNBDebit As Integer = 4
    Private Const ACDocTypeNBCredit As Integer = 5
    Private Const ACDocTypeRenwalDebit As Integer = 15
    Private Const ACDocTypeRenewalCredit As Integer = 16
    Private Const ACDocTypeEndorsmentDebit As Integer = 17
    Private Const ACDocTypeEndorsmentCredit As Integer = 18
    Private Const ACDocTypeTransferedDebit As Integer = 35
    Private Const ACDocTypeTransferedCredit As Integer = 36
    Private Const ACDocTypeShortPeriodDebit As Integer = 31
    Private Const ACDocTypeShortPeriodCredit As Integer = 32

    ' CJB 25/03/2004 Used to show a 'note' icon in the listview
    Private Const ACNoteImage As String = "NoteImage"

    ' CJB 01/04/2004 Used to show a 'cross' icon in the listview to denote
    ' transactions flagged as Do Not Report on the Business Transacted by Agent reports.
    Private Const ACNOTReported As String = "NotReported"

    Private Const cEnableClientManagerEditing As Integer = 5000
    Private Const kMultiCurrencyBanking As Integer = 5058
    Private Const OrionLink As String = "iPMBOrionLink"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer
    Private m_oInterface As Interface_Renamed
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTypeOfBusiness As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}

    Private m_oReplace As Object
    Private m_lTransdetailID As Integer
    Private m_lAccountID As Integer
    Private m_lSaveAccountID As Integer
    Private m_sDocumentRef As String = ""
    Private m_sInsuranceRef As String = ""
    Private m_lDrillLevel As Integer
    Private m_iDrillCompany As Integer
    'MKR 01/11/2004 PN 14833 changed the definition to non array variant...
    Private m_vTransdetailIDs As Object
    Private m_lAllocationTransType As Integer
    Private m_lAllocationID As Integer
    'eck170700
    Private m_lCashListTypeID As Integer
    'DD 11/04/2003: Insured Account Filter
    Private m_lInsuredAccountID As Integer
    'DD 21/08/2003: Document ID
    Private m_lDocumentId As Integer
    'DD 4/10/2004: Used for excluding during Cash List Allocation
    Private m_lExcludeTransDetailID As Integer

    Private m_lLedgerID As Integer
    Private m_sLedgerName As String = ""

    'DC140403 -ISS3503
    Private m_lCashListId As Integer

    Private m_bIsBatch As Boolean

    'RDT 10/04/2003: True if product option Display Debit/Credit is enabled
    Private m_bDisplayDebitCredit As Boolean

    'Other properties to be set/got ?

    ' What solution are we running as part of?
    'Private m_iSolutionConfig As Integer

    ' CF 120100
    ' Enable multi-currency?
    Private m_bEnableMultiCurrency As Boolean

    'eck140501
    Private m_iCompanyID As Integer
    'SP090102 - Merge catch up

    'DD 15/07/2002: True if new product option is enabled
    Private m_bEnhancedSecurity As Boolean

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iACTFindTransaction.General

    'Business Objects.
    Private m_oFindAccount As Object
    Private m_oUserAuthorities As Object
    Private m_oAllocation As Object
    Private m_oCurrency As Object
    Private m_oAccount As Object
    Private m_oCurrencyConvert As Object
    Private m_oFindCase As Object

    ' Stores the return value for a function call.
    Private m_lReturn As Integer

    'DC140706 Datasure
    Dim m_iAccountCurrencyID As Integer
    Dim m_iBaseCurrencyID As Integer
    Dim m_bMultipleOSCurrencies As Boolean
    Dim m_iBalanceCurrencyId As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the search data from the business object.
    Public m_vSearchData(,) As Object

    ' Balance only?
    Private m_bBalanceOnly As Boolean
    Private m_bOutstandingOnly As Boolean

    ' CF 050100
    ' Allow stopped accounts on Find Account
    Private m_bAllowStopped As Boolean

    Private m_vSourceArray As Object
    'VB 15/03/2005
    Private m_vSourceArrayCopy As Object

    'Thinh Nguyen 15/04/2002 (start)
    Private m_sUnderwritingOrAgency As String = ""
    'Thinh Nguyen 15/04/2002 (end)

    'eck220601
    Private m_bFirstTime As Boolean
    Private m_lAccountKey As Integer

    Private m_lHasTransWriteOffAuthority As Integer
    Private m_lHasRefundAuthority As Integer
    Private m_lHasTransferAuthority As gPMConstants.PMEReturnCode
    Private m_lHasUnrestrictedEnquiry As Integer 'AR20050414 - PN12439

    Private m_lSelectedCount As Integer
    Private m_crSelectedTotalAmount As Decimal
    Private m_vSelectedTransDetails As Object
    ' AMB 24/02/2003: PS220 - added action key
    Private m_sActionKey As String = ""

    '27/05/2003 - PWC - 186 - Debt Roll-up
    Private m_bRollup As Boolean
    Private m_vSearchParams(,) As Object

    'SJ 13/05/2003 - start
    Private m_oPMLock As Object
    Private m_vCheckSearchData(,) As Object
    Private m_cOSTransactions As ColWrapper
    'SJ 13/05/2003 - end

    'SJ 23/02/2004 - start
    Private m_bUnderwritingBranchEnabled As Boolean
    Private m_bIsUnderwritingBranch As Boolean
    'SJ 23/02/2004 - end

    Private m_lDisableReverse As gPMConstants.PMEReturnCode

    ' CJB 26/03/2004 New tooltip class (sourced by DD)
    Private m_oTTNew As ToolTip
    'Private m_oTT As CTooltip

    ' CJB 26/03/2004 Store current item index for use with tooltips
    Private m_lCurItemIndex As Integer

    ' DD 05/04/2004 - Added for Product Option Underwriting Year
    Private m_bUnderwritingYear As Boolean

    Private m_bMultiLedger As Boolean

    'DC310105 : Process Reversal Of Introducer Transaction
    Private m_bIntroducer As Boolean

    Private m_bShowPopupComments As Boolean

    'DC130606 show Year To Date figures for Datasure
    Private m_crYTDTurnover As Decimal
    Private m_crYTDIncome As Decimal
    Private m_crYTDNetIncome As Decimal
    Private m_crLastYearTurnover As Decimal
    Private m_crClientBalance As Decimal
    Private m_bCalledViaClientManager As Boolean
    Private m_cSelectedItemBalance As Decimal

    ' Payment Maintenance
    Private m_iAllowReverseAllocation As Integer
    Private m_iReverseAllocationDays As Integer
    Private m_vAllocationData(,) As Object
    Private m_iHasPaymentAuthority As Integer
    Private m_lPaymentCurrencyId As Integer
    Private m_crPaymentAmount As Decimal
    Private m_iHasClaimAuthority As Integer
    Private m_lClaimCurrencyId As Integer
    Private m_crClaimAmount As Decimal

    Private m_cBranchOSBalanceInBaseCurrency As Decimal
    Private m_sMultiCurrencyBankingOption As String = ""
    Private m_iSelectedSourceId As Integer
    Private m_iSelectedCurrencyId As Integer

    ' Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.3.1.1)
    Private m_bInsuredAccountView As Boolean
    ' End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.3.1.1)

    Private m_bOutstandingAmount As Boolean
    Private m_bEnhancedCaseSearching As Boolean

    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.2.4)
    Const m_kRootNode As String = "UserConfigXML"
    Const m_kComponentName As String = "iACTFindTransaction"
    Const m_kFrmName As String = "frmInterface"
    Const m_kGridName As String = "lvwSearchResults"
    Const m_kColumnSortKey As String = "ColumnSortKey"
    Const m_kColumnSortOrder As String = "ColumnSortOrder"
    Const m_kColumnWidth As String = "ColumnWidths"
    Private m_iSortKey As Integer
    Private m_iSortOrder As Integer
    'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.2.4)
    Private sortColumn As Integer = -1

    'Win32 API declarations to preserve list view horizontal scroll position after sort
    Private hScrollValue As Integer = 0
    Const LVM_FIRST As Int32 = &H1000
    Const LVM_SCROLL As Int32 = LVM_FIRST + 20
    Const SBS_HORZ As Integer = 0

    'Columns in the m_vSelectedTransDetails array
    Private Enum eSelectedTransDetails
        First = 0
        TransDetail_id = 0
        Amount
        '--------------
        'Add new columns above Count
        Count
    End Enum

    Private m_bDisableWildcardSearchOption As Boolean
    Private m_bEnablePartialWildcardSearchOption As Boolean
    Private m_bSplitReceiptSystemOption As Boolean
    Private m_lCurrencyID As Integer
    Private m_bIsLead As Boolean

    Private m_iRestrictAllocationReversal As Integer = 0
    ''' <summary>
    ''' variable declared to store Party Information of logged in User
    ''' </summary>
    Private m_vUserPartyArray As Object

    Public Property DisableWildcardSearchOption() As Boolean
        Get
            Return m_bDisableWildcardSearchOption
        End Get
        Set(ByVal Value As Boolean)
            m_bDisableWildcardSearchOption = Value
        End Set
    End Property

    Public Property EnhancedCaseSearching() As Boolean
        Get
            Return m_bEnhancedCaseSearching
        End Get
        Set(ByVal Value As Boolean)
            m_bEnhancedCaseSearching = Value
        End Set
    End Property

    Public Property EnablePartialWildcardSearchOption() As Boolean
        Get
            Return m_bEnablePartialWildcardSearchOption
        End Get
        Set(ByVal Value As Boolean)
            m_bEnablePartialWildcardSearchOption = Value
        End Set
    End Property

    Public Property AccountID() As Integer
        Get
            ' Return the objects parameter value.
            Return m_lAccountID
        End Get
        Set(ByVal Value As Integer)
            ' Set the object parameter value.
            m_lAccountID = Value
            txtAccountCode.Tag = CStr(Value)
        End Set
    End Property

    Public Property ActionKey() As String
        Get
            ' AMB 24/02/2003: PS220 - added actionkey property
            Return m_sActionKey
        End Get
        Set(ByVal Value As String)
            ' AMB 24/02/2003: PS220 - added actionkey property
            m_sActionKey = Value
        End Set
    End Property

    Public Property AllocationID() As Integer
        Get
            ' Return the objects parameter value.
            Return m_lAllocationID
        End Get
        Set(ByVal Value As Integer)
            ' Set the object parameter value.
            m_lAllocationID = Value
        End Set
    End Property

    Public Property AllocationTransType() As Integer
        Get
            Return m_lAllocationTransType
        End Get
        Set(ByVal Value As Integer)
            m_lAllocationTransType = Value
        End Set
    End Property

    Public WriteOnly Property ExcludeTransDetailID() As Integer
        Set(ByVal Value As Integer)
            m_lExcludeTransDetailID = Value
        End Set
    End Property

    Public Property CalledViaClientManager() As Boolean
        Get
            Return m_bCalledViaClientManager
        End Get
        Set(ByVal Value As Boolean)
            m_bCalledViaClientManager = Value
        End Set
    End Property

    Public WriteOnly Property SelectedSourceId() As Integer
        Set(ByVal Value As Integer)
            m_iSelectedSourceId = Value

        End Set
    End Property

    Public WriteOnly Property SelectedCurrencyId() As Integer
        Set(ByVal Value As Integer)
            m_iSelectedCurrencyId = Value
        End Set
    End Property
    ' Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.3.2.1)

    Public Property InsuredAccountID() As Integer
        Get
            Return m_lInsuredAccountID
        End Get
        Set(ByVal Value As Integer)
            m_lInsuredAccountID = Value
        End Set
    End Property

    Public Property InsuredAccountView() As Boolean
        Get
            Return m_bInsuredAccountView
        End Get
        Set(ByVal Value As Boolean)
            m_bInsuredAccountView = Value
        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Standard Property.
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property

    'DC140403 -ISS3503 -Start

    Public Property CashListID() As Integer
        Get
            ' Return the objects parameter value.
            Return m_lCashListId
        End Get
        Set(ByVal Value As Integer)
            ' Set the object parameter value.
            m_lCashListId = Value
        End Set
    End Property
    'DC140403 -ISS3503 -End

    Public Property CashListTypeID() As Integer
        Get
            ' Return the objects parameter value.
            Return m_lCashListTypeID
        End Get
        Set(ByVal Value As Integer)
            ' Set the object parameter value.
            m_lCashListTypeID = Value
        End Set
    End Property

    Public WriteOnly Property ControllingInterface() As Interface_Renamed
        Set(ByVal Value As Interface_Renamed)
            ' Set the controlling interface for this form
            m_oInterface = Value
        End Set
    End Property

    Public Property DocumentId() As Integer
        Get
            ' Return the objects parameter value.
            Return m_lDocumentId
        End Get
        Set(ByVal Value As Integer)
            ' Set the object parameter value.
            m_lDocumentId = Value
        End Set
    End Property

    Public Property DocumentRef() As String
        Get
            ' Return the objects parameter value.
            Return m_sDocumentRef
        End Get
        Set(ByVal Value As String)
            ' Set the object parameter value.
            m_sDocumentRef = Value
        End Set
    End Property

    'eck070600

    Public Property DrillCompany() As Integer
        Get
            ' Return the objects parameter value.
            Return m_iDrillCompany
        End Get
        Set(ByVal Value As Integer)
            ' Set the object parameter value.
            m_iDrillCompany = Value
        End Set
    End Property

    Public Property DrillLevel() As Integer
        Get
            ' Return the objects parameter value.
            Return m_lDrillLevel
        End Get
        Set(ByVal Value As Integer)
            ' Set the object parameter value.
            m_lDrillLevel = Value
        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            ' Standard Property.
            ' Set the effective date.
            m_dtEffectiveDate = Value
        End Set
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    'eck220800

    Public Property InsuranceRef() As String
        Get
            ' Return the objects parameter value.
            Return m_sInsuranceRef
        End Get
        Set(ByVal Value As String)
            ' Set the object parameter value.
            m_sInsuranceRef = Value
        End Set
    End Property

    ' CF180299

    Public Property IsBatch() As Boolean
        Get
            Return m_bIsBatch
        End Get
        Set(ByVal Value As Boolean)
            m_bIsBatch = Value
        End Set
    End Property

    Public Property LedgerID() As Integer
        Get
            ' AMB 11/02/2003: PS220 - added ledger id
            Return m_lLedgerID
        End Get
        Set(ByVal Value As Integer)
            ' AMB 11/02/2003: PS220 - added ledger id
            m_lLedgerID = Value
        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            ' Standard Property.
            ' Set the navigate flag.
            m_lNavigate = Value
        End Set
    End Property

    Public WriteOnly Property OutstandingOnly() As Boolean
        Set(ByVal Value As Boolean)
            m_bOutstandingOnly = Value
        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            ' Standard Property.
            ' Set the process mode.
            m_lProcessMode = Value
        End Set
    End Property

    '27/05/2003 - PWC - 186 - Debt Roll-up

    Public Property Rollup() As Boolean
        Get
            Return m_bRollup
        End Get
        Set(ByVal Value As Boolean)
            m_bRollup = Value
        End Set
    End Property

    Public WriteOnly Property SearchParams() As Object
        Set(ByVal Value As Object)

            m_vSearchParams = Value
        End Set
    End Property

    'eck040500
    Public WriteOnly Property SourceArray() As Object
        Set(ByVal Value As Object)
            ' Set the valid sources for the user

            m_vSourceArray = Value

            m_vSourceArrayCopy = Value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            ' Standard Property.
            ' Return the interface exit status.
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            ' Standard Property.
            ' Set the interface exit status.
            m_lStatus = Value
        End Set
    End Property
    'sj 02/10/2002 - end

    Public ReadOnly Property TransDetailId() As Integer
        Get
            ' Return the objects parameter value.
            Return m_lTransdetailID
        End Get
    End Property

    Public ReadOnly Property TransDetailIDs() As Object
        Get
            Return m_vTransdetailIDs
        End Get
    End Property

    Public WriteOnly Property TypeOfBusiness() As String
        Set(ByVal Value As String)
            ' Standard Property.
            ' Set the type of business.
            m_sTypeOfBusiness = Value
        End Set
    End Property

    Public Property SplitReceiptSystemOption() As Boolean
        Get
            Return m_bSplitReceiptSystemOption
        End Get
        Set(ByVal value As Boolean)
            m_bSplitReceiptSystemOption = value
        End Set
    End Property

    Public Property CurrencyID() As Integer
        Get
            Return m_lCurrencyID
        End Get
        Set(ByVal value As Integer)
            m_lCurrencyID = value
        End Set
    End Property

    Public Property IsLead() As Boolean
        Get
            Return m_bIsLead
        End Get
        Set(ByVal value As Boolean)
            m_bIsLead = value
        End Set
    End Property

    ''' <summary>
    ''' To store Party Information of logged in User
    ''' </summary>
    ''' <returns>Returns the user party information array</returns>
    Public Property UserPartyArray() As Object
        Get
            Return m_vUserPartyArray
        End Get
        Set(value As Object)
            m_vUserPartyArray = value
        End Set
    End Property

    <DllImport("user32.dll")>
    Private Shared Function GetScrollPos(ByVal hWnd As System.IntPtr, ByVal nBar As Integer) As Integer

    End Function
    <DllImport("user32.dll")>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As Integer, ByVal lParam As Integer) As Boolean

    End Function
    <DllImport("user32.dll")>
    Private Shared Function LockWindowUpdate(ByVal Handle As IntPtr) As Boolean

    End Function
    'Store the horizontal scroll value.
    Private Sub StoreHScrollValue()
        hScrollValue = GetScrollPos(lvwSearchResults.Handle, SBS_HORZ)
    End Sub
    'Recover the old scroll position
    Private Sub RecoverHorizontalScroll()
        LockWindowUpdate(lvwSearchResults.Handle)
        'Calculate the value the scroll needs to scroll back.
        Dim dx As Integer = hScrollValue - GetScrollPos(lvwSearchResults.Handle, SBS_HORZ)
        'Send the scroll message.
        Dim b As Boolean = SendMessage(lvwSearchResults.Handle, LVM_SCROLL, dx, 0)
        LockWindowUpdate(IntPtr.Zero)

    End Sub

    ' End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.3.2.1)

    '*************************************************************************
    'Name:          AllowTransfersNormal
    'Description:   Works out if the current User is Allowed to transfer the
    '               selected Item. Must have transfer_authority, must be doc
    '               type 1,48,22 or 36, must have item selected and must have
    '               a total amount (i.e not zero)
    'History:       14/02/2003 - TR - Created for TS220 Manage Debtor Accounts
    '*************************************************************************
    Private Function AllowTransfersNormal() As Boolean

        Dim result As Boolean = False
        Dim lDocumentType As Integer
        Dim dOSAmount As Double

        Try

            'TR - Assume NotAllowed - Only return True if gets to end successfully
            m_lSelectedCount = 0
            m_crSelectedTotalAmount = 0

            m_vSelectedTransDetails = Nothing

            'TR - First Check that the User has the authorisation rights
            If m_lHasTransferAuthority <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'TR - Make sure at least one Item has been selected
            If (lvwSearchResults.CheckedItems.Count > 0) Then
                'TR - Loop through each of the items in the list view
                For Each oListItem As ListViewItem In lvwSearchResults.Items
                    'TR - Only process the item if it selected
                    If oListItem.Checked Then
                        'TR - Increment the count of selected items
                        m_lSelectedCount += 1
                        'TR - Increase the size of the Selected Array
                        If Not Information.IsArray(m_vSelectedTransDetails) Then
                            ReDim m_vSelectedTransDetails(1, 0)
                        Else

                            ReDim Preserve m_vSelectedTransDetails(1, m_vSelectedTransDetails.GetUpperBound(0) + 1)
                        End If

                        'DD 02/05/2003: Calculate the O/S Amount
                        dOSAmount = gPMFunctions.NullToDouble(m_vSearchData(ACICurrencyAmount, Convert.ToString(oListItem.Tag))) - gPMFunctions.NullToDouble(m_vSearchData(ACIMatchedCurrencyAmount, Convert.ToString(oListItem.Tag)))

                        'TR - Now pass in the TransDetailID and Amount

                        m_vSelectedTransDetails(0, m_vSelectedTransDetails.GetUpperBound(1)) = m_vSearchData(ACITransDetailId, Convert.ToString(oListItem.Tag))

                        m_vSelectedTransDetails(1, m_vSelectedTransDetails.GetUpperBound(1)) = dOSAmount

                        'TR - Get the DocType ID from the array
                        lDocumentType = CInt(m_vSearchData(ACIDocumentTypeId, Convert.ToString(oListItem.Tag)))
                        'TR - Check the document type
                        Select Case lDocumentType
                            'TR - Journals, Binding Journals, Receipts &
                            'Transferred Credits Re OK
                            Case 1, 48, 22, 36
                                'TR - Do nothing
                            Case Else
                                Return result
                        End Select
                        lDocumentType = 0
                        'TR - Add the total for this line to the total amount
                        m_crSelectedTotalAmount += dOSAmount

                    End If
                Next oListItem
            Else
                'TR - Nothing selected so return false - doesn't matter though
                Return result
            End If

            'TR - See if more than 1 transaction is selected
            If m_lSelectedCount = 1 Then
                'TR - Only 1 transaction - Totals amount must be > 0
                If m_crSelectedTotalAmount = 0 Then
                    Return result
                End If
            Else
                'TR - Assume more than 1 as none selected would not get here
                If m_crSelectedTotalAmount = 0 Then
                    Return result
                End If
            End If

            'TR - If we can get to here, then Transfer Requirements have been met
            Return True

        Catch excep As System.Exception

            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "AllowTransfersNormal Failed", ACApp, ACClass, "AllowTransfersNormal", Information.Err().Number, excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '*************************************************************************
    'Name:          AllowTransfersRollup
    'Description:   Works out if the current User is Allowed to transfer the
    '               selected rolled ip item(s). Must have transfer_authority, must be doc
    '               type 1,48,22 or 36, must have item selected and must have
    '               a total amount (i.e not zero)
    'History:       04/06/2003 - PWC - Blagged from AllowTransfersNormal
    '*************************************************************************
    Private Function AllowTransfersRollup() As Boolean

        Dim result As Boolean = False
        Const ACMethod As String = "AllowTransfersRollup"

        Dim oListItem As ListViewItem
        Dim lDocumentType As Integer
        Dim dOSAmount As Double

        Dim vAllTrans(,) As Object
        Dim lAllTransLowerRow, lAllTransUpperRow As Integer

        Try

            'Assume NotAllowed - Only return True if gets to end successfully
            m_lSelectedCount = 0
            m_crSelectedTotalAmount = 0

            m_vSelectedTransDetails = Nothing

            'First Check that the User has the authorisation rights
            If m_lHasTransferAuthority <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'Make sure at least one Item has been selected
            If (lvwSearchResults.CheckedItems.Count = 0) Then
                Return result
            End If

            'Get the transaction records for each rolled up document row selected in the listview
            If GetAllTransForSelectedDocuments(r_vAllTrans:=vAllTrans) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", GetAllTransForSelectedDocuments failed")
            End If

            'Must have some transactions to process
            If Information.IsArray(vAllTrans) Then

                'Size the selected trans detail array to accept all the transactions found

                lAllTransLowerRow = vAllTrans.GetLowerBound(klRowDimension - 1)

                lAllTransUpperRow = vAllTrans.GetUpperBound(klRowDimension - 1)
                m_vSelectedTransDetails = Array.CreateInstance(GetType(Object), New Integer() {eSelectedTransDetails.Count - 1 - eSelectedTransDetails.First + 1, lAllTransUpperRow - lAllTransLowerRow + 1}, New Integer() {eSelectedTransDetails.First, lAllTransLowerRow})

                m_lSelectedCount = lAllTransUpperRow - lAllTransLowerRow + 1

                'Process the 'expanded' transactions
                For lRow As Integer = lAllTransLowerRow To lAllTransUpperRow
                    'Calculate the O/S Amount
                    dOSAmount = gPMFunctions.NullToDouble(vAllTrans(ACICurrencyAmount, lRow)) - gPMFunctions.NullToDouble(vAllTrans(ACIMatchedCurrencyAmount, lRow))

                    'Populate the new array with details from the source array

                    m_vSelectedTransDetails(eSelectedTransDetails.TransDetail_id, lRow) = vAllTrans(ACITransDetailId, lRow)

                    m_vSelectedTransDetails(eSelectedTransDetails.Amount, lRow) = dOSAmount

                    'Get the DocType ID from the array

                    lDocumentType = CInt(vAllTrans(ACIDocumentTypeId, lRow))

                    'Check the document type
                    Select Case lDocumentType
                    'Journals, Binding Journals, Receipts &
                    'Transferred Credits Re OK
                        Case 1, 48, 22, 36
                            'Do nothing
                        Case Else
                            Return result
                    End Select
                    lDocumentType = 0
                    'Add the total for this line to the total amount
                    m_crSelectedTotalAmount += dOSAmount

                Next lRow

                'See if more than 1 transaction is selected
                If m_lSelectedCount = 1 Then
                    'Only 1 transaction - Totals amount must be > 0
                    If m_crSelectedTotalAmount = 0 Then
                        Return result
                    End If
                Else
                    'Assume more than 1 as none selected would not get here
                    If m_crSelectedTotalAmount = 0 Then
                        Return result
                    End If
                End If
            End If

            'Transfer requirements have been met
            result = True


        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            If Not (oListItem Is Nothing) Then
                oListItem = Nothing
            End If



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: BuildOSTransactionsCollection
    '
    ' Description:
    '
    ' History: 14/05/2003 sj - Created.
    '
    ' ***************************************************************** '
    Private Function BuildOSTransactionsCollection() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vItem As Object
            m_cOSTransactions = New ColWrapper()

            m_lReturn = GetBusiness(v_bSilent:=True, v_bOverrideRollup:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBusiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildOSTransactionsCollection")

                Return result
            End If

            If Not Information.IsArray(m_vCheckSearchData) Then
                Return result
            End If

            For lRow As Integer = 0 To m_vCheckSearchData.GetUpperBound(1)
                If m_vCheckSearchData(ACIOutstandingBaseAmount, lRow) <> 0 Then
                    vItem = m_vCheckSearchData(ACITransDetailId, lRow)
                    m_lReturn = m_cOSTransactions.Add(v_vItem:=vItem, v_vKey:=CStr(vItem))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMError
                    End If
                End If
            Next lRow

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BuildOSTransactionsCollection Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildOSTransactionsCollection", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cboDepartment_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDepartment.Click
        CheckMandatoryEnable()
    End Sub

    Private Sub cboPMUser_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMUser.Click
        CheckMandatoryEnable()
    End Sub

    Private Function CheckApproval() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: CheckApproval
        ' PURPOSE: Validates the selected documents for write-off approval
        ' AUTHOR: Andrew Bibby
        ' DATE: 12/02/2003, 16:24
        ' RETURNS: PMTrue for success
        ' CHANGES: AMB PS220 - Created for IAG 220 Manage Debtors
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim dAmountTotal As Double
        Dim bSameUserID As Boolean
        Dim lDocUserID As Integer
        Dim bValidAmount As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' check the selected items account's amounts
            For Each oSelectedItem As ListViewItem In lvwSearchResults.Items
                If (oSelectedItem IsNot Nothing AndAlso oSelectedItem.Checked) Then
                    'DD 04/08/2003: We need to check against the outstanding amount
                    ' get the amount from the array
                    Dim dbNumericTemp As Double
                    If Double.TryParse(CStr(m_vSearchData(ACICurrencyAmount, Convert.ToString(oSelectedItem.Tag))), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                        m_lReturn = m_oUserAuthorities.ValidateAmounts(v_iCurrencyID:=m_vSearchData(ACICurrencyID, Convert.ToString(oSelectedItem.Tag)), v_cAmount:=m_vSearchData(ACICurrencyAmount, Convert.ToString(oSelectedItem.Tag)), v_lCompanyID:=m_vSearchData(ACISourceID, Convert.ToString(oSelectedItem.Tag)), r_vTransWriteOffValid:=bValidAmount)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If Not bValidAmount Then
                            Exit For
                        End If

                        dAmountTotal = dAmountTotal + Conversion.Val(CStr(m_vSearchData(ACICurrencyAmount, Convert.ToString(oSelectedItem.Tag)))) - Conversion.Val(CStr(m_vSearchData(ACIMatchedCurrencyAmount, Convert.ToString(oSelectedItem.Tag))))
                    End If
                End If
            Next oSelectedItem

            ' check that the total doesn't exceed write off limit
            If Not bValidAmount Then
                DisplayMessage(ACApproveWriteOffTitle, ACApproveWriteOffExceedError, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation)
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' check the auditset.userid of the selected documents
            For Each oSelectedItem As ListViewItem In lvwSearchResults.Items
                If (oSelectedItem IsNot Nothing AndAlso oSelectedItem.Checked) Then
                    ' check the auditset user id against the current user id
                    lDocUserID = CInt(m_vSearchData(ACIAuditSetUserID, Convert.ToString(oSelectedItem.Tag)))
                    If lDocUserID = g_oObjectManager.UserID Then
                        bSameUserID = True
                        Exit For
                    Else
                        bSameUserID = False
                    End If
                End If
            Next oSelectedItem

            ' check that the user is not trying to approve their own write-off (naughty)
            If bSameUserID Then
                DisplayMessage(ACApproveWriteOffTitle, ACApproveWriteOffYourselfError, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation)
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' finally, confirm with the user that the approval should go ahead
            If DisplayMessage(ACApproveWriteOffConfirmTitle, ACApproveWriteOffConfirm, MsgBoxStyle.YesNo + MsgBoxStyle.Question) = System.Windows.Forms.DialogResult.Yes Then
                result = gPMConstants.PMEReturnCode.PMTrue
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in CheckApproval", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckApproval", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result
            End Select

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory
    '
    ' Description: Check if all mandatory fields have been entered in
    '              order for the search to proceed.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Check all fields for data.

            If txtAccountCode.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If panAgentCode.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtDocumentRef.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If cmbDocTypeGroup.SelectedIndex <> -1 Then
                If VB6.GetItemData(cmbDocTypeGroup, cmbDocTypeGroup.SelectedIndex) <> -1 Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            If cmbDocumentType.SelectedIndex <> -1 Then
                If VB6.GetItemData(cmbDocumentType, cmbDocumentType.SelectedIndex) <> -1 Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            If cmbPeriod.SelectedIndex <> -1 Then
                If VB6.GetItemData(cmbPeriod, cmbPeriod.SelectedIndex) <> -1 Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            If txtDateFrom.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtDateTo.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If cboPMUser.UserID <> 0 Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtCurrencyAmount.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtInsuranceRef.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtPurchaseInvoiceNo.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtPurchaseOrderNo.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If cboDepartment.ListIndex <> 0 Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtSpare.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            '(RC) QBENZ014
            If txtAltRef.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            'DD 11/04/2003
            If txtInsuredAccountCode.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtBGRef.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtCaseNumber.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If
            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for mandatory fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub CheckMandatoryEnable()

        ' Check mandatory and enable the Find Now button accordingly
        If CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue Then
            cmdFindNow.Enabled = True
            cmdBalance.Enabled = True
        Else
            cmdFindNow.Enabled = False
            cmdBalance.Enabled = False
        End If

    End Sub

    Private Function CheckRejection() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: CheckRejection
        ' PURPOSE: Confirms that the user wants to do the rejection
        ' AUTHOR: Andrew Bibby
        ' DATE: 12/02/2003, 16:24
        ' RETURNS: PMTrue for success
        ' CHANGES: AMB PS220 - Created for IAG 220 Manage Debtors
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim lCount, lMessageNum As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            For Each oSelectedItem As ListViewItem In lvwSearchResults.Items
                If (oSelectedItem IsNot Nothing AndAlso oSelectedItem.Checked) Then
                    ' count the number of selected items
                    lCount += 1
                End If
            Next oSelectedItem

            ' decide what message to show
            Select Case lCount
                Case 0
                    ' let's get out
                    Return result
                Case 1
                    ' set the message to a singluar message
                    lMessageNum = ACRejectWriteOffSingle
                Case Is > 1
                    ' set the message to a plural message
                    lMessageNum = ACRejectWriteOffMultiple
            End Select

            ' confirm with the user that they want to do this
            If DisplayMessage(ACRejectWriteOffTitle, lMessageNum, MsgBoxStyle.Question + MsgBoxStyle.YesNo) = System.Windows.Forms.DialogResult.Yes Then
                result = gPMConstants.PMEReturnCode.PMTrue
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in CheckRejection", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckRejection", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result
            End Select

        Finally


        End Try
        Return result
    End Function

    Private Function CheckTransactionWriteOff() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: CheckTransactionWriteOff
        ' PURPOSE: Validates the selected documents for Write-off
        ' AUTHOR: Andrew Bibby
        ' DATE: 12/02/2003, 16:24
        ' RETURNS: PMTrue for success
        ' CHANGES: AMB PS220 - Created for IAG 220 Manage Debtors
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACMethod As String = "CheckTransactionWriteOff"

        Dim lSelectedInsFileCnt, lSelectedItemTag As Integer
        Dim bFailWriteOff As Boolean
        Dim vAmountArray(,) As Object
        Dim dTransDetailAmount, dMatchAmount As Double

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' check the selected item's insurance_file_cnt
            For Each oSelectedItem As ListViewItem In lvwSearchResults.Items

                ' check if the insurance_file_cnt is null
                If (oSelectedItem IsNot Nothing AndAlso oSelectedItem.Checked) Then

                    ' the tag is the index to the m_vSearchData array

                    lSelectedItemTag = Convert.ToString(oSelectedItem.Tag)
                    lSelectedInsFileCnt = gPMFunctions.NullToLong(m_vSearchData(ACIDocInsuranceFileCnt, lSelectedItemTag))

                    ' the insurance_file_cnt is null or 0, generate an error
                    If lSelectedInsFileCnt = 0 Then

                        bFailWriteOff = gPMConstants.PMEReturnCode.PMTrue

                        DisplayMessage(ACWriteOffNotAllowedTitle, ACWriteOffNotAllowedInsurancePolicy, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation)

                        ' jump out
                        Exit For

                    Else
                        bFailWriteOff = gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            Next oSelectedItem

            ' we've got this far, we need to check the refunds/credits
            If bFailWriteOff = gPMConstants.PMEReturnCode.PMFalse Then

                For Each oSelectedItem As ListViewItem In lvwSearchResults.Items

                    If (oSelectedItem IsNot Nothing AndAlso oSelectedItem.Checked) Then

                        ' pass the account id to the business

                        m_lReturn = g_oBusiness.GetAccountAmounts(v_lAccountId:=m_vSearchData(ACIAccountId, lSelectedItemTag), r_vResultArray:=vAmountArray)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            DisplayMessage(ACWriteOffNotAllowedTitle, ACWriteOffNotAllowedError, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation)
                            Return result
                        End If

                        If Information.IsArray(vAmountArray) Then

                            ' loop thru the array

                            For lLoopy As Integer = 0 To vAmountArray.GetUpperBound(1)

                                Dim dbNumericTemp As Double
                                If Double.TryParse(CStr(vAmountArray(0, lLoopy)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                                    dTransDetailAmount = gPMFunctions.NullToDouble(vAmountArray(0, lLoopy))
                                Else
                                    dTransDetailAmount = 0
                                End If

                                Dim dbNumericTemp2 As Double
                                If Double.TryParse(CStr(vAmountArray(1, lLoopy)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                                    dMatchAmount = gPMFunctions.NullToDouble(vAmountArray(1, lLoopy))
                                Else
                                    dMatchAmount = 0
                                End If

                                If (dTransDetailAmount <> 0) And (dMatchAmount <> 0) Then
                                    ' if the total is less than zero, the write-off is not allowed
                                    If dTransDetailAmount - dMatchAmount < 0 Then

                                        bFailWriteOff = True
                                        ' inform the user
                                        DisplayMessage(ACWriteOffNotAllowedTitle, ACWriteOffNotAllowedPendingAccount, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation)

                                        ' jump out
                                        Exit For

                                    End If
                                End If

                            Next lLoopy

                            ' and jump out again, we want to quit
                            If bFailWriteOff Then Exit For

                        End If

                    End If
                Next oSelectedItem

            End If

            If bFailWriteOff Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

        Catch ex As Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in " & ACMethod, vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result
            End Select

        Finally


        End Try
        Return result
    End Function

    Private Function CheckTransactionWriteOffRollup() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: CheckTransactionWriteOffRollup
        ' PURPOSE: Validates the selected documents for rolled up Write-off
        ' AUTHOR: Paul Cunningham
        ' DATE: 09/06/2003, 09:23
        ' RETURNS: PMTrue for success
        ' History: 04/06/2003 - PWC - Blagged from CheckTransactionWriteOff
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACMethod As String = "CheckTransactionWriteOffRollup"

        Dim lSelectedInsFileCnt, lSelectedItemTag As Integer
        Dim bFailWriteOff As Boolean
        Dim vAmountArray(,) As Object
        Dim dTransDetailAmount, dMatchAmount As Double

        Dim vAllTrans(,) As Object
        Dim lAllTransLowerRow, lAllTransUpperRow As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Get the transaction records for each rolled up document row selected in the listview
            If GetAllTransForSelectedDocuments(r_vAllTrans:=vAllTrans) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", GetAllTransForSelectedDocuments failed")
            End If

            If Information.IsArray(vAllTrans) Then

                'Size the selected trans detail array to accept all the transactions found

                lAllTransLowerRow = vAllTrans.GetLowerBound(klRowDimension - 1)

                lAllTransUpperRow = vAllTrans.GetUpperBound(klRowDimension - 1)

                'Process the 'expanded' transactions
                For lRow As Integer = lAllTransLowerRow To lAllTransUpperRow

                    lSelectedInsFileCnt = CInt(Conversion.Val("" & CStr(vAllTrans(ACIDocInsuranceFileCnt, lRow))))

                    ' the insurance_file_cnt is null or 0, generate an error
                    If lSelectedInsFileCnt = 0 Then

                        bFailWriteOff = gPMConstants.PMEReturnCode.PMTrue

                        DisplayMessage(ACWriteOffNotAllowedTitle, ACWriteOffNotAllowedInsurancePolicy, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation)

                        ' jump out
                        Exit For

                    Else
                        bFailWriteOff = gPMConstants.PMEReturnCode.PMFalse
                    End If
                Next lRow

                ' we've got this far, we need to check the refunds/credits
                If bFailWriteOff = gPMConstants.PMEReturnCode.PMFalse Then

                    ' check the selected items account's amounts
                    For lRow As Integer = lAllTransLowerRow To lAllTransUpperRow

                        ' pass the account id to the business

                        m_lReturn = g_oBusiness.GetAccountAmounts(v_lAccountId:=m_vSearchData(ACIAccountId, lSelectedItemTag), r_vResultArray:=vAmountArray)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            DisplayMessage(ACWriteOffNotAllowedTitle, ACWriteOffNotAllowedError, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation)
                            Return result
                        End If

                        If Information.IsArray(vAmountArray) Then

                            ' loop thru the array

                            For lLoopy As Integer = 0 To vAmountArray.GetUpperBound(1)

                                Dim dbNumericTemp As Double
                                If Double.TryParse(CStr(vAmountArray(0, lLoopy)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                                    dTransDetailAmount = gPMFunctions.NullToDouble(vAmountArray(0, lLoopy))
                                Else
                                    dTransDetailAmount = 0
                                End If

                                Dim dbNumericTemp2 As Double
                                If Double.TryParse(CStr(vAmountArray(1, lLoopy)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                                    dMatchAmount = gPMFunctions.NullToDouble(vAmountArray(1, lLoopy))
                                Else
                                    dMatchAmount = 0
                                End If

                                If (dTransDetailAmount <> 0) And (dMatchAmount <> 0) Then
                                    ' if the total is less than zero, the write-off is not allowed
                                    If dTransDetailAmount - dMatchAmount < 0 Then

                                        bFailWriteOff = True
                                        ' inform the user
                                        DisplayMessage(ACWriteOffNotAllowedTitle, ACWriteOffNotAllowedPendingAccount, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation)

                                        ' jump out
                                        Exit For

                                    End If
                                End If

                            Next lLoopy

                            ' and jump out again, we want to quit
                            If bFailWriteOff Then Exit For

                        End If

                    Next lRow

                End If

                If bFailWriteOff Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                Else
                    result = gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

        Catch ex As Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in " & ACMethod, vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result
            End Select

        Finally


        End Try
        Return result
    End Function

    Private Sub cboSource_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboSource.SelectedIndexChanged
        If cboSource.SelectedIndex <> -1 And cmbCurrency.ListIndex <> -1 Then
            m_lReturn = SetDefaultCurrencyOptions(v_iSourceId:=VB6.GetItemData(cboSource, cboSource.SelectedIndex), v_iCurrencyID:=cmbCurrency.ItemData(cmbCurrency.ListIndex))
        End If
    End Sub

    Private Sub cboSource_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboSource.Leave
        g_iCompanyID = VB6.GetItemData(cboSource, cboSource.SelectedIndex)
    End Sub

    Private Sub chkDisplayOutstanding_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkDisplayOutstanding.CheckStateChanged
        Dim bHere As Boolean = True
    End Sub

    ' ***************************************************************** '
    ' Name: ClearInterface
    '
    ' Description: Clears all of the interface details for a new
    '              search.
    '
    ' ***************************************************************** '
    Private Function ClearInterface(Optional ByRef v_bSilent As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String
        Dim bSilent As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if the user still wishes to clear
            ' the interface.

            ' Default to not silent (ie, ask the user) if missing parameter
            ' This is dont for backwards compatability
            If Not True Then
                bSilent = False
            Else
                bSilent = v_bSilent
            End If

            ' Display the message if we're not in silent mode
            If Not bSilent Then

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display the message.
                iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                ' Check message result.
                If iMsgResult = System.Windows.Forms.DialogResult.No Then
                    ' Don't continue with the clear.
                    Return result
                End If

            End If

            ' Clear the interface details.

            ' Clear the search data array.
            m_vSearchData = Nothing

            ' Clear the search list details.
            lvwSearchResults.Items.Clear()
            stbStatus.Items.Item(0).Text = ""
            stbStatus.Items.Item(1).Text = ""
            stbStatus.Items.Item(2).Text = ""
            stbStatus.Items.Item(3).Text = ""
            stbStatus.Items.Item(4).Text = ""
            stbStatus.Items.Item(5).Text = ""
            txtAccountCode.Text = ""
            txtAccountCode.Tag = ""
            txtBGRef.Text = ""
            txtDocumentRef.Text = ""
            cmbDocTypeGroup.SelectedIndex = 0
            cmbDocumentType.SelectedIndex = 0
            cmbPeriod.SelectedIndex = 0
            txtDateFrom.Text = ""
            txtDateTo.Text = ""
            txtDueDateFrom.Text = ""
            txtDueDateTo.Text = ""
            cmbCurrency.ListIndex = 0
            cboDepartment.ListIndex = 0 'MKW060504 PN10318 Populated department box.
            txtCurrencyAmount.Text = ""
            txtTolerance.Text = CStr(0)
            txtInsuranceRef.Text = ""
            cboPMUser.ListIndex = 0
            cboUnderwritingYearID.ListIndex = 0
            txtPurchaseOrderNo.Text = ""
            txtPurchaseInvoiceNo.Text = ""
            cboDepartment.ListIndex = 0
            txtSpare.Text = ""
            txtAltRef.Text = "" '(RC) QBENZ014
            txtCaseNumber.Text = ""
            panAccountName.Text = ""
            panContactName.Text = ""
            panPhoneAreaCode.Text = ""
            panPhoneNumber.Text = ""
            panPhoneExtension.Text = ""
            panAccountBalance.Text = ""
            panInvoiceBalance.Text = ""
            panDefaultedInstalments.Text = ""
            panStatus.Text = ""
            panPolicyBalance.Text = ""
            panSelectedItemBalance.Text = ""
            panPremiumFinanceBalance.Text = ""
            'DD 11/04/2003
            panInsuredAccountName.Text = ""
            'PN: 45764
            BranchOsBalInBaseCurrency.Text = ""

            panTranCurrOS.Text = ""
            cmdReverse.Enabled = False

            ' Set to the first tab.
            SSTabHelper.SetSelectedIndex(tabMain, 0)

            ' Set focus to the search details.
            If txtAccountCode.Enabled Then
                txtAccountCode.Focus()
            End If

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

            'MKW 060204 PN10320 Clear Insured Account of pressing 'New Search' Button. START.
            txtInsuredAccountCode.Text = ""
            txtInsuredAccountCode.Tag = ""
            'MKW 060204 PN10320 Clear Insured Account of pressing 'New Search' Button. END.

            ' Disable parts of the interface, so the
            ' user can now only enter a new search
            DisableInterface(bDisable:=True)

            ' m_lDrillLevel = 0

            ' Alix - 18/02/2003 - Issue 2325
            ' We found NO client, disable event button
            'changed as per naming convention in DotNet
            tbrMain.Items.Item("_Event").Enabled = False

            'Set branches back to (All).
            cboSource.SelectedIndex = 0
            cmdAllocate.Enabled = False
            'If this procedure was called by the new search button then reset option buttons.
            If Not bSilent Then
                'Set the amounts to transaction/base amounts.
                optAmountTransaction.Checked = True
                optOutstandingBase.Checked = True

                'Set account currency captions back to normal.

                optAmountAccount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                optOutstandingAccount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            CheckMandatoryEnable()

            Return result

        Catch excep As System.Exception

            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Sub cmbAccountType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbAccountType.SelectedIndexChanged
        CheckMandatoryEnable()
        If cmbAccountType.SelectedIndex > 0 Then
            cmdFindNow.Enabled = True
        End If
    End Sub

    Private Sub cmbCurrency_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbCurrency.Click

        If cboSource.SelectedIndex <> -1 And cmbCurrency.ListIndex <> -1 Then
            m_lReturn = SetDefaultCurrencyOptions(v_iSourceId:=VB6.GetItemData(cboSource, cboSource.SelectedIndex), v_iCurrencyID:=cmbCurrency.ItemData(cmbCurrency.ListIndex))
        End If
    End Sub

    Private Sub cmbDocTypeGroup_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbDocTypeGroup.SelectedIndexChanged
        CheckMandatoryEnable()
    End Sub

    Private Sub cmbDocumentType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbDocumentType.SelectedIndexChanged
        CheckMandatoryEnable()
    End Sub

    Private Sub cmbPeriod_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbPeriod.SelectedIndexChanged
        CheckMandatoryEnable()
    End Sub

    Private Sub cmdAccountCode_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAccountCode.Click

        m_lReturn = PopulateAccountCode()

    End Sub

    'EK 020300 Was Navigate - Now Match
    Private Sub cmdAllocate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAllocate.Click

        Try

            'eck180901
            If (Not Information.IsArray(m_vSearchData)) Or (lvwSearchResults.Items.Count < 1) Then
                Exit Sub
            End If

            OpenAllocateForm()

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Allocate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAllocate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdBalance_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBalance.Click

        ' Balance only
        m_bBalanceOnly = True

        ' Perform the find
        FindNow()

        ' Check the outstanding only box now
        chkDisplayOutstanding.CheckState = CheckState.Checked

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            'SMJB 20/10/03: If we're not in a roadmap then closing the form should not
            'try and return any search details
            If IsInRoadMap() Then
                ' Process the next set of actions.
                m_lReturn = m_oGeneral.ProcessCommand()
            Else
                m_lReturn = gPMConstants.PMEReturnCode.PMTrue
            End If

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdExpand_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExpand.Click
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: cmdExpand_Click
        ' PURPOSE: Spawn a new iACTFindTransaction with the current search criteria (in normal mode)
        ' AUTHOR: Paul Cunningham
        ' DATE: 28 May 2003, 09:04:29
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Const ACMethod As String = "cmdExpand_Click"

        Dim lRow As Integer

        Dim oFindTransaction, vSearchParams As Object

        Dim lUpper As Integer

        ' 'Modified add an
        arrControlList = New ArrayList

        Const ksComponent As String = "iACTFindTransaction.Interface"

        Try

            ' Check if there are any items available.
            If (lvwSearchResults.CheckedItems.Count > 0) Then
                lRow = CInt(lvwSearchResults.CheckedItems(0).Name.Substring(1))
            End If

            oFindTransaction = New Interface_Renamed()
            If Me.HasChildren Then
                ControlList(Me, arrControlList)
            End If
            With oFindTransaction

                m_lErrorNumber = .Initialise()

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Unable to Initialise: " & ksComponent)
                End If

                .CallingAppName = ACApp

                'Ensure the PMView is set so we enter in read only mode

                m_lErrorNumber = .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to set process modes")
                End If

                'Populate search criteria - loop through the controls on the form
                'testing the tag for the literal SEARCH;
                'If found, populate the array with the control name and value

                'NOTE - passing all search criteria may produce unexpected results in the expanded list
                'eg.  If user specified an amount for the initial rollup search, this would filter on the
                'total (SUM) of the amounts for a document.  When expanding this will filter on the individual
                'transactions.  The result of this is that potentially not all transactions for a document
                'will appear in the list.  I've advised Danny of this and he said that it was fine.
                lUpper = -1
                'Modified,add a for loop and a dim condition,comment the for loop
                For arrItem As Integer = 0 To arrControlList.Count - 1
                    Dim ctl As Control = arrControlList(arrItem)
                    'For Each ctl As Control In ContainerHelper.Controls(Me)
                    'NOTE - txtAccountCode.Tag / txtInsuredAccountCode already used so hardcoded hack
                    If (Convert.ToString(ControlHelper.GetTag(ctl)).IndexOf("SEARCH;") + 1) Or ctl.Name = "txtAccountCode" Or ctl.Name = "txtInsuredAccountCode" Then

                        lUpper += 1

                        'Grow the array
                        If Information.IsArray(vSearchParams) Then
                            ReDim Preserve vSearchParams(1, lUpper)
                        Else
                            ReDim vSearchParams(1, lUpper)
                        End If

                        vSearchParams(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lUpper) = ctl.Name

                        'populate the value depending on type

                        Select Case ctl.GetType().Name.ToUpper()
                            Case "TEXTBOX"
                                'Override document ref with item selected
                                If ctl.Name = "txtDocumentRef" Then

                                    vSearchParams(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lUpper) = m_vSearchData(ACIDocumentRef, lRow)
                                Else

                                    vSearchParams(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lUpper) = ctl.Text
                                End If
                            'Case "CHECKBOX", "OPTIONBUTTON"
                            Case "CHECKBOX", "RADIOBUTTON"
                                'vSearchParams(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lUpper) = ReflectionHelper.GetMember(ctl, "value")
                                If ctl.GetType().Name.ToUpper() = "RADIOBUTTON" Then
                                    vSearchParams(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lUpper) = CType(ctl, RadioButton).Checked
                                Else
                                    vSearchParams(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lUpper) = CType(ctl, CheckBox).Checked
                                End If
                            Case "COMBOBOX", "CBOPMUSERLOOKUP"
                                If ctl.GetType().Name.ToUpper() = "CBOPMUSERLOOKUP" Then

                                    vSearchParams(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lUpper) = ReflectionHelper.GetMember(ctl, "ListIndex")
                                Else

                                    vSearchParams(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lUpper) = CType(ctl, ComboBox).SelectedIndex
                                End If
                            Case "CBOPMLOOKUP"

                                vSearchParams(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lUpper) = ReflectionHelper.GetMember(ctl, "ItemId")
                        End Select
                    End If
                    'Next ctl
                Next arrItem

                'Set the start up options
                Dim vKeys(1, 1) As Object

                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameFindTransSearchParams

                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = vSearchParams
                'As we are expanding we don't want to rollup

                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameFindTransRollUp

                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = False

                m_lErrorNumber = .SetKeys(vKeys)

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to set keys")
                End If

                'DD 21/08/2003: Pass through the document_id

                .DocumentId = m_vSearchData(ACIDocDocumentID, lRow)

                ' Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.3.2.7)

                .InsuredAccountView = m_bInsuredAccountView
                ' End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.3.2.7)

                m_lErrorNumber = .Start()

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Unable to start component: " & ksComponent)
                End If

            End With


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    Exit Sub

                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub

            End Select

        Finally


        End Try
    End Sub

    Private Sub cmdFindAccTrans_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindAccTrans.Click

        m_lDrillLevel += 1
        m_lReturn = FindAccountTransactions()

    End Sub

    Private Sub cmdFindDocTrans_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindDocTrans.Click

        m_lDrillLevel += 1
        m_lReturn = FindDocumentTransactions()

    End Sub

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click

        ' Click event of the Find Now button.
        m_bBalanceOnly = False

        ' Set focus to find now
        cmdFindNow.Focus()

        'If we are searching then,reset them
        If SplitReceiptSystemOption Then
            CashListID = 0
            DocumentId = 0
            m_lTransdetailID = 0
        End If

        ' Perform the search
        FindNow()

        ' Set focus back to a control
        Select Case SSTabHelper.GetSelectedIndex(tabMain)
            Case 0
                'eck280601 make sure its enabled
                If txtAccountCode.Enabled Then
                    txtAccountCode.Focus()
                End If
            Case 1
                txtDocumentRef.Focus()
            Case 2
                txtCurrencyAmount.Focus()
            Case 3
                txtInsuranceRef.Focus()
            Case Else
                ' If something weird happened then set to the first tab
                SSTabHelper.SetSelectedIndex(tabMain, 0)
                txtAccountCode.Focus()
        End Select

        SetDrillButtons()

        If (lvwSearchResults.CheckedItems.Count = 0) Then
            cmdViewAllocation.Enabled = False
            cmdExpand.Enabled = False
            cmdAllocate.Enabled = False
        End If
    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen

        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)

    End Sub

    Private Sub cmdInsuredAccountCode_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdInsuredAccountCode.Click
        m_lReturn = PopulateAccountCode(bInsuredAccount:=True)
        SSTabHelper.SetSelectedIndex(tabMain, 3)
    End Sub

    Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click

        ' Click event of the New Search button.

        Try

            ' Clear the interface details.
            m_lReturn = ClearInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to clear the interface details.
            End If

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNewSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Dim oAllocation As Object = Nothing
        Dim nPendingApproval As Integer
        Dim nIsDebitOrderTransdetail As Integer
        Dim sMessageDesc As String
        Dim sDocRef As String
        Try

            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'If we're on a road map, then we must be on the
            'Allocation roadmap (there is only one that currently includes Find Transaction)
            If IsInRoadMap() Then
                'This test can go back in now as it will only appear
                'if we're trying to allocate something
                If (lvwSearchResults.CheckedItems.Count > 0) Then
                    If Not lvwSearchResults.CheckedItems(0).Checked Then
                        MessageBox.Show("No transactions were selected for allocation.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    Else
                        'Loop through the selection to ensure that no Instalment transactions are chosen
                        'these are handled automatically by the Instalments sub-system and must not
                        'be altered by the user.
                        m_lReturn = g_oObjectManager.GetInstance(oObject:=oAllocation, sClassName:="bACTAllocation.Form", vInstanceManager:=PMGetViaClientManager)

                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                        End If
                        'Defect TFS 6444
                        For Each oListItem As ListViewItem In lvwSearchResults.Items
                            If oListItem.Checked Then
                                If ChkDocTypeIsInstalments(ListViewHelper.GetListViewSubItem(oListItem, ACListDocumentRef).Text.Substring(0, 3)) = True And CheckIsLinkedToThirdPartyScheme(ListViewHelper.GetListViewSubItem(oListItem, ACListDocumentRef).Text) = False Then
                                    MessageBox.Show("Instalment transactions are handled automatically by Sirius. They are not allowed to be manually allocated.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    Exit Sub

                                End If
                                nPendingApproval = CLng(m_vSearchData(kPendingApproval, oListItem.Tag))
                                nIsDebitOrderTransdetail = CLng(m_vSearchData(kIsDebitOrderTransDetail, oListItem.Tag))
                                sDocRef = Trim(m_vSearchData(ACIDocumentRef, oListItem.Tag))
                                If nPendingApproval = 1 Then
                                    sMessageDesc = "Payment of Document Ref # " & sDocRef & " is gone for Approval. So it can not be allocated."
                                    Call MsgBox(sMessageDesc, vbOKOnly + vbExclamation, "Invalid selection")
                                    Exit Sub
                                ElseIf nIsDebitOrderTransdetail = 1 Then
                                    sMessageDesc = "One or more of the selected transactions are being managed by the contract system and may not be manually allocated."
                                    Call MsgBox(sMessageDesc, vbOKOnly + vbExclamation, "Invalid selection")
                                    Exit Sub
                                End If
                            End If
                        Next oListItem
                    End If
                Else
                    MessageBox.Show("No transactions were selected for allocation.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If


            ' Process the next set of actions.
            ' if its just Find Transaction skip ProcessCommand
            If m_sCallingAppName.ToUpper() <> ("iActFindTransaction").ToUpper() Then
                m_lReturn = m_oGeneral.ProcessCommand()
            End If

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                Me.Hide()
            End If

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try

    End Sub

    Private Sub cmdReverseAndReplace_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdReverseAndReplace.Click

        Dim sMessage As String = ""
        Dim eResult As DialogResult

        Try

            'Don't procede if no transaction is selected
            If Not Information.IsArray(m_vSearchData) Then
                Exit Sub
            End If

            eResult = MessageBox.Show("Are you sure you want to reverse and replace this transaction? ", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If eResult = System.Windows.Forms.DialogResult.No Then
                Exit Sub
            End If

            m_lReturn = ProcessReversal()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Unable to create Document Reversal", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            m_lReturn = RaiseTransaction()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Unable to Replace the Transaction", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            m_lReturn = m_oGeneral.GetInterfaceDetails()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("cmdReverseAndReplace_Click", "Failed to get Interface Details", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Set the focus back to the list
            lvwSearchResults.Focus()



        Catch ex As Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Reverse and Replace command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdReverseReplace_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            Exit Sub

        Finally



        End Try
    End Sub

    Private Sub cmdViewAllocation_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdViewAllocation.Click
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: cmdViewAllocation_Click
        ' PURPOSE:
        ' AUTHOR: Paul Cunnigham
        ' DATE: 18 November 2002, 10:00:31
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim lSelectedItem As Integer
        Dim oSelectedItem As ListViewItem
        Dim blnProcessing As Boolean

        Try

            blnProcessing = True

            'Only process if there is a selection made
            If (lvwSearchResults.CheckedItems.Count = 0) Then
                Exit Sub
            Else
                oSelectedItem = lvwSearchResults.CheckedItems(0)
            End If
            If oSelectedItem Is Nothing Then
                'Display standard message
                DisplayMessage(ACNoSelectionTitle, ACNoSelectionDetails, MsgBoxStyle.Exclamation)

                blnProcessing = False
                Exit Sub
            End If

            'get the TransdDetailId of the selected item

            lSelectedItem = Convert.ToString(lvwSearchResults.Items.Item(oSelectedItem.Index).Tag)
            m_lTransdetailID = CInt(m_vSearchData(ACITransDetailId, lSelectedItem))

            frmViewAllocation = New frmViewAllocation

            'Pass standard details to form properties
            With frmViewAllocation
                .CallingAppName = m_sCallingAppName
                .Task = gPMConstants.PMEComponentAction.PMEdit
                .Navigate = m_lNavigate
                .ProcessMode = m_lProcessMode
                '.TransactionType = m_sTransactionType$
                .EffectiveDate = m_dtEffectiveDate

                ' Payment Maintenance
                ' Putting values in property
                'PN: 45534
                .TransactionDate = gPMFunctions.ToSafeDate(DateTime.Parse(m_vSearchData(ACIAmountUpdated, lSelectedItem)).ToString("d"))
                .AllowReverseAllocation = m_iAllowReverseAllocation
                .ReverseAllocationDays = m_iReverseAllocationDays
                .HasPaymentAuthority = m_iHasPaymentAuthority
                .PaymentCurrencyId = m_lPaymentCurrencyId
                .PaymentAmount = m_crPaymentAmount
                .HasClaimAuthority = m_iHasClaimAuthority
                .ClaimCurrencyId = m_lClaimCurrencyId
                .ClaimAmount = m_crClaimAmount
                .TransId = m_lTransdetailID

                ' {* USER DEFINED CODE (Begin) *)

                ' PC021202
                '.TransDetailId = m_lTransdetailID
                '28/05/2003 - PWC - 186 Debt Rollup
                If m_bRollup Then
                    .DocumentId = CInt(m_vSearchData(ACIDocDocumentID, lSelectedItem))
                    .AccountID = CInt(m_vSearchData(ACIAccountId, lSelectedItem))
                Else
                    .TransDetailId = m_lTransdetailID
                End If
                .Rollup = m_bRollup
                '        ' Check the task.
                '        Select Case (r_iTaskType)
                '            Case PMEdit
                '                .CashListDrawerID = Val(Me.lvwCashListDrawers.SelectedItem.SubItems(ACCashListDrawerId))
                '            Case PMAdd
                '                GetCompany .CompanyID
                '        End Select
                ' {* USER DEFINED CODE (End) *}
                .DisableReverse = m_lDisableReverse
                .Spare = CStr(m_vSearchData(ACISpare, lSelectedItem))
                .IsLead = m_bIsLead
            End With

            ' Call the Load method to setup the interface details
            Dim tempLoadForm As frmViewAllocation = frmViewAllocation

            'Show form if no errors
            If frmViewAllocation.m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                frmViewAllocation.ShowDialog()
            End If

            ' Alix - 04/03/2003
            If frmViewAllocation.Status = gPMConstants.PMEReturnCode.PMOK Then
                cmdFindNow_Click(cmdFindNow, New EventArgs())
            End If

            frmViewAllocation.Close()
            frmViewAllocation = Nothing


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in cmdViewAllocation_Click", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdViewAllocation_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub
            End Select

        Finally
            blnProcessing = False


        End Try
    End Sub

    '*************************************************************************
    'Name:          CreateBalancingTransactions
    'Description:   Displays iACTTransaction to allow user to create
    '               balancing transactions
    'History:       14/02/03 - TR - Created
    '               for TS220 Manage Debtor Accs (transfers)
    '*************************************************************************
    Private Function CreateBalancingTransactions(ByVal v_lDocumentID As Integer, ByVal v_lAccountId As Integer) As Integer

        'TR - Things to go into input array for Create Manula Journal
        'Amount to be transferred (+ve or -ve)
        'DocumentID (just created)
        Dim result As Integer = 0
        Dim vKeyArray(1, 6) As Object
        Dim oiACTTransaction As iACTTransaction.Interface_Renamed

        Try

            'TR - Assume success
            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Populate the input array

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameDocumentID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lDocumentID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameCashListItemAmount
            'DD 06/05/2003: Negate the number to ensure the raised transaction
            'will reduce the original.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = -m_crSelectedTotalAmount

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameBankAccountID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = v_lAccountId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameActionKey

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = "TRANSFER"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameDocumentDate

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = DateTime.Now

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.ACTKeyNameBranchID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = g_iCompanyID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameFinancePlanTransactions

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_vSelectedTransDetails

            ' Create a new instance of navigator
            oiACTTransaction = New iACTTransaction.Interface_Renamed()
            'TR - Use the Transaction Interface
            With oiACTTransaction
                'TR - Initilaise the object
                m_lReturn = .Initialise()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to initialise " & "iACTTransaction object.", ACApp, ACClass, "CreateBalancingTransactions", Information.Err().Number, Information.Err().Description)
                    Return result
                End If

                ' Set the navigator keys
                If Not False Then
                    m_lReturn = .SetKeys(vKeyArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to set keys.", ACApp, ACClass, "CreateBalancingTransactions", Information.Err().Number, Information.Err().Description)
                        Return result
                    End If
                End If

                ' Set its properties
                .CallingAppName = ACApp
                .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

                ' Start it
                m_lReturn = .Start()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to start oiACTTransaction", ACApp, ACClass, "CreateBalancingTransactions", Information.Err().Number, Information.Err().Description)
                    Return result
                End If

                'TR - Make sure that the User closed the Transaction form using
                'the OK button (when the transdetails will have been saved)
                If .Status <> gPMConstants.PMEReturnCode.PMTrue Then
                    'TR - If the user cancelled the screen, delete the header document

                    m_lReturn = g_oBusiness.DeleteTransferDocument(v_lDocumentID)
                End If
            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "CreateBalancingTransactions Failed", ACApp, ACClass, "CreateBalancingTransactions", Information.Err().Number, excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Private Function CreateOrOpenCashList(ByRef r_lCashListId As Integer, ByVal v_lBankAccountId As Integer, ByVal v_dTotalAmount As Double) As Integer
        Dim result As Integer = 0

        ' ***************************************************************** '
        ' Name: CreateOrOpenCashList
        '
        ' Description: Create CashList Record or Open the Cashlist form
        '
        ' History   07012003 CMG/PB Created PS202
        '           AMB 20/02/2003: PS220 - modified for Manage Debtors development
        ' ***************************************************************** '

        Dim oACTCashList As Object
        Const CASHLIST_STATUS_CLOSED As Integer = 3
        Const CASHLIST_REF As String = "Refund"
        Const CASHLISITEM_COUNT As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' check to see if we have a bank account already, if so just create the cashlist record
            If Not (v_lBankAccountId = 0) Then

                ' Get an instance of the business object via
                ' the public object manager.
                Dim temp_oACTCashList As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oACTCashList, "bACTCashList.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oACTCashList = temp_oACTCashList

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get an instance of the business object.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'bACTCashList.Business'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrOpenCashList", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                'Direct Add
                'Pass NRMA BankAccount_Id from sys option on v_sBankAccountId

                m_lReturn = oACTCashList.DirectAdd(vCashListID:=r_lCashListId, vCashListStatusID:=CASHLIST_STATUS_CLOSED, vCashListTypeID:=gACTLibrary.ACTCashListTypePayments, vCashListRef:=CASHLIST_REF, vCompanyID:=g_oObjectManager.SourceID, vBankAccountID:=v_lBankAccountId, vCurrencyID:=g_oObjectManager.CurrencyID, vListDate:=DateTime.Now, vControlTotal:=v_dTotalAmount, vItemCount:=CASHLISITEM_COUNT)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    result = gPMConstants.PMEReturnCode.PMError
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CreateOrOpenCashList", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrOpenCashList", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If

                oACTCashList.Dispose()
                oACTCashList = Nothing

            Else
                ' open up the cashlist form
                Dim temp_oACTCashList2 As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oACTCashList2, sClassName:="iACTCashList.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                oACTCashList = temp_oACTCashList2

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMError
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iCashList", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                ' Set up keys
                Dim vKeys(1, 0) As Object

                vKeys(0, 0) = PMNavKeyConst.ACTKeyNameCashListTypeId

                vKeys(1, 0) = gACTLibrary.ACTCashListTypePayments

                ' Create a cash list

                m_lReturn = oACTCashList.Initialise
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    result = gPMConstants.PMEReturnCode.PMError
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CreateOrOpenCashList", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrOpenCashList", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    oACTCashList.Dispose()
                    oACTCashList = Nothing
                    Return result
                End If

                m_lReturn = oACTCashList.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    result = gPMConstants.PMEReturnCode.PMError
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CreateOrOpenCashList", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrOpenCashList", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    oACTCashList.Dispose()
                    oACTCashList = Nothing
                    Return result
                End If

                m_lReturn = oACTCashList.SetKeys(vKeyArray:=vKeys)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    result = gPMConstants.PMEReturnCode.PMError
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CreateOrOpenCashList", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrOpenCashList", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    oACTCashList.Dispose()
                    oACTCashList = Nothing
                    Return result
                End If

                m_lReturn = oACTCashList.Start
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    result = gPMConstants.PMEReturnCode.PMError
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CreateOrOpenCashList", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrOpenCashList", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    oACTCashList.Dispose()
                    oACTCashList = Nothing
                    Return result
                End If

                ' If form not OK ie Cancelled return status

                If oACTCashList.Status = gPMConstants.PMEReturnCode.PMOK Then
                    ' Return newly created Cash List ID

                    r_lCashListId = oACTCashList.CashListID
                Else
                    result = gPMConstants.PMEReturnCode.PMCancel
                    r_lCashListId = 0
                End If

                ' Kill the Cash List object

                oACTCashList.Dispose()
                oACTCashList = Nothing

            End If

            Return result

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CreateOrOpenCashList", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrOpenCashList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    '
    ' AMB 11/02/2003 -  added flag, insured_name and insured_account columns for
    '                   IAG PS220 Manage Debtors development6
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DataToInterface"

        Dim oListItem As ListViewItem
        Dim sLookupDesc, sFormattedCurrency, sFormattedOSCurrency As String
        Dim cOSCurrencyAmount As Decimal
        Dim sImageKey, sKey As String
        Dim lItems As Integer
        Dim cTransCurrOS As Decimal
        Dim sSelectedItemBalance As String = ""
        Dim lOSCurrencyId As Integer
        Dim bOSCurrencyConsistance As Boolean
        'PN: 45764
        Dim sFormattedBranchOSBalance As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the search details.
            lvwSearchResults.Items.Clear()
            lvwSearchResults.CheckBoxes = True
            lvwSearchResults.FullRowSelect = True
            lvwSearchResults.View = View.Details
            m_cSelectedItemBalance = 0
            m_cBranchOSBalanceInBaseCurrency = 0

            ' Check that search details are valid before
            ' continuing.
            If Information.IsArray(m_vSearchData) Then
                m_lReturn = ListViewFunc.ListViewBatchStart(lvwList:=lvwSearchResults)

                lItems = m_vSearchData.GetUpperBound(1)

                'Zero running total
                cTransCurrOS = 0
                RemoveHandler lvwSearchResults.ItemChecked, AddressOf lvwSearchResults_ItemChecked
                ' Assign the details to the interface.
                For lRow As Integer = m_vSearchData.GetLowerBound(1) To lItems

                    'PN14927 - Exclude transaction raised by Cash List Item during allocation
                    'PN: 45764 If User clicks on Balance then don't display transactions
                    If Not (m_bBalanceOnly) Then
                        If CDbl(m_vSearchData(ACITransDetailId, lRow)) <> m_lExcludeTransDetailID Then

                            ' CJB 01/04/2004 Show correct icon in the listview...
                            ' note - stripped out u/w specific processing as DD said not used...
                            m_lReturn = FindListImageSmallIconToApply(v_sTransactionComment:=CStr(m_vSearchData(ACIComment, lRow)).Trim(), v_sTransactionNotReportedFlag:=CStr(m_vSearchData(ACINotReported, lRow)), r_sImageKey:=sImageKey)

                            sKey = "K" & CStr(lRow)

                            'to display the icon, changed the blank parameter to the name of the image to be displayed
                            oListItem = lvwSearchResults.Items.Add(sKey, "", sImageKey)

                            If ((lvwSearchResults.Items.Count - 1) Mod 2 = 0) Then
                                lvwSearchResults.Items(lvwSearchResults.Items.Count - 1).BackColor = Color.White
                            Else
                                lvwSearchResults.Items(lvwSearchResults.Items.Count - 1).BackColor = Color.LightYellow
                            End If

                            With oListItem

                                ' AMB 11/02/2003: PS220 - added flag column
                                .SubItems.Insert(ACListSourceID, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACISourceID, lRow)).Trim()))

                                .SubItems.Insert(ACListFlag, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACIFlag, lRow)).Trim()))

                                .SubItems.Insert(ACListBalanceType, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACIBalanceType, lRow)).Trim()))

                                .SubItems.Insert(ACListAccountShortCode, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACIAccountShortCode, lRow)).Trim()))

                                .SubItems.Insert(ACListDocumentRef, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACIDocumentRef, lRow)).Trim()))

                                If CStr(m_vSearchData(ACIDocumentRef, lRow)).Trim().Substring(0, 2).ToUpper() <> "CL" Then
                                    .SubItems.Insert(ACListAltRef, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACITransReference, lRow)).Trim())) '(RC) QBENZ014
                                Else
                                    .SubItems.Insert(ACListAltRef, New ListViewItem.ListViewSubItem(oListItem, ""))
                                End If

                                ' Assign details to other the columns
                                .SubItems.Insert(ACListAccountingDate, New ListViewItem.ListViewSubItem(oListItem, gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=m_vSearchData(ACIAccountingDate, lRow))))

                                .SubItems.Insert(ACListDocumentDate, New ListViewItem.ListViewSubItem(oListItem, gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=m_vSearchData(ACIDocumentDate, lRow))))

                                .SubItems.Insert(ACListDueDate, New ListViewItem.ListViewSubItem(oListItem, gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=m_vSearchData(ACIDueDate, lRow))))

                                .SubItems.Insert(ACListMediaType, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACIMediaType, lRow)).Trim()))

                                If optAmountTransaction.Checked Then
                                    'Display amount column in transaction currency.
                                    m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_vSearchData(ACICurrencyID, lRow), vCurrencyAmount:=m_vSearchData(ACICurrencyAmount, lRow), vFormattedCurrency:=sFormattedCurrency)
                                ElseIf optAmountBase.Checked Then
                                    'Display amount column in Base currency.
                                    m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_vSearchData(ACIAmountCurrencyID, lRow), vCurrencyAmount:=m_vSearchData(ACIBaseAmount, lRow), vFormattedCurrency:=sFormattedCurrency)
                                Else
                                    'Display amount column in account currency.
                                    m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_vSearchData(ACIAccountCurrencyID, lRow), vCurrencyAmount:=m_vSearchData(ACIAccountAmount, lRow), vFormattedCurrency:=sFormattedCurrency)
                                End If


                                If m_bDisplayDebitCredit Then
                                    If Conversion.Val(CStr(m_vSearchData(ACIBaseAmount, lRow))) >= 0 Then
                                        .SubItems.Insert(ACListCurrencyAmount, New ListViewItem.ListViewSubItem(oListItem, sFormattedCurrency.Trim()))

                                        .SubItems.Insert(ACListCurrencyAmountCredit, New ListViewItem.ListViewSubItem(oListItem, " "))
                                    Else
                                        .SubItems.Insert(ACListCurrencyAmount, New ListViewItem.ListViewSubItem(oListItem, sFormattedCurrency.Trim()))

                                        .SubItems.Insert(ACListCurrencyAmountCredit, New ListViewItem.ListViewSubItem(oListItem, " "))
                                    End If
                                Else
                                    .SubItems.Insert(ACListCurrencyAmount, New ListViewItem.ListViewSubItem(oListItem, sFormattedCurrency.Trim()))

                                    .SubItems.Insert(ACListCurrencyAmountCredit, New ListViewItem.ListViewSubItem(oListItem, " "))
                                End If

                                If m_vSearchData(ACIPrimarySettled, lRow) <> "" AndAlso Not gPMFunctions.ToSafeBoolean(m_vSearchData(ACIPrimarySettled, lRow)) Then
                                    .SubItems.Insert(ACListPrimarySettled, New ListViewItem.ListViewSubItem(oListItem, g_sNo))
                                Else
                                    .SubItems.Insert(ACListPrimarySettled, New ListViewItem.ListViewSubItem(oListItem, g_sYes))
                                End If

                                cOSCurrencyAmount = ToSafeDecimal(m_vSearchData(ACIBaseAmount, lRow)) - ToSafeDecimal(m_vSearchData(ACIMatchedCurrencyAmount, lRow))

                                If optOutstandingBase.Checked Then
                                    'Display outstanding column in base currency.

                                    m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_vSearchData(ACIAmountCurrencyID, lRow), vCurrencyAmount:=m_vSearchData(ACIOutstandingBaseAmount, lRow), vFormattedCurrency:=sFormattedOSCurrency)

                                    cTransCurrOS += ToSafeDecimal(m_vSearchData(ACIOutstandingBaseAmount, lRow))

                                    If lRow = 0 Then
                                        lOSCurrencyId = CInt(m_vSearchData(ACIAmountCurrencyID, lRow))
                                        bOSCurrencyConsistance = True
                                    Else
                                        If lOSCurrencyId <> CDbl(m_vSearchData(ACIAmountCurrencyID, lRow)) Then
                                            bOSCurrencyConsistance = False
                                        End If
                                    End If

                                ElseIf optOutstandingTransaction.Checked Then
                                    'Display outstanding column in Transaction currency.

                                    m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_vSearchData(ACICurrencyID, lRow), vCurrencyAmount:=m_vSearchData(ACIOutstandingTransAmount, lRow), vFormattedCurrency:=sFormattedOSCurrency)

                                    cTransCurrOS += ToSafeDecimal(m_vSearchData(ACIOutstandingTransAmount, lRow))

                                    If lRow = 0 Then
                                        lOSCurrencyId = CInt(m_vSearchData(ACICurrencyID, lRow))
                                        bOSCurrencyConsistance = True
                                    Else
                                        If lOSCurrencyId <> CDbl(m_vSearchData(ACICurrencyID, lRow)) Then
                                            bOSCurrencyConsistance = False
                                        End If
                                    End If
                                Else
                                    'Display outstanding column in account currency.

                                    m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_vSearchData(ACIAccountCurrencyID, lRow), vCurrencyAmount:=m_vSearchData(ACIOutstandingAccountAmount, lRow), vFormattedCurrency:=sFormattedOSCurrency)

                                    cTransCurrOS += ToSafeDecimal(m_vSearchData(ACIOutstandingAccountAmount, lRow))

                                    If lRow = 0 Then
                                        lOSCurrencyId = CInt(m_vSearchData(ACIAccountCurrencyID, lRow))
                                        bOSCurrencyConsistance = True
                                    Else
                                        If lOSCurrencyId <> CDbl(m_vSearchData(ACIAccountCurrencyID, lRow)) Then
                                            bOSCurrencyConsistance = False
                                        End If
                                    End If
                                End If

                                If m_bDisplayDebitCredit Then
                                    If Conversion.Val(CStr(cOSCurrencyAmount)) >= 0 Then
                                        .SubItems.Insert(ACListOSCurrencyAmount, New ListViewItem.ListViewSubItem(oListItem, sFormattedOSCurrency.Trim()))

                                        .SubItems.Insert(ACListOSCurrencyAmountCredit, New ListViewItem.ListViewSubItem(oListItem, " "))
                                    Else
                                        .SubItems.Insert(ACListOSCurrencyAmount, New ListViewItem.ListViewSubItem(oListItem, " "))

                                        .SubItems.Insert(ACListOSCurrencyAmountCredit, New ListViewItem.ListViewSubItem(oListItem, sFormattedOSCurrency.Trim()))
                                    End If
                                Else
                                    .SubItems.Insert(ACListOSCurrencyAmount, New ListViewItem.ListViewSubItem(oListItem, sFormattedOSCurrency.Trim()))

                                    .SubItems.Insert(ACListOSCurrencyAmountCredit, New ListViewItem.ListViewSubItem(oListItem, " "))
                                End If

                                If m_vSearchData(ACIOutstandingBaseAmount, lRow).Equals(m_vSearchData(ACIBaseAmount, lRow)) Then
                                    .SubItems.Insert(ACListMatchDate, New ListViewItem.ListViewSubItem(oListItem, "                    "))
                                Else
                                    .SubItems.Insert(ACListMatchDate, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACIAmountUpdated, lRow)).Trim()))
                                End If

                                .SubItems.Insert(ACListPaymentDueDate, New ListViewItem.ListViewSubItem(oListItem, " "))

                                m_lReturn = GetLookupDesc(lLookupRow:=ACLDocumentType, lLookupID:=CInt(m_vSearchData(ACIDocumentTypeId, lRow)), sLookupDesc:=sLookupDesc)
                                .SubItems.Insert(ACListDocumentTypeId, New ListViewItem.ListViewSubItem(oListItem, sLookupDesc))


                                If m_bIsUnderwritingBranch Then
                                    If CStr(m_vSearchData(ACIAlternateReference, lRow)).Trim() <> "" Then
                                        .SubItems.Insert(ACListInsuranceRef, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACIAlternateReference, lRow)).Trim()))
                                    Else
                                        .SubItems.Insert(ACListInsuranceRef, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACIInsuranceRef, lRow)).Trim()))
                                    End If
                                Else
                                    .SubItems.Insert(ACListInsuranceRef, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACIInsuranceRef, lRow)).Trim()))
                                End If

                                .SubItems.Insert(ACListOperatorName, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACIOperatorName, lRow)).Trim()))


                                .SubItems.Insert(ACListPeriodName, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACIPeriodName, lRow)).Trim()))

                                ' Document Type Group
                                m_lReturn = GetLookupDesc(lLookupRow:=ACLDocTypeGroup, lLookupID:=CInt(m_vSearchData(ACIDocTypeGroupId, lRow)), sLookupDesc:=sLookupDesc)
                                .SubItems.Insert(ACListDocTypeGroupId, New ListViewItem.ListViewSubItem(oListItem, sLookupDesc))

                                ' AMB 11/02/2003: PS220 - added insured_name and insured_account
                                .SubItems.Insert(ACListInsuredName, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACIInsuredName, lRow)).Trim()))

                                .SubItems.Insert(ACListInsuredAccount, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACIInsuredAccount, lRow)).Trim()))

                                .SubItems.Insert(ACListSpare, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACISpare, lRow)).Trim()))

                                ' Don't display the following uder the SFORB solution
                                ' If m_iSolutionConfig <> gACTLibrary.ACTOrionSolutionSFORB Then
                                .SubItems.Insert(ACListPurchaseInvoiceNo, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACIPurchaseOrderNo, lRow)).Trim()))

                                .SubItems.Insert(ACListPurchaseOrderNo, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACIPurchaseInvoiceNo, lRow)).Trim()))

                                .SubItems.Insert(ACListDepartment, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACIDepartment, lRow)).Trim()))
                                'End If

                                .SubItems.Insert(ACListMatchAmount, New ListViewItem.ListViewSubItem(oListItem, ListViewHelper.GetListViewSubItem(oListItem, ACListOSCurrencyAmount).Text))

                                'DD 16/05/2003
                                .SubItems.Insert(ACListPayeeName, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACIPayeeName, lRow)).Trim()))

                                'DD 16 / 5 / 2003
                                .SubItems.Insert(ACListUnderwritingYear, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACIUnderwritingYear, lRow)).Trim()))

                                ' Added period end date so that when sorting by period we
                                ' can correctly sort the transactions
                                .SubItems.Insert(ACListPeriodEndDate, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACIPeriodEndDate, lRow)).Trim()))

                                .SubItems.Insert(ACListClaimReference, New ListViewItem.ListViewSubItem(oListItem, ""))

                                Select Case m_vSearchData(ACIRiskTransfer, lRow)
                                    'Case DBNULL.Value
                                    Case Nothing

                                        .SubItems.Insert(ACListRiskTransfer, New ListViewItem.ListViewSubItem(oListItem, Nothing))
                                    Case 0

                                        .SubItems.Insert(ACListRiskTransfer, New ListViewItem.ListViewSubItem(oListItem, "RT"))
                                    Case 1

                                        .SubItems.Insert(ACListRiskTransfer, New ListViewItem.ListViewSubItem(oListItem, "UNPAID"))
                                    Case 2

                                        .SubItems.Insert(ACListRiskTransfer, New ListViewItem.ListViewSubItem(oListItem, "PAID"))
                                    Case 3

                                        .SubItems.Insert(ACListRiskTransfer, New ListViewItem.ListViewSubItem(oListItem, "PAIDM"))
                                    Case 4

                                        .SubItems.Insert(ACListRiskTransfer, New ListViewItem.ListViewSubItem(oListItem, "REC"))
                                End Select


                                .SubItems.Insert(ACListAgentName, New ListViewItem.ListViewSubItem(oListItem, gPMFunctions.ToSafeString(m_vSearchData(ACIAgentName, lRow)).Trim()))
                                'PN: 74915
                                If m_sUnderwritingOrAgency = "U" Then
                                    .SubItems.Insert(ACListAgentName, New ListViewItem.ListViewSubItem(oListItem, gPMFunctions.ToSafeString(m_vSearchData(ACIAgentName, lRow)).Trim()))
                                    If gPMFunctions.ToSafeString(m_vSearchData(ACIDocumentRef, lRow)).Substring(0, 3) = "SND" And ToSafeCurrency(m_vSearchData(ACIOutstandingBaseAmount, lRow)) <> 0 And UBound(m_vSearchData) >= ACIMTAAgentName Then
                                        If gPMFunctions.ToSafeString(m_vSearchData(ACIAgentName, lRow)).Trim().ToUpper() <> gPMFunctions.ToSafeString(m_vSearchData(ACIMTAAgentName, lRow)).Trim().ToUpper() Then
                                            .SubItems.Insert(ACListAgentName, New ListViewItem.ListViewSubItem(oListItem, gPMFunctions.ToSafeString(m_vSearchData(ACIMTAAgentName, lRow)).Trim()))
                                        End If
                                    End If
                                End If

                                .SubItems.Insert(ACListBGRef, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACIBGRef, lRow)).Trim()))

                                .SubItems.Insert(ACListCaseNumber, New ListViewItem.ListViewSubItem(oListItem, CStr(m_vSearchData(ACICaseNumber, lRow)).Trim()))
                            End With

                            ' Set the tag property with the index of
                            ' the search data storage.
                            oListItem.Tag = CStr(lRow)
                            oListItem.ToolTipText = "test"
                        End If
                    End If
                    m_cSelectedItemBalance += ToSafeDecimal(m_vSearchData(ACIOutstandingBaseAmount, lRow))
                    m_cBranchOSBalanceInBaseCurrency += gPMFunctions.ToSafeCurrency(m_vSearchData(ACIOutstandingBaseAmount, lRow))
                Next lRow
                AddHandler lvwSearchResults.ItemChecked, AddressOf lvwSearchResults_ItemChecked
            End If

            m_lReturn = m_oCurrencyConvert.GETBASECURRENCY(v_lCompanyID:=g_iSourceID, r_iBaseCurrencyID:=m_iBaseCurrencyID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oCurrencyConvert.GETBASECURRENCY", "v_lCompanyId:=" & g_iSourceID, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Dim nBaseAmont As Decimal

            'm_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=lOSCurrencyId, lCompanyID:=g_iSourceID, cCurrencyAmount:=m_cSelectedItemBalance, cBaseAmount:=nBaseAmont)


            m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iBaseCurrencyID, vCurrencyAmount:=m_cSelectedItemBalance, vFormattedCurrency:=sSelectedItemBalance)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("FormatCurrency", "Failed to get Selected Balance", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc)
            If Not m_bInsuredAccountView Then
                panSelectedItemBalance.Text = sSelectedItemBalance
            End If
            ' End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc)

            If cmbCurrency.ListIndex = 0 Then
                panTranCurrOS.Text = ""
            Else
                If bOSCurrencyConsistance Then

                    m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=lOSCurrencyId, vCurrencyAmount:=cTransCurrOS, vFormattedCurrency:=sFormattedOSCurrency)

                    panTranCurrOS.Text = sFormattedOSCurrency
                Else
                    panTranCurrOS.Text = ""
                End If
            End If


            'PN: 45764
            'Set Branch OS balance in Base Curreny:
            'nBaseAmont = 0
            'm_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=lOSCurrencyId, lCompanyID:=g_iSourceID, cCurrencyAmount:=m_cBranchOSBalanceInBaseCurrency, cBaseAmount:=nBaseAmont)

            m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iBaseCurrencyID, vCurrencyAmount:=m_cBranchOSBalanceInBaseCurrency, vFormattedCurrency:=sFormattedBranchOSBalance)

            ' Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc)
            If Not m_bInsuredAccountView Then
                BranchOsBalInBaseCurrency.Text = sFormattedBranchOSBalance
            End If
            ' End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc)

            ' Size the columns
            If lvwSearchResults.Items.Count > 0 Then
                ListView6Autosize(lvwList:=lvwSearchResults)

                'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.2.1)
                If g_sUserConfigXMLDataset <> "" Then
                    m_lReturn = LoadInterfaceDisplaySettings()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "LoadInterfaceDisplaySettings Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Else
                    ' AMB 11/02/2003: PS220 - changed the below to use the actual columnheader constants instead of numbers
                    lvwSearchResults.Columns.Item(ACListCheckboxID).Width = CInt(VB6.TwipsToPixelsX(500))
                    lvwSearchResults.Columns.Item(ACListSourceID).Width = CInt(VB6.TwipsToPixelsX(500))
                    lvwSearchResults.Columns.Item(ACListAccountShortCode).Width = CInt(VB6.TwipsToPixelsX(1400))
                    lvwSearchResults.Columns.Item(ACListDocumentRef).Width = CInt(VB6.TwipsToPixelsX(1800))
                    ' lvwSearchResults.Columns.Item(ACListDueDate).Width = CInt(VB6.TwipsToPixelsX(1400))
                    lvwSearchResults.Columns.Item(ACListAltRef).Width = CInt(VB6.TwipsToPixelsX(1400))

                    lvwSearchResults.Columns.Item(ACListAccountingDate).Width = CInt(VB6.TwipsToPixelsX(1400))
                    lvwSearchResults.Columns.Item(ACListPayeeName).Width = CInt(VB6.TwipsToPixelsX(1000))
                    lvwSearchResults.Columns.Item(ACListPayeeName).Width = CInt(VB6.TwipsToPixelsX(1000))
                End If

                lvwSearchResults.Columns.Item(ACListFlag).Width = CInt(0)

                'If m_iSolutionConfig = gACTLibrary.ACTOrionSolutionSFORB Then
                'AMJ310701  Removed size change for media ref column at RHA's request... well effectively
                'lvwSearchResults.ColumnHeaders(ACListSpare+1).Width = 0
                lvwSearchResults.Columns.Item(ACListPurchaseInvoiceNo).Width = CInt(0)
                'lvwSearchResults.Columns.Item(ACListPurchaseOrderNo).Width = CInt(0)
                lvwSearchResults.Columns.Item(ACListDepartment).Width = CInt(0)
                'eck070801 - Hide the match amount column
                lvwSearchResults.Columns.Item(ACListMatchAmount).Width = CInt(0)

                'Else
                ' lvwSearchResults.Columns.Item(ACListClaimReference).Width = CInt(0)
                'End If

                ' RDT - 10/04/2003 - Show the Credit Debit Columns anyway
                If m_bDisplayDebitCredit Then
                    lvwSearchResults.Columns.Item(ACListCurrencyAmount).Width = CInt(VB6.TwipsToPixelsX(1000))
                    lvwSearchResults.Columns.Item(ACListCurrencyAmountCredit).Width = CInt(VB6.TwipsToPixelsX(1000))
                    lvwSearchResults.Columns.Item(ACListOSCurrencyAmount).Width = CInt(VB6.TwipsToPixelsX(1000))
                    lvwSearchResults.Columns.Item(ACListOSCurrencyAmountCredit).Width = CInt(VB6.TwipsToPixelsX(1000))
                Else
                    lvwSearchResults.Columns.Item(ACListCurrencyAmountCredit).Width = CInt(VB6.TwipsToPixelsX(300))
                    lvwSearchResults.Columns.Item(ACListOSCurrencyAmountCredit).Width = CInt(VB6.TwipsToPixelsX(300))
                End If

                'DD 03/06/2003: Code Merge
                lvwSearchResults.Columns.Item(ACListPayeeName).Width = CInt(VB6.TwipsToPixelsX(1000))

                lvwSearchResults.Columns.Item(ACListMediaType).Width = CInt(VB6.TwipsToPixelsX(900))

                ' hide the period end date as this information shouldnt be displayed to
                ' the user
                lvwSearchResults.Columns.Item(ACListPeriodEndDate).Width = CInt(0)

                'Set the Balance Type Field Width
                'Show the Balance Type Field if the Account is an Agent with either
                'Float Balance or Overdraft Option set

                'panAccountBalance.Visible = False
                'lblAccountBalance.Visible = False
                If CStr(m_vSearchData(ACIPartyType, 0)).Trim() = "AG" And (CDbl(m_vSearchData(ACIIsFlaotBalanceAccount, 0)) = 1 Or CDbl(m_vSearchData(ACIIsOverdraftAccount, 0)) = 1) Then
                    lvwSearchResults.Columns.Item(ACListBalanceType).Width = CInt(VB6.TwipsToPixelsX(1000))
                    stbStatus.Items.Item(2).Visible = CDbl(m_vSearchData(ACIIsFlaotBalanceAccount, 0)) = 1

                    stbStatus.Items.Item(3).Visible = CDbl(m_vSearchData(ACIIsOverdraftAccount, 0)) = 1
                Else
                    lvwSearchResults.Columns.Item(ACListBalanceType).Width = CInt(0)
                    'Hidethe panels for Overdraft and float balance

                    stbStatus.Items.Item(2).Visible = False
                    stbStatus.Items.Item(3).Visible = False
                End If
                'Set ledger style with alternating colours
                SetListViewLedger(lvwSearchResults, 3)
            End If
            For nCnt As Integer = 1 To ACListCaseNumber
                If lvwSearchResults.Columns.Item(nCnt).Text.Trim.Length <= 2 Then
                    lvwSearchResults.Columns.Item(nCnt).Width = 0
                End If
            Next

            ' Activate the list view
            m_lReturn = ListViewFunc.ListViewBatchEnd()
            DisplayStatusFound()

            ' Enable the interface now that the search
            ' has completed.
            DisableInterface(bDisable:=False)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToProperties
    '
    ' Description: Wrapper for DataToProperties calls
    '
    ' ***************************************************************** '

    Public Function DataToProperties() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Skip this if called from Work Manager			
            If m_sCallingAppName = "iPMWrkComponentStarter" Or m_sCallingAppName = "iPMBOrionLink" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            ElseIf Not m_bRollup Then
                Return DataToPropertiesNormal()
            Else
                Return DataToPropertiesRollup()
            End If

            Return result

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            Return result

        End Try

    End Function

    ' ***************************************************************** '
    ' Name: DataToPropertiesNormal
    '
    ' Description: Updates the property member from the search data
    '              storage.
    '
    '   AMB 11/02/2003 - added insured_name, insured_account
    '   and flag columns/fields for IAG PS220 Manage Debtors
    ' ***************************************************************** '

    Public Function DataToPropertiesNormal() As Integer

        Dim result As Integer = 0
        Dim oSelectedItem As iACTFindTransaction.SelectedItem
        Dim lSelectedItem As Integer
        Dim sFormattedBase As String = ""
        Dim iItemIdx As Integer
        Dim bAlreadyLocked As Boolean
        Dim lTransDetailId As Integer
        Dim vLockedTransDetailIds As Object
        Dim lLockedTransDetailIdsCnt As Integer
        Dim bTransactionOutstanding As Boolean
        Dim cOSBaseAmount, cTotalSelected As Decimal
        Dim sMessage As String = ""
        Dim lAnswer As DialogResult
        Dim bTransdetailIds As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.

            If (lvwSearchResults.CheckedItems.Count > 0) Then
                lSelectedItem = Convert.ToString(lvwSearchResults.Items.Item(lvwSearchResults.CheckedItems(0).Index).Tag)
            End If

            ' Update the property members.

            m_lTransdetailID = CInt(m_vSearchData(ACITransDetailId, lSelectedItem))
            m_lAccountID = CInt(m_vSearchData(ACIAccountId, lSelectedItem))
            m_sDocumentRef = CStr(m_vSearchData(ACIDocumentRef, lSelectedItem)).Trim()

            ReDim m_vTransdetailIDs(0, 0)

            'do this for payments too
            'Allow for claims too.
            If (m_lCashListTypeID = gACTLibrary.ACTCashListTypeReceipts) Or (m_lCashListTypeID = gACTLibrary.ACTCashListTypePayments) Or (m_lCashListTypeID = gACTLibrary.ACTCashListTypeClaimPayments) Then
                'Reselect all the outstanding transactions to make sure they
                'have not been allocated since the initial selection
                m_lReturn = BuildOSTransactionsCollection()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="BuildOSTransactionsCollection Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToPropertiesNormal")
                    Return result
                End If
            End If
            'SJ 14/05/2003 - end

            iItemIdx = 0
            cTotalSelected = 0
            m_oInterface.SelectedItems.Clear()

            'Create instance of SelectedItem to store for return of results to caller
            If (lvwSearchResults.CheckedItems.Count > 0) Then

                For Each oListItem As ListViewItem In lvwSearchResults.Items

                    If oListItem.Checked Then

                        lTransDetailId = CInt(m_vSearchData(ACITransDetailId, Convert.ToString(oListItem.Tag)))
                        cOSBaseAmount = CDec(m_vSearchData(ACIOutstandingBaseAmount, Convert.ToString(oListItem.Tag)))
                        cTotalSelected += cOSBaseAmount

                        m_lReturn = g_oBusiness.GetMarkTransdetails(v_lDocumentID:=m_vSearchData(ACIDocDocumentID, lSelectedItem), v_lTransDetailId:=m_vSearchData(ACITransDetailId, lSelectedItem), v_lAccountId:=m_vSearchData(ACIAccountId, lSelectedItem), r_bTransdetailIds:=bTransdetailIds)

                        If bTransdetailIds Then
                            MessageBox.Show("Transaction/s selected for allocation are marked in Insurer/Agent Payment or Subagent settlement. To proceed please unmark the transaction.", "Incorrect Selection", MessageBoxButtons.OK)
                            Return result
                        End If

                        'First check to see if the transaction is still outstanding
                        m_lReturn = m_cOSTransactions.Item(v_vKey:=CStr(lTransDetailId), r_vExists:=bTransactionOutstanding)

                        If bTransactionOutstanding Then
                            'We need to lock this transdetail_id
                            m_lReturn = LockTransdetailId(v_lTransDetailId:=lTransDetailId, r_bAlreadyLocked:=bAlreadyLocked)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="LockTransdetailId Failed for transdetailid " & lTransDetailId, vApp:=ACApp, vClass:=ACClass, vMethod:="DataToPropertiesNormal")
                                Return result
                            End If
                        End If

                        'If the tranaction has already been allocated or is locked by another user
                        If Not bTransactionOutstanding Or bAlreadyLocked Then
                            'Unlock all of the transactions that have already been selected.

                            m_lReturn = UnLockTransdetailId(v_vLockedTransDetailIds:=vLockedTransDetailIds)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UnLockTransdetailId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToPropertiesNormal")
                                Return result
                            End If

                            Return gPMConstants.PMEReturnCode.PMRecordInUse
                        Else
                            If Not Information.IsArray(vLockedTransDetailIds) Then
                                ReDim vLockedTransDetailIds(0, 0)
                            Else
                                lLockedTransDetailIdsCnt += 1
                                ReDim Preserve vLockedTransDetailIds(0, lLockedTransDetailIdsCnt)
                            End If

                            vLockedTransDetailIds(0, lLockedTransDetailIdsCnt) = lTransDetailId
                        End If

                        'DD 26/04/2004 select a new item here. Doing it outside the loop
                        'will replace the items array with duplicates
                        oSelectedItem = New iACTFindTransaction.SelectedItem()

                        With oSelectedItem
                            .TransDetailId = CInt(m_vSearchData(ACITransDetailId, Convert.ToString(oListItem.Tag)))
                            .DocumentRef = CStr(m_vSearchData(ACIDocumentRef, Convert.ToString(oListItem.Tag))).Trim()
                            .AccountingDate = CDate(ListViewHelper.GetListViewSubItem(oListItem, ACListAccountingDate).Text)
                            .PeriodName = CStr(m_vSearchData(ACIPeriodName, Convert.ToString(oListItem.Tag))).Trim()
                            .CurrencyID = CInt(m_vSearchData(ACICurrencyID, Convert.ToString(oListItem.Tag)))
                            Dim dbNumericTemp As Double
                            If Double.TryParse(CStr(m_vSearchData(ACICurrencyAmount, Convert.ToString(oListItem.Tag))), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                                .CurrencyAmount = CDec(m_vSearchData(ACICurrencyAmount, Convert.ToString(oListItem.Tag)))
                            Else
                                .CurrencyAmount = 0
                            End If
                            .FormattedCurrencyAmount = ListViewHelper.GetListViewSubItem(oListItem, ACListCurrencyAmount).Text.Trim()

                            'This is actually the outstanding base amount.
                            .BaseAmount = CDec(m_vSearchData(ACIOutstandingBaseAmount, Convert.ToString(oListItem.Tag)))

                            m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_vSearchData(ACIAmountCurrencyID, Convert.ToString(oListItem.Tag)), vCurrencyAmount:=m_vSearchData(ACIBaseAmount, Convert.ToString(oListItem.Tag)), vFormattedCurrency:=sFormattedBase)

                            .FormattedBaseAmount = sFormattedBase
                            .DocumentTypeID = CInt(m_vSearchData(ACIDocumentTypeId, Convert.ToString(oListItem.Tag)))
                            .DocumentTypeDescription = ListViewHelper.GetListViewSubItem(oListItem, ACListDocumentTypeId).Text.Trim()
                            .InsuranceRef = CStr(m_vSearchData(ACIInsuranceRef, Convert.ToString(oListItem.Tag))).Trim()
                            .OperatorName = CStr(m_vSearchData(ACIOperatorName, Convert.ToString(oListItem.Tag))).Trim()
                            .DocTypeGroupID = CInt(m_vSearchData(ACIDocTypeGroupId, Convert.ToString(oListItem.Tag)))
                            .DocTypeGroupDescription = ListViewHelper.GetListViewSubItem(oListItem, ACListDocTypeGroupId).Text.Trim()
                            .PurchaseInvoiceNo = CStr(m_vSearchData(ACIPurchaseInvoiceNo, Convert.ToString(oListItem.Tag))).Trim()
                            .PurchaseOrderNo = CStr(m_vSearchData(ACIPurchaseOrderNo, Convert.ToString(oListItem.Tag))).Trim()
                            .Department = CStr(m_vSearchData(ACIDepartment, Convert.ToString(oListItem.Tag))).Trim()
                            .Spare = CStr(m_vSearchData(ACISpare, Convert.ToString(oListItem.Tag))).Trim()
                            .AccountID = CInt(m_vSearchData(ACIAccountId, Convert.ToString(oListItem.Tag)))
                            .AccountShortCode = CStr(m_vSearchData(ACIAccountShortCode, Convert.ToString(oListItem.Tag))).Trim()
                            .Reason = CStr(m_vSearchData(ACIReason, Convert.ToString(oListItem.Tag))).Trim()
                            .Flag = CStr(m_vSearchData(ACIFlag, Convert.ToString(oListItem.Tag))).Trim()
                            .InsuredName = CStr(m_vSearchData(ACIInsuredName, Convert.ToString(oListItem.Tag))).Trim()
                            .InsuredAccount = CStr(m_vSearchData(ACIInsuredAccount, Convert.ToString(oListItem.Tag))).Trim()
                            .PayeeName = CStr(m_vSearchData(ACIPayeeName, Convert.ToString(oListItem.Tag))).Trim()
                            .CurrencyText = CStr(m_vSearchData(ACICurrencyText, Convert.ToString(oListItem.Tag))).Trim()
                            .BaseCurrencyText = CStr(m_vSearchData(ACIAmountCurrencyText, Convert.ToString(oListItem.Tag))).Trim()

                            .TransDetailTypeCode = CStr(m_vSearchData(ACIDetailTypeCode, Convert.ToString(oListItem.Tag))).Trim()
                            .TaxGroupCode = CStr(m_vSearchData(ACITaxGroupCode, Convert.ToString(oListItem.Tag))).Trim()
                            .TaxBandCode = CStr(m_vSearchData(ACITaxBandCode, Convert.ToString(oListItem.Tag))).Trim()
                            .AllocationSequence = gPMFunctions.ToSafeLong(m_vSearchData(ACIAllocSequence, Convert.ToString(oListItem.Tag)))
                            .AllocationRule = gPMFunctions.ToSafeLong(m_vSearchData(ACIAllocRule, Convert.ToString(oListItem.Tag)))

                        End With

                        m_lReturn = m_oInterface.SelectedItems.Add(oSelectedItem)

                        ReDim Preserve m_vTransdetailIDs(0, iItemIdx)

                        m_vTransdetailIDs(0, iItemIdx) = CInt(m_vSearchData(ACITransDetailId, Convert.ToString(oListItem.Tag)))
                        iItemIdx += 1

                        Dim sDocumentRef As String = ""
                        sDocumentRef = gPMFunctions.ToSafeString(m_vSearchData(ACIDocumentRef, Convert.ToString(oListItem.Tag)).Trim()).Substring(0, 3)
                        If sDocumentRef = "IND" Or sDocumentRef = "IED" Or sDocumentRef = "IRD" Or sDocumentRef = "INC" Then
                            MessageBox.Show("Instalment transactions cannot be selected for allocation where the Receipt type is 'Premium Debt'.  Use 'Instalment Debt' receipt types to allocate receipts to instalment plans", "Incorrect Selection", MessageBoxButtons.OK)
                            Return gPMConstants.PMEReturnCode.PMCancel
                        End If

                    End If
                Next oListItem

            End If

            'Is the amount selected the opposite sign of the payment/receipt.
            'If not then ask the user if they are sure.
            lAnswer = System.Windows.Forms.DialogResult.Yes
            If m_lCashListTypeID = gACTLibrary.ACTCashListTypeReceipts And cTotalSelected < 0 Then

                sMessage = "The total amount of the transactions that you have selected is the same sign as the receipt." & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "Are you sure you wish to continue?"

                lAnswer = MessageBox.Show(sMessage, "Allocating Receipt Against Credit", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            ElseIf m_lCashListTypeID = gACTLibrary.ACTCashListTypePayments And cTotalSelected > 0 Then

                sMessage = "The total amount of the transactions that you have selected is the same sign as the payment." & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "Are you sure you wish to continue?"

                lAnswer = MessageBox.Show(sMessage, "Allocating Payment Against Debit", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            End If

            If lAnswer = System.Windows.Forms.DialogResult.No Then

                m_lReturn = UnLockTransdetailId(v_vLockedTransDetailIds:=vLockedTransDetailIds)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UnLockTransdetailId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToPropertiesNormal")
                    Return result
                End If
                Return gPMConstants.PMEReturnCode.PMCancel
            End If

            ' {* USER DEFINED CODE (End) *}

            'SJ 14/05/2003 - start
            'Store these so we can unlock them later
            'KB 19/8/03 Not sure why we were only doing this for receipts
            '- seems reasonable for payments too
            If (m_lCashListTypeID = gACTLibrary.ACTCashListTypeReceipts) Or (m_lCashListTypeID = gACTLibrary.ACTCashListTypePayments) Then

                m_lReturn = g_oBusiness.UpdateUserProperty(v_sPropertyName:=PMNavKeyConst.ACTKeyNameAllocationIDs, v_vPropertyValue:=m_vTransdetailIDs)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMError
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.UpdateUserProperty Failed for " & PMNavKeyConst.ACTKeyNameAllocationIDs, vApp:=ACApp, vClass:=ACClass, vMethod:="DataToPropertiesNormal")
                    Return result
                End If
            End If
            'SJ 14/05/2003 - end

            Return result

        Catch excep As System.Exception

            ' Error Section.

            If Information.Err().Number = 91 Then
                result = gPMConstants.PMEReturnCode.PMNotFound
                txtDocumentRef.Text = ""

                UnLockTransdetailId(vLockedTransDetailIds)
            Else
                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToPropertiesNormal", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            End If
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToPropertiesRollup
    '
    ' Description: Updates the property member from the search data
    '              storage for Rollups.
    '
    '              Blagged from the old DataToProperties which is
    '              now DataToPropertiesNormal
    ' ***************************************************************** '

    Public Function DataToPropertiesRollup() As Integer

        Dim result As Integer = 0
        Const ACMethod As String = "DataToPropertiesRollup"

        Dim oSelectedItem As iACTFindTransaction.SelectedItem
        Dim lSelectedItem As Integer
        Dim sFormattedBase As String = ""
        Dim iItemIdx As Integer
        Dim bAlreadyLocked As Boolean
        Dim lTransDetailId As Integer
        Dim vLockedTransDetailIds As Object
        Dim lLockedTransDetailIdsCnt As Integer
        Dim bTransactionOutstanding As Boolean

        Dim vAllTrans(,) As Object
        Dim lAllTransLowerRow, lAllTransUpperRow As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.

            If (lvwSearchResults.CheckedItems.Count > 0) Then
                lSelectedItem = Convert.ToString(lvwSearchResults.Items.Item(lvwSearchResults.CheckedItems(0).Index).Tag)
            End If

            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}

            m_lTransdetailID = 0
            m_lAccountID = CInt(m_vSearchData(ACIAccountId, lSelectedItem))
            m_sDocumentRef = CStr(m_vSearchData(ACIDocumentRef, lSelectedItem)).Trim()

            ReDim m_vTransdetailIDs(0, 0)

            'PN14824 - allow for payments and receipts and claims.
            If (m_lCashListTypeID = gACTLibrary.ACTCashListTypeReceipts) Or (m_lCashListTypeID = gACTLibrary.ACTCashListTypePayments) Or (m_lCashListTypeID = gACTLibrary.ACTCashListTypeClaimPayments) Then
                'Reselect all the outstanding transactions to make sure they
                'have not been allocated since the initial selection
                If BuildOSTransactionsCollection() <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_bRollup = True
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", BuildOSTransactionsCollection Failed")
                End If
            End If

            'Get the transaction records for each rolled up document row selected in the listview
            If GetAllTransForSelectedDocuments(r_vAllTrans:=vAllTrans, v_bOrderBySpare:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", GetAllTransForSelectedDocuments failed")
            End If

            If Information.IsArray(vAllTrans) Then

                'Size the selected trans detail array to accept all the transactions found

                lAllTransLowerRow = vAllTrans.GetLowerBound(klRowDimension - 1)

                lAllTransUpperRow = vAllTrans.GetUpperBound(klRowDimension - 1)

                iItemIdx = 0
                m_oInterface.SelectedItems.Clear()

                'Process the 'expanded' transactions
                For lRow As Integer = lAllTransLowerRow To lAllTransUpperRow

                    oSelectedItem = New iACTFindTransaction.SelectedItem()

                    'SJ 14/05/2003 - start
                    If m_lCashListTypeID > 0 Then

                        lTransDetailId = CInt(vAllTrans(ACITransDetailId, lRow))

                        'First check to see if the transaction is still outstanding
                        m_lReturn = m_cOSTransactions.Item(v_vKey:=CStr(lTransDetailId), r_vExists:=bTransactionOutstanding)

                        'This part of the rolled-up transaction may not be outstanding so we must just
                        'note those that are to ensure we don't send zero items back

                        If bTransactionOutstanding Then
                            'We need to lock this transdetail_id
                            If LockTransdetailId(v_lTransDetailId:=lTransDetailId, r_bAlreadyLocked:=bAlreadyLocked) <> gPMConstants.PMEReturnCode.PMTrue Then

                                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "LockTransdetailId Failed for transdetailid " & lTransDetailId)
                            End If
                        End If

                        If bAlreadyLocked Then
                            'If the tranaction is locked by another user or has already
                            'been allocated

                            If UnLockTransdetailId(v_vLockedTransDetailIds:=vLockedTransDetailIds) <> gPMConstants.PMEReturnCode.PMTrue Then

                                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "UnLockTransdetailId Failed for transdetailid " & lTransDetailId)
                            End If
                            result = gPMConstants.PMEReturnCode.PMRecordInUse
                            Return result
                        ElseIf bTransactionOutstanding Then
                            If Not Information.IsArray(vLockedTransDetailIds) Then
                                ReDim vLockedTransDetailIds(0, 0)
                            Else
                                lLockedTransDetailIdsCnt += 1
                                ReDim Preserve vLockedTransDetailIds(0, lLockedTransDetailIdsCnt)
                            End If

                            vLockedTransDetailIds(0, lLockedTransDetailIdsCnt) = lTransDetailId
                        End If

                    End If
                    'SJ 14/05/2003 - end

                    If bTransactionOutstanding Then
                        With oSelectedItem

                            .TransDetailId = CInt(vAllTrans(ACITransDetailId, lRow))

                            .DocumentRef = CStr(vAllTrans(ACIDocumentRef, lRow)).Trim()

                            .AccountingDate = CDate(vAllTrans(ACIAccountingDate, lRow))

                            .PeriodName = CStr(vAllTrans(ACIPeriodName, lRow)).Trim()

                            .CurrencyID = CInt(vAllTrans(ACICurrencyID, lRow))

                            Dim dbNumericTemp As Double
                            If Double.TryParse(CStr(vAllTrans(ACICurrencyAmount, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                                .CurrencyAmount = CDec(vAllTrans(ACICurrencyAmount, lRow))
                            Else
                                .CurrencyAmount = 0
                            End If

                            .FormattedCurrencyAmount = CStr(vAllTrans(ACICurrencyAmount, lRow)).Trim()

                            .CurrencyText = CStr(vAllTrans(ACICurrencyText, lRow)).Trim()

                            .BaseCurrencyID = CDec(vAllTrans(ACIAmountCurrencyID, lRow))

                            .BaseAmount = CDec(vAllTrans(ACIOutstandingBaseAmount, lRow))

                            .BaseCurrencyText = CStr(vAllTrans(ACIAmountCurrencyText, lRow)).Trim()

                            m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=vAllTrans(ACIAmountCurrencyID, lRow), vCurrencyAmount:=vAllTrans(ACIOutstandingBaseAmount, lRow), vFormattedCurrency:=sFormattedBase)

                            .FormattedBaseAmount = sFormattedBase

                            .DocumentTypeID = CInt(vAllTrans(ACIDocumentTypeId, lRow))

                            .DocumentTypeDescription = CStr(vAllTrans(ACIDocumentTypeId, lRow)).Trim()

                            .InsuranceRef = CStr(vAllTrans(ACIInsuranceRef, lRow)).Trim()

                            .OperatorName = CStr(vAllTrans(ACIOperatorName, lRow)).Trim()

                            .DocTypeGroupID = CInt(vAllTrans(ACIDocTypeGroupId, lRow))

                            .DocTypeGroupDescription = CStr(vAllTrans(ACIDocTypeGroupId, lRow)).Trim()

                            .PurchaseInvoiceNo = CStr(vAllTrans(ACIPurchaseInvoiceNo, lRow)).Trim()

                            .PurchaseOrderNo = CStr(vAllTrans(ACIPurchaseOrderNo, lRow)).Trim()

                            .Department = CStr(vAllTrans(ACIDepartment, lRow)).Trim()

                            .Spare = CStr(vAllTrans(ACISpare, lRow)).Trim()

                            .AccountID = CInt(vAllTrans(ACIAccountId, lRow))

                            .AccountShortCode = CStr(vAllTrans(ACIAccountShortCode, lRow)).Trim()

                            .Reason = CStr(vAllTrans(ACIReason, lRow)).Trim()

                            .Flag = CStr(vAllTrans(ACIFlag, lRow)).Trim()

                            .InsuredName = CStr(vAllTrans(ACIInsuredName, lRow)).Trim()

                            .InsuredAccount = CStr(vAllTrans(ACIInsuredAccount, lRow)).Trim()

                            .PayeeName = CStr(vAllTrans(ACIPayeeName, lRow)).Trim()

                            .TransDetailTypeCode = CStr(vAllTrans(ACIDetailTypeCode, lRow)).Trim()

                            .TaxGroupCode = CStr(vAllTrans(ACITaxGroupCode, lRow)).Trim()

                            .TaxBandCode = CStr(vAllTrans(ACITaxBandCode, lRow)).Trim()
                            .AllocationSequence = gPMFunctions.ToSafeLong(vAllTrans(ACIAllocSequence, lRow))
                            .AllocationRule = gPMFunctions.ToSafeLong(vAllTrans(ACIAllocRule, lRow))

                        End With

                        m_lReturn = m_oInterface.SelectedItems.Add(oSelectedItem)

                        ReDim Preserve m_vTransdetailIDs(0, iItemIdx)

                        m_vTransdetailIDs(0, iItemIdx) = CInt(vAllTrans(ACITransDetailId, lRow))
                        iItemIdx += 1
                    End If
                Next lRow
            End If

            'Store these so we can unlock them later
            If m_lCashListTypeID > 0 Then

                If g_oBusiness.UpdateUserProperty(v_sPropertyName:=PMNavKeyConst.ACTKeyNameAllocationIDs, v_vPropertyValue:=m_vTransdetailIDs) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "m_oBusiness.UpdateUserProperty Failed for " & PMNavKeyConst.ACTKeyNameAllocationIDs)
                End If
            End If

            result = gPMConstants.PMEReturnCode.PMTrue


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case 91
                    'Criteria has invalidated the selection
                    result = gPMConstants.PMEReturnCode.PMNotFound
                    txtDocumentRef.Text = ""

                    UnLockTransdetailId(vLockedTransDetailIds)
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

    ' ***************************************************************** '
    ' Name: DisableInterface
    '
    ' Description: Disables parts of the interface while a search is
    '              in progress.
    '
    ' ***************************************************************** '
    Private Sub DisableInterface(ByRef bDisable As Boolean)

        Try
            Dim sValue As String = ""

            ' Add commands here eg.
            cmdBalance.Enabled = Not bDisable
            cmdFindNow.Enabled = Not bDisable
            cmdNewSearch.Enabled = Not bDisable

            ' Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.3.2.3)
            If m_bIsBatch Or m_bInsuredAccountView Or m_sDocumentRef <> String.Empty Then
                cmdNewSearch.Enabled = False
            End If
            ' End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.3.2.3)

            ' Always disable these if the results listview is empty
            If lvwSearchResults.Items.Count = 0 Then
                bDisable = True
            End If

            cmdOK.Enabled = Not bDisable

            If m_sCallingAppName = OrionLink Or m_sCallingAppName.ToUpper() = ("iActFindTransaction").ToUpper() Then
                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=cEnableClientManagerEditing, r_sOptionValue:=sValue, v_iSourceID:=g_iSourceID)
                If sValue = "1" Then
                    'Start - Sankar - QBENZCR001 Client Manager Changes v0.1.doc - 4.1.1
                    If Not m_bInsuredAccountView Then
                        cmdAllocate.Enabled = True
                    End If
                    'End - Sankar - QBENZCR001 Client Manager Changes v0.1.doc - 4.1.1
                    m_lDisableReverse = gPMConstants.PMEReturnCode.PMFalse
                Else
                    cmdAllocate.Enabled = False
                    m_lDisableReverse = gPMConstants.PMEReturnCode.PMTrue
                End If
                If m_lDisableReverse = False Then
                    m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROTPRestrictAllocationReversal, 1, sValue)
                    If ToSafeDouble(panPolicyBalance.Text, 0.0) <> 0.0 Then
                        m_lDisableReverse = True
                    End If
                End If
            ElseIf m_sCallingAppName = "iPMWrkComponentStarter" Then
                cmdAllocate.Enabled = True
            End If

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: DisplayAccountDetails
    '
    ' Description: Get and Display selected Account details
    '
    ' AMB 12/02/2003: PS220 - Added ledger_id
    ' ***************************************************************** '
    Private Function DisplayAccountDetails() As Integer

        Dim result As Integer = 0
        Static lPreviousAccountID As Integer

        Dim lAccountID, lInsuredAccountID As Integer
        Dim sContactName, sPhoneAreaCode, sPhoneNumber, sPhoneExtension As String
        Dim cAccountBalance As Decimal
        Dim sFormattedBalance, sStatusCode As String
        Dim iStatusID As Integer
        Dim sAccountName As String = ""
        Dim dtAccountingDate As Date
        Dim lAccountCompanyID As Integer
        'Dim iBalanceCurrencyId As Integer
        'Dim iBaseCurrencyId As Integer
        'RKS 121004 PN15630
        'Dim iAccountCurrencyID As Integer
        Dim sAccountCurrencyDescription As String = ""
        Dim lLedgerID As Integer
        Dim sLedgerShortName As String = ""
        Dim dAccountDebt As Double
        Dim sFormattedAccountDebt As String = ""
        Dim dInstalmentDebt As Double
        Dim sFormattedInstalmentDebt As String = ""
        Dim vResultArray(,) As Object
        'Dim bMultipleOSCurrencies As Boolean
        Dim OSCurrency As Boolean
        Dim lRestrictEnquiry As Integer 'AR20050414 - PN12439
        Dim cPolicyBalance As Decimal
        Dim sPolicyBalance As String = ""
        Dim cPremiumFinanceBalance As Decimal
        Dim sPremiumFinanceBalance As String = ""
        Dim lInsuranceFileCnt As Integer
        Dim vPremiumFinanceBalance, vPolicyBalance As Object
        Dim sFormattedBranchOSBalance As String = ""

        Const kPolicyBalanceAccountID As Integer = 1
        Const kPoicyBalanceAmount As Integer = 0
        Const kPremiumFinanceAccountID As Integer = 1
        Const kPremiumFinanceAmount As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'BB Because of way control currently works, only control ID if lookup performed
            If Strings.Len(Convert.ToString(txtAccountCode.Tag)) > 0 Then
                lAccountID = CInt(Convert.ToString(txtAccountCode.Tag))
            Else
                lAccountID = m_lAccountID
            End If

            'DD 11/04/2003
            If Strings.Len(Convert.ToString(txtInsuredAccountCode.Tag)) > 0 Then
                lInsuredAccountID = CInt(Convert.ToString(txtInsuredAccountCode.Tag))
            Else
                lInsuredAccountID = 0
            End If

            If chkBalance.CheckState = CheckState.Checked Then
                dtAccountingDate = #12/31/9999#
            Else
                dtAccountingDate = DateTime.Today
            End If

            ' Get the Account details
            'eck180901 return currrency id if required

            ' AMB 12/02/2003: PS220 - Added ledger_id
            'MKW110403 PN3414 added return values r_lLedgerID and r_sLedgerShortName

            'Thinh Nguyen - move this up here because drilling from bank reconcilliation account_id is zero
            Dim sFormattedFloatBalance, sFormattedOverDraftBalance As String
            If lAccountID <> 0 Then

                m_lReturn = m_oAccount.GetDetails(vAccountID:=lAccountID)
                'AR20050414 - PN12439 Retrieve Restrict Enquiry flag

                m_lReturn = m_oAccount.GetNext(vCurrencyID:=m_iAccountCurrencyID, vAccountName:=sAccountName, vContactName:=sContactName, vPhoneAreaCode:=sPhoneAreaCode, vPhoneNumber:=sPhoneNumber, vPhoneExtension:=sPhoneExtension, vLedgerId:=lLedgerID, vPartySourceID:=lAccountCompanyID, vRestrictEnquiry:=lRestrictEnquiry)

                'Get base currency from accounts branch

                m_lReturn = m_oCurrencyConvert.GETBASECURRENCY(v_lCompanyID:=lAccountCompanyID, r_iBaseCurrencyID:=m_iBaseCurrencyID)

                m_lReturn = m_oAccount.GetAccountBalance(v_vAccountID:=lAccountID, v_vAccountingDate:=dtAccountingDate, r_vResultArray:=vResultArray)

                m_lReturn = m_oAccount.GetAccountStatus(v_lAccountId:=lAccountID, r_iAccountStatus:=iStatusID, r_sAccountCode:=sStatusCode)

                'DD 29/9/2004 PN15194 - empty accounts will not return anything
                If Information.IsArray(vResultArray) Then

                    If CDbl(vResultArray(0, 0)) = 0 And vResultArray.GetUpperBound(1) = 0 Then
                        m_bMultipleOSCurrencies = False
                        cAccountBalance = 0
                        m_iBalanceCurrencyId = m_iAccountCurrencyID
                    Else
                        'Check to see if multiple outstanding currency totals are returned.
                        'This would be caused by changing the accounts currency whilst old transactions are still outstanding.
                        m_bMultipleOSCurrencies = False

                        For lLoop As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                            If gPMFunctions.ToSafeDouble(vResultArray(0, lLoop)) <> 0 Then
                                If Not OSCurrency Then
                                    OSCurrency = True
                                    cAccountBalance = gPMFunctions.ToSafeCurrency(vResultArray(0, lLoop))
                                    m_iBalanceCurrencyId = gPMFunctions.ToSafeInteger(vResultArray(1, lLoop))
                                Else
                                    m_bMultipleOSCurrencies = True
                                End If
                            End If
                        Next
                    End If
                Else
                    m_bMultipleOSCurrencies = False
                    cAccountBalance = 0
                    m_iBalanceCurrencyId = m_iAccountCurrencyID
                End If

                If Information.IsArray(vResultArray) Then
                    If m_iBalanceCurrencyId <> 0 Then
                        'Account is a bank so format in own currency.

                        m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iBalanceCurrencyId, vCurrencyAmount:=gPMFunctions.ToSafeCurrency(vResultArray(2, 0)), vFormattedCurrency:=sFormattedFloatBalance)
                    Else
                        'Account is not a bank so format in base currency.

                        m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iBaseCurrencyID, vCurrencyAmount:=gPMFunctions.ToSafeCurrency(vResultArray(2, 0)), vFormattedCurrency:=sFormattedFloatBalance)
                    End If
                    If m_iBalanceCurrencyId <> 0 Then
                        'Account is a bank so format in own currency.

                        m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iBalanceCurrencyId, vCurrencyAmount:=gPMFunctions.ToSafeCurrency(vResultArray(3, 0)), vFormattedCurrency:=sFormattedOverDraftBalance)
                    Else
                        'Account is not a bank so format in base currency.

                        m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iBaseCurrencyID, vCurrencyAmount:=gPMFunctions.ToSafeCurrency(vResultArray(3, 0)), vFormattedCurrency:=sFormattedOverDraftBalance)
                    End If

                    stbStatus.Items.Item(2).Text = "Float Balance  :  " & sFormattedFloatBalance.Trim()
                    stbStatus.Items.Item(3).Text = "OverDraft Balance  :  " & sFormattedOverDraftBalance.Trim()

                End If
                'DC140706 Datasure -make some variable global so used somewhere else
                If m_bMultipleOSCurrencies Then
                    sFormattedBalance = "N/A - Multiple O/S Currencies"
                Else
                    If m_iBalanceCurrencyId <> 0 Then
                        'Account is a bank so format in own currency.

                        m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iBalanceCurrencyId, vCurrencyAmount:=cAccountBalance, vFormattedCurrency:=sFormattedBalance)
                    Else
                        'Account is not a bank so format in base currency.

                        m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iBaseCurrencyID, vCurrencyAmount:=cAccountBalance, vFormattedCurrency:=sFormattedBalance)
                    End If
                End If

                'Get the account currency for displaying on option buttons.

                m_lReturn = m_oCurrency.GetDetails(vCurrencyID:=m_iAccountCurrencyID)

                m_lReturn = m_oCurrency.GetNext(vDescription:=sAccountCurrencyDescription)

                optAmountAccount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)) & " (" & sAccountCurrencyDescription.Trim() & ")"

                optOutstandingAccount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)) & " (" & sAccountCurrencyDescription.Trim() & ")"

                ' Setup contents of panels
                'added following check to remove object reference error
                'start

                If Information.IsNothing(sAccountName) Then
                    panAccountName.Text = ""
                ElseIf (UserPartyArray(kUserAgentCnt, 0) <> "" AndAlso txtAccountCode.Text <> String.Empty) OrElse UserPartyArray(kUserAgentCnt, 0) = "" Then
                    panAccountName.Text = sAccountName.Trim()
                End If
                If Information.IsNothing(sContactName) Then
                    panContactName.Text = ""
                Else
                    panContactName.Text = sContactName.Trim()
                End If
                If Information.IsNothing(sPhoneAreaCode) Then
                    panPhoneAreaCode.Text = ""
                Else
                    panPhoneAreaCode.Text = " " & sPhoneAreaCode.Trim()
                End If
                If Information.IsNothing(sPhoneNumber) Then
                    panPhoneNumber.Text = ""
                Else
                    panPhoneNumber.Text = " " & sPhoneNumber.Trim()
                End If
                If Information.IsNothing(sPhoneExtension) Then
                    panPhoneExtension.Text = ""
                Else
                    panPhoneExtension.Text = " " & sPhoneExtension.Trim()
                End If
                'end

                'AR20050414 - PN12439 Only show balance if not restricted
                If Not (lRestrictEnquiry = -1 And m_lHasUnrestrictedEnquiry = 0) Then
                    stbStatus.Items.Item(1).Text = "Account Balance  :  " & sFormattedBalance.Trim()
                    panAccountBalance.Text = sFormattedBalance.Trim()
                Else
                    panAccountBalance.Text = ""
                    stbStatus.Items.Item(1).Text = ""
                End If

                sStatusCode = sStatusCode.Trim()
                If sStatusCode.Length > 0 AndAlso Not String.IsNullOrEmpty(txtAccountCode.Text) Then
                    panStatus.Text = sStatusCode.Substring(0, 1).ToUpper() & sStatusCode.Substring(sStatusCode.Length - (sStatusCode.Length - 1)).ToLower()
                End If

                'If account currency differs from base currency then display account amounts.
                'DC220205 : PN18944 : Only if underwriting

                If m_iAccountCurrencyID <> m_iBaseCurrencyID And lPreviousAccountID <> lAccountID And Not Information.IsArray(m_vSearchParams) Then
                    optAmountAccount.Checked = True
                    optOutstandingAccount.Checked = True
                End If

                m_lReturn = m_oAccount.GetAccountLedger(v_lAccountId:=m_lAccountID, v_lLedgerId:=m_lLedgerID, v_sLedgerCode:=m_sLedgerName)

                lPreviousAccountID = lAccountID
            Else
                m_lLedgerID = 0
                m_sLedgerName = ""
            End If

            'DD 11/04/2003: Get Insured Account Name

            'KB PN 5893 4/8/03 Include LedgerShortName in the call
            '   and use r_vdInstalmentDebt rather than r_cInstalmentDebt
            If lInsuredAccountID <> 0 Then
                'RKS 121004 PN15630 - corrected the named argument

                m_lReturn = g_oBusiness.GetAccountDetails(r_lAccountID:=lInsuredAccountID, sAccountName:=sAccountName, sContactName:=sContactName, sPhoneAreaCode:=sPhoneAreaCode, sPhoneNumber:=sPhoneNumber, sPhoneExtension:=sPhoneExtension, r_vdAccountBalance:=cAccountBalance, r_sAccountCode:=sStatusCode, r_vlAccountCurrencyID:=m_iAccountCurrencyID, v_dtAccountingDate:=dtAccountingDate, r_lLedgerID:=lLedgerID, r_sLedgerShortName:=sLedgerShortName, r_vdAccountDebt:=dAccountDebt, r_vdInstalmentDebt:=dInstalmentDebt, v_bCalculateAccountBalance:=False)

                panInsuredAccountName.Text = sAccountName.Trim()
            End If

            m_lReturn = g_oBusiness.GetPremiumFinanceBalance(v_lAccountId:=lAccountID, v_dAccountingDate:=dtAccountingDate, v_sInsurance_ref:=m_sInsuranceRef, r_vResultArray:=vPremiumFinanceBalance, v_lInsuranceFileCnt:=lInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetPremiumFinanceBalance", "Failed to get Premium Finance Balance", gPMConstants.PMELogLevel.PMLogError)
            End If

            cPremiumFinanceBalance = CDec(vPremiumFinanceBalance(kPremiumFinanceAccountID, kPremiumFinanceAmount))

            If m_iBaseCurrencyID = 0 Then

                m_lReturn = m_oCurrencyConvert.GETBASECURRENCY(v_lCompanyID:=g_iSourceID, r_iBaseCurrencyID:=m_iBaseCurrencyID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oCurrencyConvert.GETBASECURRENCY", "v_lCompanyId:=" & g_iSourceID, gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iBaseCurrencyID, vCurrencyAmount:=cPremiumFinanceBalance, vFormattedCurrency:=sPremiumFinanceBalance)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("FormatCurrency", "Failed to Format Preimum Finance Balance", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc)
            If Not m_bInsuredAccountView Then
                panPremiumFinanceBalance.Text = sPremiumFinanceBalance
            End If
            ' End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc)
            If lAccountID <> 0 Then
                If m_sInsuranceRef.Trim() <> "" Then

                    m_lReturn = g_oBusiness.GetPolicyBalance(v_lAccountId:=lAccountID, v_dAccountingDate:=dtAccountingDate, v_sInsurance_ref:=m_sInsuranceRef, r_vResultArray:=vPolicyBalance, v_lInsuranceFileCnt:=lInsuranceFileCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("GetPolicyBalance", "Failed to Get Policy Balance", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    cPolicyBalance = CDec(vPolicyBalance(kPolicyBalanceAccountID, kPoicyBalanceAmount))
                End If

                m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iBaseCurrencyID, vCurrencyAmount:=cPolicyBalance, vFormattedCurrency:=sPolicyBalance)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("FormatCurrency", "Failed to Format Policy Balance", gPMConstants.PMELogLevel.PMLogError)
                End If

                panPolicyBalance.Text = sPolicyBalance
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the account details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayAccountDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.
            If m_bRollup Then
                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitleRollup, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Else
                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
                Return result
            End If

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdAllocate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMatchTransButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdViewAllocation.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACViewAllocationButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdReverse.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReverseButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdReverseAndReplace.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReverseAndReplaceButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdFindAccTrans.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindAccTransButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdFindDocTrans.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindDocTransButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdFindNow.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindNowButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdNewSearch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewSearchButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            For i As Integer = 0 To ACTabTitleCount - 1
                SSTabHelper.SetTabCaption(tabMain, i, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle + i, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Next i
            lvwSearchResults.Columns.Item(ACListCheckboxID).Text = ""
            lvwSearchResults.Columns.Item(ACListSourceID).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListFlag).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColFlag, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'Add new Column
            lvwSearchResults.Columns.Item(ACListBalanceType).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColBalanceType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListAccountShortCode).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColAccount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListDocumentRef).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColDocRef, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListAltRef).Text = "Alt. Ref." '(RC) QBENZ014			
            lvwSearchResults.Columns.Item(ACListAccountingDate).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColEffectiveDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListDocumentDate).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColTransDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListDueDate).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColDueDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListMediaType).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColMediaType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListCurrencyAmount).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListCurrencyAmountCredit).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColAmountCredit, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListPrimarySettled).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColPrimarySettled, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListOSCurrencyAmount).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColOSAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListOSCurrencyAmountCredit).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColOSAmountCredit, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListMatchDate).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColPaidDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListPaymentDueDate).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColPaymentDueDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListDocumentTypeId).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColDocType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListInsuranceRef).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColInsuranceRef, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListOperatorName).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColOperatorName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListPeriodName).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColPeriod, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListDocTypeGroupId).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColDocGroup, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListSpare).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColMediaRef, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListPurchaseInvoiceNo).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColPurchaseInvoiceNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListPurchaseOrderNo).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColPurchaseOrderNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListDepartment).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColDepartment, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchResults.Columns.Item(ACListMatchAmount).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColMatchAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' AMB 11/02/2003: PS220 - added insured_name and insured_account columns
            lvwSearchResults.Columns.Item(ACListInsuredName).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColInsuredName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListInsuredAccount).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColInsuredAccount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'RDT
            If m_bDisplayDebitCredit Then
                lvwSearchResults.Columns.Item(ACListCurrencyAmount).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColOSCredit, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                lvwSearchResults.Columns.Item(ACListOSCurrencyAmount).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColOSDebit, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            lvwSearchResults.Columns.Item(ACListPayeeName).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColPayeeName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListUnderwritingYear).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColUnderwritingYear, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListPaymentDueDate).Width = CInt(0)
            lvwSearchResults.Columns.Item(ACListPeriodEndDate).Width = CInt(0)
            lvwSearchResults.Columns.Item(ACListPeriodEndDate).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColPeriodEndDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'S4B Claim Enhancements R&D 2005
            lvwSearchResults.Columns.Item(ACListClaimReference).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColClaimRef, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListRiskTransfer).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColRiskTransfer, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Gaurav
            lvwSearchResults.Columns.Item(ACListBGRef).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColBGRef, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblUnderwritingYearID.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColUnderwritingYear, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)) & ":"
            lvwSearchResults.Columns.Item(ACListAgentName).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColAgentName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchResults.Columns.Item(ACListCaseNumber).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListColCaseNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' {* USER DEFINED CODE (Begin) *}			
            lblAccountName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblAccountCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblContactName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACContactName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblTelephone.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTelephone, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblAccountBalance.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountBalance, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblDocumentRef.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDocumentRef, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblDocTypeGroup.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDocTypeGroup, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblDocumentType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDocumentType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblPeriod.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPeriod, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblDateFrom.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDateFrom, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblDateTo.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDateTo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblCurrencyAmount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrencyAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblTolerance.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTolerance, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblInsuranceRef.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInsuranceRef, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblOperatorID.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOperatorName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblPurchaseInvoiceNo.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPurchaseInvoiceNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblPurchaseOrderNo.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPurchaseOrderNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblDepartment.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDepartment, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblSpare.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSpare, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If m_lAllocationTransType = gACTLibrary.ACTPrimaryForAllocation Then
                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPrimaryForAllocation, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            ElseIf m_lAllocationTransType = gACTLibrary.ACTSecondaryForAllocation Then
                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSecondaryForAllocation, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' CTAF 130300 - Balance option
            chkBalance.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBalanceOption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' DD 11/04/2003 - Insured Account
            cmdInsuredAccountCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInsuredAccount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            fraAmountColumn.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmountColumn, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            optAmountTransaction.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTransactionCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            optOutstandingTransaction.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTransactionCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            optAmountAccount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            fraOutstandingColumn.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOutstandingColumn, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            optOutstandingBase.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBaseCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            optAmountBase.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBaseCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            optOutstandingAccount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblSource.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Dim sLookupDesc As String = ""
        Dim lLower, lUpper As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get all of the lookup details.
            ' See Interface SetProcessModes

            ' {* USER DEFINED CODE (Begin) *}

            cmbDocTypeGroup.Items.Clear()
            m_lReturn = GetLookupDetails(lLookupRow:=ACLDocTypeGroup, ctlLookup:=cmbDocTypeGroup, vAllTypes:=True)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cmbDocumentType.Items.Clear()
            m_lReturn = GetLookupDetails(lLookupRow:=ACLDocumentType, ctlLookup:=cmbDocumentType, vAllTypes:=True)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sLookupDesc = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAllTypes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmbCurrency.FirstItem = sLookupDesc
            cmbCurrency.ListIndex = 0

            'MKW060504 PN10318 Populated department box. START
            cboDepartment.FirstItem = sLookupDesc
            cboDepartment.ListIndex = 0
            'MKW060504 PN10318 Populated department box. END

            m_lReturn = GetPeriodLookup()

            'Populate the source combo box
            If Information.IsArray(m_vSourceArray) Then
                cboSource.Items.Clear()

                lLower = m_vSourceArray.GetLowerBound(1)

                lUpper = m_vSourceArray.GetUpperBound(1)

                ' If we have more than one branch add an all branches options
                If lLower < lUpper Then
                    Dim cboSource_NewIndex As Integer = -1
                    cboSource_NewIndex = cboSource.Items.Add("All Branches")
                    VB6.SetItemData(cboSource, cboSource_NewIndex, 0)
                End If

                ' Load Options From Source File
                For lCount As Integer = lLower To lUpper

                    Dim newIndex As Integer = cboSource.Items.Add(New VB6.ListBoxItem(CStr(m_vSourceArray(2, lCount)), CInt(m_vSourceArray(0, lCount))))
                Next lCount

                ' Set index (should always be 0, either "All Branches" or the only branch
                If cboSource.Items.Count > 0 Then
                    cboSource.SelectedIndex = 0
                End If
            End If

            m_lReturn = PopulateAccountTypes()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("PopulateAccountTypes", "Failed to populate account types list", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception

            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    '*************************************************************************
    'Name:          DisplayNoteEntry
    'Description:   Displays the Event Notes Screen (in Add mode)
    '               Gets PartyCnt from AccountID, and passes m_lInsuranceFileCnt
    '               and g_sUserName
    'History:       07/02/2003 - TR - Created
    '               18/02/2003 - AMB PS220: Modified for Manage Debtors development
    '*************************************************************************
    Private Function DisplayNoteEntry(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lAccountId As Integer) As Integer
        Dim result As Integer = 0
        Dim oNotes As iPMBNote.Interface_Renamed
        Dim lPartyCnt As Integer

        Try

            'TR - Assume success
            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lAccountId And v_lInsuranceFileCnt Then
                ' Determine the Party_cnt/account key from the Account ID

                m_lReturn = g_oBusiness.GetAccountKeyFromId(r_lAccountKey:=lPartyCnt, v_lAccountId:=v_lAccountId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyCntFromAccount failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayNoteEntry", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Must provide account_id and insurance_file_cnt", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayNoteEntry", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Create the Notes Interface object
            Dim temp_oNotes As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oNotes, sClassName:="iPMBNote.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oNotes = temp_oNotes
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get an instance of iPMBNote.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayNoteEntry", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'TR - Populate the Notes form
            With oNotes

                .PartyCnt = lPartyCnt

                .InsuranceFileCnt = v_lInsuranceFileCnt

                .AccountKey = lPartyCnt

                .NoteDate = DateTime.Now

                .UserName = g_sUsername.Value
            End With

            m_lReturn = oNotes.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            ' fire it up

            m_lReturn = oNotes.Start

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oNotes.Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayNoteEntry", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            If oNotes.Status = gPMConstants.PMEReturnCode.PMCancel Then
                result = gPMConstants.PMEReturnCode.PMCancel
            End If

            'TR - Destroy the object

            oNotes.Dispose()
            oNotes = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayNoteEntry Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayNoteEntry", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayStatusFound
    '
    ' Description: Display the status found message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusFound()

        Static sMessage As String = ""
        Dim lItemsFound As Integer

        Try

            ' Store the total of item found.
            If (Not Information.IsArray(m_vSearchData)) Or (lvwSearchResults.Items.Count < 1) Then
                lItemsFound = 0
            Else
                lItemsFound = (m_vSearchData.GetUpperBound(1) + 1)
            End If

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            stbStatus.Items.Item(0).Text = " " & lItemsFound & " " & sMessage

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: DisplayStatusSearching
    '
    ' Description: Display the status searching message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusSearching()

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            stbStatus.Items.Item(0).Text = " " & sMessage
            _stbStatus_Panel1.Text = " " & sMessage
            Application.DoEvents()
        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Function DoTransactionWriteOff() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: DoTransactionWriteOff
        ' PURPOSE: Process the write-off of the selected documents
        ' AUTHOR: Andrew Bibby
        ' DATE: 12/02/2003, 16:59
        ' RETURNS: PMTrue for success
        ' CHANGES: AMB 12/02/2003: PS220 - Created for IAG 220 Manage Debtors
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If m_bRollup Then
                m_lReturn = CheckTransactionWriteOffRollup()
            Else
                m_lReturn = CheckTransactionWriteOff()
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                If m_bRollup Then
                    m_lReturn = ProcessTransactionWriteOffsRollup()
                Else
                    m_lReturn = ProcessTransactionWriteOffs()
                End If

                If Not (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) Then
                    Return result
                End If
            Else
                Return result
            End If

            ' update the interface
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in DoTransactionWriteOff", vApp:=ACApp, vClass:=ACClass, vMethod:="DoTransactionWriteOff", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result
            End Select

        Finally


        End Try
        Return result
    End Function

    Private Function EnableApproveRejectMenu(ByRef r_bEnabled As Boolean) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: EnableApproveRejectMenu
        ' PURPOSE: Determines whether the 'Approve' and 'Reject' right-click menu option is visible
        ' AUTHOR: Andrew Bibby
        ' DATE: 24/02/2003, 17:21
        ' RETURNS: PMTrue for success
        ' CHANGES: AMB 18/02/2003: PS220 - Manage Debtors
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACMethod As String = "EnableApproveRejectMenu"
        Dim bHasUnapprovedBadDebt As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' check for a 'UBD' flag on the selected document
            For Each oSelectedItem As ListViewItem In lvwSearchResults.Items
                If (oSelectedItem IsNot Nothing AndAlso oSelectedItem.Checked) Then
                    If ListViewHelper.GetListViewSubItem(oSelectedItem, ACListFlag).Text.Trim() = AUDITSETTYPE_UNNAPPROVED_BAD_DEBT Then
                        bHasUnapprovedBadDebt = True
                        Exit For
                    End If
                End If
            Next oSelectedItem

            ' have we found a 'UBD'?
            r_bEnabled = bHasUnapprovedBadDebt

            result = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in " & ACMethod, vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result
            End Select

        Finally


        End Try
        Return result
    End Function

    Private Function EnableEditCommentMenu(ByRef r_bEnabled As Boolean) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: EnableEditCommentMenu
        ' PURPOSE: Determines whether the 'Edit Comment' right-click menu option is visible
        ' AUTHOR: Chris Barnes
        ' DATE: 25/03/2004
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACMethod As String = "EnableEditCommentMenu"

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Ensure an item is selected first!
            If (lvwSearchResults.CheckedItems.Count > 0) Then

                ' See if the selected row has a comment
                r_bEnabled = CStr(m_vSearchData(ACIComment, Convert.ToString(lvwSearchResults.CheckedItems(0).Tag))).Trim() <> ""
            Else
                r_bEnabled = False
            End If

            result = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in " & ACMethod, vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result
            End Select

        Finally


        End Try
        Return result
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (EnableDoNotReportMenu) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function EnableDoNotReportMenu(ByRef r_bEnabled As Boolean) As Integer
    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: EnableDoNotReportMenu
    ' PURPOSE: Determines whether the 'Do Not Report' or 'Report' right-click menu
    '          option is visible.
    ' AUTHOR: Chris Barnes
    ' DATE: 30/03/2004
    ' RETURNS: PMTrue for success
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    '
    'Dim result As Integer = 0
    'Const ACMethod As String = "EnableDoNotReportMenu"
    '
    '
    'On Error GoTo Catch_Renamed
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Ensure an item is selected first!
    'If Not (lvwSearchResults.FocusedItem Is Nothing) Then
    '
    ' See what the reporting flag is for this row
    ' If it is set to 1 this means do not report - but we would then want to see the
    ' Report menu so return False...and vice versa...
    'r_bEnabled = Not (CStr(m_vSearchData(ACINotReported, Convert.ToString(lvwSearchResults.FocusedItem.Tag))) = "1")
    'Else
    'r_bEnabled = False
    'End If
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'GoTo Finally_Renamed
    '
    '---------------------------------------------------------
    'Only for Debugging, the code will never execute this line
    '---------------------------------------------------------
    'Resume 
    '
    'Catch_Renamed: '
    'Select Case Information.Err().Number
    'Case Constants.vbObjectError
    ' Log internal failure.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source)
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'GoTo Finally_Renamed
    'Case Else
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in " & ACMethod, vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    'GoTo Finally_Renamed
    'End Select
    '
    'Finally_Renamed: '
    'Return result
    '
    'End Function

    '*************************************************************************************************
    'Name:      EnableBreakdownMenu
    'Purpose:  To determine if the selected list view item will have the Breakdown option present in a context menu
    'Author:    Andrew Robinson
    'History:
    '*************************************************************************************************
    'UPGRADE_NOTE: (7001) The following declaration (EnableBreakdownMenu) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function EnableBreakdownMenu(ByRef r_bEnabled As Boolean) As Integer
    '
    'Dim result As Integer = 0
    'Const ACMethod As String = "EnableBreakdownMenu"
    'Try 
    '
    '
    'Dim lDocumentTypeID As Integer
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    'r_bEnabled = False
    '
    'If Not (Me.lvwSearchResults.FocusedItem Is Nothing) Then
    'lDocumentTypeID = gPMFunctions.ToSafeLong(m_vSearchData(ACIDocumentTypeId, Convert.ToString(Me.lvwSearchResults.FocusedItem.Tag)))
    'Select Case lDocumentTypeID
    'Case 4, 5, 15, 16, 17, 18, 31, 32, 35, 36
    'r_bEnabled = True
    'End Select
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in " & ACMethod, vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return gPMConstants.PMEReturnCode.PMError
    '
    'End Try
    'End Function

    Private Function EnableRefundMenu(ByRef r_bEnabled As Boolean) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: EnableRefundMenu
        ' PURPOSE: Determines whether the 'Refund' right-click menu option is visible
        ' AUTHOR: Andrew Bibby
        ' DATE: 18/02/2003, 17:21
        ' RETURNS: PMTrue for success
        ' CHANGES: AMB 18/02/2003: PS220 - Manage Debtors
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACMethod As String = "EnableRefundMenu"

        Dim dOSAmount As Double

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' check the user's authority
            r_bEnabled = m_lHasRefundAuthority

            If r_bEnabled Then
                ' total up the O/S amounts for the selected documents
                For Each oSelectedItem As ListViewItem In lvwSearchResults.Items
                    If (oSelectedItem IsNot Nothing AndAlso oSelectedItem.Checked) Then
                        ' work out the O/S amount
                        dOSAmount += (gPMFunctions.NullToDouble(m_vSearchData(ACICurrencyAmount, Convert.ToString(oSelectedItem.Tag))) - gPMFunctions.NullToDouble(m_vSearchData(ACIMatchedCurrencyAmount, Convert.ToString(oSelectedItem.Tag))))
                    End If
                Next oSelectedItem
                ' is the total a credit? (this means less than zero)
                r_bEnabled = dOSAmount < 0
            End If

            If r_bEnabled Then
                ' check each item for pending refund 'PR' in the flag column
                For Each oSelectedItem As ListViewItem In lvwSearchResults.Items
                    If (oSelectedItem IsNot Nothing AndAlso oSelectedItem.Checked) Then
                        ' KG 26/06/03
                        ' As per spec (DannyD) - Refund is NOT available if flag is set to Pending
                        '                        Refund IS available if flag is empty.
                        If ListViewHelper.GetListViewSubItem(oSelectedItem, ACListFlag).Text.Trim() = "" Then
                            r_bEnabled = True
                            ' let's go
                            Exit For
                        Else
                            ' Trim$(oSelectedItem.SubItems(ACListFlag) _
                            ''        ) = AUDITSETTYPE_PENDING_REFUND
                            r_bEnabled = False
                        End If
                    End If
                Next oSelectedItem
            End If

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in " & ACMethod, vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result
            End Select

        Finally


        End Try
        Return result
    End Function

    Private Function EnableWriteOffMenu(ByRef r_bEnabled As Boolean) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: EnableWriteOffMenu
        ' PURPOSE: Determines whether the 'Write-off' right-click menu option is visible
        ' AUTHOR: Andrew Bibby
        ' DATE: 18/02/2003, 17:21
        ' RETURNS: PMTrue for success
        ' CHANGES: AMB 18/02/2003: PS220 - Manage Debtors
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACMethod As String = "EnableWriteOffMenu"

        Dim dOSTotalAmount, dOSAmount As Double

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' check the ledger_id
            Select Case m_sLedgerName
                Case ACCOUNT_LEDGER_CLIENT, ACCOUNT_LEDGER_AGENT, ACCOUNT_LEDGER_OTHERPARTY
                    r_bEnabled = True
                Case Else
                    r_bEnabled = False
            End Select

            If r_bEnabled Then
                ' check the user's authority
                r_bEnabled = m_lHasTransWriteOffAuthority
            End If

            If r_bEnabled Then
                ' check that the flag for any selected items isn't 'UBD'
                For Each oSelectedItem As ListViewItem In lvwSearchResults.Items
                    If (oSelectedItem IsNot Nothing AndAlso oSelectedItem.Checked) Then
                        If ListViewHelper.GetListViewSubItem(oSelectedItem, ACListFlag).Text <> AUDITSETTYPE_UNNAPPROVED_BAD_DEBT Then
                            r_bEnabled = True
                        Else
                            r_bEnabled = False
                            ' jump out, it's all gone pear
                            Exit For
                        End If
                    End If
                Next oSelectedItem
            End If

            If r_bEnabled Then
                ' check that the total O/S amount is greater than zero
                For Each oSelectedItem As ListViewItem In lvwSearchResults.Items
                    If (oSelectedItem IsNot Nothing AndAlso oSelectedItem.Checked) Then
                        ' work out the O/S amount
                        dOSAmount = gPMFunctions.NullToDouble(m_vSearchData(ACICurrencyAmount, Convert.ToString(oSelectedItem.Tag))) - gPMFunctions.NullToDouble(m_vSearchData(ACIMatchedCurrencyAmount, Convert.ToString(oSelectedItem.Tag)))

                        ' total it up
                        dOSTotalAmount += dOSAmount
                    End If
                Next oSelectedItem

                ' only show write-off menu when total O/S amount > 0
                r_bEnabled = dOSTotalAmount > 0

            End If

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in " & ACMethod, vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result
            End Select

        Finally


        End Try
        Return result
    End Function

    Private Function EnableWriteOffMenuRollup(ByRef r_bEnabled As Boolean) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: EnableWriteOffMenuRollup
        ' PURPOSE: Determines whether the 'Write-off' right-click menu option is visible for rollups
        ' AUTHOR: Paul Cunningham
        ' DATE: 10/06/2003, 17:21
        ' RETURNS: PMTrue for success
        ' CHANGES: PWC - blagged from EnableWriteOffMenu
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACMethod As String = "EnableWriteOffMenuRollup"

        Dim crOSTotalAmount, crOSAmount As Decimal

        Dim vAllTrans(,) As Object

        Dim lAllTransLowerRow, lAllTransUpperRow As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' check the ledger_id
            Select Case m_sLedgerName
                Case ACCOUNT_LEDGER_CLIENT, ACCOUNT_LEDGER_AGENT, ACCOUNT_LEDGER_OTHERPARTY
                    r_bEnabled = True
                Case Else
                    r_bEnabled = False
            End Select

            If r_bEnabled Then
                ' check the user's authority
                r_bEnabled = m_lHasTransWriteOffAuthority
            End If

            If r_bEnabled Then
                'Get the transaction records for each rolled up document row selected in the listview
                If GetAllTransForSelectedDocuments(r_vAllTrans:=vAllTrans) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", GetAllTransForSelectedDocuments failed")
                End If

                'Must have some transactions to process
                If Not Information.IsArray(vAllTrans) Then
                    Return result
                End If

                'Size the selected trans detail array to accept all the transactions found

                lAllTransLowerRow = vAllTrans.GetLowerBound(klRowDimension - 1)

                lAllTransUpperRow = vAllTrans.GetUpperBound(klRowDimension - 1)

                ' check that the flag for any selected items isn't 'UBD'
                For lRow As Integer = lAllTransLowerRow To lAllTransUpperRow

                    If CStr(vAllTrans(27, lRow)) <> AUDITSETTYPE_UNNAPPROVED_BAD_DEBT Then
                        r_bEnabled = True
                    Else
                        r_bEnabled = False
                        ' jump out, it's all gone a bit pete tong
                        Exit For
                    End If
                Next lRow
            End If

            If r_bEnabled Then
                ' check that the total O/S amount is greater than zero
                For lRow As Integer = lAllTransLowerRow To lAllTransUpperRow
                    ' work out the O/S amount

                    crOSAmount = gPMFunctions.NullToCurrency(CStr(vAllTrans(ACICurrencyAmount, lRow))) - gPMFunctions.NullToCurrency(CStr(vAllTrans(ACIMatchedCurrencyAmount, lRow)))

                    crOSTotalAmount += crOSAmount
                Next lRow

                ' only show write-off menu when total O/S amount > 0
                r_bEnabled = crOSTotalAmount > 0

            End If

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in " & ACMethod, vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result
            End Select

        Finally


        End Try
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: FindAccountTransactions
    '
    ' Description: Start another instance of Find Transaction passing
    ' the selected transaction's account ID
    '
    ' ***************************************************************** '
    Private Function FindAccountTransactions() As Integer

        Dim result As Integer = 0
        Dim oFindTransaction As Interface_Renamed
        Dim lSelectedItem, lAccountID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oFindTransaction = New Interface_Renamed()

            'BB We don't actually need to get the business refs again since they are global
            '   but we do need some of the other processing in initialise
            m_lReturn = CType(oFindTransaction, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            m_lReturn = oFindTransaction.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            ' Get the Account ID of the selected item
            If (lvwSearchResults.CheckedItems.Count = 0) Then
                Return result
            End If
            lSelectedItem = Convert.ToString(lvwSearchResults.Items.Item(lvwSearchResults.CheckedItems(0).Index).Tag)
            lAccountID = CInt(m_vSearchData(ACIAccountId, lSelectedItem))

            ' Pass the Account ID to the new find instance
            oFindTransaction.AccountID = lAccountID
            oFindTransaction.DrillLevel = m_lDrillLevel
            oFindTransaction.CalledViaClientManager = m_bCalledViaClientManager
            oFindTransaction.CallingAppName = "iACTFindTransaction"
            m_lReturn = oFindTransaction.Start()

            m_lDrillLevel -= 1
            'BB We can't use the terminate method, since the business object refs are global
            '   and terminating them in the child form will also remove them from the parent
            '    m_lReturn = oFindTransaction.Terminate()

            oFindTransaction = Nothing

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to find account transactions", vApp:=ACApp, vClass:=ACClass, vMethod:="FindAccountTransactions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindDocumentTransactions
    '
    ' Description: Start another instance of Find Transaction passing
    ' the selected transaction's document ID
    '
    ' ***************************************************************** '
    Private Function FindDocumentTransactions() As Integer
        Dim result As Integer = 0
        Dim oFindTransaction As Interface_Renamed
        Dim lSelectedItem As Integer
        Dim sDocumentRef As String = ""
        Dim iCompanyID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oFindTransaction = New Interface_Renamed()

            'BB We don't actually need to get the business refs again since they are global
            '   but we do need some of the other processing in initialise
            m_lReturn = CType(oFindTransaction, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            m_lReturn = oFindTransaction.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            ' Get the Doc Ref of the selected item
            'eck 090500
            If (lvwSearchResults.CheckedItems.Count = 0) Then
                Return result
            End If
            lSelectedItem = Convert.ToString(lvwSearchResults.Items.Item(lvwSearchResults.CheckedItems(0).Index).Tag)
            sDocumentRef = CStr(m_vSearchData(ACIDocumentRef, lSelectedItem)).Trim()
            iCompanyID = CInt(m_vSearchData(ACISourceID, lSelectedItem))

            ' Pass the Doc Ref to the new find instance
            oFindTransaction.DocumentRef = sDocumentRef
            oFindTransaction.DrillCompany = iCompanyID
            oFindTransaction.DocumentId = CInt(m_vSearchData(ACIDocDocumentID, lSelectedItem))

            oFindTransaction.DrillLevel = m_lDrillLevel
            oFindTransaction.CalledViaClientManager = m_bCalledViaClientManager

            'DD 08/08/2003: Added to stop roll-up switching on when drilling a document
            oFindTransaction.Rollup = False

            m_lReturn = oFindTransaction.Start()

            ' CJB 01/04/2004 Since we can now make changes in the Find Transaction component
            ' (flag rows as to report or not report/add, edit, delete comments for rows) and
            ' for these changes to be visible in tooltips, and menu actions then we need to
            ' test a DataChanged flag to determine if we need to refresh the listview or not...
            If oFindTransaction.DataChanged Then
                FindNow()
            End If

            m_lDrillLevel -= 1

            'BB We can't use the terminate method, since the business object refs are global
            '   and terminating them in the child form will also remove them from the parent
            '    m_lReturn = oFindTransaction.Terminate()

            oFindTransaction = Nothing

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to find document transactions", vApp:=ACApp, vClass:=ACClass, vMethod:="FindDocumentTransactions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindNow
    '
    ' Description: Get the interface details from the query
    '
    ' ***************************************************************** '
    Public Sub FindNow()


        Dim sWildcardErrorMessage As String = ""
        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Check wildcard searches
            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtDocumentRef.Text, r_sErrorMessage:=sWildcardErrorMessage) Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Transaction")
                txtDocumentRef.Focus()
                Exit Sub
            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtInsuranceRef.Text, r_sErrorMessage:=sWildcardErrorMessage) Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Transaction")
                txtInsuranceRef.Focus()
                Exit Sub
            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtPurchaseInvoiceNo.Text, r_sErrorMessage:=sWildcardErrorMessage) Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Transaction")
                txtPurchaseInvoiceNo.Focus()
                Exit Sub
            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtPurchaseOrderNo.Text, r_sErrorMessage:=sWildcardErrorMessage) Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Transaction")
                txtPurchaseOrderNo.Focus()
                Exit Sub
            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtSpare.Text, r_sErrorMessage:=sWildcardErrorMessage) Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Transaction")
                txtSpare.Focus()
                Exit Sub
            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtAltRef.Text, r_sErrorMessage:=sWildcardErrorMessage) Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Transaction")
                txtAltRef.Focus()
                Exit Sub
            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtCaseNumber.Text, r_sErrorMessage:=sWildcardErrorMessage) Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Transaction")
                txtCaseNumber.Focus()
                Exit Sub
            End If

            'only validate the due dates are good if both are supplied
            If txtDueDateFrom.Text <> "" And txtDueDateTo.Text <> "" Then

                'validate due date to is not less than due date from
                If CDate(txtDueDateTo.Text) < CDate(txtDueDateFrom.Text) Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    MessageBox.Show("Due Date To cannot be earlier than Due Date From", "Find Transaction")
                    txtDueDateTo.Focus()
                    Exit Sub
                End If
            End If
            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
            End If

            If (lvwSearchResults.Items.Count > 1) Then
                If lvwSearchResults.Visible Then
                    m_oTTNew.RemoveAll()
                    ' Set the focus.
                    lvwSearchResults.Focus()

                    lvwSearchResults_ItemClick(lvwSearchResults.Items(0))
                End If
            End If

            ' Hide the listview tooltip as no item selected, reset the last item on index too
            If Not IsNothing(m_oTTNew) Then
                m_oTTNew.Hide(lvwSearchResults)
            End If
            m_lCurItemIndex = 0

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the FindNow command", vApp:=ACApp, vClass:=ACClass, vMethod:="FindNow", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            'BB This code needs to be here after the form is visible
            ' if it isn't the selected tab can lose it's controls
            Dim iStartTab As Integer

            ' If we have a Doc Ref show that tab to start with
            If txtDocumentRef.Text <> "" Then
                iStartTab = 1
                ' Otherwise show the account tab
            ElseIf m_bInsuredAccountView Then
                iStartTab = 3
            Else
                iStartTab = 0
                If txtAccountCode.Enabled Then
                    txtAccountCode.Focus()
                End If
            End If

            ' Show the start tab
            SSTabHelper.SetSelectedIndex(tabMain, iStartTab)
        End If
    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim vValue As Object
        Dim sValue As String = ""

        ' Forms initialise event.

        Try

            'DD 20/01/2003: Put form in taskbar
            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iACTFindTransaction.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=g_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'Thinh Nguyen 15/04/2002

            m_sUnderwritingOrAgency = g_oBusiness.UnderwritingOrAgency

            'DD 15/07/2002: Get product option setting
            iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnhancedOrionSecurity, g_iCompanyID, vValue)

            m_bEnhancedSecurity = (CStr(vValue) = "1")

            'RDT - 10/04/2003 - Check for the option to show Debits/Credits in the results list

            vValue = ""
            iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTDisplayDebitCredit, gPMConstants.SIRBCHHeadOffice, vValue)

            m_bDisplayDebitCredit = (CStr(vValue) = "1")

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            'SJ 24/02/2004 - start
            m_lReturn = CheckForUnderwritingBranch(v_iSourceId:=g_oObjectManager.SourceID, r_bUnderwritingBranchEnabled:=m_bUnderwritingBranchEnabled, r_bIsUnderwritingBranch:=m_bIsUnderwritingBranch)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckForUnderwritingBranch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise")
                Exit Sub
            End If
            'SJ 24/02/2004 - end

            'DD 05/04/2004: Get product option setting
            iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTUnderwritingYear, g_iCompanyID, vValue)

            m_bUnderwritingYear = (CStr(vValue) = "1")

            iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, g_iCompanyID, vValue)

            m_bMultiLedger = (CStr(vValue) = "1")

            m_bShowPopupComments = True

            m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROTPRestrictAllocationReversal, 1, sValue)

            If sValue = "1" Then
                m_iRestrictAllocationReversal = 1
            End If

            'Get User Authorities Object.
            Dim temp_m_oUserAuthorities As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oUserAuthorities, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oUserAuthorities = temp_m_oUserAuthorities
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'Get Currency Object.
            Dim temp_m_oCurrency As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrency, "bACTCurrency.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oCurrency = temp_m_oCurrency
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'Get Account Object.
            Dim temp_m_oAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oAccount, "bACTAccount.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oAccount = temp_m_oAccount
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'Get Currency Convert Object.
            Dim temp_m_oCurrencyConvert As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oCurrencyConvert = temp_m_oCurrencyConvert
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown, iShiftDown, iNewTab As Integer
        Dim bProcessed As Boolean = False
        Dim bTabChanged As Boolean = False

        Const ACShiftMask As Integer = 1
        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iShiftDown = (Shift And ACShiftMask) > 0
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabMain
                iNewTab = SSTabHelper.GetSelectedIndex(tabMain)
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.
                        ' Check if the control key has also been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            iNewTab = 0
                            ' New tab must be visible
                            Do Until SSTabHelper.GetTabVisible(tabMain, iNewTab)
                                iNewTab += 1
                            Loop
                        Else
                            Do
                                ' If we are on the first tab.
                                If iNewTab = 0 Then
                                    ' Display the last tab.
                                    iNewTab = SSTabHelper.GetTabCount(tabMain) - 1
                                Else
                                    ' Display the previous tab.
                                    iNewTab -= 1
                                End If
                                ' New tab must be visible
                            Loop Until SSTabHelper.GetTabVisible(tabMain, iNewTab)
                        End If
                        bProcessed = True
                        bTabChanged = True

                    Case Keys.PageDown
                        ' Page Down key has been pressed.
                        ' Check if the control key has also been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            iNewTab = SSTabHelper.GetTabCount(tabMain) - 1
                            ' New tab must be visible
                            Do Until SSTabHelper.GetTabVisible(tabMain, iNewTab)
                                iNewTab -= 1
                            Loop
                        Else
                            Do
                                ' If we are on the last tab.
                                If iNewTab = (SSTabHelper.GetTabCount(tabMain) - 1) Then
                                    ' Display the first tab.
                                    iNewTab = 0
                                Else
                                    ' Display the next tab.
                                    iNewTab += 1
                                End If
                                ' New tab must be visible
                            Loop Until SSTabHelper.GetTabVisible(tabMain, iNewTab)
                        End If
                        bProcessed = True
                        bTabChanged = True

                    Case Keys.Home
                        ' Home key has been pressed.
                        ' Check if the control key has also been pressed.
                        If iCtrlDown Then
                            ' Set focus to the first control on the tab.
                            If iNewTab <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, iNewTab).Focus()
                            End If
                            bProcessed = True
                        End If

                    Case Keys.End
                        ' End key has been pressed.
                        ' Check if the control key has also been pressed.
                        If iCtrlDown Then
                            ' Set focus to the last control on the tab.
                            If iNewTab <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, iNewTab).Focus()
                            End If
                            bProcessed = True
                        End If
                End Select
                ' Change tabs?
                If bTabChanged Then
                    SSTabHelper.SetSelectedIndex(tabMain, iNewTab)
                End If
            End With

            ' If the key was processed
            If bProcessed Then
                KeyCode = 0
            End If

        Catch

            ' Error Section.

            Exit Sub
        End Try

    End Sub

    Private Sub frmInterface_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown, iShiftDown, iNewTab As Integer
        Dim bProcessed As Boolean = False
        Dim bTabChanged As Boolean = False

        Const ACShiftMask As Integer = 1
        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iShiftDown = (Shift And ACShiftMask) > 0
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabMain
                iNewTab = SSTabHelper.GetSelectedIndex(tabMain)
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.Tab
                        ' Tab key has been pressed.
                        ' Check if the control key has also been pressed.
                        If iCtrlDown Then
                            Do
                                ' Check if the shift key has also been pressed.
                                If iShiftDown Then
                                    ' If we are on the first tab.
                                    If iNewTab = 0 Then
                                        ' Display the last tab.
                                        iNewTab = SSTabHelper.GetTabCount(tabMain) - 1
                                    Else
                                        ' Display the previous tab.
                                        iNewTab -= 1
                                    End If
                                Else
                                    ' Check we are not on the last tab.
                                    If iNewTab = (SSTabHelper.GetTabCount(tabMain) - 1) Then
                                        ' Display the first tab.
                                        iNewTab = 0
                                    Else
                                        ' Display the next tab.
                                        iNewTab += 1
                                    End If
                                End If
                                ' New tab must be visible
                            Loop Until SSTabHelper.GetTabVisible(tabMain, iNewTab)
                            bProcessed = True
                            bTabChanged = True
                        End If
                End Select
                ' Change tabs?
                If bTabChanged Then
                    SSTabHelper.SetSelectedIndex(tabMain, iNewTab)
                End If
            End With

            ' If the key was processed
            If bProcessed Then
                KeyCode = 0
            End If

            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMain.SelectedIndex = 0
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                tabMain.SelectedIndex = 1
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D3 Then
                tabMain.SelectedIndex = 2
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D4 Then
                tabMain.SelectedIndex = 3
            End If
        Catch

            ' Error Section.

            Exit Sub
        End Try

    End Sub

    Public Sub frmInterfaceLoad()

        Dim sOptionValue As String = ""
        ' Forms load event.

        Try

            'DD 20/01/2003: Put form in taskbar
            iPMFunc.ShowFormInTaskBar_Detach()

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' CJB 26/03/2004 - Initialise Tooltip class that'll be used to display comments
            ' in the listview when moving the mouse over docs that have them assigned
            'm_oTT = New CTooltip()
            'm_oTT.Style = CTooltip.ttStyleEnum.TTBalloon
            'm_oTT.Icon = CTooltip.ttIconType.TTIconInfo
            m_oTTNew = New ToolTip()
            m_oTTNew.IsBalloon = True
            m_oTTNew.ToolTipIcon = ToolTipIcon.Info
            m_oTTNew.OwnerDraw = True
            m_oTTNew.ShowAlways = False

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Check if there is sufficient search criteria

            ' {* USER DEFINED CODE (Begin) *}

            'Set default view in Transaction Currency if Milti currency banking option ticked.

            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=kMultiCurrencyBanking, r_sOptionValue:=m_sMultiCurrencyBankingOption, v_iSourceID:=g_iSourceID)

            If gPMFunctions.ToSafeInteger(m_sMultiCurrencyBankingOption) = 1 Then
                Me.optOutstandingTransaction.Checked = True
            End If

            If m_iSelectedCurrencyId > 0 Then
                cmbCurrency.ItemId = m_iSelectedCurrencyId
            End If

            'If m_iSelectedSourceId > 0 Then
            '    For iCnt As Integer = 0 To cboSource.Items.Count - 1
            '        If VB6.GetItemData(cboSource, iCnt) = m_iSelectedSourceId Then
            '            cboSource.SelectedIndex = iCnt
            '            Exit For
            '        End If
            '    Next
            '    cboSource_SelectedIndexChanged(cboSource, New EventArgs())
            'End If

            m_bBalanceOnly = False

            'If search details already supplied do search
            'Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc)
            If m_lAccountID <> 0 Or m_sDocumentRef <> "" Or m_lInsuredAccountID <> 0 Then
                ' Run the search now
                FindNow()
            End If
            'End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc)

            'eck200401
            If DrillLevel > 0 Then
                cmdAllocate.Enabled = False
                cmdReverseAndReplace.Enabled = False
                'DD 07/07/2003: OK is not used for drill down
                cmdOK.Enabled = False
            End If

            cmdExpand.Visible = False
            panInvoiceBalance.Visible = False
            panDefaultedInstalments.Visible = False
            lblInvoiceBalance.Visible = False
            lblDefaultedInstalments.Visible = False

            'DC130808

            chkIncReversedTxs.CheckState = CheckState.Checked

            'Get user authorities.

            m_lReturn = m_oUserAuthorities.GetDetails(vUserId:=g_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            m_lReturn = m_oUserAuthorities.GetNext(vHasTransWriteOffAuthority:=m_lHasTransWriteOffAuthority, vHasRefundAuthority:=m_lHasRefundAuthority, vHasTransferAuthority:=m_lHasTransferAuthority, vHasUnrestrictedEnquiry:=m_lHasUnrestrictedEnquiry)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Payment Maintenance

            m_lReturn = m_oFindPaymentMaintenance.GetUserReverseAllocation(v_UserID:=g_iUserID, vResultArray:=m_vAllocationData)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Get User Reversal Authorities
            If Not Information.IsArray(m_vAllocationData) Then
                m_iAllowReverseAllocation = 0
                m_iReverseAllocationDays = 0
                m_iHasPaymentAuthority = 0
                m_lPaymentCurrencyId = 0
                m_crPaymentAmount = 0
                m_iHasClaimAuthority = 0
                m_lClaimCurrencyId = 0
                m_crClaimAmount = 0
            Else
                m_iAllowReverseAllocation = gPMFunctions.ToSafeInteger(m_vAllocationData(ACAllowReverse, 0))
                m_iReverseAllocationDays = gPMFunctions.ToSafeInteger(m_vAllocationData(ACReverseDays, 0))
                m_iHasPaymentAuthority = gPMFunctions.ToSafeInteger(m_vAllocationData(ACHasPaymentAuthority, 0))
                m_lPaymentCurrencyId = gPMFunctions.ToSafeLong(m_vAllocationData(ACPaymentCurrencyId, 0))
                m_crPaymentAmount = gPMFunctions.ToSafeCurrency(m_vAllocationData(ACPaymentAmount, 0))
                m_iHasClaimAuthority = gPMFunctions.ToSafeInteger(m_vAllocationData(ACHasClaimAuthority, 0))
                m_lClaimCurrencyId = gPMFunctions.ToSafeLong(m_vAllocationData(ACClaimCurrencyId, 0))
                m_crClaimAmount = gPMFunctions.ToSafeCurrency(m_vAllocationData(ACClaimAmount, 0))
            End If

            m_bFirstTime = True

            Dim iStartTab As Integer

            ' If we have a Doc Ref show that tab to start with
            If txtDocumentRef.Text <> "" Then
                iStartTab = 1
                ' Otherwise show the account tab
                'Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.3.2.5)
            ElseIf m_bInsuredAccountView Then
                iStartTab = 3
                'End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.3.2.5)
            Else
                iStartTab = 0
                If txtAccountCode.Enabled Then
                    txtAccountCode.Focus()
                End If
            End If

            If Not Information.IsNothing(UserPartyArray) AndAlso UserPartyArray(0, 0) <> "" Then
                panAgentCode.Text = UserPartyArray(7, 0)
                panAgentName.Text = UserPartyArray(0, 0)
                lblAccountCode.Text = "Client"
                lblAccountName.Text = "Client Name"
                cmdAccountCode.Text = "Client Code..."

            Else
                lblAgentCode.Visible = False
                lblAgentName.Visible = False
                panAgentCode.Visible = False
                panAgentName.Visible = False
            End If

            ' Show the start tab
            SSTabHelper.SetSelectedIndex(tabMain, iStartTab)

        Catch excep As System.Exception

            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Dim sBalanceChk As String = ""

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                'SMJB 20/10/03: If we're not in a roadmap then closing the form should not
                'try and return any search details
                'MKR 01/11/2004 PN 14833 added check for IsArray(m_vTransdetailIDs)
                If IsInRoadMap() Or Not Information.IsArray(m_vTransdetailIDs) Then
                    ' Process the next set of actions depending
                    ' upon the interface task etc.
                    m_lReturn = m_oGeneral.ProcessCommand()

                    ' Check the return value.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Do not procced with the interface termination.

                        eventArgs.Cancel = True
                        Cancel = 1

                        ' Set the mouse pointer to normal.
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                        Exit Sub
                    End If
                End If
            End If

            If Not (m_oFindAccount Is Nothing) Then

                m_oFindAccount.Dispose()
                m_oFindAccount = Nothing
            End If

            ' CTAF 130300 - Store the registry settings
            If chkBalance.CheckState = CheckState.Checked Then
                sBalanceChk = "1"
            Else
                sBalanceChk = "0"
            End If

            m_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=ACTFindTransBalanceOption, v_sSettingValue:=sBalanceChk)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Doesnt really matter if it doesnt save the setting. It's not critical.
            End If

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
        Catch excep As System.Exception

            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If
        m_lReturn = ResizeInterface()
    End Sub

    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.FormClosed

        ' Forms unload event.

        Try

            ' Terminate the general object.
            m_oGeneral.Dispose()



            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' CJB 29/03/2004 Destroy the tooltips class instance from memory
            'm_oTT = Nothing
            m_oTTNew = Nothing

            'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.1.2)
            m_lReturn = SaveInterfaceDisplaySettings()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            End If
            'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.1.2)

            'Inform the Interface
            m_lReturn = m_oInterface.OnForm_Unload()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            End If

            'Destroy the interface
            m_oInterface = Nothing

            If Not (g_oAccount Is Nothing) Then

                g_oAccount.Dispose()
                g_oAccount = Nothing
            End If

            If Not (m_oPMLock Is Nothing) Then

                m_oPMLock.Dispose()
                m_oPMLock = Nothing
            End If

            If Not (m_oCurrency Is Nothing) Then

                m_oCurrency.Dispose()
                m_oCurrency = Nothing
            End If

            If Not (m_oUserAuthorities Is Nothing) Then

                m_oUserAuthorities.Dispose()
                m_oUserAuthorities = Nothing
            End If

            If Not (m_oAllocation Is Nothing) Then

                m_oAllocation.Dispose()
                m_oAllocation = Nothing
            End If

            If Not (m_oFindAccount Is Nothing) Then

                m_oFindAccount.Dispose()
                m_oFindAccount = Nothing
            End If

            If Not (m_oAccount Is Nothing) Then

                m_oAccount.Dispose()
                m_oAccount = Nothing
            End If

            If Not (m_oCurrencyConvert Is Nothing) Then

                m_oCurrencyConvert.Dispose()
                m_oCurrencyConvert = Nothing
            End If

            m_cOSTransactions = Nothing

        Catch excep As System.Exception

            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Unload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            MemoryHelper.ReleaseMemory()
        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: GetAccountID
    ' Description:
    ' History: 05/01/2000 CTAF - Created.
    ' ***************************************************************** '
    Public Function GetAccountID(ByRef r_lAccountID As Integer, Optional ByRef bInsuredAccount As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim sShortCode As String = ""
        Dim sLedgerCode As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the short code filter
            If bInsuredAccount Then
                sShortCode = txtInsuredAccountCode.Text
            ElseIf String.IsNullOrEmpty(txtAccountCode.Text) AndAlso Not String.IsNullOrEmpty(panAgentCode.Text) Then
                sShortCode = panAgentCode.Text
            Else
                If Information.IsArray(UserPartyArray) Then
                    If gPMFunctions.ToSafeString(UserPartyArray(kUserAgentCnt, 0)) <> "" Then
                        sLedgerCode = ACCOUNT_LEDGER_CLIENT
                    End If
                End If
                sShortCode = txtAccountCode.Text
            End If

            ' Call business and get the account id

            m_lReturn = g_oBusiness.GetAccountID(v_sShortCode:=sShortCode, r_lAccountID:=r_lAccountID, sLedgerCode:=sLedgerCode)

            If r_lAccountID <> 0 Then
                If bInsuredAccount Then
                    txtInsuredAccountCode.Tag = CStr(r_lAccountID)
                Else
                    txtAccountCode.Tag = CStr(r_lAccountID)
                End If
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetAllocationDetails() As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create the object if needed
        '    If ((g_oAllocationCalculate Is Nothing) = True) Then
        '
        '        m_lReturn& = g_oObjectManager.GetInstance( _
        ''            oObject:=g_oAllocationCalculate, _
        ''            sClassName:="bACTAllocationCalculate.Form", _
        ''            vInstanceManager:=PMGetViaClientManager)
        '        If (m_lReturn& <> PMTrue) Then
        '            GetAllocationDetails = PMFalse
        '            Exit Function
        '        End If
        '
        '    End If
        '
        '    m_lReturn& = g_oAllocationCalculate.GetAllocationDetails(v_lAllocationId:=m_lAllocationID)

        Return g_oBusiness.GetAllocationDetails(v_lAllocationId:=m_lAllocationID)

        ' Error Section.
        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Allocation Details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllocationDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function
    ' PRIVATE Methods (End)
    ''' <summary>
    ''' Get All Transactions For Selected Documents
    ''' </summary>
    ''' <param name="r_vAllTrans"></param>
    ''' <param name="v_bOrderBySpare"></param>
    ''' <param name="r_bInstalments"></param>
    ''' <param name="r_bIsThirdPartyScheme"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetAllTransForSelectedDocuments(ByRef r_vAllTrans As Object, Optional ByVal v_bOrderBySpare As Boolean = False, Optional ByRef r_bInstalments As Boolean = False, Optional ByRef r_bIsThirdPartyScheme As Boolean = False) As Integer

        Dim result As Integer = 0
        Const ACMethod As String = "GetAllTransForSelectedDocuments"

        Dim vTransForDoc As Object
        Dim sDocumentRef As String = ""
        Dim lAccountID As Integer
        Dim iCompanyID As Integer
        Dim sAltRef As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Get the transaction records for each rolled up document row selected in the listview
            For Each oListItem As ListViewItem In lvwSearchResults.Items
                If oListItem.Checked Then
                    sAltRef = String.Empty
                    ' KG 02/09/03
                    ' CQ2025 - need to check if any instalments included.Only check if false
                    If Not r_bInstalments Then
                        r_bInstalments = ChkDocTypeIsInstalments(ListViewHelper.GetListViewSubItem(oListItem, ACListDocumentRef).Text.Substring(0, 3))
                    End If
                    'Defect TFS 6444
                    If Not r_bIsThirdPartyScheme Then
                        r_bIsThirdPartyScheme = CheckIsLinkedToThirdPartyScheme(ListViewHelper.GetListViewSubItem(oListItem, ACListDocumentRef).Text)
                    End If
                    'Get the individual transactions for the current item (the parent document)
                    'before processing
                    sDocumentRef = CStr(m_vSearchData(ACIDocumentRef, Convert.ToString(oListItem.Tag)))
                    lAccountID = CInt(m_vSearchData(ACIAccountId, Convert.ToString(oListItem.Tag)))
                    iCompanyID = CInt(m_vSearchData(ACISourceID, Convert.ToString(oListItem.Tag)))
                    If gPMFunctions.ToSafeString(sDocumentRef).Substring(0, 2) <> "CL" Then
                        sAltRef = CStr(m_vSearchData(ACITransReference, Convert.ToString(oListItem.Tag)))
                    Else
                        sAltRef = ""
                    End If
                    If GetTransactionsForDocument(v_sDocumentRef:=sDocumentRef, v_lAccountId:=lAccountID, v_iCompanyID:=iCompanyID, r_vResultArray:=vTransForDoc, v_bOrderBySpare:=v_bOrderBySpare, v_sAltReference:=sAltRef) <> gPMConstants.PMEReturnCode.PMTrue Then

                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Unable to get transactions for document: " & sDocumentRef)
                    End If

                    'Add the results of the above call to a master trans array if any returned
                    If Information.IsArray(vTransForDoc) Then

                        If AddSourceArrayToDestinationArray(r_vSourceArray:=vTransForDoc, r_vDestinationArray:=r_vAllTrans) <> gPMConstants.PMEReturnCode.PMTrue Then

                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Unable to append transaction details for document: " & sDocumentRef)
                        End If

                    End If
                End If
            Next oListItem

            result = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result

                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result

            End Select

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness(Optional ByVal v_bSilent As Boolean = False, Optional ByVal v_bOverrideRollup As Boolean = False) As Integer

        'Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.3.2.6)
        Dim result As Integer = 0
        Const kMethodName As String = "GetBusiness"
        'End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.3.2.6)

        Dim iCompanyID As Integer
        Dim lAccountID, lInsuredAccountID As Integer 'DD 11/04/2003
        Dim sDocumentRef As String = ""
        Dim iCurrencyID As Integer
        Dim cCurrencyAmount As Decimal
        Dim iTolerance, iDocTypeGroupId, iDocumentTypeID As Integer
        Dim lPeriodId As Integer
        Dim dtDateFrom, dtDateTo As Date
        Dim dtDueDateFrom, dtDueDateTo As Date
        Dim sInsuranceRef, sOperatorName, sPurchaseInvoiceNo, sPurchaseOrderNo, sDepartment, sSpare, sShortCode, sAltRef As String '(RC) QBENZ014

        Dim iOutstandingOnly As CheckState
        Dim vSearchData As Object
        Dim bRollup, bDisplay500, bIncReversed As Boolean
        Dim iAccountType As Integer
        Dim sFormattedCurrency As String = ""
        Dim sBGRef As String = ""
        Dim sCaseNumber As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not v_bSilent Then
                DisplayStatusSearching()

                ' Disable parts of the interface while
                ' a search is in progress.
                DisableInterface(bDisable:=True)
                m_vSearchData = Nothing
            Else
                m_vCheckSearchData = Nothing
            End If

            If v_bOverrideRollup Then
                'Used for when extracting the transactions for Allocation
                bRollup = False
            Else
                bRollup = m_bRollup
            End If

            If cmbAccountType.SelectedIndex > 0 Then
                iAccountType = VB6.GetItemData(cmbAccountType, cmbAccountType.SelectedIndex)
            End If

            iCompanyID = g_iCompanyID
            If m_iDrillCompany <> 0 Then
                iCompanyID = m_iDrillCompany
                g_oBusiness.DrillCompany = True

            Else
                g_oBusiness.DrillCompany = False
                If VB6.GetItemData(cboSource, cboSource.SelectedIndex) <> 0 Then
                    iCompanyID = VB6.GetItemData(cboSource, cboSource.SelectedIndex)

                    m_vSourceArray = iCompanyID
                    g_oBusiness.RestrictByCompany = True
                Else
                    m_vSourceArray = m_vSourceArrayCopy
                    g_oBusiness.RestrictByCompany = True
                End If
            End If

            'BB Because of way control currently works, only update ID if lookup performed (or text entered (JY))

            ' We've been passed an account_id so we need to populate the code
            If (m_lAccountID <> 0) And (Strings.Len(txtAccountCode.Text) = 0) Then

                m_lReturn = g_oBusiness.GetAccountCodeFromID(r_sShortCode:=sShortCode, v_lAccountId:=m_lAccountID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
                txtAccountCode.Text = sShortCode
                txtAccountCode.Tag = CStr(m_lAccountID)
            End If

            ' We've been passed an InsuredAccount_id so we need to populate the code
            If (m_lInsuredAccountID <> 0) And (Strings.Len(txtInsuredAccountCode.Text) = 0) Then

                m_lReturn = g_oBusiness.GetAccountCodeFromID(r_sShortCode:=sShortCode, v_lAccountId:=m_lInsuredAccountID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetAccountCodeFromID Method Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                txtInsuredAccountCode.Text = sShortCode
                txtInsuredAccountCode.Tag = CStr(m_lInsuredAccountID)
            End If

            ' The user has typed in a code so we need the account_id
            If txtAccountCode.Text <> "" OrElse panAgentCode.Text <> "" Then
                If Strings.Len(Convert.ToString(txtAccountCode.Tag)) = 0 Then
                    m_lReturn = GetAccountID(r_lAccountID:=lAccountID)

                    If lAccountID = 0 Then
                        m_lReturn = PopulateAccountCode()
                        If Strings.Len(Convert.ToString(txtAccountCode.Tag)) > 0 Then
                            lAccountID = CInt(Convert.ToString(txtAccountCode.Tag))
                        End If
                    End If

                Else
                    lAccountID = CInt(Convert.ToString(txtAccountCode.Tag))
                End If

            Else
                lAccountID = m_lAccountID
            End If

            ' DD 11/04/2003
            ' The user has typed in an insured code so we need the account_id
            If txtInsuredAccountCode.Text <> "" Then
                If Strings.Len(Convert.ToString(txtInsuredAccountCode.Tag)) = 0 Then
                    m_lReturn = GetAccountID(r_lAccountID:=lInsuredAccountID, bInsuredAccount:=True)

                    If lInsuredAccountID = 0 Then
                        m_lReturn = PopulateAccountCode(bInsuredAccount:=True)
                        If Strings.Len(Convert.ToString(txtAccountCode.Tag)) > 0 Then
                            lInsuredAccountID = CInt(Convert.ToString(txtInsuredAccountCode.Tag))
                        End If
                    End If

                Else
                    lInsuredAccountID = CInt(Convert.ToString(txtInsuredAccountCode.Tag))
                End If
            End If

            If txtCaseNumber.Text <> "" Or InStr(1, txtCaseNumber.Text, "%") > 0 Then
                sCaseNumber = CStr(txtCaseNumber.Text)
                m_lReturn = GetCaseNumber(r_sCaseNumber:=sCaseNumber)

                If (sCaseNumber = "") Then
                    'txtCaseNumber.Text = ""
                    m_lReturn = PopulateCaseNumber()
                    sCaseNumber = CStr(txtCaseNumber.Text)
                End If
            End If

            ' Alix - 18/02/2003 - Issue 2325
            ' We found a client, enable event button
            'changed as per naming convention in DotNet
            tbrMain.Items.Item("_Event").Enabled = True

            'We cancelled out, did we?
            If lAccountID = 0 Then

                ' Alix - 18/02/2003 - Issue 2325
                ' We found NO client, disable event button
                'changed as per naming convention in DotNet
                tbrMain.Items.Item("_Event").Enabled = False

                'This bit is in case we clicked Find Now with nothing in the account code, which is ok.
                If txtAccountCode.Text.Trim() <> "" Then

                    ' empty the list
                    m_lReturn = ClearInterface(v_bSilent:=True)

                    DisplayStatusFound()

                    DisableInterface(bDisable:=False)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            If Not v_bSilent Then
                m_lReturn = DisplayAccountDetails()
            End If

            'DC130606 show Year To Date figures for Datasure
            m_lReturn = m_oAccount.GetAccountLedger(v_lAccountId:=lAccountID, v_lLedgerId:=m_lLedgerID, v_sLedgerCode:=m_sLedgerName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountLedger failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            sDocumentRef = txtDocumentRef.Text
            iCurrencyID = cmbCurrency.ItemId

            Dim dbNumericTemp As Double
            If Double.TryParse(txtCurrencyAmount.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                cCurrencyAmount = CDec(txtCurrencyAmount.Text)
            Else
                cCurrencyAmount = 0
            End If

            'MKW 040403 - PN3388
            Dim dbNumericTemp2 As Double
            If Double.TryParse(txtTolerance.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                iTolerance = CInt(txtTolerance.Text)
            Else
                iTolerance = 0
            End If

            iDocTypeGroupId = VB6.GetItemData(cmbDocTypeGroup, cmbDocTypeGroup.SelectedIndex)
            iDocumentTypeID = VB6.GetItemData(cmbDocumentType, cmbDocumentType.SelectedIndex)
            lPeriodId = VB6.GetItemData(cmbPeriod, cmbPeriod.SelectedIndex)

            If Information.IsDate(txtDateFrom.Text) Then
                dtDateFrom = CDate(txtDateFrom.Text)
            Else
                dtDateFrom = #12/30/1899#
            End If

            If Information.IsDate(txtDateTo.Text) Then
                dtDateTo = CDate(txtDateTo.Text)
            Else
                dtDateTo = #12/30/1899#
            End If

            If Information.IsDate(txtDueDateFrom.Text) Then
                dtDueDateFrom = CDate(txtDueDateFrom.Text)
            Else
                dtDueDateFrom = #12/30/1899#
            End If

            If Information.IsDate(txtDueDateTo.Text) Then
                dtDueDateTo = CDate(txtDueDateTo.Text)
            Else
                dtDueDateTo = #12/30/1899#
            End If

            sInsuranceRef = txtInsuranceRef.Text.Replace("'", "''")

            If cboPMUser.UserID <> 0 Then
                sOperatorName = cboPMUser.ItemUsername
            Else
                sOperatorName = ""
            End If

            sPurchaseInvoiceNo = txtPurchaseInvoiceNo.Text
            sPurchaseOrderNo = txtPurchaseOrderNo.Text

            sDepartment = cboDepartment.ItemCaption
            If sDepartment.Trim().ToLower() = "(all)" Then
                sDepartment = ""
            End If

            sSpare = txtSpare.Text
            sAltRef = txtAltRef.Text

            'Get the values from the check boxes
            iOutstandingOnly = chkDisplayOutstanding.CheckState
            bDisplay500 = chkDisplay500.CheckState = CheckState.Checked

            If v_bSilent Then
                bDisplay500 = False
            End If

            bIncReversed = chkIncReversedTxs.CheckState = CheckState.Checked
            If txtBGRef.Text <> "" Then
                sBGRef = txtBGRef.Text
            End If

            If txtCaseNumber.Text <> "" Then
                sCaseNumber = txtCaseNumber.Text
            End If

            'DD 15/07/2002: Added enhanced security call
            If m_bEnhancedSecurity Then
                m_lReturn = g_oBusiness.SelectTransQueryFiltered(r_lNumberOfRecords:=ACMaxSearchDetails, r_vResultArray:=vSearchData, v_iCompanyID:=iCompanyID, v_iAgentCnt:=IIf(Not String.IsNullOrEmpty(m_vUserPartyArray(6, 0)), UserPartyArray(6, 0), 0), v_vAccountID:=lAccountID, v_vDocumentRef:=sDocumentRef, v_vCurrencyId:=iCurrencyID, v_vCurrencyAmount:=cCurrencyAmount, v_vTolerance:=iTolerance, v_vDocTypeGroupId:=iDocTypeGroupId, v_vDocumentTypeID:=iDocumentTypeID, v_vPeriodID:=lPeriodId, v_vDateFrom:=dtDateFrom, v_vDateTo:=dtDateTo, v_vInsuranceRef:=sInsuranceRef, v_vusername:=sOperatorName, v_vPurchaseOrderNo:=sPurchaseOrderNo, v_vPurchaseInvoiceNo:=sPurchaseInvoiceNo, v_vDepartment:=sDepartment, v_vSpare:=sSpare, v_vOutstandingOnly:=iOutstandingOnly, v_vInsuredAccountID:=lInsuredAccountID, v_bRollUp:=bRollup, v_vCashListId:=m_lCashListId, v_lDocumentID:=m_lDocumentId, v_vSourceArray:=m_vSourceArray, v_bDisplay500:=bDisplay500, v_bIncludeReversedTrans:=bIncReversed, v_bInsuredAccountView:=m_bInsuredAccountView, v_vDueDateFrom:=dtDueDateFrom, v_vDueDateTo:=dtDueDateTo)
            Else
                m_lReturn = g_oBusiness.SelectTransQuery(r_lNumberOfRecords:=ACMaxSearchDetails, r_vResultArray:=vSearchData, v_iCompanyID:=iCompanyID, v_iAgentCnt:=IIf(Not String.IsNullOrEmpty(m_vUserPartyArray(6, 0)), UserPartyArray(6, 0), 0), v_vAccountID:=lAccountID, v_vDocumentRef:=sDocumentRef, v_vCurrencyId:=iCurrencyID, v_vCurrencyAmount:=cCurrencyAmount, v_vTolerance:=iTolerance, v_vDocTypeGroupId:=iDocTypeGroupId, v_vDocumentTypeID:=iDocumentTypeID, v_vPeriodID:=lPeriodId, v_vDateFrom:=dtDateFrom, v_vDateTo:=dtDateTo, v_vInsuranceRef:=sInsuranceRef, v_vusername:=sOperatorName, v_vPurchaseOrderNo:=sPurchaseOrderNo, v_vPurchaseInvoiceNo:=sPurchaseInvoiceNo, v_vDepartment:=sDepartment, v_vSpare:=sSpare, v_vOutstandingOnly:=iOutstandingOnly, v_vInsuredAccountID:=lInsuredAccountID, v_bRollUp:=bRollup, v_vCashListId:=m_lCashListId, v_lDocumentID:=m_lDocumentId, v_vUnderwritingYearID:=cboUnderwritingYearID.ItemId, v_vSourceArray:=m_vSourceArray, v_bDisplay500:=bDisplay500, v_sAltReference:=sAltRef, v_bIncludeReversedTrans:=bIncReversed, v_bInsuredAccountView:=m_bInsuredAccountView, v_sBGRef:=sBGRef, v_vDueDateFrom:=dtDueDateFrom, v_vDueDateTo:=dtDueDateTo, v_iAccountTypeID:=iAccountType, v_sCaseNumber:=sCaseNumber)
            End If

            Dim cGridTotalAmount As Decimal
            Dim cFridTotalOutAmount As Decimal
            Dim cGridTotalAmountBase As Decimal
            Dim cFridTotalOutAmountBase As Decimal

            If Information.IsArray(vSearchData) Then
                For icount As Integer = 0 To vSearchData.GetUpperBound(klRowDimension - 1)
                    cGridTotalAmount += ToSafeDouble(vSearchData(ACICurrencyAmount, icount))
                    cFridTotalOutAmount += ToSafeDouble(vSearchData(ACIOutstandingTransAmount, icount))
                    cGridTotalAmountBase += ToSafeDouble(vSearchData(ACIBaseAmount, icount))
                    cFridTotalOutAmountBase += ToSafeDouble(vSearchData(ACIOutstandingAccountAmount, icount))
                Next
                If optAmountTransaction.Checked Then
                    stbStatus.Items.Item(4).Text = "Grid Total Amount : " & cGridTotalAmount
                Else
                    stbStatus.Items.Item(4).Text = "Grid Total Amount : " & cGridTotalAmountBase
                End If

                If optOutstandingTransaction.Checked Then
                    stbStatus.Items.Item(5).Text = "Grid Total Outstanding Amount : " & cFridTotalOutAmount
                Else
                    stbStatus.Items.Item(5).Text = "Grid Total Outstanding Amount : " & cFridTotalOutAmountBase
                End If
            End If

            If Not v_bSilent Then
                m_vSearchData = vSearchData
            Else
                m_vCheckSearchData = vSearchData
            End If

            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Found search details.
                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' No search details found.
                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
                    Return result
            End Select

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try

    End Function

    ' ***************************************************************** '
    ' Name: GetPeriodLookup
    '
    ' Description: Gets a period details and populates a combo
    '
    ' ***************************************************************** '
    Private Function GetPeriodLookup() As Integer

        Dim result As Integer = 0
        Dim lPeriodId As Integer
        Dim iCompanyID As Integer
        Dim sYearName, sPeriodName As String
        Dim dtPeriodEndDate As Date
        Dim sLookupDesc As String = ""
        Dim lTodayPeriodID As Integer
        Dim iTodayPeriodListIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sLookupDesc = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAllTypes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Dim cmbPeriod_NewIndex As Integer = -1
            cmbPeriod_NewIndex = cmbPeriod.Items.Add(sLookupDesc)
            VB6.SetItemData(cmbPeriod, cmbPeriod_NewIndex, -1)

            m_lReturn = g_oBusiness.GetPeriodForDate(dtDateInPeriod:=DateTime.Today, lPeriodId:=lTodayPeriodID, vYearName:=sYearName)

            m_lReturn = g_oBusiness.GetDetails(vYearName:=sYearName)

            While m_lReturn = gPMConstants.PMEReturnCode.PMTrue

                m_lReturn = g_oBusiness.GetNext(vPeriodId:=lPeriodId, vCompanyID:=iCompanyID, vYearName:=sYearName, vPeriodName:=sPeriodName, vPeriodEndDate:=dtPeriodEndDate)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    sLookupDesc = sYearName.Trim() & ": " & sPeriodName
                    cmbPeriod_NewIndex = cmbPeriod.Items.Add(sLookupDesc)
                    VB6.SetItemData(cmbPeriod, cmbPeriod_NewIndex, lPeriodId)
                    If lPeriodId = lTodayPeriodID Then
                        iTodayPeriodListIndex = cmbPeriod_NewIndex
                    End If

                End If
            End While

            cmbPeriod.SelectedIndex = 0

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPeriodLookup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetTransactionsForDocument(ByVal v_sDocumentRef As String, ByVal v_lAccountId As Integer, ByVal v_iCompanyID As Integer, ByRef r_vResultArray As Object, Optional ByRef v_bOrderBySpare As Boolean = False, Optional ByRef v_sAltReference As String = "") As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetTransactionsForDocument
        ' PURPOSE: Get all transactions related to the passed document reference
        ' AUTHOR: Paul Cunningham
        ' DATE: 04 June 2003, 11:31:39
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------
        Dim result As Integer = 0
        Const ACMethod As String = "GetTransactionsForDocument"

        Try
            Dim bIncReversed As Boolean
            result = gPMConstants.PMEReturnCode.PMFalse
            If chkIncReversedTxs.Checked = True Then
                bIncReversed = True
            Else
                bIncReversed = False
            End If

            'Go get the details for the passed document ref
            If m_bEnhancedSecurity Then

                If g_oBusiness.SelectTransQueryFiltered(r_lNumberOfRecords:=ACMaxSearchDetails, r_vResultArray:=r_vResultArray, v_vAccountID:=v_lAccountId, v_iCompanyID:=v_iCompanyID, v_vDocumentRef:=v_sDocumentRef, v_bRollUp:=False, v_bOrderBySpare:=v_bOrderBySpare) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Unable to get transactions for document: " & v_sDocumentRef)
                End If
            Else

                If g_oBusiness.SelectTransQuery(r_lNumberOfRecords:=ACMaxSearchDetails, r_vResultArray:=r_vResultArray, v_vAccountID:=v_lAccountId, v_iCompanyID:=v_iCompanyID, v_vDocumentRef:=v_sDocumentRef, v_bRollUp:=False, v_bOrderBySpare:=v_bOrderBySpare, v_sAltReference:=v_sAltReference, v_bIncludeReversedTrans:=bIncReversed) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Unable to get transactions for document: " & v_sDocumentRef)
                End If
            End If

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result

                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result

            End Select

        Finally


        End Try
        Return result
    End Function
    'Modified add an extra funtion
    Public Function ControlList(ByVal root As Control, ByRef resultArray As ArrayList) As ArrayList

        If root.HasChildren Then
            For Each cControl As Control In root.Controls
                resultArray.Add(cControl)
                If cControl.HasChildren Then
                    If Not cControl.Name.Contains("uct") Then
                        ControlList(cControl, resultArray)
                    End If
                End If
            Next cControl
        End If
        Return resultArray
    End Function
    Public Function handleSearchParams() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: handleSearchParams
        ' PURPOSE: populate search fields from passed search details
        ' AUTHOR: Paul Cunningham
        ' DATE: 30 May 2003, 11:17:25
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACMethod As String = "handleSearchParams"
        ' 'Modified add an

        Const klRowDimension As Integer = 2

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'To search the Controls in form
            arrControlList = New ArrayList
            If Me.HasChildren Then
                ControlList(Me, arrControlList)
            End If

            'Loop through all passed search criteria and populat form fields

            For lSearch As Integer = m_vSearchParams.GetLowerBound(klRowDimension - 1) To m_vSearchParams.GetUpperBound(klRowDimension - 1)
                'Modified,add a for loop and a dim condition,comment the for loop
                For arrItem As Integer = 0 To arrControlList.Count - 1
                    Dim ctl As Control = arrControlList(arrItem)
                    'For Each ctl As Control In ContainerHelper.Controls(Me)

                    If ctl.Name = CStr(m_vSearchParams(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lSearch)) Then

                        Select Case ctl.GetType().Name.ToUpper()
                            Case "CBOPMUSERLOOKUP", "COMBOBOX"

                                'ReflectionHelper.SetMember(ctl, "ListIndex", m_vSearchParams(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lSearch))
                                If ctl.GetType().Name.ToUpper() = "CBOPMUSERLOOKUP" Then
                                    ReflectionHelper.SetMember(ctl, "ListIndex", m_vSearchParams(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lSearch))
                                Else
                                    CType(ctl, ComboBox).SelectedIndex = m_vSearchParams(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lSearch)
                                End If

                            Case "CBOPMLOOKUP"

                                ReflectionHelper.SetMember(ctl, "ItemId", m_vSearchParams(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lSearch))
                            Case "TEXTBOX"

                                ctl.Text = CStr(m_vSearchParams(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lSearch))
                                'Case "CHECKBOX", "OPTIONBUTTON"
                            Case "CHECKBOX", "RADIOBUTTON"

                                'ReflectionHelper.SetMember(ctl, "value", m_vSearchParams(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lSearch))

                                If ctl.GetType().Name.ToUpper() = "RADIOBUTTON" Then
                                    ReflectionHelper.SetMember(CType(ctl, RadioButton), "Checked", m_vSearchParams(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lSearch))
                                Else
                                    ReflectionHelper.SetMember(CType(ctl, CheckBox), "Checked", m_vSearchParams(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lSearch))
                                End If

                            Case Else
                                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unknown control")
                        End Select
                    End If
                    'Next ctl
                Next arrItem
            Next

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

    Private Function HighlightDocRefItems() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: HighlightDocRefItems
        ' PURPOSE: highlights all the other items in the listview that have the same document ref
        ' AUTHOR: Andrew Bibby
        ' DATE: 18/02/2003, 17:21
        ' RETURNS: PMTrue for success
        ' CHANGES: AMB 18/02/2003: PS220 - Manage Debtors
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim sCurrDocRef, sFoundDocRef As String
        Dim oCurrItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' get the current doc ref
            If (lvwSearchResults.CheckedItems.Count > 0) Then
                oCurrItem = lvwSearchResults.CheckedItems(0)
            Else
                Return result
            End If

            sCurrDocRef = ListViewHelper.GetListViewSubItem(oCurrItem, ACListDocumentRef).Text.Trim().ToUpper()

            ' search the rest of the listview for associated items
            For Each oSelectedItem As ListViewItem In lvwSearchResults.Items
                If (oSelectedItem IsNot Nothing) Then
                    sFoundDocRef = ListViewHelper.GetListViewSubItem(oSelectedItem, ACListDocumentRef).Text.Trim().ToUpper()

                    ' check if it's the same doc ref
                    If sFoundDocRef = sCurrDocRef Then
                        ' highlight the item
                        oSelectedItem.Selected = True
                    End If
                End If
            Next oSelectedItem

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in HighlightDocRefItems", vApp:=ACApp, vClass:=ACClass, vMethod:="HighlightDocRefItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result
            End Select

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: LockTransdetailId
    '
    ' Description:
    '
    ' History: 13/05/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function LockTransdetailId(ByVal v_lTransDetailId As Integer, ByRef r_bAlreadyLocked As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sCurrentlyLockedBy As String = ""

            'We need to lock this transdetail_id
            If m_oPMLock Is Nothing Then
                Dim temp_m_oPMLock As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oPMLock = temp_m_oPMLock
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to create instance of bPMLock.User", vApp:=ACApp, vClass:=ACClass, vMethod:="LockTransdetailId")
                    Return result
                End If
            End If

            m_lReturn = m_oPMLock.LockKey(sKeyName:="ALLOCATION", vKeyValue:=v_lTransDetailId, iUserID:=g_iUserID, sCurrentlyLockedBy:=sCurrentlyLockedBy)
            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="bPMLock.User.LockKey Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockTransdetailId")
                Return result
            End If
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                MessageBox.Show("One of the Transactions is already being allocated by " & sCurrentlyLockedBy, "Find Transaction", MessageBoxButtons.OK)
                r_bAlreadyLocked = True
            Else
                r_bAlreadyLocked = False
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockTransdetailId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockTransdetailId", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub lvwSearchResults_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwSearchResults.Click
        Try

            If (lvwSearchResults.CheckedItems.Count > 0) Then
                If Not (lvwSearchResults.CheckedItems(0) Is Nothing) Then
                    lvwSearchResults.Focus()
                    lvwSearchResults_ItemClick(lvwSearchResults.CheckedItems(0))
                End If
            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub lvwSearchResults_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchResults.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwSearchResults.Columns(eventArgs.Column)

        StoreHScrollValue()

        Try

            If (lvwSearchResults.Items.Count > 0) Then

                RemoveHandler lvwSearchResults.ItemChecked, AddressOf lvwSearchResults_ItemChecked
                lvwSearchResults.Items.Item(0).EnsureVisible()
                ' Column click event for the search details
                ' Defer to the common interface
                OnColumnClick(lvwSearchResults, ColumnHeader)
                'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.1.2)
                m_iSortKey = ColumnHeader.Index + 1
                m_iSortOrder = ListViewHelper.GetSortOrderProperty(lvwSearchResults)
                'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.1.2)

                'start
                For i As Integer = 0 To lvwSearchResults.Items.Count - 1
                    If i Mod 2 = 0 Then
                        lvwSearchResults.Items(i).BackColor = Color.White
                    Else
                        lvwSearchResults.Items(i).BackColor = Color.LightYellow
                    End If
                Next
                'end
                AddHandler lvwSearchResults.ItemChecked, AddressOf lvwSearchResults_ItemChecked
            End If
            RecoverHorizontalScroll()
        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort columns", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchResults_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.5.2)
    Private Function SaveInterfaceDisplaySettings() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SaveInterfaceDisplaySettings"

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            Dim sColumnWidth As String = ""
            Dim oXMLRootElement, oXMLComponent, oXMLInterface, oXMLListView, oXMLSortKey, oXMLSortOrder, oXMLColumnWidth As XmlElement

            g_objDOMRootForInterfaceDisplay = New XmlDocument()

            If g_sUserConfigXMLDataset = "" Then

                oXMLRootElement = g_objDOMRootForInterfaceDisplay.CreateElement(m_kRootNode)

                g_objDOMRootForInterfaceDisplay.AppendChild(oXMLRootElement)

                oXMLComponent = g_objDOMRootForInterfaceDisplay.CreateElement(m_kComponentName)

                oXMLRootElement.AppendChild(oXMLComponent)

                oXMLInterface = g_objDOMRootForInterfaceDisplay.CreateElement(m_kFrmName)

                oXMLComponent.AppendChild(oXMLInterface)

                oXMLListView = g_objDOMRootForInterfaceDisplay.CreateElement(m_kGridName)

                oXMLInterface.AppendChild(oXMLListView)

                oXMLSortKey = g_objDOMRootForInterfaceDisplay.CreateElement(m_kColumnSortKey)
                oXMLSortKey.InnerText = CStr(m_iSortKey)

                oXMLListView.AppendChild(oXMLSortKey)

                oXMLSortOrder = g_objDOMRootForInterfaceDisplay.CreateElement(m_kColumnSortOrder)
                oXMLSortOrder.InnerText = CStr(m_iSortOrder)

                oXMLListView.AppendChild(oXMLSortOrder)

                m_lReturn = GetListViewColumnWidth(sColumnWidth)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetListViewColumnWidth Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                oXMLColumnWidth = g_objDOMRootForInterfaceDisplay.CreateElement(m_kColumnWidth)
                oXMLColumnWidth.InnerText = sColumnWidth

                oXMLListView.AppendChild(oXMLColumnWidth)

                g_sUserConfigXMLDataset = g_objDOMRootForInterfaceDisplay.InnerXml

                'Sankar only for test
                'g_objDOMRootForInterfaceDisplay.save "c:\test.xml"
            Else

                Try
                    g_objDOMRootForInterfaceDisplay.LoadXml(g_sUserConfigXMLDataset)

                Catch
                End Try

                'TODOLIST: 
                'If g_objDOMRootForInterfaceDisplay.parseError.errorCode <> 0 Then

                'gPMFunctions.RaiseError(kMethodName, g_objDOMRootForInterfaceDisplay.parseError.Message, gPMConstants.PMELogLevel.PMLogError)
                'gPMFunctions.RaiseError(kMethodName, g_objDOMRootForInterfaceDisplay.InnerText, gPMConstants.PMELogLevel.PMLogError)
                'End If

                m_lReturn = GetListViewColumnWidth(sColumnWidth)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "UpdateXMLNodeValues Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = UpdateXMLNodeValues(iSortKey:=m_iSortKey, iSortOrder:=m_iSortOrder, sColumnWidth:=sColumnWidth)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "UpdateXMLNodeValues Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                'Sankar only for test
                'g_objDOMRootForInterfaceDisplay.save "c:\Output.xml"
                g_sUserConfigXMLDataset = g_objDOMRootForInterfaceDisplay.InnerXml
            End If

            Return result

        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            Return result
        End Try
    End Function

    Private Function UpdateXMLNodeValues(ByVal iSortKey As Integer, ByVal iSortOrder As Integer, ByVal sColumnWidth As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateXMLNodeValues"

        Dim bComPresent, bFrmPresent, bGridPresent As Boolean
        Dim sXMLPath As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            sXMLPath = "//" & m_kRootNode & "/" & m_kComponentName
            If Not (g_objDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath) Is Nothing) Then
                If g_objDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath).HasChildNodes Then
                    bComPresent = True

                    sXMLPath = sXMLPath & "/" & m_kFrmName
                    If Not (g_objDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath) Is Nothing) Then
                        If g_objDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath).HasChildNodes Then
                            bFrmPresent = True

                            sXMLPath = sXMLPath & "/" & m_kGridName
                            If Not (g_objDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath) Is Nothing) Then
                                If g_objDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath).HasChildNodes Then
                                    bGridPresent = True

                                    g_objDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath & "/" & m_kColumnSortKey).InnerText = CStr(iSortKey)
                                    g_objDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath & "/" & m_kColumnSortOrder).InnerText = CStr(iSortOrder)
                                    g_objDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath & "/" & m_kColumnWidth).InnerText = sColumnWidth
                                Else
                                    bGridPresent = False
                                End If
                            Else
                                bGridPresent = False
                            End If
                        Else
                            bFrmPresent = False
                        End If
                    Else
                        bFrmPresent = False
                    End If
                Else
                    bComPresent = False
                End If
            Else
                bComPresent = False
            End If

            If Not bComPresent Or Not bFrmPresent Or Not bGridPresent Then
                m_lReturn = CreateNodeIfNotExists(bComPresent:=bComPresent, bFrmPresent:=bFrmPresent, bGridPresent:=bGridPresent, iSortKey:=iSortKey, iSortOrder:=iSortOrder, sColumnWidth:=sColumnWidth)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "CreateNodeIfNotExists Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
            Return result

        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            Return result
        End Try
    End Function

    Private Function CreateNodeIfNotExists(ByVal bComPresent As Boolean, ByVal bFrmPresent As Boolean, ByVal bGridPresent As Boolean, ByVal iSortKey As Integer, ByVal iSortOrder As Integer, ByVal sColumnWidth As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateNodeIfNotExists"

        Dim oXMLDoc As New XmlDocument
        Dim oXMLRootElement, oXMLForm, oXMLListView, oXMLSortKey, oXMLSortOrder, oXMLColumnWidth As XmlElement
        Dim oUserConfigXML, oComponentLevelXML As XmlNodeList
        Dim iRootLevel As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            oXMLRootElement = oXMLDoc.CreateElement(m_kComponentName)
            oXMLForm = oXMLDoc.CreateElement(m_kFrmName)
            oXMLListView = oXMLDoc.CreateElement(m_kGridName)
            oXMLSortKey = oXMLDoc.CreateElement(m_kColumnSortKey)
            oXMLSortOrder = oXMLDoc.CreateElement(m_kColumnSortOrder)
            oXMLColumnWidth = oXMLDoc.CreateElement(m_kColumnWidth)

            oUserConfigXML = g_objDOMRootForInterfaceDisplay.SelectNodes("UserConfigXML")

            If Not bComPresent Then

                oXMLRootElement.AppendChild(oXMLForm)

                oXMLForm.AppendChild(oXMLListView)

                oXMLSortKey.InnerText = CStr(iSortKey)

                oXMLListView.AppendChild(oXMLSortKey)
                oXMLSortOrder.InnerText = CStr(iSortOrder)

                oXMLListView.AppendChild(oXMLSortOrder)
                oXMLColumnWidth.InnerText = sColumnWidth

                oXMLListView.AppendChild(oXMLColumnWidth)

                g_objDOMRootForInterfaceDisplay.ChildNodes.Item(iRootLevel).AppendChild(oXMLRootElement)

            ElseIf Not bFrmPresent And bComPresent Then
                For iComLevel As Integer = 0 To oUserConfigXML.Item(iRootLevel).ChildNodes.Count - 1
                    If oUserConfigXML.Item(iRootLevel).ChildNodes.Item(iComLevel).LocalName = m_kComponentName Then

                        oXMLForm.AppendChild(oXMLListView)
                        oXMLSortKey.InnerText = CStr(iSortKey)

                        oXMLListView.AppendChild(oXMLSortKey)
                        oXMLSortOrder.InnerText = CStr(iSortOrder)

                        oXMLListView.AppendChild(oXMLSortOrder)
                        oXMLColumnWidth.InnerText = sColumnWidth

                        oXMLListView.AppendChild(oXMLColumnWidth)

                        g_objDOMRootForInterfaceDisplay.ChildNodes.Item(iRootLevel).ChildNodes.Item(iComLevel).AppendChild(oXMLForm)
                    End If
                Next
            ElseIf Not bGridPresent And bFrmPresent And bComPresent Then
                For iComLevel As Integer = 0 To oUserConfigXML.Item(iRootLevel).ChildNodes.Count - 1
                    If oUserConfigXML.Item(iRootLevel).ChildNodes.Item(iComLevel).LocalName = m_kComponentName Then
                        oComponentLevelXML = oUserConfigXML.Item(iRootLevel).ChildNodes.Item(iComLevel).ChildNodes

                        For iFrmLevel As Integer = 0 To oComponentLevelXML.Count - 1
                            If oComponentLevelXML.Item(iFrmLevel).LocalName = m_kFrmName Then

                                oXMLSortKey.InnerText = CStr(iSortKey)

                                oXMLListView.AppendChild(oXMLSortKey)
                                oXMLSortOrder.InnerText = CStr(iSortOrder)

                                oXMLListView.AppendChild(oXMLSortOrder)
                                oXMLColumnWidth.InnerText = sColumnWidth

                                oXMLListView.AppendChild(oXMLColumnWidth)

                                g_objDOMRootForInterfaceDisplay.ChildNodes.Item(iRootLevel).ChildNodes.Item(iComLevel).ChildNodes.Item(iFrmLevel).AppendChild(oXMLListView)

                            End If
                        Next
                    End If
                Next
            End If
            Return result

        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            Return result
        End Try
    End Function

    Private Function GetListViewColumnWidth(ByRef sColumnWidth As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetListViewColumnWidth"

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'start
            If (lvwSearchResults.Columns.Count > 0) Then
                For Each oColumnHeader As ColumnHeader In lvwSearchResults.Columns
                    sColumnWidth = sColumnWidth & ";" & gPMFunctions.ToSafeString(VB6.PixelsToTwipsX(oColumnHeader.Width))
                Next oColumnHeader
                sColumnWidth = sColumnWidth.Substring(sColumnWidth.Length - (sColumnWidth.Length - 1))
            End If
            'end

            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            Return result
        End Try
    End Function
    ''' <summary>
    ''' list view Search Results
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub lvwSearchResults_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchResults.DoubleClick

        ' Double click event for the search details.
        Dim oAllocation As Object

        Try

            If IsInRoadMap() Then
                ' Check if there are any items available.
                If (lvwSearchResults.Items.Count = 0) Then
                    Exit Sub
                End If

                If (lvwSearchResults.CheckedItems.Count = 0) Then
                    Exit Sub 'was selected but then deselected as invalid selection
                End If

                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMOK

                m_lReturn = g_oObjectManager.GetInstance(oObject:=oAllocation, sClassName:="bACTAllocation.Form", vInstanceManager:=PMGetViaClientManager)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                End If

                If ChkDocTypeIsInstalments(lvwSearchResults.FocusedItem.SubItems(4).Text.Substring(0, 3)) = True _
                And CheckIsLinkedToThirdPartyScheme(lvwSearchResults.FocusedItem.SubItems(4).Text) = False Then
                    MessageBox.Show("Instalment transactions are handled automatically by Sirius. " &
                                    "They are not allowed to be manually allocated.",
                                    "Invalid Selection", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error)
                    If ChkDocTypeIsInstalments(lvwSearchResults.FocusedItem.SubItems(4).Text.Substring(0, 3)) Then
                        MessageBox.Show("Instalment transactions are handled automatically by Sirius. They are not allowed to be manually allocated.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If

                oAllocation = Nothing
                ' Process the next set of actions.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                End If
            End If

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchResults_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub lvwSearchResults_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchResults.Enter

        ' GotFocus Event for the search details

        Try

            'GSD 200802 first we need to check if there are any items available.
            If (lvwSearchResults.Items.Count = 0) Then
                Exit Sub
            End If

            ' Set the default button
            VB6.SetDefault(cmdOK, True)

            ' Allow find based on a selected trans
            '    cmdFindAccTrans.Enabled = True
            '    cmdFindDocTrans.Enabled = True
            'SetDrillButtons()

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchResults_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchResults_ItemClick(ByVal Item As ListViewItem)

        Dim lRow, lDocumentTypeID As Integer

        ' Item click event for the search details.

        Try

            ' Check if there are any items available.
            If (lvwSearchResults.Items.Count = 0) Then
                Exit Sub
            End If

            lRow = CInt(Item.Name.Substring(1))

            lDocumentTypeID = CInt(m_vSearchData(ACIDocumentTypeId, lRow))
            If g_bReverseReplaceTransactionsAuthority Then
                cmdReverseAndReplace.Enabled = lDocumentTypeID = ACDocTypeNBDebit Or lDocumentTypeID = ACDocTypeNBCredit Or lDocumentTypeID = ACDocTypeEndorsmentCredit Or lDocumentTypeID = ACDocTypeEndorsmentDebit Or lDocumentTypeID = ACDocTypeShortPeriodCredit Or lDocumentTypeID = ACDocTypeShortPeriodDebit Or lDocumentTypeID = ACDocTypeRenewalCredit Or lDocumentTypeID = ACDocTypeTransferedDebit Or lDocumentTypeID = ACDocTypeRenewalCredit Or lDocumentTypeID = ACDocTypeRenwalDebit
            Else
                cmdReverseAndReplace.Enabled = False
            End If

            ' HG26072002 - Only run the following when enhanced security
            ' has been activated.
            If m_bEnhancedSecurity Then
                'check for update authority

                ' AMB 11/02/2003: replace the hard-coded 26 with a constant so that it works properly
                ' If (m_vSearchData(26, lRow) = 0) Then
                If CDbl(m_vSearchData(ACIHasUnrestrictedEnquiry, lRow)) = 0 Then
                    'does not have update authority
                    Item.Checked = False
                    'exit sub
                End If
            End If

            If (m_lAllocationTransType = gACTLibrary.ACTPrimaryForAllocation) Or (m_lAllocationTransType = gACTLibrary.ACTSecondaryForAllocation) Then

                If CStr(m_vSearchData(ACIMatchedCurrencyAmount, lRow)) <> "" Then

                    If CDec(m_vSearchData(ACICurrencyAmount, lRow)) + CDec(m_vSearchData(ACIMatchedCurrencyAmount, lRow)) = 0 Then 'fully matched
                        Item.Checked = False
                    End If

                End If

            End If

            If m_lAllocationTransType = gACTLibrary.ACTSecondaryForAllocation Then

                If g_oBusiness.IsTransInAllocation(m_vSearchData(ACITransDetailId, lRow)) Then
                    Item.Checked = False 'its there already
                End If

            End If

            'Hide the button to avoid confusion
            If m_bRollup Then
                cmdExpand.Visible = True
                cmdExpand.Enabled = True
            Else
                'Hide the button to avoid confusion
                cmdExpand.Visible = False
            End If

            pnlReason.Text = CStr(m_vSearchData(ACIReason, lRow)).Trim()

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the ItemClick event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchResults_ItemClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchResults_ItemMouseHover(ByVal sender As Object, ByVal e As System.Windows.Forms.ListViewItemMouseHoverEventArgs) Handles lvwSearchResults.ItemMouseHover
        'Dim Button As Integer = CInt(EventArgs.Button)
        'Dim Shift As Integer = Control.ModifierKeys \ &H10000

        'start
        Dim x As Single = e.Item.Position.X ' EventArgs.X
        Dim y As Single = e.Item.Position.Y ' EventArgs.Y

        'end

        ' CJB 25/03/2004 Created - process listview tooltip display of document comment. Note that the tooltip
        ' processing is handled in the CToolTip class.

        Dim oSelectedItem As ListViewItem
        Dim sToolTipTitle As String = ""

        Try

            If m_bShowPopupComments Then
                ' Find the row that has been moved over and see if it has a comment for the document,
                ' if so then show it in a tooltip
                With lvwSearchResults

                    ' Set the selected item (if there is an item moved over with the mouse)
                    oSelectedItem = .GetItemAt(x, y)

                    'm_oTTNew = New ToolTip
                    If Not (oSelectedItem Is Nothing) Then
                        If Information.IsArray(m_vSearchData) Then

                            ' If the item we're on has changed (we don't want to keep re-displaying the tooltip!)
                            If m_lCurItemIndex <> oSelectedItem.Index + 1 Then

                                ' Save the item we're on for comparison next time
                                m_lCurItemIndex = oSelectedItem.Index + 1
                                sToolTipTitle = "Transaction Comment - "

                                ' Hide the tooltip as no item selected
                                If m_lCurItemIndex = 0 Then
                                    'm_oTT.Destroy()
                                    m_oTTNew.Hide(lvwSearchResults)
                                    m_oTTNew.Dispose()
                                Else
                                    ' If drilled by document then prefix the account in the tooltip header to make unique
                                    If DrillLevel = 1 Then
                                        sToolTipTitle = sToolTipTitle &
                                                        ListViewHelper.GetListViewSubItem(.Items.Item(oSelectedItem.Index), ACListAccountShortCode).Text.Trim() & " " &
                                                        "/ "
                                    End If

                                    sToolTipTitle = sToolTipTitle &
                                                    ListViewHelper.GetListViewSubItem(.Items.Item(oSelectedItem.Index), ACListDocumentRef).Text.Trim()
                                    'm_oTT.Title = sToolTipTitle
                                    'm_oTTNew.ToolTipTitle = sToolTipTitle
                                    'If Convert.ToString(.Items.Item(oSelectedItem.Index).Tag) <> "" Then
                                    If CStr(m_vSearchData(ACIComment, Convert.ToString(.Items.Item(oSelectedItem.Index).Tag))) <> "" Then
                                        'm_oTTNew.Hide(lvwSearchResults)

                                        If m_oTTNew.ToolTipTitle.Length > 0 Then
                                            m_oTTNew.ToolTipTitle.Replace(m_oTTNew.ToolTipTitle.ToString(), "") '.Remove(0, m_oTTNew.ToolTipTitle.Length - 1)
                                        End If
                                        m_oTTNew.ToolTipTitle = sToolTipTitle
                                        Try
                                            m_oTTNew.SetToolTip(lvwSearchResults, Trim(m_vSearchData(ACIComment, Convert.ToString(.Items.Item(oSelectedItem.Index).Tag))))
                                        Catch
                                        End Try
                                        Threading.Thread.Sleep(1500)
                                    End If
                                End If
                            End If
                        Else
                            ' Hide the tooltip as no item selected, reset the last item on index too
                            'm_oTT.Destroy()
                            'm_oTTNew.Hide(lvwSearchResults)
                            m_oTTNew.Dispose() '(lvwSearchResults)
                            m_lCurItemIndex = 0
                        End If
                    Else
                        ' Hide the tooltip as no item selected, reset the last item on index too
                        'm_oTT.Destroy()
                        'm_oTTNew.Hide(lvwSearchResults)
                        m_oTTNew.Dispose()
                        m_lCurItemIndex = 0
                    End If
                End With
            End If

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process listview tooltip display of document comment", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchResults_MouseMove", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchResults_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchResults.Leave

        ' LostFocus Event for the search details

        Try

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchResults_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchResults_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSearchResults.MouseMove
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        Dim oSelectedItem As ListViewItem
        Dim sToolTipTitle As String = ""

        Try
            If m_bShowPopupComments Then
                With lvwSearchResults
                    oSelectedItem = .GetItemAt(x, y)
                    If (oSelectedItem Is Nothing) Then
                        m_oTTNew.Hide(lvwSearchResults)
                        m_lCurItemIndex = 0
                    End If
                End With
            End If

        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process listview tooltip dispose", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchResults_MouseMove", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub

    Private Sub lvwSearchResults_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwSearchResults.LostFocus
        m_lCurItemIndex = 0
    End Sub

    Private Sub lvwSearchResults_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSearchResults.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000

        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        ' CJB 25/03/2004 On right mouse button click, show add or edit document comment menu item.
        ' CJB 30/03/2004 On right mouse button click, show 'do not report' or 'report' menu item to denote
        '                if the transaction should appear on the Business Transacted by Agent reports.

        Dim bAllowTransfer, bAllowWriteOff, bAllowRefund, bAllowEditComment As Boolean

        Try
            If Button = MouseButtons.Right Then
                ' PW271102 - Show a pop-up menu (this happens if they are over an item or not - already works this way)

                'Check an item is selected
                If Not (lvwSearchResults.GetItemAt(x, y) Is Nothing) Then

                    ' AMB 18/02/2003: PS220 - enable the 'write-off' on the right-click menu
                    If m_bRollup Then
                        m_lReturn = EnableWriteOffMenuRollup(r_bEnabled:=bAllowWriteOff)
                    Else
                        m_lReturn = EnableWriteOffMenu(r_bEnabled:=bAllowWriteOff)
                    End If
                    mnuSep2.Available = bAllowWriteOff
                    mnuTransactionWriteOff.Available = bAllowWriteOff

                    'TR - Determine if Transfers are allowed
                    If m_bRollup Then
                        bAllowTransfer = AllowTransfersRollup()
                    Else
                        bAllowTransfer = AllowTransfersNormal()
                    End If
                    mnuSep3.Available = bAllowTransfer
                    mnuTransfer.Available = bAllowTransfer

                    ' AMB 18/02/2003: PS220 - enable 'refund' on the right-click menu
                    m_lReturn = EnableRefundMenu(r_bEnabled:=bAllowRefund)
                    mnuSep4.Available = bAllowRefund
                    mnuRefund.Available = bAllowRefund

                    ' AMB 24/02/2003: PS220 - enable the 'approve' and 'reject' right-click menu
                    m_lReturn = EnableApproveRejectMenu(r_bEnabled:=bAllowRefund)
                    mnuSep5.Available = bAllowRefund
                    mnuApprove.Available = bAllowRefund
                    mnuSep6.Available = bAllowRefund
                    mnuReject.Available = bAllowRefund

                    ' CJB 25/03/2004 If there is a document comment (from the TransDetail table) then show an
                    ' Edit Comment menu, else show an Add Comment menu
                    m_lReturn = EnableEditCommentMenu(r_bEnabled:=bAllowEditComment)
                    mnuSep7.Available = True
                    If bAllowEditComment Then
                        If ((lvwSearchResults.CheckedItems.Count > 0) AndAlso (Mid(CStr(m_vSearchData(ACIComment, Convert.ToString(lvwSearchResults.CheckedItems(0).Tag))).Trim(), 1, 61) = "Original allocation reversed as part of Backdated Endorsement")) Then
                            mnuEditComment.Text = "&View Comment"
                        Else
                            mnuEditComment.Text = "&Edit Comment"
                        End If
                        mnuEditComment.Available = True
                        mnuAddComment.Available = False
                    Else
                        mnuEditComment.Available = False
                        mnuAddComment.Available = True
                    End If

                    ' CJB 30/03/2004 On right mouse button click, show 'do not report' or 'report' menu item
                    ' to denote if the transaction should appear on the Business Transacted by Agent reports.
                    'Hidden for Underwriting
                    mnuSep8.Available = False
                    mnuDoNotReport.Available = False
                    mnuReportOnThis.Available = False
                    mnuSep9.Available = False
                    mnuBreakdown.Available = False

                    ' finally, let's pop up that menu
                    Ctx_mnuListView.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
                End If
            Else
                With lvwSearchResults
                    'Ensure mouse is over an item
                    If (lvwSearchResults.CheckedItems.Count > 0) Then
                        SetDrillButtons()
                    Else
                        cmdFindAccTrans.Enabled = False
                        cmdFindDocTrans.Enabled = False
                        cmdAllocate.Enabled = False
                        cmdReverseAndReplace.Enabled = False
                        cmdViewAllocation.Enabled = False
                    End If

                    If (lvwSearchResults.CheckedItems.Count = 0) Then
                        cmdViewAllocation.Enabled = False
                        cmdExpand.Enabled = False
                        cmdAllocate.Enabled = False
                        cmdReverseAndReplace.Enabled = False
                        cmdFindAccTrans.Enabled = False
                        cmdFindDocTrans.Enabled = False
                    ElseIf lvwSearchResults.CheckedItems.Count > 1 Then
                        cmdViewAllocation.Enabled = False
                        cmdExpand.Enabled = False
                        If (m_sCallingAppName = OrionLink OrElse m_sCallingAppName.ToUpper() = ("iActFindTransaction").ToUpper()) Then
                            If Not m_bInsuredAccountView Then
                                cmdAllocate.Enabled = True
                            End If
                        ElseIf m_sCallingAppName = "iPMWrkComponentStarter" Then
                            cmdAllocate.Enabled = True
                        End If
                        cmdReverseAndReplace.Enabled = False
                        cmdFindAccTrans.Enabled = False
                        cmdFindDocTrans.Enabled = False
                    Else
                        cmdViewAllocation.Enabled = True
                        cmdExpand.Enabled = True
                        cmdAllocate.Enabled = False
                        cmdReverseAndReplace.Enabled = True
                        cmdFindAccTrans.Enabled = True
                        cmdFindDocTrans.Enabled = True
                    End If
                    If ((ModifierKeys = Keys.Control) Or (ModifierKeys = Keys.Shift)) Then
                        If lvwSearchResults.SelectedItems.Count > 0 Then
                            Dim iStart As Integer = lvwSearchResults.SelectedItems(0).Index
                            Dim iStop As Integer = lvwSearchResults.SelectedItems(lvwSearchResults.SelectedItems.Count - 1).Index
                            For i As Integer = iStart To iStop
                                If Not lvwSearchResults.Items(i).Checked Then
                                    RemoveHandler lvwSearchResults.ItemChecked, AddressOf lvwSearchResults_ItemChecked
                                    lvwSearchResults.Items(i).Checked = True
                                    AddHandler lvwSearchResults.ItemChecked, AddressOf lvwSearchResults_ItemChecked
                                End If
                            Next
                        End If
                    End If
                End With
            End If

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mouse up event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchResults_MouseUp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Public Sub mnuAddComment_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuAddComment.Click
        ' CJB 25/03/2004 Created - Process the right mouse button Add menu click

        Const ACMethod As String = "mnuAddComment_Click"

        m_lReturn = ProcessAddEditComment()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error adding a comment")
        End If

    End Sub

    Public Sub mnuBreakdown_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuBreakdown.Click

        Const ACMethod As String = "mnuBreakdown_Click"

        m_lReturn = ProcessBreakdown()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error producing the breakdown information for the selected document")
        End If

    End Sub

    Public Sub mnuDoNotReport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDoNotReport.Click
        ' CJB 30/03/2004 Created - Process the right mouse button Do Not Report menu click

        Const ACMethod As String = "mnuDoNotReportComment_Click"

        m_lReturn = ProcessDoNotReport()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error flagging the item to not report upon")
        End If

    End Sub

    Public Sub mnuEditComment_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuEditComment.Click
        ' CJB 25/03/2004 Created - Process the right mouse button Edit menu click

        Const ACMethod As String = "mnuEditComment_Click"

        m_lReturn = ProcessAddEditComment()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error editing a comment")
        End If

    End Sub

    '
    ' PW271102 - New pop-up menu item to go to the instalment plan for a
    ' specific document / transaction
    ' PS209
    '
    Public Sub mnuInstalmentPlan_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuInstalmentPlan.Click

        Dim sDocumentReference As String = ""
        Dim lCompanyID As Integer
        Dim vResultArray(,) As Object

        Dim oFinancePlanMaint As Object
        Dim vKeyArray(,) As Object

        Try
            '
            ' Get the finance plan details
            '
            ' Check if anything currently selected
            'If (lvwSearchResults.CheckedItems.Count > 0) Then
            If (lvwSearchResults.SelectedItems.Count > 0) Then

                sDocumentReference = lvwSearchResults.SelectedItems(0).SubItems(ACListDocumentRef).Text.Trim()
                lCompanyID = gPMFunctions.ToSafeLong(lvwSearchResults.SelectedItems(0).SubItems(ACListSourceID).Text, 0) 'gPMFunctions.ToSafeLong(lvwSearchResults.CheckedItems(0).Text, 0)

                ' Get the Finance Plan details for the current document

                m_lReturn = g_oBusiness.GetFinancePlanDetails(v_sDocumentReference:=sDocumentReference, v_lCompanyID:=lCompanyID, r_vResultArray:=vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetFinancePlanDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuInstalmentPlan_Click")
                    Exit Sub
                End If
                ' Check if anything returned
                If Not Information.IsArray(vResultArray) Then
                    MessageBox.Show("Could not find an Instalment Plan for the " & "selected Transaction.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If

            End If
            '
            ' Load the Finance Plan interface
            '
            ' Create an instance of the finance plan maintenance object
            Dim temp_oFinancePlanMaint As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFinancePlanMaint, sClassName:="iPMBFinancePlanMaint.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFinancePlanMaint = temp_oFinancePlanMaint

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get an instance of iPMBFinancePlanMaint.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAccountCode", excep:=New Exception(Information.Err().Description))
                Exit Sub
            End If

            ' Set the navigation keys

            ReDim vKeyArray(1, 2)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameFinancePlanCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = Conversion.Val(CStr(vResultArray(0, 0)))

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameFinancePlanVersion

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = Conversion.Val(CStr(vResultArray(1, 0)))

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = "FinancePlanEditAuthority"
            If m_sCallingAppName <> "iPMWrkComponentStarter" Then
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = False
            Else
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = True
            End If

            m_lReturn = oFinancePlanMaint.SetKeys(vKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set keys of iPMBFinancePlanMaint.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAccountCode", excep:=New Exception(Information.Err().Description))
                Exit Sub
            End If

            ' Start the interface

            m_lReturn = oFinancePlanMaint.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start iPMBFinancePlanMaint.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAccountCode", excep:=New Exception(Information.Err().Description))
                Exit Sub
            End If

            oFinancePlanMaint.Dispose()
            oFinancePlanMaint = Nothing

        Catch excep As System.Exception

            ' Error Section.
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to go to instalment plan", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuInstalmentPlan_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try
    End Sub

    Public Sub mnuReportOnThis_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuReportOnThis.Click
        ' CJB 30/03/2004 Created - Process the right mouse button Do Not Report menu click

        Const ACMethod As String = "mnuReportOnThis_Click"

        m_lReturn = ProcessDoNotReport()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error flagging the item to report upon")
        End If

    End Sub

    Public Sub mnuTransactionWriteOff_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTransactionWriteOff.Click
        ' AMB 13/02/2003: PS220 - added transaction write-off functionality

        m_lReturn = DoTransactionWriteOff()

    End Sub

    Public Sub mnuTransfer_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTransfer.Click
        'TR - Transfer the Trans Detail
        ProcessTransfer()
    End Sub
    ''' <summary>
    ''' OpenAllocateForm
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Opens the Allocation Form passing through the selected items</remarks>
    Private Function OpenAllocateForm() As Integer
        Const kMethodName As String = "OpenAllocateForm"
        Dim nResult As Integer = 0


        nResult = gPMConstants.PMEReturnCode.PMTrue

        Dim nSelectedItem As Integer
        Dim result As Integer = 0
        Dim lSelectedItem As Integer
        Dim vAllocateArray(,) As Object
        Dim bNothingToDo As Boolean
        Dim bCredits As Boolean
        Dim bDebits As Boolean
        Dim bAlreadyLocked As Boolean
        Dim bTransactionOutstanding As Boolean
        Dim bMultiCurrency As Boolean
        Dim cWriteOff As Decimal
        Dim cRate_to_use As Decimal
        Dim cRateToUse As Decimal
        Dim dConversiondate As Date
        Dim nWriteOffRow As Integer
        Dim nAccountID As Integer
        Dim nPrevAccountID As Integer
        Dim vTransdetails(,) As Object
        Dim bFound As Boolean
        Dim l_cExchangeRate As Decimal
        Dim l_bExchangeRateDiff As Boolean
        Dim nSelectedCount As Integer
        Dim bDDinMatch As Boolean
        Dim vAllTrans(,) As Object
        Dim bIsThirdPartyScheme As Boolean
        Dim nAllTransLowerRow As Integer
        Dim nAllTransUpperRow As Integer
        Dim bInstalments As Boolean
        Dim bFirstLoop As Boolean
        Dim cMatchTotal As Decimal
        Dim oAllocation As Object
        Dim frm As Form
        Dim vValue As Object
        Dim bTransdetailIds As Boolean
        Dim lCashListCount As Long
        Dim bIsOtherTransaction As Boolean
        Dim bIsSingleCashListItemAllocation As Boolean
        Dim sOptionValue As String
        Dim bThirdParty As Boolean

        bThirdParty = False
        bNothingToDo = False
        bCredits = False
        bDebits = False
        nAccountID = 0
        nPrevAccountID = 0

        nSelectedCount = -1
        bDDinMatch = False
        Try

            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=kSysOptSingleCashReceipt, r_sOptionValue:=sOptionValue)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError("OpenAllocateForm", "GetSystemOption Single Cash Reciept/Payment per Allocation Check Failed", vbObjectError)
            End If

            If sOptionValue = "1" Then
                bIsSingleCashListItemAllocation = True
            Else
                bIsSingleCashListItemAllocation = False
            End If

            Dim sDocRef As String = ""
            Dim bFoundSameDocRef As Boolean
            Dim lCountRowsSel As Integer
            Dim nPendingApproval As Integer
            Dim sMessageDesc As String
            Dim nIsDebitOrderTransdetail As Integer
            For lCnt As Integer = 1 To lvwSearchResults.Items.Count
                If lvwSearchResults.Items.Item(lCnt - 1).Checked Then
                    lCountRowsSel += 1
                    If sDocRef <> "" Then
                        If ListViewHelper.GetListViewSubItem(lvwSearchResults.Items.Item(lCnt - 1), ACListDocumentRef).Text <> sDocRef Then
                            bFoundSameDocRef = True
                            Exit For
                        End If
                    Else
                        sDocRef = ListViewHelper.GetListViewSubItem(lvwSearchResults.Items.Item(lCnt - 1), ACListDocumentRef).Text
                    End If
                End If
            Next

            'If lCountRowsSel > 1 And Not bFoundSameDocRef Then
            '    MessageBox.Show("Allocation of same Document Ref not allowed to be manually " & _
            '                    "allocated.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '    Return result
            'End If

            For iCnt As Integer = 0 To lvwSearchResults.Items.Count - 1
                If lvwSearchResults.Items.Item(iCnt).Checked = True Then
                    sDocRef = ListViewHelper.GetListViewSubItem(lvwSearchResults.Items.Item(iCnt), ACListDocumentRef).Text
                    nPendingApproval = m_vSearchData(kPendingApproval, iCnt)
                    If m_vSearchData(kIsDebitOrderTransDetail, iCnt) <> "" Then
                        nIsDebitOrderTransdetail = m_vSearchData(kIsDebitOrderTransDetail, iCnt)
                    End If
                    If nPendingApproval = 1 Then
                        sMessageDesc = "Payment of Document Ref # " & sDocRef & " is gone for Approval. So it can not be allocated."
                        Call MsgBox(sMessageDesc, vbOKOnly + vbExclamation, "Invalid selection")
                        Exit Function
                    ElseIf nIsDebitOrderTransdetail = 1 Then
                        sMessageDesc = "One or more of the selected transactions are being managed by the contract system and may not be manually allocated."
                        Call MsgBox(sMessageDesc, vbOKOnly + vbExclamation, "Invalid selection")
                        Exit Function
                    End If
                End If
            Next
            If bIsSingleCashListItemAllocation Then
                For lCnt As Integer = 1 To lvwSearchResults.Items.Count
                    If lvwSearchResults.Items.Item(lCnt - 1).Checked Then
                        sDocRef = ListViewHelper.GetListViewSubItem(lvwSearchResults.Items.Item(lCnt - 1), ACListDocumentRef).Text
                        If Strings.Left(sDocRef, 3) = "SRP" Or Strings.Left(sDocRef, 3) = "SPY" Then
                            lCashListCount = lCashListCount + 1
                        Else
                            bIsOtherTransaction = True
                        End If
                    End If
                Next
                If lCashListCount > 1 And bIsOtherTransaction Then
                    MsgBox("You can only allocate 1 SRP or 1 SPY in a single allocation with other transaction types", vbOKOnly)
                    Exit Function
                End If
            End If

            m_lReturn = BuildOSTransactionsCollection()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="BuildOSTransactionsCollection Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAllocate_Click")
                Return nResult
            End If
            If m_bRollup Then
                If GetAllTransForSelectedDocuments(r_vAllTrans:=vAllTrans, v_bOrderBySpare:=True, r_bInstalments:=bInstalments, r_bIsThirdPartyScheme:=bIsThirdPartyScheme) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", OpenAllocateForm, " + "GetAllTransForSelectedDocuments " & "failed")
                End If

                ' KG 02/09/03
                ' CQ2025 - Disallow allocation of instalments
                m_lReturn = g_oObjectManager.GetInstance(oObject:=oAllocation, sClassName:="bACTAllocation.Form", vInstanceManager:=PMGetViaClientManager)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                End If

                'Defect TFS 6444
                For Each oListItem As ListViewItem In lvwSearchResults.Items
                    If oListItem.Selected Then
                        If ChkDocTypeIsInstalments(ListViewHelper.GetListViewSubItem(oListItem, ACListDocumentRef).Text.Substring(0, 3)) Then
                            MessageBox.Show("Instalment transactions are handled automatically by Sirius. They are " &
                                            "not allowed to be manually allocated.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return result
                        End If
                    End If
                Next oListItem
                oAllocation = Nothing

                bFirstLoop = True

                If Information.IsArray(vAllTrans) Then

                    'Size the selected trans detail array to accept all the transactions found

                    nAllTransLowerRow = vAllTrans.GetLowerBound(klRowDimension - 1)

                    nAllTransUpperRow = vAllTrans.GetUpperBound(klRowDimension - 1)

                    'Process the 'expanded' transactions
                    For lCount As Integer = nAllTransLowerRow To nAllTransUpperRow

                        'First check to see if the transaction is still outstanding

                        m_lReturn = m_cOSTransactions.Item(v_vKey:=CStr(vAllTrans(ACITransDetailId, lCount)), r_vExists:=bTransactionOutstanding)

                        If bTransactionOutstanding Then
                            If Not bFirstLoop Then
                                ReDim Preserve vAllocateArray(gACTLibrary.k_ACAllocationArraySize, vAllocateArray.GetUpperBound(1) + 1)
                            Else
                                ReDim vAllocateArray(gACTLibrary.k_ACAllocationArraySize, 0)
                                bFirstLoop = False
                            End If

                            'Add the row

                            vAllocateArray(gACTLibrary.k_ACTransDetail_id, vAllocateArray.GetUpperBound(1)) = CInt(vAllTrans(ACITransDetailId, lCount))

                            vAllocateArray(gACTLibrary.k_ACDocument_Ref, vAllocateArray.GetUpperBound(1)) = CStr(vAllTrans(ACIDocumentRef, lCount)).Trim()

                            vAllocateArray(gACTLibrary.k_ACTransactionCurrencyID, vAllocateArray.GetUpperBound(1)) = vAllTrans(ACICurrencyID, lCount)

                            vAllocateArray(gACTLibrary.k_ACTransactionCurrency, vAllocateArray.GetUpperBound(1)) = CStr(vAllTrans(ACICurrencyText, lCount)).Trim()

                            vAllocateArray(gACTLibrary.k_ACTransactionAmount, vAllocateArray.GetUpperBound(1)) = vAllTrans(ACICurrencyAmount, lCount)

                            vAllocateArray(gACTLibrary.k_ACBaseCurrencyID, vAllocateArray.GetUpperBound(1)) = vAllTrans(ACIAmountCurrencyID, lCount)

                            vAllocateArray(gACTLibrary.k_ACBaseCurrency, vAllocateArray.GetUpperBound(1)) = CStr(vAllTrans(ACIAmountCurrencyText, lCount)).Trim()

                            vAllocateArray(gACTLibrary.k_ACBaseOutstanding, vAllocateArray.GetUpperBound(1)) = CDec(vAllTrans(ACIOutstandingBaseAmount, lCount))

                            vAllocateArray(gACTLibrary.k_ACTypeCode, vAllocateArray.GetUpperBound(1)) = CStr(vAllTrans(ACIDetailTypeCode, lCount)).Trim()

                            vAllocateArray(gACTLibrary.k_ACTaxBandCode, vAllocateArray.GetUpperBound(1)) = CStr(vAllTrans(ACITaxBandCode, lCount)).Trim()

                            vAllocateArray(gACTLibrary.k_ACTTaxGroupCode, vAllocateArray.GetUpperBound(1)) = CStr(vAllTrans(ACITaxGroupCode, lCount)).Trim()

                            vAllocateArray(gACTLibrary.k_ACTAllocSequence, vAllocateArray.GetUpperBound(1)) = gPMFunctions.ToSafeLong(vAllTrans(ACIAllocSequence, lCount), 1)

                            vAllocateArray(gACTLibrary.k_ACTAllocRule, vAllocateArray.GetUpperBound(1)) = gPMFunctions.ToSafeLong(vAllTrans(ACIAllocRule, lCount), 1)

                            'User cannot choose a fully allocated row

                            If CDbl(vAllocateArray(gACTLibrary.k_ACBaseOutstanding, vAllocateArray.GetUpperBound(1))) = 0 Then
                                MessageBox.Show("Fully allocated transactions cannot be included in the allocation.", "Incorrect Selection", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Return nResult
                            End If

                            nAccountID = CInt(vAllTrans(ACIAccountId, lCount))
                            If nPrevAccountID <> 0 And nAccountID <> nPrevAccountID Then
                                MessageBox.Show("Transactions selected for allocation must belong to the same account.", "Incorrect Selection", MessageBoxButtons.OK)
                                Return nResult
                            End If
                            nPrevAccountID = nAccountID

                            If CDbl(vAllocateArray(gACTLibrary.k_ACTransactionAmount, vAllocateArray.GetUpperBound(1))) < 0 Then
                                bCredits = True
                            Else
                                bDebits = True
                            End If
                        End If
                    Next lCount

                    If Not IsNothing(vAllocateArray) Then
                        bPMFunc.TransposeArray(vAllocateArray)
                    Else
                        MessageBox.Show("Fully allocated transactions cannot be included in the allocation.", "Incorrect Selection", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return result
                    End If
                End If
            Else
                ReDim vAllocateArray(lvwSearchResults.CheckedItems.Count - 1, gACTLibrary.k_ACAllocationArraySize)
                For lCount As Integer = 1 To lvwSearchResults.Items.Count
                    If lvwSearchResults.Items.Item(lCount - 1).Checked Then

                        nSelectedCount += 1

                        nSelectedItem = Convert.ToString(lvwSearchResults.Items.Item(lCount - 1).Tag)

                        'First check to see if the transaction is still outstanding
                        m_lReturn = m_cOSTransactions.Item(v_vKey:=CStr(m_vSearchData(ACITransDetailId, nSelectedItem)), r_vExists:=bTransactionOutstanding)

                        If Not bTransactionOutstanding Then
                            MessageBox.Show("Transaction already allocated please refresh the search details", "Transaction has changed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return nResult
                        End If

                        m_lReturn = g_oObjectManager.GetInstance(oObject:=oAllocation, sClassName:="bACTAllocation.Form", vInstanceManager:=PMGetViaClientManager)
                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        End If
                        'Defect TFS 6444
                        If ChkDocTypeIsInstalments(ListViewHelper.GetListViewSubItem(lvwSearchResults.Items.Item(lCount - 1), ACListDocumentRef).Text.Substring(0, 3)) = True And
                            CheckIsLinkedToThirdPartyScheme(ListViewHelper.GetListViewSubItem(lvwSearchResults.Items.Item(lCount - 1), ACListDocumentRef).Text) = False Then
                            MessageBox.Show("Instalment transactions are handled automatically by Sirius. They are " &
                                            "not allowed to be manually allocated.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return nResult
                        Else
                            bThirdParty = True
                        End If

                        oAllocation = Nothing

                        'Add the row

                        vAllocateArray(nSelectedCount, gACTLibrary.k_ACTransDetail_id) = CInt(m_vSearchData(ACITransDetailId, nSelectedItem))

                        vAllocateArray(nSelectedCount, gACTLibrary.k_ACDocument_Ref) = CStr(m_vSearchData(ACIDocumentRef, nSelectedItem)).Trim()

                        vAllocateArray(nSelectedCount, gACTLibrary.k_ACTransactionCurrencyID) = m_vSearchData(ACICurrencyID, nSelectedItem)

                        vAllocateArray(nSelectedCount, gACTLibrary.k_ACTransactionCurrency) = CStr(m_vSearchData(ACICurrencyText, nSelectedItem)).Trim()

                        vAllocateArray(nSelectedCount, gACTLibrary.k_ACTransactionAmount) = m_vSearchData(ACICurrencyAmount, nSelectedItem)

                        vAllocateArray(nSelectedCount, gACTLibrary.k_ACBaseCurrencyID) = m_vSearchData(ACIAmountCurrencyID, nSelectedItem)

                        vAllocateArray(nSelectedCount, gACTLibrary.k_ACBaseCurrency) = CStr(m_vSearchData(ACIAmountCurrencyText, nSelectedItem)).Trim()

                        vAllocateArray(nSelectedCount, gACTLibrary.k_ACBaseOutstanding) = CDec(m_vSearchData(ACIOutstandingBaseAmount, nSelectedItem))

                        vAllocateArray(nSelectedCount, gACTLibrary.k_ACTypeCode) = CStr(m_vSearchData(ACIDetailTypeCode, nSelectedItem)).Trim()

                        vAllocateArray(nSelectedCount, gACTLibrary.k_ACTaxBandCode) = CStr(m_vSearchData(ACITaxBandCode, nSelectedItem)).Trim()

                        vAllocateArray(nSelectedCount, gACTLibrary.k_ACTTaxGroupCode) = CStr(m_vSearchData(ACITaxGroupCode, nSelectedItem)).Trim()

                        vAllocateArray(nSelectedCount, gACTLibrary.k_ACTAllocSequence) = m_vSearchData(ACIAllocSequence, nSelectedItem)

                        vAllocateArray(nSelectedCount, gACTLibrary.k_ACTAllocRule) = m_vSearchData(ACIAllocRule, nSelectedItem)

                        'User cannot choose a fully allocated row

                        If CDbl(vAllocateArray(nSelectedCount, gACTLibrary.k_ACBaseOutstanding)) = 0 And CDbl(vAllocateArray(nSelectedCount, gACTLibrary.k_ACTransactionAmount)) <> 0 Then
                            MessageBox.Show("Fully allocated transactions cannot be included in the allocation.", "Incorrect Selection", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return nResult
                        End If

                        nAccountID = CInt(m_vSearchData(ACIAccountId, nSelectedItem))
                        If nPrevAccountID <> 0 And nAccountID <> nPrevAccountID Then
                            MessageBox.Show("Transactions selected for allocation must belong to the same account.", "Incorrect Selection", MessageBoxButtons.OK)
                            Return nResult
                        End If
                        nPrevAccountID = nAccountID

                        If m_sLedgerName = ACCOUNT_LEDGER_CLIENT Then

                            If CDbl(m_vSearchData(ACIDocumentTypeId, nSelectedItem)) = 33 Or CDbl(m_vSearchData(ACIDocumentTypeId, nSelectedItem)) = 34 Then
                                bDDinMatch = True
                            End If

                            If bDDinMatch And nSelectedCount > 2 Then
                                MessageBox.Show("Direct Debit transactions can only be matched to one other " &
                                            "transaction.", "Incorrect Selection", MessageBoxButtons.OK)
                                Return nResult
                            End If
                        End If

                        If CDbl(vAllocateArray(nSelectedCount, gACTLibrary.k_ACTransactionAmount)) < 0 Then
                            bCredits = True
                        Else
                            bDebits = True
                        End If

                        m_lReturn = g_oBusiness.GetMarkTransdetails(v_lDocumentID:=m_vSearchData(ACIDocDocumentID, nSelectedItem), v_lTransDetailId:=m_vSearchData(ACITransDetailId, nSelectedItem), v_lAccountId:=m_vSearchData(ACIAccountId, nSelectedItem), r_bTransdetailIds:=bTransdetailIds)

                        If bTransdetailIds Then
                            MessageBox.Show("Transaction/s selected for allocation are marked in Insurer/Agent Payment or Subagent settlement. To proceed please unmark the transaction.", "Incorrect Selection", MessageBoxButtons.OK)
                            Return nResult
                        End If

                        Dim sDocumentRef As String = ""
                        sDocumentRef = gPMFunctions.ToSafeString(m_vSearchData(ACIDocumentRef, lSelectedItem)).Trim().Substring(0, 3)
                        If Not bThirdParty Then
                            If sDocumentRef = "IND" Or sDocumentRef = "IED" Or sDocumentRef = "IRD" Or sDocumentRef = "INC" Then
                                MessageBox.Show("Instalment transactions cannot be selected for allocation where the Receipt type is 'Premium Debt'.  Use 'Instalment Debt' receipt types to allocate receipts to instalment plans", "Incorrect Selection", MessageBoxButtons.OK)
                                Return gPMConstants.PMEReturnCode.PMCancel
                            End If
                        End If
                    End If
                Next lCount
            End If

            'Default to zero if tag empty to prevent type mismatch.
            'You must fully match all transaction lines for an individual document reference on insurer accounts.
            'Except when this product option is set
            iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTAllowPartialAllocationOnInsurer, 1, vValue)
            If gPMFunctions.ToSafeInteger(vValue, 0) <> 1 Then
                If g_oBusiness.IsInsurer(IIf(Convert.ToString(txtAccountCode.Tag) <> "", (Convert.ToString(txtAccountCode.Tag)), ("0"))) Or g_oBusiness.IsAgent(IIf(Convert.ToString(txtAccountCode.Tag) <> "", (Convert.ToString(txtAccountCode.Tag)), ("0"))) Then
                    cMatchTotal = 0
                    For lCount As Integer = 0 To vAllocateArray.GetUpperBound(0)
                        cMatchTotal += CDec(vAllocateArray(lCount, gACTLibrary.k_ACBaseOutstanding))
                        m_lReturn = g_oBusiness.GetAllTransdetails(vAllocateArray(lCount, gACTLibrary.k_ACTransDetail_id), vTransdetails)
                        For lLoopA As Integer = vTransdetails.GetLowerBound(1) To vTransdetails.GetUpperBound(1)
                            bFound = False
                            For lLoopB As Integer = 0 To vAllocateArray.GetUpperBound(0)

                                If CInt(vTransdetails(0, lLoopA)) = CDbl(vAllocateArray(lLoopB, gACTLibrary.k_ACTransDetail_id)) Then
                                    bFound = True
                                    Exit For
                                End If
                            Next lLoopB
                            If bFound Then
                                Exit For
                            End If
                        Next lLoopA
                        If Not bFound Then
                            Exit For
                        End If
                    Next lCount
                    If cMatchTotal <> 0 Then
                        bFound = False
                    End If
                    If Not bFound Then
                        MessageBox.Show("You must fully match all transaction lines for an individual document reference on insurer and agent accounts.", "Incorrect Allocation", MessageBoxButtons.OK)
                    End If
                End If
            End If
            If vAllocateArray Is Nothing Then
                bNothingToDo = True
            ElseIf vAllocateArray.GetUpperBound(0) = 0 Then
                bNothingToDo = True
            Else
                bNothingToDo = False
            End If
            If bNothingToDo Then
                MessageBox.Show("You need to select 2 or more transactions to perform an allocation.", "Incorrect Selection", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf Not bCredits Or Not bDebits Then
                MessageBox.Show("You must select a mix of credits and debits.", "Incorrect Selection", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                If m_oAllocation Is Nothing Then
                    Dim temp_m_oAllocation As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_m_oAllocation, sClassName:="iACTAllocation.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                    m_oAllocation = temp_m_oAllocation
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to create instance of iACTAllocation.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="AllocateCashListItem")
                        Return nResult
                    End If
                End If

                'Call the Allocation UI

                m_oAllocation.AllocationArray = vAllocateArray
                m_oAllocation.CallingAppName = "iACTFindTransaction"
                m_oAllocation.AccountID = gPMFunctions.ToSafeLong(Convert.ToString(txtAccountCode.Tag))
                m_oAllocation.CompanyID = g_iSourceID
                nResult = m_oAllocation.Start()

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nResult
                End If

                If m_oAllocation.Status = gPMConstants.PMEReturnCode.PMOK Then
                    'Refresh the search results
                    m_lReturn = m_oGeneral.GetInterfaceDetails()
                    ' Set the focus.
                    lvwSearchResults.Focus()

                    'Solution

                    Dim temp_m_oBusiness As Object
                    Dim m_oBusiness As bACTFindTransaction.Business

                    m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTFindTransaction.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                    m_oBusiness = temp_m_oBusiness
                    Dim nTransDetailId As Integer = 0
                    For nCount As Integer = 0 To vAllocateArray.GetUpperBound(0)
                        If vAllocateArray(nCount, 1).ToString().Contains("SPY") Then
                            nTransDetailId = vAllocateArray(nCount, 0)
                            Exit For
                        End If
                    Next
                    If nTransDetailId <> 0 Then
                        m_lReturn = m_oBusiness.UpdateCashListItemForAllocationStatus(v_iTransDetailId:=nTransDetailId)
                    End If

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to process the interface.
                        nResult = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error Message
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update cashlistitem table for allocationstatus", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenAllocateForm")
                        Return nResult
                    End If
                    m_oBusiness = Nothing
                End If
            End If


            nResult = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " function failed",
                               vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, sUsername:=g_oObjectManager.UserName, excep:=ex)


            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try

        Return nResult
    End Function

    ' ***************************************************************** '
    '
    ' Name: PopulateAccountCode
    '
    ' Description:
    '
    ' History: 05/01/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function PopulateAccountCode(Optional ByRef r_lAccountID As Integer = 0, Optional ByRef bInsuredAccount As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim sAccountCode As String = ""
        Dim lAccountID As Integer
        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of Find Account if needed
            If m_oFindAccount Is Nothing Then

                Dim temp_m_oFindAccount As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oFindAccount, sClassName:="iACTFindAccount.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oFindAccount = temp_m_oFindAccount
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get an instance of iACTFindAccount.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAccountCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

                End If

            End If

            ' Get the account code
            If bInsuredAccount Then
                sAccountCode = txtInsuredAccountCode.Text
            Else
                sAccountCode = txtAccountCode.Text
            End If

            ReDim vKeyArray(1, 3)

            ' Set the keys

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyAllowStoppedAccounts

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_bAllowStopped

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameShortCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = sAccountCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameAgentCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_vUserPartyArray(kUserAgentCnt, 0)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.kACTKeyNameCallingComponent

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_kComponentName

            m_lReturn = m_oFindAccount.SetKeys(vKeyArray)

            'Setting the NotEditable property of FindAccount to avoid editing (PN 14472)

            m_oFindAccount.NotEditable = True

            m_lReturn = m_oFindAccount.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get an instance of iACTFindAccount.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAccountCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result

            End If

            ' If they didn't cancel then store the new data

            If m_oFindAccount.Status <> gPMConstants.PMEReturnCode.PMCancel Then

                sAccountCode = m_oFindAccount.ShortCode

                lAccountID = m_oFindAccount.AccountID
                If bInsuredAccount Then
                    txtInsuredAccountCode.Text = sAccountCode
                    txtInsuredAccountCode.Tag = CStr(lAccountID)
                Else
                    txtAccountCode.Text = sAccountCode
                    txtAccountCode.Tag = CStr(lAccountID)
                End If

                If Not False Then
                    r_lAccountID = lAccountID
                End If

            End If

            ' Remove the instance as it's not working a second time...

            m_oFindAccount.Dispose()
            m_oFindAccount = Nothing
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateAccountCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAccountCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'EK 020300
    ' ***************************************************************** '
    ' Name: ProcessReversal
    '
    ' Description: Reverses the Selected Document
    '
    ' ***************************************************************** '
    Private Function ProcessReversal() As Integer
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Const sMethodName As String = "ProcessReversal"

        Dim oDocumentReversal As bACTDocumentReversal.Business
        Dim oPMLookup As BPMLOOKUP.Business = Nothing
        Dim nCashlistitemReverseReasonID As Integer
        Dim nCashListItemID As Integer

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        If oDocumentReversal Is Nothing Then
            Dim temp_oDocumentReversal As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oDocumentReversal, "bACTDocumentReversal.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oDocumentReversal = temp_oDocumentReversal
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + sMethodName + ", g_oObjectManager.GetInstance failed")
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "sClassName:=bACTDocumentReversal.Business", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        For Each oSelectedItem As ListViewItem In lvwSearchResults.Items
            If (oSelectedItem IsNot Nothing AndAlso oSelectedItem.Checked) Then
                m_lTransdetailID = m_vSearchData(ACITransDetailId, Convert.ToString(oSelectedItem.Tag))
                nCashListItemID = m_vSearchData(kCashListItemID, Convert.ToString(oSelectedItem.Tag))
            End If
        Next

        oDocumentReversal.TransDetailId = m_lTransdetailID
        Dim sFailureReason As String = ""
        m_lReturn = oDocumentReversal.Start(r_sFailureReason:=sFailureReason)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = gPMConstants.PMEReturnCode.PMFalse
             If sFailureReason <> "" Then
                MsgBox(sFailureReason.ToString)
                Return gPMConstants.PMEReturnCode.PMOK
            Else
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + sMethodName + ", oDocumentReversal.Start failed")
                gPMFunctions.RaiseError("oDocumentReversal.Start", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        If oPMLookup Is Nothing Then
            Dim temp_oPMLookup As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLookup, "bPMLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLookup = temp_oPMLookup
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + sMethodName + ", g_oObjectManager.GetInstance failed")
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "sClassName:=bACTDocumentReversal.Business", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        m_lReturn = oPMLookup.GetEffectiveIDFromCode(v_sTableName:="CashListItem_Reverse_Reason", v_sCode:="OTHER", v_dtEffectiveDate:=DateTime.Now, r_lID:=nCashlistitemReverseReasonID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = gPMConstants.PMEReturnCode.PMFalse
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + sMethodName + ", oPMLookup.GetEffectiveIDFromCode failed")
            gPMFunctions.RaiseError("oPMLookup.GetEffectiveIDFromCode", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ''if the reversal is successful we need to call the below method to keep in sync with the WebMethod.
        m_lReturn = g_oBusiness.SetCashListItemFlags(
        nCashlistItemID:=nCashListItemID,
        dtReversedDate:=Date.Now,
        nCashlistitemReversePMuserID:=g_iUserID,
        nCashlistitemReverseReasonID:=nCashlistitemReverseReasonID,
        nCashlistitemReversalTransdetailID:=m_lTransdetailID,
        nIsReceiptReversal:=1)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = gPMConstants.PMEReturnCode.PMFalse
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + sMethodName + ", g_oBusiness.SetCashListItemFlags failed")
            gPMFunctions.RaiseError("g_oBusiness.SetCashListItemFlags", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        If Not (oDocumentReversal Is Nothing) Then
            oDocumentReversal.Dispose()
            oDocumentReversal = Nothing
        End If
        If Not (oPMLookup Is Nothing) Then
            oPMLookup.Dispose()
            oPMLookup = Nothing
        End If
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        ' This is for debugging only

        Return nResult
    End Function

    Private Function ProcessReversalOld() As Integer
        Dim result As Integer = 0
        Dim bACTDocumentReversal As Object

        Const kMethodName As String = "ProcessReversal"

        Dim oDocumentReversal As bACTDocumentReversal.Business
        Dim lSelectedItem As Integer
        Dim vCheckResults(,) As Object
        Dim eResult As DialogResult

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            If oDocumentReversal Is Nothing Then
                Dim temp_oDocumentReversal As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oDocumentReversal, "bACTDocumentReversal.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oDocumentReversal = temp_oDocumentReversal
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "sClassName:=bACTDocumentReversal.Business", gPMConstants.PMELogLevel.PMLogError)
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If (lvwSearchResults.CheckedItems.Count > 0) Then
                lSelectedItem = Convert.ToString(lvwSearchResults.Items.Item(lvwSearchResults.CheckedItems(0).Index).Tag)
            End If

            m_lTransdetailID = CInt(m_vSearchData(ACITransDetailId, lSelectedItem))

            If CStr(m_vSearchData(ACISpare, lSelectedItem)).Substring(0, 8) = "INT COMM" Or CStr(m_vSearchData(ACISpare, lSelectedItem)).Substring(0, 8) = "INT ADJ " Then

                m_bIntroducer = True

            End If

            'Pass in details of transaction to be reversed

            oDocumentReversal.Introducer = m_bIntroducer

            oDocumentReversal.TransDetailId = m_lTransdetailID

            oDocumentReversal.DocumentId = m_lDocumentId

            'Check to see if it is ok to reverse the transaction

            m_lReturn = oDocumentReversal.CheckTransactionForReversal(v_bOnlyCheckForInvalidTransaction:=False, r_vCheckResults:=vCheckResults)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CheckTransactionForReversal", "Failed", gPMConstants.PMELogLevel.PMLogError)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vCheckResults) Then
                'Display the results from the check

                For lLoop As Integer = 0 To vCheckResults.GetUpperBound(1)

                    Select Case CInt(vCheckResults(0, lLoop))
                        Case 1

                            Interaction.MsgBox(CStr(vCheckResults(1, lLoop)), CStr(MsgBoxStyle.OkOnly) & CStr(MsgBoxStyle.Exclamation), "Invalid Selection")
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Return result
                        Case 2

                            eResult = MessageBox.Show(CStr(vCheckResults(1, lLoop)), "Warning", MessageBoxButtons.OKCancel)
                            If eResult = System.Windows.Forms.DialogResult.Cancel Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                Return result
                            End If
                        Case 3

                            Interaction.MsgBox(CStr(vCheckResults(1, lLoop)), CStr(MsgBoxStyle.OkOnly) & CStr(MsgBoxStyle.Information), "Reminder")
                    End Select
                Next
            End If

            'Transaction is perfectly valid to be reversed, so reverse it

            m_lReturn = oDocumentReversal.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oDocumentReversal.Start", "Failed", gPMConstants.PMELogLevel.PMLogError)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            MessageBox.Show("The transaction has been successfully reversed", "Reversal Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            result = gPMConstants.PMEReturnCode.PMTrue



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here
            If Not (oDocumentReversal Is Nothing) Then

                oDocumentReversal.Dispose()
                oDocumentReversal = Nothing
            End If
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



            ' This is for debugging only



        End Try
        Return result
    End Function

    Private Function ProcessTransactionWriteOffs() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ProcessTransactionWriteOffs
        ' PURPOSE: Process the write-offs after all checks have been done
        ' AUTHOR: Andrew Bibby
        ' DATE: 12/02/2003, 17:17
        ' RETURNS: PMTrue for success
        ' CHANGES: AMB 12/02/2003: PS220 - Created for IAG 220 Manage Debtors
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim lSelectedItemTag, lUserGroupID As Integer
        Dim sUserGroupName As String = ""
        Dim lAccountID As Integer
        Dim sAccountCode As String = ""
        Dim vKeyArray(,) As Object
        Dim sTaskDesc As String = ""
        Dim lTaskInstanceCnt As Integer
        Dim sCurrDocRef, sPrevDocRef As String

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' < loop through documents >
            For Each oSelectedItem As ListViewItem In lvwSearchResults.Items

                If (oSelectedItem IsNot Nothing AndAlso oSelectedItem.Checked) Then

                    ' the tag is the index to the m_vSearchData array

                    lSelectedItemTag = Convert.ToString(oSelectedItem.Tag)

                    ' we only want to do this processing for each document, not all selected items
                    sCurrDocRef = ListViewHelper.GetListViewSubItem(oSelectedItem, ACListDocumentRef).Text.Trim().ToUpper()
                    If sCurrDocRef <> sPrevDocRef Then

                        ' open the event form
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
                        m_lReturn = DisplayNoteEntry(v_lInsuranceFileCnt:=CInt(m_vSearchData(ACIDocInsuranceFileCnt, lSelectedItemTag)), v_lAccountId:=CInt(m_vSearchData(ACIAccountId, lSelectedItemTag)))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                                Return result
                            Else
                                DisplayMessage(ACShowNoteEntryTitle, ACShowNoteEntry, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
                                Return result
                            End If
                        End If
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                        ' prompt the user for the debtor_user_group_type.code
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
                        m_lReturn = SelectUserGroup(v_sDebtorUserGroupTypeCode:=DEBTOR_USER_GROUP_TYPE_CODE_BAD_DEBT, r_lUserGroupID:=lUserGroupID, r_sUserGroupName:=sUserGroupName)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                                Return result
                            Else
                                DisplayMessage(ACSelectUserGroupTitle, ACSelectUserGroup, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
                                Return result
                            End If
                        End If
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                        ' create an auditset record
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                        m_lReturn = g_oBusiness.AddAuditSet(v_lDocumentID:=m_vSearchData(ACIDocDocumentID, lSelectedItemTag), v_lAuditSetID:=AUDITSETTYPE_ID_UNNAPPROVED_BAD_DEBT)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            DisplayMessage(ACAddAuditSetTitle, ACAddAuditSet, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
                            Return result
                        End If
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                        ' create a work manager task for this write off
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
                        If lUserGroupID Then

                            ' set the parameters
                            sAccountCode = CStr(m_vSearchData(ACIAccountShortCode, lSelectedItemTag)).Trim()
                            lAccountID = CInt(m_vSearchData(ACIAccountId, lSelectedItemTag))

                            ' get the description

                            sTaskDesc = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACIWorkManagerTaskWriteOffDesc, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                            ' populate the key array
                            ReDim vKeyArray(2, 1)

                            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

                            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = lAccountID

                            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameActionKey

                            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = ACTION_KEY_APPROVE_BAD_DEBT

                            ' make the call to create the task

                            m_lReturn = g_oBusiness.AddTaskToWorkManager(r_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt, v_sCustomer:=sAccountCode, v_sDescription:=sTaskDesc, v_dtTaskDueDate:=DateTime.Now.AddDays(7), v_sTaskCode:=CREATE_TASK_WRITEOFF_TASK_CODE, v_sTaskGroupCode:=CREATE_TASK_WRITEOFF_TASK_GROUP, v_lUserGroupID:=lUserGroupID, v_vKeyArray:=vKeyArray)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                DisplayMessage(ACSelectAddTaskTitle, ACSelectAddTask, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
                                Return result
                            End If
                        Else
                            Return result
                        End If

                        ' store the document ref so we don't process it again
                        sPrevDocRef = sCurrDocRef

                        ' tell the user we've finished
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                        DisplayMessage(ACWriteOffDoneTitle, ACWriteOffDone, MsgBoxStyle.Information + MsgBoxStyle.OkOnly)

                    End If

                End If
            Next oSelectedItem

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in ProcessTransactionWriteOffs", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionWriteOffs", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result
            End Select

        Finally

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
        Return result
    End Function

    Private Function ProcessTransactionWriteOffsRollup() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ProcessTransactionWriteOffsRollup
        ' PURPOSE: Process the write-offs after all checks have been done
        ' AUTHOR: Andrew Bibby
        ' DATE: 12/02/2003, 17:17
        ' RETURNS: PMTrue for success
        ' CHANGES: 09/06/2003 - PWC - Blagged from ProcessTransactionWriteOffs
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim lSelectedItemTag, lUserGroupID As Integer
        Dim sUserGroupName As String = ""
        Dim lAccountID As Integer
        Dim sAccountCode As String = ""
        Dim vKeyArray(,) As Object
        Dim sTaskDesc As String = ""
        Dim lTaskInstanceCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' < loop through documents >
            For Each oSelectedItem As ListViewItem In lvwSearchResults.Items

                If (oSelectedItem IsNot Nothing AndAlso oSelectedItem.Checked) Then

                    ' the tag is the index to the m_vSearchData array

                    lSelectedItemTag = Convert.ToString(oSelectedItem.Tag)

                    ' open the event form
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
                    m_lReturn = DisplayNoteEntry(v_lInsuranceFileCnt:=CInt(m_vSearchData(ACIDocInsuranceFileCnt, lSelectedItemTag)), v_lAccountId:=CInt(m_vSearchData(ACIAccountId, lSelectedItemTag)))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                            Return result
                        Else
                            DisplayMessage(ACShowNoteEntryTitle, ACShowNoteEntry, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
                            Return result
                        End If
                    End If
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    ' prompt the user for the debtor_user_group_type.code
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
                    m_lReturn = SelectUserGroup(v_sDebtorUserGroupTypeCode:=DEBTOR_USER_GROUP_TYPE_CODE_BAD_DEBT, r_lUserGroupID:=lUserGroupID, r_sUserGroupName:=sUserGroupName)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                            Return result
                        Else
                            DisplayMessage(ACSelectUserGroupTitle, ACSelectUserGroup, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
                            Return result
                        End If
                    End If
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    ' create an auditset record
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                    m_lReturn = g_oBusiness.AddAuditSet(v_lDocumentID:=m_vSearchData(ACIDocDocumentID, lSelectedItemTag), v_lAuditSetID:=AUDITSETTYPE_ID_UNNAPPROVED_BAD_DEBT)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        DisplayMessage(ACAddAuditSetTitle, ACAddAuditSet, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
                        Return result
                    End If
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    ' create a work manager task for this write off
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
                    If lUserGroupID Then

                        ' set the parameters
                        sAccountCode = CStr(m_vSearchData(ACIAccountShortCode, lSelectedItemTag)).Trim()
                        lAccountID = CInt(m_vSearchData(ACIAccountId, lSelectedItemTag))

                        ' get the description

                        sTaskDesc = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACIWorkManagerTaskWriteOffDesc, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        ' populate the key array
                        ReDim vKeyArray(2, 1)

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = lAccountID

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameActionKey

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = ACTION_KEY_APPROVE_BAD_DEBT

                        ' make the call to create the task

                        m_lReturn = g_oBusiness.AddTaskToWorkManager(r_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt, v_sCustomer:=sAccountCode, v_sDescription:=sTaskDesc, v_dtTaskDueDate:=DateTime.Now.AddDays(7), v_sTaskCode:=CREATE_TASK_WRITEOFF_TASK_CODE, v_sTaskGroupCode:=CREATE_TASK_WRITEOFF_TASK_GROUP, v_lUserGroupID:=lUserGroupID, v_vKeyArray:=vKeyArray)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            DisplayMessage(ACSelectAddTaskTitle, ACSelectAddTask, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
                            Return result
                        End If
                    Else
                        Return result
                    End If

                    ' tell the user we've finished
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    DisplayMessage(ACWriteOffDoneTitle, ACWriteOffDone, MsgBoxStyle.Information + MsgBoxStyle.OkOnly)

                End If

            Next oSelectedItem

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in ProcessTransactionWriteOffsRollup", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionWriteOffsRollup", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result
            End Select

        Finally

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
        Return result
    End Function

    '*************************************************************************
    'Name:          ProcessTransfer
    'Description:   Confirms Transfer and sets off the process
    'History:       14/02/2003 - TR - Created for TS220 Manage Debtor Accounts
    '*************************************************************************
    Private Function ProcessTransfer() As Integer

        Dim result As Integer = 0
        Dim ofrmTransferAmount As frmTransferAmount
        Dim lNewDocumentId As Integer

        Try

            'TR - Assume Success
            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Instantiate the form
            ofrmTransferAmount = New frmTransferAmount()

            'TR - Pass the Data to the form
            With ofrmTransferAmount
                .Amount = m_crSelectedTotalAmount
                'TR - Display Read-only for multiple TransDetails
                If m_lSelectedCount > 1 Then
                    .ReadOnly_Renamed = True
                    'TR - Get a Language String variable

                    .PromptMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACConfirmTransferAmount, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                Else
                    .ReadOnly_Renamed = False
                    'TR - Get a Language String variable

                    .PromptMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACTransferAmount, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    m_crSelectedTotalAmount = .Amount
                End If
                'TR - Display the form
                .ShowDialog()
                'TR - Now check the status of the form
                If .Status <> gPMConstants.PMEReturnCode.PMOK Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                Else
                    'TR - If there was a single transaction then the user may
                    'have changed the amount of the transfer
                    If m_lSelectedCount = 1 Then
                        m_crSelectedTotalAmount = .Amount

                        m_vSelectedTransDetails(eSelectedTransDetails.Amount, 0) = m_crSelectedTotalAmount
                    End If
                End If
            End With

            'TR - Destroy the objects
            ofrmTransferAmount = Nothing

            'TR - Manually create the Document

            m_lReturn = g_oBusiness.AddTransferDocument(m_crSelectedTotalAmount, g_iCompanyID, Convert.ToString(txtAccountCode.Tag), lNewDocumentId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CreateBalancingTransactions(lNewDocumentId, CInt(Conversion.Val(Convert.ToString(txtAccountCode.Tag))))

            ' KG 06/06/03 - Refresh display to prevent multiple transfers on same account balance
            ' Perform the search
            FindNow()

            Return result

        Catch excep As System.Exception

            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "ProcessTransfer Failed", ACApp, ACClass, "ProcessTransfer", Information.Err().Number, excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: PropertiesToInterface
    '
    ' Description: Updates the interface details from the property
    '              members.
    '
    ' ***************************************************************** '
    Private Function PropertiesToInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            txtInsuranceRef.Text = m_sInsuranceRef
            txtDocumentRef.Text = m_sDocumentRef
            txtTolerance.Text = CStr(0)

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ResizeInterface
    '
    ' Description: Resizes the interface controls.
    '
    ' ***************************************************************** '
    Private Function ResizeInterface() As Integer

        ' AMB 11/02/2003 - Various cosmetic issues tidied-up
        Dim result As Integer = 0
        Const BUTTON_HEIGHT_FUDGE As Integer = 720
        Const BUTTON_WIDTH_FUDGE As Integer = 1335

        Dim lResizeWidth, lResizeHeight As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lResizeWidth = CInt(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - BUTTON_WIDTH_FUDGE)
            lResizeHeight = CInt(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - BUTTON_HEIGHT_FUDGE)

            cmdFindNow.Left = VB6.TwipsToPixelsX(lResizeWidth)
            cmdNewSearch.Left = VB6.TwipsToPixelsX(lResizeWidth)
            'EK 17/10/99 Fix to re position
            cmdBalance.Left = VB6.TwipsToPixelsX(lResizeWidth)
            '
            ImgImage.Left = Me.ClientRectangle.Width - VB6.TwipsToPixelsX(990)

            'DD 20/01/2003: Fixed width
            tabMain.Width = Me.ClientRectangle.Width - VB6.TwipsToPixelsX(1650)
            tabMain.BringToFront()

            lvwSearchResults.Width = Me.ClientRectangle.Width - VB6.TwipsToPixelsX(255)
            'DC140706 PN29423 change size to fit longer currency descriptions on form header
            lvwSearchResults.Height = Me.ClientRectangle.Height - VB6.TwipsToPixelsY(5250) '4260

            'cmdHelp.Left = lResizeWidth + 105
            cmdHelp.Left = VB6.TwipsToPixelsX(lResizeWidth + 85)
            cmdHelp.Top = VB6.TwipsToPixelsY(lResizeHeight)

            cmdCancel.Left = cmdHelp.Left - VB6.TwipsToPixelsX(1100)
            cmdCancel.Top = VB6.TwipsToPixelsY(lResizeHeight)

            cmdOK.Left = cmdCancel.Left - 60 'VB6.TwipsToPixelsX(1100)
            cmdOK.Top = VB6.TwipsToPixelsY(lResizeHeight)

            '    cmdNew.Top = lresizeheight
            '    cmdEdit.Top = lresizeheight

            cmdFindAccTrans.Top = VB6.TwipsToPixelsY(lResizeHeight)
            cmdFindDocTrans.Top = VB6.TwipsToPixelsY(lResizeHeight)
            'EK 020300
            '    If (cmdNavigate.Visible = True) Then
            '        cmdNavigate.Top =lresizeheight
            cmdAllocate.Top = VB6.TwipsToPixelsY(lResizeHeight)
            cmdReverse.Top = VB6.TwipsToPixelsY(lResizeHeight)
            cmdReverseAndReplace.Top = VB6.TwipsToPixelsY(lResizeHeight)

            cmdViewAllocation.Top = VB6.TwipsToPixelsY(lResizeHeight)
            cmdViewAllocation.Top = VB6.TwipsToPixelsY(lResizeHeight)

            '29/05/2003 - PWC - 186 - Debt Rollup
            cmdExpand.Top = VB6.TwipsToPixelsY(lResizeHeight)
            cmdExpand.Left = cmdViewAllocation.Right + 5
            cmdSplitReceipt.Left = cmdOK.Left - 80
            cmdEditSplit.Left = cmdSplitReceipt.Left - 65

            Return result

        Catch

            ' Error Section.

            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    Private Function SelectUserGroup(ByVal v_sDebtorUserGroupTypeCode As String, Optional ByRef r_lUserGroupID As Integer = 0, Optional ByRef r_sUserGroupName As String = "") As Integer
        Dim result As Integer = 0
        Dim iACTManageDebtors As Object

        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: SelectUserGroup
        ' PURPOSE:  Display the iACTManageDebtors.frmInterface and
        '           return the chosen user group
        ' AUTHOR: Andrew Bibby
        ' DATE: 13/02/2003, 13:50
        ' RETURNS: PMTrue for success
        ' CHANGES: AMB 13/02/2003: PS220 - created for Manage Debtors
        ' ---------------------------------------------------------------------------

        Dim oManageDebtors As Object

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If v_sDebtorUserGroupTypeCode.Length = 0 Then
                Return result
            End If

            ' create the manage debtors interface object
            Dim temp_oManageDebtors As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oManageDebtors, sClassName:="iACTManageDebtors.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oManageDebtors = temp_oManageDebtors

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get an instance of iACTManageDebtors.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAccountCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            ' intialise the object

            m_lReturn = CType(oManageDebtors, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                Return result
            End If

            ' set the appropriate properties

            oManageDebtors.SourceID = g_iSourceID

            oManageDebtors.DebtorGroupsTypeCode = v_sDebtorUserGroupTypeCode

            ' set the process mode

            m_lReturn = oManageDebtors.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                Return result
            End If

            ' show the interface

            m_lReturn = oManageDebtors.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                Return result
            End If

            ' deal with the returns

            If oManageDebtors.Status = gPMConstants.PMEReturnCode.PMOK Then
                ' user pressed OK

                r_lUserGroupID = oManageDebtors.UserGroupID

                r_sUserGroupName = oManageDebtors.UserGroupName
            Else
                r_lUserGroupID = 0
                r_sUserGroupName = ""
                result = gPMConstants.PMEReturnCode.PMCancel
                Return result
            End If

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in SelectUserGroup", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectUserGroup", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result
            End Select

        Finally

            ' destroy the object

            oManageDebtors.Dispose()
            oManageDebtors = Nothing



        End Try
        Return result
    End Function

    Private Sub SetDrillButtons()
        If (lvwSearchResults.CheckedItems.Count = 0) Then
            cmdFindAccTrans.Enabled = False
            cmdFindDocTrans.Enabled = False
            cmdAllocate.Enabled = False
            cmdReverse.Enabled = False
            cmdViewAllocation.Enabled = False
        Else
            cmdFindAccTrans.Enabled = True
            cmdFindDocTrans.Enabled = Not m_bRollup
            cmdAllocate.Enabled = False


            cmdViewAllocation.Enabled = True
        End If

        Dim sValue As String = ""

        '<pankaj>

        m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=cEnableClientManagerEditing, r_sOptionValue:=sValue, v_iSourceID:=g_iSourceID)

        Dim bEnableClientManagerEditing As Boolean = False

        If sValue = "1" Then
            bEnableClientManagerEditing = True
        End If
        '</pankaj>

        'eck200401
        If DrillLevel = 0 Then

            If (lvwSearchResults.CheckedItems.Count > 0) Then
                cmdAllocate.Enabled = bEnableClientManagerEditing
            End If

            'Thinh Nguyen 15/04/2002 (start)

            cmdReverseAndReplace.Enabled = False

            'Thinh Nguyen 15/04/2002 (end)

        End If
        '
        '2005 Client Manager Security
        If Not g_bReverseTransactionsAuthority Then
            'cmdReverse.Enabled = False
        End If

        If Not g_bReverseReplaceTransactionsAuthority Then
            cmdReverseAndReplace.Enabled = False
        End If

        If Not g_bPerformAllocationsAuthority Then
            cmdAllocate.Enabled = False
        End If

        If txtDocumentRef.Text <> "" And txtAccountCode.Text = "" Then
            cmdFindDocTrans.Enabled = False
            'cmdAllocate.Enabled = False 'PN 42379
        ElseIf (txtDocumentRef.Text <> "" And txtAccountCode.Text <> "") Then
            'cmdAllocate.Enabled = True 'PN 42379
        ElseIf (txtAccountCode.Text <> "") Then
            cmdFindAccTrans.Enabled = False
        End If
        'PN 42379

        If m_sCallingAppName = OrionLink Then
            cmdAllocate.Enabled = bEnableClientManagerEditing
            'Start - Prakash Varghese - PN 61175
            'cmdFindDocTrans.Enabled = False PM030106 Sumit K
            'Start - Prakash Varghese - PN 61175
        Else
            If cmdFindDocTrans.Enabled Or cmdExpand.Enabled Then 'PN 42379
                cmdAllocate.Enabled = True
            End If 'PN 42379
        End If

        ' CF070898 - Allow user to drill twice (changed 0 to 1)
        If m_lDrillLevel > 1 Then
            cmdFindAccTrans.Enabled = False
            cmdFindDocTrans.Enabled = False
        End If
        'eck290800
        If m_lCashListTypeID <> 0 Then
            cmdAllocate.Enabled = False
        End If

        ' Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.3.2.4)
        If m_bInsuredAccountView Then
            cmdAllocate.Enabled = False
            cmdViewAllocation.Enabled = False
            cmdFindAccTrans.Enabled = False 'Drill A/c
        End If
        ' End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.3.2.4)

        If m_bEnhancedCaseSearching Then
            lvwSearchResults.Columns.Item(ACListCaseNumber).Width = CInt(VB6.TwipsToPixelsX(1500))
        Else
            lvwSearchResults.Columns.Item(ACListCaseNumber).Width = CInt(0)
        End If

        If CalledViaClientManager Then
            cmdAllocate.Enabled = bEnableClientManagerEditing
            cmdSplitReceipt.Enabled = False
            cmdEditSplit.Enabled = False
        End If

    End Sub

    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    '
    ' ***************************************************************** '
    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            ReDim m_ctlTabFirstLast(1, ACTabTitleCount - 1)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}
            m_ctlTabFirstLast(ACControlStart, 0) = txtAccountCode
            m_ctlTabFirstLast(ACControlEnd, 0) = cboSource

            m_ctlTabFirstLast(ACControlStart, 1) = txtDocumentRef
            m_ctlTabFirstLast(ACControlEnd, 1) = txtDateTo

            m_ctlTabFirstLast(ACControlStart, 2) = cmbCurrency
            m_ctlTabFirstLast(ACControlEnd, 2) = udTolerance

            m_ctlTabFirstLast(ACControlStart, 3) = txtInsuranceRef
            m_ctlTabFirstLast(ACControlEnd, 3) = txtAltRef '(RC) QBENZ014 'txtSpare

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' AMB 11/02/2003: PS220 - added 'flag', 'insured_name' and 'insured_account' columns
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetInterfaceDefaults"

        Dim vDefault As Object
        Dim sAppName, sResult, sBalanceChk, sSection As String
        Dim lSourceId As Integer
        Dim ACSPlitReceipt As Integer = 5091
        Dim sSplitReceiptValue As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            'sj 02/10/2002 - start
            With tbrMain

                .ImageList = ImageList1
                'changed as per the naming convention in DotNet
                'start
                .Items.Item("_Event").ImageIndex = 10
                .Items.Item("_Risk_Details").Visible = False
                .Items.Item("_Information_Checklist").Visible = False
                ' Alix - 18/02/2003 - Issue 2325
                .Items.Item("_Event").Enabled = False
                'end
            End With
            'sj 02/10/2002 - end

            ' Get what solution we're part of

            m_lReturn = g_oBusiness.GetRegSettings(sResult:=sResult, sAppName:=sAppName, sSection:=sSection, sKey:=gACTLibrary.ACTOrionSolutionValue, vDefault:=vDefault)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMOK) Or (sResult = "0") Then
                ' Default to MBP style of solution
                sResult = CStr(gACTLibrary.ACTOrionSolutionMBP)
            End If

            'm_iSolutionConfig = CInt(sResult)

            ' Get if we want multi-currency or not
            sResult = ""
            sAppName = ""

            m_lReturn = g_oBusiness.GetRegSettings(sResult:=sResult, sAppName:=sAppName, sSection:=sSection, sKey:=gACTLibrary.ACTOrionMultiCurrency, vDefault:=vDefault)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMOK) Then
                sResult = "1"
            End If

            ' CTAF 130300 - Use registry setting to decide if have any check boxes
            '               enabled by default
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=ACTFindTransBalanceOption, r_sSettingValue:=sBalanceChk)
            If sBalanceChk = "1" Then
                chkBalance.CheckState = CheckState.Checked
            Else
                chkBalance.CheckState = CheckState.Unchecked
            End If

            'DD 06/05/2003: Made default for all but drill-doc
            m_bOutstandingOnly = Not (m_sDocumentRef <> "")

            m_bEnableMultiCurrency = gPMFunctions.ToSafeBoolean(sResult)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Display all of the lookup details.
            m_lReturn = DisplayLookupDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lAllocationTransType = gACTLibrary.ACTSecondaryForAllocation Then
                m_lReturn = GetAllocationDetails()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Update the interface details with the
            ' property members.
            m_lReturn = PropertiesToInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.2.1)
            If g_sUserConfigXMLDataset <> "" Then
                m_lReturn = LoadInterfaceDisplaySettings()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "LoadInterfaceDisplaySettings Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            Else
                ' Set any other default values to the interface.

                ' {* USER DEFINED CODE (Begin) *}
                ' Set the column widths for the search list.
                'eck090500 Use calculated amount
                lvwSearchResults.Columns.Item(ACListSourceID).Width = CInt(VB6.TwipsToPixelsX(750))
                lvwSearchResults.Columns.Item(ACListFlag).Width = CInt(0)
                lvwSearchResults.Columns.Item(ACListAccountShortCode).Width = CInt(VB6.TwipsToPixelsX(600))
                'KB PN 6737 make docref wider
                'lvwSearchResults.ColumnHeaders(ACListDocumentRef + 1).Width = 600
                lvwSearchResults.Columns.Item(ACListDocumentRef).Width = CInt(VB6.TwipsToPixelsX(1200))
                lvwSearchResults.Columns.Item(ACListAccountingDate).Width = CInt(VB6.TwipsToPixelsX(1000))
                lvwSearchResults.Columns.Item(ACListDocumentDate).Width = CInt(VB6.TwipsToPixelsX(1000))

                lvwSearchResults.Columns.Item(ACListMediaType).Width = CInt(VB6.TwipsToPixelsX(900))
                lvwSearchResults.Columns.Item(ACListCurrencyAmount).Width = CInt(VB6.TwipsToPixelsX(1000))
                lvwSearchResults.Columns.Item(ACListCurrencyAmountCredit).Width = CInt(0)
                lvwSearchResults.Columns.Item(ACListPrimarySettled).Width = CInt(VB6.TwipsToPixelsX(1000))
                lvwSearchResults.Columns.Item(ACListOSCurrencyAmount).Width = CInt(VB6.TwipsToPixelsX(1000))
                lvwSearchResults.Columns.Item(ACListOSCurrencyAmountCredit).Width = CInt(0)
                lvwSearchResults.Columns.Item(ACListMatchDate).Width = CInt(VB6.TwipsToPixelsX(1000))
                lvwSearchResults.Columns.Item(ACListDocumentTypeId).Width = CInt(VB6.TwipsToPixelsX(1000))
                lvwSearchResults.Columns.Item(ACListInsuranceRef).Width = CInt(VB6.TwipsToPixelsX(1000))
                lvwSearchResults.Columns.Item(ACListOperatorName).Width = CInt(VB6.TwipsToPixelsX(1000))
                lvwSearchResults.Columns.Item(ACListPeriodName).Width = CInt(VB6.TwipsToPixelsX(1000))
                'KB PN 6138 Shrink this one
                'lvwSearchResults.ColumnHeaders(ACListDocTypeGroupId + 1).Width = 1000
                lvwSearchResults.Columns.Item(ACListDocTypeGroupId).Width = CInt(VB6.TwipsToPixelsX(400))
                lvwSearchResults.Columns.Item(ACListSpare).Width = CInt(VB6.TwipsToPixelsX(1000))
                lvwSearchResults.Columns.Item(ACListPurchaseInvoiceNo).Width = CInt(VB6.TwipsToPixelsX(1000))
                lvwSearchResults.Columns.Item(ACListPurchaseOrderNo).Width = CInt(VB6.TwipsToPixelsX(1000))
                lvwSearchResults.Columns.Item(ACListDepartment).Width = CInt(VB6.TwipsToPixelsX(1000))
                'DD 16/05/2003
                lvwSearchResults.Columns.Item(ACListPayeeName).Width = CInt(VB6.TwipsToPixelsX(1000))
                lvwSearchResults.Columns.Item(ACListInsuredName).Width = CInt(VB6.TwipsToPixelsX(1000))
                lvwSearchResults.Columns.Item(ACListInsuredAccount).Width = CInt(VB6.TwipsToPixelsX(1000))
                lvwSearchResults.Columns.Item(ACListInsuredName).Width = CInt(VB6.TwipsToPixelsX(1000))
                lvwSearchResults.Columns.Item(ACListInsuredAccount).Width = CInt(VB6.TwipsToPixelsX(1000))
            End If
            'S4BDAT004

            lvwSearchResults.Columns.Item(ACListPaymentDueDate).Width = CInt(0)

            'eck010800
            'If m_iSolutionConfig = gACTLibrary.ACTOrionSolutionSFORB Then
            lvwSearchResults.Columns.Item(ACListSpare).Width = CInt(0)
            'KIR issue 1566 - the Media Ref field is missing
            lvwSearchResults.Columns.Item(ACListDocTypeGroupId).Width = CInt(0)
            lvwSearchResults.Columns.Item(ACListSpare).Width = CInt(0)
            lvwSearchResults.Columns.Item(ACListPurchaseInvoiceNo).Width = CInt(0)
            'lvwSearchResults.Columns.Item(ACListPurchaseOrderNo).Width = CInt(0)
            lvwSearchResults.Columns.Item(ACListDepartment).Width = CInt(0)
            'eck070801 - Hide the match amount column
            lvwSearchResults.Columns.Item(ACListMatchAmount).Width = CInt(0)
            'Else
            ' lvwSearchResults.Columns.Item(ACListClaimReference).Width = CInt(0)
            'End If

            'RDT 10/04/2003 - If the hidden option is set
            If m_bDisplayDebitCredit Then
                lvwSearchResults.Columns.Item(ACListOSCurrencyAmountCredit).Width = CInt(VB6.TwipsToPixelsX(1000))
                lvwSearchResults.Columns.Item(ACListCurrencyAmountCredit).Width = CInt(VB6.TwipsToPixelsX(1000))
            End If

            'DD 05/04/2004 - Underwriting Year
            If m_bUnderwritingYear Then
                cboUnderwritingYearID.FirstItem = "(all)"
                lvwSearchResults.Columns.Item(ACListUnderwritingYear).Width = CInt(VB6.TwipsToPixelsX(1500))
                lblUnderwritingYearID.Visible = True
                cboUnderwritingYearID.Visible = True
            Else
                lvwSearchResults.Columns.Item(ACListUnderwritingYear).Width = CInt(0)
            End If

            If m_bEnhancedCaseSearching Then
                cmdCaseNumber.Visible = True
                txtCaseNumber.Visible = True
                lvwSearchResults.Columns.Item(ACListCaseNumber).Width = CInt(VB6.TwipsToPixelsX(1500))
            Else
                cmdCaseNumber.Visible = False
                txtCaseNumber.Visible = False
                lvwSearchResults.Columns.Item(ACListCaseNumber).Width = CInt(0)
            End If

            ' AMB 11/02/2003: PS220 - added 'flag', 'insured_name' and 'insured_account' columns

            ' Set Amount right aligned
            lvwSearchResults.Columns.Item(ACListCurrencyAmount).TextAlign = HorizontalAlignment.Right
            lvwSearchResults.Columns.Item(ACListCurrencyAmountCredit).TextAlign = HorizontalAlignment.Right

            ' CF150299 - Right aligned o/s currency amount
            lvwSearchResults.Columns.Item(ACListOSCurrencyAmount).TextAlign = HorizontalAlignment.Right
            lvwSearchResults.Columns.Item(ACListOSCurrencyAmountCredit).TextAlign = HorizontalAlignment.Right
            'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.2.1)

            ' AMB 11/02/2003: PS220 - added 'flag', 'insured_name' and 'insured_account' columns
            lvwSearchResults.Columns.Item(ACListInsuredName).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchResults.Columns.Item(ACListInsuredAccount).Width = CInt(VB6.TwipsToPixelsX(1000))

            If m_lAllocationTransType = gACTLibrary.ACTPrimaryForAllocation Then
                lvwSearchResults.MultiSelect = False
            End If

            g_sYes = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACYes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            g_sNo = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' CF180299
            ' if theres a batch, then its part of a roadmap so dont let the
            ' account be changed
            'eck250800 extra check on
            If (m_bIsBatch) Or (m_lCashListTypeID <> 0) Then
                'uctAccountCode.Enabled = False
                txtAccountCode.Enabled = False
                cmdAccountCode.Enabled = False
                txtAccountCode.ReadOnly = True
                'uctAccountCode.AllowStoppedAccounts = False
                m_bAllowStopped = False
                cmdNewSearch.Enabled = False
                cmdAllocate.Enabled = False

                cmdViewAllocation.Enabled = False
                ' Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.3.2.2)
            ElseIf m_bInsuredAccountView Then
                'Diasable fields for Insured Account View
                txtAccountCode.Enabled = False
                txtAccountCode.ReadOnly = True
                cmdAccountCode.Enabled = False

                txtInsuredAccountCode.Enabled = False
                txtInsuredAccountCode.ReadOnly = True
                cmdInsuredAccountCode.Enabled = False

                m_bAllowStopped = False

                'cmdNewSearch.PerformClick()
                cmdAllocate.Enabled = False
                cmdViewAllocation.Enabled = False
                ' End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.3.2.2)
            Else
                'uctAccountCode.Enabled = True
                txtAccountCode.Enabled = True
                cmdAccountCode.Enabled = True
                txtAccountCode.ReadOnly = False
                'uctAccountCode.AllowStoppedAccounts = True
                m_bAllowStopped = True
                cmdNewSearch.Enabled = True

                cmdViewAllocation.Enabled = True
                'eck200401
                If DrillLevel = 0 Then
                    cmdAllocate.Enabled = True
                End If
            End If
            '2005 Client Manager Security
            If Not g_bReverseTransactionsAuthority Then
                cmdReverse.Enabled = False
            End If

            If Not g_bReverseReplaceTransactionsAuthority Then
                cmdReverseAndReplace.Enabled = False
            End If

            If Not g_bPerformAllocationsAuthority Then
                cmdAllocate.Enabled = False
            End If

            ' CF150699
            ' Added setting to default to only outstanding transactions
            If m_bOutstandingOnly Then
                chkDisplayOutstanding.CheckState = CheckState.Checked
            Else
                chkDisplayOutstanding.CheckState = CheckState.Unchecked
            End If

            'Default to only showing latest 500 records for performance reasons
            chkDisplay500.CheckState = CheckState.Checked

            ' SFORB needs a different appearance
            'If m_iSolutionConfig = gACTLibrary.ACTOrionSolutionSFORB Then

            ' Account code lookup
            'eck220601
            '        cmdAccountCode.Left = 120
            cmdAccountCode.Width = VB6.TwipsToPixelsX(1545)
            cmdAccountCode.Left = VB6.TwipsToPixelsX(50)
            'eck290800
            cmdAccountCode.Text = "Account Code..."

            txtAccountCode.Width = VB6.TwipsToPixelsX(3105)

            lblAccountCode.Visible = False

            ' Client Name
            lblAccountName.Text = "Account Name:"

            ' Policy no
            lblInsuranceRef.Text = "Policy No:"
            'End If

            If m_bMultiLedger Then
                lSourceId = g_iSourceID
                iACTFunc.SetListIndex(cboSource, lSourceId) 'PN27383
                cboSource.Enabled = False
            End If


            cmdReverseAndReplace.Visible = False
            '        panAccountBalance.Visible = False
            '        lblAccountBalance.Visible = False

            m_lReturn = iPMFunc.GetSystemOption(CInt(ACSPlitReceipt), sSplitReceiptValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If sSplitReceiptValue = "1" Then
                SplitReceiptSystemOption = True

                cmdSplitReceipt.Visible = True

                cmdEditSplit.Visible = True
            End If

            cmdReverse.Enabled = False
            If DocumentRef <> String.Empty Then
                txtDocumentRef.Enabled = False
                txtDocumentRef.ReadOnly = True
                cmdNewSearch.Enabled = False
            End If


            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.2.3)
    Public Function LoadInterfaceDisplaySettings() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "LoadInterfaceDisplaySettings"

        Dim sColumnWidth As String = ""
        Dim oXMLColumnWidth As Object
        Dim sXMLPath As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            If g_sUserConfigXMLDataset = "" Then
                Return result
            End If
            sXMLPath = "//" & m_kRootNode & "/" & m_kComponentName & "/" & m_kFrmName & "/" & m_kGridName

            g_objDOMRootForInterfaceDisplay = New XmlDocument()
            Try
                g_objDOMRootForInterfaceDisplay.LoadXml(g_sUserConfigXMLDataset)

            Catch
            End Try

            'todolist
            'If g_objDOMRootForInterfaceDisplay.parseError.errorCode <> 0 Then

            '    
            '    'gPMFunctions.RaiseError(kMethodName, g_objDOMRootForInterfaceDisplay.parseError.Message, gPMConstants.PMELogLevel.PMLogError)
            '    gPMFunctions.RaiseError(kMethodName, g_objDOMRootForInterfaceDisplay.InnerText, gPMConstants.PMELogLevel.PMLogError)
            'End If

            If Not (g_objDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath) Is Nothing) Then
                If g_objDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath).HasChildNodes Then

                    sColumnWidth = g_objDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath & "/" & m_kColumnWidth).InnerText
                    m_iSortKey = gPMFunctions.ToSafeInteger(g_objDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath & "/" & m_kColumnSortKey).InnerText, -1)
                    m_iSortOrder = gPMFunctions.ToSafeInteger(g_objDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath & "/" & m_kColumnSortOrder).InnerText, -1)

                    oXMLColumnWidth = sColumnWidth.Split(";"c)

                    For iNoOfColumns As Integer = 0 To oXMLColumnWidth.GetUpperBound(0) - 1

                        lvwSearchResults.Columns.Item(iNoOfColumns).Width = CInt(VB6.TwipsToPixelsX(gPMFunctions.ToSafeDecimal(CStr(oXMLColumnWidth(iNoOfColumns)), 0)))
                    Next

                    If m_iSortKey > 0 Then
                        m_lReturn = SortListViewByKey()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "SortListViewByKey Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If
                End If
            End If
            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            Return result
        End Try
    End Function
    'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.2.3)

    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.2.3)
    Public Function SortListViewByKey() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SortListViewByKey"
        Dim lColumnHeaderIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lColumnHeaderIndex = m_iSortKey - 1

            With lvwSearchResults

                If lColumnHeaderIndex = ACListPeriodName Then
                    lColumnHeaderIndex = ACListPeriodEndDate
                End If

                Select Case lColumnHeaderIndex
                    Case ACListSourceID
                        ListViewHelper.SetSortedProperty(lvwSearchResults, False)
                        ListViewHelper.SetSortOrderProperty(lvwSearchResults, m_iSortOrder)
                        ListViewFunc.ListViewSortByValue(lvwSearchResults, lColumnHeaderIndex, m_iSortOrder)

                    Case ACListAccountingDate, ACListDocumentDate, ACListMatchDate, ACListPeriodEndDate
                        m_lReturn = ListViewFunc.ListViewSortByDate(v_oListView:=lvwSearchResults, v_iSourceColumn:=lColumnHeaderIndex, v_iDirection:=m_iSortOrder)

                    Case ACListCurrencyAmount, ACListOSCurrencyAmount
                        m_lReturn = ListViewSortByStringVal(v_oListView:=lvwSearchResults, v_iSourceColumn:=lColumnHeaderIndex, v_iDirection:=m_iSortOrder)

                    Case ListViewHelper.GetSortKeyProperty(lvwSearchResults)
                        ListViewHelper.SetSortOrderProperty(lvwSearchResults, m_iSortOrder)

                    Case Else
                        ListViewHelper.SetSortOrderProperty(lvwSearchResults, m_iSortOrder)
                        ListViewHelper.SetSortKeyProperty(lvwSearchResults, lColumnHeaderIndex)
                        ListViewHelper.SetSortedProperty(lvwSearchResults, True)
                End Select

            End With

            Return result

        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            Return result
        End Try
    End Function
    'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.2.3)

    Private Function ShowRefundForm(ByRef r_lMediaTypeID As Integer, ByRef r_sMediaTypeName As String, ByRef r_dRefundAmount As Double) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ShowRefundForm
        ' PURPOSE:
        ' AUTHOR: Andrew Bibby
        ' DATE: 19 Feb 2003
        ' CHANGES: AMB - PS220 Manage Debtors development - created
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim oSelectedItem As ListViewItem = Nothing
        Dim dTotalOSAmount As Double

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Only process if there is a selection made
            If (lvwSearchResults.CheckedItems.Count > 0) Then
                oSelectedItem = lvwSearchResults.CheckedItems(0)
            End If
            If oSelectedItem Is Nothing Then
                'Display standard message
                DisplayMessage(ACNoSelectionTitle, ACNoSelectionDetails, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
                Return result
            End If

            ' loop through the selected items
            For Each oSelectedItem2 As ListViewItem In lvwSearchResults.Items
                oSelectedItem = oSelectedItem2

                If (oSelectedItem IsNot Nothing AndAlso oSelectedItem.Checked) Then
                    ' work out the O/S amount
                    dTotalOSAmount += (gPMFunctions.NullToDouble(m_vSearchData(ACICurrencyAmount, Convert.ToString(oSelectedItem.Tag))) - gPMFunctions.NullToDouble(m_vSearchData(ACIMatchedCurrencyAmount, Convert.ToString(oSelectedItem.Tag))))
                End If

            Next oSelectedItem2

            ' Call the Load method to setup the interface details
            Dim tempLoadForm As frmRefund = frmRefund

            'SMJB 07/10/03: Refund amount should be the opposite of the outstanding amount
            frmRefund.RefundAmount = dTotalOSAmount * -1

            'Show form if no errors
            If frmRefund.ErrorNumber = gPMConstants.PMEReturnCode.PMTrue Then
                frmRefund.ShowDialog()
                If frmRefund.Status = gPMConstants.PMEReturnCode.PMOK Then
                    r_lMediaTypeID = frmRefund.MediaTypeID
                    r_sMediaTypeName = frmRefund.MediaTypeName
                    r_dRefundAmount = frmRefund.RefundAmount
                Else
                    r_lMediaTypeID = 0
                    r_sMediaTypeName = ""
                    r_dRefundAmount = 0
                    result = gPMConstants.PMEReturnCode.PMCancel
                    Return result
                End If
            End If

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in ShowRefundForm", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRefundForm", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError
                    Return result
            End Select

        Finally

            frmRefund.Close()


        End Try
        Return result
    End Function

    Private Sub tabMain_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMain.SelectedIndexChanged

        Try

            With tabMain
                ' Set focus to the first control on the tab.
                If SSTabHelper.GetSelectedIndex(tabMain) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    If m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMain)).Enabled Then
                        m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMain)).Focus()
                    End If
                End If

            End With

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process tabMain_Click", vApp:=ACApp, vClass:=ACClass, vMethod:="tabMain_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            tabMainPreviousTab = tabMain.SelectedIndex
        End Try

    End Sub

    'sj 02/10/2002 - start
    Private Sub tbrMain_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _Event.Click, _tbrMain_Button2.Click, _Risk_Details.Click, _tbrMain_Button4.Click, _Information_Checklist.Click
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)

        If Conversion.Val(Convert.ToString(txtAccountCode.Tag)) < 0 Then
            Exit Sub
        End If

        Select Case Button.Name
            'changed as per naming convention of Dotnet
            'Case "Event"
            Case "_Event"

                ' Alix - 18/02/2003
                ' Removed this test, we always need to reload client ID!
                'If m_lAccountKey = 0 Then

                m_lReturn = g_oBusiness.GetAccountKeyFromId(r_lAccountKey:=m_lAccountKey, v_lAccountId:=Conversion.Val(Convert.ToString(txtAccountCode.Tag)))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetAccountKeyFromId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="tbrMain_ButtonClick")
                    Exit Sub
                End If
                'End If

                ' Alix - 18/02/2003 - Issue 2189
                ' Extra parameter was causing problems, now only passing partycnt
                'm_lReturn = ShowEvents( _
                'v_lPartyCnt:=m_lAccountKey, _
                'v_lAccountKey:=m_lAccountKey)
                m_lReturn = iPMBListEvents.ShowEvents(v_lPartyCnt:=m_lAccountKey)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="ShowEvents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="tbrMain_ButtonClick")
                    Exit Sub
                End If

        End Select

    End Sub

    Private Sub txtAccountCode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountCode.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        txtAccountCode.Tag = ""

        CheckMandatoryEnable()

    End Sub

    Private Sub txtAccountCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountCode.Enter

        If Strings.Len(txtAccountCode.Text) > 0 Then
            ' CF100100 - Fixed problem
            If (Convert.ToString(txtAccountCode.Tag)) <> "" Then
                m_lSaveAccountID = CInt(Convert.ToString(txtAccountCode.Tag))

                txtAccountCode.SelectionStart = 0
                txtAccountCode.SelectionLength = Strings.Len(txtAccountCode.Text)
            End If
        End If

    End Sub

    '(RC) QBENZ014
    Private Sub txtAltRef_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAltRef.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
    End Sub

    Private Sub txtBGRef_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBGRef.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
    End Sub

    Private Sub txtBGRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBGRef.Enter
        iPMFunc.SelectText(txtBGRef)
    End Sub
    Private Sub txtCurrencyAmount_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCurrencyAmount.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
    End Sub

    Private Sub txtCurrencyAmount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCurrencyAmount.Enter
        ' Hightlight any text.
        iPMFunc.SelectText(txtCurrencyAmount)
    End Sub

    Private Sub txtDateFrom_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateFrom.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
    End Sub

    Private Sub txtDateFrom_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateFrom.Enter
        ' Hightlight any text.
        iPMFunc.SelectText(txtDateFrom)
    End Sub

    Private Sub txtDateTo_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
    End Sub

    Private Sub txtDateTo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.Enter
        ' Hightlight any text.
        iPMFunc.SelectText(txtDateTo)
    End Sub

    Private Sub txtDocumentRef_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDocumentRef.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
        cmdFindDocTrans.Enabled = False
    End Sub

    Private Sub txtDocumentRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDocumentRef.Enter
        ' Hightlight any text.
        iPMFunc.SelectText(txtDocumentRef)
    End Sub

    Private Sub txtInsuranceRef_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsuranceRef.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
    End Sub

    Private Sub txtInsuranceRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsuranceRef.Enter
        iPMFunc.SelectText(txtInsuranceRef)
    End Sub

    '<pankaj PN:39431>
    Private Sub txtCurrencyAmount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCurrencyAmount.Leave
        txtCurrencyAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, txtCurrencyAmount.Text)
    End Sub

    Private Sub txtDateFrom_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateFrom.Leave
        txtDateFrom.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, txtDateFrom.Text)
    End Sub

    Private Sub txtDateTo_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.Leave
        txtDateTo.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, txtDateTo.Text)
    End Sub

    Private Sub txtTolerance_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtTolerance.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then
            If Not (KeyAscii > 46 And KeyAscii < 58) Then
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub
    '</pankaj PN:39431>

    Private Sub txtInsuredAccountCode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsuredAccountCode.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        txtInsuredAccountCode.Tag = ""
        If Strings.Len(txtInsuredAccountCode.Text) > 0 Then
            ' CF100100 - Fixed problem
            If (Convert.ToString(txtInsuredAccountCode.Tag)) <> "" Then
                m_lInsuredAccountID = CInt(Convert.ToString(txtInsuredAccountCode.Tag))

                txtInsuredAccountCode.SelectionStart = 0
                txtInsuredAccountCode.SelectionLength = Strings.Len(txtInsuredAccountCode.Text)
            End If
        End If
        CheckMandatoryEnable()
    End Sub

    Private Sub txtCaseNumber_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCaseNumber.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
    End Sub

    Private Sub txtPurchaseInvoiceNo_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPurchaseInvoiceNo.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
    End Sub

    Private Sub txtPurchaseInvoiceNo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPurchaseInvoiceNo.Enter
        iPMFunc.SelectText(txtPurchaseInvoiceNo)
    End Sub

    Private Sub txtPurchaseOrderNo_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPurchaseOrderNo.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
    End Sub

    Private Sub txtPurchaseOrderNo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPurchaseOrderNo.Enter
        iPMFunc.SelectText(txtPurchaseOrderNo)
    End Sub

    Private Sub txtSpare_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSpare.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
    End Sub

    Private Sub txtSpare_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSpare.Enter
        iPMFunc.SelectText(txtSpare)
    End Sub

    '(RC) QBENZ014
    Private Sub txtAltRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAltRef.Enter
        iPMFunc.SelectText(txtAltRef)
    End Sub

    Private Function UpdateRejectedAuditSet(ByVal v_lAuditSetID As Integer) As Integer
        Dim result As Integer = 0
        Dim bACTAuditSet As Object
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: UpdateRejectedAuditSet
        ' PURPOSE: Update the AuditSet record with rejected details
        ' AUTHOR: Andrew Bibby
        ' DATE: 26/02/2003, 16:11
        ' RETURNS: PMTrue for success
        ' CHANGES: AMB 26/02/2003: PS220 - Manage Debtors 'reject write-off' development
        ' ---------------------------------------------------------------------------

        Dim oACTAuditSet As bACTAuditset.Form

        ' need to define these variables and fill them
        ' from the GetDetails otherwise we get problems later
        Dim lCompanyID, lUserId As Integer
        Dim vPostedDate As Object
        Dim sComment As String = ""
        Dim vApprovedDate As Object
        Dim lApprovedUserId, lAuditSetTypeId, lDocumentId, lRejected, lRejectedUserID, lCashListItemID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' must provide an auditset id
            If v_lAuditSetID = 0 Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", UpdateRejectedAuditSet, Auditset ID must be provided in order to retrieve Auditset details.")
            End If

            ' Get an instance of the business object
            Dim temp_oACTAuditSet As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oACTAuditSet, "bACTAuditSet.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oACTAuditSet = temp_oACTAuditSet
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", UpdateRejectedAuditSet, Unable to create business object 'bACTAuditSet.Form'")
            End If

            ' get the auditset record from the ID

            m_lReturn = oACTAuditSet.GetDetails(v_lAuditSetID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", UpdateRejectedAuditSet, Error getting AuditSet details")
            End If

            ' get the AuditSet details

            m_lReturn = oACTAuditSet.GetNext(vCompanyID:=lCompanyID, vUserID:=lUserId, vPostedDate:=vPostedDate, vComment:=sComment, vDocumentID:=lDocumentId, vAuditSetTypeID:=lAuditSetTypeId, vApprovedUserID:=lApprovedUserId, vApprovedDate:=vApprovedDate, vRejected:=lRejected, vRejectedUserID:=lRejectedUserID, vCashListItemID:=lCashListItemID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", UpdateRejectedAuditSet, Error performing GetNext for AuditSet details")
            End If

            ' update the properties on the AuditSet object

            m_lReturn = oACTAuditSet.EditUpdate(lRow:=1, vAuditsetID:=v_lAuditSetID, vCompanyID:=lCompanyID, vUserID:=lUserId, vPostedDate:=vPostedDate, vComment:=sComment, vDocumentID:=lDocumentId, vAuditSetTypeID:=lAuditSetTypeId, vApprovedDate:=vApprovedDate, vApprovedUserID:=lApprovedUserId, vRejected:=gPMConstants.PMEReturnCode.PMTrue, vRejectedUserID:=g_iUserID, vCashListItemID:=lCashListItemID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", UpdateRejectedAuditSet, Error performing EditUpdate with AuditSet details")
            End If

            ' do the save

            m_lReturn = oACTAuditSet.Update
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", UpdateRejectedAuditSet, Error updating AuditSet details")
            End If

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in UpdateRejectedAuditSet", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRejectedAuditSet", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result
            End Select

        Finally
            ' destroy the object

            oACTAuditSet.Dispose()
            oACTAuditSet = Nothing



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: IsInRoadMap
    '
    ' Description: Determine if we're part of a process where we are
    '              returning selected transactions. Eg. Allocation.
    '
    ' Written By: Simon.Baynes
    '
    ' Returns: True or False
    '
    ' Date: 20/10/2003
    '
    ' ***************************************************************** '
    Private Function IsInRoadMap() As Boolean

        Dim result As Boolean = False
        Try

            'If we've been called by the navigator then we're on a roadmap

            Return m_sCallingAppName.ToLower() = "iactallocation"

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsInRoadMap Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsInRoadMap", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '*************************************************************************
    ' Name:          ProcessAddEditComment
    '
    ' Description:   Process the adding/editing of a document comment for the
    '                current entry in the listview.
    '
    ' History:       CJB 25/03/2004 - Created
    '
    '*************************************************************************
    Private Function ProcessAddEditComment() As Integer

        Dim result As Integer = 0
        Dim ofrmAddEditComments As frmAddEditComments
        Dim sPreviousComment, sImageKey, sMenuBarCaption As String

        Try

            ' Assume Success
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Ensure an item is selected first!
            If (lvwSearchResults.CheckedItems.Count > 0) Then

                ' Instantiate the form
                ofrmAddEditComments = New frmAddEditComments()

                ' Save the comment so we can determine if it requires updating after
                sPreviousComment = CStr(m_vSearchData(ACIComment, Convert.ToString(lvwSearchResults.CheckedItems(0).Tag))).Trim()
                ' Pass the Data to the form
                With ofrmAddEditComments
                    .ReadOnly_Renamed = Mid(sPreviousComment, 1, 61) = "Original allocation reversed as part of Backdated Endorsement"

                    iPMForms.DisplayCaptions(ofrmAddEditComments, My.Resources.ResourceManager)
                    .Comment = sPreviousComment

                    sMenuBarCaption = " for Transaction - "
                    ' If drilled by document then prefix the account in the title bar to make unique
                    If DrillLevel = 1 Then

                        sMenuBarCaption = sMenuBarCaption & lvwSearchResults.CheckedItems(0).SubItems(ACListAccountShortCode).Text.Trim() & " / "
                    End If

                    sMenuBarCaption = sMenuBarCaption & lvwSearchResults.CheckedItems(0).SubItems(ACListDocumentRef).Text.Trim()
                    If Mid(sPreviousComment, 1, 61) = "Original allocation reversed as part of Backdated Endorsement" Then
                        .Text = "View " & sMenuBarCaption
                    Else
                        .Text = .Text & sMenuBarCaption
                    End If

                    ' Display the form
                    .ShowDialog()

                    ' Now check the status of the form
                    If .Status <> gPMConstants.PMEReturnCode.PMOK Then
                        If .Status = gPMConstants.PMEReturnCode.PMCancel Then
                            ' Cancel was clicked - just exit
                            ofrmAddEditComments = Nothing
                            Return result
                        Else
                            ' Return an error
                            result = gPMConstants.PMEReturnCode.PMFalse
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in ProcessAddEditComment...Status returned: " & Status, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAddEditComment")
                            ofrmAddEditComments = Nothing
                            Return result
                        End If
                    Else
                        ' OK was pressed in the add/edit comment form...see if data changed/added
                        If .Comment.Trim() <> sPreviousComment Then
                            ' Update the listview
                            m_vSearchData(ACIComment, Convert.ToString(lvwSearchResults.CheckedItems(0).Tag)) = .Comment

                            ' Show correct icon in the listview...
                            m_lReturn = FindListImageSmallIconToApply(v_sTransactionComment:= .Comment.Trim(), v_sTransactionNotReportedFlag:=CStr(m_vSearchData(ACINotReported, Convert.ToString(lvwSearchResults.FocusedItem.Tag))), r_sImageKey:=sImageKey)

                            lvwSearchResults.CheckedItems(0).ImageKey = sImageKey

                            ' Set flag (for use when drilling) to indicate that a refresh of the listview is rqd
                            m_oInterface.DataChanged = True

                            ' get the TransdDetailId of the selected item
                            m_lTransdetailID = CInt(m_vSearchData(ACITransDetailId, Convert.ToString(lvwSearchResults.CheckedItems(0).Tag)))

                            ' Call business object to update the comment for this entry in the db

                            m_lReturn = g_oBusiness.UpdateComment(v_lTransDetailId:=m_lTransdetailID, v_sComment:= .Comment)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = m_lReturn
                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in g_oBusiness.UpdateComment...m_lReturn: " & m_lReturn, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAddEditComment")
                                ofrmAddEditComments = Nothing
                                Return result
                            End If
                        End If
                    End If
                End With

                ofrmAddEditComments = Nothing
            Else
                'Display standard message that can't do this as nothing selected in ListView
                DisplayMessage(ACNoSelectionTitle, ACNoSelectionDetails, MsgBoxStyle.Exclamation)
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAddEditComment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAddEditComment", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '*************************************************************************
    ' Name:             ProcessBreakdown
    ' Description:    Display the section breakdown for the selected document
    ' Author:           Andrew Robinson
    ' History:
    '*************************************************************************
    Private Function ProcessBreakdown() As Integer

        Dim result As Integer = 0
        Const AC_METHOD As String = "ProcessBreakdown"
        Dim oPremiumBreakdown As Object
        Try

            Const AC_BREAKDOWN_CLASSNAME As String = "iPMBPremiumBreakdown.Interface"

            Dim vKeyArray As Array
            Dim lTag As Integer
            Dim sDocumentRef As String = ""
            Dim lSourceId, lInsuranceFileCnt, lDocumentTypeID As Integer

            result = gPMConstants.PMEReturnCode.PMTrue

            If (Me.lvwSearchResults.CheckedItems.Count > 0) Then

                lTag = gPMFunctions.ToSafeLong(Convert.ToString(Me.lvwSearchResults.CheckedItems(0).Tag), -1)
                sDocumentRef = gPMFunctions.ToSafeString(m_vSearchData(ACIDocumentRef, lTag)).Trim()
                lSourceId = gPMFunctions.ToSafeLong(m_vSearchData(ACISourceID, lTag))
                lDocumentTypeID = gPMFunctions.ToSafeLong(m_vSearchData(ACIDocumentTypeId, Convert.ToString(Me.lvwSearchResults.CheckedItems(0).Tag)))

                If Not (g_oBusiness Is Nothing) Then

                    m_lReturn = g_oBusiness.GetEventInsuranceFileForDocument(v_sDocumentRef:=sDocumentRef, v_lSourceID:=lSourceId, r_lInsuranceFileCnt:=lInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            MessageBox.Show("The breakdown information is unavailable for this transaction.", "Breakdown Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Return result
                        Else
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", Process Breakdown, Failed to get insurance file id for the selected document")
                        End If
                    End If

                Else
                    Throw New System.Exception((Constants.vbObjectError + 1).ToString() + ", Process Breakdown, Failed to find instance of business object")
                End If

                vKeyArray = Array.CreateInstance(GetType(Object), New Integer() {gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue - gPMConstants.PMENavLetGetKeyColPosition.PMKeyName + 1, 2}, New Integer() {gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0})
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameInsuranceFileCnt
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = lInsuranceFileCnt
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameDocumentTypeId
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = lDocumentTypeID

                m_lReturn = g_oObjectManager.GetInstance(oObject:=oPremiumBreakdown, sClassName:=AC_BREAKDOWN_CLASSNAME, vInstanceManager:=gPMConstants.PMGetLocalInterface)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception((Constants.vbObjectError + 2).ToString() + ", Process Breakdown, " + "Failed to create instance of " & AC_BREAKDOWN_CLASSNAME)
                End If

                m_lReturn = oPremiumBreakdown.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vEffectiveDate:=m_dtEffectiveDate, vFromBreakDown:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception((Constants.vbObjectError + 4).ToString() + ", Process Breakdown, " + "Method SetProcessModes of " & AC_BREAKDOWN_CLASSNAME & " failed")
                End If

                m_lReturn = oPremiumBreakdown.SetKeys(vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception((Constants.vbObjectError + 5).ToString() + ", Process Breakdown, " + "Method SetKeys of " & AC_BREAKDOWN_CLASSNAME & " failed")
                End If

                m_lReturn = oPremiumBreakdown.Start
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception((Constants.vbObjectError + 6).ToString() + ", Process Breakdown, " + "Method Start of " & AC_BREAKDOWN_CLASSNAME & " failed")
                End If

                oPremiumBreakdown.Dispose()


                oPremiumBreakdown = Nothing

            End If

            Return result

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=AC_METHOD & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=AC_METHOD, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            result = gPMConstants.PMEReturnCode.PMError

            If Not (oPremiumBreakdown Is Nothing) Then

                oPremiumBreakdown.Dispose()
                oPremiumBreakdown = Nothing
            End If

            Return result
        End Try
    End Function

    '*************************************************************************
    ' Name:          ProcessDoNotReport
    '
    ' Description:   Process the Do Not Report/Report flag for the current
    '                entry in the listview. If flagged as Do Not Report then
    '                the transaction will not appear on the Business
    '                Transacted by Agent reports.
    '
    ' History:       CJB 30/03/2004 - Created
    '
    '*************************************************************************
    Private Function ProcessDoNotReport() As Integer

        Dim result As Integer = 0
        Dim sImageKey As String = ""
        Dim boNotReported As Boolean

        Try

            ' Assume Success
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Ensure an item is selected first!
            If (lvwSearchResults.CheckedItems.Count > 0) Then

                ' get the TransdDetailId of the selected item
                m_lTransdetailID = CInt(m_vSearchData(ACITransDetailId, Convert.ToString(lvwSearchResults.CheckedItems(0).Tag)))

                ' Toggle the value of the not_reported flag before we update it in the db
                If CStr(m_vSearchData(ACINotReported, Convert.ToString(lvwSearchResults.CheckedItems(0).Tag))) = "1" Then
                    m_vSearchData(ACINotReported, Convert.ToString(lvwSearchResults.CheckedItems(0).Tag)) = "0"
                    boNotReported = False
                Else
                    m_vSearchData(ACINotReported, Convert.ToString(lvwSearchResults.CheckedItems(0).Tag)) = "1"
                    boNotReported = True
                End If

                ' Show correct icon in the listview
                m_lReturn = FindListImageSmallIconToApply(v_sTransactionComment:=CStr(m_vSearchData(ACIComment, Convert.ToString(lvwSearchResults.CheckedItems(0).Tag))).Trim(), v_sTransactionNotReportedFlag:=CStr(m_vSearchData(ACINotReported, Convert.ToString(lvwSearchResults.CheckedItems(0).Tag))), r_sImageKey:=sImageKey)

                lvwSearchResults.CheckedItems(0).ImageIndex = sImageKey

                ' Set flag (for use when drilling) to indicate that a refresh of the listview is rqd
                m_oInterface.DataChanged = True

                ' Call business object to update the Do Not Report (Transdetail.not_reported) flag for this
                ' entry in the db

                m_lReturn = g_oBusiness.UpdateNotReported(v_lTransDetailId:=m_lTransdetailID, v_boNotReported:=boNotReported)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured in g_oBusiness.UpdateNotReported...m_lReturn: " & m_lReturn, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessDoNotReport")
                    Return result
                End If

            Else
                'Display standard message that can't do this as nothing selected in ListView
                DisplayMessage(ACNoSelectionTitle, ACNoSelectionDetails, MsgBoxStyle.Exclamation)
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessDoNotReport Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessDoNotReport", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '*************************************************************************
    ' Name:          FindListImageSmallIconToApply
    '
    ' Description:   Set the small icon in the ListView for the current row
    '                based upon the following priority:
    '                  If transaction is flagged as Not Reported, show 'cross'
    '                    (Note that this will cause transaction to NOT appear on the
    '                    Business Transacted by Agent reports.)
    '                  Else if transaction has a comment show 'note',
    '                  Else show standard icon...
    '
    ' History:       CJB 01/04/2004 - Created
    '
    '*************************************************************************
    Private Function FindListImageSmallIconToApply(ByVal v_sTransactionComment As String, ByVal v_sTransactionNotReportedFlag As String, ByRef r_sImageKey As String) As Integer

        Dim result As Integer = 0
        Dim sImageKey As String = ""

        Try

            ' Assume Success
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the icon in the first column of the listview to indicate
            ' if the transaction is to be reported on or not
            If v_sTransactionNotReportedFlag.Trim() = "1" Then
                r_sImageKey = ACNOTReported
            Else
                ' If there is a comment for this row then show a notes icon in the listview
                ' else just show the std one
                If v_sTransactionComment.Trim() <> "" Then
                    r_sImageKey = ACNoteImage
                Else
                    r_sImageKey = ACSecondaryImage
                End If
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindListImageSmallIconToApply Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindListImageSmallIconToApply", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    'Purpose     :  Returns a count of the selected items in a listview.
    'Inputs      :  lhwndListview               The handle to the listview (eg. Listview1.Hwnd)
    'Outputs     :  Returns a count on the number of selected items.
    'Author      :  Andrew Baker
    'Date        :  23/07/2000 18:52
    'Notes       :  Works on all versions of the listview
    'Revisions   :

    Function LVCountSelected(ByRef lhwndListview As Integer) As Integer
        Const LVM_FIRST As Integer = &H1000S
        Const LVM_GETSELECTEDCOUNT As Integer = (LVM_FIRST + 50)

        Dim tmpPtr As IntPtr = IntPtr.Zero
        Return SendMessage(lhwndListview, LVM_GETSELECTEDCOUNT, 0, tmpPtr)
    End Function

    '*************************************************************************
    ' Name:          PopulateAccountTypes
    '
    ' Description:   Call GetAccountTypes Function of Business Component bActFindTransaction and gets list of account to display in
    '                cmbAccountType combo box
    '
    ' History:       VC 18/03/2008 - Created
    '
    '*************************************************************************

    Private Function PopulateAccountTypes() As Integer

        Dim result As Integer = 0
        Const kAccountTypeCode As Integer = 1
        Const kAccountTypeID As Integer = 0
        Dim vAccountTypeArray(,) As Object
        Dim iclientIndexid As Integer
        Dim sErrorMessage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmbAccountType.Items.Clear()

            m_lReturn = g_oBusiness.GetAccountTypes(r_vResultArray:=vAccountTypeArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrorMessage = "Failed to Get AccountTypes"
                Throw New Exception(sErrorMessage)
            End If

            For iArrayIndex As Integer = 0 To vAccountTypeArray.GetUpperBound(1)

                If CStr(vAccountTypeArray(kAccountTypeCode, iArrayIndex)).Trim().ToUpper() = "CLIENT" Then
                    iclientIndexid = iArrayIndex
                End If
                Dim cmbAccountType_NewIndex As Integer = -1

                cmbAccountType_NewIndex = cmbAccountType.Items.Add(CStr(vAccountTypeArray(1, iArrayIndex)))

                VB6.SetItemData(cmbAccountType, cmbAccountType_NewIndex, CInt(vAccountTypeArray(kAccountTypeID, iArrayIndex)))
            Next

            cmbAccountType.SelectedIndex = 0

            If m_bCalledViaClientManager Then
                cmbAccountType.SelectedIndex = iclientIndexid
                cmbAccountType.Enabled = gPMConstants.PMEReturnCode.PMFalse
            End If

            result = gPMConstants.PMEReturnCode.PMTrue



        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrorMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAccountTypes", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        Finally


        End Try
        Return result
    End Function

    '*************************************************************************
    ' Name:          RaiseTransaction
    '
    ' Description:   Raise a Transation when button "Reverse and Replace" is clicked
    '
    ' History:       VC 19/03/2008 - Created
    '
    '*************************************************************************

    Private Function RaiseTransaction() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RaiseTransaction"
        Dim lSelectedItemTag, lInsuranceFileCnt, lAccountID, lDocumentTypeID As Integer
        Dim sErrorMessage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lSelectedItemTag = Convert.ToString(lvwSearchResults.CheckedItems(0).Tag)
            lInsuranceFileCnt = CInt(m_vSearchData(ACIDocInsuranceFileCnt, lSelectedItemTag))
            lAccountID = CInt(m_vSearchData(ACIAccountId, lSelectedItemTag))
            lDocumentTypeID = CInt(m_vSearchData(ACIDocumentTypeId, lSelectedItemTag))

            Dim temp_m_oReplace As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oReplace, sClassName:="iPMBTransactions.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oReplace = temp_m_oReplace

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrorMessage = "Failed to get instance of iPMBTransactions.Interface"
                gPMFunctions.RaiseError(kMethodName, sErrorMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_oReplace.InsuranceFileCnt = lInsuranceFileCnt

            m_oReplace.FromEvent = False

            If lDocumentTypeID = ACDocTypeEndorsmentCredit Or lDocumentTypeID = ACDocTypeShortPeriodCredit Or lDocumentTypeID = ACDocTypeNBCredit Or lDocumentTypeID = ACDocTypeRenewalCredit Or lDocumentTypeID = ACDocTypeTransferedCredit Then

                m_oReplace.DebitCredit = "C"
            Else

                m_oReplace.DebitCredit = "D"
            End If

            m_oReplace.FromClientManager = m_bCalledViaClientManager

            m_lReturn = CType(m_oReplace, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrorMessage = "Failed to initialise Transaction."
                gPMFunctions.RaiseError(kMethodName, sErrorMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oReplace.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:="", vEffectiveDate:=DateTime.Now)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrorMessage = "Failed to set process modes."
                gPMFunctions.RaiseError(kMethodName, sErrorMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oReplace.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrorMessage = "Failed to start Raise transaction."
                gPMFunctions.RaiseError(kMethodName, sErrorMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_oReplace.Dispose()


            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrorMessage, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            Return result

        Finally


        End Try
        Return result
    End Function

    Private Function SetDefaultCurrencyOptions(ByVal v_iSourceId As Integer, ByVal v_iCurrencyID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetDefaultCurrencyOptions"

        Try
            Dim iBaseCurrencyID As Integer

            result = gPMConstants.PMEReturnCode.PMTrue

            'Check for Multicurrency Banking Option
            If gPMFunctions.ToSafeInteger(m_sMultiCurrencyBankingOption) <> 1 Then
                Return result
            End If

            'Check for Valid source and currency ID
            If (v_iSourceId < 1) Or (v_iCurrencyID < 1) Then
                Return result
            End If

            m_lReturn = m_oCurrencyConvert.GETBASECURRENCY(v_lCompanyID:=v_iSourceId, r_iBaseCurrencyID:=iBaseCurrencyID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Fails to get base currency for source id " & v_iSourceId, gPMConstants.PMELogLevel.PMLogError)
            End If

            If iBaseCurrencyID = v_iCurrencyID Then
                optAmountTransaction.Checked = True
                optOutstandingBase.Checked = True
            Else
                optAmountTransaction.Checked = True
                optOutstandingTransaction.Checked = True
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Fails to SetDefaultCurrencyOptions", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            Return result

        Finally

        End Try
        Return result
    End Function

    Private Sub udTolerance_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles udTolerance.ValueChanged
        txtTolerance.Text = udTolerance.Value
    End Sub

    Private Sub txtTolerance_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtTolerance.Validating
        If ToSafeInteger(txtTolerance.Text) > 100 Then
            MsgBox("Percentage should be between 1 and 100", vbOKOnly + vbCritical, "Inavalid Selection")
            txtTolerance.Text = ""
            e.Cancel = True
        End If
    End Sub

    Private Sub cmdSplitReceipt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSplitReceipt.Click

        Try

            If m_bOutstandingAmount = False Then
                MsgBox("Cannot split already allocated cashlist.")
                Exit Sub
            End If

            m_lReturn = OpenCashListForSplitReceipt(CashListID, DocumentId, TransDetailId)

        Catch excep As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Split Receipt command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdSplit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub cmdEditSplit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEditSplit.Click

        Try

            m_lReturn = OpenCashListForSplitReceipt(CashListID, DocumentId, TransDetailId)

        Catch excep As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Edit Split command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditSplit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Function OpenCashListForSplitReceipt(ByVal v_lCashListID As Integer, ByVal v_lDocumentID As Integer, ByVal v_lTransDetailID As Integer) As Integer
        Const kMethodName As String = "OpenCashListForSplitReceipt"
        Dim lResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try

            Dim oACTCashList As Object

            m_lReturn = g_oObjectManager.GetInstance(oACTCashList, sClassName:="iACTCashListItem.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                lResult = gPMConstants.PMEReturnCode.PMError
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iACTCashListItem", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Dim vKeys(1, 3) As Object

            vKeys(0, 0) = PMNavKeyConst.ACTKeyNameCashListTypeId
            'vKeys(1, 0) = gACTLibrary.ACTCashListTypePayments
            vKeys(1, 0) = gACTLibrary.ACTCashListTypeReceipts

            vKeys(0, 1) = PMNavKeyConst.ACTKeyNameCashListId
            vKeys(1, 1) = v_lCashListID

            vKeys(0, 2) = PMNavKeyConst.ACTKeyNameCurrencyID
            vKeys(1, 2) = CurrencyID

            vKeys(0, 3) = PMNavKeyConst.ACTKeyNameBranchID
            vKeys(1, 3) = m_iCompanyID

            m_lReturn = oACTCashList.Initialise
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                lResult = gPMConstants.PMEReturnCode.PMError
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CreateOrOpenCashList", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenCashListForSplitReceipt", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                oACTCashList.Dispose()
                oACTCashList = Nothing
                Return lResult
            End If

            m_lReturn = oACTCashList.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                lResult = gPMConstants.PMEReturnCode.PMError
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CreateOrOpenCashList", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenCashListForSplitReceipt", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                oACTCashList.Dispose()
                oACTCashList = Nothing
                Return lResult
            End If

            m_lReturn = oACTCashList.SetKeys(vKeyArray:=vKeys)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                lResult = gPMConstants.PMEReturnCode.PMError
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CreateOrOpenCashList", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenCashListForSplitReceipt", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                oACTCashList.Dispose()
                oACTCashList = Nothing
                Return lResult
            End If

            oACTCashList.CallingAppName = "SplitReceiptsFromFindTransaction"
            oACTCashList.DocumentID = v_lDocumentID

            m_lReturn = oACTCashList.Start
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                lResult = gPMConstants.PMEReturnCode.PMError
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CreateOrOpenCashList", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenCashListForSplitReceipt", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                oACTCashList.Dispose()
                oACTCashList = Nothing
                Return lResult
            End If

            If oACTCashList.Status = gPMConstants.PMEReturnCode.PMOK Then

                'Reverse the old Cash List
                'Reversal will happen on Click of Post Button
                'm_lReturn = ReverseCashList(v_lDocumentID)

                'Refresh the screen
                m_lReturn = m_oGeneral.GetInterfaceDetails()

                'Set Focus
                lvwSearchResults.Focus()
            Else

                lResult = gPMConstants.PMEReturnCode.PMCancel
                'r_lCashListId = 0
                v_lCashListID = 0
            End If

            oACTCashList.Dispose()
            oACTCashList = Nothing

        Catch excep As Exception
            lResult = gPMConstants.PMEReturnCode.PMFalse
            'Log Error

        End Try

        Return lResult

    End Function

    Private Sub txtDueDateFrom_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDueDateFrom.Enter
        iPMFunc.SelectText(txtDueDateFrom)
    End Sub

    Private Sub txtDueDateFrom_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDueDateFrom.Leave
        txtDueDateFrom.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, txtDueDateFrom.Text)
    End Sub

    Private Sub txtDueDateFrom_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDueDateFrom.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
    End Sub

    Private Sub txtDueDateTo_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDueDateTo.Enter
        iPMFunc.SelectText(txtDueDateTo)
    End Sub

    Private Sub txtDueDateTo_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDueDateTo.Leave
        txtDueDateTo.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, txtDueDateTo.Text)
    End Sub

    Private Sub txtDueDateTo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDueDateTo.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
    End Sub

    'Private Sub lvwSearchResults_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwSearchResults.SelectedIndexChanged
    '    If lvwSearchResults.SelectedItems.Count > 0 Then
    '        lvwSearchResults_ItemClick(lvwSearchResults.Items(lvwSearchResults.SelectedItems(0).Index))
    '    End If
    'End Sub

    Private Sub cmdCaseNumber_Click(sender As Object, e As EventArgs) Handles cmdCaseNumber.Click
        m_lReturn = PopulateCaseNumber()

    End Sub


    Public Function PopulateCaseNumber() As Long

        Dim result As Integer = 0
        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of Find Account if needed
            If ((m_oFindCase Is Nothing) = True) Then



                m_lReturn = g_oObjectManager.GetInstance(oObject:=m_oFindCase,
                        sClassName:="iCLMFindCase.Interface_Renamed",
                        vInstanceManager:=PMGetLocalInterface)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                    iPMFunc.LogMessage(
                            iType:=gPMConstants.PMELogLevel.PMLogOnError,
                            sMsg:="Failed to get an instance of iCLMFindcase.Interface_Renamed",
                            vApp:=ACApp,
                            vClass:=ACClass,
                            vMethod:="PopulateCaseNumber",
                            vErrNo:=Err.Number,
                            vErrDesc:=Err.Description)

                    Return result

                End If

            End If

            ' Get the account code
            ReDim vKeyArray(1, 0)

            ' Set the keys

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameCaseNumber

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = ToSafeString(txtCaseNumber.Text)


            m_lReturn = m_oFindCase.SetKeys(vKeyArray)

            'Setting the NotEditable property of FindAccount to avoid editing (PN 14472)
            m_oFindCase.CaseNumber = txtCaseNumber.Text

            m_lReturn = m_oFindCase.Start()
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(
                        iType:=gPMConstants.PMELogLevel.PMLogOnError,
                        sMsg:="Failed to get an instance of iCLMFindCase.Interface",
                        vApp:=ACApp,
                        vClass:=ACClass,
                        vMethod:="PopulateCaseNumber",
                        vErrNo:=Err.Number,
                        vErrDesc:=Err.Description)

                Return result

            End If

            ' If they didn't cancel then store the new data
            If (m_oFindCase.Status <> gPMConstants.PMEReturnCode.PMCancel) Then
                txtCaseNumber.Text = m_oFindCase.CaseNumber
            End If

            ' Remove the instance as it's not working a second time...
            m_oFindCase.Dispose()
            m_oFindCase = Nothing
            SSTabHelper.SetSelectedIndex(tabMain, 3)
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateCaseNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateCaseNumber", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=excep)

            Return result

        End Try

    End Function

    Public Function GetCaseNumber(ByRef r_sCaseNumber As String) As Long

        Dim sCaseNumber As String
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the short code filter

            sCaseNumber = txtCaseNumber.Text

            ' Call business and get the account id
            m_lReturn = g_oBusiness.GetCaseNumber(r_sCaseNumber:=sCaseNumber)

            If (sCaseNumber <> "") Then
                r_sCaseNumber = sCaseNumber
                txtCaseNumber.Text = sCaseNumber
            Else
                r_sCaseNumber = ""
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaseNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaseNumber", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=excep)

            Return result

        End Try

    End Function

    Private Sub frmInterface_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub lvwSearchResults_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles lvwSearchResults.ItemChecked
        Dim sDocumentRef As String
        Dim sIsSplitReceipt As String
        Dim bSelectedItem As Boolean = False
        Dim sSRP As String
        Dim vResultArray(,) As Object
        Dim iInsuranceFileCnt As Integer

        Dim sMediaRef As String
        Dim nAllowReceiptReversal As Integer
        Dim nAmount As Decimal
        Dim nOutstandingAmount As Decimal
        Dim nAccountKey As Integer
        Dim nPrevAccountID As Integer
        If e.Item.Checked Then
            e.Item.Selected = True
            e.Item.BackColor = Color.LightGray
        Else
            e.Item.Selected = False
            If ((e.Item.Index) Mod 2 = 0) Then
                e.Item.BackColor = Color.White
            Else
                e.Item.BackColor = Color.LightYellow
            End If
        End If

        Try
            If SplitReceiptSystemOption Then

                'Disable both the buttons
                cmdSplitReceipt.Enabled = False
                cmdEditSplit.Enabled = False

                For Each oSelectedItem As ListViewItem In lvwSearchResults.Items

                    'More than one row is selected
                    If (oSelectedItem IsNot Nothing AndAlso oSelectedItem.Checked) Then

                        sDocumentRef = m_vSearchData(ACIDocumentRef, Convert.ToString(oSelectedItem.Tag))
                        CurrencyID = m_vSearchData(ACICurrencyID, Convert.ToString(oSelectedItem.Tag))
                        sSRP = sDocumentRef.Substring(0, 3).ToUpper()

                        If m_iRestrictAllocationReversal = 1 And (m_vSearchData(ACIPartyType, Convert.ToString(oSelectedItem.Tag)) = "AG") And (Convert.ToDouble(m_vSearchData(ACIAccountAmount, Convert.ToString(oSelectedItem.Tag))) <> Convert.ToDouble(m_vSearchData(ACIOutstandingAccountAmount, Convert.ToString(oSelectedItem.Tag)))) Then
                            'Call the SP
                            'Loop and set and 

                            If (m_vSearchData(ACIDocInsuranceFileCnt, Convert.ToString(oSelectedItem.Tag))) <> "" Then
                                iInsuranceFileCnt = Convert.ToInt32((m_vSearchData(ACIDocInsuranceFileCnt, Convert.ToString(oSelectedItem.Tag))))
                            Else
                                iInsuranceFileCnt = 0
                            End If

                            m_lReturn = g_oBusiness.GetSubAgentDetails(iInsuranceFileCnt, vResultArray)
                            'm_lReturn = g_oBusiness.GetSubAgentDetails(m_vSearchData(ACIDocInsuranceFileCnt, Convert.ToString(oSelectedItem.Tag)), vResultArray)
                            If vResultArray IsNot Nothing AndAlso Information.IsArray(vResultArray) Then
                                Dim iUBound As Integer = 0
                                Dim iLBound As Integer = 0
                                iLBound = vResultArray.GetLowerBound(1)
                                iUBound = vResultArray.GetUpperBound(1)

                                For iCNT As Integer = iLBound To iUBound
                                    If CStr(vResultArray(5, iCNT)) <> "1" Then
                                        m_lDisableReverse = gPMConstants.PMEReturnCode.PMTrue
                                    Else
                                        m_lDisableReverse = gPMConstants.PMEReturnCode.PMFalse
                                    End If
                                Next
                            Else
                                m_lDisableReverse = gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If

                        If sSRP = "SRP" Then

                            sIsSplitReceipt = m_vSearchData(ACIsSplitReceipt, Convert.ToString(oSelectedItem.Tag)).ToString()
                            If m_sCallingAppName <> "iACTAllocation" Then
                                CashListID = gPMFunctions.ToSafeInteger(m_vSearchData(ACCashListID, Convert.ToString(oSelectedItem.Tag)).ToString())
                                DocumentId = gPMFunctions.ToSafeInteger(m_vSearchData(ACIDocDocumentID, Convert.ToString(oSelectedItem.Tag)).ToString())
                            End If
                            m_lTransdetailID = m_vSearchData(ACITransDetailId, Convert.ToString(oSelectedItem.Tag)).ToString()
                            If gPMFunctions.ToSafeInteger(m_vSearchData(ACIsLead, Convert.ToString(oSelectedItem.Tag))) = 1 Then
                                IsLead = True
                            Else
                                IsLead = False
                            End If

                            If (m_vSearchData(ACIOutstandingBaseAmount, Convert.ToString(oSelectedItem.Tag)).ToString()) = "" Then
                                m_bOutstandingAmount = False
                            Else
                                m_bOutstandingAmount = True
                            End If

                            If CashListID <> 0 Then
                                If sIsSplitReceipt = "1" Then

                                    cmdSplitReceipt.Enabled = False
                                    cmdEditSplit.Enabled = True
                                Else
                                    cmdSplitReceipt.Enabled = True
                                    cmdEditSplit.Enabled = False
                                End If
                            End If

                        End If

                        If bSelectedItem = False Then
                            bSelectedItem = True
                            cmdViewAllocation.Enabled = True
                        Else
                            'Multiple rows have been selected
                            cmdEditSplit.Enabled = False
                            cmdSplitReceipt.Enabled = False
                            sIsSplitReceipt = ""
                            CashListID = 0
                            DocumentId = 0
                            m_lTransdetailID = 0
                        End If

                    End If
                Next oSelectedItem
            End If

            cmdAllocate.Visible = True

            For Each oSelectedItem As ListViewItem In lvwSearchResults.Items
                If (oSelectedItem IsNot Nothing AndAlso oSelectedItem.Checked) Then
                    ''get user authority i.e. if user has authority to reverse receipt or not.
                    m_lReturn = GetReceiptReversalUserAuthority(nAllowReceiptReversal:=nAllowReceiptReversal)

                    sDocumentRef = m_vSearchData(ACIDocumentRef, Convert.ToString(oSelectedItem.Tag))
                    nAmount = ToSafeDecimal(m_vSearchData(ACIAccountAmount, Convert.ToString(oSelectedItem.Tag)))
                    nOutstandingAmount = ToSafeDecimal(m_vSearchData(ACIOutstandingAccountAmount, Convert.ToString(oSelectedItem.Tag)))
                    sMediaRef = CStr(m_vSearchData(ACISpare, Convert.ToString(oSelectedItem.Tag))).Trim()
                    nAccountKey = CInt(m_vSearchData(kAccount_Key, Convert.ToString(oSelectedItem.Tag)))
                    ''Reverse button enabled if
                    ''single SRP selected, user has authority to reverse receipts, SRP is unallocated and has not already been reversed
                    If lvwSearchResults.CheckedItems.Count = 1 _
                                 AndAlso sDocumentRef.Contains("SRP") _
                                 AndAlso nAllowReceiptReversal = 1 _
                                 AndAlso nAmount = nOutstandingAmount _
                                 AndAlso Not sMediaRef.ToUpper().Contains("REVERSED") _
                                 AndAlso Not sMediaRef.ToUpper().Contains("REVERSAL") _
                                 AndAlso Not nAccountKey = 0 Then
                        cmdReverse.Enabled = True
                    Else
                        cmdReverse.Enabled = False
                    End If
                    nAccountKey = CInt(m_vSearchData(ACIAccountId, oSelectedItem.Index))
                    If nPrevAccountID <> 0 And nAccountKey <> nPrevAccountID Then
                        cmdAllocate.Visible = False
                    End If
                    nPrevAccountID = nAccountKey
                End If
            Next

            If (lvwSearchResults.CheckedItems.Count = 0) Then
                cmdViewAllocation.Enabled = False
                cmdExpand.Enabled = False
                cmdAllocate.Enabled = False
                cmdReverse.Enabled = False
                cmdReverseAndReplace.Enabled = False
                cmdFindAccTrans.Enabled = False
                cmdFindDocTrans.Enabled = False
            ElseIf lvwSearchResults.CheckedItems.Count > 1 Then
                cmdViewAllocation.Enabled = False
                cmdExpand.Enabled = False
                If (m_sCallingAppName = OrionLink OrElse m_sCallingAppName.ToUpper() = ("iActFindTransaction").ToUpper()) Then
                    If Not m_bInsuredAccountView Then
                        cmdAllocate.Enabled = True
                    End If
                ElseIf m_sCallingAppName = "iPMWrkComponentStarter" Then
                    cmdAllocate.Enabled = True
                End If
                cmdReverseAndReplace.Enabled = False
                cmdFindAccTrans.Enabled = False
                cmdFindDocTrans.Enabled = False

            Else
                cmdViewAllocation.Enabled = True
                cmdExpand.Enabled = True
                cmdAllocate.Enabled = False
                cmdReverseAndReplace.Enabled = True
                cmdFindAccTrans.Enabled = True
                cmdFindDocTrans.Enabled = True
            End If
        Catch ex As ArgumentOutOfRangeException
            ' Do nothing

        Catch excep As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Split Receipt command button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchResults_ItemChanged", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try
    End Sub

    ''' <summary>
    ''' Get user authority for receipt reversal
    ''' </summary>
    ''' <param name="nAllowReceiptReversal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetReceiptReversalUserAuthority(ByRef nAllowReceiptReversal As Integer) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim sMethodName As String = "GetReceiptReversalUserAuthority"

        m_lReturn = g_oBusiness.GetReceiptReversalUserAuthority(nUserID:=g_iUserID, vResultArray:=m_vAllocationData)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + sMethodName + ", GetReceiptReversalUserAuthority failed")
            nResult = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        If Not Information.IsArray(m_vAllocationData) Then
            nAllowReceiptReversal = 0
        Else
            nAllowReceiptReversal = gPMFunctions.ToSafeInteger(m_vAllocationData(ACCanReverseReceiptArrPos, 0))
        End If

        Return nResult

    End Function

    Private Sub OldReverse()
        Const ACPostBatchReceipt As String = "82"
        Dim sPostBatchReceiptValue As String
        Dim eResult As DialogResult

        Try

            'Don't procede if no transaction is selected
            If Not Information.IsArray(m_vSearchData) Then
                Exit Sub
            End If

            m_lReturn = iPMFunc.GetSystemOption(CInt(ACPostBatchReceipt), sPostBatchReceiptValue)

            'Not checking m_lReturn as we're only checking the system option so
            'we can provide a nice message to the user.  If it fails we're in an
            'error handler anyway
            '    If sPostBatchReceiptValue = "1" Then
            '        sMessage = "Can not reverse this transaction as the system option for batch bank reconciliation is switched on."
            '        MsgBox sMessage, vbInformation, Me.Caption
            '        Exit Sub
            '    End If

            'Check they didn't click the button by accident
            eResult = MessageBox.Show("Are you sure you want to reverse this transaction? ", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If eResult = System.Windows.Forms.DialogResult.No Then
                Exit Sub
            End If

            m_lReturn = ProcessReversal()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Unable to create Document Reversal", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            m_lReturn = m_oGeneral.GetInterfaceDetails()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("cmdReverse_Click", "Failed to get Interface Details", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Set the focus back to the list
            lvwSearchResults.Focus()

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdReverse_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub cmdReverse_Click(sender As Object, e As EventArgs) Handles cmdReverse.Click

        Dim eResult As DialogResult
        Dim nSelectedItem As Integer
        Try
            'Don't proceed if no transaction is selected
            If Not Information.IsArray(m_vSearchData) Then
                Exit Sub
            End If

            eResult = MessageBox.Show("Do you want to reverse the selected cash item? ", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If eResult = System.Windows.Forms.DialogResult.No Then
                Exit Sub
            End If

            m_lReturn = ProcessReversal()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue 
               If m_lReturn <> gPMConstants.PMEReturnCode.PMOK Then
                   MessageBox.Show("The Selected cash item reversal failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
               End if
               Exit Sub
            End If

            'Value for the CashList_id, Branch, Currency_id is available in the m_vSearchData. Values for the BankAccount, CashList_Ref  also needs to be retrived. 
            Dim nCurrencyID As Integer
            Dim nCashListID As Integer
            Dim nBankAccountID As Integer
            Dim sCashListRef As String
            Dim nBranchID As Integer

            For Each oSelectedItem As ListViewItem In lvwSearchResults.Items
                If (oSelectedItem IsNot Nothing AndAlso oSelectedItem.Checked) Then
                    nCurrencyID = CInt(m_vSearchData(ACICurrencyID, Convert.ToString(oSelectedItem.Tag)))
                    nCashListID = CInt(m_vSearchData(ACCashListID, Convert.ToString(oSelectedItem.Tag)))
                    nBankAccountID = CInt(m_vSearchData(kBankAccountID, Convert.ToString(oSelectedItem.Tag)))
                    sCashListRef = CStr(m_vSearchData(kCashlistRef, Convert.ToString(oSelectedItem.Tag)))
                    nBranchID = CInt(m_vSearchData(ACISourceID, Convert.ToString(oSelectedItem.Tag)))
                End If
            Next

            ''reset values
            CashListID = 0
            DocumentId = 0
            m_lTransdetailID = 0
            m_lReturn = m_oGeneral.GetInterfaceDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("cmdReverse_Click", "Failed to get Interface Details", gPMConstants.PMELogLevel.PMLogError)
            End If
            'Set the focus back to the list
            lvwSearchResults.Focus()

            Dim dResult As DialogResult = MessageBox.Show("The highlighted cash item has been successfully reversed and allocated.", "Reversal Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

            'On clicking OK show cashlist screen
            If dResult = Windows.Forms.DialogResult.OK Then
                m_lReturn = CType(CreateCashListReceipt(nCurrencyId:=nCurrencyID, nCashListID:=nCashListID, nBankAccountID:=nBankAccountID, sCashListRef:=sCashListRef, nBranchID:=nBranchID), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("CreateCashListReceipt", "CreateCashListReceipt Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

        Catch excep As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdReverse_Click", excep:=excep)
            MessageBox.Show("The Selected cash item reversal failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End Try
    End Sub

    ' ***************************************************************** '
    ' Region: m_oNavStart Events
    ' ***************************************************************** '
    Private Sub m_oNavStart_NavigatorClose() Handles m_oNavStart.NavigatorClose
        m_bNavCompleted = True
    End Sub

    Private Sub m_oNavStart_SetProcessStatus(ByVal v_bProcessComplete As Boolean) Handles m_oNavStart.SetProcessStatus
        m_bProcessComplete = v_bProcessComplete
    End Sub

    ''' <summary>
    ''' Starts the cashlist process
    ''' </summary>
    ''' <param name="nCurrencyId"></param>
    ''' <param name="nCashListID"></param>
    ''' <param name="nBankAccountID"></param>
    ''' <param name="sCashListRef"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateCashListReceipt(ByVal nCurrencyId As Integer, ByVal nCashListID As Integer, ByVal nBankAccountID As Integer, ByVal sCashListRef As String, ByVal nBranchID As Integer) As Integer
        Dim nResult As Integer = 0
        Const sMethodName As String = "CreateCashListReceipt"

        Dim m_lReturn As gPMConstants.PMEReturnCode
        Dim oKeyArray(1, 6) As Object

        Try
            ' create an instance of navigator xm
            m_oNavStart = New iPMNavStart.Interface_Renamed()

            m_lReturn = m_oNavStart.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(sMethodName, "Failed to initialise iPMNavStart.Interface")
            End If

            ' set its properties
            m_oNavStart.CallingAppName = ACApp

            ' set the process to start
            m_oNavStart.ProcessCode = "ACTRCTV2"

            'The XML roadmap to use
            m_oNavStart.NavXMLFile = "ACTRCTV2.XML"

            ' Assign the key array with the parameter members
            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "cash_list_roadmap"

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = "RECEIPTS"

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameCurrencyID

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = nCurrencyId

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyAllowAllocateButton

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = 0

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameBankAccountID

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = nBankAccountID

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameCashlistRef

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = sCashListRef

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameSourceId

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = nBranchID

            ' set the navigators processes keys
            m_lReturn = m_oNavStart.SetKeys(vKeyArray:=oKeyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(sMethodName, "iPMNavStart.Interface.SetKeys Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' default the navigation completed actions to false
            m_bProcessComplete = False
            m_bNavCompleted = False

            ' start the specified navigator process
            m_lReturn = m_oNavStart.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(sMethodName, "iPMUNavStart.Interface.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' wait while the navigator process is completed
            Do
                Application.DoEvents()
            Loop While Not m_bNavCompleted

        Catch ex As Exception
            ' Do Not Call any functions before here or the error will be lost
            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=sMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally
            ' terminate this instance of the navigator process
            m_oNavStart.Dispose()

            ' clean up the object instances
            m_oNavStart = Nothing
            ' if the process is now complete
        End Try

        If m_bProcessComplete Then
            ' indicate the procedure was successfully run
            nResult = gPMConstants.PMEReturnCode.PMTrue
        Else
            ' indicate the procedure failed to complete
            nResult = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return nResult
    End Function

    Private Sub panAgentCode_TextChanged(sender As Object, e As EventArgs) Handles panAgentCode.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        txtAccountCode.Tag = ""
        panAgentCode.Tag = ""

        CheckMandatoryEnable()

    End Sub
End Class
