Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Drawing
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Private m_iTask As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	Private m_sStepStatus As String = ""
	
	' Stores the return value for the a function call.
	Private m_lReturn As Integer

    'policy version list
    Private Const ACFieldPVInsuranceFileCnt As Integer = 0
    Private Const ACFieldPVInsuranceRef As Integer = 1
    Private Const ACFieldPVDocumentID As Integer = 2
    Private Const ACFieldPVDocumentRef As Integer = 3
    Private Const ACFieldPVSpare As Integer = 4
    Private Const ACFieldPVCommAmount As Integer = 5
    Private Const ACFieldPVTransTypeID As Integer = 6
    Private Const ACFieldPVTransTypeDesc As Integer = 7
    Private Const ACFieldPVTransDate As Integer = 8

    'Incorrect Commision List
    Private Const ACFieldCMInsuranceFileCnt As Integer = 0
    Private Const ACFieldCMInsuranceRef As Integer = 1
    Private Const ACFieldCMPremFinanceCnt As Integer = 2
    Private Const ACFieldCMPremFinanceVersion As Integer = 3
    Private Const ACFieldCMAccountID As Integer = 4
    Private Const ACFieldCMInstalmentID As Integer = 5
    Private Const ACFieldCMInstalmentAmt As Integer = 6
    Private Const ACFieldCMAmountToFinance As Integer = 7
    Private Const ACFieldCMTotalCommission As Integer = 8
    Private Const ACFieldCMPaidCommission As Integer = 9
    Private Const ACFieldCMCorrectCommission As Integer = 10
    Private Const ACFieldCMCorrectionAmt As Integer = 11
    Private Const ACFieldCMTransDate As Integer = 12


    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    Private m_lWidth As Integer
	Private m_lHeight As Integer
    Private bIsSucesfullyCompleted As Boolean
   Private m_bisFirstSelect As Boolean = False
	
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			
			' Return any error number that might have
			' occurred on the interface.
			Return m_lErrorNumber
			
		End Get
	End Property
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			
			' Set the calling application name.
			m_sCallingAppName = Value
			
		End Set
	End Property
	
	
	'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub Status(ByVal Value As Integer)
		'
		' Set the interface exit status.
		'm_lStatus = Value
		'
	'End Sub
	Public ReadOnly Property Status() As Integer
		Get
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
	End Property
	
	Public Property Task() As Integer
		Get
			
			Return m_iTask
			
		End Get
		Set(ByVal Value As Integer)
			
			m_iTask = Value
			
		End Set
	End Property
	
	Public WriteOnly Property Navigate() As Integer
		Set(ByVal Value As Integer)
			
			m_lNavigate = Value
			
		End Set
	End Property
	
	Public Property StepStatus() As String
		Get
			
			Return m_sStepStatus
			
		End Get
		Set(ByVal Value As String)
			
			m_sStepStatus = Value
			
		End Set
	End Property
	Public WriteOnly Property ProcessMode() As Integer
		Set(ByVal Value As Integer)
			
			m_lProcessMode = Value
			
		End Set
	End Property
	
	Public WriteOnly Property TransactionType() As String
		Set(ByVal Value As String)
			
			m_sTransactionType = Value
			
		End Set
	End Property
	
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			
			m_dtEffectiveDate = Value
			
		End Set
	End Property

    ' ***********************************************************
    ' Set the resizing anchors
    ' ***********************************************************


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        iPMFunc.ShowFormInTaskBar_Detach()
        SSTabHelper.SetSelectedIndex(Me.tabPolicyVersion, 0)
        '        txtSqlQuery.Text = "Select DOC.insurance_file_cnt,SF.Insurance_Ref,DOC.Document_ID,Doc.Document_Ref,Spare,cast(TD.Amount as decimal(19,2)),SF.transaction_type_id,tt.[description],TD.accounting_Date    --- Find the Original Commission which should have been suspended
        'From Document  DOC
        'Inner JOIN Stats_Folder SF ON DOC.Document_Ref = SF.Document_Ref
        'inner JOIN TransDetail TD on TD.document_id = Doc.Document_ID
        'Inner JOIN Account ACCT on ACCT.account_id = TD.Account_ID
        'inner JOIN Ledger on Ledger.Ledger_ID = ACCT.Ledger_Id
        'inner JOIN Transaction_Type tt on SF.transaction_type_id = tt.transaction_type_id
        'Where Ledger.ledger_name = 'Sub Agent'
        'and cast(TD.Outstanding_Amount as INT) <> 0
        'and Doc.created_date > '2020-02-29'
        '--and SF.Insurance_ref Like 'HEPBUR00053366'
        'and DOC.Insurance_File_Cnt In
        '		(Select Distinct INF.Insurance_File_Cnt   --- Find All INC record missing Agent's Commission Suspense[Note, this does not include cancellation JN's)
        '				From PFPremiumFinance PFF
        '				Inner JOIN Insurance_File INF on INF.Insurance_File_Cnt = PFF.Insurance_File_Cnt
        '				Inner JOIN Document DOC on DOC.Insurance_File_Cnt = INF.Insurance_File_Cnt
        '				Inner JOIN TransDetail  TD on TD.Document_ID = DOC.Document_ID
        '				Inner JOIN Account ACCT on ACCT.Account_ID = TD.Account_ID

        '				Where LEFT(Document_ref, 3)  IN ('INC', 'IRC')
        '				and Doc.created_date > '2020-02-29'
        '				and pff.StatusInd <> '999'
        '				and TD.Document_ID NOT In 
        '				( Select Distinct(document_id) 
        '				From [dbo].[CashListItem_Instalments] CLINST
        '				Inner JOIN CashListitem CLI ON CLI.CashListitem_ID = CLINST.cashlistitem_id
        '				Inner JOIN TransDetail  TD  On TD.transdetail_id = CLI.transdetail_id)
        '				and Doc.Document_Ref NOT IN 


        '										(Select Distinct (Doc.Document_Ref)  ---   Find all records with Sub- Agents Commission Suspense
        '										From PFPremiumFinance PFF
        '										Inner JOIN Insurance_File INF on INF.Insurance_File_Cnt = PFF.Insurance_File_Cnt
        '										Inner JOIN Document DOC on DOC.Insurance_File_Cnt = INF.Insurance_File_Cnt
        '										Inner JOIN TransDetail  TD on TD.Document_ID = DOC.Document_ID
        '										Inner JOIN Account ACCT on ACCT.Account_ID = TD.Account_ID

        '										Where LEFT(Document_ref, 3)  IN ('INC', 'IRC')
        '										and Doc.created_date > '2020-02-29'
        '										and Acct.short_code = 'COLLACC'
        '										and TD.Document_ID NOT In 
        '										( Select Distinct(document_id) 
        '										From [dbo].[CashListItem_Instalments] CLINST
        '										Inner JOIN CashListitem CLI ON CLI.CashListitem_ID = CLINST.cashlistitem_id
        '										Inner JOIN TransDetail  TD  On TD.transdetail_id = CLI.transdetail_id)))

        'AND DOC.Insurance_File_Cnt NOT In (select insurance_file_cnt from PM058633_DataFix_Part1_log)

        'Order by SF.Insurance_Ref"

        '        txtSqlQuery1.Text = "DECLARE @insurance_File_cnt INT
        'DECLARE @insurance_Ref VARCHAR(30)
        'DECLARE @pfprem_finance_cnt INT
        'DECLARE @pfprem_finance_Version INT
        'DECLARE @account_id INT
        'DECLARE @PfInstalments_id INT
        'DECLARE @InstalmentPaidAmount Numeric(19,4)
        'DECLARE @AmountToFinance Numeric(19,4)
        'DECLARE @TotalCommissionValue Numeric(19,4)
        'DECLARE @PaidCommissonValue Numeric(19,4)
        'DECLARE @ComputedPaidCommissonValue Numeric(19,4)
        'DECLARE @DiffInPaidCommissonValue Numeric(19,4)
        'DECLARE @posting_document_id INT 
        'DECLARE @reference_credit_transdetail_id INT
        '--DECLARE @reference_debit_transdetail_id INT
        'DECLARE @reference_credit_transdetail_date DATETIME
        'DECLARE @NewDocRef varchar(25)
        'DECLARE @TodaysDate DATETIME
        'SET @TodaysDate = GETDATE()

        'Create table #Result
        '(
        '	insurance_File_cnt int,
        '	insurance_Ref VARCHAR(30),
        '	pfprem_finance_cnt int,
        '	pfprem_finance_Version int,
        '	account_id int
        ')

        'Create table #Result1
        '(
        '	insurance_File_cnt int,
        '	insurance_Ref VARCHAR(30),
        '	pfprem_finance_cnt int,
        '	pfprem_finance_Version int,
        '	account_id int,
        '	PfInstalments_id int,
        '	InstalmentPaidAmount Numeric(19,2),
        '	AmountToFinance Numeric(19,2),
        '	TotalCommission Numeric(19,2),
        '	PaidCommissonValue Numeric(19,2),
        '	ComputedPaidCommissonValue Numeric(19,2),
        '	DiffInPaidCommissonValue Numeric(19,2),
        '	--reference_credit_transdetail_id int,
        '	reference_credit_transdetail_date date
        ')

        'insert into #Result
        'select distinct ia.insurance_file_cnt, i.insurance_ref,pf.pfprem_finance_cnt,pf.pfprem_finance_Version,a1.account_id from insurance_file_agent ia
        'join (select MAX(insurance_file_cnt) AS insurance_file_cnt,insurance_ref from insurance_file group by insurance_ref) i on i.insurance_file_cnt = ia.insurance_file_cnt
        'join insurance_file i1 on i.insurance_file_cnt = i1.insurance_file_cnt
        'join pfpremiumfinance pf on pf.insurance_file_cnt = i1.insurance_file_cnt and pf.statusind in ('040','990')
        'join document d on d.insurance_file_cnt = i1.insurance_file_cnt
        'join transdetail t on d.document_id = t.document_id
        'join account a on a.account_id = t.account_id
        'join account a1 on a1.account_key = ia.party_cnt
        'where i1.insurance_file_type_id in (2,5,9) and a.short_code = 'COLLACC' and d.document_date <'2020-03-01' and d.document_ref Like 'JN%'  
        'and i1.insurance_ref = 'HOPMOT00120497'
        'order by ia.insurance_file_cnt

        'Declare c Cursor For select insurance_File_cnt,insurance_Ref,pfprem_finance_cnt,pfprem_finance_Version,account_id from #Result
        'Open c
        'Fetch next From c into @insurance_File_cnt,@insurance_Ref,@pfprem_finance_cnt,@pfprem_finance_Version,@account_id
        'While @@Fetch_Status=0 Begin

        '	select @TotalCommissionValue = Commission_value from agent_commission where insurance_file_cnt = @insurance_File_cnt

        '	Declare c1 Cursor For select pfi.pfinstalments_id,pfi.amount,p.amounttofinance from PFPremiumFinance p
        '							join PFInstalments pfi on p.pfprem_finance_cnt = pfi.pfprem_finance_cnt and p.pfprem_finance_version = pfi.pfprem_finance_version
        '							where p.pfprem_finance_cnt = @pfprem_finance_cnt and p.pfprem_finance_version = @pfprem_finance_Version and pfi.status = 3 
        '							and instalmentnumber >= 0
        '	Open c1
        '	Fetch next From c1 into @PfInstalments_id,@InstalmentPaidAmount,@AmountToFinance
        '	While @@Fetch_Status=0 Begin
        '		SELECT @PaidCommissonValue = SUM(ISNULL(t.amount,0)) from Released_Accounts_Transactions rat 
        '		join transdetail t on t.transdetail_id = rat.destination_transdetail_id
        '		where rat.pfinstalments_id = @PfInstalments_id group by rat.pfinstalments_id

        '		select @reference_credit_transdetail_date = t.accounting_date from pfinstalments pfi
        '		join transdetail t on pfi.pftransaction_id = t.transdetail_id
        '		where pfi.pfinstalments_id = @PfInstalments_id

        '		SET @ComputedPaidCommissonValue = ((@InstalmentPaidAmount / @AmountToFinance) * @TotalCommissionValue) * -1
        '		SET @DiffInPaidCommissonValue = @PaidCommissonValue - @ComputedPaidCommissonValue
        '		IF (CAST(@DiffInPaidCommissonValue AS INT) <> 0 AND CAST(@PaidCommissonValue AS INT) <> 0)
        '		BEGIN
        '			INSERT INTO #Result1 values(@insurance_File_cnt,@insurance_Ref,@pfprem_finance_cnt,@pfprem_finance_Version,@account_id,@PfInstalments_id,ROUND(@InstalmentPaidAmount,2), ROUND(@AmountToFinance,2), ROUND(@TotalCommissionValue,2),ROUND(@PaidCommissonValue,2),ROUND(@ComputedPaidCommissonValue,2), ROUND(@DiffInPaidCommissonValue,2),@reference_credit_transdetail_date)
        '		END
        '		SET @PaidCommissonValue = 0
        '	Fetch next From c1 into @PfInstalments_id,@InstalmentPaidAmount,@AmountToFinance
        '	End
        '	Close c1
        '	Deallocate c1
        'Fetch next From c into @insurance_File_cnt,@insurance_Ref,@pfprem_finance_cnt,@pfprem_finance_Version,@account_id
        'End
        'Close c
        'Deallocate c

        'select * from #Result1

        'If(OBJECT_ID('tempdb..#Result1') Is Not Null)
        'Begin
        '    Drop Table #Result1
        'End

        'If(OBJECT_ID('tempdb..#Result') Is Not Null)
        'Begin
        '    Drop Table #Result
        'End"
    End Sub

    Private isInitializingComponent As Boolean

    Private Sub lvwPolicyVersion_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs)
        Dim ColumnHeader As ColumnHeader = lvwPolicyVersion.Columns(eventArgs.Column)

        ' Column click event for the search details
        Try

            With lvwPolicyVersion
                'Identify the Date type columns
                If Convert.ToString(ColumnHeader.Tag).ToUpper() = "DATESORT" Then
                    ListViewHelper.SetSortedProperty(lvwPolicyVersion, False)
                    ListViewHelper.SetSortOrderProperty(lvwPolicyVersion, (ListViewHelper.GetSortOrderProperty(lvwPolicyVersion) + 1) Mod 2)
                    ListViewFunc.ListViewSortByDate(lvwPolicyVersion, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwPolicyVersion))

                    'Identify the Value type columns (to sort numerics correctly)
                ElseIf Convert.ToString(ColumnHeader.Tag).ToUpper() = "VALUESORT" Then
                    ListViewHelper.SetSortedProperty(lvwPolicyVersion, False)
                    ListViewHelper.SetSortOrderProperty(lvwPolicyVersion, (ListViewHelper.GetSortOrderProperty(lvwPolicyVersion) + 1) Mod 2)
                    ListViewFunc.ListViewSortByValue(lvwPolicyVersion, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwPolicyVersion))

                    'See if this the column already sorted on
                ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwPolicyVersion)) Then
                    ' Set sort order opposite of current direction.
                    ListViewHelper.SetSortOrderProperty(lvwPolicyVersion, (ListViewHelper.GetSortOrderProperty(lvwPolicyVersion) + 1) Mod 2)
                    'If this is the very first time that a header is clicked then
                    'if clicking on the first column we need to refresh. Otherwise we don't
                    If Not ListViewHelper.GetSortedProperty(lvwPolicyVersion) Then
                        'Do the refresh
                        ListViewHelper.SetSortedProperty(lvwPolicyVersion, True)
                    End If

                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwPolicyVersion, False)
                    ' Turn off sorting so that the list is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwPolicyVersion, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwPolicyVersion, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwPolicyVersion, True)
                End If

            End With

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed lvwPolicyVersion_ColumnClick", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwPolicyVersion_ColumnClick", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

        End Try

    End Sub

    ''' <summary>
    ''' cmdGetPolicyVersion_Click
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdGetPolicyVersion_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdGetPolicyVersion.Click


        Dim aoResultArray(,) As Object = Nothing
        Dim oListItem As ListViewItem = Nothing
        Dim sSql As String = String.Empty
        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            Me.cmdGetPolicyVersion.Enabled = False

            stbMain.Items.Item("MESSAGE").Text = "Getting all records missing sub-agent suspence entry."

            sSql = txtSqlQuery.Text

            If InStr(1, (sSql).ToLower, "delete") <> 0 Or InStr(1, sSql, "update") Then
                MsgBox("Update/Delete is not allowed ", vbCritical, ACApp)
                Exit Sub
            End If

            m_lReturn = m_oBusiness.ExecuteSql(v_sSql:=sSql, r_vResultArray:=aoResultArray)

            If Not Information.IsArray(aoResultArray) Then
                lvwPolicyVersion.Items.Clear()
                Exit Sub
            End If

            lvwPolicyVersion.Items.Clear()

            For lCount As Integer = 0 To aoResultArray.GetUpperBound(1)

                oListItem = Me.lvwPolicyVersion.Items.Add(CStr(aoResultArray(ACFieldPVInsuranceFileCnt, lCount)))

                oListItem.SubItems.Insert(ACFieldPVInsuranceRef,
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVInsuranceRef, lCount).Trim()))

                oListItem.SubItems.Insert(ACFieldPVDocumentID,
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVDocumentID, lCount).Trim()))

                oListItem.SubItems.Insert(ACFieldPVDocumentRef,
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVDocumentRef, lCount).Trim()))

                oListItem.SubItems.Insert(ACFieldPVSpare,
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVSpare, lCount).Trim()))

                oListItem.SubItems.Insert(ACFieldPVCommAmount,
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVCommAmount, lCount).Trim()))

                oListItem.SubItems.Insert(ACFieldPVTransTypeID,
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVTransTypeID, lCount).Trim()))

                oListItem.SubItems.Insert(ACFieldPVTransTypeDesc,
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVTransTypeDesc, lCount).Trim()))

                oListItem.SubItems.Insert(ACFieldPVTransDate,
                                          New ListViewItem.ListViewSubItem(oListItem, CDate(aoResultArray(ACFieldPVTransDate, lCount)).ToShortDateString()))
            Next

            If lvwPolicyVersion.Items.Count > 0 Then
                lvwPolicyVersion.Items.Item(0).Selected = True
            End If

            stbMain.Items.Item("COUNT").Text = CStr(Me.lvwPolicyVersion.Items.Count)

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed cmdGetPolicyVersion_Click", vApp:=ACApp,
                                         vClass:=ACClass, vMethod:="cmdGetPolicyVersion_Click", vErrNo:=CStr(Information.Err().Number),
                                         vErrDesc:=Information.Err().Description, excep:=ex)
        Finally

            Me.cmdGetPolicyVersion.Enabled = True
            stbMain.Items.Item("MESSAGE").Text = "Ready"
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        End Try

    End Sub

    Private Sub Form_Initialize_Renamed()


        ' Forms initialise event.
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            'Get an instance of the business object via
            'the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRRepostTransaction.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display message.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bSirRepostTransaction.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

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

            If Not (m_oBusiness Is Nothing) Then

                m_lReturn = m_oBusiness.Terminate
                m_oBusiness = Nothing
            End If

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    '*********************************************************************************************
    ' we only recreate stats details and transaction exports if required.
    ' we do not recreate stats folder because we still want the old document ref
    ' if we need to recreate stats folder in the future then this can be added easily
    '*********************************************************************************************
    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click

        Me.cmdOk.Enabled = False
        Me.cmdExit.Enabled = False
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        Select Case SSTabHelper.GetSelectedIndex(Me.tabPolicyVersion)
            Case 0

                ProcessRecords()
                bIsSucesfullyCompleted = True

                stbMain.Items.Item("COUNT").Text = CStr(Me.lvwPolicyVersion.Items.Count)
        End Select

        If bIsSucesfullyCompleted Then
            MsgBox("Item Processed successfully", vbInformation, "Data Fix Utility")
        End If


        Me.cmdOk.Enabled = True
        Me.cmdExit.Enabled = True
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        cmdGetPolicyVersion_Click(Nothing, Nothing)

    End Sub

    Private Sub ProcessRecords()
        Dim lReturn As Integer = 0
        Dim vAttachedSubAgents As Object = Nothing
        Try
            lReturn = gPMConstants.PMEReturnCode.PMTrue
            For Each lstItem As ListViewItem In lvwPolicyVersion.CheckedItems
                lReturn = m_oBusiness.GetSubAgentAccountID(ToSafeInteger(lstItem.SubItems(0).Text), vAttachedSubAgents)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception("Get Sub Agent Account ID failed.")
                End If
                For lCount As Integer = 0 To vAttachedSubAgents.GetUpperBound(1)
                    lReturn = m_oBusiness.InsertMissingSubAgentCommSuspenseTD(vAttachedSubAgents(0, lCount), ToSafeInteger(lstItem.SubItems(0).Text), ToSafeInteger(lstItem.SubItems(2).Text), ToSafeInteger(lstItem.SubItems(6).Text), ToSafeDecimal(lstItem.SubItems(5).Text))
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception("Insert Missing SubAgent Commission Suspense details failed.")
                    End If
                Next
                m_lReturn = m_oBusiness.AddPM058633DataFixPart1log(v_sCreatedByID:=g_iUserID, v_lInsuranceFileCnt:=ToSafeInteger(lstItem.SubItems(0).Text))
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception("Insert to log table failed.")
                End If
            Next
            m_lReturn = m_oBusiness.GetMissingComissionReleaseToAgent()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception("Insert Missing SubAgent Commission Release failed.")
            End If

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed ProcessRecords method.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdExit_Click", vErrNo:=CStr(Information.Err().Number), vErrDesc:=ex.Message)
        End Try
    End Sub


    Private Sub cmdExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExit.Click, cmdexit1.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            Me.Hide()

        Catch excep As System.Exception




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdExit_Click", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Exit Sub

        End Try

    End Sub

    '********************************************************************************************
    ' tick record on list view
    ' v_lAll = 1 for all records, v_lAll = 0 for high lighted records
    '********************************************************************************************
    Private Sub TickListView(ByRef r_oListView As ListView, Optional ByVal v_lAll As Integer = 0, Optional ByVal v_bValue As Boolean = True)

        Try

            'loop though all records and tick all if v_lall = 1 or just tick selected ones
            For Each oListItem As ListViewItem In r_oListView.Items
                oListItem.Checked = (v_lAll = 1 Or oListItem.Selected Or oListItem.Checked) And v_bValue
            Next oListItem

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed tick list view", vApp:=ACApp, vClass:=ACClass, vMethod:="TickListView()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

        End Try

    End Sub

    '********************************************************************************************
    'return number of items ticked on list view
    '
    '********************************************************************************************
    Private Function IsTick(ByVal oListView As ListView) As Integer

        Dim lCount As Integer

        On Error GoTo Catch_Renamed
        lCount = 0

        For Each oListItem As ListViewItem In oListView.Items
            If oListItem.Checked Then
                lCount += 1
            End If
        Next oListItem

        GoTo Finally_Renamed

Catch_Renamed:
        lCount = -1

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To check For ticked items On listview", vApp:=ACApp, vClass:=ACClass, vMethod:="IsTick()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return lCount
    End Function

    '*************************************************************************************************************
    ' deleted selected records from listview
    '*************************************************************************************************************
    Private Sub DeleteSelected(ByRef r_oListView As ListView)


        On Error GoTo Catch_Renamed

        For lCount As Integer = r_oListView.Items.Count To 1 Step -1
            If r_oListView.Items.Item(lCount - 1).Checked Then
                r_oListView.Items.RemoveAt(lCount - 1)
            End If
        Next

        If r_oListView.Items.Count > 0 Then
            stbMain.Items.Item("COUNT").Text = CStr(r_oListView.Items.Count)
        End If

        GoTo Finally_Renamed

Catch_Renamed:

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to deleted selected records from list", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteSelected()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Exit Sub
    End Sub

    Private Sub CmdSelectAllPolicy_Click(sender As Object, e As EventArgs) Handles CmdSelectAllPolicy.Click
        Dim i As Integer
        If CmdSelectAllPolicy.Text = "Select All" Then
            For i = 0 To lvwPolicyVersion.Items.Count - 1
                lvwPolicyVersion.Items(i).Checked = True
            Next
            CmdSelectAllPolicy.Text = "UnSelect All"
            Exit Sub
        End If
        If CmdSelectAllPolicy.Text = "UnSelect All" Then
            For i = 0 To lvwPolicyVersion.Items.Count - 1
                lvwPolicyVersion.Items(i).Checked = False
            Next
            CmdSelectAllPolicy.Text = "Select All"
        End If
    End Sub



    Private Function ValidateDuplicate(ByRef sInsuranceRef As String) As Boolean
        Try

            Dim InsuranceRef1 As String = ""
            Dim InsuranceRef As String = ""
            Dim nCount As Integer = 0

            For Each oListItem As ListViewItem In Me.lvwPolicyVersion.Items

                If oListItem.Checked Then
                    InsuranceRef = oListItem.SubItems(1).Text

                End If
                For Each oListItem1 As ListViewItem In Me.lvwPolicyVersion.Items
                    If oListItem1.Checked Then
                        InsuranceRef1 = oListItem1.SubItems(1).Text
                        If InsuranceRef1 = InsuranceRef Then
                            nCount = nCount + 1
                            If nCount > 1 Then
                                ValidateDuplicate = False
                                sInsuranceRef = InsuranceRef
                                Exit Function
                            End If
                        End If
                    End If


                Next oListItem1
                nCount = 0
            Next oListItem
            ValidateDuplicate = True

        Catch ex As Exception
            ValidateDuplicate = False
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CheckedOtherRefrence ", vApp:=ACApp, vClass:=ACClass, vMethod:="ReversePolicyTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
        End Try

    End Function
    Private Function ValidateDuplicateInsCnt(ByRef sInsuranceRef As String) As Boolean
        Try

            Dim InsuranceFileCnt1 As String = ""
            Dim InsuranceFileCnt As String = ""
            Dim nCount As Integer = 0

            For Each oListItem As ListViewItem In Me.lvwPolicyVersion.Items

                If oListItem.Checked Then
                    InsuranceFileCnt = oListItem.SubItems(0).Text

                End If
                For Each oListItem1 As ListViewItem In Me.lvwPolicyVersion.Items
                    If oListItem1.Checked Then
                        InsuranceFileCnt1 = oListItem1.SubItems(0).Text
                        If InsuranceFileCnt1 = InsuranceFileCnt Then
                            nCount = nCount + 1
                            If nCount > 1 Then
                                ValidateDuplicateInsCnt = False
                                sInsuranceRef = InsuranceFileCnt
                                Exit Function
                            End If
                        End If
                    End If


                Next oListItem1
                nCount = 0
            Next oListItem
            ValidateDuplicateInsCnt = True

        Catch ex As Exception
            ValidateDuplicateInsCnt = False
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CheckedOtherRefrence ", vApp:=ACApp, vClass:=ACClass, vMethod:="ReversePolicyTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
        End Try

    End Function

    Private Sub CheckedDuplicateInsuranceRef(ch As Boolean)
        Try

            For Each oListItem As ListViewItem In Me.lvwPolicyVersion.Items
                If oListItem.Checked Then
                    Dim InsuranceRef As String = oListItem.SubItems(1).Text
                    CheckedOtherRefrence(InsuranceRef, ch)
                    ' Exit Sub
                End If
            Next oListItem

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CheckedDuplicateInsuranceRef ", vApp:=ACApp, vClass:=ACClass, vMethod:="ReversePolicyTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
        End Try

    End Sub

    Private Sub CheckedOtherRefrence(sInsurance As String, ch As Boolean)
        Try

            For Each oListItem As ListViewItem In Me.lvwPolicyVersion.Items
                Dim InsuranceRef As String = oListItem.SubItems(1).Text
                If sInsurance = InsuranceRef Then
                    If ch = True Then
                        oListItem.Checked = True
                    Else
                        oListItem.Checked = False
                    End If
                End If
            Next oListItem

        Catch ex As Exception
            bIsSucesfullyCompleted = False
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CheckedOtherRefrence ", vApp:=ACApp, vClass:=ACClass, vMethod:="ReversePolicyTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
        End Try

    End Sub

    Private Sub lvwPolicyVersion_ItemChecked(sender As Object, e As ItemCheckedEventArgs)
        Dim x As Boolean
        x = e.Item.Checked
        If Not m_bisFirstSelect Then
            m_bisFirstSelect = True
            CheckedDuplicateInsuranceRef(x)
            m_bisFirstSelect = False
        End If

    End Sub

    Private Sub cmdGetRecords_Click(sender As Object, e As EventArgs) Handles cmdGetRecords.Click
        Dim aoResultArray(,) As Object = Nothing
        Dim oListItem As ListViewItem = Nothing
        Dim sSql As String = String.Empty
        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            Me.cmdGetRecords.Enabled = False

            stbMain.Items.Item("MESSAGE").Text = "Getting all records missing sub-agent suspence entry."

            sSql = txtSqlQuery1.Text

            If InStr(1, (sSql).ToLower, "delete") <> 0 Or InStr(1, sSql, "update") Then
                MsgBox("Update/Delete is not allowed ", vbCritical, ACApp)
                Exit Sub
            End If

            m_lReturn = m_oBusiness.ExecuteSql(v_sSql:=sSql, r_vResultArray:=aoResultArray)

            If Not Information.IsArray(aoResultArray) Then
                lvwIncorrectCommissionPostingList.Items.Clear()
                Exit Sub
            End If

            lvwIncorrectCommissionPostingList.Items.Clear()

            For lCount As Integer = 0 To aoResultArray.GetUpperBound(1)

                oListItem = Me.lvwIncorrectCommissionPostingList.Items.Add(CStr(aoResultArray(ACFieldCMInsuranceFileCnt, lCount)))

                oListItem.SubItems.Insert(ACFieldCMInsuranceRef,
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldCMInsuranceRef, lCount).Trim()))

                oListItem.SubItems.Insert(ACFieldCMPremFinanceCnt,
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldCMPremFinanceCnt, lCount).Trim()))

                oListItem.SubItems.Insert(ACFieldCMPremFinanceVersion,
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldCMPremFinanceVersion, lCount).Trim()))

                oListItem.SubItems.Insert(ACFieldCMAccountID,
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldCMAccountID, lCount).Trim()))

                oListItem.SubItems.Insert(ACFieldCMInstalmentID,
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldCMInstalmentID, lCount).Trim()))

                oListItem.SubItems.Insert(ACFieldCMInstalmentAmt,
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldCMInstalmentAmt, lCount).Trim()))

                oListItem.SubItems.Insert(ACFieldCMAmountToFinance,
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldCMAmountToFinance, lCount).Trim()))

                oListItem.SubItems.Insert(ACFieldCMTotalCommission,
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldCMTotalCommission, lCount).Trim()))

                oListItem.SubItems.Insert(ACFieldCMPaidCommission,
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldCMPaidCommission, lCount).Trim()))

                oListItem.SubItems.Insert(ACFieldCMCorrectCommission,
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldCMCorrectCommission, lCount).Trim()))

                oListItem.SubItems.Insert(ACFieldCMCorrectionAmt,
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldCMCorrectionAmt, lCount).Trim()))

                oListItem.SubItems.Insert(ACFieldCMTransDate,
                                          New ListViewItem.ListViewSubItem(oListItem, CDate(aoResultArray(ACFieldCMTransDate, lCount)).ToShortDateString()))
            Next

            If lvwIncorrectCommissionPostingList.Items.Count > 0 Then
                lvwIncorrectCommissionPostingList.Items.Item(0).Selected = True
            End If

            stbMain.Items.Item("COUNT").Text = CStr(Me.lvwIncorrectCommissionPostingList.Items.Count)

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed cmdGetPolicyVersion_Click", vApp:=ACApp,
                                         vClass:=ACClass, vMethod:="cmdGetPolicyVersion_Click", vErrNo:=CStr(Information.Err().Number),
                                         vErrDesc:=Information.Err().Description, excep:=ex)
        Finally

            Me.cmdGetRecords.Enabled = True
            stbMain.Items.Item("MESSAGE").Text = "Ready"
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        End Try
    End Sub

    Private Sub cmdselectall1_Click(sender As Object, e As EventArgs) Handles cmdselectall1.Click
        Dim i As Integer
        If cmdselectall1.Text = "Select All" Then
            For i = 0 To lvwIncorrectCommissionPostingList.Items.Count - 1
                lvwIncorrectCommissionPostingList.Items(i).Checked = True
            Next
            cmdselectall1.Text = "UnSelect All"
            Exit Sub
        End If
        If cmdselectall1.Text = "UnSelect All" Then
            For i = 0 To lvwIncorrectCommissionPostingList.Items.Count - 1
                lvwIncorrectCommissionPostingList.Items(i).Checked = False
            Next
            cmdselectall1.Text = "Select All"
        End If
    End Sub

    Private Sub cmdok1_Click(sender As Object, e As EventArgs) Handles cmdok1.Click
        Me.cmdOk.Enabled = False
        Me.cmdExit.Enabled = False
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        Select Case SSTabHelper.GetSelectedIndex(Me.tabPolicyVersion)
            Case 1

                CorrectCommissionPosting()
                bIsSucesfullyCompleted = True

                stbMain.Items.Item("COUNT").Text = CStr(Me.lvwIncorrectCommissionPostingList.Items.Count)
        End Select

        If bIsSucesfullyCompleted Then
            MsgBox("Item Processed successfully", vbInformation, "Data Fix Utility")
        End If


        Me.cmdOk.Enabled = True
        Me.cmdExit.Enabled = True
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        cmdGetRecords_Click(Nothing, Nothing)
    End Sub

    Private Sub CorrectCommissionPosting()
        Dim lReturn As Integer = 0
        Try
            lReturn = gPMConstants.PMEReturnCode.PMTrue
            For Each lstItem As ListViewItem In lvwIncorrectCommissionPostingList.CheckedItems
                lReturn = m_oBusiness.CorrectCommissionPosting(ToSafeInteger(lstItem.SubItems(ACFieldCMInsuranceFileCnt).Text),
                                                               ToSafeInteger(lstItem.SubItems(ACFieldCMInstalmentID).Text),
                                                               ToSafeInteger(lstItem.SubItems(ACFieldCMAccountID).Text),
                                                               ToSafeDecimal(lstItem.SubItems(ACFieldCMCorrectionAmt).Text))
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception("Get Sub Agent Account ID failed.")
                End If
            Next

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed ProcessRecords method.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdExit_Click", vErrNo:=CStr(Information.Err().Number), vErrDesc:=ex.Message)
        End Try
    End Sub
End Class


