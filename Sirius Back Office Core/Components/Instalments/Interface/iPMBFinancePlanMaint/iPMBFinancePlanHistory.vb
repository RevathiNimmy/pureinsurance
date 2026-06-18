Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129 (guide)
Imports SharedFiles
Partial Friend Class frmHistory
	Inherits System.Windows.Forms.Form
	Private m_vFinancePlanVersions( ,  ) As Object
	Private m_lVersion As Integer
	
	Public Property FinancePlanVersions() As Object
		Get
			
			Return VB6.CopyArray(m_vFinancePlanVersions)
			
		End Get
		Set(ByVal Value As Object)
			
			m_vFinancePlanVersions = Value
			
		End Set
	End Property
	Public Property Version() As Integer
		Get
			
			Return m_lVersion
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lVersion = Value
			
		End Set
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_lVersion = 0
		Me.Hide()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		Dim lRowID As Integer
		
		If Not (lvwHistory.FocusedItem Is Nothing) Then

			lRowID = Convert.ToString(lvwHistory.FocusedItem.Tag)
			
			m_lVersion = CInt(m_vFinancePlanVersions(bSIRPremFinConst.k_PFPlanPFVersion, lRowID))
			Me.Hide()
		End If
	End Sub
	

	Private Sub frmHistory_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		Dim oListItem As ListViewItem

		If Not Information.IsArray(m_vFinancePlanVersions) Then
			Exit Sub
		End If
		
		SetExtraListViewProperties(v_hWndList:=Me.lvwHistory.Handle.ToInt32(), v_vShowRowSelect:=True)
		
		'DC091105 PN25466 show total amount on plan against each version of plan
		'DC270106 PN27067 total is already stored as a cumulitive total
		'cTotalAmountOnPlan = 0
		
		' Assign the details to the interface.
		'PN11684 Don't understand why last line is being omitted
		'For lRow& = LBound(m_vFinancePlanVersions, 2) To UBound(m_vFinancePlanVersions, 2) - 1
		For lRow As Integer = m_vFinancePlanVersions.GetLowerBound(1) To m_vFinancePlanVersions.GetUpperBound(1)
			' {* USER DEFINED CODE (Begin) *}
			
			'DC091105 PN25466 show total amount on plan against each version of plan
			'DC270106 PN27067 total is already stored as a cumulitive total
			'            If Trim$(m_vFinancePlanVersions(k_PFPlanProductClass, lRow&)) <> "M" And _
			''                Trim$(m_vFinancePlanVersions(k_PFPlanProductClass, lRow&)) <> "MTA" Then
			'                cTotalAmountOnPlan = 0
			'            End If
			'            cTotalAmountOnPlan = cTotalAmountOnPlan + CCur(Trim$(m_vFinancePlanVersions(k_PFPlanTotalCost, lRow&)))
			
			' Assign the details to the first column.
			' Column 1 Version

			oListItem = lvwHistory.Items.Add(CStr(m_vFinancePlanVersions(bSIRPremFinConst.k_PFPlanPFVersion, lRow)).Trim(), "")
			
			'DC091105 PN25466 show total amount on plan against each version of plan and plan reference
			'            ' Assign details to other the columns
			'            ' Column 2 Start Date
			'            oListItem.SubItems(1) = Trim$(m_vFinancePlanVersions(k_PFPlanStartDate, lRow&))
			'
			'            ' Column 3 Status
			'            oListItem.SubItems(2) = Trim$(m_vFinancePlanVersions(k_PFPlanStatusInd, lRow&))
			'
			'            ' Column 4 CAmount Financed
			'            oListItem.SubItems(3) = Trim$(m_vFinancePlanVersions(k_PFPlanAmountToFinance, lRow&))
			
			' Column 2 Plan Ref
			ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vFinancePlanVersions(bSIRPremFinConst.k_PFPlanAutoGenPlanRef, lRow)).Trim()
			
			' Column 3 Start Date
			ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vFinancePlanVersions(bSIRPremFinConst.k_PFPlanStartDate, lRow)).Trim()
			
			' Column 4 Status
			Select Case CStr(m_vFinancePlanVersions(bSIRPremFinConst.k_PFPlanStatusInd, lRow)).Trim()
				Case "010" : ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "Saved"
				Case "011" : ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "Cancelled"
				Case "012" : ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "Quote Printed"
				Case "040" : ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "Live"
				Case "140" : ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "On Hold"
				Case "900" : ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "Completed"
				Case "990" : ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "Superseded"
				Case "999" : ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "Updated"
			End Select
			
			' Column 5 Amount Financed
			ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vFinancePlanVersions(bSIRPremFinConst.k_PFPlanAmountToFinance, lRow)).Trim()
			
			' Column 6 Total Amount On Plan
			'DC270106 PN27067 total is already stored as a cumulitive total
			ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vFinancePlanVersions(bSIRPremFinConst.k_PFPlanTotalCost, lRow)).Trim()
			
			' {* USER DEFINED CODE (End) *}
			
			' Set the tag property with the index of
			' the search data storage.
			oListItem.Tag = CStr(lRow)
			
			' Refresh the first X amount of rows, to
			' allow the user to see the results instantly.
			If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
				' Select the first item.
				lvwHistory.Items.Item(0).Selected = True
				
				' Refresh the initial results.
				lvwHistory.Refresh()
			End If
			
		Next lRow
		
		' Select the first item.
		If lvwHistory.Items.Count > 0 Then
			lvwHistory.Items.Item(0).Selected = True
			
		End If
		
		iPMFunc.CenterForm(Me)
		
	End Sub
	
	Private Sub lvwHistory_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwHistory.DoubleClick
		cmdOK_Click(cmdOK, New EventArgs())
	End Sub
End Class