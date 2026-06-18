Option Strict Off
Option Explicit On
Friend Class frmMessage
	Inherits System.Windows.Forms.Form
	' RDC 24112000 new facility to allow administrators to message users
	
	
	
	Private m_lStatus As Integer
	Private m_lReturn As Integer
	
	Private m_sMachine As String
	Private m_vMachines As Object
	
	Public ReadOnly Property Status() As Integer
		Get
			Status = m_lStatus
		End Get
	End Property
	
	Public WriteOnly Property Machine() As String
		Set(ByVal Value As String)
			m_sMachine = Value
		End Set
	End Property
	
	Public WriteOnly Property Machines() As Object
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: Couldn't resolve default property of object vM. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			'UPGRADE_WARNING: Couldn't resolve default property of object m_vMachines. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			m_vMachines = Value
		End Set
	End Property
	
	Private Sub cmdOk_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOk.Click
		Me.Close()
	End Sub
	
	Private Sub cmdSend_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSend.Click
		
		Dim iLoop As Short
		
		If optSelectedUsers.Checked = True Then
			m_lReturn = Shell("net send " & m_sMachine & " " & txtMessage.Text, AppWinStyle.Hide)
		Else
			For iLoop = LBound(m_vMachines) To UBound(m_vMachines)
				'UPGRADE_WARNING: Couldn't resolve default property of object m_vMachines(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				m_lReturn = Shell("net send " & m_vMachines(iLoop) & " " & txtMessage.Text, AppWinStyle.Hide)
			Next 
		End If
		
	End Sub
	
	Private Sub frmMessage_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		If m_sMachine = "" Then
			optAllUsers.Checked = True
			optSelectedUsers.Enabled = False
		Else
			optSelectedUsers.Checked = True
		End If
		
	End Sub
	
	Private Sub txtMessage_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtMessage.Enter
		
		txtMessage.SelectionStart = 0
		txtMessage.SelectionLength = Len(txtMessage.Text)
		
	End Sub
	
	Private Sub txtMessage_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtMessage.Leave
		
		txtMessage.Text = Trim(txtMessage.Text)
		
	End Sub
End Class