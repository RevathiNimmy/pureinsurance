Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmDescription
	Inherits System.Windows.Forms.Form
	
	Private m_lReturn As Integer
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Private m_lMode As Integer
	
	Private m_sCode As String = ""
	Private m_sDesc As String = ""
	
    'Private m_vCodes( ,  ) As Object
    Private m_vCodes As Object
	Private m_sOldCode As String = ""
	Private m_sOldDesc As String = ""
	
	Private Const ACClass As String = "frmDescription"
	
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	Public WriteOnly Property Mode() As Integer
		Set(ByVal Value As Integer)
			m_lMode = Value
		End Set
	End Property
	
	
	Public Property Code() As String
		Get
			Return m_sCode
		End Get
		Set(ByVal Value As String)
			m_sCode = Value
		End Set
	End Property
	
	
	Public Property Desc() As String
		Get
			Return m_sDesc
		End Get
		Set(ByVal Value As String)
			m_sDesc = Value
		End Set
	End Property
	
	Public WriteOnly Property VersionCodes() As Object()
        Set(ByVal Value() As Object)
            m_vCodes = Value
        End Set
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMOk
		
		Me.Close()
		
	End Sub
	
	Private Sub frmDescription_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			lblMessage.Text = ""
			
			Select Case m_lMode
				Case MSG_MODE_NEWMAP
					lblMessage.Text = "Select new code and description for roadmap."
				Case MSG_MODE_OLDMAP
					lblMessage.Text = "Enter roadmap code and description. If you change the code, it will be saved as a new version of the roadmap."
				Case MSG_MODE_NEWTASK
					lblMessage.Text = "Select code and description for the new Work Manager task. The code must be unique."
			End Select
			
			' take a copy of the original details
			m_sOldCode = m_sCode
			m_sOldDesc = m_sDesc
			
			txtCode.Text = m_sCode.Trim()
			txtDescription.Text = m_sDesc.Trim()
			
			cmdOk.Enabled = m_sCode <> "" And m_sDesc <> ""
			
		End If
	End Sub
	
	Private Sub frmDescription_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		Dim bUnique As Boolean
		
		Try 
			
			If m_lStatus = gPMConstants.PMEReturnCode.PMOk And (m_sDesc.Trim() = "" Or m_sCode.Trim() = "") Then
				m_lStatus = gPMConstants.PMEReturnCode.PMCancel
				Cancel = 1
				Exit Sub
			End If
			
			If m_lStatus = gPMConstants.PMEReturnCode.PMOk And m_lMode = MSG_MODE_NEWMAP And m_sOldCode.ToUpper() = m_sCode.ToUpper() Then
				
				MessageBox.Show("Roadmap code already exists. A unique code must be specified", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
				
				m_lStatus = gPMConstants.PMEReturnCode.PMCancel
				Cancel = 1
				Exit Sub
			End If
			
			If m_lStatus = gPMConstants.PMEReturnCode.PMOk And (m_sOldDesc.ToUpper() <> m_sDesc.ToUpper() Or m_sOldCode.ToUpper() <> m_sCode.ToUpper()) Then
				
				m_lReturn = CheckCodeUnique(bUnique)
				
				If Not bUnique Then
					MessageBox.Show("Code already exists. A unique code must be specified", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
					
					m_lStatus = gPMConstants.PMEReturnCode.PMCancel
					Cancel = 1
					Exit Sub
				End If
				
			End If
		
		Catch excep As System.Exception
			
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form_QueryUnload failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			eventArgs.Cancel = Cancel <> 0
		End Try
		
	End Sub
	
	Private Function CheckCodeUnique(ByRef bUnique As Boolean) As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			bUnique = True
			
			If Not Information.IsArray(m_vCodes) Then
				Return gPMConstants.PMEReturnCode.PMTrue
			End If
			
			For lLoop As Integer = m_vCodes.GetLowerBound(1) To m_vCodes.GetUpperBound(1)
				If m_sCode.Trim().ToUpper() = CStr(m_vCodes(0, lLoop)).Trim().ToUpper() Then
					bUnique = False
					Exit For
				End If
			Next 
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckCodeUnique failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckCodeUnique", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	Private isInitializingComponent As Boolean
	Private Sub txtCode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		m_sCode = txtCode.Text.Trim()
		
		cmdOk.Enabled = m_sCode <> "" And m_sDesc <> ""
		
	End Sub
	
	Private Sub txtCode_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtCode.KeyPress
		Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
		
		If KeyAscii = CInt(Keys.Return) Then
			m_lStatus = gPMConstants.PMEReturnCode.PMOk
			Me.Close()
		End If
		
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		eventArgs.KeyChar = Convert.ToChar(KeyAscii)
	End Sub
	
	Private Sub txtDescription_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		m_sDesc = txtDescription.Text.Trim()
		
		cmdOk.Enabled = m_sCode <> "" And m_sDesc <> ""
		
	End Sub
	
	Private Sub txtDescription_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtDescription.KeyPress
		Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
		
		If KeyAscii = CInt(Keys.Return) Then
			m_lStatus = gPMConstants.PMEReturnCode.PMOk
			Me.Close()
		End If
		
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		eventArgs.KeyChar = Convert.ToChar(KeyAscii)
	End Sub
End Class
