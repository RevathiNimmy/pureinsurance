Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Friend Partial Class frmCopyRisk
	Inherits System.Windows.Forms.Form
	
	Public CopyType As Integer
	Public OK As Boolean
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		OK = False
		Me.Hide()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		OK = True
		CopyType = optCopyType(1).Checked
		Me.Hide()
	End Sub
	

	Private Sub frmCopyRisk_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		optCopyType(0).Checked = True
		optCopyType(1).Checked = False
		iPMFunc.CenterForm(Me)
	End Sub
End Class