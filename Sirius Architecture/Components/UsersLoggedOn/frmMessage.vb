Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
Friend Partial Class frmMessage
	Inherits System.Windows.Forms.Form
	Private Sub frmMessage_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	' RDC 24112000 new facility to allow administrators to message users
	
	
	
	Private m_lStatus As Integer
	Private m_lReturn As Integer
	
	Private m_sMachine As String = ""
	Private m_vMachines() As Object
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	Public WriteOnly Property Machine() As String
		Set(ByVal Value As String)
			m_sMachine = Value
		End Set
	End Property
	
	Public WriteOnly Property Machines() As Object()
		Set(ByVal Value() As Object)
			m_vMachines = Value
		End Set
	End Property
	
	Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
		Me.Close()
	End Sub
	
	Private Sub cmdSend_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSend.Click
		
		
		If optSelectedUsers.Checked Then

			Dim startInfo As ProcessStartInfo = New ProcessStartInfo("net send " &  _
			                                    m_sMachine & " " &  _
			                                    txtMessage.Text)
			startInfo.WindowStyle = ProcessWindowStyle.Hidden
			m_lReturn = CInt(Process.Start(startInfo).Id)
		Else
			For	Each m_vMachines_item As Object In m_vMachines

				Dim startInfo2 As ProcessStartInfo = New ProcessStartInfo("net send " &  _
				                                     CStr(m_vMachines_item) & " " &  _
				                                     txtMessage.Text)
				startInfo2.WindowStyle = ProcessWindowStyle.Hidden
				m_lReturn = CInt(Process.Start(startInfo2).Id)
			Next m_vMachines_item
		End If
		
	End Sub
	

	Private Sub frmMessage_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		If m_sMachine = "" Then
			optAllUsers.Checked = True
			optSelectedUsers.Enabled = False
		Else
			optSelectedUsers.Checked = True
		End If
		
	End Sub
	
	Private Sub txtMessage_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMessage.Enter
		
		txtMessage.SelectionStart = 0
		txtMessage.SelectionLength = Strings.Len(txtMessage.Text)
		
	End Sub
	
	Private Sub txtMessage_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMessage.Leave
		
		txtMessage.Text = txtMessage.Text.Trim()
		
	End Sub
End Class