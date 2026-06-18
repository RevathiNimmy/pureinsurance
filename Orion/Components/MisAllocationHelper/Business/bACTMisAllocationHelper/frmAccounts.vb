Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.DB.DAO
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmAccounts
	Inherits System.Windows.Forms.Form
	
	Public m_oConnection As SqlConnection
	Private m_oRSAccounts As DataSet
	Private m_oRSAllocations As DataSet
	Private m_oRSTransactions As DataSet
	
	Private m_bLogFileOpen As Boolean
	Public m_tsLog As FileStream
	
	Private m_lOrderBy As Integer
	Private m_bAscending As Boolean
	
	Private Sub cmdAddMissing_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddMissing.Click
		Dim oCommand As SqlCommand
		Dim sSQL As New StringBuilder
		Dim lMatchID As Integer
		Dim frmAddRelatedInstance As frmAddRelated
		
		Try 
			
			lMatchID = grdAllocations.CurrentRow().Cells(0).Value
			
			frmAddRelatedInstance = New frmAddRelated()
			
			frmAddRelatedInstance.MatchID = lMatchID
			frmAddRelatedInstance.AddType = ACATAddMissing
			
			frmAddRelatedInstance.ShowDialog()
			
			If Information.IsArray(frmAddRelatedInstance.Transactions) Then
				
				For lCount As Integer = frmAddRelatedInstance.Transactions.GetLowerBound(0) To frmAddRelatedInstance.Transactions.GetUpperBound(0)
					sSQL = New StringBuilder("")
					sSQL.Append("DECLARE @match_id INT" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("DECLARE @transdetail_id INT" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("DECLARE @allocationdetail_id INT" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("SELECT @match_id = " & lMatchID & Strings.Chr(13) & Strings.Chr(10))

					sSQL.Append("SELECT @allocationdetail_id = " & CStr(frmAddRelatedInstance.Transactions(lCount)) & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("SELECT @transdetail_id = transdetail_id" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("FROM allocationdetail" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("WHERE allocationdetail_id = @allocationdetail_id" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("UPDATE ad" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("SET ad.os_base_amount = td.amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    ad.os_ccy_amount = td.currency_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    ad.alloc_base_amount = td.amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    ad.alloc_ccy_amount = td.currency_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    ad.fully_matched = td.fully_matched," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    ad.write_off_amount = NULL," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    ad.write_off_reason_id = NULL," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    ad.new_os_ccy_amount = 0," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    ad.new_os_base_amount = 0," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    ad.loss_gain_amount = 0" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("FROM allocationdetail ad" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("JOIN transdetail td" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    ON td.transdetail_id = ad.transdetail_id" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("WHERE allocationdetail_id = @allocationdetail_id" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("INSERT INTO transmatch" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("(" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    currency_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    allocationdetail_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    transdetail_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    match_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    base_match_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    currency_match_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    currency_match_xrate," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    is_reversed" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append(")" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("SELECT" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    currency_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    @allocationdetail_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    transdetail_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    @match_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    currency_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    currency_base_xrate," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    NULL" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("FROM transdetail" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("WHERE transdetail_id = @transdetail_id" & Strings.Chr(13) & Strings.Chr(10))
					
					oCommand = New SqlCommand()
					oCommand.Connection = m_oConnection
					oCommand.CommandText = sSQL.ToString()
					oCommand.CommandTimeout = kcTimeout
					oCommand.ExecuteNonQuery()
					
				Next 
				
				RefreshTransactionsGrid()
				
			End If
			
			frmAddRelatedInstance = Nothing
		
		Catch 
			
			
			
			MessageBox.Show("Script failed to run", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
		
	End Sub
	
	Private Sub cmdAddOther_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddOther.Click
		Dim oCommand As SqlCommand
		Dim sSQL As New StringBuilder
		Dim lMatchID As Integer
		Dim frmAddRelatedInstance As frmAddRelated
		Dim sAccountCode As String = ""
		
		Try 
			
			lMatchID = grdAllocations.CurrentRow().Cells(0).Value
			sAccountCode = grdAccounts.CurrentRow().Cells(0).Value
			
			frmAddRelatedInstance = New frmAddRelated()
			
			frmAddRelatedInstance.AccountCode = sAccountCode
			frmAddRelatedInstance.AddType = ACATAddOther
			
			frmAddRelatedInstance.ShowDialog()
			
			If Information.IsArray(frmAddRelatedInstance.Transactions) Then
				
				For lCount As Integer = frmAddRelatedInstance.Transactions.GetLowerBound(0) To frmAddRelatedInstance.Transactions.GetUpperBound(0)
					sSQL = New StringBuilder("")
					sSQL.Append("DECLARE @match_id INT" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("DECLARE @transdetail_id INT" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("DECLARE @allocation_id INT" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("DECLARE @cashlistitem_id INT" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("DECLARE @allocationdetail_id INT" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("SELECT @match_id = " & lMatchID & Strings.Chr(13) & Strings.Chr(10))

					sSQL.Append("SELECT @transdetail_id = " & CStr(frmAddRelatedInstance.Transactions(lCount)) & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("SELECT @allocation_id = MAX(ISNULL(ad.allocation_id,0))" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("FROM transmatch tm" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("JOIN allocationdetail ad" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    ON ad.allocationdetail_id = tm.allocationdetail_id" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    AND ad.transdetail_id = tm.transdetail_id" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("WHERE tm.match_id = @match_id" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("SELECT @cashlistitem_id = MAX(ISNULL(cashlistitem_id,0))" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("FROM allocationdetail" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("WHERE allocation_id = @allocation_id" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("IF @cashlistitem_id = 0" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("BEGIN" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    SELECT @cashlistitem_id = NULL" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("END" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("INSERT INTO allocationdetail" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("(" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    cashlistitem_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    allocation_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    original_currency," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    transdetail_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    documenttype_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    document_ref," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    accounting_date," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    original_date," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    allocate_to_base," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    orig_base_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    orig_base_amount_unrounded," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    orig_ccy_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    orig_ccy_amount_unrounded," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    orig_xrate," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    effective_xrate," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    os_base_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    os_ccy_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    alloc_base_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    alloc_ccy_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    fully_matched," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    write_off_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    write_off_reason_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    new_os_ccy_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    new_os_base_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    loss_gain_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    is_primary," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    euro_currency_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    euro_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    euro_base_xrate," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    euro_ccy_xrate" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append(")" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("SELECT" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    @cashlistitem_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    @allocation_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.currency_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.transdetail_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    d.documenttype_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    d.document_ref," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.accounting_date," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.accounting_date," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    0," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.base_amount_unrounded," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.currency_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.currency_amount_unrounded," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.currency_base_xrate," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.currency_base_xrate," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.currency_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.currency_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.fully_matched," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    NULL," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    NULL," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    0," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    0," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    0," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    0," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.euro_currency_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.euro_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.euro_base_xrate," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.euro_ccy_xrate" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("FROM transdetail td" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("JOIN document d" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    ON d.document_id = td.document_id" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("WHERE transdetail_id = @transdetail_id" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("SELECT @allocationdetail_id = @@IDENTITY" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("INSERT INTO transmatch" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("(" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    currency_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    allocationdetail_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    transdetail_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    match_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    base_match_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    currency_match_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    currency_match_xrate," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    is_reversed" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append(")" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("SELECT" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    currency_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    @allocationdetail_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    transdetail_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    @match_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    currency_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    currency_base_xrate," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    NULL" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("FROM transdetail" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("WHERE transdetail_id = @transdetail_id" & Strings.Chr(13) & Strings.Chr(10))
					
					oCommand = New SqlCommand()
					
					oCommand.Connection = m_oConnection
					oCommand.CommandText = sSQL.ToString()
					oCommand.CommandTimeout = kcTimeout
					oCommand.ExecuteNonQuery()
					
				Next 
				
				RefreshTransactionsGrid()
				
			End If
			
			frmAddRelatedInstance = Nothing
		
		Catch 
			
			
			
			MessageBox.Show("Script failed to run", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
		
		
	End Sub
	
	Private Sub cmdAddRelated_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddRelated.Click
		Dim oCommand As SqlCommand
		Dim sSQL As New StringBuilder
		Dim lMatchID As Integer
		Dim frmAddRelatedInstance As frmAddRelated
		
		Try 
			
			lMatchID = grdAllocations.CurrentRow().Cells(0).Value
			
			frmAddRelatedInstance = New frmAddRelated()
			
			frmAddRelatedInstance.MatchID = lMatchID
			frmAddRelatedInstance.AddType = ACATAddRelated
			
			frmAddRelatedInstance.ShowDialog()
			
			If Information.IsArray(frmAddRelatedInstance.Transactions) Then
				
				For lCount As Integer = frmAddRelatedInstance.Transactions.GetLowerBound(0) To frmAddRelatedInstance.Transactions.GetUpperBound(0)
					sSQL = New StringBuilder("")
					sSQL.Append("DECLARE @match_id INT" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("DECLARE @transdetail_id INT" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("DECLARE @allocation_id INT" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("DECLARE @cashlistitem_id INT" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("DECLARE @allocationdetail_id INT" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("SELECT @match_id = " & lMatchID & Strings.Chr(13) & Strings.Chr(10))

					sSQL.Append("SELECT @transdetail_id = " & CStr(frmAddRelatedInstance.Transactions(lCount)) & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("SELECT @allocation_id = MAX(ISNULL(ad.allocation_id,0))" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("FROM transmatch tm" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("JOIN allocationdetail ad" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    ON ad.allocationdetail_id = tm.allocationdetail_id" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    AND ad.transdetail_id = tm.transdetail_id" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("WHERE tm.match_id = @match_id" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("SELECT @cashlistitem_id = MAX(ISNULL(cashlistitem_id,0))" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("FROM allocationdetail" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("WHERE allocation_id = @allocation_id" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("IF @cashlistitem_id = 0" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("BEGIN" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    SELECT @cashlistitem_id = NULL" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("END" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("INSERT INTO allocationdetail" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("(" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    cashlistitem_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    allocation_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    original_currency," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    transdetail_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    documenttype_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    document_ref," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    accounting_date," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    original_date," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    allocate_to_base," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    orig_base_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    orig_base_amount_unrounded," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    orig_ccy_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    orig_ccy_amount_unrounded," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    orig_xrate," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    effective_xrate," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    os_base_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    os_ccy_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    alloc_base_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    alloc_ccy_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    fully_matched," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    write_off_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    write_off_reason_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    new_os_ccy_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    new_os_base_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    loss_gain_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    is_primary," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    euro_currency_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    euro_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    euro_base_xrate," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    euro_ccy_xrate" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append(")" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("SELECT" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    @cashlistitem_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    @allocation_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.currency_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.transdetail_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    d.documenttype_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    d.document_ref," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.accounting_date," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.accounting_date," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    0," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.base_amount_unrounded," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.currency_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.currency_amount_unrounded," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.currency_base_xrate," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.currency_base_xrate," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.currency_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.currency_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.fully_matched," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    NULL," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    NULL," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    0," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    0," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    0," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    0," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.euro_currency_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.euro_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.euro_base_xrate," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    td.euro_ccy_xrate" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("FROM transdetail td" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("JOIN document d" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    ON d.document_id = td.document_id" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("WHERE transdetail_id = @transdetail_id" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("SELECT @allocationdetail_id = @@IDENTITY" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("INSERT INTO transmatch" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("(" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    currency_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    allocationdetail_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    transdetail_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    match_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    base_match_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    currency_match_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    currency_match_xrate," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    is_reversed" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append(")" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("SELECT" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    currency_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    @allocationdetail_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    transdetail_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    @match_id," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    currency_amount," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    currency_base_xrate," & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("    NULL" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("FROM transdetail" & Strings.Chr(13) & Strings.Chr(10))
					sSQL.Append("WHERE transdetail_id = @transdetail_id" & Strings.Chr(13) & Strings.Chr(10))
					
					oCommand = New SqlCommand()
					
					oCommand.Connection = m_oConnection
					oCommand.CommandText = sSQL.ToString()
					oCommand.CommandTimeout = kcTimeout
					oCommand.ExecuteNonQuery()
					
				Next 
				
				RefreshTransactionsGrid()
				
			End If
			
			frmAddRelatedInstance = Nothing
		
		Catch 
			
			
			
			MessageBox.Show("Script failed to run", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
		
		
	End Sub
	
	Private Sub cmdCopyToClipboard_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCopyToClipboard.Click
		
		Dim lCount As Integer = 0
		Dim sClipboard As String = ""
		Dim sTemp As String = "Account Code"
		sClipboard = sClipboard & sTemp & New String(" "c, 20 - sTemp.Length)
		
		sTemp = "Document Ref"
		sClipboard = sClipboard & sTemp & New String(" "c, 25 - sTemp.Length)
		
		sTemp = "Company"
		sClipboard = sClipboard & sTemp & New String(" "c, 10 - sTemp.Length)
		
		sTemp = "Spare"
		sClipboard = sClipboard & sTemp & New String(" "c, 20 - sTemp.Length)
		
		sTemp = "Currency"
		sClipboard = sClipboard & sTemp & New String(" "c, 10 - sTemp.Length)
		
		sTemp = "Original Amount"
		sClipboard = sClipboard & sTemp & New String(" "c, 20 - sTemp.Length)
		
		sTemp = "Allocated Amount"
		sClipboard = sClipboard & sTemp & New String(" "c, 20 - sTemp.Length)
		
		sTemp = "Outstanding Amount"
		sClipboard = sClipboard & sTemp & New String(" "c, 20 - sTemp.Length)
		
		sClipboard = sClipboard & Strings.Chr(13) & Strings.Chr(10)
		
        grdTransactions.MoveFirst()
        'TODO ExtendedDataGridView



        'Dim vBookmark As Object = grdTransactions.GetBookmark(lCount)

        'Do While Not (Convert.IsDBNull(vBookmark) Or IsNothing(vBookmark))

        'sClipboard = sClipboard & CStr(grdTransactions.Columns(ACTCAccountCode).CellValue(vBookmark))


        '	sClipboard = sClipboard & CStr(grdTransactions.Columns(ACTCDocumentRef).CellValue(vBookmark))

        '	sClipboard = sClipboard & CStr(grdTransactions.Columns(ACTCCompany).CellValue(vBookmark))

        '	sClipboard = sClipboard & CStr(grdTransactions.Columns(ACTCSpare).CellValue(vBookmark)) & New String(" "c, 20 - Strings.Len(CStr(grdTransactions.Columns(ACTCSpare).CellValue(vBookmark))))

        '	sClipboard = sClipboard & CStr(grdTransactions.Columns(ACTCCurrency).CellValue(vBookmark))


        '	sTemp = StringsHelper.Format(grdTransactions.Columns(ACTCOriginalAmount).CellValue(vBookmark), "#0.00##")
        '	sClipboard = sClipboard & sTemp & New String(" "c, 20 - sTemp.Length)


        '	sTemp = StringsHelper.Format(grdTransactions.Columns(ACTCAllocatedAmount).CellValue(vBookmark), "#0.00##")
        '	sClipboard = sClipboard & sTemp & New String(" "c, 20 - sTemp.Length)


        '	sTemp = StringsHelper.Format(grdTransactions.Columns(ACTCOSAmount).CellValue(vBookmark), "#0.00##")
        '	sClipboard = sClipboard & sTemp & New String(" "c, 20 - sTemp.Length)

        '	sClipboard = sClipboard & Strings.Chr(13) & Strings.Chr(10)

        '	lCount += 1



        '	vBookmark = grdTransactions.GetBookmark(lCount)
        'Loop 


        My.Computer.Clipboard.SetText(sClipboard)

    End Sub
	
	Private Sub cmdDeleteAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteAll.Click
		Dim oCommand As SqlCommand
		Dim sSQL As String = ""
		Dim lMatchID As Integer
		Dim sMessage As String = ""
		Dim lAnswer As DialogResult
		Dim lCount As Integer
		Dim vBookmark As Object
		Dim sLogLine As String = ""
		
		Try 
			
			sMessage = ""
			sMessage = sMessage & "This will delete the whole allocation." & Strings.Chr(13) & Strings.Chr(10)
			sMessage = sMessage & Strings.Chr(13) & Strings.Chr(10)
			sMessage = sMessage & "Are you sure you wish to continue?"
			
			lAnswer = MessageBox.Show(sMessage, "Delete Allocation", MessageBoxButtons.YesNo)
			
			If lAnswer = System.Windows.Forms.DialogResult.No Then
				Exit Sub
			End If
			
			lMatchID = grdAllocations.CurrentRow().Cells(0).Value
			
			sSQL = ""
			sSQL = sSQL & "DECLARE @allocation_id INT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "SELECT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    @allocation_id = MAX(allocation_id)" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "FROM allocationdetail ad" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "JOIN transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    ON tm.allocationdetail_id = ad.allocationdetail_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    AND tm.transdetail_id = ad.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "WHERE match_id = " & CStr(lMatchID) & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DELETE" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "FROM allocationdetail" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "WHERE allocation_id = @allocation_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DELETE" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "FROM allocation" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "WHERE allocation_id = @allocation_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DELETE" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "FROM transmatch" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "WHERE match_id = " & CStr(lMatchID) & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DELETE" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "FROM matchgroup" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "WHERE match_id = " & CStr(lMatchID) & Strings.Chr(13) & Strings.Chr(10)
			
			oCommand = New SqlCommand()
			
			oCommand.Connection = m_oConnection
			oCommand.CommandText = sSQL
			oCommand.CommandTimeout = kcTimeout
			oCommand.ExecuteNonQuery()
			
			'Log changes
			OpenLogFile()
			Dim m_tsLogWriter As StreamWriter = New StreamWriter(m_tsLog)
			m_tsLogWriter.WriteLine("DELETE ALLOCATION")
			
			lCount = 0
			grdTransactions.MoveFirst()



            'ToDoList -To be handled at runtime
            'vBookmark = grdTransactions.GetBookmark(lCount)

			Do While Not (Convert.IsDBNull(vBookmark) Or IsNothing(vBookmark))
				sLogLine = ""
				sLogLine = sLogLine & grdAllocations.CurrentRow().Cells(0).Value & "|"

                'ToDoList -To be handled at runtime
                'sLogLine = sLogLine & CStr(grdTransactions.Columns(ACTCAccountCode).CellValue(vBookmark)) & "|"


                'ToDoList -To be handled at runtime
                'sLogLine = sLogLine & CStr(grdTransactions.Columns(ACTCDocumentRef).CellValue(vBookmark)) & "|"

                'ToDoList -To be handled at runtime
                'sLogLine = sLogLine & CStr(grdTransactions.Columns(ACTCCompany).CellValue(vBookmark)) & "|"

                'ToDoList -To be handled at runtime
                'sLogLine = sLogLine & StringsHelper.Format(grdTransactions.Columns(ACTCAllocatedAmount).CellValue(vBookmark), "#0.00")
				
				m_tsLogWriter.WriteLine(sLogLine)
				
				lCount += 1



                'ToDoList -To be handled at runtime
                'vBookmark = grdTransactions.GetBookmark(lCount)
			Loop 
			m_tsLogWriter.WriteLine("*****")
			
			'Refresh the grids
			If grdAllocations.RowsCount = 1 Then
				RefreshAccountsGrid()
			Else
				RefreshAllocationsGrid()
			End If
		
		Catch 
			
			
			
			MessageBox.Show("Script failed to run", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
		
		
	End Sub
	
	Private Sub cmdDeleteSingle_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteSingle.Click
		Dim oCommand As SqlCommand
		Dim sSQL As String = ""
		Dim lTransMatchID As Integer
		Dim sMessage As String = ""
		Dim lAnswer As DialogResult
		Dim sLogLine As String = ""
		
		Try 
			
			If grdTransactions.RowsCount = 1 Then
				cmdDeleteAll_Click(cmdDeleteAll, New EventArgs())
				Exit Sub
			End If
			
			sMessage = ""
			sMessage = sMessage & "This will remove the selected transaction from the allocation." & Strings.Chr(13) & Strings.Chr(10)
			sMessage = sMessage & Strings.Chr(13) & Strings.Chr(10)
			sMessage = sMessage & "Are you sure you wish to continue?"
			
			lAnswer = MessageBox.Show(sMessage, "Remove Transaction From Allocation", MessageBoxButtons.YesNo)
			
			If lAnswer = System.Windows.Forms.DialogResult.No Then
				Exit Sub
			End If
			
			lTransMatchID = grdTransactions.CurrentRow().Cells(ACTCTransMatchID).Value
			
			sSQL = ""
			sSQL = sSQL & "DECLARE @NoOfTM INT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "SELECT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    @NoOfTM = SUM(1)" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "FROM transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "JOIN allocationdetail ad" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    ON ad.allocationdetail_id = tm.allocationdetail_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    AND ad.transdetail_id = tm.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "JOIN transmatch tm2" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    ON tm2.allocationdetail_id = ad.allocationdetail_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    AND tm2.transdetail_id = ad.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "WHERE tm2.transmatch_id = " & CStr(lTransMatchID) & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "IF ISNULL(@NoOfTM,0) = 1" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "BEGIN" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    DELETE allocationdetail" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    FROM allocationdetail ad" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    JOIN transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "        ON tm.allocationdetail_id = ad.allocationdetail_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "        AND tm.transdetail_id = ad.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    WHERE tm.transmatch_id = " & CStr(lTransMatchID) & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "END" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DELETE" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "FROM transmatch" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "WHERE transmatch_id = " & CStr(lTransMatchID) & Strings.Chr(13) & Strings.Chr(10)
			
			oCommand = New SqlCommand()
			
			oCommand.Connection = m_oConnection
			oCommand.CommandText = sSQL
			oCommand.CommandTimeout = kcTimeout
			oCommand.ExecuteNonQuery()
			
			'Log changes
			OpenLogFile()
			Dim m_tsLogWriter As StreamWriter = New StreamWriter(m_tsLog)
			m_tsLogWriter.WriteLine("REMOVE TRANSACTION")
			
			sLogLine = ""
			sLogLine = sLogLine & grdAllocations.CurrentRow().Cells(0).Value & "|"
			sLogLine = sLogLine & grdTransactions.CurrentRow().Cells(ACTCAccountCode).Value & "|"
			sLogLine = sLogLine & grdTransactions.CurrentRow().Cells(ACTCDocumentRef).Value & "|"
			sLogLine = sLogLine & grdTransactions.CurrentRow().Cells(ACTCCompany).Value & "|"
			sLogLine = sLogLine & StringsHelper.Format(grdTransactions.CurrentRow().Cells(ACTCAllocatedAmount).Value, "#0.00")
			
			m_tsLogWriter.WriteLine(sLogLine)
			m_tsLogWriter.WriteLine("*****")
			
			RefreshTransactionsGrid()
		
		Catch 
			
			
			
			MessageBox.Show("Script failed to run", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
		
		
	End Sub
	
	Private Sub cmdEditAmount_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditAmount.Click
		Dim oCommand As SqlCommand
		Dim sSQL As String = ""
		Dim lTransMatchID As Integer
		Dim cOriginalAmount, cAllocatedAmount, cOSAmount, cCurrencyAllocatedAmount As Decimal
		Dim frmEditInstance As frmEdit
		Dim sLogLine As String = ""
		
		Try 
			
			cOriginalAmount = grdTransactions.CurrentRow().Cells(ACTCOriginalAmount).Value
			cAllocatedAmount = grdTransactions.CurrentRow().Cells(ACTCAllocatedAmount).Value
			lTransMatchID = grdTransactions.CurrentRow().Cells(ACTCTransMatchID).Value
			cOSAmount = grdTransactions.CurrentRow().Cells(ACTCOSAmount).Value
			cCurrencyAllocatedAmount = grdTransactions.CurrentRow().Cells(ACTCCurrencyAllocatedAmount).Value
			
			If cCurrencyAllocatedAmount <> cAllocatedAmount Then
				MessageBox.Show("Cannot edit allocated amounts of non-base currency transactions.", "Cannot Edit Amount", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Else
				frmEditInstance = New frmEdit()
				
				frmEditInstance.OriginalAmount = cOriginalAmount
				frmEditInstance.AllocatedAmount = cAllocatedAmount
				frmEditInstance.OSAmount = cOSAmount
				
				frmEditInstance.ShowDialog()
				
				If frmEditInstance.AllocatedAmount <> cAllocatedAmount Then
					
					cAllocatedAmount = frmEditInstance.AllocatedAmount
					
					sSQL = ""
					sSQL = sSQL & "UPDATE transmatch" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "SET base_match_amount = " & CStr(cAllocatedAmount) & "," & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "    currency_match_amount = " & CStr(cAllocatedAmount) & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "WHERE transmatch_id = " & CStr(lTransMatchID) & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "UPDATE ad" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "SET ad.alloc_base_amount = " & CStr(cAllocatedAmount) & "," & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "    ad.alloc_ccy_amount = " & CStr(cAllocatedAmount) & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "FROM allocationdetail ad" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "JOIN transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "    ON tm.allocationdetail_id = ad.allocationdetail_id" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "    AND tm.transdetail_id = ad.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "WHERE transmatch_id = " & CStr(lTransMatchID) & Strings.Chr(13) & Strings.Chr(10)
					
					oCommand = New SqlCommand()
					
					oCommand.Connection = m_oConnection
					oCommand.CommandText = sSQL
					oCommand.CommandTimeout = kcTimeout
					oCommand.ExecuteNonQuery()
					
					'Log changes
					OpenLogFile()
					Dim m_tsLogWriter As StreamWriter = New StreamWriter(m_tsLog)
					m_tsLogWriter.WriteLine("EDIT ALLOCATED AMOUNT")
					
					sLogLine = ""
					sLogLine = sLogLine & grdAllocations.CurrentRow().Cells(0).Value & "|"
					sLogLine = sLogLine & grdTransactions.CurrentRow().Cells(ACTCAccountCode).Value & "|"
					sLogLine = sLogLine & grdTransactions.CurrentRow().Cells(ACTCDocumentRef).Value & "|"
					sLogLine = sLogLine & grdTransactions.CurrentRow().Cells(ACTCCompany).Value & "|"
					sLogLine = sLogLine & StringsHelper.Format(grdTransactions.CurrentRow().Cells(ACTCAllocatedAmount).Value, "#0.00") & "|"
					sLogLine = sLogLine & StringsHelper.Format(cAllocatedAmount, "#0.00")
					
					m_tsLogWriter.WriteLine(sLogLine)
					m_tsLogWriter.WriteLine("*****")
					
					RefreshTransactionsGrid()
					
				End If
				
				frmEditInstance = Nothing
			End If
		
		Catch 
			
			
			
			MessageBox.Show("Script failed to run", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
		
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExit.Click
		Me.Close()
	End Sub
	
	Private Sub cmdMergeAllocations_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMergeAllocations.Click
		Dim vBookmark As Object
		Dim lTransCount, lMatchID, lNewMatchID As Integer
		Dim sSQL As New StringBuilder
		Dim oCommand As SqlCommand
		Dim avBookMarks As Object
		Dim sLogLine As String = ""
		
		Try 
			

            'ToDoList -To be handled at runtime SelBookmarks is not a method of grdAllocations
            '' If grdAllocations.SelBookmarks.Count < 2 Then
            'MessageBox.Show("You need to select at least two allocations", "Not Enough Allocations Line Selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            'Else

            ''Get selected rows bookmarks from grid.

            'ReDim avBookMarks(grdAllocations.SelBookmarks.Count - 1)

            'For lCount As Integer = 0 To grdAllocations.SelBookmarks.Count - 1



            '    avBookMarks.SetValue(grdAllocations.SelBookmarks.Item(lCount), lCount)
            'Next

            'Log changes
            OpenLogFile()
            Dim m_tsLogWriter As StreamWriter = New StreamWriter(m_tsLog)
            m_tsLogWriter.WriteLine("MERGE ALLOCATIONS")
            'Loop around each allocation.

            For lCount As Integer = avBookMarks.GetLowerBound(0) To avBookMarks.GetUpperBound(0)



                'grdAllocations.Bookmark = avBookMarks(lCount)
                RefreshTransactionsGrid()

                lTransCount = 0
                grdTransactions.MoveFirst()



                'vBookmark = grdTransactions.GetBookmark(lTransCount)

                Do While Not (Convert.IsDBNull(vBookmark) Or IsNothing(vBookmark))
                    sLogLine = ""
                    sLogLine = sLogLine & grdAllocations.CurrentRow().Cells(0).Value & "|"

                    ' sLogLine = sLogLine & CStr(grdTransactions.Columns(ACTCAccountCode).CellValue(vBookmark)) & "|"

                    ' sLogLine = sLogLine & CStr(grdTransactions.Columns(ACTCDocumentRef).CellValue(vBookmark)) & "|"

                    'sLogLine = sLogLine & CStr(grdTransactions.Columns(ACTCCompany).CellValue(vBookmark)) & "|"

                    ' sLogLine = sLogLine & StringsHelper.Format(grdTransactions.Columns(ACTCAllocatedAmount).CellValue(vBookmark), "#0.00")

                    m_tsLogWriter.WriteLine(sLogLine)

                    lTransCount += 1



                    ' vBookmark = grdTransactions.GetBookmark(lTransCount)
                Loop

            Next
            m_tsLogWriter.WriteLine("*****")


            For lCount As Integer = avBookMarks.GetLowerBound(0) To avBookMarks.GetUpperBound(0)



                vBookmark = avBookMarks(lCount)

                'lMatchID = CInt(grdAllocations.Columns(0).CellValue(vBookmark))

                If lCount = 0 Then
                    lNewMatchID = lMatchID
                Else

                    sSQL = New StringBuilder("")
                    sSQL.Append("DECLARE @match_id INT" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("DECLARE @new_match_id INT" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("DECLARE @match_date DATETIME" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("DECLARE @new_match_date DATETIME" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("DECLARE @allocation_id INT" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("DECLARE @new_allocation_id INT" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("DECLARE @allocation_date DATETIME" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("DECLARE @new_allocation_date DATETIME" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("SELECT @match_id = " & lMatchID & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("SELECT @new_match_id = " & lNewMatchID & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("/*Merge AllocationDetail Records*/" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("SELECT @allocation_id = MAX(ad.allocation_id)" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("FROM allocationdetail ad" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("JOIN transmatch tm" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("    ON tm.allocationdetail_id = ad.allocationdetail_id" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("    AND tm.transdetail_id = ad.transdetail_id" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("WHERE tm.match_id = @match_id" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("SELECT @new_allocation_id = MAX(ad.allocation_id)" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("FROM allocationdetail ad" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("JOIN transmatch tm" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("    ON tm.allocationdetail_id = ad.allocationdetail_id" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("    AND tm.transdetail_id = ad.transdetail_id" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("WHERE tm.match_id = @new_match_id" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("SELECT @allocation_date = allocation_date" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("FROM allocation" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("WHERE allocation_id = @allocation_id" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("SELECT @new_allocation_date = allocation_date" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("FROM allocation" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("WHERE allocation_id = @new_allocation_id" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("IF @allocation_date > @new_allocation_date" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("BEGIN" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("    SELECT @new_allocation_date = @allocation_date" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("    UPDATE allocation" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("    SET allocation_date = @new_allocation_date" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("    WHERE allocation_id = @new_allocation_id" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("END" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("UPDATE allocationdetail" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("SET allocation_id = @new_allocation_id" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("WHERE allocation_id = @allocation_id" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("IF @allocation_id <> @new_allocation_id" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("BEGIN" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("    DELETE allocation" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("    WHERE allocation_id = @allocation_id" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("END" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("/*Merge TransMatch Records*/" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("SELECT @match_date = match_date" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("FROM matchgroup" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("WHERE match_id = @match_id" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("SELECT @new_match_date = match_date" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("FROM matchgroup" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("WHERE match_id = @new_match_id" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("IF @match_date > @new_match_date" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("BEGIN" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("    SELECT @new_match_date = @match_date" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("    UPDATE matchgroup" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("    SET match_date = @new_match_date" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("    WHERE match_id = @new_match_id" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("END" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("UPDATE transmatch" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("SET match_id = @new_match_id" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("WHERE match_id = @match_id" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("DELETE matchgroup" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("WHERE match_id = @match_id" & Strings.Chr(13) & Strings.Chr(10))

                    oCommand = New SqlCommand()

                    oCommand.Connection = m_oConnection
                    oCommand.CommandText = sSQL.ToString()
                    oCommand.CommandTimeout = kcTimeout
                    oCommand.ExecuteNonQuery()

                End If
            Next

            RefreshAllocationsGrid()

            If grdAllocations.RowsCount = 0 Then
                RefreshAccountsGrid()
            End If

            'End If

        Catch



            MessageBox.Show("Script failed to run", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
		
		
	End Sub
	
	Private Sub cmdRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRefresh.Click
		RefreshAccountsGrid()
	End Sub
	
	
	Private Sub cmdWriteoff_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdWriteoff.Click
		Dim oCommand As SqlCommand
		Dim sSQL As String = ""
		Dim lMatchID As Integer
		Dim sMessage As String = ""
		Dim lAnswer As DialogResult
		Dim lCount As Integer
        Dim vBookmark As Object
		Dim sLogLine As String = ""
		Dim cWriteOffAmount As Decimal
		Dim sLedgerName As String = ""
		Dim lAccountID, lTransMatchID As Integer
		
		Try 
			
			sLedgerName = grdAccounts.CurrentRow().Cells(ACACLedgerName).Value.Trim()
			
			If sLedgerName <> "Client" And sLedgerName <> "Purchase" Then
				
				sMessage = ""
				sMessage = sMessage & "You cannot create a writeoff for this type of account." & Strings.Chr(13) & Strings.Chr(10)
				sMessage = sMessage & "Only client and purchase accounts can be written off."
				
				MessageBox.Show(sMessage, "Write Off", MessageBoxButtons.OK)
				
				Exit Sub
			End If
			
            'cWriteOffAmount = CDec(grdTransactions.Columns(ACTCAllocatedAmount).FooterText) * -1
			
			sMessage = ""
			sMessage = sMessage & "This will create a write off transaction with an amount of " & StringsHelper.Format(cWriteOffAmount, "0.00") & " and add it into the current allocation." & Strings.Chr(13) & Strings.Chr(10)
			sMessage = sMessage & Strings.Chr(13) & Strings.Chr(10)
			sMessage = sMessage & "Are you sure you wish to continue?"
			
			lAnswer = MessageBox.Show(sMessage, "Write Off", MessageBoxButtons.YesNo)
			
			If lAnswer = System.Windows.Forms.DialogResult.No Then
				Exit Sub
			End If
			
			lMatchID = grdAllocations.CurrentRow().Cells(ACAlCMatchID).Value
			lAccountID = grdAccounts.CurrentRow().Cells(ACACAccountID).Value
			
			sSQL = ""
			sSQL = sSQL & "DECLARE @company_id INT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DECLARE @document_id INT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DECLARE @document_date DATETIME" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DECLARE @ref_date DATETIME" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DECLARE @accounting_date DATETIME" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DECLARE @match_id INT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DECLARE @document_ref VARCHAR(25)" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DECLARE @actnumber_id INT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DECLARE @actnumber_str VARCHAR(25)" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DECLARE @transdetail_id INT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DECLARE @wo_transdetail_id INT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DECLARE @account_code VARCHAR(20)" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DECLARE @account_id INT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DECLARE @amount MONEY" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DECLARE @wo_account_code VARCHAR(20)" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DECLARE @wo_account_id INT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DECLARE @wo_amount MONEY" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DECLARE @period_id INT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DECLARE @current_period_id INT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DECLARE @transmatch_id INT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DECLARE @allocationdetail_id INT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "DECLARE @allocation_id INT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "SELECT @match_id = " & CStr(lMatchID) & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "SELECT @account_id = " & CStr(lAccountID) & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "SELECT @amount = " & CStr(cWriteOffAmount) & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "SELECT @wo_amount = @amount * -1" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "SELECT @company_id = MIN(td.company_id)" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "FROM transdetail td" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "JOIN transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & " ON tm.transdetail_id = td.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "WHERE tm.match_id = @match_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "SELECT @allocation_id = MAX(allocation_id)" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "FROM allocationdetail ad" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "JOIN transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & " ON tm.allocationdetail_id = ad.allocationdetail_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & " AND tm.transdetail_id = ad.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "WHERE tm.match_id = @match_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "SELECT " & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & " @wo_account_code =" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     CASE l.ledger_short_name" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "         WHEN 'SA' THEN 'N4092'" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "         WHEN 'PU' THEN 'N5092'" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     END" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "FROM account a" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "JOIN ledger l" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & " ON l.ledger_id = a.ledger_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "WHERE a.account_id = @account_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & " " & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "SELECT @wo_account_id = a.account_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "FROM account a" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "WHERE a.short_code = @wo_account_code " & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "SELECT @document_date = GETDATE()" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "SELECT " & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & " @ref_date = match_date" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "FROM matchgroup" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "WHERE match_id = @match_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "SELECT @period_id = period_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "FROM period" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "WHERE company_id = @company_id " & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "AND period_end_date =" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & " (" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     SELECT MIN(period_end_date)" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     FROM period" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     WHERE company_id = @company_id " & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     AND period_end_date >= @document_date" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & " )" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "SELECT @current_period_id = current_period_id " & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "FROM ledger " & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "WHERE ledger_id = " & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & " (" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "         SELECT ledger_id " & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "         FROM account " & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "         WHERE account_id = @account_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & " )" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "EXEC spu_ACT_Get_Period_Start_Date" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @period_id = @current_period_id," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @period_start_date = @accounting_date OUTPUT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "IF @ref_date > @accounting_date" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "BEGIN" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & " SELECT @accounting_date = @ref_date" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "END" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "/*Create New Document*/" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "SELECT @actnumber_id = NULL" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "EXEC spe_ACTNumber_add   " & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @actnumber_id OUTPUT," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     14,/*SWD*/" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     1,/*sirius*/" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @company_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "SELECT @actnumber_str = CONVERT(VARCHAR(25),@actnumber_id)" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "SELECT @document_ref = LEFT('SWD00000000',11 - LEN(@actnumber_str)) + @actnumber_str" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "SELECT @document_id = NULL" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "EXEC spu_ACT_Add_Document    " & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @document_id = @document_id OUTPUT," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @company_id = @company_id," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @postingstatus_id = 3," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @documenttype_id = 14," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @auditset_id = NULL," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @batch_id = NULL, " & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @document_ref = @document_ref," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @document_date = @document_date," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @created_date = @document_date," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @authorised_date = @document_date, " & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @comment = 'Write Off Document (bACTMisallocationsHelper)', " & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @write_off_reason_id = 1," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @reason = '' " & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "EXEC spu_ACT_Add_TransDetail" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @transdetail_id = @transdetail_id OUTPUT," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @account_id = @account_id," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @postingstatus_id = 3," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @company_id = @company_id," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @currency_id = 26, /*Hard coded to GBP*/" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @period_id = @period_id," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @document_id = @document_id," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @document_sequence = 1," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @accounting_date = @accounting_date," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @amount = @amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @base_amount_unrounded = @amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @fully_matched = 0," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @currency_amount = @amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @currency_amount_unrounded = @amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @currency_base_xrate = 1," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @euro_currency_id = NULL," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @euro_amount = 0," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @euro_base_xrate = 1," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @euro_ccy_xrate = 1," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @comment = 'Write Off Transaction'," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @insurance_ref = NULL," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @operator_id = 1," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @purchase_order_no = NULL," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @purchase_invoice_no = NULL," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @department = NULL," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @spare = ''," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @ref_date = @ref_date," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @ref_amount = 0," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @ref_quantity = 0," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @ref_units = 0," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @insurance_ref_index = NULL," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @department_id = NULL" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "EXEC spu_ACT_Add_TransDetail" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @transdetail_id = @wo_transdetail_id OUTPUT," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @account_id = @wo_account_id," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @postingstatus_id = 3," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @company_id = @company_id," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @currency_id = 26, /*Hard coded to GBP*/" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @period_id = @period_id," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @document_id = @document_id," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @document_sequence = 2," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @accounting_date = @accounting_date," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @amount = @wo_amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @base_amount_unrounded = @wo_amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @fully_matched = 0," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @currency_amount = @wo_amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @currency_amount_unrounded = @wo_amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @currency_base_xrate = 1," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @euro_currency_id = NULL," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @euro_amount = 0," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @euro_base_xrate = 1," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @euro_ccy_xrate = 1," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @comment = 'Matching Write Off Transaction'," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @insurance_ref = NULL," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @operator_id = 1," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @purchase_order_no = NULL," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @purchase_invoice_no = NULL," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @department = NULL," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @spare = ''," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @ref_date = @ref_date," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @ref_amount = 0," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @ref_quantity = 0," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @ref_units = 0," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @insurance_ref_index = NULL," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @department_id = NULL" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     " & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     " & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "EXEC spu_ACT_Add_AllocationDetail" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @allocationdetail_id = @allocationdetail_id OUTPUT," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @cashlistitem_id = NULL," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @allocation_id = @allocation_id," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @original_currency = 26, /*Hard coded to GBP*/" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @transdetail_id = @transdetail_id," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @documenttype_id = 14," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @document_ref = @document_ref," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @accounting_date = @ref_date," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @original_date = @document_date," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @allocate_to_base = 0," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @orig_base_amount = @amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @orig_base_amount_unrounded = @amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @orig_ccy_amount = @amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @orig_ccy_amount_unrounded = @amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @orig_xrate = 1," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @effective_xrate = 1," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @os_base_amount = @amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @os_ccy_amount = @amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @alloc_base_amount = @amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @alloc_ccy_amount = @amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @fully_matched = 1," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @write_off_amount = NULL," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @write_off_reason_id = NULL," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @new_os_ccy_amount = 0," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @new_os_base_amount = 0," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @loss_gain_amount = 0," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @is_primary = 0," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @euro_currency_id = 0," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @euro_amount = 0," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @euro_base_xrate = 1," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @euro_ccy_xrate = 1" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     " & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "EXEC spu_ACT_Add_TransMatch" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @transmatch_id = @transmatch_id OUTPUT," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @allocationdetail_id =@allocationdetail_id," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @transdetail_id = @transdetail_id," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @match_id = @match_id," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @currency_id = 26, /*Hard coded to GBP*/" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @base_match_amount = @amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @currency_match_amount = @amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "     @currency_match_xrate = 1" & Strings.Chr(13) & Strings.Chr(10)
			
			oCommand = New SqlCommand()
			
			oCommand.Connection = m_oConnection
			oCommand.CommandText = sSQL
			oCommand.CommandTimeout = kcTimeout
			oCommand.ExecuteNonQuery()
			
			'Refresh the grids
			RefreshTransactionsGrid()
			
			'Log changes
			OpenLogFile()
			Dim m_tsLogWriter As StreamWriter = New StreamWriter(m_tsLog)
			m_tsLogWriter.WriteLine("WRITE OFF")
			
			lCount = 0
			lTransMatchID = 0
			grdTransactions.MoveFirst()



            'vBookmark = grdTransactions.GetBookmark(lCount)

			Do While Not (Convert.IsDBNull(vBookmark) Or IsNothing(vBookmark))
				

                'If lTransMatchID < CDbl(grdTransactions.Columns(ACTCTransMatchID).CellValue(vBookmark)) Then

                '	lTransMatchID = CInt(grdTransactions.Columns(ACTCTransMatchID).CellValue(vBookmark))


                '	vWOBookmark = vBookmark
                'End If
				
				lCount += 1



                '	vBookmark = grdTransactions.GetBookmark(lCount)
			Loop 
			
			sLogLine = ""
			sLogLine = sLogLine & grdAllocations.CurrentRow().Cells(ACAlCMatchID).Value & "|"

            'sLogLine = sLogLine & CStr(grdTransactions.Columns(ACTCAccountCode).CellValue(vWOBookmark)) & "|"

            'sLogLine = sLogLine & CStr(grdTransactions.Columns(ACTCDocumentRef).CellValue(vWOBookmark)) & "|"

            'sLogLine = sLogLine & CStr(grdTransactions.Columns(ACTCCompany).CellValue(vWOBookmark)) & "|"

            'sLogLine = sLogLine & StringsHelper.Format(grdTransactions.Columns(ACTCAllocatedAmount).CellValue(vWOBookmark), "#0.00")
			
			m_tsLogWriter.WriteLine(sLogLine)
			
			m_tsLogWriter.WriteLine("*****")
		
		Catch 
			
			
			
			MessageBox.Show("Script failed to run", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
		
		
	End Sub
	

	Private Sub grdAccounts_CellFormatting(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellFormattingEventArgs) Handles grdAccounts.CellFormatting
		Dim ColIndex As Integer = eventArgs.ColumnIndex
		Dim Value As Object = eventArgs.Value
		Dim Bookmark As DatagridViewRow = Nothing
		If eventArgs.RowIndex > 0 Then
			Bookmark = grdAccounts.Rows(eventArgs.RowIndex)
		End If
		
		Select Case ColIndex
			Case ACACMisAllocAmount, ACACTransdetailAmount, ACACDisplayAmount
				

				If Math.Round(CDbl(CDbl(Value)), 2) = CDbl(Value) Then


					Value = StringsHelper.Format(Value, "#0.00")
				Else


					Value = StringsHelper.Format(Value, "#0.0000")
				End If
				
		End Select
		
	End Sub
	
	Private Sub grdAccounts_CellEnter(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellEventArgs) Handles grdAccounts.CellEnter
		Dim LastRow As DataGridViewRow = Nothing
		Dim LastCol As Integer = -1
		If Not IsNothing(grdAccounts.PreviousCell) Then
			If grdAccounts.PreviousCell.RowIndex > grdAccounts.Rows.Count Then
				LastRow = grdAccounts.Rows(grdAccounts.PreviousCell.RowIndex)
			End If
			LastCol = grdAccounts.PreviousCell.ColumnIndex
		End If
		RefreshAllocationsGrid()
	End Sub
	

	Private Sub grdAllocations_CellFormatting(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellFormattingEventArgs) Handles grdAllocations.CellFormatting
		Dim ColIndex As Integer = eventArgs.ColumnIndex
		Dim Value As Object = eventArgs.Value
		Dim Bookmark As DatagridViewRow = Nothing
		If eventArgs.RowIndex > 0 Then
			Bookmark = grdAllocations.Rows(eventArgs.RowIndex)
		End If
		
		Select Case ColIndex
			Case ACAlCMisAllocAmount
				

				If Math.Round(CDbl(CDbl(Value)), 2) = CDbl(Value) Then


					Value = StringsHelper.Format(Value, "#0.00")
				Else


					Value = StringsHelper.Format(Value, "#0.0000")
				End If
				
		End Select
		
	End Sub
	
	Private Sub grdAllocations_CellEnter(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellEventArgs) Handles grdAllocations.CellEnter
		Dim LastRow As DataGridViewRow = Nothing
		Dim LastCol As Integer = -1
		If Not IsNothing(grdAllocations.PreviousCell) Then
			If grdAllocations.PreviousCell.RowIndex > grdAllocations.Rows.Count Then
				LastRow = grdAllocations.Rows(grdAllocations.PreviousCell.RowIndex)
			End If
			LastCol = grdAllocations.PreviousCell.ColumnIndex
		End If
		RefreshTransactionsGrid()
	End Sub
	

	Private Sub frmAccounts_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Dim sMessage As String = ""
		
		
		Dim sPath As String = "SOFTWARE\PM\SiriusArchitecture\Server\Databases\SiriusSolutions"
		
		Dim sSetting As String = "Provider"
		Dim sProvider As String = gPMFunctions.QueryKeyValue(gPMConstants.HKEY_LOCAL_MACHINE, sPath, sSetting)
		
		sSetting = "Server"
		Dim sDataSource As String = gPMFunctions.QueryKeyValue(gPMConstants.HKEY_LOCAL_MACHINE, sPath, sSetting)
		
		sSetting = "Database"
		Dim sInitialCatalog As String = gPMFunctions.QueryKeyValue(gPMConstants.HKEY_LOCAL_MACHINE, sPath, sSetting)
		
		If sProvider.Trim() = "" Or sDataSource.Trim() = "" Or sInitialCatalog.Trim() = "" Then
			MessageBox.Show("Unable to connect to database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Environment.Exit(0)
		End If
		
		If Not CheckPassword() Then
			Environment.Exit(0)
		End If
		
		' Open the connection.
		m_oConnection = New SqlConnection()

		m_oConnection.ConnectionString = "Provider=" & sProvider & "; Data Source=" & sDataSource & ";Initial Catalog=" & sInitialCatalog & ";User ID=sirius;Password=$1R1U5;"
		m_oConnection.Open()

        'm_oConnection.CommandTimeout = kcTimeout
		
		'Set default order for transaction grid.
		m_lOrderBy = ACTCDocumentRef
		m_bAscending = True
		
		RefreshAccountsGrid()
		
	End Sub
	
	Private Sub frmAccounts_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		
		CloseLogFile()
		
		If Not (m_oRSAccounts Is Nothing) Then
		End If
		If Not (m_oRSAllocations Is Nothing) Then
		End If
		If Not (m_oRSTransactions Is Nothing) Then
		End If
	End Sub
	
	Private Sub RefreshAccountsGrid()
		
		Dim sSQL As String = ""
		
		Try 
			
			If optMisallocations.Checked Then
				
				grdAccounts.Columns(ACACMisAllocAmount).HeaderText = ksMisAllocated
				
				sSQL = ""
				sSQL = sSQL & "SELECT" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    a.short_code," & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    l.ledger_name," & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    SUM (tm.base_match_amount) 'amount'," & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    (" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        SELECT SUM(amount)" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        FROM transdetail" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        WHERE account_id = a.account_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ) 'transdetail_amount'," & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    (" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        SELECT ISNULL(SUM(amount), 0)" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        FROM transdetail" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        WHERE account_id = a.account_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        AND postingstatus_id = 3" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    )" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    -" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    (" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        SELECT ISNULL(SUM(tm.base_match_amount), 0)" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        FROM transdetail td" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        JOIN transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "            ON td.transdetail_id = tm.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        WHERE account_id = a.account_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        AND postingstatus_id = 3" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ) 'display_amount'," & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    a.account_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "FROM account a" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "JOIN ledger l" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ON l.ledger_id = a.ledger_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "JOIN transdetail td" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ON a.account_id = td.account_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "JOIN document d" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ON d.document_id = td.document_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "JOIN transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ON td.transdetail_id = tm.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "WHERE tm.allocationdetail_id IS NOT NULL" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "GROUP BY a.account_id, a.short_code, l.ledger_name" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "HAVING SUM(tm.base_match_amount) <> 0" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "ORDER BY" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    a.short_code" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "" & Strings.Chr(13) & Strings.Chr(10)
				
			End If
			
			If optUnallocatedCash.Checked Then
				
				grdAccounts.Columns(ACACMisAllocAmount).HeaderText = ksUnallocatedCash
				
				sSQL = ""
				sSQL = sSQL & "SELECT" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    a.short_code," & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    l.ledger_name," & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    MAX(x.amount) 'amount'," & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    (" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        SELECT SUM(amount)" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        FROM transdetail" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        WHERE account_id = a.account_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ) 'transdetail_amount'," & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    (" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        SELECT ISNULL(SUM(amount), 0)" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        FROM transdetail" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        WHERE account_id = a.account_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        AND postingstatus_id = 3" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    )" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    -" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    (" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        SELECT ISNULL(SUM(tm.base_match_amount), 0)" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        FROM transdetail td" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        JOIN transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "            ON td.transdetail_id = tm.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        WHERE account_id = a.account_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        AND postingstatus_id = 3" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ) 'display_amount'," & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    a.account_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "FROM account a" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "JOIN ledger l" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ON l.ledger_id = a.ledger_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "JOIN " & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    (" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        SELECT " & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "            account_id," & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "            (" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "                SELECT SUM(td.amount)" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "                FROM transdetail td" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "                JOIN document d" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "                    ON d.document_id = td.document_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "                JOIN documenttype dt" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "                    ON dt.documenttype_id = d.documenttype_id  " & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "                WHERE td.account_id = a.account_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "                AND dt.code in ('SPY','SRP')" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "            )" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "            -" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "            (" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "                SELECT SUM(tm.base_match_amount)" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "                FROM transdetail td" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "                JOIN document d" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "                    ON d.document_id = td.document_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "                JOIN documenttype dt" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "                    ON dt.documenttype_id = d.documenttype_id  " & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "                JOIN transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "                    ON tm.transdetail_id = td.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "                WHERE td.account_id = a.account_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "                AND dt.code in ('SPY','SRP')" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "                AND tm.allocationdetail_id IS NOT NULL" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "            ) 'amount'" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        FROM account a" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ) x" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ON x.account_id = a.account_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "GROUP BY " & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    a.account_id, " & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    a.short_code, " & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    l.ledger_name" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "HAVING " & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    MAX(x.amount) <> 0" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "ORDER BY" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    a.short_code" & Strings.Chr(13) & Strings.Chr(10)
				
			End If
			
			m_oRSAccounts = New DataSet()
			
			Dim com As New SqlCommand
			com.Connection = m_oConnection
			com.CommandText = sSQL
			Dim adap As SqlDataAdapter = New SqlDataAdapter(com.CommandText, com.Connection)
			m_oRSAccounts = New DataSet("dsl")
			adap.Fill(m_oRSAccounts)
			
			grdAccounts.DataSource = m_oRSAccounts
			grdAccounts.ReBind()
			
			If Strings.Len(grdAccounts.CurrentRow().Cells(0).Value) > 0 Then
				RefreshAllocationsGrid()
			End If
		
		Catch 
			
			
			
			MessageBox.Show("Script failed to run", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
		
		
	End Sub
	
	Private Sub RefreshAllocationsGrid()
		
		Dim sSQL As String = ""
		
		Try 
			
			If optMisallocations.Checked Then
				
				grdAllocations.Columns(ACAlCMisAllocAmount).HeaderText = ksMisAllocated
				
				sSQL = ""
				sSQL = sSQL & "SELECT" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    tm.match_id," & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    mg.match_date," & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    Sum (tm.base_match_amount) 'amount'" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "FROM account a" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "JOIN transdetail td" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ON a.account_id = td.account_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "JOIN transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ON tm.transdetail_id = td.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "JOIN matchgroup mg" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ON mg.match_id = tm.match_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "WHERE a.short_code = '" & grdAccounts.CurrentRow().Cells(0).Value.Trim().Replace("'", "''") & "'" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "AND tm.allocationdetail_id IS NOT NULL" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "GROUP BY" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    tm.match_id," & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    mg.match_date" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "HAVING SUM(tm.base_match_amount) <> 0" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "ORDER BY" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    tm.match_id," & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    mg.match_date" & Strings.Chr(13) & Strings.Chr(10)
				
			End If
			
			If optUnallocatedCash.Checked Then
				
				grdAllocations.Columns(ACAlCMisAllocAmount).HeaderText = ksAllocatedCash
				
				sSQL = sSQL & ""
				sSQL = sSQL & "SELECT" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    mg.match_id," & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    mg.match_date," & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    SUM(tm.base_match_amount) 'amount'" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "FROM account a" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "JOIN transdetail td" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ON a.account_id = td.account_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "JOIN document d" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ON d.document_id = td.document_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "JOIN documenttype dt" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ON dt.documenttype_id = d.documenttype_id  " & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "JOIN transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ON tm.transdetail_id = td.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "JOIN matchgroup mg" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ON mg.match_id = tm.match_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "WHERE a.short_code = '" & grdAccounts.CurrentRow().Cells(0).Value.Trim().Replace("'", "''") & "'" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "AND dt.code in ('SPY','SRP')" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "AND tm.allocationdetail_id IS NOT NULL" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "GROUP BY" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    mg.match_id," & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    mg.match_date" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "HAVING " & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    (" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        SELECT " & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "            SUM(td.amount) " & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        FROM transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        JOIN transdetail td" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "            ON td.transdetail_id = tm.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        JOIN document d" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "            ON d.document_id = td.document_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        JOIN documenttype dt" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "            ON dt.documenttype_id = d.documenttype_id  " & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        WHERE tm.match_id = mg.match_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        AND dt.code in ('SPY','SRP')" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    )" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    -" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    (" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        SELECT " & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "            SUM(ISNULL(tmx.base_match_amount,0))" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        FROM transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        JOIN transdetail td" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "            ON td.transdetail_id = tm.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        JOIN document d" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "            ON d.document_id = td.document_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        JOIN documenttype dt" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "            ON dt.documenttype_id = d.documenttype_id  " & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        LEFT JOIN transmatch tmx " & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "            ON tmx.transdetail_id = td.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        WHERE tm.match_id = mg.match_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        AND dt.code in ('SPY','SRP')" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ) <> 0" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "ORDER BY" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    mg.match_id," & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    mg.match_date" & Strings.Chr(13) & Strings.Chr(10)
				
			End If
			
			If Not (m_oRSAllocations Is Nothing) Then
			End If
			
			m_oRSAllocations = New DataSet()
			
			Dim com As New SqlCommand
			com.Connection = m_oConnection
			com.CommandText = sSQL
			Dim adap As SqlDataAdapter = New SqlDataAdapter(com.CommandText, com.Connection)
			m_oRSAllocations = New DataSet("dsl")
			adap.Fill(m_oRSAllocations)
			
			grdAllocations.DataSource = m_oRSAllocations
			grdAllocations.ReBind()
			
			If grdAllocations.CurrentRow().Cells(0).Value <> 0 Then
				RefreshTransactionsGrid()
			End If
		
		Catch 
			
			
			
			MessageBox.Show("Script failed to run", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
		
		
	End Sub
	
	Private Sub RefreshTransactionsGrid()
		
		Dim sSQL As String = ""
		Dim lTransCount As Integer
		Dim vBookmark As Object
		Dim cOriginal, cAllocated, cOutstanding As Decimal
		
		Try 
			
			sSQL = ""
			sSQL = sSQL & "SELECT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    a.short_code," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    d.document_ref," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    td.amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    tm.base_match_amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    tm.transmatch_id," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    (" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "        SELECT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "            td.amount - SUM(tm.base_match_amount)" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "        FROM transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "        WHERE tm.transdetail_id = td.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "        AND tm.allocationdetail_id IS NOT NULL" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    ) 'os_amount'," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    c.code 'currency_code'," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    s.code 'source_code'," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    tm.currency_match_amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    td.spare" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "FROM account a" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "JOIN transdetail td" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    ON a.account_id = td.account_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "JOIN document d" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    ON d.document_id = td.document_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "JOIN transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    ON tm.transdetail_id = td.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "JOIN currency c" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    ON c.currency_id = td.currency_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "JOIN source s" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    ON s.source_id = d.company_id" & Strings.Chr(13) & Strings.Chr(10)

			If Convert.IsDBNull(grdAllocations.CurrentRow().Cells(0).Value) Or IsNothing(grdAllocations.CurrentRow().Cells(0).Value) Then
				'Don't show any lines.
				sSQL = sSQL & "WHERE tm.match_id = -1" & Strings.Chr(13) & Strings.Chr(10)
			Else
				sSQL = sSQL & "WHERE tm.match_id = " & CStr(grdAllocations.CurrentRow().Cells(0).Value) & Strings.Chr(13) & Strings.Chr(10)
			End If
			sSQL = sSQL & "ORDER BY" & Strings.Chr(13) & Strings.Chr(10)
			
			Select Case m_lOrderBy
				Case ACTCAccountCode
					sSQL = sSQL & "    a.short_code " & (IIf(m_bAscending, "ASC", "DESC")) & Strings.Chr(13) & Strings.Chr(10)
				Case ACTCDocumentRef
					sSQL = sSQL & "    d.document_ref " & (IIf(m_bAscending, "ASC", "DESC")) & Strings.Chr(13) & Strings.Chr(10)
				Case ACTCCompany
					sSQL = sSQL & "    s.code " & (IIf(m_bAscending, "ASC", "DESC")) & Strings.Chr(13) & Strings.Chr(10)
				Case ACTCSpare
					sSQL = sSQL & "    td.spare " & (IIf(m_bAscending, "ASC", "DESC")) & Strings.Chr(13) & Strings.Chr(10)
				Case ACTCCurrency
					sSQL = sSQL & "    c.code " & (IIf(m_bAscending, "ASC", "DESC")) & Strings.Chr(13) & Strings.Chr(10)
				Case ACTCOriginalAmount
					sSQL = sSQL & "    td.amount " & (IIf(m_bAscending, "ASC", "DESC")) & Strings.Chr(13) & Strings.Chr(10)
				Case ACTCAllocatedAmount
					sSQL = sSQL & "    tm.base_match_amount " & (IIf(m_bAscending, "ASC", "DESC")) & Strings.Chr(13) & Strings.Chr(10)
				Case ACTCTransMatchID
					sSQL = sSQL & "    tm.transmatch_id " & (IIf(m_bAscending, "ASC", "DESC")) & Strings.Chr(13) & Strings.Chr(10)
				Case ACTCOSAmount
					sSQL = sSQL & "    'os_amount' " & (IIf(m_bAscending, "ASC", "DESC")) & Strings.Chr(13) & Strings.Chr(10)
				Case ACTCCurrencyAllocatedAmount
					sSQL = sSQL & "    tm.currency_match_amount " & (IIf(m_bAscending, "ASC", "DESC")) & Strings.Chr(13) & Strings.Chr(10)
			End Select
			
			If Not (m_oRSTransactions Is Nothing) Then
			End If
			
			m_oRSTransactions = New DataSet()
			
			Dim com As New SqlCommand
			com.Connection = m_oConnection
			com.CommandText = sSQL
			Dim adap As SqlDataAdapter = New SqlDataAdapter(com.CommandText, com.Connection)
			m_oRSTransactions = New DataSet("dsl")
			adap.Fill(m_oRSTransactions)
			
			grdTransactions.DataSource = m_oRSTransactions
			grdTransactions.ReBind()
			
			'Initialise totals
			cOriginal = 0
			cAllocated = 0
			cOutstanding = 0
			
			lTransCount = 0
			grdTransactions.MoveFirst()



            'vBookmark = grdTransactions.GetBookmark(lTransCount)

			Do While Not (Convert.IsDBNull(vBookmark) Or IsNothing(vBookmark))
				

                '	cOriginal += CDec(grdTransactions.Columns(ACTCOriginalAmount).CellValue(vBookmark))

                'cAllocated += CDec(grdTransactions.Columns(ACTCAllocatedAmount).CellValue(vBookmark))

                'cOutstanding += CDec(grdTransactions.Columns(ACTCOSAmount).CellValue(vBookmark))
				
				lTransCount += 1



                'vBookmark = grdTransactions.GetBookmark(lTransCount)
			Loop 
			grdTransactions.MoveFirst()
			
            'If Math.Round(cOriginal, 2) = cOriginal Then
            '	grdTransactions.Columns(ACTCOriginalAmount).FooterText = StringsHelper.Format(cOriginal, "#0.00")
            'Else
            '	grdTransactions.Columns(ACTCOriginalAmount).FooterText = StringsHelper.Format(cOriginal, "#0.0000")
            'End If
            'grdTransactions.Columns(ACTCOriginalAmount).FooterAlignment = ContentAlignment.MiddleRight

            'If Math.Round(cAllocated, 2) = cAllocated Then
            '	grdTransactions.Columns(ACTCAllocatedAmount).FooterText = StringsHelper.Format(cAllocated, "#0.00")
            'Else
            '	grdTransactions.Columns(ACTCAllocatedAmount).FooterText = StringsHelper.Format(cAllocated, "#0.0000")
            'End If
            'grdTransactions.Columns(ACTCAllocatedAmount).FooterAlignment = ContentAlignment.MiddleRight

            'If Math.Round(cOutstanding, 2) = cOutstanding Then
            '	grdTransactions.Columns(ACTCOSAmount).FooterText = StringsHelper.Format(cOutstanding, "#0.00")
            'Else
            '	grdTransactions.Columns(ACTCOSAmount).FooterText = StringsHelper.Format(cOutstanding, "#0.0000")
            'End If
            'grdTransactions.Columns(ACTCOSAmount).FooterAlignment = ContentAlignment.MiddleRight
		
		Catch 
			
			
			
			MessageBox.Show("Script failed to run", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
		
		
	End Sub
	
	Public Sub OpenLogFile()
		Dim FSO As Object
		Dim sPath, sSetting, sLogFilePath As String
		
		If Not m_bLogFileOpen Then
			FSO = New Object()
			
			sPath = "SOFTWARE\PM\SiriusArchitecture\Common"
			sSetting = "LogFileName"
			
			sLogFilePath = gPMFunctions.QueryKeyValue(gPMConstants.HKEY_LOCAL_MACHINE, sPath, sSetting)
			
			sLogFilePath = sLogFilePath.Substring(0, IIf(sLogFilePath = "" And "\" = "", 0, (sLogFilePath.LastIndexOf("\") + 1)))
			
			sLogFilePath = sLogFilePath & "DATA" & DateTime.Now.ToString("yyyyMMddHHmmss") & ".txt"
			
			m_tsLog = New FileStream(sLogFilePath, FileMode.OpenOrCreate, FileAccess.Write)
			
			Dim m_tsLogWriter As StreamWriter = New StreamWriter(m_tsLog)
			m_tsLogWriter.WriteLine("Data Correction Started: " & DateTime.Now.ToString("HH:mm:ss dd-MM-yyyy"))
			m_tsLogWriter.WriteLine("*****")
			
			m_bLogFileOpen = True
		End If
		
	End Sub
	
	Private Sub CloseLogFile()
		If m_bLogFileOpen Then
			Dim m_tsLogWriter As StreamWriter = New StreamWriter(m_tsLog)
			m_tsLogWriter.WriteLine("Data Correction Finished: " & DateTime.Now.ToString("HH:mm:ss dd-MM-yyyy"))
            m_tsLogWriter.Close()
		End If
	End Sub
	
	Private Function CheckPassword() As Boolean
		
		Dim result As Boolean = False
		Dim sPromptInput As String = ""
		sPromptInput = sPromptInput & "This program is restricted, please enter the password corresponding to the seed value %sv%." & Strings.Chr(13) & Strings.Chr(10)
		sPromptInput = sPromptInput & Strings.Chr(13) & Strings.Chr(10)
		sPromptInput = sPromptInput & "WARNING - This password must be entered within about the next 7-8 minutes or it will cease to be valid."
		
		Dim sPromptError As String = "Incorrect password."
		
		
		Dim sTitle As String = "Mis-Allocations Helper"
		VBMath.Randomize()
		Dim nSeed As Integer = CInt(VBMath.Rnd() * 1000000#)
		Dim nPassword As Integer = GeneratePassword(nSeed)
		
		Dim sPassword As String = Interaction.InputBox(sPromptInput.Replace("%sv%", CStr(nSeed)), sTitle, "")
		
		If sPassword <> "" Then
			Dim dbNumericTemp As Double
			If Double.TryParse(sPassword, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
				If CInt(sPassword) = nPassword Then
					result = True
				End If
			End If
			If Not result Then
				MessageBox.Show(sPromptError, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
			End If
		End If
		
		Return result
	End Function
	

	Private Sub grdTransactions_CellFormatting(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellFormattingEventArgs) Handles grdTransactions.CellFormatting
		Dim ColIndex As Integer = eventArgs.ColumnIndex
		Dim Value As Object = eventArgs.Value
		Dim Bookmark As DatagridViewRow = Nothing
		If eventArgs.RowIndex > 0 Then
			Bookmark = grdTransactions.Rows(eventArgs.RowIndex)
		End If
		
		Select Case ColIndex
			Case ACTCOriginalAmount, ACTCAllocatedAmount, ACTCOSAmount
				

				If Math.Round(CDbl(CDbl(Value)), 2) = CDbl(Value) Then


					Value = StringsHelper.Format(Value, "#0.00")
				Else


					Value = StringsHelper.Format(Value, "#0.0000")
				End If
				
		End Select
		
	End Sub
	
	Private Sub grdTransactions_ColumnHeaderMouseClick(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellMouseEventArgs) Handles grdTransactions.ColumnHeaderMouseClick
		Dim ColIndex As Integer = eventArgs.ColumnIndex
		
		If m_lOrderBy = ColIndex Then
			m_bAscending = Not m_bAscending
		Else
			m_bAscending = True
		End If
		
		m_lOrderBy = ColIndex
		
		RefreshTransactionsGrid()
		
	End Sub
End Class
