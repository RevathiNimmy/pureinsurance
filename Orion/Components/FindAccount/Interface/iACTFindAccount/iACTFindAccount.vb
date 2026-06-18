Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
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
    ' Edit History: 12OCT98 CF  Updated due to changes to TransDetail
    '                           table
    ' CJB 070405 PN14472 If m_bNotEditable is True then hide the Edit button as we only
    '            want to make visible if specifically set to False (in Account Explorer).
    ' CJB 200705 PN22518 Only set Status to PMOK if an a/c was selected in cmdAccountLookup_Click
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    'developer guide no.7(Latest Guide)
    Private Const vbFormCode As Integer = 0
    ' PRIVATE Data Members (Begin)

    ' Constants for the search data array indexes.
    Private Const ACIFullKey As Integer = 0
    Private Const ACIShortCode As Integer = 1
    Private Const ACIAccountName As Integer = 2
    Private Const ACILedgerID As Integer = 3
    Private Const ACIAccountType As Integer = 4
    Private Const ACIAccountID As Integer = 5
    Private Const ACIAccountUIK As Integer = 6
    Private Const ACINominalCode As Integer = 7
    Private Const ACIAccountStatus As Integer = 8
    Private Const ACIAccountStatusID As Integer = 9
    Private Const ACISourceID As Integer = 10
    Private Const ACIBalance As Integer = 11
    Private Const ACIFullName As Integer = 12
    Private Const ACIAddress As Integer = 13
    Private Const ACIForename As Integer = 14
    Private Const ACISourceCode As Integer = 18

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    ''' <summary>
    ''' Variable added to store AgentCnt of Agent linked to logged in User
    ''' </summary>
    Private m_nAgentCnt As integer
    ''' <summary>
    ''' Variable Declared to Set the Calling Component Name
    ''' </summary>
    Private m_sAppName As String = ""
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lAccountID As Integer
    Private m_iLedgerID As Integer
    Private m_iLedgerTypeID As Integer
    Private m_sShortCode As String = ""
    Private m_sFullKey As String = ""
    Private m_sAccountName As String = ""
    Private m_lAccountUIK As Integer
    Private m_lNominalAccountID As Integer
    Private m_iAccountCompanyId As Integer 'PN6169
    Private m_vSourceArray(,) As Object
    'DD 15/07/2002: Filter search list on only updatable accounts
    Public OnlyUpdatableAccounts As Boolean

    'DD 15/07/2002: True if new product option is enabled

    Private m_bEnhancedSecurity As Boolean
    Private hScrollValue As Integer = 0

    'Win32 API declarations to preserve list view horizontal scroll position after sort
    Const LVM_FIRST As Int32 = &H1000
    Const LVM_SCROLL As Int32 = LVM_FIRST + 20
    Const SBS_HORZ As Integer = 0


    'DJM 21/10/2002 : Source ID added to the list view
    Private m_lMaxColWidth As Array = Array.CreateInstance(GetType(Integer), New Integer() {ACListISourceID - ACListIShortCode + 1}, New Integer() {ACListIShortCode})

    'PSL 09/06/2003 Issue4434
    Private m_bInsurersAgents As Boolean

    Private m_bExcludeInsurersAgents As Boolean

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iACTFindAccount.General

    ' Stores the return value for a function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the search data from the business object.
    Public m_vSearchData(,) As Object
    'Private Const ACIProject = 5
    'Private Const ACIContract = 6
    'Private Const ACITelephone = 7
    'Private Const ACIDepartment = 8
    'Private Const ACIAgent = 9
    'Private Const ACIClient = 11
    'Private Const ACIAccountID = 11
    ' PRIVATE Data Members (End)

    ' Authority Level
    Private m_lPMAuthorityLevel As Integer

    ' Allow stopped accounts?
    Private m_bAllowStoppedAccounts As Boolean

    'DD 28/01/2003: Added for compatibility with iPMBParty
    Private m_bNotEditable As Boolean

    'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.10.2.1)
    Private m_bDisableWildcardSearchOption As Boolean
    Private m_bEnablePartialWildcardSearchOption As Boolean


    Public Property DisableWildcardSearchOption() As Boolean
        Get
            Return m_bDisableWildcardSearchOption
        End Get
        Set(ByVal Value As Boolean)
            m_bDisableWildcardSearchOption = Value
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
    'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.10.2.1)


    ' PUBLIC Property Procedures (Begin)
    Public WriteOnly Property AllowStoppedAccounts() As Boolean
        Set(ByVal Value As Boolean)
            m_bAllowStoppedAccounts = Value
        End Set
    End Property


    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
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

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    ''' <summary>
    ''' Property to Store Calling Component Name
    ''' </summary>
    ''' <returns>m_sAppName</returns>
    Public Property AppName() As string
        Get
            Return m_sAppName
        End Get
        Set(ByVal Value As string)

            m_sAppName = Value

        End Set
    End Property

    ''' <summary>
    ''' To store AgentCnt of Agent linked to logged in User
    ''' </summary>
    ''' <returns>Value of m_nAgentCnt</returns>
    Public Property AgentCnt() As integer
        Get
            Return m_nAgentCnt
        End Get
        Set(ByVal Value As integer)

            m_nAgentCnt = Value

        End Set
    End Property

    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)
    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

        End Set
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    Public ReadOnly Property AccountID() As Integer
        Get

            Return m_lAccountID

        End Get
    End Property

    Public Property LedgerID() As Integer
        Get

            Return m_iLedgerID

        End Get
        Set(ByVal Value As Integer)

            m_iLedgerID = Value

        End Set
    End Property

    Public Property LedgerTypeID() As Integer
        Get

            Return m_iLedgerTypeID

        End Get
        Set(ByVal Value As Integer)

            m_iLedgerTypeID = Value

        End Set
    End Property

    Public Property ShortCode() As String
        Get

            Return m_sShortCode

        End Get
        Set(ByVal Value As String)

            m_sShortCode = Value

        End Set
    End Property

    Public Property FullKey() As String
        Get

            Return m_sFullKey

        End Get
        Set(ByVal Value As String)

            m_sFullKey = Value

        End Set
    End Property

    Public ReadOnly Property AccountName() As String
        Get

            Return m_sAccountName

        End Get
    End Property

    Public Property AccountUIK() As Integer
        Get
            Return m_lAccountUIK
        End Get
        Set(ByVal Value As Integer)
            m_lAccountUIK = Value
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

    Public Property NominalAccountID() As Integer
        Get
            Return m_lNominalAccountID
        End Get
        Set(ByVal Value As Integer)
            m_lNominalAccountID = Value
        End Set
    End Property
    'PN6169
    Public ReadOnly Property AccountCompanyID() As Integer
        Get

            Return m_iAccountCompanyId

        End Get
    End Property
    'eck040500
    'developer guide no. 101
    Public WriteOnly Property SourceArray() As Object
        Set(ByVal Value As Object)

            ' Set the valid sources for the user
            m_vSourceArray = Value

        End Set
    End Property

    'DD 28/01/2003: Added for compatibility with iPMBParty

    Public Property NotEditable() As Boolean
        Get
            Return m_bNotEditable
        End Get
        Set(ByVal Value As Boolean)
            m_bNotEditable = Value
        End Set
    End Property


    Public Property ExcludeInsurersAgents() As Boolean
        Get
            Return m_bExcludeInsurersAgents
        End Get
        Set(ByVal Value As Boolean)
            m_bExcludeInsurersAgents = Value
        End Set
    End Property

    'PSL 09/06/2003 Issue 4434 need to know if this was called by insurer payment screen

    Public Property InsurersAgents() As Boolean
        Get
            Return m_bInsurersAgents
        End Get
        Set(ByVal Value As Boolean)
            m_bInsurersAgents = Value
        End Set
    End Property


    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim sUserID, sPaymentLedgersString As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display a searching message.
            DisplayStatusSearching()

            ' Disable parts of the interface while
            ' a search is in progress.
            m_lReturn = DisableInterface(bDisable:=True)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            If cboPMUser.UserID = 0 Then
                sUserID = ""
            Else
                sUserID = CStr(cboPMUser.UserID)
            End If

            'PSL 09/06/2003 4434 If it is called by insurer payment thwen do different select
            If m_bInsurersAgents Then


                m_lReturn = g_oBusiness.GetPaymentLedgers(sPaymentLedgersString)

                If m_bEnhancedSecurity Then
                    ' Get the filtered details from the business object.

                    m_lReturn = g_oBusiness.SelectAccountQueryFiltered(lNumberOfRecords:=ACMaxSearchDetails, vResultArray:=m_vSearchData, iCompanyID:=g_iCompanyID, vFullKey:=txtFullKey.Text, vLedgerID:=VB6.GetItemData(cboLedger, cboLedger.SelectedIndex), vAccountName:=txtName.Text, vAccountType:=VB6.GetItemData(cboAccountType, cboAccountType.SelectedIndex), vShortCode:=txtShortCode.Text, vInsuranceRef:=txtInsuranceRef.Text, vOperatorID:=sUserID, vPurchaseOrderNo:=txtPurchaseOrderNo.Text, vPurchaseInvoiceNo:=txtPurchaseInvoiceNo.Text, vSpare:="", vShowDeleted:=chkShowDeleted.CheckState, v_bOnlyUpdatable:=OnlyUpdatableAccounts, vPaymentLedgerIDs:=sPaymentLedgersString)
                Else
                    ' Get the details from the business object.

                    m_lReturn = g_oBusiness.SelectAccountQuery(lNumberOfRecords:=ACMaxSearchDetails, vResultArray:=m_vSearchData, iCompanyID:=g_iCompanyID, vFullKey:=txtFullKey.Text, vLedgerID:=VB6.GetItemData(cboLedger, cboLedger.SelectedIndex), vAccountName:=txtName.Text, vAccountType:=VB6.GetItemData(cboAccountType, cboAccountType.SelectedIndex), vShortCode:=txtShortCode.Text, vInsuranceRef:=txtInsuranceRef.Text, vOperatorID:=sUserID, vPurchaseOrderNo:=txtPurchaseOrderNo.Text, vPurchaseInvoiceNo:=txtPurchaseInvoiceNo.Text, vSpare:="", vShowDeleted:=chkShowDeleted.CheckState, vPaymentLedgerIDs:=sPaymentLedgersString, vShowBalance:=chkShowBalance.CheckState , vBrokerCnt:=m_nAgentCnt)
                End If

            ElseIf m_bExcludeInsurersAgents Then


                m_lReturn = g_oBusiness.GetPaymentLedgers(sPaymentLedgersString)

                If m_bEnhancedSecurity Then
                    ' Get the filtered details from the business object.

                    m_lReturn = g_oBusiness.SelectAccountQueryFiltered(lNumberOfRecords:=ACMaxSearchDetails, vResultArray:=m_vSearchData, iCompanyID:=g_iCompanyID, vFullKey:=txtFullKey.Text, vLedgerID:=VB6.GetItemData(cboLedger, cboLedger.SelectedIndex), vAccountName:=txtName.Text, vAccountType:=VB6.GetItemData(cboAccountType, cboAccountType.SelectedIndex), vShortCode:=txtShortCode.Text, vInsuranceRef:=txtInsuranceRef.Text, vOperatorID:=sUserID, vPurchaseOrderNo:=txtPurchaseOrderNo.Text, vPurchaseInvoiceNo:=txtPurchaseInvoiceNo.Text, vSpare:="", vShowDeleted:=chkShowDeleted.CheckState, v_bOnlyUpdatable:=OnlyUpdatableAccounts, vExcludeLedgerIDs:=sPaymentLedgersString)
                Else
                    ' Get the details from the business object.

                    m_lReturn = g_oBusiness.SelectAccountQuery(lNumberOfRecords:=ACMaxSearchDetails, vResultArray:=m_vSearchData, iCompanyID:=g_iCompanyID, vFullKey:=txtFullKey.Text, vLedgerID:=VB6.GetItemData(cboLedger, cboLedger.SelectedIndex), vAccountName:=txtName.Text, vAccountType:=VB6.GetItemData(cboAccountType, cboAccountType.SelectedIndex), vShortCode:=txtShortCode.Text, vInsuranceRef:=txtInsuranceRef.Text, vOperatorID:=sUserID, vPurchaseOrderNo:=txtPurchaseOrderNo.Text, vPurchaseInvoiceNo:=txtPurchaseInvoiceNo.Text, vSpare:="", vShowDeleted:=chkShowDeleted.CheckState, vExcludeLedgerIDs:=sPaymentLedgersString, vShowBalance:=chkShowBalance.CheckState, vBrokerCnt:=m_nAgentCnt)
                End If

            Else
                'DD 12/07/2002: Added new Product Option

                If m_bEnhancedSecurity Then
                    ' Get the filtered details from the business object.

                    m_lReturn = g_oBusiness.SelectAccountQueryFiltered(lNumberOfRecords:=ACMaxSearchDetails, vResultArray:=m_vSearchData, iCompanyID:=g_iCompanyID, vFullKey:=txtFullKey.Text, vLedgerID:=VB6.GetItemData(cboLedger, cboLedger.SelectedIndex), vAccountName:=txtName.Text, vAccountType:=VB6.GetItemData(cboAccountType, cboAccountType.SelectedIndex), vShortCode:=txtShortCode.Text, vInsuranceRef:=txtInsuranceRef.Text, vOperatorID:=sUserID, vPurchaseOrderNo:=txtPurchaseOrderNo.Text, vPurchaseInvoiceNo:=txtPurchaseInvoiceNo.Text, vSpare:="", vShowDeleted:=chkShowDeleted.CheckState, v_bOnlyUpdatable:=OnlyUpdatableAccounts)
                Else
                    ' Get the details from the business object.

                    m_lReturn = g_oBusiness.SelectAccountQuery(lNumberOfRecords:=ACMaxSearchDetails, vResultArray:=m_vSearchData, iCompanyID:=g_iCompanyID, vFullKey:=txtFullKey.Text, vLedgerID:=cboLedger.SelectedItem.itemdata, vAccountName:=txtName.Text, vAccountType:=cboAccountType.SelectedItem.itemdata, vShortCode:=txtShortCode.Text, vInsuranceRef:=txtInsuranceRef.Text, vOperatorID:=sUserID, vPurchaseOrderNo:=txtPurchaseOrderNo.Text, vPurchaseInvoiceNo:=txtPurchaseInvoiceNo.Text, vSpare:="", vShowDeleted:=chkShowDeleted.CheckState, vShowBalance:=chkShowBalance.CheckState, vBrokerCnt:=m_nAgentCnt)
                End If
            End If

            'Assign Values to Interface
            m_lReturn = DataToInterface()

            ' {* USER DEFINED CODE (End) *}

            ' Check the return values.
            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Found search details.
                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' No search details found.
                Case Else
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
            End Select

            ' Display the number of item found message.
            DisplayStatusFound()

            'CMG/PB Bug 2253 Dont disable the find now button
            CheckMandatoryEnable()

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    '
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim sLookupDesc As String = ""
        Dim vLedgerName As String = ""
        Dim lTextWidth As Integer
        Dim v_vValue As String = ""
        Dim bMultiTreeAccounting As Boolean

        ' Const ACFindImage As String = "FindImage"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwSearchResults.Items.Clear()

            ' Check that search details are valid before continuing.
            If Not Information.IsArray(m_vSearchData) Then
                Return result
            End If

            ' Retrieve MultiTree Accounting Flag START
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=gPMConstants.SIRBCHHeadOffice, r_vUnderwriting:=v_vValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log an error, set to non multitreeaccounting and continue (as not serious)
                v_vValue = "0"
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call getProductOptionValue", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If
            bMultiTreeAccounting = v_vValue = "1"

            ' Dont update the list view till we've populated it.
            'developer guide no. 170
            m_lReturn = ListViewFunc.ListViewBatchStart(lvwSearchResults)

            ' Assign the details to the interface.
            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                ' Display anyhow if system if not multitree accounting
                If (ValidSource(vSource:=m_vSearchData(ACISourceID, lRow))) Or (Not bMultiTreeAccounting) Then

                    ' Get the ledger name from the ledger id
                    m_lReturn = GetLedgerNames(iLedgerID:=Conversion.Val(CStr(m_vSearchData(ACILedgerID, lRow))), vLedgerShortName:=vLedgerName)

                    ' Assign the details to the first column.

                    ' Column 1 ShortName

                    'Add the image name to display the row icon.
                    oListItem = lvwSearchResults.Items.Add(CStr(m_vSearchData(ACIShortCode, lRow)).Trim(), "FindImage")

                    ' Assign details to other the columns
                    With oListItem

                        ' Column 2 Resolved Name
                        'RKS 26/08/2004
                        ListViewHelper.GetListViewSubItem(oListItem, ACListIAccountName).Text = CStr(m_vSearchData(ACIAccountName, lRow)).Trim()

                        ' Column 3 First Name
                        ListViewHelper.GetListViewSubItem(oListItem, ACListIForename).Text = CStr(m_vSearchData(ACIForename, lRow)).Trim()

                        ' Column 4 First line of address
                        ListViewHelper.GetListViewSubItem(oListItem, ACListIAddress).Text = CStr(m_vSearchData(ACIAddress, lRow)).Trim()

                        ' Column 5 Account Status
                        ListViewHelper.GetListViewSubItem(oListItem, ACListIAccountStatus).Text = CStr(m_vSearchData(ACIAccountStatus, lRow)).Trim()

                        ' Column 6 Ledger Name
                        ListViewHelper.GetListViewSubItem(oListItem, ACListILedger).Text = vLedgerName.Trim()

                        Dim dbNumericTemp As Double
                        If Double.TryParse(CStr(m_vSearchData(ACIAccountType, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then 'PN 21016
                            m_lReturn = GetLookupDesc(lLookupRow:=ACLAccountType, lLookupID:=CInt(m_vSearchData(ACIAccountType, lRow)), sLookupDesc:=sLookupDesc)
                        Else
                            sLookupDesc = ""
                        End If

                        ' Column 7 Account Type
                        ListViewHelper.GetListViewSubItem(oListItem, ACListIAccountType).Text = sLookupDesc

                        ' Column 8 Balance
                        If chkShowBalance.CheckState = CheckState.Checked Then
                            ListViewHelper.GetListViewSubItem(oListItem, ACListIBalance).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_vSearchData(ACIBalance, lRow)))
                        End If

                        ' Column 9 Source ID
                        ListViewHelper.GetListViewSubItem(oListItem, ACListISourceID).Text = CStr(m_vSearchData(ACISourceCode, lRow)).Trim()

                    End With

                    ' Expand column widths to fit the text where necessary
                    With lvwSearchResults

                        lTextWidth = VB6.PixelsToTwipsX(CreateGraphics().MeasureString(CStr(m_vSearchData(ACIShortCode, lRow)).Trim(), Font).Width)

                        If lTextWidth > m_lMaxColWidth(ACListIShortCode) Then
                            m_lMaxColWidth(ACListIShortCode) = lTextWidth
                            .Columns.Item(ACListIShortCode).Width = CInt(VB6.TwipsToPixelsX(lTextWidth))
                        End If

                        lTextWidth = VB6.PixelsToTwipsX(CreateGraphics().MeasureString(CStr(m_vSearchData(ACIFullName, lRow)).Trim(), Font).Width)

                        If lTextWidth > m_lMaxColWidth(ACListIAccountName) Then
                            m_lMaxColWidth(ACListIAccountName) = lTextWidth
                            .Columns.Item(ACListIAccountName).Width = CInt(VB6.TwipsToPixelsX(lTextWidth))
                        End If

                        lTextWidth = VB6.PixelsToTwipsX(CreateGraphics().MeasureString(CStr(m_vSearchData(ACIForename, lRow)).Trim(), Font).Width)

                        If lTextWidth > m_lMaxColWidth(ACListIForename) Then
                            m_lMaxColWidth(ACListIForename) = lTextWidth
                            .Columns.Item(ACListIForename).Width = CInt(VB6.TwipsToPixelsX(lTextWidth))
                        End If

                        lTextWidth = VB6.PixelsToTwipsX(CreateGraphics().MeasureString(CStr(m_vSearchData(ACIAddress, lRow)).Trim(), Font).Width)

                        If lTextWidth > m_lMaxColWidth(ACListIAddress) Then
                            m_lMaxColWidth(ACListIAddress) = lTextWidth
                            .Columns.Item(ACListIAddress).Width = CInt(VB6.TwipsToPixelsX(lTextWidth))
                        End If

                        lTextWidth = VB6.PixelsToTwipsX(CreateGraphics().MeasureString(CStr(m_vSearchData(ACIAccountStatus, lRow)).Trim(), Font).Width)

                        If lTextWidth > m_lMaxColWidth(ACListIAccountStatus) Then
                            m_lMaxColWidth(ACListIAccountStatus) = lTextWidth
                            .Columns.Item(ACListIAccountStatus).Width = CInt(VB6.TwipsToPixelsX(lTextWidth))
                        End If


                        lTextWidth = VB6.PixelsToTwipsX(CreateGraphics().MeasureString(vLedgerName.Trim(), Font).Width)

                        If lTextWidth > m_lMaxColWidth(ACListILedger) Then
                            m_lMaxColWidth(ACListILedger) = lTextWidth
                            .Columns.Item(ACListILedger).Width = CInt(VB6.TwipsToPixelsX(lTextWidth))
                        End If

                        lTextWidth = VB6.PixelsToTwipsX(CreateGraphics().MeasureString(sLookupDesc.Trim(), Font).Width)

                        If lTextWidth > m_lMaxColWidth(ACListIAccountType) Then
                            m_lMaxColWidth(ACListIAccountType) = lTextWidth
                            .Columns.Item(ACListIAccountType).Width = CInt(VB6.TwipsToPixelsX(lTextWidth))
                        End If

                        'DJM 21/10/2002 : Source ID added to the list view.
                        lTextWidth = VB6.PixelsToTwipsX(CreateGraphics().MeasureString(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_vSearchData(ACIBalance, lRow))), Font).Width)

                        'Hide the Balance column if not required.
                        If chkShowBalance.CheckState = CheckState.Checked Then
                            If lTextWidth > m_lMaxColWidth(ACListIBalance) Then
                                m_lMaxColWidth(ACListIBalance) = lTextWidth
                                .Columns.Item(ACListIBalance).Width = CInt(VB6.TwipsToPixelsX(lTextWidth))
                            ElseIf VB6.PixelsToTwipsX(.Columns.Item(ACListIBalance).Width) < lTextWidth Then
                                .Columns.Item(ACListIBalance).Width = CInt(VB6.TwipsToPixelsX(lTextWidth))
                            End If
                        Else
                            .Columns.Item(ACListIBalance).Width = CInt(0)
                        End If

                        lTextWidth = VB6.PixelsToTwipsX(CreateGraphics().MeasureString(CStr(m_vSearchData(ACISourceID, lRow)).Trim(), Font).Width)

                        If lTextWidth > m_lMaxColWidth(ACListISourceID) Then
                            m_lMaxColWidth(ACListISourceID) = lTextWidth
                            .Columns.Item(ACListISourceID).Width = CInt(VB6.TwipsToPixelsX(lTextWidth))
                        End If


                    End With
                    ' {* USER DEFINED CODE (End) *}

                    ' Set the tag property with the index of
                    ' the search data storage.
                    oListItem.Tag = CStr(lRow)

                    ' Refresh the first X amount of rows, to
                    ' allow the user to see the results instantly.
                    If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                        ' Select the first item.
                        lvwSearchResults.Items.Item(0).Selected = True

                        ' Refresh the initial results.
                        lvwSearchResults.Refresh()
                    End If
                End If
            Next lRow

            If lvwSearchResults.Items.Count > 0 Then

                ' Enable the interface now that the search has completed.
                m_lReturn = DisableInterface(bDisable:=False)

                ' CTAF 291099 - Re-enable the list view
                'developer guide no. 170
                m_lReturn = ListViewFunc.ListViewBatchEnd()

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                ' SET 30042002 -
                '   Moved the following code down cos 'lvwSearchResults_Click'
                '   changes the value of m_lReturn&

                ' Select the first item.
                lvwSearchResults.FocusedItem = lvwSearchResults.Items.Item(0)
                lvwSearchResults_Click(lvwSearchResults, New EventArgs())
                ' SET 30042002 -

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToProperties
    '
    ' Description: Updates the property member from the search data
    '              storage.
    '
    ' ***************************************************************** '
    Public Function DataToProperties() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.

            lSelectedItem = Convert.ToString(lvwSearchResults.Items.Item(lvwSearchResults.FocusedItem.Index).Tag)

            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}
            'eck PN6169
            m_iAccountCompanyId = CInt(Conversion.Val(CStr(m_vSearchData(ACISourceID, lSelectedItem))))
            m_lAccountID = CInt(Conversion.Val(CStr(m_vSearchData(ACIAccountID, lSelectedItem))))
            m_iLedgerID = Conversion.Val(CStr(m_vSearchData(ACILedgerID, lSelectedItem)))
            m_lReturn = GetLedgerTypeID(m_iLedgerID, m_iLedgerTypeID)
            m_sShortCode = CStr(m_vSearchData(ACIShortCode, lSelectedItem)).Trim()
            '    m_sFullKey$ = Trim$(m_vSearchData(ACIFullKey, lSelectedItem&))
            m_sAccountName = CStr(m_vSearchData(ACIAccountName, lSelectedItem)).Trim()
            m_lAccountUIK = CInt(Conversion.Val(CStr(m_vSearchData(ACIAccountUIK, lSelectedItem))))
            m_lNominalAccountID = CInt(Conversion.Val(CStr(m_vSearchData(ACINominalCode, lSelectedItem))))

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get all of the lookup details.
            ' See Interface SetProcessModes

            ' {* USER DEFINED CODE (Begin) *}

            'PSL 09/06/2003 Issue4434
            'If Not m_bInsurersAgents Then

            cboLedger.Items.Clear()
            m_lReturn = GetLedgerDetails(ctlLookup:=cboLedger, vAllTypes:=True)
            
            If AgentCnt > 0 AndAlso AppName = "iACTFindTransaction" Then
                cboLedger.SelectedIndex = 2
                cboLedger.Enabled = false
            End If
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'End If

            cboAccountType.Items.Clear()

            m_lReturn = GetLookupDetails(lLookupRow:=ACLAccountType, ctlLookup:=cboAccountType, vAllTypes:=True)


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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

    ' PRIVATE Methods (Begin)
    'eck090500
    ' ********************************************************************************* '
    ' Name: ValidSource                                                            '
    '                                                                                   '
    ' Description: Checks that the transaction is for one of the branches being paid    '
    '                                                                                   '
    ' ********************************************************************************* '
    Private Function ValidSource(ByVal vSource As Object) As Boolean
        Dim result As Boolean = False
        If Not Information.IsArray(m_vSourceArray) Then
            Return True
        End If

        For i As Integer = 0 To m_vSourceArray.GetUpperBound(1)
            If CInt(m_vSourceArray(0, i)) = CInt(vSource) Then
                result = True
            End If
        Next i
        Return result
    End Function

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

            ' {* USER DEFINED CODE (Begin) *}

            ' GetLedgerDetails must have been called
            '    Dim i As Integer
            '    Dim iLedgerTypeID As Integer
            '    If m_iLedgerID% <> -1 Then
            '        ' Select the requested ledger
            '        For i = 0 To cboLedger.ListCount - 1
            '            If cboLedger.ItemData(i) = m_iLedgerID% Then
            '                cboLedger.ListIndex = i
            '                cboLedger.Enabled = False
            '                Exit For
            '            End If
            '        Next i
            '    ElseIf m_iLedgerTypeID% <> -1 Then
            '        ' Select the requested ledger type
            '        For i = 0 To cboLedger.ListCount - 1
            '            m_lReturn = GetLedgerTypeID(cboLedger.ItemData(i), iLedgerTypeID)
            '            If iLedgerTypeID = m_iLedgerTypeID% Then
            '                cboLedger.ListIndex = i
            '                cboLedger.Enabled = False
            '                Exit For
            '            End If
            '        Next i
            '    End If
            txtShortCode.Text = m_sShortCode.Trim()
            txtFullKey.Text = m_sFullKey.Trim()

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False
            End Select

            ' Get all of the lookup values as related to effective date
            m_lReturn = GetLookupValues()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'PSL 09/06/2003 Iss4434 if it is insurer payment then don't want ledger lookup
            'we will just include Insurer, Agent, Extra ledgers
            If m_bInsurersAgents Then
                cboLedger.Visible = False
                lblLedger.Visible = False
                cmdFindNow.Enabled = True
            End If
            ' Get the ledgers.
            m_lReturn = GetLedgers()

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

            ' Update the interface details with the
            ' property members.
            m_lReturn = PropertiesToInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' Set the column widths for the search list.
            With lvwSearchResults
                .Columns.Item(ACListIShortCode).Width = CInt(CreateGraphics().MeasureString(.Columns.Item(ACListIShortCode).Text, Font).Width) + 20
                m_lMaxColWidth(ACListIShortCode) = CInt(VB6.PixelsToTwipsX(.Columns.Item(ACListIShortCode).Width))
                .Columns.Item(ACListIAccountStatus).Width = CInt(CreateGraphics().MeasureString(.Columns.Item(ACListIAccountStatus).Text, Font).Width) + 20
                m_lMaxColWidth(ACListIAccountStatus) = CInt(VB6.PixelsToTwipsX(.Columns.Item(ACListIAccountStatus).Width))
                .Columns.Item(ACListIAccountName).Width = CInt(CreateGraphics().MeasureString(.Columns.Item(ACListIAccountName).Text, Font).Width) + 20
                m_lMaxColWidth(ACListIAccountName) = CInt(VB6.PixelsToTwipsX(.Columns.Item(ACListIAccountName).Width))
                .Columns.Item(ACListILedger).Width = CInt(CreateGraphics().MeasureString(.Columns.Item(ACListILedger).Text, Font).Width) + 20
                m_lMaxColWidth(ACListILedger) = CInt(VB6.PixelsToTwipsX(.Columns.Item(ACListILedger).Width))
                .Columns.Item(ACListIAccountType).Width = CInt(CreateGraphics().MeasureString(.Columns.Item(ACListIAccountType).Text, Font).Width) + 20
                m_lMaxColWidth(ACListIAccountType) = CInt(VB6.PixelsToTwipsX(.Columns.Item(ACListIAccountType).Width))
                .Columns.Item(ACListIBalance).Width = CInt(CreateGraphics().MeasureString(.Columns.Item(ACListIBalance).Text, Font).Width) + 20
                m_lMaxColWidth(ACListIBalance) = CInt(VB6.PixelsToTwipsX(.Columns.Item(ACListIBalance).Width))
                .Columns.Item(ACListISourceID).Width = CInt(CreateGraphics().MeasureString(.Columns.Item(ACListISourceID).Text, Font).Width) + 20
                m_lMaxColWidth(ACListISourceID) = CInt(VB6.PixelsToTwipsX(.Columns.Item(ACListISourceID).Width))
                .Columns.Item(ACListIForename).Width = CInt(CreateGraphics().MeasureString(.Columns.Item(ACListIForename).Text, Font).Width) + 20
                m_lMaxColWidth(ACListIForename) = CInt(VB6.PixelsToTwipsX(.Columns.Item(ACListIForename).Width))
                .Columns.Item(ACListIAddress).Width = CInt(CreateGraphics().MeasureString(.Columns.Item(ACListIAddress).Text, Font).Width) + 20
                m_lMaxColWidth(ACListIAddress) = CInt(VB6.PixelsToTwipsX(.Columns.Item(ACListIAddress).Width))
            End With

            ' Show the default tab (may be a parameter?)
            SSTabHelper.SetSelectedIndex(tabMain, 0)
            ' Disable all the other tab panels
            For i As Integer = 0 To ACTabTitleCount - 1
                pnlMain(i).Enabled = (i = 0)
            Next i

            ' CF090499
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwSearchResults.Handle.ToInt32(), v_vShowRowSelect:=True)
            lvwSearchResults.FullRowSelect = True
            ' Alix - Always false until we find an agent
            cmdEdit.Enabled = False


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ClearInterface
    '
    ' Description: Clears all of the interface details for a new
    '              search.
    '
    ' ***************************************************************** '
    Private Function ClearInterface() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if the user still wishes to clear
            ' the interface.


            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Display the message.
            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            ' Check message result.
            If iMsgResult = System.Windows.Forms.DialogResult.No Then
                ' Don't continue with the clear.
                Return result
            End If

            ' Clear the interface details.

            ' Clear the search data array.
            m_vSearchData = Nothing

            ' Clear the search list details.
            lvwSearchResults.Items.Clear()

            ' Clear the search status bar.

            _stbStatus_Panel1.Text = ""

            ' {* USER DEFINED CODE (Begin) *}
            If AgentCnt > 0 AndAlso AppName = "iACTFindTransaction" Then
                cboLedger.SelectedIndex = 2
            Else
                cboLedger.SelectedIndex = 0
            End If
            
            txtShortCode.Text = ""
            txtFullKey.Text = ""
            txtName.Text = ""
            cboAccountType.SelectedIndex = 0
            txtInsuranceRef.Text = ""
            'txtOperatorID.Text = ""
            cboPMUser.ListIndex = 0
            txtPurchaseOrderNo.Text = ""
            txtPurchaseInvoiceNo.Text = ""

            chkShowDeleted.CheckState = CheckState.Unchecked
            chkShowBalance.CheckState = CheckState.Unchecked

            ' Set to the first tab.
            SSTabHelper.SetSelectedIndex(tabMain, 0)

            ' Set focus to the search details.
            txtShortCode.Focus()

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

            ' {* USER DEFINED CODE (End) *}

            ' Disable parts of the interface, so the
            ' user can now only enter a new search
            m_lReturn = DisableInterface(bDisable:=True)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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

            m_ctlTabFirstLast(ACControlStart, 0) = txtShortCode
            m_ctlTabFirstLast(ACControlEnd, 0) = cboLedger

            m_ctlTabFirstLast(ACControlStart, 1) = txtInsuranceRef
            m_ctlTabFirstLast(ACControlEnd, 1) = txtPurchaseInvoiceNo

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            'developer guide no.243
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdFindNow.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindNowButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNewSearch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewSearchButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNew.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            For i As Integer = 0 To ACTabTitleCount - 1

                SSTabHelper.SetTabCaption(tabMain, i, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle + i, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Next i



            lvwSearchResults.Columns.Item(ACListIShortCode).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColShortCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchResults.Columns.Item(ACListIAccountName).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchResults.Columns.Item(ACListIAccountStatus).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchResults.Columns.Item(ACListILedger).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColLedger, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchResults.Columns.Item(ACListIAccountType).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColAccountType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'eck050302


            lvwSearchResults.Columns.Item(ACListIBalance).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColBalance, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'DJM 21/10/2002 : Source ID added to the list view


            lvwSearchResults.Columns.Item(ACListISourceID).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColSourceID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchResults.Columns.Item(ACListIForename).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFirstName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchResults.Columns.Item(ACListIAddress).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddress, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (Begin) *}


            lblShortCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACShortCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblFullKey.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFullKey, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblLedger.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLedgerID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAccountType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblInsuranceRef.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInsuranceRef, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblOperatorID.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOperatorID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPurchaseInvoiceNo.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPurchaseInvoiceNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPurchaseOrderNo.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPurchaseOrderNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Control removed bg 2253lblDepartment.Caption = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lID:=ACDepartment, _
            'iDataType:=PMResString)


            chkShowDeleted.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACShowDeleted, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            chkShowBalance.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACShowBalance, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisableInterface
    '
    ' Description: Disables parts of the interface while a search is
    '              in progress.
    '
    ' ***************************************************************** '
    Private Function DisableInterface(ByRef bDisable As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add commands here eg.
            cmdOK.Enabled = Not bDisable
            If m_sCallingAppName = "iPMWrkComponentStarter" Then
               cmdEdit.Enabled = Not bDisable
            End If   
            cmdFindNow.Enabled = Not bDisable
            'JAS(CMG) issue ref:859 removing disabling of new search
            'cmdNewSearch.Enabled = Not bDisable

            ' Always disable these if the results listview is empty
            If lvwSearchResults.Items.Count = 0 Then
                bDisable = True
                cmdOK.Enabled = Not bDisable
                cmdEdit.Enabled = Not bDisable
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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
            stbStatus.Text = " " & sMessage

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

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
            'eck190600

            '    If (IsArray(m_vSearchData) = False) Then
            '        lItemsFound& = 0
            '    Else
            '        lItemsFound& = (UBound(m_vSearchData, 2) + 1)
            '    End If

            lItemsFound = lvwSearchResults.Items.Count

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _stbStatus_Panel1.Text = " " & lItemsFound & " " & sMessage

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

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
            ' At least one field must be populated
            If txtShortCode.Text.Trim() <> "" Then
                If txtShortCode.Text.Trim().Length >= ACMinSearchLength Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            If txtFullKey.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If cboLedger.SelectedIndex <> -1 Then
                If VB6.GetItemData(cboLedger, cboLedger.SelectedIndex) <> -1 Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            If txtName.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If cboAccountType.SelectedIndex <> -1 Then
                If VB6.GetItemData(cboAccountType, cboAccountType.SelectedIndex) <> -1 Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            If txtInsuranceRef.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If cboPMUser.UserID <> 0 Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtPurchaseOrderNo.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtPurchaseInvoiceNo.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch excep As System.Exception




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
        Else
            'PSL 10/06/2003 Iss4434
            If Not m_bInsurersAgents Then

                cmdFindNow.Enabled = False
            End If
        End If

    End Sub



    ' ***************************************************************** '
    ' Name: ResizeInterface
    '
    ' Description: Resizes the interface controls.
    '
    ' ***************************************************************** '
    Private Function ResizeInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdFindNow.Left = Me.Width - VB6.TwipsToPixelsX(1500)
            cmdNewSearch.Left = Me.Width - VB6.TwipsToPixelsX(1500)

            ImgImage.Left = Me.Width - VB6.TwipsToPixelsX(975)

            tabMain.Width = Me.Width - VB6.TwipsToPixelsX(1700)

            lvwSearchResults.Width = Me.Width - VB6.TwipsToPixelsX(360)
            lvwSearchResults.Height = Me.Height - VB6.TwipsToPixelsY(4000)

            cmdHelp.Left = Me.Width - VB6.TwipsToPixelsX(1335)
            cmdHelp.Top = Me.Height - VB6.TwipsToPixelsY(1110)

            cmdCancel.Left = Me.Width - VB6.TwipsToPixelsX(2535)
            cmdCancel.Top = Me.Height - VB6.TwipsToPixelsY(1110)

            cmdOK.Left = Me.Width - VB6.TwipsToPixelsX(3735)
            cmdOK.Top = Me.Height - VB6.TwipsToPixelsY(1110)

            cmdNew.Top = Me.Height - VB6.TwipsToPixelsY(1110)
            cmdEdit.Top = Me.Height - VB6.TwipsToPixelsY(1110)

            If cmdNavigate.Visible Then
                cmdNavigate.Top = Me.Height - VB6.TwipsToPixelsY(1110)
            End If

            Me.Refresh()

            Return result

        Catch





            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: FindNow
    '
    ' Description: Get the interface details from the query
    '
    ' ***************************************************************** '
    Private Sub FindNow()
        'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.10.2.2)
        Dim sWildcardErrorMessage As String = ""
        'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.10.2.2)

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.9.2.2)
            'Check wildcard searches

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtShortCode.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Transaction")
                txtShortCode.Focus()

                Exit Sub
            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtName.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Transaction")
                txtName.Focus()

                Exit Sub
            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtInsuranceRef.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Account")
                txtInsuranceRef.Focus()

                Exit Sub
            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtPurchaseOrderNo.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Account")
                txtPurchaseOrderNo.Focus()

                Exit Sub
            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtPurchaseInvoiceNo.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Account")
                txtPurchaseInvoiceNo.Focus()

                Exit Sub
            End If

            'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.9.2.2)


            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
            End If

            If lvwSearchResults.Items.Count > 0 Then
                VB6.SetDefault(cmdFindNow, False)
                VB6.SetDefault(cmdOK, False)
            End If

            ' Set the focus.
            lvwSearchResults.Focus()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the FindNow command", vApp:=ACApp, vClass:=ACClass, vMethod:="FindNow", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' PRIVATE Methods (End)

    Private Sub cboPMUser_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMUser.Click
        CheckMandatoryEnable()
    End Sub

    ' PRIVATE Events (Begin)

    Private Sub cmdAccountLookup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAccountLookup.Click

        Dim oInterface As iACTExplorer.Interface_Renamed
        Dim lError As Integer
        Dim vSetKeyArray As Object

        ' Create a new instance of the interface.
        Dim temp_oInterface As Object
        'Developer Guide No 218
        m_lReturn = g_oObjectManager.GetInstance(temp_oInterface, sClassName:="iACTExplorer.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oInterface = temp_oInterface

        With oInterface
            'lError& = .Initialise()


            .CallingAppName = "FindAccount"

            .CompanyID = g_iCompanyID

            ReDim vSetKeyArray(1, 0)

            vSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameExplorerReadOnly

            vSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = gPMConstants.PMEReturnCode.PMTrue

            lError = .SetKeys(vSetKeyArray)
            'EK111298   - make sure explorer always starts from top
            '       .StartKey = txtFullKey.Text

            .StartKey = ""

            lError = .Initialise()

            If lError = gPMConstants.PMEReturnCode.PMTrue Then

                lError = .Start()

                'MKW 07/06/04 PN12184 removed check as status will always return pmcancel from account explorer
                '            If (.Status = PMOK) Then


                If .AccountID > 0 Then 'PN22518
                    m_lStatus = gPMConstants.PMEReturnCode.PMOK
                Else
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                End If


                m_lAccountID = .AccountID

                m_iLedgerID = .LedgerID

                m_iLedgerTypeID = .LedgerTypeID

                m_sAccountName = .AccountName

                m_sShortCode = .ShortCode

                m_sFullKey = .FullKey
                'JK101298
                '                Me.Hide

                'JK101298 - return value back to interface
                txtFullKey.Text = m_sFullKey

                'EK111298 - fill interface with default select criteria
                txtShortCode.Text = m_sShortCode
                txtName.Text = m_sAccountName
                '            End If
            End If

            'JK101298
            '        lError& = .Terminate()

            .Dispose()
        End With

        oInterface = Nothing

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'developer guide no. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)


    End Sub

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the object parameters to default values
            m_iLedgerID = -1 ' Undefined
            m_iLedgerTypeID = -1
            m_sShortCode = ""
            m_sFullKey = ""

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iACTFindAccount.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=g_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'DD 15/07/2002: Get product option setting
            Dim vValue As String = ""
            iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnhancedOrionSecurity, g_iCompanyID, vValue)
            m_bEnhancedSecurity = (vValue = "1")

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

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

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Check if there is sufficient search criteria
            If CheckMandatory() <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Inadequate data so cannot
                ' continue with the search.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' {* USER DEFINED CODE (End) *}

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.


            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.

                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    'developer guide no.7(Latest guide)
                    eventArgs.Cancel = True
                    Cancel = 1

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()



            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown, iNewTab As Integer
        Dim bProcessed As Boolean = False
        Dim bTabChanged As Boolean = False

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
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


            'developer guide no.293

            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMain.SelectedIndex = 0
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                tabMain.SelectedIndex = 1
            End If
            ' If the key was processed
            If bProcessed Then
                KeyCode = 0
            End If

        Catch




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

            'Developer Guie no 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMain.SelectedIndex = 0
            End If

        Catch




            Exit Sub
        End Try


    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If
        m_lReturn = ResizeInterface()
    End Sub

    Private Sub tabMain_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMain.SelectedIndexChanged

        pnlMain(SSTabHelper.GetSelectedIndex(tabMain)).Enabled = True
        pnlMain(tabMainPreviousTab).Enabled = False
        If SSTabHelper.GetSelectedIndex(tabMain) = 1 Then
            txtInsuranceRef.Focus()
        End If
        With tabMain
            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMain) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                'm_ctlTabFirstLast(ACControlStart, .Tab).SetFocus
            End If
        End With

        tabMainPreviousTab = tabMain.SelectedIndex
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.
        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()


            ' Reset the properties so they dont get taken back to the calling app
            m_lAccountID = 0
            m_sShortCode = ""
            m_sAccountName = ""

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click
        ' Click event of the Find Now button.
        FindNow()
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




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNewSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Cancel button.
        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNew.Click

        '    ' Click event of the New Button.
        '
        '    On Error GoTo Err_cmdNew
        '
        '    ' {* USER DEFINED CODE (Begin) *}
        '
        ' *** Button currently invisible.  Currently one must use the
        ' *** Account Explorer to create an account. It was decided not
        ' *** be able to start that up from here.


        Exit Sub




        m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the New button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNew_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Exit Sub

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        Dim oInterface As iACTAccount.Interface_Renamed

        ' Click event of the Edit Button.

        Try

            ' {* USER DEFINED CODE (Begin) *}

            'Check before calling edit that any item is selecetd or not
            If lvwSearchResults.FocusedItem Is Nothing Then
                Exit Sub
            End If

            m_lReturn = DataToProperties()


            Dim temp_oInterface As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oInterface, sClassName:="iACTAccount.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oInterface = temp_oInterface

            '    ' Create a new instance of the interface.
            '    Set oInterface = CreateObject("iACTAccount.Interface")

            With oInterface

                .AccountID = m_lAccountID

                .CompanyID = g_iCompanyID

                .PMAuthorityLevel = m_lPMAuthorityLevel

                .Task = gPMConstants.PMEComponentAction.PMEdit
            End With


            m_lReturn = oInterface.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to start Account interface.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            ' Set the interface object to nothing.
            oInterface = Nothing

            ' {* USER DEFINED CODE (End) *}

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Edit button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchResults_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchResults.Enter

        ' GotFocus Event for the search details

        Try

            ' Unset any default buttons so can select by keys
            VB6.SetDefault(cmdFindNow, False)
            VB6.SetDefault(cmdOK, False)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchResults_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchResults_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchResults.Leave

        ' LostFocus Event for the search details

        Try

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchResults_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchResults_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchResults.Click

        Dim sShortCode As String = ""
        Dim sFullKey, sAccountName As String
        Dim iAccountStatusID As Integer

        Try

            ' CF 120199 - Changed to find it by long name as well as short code
            ' CF 050399 - This shouldnt be neccessary now as short code IS unique

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Populate controls from Data array for selected Account
            If lvwSearchResults.Items.Count > 0 Then
                'RKS 26/08/2004
                'developer guide no. 52
                sAccountName = lvwSearchResults.FocusedItem.SubItems(ACListIAccountName).Text
                sShortCode = lvwSearchResults.FocusedItem.Text

                ' loop around and get the other details...
                For iCount As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                    If (CStr(m_vSearchData(ACIAccountName, iCount)).Trim() = sAccountName) And (CStr(m_vSearchData(ACIShortCode, iCount)).Trim() = sShortCode) Then

                        txtShortCode.Text = CStr(m_vSearchData(ACIShortCode, iCount)).Trim()
                        txtName.Text = CStr(m_vSearchData(ACIAccountName, iCount)).Trim()

                        iAccountStatusID = CInt(m_vSearchData(ACIAccountStatusID, iCount))

                        ' Set combos from elements in list view, not data array.
                        '                cboAccountType.Text = lvwSearchResults.SelectedItem.SubItems(ACListIAccountType)
                        '                cboLedger.Text = lvwSearchResults.SelectedItem.SubItems(ACListILedger)

                        ' Get the Full Key

                        m_lReturn = g_oBusiness.GetFullKey(v_lAccountID:=CInt(m_vSearchData(ACIAccountID, iCount)), r_sFullKey:=sFullKey)
                        txtFullKey.Text = sFullKey

                    End If
                Next iCount

                ' CF110399
                If Not m_bAllowStoppedAccounts Then
                    ' CF 050399 - Only enable OK for active accounts

                    If iAccountStatusID = gACTLibrary.ACTAccountStatusActive Then
                        cmdOK.Enabled = True
                        VB6.SetDefault(cmdOK, True)
                    Else
                        cmdOK.Enabled = False
                        VB6.SetDefault(cmdCancel, True)
                    End If
                End If

            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchResults_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchResults_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchResults.DoubleClick

        ' Double click event for the search details.
        Try

            ' Check if there are any items available.
            If lvwSearchResults.Items.Count = 0 Then
                Exit Sub
            End If

            'RKS 26/08/2004
            'Check OK button status if disabled then do nothing
            If Not cmdOK.Enabled Then
                Exit Sub
            End If

            ' Set the interface status.

            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchResults_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchResults_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchResults.ColumnClick
        'Dim ColumnHeader As ColumnHeader = lvwSearchResults.Columns(eventArgs.Column)
        StoreHScrollValue()
        '' Column click event for the search details
        '' Defer to the common interface
        'OnColumnClick(lvwSearchResults, ColumnHeader)
        ListViewFunc.SortListView(lvwSearchResults, eventArgs)
        RecoverHorizontalScroll()
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

    Private Sub txtShortCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtShortCode.Enter
        ' Hightlight any text.
        iPMFunc.SelectText(txtShortCode)
    End Sub

    Private Sub txtShortCode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtShortCode.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
    End Sub

    Private Sub txtFullKey_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFullKey.Enter
        ' Hightlight any text.
        iPMFunc.SelectText(txtFullKey)
    End Sub

    Private Sub txtFullKey_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFullKey.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
    End Sub

    Private Sub cboLedger_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboLedger.SelectedIndexChanged
        CheckMandatoryEnable()
    End Sub

    Private Sub txtName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtName.Enter
        ' Hightlight any text.
        iPMFunc.SelectText(txtName)
    End Sub

    Private Sub txtName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtName.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
    End Sub

    Private Sub cboAccountType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAccountType.SelectedIndexChanged
        CheckMandatoryEnable()
    End Sub
    'developer guide 243

    ' PRIVATE Events (End)

    Private Sub _stbStatus_Panel1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _stbStatus_Panel1.Click

    End Sub

End Class
