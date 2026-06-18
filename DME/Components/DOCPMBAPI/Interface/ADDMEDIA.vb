Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.DB.DAO
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.Common
Imports System.Windows.Forms
Friend Partial Class frmVolumeName
	Inherits System.Windows.Forms.Form
	Private Sub frmVolumeName_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		txtVolName.Text = ""
		Me.Hide()
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		Dim ssVolume As DAO.Snapshot
		
		If txtVolName.Text.Trim() = "" Then
			Interaction.MsgBox("Volume must have a Name", MB_ICONEXCLAMATION)
		ElseIf txtRoot.Text.Trim() = "" Then 
			Interaction.MsgBox("Data Root directory is missing (ie. \user\data )", MB_ICONEXCLAMATION)
			txtRoot.Focus()
		Else
            ssVolume = g_dbDDB.CreateSnapshot("SELECT volume_name FROM volume WHERE volume_name = '" & txtVolName.Text & "'")
			DAO_DBEngine_definst.FreeLocks()
			
			If ssVolume.RecordCount > 0 Then
				MessageBox.Show("The Volume '" & txtVolName.Text & "' aready exists, MB_ICONEXCLAMATION", Application.ProductName)
			Else
				Me.Hide()
			End If
		End If
		
	End Sub
	

	Private Sub frmVolumeName_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        'TODO
        'CenterForm(Me)
		Me.Cursor = Cursors.Default
		
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub txtVolName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtVolName.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		If Strings.Len(txtVolName.Text) > 20 Then
			SendKeys.Send("{BS}")
		End If
		
		If txtVolName.Text.Substring(txtVolName.Text.Length - 1) = Strings.Chr(39).ToString() Or txtVolName.Text.Substring(txtVolName.Text.Length - 1) = Strings.Chr(34).ToString() Then
			SendKeys.Send("{BS}")
		End If
		
	End Sub
End Class