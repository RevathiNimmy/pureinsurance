Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Friend Partial Class Form1
	Inherits System.Windows.Forms.Form
	Private Sub cmdGetTotalFees_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdGetTotalFees.Click

		txtTotalFees.Text = uctPMUFees1.TotalFees
	End Sub
	
	Private Sub cmdLoad_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdLoad.Click
		

		CType(uctPMUFees1, SSP.S4I.Interfaces.ILocalInterface).Initialise()

		uctPMUFees1.SetProcessModes(txtTask.Text,  ,  , txtTransactionType.Text, DateTime.Today)

		uctPMUFees1.ReadOnly = CBool(txtReadOnly.Text)
		
		If txtInsFileCnt.Text <> "" Then

			uctPMUFees1.InsuranceFileCnt = txtInsFileCnt.Text
		Else

			uctPMUFees1.InsuranceFileCnt = 0
		End If
		
		If txtRiskCnt.Text <> "" Then

			uctPMUFees1.RiskCnt = txtRiskCnt.Text
		Else

			uctPMUFees1.RiskCnt = 0
		End If
		

		uctPMUFees1.Load()
		
	End Sub
	
	Private Sub cmdRecalc_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRecalc.Click

		uctPMUFees1.Recalculate()
	End Sub
	
	Private Sub cmdTotalTax_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdTotalTax.Click

		txtTotalTax.Text = uctPMUFees1.TotalTax
	End Sub
End Class