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
    ' Date: 02/09/1999
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' RAW 09/01/2003 : catchup : added code from old sourcesafe
    ' PWF 06/11/2003 : Major overhaul for SFU ONLY!
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.

    Private Const ACClass As String = "frmInterface"

    Private Const ACMAXCOLUMNWIDTH As Integer = 3000

    ' Standard object parameter members.
    Private m_bProcessComplete As Boolean
    Private m_bNavCompleted As Boolean
    Dim bInstalmentByDuedate As Boolean = False

    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer

    Private m_lNavigate As gPMConstants.PMENavigateButtonStatus
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    Private m_iTask As Integer
    Private m_bCancelled As Boolean

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer


    ' Declare an instance of the general interface object.
    Private m_oGeneral As iACTInsurerPaymentSFU.General

    ' Find Transaction object for "Drill"ing
    Private m_oFindTransaction As Object
    Private m_oAccount As Object
    Private m_oCurrencyConvert As Object
    Private m_oCurrency As Object
    Private m_oUserAuthority As Object

    ' Declare an instance of the Business object.
    '--Added for Remittance Advice Agency Report

    Private m_oBusiness As iPMBReportPrint.Interface_Renamed

    ' Declare an instance of the nav starter for processing payments
    'Private WithEvents m_oNavStart As iPMNavStart.Interface_Renamed
    Private WithEvents m_oNavigatorXM As iPMNavigatorXM.Interface_Renamed
    ''m_oNavigatorXM.VB_VarHelpID = -1


    ' Last size variables for screen resizing
    Private m_lWidth As Integer
    Private m_lHeight As Integer

    ' Collection class for transactions
    Private m_cTransactions As Collection
    Private m_lTotalItems As Integer

    ' The default marked currency.
    ' All transactions in one action MUST be of the same currency.
    Private m_lActiveCurrencyID As Integer

    ' Current total of marked items
    Private m_cTotalMarked As Decimal

    ' Account details
    Private m_lAccountId As Integer
    Private m_sLedgerCode As String = ""

    ' Payment groups and branches
    Private m_bFromGroups As Boolean
    Private m_vPaymentGroups(,) As Object
    Private m_vSourceArray(,) As Object
    Private m_vPaySources() As Object

    Private m_lIsReportGenerated As gPMConstants.PMEReturnCode

    'Intermediary Writeoff
    Private m_sWriteOffAccountCode As String = ""
    Private m_bHasWriteOffAuthority As Boolean
    Private m_lWriteOffAmount As Decimal
    Private m_cTransOutstandingAmt As Decimal
    Private m_sTransKey As String = ""
    Private m_lWriteOffAcc_id As Integer
    Private HasWriteoff As Boolean

    Private bPayButtonClicked As Boolean
    Private bAllocateButtonClicked As Boolean
    Private m_cUnallocatedAmountForPost As Decimal
    Private m_bIsUnallocatedAmountForPost As Boolean

    Private m_lCurItemIndex As Integer = 0
    Private m_ottnew As ToolTip
    Private m_oTT As CTooltip
    Public m_sTransType As String = ""
    Private m_sAgentType As String
    Private m_bReciptAmountEntered As Boolean
    Private m_cReciptAmount As Decimal
    Private m_iCurrencyId As Integer
    Private m_iMarkedcurrencyId As Integer
    Private m_bMarkedTransaction As Boolean
    Private m_sDocRef As String
    Private m_bReciptOrPay As Boolean 'if true then recipt else pay
    Private m_sPartyType As String
    Private m_iPreviousCurrencyId As Integer
    Private m_iSourceId As Integer
    Private m_bTransactionRightButton As Boolean
    Private m_bInstalmentRightButton As Boolean
    Private m_bPaidByClient As Boolean

    Const m_kRootNode As String = "UserConfigXML"
    Const m_kComponentName As String = "iACTInsurerPaymentSFU"
    Const m_kFrmName As String = "frmInterface"
    Const m_kTransGridName As String = "lvwTransactions"
    Const m_kEntriesGridName As String = "lvwEntries"
    Const m_kColumnSortKey As String = "ColumnSortKey"
    Const m_kColumnSortOrder As String = "ColumnSortOrder"
    Const m_kColumnWidth As String = "ColumnWidths"
    Private m_iTransSortKey As Integer
    Private m_iTransSortOrder As Integer
    Private m_iEntriesSortKey As Integer
    Private m_iEntriesSortOrder As Integer
    Private Const vbFormCode As Integer = 0

    Private m_nBatchID As Integer
    Private m_sView As String = ""
    Private m_sAllocationCallingAppName As String
    Private m_oSearchData(,) As Object

    Private hScrollValue As Integer = 0
    'Win32 API declarations to preserve list view horizontal scroll position after sort
    Const LVM_FIRST As Int32 = &H1000
    Const LVM_SCROLL As Int32 = LVM_FIRST + 20
    Const SBS_HORZ As Integer = 0

    ' *******************************************************************************
    ' PUBLIC PROPERTIES
    ' *******************************************************************************
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
        hScrollValue = GetScrollPos(lvwTransactions.Handle, SBS_HORZ)
    End Sub
    'Recover the old scroll position
    Private Sub RecoverHorizontalScroll()
        LockWindowUpdate(lvwTransactions.Handle)
        'Calculate the value the scroll needs to scroll back.
        Dim dx As Integer = hScrollValue - GetScrollPos(lvwTransactions.Handle, SBS_HORZ)
        'Send the scroll message.
        Dim b As Boolean = SendMessage(lvwTransactions.Handle, LVM_SCROLL, dx, 0)
        LockWindowUpdate(IntPtr.Zero)
    End Sub

    Public Property accountID() As Integer
        Get
            ' Return the objects parameter value.
            Return m_lAccountId
        End Get
        Set(ByVal Value As Integer)
            ' Set the object parameter value.
            m_lAccountId = Value
        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            ' Set the effective date.
            m_dtEffectiveDate = Value
        End Set
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get
            ' Return any error number that might have occurred on the interface.
            Return m_lErrorNumber
        End Get
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            ' Set the navigate flag.
            m_lNavigate = Value
        End Set
    End Property

    Public ReadOnly Property NavigatorTitle() As String
        Get
            ' Return the objects parameter value.
            Return m_sNavigatorTitle
        End Get
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            ' Set the process mode.
            m_lProcessMode = Value
        End Set
    End Property

    Public WriteOnly Property SourceArray() As Object(,)
        Set(ByVal Value As Object(,))
            ' Set the valid sources for the user
            m_vSourceArray = Value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            ' Return the interface exit status.
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            ' Set the interface exit status.
            m_lStatus = Value
        End Set
    End Property

    Public ReadOnly Property StepStatus() As String
        Get
            ' Return the Steps Status
            Return m_sStepStatus.Value
        End Get
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            ' Set the type of business.
            m_sTransactionType = Value
        End Set
    End Property


    Public Property BatchID() As Integer
        Get
            Return m_nBatchID
        End Get
        Set(ByVal Value As Integer)
            m_nBatchID = Value
        End Set
    End Property

    Public Property AllocationView() As String
        Get
            Return m_sView
        End Get
        Set(ByVal Value As String)
            m_sView = Value
        End Set
    End Property

    Public Property AllocationCallingAppName() As String
        Get
            Return m_sAllocationCallingAppName
        End Get
        Set(ByVal Value As String)
            m_sAllocationCallingAppName = Value
        End Set
    End Property

    ' Updates the property member from the search data storage.
    Public Function DataToProperties() As Integer
        ' Not required for this component
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function


    ' Displays all of the lookup details using the lookup values/details.
    Public Function DisplayLookupDetails() As Integer
        ' Nothing to do
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    ''' <summary>
    ''' Retrieves the details from the business object.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBusiness() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim nAccountID As Integer
        Dim oDateTo As Object
        Dim bDateByTrans As Integer
        Dim nMarkedStatus, nMonth As Integer
        Dim nLedgerID As Integer
        Dim sAlternateRef As String = ""
        Dim oDueDateFrom As Object
        Dim oDueDateTo As Object
        Dim sReference As String
        Dim bGrossAgent As Boolean
        Dim nMediaTypeId As Integer
        Try

            ToolStripStatusLabel.Text = " " & GetCaption(ACStatusSearching)

            If m_sView IsNot Nothing AndAlso m_sView = PMEComponentAction.PMView Then
                m_lReturn = g_oBusiness.SearchDetailsForBatch(nBatchID:=m_nBatchID,
                                                              r_oaResultArray:=m_oSearchData)

                Select Case m_lReturn
                    Case PMEReturnCode.PMTrue
                    Case PMEReturnCode.PMNotFound
                    Case Else
                        RaiseError("GetBusiness", "Failed to get search details from the business object.", vbObjectError)
                End Select
            Else
                nAccountID = m_lAccountId
                If txtDueDateTo.Checked Then
                    oDateTo = txtDueDateTo.Value
                Else
                    oDateTo = Nothing
                End If

                If txtDueDateFrom.Checked Then
                    oDueDateFrom = txtDueDateFrom.Value.Date
                Else
                    oDueDateFrom = Nothing
                End If

                If txtDueDateTo.Checked Then
                    oDueDateTo = txtDueDateTo.Value.Date
                Else
                    oDueDateTo = Nothing
                End If


                If optEffectiveDate.Checked Then
                    bDateByTrans = 0
                ElseIf optTransDate.Checked Then
                    bDateByTrans = 1
                ElseIf optDueDate.Checked Then
                    bDateByTrans = 2
                End If
                nMarkedStatus = cboMarkedStatus.SelectedIndex - 1
                nMonth = cboMonth.SelectedIndex
                sAlternateRef = txtAlternateRef.Text.Trim()
                If chkDateFilterToinstalmentDueDates.Checked Then
                    bInstalmentByDuedate = True
                Else
                    bInstalmentByDuedate = False
                End If
                sReference = Trim(txtReference.Text)
                bGrossAgent = (optGross.Checked = True)
                If cboMediaType.ItemId = 0 Then
                    nMediaTypeId = 0
                Else
                    nMediaTypeId = cboMediaType.ItemId
                End If

                m_lReturn = GetLedger(v_lAccountID:=nAccountID,
                                      v_lLedgerId:=nLedgerID,
                                      v_sLedgerCode:=m_sLedgerCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", GetBusiness, " +
                                               CStr(CDbl("Failed to get ledger information for account id: ") + nAccountID))
                End If

                Dim nCnt As Integer = 0
                Dim oQueryStringYear As Object()
                Dim oQueryStringPeriod As Object()
                Dim sSplit As String() = Nothing
                Dim sAllocationPeriod As String
                Dim nCount As Integer = 0

                For nCnt = 0 To chkAllocationPeriod.CheckedItems.Count - 1
                    ReDim Preserve oQueryStringYear(nCnt)
                    ReDim Preserve oQueryStringPeriod(nCnt)
                    sAllocationPeriod = chkAllocationPeriod.CheckedItems.Item(nCnt).ToString
                    sSplit = sAllocationPeriod.Split("|")
                    oQueryStringPeriod(nCnt) = sSplit(0)
                    For nCount = 1 To sSplit.Length - 1
                        If Trim(sSplit(nCount)) <> "" Then
                            oQueryStringYear(nCnt) = sSplit(nCount)
                            Exit For
                        End If
                    Next
                    nCount = 1
                Next
                Dim nCurrencyId As Integer = 0
                If optViewByTransaction.Checked Then
                    nCurrencyId = uctTransactionCurrency.CurrencyId
                End If

                m_lReturn = g_oBusiness.SearchDetails(v_vAccountID:=nAccountID,
                                                      v_vDateTo:=oDateTo,
                                                      v_bDateByTrans:=bDateByTrans,
                                                      v_lMarkedStatus:=nMarkedStatus,
                                                      v_lMonth:=nMonth,
                                                      v_sLedgerCode:=m_sLedgerCode,
                                                      v_sAlternateRef:=sAlternateRef,
                                                      v_vDueDateFrom:=oDueDateFrom,
                                                      v_vDueDateTo:=oDueDateTo,
                                                      r_vResultArray:=m_oSearchData,
                                                      vQueryStringyear:=oQueryStringYear,
                                                      vQueryStringPeriod:=oQueryStringPeriod,
                                                      v_iCurrencyId:=nCurrencyId,
                                                      v_bInstalmentByDuedate:=bInstalmentByDuedate,
                                                      v_sReference:=sReference,
                                                      v_bGrossAgent:=bGrossAgent,
                                                      v_lMediaTypeId:=nMediaTypeId)

                Select Case m_lReturn
                    Case gPMConstants.PMEReturnCode.PMTrue
                    Case gPMConstants.PMEReturnCode.PMNotFound
                    Case Else
                        Throw New System.Exception(Constants.vbObjectError.ToString() +
                                                   ", GetBusiness, Failed to get search details from the business object.")
                End Select

            End If

            ' Copy the details into our collection
            m_lReturn = DataToCollection(m_oSearchData)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() +
                                           ", GetBusiness, Failed to create transaction collection")
            End If

            ' Lock the process for this insurer account
            m_lReturn = LockInsurer(v_lAccountID:=nAccountID)
            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                Throw New System.Exception(Constants.vbObjectError.ToString() +
                                           ", GetBusiness, Failed to lock account.")
            ElseIf (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                nResult = m_lReturn
            End If

            Return nResult
        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=excep.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return IIf(Information.Err().Number = Constants.vbObjectError,
                       gPMConstants.PMEReturnCode.PMFalse,
                       gPMConstants.PMEReturnCode.PMError)
        End Try
    End Function

    ' ***********************************************************
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***********************************************************
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return gPMConstants.PMEReturnCode.PMError
        End Try
    End Function


    ' *******************************************************************************
    ' PUBLIC METHODS
    ' *******************************************************************************


    ' *******************************************************************************
    ' PRIVATE METHODS
    ' *******************************************************************************
    ' ***********************************************************
    ' Clears all of the interface details for a new search.
    ' ***********************************************************
    Private Function ClearInterface(Optional ByVal v_bTransactionsOnly As Boolean = False, Optional ByVal v_bIgnoreMessage As Boolean = False) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try
            ' Clear the transaction listviews
            lvwTransactions.Enabled = False
            lvwTransactions.Items.Clear()
            lvwTransactions.Enabled = True

            lvwEntries.Enabled = False
            lvwEntries.Items.Clear()
            lvwEntries.Enabled = True
            lvwEntries.Tag = ""

            lvwInstalmentEntries.Enabled = False
            lvwInstalmentEntries.Items.Clear()
            lvwInstalmentEntries.Enabled = True
            lvwInstalmentEntries.Tag = ""

            ' Should we clear everything?
            If Not v_bTransactionsOnly Then
                ' Clear account code (this will trigger more work!)
                txtAccountCode.Text = ""

                ' Set defaults for search criteria
                cboMarkedStatus.SelectedIndex = 0
                cboMonth.SelectedIndex = 0
                optEffectiveDate.Checked = True

                ' Default the date (first to today as default, and then to null to disable)
                txtDueDateTo.Value = DateTime.Today

                ' Disable payment group (until an account has been selected)
                cboPaymentGroup.Enabled = False

                ' Set focus to the search details
                txtAccountCode.Focus()
                txtAlternateRef.Text = ""
                ' Clear the payment sources
                m_vPaySources = VB6.CopyArray(Nothing)

                ' Set account currency caption back to normal.
                optViewByAccount.Text = GetCaption(ACAccountCurrencyCaption)
                txtReciptPaymentAmount.Text = ""
                UctCurrency.Text = "(None)"
                chkDateFilterToinstalmentDueDates.Checked = False
                txtReference.Text = ""
                cboMediaType.ItemId = 0
                txtDueDateFrom.Checked = False
                txtDueDateTo.Checked = False
                txtAlternateRef.Text = ""
            End If

            ' Clear the search status bar.
            If Not v_bIgnoreMessage Then
                ToolStripStatusLabel.Text = ""
                'tsPgBarTransactions = Nothing
            End If

            txtMarked.Text = "0.00"
            TxttotalWriteoff.Text = "0.00"
            txtunallocatedAmount.Text = "0.00"

            ' Refresh our buttons
            SetButtonStatus()

            Return result

        Catch excep As System.Exception

            result = IIf(Information.Err().Number = Constants.vbObjectError, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMError)
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=excep.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try

    End Function


    ' ***************************************************************** '
    ' Name: ShowRemitanceAdviceReport
    '
    ' Description: Shows Remitance_Advice_Agency Report.
    '
    ' ***************************************************************** '
    Private Sub ShowRemitanceAdviceReport()

        Dim vKeyArray(1, 11) As Object 'As Variant

        Try

            'Method Code folows

            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, sClassName:="iPMBReportPrint.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oBusiness = temp_m_oBusiness

            With m_oBusiness

                ' Send Report & Parameters into Report via SetKeys()

                vKeyArray(0, 0) = PMNavKeyConst.PMKeyNameReportName '"report_name"

                vKeyArray(1, 0) = "Remittance_Advice_Agency"


                vKeyArray(0, 1) = PMNavKeyConst.PMKeyNamePrintReport '"report_print_options"

                vKeyArray(1, 1) = PMNavKeyConst.AC_VIEW_ONLY 'AC_PRINT_AND_VIEW

                'Submit (Account Id) to generate raport.

                vKeyArray(0, 2) = PMNavKeyConst.PMKeyNameParam1Name '"param_name1"

                vKeyArray(1, 2) = "user_id"


                vKeyArray(0, 3) = "user_id"

                vKeyArray(1, 3) = g_iUserID

                m_lReturn = .SetKeys(vKeyArray:=vKeyArray)

                m_lReturn = .Start()

                ' Close Report Component

                .Dispose()
                m_oBusiness = Nothing


            End With

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display Remitance Advice", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRemitanceAdviceReport", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try


    End Sub




    ' ***********************************************************
    ' Creates a new batch with the TransDetailIDs, for the roadmap.
    ' ***********************************************************
    Private Function CreateBatch(ByVal v_vTransDetailIDs() As Object, ByRef r_lBatchID As Integer) As Integer
        Dim result As Integer = 0
        Dim oBatch As bPMNavBatch.Business
        Dim vBatchArray As Array

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get an instance of the batch object
        Dim temp_oBatch As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oBatch, "bPMNavBatch.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oBatch = temp_oBatch
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", CreateBatch, Failed to get instance of bPMNavBatch")
        End If

        ' Create the batch

        m_lReturn = oBatch.CreateBatchSet(v_sNavBatchCode:=ACBatchCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", CreateBatch, Failed to oBatch.CreateBatchSet")
        End If

        ' Get the ID

        r_lBatchID = oBatch.BatchSetID

        ' Convert the batch to a 2d array
        vBatchArray = Array.CreateInstance(GetType(Object), New Integer() {1, v_vTransDetailIDs.GetUpperBound(0) - v_vTransDetailIDs.GetLowerBound(0) + 1}, New Integer() {0, v_vTransDetailIDs.GetLowerBound(0)})
        For lCount As Integer = v_vTransDetailIDs.GetLowerBound(0) To v_vTransDetailIDs.GetUpperBound(0)

            vBatchArray(0, lCount) = v_vTransDetailIDs(lCount)
        Next lCount

        ' Set the values in the batch

        m_lReturn = oBatch.AddBatchRecord(v_vBatchArray:=vBatchArray, v_sNavBatchCode:=ACBatchCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", CreateBatch, Failed to oBatch.AddBatchRecord")
        End If

        GoTo Finally_Renamed
Catch_Renamed:
        result = IIf(Information.Err().Number = Constants.vbObjectError, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMError)

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBatch", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Try

            ' Terminate and remove the instance

            oBatch.Dispose()

        Catch
        End Try
        Return result
    End Function

    ' ***********************************************************
    ' Load the search array data into a collection
    ' ***********************************************************
    Private Function DataToCollection(ByVal v_vSearchData(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lLower, lUpper As Integer
        Dim oTransaction As Transaction
        Dim oEntry As TransactionEntry
        Dim oInstalmentEntry As TransactionInst
        Dim bWriteOff As Boolean
        Dim bSetteledPremium As Boolean
        Dim vArray(,) As Object
        Dim nTransDetailsID As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Reset transaction array and set dummy transaction for first
            m_cTransactions = New Collection()
            oTransaction = New Transaction()

            If Information.IsArray(v_vSearchData) Then
                ' Get bounds
                lLower = v_vSearchData.GetLowerBound(1)
                lUpper = v_vSearchData.GetUpperBound(1)

                ' Process all rows
                For lRow As Integer = lLower To lUpper

                    ' Check for new transaction
                    bSetteledPremium = True

                    If NullToString(v_vSearchData(MainModule.SearchArrayEnum.ACSASpare, lRow)) = "COMM" And m_bPaidByClient Then
                        ' Get agent details
                        m_lReturn = g_oBusiness.GetTransDetailIdForSetteledPremium(v_iTransdetailid:=ToSafeLong(v_vSearchData(MainModule.SearchArrayEnum.ACSADetailId, lRow)), r_vResultsArray:=vArray)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError("DataToCollection", "GetAgentDetailForAccount failed")
                        End If
                        If Not IsArray(vArray) Then
                            bSetteledPremium = False
                        End If
                    End If

                    If bSetteledPremium Then
                        If oTransaction.DocumentID <> CDbl(v_vSearchData(MainModule.SearchArrayEnum.ACSADocumentID, lRow)) Or (CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSADocumentRef, lRow)).Substring(0, 2) = ACInsurerPaymentJournal) Then

                            ' Check if we already have this transaction
                            Try
                                oTransaction = Nothing

                                If CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSADocumentRef, lRow)).Substring(0, 2) <> ACInsurerPaymentJournal Then

                                    If CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAAlternateRef, lRow)) <> ACInsurerPaymentJournalEmpty Then
                                        oTransaction = m_cTransactions.Item("t_" & CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSADocumentID, lRow)))
                                    Else
                                        oTransaction = m_cTransactions.Item("t_" & CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSADocumentID, lRow)))
                                    End If

                                Else
                                    If CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSADocumentRef, lRow)) <> ACInsurerPaymentJournalEmpty Then
                                        oTransaction = m_cTransactions.Item("t_" & CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSADocumentID, lRow)) & "_d_" & CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSADetailId, lRow)))
                                    Else
                                        oTransaction = m_cTransactions.Item("t_" & CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSADocumentID, lRow)) & "_d_" & CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSADetailId, lRow)))
                                    End If
                                End If
                            Catch
                                'we just want to ignore errors that happened in this block as per old code - surely this needs revisiting?
                            End Try
                            bWriteOff = False
                            ' If we haven't already, create new object
                            If oTransaction Is Nothing Then
                                oTransaction = New Transaction()

                                ' Populate

                                oTransaction.InsuranceRef = gPMFunctions.NullToString(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAInsuranceRef, lRow))).Trim()
                                oTransaction.DetailID = CInt(v_vSearchData(MainModule.SearchArrayEnum.ACSADetailId, lRow))
                                oTransaction.DocumentID = CInt(v_vSearchData(MainModule.SearchArrayEnum.ACSADocumentID, lRow))
                                oTransaction.HolderCode = gPMFunctions.NullToString(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAHolderCode, lRow))).Trim()
                                oTransaction.HolderName = gPMFunctions.NullToString(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAHolderName, lRow))).Trim()
                                oTransaction.DocumentRef = CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSADocumentRef, lRow)).Trim()
                                oTransaction.AlternateRef = gPMFunctions.NullToString(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAAlternateRef, lRow))).Trim()

                                If Not (Convert.IsDBNull(v_vSearchData(MainModule.SearchArrayEnum.ACSAEffectiveDate, lRow)) Or IsNothing(v_vSearchData(MainModule.SearchArrayEnum.ACSAEffectiveDate, lRow))) Then
                                    oTransaction.EffectiveDate = gPMFunctions.ToSafeDate(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAEffectiveDate, lRow)))
                                End If

                                'oTransaction.DueDate = gPMFunctions.ToSafeDate(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAnewDueDate, lRow)))
                                If m_nBatchID = 0 AndAlso Not IsNothing(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAnewDueDate, lRow))) Then
                                    oTransaction.DueDate = gPMFunctions.ToSafeDate(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAnewDueDate, lRow)))
                                End If

                                oTransaction.CurrencyID = CInt(v_vSearchData(MainModule.SearchArrayEnum.ACSACurrencyID, lRow))
                                oTransaction.FullyPaidAmount = gPMFunctions.NullToCurrency(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAFullyPaidAmount, lRow)))
                                oTransaction.ClientOSAmount = gPMFunctions.NullToCurrency(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAClientOSAmount, lRow)))
                                oTransaction.CurrencyRate = gPMFunctions.NullToDouble(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSACurrencyRate, lRow)))
                                oTransaction.AccountCurrencyID = CInt(v_vSearchData(MainModule.SearchArrayEnum.ACSAAccountCurrencyID, lRow))
                                oTransaction.FullyPaidAccountAmount = gPMFunctions.NullToCurrency(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAFullyPaidAccountAmount, lRow)))
                                oTransaction.ClientOSAccountAmount = gPMFunctions.NullToCurrency(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAClientOSAccountAmount, lRow)))
                                oTransaction.AccountCurrencyRate = gPMFunctions.NullToDouble(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAAccountCurrencyRate, lRow)))
                                oTransaction.IsConsolidateBinder = CBool(v_vSearchData(MainModule.SearchArrayEnum.ACSAIsConsolidatedBinder, lRow))
                                oTransaction.AccountingDate = CDate(v_vSearchData(MainModule.SearchArrayEnum.ACSAAccountingDate, lRow))
                                oTransaction.CompanyID = gPMFunctions.ToSafeInteger(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSACompanyID, lRow)))
                                oTransaction.Month = CInt(v_vSearchData(MainModule.SearchArrayEnum.ACSAMonth, lRow))
                                oTransaction.Period = CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAPeriodName, lRow))
                                oTransaction.IsDebitOrderTransDetail = CBool(v_vSearchData(MainModule.SearchArrayEnum.kSAIsDebitOrderTransDetail, lRow))
                                oTransaction.ClientAccountCurrencyID = CInt(v_vSearchData(MainModule.SearchArrayEnum.ACSAClientAccountCurrencyID, lRow))
                                oTransaction.AllocationPeriod = CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAPeriodName, lRow)) + "  " + CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAYearName, lRow))
                                If m_nBatchID = 0 Then
                                    oTransaction.MediaType = Trim$(NullToString(v_vSearchData(MainModule.SearchArrayEnum.ACSAMediaType, lRow)))
                                End If

                                ' Add to collection
                                m_cTransactions.Add(oTransaction, oTransaction.Key)
                            End If
                        End If

                        If Mid(Trim$(v_vSearchData(MainModule.SearchArrayEnum.ACSADocumentRef, lRow)), 1, 3) <> "IND" AndAlso
                    Mid(Trim$(v_vSearchData(MainModule.SearchArrayEnum.ACSADocumentRef, lRow)), 1, 3) <> "IED" AndAlso
                    Mid(Trim$(v_vSearchData(MainModule.SearchArrayEnum.ACSADocumentRef, lRow)), 1, 3) <> "IRD" AndAlso
                      nTransDetailsID <> CInt(v_vSearchData(MainModule.SearchArrayEnum.ACSADetailId, lRow)) Then
                            oEntry = New TransactionEntry()

                            ' Populate

                            oEntry.AccountingDate = CDate(v_vSearchData(MainModule.SearchArrayEnum.ACSAAccountingDate, lRow))
                            oEntry.CompanyID = gPMFunctions.ToSafeInteger(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSACompanyID, lRow)))
                            oEntry.CurrencyAmount = CDec(v_vSearchData(MainModule.SearchArrayEnum.ACSACurrencyAmount, lRow))
                            oEntry.CurrencyID = CInt(v_vSearchData(MainModule.SearchArrayEnum.ACSACurrencyID, lRow))
                            oEntry.CurrencyCode = CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSACurrencyCode, lRow))
                            oEntry.MarkedAmount = gPMFunctions.NullToCurrency(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAMarkedAmount, lRow)))
                            oEntry.PaidAmount = gPMFunctions.NullToCurrency(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAPaidAmount, lRow)))
                            oEntry.CurrencyAccountAmount = CDec(v_vSearchData(MainModule.SearchArrayEnum.ACSACurrencyAccountAmount, lRow))
                            oEntry.AccountCurrencyID = CInt(v_vSearchData(MainModule.SearchArrayEnum.ACSAAccountCurrencyID, lRow))
                            oEntry.AccountCurrencyCode = CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAAccountCurrencyCode, lRow))
                            oEntry.MarkedAccountAmount = gPMFunctions.NullToCurrency(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAMarkedAccountAmount, lRow)))
                            oEntry.PaidAccountAmount = gPMFunctions.NullToCurrency(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAPaidAccountAmount, lRow)))
                            oEntry.DetailID = CInt(v_vSearchData(MainModule.SearchArrayEnum.ACSADetailId, lRow))
                            nTransDetailsID = oEntry.DetailID
                            oEntry.Month = CInt(v_vSearchData(MainModule.SearchArrayEnum.ACSAMonth, lRow))
                            oEntry.Period = CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAPeriodName, lRow))
                            oEntry.Spare = NullToString(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSASpare, lRow)))

                            If CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAAlternateRef, lRow)) <> "" Then
                                oEntry.AltRef = CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAAlternateRef, lRow))
                            Else
                                oEntry.AltRef = ""
                            End If
                            oEntry.Comment = gPMFunctions.NullToString(CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAComment, lRow)))
                            oEntry.AllocationPeriod = CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAPeriodName, lRow)) + "  " + CStr(v_vSearchData(MainModule.SearchArrayEnum.ACSAYearName, lRow))

                            ' End - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.4.1.1)

                            ' Add to collection
                            oTransaction.Add(oEntry)
                        Else
                            If bWriteOff = False Then
                                ' Create new transaction entry
                                oInstalmentEntry = New TransactionInst
                                If lRow > 0 Then
                                    If v_vSearchData(MainModule.SearchArrayEnum.ACSADetailId, lRow) <> v_vSearchData(MainModule.SearchArrayEnum.ACSADetailId, lRow - 1) Then
                                        oTransaction.TransAmount = NullToCurrency(v_vSearchData(MainModule.SearchArrayEnum.ACSACurrencyAmount, lRow))
                                    End If
                                Else
                                    oTransaction.TransAmount = NullToCurrency(v_vSearchData(MainModule.SearchArrayEnum.ACSACurrencyAmount, lRow))
                                End If
                                oInstalmentEntry.AccountingDate = v_vSearchData(MainModule.SearchArrayEnum.ACSAAccountingDate, lRow)
                                oInstalmentEntry.CompanyID = ToSafeInteger(v_vSearchData(MainModule.SearchArrayEnum.ACSACompanyID, lRow))

                                If UCase(Trim(v_vSearchData(MainModule.SearchArrayEnum.ACSASpare, lRow))) = "WRITEOFF" Then
                                    oInstalmentEntry.CurrencyAmount = v_vSearchData(MainModule.SearchArrayEnum.ACSACurrencyAmount, lRow)
                                    oInstalmentEntry.CurrencyAccountAmount = v_vSearchData(MainModule.SearchArrayEnum.ACSACurrencyAccountAmount, lRow)
                                Else
                                    oInstalmentEntry.CurrencyAmount = NullToCurrency(v_vSearchData(MainModule.SearchArrayEnum.ACSAInstAmount, lRow))
                                    If optViewByTransaction.Checked = True Then
                                        If v_vSearchData(MainModule.SearchArrayEnum.ACSACurrencyID, lRow) = v_vSearchData(MainModule.SearchArrayEnum.ACSAAccountCurrencyID, lRow) Then
                                            oInstalmentEntry.CurrencyAccountAmount = NullToCurrency(v_vSearchData(MainModule.SearchArrayEnum.ACSAInstAccountAmount, lRow))
                                        Else
                                            oInstalmentEntry.CurrencyAccountAmount = NullToCurrency(v_vSearchData(MainModule.SearchArrayEnum.ACSAInstAmount, lRow) / v_vSearchData(MainModule.SearchArrayEnum.ACSACurrencyRate, lRow))
                                        End If
                                    Else
                                        oInstalmentEntry.CurrencyAccountAmount = NullToCurrency(v_vSearchData(MainModule.SearchArrayEnum.ACSAInstAccountAmount, lRow))
                                    End If
                                    'oInstalmentEntry.CurrencyAmount = v_vSearchData(MainModule.SearchArrayEnum.ACSAInstAmount, lRow)
                                    'oInstalmentEntry.CurrencyAccountAmount = v_vSearchData(MainModule.SearchArrayEnum.ACSAInstAmount, lRow)
                                    oInstalmentEntry.InstalmentNumber = v_vSearchData(MainModule.SearchArrayEnum.ACSAInstalmentNumber, lRow)
                                    oInstalmentEntry.DueDate = ToSafeDate(v_vSearchData(MainModule.SearchArrayEnum.ACSAInstDueDate, lRow))
                                    oInstalmentEntry.PremiumFinanceCnt = ToSafeLong(v_vSearchData(MainModule.SearchArrayEnum.ACSAInstPremiumFinaceCnt, lRow))
                                    oInstalmentEntry.PremiumFinanceVersion = ToSafeLong(v_vSearchData(MainModule.SearchArrayEnum.ACSAInstPremiumFinaceVersion, lRow))
                                End If
                                oInstalmentEntry.CurrencyID = v_vSearchData(MainModule.SearchArrayEnum.ACSACurrencyID, lRow)
                                oInstalmentEntry.CurrencyCode = v_vSearchData(MainModule.SearchArrayEnum.ACSACurrencyCode, lRow)
                                oInstalmentEntry.MarkedAmount = NullToCurrency(v_vSearchData(MainModule.SearchArrayEnum.ACSAMarkedAmount, lRow))
                                oInstalmentEntry.AccountCurrencyID = v_vSearchData(MainModule.SearchArrayEnum.ACSAAccountCurrencyID, lRow)
                                oInstalmentEntry.AccountCurrencyCode = v_vSearchData(MainModule.SearchArrayEnum.ACSAAccountCurrencyCode, lRow)
                                oInstalmentEntry.MarkedAccountAmount = NullToCurrency(v_vSearchData(MainModule.SearchArrayEnum.ACSAMarkedAccountAmount, lRow))
                                oInstalmentEntry.DetailID = v_vSearchData(MainModule.SearchArrayEnum.ACSADetailId, lRow)
                                oInstalmentEntry.Month_Renamed = v_vSearchData(MainModule.SearchArrayEnum.ACSAMonth, lRow)
                                oInstalmentEntry.Period = v_vSearchData(MainModule.SearchArrayEnum.ACSAPeriodName, lRow)
                                oInstalmentEntry.Spare = v_vSearchData(MainModule.SearchArrayEnum.ACSASpare, lRow)
                                If v_vSearchData(MainModule.SearchArrayEnum.ACSAAlternateRef, lRow) <> "" Then
                                    oInstalmentEntry.AltRef = v_vSearchData(MainModule.SearchArrayEnum.ACSAAlternateRef, lRow)
                                Else
                                    oInstalmentEntry.AltRef = ""
                                End If

                                If UCase(Trim(v_vSearchData(MainModule.SearchArrayEnum.ACSASpare, lRow))) = "WRITEOFF" Then
                                    bWriteOff = True
                                End If

                                oInstalmentEntry.Comment = NullToString(v_vSearchData(MainModule.SearchArrayEnum.ACSAComment, lRow))

                                oInstalmentEntry.PFInstalmentsId = ToSafeLong(v_vSearchData(MainModule.SearchArrayEnum.ACSAInstPfInstalmentsId, lRow))
                                ' Add to collection
                                oTransaction.AddInstalment(oInstalmentEntry)
                            End If
                        End If
                    End If

                Next lRow

                ' Store and display item count
                m_lTotalItems = (lUpper - lLower) + 1
                ToolStripStatusLabel.Text = " Found " & m_lTotalItems &
                        " entries in " & CStr(m_cTransactions.Count) & " transactions."

            Else
                ' Set status to nothing found
                m_lTotalItems = 0
                ToolStripStatusLabel.Text = "No outstanding transactions found."
            End If

            Return result

        Catch ex As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:=Information.Err().Description,
                               vApp:=ACApp, vClass:=ACClass,
                               vMethod:="DataToCollection",
                               vErrNo:=Information.Err().Number,
                               vErrDesc:=Information.Err().Description,
                               excep:=ex)
            Return gPMConstants.PMEReturnCode.PMError

        End Try

    End Function

    Private Sub SetProgressBarMaximum(ByVal value As Integer)
        If tsPgBarTransactions IsNot Nothing Then
            tsPgBarTransactions.Maximum = value
        End If
    End Sub

    Private Sub ResetProgressBarMaximum()
        If (tsPgBarTransactions IsNot Nothing) Then
            tsPgBarTransactions.Maximum = 0
        End If
    End Sub
    Private Sub ShowProgressBar(ByVal value As Boolean)
        If (tsPgBarTransactions Is Nothing) And (value) Then
            tsPgBarTransactions = New ToolStripProgressBar
            Me.stbStatus.Items.Add(tsPgBarTransactions)
        End If
        If (tsPgBarTransactions IsNot Nothing) Then
            If (tsPgBarTransactions.Visible <> value) Then
                tsPgBarTransactions.Visible = value
            End If
            If value = False Then
                tsPgBarTransactions = Nothing
            End If
        End If
    End Sub
    Private Sub AddValueToProgressBar()
        If (tsPgBarTransactions.Value < tsPgBarTransactions.Maximum) Then
            tsPgBarTransactions.Value += 1
        End If
    End Sub


    ''' <summary>
    ''' Updates all interface details from the search data storage.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DataToInterface() As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oListItem As ListViewItem
        Dim nTransId As Integer
        Dim nRowPos As Integer
        Dim sTransTp As String = "" 'PS RSA Curacao - Insurer Payment transaction type selection - Upendra
        Dim sListItem() As String
        Dim oListItemArr As New System.Collections.Generic.List(Of ListViewItem)

        Try
            ' Clear all transactions
            ClearInterface(True, True)

            Application.DoEvents()
            ListViewFunc.ListViewBatchStart(lvwTransactions)

            ' Walk all trasactions
            Dim lRow As Integer
            lRow = 0

            If m_cTransactions Is Nothing Then
                Return nResult
            End If

            ReDim sListItem(MainModule.ListViewTransactionEnum.ACLTMediaType + 1)

            ShowProgressBar(True)
            SetProgressBarMaximum(m_cTransactions.Count)

            For Each oTransaction As Transaction In m_cTransactions

                'PS RSA Curacao - Insurer Payment transaction type selection - Upendra
                sTransTp = oTransaction.DocumentRef.Substring(0, 3)

                If (cboTransType.SelectedIndex = 0 OrElse cboTransType.SelectedIndex = -1) OrElse
                    (cboTransType.SelectedIndex = 1 AndAlso (sTransTp = "CLR" OrElse sTransTp = "CLP")) OrElse
                 (cboTransType.SelectedIndex = 2 AndAlso sTransTp <> "CLR" AndAlso sTransTp <> "CLP") Then

                    If ValidSource(vSource:=oTransaction.CompanyID) OrElse (m_sView IsNot Nothing AndAlso m_sView = PMEComponentAction.PMView) Then
                        sListItem(0) = ""
                        Application.DoEvents()
                        If Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IND" AndAlso
                                        Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IED" AndAlso
                                    Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IRD" Then

                            'oListItem = lvwTransactions.Items.Add(oTransaction.Key, "", "")
                            'oTransaction.GetMinTransactionId(oTransaction, nTransId, nRowPos, True)
                            'Dim subitem As New ListViewItem.ListViewSubItem
                            'subitem.Text = CStr(oTransaction.Item(1).CompanyID)
                            'If oTransaction.Item(nRowPos).Comment = "" Then
                            '    oListItem.SubItems.Insert(ListViewTransactionEnum.ACLTIconColumn + 1, subitem)
                            'Else
                            '    oListItem.SubItems.Insert(ListViewTransactionEnum.ACLTIconColumn + 1, subitem)
                            'End If

                            sListItem(MainModule.ListViewTransactionEnum.ACLTIconColumn + 1) = CStr(oTransaction.Item(1).CompanyID)
                            sListItem(MainModule.ListViewTransactionEnum.ACLTHolderName + 1) = oTransaction.HolderName
                            sListItem(MainModule.ListViewTransactionEnum.ACLTInsuranceRef + 1) = oTransaction.InsuranceRef
                            sListItem(MainModule.ListViewTransactionEnum.ACLTDocumentRef + 1) = oTransaction.DocumentRef
                            sListItem(MainModule.ListViewTransactionEnum.ACLTAlternateRef + 1) = oTransaction.AlternateRef
                            If oTransaction.EffectiveDate.Year > 1900 Then
                                sListItem(MainModule.ListViewTransactionEnum.ACLTEffectiveDate + 1) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, oTransaction.EffectiveDate)
                            End If

                            If oTransaction.DueDate.Year = 1 Then
                                sListItem(MainModule.ListViewTransactionEnum.ACLTDueDate + 1) = ""
                            Else
                                sListItem(MainModule.ListViewTransactionEnum.ACLTDueDate + 1) = gPMFunctions.FormatField(PMEFormatStyle.PMFormatDateShort, oTransaction.DueDate)
                            End If


                            sListItem(MainModule.ListViewTransactionEnum.ACLTAccountingDate + 1) = gPMFunctions.FormatField(PMEFormatStyle.PMFormatDateShort, oTransaction.AccountingDate)
                            If optViewByTransaction.Checked Then
                                If oTransaction.DocumentRef.Substring(0, 2) = "JN" Then
                                    For Each oEntry As TransactionEntry In oTransaction
                                        sListItem(MainModule.ListViewTransactionEnum.ACLTCurrencyTotal + 1) = g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.CurrencyID, v_cCurrencyAmount:=oEntry.CurrencyAmount)
                                        sListItem(MainModule.ListViewTransactionEnum.ACLTPaidTotal + 1) = g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.CurrencyID, v_cCurrencyAmount:=oEntry.PaidAmount)
                                        sListItem(MainModule.ListViewTransactionEnum.ACLTNetTotal + 1) = g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.CurrencyID, v_cCurrencyAmount:=oEntry.OutstandingAmount)
                                    Next oEntry
                                Else
                                    sListItem(MainModule.ListViewTransactionEnum.ACLTCurrencyTotal + 1) = g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.CurrencyID, v_cCurrencyAmount:=oTransaction.TotalCurrency)
                                    sListItem(MainModule.ListViewTransactionEnum.ACLTPaidTotal + 1) = g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.CurrencyID, v_cCurrencyAmount:=oTransaction.TotalPaid)
                                    sListItem(MainModule.ListViewTransactionEnum.ACLTNetTotal + 1) = g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.CurrencyID, v_cCurrencyAmount:=oTransaction.TotalOutstanding)
                                End If
                            Else
                                If oTransaction.DocumentRef.Substring(0, 2) = "JN" Then
                                    For Each oEntry As TransactionEntry In oTransaction
                                        sListItem(MainModule.ListViewTransactionEnum.ACLTCurrencyTotal + 1) = g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.AccountCurrencyID, v_cCurrencyAmount:=oEntry.CurrencyAccountAmount)
                                        sListItem(MainModule.ListViewTransactionEnum.ACLTPaidTotal + 1) = g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.AccountCurrencyID, v_cCurrencyAmount:=oEntry.PaidAccountAmount)
                                        sListItem(MainModule.ListViewTransactionEnum.ACLTNetTotal + 1) = g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.AccountCurrencyID, v_cCurrencyAmount:=oEntry.OutstandingAccountAmount)
                                    Next oEntry
                                Else
                                    sListItem(MainModule.ListViewTransactionEnum.ACLTCurrencyTotal + 1) = g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.AccountCurrencyID, v_cCurrencyAmount:=oTransaction.TotalAccountCurrency)
                                    sListItem(MainModule.ListViewTransactionEnum.ACLTPaidTotal + 1) = g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.AccountCurrencyID, v_cCurrencyAmount:=oTransaction.TotalAccountPaid)
                                    sListItem(MainModule.ListViewTransactionEnum.ACLTNetTotal + 1) = g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.AccountCurrencyID, v_cCurrencyAmount:=oTransaction.TotalAccountOutstanding)
                                End If
                            End If
                            'Client OS amount is always in Transaction Currency.
                            If optViewByAccount.Checked Then
                                If oTransaction.DocumentRef.Substring(0, 2) = "JN" OrElse
                                    oTransaction.DocumentRef.Substring(0, 3) = "SRP" OrElse
                                   oTransaction.DocumentRef.Substring(0, 3) = "SPY" Then
                                    sListItem(MainModule.ListViewTransactionEnum.ACLTClientOS + 1) = g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.AccountCurrencyID, v_cCurrencyAmount:=oTransaction.ClientOSAccountAmount)
                                Else
                                    If oTransaction.AccountCurrencyID = oTransaction.CurrencyID OrElse oTransaction.ClientAccountCurrencyID = 0 Then
                                        sListItem(MainModule.ListViewTransactionEnum.ACLTClientOS + 1) =
                                            g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.AccountCurrencyID, v_cCurrencyAmount:=oTransaction.ClientOSAmount)
                                    Else
                                        Dim new_ClientOSAccountAmount As Decimal
                                        m_lReturn = g_oCurrencyConvert.CurrencyToCurrencyConversion(oTransaction.ClientAccountCurrencyID,
                                                                                                    oTransaction.ClientOSAmount,
                                                                                                    oTransaction.CompanyID,
                                                                                                    oTransaction.AccountCurrencyID,
                                                                                                    new_ClientOSAccountAmount)
                                        sListItem(MainModule.ListViewTransactionEnum.ACLTClientOS + 1) =
                                            g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.AccountCurrencyID, v_cCurrencyAmount:=new_ClientOSAccountAmount)
                                    End If
                                End If
                            Else
                                If oTransaction.DocumentRef.Substring(0, 2) = "JN" OrElse
                                   oTransaction.DocumentRef.Substring(0, 3) = "SRP" OrElse
                                   oTransaction.DocumentRef.Substring(0, 3) = "SPY" Then
                                    sListItem(MainModule.ListViewTransactionEnum.ACLTClientOS + 1) = g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.CurrencyID, v_cCurrencyAmount:=oTransaction.ClientOSAmount)
                                Else
                                    sListItem(MainModule.ListViewTransactionEnum.ACLTClientOS + 1) =
                                        g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.CurrencyID, v_cCurrencyAmount:=oTransaction.ClientOSAmount)
                                End If
                            End If
                            sListItem(MainModule.ListViewTransactionEnum.ACLTHolderCode + 1) = oTransaction.HolderCode

                            If oTransaction.DueDate <> Nothing AndAlso oTransaction.DueDate <> "12:00:00 AM" Then
                                sListItem(MainModule.ListViewTransactionEnum.ACLTDueDate + 1) =
                                    gPMFunctions.FormatField(PMEFormatStyle.PMFormatDateShort, ZeroToNull(oTransaction.DueDate))
                            Else
                                sListItem(MainModule.ListViewTransactionEnum.ACLTDueDate + 1) = String.Empty
                            End If
                            sListItem(MainModule.ListViewTransactionEnum.ACLTMediaType + 1) =
                                oTransaction.MediaType

                            sListItem(MainModule.ListViewTransactionEnum.ACLTAllocationPeriod + 1) =
                                oTransaction.AllocationPeriod

                            If oTransaction.IsMarked = TransactionEntry.MarkedStatusEnum.acmseNotMarked Then
                                If optViewByAccount.Checked Then
                                    sListItem(MainModule.ListViewTransactionEnum.ACLTMarkedTotal + 1) =
                                    g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.AccountCurrencyID, v_cCurrencyAmount:=0)
                                Else
                                    sListItem(MainModule.ListViewTransactionEnum.ACLTMarkedTotal + 1) =
                                    g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.CurrencyID, v_cCurrencyAmount:=0)
                                End If
                            End If
                            oListItem = New ListViewItem(sListItem)
                            ' Set marked amount and icons
                            'Devrloper Guide No.49 (Guide)
                            oListItem.ImageKey = ACIconBlank
                            oListItem.Tag = CStr(oTransaction.Key)
                            oListItem.Name = CStr(oTransaction.Key)
                            oListItemArr.Add(oListItem)

                            'oListItem.Tag = CStr(oTransaction.Key)
                            lRow += 1
                            'RefreshTransaction(oTransaction, oListItem) ' Set marked amount and icons
                        Else
                            'oListItem = lvwTransactions.Items.Add(oTransaction.Key, "", "")
                            'oTransaction.GetMinTransactionId(oTransaction, nTransId, nRowPos, True)
                            'Dim subitem As New ListViewItem.ListViewSubItem
                            'subitem.Text = CStr(oTransaction.ItemInstalment(1).CompanyID)

                            'If (oTransaction.ItemInstalment(nRowPos).Comment = "") Then
                            '    oListItem.SubItems.Insert(ListViewTransactionEnum.ACLTIconColumn + 1, subitem)
                            'Else
                            '    oListItem.SubItems.Insert(ListViewTransactionEnum.ACLTIconColumn + 1, subitem)
                            'End If
                            sListItem(MainModule.ListViewTransactionEnum.ACLTHolderName + 1) = oTransaction.HolderName


                            sListItem(MainModule.ListViewTransactionEnum.ACLTInsuranceRef + 1) = oTransaction.InsuranceRef
                            sListItem(MainModule.ListViewTransactionEnum.ACLTDocumentRef + 1) = oTransaction.DocumentRef
                            sListItem(MainModule.ListViewTransactionEnum.ACLTAlternateRef + 1) = oTransaction.AlternateRef
                            If Year(oTransaction.EffectiveDate) > 1900 Then
                                sListItem(MainModule.ListViewTransactionEnum.ACLTEffectiveDate + 1) = oTransaction.EffectiveDate
                            End If

                            sListItem(MainModule.ListViewTransactionEnum.ACLTAccountingDate + 1) =
                                gPMFunctions.FormatField(PMEFormatStyle.PMFormatDateShort, oTransaction.AccountingDate)

                            If optViewByTransaction.Checked = True Then
                                sListItem(MainModule.ListViewTransactionEnum.ACLTCurrencyTotal + 1) =
                                    g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.CurrencyID, v_cCurrencyAmount:=oTransaction.TotalCurrency)

                                sListItem(MainModule.ListViewTransactionEnum.ACLTPaidTotal + 1) =
                                    g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.CurrencyID, v_cCurrencyAmount:=oTransaction.TotalPaid)

                                sListItem(MainModule.ListViewTransactionEnum.ACLTNetTotal + 1) =
                                    g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.CurrencyID, v_cCurrencyAmount:=oTransaction.TotalOutstanding)

                                sListItem(MainModule.ListViewTransactionEnum.ACLTClientOS + 1) =
                                    g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.CurrencyID, v_cCurrencyAmount:=oTransaction.ClientOSAmount)

                            Else
                                sListItem(MainModule.ListViewTransactionEnum.ACLTCurrencyTotal + 1) =
                                    g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.AccountCurrencyID, v_cCurrencyAmount:=oTransaction.TotalAccountCurrency)

                                sListItem(MainModule.ListViewTransactionEnum.ACLTPaidTotal + 1) =
                                    g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.AccountCurrencyID, v_cCurrencyAmount:=oTransaction.TotalAccountPaid)

                                sListItem(MainModule.ListViewTransactionEnum.ACLTNetTotal + 1) =
                                    g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.AccountCurrencyID, v_cCurrencyAmount:=oTransaction.TotalAccountOutstanding)

                                sListItem(MainModule.ListViewTransactionEnum.ACLTClientOS + 1) =
                                    g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.AccountCurrencyID, v_cCurrencyAmount:=oTransaction.ClientOSAccountAmount)
                            End If
                            If bInstalmentByDuedate = True Then
                                If optViewByTransaction.Checked = True Then
                                    sListItem(MainModule.ListViewTransactionEnum.ACLTCurrencyTotal + 1) =
                                        g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.CurrencyID, v_cCurrencyAmount:=oTransaction.TotalAccountCurrency)
                                Else
                                    sListItem(MainModule.ListViewTransactionEnum.ACLTCurrencyTotal + 1) =
                                        g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.AccountCurrencyID, v_cCurrencyAmount:=oTransaction.TotalAccountCurrency)
                                End If

                            End If
                            'Client OS amount is always in Transaction Currency.
                            sListItem(MainModule.ListViewTransactionEnum.ACLTHolderCode + 1) = oTransaction.HolderCode

                            sListItem(MainModule.ListViewTransactionEnum.ACLTDueDate + 1) =
                                gPMFunctions.FormatField(PMEFormatStyle.PMFormatDateShort, oTransaction.DueDate)

                            sListItem(MainModule.ListViewTransactionEnum.ACLTMediaType + 1) = oTransaction.MediaType

                            oListItem = New ListViewItem(sListItem)
                            ' Set marked amount and icons
                            'Devrloper Guide No.49 (Guide)
                            oListItem.ImageKey = ACIconBlank
                            oListItem.Tag = CStr(oTransaction.Key)
                            oListItem.Name = CStr(oTransaction.Key)
                            oListItemArr.Add(oListItem)

                            'oListItem.Tag = CStr(oTransaction.Key)
                            lRow = lRow + 1
                            ' Set marked amount and icons
                            'Call RefreshTransaction(oTransaction, oListItem)
                        End If
                    End If
                End If
                AddValueToProgressBar()
            Next oTransaction
            lvwTransactions.Items.AddRange(oListItemArr.ToArray())
            ' Select first item and force a refresh
            If lvwTransactions.Items.Count Then
                lvwTransactions.Items(0).Focused = True
                For Each oNewListItem As ListViewItem In lvwTransactions.Items
                    Dim oTransaction As Transaction = m_cTransactions.Item(oNewListItem.Name)
                    If oTransaction.IsMarked = TransactionEntry.MarkedStatusEnum.acmseFullyMarked Then
                        RefreshEntries(oNewListItem)
                    End If
                Next
                lvwTransactions.Items(0).Selected = True
            Else
                ' Refresh our buttons
                SetButtonStatus()
            End If
               m_lTotalItems = lvwTransactions.Items.Count
            ' Display found state
             m_lTotalItems = lvwTransactions.Items.Count
            If m_lTotalItems Then
                ' Report status
                ToolStripStatusLabel.Text = " Found " & m_lTotalItems &
                        " entries in " & CStr(m_cTransactions.Count) & " transactions."
            Else
                ' Set status to nothing
                ToolStripStatusLabel.Text = "No outstanding transactions found."
            End If

            ResetProgressBarMaximum()
            ShowProgressBar(False)

            Return nResult
        Catch ex As Exception
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            nResult = IIf(Information.Err().Number = Constants.vbObjectError,
                          PMEReturnCode.PMFalse,
                          PMEReturnCode.PMError)
        Finally
            If m_cTransactions IsNot Nothing Then
                RefreshTotalMarked()
                RefreshTotalWriteOff()
                RefreshUnallocatedAmount()
                ListViewFunc.ListViewBatchEnd()
            End If
        End Try


    End Function


    ' ***************************************************************** '
    ' Display language specific captions.
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set form caption
            Me.Text = GetCaption(ACInterfaceTitle)
            ' Set button captions
            cmdMarkTrans.Text = GetCaption(ACMarkButton)
            cmdDrill.Text = GetCaption(ACDrillButton)

            cmdOK.Text = GetCaption(ACOKButton)
            cmdCancel.Text = GetCaption(ACCancelButton)
            cmdHelp.Text = GetCaption(ACHelpButton)

            cmdPay.Text = GetCaption(ACPayButton)
            cmdAllocate.Text = GetCaption(ACAllocateButton)

            ' Set all transaction headers
            For lCount As Integer = 0 To lvwTransactions.Columns.Count - 2
                lvwTransactions.Columns.Item(lCount + 1).Text = GetCaption(ACTransactionListBase + lCount)
            Next

            ' Set all entry headers
            For lCount As Integer = 1 To lvwEntries.Columns.Count - 1
                lvwEntries.Columns.Item(lCount).Text = GetCaption(ACEntryListBase + lCount)
            Next
            ' Set all instalmententry headers
            For lCount As Integer = 1 To lvwInstalmentEntries.Columns.Count - 1
                lvwInstalmentEntries.Columns.Item(lCount).Text = GetCaption(ACInstalmentEntryListBase + lCount)
            Next
            fraViewBy.Text = GetCaption(ACViewByCaption)
            optViewByTransaction.Text = GetCaption(ACTransactionCurrencyCaption)
            optViewByAccount.Text = GetCaption(ACAccountCurrencyCaption)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***********************************************************
    ' Drills the document selected using FindTransaction
    ' ***********************************************************
    Private Function DrillDocument() As Integer

        Dim result As Integer = 0
        Dim oTransaction As Transaction
        Dim sDocumentRef As String = ""
        result = gPMConstants.PMEReturnCode.PMTrue




        ' Mouse pointer to busy
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        ' Get the current active document
        Try
            oTransaction = m_cTransactions.Item(lvwTransactions.FocusedItem.Name)

        Catch
        End Try



        ' Check we found something
        If oTransaction Is Nothing Then
            MessageBox.Show("Please select the transaction you wish to Drill", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            result = gPMConstants.PMEReturnCode.PMFalse
        Else
            ' If we haven't already done this do it now
            If m_oFindTransaction Is Nothing Then
                ' Get an instance of the find object
                Dim temp_m_oFindTransaction As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oFindTransaction, sClassName:="iACTFindTransaction.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oFindTransaction = temp_m_oFindTransaction

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", DrillDocument, Unable to create an instance of iACTFindTransaction")
                End If
            End If

            ' Dont want to let them drill again

            m_oFindTransaction.DrillLevel = 2
            ' Document Ref

            m_oFindTransaction.DocumentRef = oTransaction.DocumentRef
            ' Use source id from first entry
            If Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IND" And
                Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IED" And
                Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IRD" Then
                m_oFindTransaction.DrillCompany = oTransaction.Item(1).CompanyID
            Else
                m_oFindTransaction.drillCompany = oTransaction.ItemInstalment(1).CompanyID
            End If
            ' Mouse pointer back to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Start the object

            m_lReturn = m_oFindTransaction.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", DrillDocument, Unable to start iACTFindTransaction")
            End If
        End If

        GoTo Finally_Renamed
Catch_Renamed:
        ' Set status
        result = IIf(Information.Err().Number = Constants.vbObjectError, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMError)

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="DrillDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

Finally_Renamed:
        ' Mouse pointer back to normal
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Return result
    End Function

    ' ***************************************************************** '
    ' Displays the find account dialog
    ' ***************************************************************** '
    Public Function FindAccount(Optional ByRef r_lAccountId As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim bAllowStopped As Boolean
        Dim vKeyArray, oFindAccount As Object


        result = gPMConstants.PMEReturnCode.PMTrue

        Try


            ' Get an instance of Find Account (This cannot be cached as it fails on reruns)
            Dim temp_oFindAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindAccount, sClassName:="iACTFindAccount.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindAccount = temp_oFindAccount
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", FindAccount, Failed to get an instance of iACTFindAccount.Interface")
            End If

            ' Create key array and set the keys

            ReDim vKeyArray(1, 2)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyAllowStoppedAccounts

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = bAllowStopped

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameShortCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = txtAccountCode.Text

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyOnlyInsurerAndAgentAccounts

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = True

            ' Set keys

            m_lReturn = oFindAccount.SetKeys(vKeyArray)

            ' Start process

            m_lReturn = oFindAccount.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", FindAccount, Failed to start iACTFindAccount.Interface")
            End If

            ' If they didn't cancel then store the new data

            If oFindAccount.Status <> gPMConstants.PMEReturnCode.PMCancel Then
                ' Store found code and set return ID

                txtAccountCode.Text = oFindAccount.ShortCode

                r_lAccountId = oFindAccount.accountID
            End If

            ' Remove the instance as it doesn't work a second time...
            TerminateObject(oFindAccount)

            Return result

        Catch excep As System.Exception

            result = IIf(Information.Err().Number = Constants.vbObjectError, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMError)

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindAccount", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)



            Return result
        End Try
    End Function

    ' ***********************************************************
    ' Get caption from res file
    ' ***********************************************************
    Private Function GetCaption(ByVal lCaptionID As Integer) As String

        Try
            ' Get caption with current language as string
            Return CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=lCaptionID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)).Trim()
        Catch
            ' Error return notification
            Return "<invalid>"
        End Try

    End Function

    ' ***********************************************************
    ' Get ledger details
    ' ***********************************************************
    Private Function GetLedger(ByVal v_lAccountID As Integer, ByRef v_lLedgerId As Integer, ByRef v_sLedgerCode As String) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        result = gPMConstants.PMEReturnCode.PMTrue



        ' Get the business object
        Dim temp_g_oAccount As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_g_oAccount, "bACTAccount.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        g_oAccount = temp_g_oAccount

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the accounts ledger

        m_lReturn = g_oAccount.GetAccountLedger(v_lAccountId:=v_lAccountID, v_lLedgerId:=v_lLedgerId, v_sLedgerCode:=v_sLedgerCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        Try

            ' Terminate the object
            m_lReturn = gPMConstants.PMEReturnCode.PMError

            g_oAccount.Dispose()
            g_oAccount = Nothing

            Return result

        Catch exc As System.Exception

        End Try
        Return result
    End Function

    ' ***********************************************************
    ' Loads and lock a new account
    ' ***********************************************************
    Private Function LoadAccount(ByVal lAccountID As Integer) As Integer

        Dim iAccountCurrencyID As Integer
        Dim sAccountCurrencyCode As String = ""

        ' Check for new account
        If lAccountID > 0 Then
            ' Don't refresh everything if the account hasn't changed
            If lAccountID <> m_lAccountId Then
                ' If it has change clear out the old account
                If m_lAccountId > 0 Then
                    ' Unlock the insurer
                    m_lReturn = UnlockInsurer(m_lAccountId)
                    ' Clear the interface
                    ClearInterface(True)
                End If

                ' Store new id
                m_lAccountId = lAccountID
                ' Load payment groups
                m_lReturn = LoadPaymentGroups()
                'Show commission frame
                m_lReturn = ShowCommissionFrame()
                ' Lock the account
                m_lReturn = LockInsurer(m_lAccountId)

                ' Get account currency

                m_lReturn = m_oAccount.GetDetails(vAccountID:=lAccountID)

                m_lReturn = m_oAccount.GetNext(vCurrencyID:=iAccountCurrencyID)

                ' Change the account currency caption to include the currency code.

                m_lReturn = m_oCurrency.GetDetails(vCurrencyID:=iAccountCurrencyID)

                m_lReturn = m_oCurrency.GetNext(vCode:=sAccountCurrencyCode)

                ' Display current code on "Account Currency" option
                optViewByAccount.Text = GetCaption(ACAccountCurrencyCaption) & " (" & sAccountCurrencyCode.Trim() & ")"
            End If
        Else
            ' Disable the payment group in protest ;-)
            cboPaymentGroup.Enabled = False
        End If

    End Function

    ' ***********************************************************
    ' Loads up the combo box with payment groups if present else
    '   it loads up the branches available
    ' ***********************************************************
    Private Function LoadPaymentGroups() As Integer

        Dim result As Integer = 0
        Dim lLast As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Get payment groups

            m_lReturn = g_oPaymentGroups.GetDetails(laccountID:=m_lAccountId, vPaymentGroups:=m_vPaymentGroups)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", LoadPaymentGroups, Unable to access Payment Groups")
            End If

            ' Clear list
            cboPaymentGroup.Items.Clear()

            ' Check return
            m_bFromGroups = Information.IsArray(m_vPaymentGroups)
            Dim cboPaymentGroup_NewIndex As Integer = -1
            If m_bFromGroups Then
                ' Load options from Payment groups
                For lCount As Integer = 0 To m_vPaymentGroups.GetUpperBound(1)
                    If CInt(m_vPaymentGroups(2, lCount)) <> StringsHelper.ToDoubleSafe(lLast) Then
                        ' New group add text and id (as itemdata)
                        'developer guide no.153
                        cboPaymentGroup_NewIndex = cboPaymentGroup.Items.Add(New VB6.ListBoxItem(CStr(m_vPaymentGroups(0, lCount)), CInt(m_vPaymentGroups(2, lCount))))
                    End If
                    lLast = CStr(m_vPaymentGroups(2, lCount))
                Next lCount

                ' Set default
                cboPaymentGroup.SelectedIndex = -1
            Else
                ' If we have more than one branch add an all branches options
                If m_vSourceArray.GetUpperBound(1) + 1 <> m_vSourceArray.GetLowerBound(1) + 1 Then
                    'developer guide no.153
                    cboPaymentGroup_NewIndex = cboPaymentGroup.Items.Add(New VB6.ListBoxItem("All Branches", 0))
                End If

                ' Load Options From Source File
                For lCount As Integer = 1 To m_vSourceArray.GetUpperBound(1)
                    'developer guide no.153
                    cboPaymentGroup_NewIndex = cboPaymentGroup.Items.Add(New VB6.ListBoxItem(CStr(m_vSourceArray(2, lCount)), CInt(m_vSourceArray(0, lCount))))
                Next lCount

                ' Set default
                cboPaymentGroup.SelectedIndex = 0
            End If

            ' Force a validate
            cboPaymentGroup_Leave(cboPaymentGroup, New EventArgs())

            Return result

        Catch excep As System.Exception

            result = IIf(Information.Err().Number = Constants.vbObjectError, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMError)

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=excep.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="FindAccount", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)



            Return result
        End Try
    End Function

    ' ***********************************************************
    ' Lock the insurers account
    ' ***********************************************************
    Private Function LockInsurer(ByVal v_lAccountID As Integer) As Integer
        Dim result As Integer = 0
        Dim oPMLock As bPMLock.User
        Dim sLockedBy As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Get an instance of the lock object
            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", LockInsurer, Unable to get instance of bPMLock")
            End If

            ' Lock the insurer

            m_lReturn = oPMLock.LockKey(sKeyName:="Insurer_Payment", vKeyValue:=v_lAccountID, iUserID:=g_iUserID, sCurrentlyLockedBy:=sLockedBy)

            ' Check return
            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Everything okay
                Case gPMConstants.PMEReturnCode.PMFalse
                    ' Locked or error?
                    If sLockedBy = "ERROR" Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", LockInsurer, " + CStr(CDbl("Failed to lock insurer. Account id: ") + v_lAccountID))
                    Else
                        ' Let the user know who has this locked and return false
                        MessageBox.Show("Insurer Payment for this account is currently locked by " & sLockedBy & ".", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case Else
                    ' General error
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", LockInsurer, " + CStr(CDbl("Failed to lock insurer. Account id: ") + v_lAccountID))
            End Select

            Return result

        Catch excep As System.Exception

            ' Set return
            result = IIf(Information.Err().Number = Constants.vbObjectError, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMError)

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=excep.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="LockInsurer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result
        End Try
    End Function

    ' ***********************************************************
    ' Marks all transactions
    ' ***********************************************************
    Private Function MarkAll() As Integer

        Dim result As Integer = 0
        Dim oTransaction As Transaction

        ' Default response
        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Set the mouse pointer to busy
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ShowProgressBar(True)
            SetProgressBarMaximum(lvwTransactions.Items.Count)
            ' Process all items
            For Each oListItem As ListViewItem In lvwTransactions.Items
                ' Get the transaction
                oTransaction = m_cTransactions(oListItem.Name)

                ' Mark the whole transaction if not already marked
                If oTransaction.TotalMarked <> oTransaction.TotalOutstanding Then
                    m_lReturn = MarkTransaction(oTransaction:=oTransaction, bForceMark:=True)

                    ' Check return
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", MarkAll, " + "Unable to mark transaction: " & oTransaction.Key)
                    End If
                End If
                ' tsPgBarTransactions.Value += 1
                AddValueToProgressBar()
            Next oListItem
            m_iPreviousCurrencyId = 0
            ' Update total marked for payment
            RefreshTotalMarked()
            RefreshTotalWriteOff()
            RefreshUnallocatedAmount()
            ResetProgressBarMaximum()
            ShowProgressBar(False)


        Catch ex As Exception
            ' Set return
            result = IIf(Information.Err().Number = Constants.vbObjectError, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMError)

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="MarkAll", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            ' Set the mouse pointer back to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
        Return result
    End Function

    ' ***********************************************************
    ' Marks all transactions
    ' ***********************************************************
    Private Function UnMarkAll() As Integer

        Dim result As Integer = 0
        Dim oTransaction As Transaction

        ' Default response
        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Set the mouse pointer to busy
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ShowProgressBar(True)
            SetProgressBarMaximum(lvwTransactions.Items.Count)
            ' Process all items
            For Each oListItem As ListViewItem In lvwTransactions.Items
                ' Get the transaction
                oTransaction = m_cTransactions(oListItem.Name)

                ' Mark the whole transaction if not already marked
                If oTransaction.TotalMarked = oTransaction.TotalOutstanding Then
                    m_lReturn = MarkTransaction(oTransaction:=oTransaction, v_bUnMarkTrans:=True)

                    ' Check return
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", MarkAll, " + "Unable to mark transaction: " & oTransaction.Key)
                    End If
                End If
                ' tsPgBarTransactions.Value += 1
                AddValueToProgressBar()
            Next oListItem
            m_iPreviousCurrencyId = 0
            ' Update total marked for payment
            RefreshTotalMarked()
            RefreshTotalWriteOff()
            RefreshUnallocatedAmount()
            ResetProgressBarMaximum()
            ShowProgressBar(False)


        Catch ex As Exception
            ' Set return
            result = IIf(Information.Err().Number = Constants.vbObjectError, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMError)

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="MarkAll", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            ' Set the mouse pointer back to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
        Return result
    End Function

    ' ***********************************************************
    ' Mark, unmark, or partmark (partpay) a transaction entry
    ' ***********************************************************
    Private Function MarkEntry(ByVal oTransaction As Transaction, ByVal oEntry As TransactionEntry, Optional ByVal cMarkAmount As Decimal = 0, Optional ByVal bIgnoreCurrency As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim iBaseCurrencyID As Integer
        Dim cMarkBaseAmount, cMarkBaseAmountUnrounded As Decimal

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Check this isn't already appropriately marked
            If oEntry.MarkedAmount <> cMarkAmount Then
                ' Ensure this transaction is not already marked!

                ' Note: This is irrelevent of what we intend to do!
                m_lReturn = g_oBusiness.UnMarkTransaction(v_lTransDetailId:=oEntry.DetailID)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", MarkEntry, " + "Unable to unmark transaction entry: " & oEntry.Key)
                End If

                ' Set marked amount to zero, just in case the mark then fails
                oEntry.MarkedAmount = 0
                oEntry.MarkedAccountAmount = 0

                ' Check if we should now mark the item
                If cMarkAmount Then

                    ' Mark this item for the amount specified

                    m_lReturn = g_oBusiness.MarkTransaction(v_lTransactionID:=oEntry.DetailID, v_iCurrencyID:=oEntry.CurrencyID, v_cPayment:=cMarkAmount)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", MarkEntry, " + "Unable to mark transaction entry: " & oEntry.Key)
                    End If

                    ' Set new marked amount on object
                    oEntry.MarkedAmount = cMarkAmount

                    'Work out the Marked Account Amount
                    If oEntry.CurrencyID <> oEntry.AccountCurrencyID Then

                        m_lReturn = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyId:=oEntry.CompanyID, r_iBaseCurrencyID:=iBaseCurrencyID)

                        If oEntry.CurrencyID <> iBaseCurrencyID Then

                            m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(lCurrencyID:=oEntry.CurrencyID, lCompanyID:=oEntry.CompanyID, cBaseAmount:=cMarkBaseAmount, cCurrencyAmount:=cMarkAmount, vConversionDate:=DateTime.Today, vConversionRate:=oTransaction.CurrencyRate, vBaseAmountUnRounded:=cMarkBaseAmountUnrounded)
                        Else
                            cMarkBaseAmount = cMarkAmount
                            cMarkBaseAmountUnrounded = cMarkAmount
                        End If

                        If oEntry.AccountCurrencyID <> iBaseCurrencyID Then

                            cMarkBaseAmount = cMarkBaseAmountUnrounded
                            m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=oEntry.AccountCurrencyID, lCompanyID:=oEntry.CompanyID, cBaseAmount:=cMarkBaseAmount, cCurrencyAmount:=cMarkAmount, vConversionDate:=DateTime.Today, vConversionRate:=oTransaction.AccountCurrencyRate)
                        Else
                            cMarkAmount = cMarkBaseAmount
                        End If
                    End If
                    'Set the Marked Account Amount
                    oEntry.MarkedAccountAmount = cMarkAmount

                End If

                ' Refresh the item
                RefreshEntry(oEntry)
            End If
            m_iPreviousCurrencyId = 0
            Return result

        Catch excep As System.Exception

            result = IIf(Information.Err().Number = Constants.vbObjectError, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMError)

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=excep.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="MarkEntry", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)



            Return result
        End Try
    End Function

    ''' <summary>
    ''' MarkEntries
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function MarkEntries() As Integer

        Dim result As Integer = 0
        Dim oTransaction As Transaction
        Dim oEntry As TransactionEntry

        result = gPMConstants.PMEReturnCode.PMTrue
        Dim IsWOFF As Boolean = False
        Try

            ' Set the mouse pointer to busy
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Only process if we have a current transaction
            If Strings.Len(Convert.ToString(lvwEntries.Tag)) Then
                ' Get the transaction and the entry
                oTransaction = m_cTransactions(Convert.ToString(lvwEntries.Tag))

                ShowProgressBar(True)
                SetProgressBarMaximum(lvwEntries.Items.Count)
                ' Is the entry already marked? I
                For Each oListItem As ListViewItem In lvwEntries.Items
                    ' Check selected
                    If oListItem.Selected Then
                        ' Get entry
                        oEntry = oTransaction.Item(CInt(oListItem.Index + 1))
                        If oTransaction.IsDebitOrderTransDetail Then
                            MsgBox("One or more of the selected transactions are being managed by the contract system and may not be manually allocated", vbOKOnly + vbExclamation, "Invalid Selection")
                            Return result
                        End If
                        If oEntry.Spare = "WRITEOFF" Then
                            If System.Windows.Forms.DialogResult.Cancel = MessageBox.Show("Unmarking will discard Write-Off Transaction." & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to continue?", Me.Text, MessageBoxButtons.OKCancel) Then
                                Exit For
                            Else
                                'delete Write Off transaction

                                m_lReturn = g_oBusiness.DeleteWriteOffTransaction(Conversion.Val(m_sTransKey.Substring(m_sTransKey.Length - (m_sTransKey.Length - 2))))
                                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", MarkEntry, " + "Unable to unmark transaction entry: " & oEntry.Key)
                                End If
                                IsWOFF = True
                            End If

                        Else
                            ' Mark/unmark it based on current state
                            If oEntry.OutstandingAmount <> oEntry.MarkedAmount Then
                                m_lReturn = MarkEntry(oTransaction, oEntry, oEntry.OutstandingAmount)
                            Else
                                m_lReturn = MarkEntry(oTransaction, oEntry, 0)
                            End If

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                ' If the user is cancelling, alter to true but still exit
                                result = IIf(m_lReturn = gPMConstants.PMEReturnCode.PMCancel, gPMConstants.PMEReturnCode.PMTrue, m_lReturn)
                                Return result
                            End If

                            ' Refresh the entry
                            RefreshEntry(oEntry)
                        End If
                    End If
                    'tsPgBarTransactions.Value += 1
                    AddValueToProgressBar()
                Next oListItem
            End If

            ' Update total marked for payment
            RefreshTotalMarked()
            RefreshTotalWriteOff()
            RefreshUnallocatedAmount()
            EnableDisableAllocateButton()
            ResetProgressBarMaximum()
            ShowProgressBar(False)


        Catch ex As Exception
            result = IIf(Information.Err().Number = Constants.vbObjectError, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMError)

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="MarkEntries", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            ' Always refresh the transaction, if we have one!
            If Not (oTransaction Is Nothing) Then
                RefreshTransaction(oTransaction)
            End If

            If IsWOFF Then PerformSearch()

            ' Set the mouse pointer back to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
        Return result
    End Function

    ' ***********************************************************
    ' Mark for part payment against selected entries
    ' ***********************************************************
    Private Function MarkPartPayment(ByVal bTotalPayment As Boolean) As Integer

        Dim result As Integer = 0
        Dim iSign As Integer
        Dim oTransaction As Transaction
        Dim oEntry As TransactionEntry
        Dim sPayment As String = ""
        Dim iBaseCurrencyID As Integer
        Dim cPayment, cBasePayment As Decimal
        Dim sTemp As String = ""
        result = gPMConstants.PMEReturnCode.PMTrue
        Dim cOutstandingAmount As Decimal
        Try

            ' Set the mouse pointer to busy
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get the transaction
            oTransaction = m_cTransactions(lvwTransactions.FocusedItem.Name)
            If Mid(Trim$(oTransaction.DocumentRef), 1, 3) = "IND" Or
                Mid(Trim$(oTransaction.DocumentRef), 1, 3) = "IED" Or
                Mid(Trim$(oTransaction.DocumentRef), 1, 3) = "IRD" Then
                MarkPartPayment = MarkPartPaymentInst(bTotalPayment)
                Return result
            End If
            ' Check for any marked entries
            For Each oListItem As ListViewItem In lvwEntries.Items
                ' Check for selected
                If oListItem.Selected Then
                    ' Get transaction entry
                    oEntry = oTransaction(CInt(oListItem.Index + 1))

                    If oEntry.Spare <> "WRITEOFF" Then

                        If optViewByTransaction.Checked Then
                            sPayment = CStr(oEntry.OutstandingAmount)
                            cOutstandingAmount = oEntry.OutstandingAmount
                        Else
                            sPayment = CStr(oEntry.OutstandingAccountAmount)
                            cOutstandingAmount = oEntry.OutstandingAccountAmount
                        End If

                        iSign = IIf(StringsHelper.ToDoubleSafe(sPayment) < 0, -1, 1)
                        sPayment = Interaction.InputBox("Partial Offset Amount:", "Partial Offset", gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, Math.Abs(CDbl(sPayment))))

                        Dim dbNumericTemp As Double
                        If Double.TryParse(sPayment, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                            'Offset Amount must be between zero and the Outstandinf Amount
                            If iSign = -1 Then
                                If cPayment > 0 Or cPayment < cOutstandingAmount Then
                                    Call MsgBox("Value specified must be between the defaulted O/S amount and zero.", vbExclamation, Me.Text)
                                    m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                    Exit Function
                                End If
                            Else
                                If cPayment < 0 Or cPayment > cOutstandingAmount Then
                                    Call MsgBox("Value specified must be between the defaulted O/S amount and zero.", vbExclamation, Me.Text)
                                    m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                    Exit Function
                                End If
                            End If
                        End If

                        ' Check return
                        Dim dbNumericTemp2 As Double
                        If Double.TryParse(sPayment, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                            If InStr(1, sPayment, ".") > 0 Then
                                sTemp = Mid(sPayment, 1, InStr(1, sPayment, ".") - 1)
                            Else
                                sTemp = sPayment
                            End If
                            If Len(sTemp) > 12 Then
                                Call MsgBox("Value specified must be between the defaulted O/S amount and zero.", vbExclamation, Me.Text)
                                m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                Exit Function
                            End If

                            'RKS PN13755
                            cPayment = CDec(sPayment) * iSign
                            If Not optViewByTransaction.Checked Then
                                If oEntry.CurrencyID <> oEntry.AccountCurrencyID Then

                                    m_lReturn = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyId:=oEntry.CompanyID, r_iBaseCurrencyID:=iBaseCurrencyID)

                                    If oEntry.AccountCurrencyID <> iBaseCurrencyID Then

                                        m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(lCurrencyID:=oEntry.AccountCurrencyID, lCompanyID:=oEntry.CompanyID, cBaseAmount:=cBasePayment, cCurrencyAmount:=cPayment, vConversionDate:=DateTime.Today, vConversionRate:=oTransaction.AccountCurrencyRate) 'Must use Transcation's Account Currency Rates
                                    Else
                                        cBasePayment = cPayment
                                    End If

                                    If oEntry.CurrencyID <> iBaseCurrencyID Then

                                        m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=oEntry.CurrencyID, lCompanyID:=oEntry.CompanyID, cBaseAmount:=cBasePayment, cCurrencyAmount:=cPayment, vConversionDate:=DateTime.Today, vConversionRate:=oTransaction.CurrencyRate) 'Must use Transcation's Currency Rates
                                    Else
                                        cPayment = cBasePayment
                                    End If
                                End If
                            End If
                            ' Mark the transaction at the supplied amount
                            m_lReturn = MarkEntry(oTransaction, oEntry, cPayment)
                        Else
                            ' Warn the user, return true though as we are warning the user here!!
                            MessageBox.Show("Please enter a valid currency amount", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                        End If

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = m_lReturn
                            Return result
                        End If
                    Else
                        MessageBox.Show("Cannot Part Pay a write off transaction", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)

                    End If
                End If
            Next oListItem
            m_iPreviousCurrencyId = 0
            ' Update total marked for payment
            RefreshTotalMarked()
            RefreshTotalWriteOff()
            RefreshUnallocatedAmount()
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch ex As Exception
            result = IIf(Information.Err().Number = Constants.vbObjectError, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMError)

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="MarkPartPayment", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            ' Always refresh the transaction, if we have one!
            'If Not (oTransaction Is Nothing) Then
            '    RefreshTransaction(oTransaction)
            'End If

            ' Set the mouse pointer back to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
        Return result
    End Function

    ' ***********************************************************
    ' Mark (or unmark) a whole transaction
    ' ***********************************************************
    Private Function MarkTransaction(ByVal oTransaction As Transaction, Optional ByVal bForceMark As Boolean = False, Optional ByRef IsWOFF As Boolean = False, Optional ByVal v_bUnMarkTrans As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim bMarking As Boolean
        Dim oInstalmentEntry As TransactionInst
        Dim oEntry As TransactionEntry
        ' Default response
        result = gPMConstants.PMEReturnCode.PMTrue
        IsWOFF = False
        Try

            ' Set the mouse pointer to busy
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            If v_bUnMarkTrans Then
                bMarking = False
            Else
                ' Check if we should mark or unmark this transaction? also include override flag
                bMarking = (oTransaction.TotalMarked <> oTransaction.TotalOutstanding) Or bForceMark
            End If
            If Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IND" And
                   Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IED" And
                   Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IRD" Then
                oTransaction.Transtype = 0
                ' Process all transaction entries
                For Each oEntry In oTransaction
                    ' Mark the entry.
                    ' Either full amount or zero (unmark)
                    ' If we are forcing a mark we also don't mind about currency errors
                    If oEntry.Spare = "WRITEOFF" Then
                        If System.Windows.Forms.DialogResult.Cancel = MessageBox.Show("Unmarking will discard Write-Off Transaction." & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to continue?", Me.Text, MessageBoxButtons.OKCancel) Then
                            Exit For
                        Else
                            'delete Write Off transaction

                            m_lReturn = g_oBusiness.DeleteWriteOffTransaction(Conversion.Val(m_sTransKey.Substring(m_sTransKey.Length - (m_sTransKey.Length - 2))))
                            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                                Throw New System.Exception(Constants.vbObjectError.ToString() + ", MarkEntry, " + "Unable to unmark transaction entry: " & oEntry.Key)
                            End If
                            IsWOFF = True

                        End If
                    Else
                        m_lReturn = MarkEntry(oTransaction, oEntry, IIf(bMarking, oEntry.OutstandingAmount, 0), bForceMark)

                        ' Check return
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' If the user is cancelling, alter to true but still exit
                            result = IIf(m_lReturn = gPMConstants.PMEReturnCode.PMCancel, gPMConstants.PMEReturnCode.PMTrue, m_lReturn)
                            Return result
                        End If
                    End If
                Next oEntry
            Else
                oTransaction.Transtype = 1
                ' Process all transaction entries
                For Each oInstalmentEntry In oTransaction
                    ' Mark the entry.
                    ' Either full amount or zero (unmark)
                    ' If we are forcing a mark we also don't mind about currency errors
                    If oInstalmentEntry.Spare = "WRITEOFF" Then
                        If vbCancel = MsgBox("Unmarking will discard Write-Off Transaction." & vbCrLf & "Do you wish to continue?", vbOKCancel, Me.Text) Then
                            Exit For
                        Else
                            'delete Write Off transaction
                            m_lReturn = g_oBusiness.DeleteWriteOffTransaction(Val(Strings.Right(m_sTransKey, Len(m_sTransKey) - 2)))
                            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                                Throw New System.Exception(Constants.vbObjectError.ToString() + ", MarkEntry, " + "Unable to unmark transaction entry: " & oEntry.Key)
                            End If
                            IsWOFF = True

                        End If
                    Else
                        m_lReturn = MarkInstEntry(
                            oTransaction,
                            oInstalmentEntry,
                            IIf(bMarking, oInstalmentEntry.OutstandingAmount, 0),
                            bForceMark)

                        ' Check return
                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            ' If the user is cancelling, alter to true but still exit
                            MarkTransaction = IIf(m_lReturn = gPMConstants.PMEReturnCode.PMCancel, gPMConstants.PMEReturnCode.PMTrue, m_lReturn)
                            Return result
                        End If
                    End If
                Next oInstalmentEntry
            End If

        Catch ex As Exception
            ' Set return
            result = IIf(Information.Err().Number = Constants.vbObjectError, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMError)

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="MarkTransaction", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            ' Always refresh the transaction!
            RefreshTransaction(oTransaction)

            ' Set the mouse pointer back to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
        Return result
    End Function

    ' ***********************************************************
    ' Mark all selected transactions
    ' ***********************************************************
    Private Function MarkTransactions(Optional ByRef v_bUnMarkTrans As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim oTransaction As Transaction
        Dim oEntry As TransactionEntry
        Dim IsWOFF As Boolean
        Dim currencyOfMarkedTransaction As String
        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Set the mouse pointer to busy
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            IsWOFF = False
            ' Is the entry already marked? I
            ShowProgressBar(True)
            SetProgressBarMaximum(lvwTransactions.Items.Count)

            For Each oTransaction In m_cTransactions
                If oTransaction.IsMarked = iACTInsurerPaymentSFU.TransactionEntry.MarkedStatusEnum.acmseFullyMarked Then                   
                        currencyOfMarkedTransaction = oTransaction.CurrencyID                   
                    Exit For
                End If
            Next

            '  If String.IsNullOrEmpty(currencyOfMarkedTransaction) OrElse currencyOfMarkedTransaction = 
            For Each oListItem As ListViewItem In lvwTransactions.Items

                If v_bUnMarkTrans Then
                    ' Get the transaction and the entry
                    oTransaction = m_cTransactions.Item(oListItem.Name)
                    If oTransaction.IsDebitOrderTransDetail = False Then
                        ' Mark the transaction in it's default mode
                        m_lReturn = MarkTransaction(oTransaction:=oTransaction, IsWOFF:=IsWOFF, v_bUnMarkTrans:=v_bUnMarkTrans)
                    Else
                        MsgBox("One or more of the selected transactions are being managed by the contract system and may not be manually allocated", vbOKOnly + vbExclamation, "Invalid Selection")
                        Return result
                    End If
                    ' Refresh the transaction anyway!
                    RefreshTransaction(oTransaction, oListItem)

                    ' Now check return state
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        Return result
                    End If
                Else


                    ' Check selected
                    If oListItem.Selected Then
                        ' Get the transaction and the entry
                        oTransaction = m_cTransactions(oListItem.Name)
                        If String.IsNullOrEmpty(currencyOfMarkedTransaction) OrElse (oTransaction.CurrencyID = currencyOfMarkedTransaction) OrElse oTransaction.TotalMarked <> 0 Then
                            If oTransaction.IsDebitOrderTransDetail = False Then
                                ' Mark the transaction in it's default mode
                                m_lReturn = MarkTransaction(oTransaction:=oTransaction, IsWOFF:=IsWOFF, v_bUnMarkTrans:=v_bUnMarkTrans, bForceMark:=v_bUnMarkTrans)
                                ' Refresh the transaction anyway!
                            Else
                                MsgBox("One or more of the selected transactions are being managed by the contract system and may not be manually allocated", vbOKOnly + vbExclamation, "Invalid Selection")
                                Return result
                            End If
                            RefreshTransaction(oTransaction, oListItem)
                        Else
                            MsgBox("Entries with different currencies cannot be selected", vbOKOnly + vbExclamation, "Invalid Selection")
                            Return gPMConstants.PMEReturnCode.PMTrue
                        End If

                        ' Now check return state
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = m_lReturn
                            Return result
                        End If
                    End If
                End If
                ' tsPgBarTransactions.Value += 1
                AddValueToProgressBar()
            Next oListItem
            m_iPreviousCurrencyId = 0
            ' Update total marked for payment
            RefreshTotalMarked()
            RefreshTotalWriteOff()
            RefreshUnallocatedAmount()
            EnableDisableAllocateButton()
            ResetProgressBarMaximum()
            ShowProgressBar(False)


        Catch ex As Exception
            result = IIf(Information.Err().Number = Constants.vbObjectError, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMError)

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="MarkTransactions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            If IsWOFF Then
                PerformSearch()
                lvwTransactions.Items.Item(lvwTransactions.Items.Item(m_sTransKey).Index).Selected = True

                If Not lvwTransactions.FocusedItem Is Nothing Then
                    lvwTransactions.FocusedItem.EnsureVisible()
                End If
            End If

            ' Set the mouse pointer back to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End Try
        Return result
    End Function

    ' ***********************************************************
    ' Perform the search
    ' ***********************************************************
    Private Function PerformSearch() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PerformSearch"

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Force an account refresh (this happens due to the default button stealing focus)
            txtAccountCode_Leave(txtAccountCode, New EventArgs())

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            m_iPreviousCurrencyId = 0
            ' Clear the existing interface (most of it)
            ClearInterface(True)

            If m_lAccountId = 0 Then
                MessageBox.Show("Please select a valid account", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return result
            End If

            ' Check for payment sources
            If Not Information.IsArray(m_vPaySources) Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", PerformSearch, Unable to validate payment groups")
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.6.1)
            m_lReturn = LoadInterfaceDisplaySettings(lvwTransactions)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "LoadInterfaceDisplaySettings Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = LoadInterfaceDisplaySettings(lvwEntries)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "LoadInterfaceDisplaySettings Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.6.1)
            'g_oDOMRootForInterfaceDisplay.save "c:\Input.xml"


        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="PerformSearch", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
        Return result
    End Function

    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.6.1)
    Public Function LoadInterfaceDisplaySettings(ByRef oListView As ListView) As Integer

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

            sXMLPath = "//" & m_kRootNode & "/" & m_kComponentName & "/" & m_kFrmName & "/" & oListView.Name

            g_oDOMRootForInterfaceDisplay = New XmlDocument()
            Try
                g_oDOMRootForInterfaceDisplay.LoadXml(g_sUserConfigXMLDataset)

            Catch
            End Try
            'TODO: Vikas


            Return result

            'Devrloper Guide No.36 (No Solutions)
            If g_oDOMRootForInterfaceDisplay.InnerText <> 0 Then

                'Devrloper Guide No.36 (No Solutions)
                gPMFunctions.RaiseError(kMethodName, g_oDOMRootForInterfaceDisplay.InnerText, gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not (g_oDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath) Is Nothing) Then
                If g_oDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath).HasChildNodes Then

                    sColumnWidth = g_oDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath & "/" & m_kColumnWidth).InnerText

                    oXMLColumnWidth = sColumnWidth.Split(";"c)

                    For iNoOfColumns As Integer = 0 To oXMLColumnWidth.GetUpperBound(0) - 1

                        oListView.Columns.Item(iNoOfColumns).Width = CInt(VB6.TwipsToPixelsX(gPMFunctions.ToSafeDecimal(CStr(oXMLColumnWidth(iNoOfColumns)), 0)))
                    Next

                    If m_kTransGridName = oListView.Name Then

                        m_iTransSortKey = gPMFunctions.ToSafeInteger(g_oDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath & "/" & m_kColumnSortKey).InnerText, -1)
                        m_iTransSortOrder = gPMFunctions.ToSafeInteger(g_oDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath & "/" & m_kColumnSortOrder).InnerText, -1)
                        If m_iTransSortKey > 0 Then
                            m_lReturn = SortListViewByKey(oListView, m_iTransSortKey, m_iTransSortOrder)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "SortListViewByKey Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If

                    ElseIf m_kEntriesGridName = oListView.Name Then
                        m_iEntriesSortKey = gPMFunctions.ToSafeInteger(g_oDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath & "/" & m_kColumnSortKey).InnerText, -1)
                        m_iEntriesSortOrder = gPMFunctions.ToSafeInteger(g_oDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath & "/" & m_kColumnSortOrder).InnerText, -1)
                        If m_iEntriesSortKey > 0 Then
                            m_lReturn = SortListViewByKey(oListView, m_iEntriesSortKey, m_iEntriesSortOrder)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "SortListViewByKey Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
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
    'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.6.1)

    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.6.1)
    Private Function SortListViewByKey(ByVal oListView As ListView, ByVal iSortKey As Integer, ByVal iSortOrder As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SortListViewByKey"

        Try
            result = gPMConstants.PMEReturnCode.PMTrue



            If oListView.Name = "lvwTransactions" Then
                ' If current sort column header is pressed.
                Select Case iSortKey - 1
                    Case MainModule.ListViewTransactionEnum.ACLTHolderName, MainModule.ListViewTransactionEnum.ACLTInsuranceRef, MainModule.ListViewTransactionEnum.ACLTDocumentRef, MainModule.ListViewTransactionEnum.ACLTHolderCode, MainModule.ListViewTransactionEnum.ACLTAlternateRef 'PN 33593 (RC)
                        ' String columns
                        ListViewHelper.SetSortedProperty(lvwTransactions, False)
                        ListViewHelper.SetSortKeyProperty(lvwTransactions, iSortKey - 1)
                        ListViewHelper.SetSortOrderProperty(lvwTransactions, iSortOrder)
                        ListViewHelper.SetSortedProperty(lvwTransactions, True)

                    Case MainModule.ListViewTransactionEnum.ACLTAccountingDate, MainModule.ListViewTransactionEnum.ACLTEffectiveDate 'PN 33593 (RC)
                        ' Date columns
                        'Devrloper Guide No.178 (Latest Guide)
                        m_lReturn = ListView6Func.ListViewSortByDate(v_oListView:=oListView, v_iSourceColumn:=iSortKey - 1, v_iDirection:=iSortOrder, v_bMarkSortedColumn:=True)
                    Case MainModule.ListViewTransactionEnum.ACLTCurrencyTotal, MainModule.ListViewTransactionEnum.ACLTPaidTotal, MainModule.ListViewTransactionEnum.ACLTNetTotal, MainModule.ListViewTransactionEnum.ACLTMarkedTotal, MainModule.ListViewTransactionEnum.ACLTClientOS
                        ' Currency columns
                        'Devrloper Guide No.178 (Latest Guide)
                        m_lReturn = ListView6Func.ListViewSortByValue(v_oListView:=oListView, v_iSourceColumn:=iSortKey - 1, v_iDirection:=iSortOrder, v_bMarkSortedColumn:=True, v_bIsCurrency:=True)
                End Select
            ElseIf oListView.Name = "lvwEntries" Then
                Select Case iSortKey - 1
                    Case MainModule.ListViewEntryEnum.ACLECompany, MainModule.ListViewEntryEnum.ACLEPeriod, MainModule.ListViewEntryEnum.ACLEDocumentRef, MainModule.ListViewEntryEnum.ACLESpare
                        ' String columns
                        ListViewHelper.SetSortedProperty(oListView, False)
                        ListViewHelper.SetSortKeyProperty(oListView, iSortKey - 1)
                        ListViewHelper.SetSortOrderProperty(oListView, iSortOrder)
                        ListViewHelper.SetSortedProperty(oListView, True)

                    Case MainModule.ListViewEntryEnum.ACLECurrencyAmount, MainModule.ListViewEntryEnum.ACLEPaidAmount, MainModule.ListViewEntryEnum.ACLENetAmount, MainModule.ListViewEntryEnum.ACLEMarkedAmount
                        ' Currency columns
                        'Devrloper Guide No.178 (Latest Guide)
                        m_lReturn = ListView6Func.ListViewSortByValue(v_oListView:=oListView, v_iSourceColumn:=iSortKey - 1, v_iDirection:=iSortOrder, v_bMarkSortedColumn:=True, v_bIsCurrency:=True)
                End Select
            End If
            Return result

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            Return result
        End Try
    End Function
    'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.6.1)

    ' ***********************************************************
    ' Pay Transactions with a binder journal
    ' ***********************************************************
    Private Function ProcessBinder() As Integer

        Dim result As Integer = 0
        Dim cBinderAmount As Decimal
        Dim lBinderItems As Integer
        Dim vTransDetailIDs As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Busy mouse
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Create the array now so we don't have to check it every loop!
            ReDim vTransDetailIDs(0)

            ' Build an array of marked transdetails
            For Each oTransaction As Transaction In m_cTransactions
                ' Check for consolidate binder items
                If oTransaction.IsConsolidateBinder Then
                    MessageBox.Show("Cannot binder previously binder items", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    result = gPMConstants.PMEReturnCode.PMCancel
                    Return result
                End If

                ' Check each entry
                For Each oEntry As TransactionEntry In oTransaction
                    ' Check status
                    If oEntry.MarkedAmount <> 0 Then
                        ' Resize the array and store detail id
                        ReDim Preserve vTransDetailIDs(lBinderItems)

                        vTransDetailIDs(lBinderItems) = oEntry.DetailID

                        ' Increment counters
                        cBinderAmount += oEntry.MarkedAmount
                        lBinderItems += 1
                    End If
                Next oEntry
            Next oTransaction

            ' Check for selected items
            If lBinderItems = 0 Then
                MessageBox.Show("Nothing has been selected for binding", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                result = gPMConstants.PMEReturnCode.PMCancel
                Return result
            End If

            ' Process the binder

            m_lReturn = g_oBusiness.ProcessBinder(v_lAccountId:=m_lAccountId, v_cPayment:=cBinderAmount, v_iCurrencyID:=m_lActiveCurrencyID, v_vTransdetailIds:=vTransDetailIDs)

            ' Check return
            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Success
                    MessageBox.Show("Binder Payment successful", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' The insurer suspense account is missing
                    MessageBox.Show("Please create INSURERSUSPENSE account", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    result = gPMConstants.PMEReturnCode.PMCancel

                Case gPMConstants.PMEReturnCode.PMInvalidRequest
                    ' The total amount marked was zero, unable to binder
                    MessageBox.Show("Cannot binder a zero total", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    result = gPMConstants.PMEReturnCode.PMCancel

                Case Else
                    ' Return any other status
                    result = m_lReturn
            End Select


        Catch ex As Exception
            ' Set return
            result = IIf(Information.Err().Number = Constants.vbObjectError, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMError)

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessBinder", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            ' Reset mouse
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
        Return result
    End Function
    ''' <summary>
    ''' ProcessPay
    ''' </summary>
    ''' <param name="bIsAllocate"></param>
    ''' <returns></returns>
    ''' <remarks>Process a Payment</remarks>
    Private Function ProcessPay(Optional ByVal bIsAllocate As Boolean = False) As Integer
        Const kMethodName As String = "ProcessPay"

        Dim oTransaction As Transaction
        Dim oEntry As TransactionEntry
        Dim cPayAmount As Decimal
        Dim nPayItems As Integer
        Dim nBatchID As Integer
        Dim oTransDetailIDs As Object
        Dim oKeyArray(,) As Object

        Dim cPayAccountAmount As Decimal
        Dim bMultipleCurrencies As Boolean
        Dim nCurrencyID As Integer
        Dim nAccountCurrencyID As Integer

        Dim crWriteOffAmt As Decimal
        Dim crWriteOffAccountAmt As Decimal

        Dim sPreviousPayRef As String
        Dim bIsSimilarPayRef As Boolean
        Dim sOptionValue As String
        Dim bPaymentRefCheckEnabled As Boolean

        Dim oInnerKeyArray As Object
        Dim oInstalmentEntry As TransactionInst
        Dim oInstalmentArray As Object
        Dim sTransDetailId As String
        Dim nPremiumFinanceCnt As Integer
        Dim nPremiumFinanceVersion As Integer
        Dim crPayedAmount As Decimal
        Dim crPayedAccountAmount As Decimal
        Dim nCount As Integer
        Dim sViewType As String
        m_bIsUnallocatedAmountForPost = False

        Dim nBaseCurrencyID As Integer
        Dim crReceiptCurrAmount As Decimal
        Dim crBaseAmount As Decimal
        Dim crMarkBaseAmountUnrounded As Decimal
        Dim crReceiptCurrTotal As Decimal
        Dim nCashListCount As Integer
        Dim bIsOtherTransaction As Boolean
        Dim bIsSingleCashListItemAllocation As Boolean
        Dim bIsDebitPolicyTrans As Boolean
        Dim bIsCreditPolicyTrans As Boolean
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim nTransBranchID As Integer
        Try

            ProcessPay = PMEReturnCode.PMTrue
            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseBusy)
            ReDim oTransDetailIDs(0 To 0)

            cPayAmount = 0
            cPayAccountAmount = 0
            bMultipleCurrencies = False
            nCurrencyID = 0
            nAccountCurrencyID = 0
            crWriteOffAmt = 0
            crWriteOffAccountAmt = 0
            bIsSimilarPayRef = True
            sPreviousPayRef = ""
            crReceiptCurrTotal = 0

            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=kSysOptPaymentRefCheck, r_sOptionValue:=sOptionValue)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessPay, GetSystemOption Payment Ref Check Failed")
            End If


            If sOptionValue = "1" Then
                bPaymentRefCheckEnabled = True
            Else
                bPaymentRefCheckEnabled = False
            End If

            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=kSysOptSingleCashReceipt, r_sOptionValue:=sOptionValue)
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                RaiseError("ProcessPay", "GetSystemOption Single Cash Reciept/Payment per Allocation Check Failed", vbObjectError)
            End If

            If sOptionValue = "1" Then
                bIsSingleCashListItemAllocation = True
            Else
                bIsSingleCashListItemAllocation = False
            End If

            nCurrencyID = UctCurrency.CurrencyId 'Richard Clarke

            If m_iSourceId <> 0 Then
                m_lReturn = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyId:=m_iSourceId,
                            r_iBaseCurrencyID:=nBaseCurrencyID)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    RaiseError("ProcessPay", "Get Base Currency failed", vbObjectError)
                End If
            End If
            ' ShowProgressBar(True)
            SetProgressBarMaximum(m_cTransactions.Count)

            ' Build an array of marked transdetails
            For Each oTransaction In m_cTransactions

                If ValidSource(vSource:=oTransaction.CompanyID) = True Then
                    ' Check each entry
                    For Each oEntry In oTransaction
                        ' Check status
                        If oEntry.MarkedAmount <> 0 Then
                            If Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IND" AndAlso
                                Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IED" AndAlso
                                Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IRD" Then
                                ' Resize the array and store detail id
                                ReDim Preserve oTransDetailIDs(nPayItems)
                                oTransDetailIDs(nPayItems) = oEntry.DetailID

                                ' Add the total marked in transaction currency
                                ' if the user changed the currency we need to do a currency conversion here.

                                'Set transaction currency and check if there is more than one currency involved.
                                If nCurrencyID = 0 Then
                                    nCurrencyID = oEntry.CurrencyID
                                ElseIf nCurrencyID <> oEntry.CurrencyID Then
                                    bMultipleCurrencies = True
                                End If

                                crReceiptCurrAmount = 0
                                crBaseAmount = 0
                                crMarkBaseAmountUnrounded = 0

                                If nCurrencyID <> oEntry.CurrencyID Then

                                    m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(
                                                lCurrencyID:=oEntry.CurrencyID,
                                                lCompanyID:=m_iSourceId,
                                                cBaseAmount:=crBaseAmount,
                                                cCurrencyAmount:=oEntry.MarkedAmount,
                                                vConversionDate:=DateTime.Now,
                                                vBaseAmountUnRounded:=crMarkBaseAmountUnrounded)

                                    If nCurrencyID <> nBaseCurrencyID Then
                                        m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(
                                                    lCurrencyID:=nCurrencyID,
                                                    lCompanyID:=m_iSourceId,
                                                    cBaseAmount:=crBaseAmount,
                                                    cCurrencyAmount:=crReceiptCurrAmount,
                                                    vConversionDate:=DateTime.Now)
                                    End If
                                    crReceiptCurrTotal = crReceiptCurrTotal + crReceiptCurrAmount
                                Else
                                    cPayAmount = cPayAmount + oEntry.MarkedAmount
                                End If

                                ' Add the total marked in account currency
                                cPayAccountAmount += oEntry.MarkedAccountAmount

                                'Add Write-Off Amount
                                If oEntry.Spare = "WRITEOFF" Then
                                    crWriteOffAmt += oEntry.MarkedAmount
                                    crWriteOffAccountAmt += oEntry.MarkedAccountAmount
                                End If



                                'Set transaction currency and check if there are more than one currency involved.
                                If nCurrencyID = 0 Then
                                    nCurrencyID = oEntry.CurrencyID
                                ElseIf nCurrencyID <> oEntry.CurrencyID Then
                                    bMultipleCurrencies = True
                                End If

                                'Set account currency. Only need to do it once as it will be the same on all entries.
                                If nAccountCurrencyID = 0 Then
                                    nAccountCurrencyID = oEntry.AccountCurrencyID
                                End If

                                'Set account currency. Only need to do it once as it will be the same on all entries.
                                If nAccountCurrencyID = 0 Then
                                    nAccountCurrencyID = oEntry.AccountCurrencyID
                                End If

                                nPayItems += 1

                                If bPaymentRefCheckEnabled = True Then
                                    If nPayItems > 1 AndAlso bIsSimilarPayRef = True Then
                                        'look if all payment refs are same
                                        If sPreviousPayRef <> gPMFunctions.ToSafeString(CSng(oEntry.AltRef)).ToUpper() Then
                                            bIsSimilarPayRef = False
                                        End If
                                    End If

                                    sPreviousPayRef = gPMFunctions.ToSafeString(CSng(oEntry.AltRef)).ToUpper()

                                End If
                                nTransBranchID = oTransaction.CompanyID
                            End If
                            If UCase(Strings.Left(oTransaction.DocumentRef, 3)) = "SRP" Or UCase(Strings.Left(oTransaction.DocumentRef, 3)) = "SPY" Then
                                nCashListCount = nCashListCount + 1
                            Else
                                bIsOtherTransaction = True
                            End If
                        End If
                    Next oEntry
                End If
                AddValueToProgressBar()
            Next oTransaction

            If ((bIsDebitPolicyTrans = True OrElse bIsCreditPolicyTrans = True OrElse bIsOtherTransaction = True) AndAlso
              ((nCashListCount = 1 AndAlso cPayAmount <> 0) OrElse (nCashListCount > 1))) AndAlso
          bIsSingleCashListItemAllocation = True Then
                If nCashListCount > 1 Then
                    MsgBox("You can only allocate 1SRP OR 1SPY in a single allocation with other transaction types", vbOKOnly)
                ElseIf nCashListCount = 1 AndAlso cPayAmount > 0 Then
                    MsgBox("This payment would result in as SRP being generated and there is already an SPY or SRP in the list and you can't allocate more than one in a single Allocation", vbOKOnly)
                ElseIf nCashListCount = 1 AndAlso cPayAmount < 0 Then
                    MsgBox("This payment would result in as SPY being generated and there is already an SPR or SPY in the list and you can't allocate more than one in a single Allocation", vbOKOnly)
                End If
                ProcessPay = PMEReturnCode.PMCancel
                iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
                Exit Function
            End If

            ' Check for selected items
            If nPayItems = 0 Then
                nResult = PMEReturnCode.PMNotFound
                Return nResult
            ElseIf Not bIsSimilarPayRef AndAlso bPaymentRefCheckEnabled Then
                If System.Windows.Forms.DialogResult.Cancel =
                    MessageBox.Show("Warning: All marked items do not have the same payment reference." &
                                    Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue?",
                                    "Insurer Payment",
                                    MessageBoxButtons.OKCancel) Then
                    nResult = PMEReturnCode.PMCancel
                    Return nResult
                End If
            Else
                If m_bReciptOrPay = True Then
                    cPayAmount = cPayAmount + ConvertCurrencyStringToValue(txtunallocatedAmount.Text)
                End If
                ' Create a batch
                m_lReturn = CreateBatch(
                    v_vTransDetailIDs:=oTransDetailIDs,
                    r_lBatchID:=nBatchID)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() +
                                               ", ProcessPay, Unable to create batch for payment.")
                End If

                ' Get an instance of iPMNavStart
                m_oNavigatorXM = New iPMNavigatorXM.Interface_Renamed

                ' Initialise it
                m_lReturn = m_oNavigatorXM.Initialise()
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() +
                                               ", ProcessPay, Failed to initialise iPMNavStart.Interface.")
                End If

                ' Set properties
                m_oNavigatorXM.CallingAppName = ACApp
                'The XML roadmap to use
                m_oNavigatorXM.XMLFileName = "ACTIPAY.XML"

                ' Set the keys
                ReDim oKeyArray(0 To 1, 0 To 7)
                ReDim oInnerKeyArray(0 To 1, 0 To 2)
                oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 0) = PMKeyNameBatchID
                oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 0) = nBatchID
                oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 1) = ACTKeyNameAccountID
                oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lAccountId

                oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 5) = ACTKeyAllowAllocateButton
                oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 5) = "-1"

                oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 6) = ACTKeyNameCashListAllocationRoadmap
                oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 6) = "ACTINSPAY2"

                oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.PMKeyNameSourceId
                oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 7) = nTransBranchID

                If optViewByTransaction.Checked = True AndAlso Not bMultipleCurrencies Then
                    oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 0) = ACTKeyNameInsurerPayment
                    oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 0) = cPayAmount
                    If Trim(UCase(m_sPartyType)) = "AG" Then
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 1) = ACTKeyNameCashListItemPaymentTypeID
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 1) = 7
                    ElseIf Trim(UCase(m_sPartyType)) = "BR" Then
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 1) = ACTKeyNameCashListItemPaymentTypeID
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 1) = 7
                    ElseIf Trim(UCase(m_sPartyType)) = "IN" Then
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 1) = ACTKeyNameCashListItemPaymentTypeID
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 1) = 6
                    Else
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 1) = ACTKeyNameCashListItemPaymentTypeID
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 1) = 9
                    End If
                    oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 2) = "NoWriteOff"
                    oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 2) = "1"
                    oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 2) = ACTKeyNameInsurerPayment
                    oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 2) = oInnerKeyArray
                    oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 3) = ACTKeyNameCurrencyID
                    oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 3) = nCurrencyID
                    oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 4) = ACTKeyNameWriteOffAmount
                    oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 4) = crWriteOffAmt
                    'oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 6) = uctTransactionCurrency.CurrencyId
                Else
                    oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 0) = ACTKeyNameInsurerPayment
                    oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 0) = cPayAccountAmount
                    If Trim(UCase(m_sPartyType)) = "AG" Then
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 1) = ACTKeyNameCashListItemPaymentTypeID
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 1) = 7
                    ElseIf Trim(UCase(m_sPartyType)) = "BR" Then
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 1) = ACTKeyNameCashListItemPaymentTypeID
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 1) = 7
                    ElseIf Trim(UCase(m_sPartyType)) = "IN" Then
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 1) = ACTKeyNameCashListItemPaymentTypeID
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 1) = 6
                    Else
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 1) = ACTKeyNameCashListItemPaymentTypeID
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 1) = 9
                    End If
                    oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 2) = "NoWriteOff"
                    oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 2) = "1"
                    oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 2) = ACTKeyNameInsurerPayment
                    oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 2) = oInnerKeyArray
                    oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 3) = ACTKeyNameCurrencyID
                    oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 3) = nCurrencyID
                    oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 4) = ACTKeyNameWriteOffAmount
                    oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 4) = crWriteOffAccountAmt
                    oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 5) = "NoWriteOff"
                    oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 5) = "1"
                    'oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 6) = uctTransactionCurrency.CurrencyId

                End If

                m_lReturn = m_oNavigatorXM.SetKeys(vKeyArray:=oKeyArray)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() +
                                               ", ProcessPay, Unable to assign key array to payment navigator")
                End If


                ' Reset mouse (so it doesn't interfere with payment screens)
                iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)

                ' Reset completed flag
                m_bNavCompleted = False

                ' Start navigator
                m_lReturn = m_oNavigatorXM.Start()
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() +
                                               ", ProcessPay, Failed to start payment navigator")
                End If

                ' Don't continue till roadmap ends
                Do
                    Application.DoEvents()
                Loop While Not m_bNavCompleted

                ' Check return status
                If (m_bProcessComplete AndAlso m_oNavigatorXM.Status <> PMEReturnCode.PMCancel AndAlso m_oNavigatorXM.Status <> PMEReturnCode.PMError) Then
                    ProcessPay = PMEReturnCode.PMTrue
                    '--Call Remitance Advice Report if payment succeeded
                    Call ShowRemitanceAdviceReport()
                Else
                    'check the navigator status as they might have cancelled on cashlist screen
                     nResult = m_oNavigatorXM.Status
                    'ProcessPay = m_oNavigatorXM.Status
                    Return nResult
                End If
                m_oNavigatorXM.NavigatorClose_Renamed()
                m_oNavigatorXM.Dispose()
            End If

            'Get user payemnt authority

            Dim oAthourityResultArray As Object
            Dim bUserHasPaymentAuthority As Boolean
            Dim crUserAuthorityLimit As Double

            m_lReturn = m_oUserAuthority.GetUserAuthoritiesDetails(g_iUserID, oAthourityResultArray)
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                RaiseError("ProcessPay", "Unable to Get UserAthourity", vbObjectError)
            End If

            If IsArray(oAthourityResultArray) Then
                If oAthourityResultArray(10, 0) = 0 Then
                    bUserHasPaymentAuthority = False
                Else
                    crUserAuthorityLimit = ToSafeDouble(oAthourityResultArray(11, 0))
                    bUserHasPaymentAuthority = True
                End If
            End If

            If cPayAmount > crUserAuthorityLimit And bUserHasPaymentAuthority = True Then
                MsgBox("This Payment exceeds the payment limit of user", vbOKOnly)
                ProcessPay = PMEReturnCode.PMCancel
                Return nResult
            End If

            crPayedAmount = cPayAmount
            crPayedAccountAmount = cPayAccountAmount

            If (nPayItems = 0 AndAlso ConvertCurrencyStringToValue(txtunallocatedAmount.Text) <> 0) Then
                'need to populate two variables for outstanding amount with installments
                m_cUnallocatedAmountForPost = ConvertCurrencyStringToValue(txtunallocatedAmount.Text)
                crPayedAmount = ConvertCurrencyStringToValue(txtunallocatedAmount.Text)
                m_bIsUnallocatedAmountForPost = True
            End If

            cPayAmount = 0
            cPayAccountAmount = 0
            nPayItems = 0
            crReceiptCurrTotal = 0

            nCurrencyID = UctCurrency.CurrencyId
            ' Build an array of marked transdetails (for instalment entries i.e IND,IED,IRD
            For Each oTransaction In m_cTransactions

                If ValidSource(vSource:=oTransaction.CompanyID) = True Then
                    If Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IND" AndAlso
                        Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IED" AndAlso
                        Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IRD" Then

                    Else
                        ' Check each entry
                        For Each oInstalmentEntry In oTransaction
                            ' Check status
                            If oInstalmentEntry.MarkedAmount <> 0 Then
                                ' Resize the array and store detail id
                                ReDim Preserve oTransDetailIDs(0 To nPayItems)
                                oTransDetailIDs(nPayItems) = oInstalmentEntry.DetailID

                                nPremiumFinanceCnt = oInstalmentEntry.PremiumFinanceCnt
                                nPremiumFinanceVersion = oInstalmentEntry.PremiumFinanceVersion
                                sTransDetailId = oInstalmentEntry.DetailID
                                crReceiptCurrAmount = 0
                                crBaseAmount = 0
                                crMarkBaseAmountUnrounded = 0

                                'Set transaction currency and check if there is more than one currency involved.
                                If nCurrencyID = 0 Then
                                    nCurrencyID = oInstalmentEntry.CurrencyID
                                ElseIf nCurrencyID <> oInstalmentEntry.CurrencyID Then
                                    bMultipleCurrencies = True
                                End If

                                If nCurrencyID <> oInstalmentEntry.CurrencyID Then

                                    m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(
                                                lCurrencyID:=oInstalmentEntry.CurrencyID,
                                                lCompanyID:=m_iSourceId,
                                                cBaseAmount:=crBaseAmount,
                                                cCurrencyAmount:=oInstalmentEntry.MarkedAmount,
                                                vConversionDate:=DateTime.Now,
                                                vBaseAmountUnRounded:=crMarkBaseAmountUnrounded)

                                    If nCurrencyID <> nBaseCurrencyID Then
                                        m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(
                                                    lCurrencyID:=nCurrencyID,
                                                    lCompanyID:=m_iSourceId,
                                                    cBaseAmount:=crBaseAmount,
                                                    cCurrencyAmount:=crReceiptCurrAmount,
                                                    vConversionDate:=DateTime.Now)
                                    End If
                                    crReceiptCurrTotal = crReceiptCurrTotal + crReceiptCurrAmount
                                Else
                                    cPayAmount = cPayAmount + oInstalmentEntry.MarkedAmount
                                End If

                                ' Add the total marked in account currency
                                cPayAccountAmount = cPayAccountAmount + oInstalmentEntry.MarkedAccountAmount

                                'Add Write-Off Amount
                                If oInstalmentEntry.Spare = "WRITEOFF" Then
                                    crWriteOffAmt = crWriteOffAmt + oInstalmentEntry.MarkedAmount
                                    crWriteOffAccountAmt = crWriteOffAccountAmt + oInstalmentEntry.MarkedAccountAmount
                                End If

                                'Set transaction currency and check if there are more than one currency involved.
                                If nCurrencyID = 0 Then
                                    nCurrencyID = oInstalmentEntry.CurrencyID
                                ElseIf nCurrencyID <> oInstalmentEntry.CurrencyID Then
                                    bMultipleCurrencies = True
                                End If

                                'Set account currency. Only need to do it once as it will be the same on all entries.
                                If nAccountCurrencyID = 0 Then
                                    nAccountCurrencyID = oInstalmentEntry.AccountCurrencyID
                                End If

                                nPayItems = nPayItems + 1

                                If bPaymentRefCheckEnabled = True Then
                                    If nPayItems > 1 AndAlso bIsSimilarPayRef = True Then
                                        'look if all payment refs are same
                                        If sPreviousPayRef <> UCase(ToSafeString(oInstalmentEntry.AltRef)) Then
                                            bIsSimilarPayRef = False
                                        End If
                                    End If

                                    sPreviousPayRef = UCase(ToSafeString(oInstalmentEntry.AltRef))
                                End If
                            End If
                        Next oInstalmentEntry
                    End If
                End If

            Next oTransaction



            ' Check for selected items
            If (nPayItems = 0) Then
                'ProcessPay = PMNotFound
                'GoTo Finally
            ElseIf bIsSimilarPayRef = False AndAlso bPaymentRefCheckEnabled = True Then
                If vbCancel = MsgBox("Warning: All marked items do not have the same payment reference." &
                                     vbCrLf & "Do you want to continue?", vbOKCancel, Me.Text) Then
                    ProcessPay = PMEReturnCode.PMCancel
                    Return nResult
                End If
            Else

                If m_bReciptOrPay Then
                    If m_bIsUnallocatedAmountForPost Then
                        cPayAmount = ConvertCurrencyStringToValue(txtMarked.Text) + ConvertCurrencyStringToValue(txtunallocatedAmount.Text)
                    Else
                        cPayAmount = ConvertCurrencyStringToValue(txtMarked.Text) + ConvertCurrencyStringToValue(txtunallocatedAmount.Text) - crPayedAmount
                    End If
                    cPayAccountAmount = ConvertCurrencyStringToValue(g_oBusiness.FormatCurrency(
                                                v_lCurrencyID:=nAccountCurrencyID,
                                                v_cCurrencyAmount:=cPayAmount))
                End If
                sViewType = ""
                If optViewByTransaction.Checked = True Then
                    sViewType = "TRN"
                ElseIf optViewByAccount.Checked = True Then
                    sViewType = "ACC"
                End If
                m_lReturn = g_oBusiness.GetInstalmentDetailsForInsurerPayment(m_lAccountId, sViewType, oInstalmentArray)

                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessPay, Unable to create batch for payment.")
                End If
                ' Create a batch
                m_lReturn = CreateBatch(
                    v_vTransDetailIDs:=oTransDetailIDs,
                    r_lBatchID:=nBatchID)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessPay, Unable to create batch for payment.")
                End If


                If bIsAllocate = False Then

                    ' Get an instance of iPMNavStart
                    m_oNavigatorXM = New iPMNavigatorXM.Interface_Renamed

                    ' Initialise it
                    m_lReturn = m_oNavigatorXM.Initialise()
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessPay, Failed to initialise iPMNavStart.Interface.")
                    End If
                    ' Set properties
                    m_oNavigatorXM.CallingAppName = "iACTInsurerPaymentInst"
                    m_oNavigatorXM.XMLFileName = "ACTIPAYINST.XML"
                    ''m_oNavigatorXM.ProcessCode = ACProcessCode

                    ' Set the keys
                    ReDim oKeyArray(0 To 1, 0 To 4)
                    ' Set the keys
                    If m_bIsUnallocatedAmountForPost Then
                        ReDim oInnerKeyArray(0 To 1, 0 To 4)
                    Else
                        ReDim oInnerKeyArray(0 To 1, 0 To 2)
                    End If
                    oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 0) = PMKeyNameBatchID
                    oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 0) = nBatchID
                    oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 1) = ACTKeyNameAccountID
                    oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lAccountId

                    If optViewByTransaction.Checked = True AndAlso Not bMultipleCurrencies Then
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 0) = ACTKeyNameInsurerPayment
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 0) = cPayAmount
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 1) = "cashlistitem_receipt_type_id"
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 1) = 2
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 2) = "InstalmentArray"
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 2) = oInstalmentArray
                        If m_bIsUnallocatedAmountForPost Then
                            oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 3) = ACTKeyNameUnallocatedAmountForPost
                            oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_cUnallocatedAmountForPost
                            oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 4) = ACTKeyNameIsUnallocatedAmountForPost
                            oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_bIsUnallocatedAmountForPost
                        End If
                        oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 2) = ACTKeyNameInsurerPayment
                        oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 2) = oInnerKeyArray
                        oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 3) = ACTKeyNameCurrencyID
                        oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 3) = nCurrencyID
                        oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 4) = ACTKeyNameWriteOffAmount
                        oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 4) = crWriteOffAmt
                    Else
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 0) = ACTKeyNameInsurerPayment
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 0) = crReceiptCurrTotal
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 1) = "cashlistitem_receipt_type_id"
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 1) = 2
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 2) = "InstalmentArray"
                        oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 2) = oInstalmentArray
                        If m_bIsUnallocatedAmountForPost Then
                            oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 3) = ACTKeyNameUnallocatedAmountForPost
                            oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_cUnallocatedAmountForPost
                            oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 4) = ACTKeyNameIsUnallocatedAmountForPost
                            oInnerKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_bIsUnallocatedAmountForPost
                        End If
                        oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 2) = ACTKeyNameInsurerPayment
                        oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 2) = oInnerKeyArray
                        oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 3) = ACTKeyNameCurrencyID
                        oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 3) = nCurrencyID
                        oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 4) = ACTKeyNameWriteOffAmount
                        oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 4) = crWriteOffAccountAmt
                    End If

                    m_lReturn = m_oNavigatorXM.SetKeys(vKeyArray:=oKeyArray)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessPay, Unable to assign key array to payment navigator")
                    End If
                    ' Reset mouse (so it doesn't interfere with payment screens)
                    iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)

                    ' Reset completed flag
                    m_bNavCompleted = False

                    ' Start navigator
                    m_lReturn = m_oNavigatorXM.Start()
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessPay, Failed to start payment navigator")
                    End If
                    ' Don't continue till roadmap ends
                    Do
                        Application.DoEvents()
                    Loop While Not m_bNavCompleted

                Else

                    m_lReturn = UpdateWriteOffDocumentRef(v_lBatchID:=nBatchID)
                    If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                        RaiseError("ProcessPay", "UpdateWriteOffDocumentRef Failed", vbObjectError)
                    End If

                End If

                ' Check return status
                '' If (m_bProcessComplete) Then
                If (m_bProcessComplete AndAlso m_oNavigatorXM.Status <> PMEReturnCode.PMCancel AndAlso m_oNavigatorXM.Status <> PMEReturnCode.PMError) Then

                    ProcessPay = PMEReturnCode.PMTrue
                    For nCount = 0 To UBound(oTransDetailIDs)
                        m_lReturn = g_oBusiness.DeleteTransMatchInst(
                                  v_iTransdetailid:=oTransDetailIDs(nCount))

                        If (m_lReturn <> PMEReturnCode.PMTrue) AndAlso (m_lReturn <> PMEReturnCode.PMNotFound) Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessPay,Unable to delete transmatch")
                        End If
                    Next
                Else
                    ProcessPay = m_oNavigatorXM.Status
                End If
                m_oNavigatorXM.NavigatorClose_Renamed()
                m_oNavigatorXM.Dispose()
            End If

            nResult = PMEReturnCode.PMTrue

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:=kMethodName & " function failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, sUsername:=g_oObjectManager.UserName, excep:=ex)
        Finally

        End Try
        Return nResult
        iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)

    End Function

    ' ***********************************************************
    ' Refresh the transaction entries for the current transaction
    ' ***********************************************************
    Private Sub RefreshEntries(ByVal ListItem As ListViewItem)

        Dim oTransaction As Transaction = Nothing
        Dim oListItem As ListViewItem
        Dim lRow As Integer

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Try

            HasWriteoff = False
            lvwEntries.Enabled = False
            lvwInstalmentEntries.Enabled = False
            lvwEntries.Items.Clear()
            lvwInstalmentEntries.Items.Clear()

            ' Get the transaction
            oTransaction = m_cTransactions.Item(ListItem.Name)
            m_sDocRef = oTransaction.DocumentRef
            If Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IND" And
                       Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IED" And
                       Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IRD" Then
                ' Process entries
                lvwInstalmentEntries.Visible = False
                lvwEntries.Visible = True
                lRow = 1
                oTransaction.Transtype = 0
                For Each oEntry As TransactionEntry In oTransaction
                    ' Start - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.4.1.3)
                    ' Add new item
                    oListItem = lvwEntries.Items.Add(oEntry.Key, "", "")

                    Dim subitm As New ListViewItem.ListViewSubItem
                    subitm.Text = CStr(oEntry.CompanyID)
                    If oEntry.Comment = "" Then

                        'TODO Add comment
                        oListItem.SubItems.Insert(MainModule.ListViewEntryEnum.ACLECompany, subitm)
                        ListViewHelper.SetListItemSmallIconProperty(oListItem, "")
                    Else
                        oListItem.SubItems.Insert(MainModule.ListViewEntryEnum.ACLECompany, subitm)
                        ListViewHelper.SetListItemSmallIconProperty(oListItem, "")
                    End If


                    ' Refresh fields
                    ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryEnum.ACLEPeriod).Text = oEntry.Period
                    ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryEnum.ACLEDocumentRef).Text = oTransaction.DocumentRef
                    ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryEnum.ACLEAltRef).Text = oEntry.AltRef
                    If optViewByTransaction.Checked Then

                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryEnum.ACLECurrencyAmount).Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=oEntry.CurrencyID, v_cCurrencyAmount:=oEntry.CurrencyAmount)

                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryEnum.ACLEPaidAmount).Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=oEntry.CurrencyID, v_cCurrencyAmount:=oEntry.PaidAmount)

                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryEnum.ACLENetAmount).Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=oEntry.CurrencyID, v_cCurrencyAmount:=oEntry.OutstandingAmount)
                    Else

                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryEnum.ACLECurrencyAmount).Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=oEntry.AccountCurrencyID, v_cCurrencyAmount:=oEntry.CurrencyAccountAmount)

                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryEnum.ACLEPaidAmount).Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=oEntry.AccountCurrencyID, v_cCurrencyAmount:=oEntry.PaidAccountAmount)

                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryEnum.ACLENetAmount).Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=oEntry.AccountCurrencyID, v_cCurrencyAmount:=oEntry.OutstandingAccountAmount)
                    End If

                    ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryEnum.ACLESpare).Text = oEntry.Spare
                    ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryEnum.ACLEAllocationPeriod).Text = oEntry.AllocationPeriod

                    If oEntry.Spare = "WRITEOFF" Then
                        HasWriteoff = True
                    End If

                    ' Set marked amount and icons
                    oListItem.Tag = CStr(oEntry.Key)
                    lRow += 1
                    RefreshEntry(oEntry, oListItem)
                Next oEntry

                ' Store active transaction key on lvw
                lvwEntries.Tag = oTransaction.Key
                lvwInstalmentEntries.Tag = ""
                'intermidiary Writeoff
                lvwEntries.Items(0).Selected = True
                lvwEntries.Items(0).Focused = True
                RefreshTransaction(oTransaction)

                lvwEntries.BeginUpdate()
                lvwEntries.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
                lvwEntries.EndUpdate()
            Else
                ' Process entries
                lvwInstalmentEntries.Visible = True
                lvwEntries.Visible = False
                lRow = 1
                oTransaction.Transtype = 1
                For Each oInstalmentEntry As TransactionInst In oTransaction
                    ' Start - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.4.1.3)
                    ' Add new item
                    oListItem = lvwInstalmentEntries.Items.Add(oInstalmentEntry.Key, "", "")
                    '            If (oEntry.Comment = "") Then
                    '                oListItem.ListSubItems.Add Text:=oInstalmentEntry.CompanyID, ReportIcon:=ACIconNoComment
                    '            Else
                    '                oListItem.ListSubItems.Add Text:=oInstalmentEntry.CompanyID, ReportIcon:=ACIconComment
                    '            End If
                    ' End - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.4.1.3)

                    ' Refresh fields
                    'oListItem.SubItems(ACLEPeriod) = oInstalmentEntry.Period
                    If oInstalmentEntry.InstalmentNumber <> 0 Then
                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryInstEnum.ACLIInstalmentNumber).Text = oInstalmentEntry.InstalmentNumber
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryInstEnum.ACLIInstalmentNumber).Text = ""
                    End If
                    ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryInstEnum.ACLIDocumentRef).Text = oTransaction.DocumentRef
                    If oInstalmentEntry.DueDate <> "00:00:00" Then
                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryInstEnum.ACLIDueDate).Text = oInstalmentEntry.DueDate
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryInstEnum.ACLIDueDate).Text = ""
                    End If
                    'oListItem.SubItems(ACLEAltRef) = oInstalmentEntry.AltRef
                    If optViewByTransaction.Checked = True Then
                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryInstEnum.ACLICurrencyAmount).Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=oInstalmentEntry.CurrencyID, v_cCurrencyAmount:=oInstalmentEntry.CurrencyAmount)
                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryInstEnum.ACLIPaidAmount).Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=oInstalmentEntry.CurrencyID, v_cCurrencyAmount:=oInstalmentEntry.PaidAmount)
                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryInstEnum.ACLINetAmount).Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=oInstalmentEntry.CurrencyID, v_cCurrencyAmount:=oInstalmentEntry.OutstandingAmount)
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryInstEnum.ACLICurrencyAmount).Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=oInstalmentEntry.AccountCurrencyID, v_cCurrencyAmount:=oInstalmentEntry.CurrencyAccountAmount)
                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryInstEnum.ACLIPaidAmount).Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=oInstalmentEntry.AccountCurrencyID, v_cCurrencyAmount:=oInstalmentEntry.PaidAccountAmount)
                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryInstEnum.ACLINetAmount).Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=oInstalmentEntry.AccountCurrencyID, v_cCurrencyAmount:=oInstalmentEntry.OutstandingAccountAmount)
                    End If
                    If oInstalmentEntry.Spare = "WRITEOFF" Then
                        HasWriteoff = True
                    End If

                    ' Set marked amount and icons
                    Call RefreshInstEntry(oInstalmentEntry, oListItem)
                    oListItem.Tag = lRow
                    lRow = lRow + 1
                Next oInstalmentEntry

                'Sort on Amount Column
                m_lReturn = ListView6Func.ListViewSortByValue(
                        v_oListView:=lvwInstalmentEntries,
                        v_iSourceColumn:=MainModule.ListViewEntryInstEnum.ACLIInstalmentNumber,
                        v_iDirection:=0,
                        v_bMarkSortedColumn:=True,
                        v_bIsCurrency:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", RefreshEntries, Unable to sortvalues")
                End If

                ' Store active transaction key on lvw
                lvwInstalmentEntries.Tag = oTransaction.Key
                lvwEntries.Tag = ""
                lvwInstalmentEntries.Items(0).Selected = True
                lvwInstalmentEntries.Items(0).Focused = True
                'intermidiary Writeoff
                lvwInstalmentEntries.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
                Call RefreshTransaction(oTransaction)

            End If
        Catch
        End Try

        Try
            lvwEntries.Enabled = True

            ' Refresh our buttons
            SetButtonStatus()

        Catch exc As System.Exception

        Finally
            ' Always refresh the transaction, if we have one!
            If Not (oTransaction Is Nothing) Then
                RefreshTransaction(oTransaction)
            End If
        End Try
        lvwInstalmentEntries.Enabled = True
    End Sub

    ' ***********************************************************
    ' Refresh the icons and marked amount for an entry
    ' ***********************************************************
    Private Sub RefreshEntry(ByVal oEntry As TransactionEntry, Optional ByVal oListItem As ListViewItem = Nothing)

        ' Were we passed a listitem?
        If oListItem Is Nothing Then
            ' Get the listitem (protect from errors as it may not be currently visible)


            oListItem = lvwEntries.Items.Item(oEntry.Key)
        End If

        Try

            ' Ensure we found an item
            If Not (oListItem Is Nothing) Then
                ' Set icons, parent row will be done by calling function
                Select Case oEntry.IsMarked
                    Case TransactionEntry.MarkedStatusEnum.acmseFullyMarked
                        ' Set the icon to checked

                        'Devrloper Guide No.49 (Guide)
                        oListItem.ImageKey = ACIconCheck

                    Case TransactionEntry.MarkedStatusEnum.acmsePartMarked
                        ' Set the icon to part checked

                        'Devrloper Guide No.127 (Latest Guide)
                        oListItem.ImageKey = ACIconPart

                    Case Else
                        ' No icons

                        'Devrloper Guide No.49 (Guide)
                        oListItem.ImageKey = ACIconBlank

                End Select


                ' Set new marked amount on display
                If optViewByTransaction.Checked Then

                    ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryEnum.ACLEMarkedAmount).Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=oEntry.CurrencyID, v_cCurrencyAmount:=oEntry.MarkedAmount)
                Else

                    ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryEnum.ACLEMarkedAmount).Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=oEntry.AccountCurrencyID, v_cCurrencyAmount:=oEntry.MarkedAccountAmount)
                End If
            End If

        Catch

            ' Major problem, data could be seriously out of sync
            MessageBox.Show("Unable to refresh transaction detail", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)

            ' Re-search
            PerformSearch()
        End Try




    End Sub

    ''' <summary>
    ''' Update the total marked value
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RefreshTotalMarked()

        Dim crTotalMarked As Decimal
        Dim nTotalMarked As Integer
        Dim crTotalAccountMarked As Decimal
        Dim bMultipleCurrencies As Boolean
        Dim nCurrencyID As Integer
        Dim iAccountCurrencyID As Double
        Dim oInstalmentEntry As TransactionInst
        Dim nBaseCurrencyID As Integer

        Try

            'Intialise variables
            crTotalMarked = 0
            crTotalAccountMarked = 0
            bMultipleCurrencies = False
            nCurrencyID = 0
            iAccountCurrencyID = 0

            If m_iSourceId <> 0 Then
                m_lReturn = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyId:=m_iSourceId,
                            r_iBaseCurrencyID:=nBaseCurrencyID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception("Get Base Currency failed")
                End If
            End If

            ' Process all transactions
            For Each oTransaction As Transaction In m_cTransactions

                If ValidSource(vSource:=oTransaction.CompanyID) Then

                    If Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IND" And
                Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IED" And
                Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IRD" Then
                        oTransaction.Transtype = 0
                        For Each oEntry As TransactionEntry In oTransaction
                            If oEntry.MarkedAmount <> 0 Then

                                Dim crBaseAmount As Decimal = 0
                                Dim crMarkBaseAmountUnrounded As Decimal = 0
                                'Set transaction currency and check if there are more than one currency involved.
                                If nCurrencyID = 0 Then
                                    nCurrencyID = oEntry.CurrencyID
                                ElseIf nCurrencyID <> oEntry.CurrencyID Then
                                    bMultipleCurrencies = True
                                    m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(lCurrencyID:=oEntry.CurrencyID,
                                                                                         lCompanyID:=m_iSourceId,
                                                                                         cBaseAmount:=crBaseAmount,
                                                                                         cCurrencyAmount:=oEntry.MarkedAmount,
                                                                                         vConversionDate:=DateTime.Now,
                                                                                         vBaseAmountUnRounded:=crMarkBaseAmountUnrounded)

                                    If nCurrencyID <> nBaseCurrencyID Then
                                        m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=nCurrencyID,
                                                                                             lCompanyID:=m_iSourceId,
                                                                                             cBaseAmount:=crBaseAmount,
                                                                                             cCurrencyAmount:=crMarkBaseAmountUnrounded,
                                                                                             vConversionDate:=DateTime.Now)
                                    End If
                                End If

                                If nCurrencyID <> oEntry.CurrencyID Then
                                    crTotalMarked += crMarkBaseAmountUnrounded
                                    crTotalAccountMarked += oEntry.MarkedAccountAmount
                                Else
                                    ' Add the total marked in transaction currency
                                    crTotalMarked += oEntry.MarkedAmount
                                    ' Add the total marked in account currency
                                    crTotalAccountMarked += oEntry.MarkedAccountAmount
                                End If


                                'Set account currency. Only need to do it once as it will be the same on all entries.
                                If iAccountCurrencyID = 0 Then
                                    iAccountCurrencyID = oEntry.AccountCurrencyID
                                End If

                                nTotalMarked += 1
                            End If
                        Next oEntry
                    Else
                        oTransaction.Transtype = 1
                        For Each oInstalmentEntry In oTransaction
                            If oInstalmentEntry.MarkedAmount Then

                                Dim crBaseAmount As Decimal = 0
                                Dim crMarkBaseAmountUnrounded As Decimal = 0
                                'Set transaction currency and check if there are more than one currency involved.
                                If nCurrencyID = 0 Then
                                    nCurrencyID = oInstalmentEntry.CurrencyID
                                ElseIf nCurrencyID <> oInstalmentEntry.CurrencyID Then
                                    bMultipleCurrencies = True
                                    m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(lCurrencyID:=oInstalmentEntry.CurrencyID,
                                                                                         lCompanyID:=m_iSourceId,
                                                                                         cBaseAmount:=crBaseAmount,
                                                                                         cCurrencyAmount:=oInstalmentEntry.MarkedAmount,
                                                                                         vConversionDate:=DateTime.Now,
                                                                                         vBaseAmountUnRounded:=crMarkBaseAmountUnrounded)

                                    If nCurrencyID <> nBaseCurrencyID Then
                                        m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=nCurrencyID,
                                                                                             lCompanyID:=m_iSourceId,
                                                                                             cBaseAmount:=crBaseAmount,
                                                                                             cCurrencyAmount:=crMarkBaseAmountUnrounded,
                                                                                             vConversionDate:=DateTime.Now)
                                    End If

                                End If

                                If nCurrencyID <> oInstalmentEntry.CurrencyID Then
                                    crTotalMarked += crMarkBaseAmountUnrounded
                                    crTotalAccountMarked += oInstalmentEntry.MarkedAccountAmount
                                Else
                                    ' Add the total marked in transaction currency
                                    crTotalMarked += oInstalmentEntry.MarkedAmount
                                    ' Add the total marked in account currency
                                    crTotalAccountMarked += oInstalmentEntry.MarkedAccountAmount
                                End If


                                'Set account currency. Only need to do it once as it will be the same on all entries.
                                If iAccountCurrencyID = 0 Then
                                    iAccountCurrencyID = oInstalmentEntry.AccountCurrencyID
                                End If

                                nTotalMarked = nTotalMarked + 1
                            End If
                        Next oInstalmentEntry
                    End If
                End If
            Next oTransaction

            'If we have unmarked everything reset the active currency
            If nTotalMarked = 0 Then
                'Show total without symbol
                txtMarked.Text = "0.00"
                If Not m_bReciptAmountEntered Then
                    txtReciptPaymentAmount.Text = ""
                End If
                UctCurrency.Text = "(None)"

            Else
                'Show total with symbol
                If optViewByTransaction.Checked And Not bMultipleCurrencies Then

                    txtMarked.Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=nCurrencyID, v_cCurrencyAmount:=crTotalMarked)
                    If Not m_bReciptAmountEntered Then
                        If crTotalMarked > 0 Then
                            txtReciptPaymentAmount.Text = g_oBusiness.FormatCurrency(
                                        v_lCurrencyID:=nCurrencyID,
                                        v_cCurrencyAmount:=crTotalMarked)
                        Else
                            txtReciptPaymentAmount.Text = ""
                        End If
                    End If
                Else

                    txtMarked.Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=iAccountCurrencyID, v_cCurrencyAmount:=crTotalAccountMarked)
                    If Not m_bReciptAmountEntered Then
                        If crTotalAccountMarked > 0 Then
                            txtReciptPaymentAmount.Text = g_oBusiness.FormatCurrency(
                                        v_lCurrencyID:=iAccountCurrencyID,
                                        v_cCurrencyAmount:=crTotalAccountMarked)
                        Else
                            txtReciptPaymentAmount.Text = ""
                        End If
                    End If
                End If

                If optViewByTransaction.Checked = True Then
                    If bMultipleCurrencies Then
                        '' UctCurrency.Text = "(None)"
                        UctCurrency.CurrencyId = m_iCurrencyId
                        m_iMarkedcurrencyId = m_iCurrencyId
                    Else
                        ''UctCurrency.Text = "(None)"
                        UctCurrency.CurrencyId = nCurrencyID
                        m_iMarkedcurrencyId = nCurrencyID
                    End If

                ElseIf optViewByAccount.Checked = True Then
                    ''UctCurrency.Text = "(None)"
                    UctCurrency.CurrencyId = m_iCurrencyId
                    m_iMarkedcurrencyId = m_iCurrencyId
                End If


                If crTotalMarked > 0 Or txtReciptPaymentAmount.Text <> "" Then
                    m_bReciptOrPay = True
                    cmdPay.Text = "Receipt"
                Else
                    m_bReciptOrPay = False
                    cmdPay.Text = "Pay"
                End If

                m_bMarkedTransaction = True
            End If

        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=excep.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshTotalMarked", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            txtMarked.Text = "<Error>"
        End Try
    End Sub

    ''' <summary>
    ''' Refresh the icons and Writeoff amount for a transaction
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RefreshTotalWriteOff()
        Dim crTotalMarked As Decimal
        Dim nTotalMarked As Integer
        Dim crTotalAccountMarked As Decimal
        Dim bMultipleCurrencies As Boolean
        Dim nCurrencyID As Integer
        Dim crAccountCurrencyID As Double
        Dim oInstalmentEntry As TransactionInst
        Dim nBaseCurrencyID As Integer

        Try

            'Intialise variables
            crTotalMarked = 0
            crTotalAccountMarked = 0
            bMultipleCurrencies = False
            nCurrencyID = 0
            crAccountCurrencyID = 0

            If m_iSourceId <> 0 Then
                m_lReturn = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyId:=m_iSourceId,
                            r_iBaseCurrencyID:=nBaseCurrencyID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception("Get Base Currency failed")
                End If
            End If

            ' Process all transactions
            For Each oTransaction As Transaction In m_cTransactions

                If ValidSource(vSource:=oTransaction.CompanyID) Then
                    If Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IND" And
                        Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IED" And
                        Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IRD" Then
                        oTransaction.Transtype = 0
                        For Each oEntry As TransactionEntry In oTransaction
                            If oEntry.MarkedAmount <> 0 And oEntry.Spare = "WRITEOFF" Then

                                Dim crBaseAmount As Decimal = 0
                                Dim crMarkBaseAmountUnrounded As Decimal = 0
                                'Set transaction currency and check if there are more than one currency involved.
                                If nCurrencyID = 0 Then
                                    nCurrencyID = oEntry.CurrencyID
                                ElseIf nCurrencyID <> oEntry.CurrencyID Then
                                    bMultipleCurrencies = True
                                    m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(lCurrencyID:=oEntry.CurrencyID,
                                                                                         lCompanyID:=m_iSourceId,
                                                                                         cBaseAmount:=crBaseAmount,
                                                                                         cCurrencyAmount:=oEntry.MarkedAmount,
                                                                                         vConversionDate:=DateTime.Now,
                                                                                         vBaseAmountUnRounded:=crMarkBaseAmountUnrounded)


                                    If nCurrencyID <> nBaseCurrencyID Then
                                        m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=nCurrencyID,
                                                                                             lCompanyID:=m_iSourceId,
                                                                                             cBaseAmount:=crBaseAmount,
                                                                                             cCurrencyAmount:=crMarkBaseAmountUnrounded,
                                                                                             vConversionDate:=DateTime.Now)
                                    End If

                                End If

                                If nCurrencyID <> oEntry.CurrencyID Then
                                    crTotalMarked += crMarkBaseAmountUnrounded
                                    crTotalAccountMarked += oEntry.MarkedAccountAmount
                                Else
                                    ' Add the total marked in transaction currency
                                    crTotalMarked += oEntry.MarkedAmount
                                    ' Add the total marked in account currency
                                    crTotalAccountMarked += oEntry.MarkedAccountAmount
                                End If

                                'Set account currency. Only need to do it once as it will be the same on all entries.
                                If crAccountCurrencyID = 0 Then
                                    crAccountCurrencyID = oEntry.AccountCurrencyID
                                End If

                                nTotalMarked += 1
                            End If
                        Next oEntry
                    Else
                        oTransaction.Transtype = 1
                        For Each oInstalmentEntry In oTransaction
                            If oInstalmentEntry.MarkedAmount <> 0 And oInstalmentEntry.Spare = "WRITEOFF" Then

                                Dim crBaseAmount As Decimal = 0
                                Dim crMarkBaseAmountUnrounded As Decimal = 0
                                'Set transaction currency and check if there are more than one currency involved.
                                If nCurrencyID = 0 Then
                                    nCurrencyID = oInstalmentEntry.CurrencyID
                                ElseIf nCurrencyID <> oInstalmentEntry.CurrencyID Then
                                    bMultipleCurrencies = True
                                    m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(lCurrencyID:=oInstalmentEntry.CurrencyID,
                                                                                         lCompanyID:=m_iSourceId,
                                                                                         cBaseAmount:=crBaseAmount,
                                                                                         cCurrencyAmount:=oInstalmentEntry.MarkedAmount,
                                                                                         vConversionDate:=DateTime.Now,
                                                                                         vBaseAmountUnRounded:=crMarkBaseAmountUnrounded)

                                    If nCurrencyID <> nBaseCurrencyID Then
                                        m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=nCurrencyID,
                                                                                             lCompanyID:=m_iSourceId,
                                                                                             cBaseAmount:=crBaseAmount,
                                                                                             cCurrencyAmount:=crMarkBaseAmountUnrounded,
                                                                                             vConversionDate:=DateTime.Now)
                                    End If

                                End If

                                If nCurrencyID <> oInstalmentEntry.CurrencyID Then
                                    crTotalMarked += crMarkBaseAmountUnrounded
                                    crTotalAccountMarked += oInstalmentEntry.MarkedAccountAmount
                                Else
                                    ' Add the total marked in transaction currency
                                    crTotalMarked = crTotalMarked + oInstalmentEntry.MarkedAmount
                                    ' Add the total marked in account currency
                                    crTotalAccountMarked = crTotalAccountMarked + oInstalmentEntry.MarkedAccountAmount
                                End If



                                'Set account currency. Only need to do it once as it will be the same on all entries.
                                If crAccountCurrencyID = 0 Then
                                    crAccountCurrencyID = oInstalmentEntry.AccountCurrencyID
                                End If

                                nTotalMarked = nTotalMarked + 1
                            End If
                        Next oInstalmentEntry
                    End If
                End If
            Next oTransaction

            'If we have unmarked everything reset the active currency
            If nTotalMarked = 0 Then
                'Show total without symbol
                TxttotalWriteoff.Text = "0.00"
            Else
                'Show total with symbol
                If optViewByTransaction.Checked And Not bMultipleCurrencies Then
                    If crTotalMarked <> 0 Then
                        TxttotalWriteoff.Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=nCurrencyID, v_cCurrencyAmount:=crTotalMarked)
                    Else
                        TxttotalWriteoff.Text = "0.00"
                    End If
                Else
                    If crTotalMarked <> 0 Then
                        TxttotalWriteoff.Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=crAccountCurrencyID, v_cCurrencyAmount:=crTotalAccountMarked)
                    Else
                        TxttotalWriteoff.Text = "0.00"
                    End If
                End If
            End If

        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=excep.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshTotalMarked", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            txtMarked.Text = "<Error>"

        End Try

    End Sub

    ''' <summary>
    ''' Refresh the icons and marked amount for a transaction
    ''' </summary>
    ''' <param name="oTransaction"></param>
    ''' <param name="oListItem"></param>
    ''' <remarks></remarks>
    Private Sub RefreshTransaction(ByVal oTransaction As Transaction,
                                   Optional ByVal oListItem As ListViewItem = Nothing)

        Try

            ' Were we passed a listitem?
            If oListItem Is Nothing Then
                ' Get the listitem (protect from errors as it may not be currently visible)
                oListItem = lvwTransactions.Items.Item(oTransaction.Key)
                m_sTransKey = oTransaction.Key
            End If
            If Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IND" And
            Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IED" And
            Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IRD" Then
                ' Set icons, parent row will be done by calling function
                Select Case oTransaction.IsMarked
                    Case TransactionEntry.MarkedStatusEnum.acmseFullyMarked
                        ' Set the icon to checked
                        oListItem.ImageKey = ACIconCheck
                        cmdwriteoff.Enabled = Not HasWriteoff And m_sWriteOffAccountCode.Trim() <> ""
                        m_cTransOutstandingAmt = oTransaction.TotalOutstanding
                    Case TransactionEntry.MarkedStatusEnum.acmsePartMarked
                        ' Set the icon to part checked
                        oListItem.ImageKey = ACIconPart
                        cmdwriteoff.Enabled = False
                    Case Else
                        ' No icons
                        oListItem.ImageKey = ACIconBlank
                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewTransactionEnum.ACLTMarkedTotal + 1).Text =
                            g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.CurrencyID, v_cCurrencyAmount:=0)
                        cmdwriteoff.Enabled = False
                End Select

                ' Set new marked amount on display
                If optViewByTransaction.Checked Then
                    ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewTransactionEnum.ACLTMarkedTotal + 1).Text =
                        g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.CurrencyID, v_cCurrencyAmount:=oTransaction.TotalMarked)
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewTransactionEnum.ACLTMarkedTotal + 1).Text =
                        g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.AccountCurrencyID, v_cCurrencyAmount:=oTransaction.TotalAccountMarked)
                End If
            Else
                Select Case oTransaction.IsMarkedInstalment
                    Case TransactionEntry.MarkedStatusEnum.acmseFullyMarked
                        ' Set the icon to checked
                        oListItem.ImageKey = ACIconCheck
                        oListItem.ImageKey = ACIconCheck
                        If HasWriteoff = False And Trim$(m_sWriteOffAccountCode) <> "" Then
                            cmdwriteoff.Enabled = True
                        Else
                            cmdwriteoff.Enabled = False
                        End If
                        m_cTransOutstandingAmt = oTransaction.TotalOutstanding
                    Case TransactionEntry.MarkedStatusEnum.acmsePartMarked
                        ' Set the icon to part checked
                        oListItem.ImageKey = ACIconPart
                        oListItem.ImageKey = ACIconPart
                        cmdwriteoff.Enabled = False
                    Case Else
                        ' No icons
                        oListItem.ImageKey = ACIconBlank
                        oListItem.ImageKey = ACIconBlank
                        cmdwriteoff.Enabled = False
                End Select

                ' Set new marked amount on display
                If optViewByTransaction.Checked = True Then
                    ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewTransactionEnum.ACLTMarkedTotal + 1).Text =
                        g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.CurrencyID, v_cCurrencyAmount:=oTransaction.TotalMarked)
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewTransactionEnum.ACLTMarkedTotal + 1).Text =
                        g_oBusiness.FormatCurrency(v_lCurrencyID:=oTransaction.AccountCurrencyID, v_cCurrencyAmount:=oTransaction.TotalAccountMarked)
                End If

            End If
        Catch
            ' Major problem, data could be seriously out of sync
            MessageBox.Show("Unable to refresh transaction", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            ' Re-search
            PerformSearch()
            ' Just in case it's needed (Note: it IS needed)
            m_lReturn = gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Sub

    Private Function SetButtonStatus() As Object


        ' Set transaction buttons
        cmdMarkAll.Enabled = (lvwTransactions.Items.Count > 0)
        cmdUnMarkAll.Enabled = (lvwTransactions.Items.Count > 0)
        cmdMarkTrans.Enabled = (lvwTransactions.Items.Count > 0)
        cmdDrill.Enabled = (lvwTransactions.Items.Count > 0)

        ' Set entry buttons
        If Mid(Trim$(m_sDocRef), 1, 3) <> "IND" And
            Mid(Trim$(m_sDocRef), 1, 3) <> "IED" And
            Mid(Trim$(m_sDocRef), 1, 3) <> "IRD" Then
            cmdMarkEntries.Enabled = Not (lvwEntries.FocusedItem Is Nothing)
            cmdPartPay.Enabled = Not (lvwEntries.FocusedItem Is Nothing)
            'cmdwriteoff.Enabled = Not (lvwEntries.SelectedItem Is Nothing)
        Else
            cmdMarkEntries.Enabled = Not (lvwInstalmentEntries.FocusedItem Is Nothing)
            cmdPartPay.Enabled = Not (lvwInstalmentEntries.FocusedItem Is Nothing)
            'cmdwriteoff.Enabled = Not (lvwInstalmentEntries.SelectedItem Is Nothing)
        End If
        ' Set pay / binder buttons
        cmdBinder.Enabled = (lvwTransactions.Items.Count > 0) AndAlso (m_sLedgerCode = ACInsurerLedger)

        '(RC) PN 37251 - Don't Enable Pay button if its already clicked (untill the process completes)
        If Not bPayButtonClicked Then
            cmdPay.Enabled = (lvwTransactions.Items.Count > 0) And ConvertCurrencyStringToValue(txtMarked.Text) <> 0
        End If

        If Not bAllocateButtonClicked Then
            cmdPay.Enabled = (lvwTransactions.Items.Count > 0) And ConvertCurrencyStringToValue(txtMarked.Text) <> 0
        End If
    End Function

    ''' <summary>
    ''' Sets all of the interface default values.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetInterfaceDefaults() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sOptionValue As String = ""

        Try
            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() +
                                           ", SetInterfaceDefaults, Unable to set captions")
            End If

            ' Set defaults for search criteria
            cboMarkedStatus.SelectedIndex = 0
            cboMonth.SelectedIndex = 0

            'PS RSA Curacao - Insurer Payment transaction type selection - Upendra
            cboTransType.SelectedIndex = 0
            optEffectiveDate.Checked = True

            ' Default the date (first to today as default, and then to null to disable)
            txtDueDateTo.Value = DateTime.Today

            ' Disable payment group (until an account has been selected)
            cboPaymentGroup.Enabled = False
            lvwEntries.Visible = True
            lvwInstalmentEntries.Visible = False
            ' Intermidiary WriteOff
            m_lReturn = iPMFunc.GetSystemOption(5028, sOptionValue)
            m_sWriteOffAccountCode = sOptionValue
            If m_lReturn <> PMEReturnCode.PMTrue Then

                nResult = PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                   sMsg:="Failed to initialise the system option object",
                                   vApp:=ACApp, vClass:=ACClass,
                                   vMethod:="SetInterfaceDefaults",
                                   vErrNo:=Information.Err().Number,
                                   vErrDesc:=Information.Err().Description)
                Return nResult
            ElseIf m_sView IsNot Nothing AndAlso m_sView <> "1" Then

                If Convert.IsDBNull(m_sWriteOffAccountCode) OrElse
                    IsNothing(m_sWriteOffAccountCode) OrElse
                    m_sWriteOffAccountCode.Trim() = "" Then
                    cmdwriteoff.Enabled = False
                End If
            End If
            LoadAllocationPeriod()
            Return nResult

        Catch excep As System.Exception
            nResult = IIf(Information.Err().Number = Constants.vbObjectError,
                          PMEReturnCode.PMFalse,
                          PMEReturnCode.PMError)
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:=excep.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ' ***********************************************************
    ' Set the resizing anchors
    ' ***********************************************************
    Private Sub SetResize()
        Dim AnchorBottom As Object
        Dim AnchorLeft As Object
        Try

            ' Set start dimensions
            m_lWidth = CInt(VB6.PixelsToTwipsX(ClientRectangle.Width))
            m_lHeight = CInt(VB6.PixelsToTwipsY(ClientRectangle.Height))

            ' Search Block
            uctAnchor.Add(divSearch, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
            uctAnchor.Add(cmdFindNow, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
            uctAnchor.Add(cmdNewSearch, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)

            ' Report Block
            uctAnchor.Add(cmdReport, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)

            ' Transaction Block
            uctAnchor.Add(divTransactions, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
            uctAnchor.Add(lvwTransactions, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
            uctAnchor.Add(cmdMarkAll, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
            uctAnchor.Add(cmdUnMarkAll, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
            uctAnchor.Add(cmdMarkTrans, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
            uctAnchor.Add(cmdDrill, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)

            ' Transaction Detail Block
            uctAnchor.Add(divTransDetails, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
            uctAnchor.Add(lvwEntries, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
            uctAnchor.Add(lvwInstalmentEntries, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
            uctAnchor.Add(cmdMarkEntries, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
            uctAnchor.Add(cmdPartPay, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
            uctAnchor.Add(cmdwriteoff, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)

            ' Control Buttons
            uctAnchor.Add(cmdBinder, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
            uctAnchor.Add(cmdPay, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
            uctAnchor.Add(cmdAllocate, AnchorLeft + AnchorBottom)
            uctAnchor.Add(cmdOK, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
            uctAnchor.Add(cmdCancel, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
            uctAnchor.Add(cmdHelp, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
            uctAnchor.Add(cmdAllocate, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)

        Catch
        End Try




    End Sub

    ' Terminate a Sirius component
    Private Sub TerminateObject(ByRef v_oObject As Object)

        Try

            ' Check for creation
            If Not (v_oObject Is Nothing) Then
                ' Call terminate

                v_oObject.Dispose()

                ' Set nothing
                v_oObject = Nothing
            End If

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate object", vApp:=ACApp, vClass:=ACClass, vMethod:="TerminateObject", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


        End Try

    End Sub

    ' ***********************************************************
    ' Unlock the insurers account
    ' ***********************************************************
    Private Function UnlockInsurer(ByVal v_lAccountID As Integer) As Integer
        Dim result As Integer = 0

        Dim oPMLock As bPMLock.User

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Get an instance of the lock object
            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", UnlockInsurer, Unable to get instance of bPMLock")
            End If

            ' Unlock the current insurer

            m_lReturn = oPMLock.UnLockKey(sKeyName:="Insurer_Payment", vKeyValue:=v_lAccountID, iUserID:=g_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", UnlockInsurer, " + CStr(CDbl("Failed to unlock insurer. Account id: ") + v_lAccountID))
            End If

            Return result

        Catch excep As System.Exception

            ' Set return
            result = IIf(Information.Err().Number = Constants.vbObjectError, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMError)

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=excep.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockInsurer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result
        End Try
    End Function


    Private Function ValidSource(ByVal vSource As Object) As Boolean
        Dim result As Boolean = False


        'tot210801

        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vSource), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return result
        End If

        If IsArray(m_vPaySources) Then
            For i As Integer = 0 To m_vPaySources.GetUpperBound(0)

                If CInt(m_vPaySources(i)) = CInt(vSource) Then
                    result = True
                    Exit For
                End If
            Next i
        End If
        Return result
    End Function
    ' ***********************************************************
    ' Validate WriteOff
    ' ***********************************************************
    Private Function ValidateWriteOff() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim IsAccountCode As Boolean = False

        'Validate WriteOff AccountCode
        If m_sWriteOffAccountCode.Trim() <> "" Then

            m_lReturn = m_oAccount.IsValidAccountCode(m_sWriteOffAccountCode, IsAccountCode, m_lWriteOffAcc_id)

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Or Not IsAccountCode Then
                MessageBox.Show("The Intermediary Write Off account code " & m_sWriteOffAccountCode & " does not exist.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If
        Else
            MessageBox.Show("The Intermediary Write Off account code is not selected.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return result
        End If

        'Validate WriteOff Permission

        m_lReturn = m_oUserAuthority.GetUserWriteOffDetails(g_iUserID, vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        Else
            If Information.IsArray(vResultArray) Then
                m_bHasWriteOffAuthority = gPMFunctions.ToSafeBoolean(vResultArray(1, 0))
                m_lWriteOffAmount = gPMFunctions.ToSafeCurrency(vResultArray(2, 0))
            Else
                MessageBox.Show("User doesnot have Write Off Authority.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If
        End If

        Return result
    End Function
    Private Sub cmdAllocate_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAllocate.Click
        Dim iMsgResult As Integer

        iMsgResult = MsgBox("Are you sure you want to allocate these transactions together?",
                vbYesNo + vbDefaultButton2 + vbQuestion, Me.Text)

        If iMsgResult = vbYes Then
            m_lReturn = Allocate()
            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Success
                    Me.Focus()
                    MessageBox.Show("The Allocation process is completed", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    m_bReciptAmountEntered = False
                    txtReciptPaymentAmount.Text = ""
                    ''--Call Remitance Advice Report if payment succeeded
                    ''ShowRemitanceAdviceReport()

                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' No records found for operation this is
                    MessageBox.Show("Nothing has been selected for payment", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                Case gPMConstants.PMEReturnCode.PMCancel
                    ' The total amount marked was zero, unable to binder
                    MessageBox.Show("Posting aborted", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Case Else
                    ' Error
                    MessageBox.Show("Roadmap failed, please check insurer payment in account enquiry.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Select
            Call ShowRemitanceAdviceReport()

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                PerformSearch()
                m_bMarkedTransaction = False
                EnableDisableAllocateButton()
            End If
        End If
        txtunallocatedAmount.Text = "0.00"
        txtMarked.Text = "0.00"

    End Sub
    ' *******************************************************************************
    ' PRIVATE METHODS
    ' *******************************************************************************



    ' *******************************************************************************
    ' FORM EVENTS
    ' *******************************************************************************
    ' ***********************************************************
    ' Creates an array of Branches(sources) for which payment may
    ' be made. Validates the Payment group against users access
    ' ***********************************************************
    Private Sub cboPaymentGroup_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPaymentGroup.Leave

        Dim lSources As Integer
        Dim vSources As Object
        Dim lSourceID As Integer
        Dim bAccessOK As Boolean

        Try

            ' Check for no selection
            If cboPaymentGroup.SelectedIndex = -1 Then
                m_vPaySources = VB6.CopyArray(Nothing)
                Exit Sub
            End If

            ' Create base array
            ReDim vSources(0)

            If m_bFromGroups Then
                ' Set Up Payment Sources from Payment table and validate against User Access
                lSourceID = VB6.GetItemData(cboPaymentGroup, cboPaymentGroup.SelectedIndex)

                ' Find all sources
                For lCount As Integer = m_vPaymentGroups.GetLowerBound(1) To m_vPaymentGroups.GetUpperBound(1)
                    If CDbl(m_vPaymentGroups(2, lCount)) = lSourceID Then
                        ' Increase array and add value
                        ReDim Preserve vSources(lSources)

                        vSources(lSources) = m_vPaymentGroups(3, lCount)
                        lSources += 1
                    End If
                Next lCount

                ' Validate sources

                For lCount As Integer = vSources.GetLowerBound(0) To vSources.GetUpperBound(0)
                    bAccessOK = False
                    ' Check for match
                    For lCount2 As Integer = 1 To m_vSourceArray.GetUpperBound(1) + 1

                        If CDbl(m_vSourceArray(0, lCount2 - 1)) = CInt(vSources(lCount)) Then
                            bAccessOK = True
                            Exit For
                        End If
                    Next lCount2

                    If Not bAccessOK Then
                        ' No access, warn the user
                        MessageBox.Show("You Do not have access to All the Companies in this Group - Please Reselect", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                        ' Clear down source array

                        vSources = Nothing
                        lSources = 0
                        Exit For
                    End If
                Next lCount
            Else
                ' Set Up Payment Sources from Source Table
                If cboPaymentGroup.Text <> "All Branches" Then
                    ' Single Source

                    vSources(lSources) = VB6.GetItemData(cboPaymentGroup, cboPaymentGroup.SelectedIndex)
                    lSources += 1
                Else
                    ' All sources
                    For lCount As Integer = m_vSourceArray.GetLowerBound(1) + 1 To m_vSourceArray.GetUpperBound(1) + 1
                        ' Increase array and add source
                        ReDim Preserve vSources(lSources)

                        vSources(lSources) = m_vSourceArray(0, lCount - 1)
                        lSources += 1
                    Next lCount
                End If
            End If

            ' Set modular array of sources

            m_vPaySources = vSources

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=excep.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="cboPaymentGroup_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)



        End Try

    End Sub

    Private Sub cboTransType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTransType.SelectedIndexChanged

        '**************************************************************
        ' PURPOSE :  'PS RSA Curacao - Insurer Payment transaction type selection
        ' Add By  :  Upendra
        ' Created :  11-Jan-2010
        '**************************************************************

        If txtAccountCode.Text <> "" Then
            ClearInterface(True)
            m_lReturn = DataToInterface()
        End If
    End Sub


    '    Private Sub cmdAllocate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAllocate.Click

    '        Dim bMarked As Boolean

    '        m_lStatus = gPMConstants.PMEReturnCode.PMOK

    '        On Error GoTo Catch_Renamed



    '        bMarked = False

    '        For iLoop As Integer = 1 To m_cTransactions.Count

    '            If m_cTransactions(iLoop).IsMarked = TransactionEntry.MarkedStatusEnum.acmsePartMarked Or m_cTransactions(iLoop).IsMarked = TransactionEntry.MarkedStatusEnum.acmseFullyMarked Then
    '                bMarked = True
    '                Exit For
    '            End If

    '        Next

    '        If gPMFunctions.ToSafeCurrency(m_cTotalMarked) = 0 And bMarked Then

    '            If MessageBox.Show("Are you sure you want go ahead with the allocation?", "Perform Allocation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.No Then Exit Sub

    '            If Not bAllocateButtonClicked Then
    '                cmdAllocate.Enabled = False
    '                bAllocateButtonClicked = True
    '            End If

    '            ' Process the binder
    '            m_lReturn = ProcessPay(bIsAllocate:=True)

    '            ' Check return and inform the user
    '            Select Case m_lReturn
    '                Case gPMConstants.PMEReturnCode.PMTrue
    '                    ' Success
    '                    MessageBox.Show("Payment posted succesfully", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '                    '--Call Remitance Advice Report if payment succeeded
    '                    ShowRemitanceAdviceReport()
    '                    '
    '                    '        Case PMNotFound
    '                    '            ' No records found for operation this is
    '                    '            Call MsgBox("Nothing has been selected for payment", vbInformation, Caption)
    '                    '            GoTo Finally
    '                    '
    '                    '        Case PMCancel
    '                    '            ' The total amount marked was zero, unable to binder
    '                    '            Call MsgBox("Posting aborted", vbExclamation, Caption)

    '                Case Else
    '                    ' Error
    '                    MessageBox.Show("Allocation Failed, please check insurer payment in account enquiry.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            End Select

    '            ' Refresh the list on successful or error binder
    '            PerformSearch()
    '        Else
    '            MessageBox.Show("The Document must balance to Zero!", "Invalid Document Balance", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        End If

    '        GoTo Finally_Renamed
    'Catch_Renamed:
    '        ' Log Error Message
    '        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAllocate_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

    'Finally_Renamed:

    '        cmdAllocate.Enabled = True
    '        bAllocateButtonClicked = False

    '    End Sub

    Private Sub cmdBinder_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBinder.Click
        Dim sMessage As String = ""
        Dim iMsgResult As DialogResult

        m_lStatus = gPMConstants.PMEReturnCode.PMOK
        Try


            '--------------------
            If m_lIsReportGenerated = gPMConstants.PMEReturnCode.PMFalse Then
                sMessage = "Marked Items Report has not been produced. Do you wish to continue Y/N?"
                iMsgResult = MessageBox.Show(sMessage, "Confirm Binding", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            End If
            '--------------------


            ' Process the binder
            '--------------------
            If iMsgResult = System.Windows.Forms.DialogResult.Yes Then
                m_lReturn = ProcessBinder()
            ElseIf iMsgResult = System.Windows.Forms.DialogResult.No Then
                m_lReturn = gPMConstants.PMEReturnCode.PMCancel
            End If
            '--------------------


            ' Refresh the list on successful or error binder
            If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                PerformSearch()
            End If

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=excep.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdBinder_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)



        End Try

    End Sub

    Private Sub cmdReport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdReport.Click
        m_lReturn = ReportMarkedItems()
        m_lIsReportGenerated = gPMConstants.PMEReturnCode.PMTrue
    End Sub

    '----------------------------------
    'Purpose - Design to produce an (Insurer Payment Marked Items Report)
    '----------------------------------
    Private Function ReportMarkedItems() As Integer
        Dim result As Integer = 0
        Dim oReport As iPMBReportPrint.Interface_Renamed
        Dim vKeyArray(1, 11) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get Instance of Reportprint component
            Dim temp_oReport As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oReport, sClassName:="iPMBReportPrint.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oReport = temp_oReport

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create 'iPMBReportPrint.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="ReportMarkedItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                If Information.IsReference(oReport) Then

                    oReport.Dispose()
                    oReport = Nothing
                End If
                Return result
            End If

            With oReport

                ' Send Report & Parameters into Report via SetKeys()

                vKeyArray(0, 0) = PMNavKeyConst.PMKeyNameReportName '"report_name"

                vKeyArray(1, 0) = "Insurer_Payment_Marked_Items"


                vKeyArray(0, 1) = PMNavKeyConst.PMKeyNamePrintReport '"report_print_options"

                vKeyArray(1, 1) = PMNavKeyConst.AC_PRINT_AND_VIEW

                vKeyArray(1, 1) = PMNavKeyConst.AC_VIEW_ONLY


                'Submit (Account Id) to generate raport.

                vKeyArray(0, 2) = PMNavKeyConst.PMKeyNameParam1Name '"param_name1"

                vKeyArray(1, 2) = "account_id"


                vKeyArray(0, 3) = "account_id"

                vKeyArray(1, 3) = CStr(m_lAccountId).Trim()
                '

                'Submit (Date to) Parameter to generate report.

                vKeyArray(0, 4) = PMNavKeyConst.PMKeyNameParam2Name '"param_name2"

                vKeyArray(1, 4) = "date_to"


                vKeyArray(0, 5) = "date_to"

                If Not (Convert.IsDBNull(txtDueDateTo.Value) Or IsNothing(txtDueDateTo.Value)) Then

                    vKeyArray(1, 5) = txtDueDateTo.Value.ToString.Trim()
                ElseIf Convert.IsDBNull(txtDueDateTo.Value) Or IsNothing(txtDueDateTo.Value) Then

                    vKeyArray(1, 5) = DateTime.Today
                End If
                '
                'Submit  (date_by_trans) Parameter to generate report.

                vKeyArray(0, 6) = PMNavKeyConst.PMKeyNameParam3Name '"param_name3"

                vKeyArray(1, 6) = "date_by_trans"


                vKeyArray(0, 7) = "date_by_trans"
                If optEffectiveDate.Checked Then
                    vKeyArray(1, 7) = 0
                ElseIf optTransDate.Checked Then
                    vKeyArray(1, 7) = 1
                ElseIf optDueDate.Checked Then
                    vKeyArray(1, 7) = 2
                End If



                'Submit  (marked_status) Parameter to generate report.

                vKeyArray(0, 8) = PMNavKeyConst.PMKeyNameParam4Name '"param_name4"

                vKeyArray(1, 8) = "marked_status"


                vKeyArray(0, 9) = "marked_status"

                vKeyArray(1, 9) = cboMarkedStatus.SelectedIndex - 1

                'Submit  (month) Parameter to generate report.

                vKeyArray(0, 10) = PMNavKeyConst.PMKeyNameParam5Name '"param_name5"

                vKeyArray(1, 10) = "month"


                vKeyArray(0, 11) = "month"

                vKeyArray(1, 11) = cboMonth.SelectedIndex



                m_lReturn = .SetKeys(vKeyArray:=vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Set Keys for Report.", vApp:=ACApp, vClass:=ACClass, vMethod:="ReportMarkedItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)


                    .Dispose()
                    oReport = Nothing
                    Return result
                End If


                ' Generate Report

                m_lReturn = .Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Generate the Report.", vApp:=ACApp, vClass:=ACClass, vMethod:="ReportMarkedItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)


                    .Dispose()
                    oReport = Nothing
                    Return result
                End If

                ' Close Report Component

                .Dispose()
                oReport = Nothing

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReportMarkedItems Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReportMarkedItems", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Set the interface status.
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        Try

            ' If nothing showing, hide instantly
            If lvwTransactions.Items.Count Then
                ' Process the next set of actions (i.e. Ask the user)
                m_lReturn = m_oGeneral.ProcessCommand()
            Else
                m_lReturn = gPMConstants.PMEReturnCode.PMTrue
            End If

            ' Have we decided to close?
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Unlock the insurer, if we have one
                If m_lAccountId <> 0 Then
                    m_lReturn = UnlockInsurer(m_lAccountId)
                End If

                ' Hide form to trigger shutdown
                Me.Hide()
            End If

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub cmdDrill_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDrill.Click
        ' Drill the document using FindTransaction
        m_lReturn = DrillDocument()
    End Sub

    Private Sub cmdFindAccount_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindAccount.Click

        Dim lAccountID As Integer

        'PS RSA Curacao - Insurer Payment transaction type selection - Upendra
        'Preventing the display data to another Agent.
        m_cTransactions = Nothing

        ' Perform search
        m_lReturn = FindAccount(r_lAccountId:=lAccountID)

        ' Load the new account
        m_lReturn = LoadAccount(lAccountID)

    End Sub

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click
        cmdFindNow.Enabled = False
        ' Deletegate
        m_lReturn = PerformSearch()

        cmdFindNow.Enabled = True

        EnableDisableAllocateButton()
    End Sub

    Private Sub cmdMarkAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMarkAll.Click

        If Not (m_cTransactions Is Nothing) Then
            ' This can take a (very) long time, confirm
            If lvwTransactions.Items.Count > 10 Then
                If MessageBox.Show("Marking all items may take some time." & Strings.Chr(13) & Strings.Chr(10) &
                                   "Are you sure you want to mark these " & CStr(lvwTransactions.Items.Count) & " items?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If
            End If

            ' Delegate
            m_lReturn = MarkAll()
            ' If this failed warn the user
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("An error occured marking all transactions", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            EnableDisableAllocateButton()
        End If
    End Sub

    Private Sub cmdMarkEntries_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMarkEntries.Click
        ' Process all marked entries
        If Mid(Trim$(m_sDocRef), 1, 3) <> "IND" And
                 Mid(Trim$(m_sDocRef), 1, 3) <> "IED" And
                 Mid(Trim$(m_sDocRef), 1, 3) <> "IRD" Then
            m_lReturn = MarkEntries()
        Else
            m_lReturn = MarkInstEntries()
        End If
        ' If this failed warn the user
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            Call MsgBox("An error occured marking selected transaction entries", vbCritical, Me.Text)
        End If
        EnableDisableAllocateButton()
    End Sub

    Private Sub cmdMarkTrans_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMarkTrans.Click
        ' Mark the select transactions
        m_lReturn = MarkTransactions()

        ' If this failed warn the user
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("An error occured marking selected transactions", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        EnableDisableAllocateButton()
        If lvwTransactions.Items.Count > 0 Then
            RefreshEntries(lvwTransactions.SelectedItems(0))
        End If
    End Sub

    Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click


        ' Check if the user still wishes to clear the interface.
        Dim lResult As DialogResult = MessageBox.Show(GetCaption(ACClearDetails), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        ' Check message result.
        If lResult = System.Windows.Forms.DialogResult.Yes Then
            ' Clear the interface details.
            m_lReturn = ClearInterface()
            m_bReciptOrPay = False
            m_bReciptAmountEntered = False
        End If
        optNet.Checked = False
        optGross.Checked = False
        optViewByTransaction.Checked = True
        UctCurrency.CurrencyId = 0
        ' If this failed warn the user
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("An error occured clearing the interface.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.
        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' If we have data process the next set of actions.
            If lvwTransactions.Items.Count Then
                m_lReturn = m_oGeneral.ProcessCommand()
            Else
                m_lReturn = gPMConstants.PMEReturnCode.PMTrue
            End If

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' If we have an active account unlock it
                If m_lAccountId > 0 Then
                    m_lReturn = UnlockInsurer(m_lAccountId)
                End If

                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub cmdPartPay_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPartPay.Click
        ' Part pay selected entries

        m_lReturn = MarkPartPayment(False)

        ' If this failed warn the user
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("An error occured part paying selected transaction entries", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        EnableDisableAllocateButton()
        'Call this function because Marked value was not refereshing while doing Part Payment.
        If lvwTransactions.Items.Count > 0 Then
            RefreshEntries(lvwTransactions.SelectedItems(0))
        End If

    End Sub

    Private Sub cmdPay_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPay.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        Try


            '(RC) PN 37251 - Disable Pay button untill the process completes to avoid multiple clicks
            If Not bPayButtonClicked Then
                cmdPay.Enabled = False
                bPayButtonClicked = True
            End If
            If m_bReciptOrPay Then
                m_lReturn = Validation()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
            End If
            ' Process the binder
            ShowProgressBar(True)
            m_lReturn = ProcessPay()

            ' Check return and inform the user
            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Success
                    Me.Focus()
                    MessageBox.Show("Payment posted succesfully", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    m_bReciptAmountEntered = False
                    txtReciptPaymentAmount.Text = ""
                    ''--Call Remitance Advice Report if payment succeeded
                    ''ShowRemitanceAdviceReport()

                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' No records found for operation this is
                    MessageBox.Show("Nothing has been selected for payment", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub

                Case gPMConstants.PMEReturnCode.PMCancel
                    ' The total amount marked was zero, unable to binder
                    MessageBox.Show("Posting aborted", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Case Else
                    ' Error
                    MessageBox.Show("Roadmap failed, please check insurer payment in account enquiry.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Select

            ' Refresh the list on successful or error binder
            PerformSearch()

            ResetProgressBarMaximum()
            ShowProgressBar(False)


        Catch ex As Exception
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdPay_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            '(RC) PN 37251 - Enable Pay button after the process completes
            'cmdPay.Enabled = True
            bPayButtonClicked = False

        End Try
    End Sub

    Private Sub cmdwriteoff_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdwriteoff.Click

        Dim DocumentID As Integer
        Dim sSelectedTrans, sTemp As String
        Dim sAltReferance As String = String.Empty

        'Validate WriteOffPermissions
        m_lReturn = ValidateWriteOff()

        Dim Str As String = "0"
        Dim VarLoop As Integer = 1
        Dim WriteOffamt As Decimal = 0
        Dim iSign As Integer = 1

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("cmdwriteoff_Click", "Unable to retrieve User Authority Write Off details.")
            Exit Sub
        Else
            If m_bHasWriteOffAuthority Then
                If m_lWriteOffAmount > 0 Then
                    While VarLoop = 1
                        Str = Interaction.InputBox("Enter Write Off Amount", "Payment", gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMFunctions.ToSafeCurrency(Str)))
                        sTemp = Str
                        If Str.IndexOf("."c) >= 0 Then
                            sTemp = Mid(Str, 1, Str.IndexOf("."c))
                        Else
                            sTemp = Str
                        End If

                        If sTemp.Length > 12 Then
                            MessageBox.Show("Please enter a valid Currency amount.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            Exit Sub
                        End If
                        Dim dbNumericTemp As Double
                        If gPMFunctions.ToSafeCurrency(Str) = 0 Then 'User has pressed Ok with 0 or Pressed Cancel Button
                            If MessageBox.Show("You have not entered the write off amount." & Strings.Chr(13) & Strings.Chr(10) &
                                               "Do you wish to re-enter the amount?", "Insurer Payment", MessageBoxButtons.OKCancel) = System.Windows.Forms.DialogResult.OK Then
                                VarLoop = 1
                            Else
                                VarLoop = 2 'Dont wish to modify writeoff entry
                                Exit Sub
                            End If
                        ElseIf Not Double.TryParse(Str.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then  'User has eneterd wrong amount
                            If MessageBox.Show("You have entered an invalid write off amount." & Strings.Chr(13) & Strings.Chr(10) &
                                               "Do you wish to re-enter the amount?", "Insurer Payment", MessageBoxButtons.OKCancel) = System.Windows.Forms.DialogResult.OK Then
                                VarLoop = 1
                                Str = "0"
                            Else
                                VarLoop = 2 'Dont wish to modify writeoff entry
                                Exit Sub
                            End If
                        Else
                            WriteOffamt = gPMFunctions.ConvertCurrencyStringToValue(Str)
                            If Math.Abs(WriteOffamt) > Math.Abs(m_lWriteOffAmount) Then 'if write off amount exceeds user Write Off Limit
                                If MessageBox.Show("Entered write off amount exceeds user Write Off Limit." & Strings.Chr(13) & Strings.Chr(10) &
                                                   "Do you wish to re-enter the amount?", "Insurer Payment", MessageBoxButtons.OKCancel) = System.Windows.Forms.DialogResult.OK Then
                                    VarLoop = 1
                                Else
                                    VarLoop = 2 'Dont wish to modify writeoff entry
                                    Exit Sub
                                End If
                            Else
                                VarLoop = 0
                            End If
                        End If
                    End While
                    'code to handle write off
                    If VarLoop = 0 Then
                        'Check if the write off > outstanding amount
                        If Math.Abs(WriteOffamt) > Math.Abs(m_cTransOutstandingAmt) Then
                            MessageBox.Show("Write-Off amount cannot be greater than Total Outstanding Amount", "Insurer Payment", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Sub
                        Else
                            sSelectedTrans = m_sTransKey
                            'Get Document_id out of ListKey
                            DocumentID = CInt(Conversion.Val(m_sTransKey.Substring(m_sTransKey.Length - (m_sTransKey.Length - 2))))

                            'Get Alt Referance 'PM032903
                            Dim oTransaction As Transaction = m_cTransactions.Item(m_sTransKey)
                            sAltReferance = oTransaction.AlternateRef
                            'Add writeoff Transaction

                            m_lReturn = g_oBusiness.AddWriteOffTransaction(DocumentID, m_lAccountId, m_lWriteOffAcc_id, WriteOffamt, sAltReferance)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                MessageBox.Show("Failed to add Write Off Transaction", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Exit Sub
                            End If

                            PerformSearch()
                            For iCounter As Integer = 1 To lvwTransactions.Items.Count
                                If lvwTransactions.Items.Item(iCounter - 1).Name = sSelectedTrans Then
                                    lvwTransactions.Items.Item(iCounter - 1).Selected = True
                                    lvwTransactions.FocusedItem = lvwTransactions.Items.Item(iCounter - 1)
                                    lvwTransactions.FocusedItem.EnsureVisible()
                                    'Devrloper Guide No.49 (Guide)
                                    ''lvwTransactions_ItemClick(lvwTransactions.Items(iCounter - 1))
                                    If lvwTransactions.SelectedItems.Count > 0 Then
                                        RefreshEntries(lvwTransactions.Items(iCounter - 1))
                                    End If
                                Else
                                    'Let the other trans
                                    lvwTransactions.Items.Item(iCounter - 1).Selected = False
                                End If
                            Next iCounter
                        End If
                    End If
                Else
                    MessageBox.Show("You don't have sufficient authority limit to write off selected transaction.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If
            Else
                MessageBox.Show("The user does not have the authority to write off.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
        End If

    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            If txtAccountCode.Enabled Then
                txtAccountCode.Focus()
            End If
            If m_sView IsNot Nothing AndAlso m_sView = PMEComponentAction.PMView AndAlso m_nBatchID <> 0 Then
                m_lReturn = GetBusiness()
                m_lReturn = DataToInterface()
                cmdOK.Enabled = False
                cmdPay.Enabled = False
                cmdHelp.Enabled = False
            End If
        End If
    End Sub


    Private Sub lvwInstalmentEntries_DblClick() Handles lvwInstalmentEntries.DoubleClick
        If Not m_bInstalmentRightButton Then
            If lvwInstalmentEntries.Items.Count = 0 Then
                Exit Sub
            End If

            ' Mark the select transactions
            m_lReturn = MarkInstEntries()

            ' If this failed warn the user
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Call MsgBox("An error occured marking selected transactions", vbCritical, Me.Text)
            End If
        End If
    End Sub

    Private Sub lvwInstalmentEntries_MouseUp(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Single, ByVal y As Single)

        Const kMethodName As String = "lvwInstalmentEntries_MouseUp"
        Try
            m_bInstalmentRightButton = False

            If Button = MouseButtonConstants.RightButton Then
                m_bInstalmentRightButton = True
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
        End Try
    End Sub


    Private Sub Form_Initialize_Renamed()
        ' Forms initialise event.
        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set resize details for form controls
            SetResize()

            ' Create and initialise an instance of the general interface object.
            m_oGeneral = New iACTInsurerPaymentSFU.General()
            g_oObjectManager = New bObjectManager.ObjectManager()
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=g_oBusiness)
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

            'Get Currency Object.
            Dim temp_m_oCurrency As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrency, "bACTCurrency.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oCurrency = temp_m_oCurrency
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'Get User Authorities Object
            Dim temp_m_oUserAuthority As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oUserAuthority, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oUserAuthority = temp_m_oUserAuthority
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Set the interface status to cancelled. This is done so that any interface termination
            ' will be noted as cancelled except in the event of accepting the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel


        Catch ex As Exception
            ' Error Section
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.
        Try

            iPMFunc.ShowFormInTaskBar_Detach()


            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error, so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the status for the business object.

            m_lReturn = g_oBusiness.SetStatus(sProcessStatus:=m_sProcessStatus.Value, sMapStatus:=m_sMapStatus.Value, sStepStatus:=m_sStepStatus.Value)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Form_Load, Failed to set the status for the business object")
            End If

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            Me.Refresh()

            ' Start - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.4.1.9)
            'm_oTT = New CTooltip()
            'm_oTT.Style = CTooltip.ttStyleEnum.TTBalloon
            'm_oTT.Icon = CTooltip.ttIconType.TTIconInfo
            m_ottnew = New ToolTip
            m_ottnew.IsBalloon = True
            m_ottnew.ToolTipIcon = ToolTipIcon.Info
            m_ottnew.OwnerDraw = True
            m_ottnew.ShowAlways = False
            'm_ottnew.InitialDelay = 0
            'm_ottnew.AutoPopDelay = 5000000

            'm_ottnew.


            Me.txtDueDateTo.Checked = False
            ' End - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.4.1.9)
            Me.cboMediaType.FirstItem = "(All)"

        Catch ex As Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

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

            'Developer Guide No.7
            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    eventArgs.Cancel = True
                    Cancel = 1
                    Exit Sub
                End If

                ' Unlock the insurer, if we have one
                If m_lAccountId <> 0 Then
                    m_lReturn = UnlockInsurer(m_lAccountId)
                End If
            End If

            ' Terminate any object we have been using
            TerminateObject(m_oFindTransaction)
            TerminateObject(m_oGeneral)
            If (m_oNavigatorXM Is Nothing) = False Then
                m_oNavigatorXM.Dispose()
            End If
            TerminateObject(m_oNavigatorXM)


        Catch ex As Exception
            ' Error Section.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            eventArgs.Cancel = Cancel <> 0
        End Try
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            ' Enforce minimum sizes
            If Me.WindowState = FormWindowState.Normal Then
                If VB6.PixelsToTwipsX(Width) < 10605 Then Width = VB6.TwipsToPixelsX(10605)
                If VB6.PixelsToTwipsY(Height) < 7365 Then Height = VB6.TwipsToPixelsY(7365)
            End If

            If Me.WindowState <> FormWindowState.Minimized Then
                ' Resize the screen
                uctAnchor.Resize_Renamed(m_lWidth, m_lHeight, CInt(VB6.PixelsToTwipsX(ClientRectangle.Width)), CInt(VB6.PixelsToTwipsY(ClientRectangle.Height)))

                ' Store last sizes
                m_lWidth = CInt(VB6.PixelsToTwipsX(ClientRectangle.Width))
                m_lHeight = CInt(VB6.PixelsToTwipsY(ClientRectangle.Height))
            End If

        Catch exc As System.Exception

        End Try
    End Sub

    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        If Not (m_oAccount Is Nothing) Then

            m_oAccount.Dispose()
            m_oAccount = Nothing
        End If

        If Not (m_oCurrencyConvert Is Nothing) Then

            m_oCurrencyConvert.Dispose()
            m_oCurrencyConvert = Nothing
        End If

        If Not (m_oCurrency Is Nothing) Then

            m_oCurrency.Dispose()
            m_oCurrency = Nothing
        End If

        SaveInterfaceDisplaySettings(lvwTransactions)
        SaveInterfaceDisplaySettings(lvwEntries)

    End Sub

    Private Sub lvwEntries_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwEntries.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwEntries.Columns(eventArgs.Column)

        Try
            ' If new column reset sort order
            If ListViewHelper.GetSortKeyProperty(lvwEntries) <> ColumnHeader.Index Then
                ListViewHelper.SetSortOrderProperty(lvwEntries, SortOrder.Descending)
            End If

            '' If current sort column header is pressed.
            Select Case ColumnHeader.Index
                Case MainModule.ListViewEntryEnum.ACLECompany, MainModule.ListViewEntryEnum.ACLEPeriod, MainModule.ListViewEntryEnum.ACLEDocumentRef, MainModule.ListViewEntryEnum.ACLESpare
                    ' String columns
                    ListViewHelper.SetSortedProperty(lvwTransactions, False)
                    ListViewHelper.SetSortKeyProperty(lvwTransactions, ColumnHeader.Index)
                    If ListViewHelper.GetSortOrderProperty(lvwEntries) = SortOrder.Ascending Then
                        ListViewHelper.SetSortOrderProperty(lvwTransactions, SortOrder.Descending)
                    Else
                        ListViewHelper.SetSortOrderProperty(lvwTransactions, SortOrder.Ascending)
                    End If
                    ListViewHelper.SetSortedProperty(lvwTransactions, True)

                Case MainModule.ListViewEntryEnum.ACLECurrencyAmount, MainModule.ListViewEntryEnum.ACLEPaidAmount, MainModule.ListViewEntryEnum.ACLENetAmount, MainModule.ListViewEntryEnum.ACLEMarkedAmount
                    ' Currency columns
                    ListViewHelper.SetSortedProperty(lvwEntries, False)
                    'Devrloper Guide No.178 (Latest Guide)
                    If ListViewHelper.GetSortOrderProperty(lvwEntries) = SortOrder.Ascending Then
                        m_lReturn = ListView6Func.ListViewSortByValue(v_oListView:=lvwEntries, v_iSourceColumn:=ColumnHeader.Index, v_iDirection:=SortOrder.Descending, v_bMarkSortedColumn:=True, v_bIsCurrency:=True)
                    Else
                        m_lReturn = ListView6Func.ListViewSortByValue(v_oListView:=lvwEntries, v_iSourceColumn:=ColumnHeader.Index, v_iDirection:=SortOrder.Ascending, v_bMarkSortedColumn:=True, v_bIsCurrency:=True)
                    End If
                    ListViewHelper.SetSortKeyProperty(lvwEntries, ColumnHeader.Index)
            End Select
            'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.6.1)
            m_iEntriesSortKey = ColumnHeader.Index + 1
            m_iEntriesSortOrder = ListViewHelper.GetSortOrderProperty(lvwEntries)
            'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.6.1)

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwEntries_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


        End Try

    End Sub

    Private Sub lvwEntries_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwEntries.DoubleClick
        'RKS 28/06/2004
        If lvwEntries.Items.Count = 0 Then
            Exit Sub
        End If

        ' Mark the select transactions
        m_lReturn = MarkEntries()

        ' If this failed warn the user
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("An error occured marking selected transactions", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub lvwEntries_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawListViewItemEventArgs) Handles lvwEntries.DrawItem
        e.DrawDefault = False
        e.DrawBackground()
        'Dim oEntry As TransactionEntry
        'oEntry = m_cTransactions.Item(lvwTransactions.SelectedItems(0).Index + 1)(e.Item.Index + 1)
        'If oEntry.Comment = "" Then
        '    e.Graphics.DrawImage(imglImages.Images(ACIconNoComment), e.Item.Bounds.Location)
        'Else
        '    e.Graphics.DrawImage(imglImages.Images(ACIconComment), e.Item.Bounds.Location)
        'End If
        e.Graphics.DrawString(e.Item.Text, e.Item.Font, New SolidBrush(e.Item.ForeColor), e.Item.Bounds.Location.X + imglImages.Images(ACIconComment).Width, e.Item.Bounds.Location.Y)
        Dim oEntry As TransactionEntry
        oEntry = m_cTransactions.Item(lvwTransactions.SelectedItems(0).Tag)(e.Item.Index + 1)

        drawEntryimage(oEntry, e.Item)


    End Sub
    Private Sub drawEntryimage(ByVal oEntry As TransactionEntry, Optional ByVal oListItem As ListViewItem = Nothing)

        ' Were we passed a listitem?
        If oListItem Is Nothing Then
            ' Get the listitem (protect from errors as it may not be currently visible)


            oListItem = lvwEntries.Items.Item(oEntry.Key)
        End If

        Try

            ' Ensure we found an item
            If Not (oListItem Is Nothing) Then
                ' Set icons, parent row will be done by calling function
                Select Case oEntry.IsMarked
                    Case TransactionEntry.MarkedStatusEnum.acmseFullyMarked
                        ' Set the icon to checked

                        'Devrloper Guide No.49 (Guide)
                        oListItem.ImageKey = ACIconCheck

                    Case TransactionEntry.MarkedStatusEnum.acmsePartMarked
                        ' Set the icon to part checked

                        'Devrloper Guide No.127 (Latest Guide)

                        oListItem.ImageKey = ACIconPart


                    Case Else
                        ' No icons

                        'Devrloper Guide No.49 (Guide)
                        oListItem.ImageKey = ACIconBlank

                End Select
            End If

        Catch
        End Try

    End Sub

    Private Sub lvwEntries_DrawSubItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawListViewSubItemEventArgs) Handles lvwEntries.DrawSubItem
        If e.Item.SubItems(1) Is e.SubItem Then
            e.DrawDefault = False
            e.DrawBackground()
            Dim oEntry As TransactionEntry
            oEntry = m_cTransactions.Item(lvwTransactions.SelectedItems(0).Tag)(e.Item.Index + 1)
            If oEntry.Comment = "" Then
                e.Graphics.DrawImage(imglImages.Images(ACIconNoComment), e.SubItem.Bounds.Location)
            Else
                e.Graphics.DrawImage(imglImages.Images(ACIconComment), e.SubItem.Bounds.Location)
            End If

            e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, New SolidBrush(e.SubItem.ForeColor), e.SubItem.Bounds.Location.X + imglImages.Images(ACIconComment).Width, e.SubItem.Bounds.Location.Y)
        Else
            e.DrawDefault = True
        End If

    End Sub

    Private Sub lvwEntries_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwEntries.Enter
        VB6.SetDefault(cmdFindNow, False)
    End Sub

    Private Sub lvwEntries_ItemMouseHover(ByVal sender As Object, ByVal e As System.Windows.Forms.ListViewItemMouseHoverEventArgs) Handles lvwEntries.ItemMouseHover
        Dim x As Single = e.Item.Position.X 'EventArgs.X
        Dim y As Single = e.Item.Position.Y 'EventArgs.Y

        Const kMethodName As String = "lvwEntries_MouseMove"

        Dim oSelectedItem As ListViewItem
        Dim sToolTipTitle As String = ""
        Try


            With lvwEntries
                oSelectedItem = .GetItemAt(x, y)
                If Not (oSelectedItem Is Nothing) And Not IsNothing(lvwTransactions.FocusedItem) Then
                    If Not (m_cTransactions Is Nothing) Then
                        If m_lCurItemIndex <> oSelectedItem.Index + 1 Then
                            m_lCurItemIndex = oSelectedItem.Index + 1
                            sToolTipTitle = ACTransactionComment
                            m_ottnew.ToolTipTitle = sToolTipTitle

                            If m_lCurItemIndex = 0 Then
                                'm_oTT.Destroy()
                                'm_ottnew.ToolTipTitle = ""
                                'm_ottnew.Show("", lvwEntries, 0)
                                m_ottnew.Hide(lvwEntries)
                            Else
                                'sToolTipTitle = sToolTipTitle & _
                                '                lvwTransactions.listViewHelper1.GetListViewSubItem(lvwTransactions.FocusedItem, MainModule.ListViewTransactionEnum.ACLTDocumentRef + 1).Text.Trim()
                                sToolTipTitle = sToolTipTitle &
                                ListViewHelper.GetListViewSubItem(lvwTransactions.FocusedItem, MainModule.ListViewTransactionEnum.ACLTDocumentRef + 1).Text.Trim()
                                'm_oTT.Title = sToolTipTitle
                                'm_ottnew = New ToolTip
                                'm_ottnew.IsBalloon = True
                                'm_ottnew.ToolTipIcon = ToolTipIcon.Info
                                'm_ottnew.OwnerDraw = True
                                m_ottnew.ToolTipTitle = sToolTipTitle
                                'If (m_cTransactions.Item(Convert.ToString(.Items.Item(oSelectedItem.Index).Tag)).Item(iRowPos).Comment) <> "" Then

                                'End If
                                If (m_cTransactions.Item(Convert.ToString(lvwTransactions.FocusedItem.Tag)).Item(Convert.ToString(.Items.Item(oSelectedItem.Index).Index + 1)).Comment) <> "" Then

                                    'm_oTT.TipText = m_cTransactions.Item(Convert.ToString(lvwTransactions.FocusedItem.Tag)).Item(Convert.ToString(.Items.Item(oSelectedItem.Index).Tag)).Comment.Trim()
                                    'm_oTT.Create(lvwEntries.Handle.ToInt32())
                                    'm_ottnew.SetToolTip(lvwEntries, m_cTransactions.Item(Convert.ToString(lvwTransactions.FocusedItem.Tag)).Item(Convert.ToString(.Items.Item(oSelectedItem.Index).Tag)).Comment.Trim())
                                    m_ottnew.Show(m_cTransactions.Item(Convert.ToString(lvwTransactions.FocusedItem.Tag)).Item(Convert.ToString(.Items.Item(oSelectedItem.Index).Index + 1)).Comment.Trim(), lvwEntries)
                                    Threading.Thread.Sleep(750)
                                Else
                                    'm_oTT.Destroy()
                                    m_ottnew.ToolTipTitle = ""
                                    'm_ottnew.Show("", lvwEntries, 0)
                                    'm_ottnew.Hide(lvwEntries)
                                End If
                            End If
                        End If
                    Else
                        'm_oTT.Destroy()
                        'm_ottnew.ToolTipTitle = ""
                        'm_ottnew.Show("", lvwEntries)
                        m_ottnew.Hide(lvwEntries)
                        m_lCurItemIndex = 0
                    End If
                Else
                    'm_oTT.Destroy()
                    m_ottnew.Hide(lvwEntries)
                    m_lCurItemIndex = 0
                End If

            End With
            'End If



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
        Finally


        End Try
    End Sub

    Private Sub lvwEntries_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwEntries.Leave
        VB6.SetDefault(cmdFindNow, True)
    End Sub

    Private Sub lvwEntries_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwEntries.LostFocus
        m_lCurItemIndex = 0
    End Sub


    ' Start - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.4.1.9)
    Private Sub lvwEntries_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) 'Handles lvwEntries.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        Const kMethodName As String = "lvwEntries_MouseMove"

        Dim oSelectedItem As ListViewItem
        Dim sToolTipTitle As String = ""
        Try


            With lvwEntries
                oSelectedItem = .GetItemAt(x, y)
                If Not (oSelectedItem Is Nothing) Then
                    If Not (m_cTransactions Is Nothing) Then
                        If m_lCurItemIndex <> oSelectedItem.Index + 1 Then
                            m_lCurItemIndex = oSelectedItem.Index + 1
                            sToolTipTitle = ACTransactionComment

                            If m_lCurItemIndex = 0 Then
                                'm_oTT.Destroy()
                                m_ottnew.Hide(lvwEntries)
                            Else
                                'sToolTipTitle = sToolTipTitle & _
                                '                lvwTransactions.listViewHelper1.GetListViewSubItem(lvwTransactions.FocusedItem, MainModule.ListViewTransactionEnum.ACLTDocumentRef + 1).Text.Trim()
                                sToolTipTitle = sToolTipTitle &
                                ListViewHelper.GetListViewSubItem(lvwTransactions.FocusedItem, MainModule.ListViewTransactionEnum.ACLTDocumentRef + 1).Text.Trim()
                                'm_oTT.Title = sToolTipTitle
                                'm_ottnew = New ToolTip
                                'm_ottnew.IsBalloon = True
                                'm_ottnew.ToolTipIcon = ToolTipIcon.Info
                                'm_ottnew.OwnerDraw = True
                                m_ottnew.ToolTipTitle = sToolTipTitle

                                'If (m_cTransactions.Item(Convert.ToString(.Items.Item(oSelectedItem.Index).Tag)).Item(iRowPos).Comment) <> "" Then

                                'End If
                                If (m_cTransactions.Item(Convert.ToString(lvwTransactions.FocusedItem.Tag)).Item(Convert.ToString(.Items.Item(oSelectedItem.Index).Index + 1)).Comment) <> "" Then

                                    'm_oTT.TipText = m_cTransactions.Item(Convert.ToString(lvwTransactions.FocusedItem.Tag)).Item(Convert.ToString(.Items.Item(oSelectedItem.Index).Tag)).Comment.Trim()
                                    'm_oTT.Create(lvwEntries.Handle.ToInt32())
                                    'm_ottnew.SetToolTip(lvwEntries, m_cTransactions.Item(Convert.ToString(lvwTransactions.FocusedItem.Tag)).Item(Convert.ToString(.Items.Item(oSelectedItem.Index).Tag)).Comment.Trim())
                                    m_ottnew.Show(m_cTransactions.Item(Convert.ToString(lvwTransactions.FocusedItem.Tag)).Item(Convert.ToString(.Items.Item(oSelectedItem.Index).Index + 1)).Comment.Trim(), lvwEntries)
                                Else
                                    'm_oTT.Destroy()
                                    m_ottnew.Hide(lvwEntries)
                                End If
                            End If
                        End If
                    Else
                        'm_oTT.Destroy()
                        m_ottnew.Hide(lvwEntries)
                        m_lCurItemIndex = 0
                    End If
                Else
                    'm_oTT.Destroy()
                    m_ottnew.Hide(lvwEntries)
                    m_lCurItemIndex = 0
                End If

            End With
            'End If



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
    End Sub
    ' End - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.4.1.9)

    ' Start - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.4.1.5)
    Private Sub lvwEntries_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwEntries.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        Const kMethodName As String = "lvwEntries_MouseUp"
        Try


            If Button = MouseButtons.Right Then
                If (lvwTransactions.Items.Count > 0) And (lvwEntries.Items.Count > 0) Then

                    If m_cTransactions.Item(Convert.ToString(lvwTransactions.FocusedItem.Tag)).Item(Convert.ToString(lvwEntries.FocusedItem.Index + 1)).Comment = "" Then
                        mnuAddComment.Available = True
                        mnuEditComment.Available = False
                    Else
                        mnuEditComment.Available = True
                        mnuAddComment.Available = False
                    End If
                    m_sTransType = ACTRANSENTRY
                    Ctx_mnuAddEditComment.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)

                End If
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
        End Try



    End Sub

    ' End - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.4.1.5)

    Private Sub lvwTransactions_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwTransactions.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwTransactions.Columns(eventArgs.Column)
        StoreHScrollValue()
        Try

            ' If new column reset sort order
            If ListViewHelper.GetSortKeyProperty(lvwTransactions) <> ColumnHeader.Index Then
                ListViewHelper.SetSortOrderProperty(lvwTransactions, SortOrder.Descending)
            End If

            ' If current sort column header is pressed.
            Select Case ColumnHeader.Index - 1
                Case IIf(LCase(ColumnHeader.Text) = "branch", 0, -1), MainModule.ListViewTransactionEnum.ACLTHolderName, MainModule.ListViewTransactionEnum.ACLTInsuranceRef, MainModule.ListViewTransactionEnum.ACLTDocumentRef, MainModule.ListViewTransactionEnum.ACLTHolderCode, MainModule.ListViewTransactionEnum.ACLTAlternateRef 'PN 33593 (RC)
                    ' String columns
                    ListViewHelper.SetSortedProperty(lvwTransactions, False)
                    ListViewHelper.SetSortKeyProperty(lvwTransactions, ColumnHeader.Index)
                    If ListViewHelper.GetSortOrderProperty(lvwTransactions) = SortOrder.Ascending Then
                        ListViewHelper.SetSortOrderProperty(lvwTransactions, SortOrder.Descending)
                    Else
                        ListViewHelper.SetSortOrderProperty(lvwTransactions, SortOrder.Ascending)
                    End If
                    'ListViewHelper.SetSortOrderProperty(lvwTransactions, (ListViewHelper.GetSortOrderProperty(lvwTransactions) + 1) Mod 2)
                    ListViewHelper.SetSortedProperty(lvwTransactions, True)

                Case MainModule.ListViewTransactionEnum.ACLTAccountingDate, MainModule.ListViewTransactionEnum.ACLTEffectiveDate, MainModule.ListViewTransactionEnum.ACLTDueDate, MainModule.ListViewTransactionEnum.ACLTAllocationPeriod
                    ' Date columns
                    'Devrloper Guide No.178 (Latest Guide)
                    If ListViewHelper.GetSortOrderProperty(lvwTransactions) = SortOrder.Ascending Then
                        m_lReturn = ListView6Func.ListViewSortByDate(v_oListView:=lvwTransactions, v_iSourceColumn:=ColumnHeader.Index, v_iDirection:=SortOrder.Descending, v_bMarkSortedColumn:=True)
                    Else
                        m_lReturn = ListView6Func.ListViewSortByDate(v_oListView:=lvwTransactions, v_iSourceColumn:=ColumnHeader.Index, v_iDirection:=SortOrder.Ascending, v_bMarkSortedColumn:=True)
                    End If
                Case MainModule.ListViewTransactionEnum.ACLTCurrencyTotal, MainModule.ListViewTransactionEnum.ACLTPaidTotal, MainModule.ListViewTransactionEnum.ACLTNetTotal, MainModule.ListViewTransactionEnum.ACLTMarkedTotal, MainModule.ListViewTransactionEnum.ACLTClientOS
                    ' Currency columns
                    'Devrloper Guide No.178 (Latest Guide)
                    If ListViewHelper.GetSortOrderProperty(lvwTransactions) = SortOrder.Ascending Then
                        m_lReturn = ListView6Func.ListViewSortByValue(v_oListView:=lvwTransactions, v_iSourceColumn:=ColumnHeader.Index, v_iDirection:=SortOrder.Descending, v_bMarkSortedColumn:=True, v_bIsCurrency:=True)
                    Else
                        m_lReturn = ListView6Func.ListViewSortByValue(v_oListView:=lvwTransactions, v_iSourceColumn:=ColumnHeader.Index, v_iDirection:=SortOrder.Ascending, v_bMarkSortedColumn:=True, v_bIsCurrency:=True)
                    End If
            End Select
            'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.6.1)
            m_iTransSortKey = ColumnHeader.Index + 1
            m_iTransSortOrder = ListViewHelper.GetSortOrderProperty(lvwTransactions)
            'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.6.1)
            RecoverHorizontalScroll()
        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwTransactions_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


        End Try
    End Sub

    Private Sub lvwTransactions_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwTransactions.DoubleClick
        'RKS 26/08/2004
        If lvwTransactions.Items.Count = 0 Then
            Exit Sub
        End If

        ' Mark the select transactions
        m_lReturn = MarkTransactions()

        ' If this failed warn the user
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("An error occured marking selected transactions", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        If lvwTransactions.Items.Count > 0 Then
            RefreshEntries(lvwTransactions.SelectedItems(0))
        End If
    End Sub

    Private Sub lvwTransactions_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawListViewItemEventArgs) Handles lvwTransactions.DrawItem
        e.DrawDefault = False
        e.DrawBackground()
        e.Graphics.DrawString(e.Item.Text, e.Item.Font, New SolidBrush(e.Item.ForeColor), e.Item.Bounds.Location.X + imglImages.Images(ACIconComment).Width, e.Item.Bounds.Location.Y)
        Dim oTransaction As Transaction
        oTransaction = m_cTransactions(e.Item.Tag)
        drawtransactionimage(oTransaction, e.Item)

    End Sub
    Private Sub lvwInstalmentEntries_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawListViewItemEventArgs) Handles lvwInstalmentEntries.DrawItem

        e.DrawDefault = False
        e.DrawBackground()
        e.Graphics.DrawString(e.Item.Text, e.Item.Font, New SolidBrush(e.Item.ForeColor), e.Item.Bounds.Location.X + imglImages.Images(ACIconComment).Width, e.Item.Bounds.Location.Y)
        Dim oTransaction As Transaction
        Dim oInstalmentEntry As TransactionInst
        ' Dim oListItem As ListViewItem
        oTransaction = m_cTransactions.Item(lvwTransactions.SelectedItems(0).Tag)
        oInstalmentEntry = oTransaction.ItemInstalment(e.Item.Index + 1)
        drawinstalmentimage(oInstalmentEntry, e.Item)

    End Sub
    Private Sub drawinstalmentimage(ByVal oInstalmentEntry As TransactionInst, Optional ByVal oListItem As ListViewItem = Nothing)
        ' Were we passed a listitem?
        If oListItem Is Nothing Then
            ' Get the listitem (protect from errors as it may not be currently visible)
            oListItem = lvwInstalmentEntries.Items(oInstalmentEntry.Key)
        End If
        Try
            ' Ensure we found an item
            If Not (oListItem Is Nothing) Then
                ' Set icons, parent row will be done by calling function
                ' Set icons, parent row will be done by calling function
                Select Case oInstalmentEntry.IsMarked
                    Case TransactionEntry.MarkedStatusEnum.acmseFullyMarked
                        ' Set the icon to checked
                        oListItem.ImageKey = ACIconCheck
                        oListItem.ImageKey = ACIconCheck
                    Case TransactionEntry.MarkedStatusEnum.acmsePartMarked
                        ' Set the icon to part checked
                        oListItem.ImageKey = ACIconPart
                        oListItem.ImageKey = ACIconPart
                    Case Else
                        ' No icons
                        oListItem.ImageKey = ACIconBlank
                        oListItem.ImageKey = ACIconBlank
                End Select
            End If
        Catch
        End Try

    End Sub

    Private Sub lvwInstalmentEntries_DrawSubItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawListViewSubItemEventArgs) Handles lvwInstalmentEntries.DrawSubItem
        If e.Item.SubItems(1) Is e.SubItem Then
            e.DrawDefault = False
            e.DrawBackground()
            Dim sTransTp As String = ""
            Dim oTransaction As Transaction
            Dim lTransId As Integer
            Dim iRowPos As Integer
            sTransTp = e.Item.SubItems(4).Text.Substring(0, 3)
            If (cboTransType.SelectedIndex = 0) Or (cboTransType.SelectedIndex = 1 And (sTransTp = "CLR" Or sTransTp = "CLP")) Or (cboTransType.SelectedIndex = 2 And sTransTp <> "CLR" And sTransTp <> "CLP") Then
                oTransaction = m_cTransactions(e.Item.Tag)
                If ValidSource(vSource:=oTransaction.CompanyID) Then

                    'oListItem = lvwTransactions.Items.Add(oTransaction.Key, "", "")
                    oTransaction.GetMinTransactionId(oTransaction, lTransId, iRowPos, True)

                    Dim subitm As New ListViewItem.ListViewSubItem
                    subitm.Text = CStr(oTransaction.Item(1).CompanyID)
                    If oTransaction.Item(iRowPos).Comment = "" Then

                        e.Graphics.DrawImage(imglImages.Images(ACIconNoComment), e.SubItem.Bounds.Location)
                    Else

                        e.Graphics.DrawImage(imglImages.Images(ACIconComment), e.SubItem.Bounds.Location)
                    End If
                End If
            End If
            e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, New SolidBrush(e.SubItem.ForeColor), e.SubItem.Bounds.Location.X + imglImages.Images(ACIconComment).Width, e.SubItem.Bounds.Location.Y)
        Else
            e.DrawDefault = True
        End If
    End Sub

    Private Sub lvwTransactions_DrawSubItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawListViewSubItemEventArgs) Handles lvwTransactions.DrawSubItem
        If e.Item.SubItems(1) Is e.SubItem Then
            e.DrawDefault = False
            e.DrawBackground()
            Dim sTransTp As String = ""
            Dim oTransaction As Transaction
            Dim lTransId As Integer
            Dim iRowPos As Integer
            sTransTp = e.Item.SubItems(4).Text.Substring(0, 3)
            '' oTransaction.DocumentRef.Substring(0, 3)
            If (cboTransType.SelectedIndex = 0) Or (cboTransType.SelectedIndex = 1 And (sTransTp = "CLR" Or sTransTp = "CLP")) Or (cboTransType.SelectedIndex = 2 And sTransTp <> "CLR" And sTransTp <> "CLP") Then
                oTransaction = m_cTransactions(e.Item.Tag)
                If ValidSource(vSource:=oTransaction.CompanyID) Then

                    'oListItem = lvwTransactions.Items.Add(oTransaction.Key, "", "")
                    oTransaction.GetMinTransactionId(oTransaction, lTransId, iRowPos, True)

                    Dim subitm As New ListViewItem.ListViewSubItem
                    subitm.Text = CStr(oTransaction.Item(1).CompanyID)
                    If oTransaction.Item(iRowPos).Comment = "" Then

                        e.Graphics.DrawImage(imglImages.Images(ACIconNoComment), e.SubItem.Bounds.Location)
                    Else

                        e.Graphics.DrawImage(imglImages.Images(ACIconComment), e.SubItem.Bounds.Location)
                    End If
                End If
            End If
            e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, New SolidBrush(e.SubItem.ForeColor), e.SubItem.Bounds.Location.X + imglImages.Images(ACIconComment).Width, e.SubItem.Bounds.Location.Y)
        Else
            e.DrawDefault = True
        End If
    End Sub
    Private Sub drawtransactionimage(ByVal oTransaction As Transaction, Optional ByVal oListItem As ListViewItem = Nothing)

        Try

            ' Were we passed a listitem?
            If oListItem Is Nothing Then
                ' Get the listitem (protect from errors as it may not be currently visible)

                oListItem = lvwTransactions.Items.Item(oTransaction.Key)
                m_sTransKey = oTransaction.Key
            End If
            ' Set icons, parent row will be done by calling function
            If Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IND" And
            Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IED" And
            Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IRD" Then

                Select Case oTransaction.IsMarked
                    Case TransactionEntry.MarkedStatusEnum.acmseFullyMarked
                        ' Set the icon to checked
                        oListItem.ImageKey = ACIconCheck

                    Case TransactionEntry.MarkedStatusEnum.acmsePartMarked
                        ' Set the icon to part checked

                        oListItem.ImageKey = ACIconPart

                    Case Else
                        ' No icons
                        oListItem.ImageKey = ACIconBlank
                End Select
            Else
                Select Case oTransaction.IsMarkedInstalment
                    Case TransactionEntry.MarkedStatusEnum.acmseFullyMarked
                        ' Set the icon to checked
                        oListItem.ImageKey = ACIconCheck

                    Case TransactionEntry.MarkedStatusEnum.acmsePartMarked
                        ' Set the icon to part checked
                        oListItem.ImageKey = ACIconPart

                    Case Else
                        ' No icons
                        oListItem.ImageKey = ACIconBlank
                End Select
            End If
        Catch
            m_lReturn = gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Sub

    'vikas 'TODO Prateek no such function in vb 
    'Private Sub lvwTransactions_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawListViewItemEventArgs) Handles lvwTransactions.DrawItem
    '    e.Graphics.DrawImage(imglImages.Images(ACIconNoComment), DirectCast(e.Item, System.Windows.Forms.ListViewItem).SubItems.Item(1).Bounds.Location)
    'End Sub


    Private Sub lvwTransactions_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwTransactions.Enter
        VB6.SetDefault(cmdFindNow, False)
    End Sub


    Private Sub lvwTransactions_ItemMouseHover(ByVal sender As Object, ByVal e As System.Windows.Forms.ListViewItemMouseHoverEventArgs) Handles lvwTransactions.ItemMouseHover
        Dim x As Integer = e.Item.Position.X 'EventArgs.X
        Dim y As Integer = e.Item.Position.Y 'EventArgs.Y

        Const kMethodName As String = "lvwTransactions_MouseMove"

        Dim oSelectedItem As ListViewItem
        Dim sToolTipTitle As String = ""
        Dim oTransaction As New Transaction
        Dim lTransId As Integer
        Dim iRowPos As Integer
        Dim bInstalment As Boolean
        Try

            With lvwTransactions
                oSelectedItem = .GetItemAt(x, y)

                If Not (oSelectedItem Is Nothing) Then
                    If Not (m_cTransactions Is Nothing) Then
                        If m_lCurItemIndex <> oSelectedItem.Index + 1 Then
                            m_lCurItemIndex = oSelectedItem.Index + 1
                            sToolTipTitle = ACTransactionComment

                            m_ottnew.ToolTipTitle = sToolTipTitle
                            ' Hide the tooltip as no item selected
                            If m_lCurItemIndex = 0 Then
                                'm_oTT.Destroy()
                                m_ottnew.Hide(lvwTransactions)
                            Else
                                sToolTipTitle = sToolTipTitle &
                                                ListViewHelper.GetListViewSubItem(.Items.Item(oSelectedItem.Index), MainModule.ListViewTransactionEnum.ACLTDocumentRef + 1).Text.Trim()
                                m_ottnew.ToolTipTitle = sToolTipTitle

                                oTransaction.GetMinTransactionId(m_cTransactions.Item(Convert.ToString(.Items.Item(oSelectedItem.Index).Tag)), lTransId, iRowPos, bInstalment)
                                If Not bInstalment Then
                                    If (m_cTransactions.Item(Convert.ToString(.Items.Item(oSelectedItem.Index).Tag)).Item(iRowPos).Comment) <> "" Then
                                        m_ottnew.Show(m_cTransactions.Item(Convert.ToString(.Items.Item(oSelectedItem.Index).Tag)).Item(iRowPos).Comment.Trim(), lvwTransactions)
                                        Threading.Thread.Sleep(750)
                                    Else
                                        m_ottnew.ToolTipTitle = ""
                                    End If
                                End If
                            End If
                        End If
                    Else
                        m_ottnew.Hide(lvwTransactions)
                        m_lCurItemIndex = 0
                    End If
                Else
                    m_ottnew.Hide(lvwTransactions)
                    m_lCurItemIndex = 0
                End If

            End With

        Catch ex As Exception



            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
        End Try

    End Sub
    Private Sub lvwTransactions_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwTransactions.SelectedIndexChanged
        If lvwTransactions.SelectedItems.Count > 0 Then
            RefreshEntries(lvwTransactions.SelectedItems(0))
        End If
    End Sub

    Private Sub lvwTransactions_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwTransactions.Leave
        VB6.SetDefault(cmdFindNow, True)
    End Sub

    ' Start - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.4.1.9)
    Private Sub lvwTransactions_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwTransactions.MouseMove
        Dim x As Integer = eventArgs.X
        Dim y As Integer = eventArgs.Y

        Const kMethodName As String = "lvwTransactions_MouseMove"

        Dim oSelectedItem As ListViewItem
        Dim sToolTipTitle As String = ""
        Dim oTransaction As New Transaction
        Dim lTransId As Integer
        Dim iRowPos As Integer

        Try

            With lvwTransactions
                oSelectedItem = .GetItemAt(x, y)

                If Not (oSelectedItem Is Nothing) Then
                    If Not (m_cTransactions Is Nothing) Then
                        If m_lCurItemIndex <> oSelectedItem.Index + 1 Then
                            m_lCurItemIndex = oSelectedItem.Index + 1
                            sToolTipTitle = ACTransactionComment

                            ' Hide the tooltip as no item selected
                            If m_lCurItemIndex = 0 Then
                                'm_oTT.Destroy()
                                m_ottnew.Hide(lvwTransactions)
                            Else
                                sToolTipTitle = sToolTipTitle &
                                                ListViewHelper.GetListViewSubItem(.Items.Item(oSelectedItem.Index), MainModule.ListViewTransactionEnum.ACLTDocumentRef + 1).Text.Trim()
                                m_ottnew.ToolTipTitle = sToolTipTitle
                                If Mid(Trim(.Items.Item(oSelectedItem.Index).SubItems(MainModule.ListViewTransactionEnum.ACLTDocumentRef + 1).Text), 1, 3) <> "IND" And Mid(Trim(.Items.Item(oSelectedItem.Index).SubItems(MainModule.ListViewTransactionEnum.ACLTDocumentRef + 1).Text), 1, 3) <> "IED" And Mid(Trim(.Items.Item(oSelectedItem.Index).SubItems(MainModule.ListViewTransactionEnum.ACLTDocumentRef + 1).Text), 1, 3) <> "IRD" Then
                                    oTransaction.GetMinTransactionId(m_cTransactions.Item(Convert.ToString(.Items.Item(oSelectedItem.Index).Tag)), lTransId, iRowPos, True)


                                    If (m_cTransactions.Item(Convert.ToString(.Items.Item(oSelectedItem.Index).Tag)).Item(iRowPos).Comment) <> "" Then
                                        m_ottnew.Show(m_cTransactions.Item(Convert.ToString(.Items.Item(oSelectedItem.Index).Tag)).Item(iRowPos).Comment.Trim(), lvwTransactions)
                                        Threading.Thread.Sleep(750)
                                    Else
                                        m_ottnew.Hide(lvwTransactions)
                                    End If

                                End If
                            End If
                        Else
                            m_ottnew.Hide(lvwTransactions)
                            m_lCurItemIndex = 0
                        End If
                    Else
                        m_ottnew.Hide(lvwTransactions)
                        m_lCurItemIndex = 0
                    End If
                End If
            End With

        Catch ex As Exception



            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
        End Try



    End Sub

    Private Sub lvwTransactions_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwTransactions.LostFocus
        m_lCurItemIndex = 0
    End Sub
    ' End - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.4.1.9)

    ' Start - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.4.1.4)
    Private Sub lvwTransactions_MouseUp(ByVal eventSender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvwTransactions.MouseUp
        Dim Button As Integer = CInt(e.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = e.X
        Dim y As Single = e.Y

        Const kMethodName As String = "lvwTransactions_MouseUp"

        Dim lTransId As Integer
        Dim iRowPos As Integer
        Dim oTransaction As New Transaction
        Dim bInstalment As Boolean
        Try

            m_bTransactionRightButton = False
            If Button = MouseButtons.Right Then
                If lvwTransactions.Items.Count > 0 Then

                    oTransaction.GetMinTransactionId(m_cTransactions.Item(Convert.ToString(lvwTransactions.FocusedItem.Tag)), lTransId, iRowPos, bInstalment)
                    If Not bInstalment Then

                        If m_cTransactions.Item(Convert.ToString(lvwTransactions.FocusedItem.Tag)).Item(iRowPos).Comment = "" Then
                            mnuAddComment.Available = True
                            mnuEditComment.Available = False
                        Else
                            mnuEditComment.Available = True
                            mnuAddComment.Available = False
                        End If
                        m_sTransType = ACTRANSACTION
                        Ctx_mnuAddEditComment.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
                    Else
                        m_bTransactionRightButton = True
                    End If
                End If
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
        Finally
            oTransaction = Nothing
        End Try
    End Sub
    ' End - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.4.1.4)

    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.6.1)
    Private Sub SaveInterfaceDisplaySettings(ByVal oListView As ListView)

        Const kMethodName As String = "SaveInterfaceDisplaySettings"

        Dim sColumnWidth As String = ""
        Dim oXMLRootElement, oXMLComponent, oXMLInterface, oXMLListView, oXMLSortKey, oXMLSortOrder, oXMLColumnWidth As XmlElement


        g_oDOMRootForInterfaceDisplay = New XmlDocument()

        g_sUserConfigXMLDataset = ""

        If g_sUserConfigXMLDataset = "" Then

            oXMLRootElement = g_oDOMRootForInterfaceDisplay.CreateElement(m_kRootNode)

            g_oDOMRootForInterfaceDisplay.AppendChild(oXMLRootElement)

            oXMLComponent = g_oDOMRootForInterfaceDisplay.CreateElement(m_kComponentName)

            oXMLRootElement.AppendChild(oXMLComponent)

            oXMLInterface = g_oDOMRootForInterfaceDisplay.CreateElement(m_kFrmName)

            oXMLComponent.AppendChild(oXMLInterface)

            oXMLListView = g_oDOMRootForInterfaceDisplay.CreateElement(oListView.Name)

            oXMLInterface.AppendChild(oXMLListView)

            oXMLSortKey = g_oDOMRootForInterfaceDisplay.CreateElement(m_kColumnSortKey)
            If m_kTransGridName = oListView.Name Then
                oXMLSortKey.InnerText = CStr(m_iTransSortKey)
            ElseIf m_kEntriesGridName = oListView.Name Then
                oXMLSortKey.InnerText = CStr(m_iEntriesSortKey)
            End If

            oXMLListView.AppendChild(oXMLSortKey)

            oXMLSortOrder = g_oDOMRootForInterfaceDisplay.CreateElement(m_kColumnSortOrder)
            If m_kTransGridName = oListView.Name Then
                oXMLSortOrder.InnerText = CStr(m_iTransSortOrder)
            ElseIf m_kEntriesGridName = oListView.Name Then
                oXMLSortOrder.InnerText = CStr(m_iEntriesSortOrder)
            End If

            oXMLListView.AppendChild(oXMLSortOrder)

            m_lReturn = GetListViewColumnWidth(oListView, sColumnWidth)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetListViewColumnWidth Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            oXMLColumnWidth = g_oDOMRootForInterfaceDisplay.CreateElement(m_kColumnWidth)
            oXMLColumnWidth.InnerText = sColumnWidth

            oXMLListView.AppendChild(oXMLColumnWidth)

            g_sUserConfigXMLDataset = g_oDOMRootForInterfaceDisplay.InnerXml

            'Sankar only for test
            'g_oDOMRootForInterfaceDisplay.save "c:\test.xml"
        Else
            Try
                g_oDOMRootForInterfaceDisplay.LoadXml(g_sUserConfigXMLDataset)

            Catch
            End Try

            'Devrloper Guide No.36 (No Solutions)
            If g_oDOMRootForInterfaceDisplay.InnerText <> 0 Then

                'Devrloper Guide No.36 (No Solutions)
                gPMFunctions.RaiseError(kMethodName, g_oDOMRootForInterfaceDisplay.InnerText, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = GetListViewColumnWidth(oListView, sColumnWidth)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdateXMLNodeValues Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If oListView.Name = m_kTransGridName Then
                m_lReturn = UpdateXMLNodeValues(sListViewName:=oListView.Name, iSortKey:=m_iTransSortKey, iSortOrder:=m_iTransSortOrder, sColumnWidth:=sColumnWidth)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "UpdateXMLNodeValues Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            ElseIf oListView.Name = m_kEntriesGridName Then
                m_lReturn = UpdateXMLNodeValues(sListViewName:=oListView.Name, iSortKey:=m_iEntriesSortKey, iSortOrder:=m_iEntriesSortOrder, sColumnWidth:=sColumnWidth)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "UpdateXMLNodeValues Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
            'Sankar only for test
            'g_oDOMRootForInterfaceDisplay.save "c:\Output.xml"
            g_sUserConfigXMLDataset = g_oDOMRootForInterfaceDisplay.InnerXml
        End If

        Exit Sub


        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMError)


    End Sub
    'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.2.4)

    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.2.4)
    Private Function UpdateXMLNodeValues(ByVal sListViewName As String, ByVal iSortKey As Integer, ByVal iSortOrder As Integer, ByVal sColumnWidth As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateXMLNodeValues"

        Dim bComPresent, bFrmPresent, bGridPresent As Boolean
        Dim sXMLPath As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            sXMLPath = "//" & m_kRootNode & "/" & m_kComponentName
            If Not (g_oDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath) Is Nothing) Then
                If g_oDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath).HasChildNodes Then
                    bComPresent = True

                    sXMLPath = sXMLPath & "/" & m_kFrmName
                    If Not (g_oDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath) Is Nothing) Then
                        If g_oDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath).HasChildNodes Then
                            bFrmPresent = True

                            sXMLPath = sXMLPath & "/" & sListViewName
                            If Not (g_oDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath) Is Nothing) Then
                                If g_oDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath).HasChildNodes Then
                                    bGridPresent = True

                                    g_oDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath & "/" & m_kColumnSortKey).InnerText = CStr(iSortKey)
                                    g_oDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath & "/" & m_kColumnSortOrder).InnerText = CStr(iSortOrder)
                                    g_oDOMRootForInterfaceDisplay.SelectSingleNode(sXMLPath & "/" & m_kColumnWidth).InnerText = sColumnWidth
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
                m_lReturn = CreateNodeIfNotExists(sListViewName:=sListViewName, bComPresent:=bComPresent, bFrmPresent:=bFrmPresent, bGridPresent:=bGridPresent, iSortKey:=iSortKey, iSortOrder:=iSortOrder, sColumnWidth:=sColumnWidth)
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
    'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.2.4)

    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.2.4)
    Private Function CreateNodeIfNotExists(ByRef sListViewName As String, ByVal bComPresent As Boolean, ByVal bFrmPresent As Boolean, ByVal bGridPresent As Boolean, ByVal iSortKey As Integer, ByVal iSortOrder As Integer, ByVal sColumnWidth As String) As Integer

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
            oXMLListView = oXMLDoc.CreateElement(sListViewName)
            oXMLSortKey = oXMLDoc.CreateElement(m_kColumnSortKey)
            oXMLSortOrder = oXMLDoc.CreateElement(m_kColumnSortOrder)
            oXMLColumnWidth = oXMLDoc.CreateElement(m_kColumnWidth)

            oUserConfigXML = g_oDOMRootForInterfaceDisplay.SelectNodes("UserConfigXML")

            If Not bComPresent Then


                oXMLRootElement.AppendChild(oXMLForm)

                oXMLForm.AppendChild(oXMLListView)

                oXMLSortKey.InnerText = CStr(iSortKey)

                oXMLListView.AppendChild(oXMLSortKey)
                oXMLSortOrder.InnerText = CStr(iSortOrder)

                oXMLListView.AppendChild(oXMLSortOrder)
                oXMLColumnWidth.InnerText = sColumnWidth

                oXMLListView.AppendChild(oXMLColumnWidth)


                g_oDOMRootForInterfaceDisplay.ChildNodes.Item(iRootLevel).AppendChild(oXMLRootElement)

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


                        g_oDOMRootForInterfaceDisplay.ChildNodes.Item(iRootLevel).ChildNodes.Item(iComLevel).AppendChild(oXMLForm)
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

                                g_oDOMRootForInterfaceDisplay.ChildNodes.Item(iRootLevel).ChildNodes.Item(iComLevel).ChildNodes.Item(iFrmLevel).AppendChild(g_oDOMRootForInterfaceDisplay.ImportNode(oXMLListView, True))
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
    'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.2.4)

    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.2.4)
    Private Function GetListViewColumnWidth(ByVal oListView As ListView, ByRef sColumnWidth As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetListViewColumnWidth"


        result = gPMConstants.PMEReturnCode.PMTrue

        Try
            If oListView.Columns.Count > 0 Then
                For Each oColumnHeader As ColumnHeader In oListView.Columns
                    sColumnWidth = sColumnWidth & ";" & gPMFunctions.ToSafeString(VB6.PixelsToTwipsX(oColumnHeader.Width))
                Next oColumnHeader
                sColumnWidth = sColumnWidth.Substring(sColumnWidth.Length - (sColumnWidth.Length - 1))
            End If

            Return result

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            Return result
        End Try
    End Function
    'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.2.4)

    Private Sub m_oNavStart_NavigatorClose() Handles m_oNavigatorXM.NavigatorClose
        ' Set complete
        m_bNavCompleted = True
    End Sub

    Private Sub m_oNavStart_SetProcessStatus(ByVal v_bProcessComplete As Boolean) Handles m_oNavigatorXM.SetProcessStatus
        ' Store the result
        m_bProcessComplete = v_bProcessComplete
    End Sub

    ' Start - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.4.1.7)
    Public Sub mnuAddComment_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuAddComment.Click

        Const kMethodName As String = "mnuAddComment_Click"

        m_lReturn = ProcessAddEditComment()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "ProcessAddEditComment Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
    End Sub
    Private Sub optGross_Click() Handles optGross.Click
        cmdFindNow_Click(Nothing, Nothing)
        If lvwTransactions.Items.Count Then
            m_lReturn = MarkTransactions(v_bUnMarkTrans:=True)
        End If
    End Sub

    Private Sub optNet_Click() Handles optNet.Click
        cmdFindNow_Click(Nothing, Nothing)
        If lvwTransactions.Items.Count Then
            m_lReturn = MarkTransactions(v_bUnMarkTrans:=True)
        End If
    End Sub
    Public Sub mnuEditComment_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuEditComment.Click

        Const kMethodName As String = "mnuEditComment_Click"

        m_lReturn = ProcessAddEditComment()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "ProcessAddEditComment Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
    End Sub

    Private Function ProcessAddEditComment() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessAddEditComment"

        Dim ofrmAddEditComments As frmAddEditComments
        Dim sPreviousComment, sMenuBarCaption As String
        Dim lTransId As Integer
        Dim iRow As Integer
        Dim lTransdetailID As Integer
        Dim oTransaction As New Transaction

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If Not (lvwTransactions.FocusedItem Is Nothing) Then

                ofrmAddEditComments = New frmAddEditComments()

                ' Save the comment so we can determine if it requires updating after
                If m_sTransType = ACTRANSENTRY Then

                    sPreviousComment = m_cTransactions.Item(Convert.ToString(lvwTransactions.FocusedItem.Tag)).Item(Convert.ToString(lvwEntries.FocusedItem.Index + 1)).Comment.Trim()
                Else
                    'Get the min TransDetail ID which is to be updated
                    m_lReturn = oTransaction.GetMinTransactionId(m_cTransactions.Item(Convert.ToString(lvwTransactions.FocusedItem.Tag)), lTransdetailID, iRow, True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("ProcessAddEditComment", "Failed to get Transaction Id")
                    End If

                    sPreviousComment = m_cTransactions.Item(Convert.ToString(lvwTransactions.FocusedItem.Tag)).Item(iRow).Comment.Trim()
                End If

                With ofrmAddEditComments
                    .Comment = sPreviousComment
                    sMenuBarCaption = ACForTransaction

                    sMenuBarCaption = sMenuBarCaption & ListViewHelper.GetListViewSubItem(lvwTransactions.FocusedItem, MainModule.SearchArrayEnum.ACSADocumentRef + 2).Text.Trim()
                    .Text = .Text & sMenuBarCaption

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
                            gPMFunctions.RaiseError("ProcessAddEditComment", "Error occured in ProcessAddEditComment...Status returned: " & Status)
                            ofrmAddEditComments = Nothing
                            Return result
                        End If
                    Else
                        ' OK was pressed in the add/edit comment form...see if data changed/added
                        If .Comment.Trim() <> sPreviousComment Then
                            If m_sTransType = ACTRANSENTRY Then
                                m_lReturn = oTransaction.GetMinTransactionId(m_cTransactions.Item(Convert.ToString(lvwTransactions.FocusedItem.Tag)), lTransId, iRow, True)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Throw New Exception()
                                End If

                                m_cTransactions.Item(Convert.ToString(lvwTransactions.FocusedItem.Tag)).Item(Convert.ToString(lvwEntries.FocusedItem.Index + 1)).Comment = .Comment

                                lTransdetailID = m_cTransactions.Item(Convert.ToString(lvwTransactions.FocusedItem.Tag)).Item(Convert.ToString(lvwEntries.FocusedItem.Index + 1)).DetailID
                                lvwEntries.Items(lvwEntries.FocusedItem.Index).Selected = False
                                lvwEntries.Items(lvwEntries.FocusedItem.Index).Selected = True
                                'TODO
                                'If .Comment = "" Then

                                '                            lvwEntries.SelectedItem.ListSubItems(1).ReportIcon = ACIconNoComment
                                '	If lTransdetailID = lTransId Then

                                '		lvwTransactions.SelectedItem.ListSubItems(1).ReportIcon = ACIconNoComment
                                '	End If
                                'Else

                                '	lvwEntries.SelectedItem.ListSubItems(1).ReportIcon = ACIconComment
                                '	If lTransdetailID = lTransId Then

                                '		lvwTransactions.SelectedItem.ListSubItems(1).ReportIcon = ACIconComment
                                '	End If
                                'End If
                            Else

                                m_cTransactions.Item(Convert.ToString(lvwTransactions.FocusedItem.Tag)).Item(iRow).Comment = .Comment
                                RefreshEntries(lvwTransactions.Items.Item(lvwTransactions.FocusedItem.Index))
                                'TODO
                                'If .Comment = "" Then

                                '	lvwTransactions.SelectedItem.ListSubItems(1).ReportIcon = ACIconNoComment
                                '	'lvwEntries.ListItems(iRow).ListSubItems(1).ReportIcon = ACIconNoComment
                                'Else

                                '	lvwTransactions.SelectedItem.ListSubItems(1).ReportIcon = ACIconComment
                                '	'lvwEntries.ListItems(iRow).ListSubItems(1).ReportIcon = ACIconComment
                                'End If
                            End If

                            ' Call business object to update the comment for this entry in the db

                            m_lReturn = g_oBusiness.UpdateComment(v_lTransDetailId:=lTransdetailID, v_sComment:= .Comment)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("ProcessAddEditComment", "Error occured in g_oBusiness.UpdateComment...m_lReturn: " & m_lReturn)
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

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
            Return result
        End Try
    End Function
    ' End - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.4.1.10)

    Private Sub txtAccountCode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountCode.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        ' Enable payment group if we have a possible valid name
        cboPaymentGroup.Enabled = txtAccountCode.Text.Trim().Length > 0
        cboPaymentGroup.SelectedIndex = -1

        ' If we are changing an 'active' account...
        If m_lAccountId > 0 Then
            ' Unlock the insurer
            m_lReturn = UnlockInsurer(m_lAccountId)
            ' Clear the interface
            ClearInterface(True)
            m_lAccountId = 0
        End If
    End Sub

    Private Sub txtAccountCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountCode.Enter
        ' Select entire code
        txtAccountCode.SelectionStart = 0
        txtAccountCode.SelectionLength = Strings.Len(txtAccountCode.Text)
    End Sub

    Private Sub txtAccountCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountCode.Leave

        Dim lAccountID As Integer

        ' If we have something...
        If txtAccountCode.Text.Trim().Length Then
            ' Find the account

            m_lReturn = g_oBusiness.GetAccountFromShortCode(v_sShortCode:=txtAccountCode.Text.Trim(), r_lAccountId:=lAccountID)

            ' Load the new account
            m_lReturn = LoadAccount(lAccountID)
        End If

    End Sub
    ' *******************************************************************************
    ' FORM EVENTS
    ' *******************************************************************************


    '    Private Function Allocate() As Integer

    '        Dim result As Integer = 0
    '        Const kMethodName As String = "Allocate"


    '        Dim vMatchTransAlloc() As Object

    '        Dim iCount, iMainRow, iMatchRow As Integer

    '        On Error GoTo Catch_Renamed


    '        result = gPMConstants.PMEReturnCode.PMTrue

    '        ' Set the mouse pointer to busy.
    '        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
    '        cmdAllocate.Enabled = False

    '        ' Build an array of marked transdetails (for instalment entries i.e IND,IED,IRD
    '        For Each oTransaction As Transaction In m_cTransactions

    '            If ValidSource(vSource:=oTransaction.CompanyID) Then
    '                If Mid(oTransaction.DocumentRef.Trim(), 1, 3) <> "IND" And Mid(oTransaction.DocumentRef.Trim(), 1, 3) <> "IED" And Mid(oTransaction.DocumentRef.Trim(), 1, 3) <> "IRD" Then

    '                    For Each oEntry As TransactionEntry In oTransaction
    '                        If oEntry.MarkedAmount <> 0 Then
    '                            ReDim Preserve vMatchTransAlloc(iCount)
    '                            If oEntry.MarkedAmount < 0 Then

    '                                vMatchTransAlloc(iCount) = gPMFunctions.ToSafeString(oEntry.DetailID) & "|" & gPMFunctions.ToSafeString(oEntry.MarkedAccountAmount, "0")
    '                                iMainRow = iCount
    '                            Else

    '                                vMatchTransAlloc(iCount) = gPMFunctions.ToSafeString(oEntry.DetailID) & "|" & gPMFunctions.ToSafeString(oEntry.MarkedAccountAmount, "0")
    '                            End If
    '                            iCount += 1
    '                        End If
    '                    Next oEntry
    '                End If
    '            End If
    '        Next oTransaction

    '        'Do Allocation
    '        Dim vKeys(1, 3) As Object
    '        Dim vMatchTrans(vMatchTransAlloc.GetUpperBound(0) - 1) As Object
    '        iMatchRow = 0
    '        For ivar As Integer = 0 To vMatchTransAlloc.GetUpperBound(0)
    '            If ivar = iMainRow Then
    '                'AllocationTransID

    '                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID


    '                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = vMatchTransAlloc(ivar)
    '            Else


    '                vMatchTrans(iMatchRow) = vMatchTransAlloc(ivar)
    '                iMatchRow += 1
    '            End If
    '        Next ivar
    '        ' AccountID

    '        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

    '        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lAccountId


    '        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs

    '        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vMatchTrans

    '        m_lReturn = AllocateTransaction(vKeys)
    '        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '            gPMFunctions.RaiseError("Allocate", "AllocateTransaction failed.")
    '        End If


    '        GoTo Finally_Renamed

    'Catch_Renamed:
    '        ' DO Not Call any functions before here or the error will be lost
    '        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    '        ' If you want to rollback a transaction or something, do it here

    'Finally_Renamed:
    '        ' Mouse pointer back to normal
    '        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
    '        ' Terminate

    '        Return result
    '    End Function

    Private Function AllocateTransaction(ByVal vKeys(,) As Object) As Integer
        Dim result As Integer = 0
        Dim bACTAllocationManual As Object

        Const kMethodName As String = "AllocateTransaction"

        Dim oAllocationManual As bACTAllocationManual.Business

        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            'Do Allocation

            Dim temp_oAllocationManual As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oAllocationManual, "bACTAllocationManual.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oAllocationManual = temp_oAllocationManual
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("AllocateTransaction", "bACTAllocationManual.GetInstance failed.")
            End If



            m_lReturn = oAllocationManual.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("AllocateTransaction", "oAllocationManual.SetProcessModes failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set the keys

            m_lReturn = oAllocationManual.SetKeys(vKeyArray:=vKeys)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("AllocateTransaction", "oAllocationManual.SetKeys failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Start it

            oAllocationManual.CompanyId = g_iSourceID

            m_lReturn = oAllocationManual.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("AllocateTransaction", "oAllocationManual.Start failed.", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally
            ' Terminate

            oAllocationManual.Dispose()


        End Try
        Return result
    End Function

    Private Sub lvwTransactions_ColumnWidthChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ColumnWidthChangedEventArgs) Handles lvwTransactions.ColumnWidthChanged
        lvwTransactions.Refresh()
    End Sub

    Private Sub lvwEntries_ColumnWidthChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ColumnWidthChangedEventArgs) Handles lvwEntries.ColumnWidthChanged
        lvwEntries.Refresh()
    End Sub

    Private Sub LoadAllocationPeriod()
        Dim vResultArray As Object(,)
        Dim iLower, iUpper As Integer
        Dim iSystemCurrencyID As Integer
        Try
            m_lReturn = g_oBusiness.LoadAllocationPeriod(vResultArray)

            If vResultArray IsNot Nothing AndAlso Information.IsArray(vResultArray) Then
                ' Get bounds
                iLower = vResultArray.GetLowerBound(1)
                iUpper = vResultArray.GetUpperBound(1)
                Dim sCurrentPeriod As String = "Period" & Date.Today.Month.ToString() & " | " & Date.Today.Year.ToString()

                For lRow As Integer = iLower To iUpper
                    chkAllocationPeriod.Items.Add(vResultArray(0, lRow), False)
                Next
                chkAllocationPeriod.TopIndex = IIf(chkAllocationPeriod.Items.IndexOf(sCurrentPeriod) <> -1, chkAllocationPeriod.Items.IndexOf(sCurrentPeriod), 0)
                iSystemCurrencyID = Convert.ToInt32(vResultArray(vResultArray.GetUpperBound(0) - 1, 0)) '--PN 1996
            End If

            If optViewByTransaction.Checked Then
                UctCurrency.Enabled = True
                UctCurrency.CompanyId = g_iSourceID
                UctCurrency.RefreshList()

                uctTransactionCurrency.Enabled = True
                uctTransactionCurrency.CompanyId = g_iSourceID
                uctTransactionCurrency.RefreshList()

            Else
                UctCurrency.Enabled = False
                uctTransactionCurrency.Enabled = False
            End If

        Catch

        End Try


    End Sub


    Private Function ShowCommissionFrame() As Integer

        Dim bIsGrossAgent As Boolean
        Dim vArray(,) As Object
        ShowCommissionFrame = gPMConstants.PMEReturnCode.PMTrue
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Get agent details
            m_lReturn = g_oBusiness.GetAgentDetailForAccount(
                v_lAccountid:=m_lAccountId,
                r_vResultsArray:=vArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ",ShowCommissionFrame, Unable to access agent details")
            End If
            If IsArray(vArray) Then
                m_sAgentType = ToSafeString(vArray(0, 0))
                bIsGrossAgent = ToSafeBoolean(vArray(1, 0))
                m_iCurrencyId = ToSafeInteger(vArray(2, 0))
                m_sPartyType = ToSafeString(vArray(3, 0))
                m_iSourceId = ToSafeInteger(vArray(4, 0))
                If ToSafeInteger(vArray(5, 0)) = 1 Then
                    m_bPaidByClient = True
                Else
                    m_bPaidByClient = False
                End If
            Else
                m_sAgentType = ""
                bIsGrossAgent = False
                m_iCurrencyId = 0
                m_sPartyType = ""
                m_iSourceId = 0
                m_bPaidByClient = False
            End If

            If Trim(UCase(m_sAgentType)) = "BROKER" Then
                fraCommission.Visible = True
                optGross.Checked = bIsGrossAgent
                optNet.Checked = Not bIsGrossAgent
            Else
                fraCommission.Visible = False
                optNet.Checked = True
            End If
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="FindAccount", r_lFunctionReturn:=result, excep:=ex)
            Return result
        End Try

    End Function

    Private Sub txtReciptPaymentAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtReciptPaymentAmount.KeyPress
        Dim KeyAscii As Short = Asc(e.KeyChar)
        If KeyAscii >= 48 And KeyAscii <= 57 Then
            If ConvertCurrencyStringToValue(txtReciptPaymentAmount.Text) * 10 > 9999999999.0# Then
                KeyAscii = 0
            End If
        End If
        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
    End Sub


    Private Sub UctCurrency_Click() Handles UctCurrency.Click
        Dim cReciptAmount As Decimal
        Dim cTotalMarked As Decimal
        Dim cTotalWriteOff As Decimal
        Dim cUnAllocatedAmount As Decimal
        Dim iBaseCurrencyID As Integer
        Dim cBaseAmount As Decimal
        Dim cCurrencyAmount As Decimal
        Dim cAmount As Decimal

        cReciptAmount = ConvertCurrencyStringToValue(txtReciptPaymentAmount.Text)
        cTotalMarked = ConvertCurrencyStringToValue(txtMarked.Text)
        cTotalWriteOff = ConvertCurrencyStringToValue(TxttotalWriteoff.Text)
        cUnAllocatedAmount = ConvertCurrencyStringToValue(txtunallocatedAmount.Text)
        If m_iPreviousCurrencyId = 0 Then
            m_iPreviousCurrencyId = UctCurrency.CurrencyId
        End If

        ' Get the Company's base currency
        If m_iSourceId <> 0 Then
            m_lReturn =
                        m_oCurrencyConvert.GetBaseCurrency(v_lCompanyId:=m_iSourceId,
                        r_iBaseCurrencyID:=iBaseCurrencyID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("UctCurrency_Click", "Unable to retrieve base currency.")
                Exit Sub
            End If
        End If
        If UctCurrency.Text <> "(None)" Then
            If cReciptAmount <> 0 Then
                m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(
                    lCurrencyID:=m_iPreviousCurrencyId,
                    lCompanyID:=m_iSourceId,
                    cBaseAmount:=cBaseAmount,
                    cCurrencyAmount:=cReciptAmount, vConversionDate:=Date.Today())

                If UctCurrency.CurrencyId <> iBaseCurrencyID Then
                    m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(
                                lCurrencyID:=UctCurrency.CurrencyId,
                                lCompanyID:=m_iSourceId,
                                cBaseAmount:=cBaseAmount,
                                cCurrencyAmount:=cCurrencyAmount,
                                vConversionDate:=Date.Today())

                    cAmount = cCurrencyAmount
                Else
                    cAmount = cBaseAmount
                End If
                txtReciptPaymentAmount.Text = g_oBusiness.FormatCurrency(
                                    v_lCurrencyID:=UctCurrency.CurrencyId,
                                    v_cCurrencyAmount:=cAmount)
            End If
            If cTotalMarked <> 0 Then
                m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(
                    lCurrencyID:=m_iPreviousCurrencyId,
                    lCompanyID:=m_iSourceId,
                    cBaseAmount:=cBaseAmount,
                    cCurrencyAmount:=cTotalMarked,
                    vConversionDate:=Date.Today())

                If UctCurrency.CurrencyId <> iBaseCurrencyID Then
                    m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(
                                lCurrencyID:=UctCurrency.CurrencyId,
                                lCompanyID:=m_iSourceId,
                                cBaseAmount:=cBaseAmount,
                                cCurrencyAmount:=cCurrencyAmount,
                                vConversionDate:=Date.Today())

                    cAmount = cCurrencyAmount
                Else
                    cAmount = cBaseAmount
                End If
                txtMarked.Text = g_oBusiness.FormatCurrency(
                                    v_lCurrencyID:=UctCurrency.CurrencyId,
                                    v_cCurrencyAmount:=cAmount)
            End If
            If cTotalWriteOff <> 0 Then
                m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(
                    lCurrencyID:=m_iPreviousCurrencyId,
                    lCompanyID:=m_iSourceId,
                    cBaseAmount:=cBaseAmount,
                    cCurrencyAmount:=cTotalWriteOff,
                    vConversionDate:=Date.Today())

                If UctCurrency.CurrencyId <> iBaseCurrencyID Then
                    m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(
                                lCurrencyID:=UctCurrency.CurrencyId,
                                lCompanyID:=m_iSourceId,
                                cBaseAmount:=cBaseAmount,
                                cCurrencyAmount:=cCurrencyAmount,
                                vConversionDate:=Date.Today())

                    cAmount = cCurrencyAmount
                Else
                    cAmount = cBaseAmount
                End If
                TxttotalWriteoff.Text = g_oBusiness.FormatCurrency(
                                    v_lCurrencyID:=UctCurrency.CurrencyId,
                                    v_cCurrencyAmount:=cAmount)
            End If
            If cUnAllocatedAmount <> 0 Then
                m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(
                    lCurrencyID:=m_iPreviousCurrencyId,
                    lCompanyID:=m_iSourceId,
                    cBaseAmount:=cBaseAmount,
                    cCurrencyAmount:=cUnAllocatedAmount,
                    vConversionDate:=Date.Today())

                If UctCurrency.CurrencyId <> iBaseCurrencyID Then
                    m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(
                                lCurrencyID:=UctCurrency.CurrencyId,
                                lCompanyID:=m_iSourceId,
                                cBaseAmount:=cBaseAmount,
                                cCurrencyAmount:=cCurrencyAmount,
                                vConversionDate:=Date.Today())

                    cAmount = cCurrencyAmount
                Else
                    cAmount = cBaseAmount
                End If
                txtunallocatedAmount.Text = g_oBusiness.FormatCurrency(
                            v_lCurrencyID:=UctCurrency.CurrencyId,
                            v_cCurrencyAmount:=cAmount)
            End If
        End If

        If UctCurrency.CurrencyId <> 0 Then
            m_iPreviousCurrencyId = UctCurrency.CurrencyId
        End If
    End Sub
    Private Sub EnableDisableAllocateButton()
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Try
            If m_bMarkedTransaction And txtReciptPaymentAmount.Text = "" And ConvertCurrencyStringToValue(txtMarked.Text) = 0 Then
                cmdAllocate.Enabled = True
            Else
                cmdAllocate.Enabled = False
            End If
        Catch ex As Exception
            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="EnableDisableAllocateButton", r_lFunctionReturn:=result, excep:=ex)
        End Try

    End Sub

    ' ***********************************************************
    ' Mark, unmark, or partmark (partpay) a transaction entry
    ' ***********************************************************
    Private Function MarkInstEntry(ByVal oTransaction As Transaction, ByVal oInstalmentEntry As TransactionInst, Optional ByVal cMarkAmount As Decimal = Nothing, Optional ByVal bIgnoreCurrency As Boolean = Nothing) As Long

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Dim iBaseCurrencyID As Integer
        Dim cMarkBaseAmount As Decimal
        Dim cMarkBaseAmountUnrounded As Decimal

        Try
            ' Check this isn't already appropriately marked
            If (oInstalmentEntry.MarkedAmount <> cMarkAmount) Then
                ' Ensure this transaction is not already marked!

                'If cMarkAmount = 0 Then
                ' Note: This is irrelevent of what we intend to do!
                m_lReturn = g_oBusiness.UnMarkInstTransaction(
                    v_iTransdetailid:=oInstalmentEntry.DetailID,
                    v_iInstalmentNumber:=oInstalmentEntry.InstalmentNumber)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", MarkEntry, Unable to unmark transaction entry:" & oInstalmentEntry.Key)
                End If
                'End If
                ' Set marked amount to zero, just in case the mark then fails
                oInstalmentEntry.MarkedAmount = 0
                oInstalmentEntry.MarkedAccountAmount = 0

                ' Check if we should now mark the item
                If cMarkAmount Then

                    ' Mark this item for the amount specified
                    m_lReturn = g_oBusiness.MarkTransaction(
                        v_lTransactionID:=oInstalmentEntry.DetailID,
                        v_iCurrencyID:=oInstalmentEntry.CurrencyID,
                        v_cPayment:=cMarkAmount,
                        v_iInstalmentNumber:=oInstalmentEntry.InstalmentNumber)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", MarkEntry, Unable to mark transaction entry: " & oInstalmentEntry.Key)
                    End If
                    ' Set new marked amount on object
                    oInstalmentEntry.MarkedAmount = cMarkAmount

                    'Work out the Marked Account Amount
                    If oInstalmentEntry.CurrencyID <> oInstalmentEntry.AccountCurrencyID Then
                        m_lReturn = m_oCurrencyConvert.GetBaseCurrency(
                                            v_lCompanyId:=oInstalmentEntry.CompanyID,
                                            r_iBaseCurrencyID:=iBaseCurrencyID)

                        If oInstalmentEntry.CurrencyID <> iBaseCurrencyID Then
                            m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(
                                            lCurrencyID:=oInstalmentEntry.CurrencyID,
                                            lCompanyID:=oInstalmentEntry.CompanyID,
                                            cBaseAmount:=cMarkBaseAmount,
                                            cCurrencyAmount:=cMarkAmount,
                                            vConversionDate:=Date.Today,
                                            vConversionRate:=oTransaction.CurrencyRate,
                                            vBaseAmountUnRounded:=cMarkBaseAmountUnrounded)
                        Else
                            cMarkBaseAmount = cMarkAmount
                            cMarkBaseAmountUnrounded = cMarkAmount
                        End If

                        If oInstalmentEntry.AccountCurrencyID <> iBaseCurrencyID Then

                            cMarkBaseAmount = cMarkBaseAmountUnrounded

                            m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(
                                            lCurrencyID:=oInstalmentEntry.AccountCurrencyID,
                                            lCompanyID:=oInstalmentEntry.CompanyID,
                                            cBaseAmount:=cMarkBaseAmount,
                                            cCurrencyAmount:=cMarkAmount,
                                            vConversionDate:=Date.Today,
                                            vConversionRate:=oTransaction.AccountCurrencyRate)
                        Else
                            cMarkAmount = cMarkBaseAmount
                        End If
                    End If
                    'Set the Marked Account Amount
                    oInstalmentEntry.MarkedAccountAmount = cMarkAmount

                End If

                ' Refresh the item
                RefreshInstEntry(oInstalmentEntry)
            End If
            m_iPreviousCurrencyId = 0
            Return result
        Catch ex As Exception
            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="MarkInstEntry", r_lFunctionReturn:=result, excep:=ex)
            Return result
        End Try
    End Function

    ' ***********************************************************
    ' Refresh the icons and marked amount for an entry
    ' ***********************************************************
    Private Sub RefreshInstEntry(ByVal oInstalmentEntry As TransactionInst, Optional ByVal oListItem As ListViewItem = Nothing)

        ' Were we passed a listitem?
        If (oListItem Is Nothing) Then
            ' Get the listitem (protect from errors as it may not be currently visible)
            oListItem = lvwInstalmentEntries.Items(oInstalmentEntry.Key)
        End If
        Dim result As Boolean = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Ensure we found an item
            If Not (oListItem Is Nothing) Then
                ' Set icons, parent row will be done by calling function
                Select Case oInstalmentEntry.IsMarked
                    Case TransactionEntry.MarkedStatusEnum.acmseFullyMarked
                        ' Set the icon to checked
                        oListItem.ImageKey = ACIconCheck
                        oListItem.ImageKey = ACIconCheck
                    Case TransactionEntry.MarkedStatusEnum.acmsePartMarked
                        ' Set the icon to part checked
                        oListItem.ImageKey = ACIconPart
                        oListItem.ImageKey = ACIconPart
                    Case Else
                        ' No icons
                        oListItem.ImageKey = ACIconBlank
                        oListItem.ImageKey = ACIconBlank
                End Select

                ' Set new marked amount on display
                If optViewByTransaction.Checked = True Then
                    ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryInstEnum.ACLIMarkedAmount).Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=oInstalmentEntry.CurrencyID, v_cCurrencyAmount:=oInstalmentEntry.MarkedAmount)
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, MainModule.ListViewEntryInstEnum.ACLIMarkedAmount).Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=oInstalmentEntry.AccountCurrencyID, v_cCurrencyAmount:=oInstalmentEntry.MarkedAccountAmount)
                End If
            End If

        Catch
            ' Major problem, data could be seriously out of sync
            Call MsgBox("Unable to refresh transaction detail", vbCritical, Me.Text)

            ' Re-search
            PerformSearch()

        End Try
    End Sub
    ' ***********************************************************
    ' Mark all selected entries
    ' ***********************************************************
    Private Function MarkInstEntries() As Long

        Dim oListItem As ListViewItem
        Dim oTransaction As Transaction
        Dim oInstalmentEntry As TransactionInst
        Dim IsWOFF As Boolean
        Dim result As Integer = 0
        MarkInstEntries = gPMConstants.PMEReturnCode.PMTrue
        IsWOFF = False

        result = gPMConstants.PMEReturnCode.PMTrue
        ' Set the mouse pointer to busy
        Try
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Only process if we have a current transaction
            If Len(lvwInstalmentEntries.Tag) Then
                ' Get the transaction and the entry
                oTransaction = m_cTransactions.Item(lvwInstalmentEntries.Tag)

                ' Is the entry already marked? I
                For Each oListItem In lvwInstalmentEntries.Items
                    ' Check selected
                    If oListItem.Selected Then
                        ' Get entry
                        oInstalmentEntry = oTransaction.ItemInstalment(oListItem.Tag)

                        If oInstalmentEntry.Spare = "WRITEOFF" Then
                            If vbCancel = MsgBox("Unmarking will discard Write-Off Transaction." & vbCrLf & "Do you wish to continue?", vbOKCancel, Me.Text) Then
                                Exit For
                            Else
                                'delete Write Off transaction
                                m_lReturn = g_oBusiness.DeleteWriteOffTransaction(Val(Strings.Right(m_sTransKey, Len(m_sTransKey) - 2)))
                                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", MarkEntry, Unable to unmark transaction entry: " & oInstalmentEntry.Key)
                                End If
                                IsWOFF = True
                            End If

                        Else
                            ' Mark/unmark it based on current state
                            If (oInstalmentEntry.OutstandingAmount <> oInstalmentEntry.MarkedAmount) Then
                                m_lReturn = MarkInstEntry(oTransaction, oInstalmentEntry, oInstalmentEntry.OutstandingAmount)
                            Else
                                m_lReturn = MarkInstEntry(oTransaction, oInstalmentEntry, 0)
                            End If

                            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                ' If the user is cancelling, alter to true but still exit
                                result = gPMConstants.PMEReturnCode.PMFalse
                                Return result
                            End If

                            ' Refresh the entry
                            Call RefreshInstEntry(oInstalmentEntry)
                        End If
                    End If
                Next oListItem
            End If

            ' Update total marked for payment
            RefreshTotalMarked()
            RefreshTotalWriteOff()
            RefreshUnallocatedAmount()
            EnableDisableAllocateButton()
            Return result
        Catch ex As System.Exception
            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="MarkInstEntries", r_lFunctionReturn:=result, excep:=ex)
        Finally
            ' Always refresh the transaction, if we have one!
            If Not (oTransaction Is Nothing) Then
                RefreshTransaction(oTransaction)
            End If

            If IsWOFF = True Then PerformSearch()

            ' Set the mouse pointer back to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End Try
    End Function

    Private Sub RefreshUnallocatedAmount()
        Dim cUnAllocatedAmount As Decimal
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            If Trim(txtReciptPaymentAmount.Text) = "" Then
                cUnAllocatedAmount = -ConvertCurrencyStringToValue(txtMarked.Text)
            Else
                cUnAllocatedAmount = (ConvertCurrencyStringToValue(txtReciptPaymentAmount.Text) - ConvertCurrencyStringToValue(txtMarked.Text))
            End If
            If cUnAllocatedAmount <> 0 Then
                txtunallocatedAmount.Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=UctCurrency.CurrencyId, v_cCurrencyAmount:=cUnAllocatedAmount)
            Else
                txtunallocatedAmount.Text = "0.00"
            End If
        Catch ex As Exception
            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="RefreshUnallocatedAmount", r_lFunctionReturn:=result, excep:=ex)
            ' Show total as error to highlight
            txtunallocatedAmount.Text = "<Error>"
        End Try
    End Sub
    ' ***********************************************************
    ' Validation
    ' ***********************************************************
    Private Function Validation() As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Try
            'Validate
            If ConvertCurrencyStringToValue(txtMarked.Text) < 0 Then
                MsgBox("The Total Marked indicates that a payment is required,however a Recipt Amount has been entered.Please clear the Recipt Amount or change the marked transactions so that the Total Marked is a positive value.", vbCritical, Me.Text)
                result = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            If ConvertCurrencyStringToValue(txtMarked.Text) > ConvertCurrencyStringToValue(txtReciptPaymentAmount.Text) Then
                MsgBox("The Total Marked exceeds the value entered in respect of Recipt Amount,Please revise the Recipt Amount or change the marked transactions so that the Total Marked is less than the Recipt Amount.", vbCritical, Me.Text)
                result = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
        Catch ex As Exception
            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="Validation", r_lFunctionReturn:=result, excep:=ex)
        End Try
        Return result
    End Function

    Private Function MarkPartPaymentInst(ByVal bTotalPayment As Boolean) As Long

        Dim iSign As Integer
        Dim oListItem As ListViewItem
        Dim oTransaction As Transaction
        Dim oEntryInstalment As TransactionInst
        Dim result As Integer = 0
        Dim sPayment As String = ""
        Dim iBaseCurrencyID As Integer
        Dim cPayment As Decimal
        Dim cBasePayment As Decimal
        Dim dPercentage As Double
        Dim cOutstandingAmount As Decimal
        Dim sTemp As String
        Const kMethodName As String = "MarkPartPaymentInst"

        result = gPMConstants.PMEReturnCode.PMTrue

        Try
            ' Set the mouse pointer to busy
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            dPercentage = 1
            ' Get the transaction
            oTransaction = m_cTransactions.Item(lvwInstalmentEntries.Tag)

            ' Check for any marked entries
            For Each oListItem In lvwInstalmentEntries.Items
                ' Check for selected
                If (oListItem.Selected) Then
                    ' Get transaction entry

                    'Set oEntryInstalment = oTransaction.item(oListItem.Key)
                    oEntryInstalment = oTransaction.ItemInstalment(oListItem.Tag)

                    If oEntryInstalment.Spare <> "WRITEOFF" Then
                        If optViewByTransaction.Checked = True Then
                            sPayment = oEntryInstalment.OutstandingAmount
                            cOutstandingAmount = oEntryInstalment.OutstandingAmount
                        Else
                            sPayment = oEntryInstalment.OutstandingAccountAmount
                            cOutstandingAmount = oEntryInstalment.OutstandingAccountAmount
                        End If

                        iSign = IIf(sPayment < 0, -1, 1)
                        sPayment = InputBox("Partial Offset Amount:", "Partial Offset", gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, Math.Abs(CDbl(sPayment))))

                        ' Check return
                        If IsNumeric(sPayment) Then
                            If InStr(1, sPayment, ".") > 0 Then
                                sTemp = Mid(sPayment, 1, InStr(1, sPayment, ".") - 1)
                            Else
                                sTemp = sPayment
                            End If
                            If Len(sTemp) > 12 Then
                                Call MsgBox("Value specified must be between the defaulted O/S amount and zero.", vbExclamation, Me.Text)
                                m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                Exit Function
                            End If
                            cPayment = Math.Round(CDbl(sPayment) * iSign * dPercentage, 2)

                            'Offset Amount must be between zero and the Outstandinf Amount
                            If iSign = -1 Then
                                If cPayment > 0 Or cPayment < cOutstandingAmount Then
                                    Call MsgBox("Value specified must be between the defaulted O/S amount and zero.", vbExclamation, Me.Text)
                                    m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                    Exit Function
                                End If
                            Else
                                If cPayment < 0 Or cPayment > cOutstandingAmount Then
                                    Call MsgBox("Value specified must be between the defaulted O/S amount and zero.", vbExclamation, Me.Text)
                                    m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                    Exit Function
                                End If
                            End If

                            If optViewByTransaction.Checked = False Then
                                If oEntryInstalment.CurrencyID <> oEntryInstalment.AccountCurrencyID Then
                                    m_lReturn = m_oCurrencyConvert.GetBaseCurrency(
                                                        v_lCompanyId:=oEntryInstalment.CompanyID,
                                                        r_iBaseCurrencyID:=iBaseCurrencyID)

                                    If oEntryInstalment.AccountCurrencyID <> iBaseCurrencyID Then
                                        m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(
                                                        lCurrencyID:=oEntryInstalment.AccountCurrencyID,
                                                        lCompanyID:=oEntryInstalment.CompanyID,
                                                        cBaseAmount:=cBasePayment,
                                                        cCurrencyAmount:=cPayment,
                                                        vConversionDate:=Date.Today)
                                    Else
                                        cBasePayment = cPayment
                                    End If

                                    If oEntryInstalment.CurrencyID <> iBaseCurrencyID Then
                                        m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(
                                                        lCurrencyID:=oEntryInstalment.CurrencyID,
                                                        lCompanyID:=oEntryInstalment.CompanyID,
                                                        cBaseAmount:=cBasePayment,
                                                        cCurrencyAmount:=cPayment,
                                                        vConversionDate:=Date.Today)
                                    Else
                                        cPayment = cBasePayment
                                    End If
                                End If
                            End If
                            ' Mark the transaction at the supplied amount
                            m_lReturn = MarkInstEntry(oTransaction, oEntryInstalment, cPayment)
                        Else
                            ' Warn the user, return true though as we are warning the user here!!
                            Call MsgBox("Please enter a valid currency amount", vbExclamation, Me.Text)
                            m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                        End If

                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            MarkPartPaymentInst(m_lReturn)
                            Return m_lReturn
                        End If
                    Else
                        If Not bTotalPayment Then
                            MsgBox("Cannot Part Pay a write off transaction", vbCritical, Me.Text)
                        End If
                    End If
                End If
            Next oListItem

            m_iPreviousCurrencyId = 0
            ' Update total marked for payment
            RefreshTotalMarked()
            RefreshTotalWriteOff()
            RefreshUnallocatedAmount()
            Return result
        Catch ex As System.Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            Return result
        Finally
            ' Always refresh the transaction, if we have one!
            If Not (oTransaction Is Nothing) Then
                Call RefreshTransaction(oTransaction)
            End If

            ' Set the mouse pointer back to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End Try
        Return result
    End Function

    ''' <summary>
    ''' Allocate
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Allocate() As Integer

        Const kMethodName As String = "Allocate"

        Dim oInstalmentEntry As TransactionInst
        Dim oEntry As TransactionEntry
        Dim result As Integer = 0
        Dim vMatchTransAlloc() As Object
        Dim iCount, iMainRow, iMatchRow As Integer
        Dim nFailedPosting As Integer
        Dim oTransaction As Transaction
        Dim oPFInstalmentArray() As Object
        Dim nVar As Integer
        Dim oPFSIRInstalments As Object
        Dim oMatchTransAlloc() As Object
        Dim sParams As String
        Dim oResultArray(,) As Object
        Dim crJNAmount As Decimal
        Dim lnNTransDetailID_Bank As Integer
        Dim nJNTransDetailID_Party As Integer
        Dim oMatchTrans As Object
        Dim oKeys As Object
        Dim nMainRow As Integer
        Dim nMatchRow As Integer
        Dim nCount As Integer
        Dim iCountInst As Integer
        Dim bInstalment As Boolean
        Dim nCompanyID As Integer
        Dim nBaseCurrencyID As Integer
        Dim nResult As Integer = 0
        Dim bIsDebitPolicyTrans As Boolean
        Dim bIsCreditPolicyTrans As Boolean
        Dim nCashListCount As Integer
        Dim bIsSingleCashListItemAllocation As Boolean
        Dim bIsOtherTransaction As Boolean
        Dim crPayAmount As Decimal
        Dim bHasCashTransaction As Boolean
        Dim sOptionValue As String
        Dim bMessageDisplayed As Boolean = False
        nResult = PMEReturnCode.PMTrue

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseBusy)
            cmdAllocate.Enabled = False

            'Get Account Object.
            m_lReturn = g_oObjectManager.GetInstance(oObject:=oPFSIRInstalments,
                    sClassName:="bSIRPFInstalments.Business",
                    vInstanceManager:=PMGetViaClientManager)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                nResult = PMEReturnCode.PMFalse
                Return nResult
            End If

            crPayAmount = 0

            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=kSysOptSingleCashReceipt, r_sOptionValue:=sOptionValue)
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Throw New Exception("GetSystemOption Single Cash Reciept/Payment per Allocation Check Failed")
            End If

            If sOptionValue = "1" Then
                bIsSingleCashListItemAllocation = True
            Else
                bIsSingleCashListItemAllocation = False
            End If

            ' Build an array of marked transdetails (for instalment entries i.e IND,IED,IRD
            For Each oTransaction In m_cTransactions

                If oTransaction.TotalMarked <> 0 Then
                    If Mid(Trim$(oTransaction.DocumentRef), 1, 3) = "SND" _
                       Or Mid(Trim$(oTransaction.DocumentRef), 1, 3) = "SED" _
                       Or Mid(Trim$(oTransaction.DocumentRef), 1, 3) = "SID" _
                       Or Mid(Trim$(oTransaction.DocumentRef), 1, 3) = "SRD" Then
                        bIsDebitPolicyTrans = True
                    ElseIf Mid(Trim$(oTransaction.DocumentRef), 1, 3) = "SEC" _
                       Or Mid(Trim$(oTransaction.DocumentRef), 1, 3) = "SRC" _
                       Or Mid(Trim$(oTransaction.DocumentRef), 1, 3) = "SNC" Then
                        bIsCreditPolicyTrans = True
                    ElseIf Mid(Trim$(oTransaction.DocumentRef), 1, 3) = "SRP" _
                       Or Mid(Trim$(oTransaction.DocumentRef), 1, 3) = "SPY" Then
                        bHasCashTransaction = True
                        nCashListCount = nCashListCount + 1
                    Else
                        bIsOtherTransaction = True
                    End If
                End If


                If ValidSource(vSource:=oTransaction.CompanyID) = True Then
                    If Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IND" And
                        Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IED" And
                        Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IRD" Then

                        For Each oEntry In oTransaction
                            ' Check status
                            If oEntry.MarkedAmount <> 0 Then

                                ' Resize the array and store detail id
                                'ivar = 0
                                ReDim Preserve oMatchTransAlloc(nCount)

                                'Convert the currency into base Currency.
                                Dim crBaseCurrencyAmount As Double
                                m_lReturn = ConvertAmountIntoBaseCurrency(oTransaction, oEntry, crBaseCurrencyAmount, ToSafeString(oEntry.MarkedAccountAmount, "0"))

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    RaiseError("Allocate", "ConvertAmountIntoBaseCurrency failed.")
                                    Return m_lReturn
                                End If

                                If oEntry.MarkedAmount < 0 Then
                                    oMatchTransAlloc(nCount) = ToSafeString(oEntry.DetailID) & "|" & ToSafeString(crBaseCurrencyAmount, "0")
                                    iMainRow = nCount
                                Else
                                    oMatchTransAlloc(nCount) = ToSafeString(oEntry.DetailID) & "|" & ToSafeString(crBaseCurrencyAmount, "0")
                                End If
                                nCount = nCount + 1
                            End If
                            m_lReturn = g_oBusiness.UnMarkTransaction(v_lTransDetailId:=oEntry.DetailID)
                            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                                result = m_lReturn
                            End If
                        Next
                    Else

                        If MessageBox.Show("Are you sure you want to allocate these transactions together?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                            nResult = PMEReturnCode.PMCancel
                            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
                            Return nResult
                        End If
                        bMessageDisplayed = True

                        ' Check each entry
                        For Each oInstalmentEntry In oTransaction
                            ' Check status
                            If oInstalmentEntry.MarkedAmount <> 0 Then

                                crPayAmount = crPayAmount + oEntry.MarkedAmount

                                ReDim Preserve oPFInstalmentArray(iCountInst)
                                nCompanyID = oInstalmentEntry.CompanyID
                                If oInstalmentEntry.MarkedAmount < oInstalmentEntry.OutstandingAmount Then
                                    m_lReturn = oPFSIRInstalments.PostIndividualInstalment(v_cAmount:=oInstalmentEntry.OutstandingAmount,
                                                v_lPlanTransDetailID:=oInstalmentEntry.DetailID,
                                                v_lInstalmentId:=oInstalmentEntry.PFInstalmentsId,
                                                r_lFailedPosting:=nFailedPosting,
                                                v_cPartialAmount:=oInstalmentEntry.MarkedAccountAmount)

                                Else
                                    m_lReturn = oPFSIRInstalments.PostIndividualInstalment(v_cAmount:=oInstalmentEntry.MarkedAccountAmount,
                                                    v_lPlanTransDetailID:=oInstalmentEntry.DetailID,
                                                    v_lInstalmentId:=oInstalmentEntry.PFInstalmentsId,
                                                    r_lFailedPosting:=nFailedPosting)
                                End If
                                'v_cWriteOffAmount
                                oPFInstalmentArray(iCountInst) = oInstalmentEntry.PFInstalmentsId
                                iCountInst = iCountInst + 1

                                m_lReturn = g_oBusiness.DeleteTransMatchInst(
                                    v_iTransdetailid:=oInstalmentEntry.DetailID)
                                If (m_lReturn <> PMEReturnCode.PMTrue) And (m_lReturn <> PMEReturnCode.PMNotFound) Then
                                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessPay, Unable to delete transmatch")
                                End If
                                bInstalment = True
                            End If
                        Next oInstalmentEntry
                    End If
                End If
            Next oTransaction

            If ((bIsDebitPolicyTrans = True OrElse bIsCreditPolicyTrans = True OrElse bIsOtherTransaction = True) AndAlso
              ((nCashListCount = 1 AndAlso crPayAmount <> 0) OrElse (nCashListCount > 1))) AndAlso
          bIsSingleCashListItemAllocation = True Then
                If nCashListCount > 1 Then
                    MessageBox.Show("You can only allocate 1SRP OR 1SPY in a single allocation with other transaction types", "Insurer Payment", MessageBoxButtons.OK)
                ElseIf nCashListCount = 1 AndAlso crPayAmount > 0 Then
                    MessageBox.Show("This payment would result in as SRP being generated and there is already an SPY or SRP in the list and you can't allocate more than one in a single Allocation", "Insurer Payment", MessageBoxButtons.OK)
                ElseIf nCashListCount = 1 AndAlso crPayAmount < 0 Then
                    MessageBox.Show("This payment would result in as SPY being generated and there is already an SPR or SPY in the list and you can't allocate more than one in a single Allocation", "Insurer Payment", MessageBoxButtons.OK)
                End If
                nResult = PMEReturnCode.PMCancel
                iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
                Return nResult
            End If

            If bInstalment Then
                If IsArray(oPFInstalmentArray) Then
                    For nVar = 0 To UBound(oPFInstalmentArray)
                        sParams = sParams + ToSafeString(oPFInstalmentArray(nVar)) + ","
                    Next

                    sParams = Strings.Left(sParams, Len(sParams) - 1)
                    'Call business
                    m_lReturn = g_oBusiness.GetTranDetailContraEntriesForInstalments(v_sParam:=sParams, vResultArray:=oResultArray)

                    If (m_lReturn <> PMEReturnCode.PMTrue) Then
                        RaiseError("Allocate", "GetTranDetailContraEntriesForInstalments failed.")
                    End If
                End If
            End If
            If IsArray(oResultArray) And bInstalment Then
                For nVar = 0 To UBound(oResultArray, 2)
                    crJNAmount = crJNAmount + ToSafeCurrency(oResultArray(1, nVar))
                Next
                m_lReturn = m_oCurrencyConvert.GetBaseCurrency(
                                v_lCompanyId:=nCompanyID,
                                r_iBaseCurrencyID:=nBaseCurrencyID)

                m_lReturn = CreateJournal(v_iBankAccountID:=oResultArray(2, 0),
                                        v_iPartyAccountID:=m_lAccountId,
                                        v_cAmount:=crJNAmount,
                                        r_iJNTransDetailID_Bank:=lnNTransDetailID_Bank,
                                        r_iJNTransDetailID_Party:=nJNTransDetailID_Party,
                                        v_iCurrencyId:=nBaseCurrencyID)

                If (m_lReturn <> PMEReturnCode.PMTrue) Then
                    RaiseError("Allocate", "CreateJournal failed.")
                End If

                'Do Allocation
                ReDim oKeys(0 To 1, 0 To 3)

                'AllocationTransID
                oKeys(PMENavLetGetKeyColPosition.PMKeyName, 1) = ACTKeyNameTransDetailID
                oKeys(PMENavLetGetKeyColPosition.PMKeyValue, 1) = lnNTransDetailID_Bank & "|" & (Math.Abs(crJNAmount) * Math.Sign(SharedFiles.gACTLibrary.ACTEAccountSign.acteSignCredit))

                ReDim oMatchTrans(UBound(oResultArray, 2))

                For nVar = 0 To UBound(oResultArray, 2)
                    oMatchTrans(nVar) = oResultArray(0, nVar) & "|" & ToSafeString(Math.Abs(oResultArray(1, nVar)) * Math.Sign(SharedFiles.gACTLibrary.ACTEAccountSign.acteSignDebit), "0")
                Next nVar

                ' AccountID
                oKeys(PMENavLetGetKeyColPosition.PMKeyName, 0) = ACTKeyNameAccountID
                oKeys(PMENavLetGetKeyColPosition.PMKeyValue, 0) = oResultArray(2, 0)

                oKeys(PMENavLetGetKeyColPosition.PMKeyName, 2) = ACTKeyNameTransDetailIDs
                oKeys(PMENavLetGetKeyColPosition.PMKeyValue, 2) = oMatchTrans

                m_lReturn = AllocateTransaction(oKeys)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    RaiseError("Allocate", "AllocateTransaction failed.")
                End If
                'Do Allocation
                ReDim oKeys(0 To 1, 0 To 3)
                ReDim oMatchTrans(UBound(oMatchTransAlloc))
                nMatchRow = 0
                For nVar = 0 To UBound(oMatchTransAlloc)
                    If nVar = nMainRow Then
                        'AllocationTransID
                        oKeys(PMENavLetGetKeyColPosition.PMKeyName, 1) = ACTKeyNameTransDetailID
                        oKeys(PMENavLetGetKeyColPosition.PMKeyValue, 1) = oMatchTransAlloc(nVar)
                    Else
                        oMatchTrans(nMatchRow) = oMatchTransAlloc(nVar)
                        nMatchRow = nMatchRow + 1
                    End If
                Next nVar

                oMatchTrans(nMatchRow) = ToSafeString(nJNTransDetailID_Party) & "|" & ToSafeString(Math.Abs(crJNAmount))

                ' AccountID
                oKeys(PMENavLetGetKeyColPosition.PMKeyName, 0) = ACTKeyNameAccountID
                oKeys(PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lAccountId

                oKeys(PMENavLetGetKeyColPosition.PMKeyName, 2) = ACTKeyNameTransDetailIDs
                oKeys(PMENavLetGetKeyColPosition.PMKeyValue, 2) = oMatchTrans

                m_lReturn = AllocateTransaction(oKeys)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    RaiseError("Allocate", "AllocateTransaction failed.")
                End If
            Else
                'Do Allocation
                If (IsNothing(oMatchTrans) = False Or IsNothing(oMatchTransAlloc) = False) Then
                    ReDim oKeys(0 To 1, 0 To 3)
                    ReDim oMatchTrans(UBound(oMatchTransAlloc) - 1)
                    nMatchRow = 0
                    For nVar = 0 To UBound(oMatchTransAlloc)
                        If nVar = nMainRow Then
                            'AllocationTransID
                            oKeys(PMENavLetGetKeyColPosition.PMKeyName, 1) = ACTKeyNameTransDetailID
                            oKeys(PMENavLetGetKeyColPosition.PMKeyValue, 1) = oMatchTransAlloc(nVar)
                        Else
                            oMatchTrans(nMatchRow) = oMatchTransAlloc(nVar)
                            nMatchRow = nMatchRow + 1
                        End If
                    Next nVar
                    ' AccountID
                    oKeys(PMENavLetGetKeyColPosition.PMKeyName, 0) = ACTKeyNameAccountID
                    oKeys(PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lAccountId

                    oKeys(PMENavLetGetKeyColPosition.PMKeyName, 2) = ACTKeyNameTransDetailIDs
                    oKeys(PMENavLetGetKeyColPosition.PMKeyValue, 2) = oMatchTrans

                    m_lReturn = AllocateTransaction(oKeys)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        RaiseError("Allocate", "AllocateTransaction failed.")
                    End If

                End If

            End If
        Catch ex As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
            nResult = PMEReturnCode.PMFalse
        Finally
            ' Mouse pointer back to normal
            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
            ' Terminate
            oPFSIRInstalments.Dispose()
            oPFSIRInstalments = Nothing
        End Try
        Return nResult
    End Function
    ''' <summary>
    ''' Convert to base currency so that we can pass the amount when the Base currency and transaction currency are different.
    ''' </summary>
    ''' <param name="r_crBaseAmount"></param>
    ''' <param name="crCurrencyAmount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConvertAmountIntoBaseCurrency(ByVal oTransaction As Transaction,
                                                   ByVal oEntry As TransactionEntry,
                                                   ByRef r_crBaseAmount As Double,
                                                   ByVal crCurrencyAmount As Double
                                                   ) As Integer

        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMFalse
        Try
            If optViewByAccount.Checked = True Then
                r_crBaseAmount = crCurrencyAmount * oTransaction.AccountCurrencyRate
            ElseIf optViewByTransaction.Checked = True Then
                r_crBaseAmount = crCurrencyAmount * oTransaction.CurrencyRate
            End If
            nReturn = gPMConstants.PMEReturnCode.PMTrue
        Catch ex As ApplicationException
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="ConvertAmountIntobaseCurrency", r_lFunctionReturn:=nReturn, excep:=ex)
            Return nReturn
        End Try
        Return nReturn
    End Function
    Private Function AllocateTransaction(ByVal vKeys As Object) As Integer

        Const kMethodName As String = "AllocateTransaction"
        Dim oAllocationManual As Object
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            'Do Allocation

            m_lReturn = g_oObjectManager.GetInstance(oObject:=oAllocationManual,
                    sClassName:="bACTAllocationManual.Business",
                    vInstanceManager:=PMGetViaClientManager)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("AllocateTransaction", "bACTAllocationManual.GetInstance failed.")
            End If


            m_lReturn = oAllocationManual.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError("AllocateTransaction", "oAllocationManual.SetProcessModes failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set the keys
            m_lReturn = oAllocationManual.SetKeys(vKeyArray:=vKeys)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError("AllocateTransaction", "oAllocationManual.SetKeys failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Start it
            oAllocationManual.CompanyID = g_iSourceID
            m_lReturn = oAllocationManual.Start()
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError("AllocateTransaction", "oAllocationManual.Start failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As ApplicationException
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            Return result
        End Try

        ' Terminate
        oAllocationManual.Dispose()
        oAllocationManual = Nothing
        Return result
    End Function
    Private Function CreateJournal(ByVal v_iBankAccountID As Integer,
                                   ByVal v_iPartyAccountID As Integer,
                                    ByVal v_cAmount As Decimal,
                                    ByRef r_iJNTransDetailID_Bank As Integer,
                                    ByRef r_iJNTransDetailID_Party As Integer,
                                    ByVal v_iCurrencyId As Integer) As Integer


        Const kMethodName As String = "CreateJournal"
        Dim oAutoNumber As Object
        Dim oDocumentPost As Object

        Dim lNumberRangeId As Integer
        Dim lNumber As Integer
        Dim sDocumentRef As String
        'Const ReleasedDocumentTypeId = 1
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Add parameters as per new Standards
            m_lReturn = g_oObjectManager.GetInstance(oObject:=oAutoNumber,
                    sClassName:="bACTAutoNumber.Business",
                    vInstanceManager:=PMGetViaClientManager)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("CreateJournal", "GetInstance failed.")
            End If
            m_lReturn = g_oObjectManager.GetInstance(oObject:=oDocumentPost,
                    sClassName:="bACTDocumentPost.Form",
                    vInstanceManager:=PMGetViaClientManager)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("CreateJournal", "GetInstance failed.")
            End If


            'Get the number range
            m_lReturn = oAutoNumber.GetNumberRange(
                v_sGroupCode:=ACTConst.ACTAutoNumberGroupCodeDocumentRef1,
                v_sRangeCode:=ACTConst.ACTAutoNumberRangeCodeJn,
                r_lNumberRangeID:=lNumberRangeId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("m_oAutoNumber.GetNumberRange", "v_sRangeCode:=" & ACTConst.ACTAutoNumberRangeCodeJn, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Generate the next number
            m_lReturn = oAutoNumber.GenerateNumber(
                v_lNumberRangeID:=lNumberRangeId,
                v_iUserID:=g_iUserID,
                v_iCompanyid:=g_iSourceID,
                r_lNumber:=lNumber)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("m_oAutoNumber.GenerateNumber", "v_lNumberRangeID:=" & lNumberRangeId, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Format the number
            sDocumentRef = gPMFunctions.FormatField("00000000", lNumber)
            'Generate document
            m_lReturn = oDocumentPost.AddDocument(
                v_lDocumentTypeID:=ACTConst.ACTDocTypeJournal,
                v_sDocumentRef:=sDocumentRef,
                v_dtDocumentDate:=Date.Today,
                v_sComment:="",
                r_vDocSourceID:=g_iSourceID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("oDocumentPost.AddDocument", "v_sDocumentRef:=" & sDocumentRef, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = oDocumentPost.AddTransaction(
                v_lAccountID:=v_iPartyAccountID,
                v_iCurrencyId:=v_iCurrencyId,
                v_cAmount:=v_cAmount,
                v_cCurrencyAmount:=v_cAmount,
                v_vdCurrencyBaseXRate:=1,
                r_vTransDetailId:=r_iJNTransDetailID_Party,
                v_vDocumentSequence:=1,
                v_vComment:="",
                v_vAccountingDate:=Now)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("oDocumentPost.AddTransaction", "")
            End If

            m_lReturn = oDocumentPost.AddTransaction(
                v_lAccountID:=v_iBankAccountID,
                v_iCurrencyId:=v_iCurrencyId,
                v_cAmount:=v_cAmount * -1,
                v_cCurrencyAmount:=v_cAmount * -1,
                v_vdCurrencyBaseXRate:=1,
                r_vTransDetailId:=r_iJNTransDetailID_Bank,
                v_vDocumentSequence:=2,
                v_vComment:="",
                v_vAccountingDate:=Now)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("oDocumentPost.AddTransaction", " AddTransaction failed.")
            End If
        Catch ex As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            Return result
        End Try
        oAutoNumber.Dispose()
        oDocumentPost.Dispose()
        oAutoNumber = Nothing
        oDocumentPost = Nothing
        Return result
    End Function


    Private Sub lvwInstalmentEntries_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwInstalmentEntries.ColumnClick
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' If new column reset sort order
            Dim ColumnHeader As ColumnHeader = lvwEntries.Columns(e.Column)

            If ListViewHelper.GetSortKeyProperty(lvwInstalmentEntries) <> ColumnHeader.Index Then
                ListViewHelper.SetSortOrderProperty(lvwInstalmentEntries, SortOrder.Descending)
            End If

            ' If current sort column header is pressed.
            Select Case ColumnHeader.Index - 1
                Case MainModule.ListViewEntryInstEnum.ACLIAltRef, MainModule.ListViewEntryInstEnum.ACLIDocumentRef, MainModule.ListViewEntryInstEnum.ACLISpare
                    ' String columns
                    ListViewHelper.SetSortedProperty(lvwTransactions, False)
                    ListViewHelper.SetSortKeyProperty(lvwTransactions, ColumnHeader.Index)
                    ListViewHelper.SetSortOrderProperty(lvwTransactions, SortOrder.Descending)
                    ListViewHelper.SetSortedProperty(lvwTransactions, True)
                    ''lvwTransactions.SortOrder = ((lvwInstalmentEntries.SortOrder + 1) Mod 2)

                Case MainModule.ListViewEntryInstEnum.ACLICurrencyAmount, MainModule.ListViewEntryInstEnum.ACLIPaidAmount, MainModule.ListViewEntryInstEnum.ACLINetAmount, MainModule.ListViewEntryInstEnum.ACLIMarkedAmount, MainModule.ListViewEntryInstEnum.ACLIInstalmentNumber
                    ' Currency columns
                    m_lReturn = ListView6Func.ListViewSortByValue(v_oListView:=lvwInstalmentEntries, v_iSourceColumn:=ColumnHeader.Index - 1, v_iDirection:=SortOrder.Ascending,
                        v_bMarkSortedColumn:=True,
                        v_bIsCurrency:=True)
            End Select
        Catch ex As System.Exception
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="lvwInstalmentEntries_ColumnClick", r_lFunctionReturn:=result, excep:=ex)
        End Try

    End Sub

    Private Sub txtReciptPaymentAmount_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtReciptPaymentAmount.Leave
        If ConvertCurrencyStringToValue(txtReciptPaymentAmount.Text) < 0 Then
            Call MsgBox("Recipt amount can't be negative.", MsgBoxStyle.Information, Me.Text)
            txtReciptPaymentAmount.Focus()
        Else
            If txtReciptPaymentAmount.Text = "" Then
                m_bReciptAmountEntered = False
                m_bReciptOrPay = False
                cmdPay.Text = "Pay"
            Else
                m_cReciptAmount = ConvertCurrencyStringToValue(txtReciptPaymentAmount.Text)
                If UctCurrency.CurrencyId = 0 Then
                    UctCurrency.CurrencyId = m_iMarkedcurrencyId
                End If
                If ConvertCurrencyStringToValue(txtReciptPaymentAmount.Text) <> ConvertCurrencyStringToValue(txtMarked.Text) Then
                    m_bReciptAmountEntered = True
                Else
                    m_bReciptAmountEntered = False
                End If

                If UctCurrency.CurrencyId <> 0 Then

                    txtReciptPaymentAmount.Text = g_oBusiness.FormatCurrency(v_lCurrencyID:=UctCurrency.CurrencyId, v_cCurrencyAmount:=m_cReciptAmount)
                End If

                m_bReciptOrPay = True
                cmdPay.Text = "Receipt"
            End If
            RefreshUnallocatedAmount()
            'PerformSearch
        End If
        'RefreshUnallocatedAmount
    End Sub
    Private Sub m_oNavigatorXM_NavigatorClose()
        ' Set complete
        m_bNavCompleted = True
    End Sub

    Private Sub m_oNavigatorXM_SetProcessStatus(ByVal v_bProcessComplete As Boolean)
        ' Store the result
        m_bProcessComplete = v_bProcessComplete
    End Sub

    Private Sub txtunallocatedAmount_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtunallocatedAmount.KeyPress
        Dim KeyAscii As Short = Asc(e.KeyChar)
        If KeyAscii >= 48 And KeyAscii <= 57 Then
            If ConvertCurrencyStringToValue(txtunallocatedAmount.Text) * 10 > 9999999999.0# Then
                KeyAscii = 0
            End If
        End If
        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
    End Sub


    Private Sub optViewByTransaction_CheckedChanged(sender As Object, e As EventArgs) Handles optViewByTransaction.CheckedChanged
        If optViewByTransaction.Checked Then
            uctTransactionCurrency.Enabled = True
        Else
            uctTransactionCurrency.Enabled = False
        End If
    End Sub

    Private Sub optViewByAccount_CheckedChanged(sender As Object, e As EventArgs) Handles optViewByAccount.CheckedChanged
        If optViewByAccount.Checked Then
            uctTransactionCurrency.Enabled = False
        Else
            uctTransactionCurrency.Enabled = True
        End If
    End Sub


    Private Sub lvwTransactions_KeyPress(sender As Object, e As KeyPressEventArgs) Handles lvwTransactions.KeyPress
        lvwTransactions.SelectedItems.Clear()
        Dim i As Integer
        For i = 1 To lvwTransactions.Items.Count - 1
            If (lvwTransactions.Items(i).SubItems(2).Text.ToString <> "") Then
                If UCase(lvwTransactions.Items(i).SubItems(2).Text.ToString.Substring(0, 1)) = UCase(e.KeyChar) Then
                    lvwTransactions.Items(i).EnsureVisible()
                    lvwTransactions.Items(i).Selected = True
                    Exit For
                End If
            End If
        Next
    End Sub
    Private Function UpdateWriteOffDocumentRef(ByVal v_lBatchID As Long) As Long

        Const kMethodName As String = "UpdateWriteOffDocumentRef"
        UpdateWriteOffDocumentRef = gPMConstants.PMEReturnCode.PMTrue

        Try
            Dim vResultArray(,) As Object
            Dim nCount As Integer
            Dim sDocumentRef As String
            Dim nNumberRangeID As Integer
            Dim nNumber As Integer
            Dim sRangeCode As String
            Dim nDocumentType As Integer
            Dim nDocumentID As Integer
            Dim sGroupCode As String
            Dim nCreditOrDebit As Integer
            Dim oPMAutoNumber As Object
            Dim oDocumentPost As Object

            m_lReturn = g_oObjectManager.GetInstance(
                    oObject:=oPMAutoNumber,
                    sClassName:="bACTAutoNumber.Business",
                    vInstanceManager:=PMGetViaClientManager)

            m_lReturn = g_oObjectManager.GetInstance(
                    oObject:=oDocumentPost,
                    sClassName:="bACTDocumentPost.Form",
                    vInstanceManager:=PMGetViaClientManager)

            m_lReturn = g_oBusiness.GetTransDetailsFromBatch(v_lBatchID:=v_lBatchID, r_vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetTransactionsFromBatchFailed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If IsArray(vResultArray) Then
                For nCount = 0 To UBound(vResultArray, 2)
                    ' Create the autonumber object using component services

                    nDocumentType = gACTLibrary.ACTDocTypeWriteOff
                    nCreditOrDebit = SharedFiles.gACTLibrary.ACTEAccountSign.acteSignCredit
                    sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSwd
                    sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef14

                    ' Get the number range
                    m_lReturn = oPMAutoNumber.GetNumberRange(
                            v_sGroupCode:=sGroupCode,
                            v_sRangeCode:=sRangeCode,
                            r_lNumberRangeID:=nNumberRangeID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "GetNumberRange Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ''Generate the next number
                    m_lReturn = oPMAutoNumber.GenerateNumber(
                            v_lNumberRangeID:=nNumberRangeID,
                            v_iUserID:=g_iUserID,
                            v_iCompanyID:=g_iSourceID,
                            r_lNumber:=nNumber)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "GenerateNumber Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' Format the number
                    sDocumentRef = Microsoft.VisualBasic.Format(nNumber, "00000000")
                    sDocumentRef = sRangeCode & sDocumentRef


                    m_lReturn = oDocumentPost.AddDocument(
                            v_lDocumentTypeId:=nDocumentType,
                            v_sDocumentRef:=sDocumentRef,
                            v_dtDocumentDate:=Now,
                            v_sComment:="WRITEOFF",
                            r_vDocumentId:=nDocumentID,
                            r_vDocSourceID:=g_iSourceID)

                    m_lReturn = g_oBusiness.UpdateWriteOffDocumentRef(v_lOldDocumentId:=vResultArray(1, nCount),
                                                                       v_lNewDocumentId:=nDocumentID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "UpdateWriteOffDocumentRef Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Next
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=UpdateWriteOffDocumentRef, excep:=ex)
        End Try

    End Function

    Private Sub cmdUnMarkAll_Click(sender As Object, e As EventArgs) Handles cmdUnMarkAll.Click
        If Not (m_cTransactions Is Nothing) Then
            ' Delegate
            m_lReturn = UnMarkAll()
            ' If this failed warn the user
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("An error occured unmarking all transactions", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            EnableDisableAllocateButton()
        End If
    End Sub
End Class



