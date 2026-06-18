Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports System.Windows.Forms
Friend Partial Class frmAddRelated
	Inherits System.Windows.Forms.Form
	
	Private m_oRSTransactions As DataSet
	Private m_lMatchID As Integer
	Private m_sAccountCode As String = ""
	Private m_vTransactions As Object
	Private m_lAddType As Integer
	
	Public WriteOnly Property AddType() As Integer
		Set(ByVal Value As Integer)
			m_lAddType = Value
		End Set
	End Property
	
	Public Property MatchID() As Integer
		Get
			Return m_lMatchID
		End Get
		Set(ByVal Value As Integer)
			m_lMatchID = Value
		End Set
	End Property
	
	Public Property AccountCode() As String
		Get
			Return m_sAccountCode
		End Get
		Set(ByVal Value As String)
			m_sAccountCode = Value
		End Set
	End Property
	
	Public ReadOnly Property Transactions() As Object
		Get
			Return m_vTransactions
		End Get
	End Property
	
	
	Private Sub RefreshTransactionsGrid()
		
		Dim sSQL As String = ""
		
		Try 
			
			sSQL = ""
			sSQL = sSQL & "SELECT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    a.short_code," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    d.document_ref," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    td.amount," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    0," & Strings.Chr(13) & Strings.Chr(10)
			
			Select Case m_lAddType
				Case ACATAddRelated, ACATAddOther
					
					sSQL = sSQL & "    td.transdetail_id 'ID'," & Strings.Chr(13) & Strings.Chr(10)
					
				Case ACATAddMissing
					
					sSQL = sSQL & "    ad.allocationdetail_id 'ID'," & Strings.Chr(13) & Strings.Chr(10)
					
			End Select
			sSQL = sSQL & "    (td.amount -" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    ISNULL(" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "        (" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "            SELECT" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "                SUM(tm.base_match_amount)" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "            FROM transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "            WHERE tm.transdetail_id = td.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "            AND tm.allocationdetail_id IS NOT NULL" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "        )" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    ,0)) 'os_amount'," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    c.code 'currency_code'," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    s.code 'source_code'," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    0" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "FROM account a" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "JOIN transdetail td" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    ON a.account_id = td.account_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "JOIN document d" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    ON d.document_id = td.document_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "JOIN currency c" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    ON c.currency_id = td.currency_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "JOIN source s" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    ON s.source_id = d.company_id" & Strings.Chr(13) & Strings.Chr(10)
			
			Select Case m_lAddType
				Case ACATAddRelated
					
					sSQL = sSQL & "WHERE td.transdetail_id IN " & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "    (" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "        SELECT tdx.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "        FROM transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "        JOIN transdetail td" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "            ON td.transdetail_id = tm.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "        JOIN transdetail tdx" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "            ON tdx.document_id = td.document_id" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "            AND tdx.account_id = td.account_id" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "        WHERE tm.match_id = " & CStr(m_lMatchID) & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "        AND NOT EXISTS" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "            (" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "                SELECT NULL" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "                FROM transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "                WHERE tm.transdetail_id = tdx.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "                AND tm.match_id = " & CStr(m_lMatchID) & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "            )" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "    )" & Strings.Chr(13) & Strings.Chr(10)
					
				Case ACATAddMissing
					
					sSQL = sSQL & "JOIN allocationdetail ad" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "    ON ad.transdetail_id = td.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "WHERE ad.allocationdetail_id IN " & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "    (" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "        SELECT adx.allocationdetail_id" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "        FROM transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "        JOIN allocationdetail ad" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "            ON ad.allocationdetail_id = tm.allocationdetail_id" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "            AND ad.transdetail_id = tm.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "        JOIN allocationdetail adx" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "            ON adx.allocation_id = ad.allocation_id" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "        WHERE tm.match_id = " & CStr(m_lMatchID) & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "        AND NOT EXISTS" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "            (" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "                SELECT NULL" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "                FROM transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "                WHERE tm.transdetail_id = adx.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "                AND tm.match_id = " & CStr(m_lMatchID) & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "            )" & Strings.Chr(13) & Strings.Chr(10)
					sSQL = sSQL & "    )" & Strings.Chr(13) & Strings.Chr(10)
					
				Case ACATAddOther
					
					sSQL = sSQL & "WHERE a.short_code = '" & m_sAccountCode & "'" & Strings.Chr(13) & Strings.Chr(10)
					
			End Select
			
			If chkOnlyShowOS.CheckState = CheckState.Checked Then
				sSQL = sSQL & "AND 0 <> " & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    (td.amount -" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ISNULL(" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        (" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "            SELECT" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "                SUM(tm.base_match_amount)" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "            FROM transmatch tm" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "            WHERE tm.transdetail_id = td.transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "            AND tm.allocationdetail_id IS NOT NULL" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "        )" & Strings.Chr(13) & Strings.Chr(10)
				sSQL = sSQL & "    ,0))" & Strings.Chr(13) & Strings.Chr(10)
			End If
			
			If txtDocumentRef.Text.Trim() <> "" Then
				sSQL = sSQL & "AND d.document_ref LIKE '" & txtDocumentRef.Text & "'" & Strings.Chr(13) & Strings.Chr(10)
			End If
			
			If txtAmount.Text.Trim() <> "" Then
				Dim dbNumericTemp As Double
				If Double.TryParse(txtAmount.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
					sSQL = sSQL & "AND td.amount = " & txtAmount.Text & Strings.Chr(13) & Strings.Chr(10)
				End If
			End If
			
			sSQL = sSQL & "ORDER BY" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    a.short_code," & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "    d.document_ref" & Strings.Chr(13) & Strings.Chr(10)
			
			
			If Not (m_oRSTransactions Is Nothing) Then
			End If
			
			m_oRSTransactions = New DataSet()
			
			Dim com As New SqlCommand
			com.Connection = frmAccounts.m_oConnection
			com.CommandText = sSQL
			Dim adap As SqlDataAdapter = New SqlDataAdapter(com.CommandText, com.Connection)
			m_oRSTransactions = New DataSet("dsl")
			adap.Fill(m_oRSTransactions)
			
			grdTransactions.DataSource = m_oRSTransactions
			grdTransactions.ReBind()
		
		Catch 
			
			
			
			MessageBox.Show("Script failed to run", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
		
		
	End Sub
	
	
	Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

		Dim sLogLine As String = ""
		

        'ToDoList -To be handled at runtime
        'If grdTransactions.SelBookmarks.Count = 0 Then
        MessageBox.Show("You need to select one or more transaction lines to add", "No Line Selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        'ToDoList -To be handled at runtime
        'Else

        '	ReDim m_vTransactions(0)


        '	For lCount As Integer = 0 To grdTransactions.SelBookmarks.Count - 1



        '		vBookmark = grdTransactions.SelBookmarks.Item(lCount)

        '		ReDim Preserve m_vTransactions(lCount)



        '		m_vTransactions.SetValue(grdTransactions.Columns(ACARCID).CellValue(vBookmark), lCount)

        '		'            If grdTransactions.Columns(ACARCOSAmount).CellValue(vBookmark) = 0 Then
        '		'                ReDim m_vTransactions(0)
        '		'
        '		'                MsgBox "Cannot add a transaction that is fully allocated.", vbExclamation, "Invalid Selection"
        '		'
        '		'                Exit Sub
        '		'            End If
        '	Next 

        'Log changes
        frmAccounts.OpenLogFile()
        Dim Writer As StreamWriter = New StreamWriter(frmAccounts.m_tsLog)
        Writer.WriteLine("ADD TRANSACTION LINES")


        ' For lCount As Integer = 0 To grdTransactions.SelBookmarks.Count - 1



        'vBookmark = grdTransactions.SelBookmarks.Item(lCount)

        sLogLine = ""
        sLogLine = sLogLine & CStr(m_lMatchID) & "|"

        ' sLogLine = sLogLine & CStr(grdTransactions.Columns(ACARCAccountCode).CellValue(vBookmark)) & "|"

        ' sLogLine = sLogLine & CStr(grdTransactions.Columns(ACARCDocumentRef).CellValue(vBookmark)) & "|"

        ' sLogLine = sLogLine & CStr(grdTransactions.Columns(ACARCCompany).CellValue(vBookmark)) & "|"

        ' sLogLine = sLogLine & StringsHelper.Format(grdTransactions.Columns(ACARCOSAmount).CellValue(vBookmark), "#0.00")

        Writer.WriteLine(sLogLine)
        'ToDoList -To be handled at runtime
        'Next

        Writer.WriteLine("*****")

        Me.Close()
        'End If

    End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		Me.Close()
	End Sub
	
	Private Sub cmdRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRefresh.Click
		RefreshTransactionsGrid()
	End Sub
	
	Private Sub frmAddRelated_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			If grdTransactions.RowsCount < 0 Then


                '		grdTransactions.SelBookmarks.Add(grdTransactions.Bookmark)
			End If
		End If
	End Sub
	

	Private Sub frmAddRelated_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

		m_vTransactions = ""
		
		Select Case m_lAddType
			Case ACATAddRelated, ACATAddMissing
				chkOnlyShowOS.CheckState = CheckState.Unchecked
			Case ACATAddOther
				chkOnlyShowOS.CheckState = CheckState.Checked
		End Select
		
		RefreshTransactionsGrid()
	End Sub
	
	Private Sub grdTransactions_SelectionChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles grdTransactions.SelectionChanged
		Dim Cancel As Integer = 0
		
		cmdAdd.Enabled = True
		

        'If grdTransactions.SelBookmarks.Count > 0 Then

        '	For lCount As Integer = 0 To grdTransactions.SelBookmarks.Count - 1



        '		vBookmark = grdTransactions.SelBookmarks.Item(lCount)


        '		If CDbl(grdTransactions.Columns(ACARCOSAmount).CellValue(vBookmark)) = 0 Then
        '			cmdAdd.Enabled = False
        '		End If

        '	Next 
        'End If
		
		If Cancel <> 0 Then
			grdTransactions.CancelEdit()
		End If
	End Sub
End Class
