Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
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
    ' Date: 14 Mar 2008
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    Private Const ACClass As String = "frmInterface"
    Public frmCancel As frmCancelPayment
    Public m_vSearchData(,) As Object
    Public m_vAllocationData(,) As Object
    Public m_vResultArray As Object

    Private Const vbFormCode As Integer = 0
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""

    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    Private m_lCashListID As Integer
    Private m_lCashListTypeID As Integer

    Private m_iCashListCompanyID As Integer

    Private m_oCashList As Object
    Private m_oCashListItem As Object

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iACTPaymentMaintenance.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    Private m_vSourceArray As Object

    Dim frmCancelPayment As frmCancelPayment

    Private m_oPMUser As bPMUser.Business
    Private m_vBranch As Integer

    Private m_iAllowReverseAllocation As Integer
    Private m_iReverseAllocationDays As Integer
    Private m_lEventTypeId As Integer
    Private m_iHasPaymentAuthority As Integer
    Private m_lPaymentCurrency As Integer
    Private m_crPaymentAmount As Decimal
    Private m_crAmt As Decimal
    Private m_crAmount As Decimal
    Private m_vMediaTypeArrayForCheque(,) As Object
    ' Declare an instance of the FormControl object
    ' Form control
    Private m_oFormFields As iPMFormControl.FormFields

    ' For Navigator Set Get Properties
    Private m_iBranchId As Integer
    Private m_iMaxRowToFetch As Integer
    Private m_iBankAccountId As Integer
    Private m_iUserId As Integer
    Private m_sClientCode As String = ""
    Private m_sClientAccNumber As String = ""
    Private m_sPayeeName As String = ""
    Private m_sPolicyClaimNumber As String = ""
    Private m_iPaymentTypeId As Integer
    Private m_iPaymentMediaTypeId As Integer
    Private m_vMediaFrom As String = ""
    Private m_vMediaTo As String = ""
    Private m_vAmoutFrom As String = ""
    Private m_vAmountTo As String = ""
    Private m_vDateFrom As String = ""
    Private m_vDateTo As String = ""
    Private m_iShowOnlyOutstanding As Integer
    Private m_bIsNavigatorProcess As Boolean
    Private m_iPaymentStatus As Integer
    Private m_sBatchReference As String = ""
    Private hScrollValue As Integer = 0

    'Win32 API declarations to preserve list view horizontal scroll position after sort
    Const LVM_FIRST As Int32 = &H1000
    Const LVM_SCROLL As Int32 = LVM_FIRST + 20
    Const SBS_HORZ As Integer = 0

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

    Public ReadOnly Property NavigatorTitle() As String
        Get

            ' Return the objects parameter value.
            Return m_sNavigatorTitle

        End Get
    End Property

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property

    Public WriteOnly Property SourceArray() As Object
        Set(ByVal Value As Object)

            ' Set the valid sources for the user
            m_vSourceArray = Value

        End Set
    End Property


    Public Property BranchId() As Integer
        Get

            Return m_iBranchId

        End Get
        Set(ByVal Value As Integer)

            m_iBranchId = Value

        End Set
    End Property


    'UPGRADE_NOTE: (7001) The following declaration (get MaxRowToFetch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function MaxRowToFetch() As Integer
    '
    'Return m_iMaxRowToFetch
    '
    'End Function
    Public WriteOnly Property MaxRowToFetch() As Integer
        Set(ByVal Value As Integer)

            m_iMaxRowToFetch = Value

        End Set
    End Property


    Public Property BankAccountId() As Integer
        Get

            Return m_iBankAccountId

        End Get
        Set(ByVal Value As Integer)

            m_iBankAccountId = Value

        End Set
    End Property


    'UPGRADE_NOTE: (7001) The following declaration (get UserId) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function UserId() As Integer
    '
    'Return m_iUserId
    '
    'End Function
    Public WriteOnly Property UserId() As Integer
        Set(ByVal Value As Integer)

            m_iUserId = Value

        End Set
    End Property


    Public Property ClientCode() As String
        Get

            Return m_sClientCode

        End Get
        Set(ByVal Value As String)

            m_sClientCode = Value

        End Set
    End Property


    Public Property ClientAccNumber() As String
        Get

            Return m_sClientAccNumber

        End Get
        Set(ByVal Value As String)

            m_sClientAccNumber = Value

        End Set
    End Property


    Public Property PayeeName() As String
        Get

            Return m_sPayeeName

        End Get
        Set(ByVal Value As String)

            m_sPayeeName = Value

        End Set
    End Property


    Public Property PolicyClaimNumber() As String
        Get

            Return m_sPolicyClaimNumber

        End Get
        Set(ByVal Value As String)

            m_sPolicyClaimNumber = Value

        End Set
    End Property


    Public Property PaymentTypeId() As Integer
        Get

            Return m_iPaymentTypeId

        End Get
        Set(ByVal Value As Integer)

            m_iPaymentTypeId = Value

        End Set
    End Property


    Public Property PaymentMediaTypeId() As Integer
        Get

            Return m_iPaymentMediaTypeId

        End Get
        Set(ByVal Value As Integer)

            m_iPaymentMediaTypeId = Value

        End Set
    End Property

    Public Property MediaFrom() As String
        Get

            Return m_vMediaFrom

        End Get
        Set(ByVal Value As String)

            m_vMediaFrom = Value

        End Set
    End Property


    Public Property MediaTo() As String
        Get

            Return m_vMediaTo

        End Get
        Set(ByVal Value As String)


            m_vMediaTo = CStr(Value)

        End Set
    End Property


    Public Property AmoutFrom() As String
        Get

            Return m_vAmoutFrom

        End Get
        Set(ByVal Value As String)


            m_vAmoutFrom = CStr(Value)

        End Set
    End Property


    Public Property DateFrom() As String
        Get

            Return m_vDateFrom

        End Get
        Set(ByVal Value As String)


            m_vDateFrom = CStr(Value)

        End Set
    End Property


    Public Property DateTo() As String
        Get

            Return m_vDateTo

        End Get
        Set(ByVal Value As String)


            m_vDateTo = CStr(Value)

        End Set
    End Property


    Public Property AmountTo() As String
        Get

            Return m_vAmountTo

        End Get
        Set(ByVal Value As String)


            m_vAmountTo = CStr(Value)

        End Set
    End Property


    Public Property ShowOnlyOutstanding() As Integer
        Get

            Return m_iShowOnlyOutstanding

        End Get
        Set(ByVal Value As Integer)

            m_iShowOnlyOutstanding = Value

        End Set
    End Property


    Public Property IsNavigatorProcess() As Boolean
        Get

            Return m_bIsNavigatorProcess

        End Get
        Set(ByVal Value As Boolean)

            m_bIsNavigatorProcess = Value

        End Set
    End Property


    Public Property PaymentStatus() As Integer
        Get

            Return m_iPaymentStatus

        End Get
        Set(ByVal Value As Integer)

            m_iPaymentStatus = Value

        End Set
    End Property


    Public Property BatchReference() As String
        Get

            Return m_sBatchReference

        End Get
        Set(ByVal Value As String)

            m_sBatchReference = Value

        End Set
    End Property
    <DllImport("user32.dll")> _
    Private Shared Function GetScrollPos(ByVal hWnd As System.IntPtr, ByVal nBar As Integer) As Integer

    End Function
    <DllImport("user32.dll")> _
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As Integer, ByVal lParam As Integer) As Boolean

    End Function
    <DllImport("user32.dll")> _
    Private Shared Function LockWindowUpdate(ByVal Handle As IntPtr) As Boolean

    End Function
    'Store the horizontal scroll value.
    Private Sub StoreHScrollValue()
        hScrollValue = GetScrollPos(lvwFindParty.Handle, SBS_HORZ)
    End Sub
    'Recover the old scroll position
    Private Sub RecoverHorizontalScroll()
        LockWindowUpdate(lvwFindParty.Handle)
        'Calculate the value the scroll needs to scroll back.
        Dim dx As Integer = hScrollValue - GetScrollPos(lvwFindParty.Handle, SBS_HORZ)
        'Send the scroll message.
        Dim b As Boolean = SendMessage(lvwFindParty.Handle, LVM_SCROLL, dx, 0)
        LockWindowUpdate(IntPtr.Zero)

    End Sub

    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetStatus"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()


        Catch ex As Exception
            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PerformSearch
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function PerformSearch() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PerformSearch"
        Dim iCompanyID, iBankAccountId As Integer
        Dim dtStartDate, dtEndDate As Date
        Dim lItemCount As Integer

        Dim iBranch As Integer
        Dim sClientCode, sClientAccountNumber, sPolicyClaimNumber, sMediaFrom, sMediaTo As String
        Dim dAmountFrom, dAmountTo As Double

        Dim sPayeeName As String = ""
        Dim iPaymentTypeId, iPaymentMediaTypeId, iPaymentStatusID As Integer
        Dim sBatchRef As String = ""
        Dim iShowOnlyOutstanding As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' Display a searching message.
            DisplayStatusSearching()

            ' Disable parts of the interface while
            ' a search is in progress.
            m_lReturn = DisableInterface(bDisable:=True)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}
            iCompanyID = g_iCompanyID

            ' Bank: If any list item other than (Any) selected
            If uctBankAccount.Id > 0 Then
                iBankAccountId = uctBankAccount.Id
            Else
                ' Must be (Any) so send -1
                iBankAccountId = -1
            End If

            ' Date From
            If Information.IsDate(txtDateFrom.Text) Then
                dtStartDate = CDate(txtDateFrom.Text)
            Else
                dtStartDate = #12/30/1899#
            End If

            ' Date To
            If Information.IsDate(txtDateTo.Text) Then
                dtEndDate = CDate(txtDateTo.Text)
            Else
                dtEndDate = #12/30/1899#
            End If

            ' Branch ID
            If cboBranch.SelectedIndex > 0 Then
                iBranch = VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)
            Else
                ' Must be (Any) so send -1
                iBranch = -1
            End If

            ' Payment Type ID
            If cboPaymentType.ListIndex > 0 Then
                iPaymentTypeId = cboPaymentType.ItemData(cboPaymentType.ListIndex)
            Else
                ' Must be (Any) so send -1
                iPaymentTypeId = -1
            End If

            ' Media Type ID
            If cboMediaType.ListIndex > 0 Then
                iPaymentMediaTypeId = cboMediaType.ItemData(cboMediaType.ListIndex)
            Else
                ' Must be (Any) so send -1
                iPaymentMediaTypeId = -1
            End If

            ' Payment Status ID
            If cboPaymentStatus.ListIndex > 0 Then
                iPaymentStatusID = cboPaymentStatus.ItemData(cboPaymentStatus.ListIndex)
            Else
                ' Must be (Any) so send -1
                iPaymentStatusID = -1
            End If

            ' Client Code
            If pnlClientCode.Text <> "" Then
                sClientCode = pnlClientCode.Text
            Else
                sClientCode = ""
            End If

            ' Client Account Number
            sClientAccountNumber = txtClientAccountNumber.Text
            If txtClientAccountNumber.Text <> "" Then
                m_lReturn = gPMFunctions.ConvertWildCardsForSQL(r_sTextString:=sClientAccountNumber)
            Else
                sClientAccountNumber = ""
            End If

            ' Policy Claim Number
            sPolicyClaimNumber = txtPolicyClaimNumber.Text
            If txtPolicyClaimNumber.Text <> "" Then
                m_lReturn = gPMFunctions.ConvertWildCardsForSQL(r_sTextString:=sPolicyClaimNumber)
            Else
                sPolicyClaimNumber = ""
            End If

            ' Media From

            If txtMediaReferenceFrom.Text <> "" Then
                sMediaFrom = txtMediaReferenceFrom.Text
            Else
                sMediaFrom = ""
            End If

            ' Media To

            If txtMediaReferenceTo.Text <> "" Then
                sMediaTo = txtMediaReferenceTo.Text
            Else
                sMediaTo = ""
            End If

            ' Amount Range From
            Dim dbNumericTemp As Double
            If txtAmountRangeFrom.Text <> "" And Double.TryParse(txtAmountRangeFrom.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                dAmountFrom = CDbl(txtAmountRangeFrom.Text)
            Else
                dAmountFrom = 0
            End If

            ' Amount Range To
            Dim dbNumericTemp2 As Double
            If txtAmountRangeTo.Text <> "" And Double.TryParse(txtAmountRangeTo.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                dAmountTo = CDbl(txtAmountRangeTo.Text)
            Else
                dAmountTo = 0
            End If

            ' Payee Name
            If txtPayeeName.Text <> "" Then
                sPayeeName = txtPayeeName.Text
            Else
                sPayeeName = ""
            End If

            ' Batch Reference
            sBatchRef = txtBatchReference.Text
            If txtBatchReference.Text <> "" Then
                m_lReturn = gPMFunctions.ConvertWildCardsForSQL(r_sTextString:=sBatchRef)
            Else
                sBatchRef = ""
            End If

            ' Show Outstanding
            iShowOnlyOutstanding = chkShowOnlyOutstanding.CheckState
            If iShowOnlyOutstanding = StringsHelper.ToDoubleSafe("1") Then

            Else
                iShowOnlyOutstanding = 0
            End If

            'Call bACTFindCashListItem.SearchPaymentDetails

            m_lReturn = g_oACTCashListBusiness.SearchPaymentDetails(r_lNumberOfRecords:=ACMaxSearchDetails, r_vResultArray:=m_vSearchData, v_vBranchID:=iBranch, v_vPayeeName:=sPayeeName, v_vPaymentTypeID:=iPaymentTypeId, v_vPaymentMediaTypeID:=iPaymentMediaTypeId, v_vPaymentStatusID:=iPaymentStatusID, v_vBatchReference:=sBatchRef, v_vBankAccountID:=iBankAccountId, v_vClientCode:=sClientCode, v_vClientAccountNumber:=sClientAccountNumber, v_vPolicyClaimNumber:=sPolicyClaimNumber, v_vMediaFrom:=sMediaFrom, v_vMediaTo:=sMediaTo, v_vAmountFrom:=dAmountFrom, v_vAmountTo:=dAmountTo, v_vStartDate:=dtStartDate, v_vEndDate:=dtEndDate, v_vShowOnlyOutStanding:=iShowOnlyOutstanding, v_vUserID:=g_iUserID)


            ' Check the return values.
            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Found search details.

                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' No found search details

                Case Else
                    ' Failed to get details.
                    ' Raise Error.
                    gPMFunctions.RaiseError(kMethodName, "Failed to get search details from the business object")

            End Select

            ' Display the number of item found message.
            DisplayStatusFound()


        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
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
        Const kMethodName As String = "DataToInterface"
        Dim oListItem As ListViewItem

        Dim sClientCode, sPolicyHolder, sPolicyClaimNumber As String
        Dim dAmount As Decimal
        Dim dtPaymentDate As Date
        Dim sMediaReference, sPaymentStatus, sCancelReason As String
        Dim dtCancellationDate As Date
        Dim sAccountNumber, sBankSortCode, sBranchCode, sBankAccount, sBatchReference, sTheirReference, sOurReference, sDocumentReference, sPaymentType, sMediaType, sUsername, sPayeeName As String

        Dim iReverseReasonID, iAllowReverseAllocation, iReverseAllocationDays As Integer
        Dim lCashListItemID, lTransDetailID, lPartyCnt, lInsuranceFileCnt, lClaimId, lCurrencyId As Integer
        Dim dtBankReconcilationDate As Date

        Const ACFindImage As String = "FindImage"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the search details.
            lvwFindParty.Items.Clear()

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vSearchData) Then
                Return result
            End If

            ' Assign the details to the interface.

            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                ' Assign details to other the columns
                sClientCode = CStr(m_vSearchData(ACPaymentClientCode, lRow))
                oListItem = lvwFindParty.Items.Add(sClientCode.Trim())

                sPolicyHolder = CStr(m_vSearchData(ACPaymentPolicyHolder, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexPolicyHolder).Text = sPolicyHolder.Trim()

                sPolicyClaimNumber = CStr(m_vSearchData(ACPaymentPolicyClaimNumber, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexPolicyClaimNumber).Text = sPolicyClaimNumber.Trim()

                'So as to do -ve amount +ve
                dAmount = gPMFunctions.ToSafeDecimal(m_vSearchData(ACPaymentAmount, lRow))
                If dAmount < 0 Then
                    dAmount *= -1
                End If
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexAmount).Text = gPMFunctions.ToSafeString(dAmount)

                If Information.IsDate(m_vSearchData(ACPaymentPaymentDate, lRow)) Then
                    dtPaymentDate = CDate(m_vSearchData(ACPaymentPaymentDate, lRow))
                    ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexPaymentDate).Text = dtPaymentDate.ToString("dd MMM yyyy")
                End If

                sMediaReference = CStr(m_vSearchData(ACPaymentMediaReference, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexMediaReference).Text = sMediaReference.Trim()

                sPaymentStatus = CStr(m_vSearchData(ACPaymentPaymentStatus, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexPaymentStatus).Text = sPaymentStatus.Trim()

                If Information.IsDate(m_vSearchData(ACPaymentBankReconcilationDate, lRow)) Then
                    dtBankReconcilationDate = CDate(m_vSearchData(ACPaymentBankReconcilationDate, lRow))
                    ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexBankReconcilationDate).Text = dtBankReconcilationDate.ToString("dd MMM yyyy")
                End If

                sCancelReason = CStr(m_vSearchData(ACPaymentCancelReason, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexCancelReason).Text = sCancelReason.Trim()

                If Information.IsDate(m_vSearchData(ACPaymentCancellationDate, lRow)) Then
                    dtCancellationDate = CDate(m_vSearchData(ACPaymentCancellationDate, lRow))
                    ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexCancellationDate).Text = dtCancellationDate.ToString("dd MMM yyyy")
                End If

                'PN: 47700
                sAccountNumber = CStr(m_vSearchData(ACPaymentAccountCode, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexAccountNumber).Text = sAccountNumber.Trim()

                'PN: 47700
                sBankSortCode = CStr(m_vSearchData(ACPaymentAccountBankBranchCode, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexBankSortCode).Text = sBankSortCode.Trim()

                sBranchCode = CStr(m_vSearchData(ACPaymentBranchCode, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexBranchCode).Text = sBranchCode.Trim()

                sBankAccount = CStr(m_vSearchData(ACPaymentBankAccount, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexBankAccount).Text = sBankAccount.Trim()

                sBatchReference = CStr(m_vSearchData(ACPaymentBatchReference, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexBatchReference).Text = sBatchReference.Trim()

                sTheirReference = CStr(m_vSearchData(ACPaymentTheirReference, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexTheirReference).Text = sTheirReference.Trim()

                sOurReference = CStr(m_vSearchData(ACPaymentOurReference, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexOurReference).Text = sOurReference.Trim()

                sDocumentReference = CStr(m_vSearchData(ACPaymentDocumentReference, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexDocumentReference).Text = sDocumentReference.Trim()

                sPaymentType = CStr(m_vSearchData(ACPaymentPaymentType, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexPaymentType).Text = sPaymentType.Trim()

                sMediaType = CStr(m_vSearchData(ACPaymentMediaType, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexMediaType).Text = sMediaType.Trim()

                sUsername = CStr(m_vSearchData(ACPaymentUser, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexUser).Text = sUsername.Trim()

                sPayeeName = CStr(m_vSearchData(ACPaymentPayeeName, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexPayeeName).Text = sPayeeName.Trim()

                iReverseReasonID = gPMFunctions.ToSafeInteger(m_vSearchData(ACPaymentReverseReasonID, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexReverseReasonID).Text = CStr(iReverseReasonID)

                iAllowReverseAllocation = gPMFunctions.ToSafeInteger(m_vSearchData(ACPaymentAllowReverseAllocation, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexAllowReverseAllocation).Text = CStr(iAllowReverseAllocation)

                iReverseAllocationDays = gPMFunctions.ToSafeInteger(m_vSearchData(ACPaymentReverseAllocationDays, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexReverseAllocationDays).Text = CStr(iReverseAllocationDays)

                lCashListItemID = gPMFunctions.ToSafeLong(m_vSearchData(ACPaymentCashListItemID, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexCashListItemID).Text = CStr(lCashListItemID)

                lTransDetailID = gPMFunctions.ToSafeLong(m_vSearchData(ACPaymentTransDetailID, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexTransDetailID).Text = CStr(lTransDetailID)

                lPartyCnt = gPMFunctions.ToSafeLong(m_vSearchData(ACPaymentPartyCnt, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexPartyCnt).Text = CStr(lPartyCnt)

                lInsuranceFileCnt = gPMFunctions.ToSafeLong(m_vSearchData(ACPaymentInsuranceFileCnt, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexInsuranceFileCnt).Text = CStr(lInsuranceFileCnt)

                lClaimId = gPMFunctions.ToSafeLong(m_vSearchData(ACPaymentClaimId, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexClaimId).Text = CStr(lClaimId)

                lCurrencyId = gPMFunctions.ToSafeLong(m_vSearchData(ACPaymentTranCurrencyId, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kPaymentMaintColHIndexCurrencyId).Text = CStr(lCurrencyId)

                oListItem.Tag = CStr(lRow)
                'End If
            Next lRow

            ' Select the first item.
            lvwFindParty.Items.Item(0).Selected = True

            ' Enable the interface now that the search
            ' has completed.
            m_lReturn = DisableInterface(bDisable:=False)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
            End If


        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
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
        Const kMethodName As String = "DataToProperties"
        Dim lSelectedItem As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.

            lSelectedItem = Convert.ToString(lvwFindParty.Items.Item(lvwFindParty.FocusedItem.Index).Tag)


        Catch ex As Exception

            ' Error Section.
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ********************************************************************************* '
    ' Name: Private Function                                                            '
    '                                                                                   '
    ' Description: Checks that the transaction is for one of the branches being paid    '
    '                                                                                   '
    ' ********************************************************************************* '
    'UPGRADE_NOTE: (7001) The following declaration (ValidSource) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ValidSource(ByVal vSource As Object) As Boolean
    '
    'Dim result As Boolean = False
    'If Not Information.IsArray(m_vSourceArray) Then
    'Return True
    'End If
    'For 'i As Integer = 1 To m_vSourceArray.GetUpperBound(1)

    'If CInt(m_vSourceArray(1, i)) = CInt(vSource) Then
    'result = True
    'End If
    'Next i
    'Return result
    'End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetInterfaceDefaults"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'Fill Branchs
            m_lReturn = GetBranchDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwFindParty.Handle.ToInt32(), v_vShowRowSelect:=True)
            lvwFindParty.FullRowSelect = True


        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally




        End Try
        Return result
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
        Const kMethodName As String = "ClearInterface"
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display the message.
            iMsgResult = MessageBox.Show("A new search will clear all of your existing search details." & Strings.Chr(13) & Strings.Chr(10) & _
                         "Do you wish to continue?", "New Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            ' Check message result.
            If iMsgResult = System.Windows.Forms.DialogResult.No Then
                ' Don't continue with the clear.
                Return result
            End If

            ' Clear the interface details.

            ' Clear the search data array.
            m_vSearchData = Nothing

            ' Clear the search list details.
            lvwFindParty.Items.Clear()

            ' Clear the search status bar.
            _stbStatus_Panel1.Text = ""
            stbStatus.Refresh()

            txtDateFrom.Text = ""
            txtDateTo.Text = ""
            uctBankAccount.Id = 0
            cboBranch.SelectedIndex = 0
            pnlClientCode.Text = ""
            txtClientAccountNumber.Text = ""
            txtPayeeName.Text = ""
            txtPolicyClaimNumber.Text = ""
            cboPaymentType.ListIndex = 0
            cboMediaType.ListIndex = 0
            txtBatchReference.Text = ""
            cboPaymentStatus.ListIndex = 0
            txtMediaReferenceFrom.Text = ""
            txtMediaReferenceTo.Text = ""
            txtAmountRangeFrom.Text = ""
            txtAmountRangeTo.Text = ""
            txtDateFrom.Text = ""
            txtDateTo.Text = ""
            chkShowOnlyOutstanding.CheckState = CheckState.Unchecked

            ' Set focus to the search details.
            cboBranch.Focus()

            ' Disable parts of the interface, so the
            ' user can now only enter a new search
            m_lReturn = DisableInterface(bDisable:=True)

            With lvwFindParty
                '.SortOrder = (.SortOrder + 1) Mod 2
                '.SortKey = ColumnHeader.Index - 1
                ListViewHelper.SetSortedProperty(lvwFindParty, True)
            End With



        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DisplayCaptions"
        Dim sAnyText As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Raise Error.
                gPMFunctions.RaiseError("DisplayCaptions", "Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                        "Please check the file exists and the correct captions are available")
                Return result

            End If

            sAnyText = "Any"

            uctBankAccount.FirstItem = sAnyText
            cboPaymentType.FirstItem = sAnyText
            cboMediaType.FirstItem = sAnyText
            cboPaymentStatus.FirstItem = sAnyText



        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

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
    Private Function DisableInterface(ByRef bDisable As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DisableInterface"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            cmdOK.Enabled = Not bDisable



        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally




        End Try
        Return result
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
                sMessage = "Searching. Please wait.."
            End If

            ' Display the status message.
            _stbStatus_Panel1.Text = " " & sMessage
            stbStatus.Refresh()


        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

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
            If Not Information.IsArray(m_vSearchData) Then
                lItemsFound = 0
            Else
                lItemsFound = (m_vSearchData.GetUpperBound(1) + 1)
            End If

            ' Get message text if not already present.
            If sMessage = "" Then
                sMessage = "Item(s) found"
            End If

            ' Display the status message.
            _stbStatus_Panel1.Text = " " & lItemsFound & " " & sMessage
            stbStatus.Refresh()


        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
    End Sub
    ' PRIVATE Methods (End)

    Private Sub cmdAddTask_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddTask.Click

        Try



            m_lReturn = CreateWorkManagerTask()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If



        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Task Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddTask_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        Finally


        End Try
    End Sub

    Private Sub cmdCancelPayment_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancelPayment.Click

        Const kMethodName As String = "cmdCancelPayment_Click"
        Dim oListItem As ListViewItem


        Try



            If m_iAllowReverseAllocation <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("User is not Authorised to Reverse Payment Allocation", "Find Payment", MessageBoxButtons.OK)
                Exit Sub
            End If

            'Check if already been cancelled
            If lvwFindParty.FocusedItem Is Nothing Then
                lvwFindParty.Items.Item(0).Focused = True
            End If
            If lvwFindParty.FocusedItem.Index >= 0 Then
                If gPMFunctions.ToSafeInteger(CInt(lvwFindParty.FocusedItem.SubItems.Item(kPaymentMaintColHIndexReverseReasonID).Text)) > 0 Then

                    cmdCancelPayment.Enabled = False
                    MessageBox.Show("Payment is already been cancelled", "Find Payment", MessageBoxButtons.OK)
                    Exit Sub
                End If
            End If

            frmCancel = New frmCancelPayment()



            frmCancel.ExistingCancelPayment = m_vResultArray

            ClearCancelPaymentScreen()


            With frmCancel
                .TransDetailID = gPMFunctions.ToSafeLong(lvwFindParty.FocusedItem.SubItems.Item(kPaymentMaintColHIndexTransDetailID).Text)
                .Amount = gPMFunctions.ToSafeCurrency(lvwFindParty.FocusedItem.SubItems.Item(kPaymentMaintColHIndexAmount).Text)
                'PN: 45889
                .MediaRef = gPMFunctions.ToSafeString(lvwFindParty.FocusedItem.SubItems.Item(kPaymentMaintColHIndexMediaReference).Text)
                .BankAcccountNo = gPMFunctions.ToSafeString(lvwFindParty.FocusedItem.SubItems.Item(kPaymentMaintColHIndexBankAccount).Text)
                .PolicyHolder = gPMFunctions.ToSafeString(lvwFindParty.FocusedItem.SubItems.Item(kPaymentMaintColHIndexPolicyHolder).Text)
                .PaymentDate = gPMFunctions.ToSafeDate(lvwFindParty.FocusedItem.SubItems.Item(kPaymentMaintColHIndexPaymentDate).Text)
                .MediaType = gPMFunctions.ToSafeString(lvwFindParty.FocusedItem.SubItems.Item(kPaymentMaintColHIndexMediaType).Text)
                .BankSortCode = gPMFunctions.ToSafeString(lvwFindParty.FocusedItem.SubItems.Item(kPaymentMaintColHIndexBankSortCode).Text)
                .DocumentRef = gPMFunctions.ToSafeString(lvwFindParty.FocusedItem.SubItems.Item(kPaymentMaintColHIndexDocumentReference).Text)
                .ClientCode = gPMFunctions.ToSafeString(lvwFindParty.FocusedItem.Text)

                .CashListItemID = CStr(gPMFunctions.ToSafeLong(lvwFindParty.FocusedItem.SubItems.Item(kPaymentMaintColHIndexCashListItemID).Text))
                .PartyCnt = gPMFunctions.ToSafeLong(lvwFindParty.FocusedItem.SubItems.Item(kPaymentMaintColHIndexPartyCnt).Text)

                .InsuranceFileCnt = gPMFunctions.ToSafeLong(lvwFindParty.FocusedItem.SubItems.Item(kPaymentMaintColHIndexInsuranceFileCnt).Text)
                .ClaimId = gPMFunctions.ToSafeLong(lvwFindParty.FocusedItem.SubItems.Item(kPaymentMaintColHIndexClaimId).Text)
                .EventTypeId = gPMFunctions.ToSafeLong(m_lEventTypeId)

                .TransCurrencyId = gPMFunctions.ToSafeLong(lvwFindParty.FocusedItem.SubItems.Item(kPaymentMaintColHIndexCurrencyId).Text)

                .HasPaymentAuthority = gPMFunctions.ToSafeInteger(m_iHasPaymentAuthority)
                .PaymentCurrency = gPMFunctions.ToSafeLong(m_lPaymentCurrency)
                .PaymentAmount = gPMFunctions.ToSafeCurrency(m_crPaymentAmount)


                .PolicyClaimRef = gPMFunctions.ToSafeString(lvwFindParty.FocusedItem.SubItems.Item(kPaymentMaintColHIndexPolicyClaimNumber).Text)

                m_lReturn = .FillProperties()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                m_lReturn = .FillCancelGrid()
                If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                    Exit Sub
                End If

            End With


            frmCancel.ShowDialog()


            frmCancel = Nothing
            m_lReturn = m_oGeneral.GetInterfaceDetails()
            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                Exit Sub
            End If


        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Client Add command button", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
    End Sub

    Private Sub cmdClientCode_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClientCode.Click

        Dim vCnt As Object
        Dim vShortName As String = ""
        Dim vName As Object
        Const kMethodName As String = "cmdClientCode_Click"


        Try



            m_lReturn = SelectParty(vPartyCnt:=CInt(vCnt), vShortName:=vShortName, vName:=CStr(vName))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Raise Error
                gPMFunctions.RaiseError(v_sSource:=kMethodName, v_sDescription:="Failed to Process Select Party")
                Exit Sub
            End If

            pnlClientCode.Text = vShortName



        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process Select Party, cmdClientCode_Click", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        ''m_lReturn& = ShowHelp(dlgHelp, ScreenHelpID)
    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            frmCancelPayment = New frmCancelPayment

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iACTPaymentMaintenance.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            Dim temp_m_oPMUser As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPMUser = temp_m_oPMUser

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Raise Error.
                gPMFunctions.RaiseError("Form_Initialize", "Failed to get PMUser")
                Exit Sub

            End If

            m_oFormFields = New iPMFormControl.FormFields()
            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        Catch ex As Exception

            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialize interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initilize", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Const knMediaTypeCheque As Integer = 5
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

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Raise Error
                gPMFunctions.RaiseError("Form_Load", "Failed to set the status for the business object")
                Exit Sub
            End If

            'Validate fields using Forms Control
            m_lReturn = SetFieldValidation()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            m_lReturn = SetUserReverseAllcation()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Navigator Process
            If IsNavigatorProcess Then
                m_lReturn = RunNavigatorProcess()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                End If
            End If

            m_lReturn = EnableDisableCancelButton()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If


            m_lReturn = g_oBusiness.GetArrayForMediaTypeValidationId(v_iMediaTypeValidationID:=knMediaTypeCheque, v_vMediaTypeArray:=m_vMediaTypeArrayForCheque)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            cboBranch.Select()
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        If (eventArgs.Cancel = True) Then
            ' Forms query unload event.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                eventArgs.Cancel = True
            End If

        End If
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
                    Cancel = 1
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            If Not (m_oPMUser Is Nothing) Then


                m_oPMUser.Dispose()
                m_oPMUser = Nothing
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()

            

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)



        Catch ex As Exception

            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            eventArgs.Cancel = Cancel <> 0
        End Try
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



        Catch ex As Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try



            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If



        Catch ex As Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
    End Sub

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click

        ' Click event of the Cancel button.
        Dim bIsValid As Boolean
        Try



            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = ValidateInput(bIsValid)
            If Not bIsValid Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If
            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
            End If

            m_lReturn = EnableDisableCancelButton()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Failed in EnableDisableCancel method
            End If
            ' Set the focus.
            lvwFindParty.Focus()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Find Now command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdFindNow_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
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



        Catch ex As Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNewSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
    End Sub

    Private Sub lvwFindParty_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwFindParty.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwFindParty.Columns(eventArgs.Column)

        ' Column click event for the search details

        Try

            StoreHScrollValue()
            If lvwFindParty.Items.Count > 0 Then
                ' OnColumnClick(lvwFindParty, ColumnHeader)
                ListViewFunc.SortListView(lvwFindParty, eventArgs)
            End If
            RecoverHorizontalScroll()


        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwFindParty_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
    End Sub

    Private Sub lvwFindParty_ItemClick(ByVal Item As ListViewItem)

        m_lReturn = EnableDisableCancelButton()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

    End Sub

    Private Sub lvwFindParty_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwFindParty.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        m_lReturn = EnableDisableCancelButton()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

    End Sub

    Private Sub txtAmountRangeFrom_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtAmountRangeFrom.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then
            If (KeyAscii < 48 Or KeyAscii > 57) And (KeyAscii <> 46) Then
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtAmountRangeTo_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtAmountRangeTo.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then
            If (KeyAscii < 48 Or KeyAscii > 57) And (KeyAscii <> 46) Then
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtBatchReference_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtBatchReference.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then
            If (KeyAscii < 48 Or KeyAscii > 57) And (KeyAscii < 65 Or KeyAscii > 90) And (KeyAscii < 97 Or KeyAscii > 122) And KeyAscii <> 37 Then 'PN 61824
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtClientAccountNumber_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtClientAccountNumber.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then
            If (KeyAscii < 48 Or KeyAscii > 57) And (KeyAscii < 65 Or KeyAscii > 90) And (KeyAscii < 97 Or KeyAscii > 122) And (KeyAscii <> 37) Then 'PN 61824
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtDateFrom_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateFrom.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDateFrom)

    End Sub

    Private Sub txtDateFrom_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateFrom.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDateFrom)

    End Sub
    Private Sub txtDateTo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDateTo)

    End Sub

    Private Sub txtDateTo_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDateTo)

    End Sub

    ' ***************************************************************** '
    ' Name: GetBranchDetails
    '
    ' Description: Gets all of the branch details
    '
    ' Updates: taken from PartyPC
    '
    ' ***************************************************************** '
    Private Function GetBranchDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetBranchDetails"

        Dim vSourceArray(,) As Object

        Dim lReturn, lDefaultCurrencyId As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Only populate combo with addresses the user is authorised to access.

            m_lReturn = m_oPMUser.GetUserSources(r_vSourceArray:=vSourceArray, v_vUserID:=g_iUserID, v_bIncludeDeletedSources:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get branch details for the dropdown list")
            End If

            'PN 19428

            m_vSourceArray = vSourceArray

            'Clear combo.
            cboBranch.Items.Clear()

            Dim cboBranch_NewIndex As Integer = -1
            cboBranch_NewIndex = cboBranch.Items.Add("(Any)")
            VB6.SetItemData(cboBranch, cboBranch_NewIndex, 0)

            'Populate branch combo


            For i As Integer = 0 To vSourceArray.GetUpperBound(1)

                cboBranch_NewIndex = cboBranch.Items.Add(vSourceArray(2, i))

                VB6.SetItemData(cboBranch, cboBranch_NewIndex, CInt(vSourceArray(0, i)))

                If CInt(vSourceArray(0, i)) = m_vBranch Then
                    cboBranch.SelectedIndex = cboBranch_NewIndex
                End If
            Next i
            cboBranch.SelectedIndex = 0



        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SelectParty
    '
    ' Description: Call Find Party component to choose a party
    '
    ' ***************************************************************** '
    Private Function SelectParty(ByRef vPartyCnt As Integer, ByRef vShortName As String, Optional ByRef vName As String = "", Optional ByRef vSpecialParty As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SelectParty"

        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim vKeyArray(,) As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            oFindParty = New iPMBFindParty.Interface_Renamed

            'Set appropriate key if agent only


            If (Not Information.IsNothing(vSpecialParty)) And (Not String.IsNullOrEmpty(vSpecialParty)) Then

                ReDim vKeyArray(1, 0)

                vKeyArray(0, 0) = "special_party"

                vKeyArray(1, 0) = vSpecialParty

                m_lReturn = oFindParty.SetKeys(vKeyArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    oFindParty = Nothing
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If

            m_lReturn = oFindParty.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
            oFindParty.AllowAgentSearch = True
            oFindParty.CallingAppName = "uctInvoiceAccount"

            m_lReturn = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            oFindParty.IgnoreDPAQuestions = True

            m_lReturn = oFindParty.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
                vPartyCnt = oFindParty.PartyCnt
                vShortName = oFindParty.ShortName

                If Not Information.IsNothing(vName) Then
                    vName = oFindParty.LongName
                End If
            End If

            oFindParty.Dispose()

            oFindParty = Nothing



        Catch ex As Exception

            ' Log Error
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    Private Function CreateWorkManagerTask() As Integer
        Dim result As Integer = 0
        Dim bPMLookup, iPMWrkTaskInstance As Object

        Const kMethodName As String = "SelectParty"

        Dim oWrkTaskInstance As iPMWrkTaskInstance.NavigatorV3

        Dim oPMLookUp As bPMLookup.Business
        Dim lTaskID, lTaskGroupID As Integer
        Dim vKeys As Object
        Dim sTaskDesc As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vKeys(1, 26)

            ' Change the cursor mode
            iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Object to create work manager tasks
            Dim temp_oWrkTaskInstance As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oWrkTaskInstance, "iPMWrkTaskInstance.NavigatorV3", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oWrkTaskInstance = temp_oWrkTaskInstance
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get an instance of iPMWrkTaskInstance.NavigatorV3")
                ' Change the cursor mode
                iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Set to ADD mode
            m_lReturn = oWrkTaskInstance.NavigatorV3_SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "oWrkTaskInstance.NavigatorV3_SetProcessModes Failed")
                ' Change the cursor mode
                iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Set the authority level
            ''oWrkTaskInstance.NavigatorV3_PMAuthorityLevel = m_lPMAuthorityLevel&


            ' Create an instance of bPMLookup
            Dim temp_oPMLookUp As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLookUp, "bPMLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLookUp = temp_oPMLookUp
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get an instance of bPMLookup.Business")
                ' Change the cursor mode
                iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Set the product family

            oPMLookUp.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusUnderwriting

            ' Use the lookup to get the ID of the PMTMAINT task

            m_lReturn = oPMLookUp.GetEffectiveIDFromCode(v_sTableName:="pmwrk_task", v_sCode:="PMTMAINT", v_dtEffectiveDate:=DateTime.Now, r_lID:=lTaskID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to GetEffectiveIDFromCode for " & Environment.NewLine & _
                                        "TableName: pmwrk_task" & Environment.NewLine & _
                                        "Code: PMTMAINT" & Environment.NewLine & _
                                        "EffectiveDate: " & DateTimeHelper.ToString(DateTime.Today))
                iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Use the lookup to get the ID of the SLACS task group

            m_lReturn = oPMLookUp.GetEffectiveIDFromCode(v_sTableName:="pmwrk_task_group", v_sCode:="SLACS", v_dtEffectiveDate:=DateTime.Now, r_lID:=lTaskGroupID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to GetEffectiveIDFromCode for " & Environment.NewLine & _
                                        "TableName: pmwrk_task_group" & Environment.NewLine & _
                                        "Code: SLACS" & Environment.NewLine & _
                                        "EffectiveDate: " & DateTimeHelper.ToString(DateTime.Today))
                ' Change the cursor mode
                iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Remove instance of lookup

            oPMLookUp.Dispose()
            oPMLookUp = Nothing

            sTaskDesc = "Payment Maintenance"
            ' Set up the key array

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameTaskGroupCode

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = "SLACS"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameTaskID

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = lTaskID

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameTaskCode

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = "PMTMAINT"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameTaskDescription

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = sTaskDesc

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameTaskCustomer

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = "Customer"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameTaskDueDate

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = DateTime.Today.AddDays(1).AddSeconds(-1)

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameTaskIsUrgent

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = gPMConstants.PMEReturnCode.PMTrue

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.PMKeyNameTaskGroupID

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = lTaskGroupID

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = "BranchId"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = "MaxRowToFetch"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = ACMaxSearchDetails

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = "BankAccountId"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = uctBankAccount.Id

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 11) = "UserId"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 11) = g_iUserID

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 12) = "ClientCode"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 12) = pnlClientCode.Text.Trim()

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 13) = "ClientAccNumber"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 13) = txtClientAccountNumber.Text.Trim()

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 14) = "PayeeName"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 14) = txtPayeeName.Text.Trim()

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 15) = "PolicyClaimNumber"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 15) = txtPolicyClaimNumber.Text.Trim()

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 16) = "PaymentTypeId"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 16) = cboPaymentType.ItemId

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 17) = "PaymentMediaTypeId"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 17) = cboMediaType.ItemId

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 18) = "PaymentStatusId"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 18) = cboPaymentStatus.ItemId

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 19) = "BatchReference"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 19) = txtBatchReference.Text.Trim()

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 20) = "MediaFrom"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 20) = txtMediaReferenceFrom.Text.Trim()

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 21) = "MediaTo"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 21) = txtMediaReferenceTo.Text.Trim()

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 22) = "AmoutFrom"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 22) = txtAmountRangeFrom.Text

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 23) = "AmountTo"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 23) = txtAmountRangeTo.Text

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 24) = "DateFrom"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 24) = txtDateFrom.Text

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 25) = "DateTo"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 25) = txtDateTo.Text

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 26) = "ShowOnlyOutstanding"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 26) = chkShowOnlyOutstanding.CheckState

            ' Pass the keys in
            m_lReturn = oWrkTaskInstance.NavigatorV3_SetKeys(vKeyArray:=vKeys)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to set keys.")
                iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Change the cursor mode
            iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Display the form

            m_lReturn = oWrkTaskInstance.NavigatorV3_Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to start WrkTaskInstance")
            End If


            If oWrkTaskInstance.NavigatorV3_Status = gPMConstants.PMEReturnCode.PMCancel Then
                result = gPMConstants.PMEReturnCode.PMCancel
            End If



        Catch ex As Exception

            ' Log Error
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            ' Terminate the object

            oWrkTaskInstance.Dispose()


        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: SetUserReverseAllcation
    '
    ' Description: Checks whether login user reversal authorities
    '
    ' ***************************************************************** '

    Private Function SetUserReverseAllcation() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetUserReverseAllcation"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oBusiness.GetUserReverseAllocation(vResultArray:=m_vAllocationData, v_UserID:=g_iUserID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                Return result
            End If

            If Not Information.IsArray(m_vAllocationData) Then
                m_iAllowReverseAllocation = 0
                m_iReverseAllocationDays = 0
                m_iHasPaymentAuthority = 0
                m_lPaymentCurrency = 0
                m_crPaymentAmount = 0
            Else
                m_iAllowReverseAllocation = gPMFunctions.ToSafeInteger(m_vAllocationData(ACAllowReverse, 0))
                m_iReverseAllocationDays = gPMFunctions.ToSafeInteger(m_vAllocationData(ACReverseDays, 0))
                m_iHasPaymentAuthority = gPMFunctions.ToSafeInteger(m_vAllocationData(ACHasPaymentAuthority, 0))
                m_lPaymentCurrency = gPMFunctions.ToSafeLong(m_vAllocationData(ACPaymentCurrencyId, 0))
                m_crPaymentAmount = gPMFunctions.ToSafeCurrency(m_vAllocationData(ACPaymentAmt, 0))
            End If

            ' Get Event Id

            m_lReturn = g_oBusiness.GetEventTypeId(vResultArray:=m_vAllocationData, v_sEventCode:=ACEventCode)

            If Not Information.IsArray(m_vAllocationData) Then
                m_lEventTypeId = 0
            Else
                m_lEventTypeId = gPMFunctions.ToSafeLong(m_vAllocationData(ACEventTypeId, 0))
            End If



        Catch ex As Exception

            ' Log Error
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: EnableDisableCancelButton
    '
    ' Description: Disables Enable Cancel Payment Button
    '
    ' ***************************************************************** '
    Private Function EnableDisableCancelButton() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "EnableDisableCancelButton"
        Dim iReverseAllocationDays As Integer
        Dim dtPaymentDate As Date
        Dim crPaymentAmt, crPayAmt, crTransAmt As Decimal
        Dim lTransCurrId As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Disable when user has no rights to Reverse-Allocation
            If m_iAllowReverseAllocation <> gPMConstants.PMEReturnCode.PMTrue Then
                cmdCancelPayment.Enabled = False
                'EnableDisableCancelButton = PMFalse
                Return result
            End If

            If lvwFindParty.Items.Count = 0 Then
                cmdCancelPayment.Enabled = False
                'EnableDisableCancelButton = PMFalse
                Return result
            End If

            'Check if already been cancelled
            'changes as focused item is not set by default
            If IsNothing(lvwFindParty.FocusedItem) = True Then
                lvwFindParty.FocusedItem = lvwFindParty.SelectedItems.Item(0)
            End If

            If lvwFindParty.FocusedItem.Index >= 0 Then
                If gPMFunctions.ToSafeInteger(lvwFindParty.FocusedItem.SubItems.Item(kPaymentMaintColHIndexReverseReasonID).Text) > 0 Then

                    cmdCancelPayment.Enabled = False
                ElseIf m_iAllowReverseAllocation = 1 Then
                    'Check if Reverse Allocation Days exceeds
                    ' iReverseAllocationDays = gPMFunctions.ToSafeInteger(lvwFindParty.FocusedItem.SubItems.Item(kPaymentMaintColHIndexReverseAllocationDays).Text)

                    'Take the Allocation days of current user
                    iReverseAllocationDays = m_iReverseAllocationDays
                    dtPaymentDate = gPMFunctions.ToSafeDate(lvwFindParty.FocusedItem.SubItems.Item(kPaymentMaintColHIndexPaymentDate).Text)
                    dtPaymentDate = dtPaymentDate.AddDays(iReverseAllocationDays)
                    cmdCancelPayment.Enabled = dtPaymentDate >= DateTime.Today

                End If
            End If
            ' Has Payment Authority
            If m_iHasPaymentAuthority = gPMConstants.PMEReturnCode.PMTrue Then
                m_crAmount = gPMFunctions.ToSafeCurrency(lvwFindParty.FocusedItem.SubItems.Item(kPaymentMaintColHIndexAmount).Text)

                crTransAmt = m_crAmount
                If crTransAmt < 0 Then
                    crTransAmt = -(m_crAmount)
                End If
                lTransCurrId = gPMFunctions.ToSafeLong(lvwFindParty.FocusedItem.SubItems.Item(kPaymentMaintColHIndexCurrencyId).Text)
                ' Do Conversion of Currency

                m_lReturn = g_obACTCurrencyConvert.CurrencyToCurrencyConversion(v_lCurrencyIdFrom:=lTransCurrId, v_crCurrencyAmountFrom:=crTransAmt, v_lCompanyId:=g_iSourceID, v_lCurrencyIdTo:=m_lPaymentCurrency, r_crCurrencyAmountTo:=crPayAmt)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Currency Conversion Failed", "Cancel Payment", MessageBoxButtons.OK)
                    Return result
                End If

                ' User Authority allow Payment Cancel Amount
                crPaymentAmt = m_crPaymentAmount

                ' Checks Transaction Converted amount with
                ' User Authority allow Payment Amount
                If crPayAmt > crPaymentAmt Then
                    cmdCancelPayment.Enabled = False
                    Return result
                End If

            End If



        Catch ex As Exception
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ClearCancelPaymentScreen
    '
    ' Description: Clears CancelPayment Screen
    '
    ' ***************************************************************** '

    Private Sub ClearCancelPaymentScreen()
        Const kMethodName As String = "ClearCancelPaymentScreen"

        Try



            With frmCancel

                .txtClientCode.Text = ""
                .txtPolicyHolder.Text = ""
                .txtAmount.Text = ""
                .txtPaymentDate.Text = ""
                .txtMediaRef.Text = ""
                .txtMediaType.Text = ""
                .txtBankAccNo.Text = ""
                .txtBankSortCode.Text = ""
                .txtDocRef.Text = ""
                .cboCancelledReason.ListIndex = 0
            End With



        Catch ex As Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed clearing cancel detail screen", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearCancelPaymentScreen", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
    End Sub

    Private Function ValidateInput(ByRef r_bIsValid As Boolean) As Integer
        Dim bIsCheque As Boolean

        Try

            bIsCheque = False
            r_bIsValid = True

            If Information.IsArray(m_vMediaTypeArrayForCheque) Then
                For iCount As Integer = 0 To m_vMediaTypeArrayForCheque.GetUpperBound(1)
                    If gPMFunctions.ToSafeInteger(m_vMediaTypeArrayForCheque(iCount, 0)) = cboMediaType.ItemId Then
                        bIsCheque = True
                        Exit For
                    End If
                Next
            End If

            If txtPayeeName.Text.Trim().Length <> 0 Then
                If Not ValidateString(txtPayeeName.Text, "") Then
                    MessageBox.Show("Specified Payee Name is not valid", "Payee Name Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtPayeeName.Focus()
                    r_bIsValid = False
                    Exit Function
                End If
            End If

            If txtClientAccountNumber.Text.Trim().Length <> 0 Then
                If Not ValidateString(txtClientAccountNumber.Text, "") Then
                    MessageBox.Show("Specified Client Account Number is not valid", "Client Account Number Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtClientAccountNumber.Focus()
                    r_bIsValid = False
                    Exit Function
                End If
            End If

            If txtPolicyClaimNumber.Text.Trim().Length <> 0 Then
                If Not ValidateString(txtPolicyClaimNumber.Text, "/\-_") Then
                    MessageBox.Show("Specified Policy/Claim Number is not valid", "Policy/Claim Number Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtPolicyClaimNumber.Focus()
                    r_bIsValid = False
                    Exit Function
                End If
                If (txtPolicyClaimNumber.Text.IndexOf("/"c) + 1) = 1 Or (txtPolicyClaimNumber.Text.IndexOf("\"c) + 1) = 1 Or (txtPolicyClaimNumber.Text.IndexOf("-"c) + 1) = 1 Or (txtPolicyClaimNumber.Text.IndexOf("_"c) + 1) = 1 Then
                    MessageBox.Show("Specified Policy/Claim Number is not valid", "Policy/Claim Number Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtPolicyClaimNumber.Focus()
                    r_bIsValid = False
                    Exit Function
                End If
            End If

            If txtBatchReference.Text.Trim().Length <> 0 Then
                If Not ValidateString(txtBatchReference.Text, "") Then
                    MessageBox.Show("Specified Batch Reference is not valid", "Batch Reference Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtBatchReference.Focus()
                    r_bIsValid = False
                    Exit Function
                End If
            End If

            If txtAmountRangeFrom.Text.Trim().Length <> 0 Then
                Dim dbNumericTemp As Double
                If Not Double.TryParse(txtAmountRangeFrom.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    MessageBox.Show("Specified Amount Range From is not valid.Only numeric values up to 15 digits are allowed", "Media Ref Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtAmountRangeFrom.Focus()
                    r_bIsValid = False
                    Exit Function
                End If
            End If

            If txtAmountRangeTo.Text.Trim().Length <> 0 Then
                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(txtAmountRangeTo.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                    MessageBox.Show("Specified Amount Range To is not valid.Only numeric values up to 15 digits are allowed", "Media Ref Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtAmountRangeTo.Focus()
                    r_bIsValid = False
                    Exit Function
                End If
            End If

            If bIsCheque Then
                If txtMediaReferenceFrom.Text.Trim().Length <> 0 Then
                    Dim dbNumericTemp3 As Double
                    If Not Double.TryParse(txtMediaReferenceFrom.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Or Strings.Len(txtMediaReferenceFrom.Text) > 10 Or txtMediaReferenceFrom.Text.IndexOf("."c) >= 0 Then
                        MessageBox.Show("Specified Media Ref From is not valid.Only numeric values up to 10 digits are allowed", "Media Ref Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        txtMediaReferenceFrom.Focus()
                        r_bIsValid = False
                        Exit Function
                    End If
                End If
                If txtMediaReferenceTo.Text.Trim().Length <> 0 Then
                    Dim dbNumericTemp4 As Double
                    If Not Double.TryParse(txtMediaReferenceTo.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Or Strings.Len(txtMediaReferenceTo.Text) > 10 Or txtMediaReferenceTo.Text.IndexOf("."c) >= 0 Then
                        MessageBox.Show("Specified Media Ref To is not valid.Only numeric values up to 10 digits are allowed", "Media Ref Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        txtMediaReferenceTo.Focus()
                        r_bIsValid = False
                        Exit Function
                    End If
                End If
            Else
                If txtMediaReferenceFrom.Text.Trim().Length <> 0 Then
                    If Not ValidateString(txtMediaReferenceFrom.Text, "/\-_.") Then
                        MessageBox.Show("Specified Media Ref From is not valid", "Media Ref Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        txtMediaReferenceFrom.Focus()
                        r_bIsValid = False
                        Exit Function
                    End If

                    If (txtMediaReferenceFrom.Text.IndexOf("/"c) + 1) = 1 Or (txtMediaReferenceFrom.Text.IndexOf("\"c) + 1) = 1 Or (txtMediaReferenceFrom.Text.IndexOf("-"c) + 1) = 1 Or (txtMediaReferenceFrom.Text.IndexOf("_"c) + 1) = 1 Or (txtMediaReferenceFrom.Text.IndexOf("."c) + 1) = 1 Then
                        MessageBox.Show("Specified Media Ref From is not valid", "Media Ref Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        txtMediaReferenceFrom.Focus()
                        r_bIsValid = False
                        Exit Function
                    End If
                End If
                If txtMediaReferenceTo.Text.Trim().Length <> 0 Then
                    If Not ValidateString(txtMediaReferenceTo.Text, "/\-_.") Then
                        MessageBox.Show("Specified Media Ref To is not valid", "Media Ref Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        txtMediaReferenceTo.Focus()
                        r_bIsValid = False
                        Exit Function
                    End If

                    If (txtMediaReferenceTo.Text.IndexOf("/"c) + 1) = 1 Or (txtMediaReferenceTo.Text.IndexOf("\"c) + 1) = 1 Or (txtMediaReferenceTo.Text.IndexOf("-"c) + 1) = 1 Or (txtMediaReferenceTo.Text.IndexOf("_"c) + 1) = 1 Or (txtMediaReferenceTo.Text.IndexOf("."c) + 1) = 1 Then
                        MessageBox.Show("Specified Media Ref To is not valid.", "Media Ref Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        txtMediaReferenceTo.Focus()
                        r_bIsValid = False
                        Exit Function
                    End If

                    If txtPolicyClaimNumber.Text.Trim().Length <> 0 Then
                        If Not ValidateString(txtPolicyClaimNumber.Text, "/\-_") Then
                            MessageBox.Show("Policy/Claim Number is not valid", "Policy/Claim Number Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            txtPolicyClaimNumber.Focus()
                            r_bIsValid = False
                            Exit Function
                        End If
                    End If
                End If
            End If


        Catch ex As Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate Inputs", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateInput", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        Finally

        End Try
    End Function

    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            'From Date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDateFrom, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'To Date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDateTo, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function RunNavigatorProcess() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RunNavigatorProcess"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For i As Integer = 0 To cboBranch.Items.Count - 1
                If VB6.GetItemData(cboBranch, i) = BranchId Then
                    cboBranch.SelectedIndex = i
                    Exit For
                End If
            Next
            uctBankAccount.Id = BankAccountId
            pnlClientCode.Text = ClientCode
            txtClientAccountNumber.Text = ClientAccNumber
            txtPayeeName.Text = PayeeName
            txtPolicyClaimNumber.Text = PolicyClaimNumber
            cboPaymentType.ItemId = PaymentTypeId
            cboMediaType.ItemId = PaymentMediaTypeId
            txtMediaReferenceFrom.Text = MediaFrom
            txtMediaReferenceTo.Text = MediaTo
            txtAmountRangeFrom.Text = AmoutFrom
            txtAmountRangeTo.Text = AmountTo
            txtDateFrom.Text = DateFrom
            txtDateTo.Text = DateTo
            chkShowOnlyOutstanding.CheckState = ShowOnlyOutstanding
            cboPaymentStatus.ItemId = PaymentStatus
            txtBatchReference.Text = BatchReference

            m_lReturn = PerformSearch()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get Perform Search")
            End If
            m_lReturn = DataToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to call DataToInterface")
            End If


        Catch ex As Exception
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    Private Sub txtMediaReferenceFrom_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtMediaReferenceFrom.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then
            If (KeyAscii < 48 Or KeyAscii > 57) And (KeyAscii <> 32) And (KeyAscii <> 45) And (KeyAscii <> 46) And (KeyAscii <> 47) And (KeyAscii <> 92) And (KeyAscii <> 95) And (KeyAscii < 65 Or KeyAscii > 90) And (KeyAscii < 97 Or KeyAscii > 122) And (KeyAscii <> 37) Then 'PN 61824
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtMediaReferenceTo_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtMediaReferenceTo.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then
            If (KeyAscii < 48 Or KeyAscii > 57) And (KeyAscii <> 32) And (KeyAscii <> 45) And (KeyAscii <> 46) And (KeyAscii <> 47) And (KeyAscii <> 92) And (KeyAscii <> 95) And (KeyAscii < 65 Or KeyAscii > 90) And (KeyAscii < 97 Or KeyAscii > 122) And (KeyAscii <> 37) Then 'PN 61824
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtPayeeName_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtPayeeName.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        If KeyAscii <> 8 Then
            If (KeyAscii < 48 Or KeyAscii > 57) And (KeyAscii < 65 Or KeyAscii > 90) And (KeyAscii < 97 Or KeyAscii > 122) And (KeyAscii <> 37) Then 'PN 61824
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtPolicyClaimNumber_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtPolicyClaimNumber.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then
            If (KeyAscii < 48 Or KeyAscii > 57) And (KeyAscii <> 32) And (KeyAscii <> 45) And (KeyAscii <> 47) And (KeyAscii <> 92) And (KeyAscii <> 95) And (KeyAscii < 65 Or KeyAscii > 90) And (KeyAscii < 97 Or KeyAscii > 122) And (KeyAscii <> 37) Then 'PN 61824
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub
    Private Function ValidateString(ByRef s_InputString As String, ByRef s_AllowedSpecialChars As String) As Boolean
        Dim result As Boolean = False
        Dim iKeyAscii As Integer

        result = True
        For iLen As Integer = 1 To s_InputString.Length
            iKeyAscii = Strings.Asc(Mid(s_InputString, iLen, 1)(0))

            If (iKeyAscii < 48 Or iKeyAscii > 57) And (iKeyAscii < 65 Or iKeyAscii > 90) And (iKeyAscii < 97 Or iKeyAscii > 122) And iKeyAscii <> 37 Then
                If s_AllowedSpecialChars.IndexOf(Mid(s_InputString, iLen, 1)) >= 0 Then
                    result = True
                Else
                    result = False
                    Exit For
                End If
            End If
        Next
        Return result
    End Function

End Class
