Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Friend Partial Class Form1
	Inherits System.Windows.Forms.Form
	Private Sub cmdLoad_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdLoad.Click
		
		
		CType(uctPMURITax1, SSP.S4I.Interfaces.ILocalInterface).Initialise()
		uctPMURITax1.SetProcessModes(txtTask.Text,  ,  , txtTransactionType.Text, DateTime.Today)
		uctPMURITax1.ReadOnly_Renamed = CBool(txtReadOnly.Text)
		If Text1.Text <> "" Then
			uctPMURITax1.InsuranceFileCnt = CInt(Text1.Text)
		Else
			uctPMURITax1.InsuranceFileCnt = 0
		End If
		
		If txtRiskCnt.Text <> "" Then
			uctPMURITax1.RiskCnt = CInt(txtRiskCnt.Text)
		Else
			uctPMURITax1.RiskCnt = 0
		End If
		
		uctPMURITax1.Load_Renamed()
		
	End Sub
	
	Private Sub cmdRecalc_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRecalc.Click
		uctPMURITax1.Recalculate()
	End Sub
	
	Private Sub cmdTotalTax_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdTotalTax.Click
		MessageBox.Show(CStr(uctPMURITax1.TotalTax), Application.ProductName)
	End Sub
	
	Private Sub uctPMURITax1_Change(ByVal Sender As Object, ByVal e As EventArgs) Handles uctPMURITax1.Change
		MessageBox.Show("something has changed", Application.ProductName)
	End Sub
End Class