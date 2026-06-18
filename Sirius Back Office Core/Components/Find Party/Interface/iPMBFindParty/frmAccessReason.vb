Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmAccessReason
	Inherits System.Windows.Forms.Form
	Private Sub frmAccessReason_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	
	' ***************************************************************** '
	' Form Name: frmSecurityQuestion
	'
	' Date: 31/10/2003
	'
	' Description: Access Reason Popup
	'
	' Edit History: DD 31/10/2003 Created
	'
	' ***************************************************************** '
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmAccessReason"
	
	Private m_sReason As String = ""
	
	Public ReadOnly Property Reason() As String
		Get
			Return m_sReason
		End Get
	End Property
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		'AR20041102 - PN16351
		If txtReason.Text.Trim() = "" Then
			MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - Client Access Reason", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Me.txtReason.Focus()
		Else
			m_sReason = Me.txtReason.Text.Trim()
			Me.Hide()
		End If
	End Sub
	

	Private Sub frmAccessReason_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		iPMFunc.CenterForm(Me)
	End Sub
	
	Private Sub frmAccessReason_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		'AR20041102 - PN16351
		'Only validate if user has closed the form
		If UnloadMode = 0 Then
			
			If txtReason.Text.Trim() = "" Then
				MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - Client Access Reason", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Me.txtReason.Focus()
				
				
				
				Cancel = 1
			Else
				m_sReason = Me.txtReason.Text.Trim()
			End If
			
		End If
		
		eventArgs.Cancel = Cancel <> 0
	End Sub
End Class