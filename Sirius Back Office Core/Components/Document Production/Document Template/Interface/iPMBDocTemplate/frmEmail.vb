Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms

Imports SharedFiles
Friend Partial Class frmEmail
	Inherits System.Windows.Forms.Form
	
	Private m_lReturn As DialogResult
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Private m_sEmailTo As String = ""
	Private m_sEmailSubject As String = ""
	Private m_sEmailMessage As String = ""
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	Public ReadOnly Property EmailTo() As String
		Get
			Return m_sEmailTo
		End Get
	End Property
	
	Public ReadOnly Property EmailSubject() As String
		Get
			Return m_sEmailSubject
		End Get
	End Property
	
	Public ReadOnly Property EmailMessage() As String
		Get
			Return m_sEmailMessage
		End Get
	End Property
	
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMOk
		
		Me.Close()
		
	End Sub
	

	Private Sub frmEmail_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
	End Sub
	
	Private Sub frmEmail_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		If m_lStatus = gPMConstants.PMEReturnCode.PMCancel Then
			m_lReturn = MessageBox.Show("Are you sure that you want to cancel?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
			
			If m_lReturn <> System.Windows.Forms.DialogResult.Yes Then
				Cancel = True
				Exit Sub
			End If
		End If
		
		m_sEmailTo = txtTo.Text
		m_sEmailSubject = txtSubject.Text
		m_sEmailMessage = txtMessage.Text
		
		eventArgs.Cancel = Cancel <> 0
	End Sub
	
	Private Sub txtMessage_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMessage.Enter
		
		txtMessage.SelectionStart = 0
		txtMessage.SelectionLength = Strings.Len(txtMessage.Text)
		
	End Sub
	
	Private Sub txtSubject_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSubject.Enter
		
		txtSubject.SelectionStart = 0
		txtSubject.SelectionLength = Strings.Len(txtSubject.Text)
		
	End Sub
	
	Private Sub txtTo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTo.Enter
		
		txtTo.SelectionStart = 0
		txtTo.SelectionLength = Strings.Len(txtTo.Text)
		
	End Sub
End Class