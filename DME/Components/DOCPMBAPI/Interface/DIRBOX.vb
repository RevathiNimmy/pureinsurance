Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Friend Partial Class frmSelectDir
	Inherits System.Windows.Forms.Form
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		

        'developer guide no.26
        lbl3Path.Text = ""
		Me.Hide()
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		Me.Hide()
		
	End Sub
	
	Private Sub Dir1_Change(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Dir1.Change
		

        'pan3Path._Caption = Dir1.Path
        lbl3Path.Text = Dir1.Path
	End Sub
	
	Private Sub Drive1_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Drive1.SelectedIndexChanged
		
		Dir1.Path = Drive1.Drive
		
	End Sub
	

	Private Sub frmSelectDir_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        'TODO
        'CenterForm(Me)
		
		Dir1.Path = Drive1.Drive

        'pan3Path._Caption = Dir1.Path
        lbl3Path.Text = Dir1.Path
	End Sub
End Class