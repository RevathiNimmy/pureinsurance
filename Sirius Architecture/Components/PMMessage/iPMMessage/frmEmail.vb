Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmEmail
	Inherits System.Windows.Forms.Form
	
	Private m_lReturn As Integer
	
	' HasClientLog
	Private m_bHasClientLog As Boolean
	' HasServerLog
	Private m_bHasServerLog As Boolean
	' HasCobolLog
	Private m_bHasCobolLog As Boolean
	' Status
	Private m_lStatus As Integer
	' Subject
	Private m_sSubject As String = ""
	' Message
	Private m_sMessage As String = ""
	' Rcpt
	Private m_sRcpt As String = ""
	'LogNumber RDC 27092001
	Private m_sLogNumber As String = ""
	
	' RDC 27092001
	Private Property LogNumber() As String
		Get
			Return m_sLogNumber
		End Get
		Set(ByVal Value As String)
			m_sLogNumber = Value
		End Set
	End Property
	
	Public Property Rcpt() As String
		Get
			Return m_sRcpt
		End Get
		Set(ByVal Value As String)
			m_sRcpt = Value
		End Set
	End Property
	
	Public WriteOnly Property HasClientLog() As Boolean
		Set(ByVal Value As Boolean)
			m_bHasClientLog = Value
		End Set
	End Property
	
	Public WriteOnly Property HasServerLog() As Boolean
		Set(ByVal Value As Boolean)
			m_bHasServerLog = Value
		End Set
	End Property
	
	Public WriteOnly Property HasCobolLog() As Boolean
		Set(ByVal Value As Boolean)
			m_bHasCobolLog = Value
		End Set
	End Property
	
	Public Property Status() As Integer
		Get
			Return m_lStatus
		End Get
		Set(ByVal Value As Integer)
			m_lStatus = Value
		End Set
	End Property
	
	Public Property Subject() As String
		Get
			Return m_sSubject
		End Get
		Set(ByVal Value As String)
			m_sSubject = Value
		End Set
	End Property
	
	Public Property Message() As String
		Get
			Return m_sMessage
		End Get
		Set(ByVal Value As String)
			m_sMessage = Value
		End Set
	End Property
	
	' ***************************************************************** '
	'
	' Name: InterfaceToProperties
	'
	' Description:
	'
	' History: 19/07/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Public Function InterfaceToProperties() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' RDC 27092001
			If txtLogNumber.Text.ToUpper() = "<LOGNUMBER>" Then
				txtLogNumber.Text = ""
			End If
			
			LogNumber = txtLogNumber.Text.Trim()
			Rcpt = txtRcpt.Text.Trim()
			Message = txtMessage.Text.Trim()
			
			' RDC 27092001 add Support Works facilities
			Select Case chkUpdate.CheckState
				Case CheckState.Checked ' update to previously logged call
					If LogNumber = "" Then
						' user has not filled in the log number
						MessageBox.Show("Log number must be specified when report is an update to existing fault", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
						
						txtLogNumber.Focus()
						Return gPMConstants.PMEReturnCode.PMFalse
					End If
					'PN11885
					Subject = "?UPDATECALL " & LogNumber & " " & txtSubject.Text.Trim()
				Case Else ' new fault report
					LogNumber = ""
					'PN11885
					Subject = "?LOGCALL " & txtSubject.Text.Trim()
			End Select
			
			Return result
		
		Catch 
			
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
			


			
			Return result
		End Try
	End Function
	
	Private Sub chkUpdate_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkUpdate.CheckStateChanged
		
		If chkUpdate.CheckState = CheckState.Unchecked Then
			txtLogNumber.Enabled = False
			txtLogNumber.BackColor = SystemColors.InactiveCaptionText
		Else
			txtLogNumber.Enabled = True
			txtLogNumber.BackColor = SystemColors.Window
		End If
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		Status = gPMConstants.PMEReturnCode.PMCancel
		
		Me.Hide()
		
	End Sub
	
	Private Sub cmdSend_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSend.Click
		
		Status = gPMConstants.PMEReturnCode.PMOK
		
		m_lReturn = InterfaceToProperties()
		
		' RDC 27092001 in case user has selected 'call update' and not filled in log number
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Exit Sub
		End If
		
		Me.Hide()
		
	End Sub
	

	Private Sub frmEmail_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		' RDC 27092001 for e-mail addresses from registry
		
		Const ACClientIndex As Integer = 0
		Const ACServerIndex As Integer = 1
		Const ACCobolIndex As Integer = 2
		
		' Show the respective attachments
		
		If m_bHasClientLog Then
			lblLog(ACClientIndex).Visible = True
			picLog(ACClientIndex).Visible = True
		Else
			lblLog(ACClientIndex).Visible = False
			picLog(ACClientIndex).Visible = False
		End If
		
		If m_bHasServerLog Then
			lblLog(ACServerIndex).Visible = True
			picLog(ACServerIndex).Visible = True
		Else
			lblLog(ACServerIndex).Visible = False
			picLog(ACServerIndex).Visible = False
		End If
		
		If m_bHasCobolLog Then
			lblLog(ACCobolIndex).Visible = True
			picLog(ACCobolIndex).Visible = True
		Else
			lblLog(ACCobolIndex).Visible = False
			picLog(ACCobolIndex).Visible = False
		End If
		
		' RDC 27092001 START
		' get support e-mail address from pm\sa\common registry
		Dim sSupportEmail As String = ADVReg.ReadRegistry(gPMConstants.HKEY_LOCAL_MACHINE, "SOFTWARE\PM\SiriusArchitecture\Common", "SupportEmailAddress")
		
		If sSupportEmail = "Not Found" Then
			' set to default address
			sSupportEmail = "siriussupport@siriusgroup.co.uk"
		End If
		
		' set form property
		txtRcpt.Text = sSupportEmail
		' RDC 27092001 END
		
		Status = gPMConstants.PMEReturnCode.PMCancel
		
	End Sub
	
	Private Sub lblLog_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _lblLog_3.Click, _lblLog_0.Click, _lblLog_2.Click, _lblLog_1.Click
		Dim Index As Integer = Array.IndexOf(lblLog, eventSender)
		
		picLog_Click(picLog(Index), New EventArgs())
		
	End Sub
	
	Private Sub picAttachments_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles picAttachments.Click
		
		
		For iLoop1 As Integer = 0 To 3
			picLog(iLoop1).BackColor = SystemColors.ControlLight
			lblLog(iLoop1).BackColor = SystemColors.ControlLight
		Next iLoop1
		
	End Sub
	
	Private Sub picLog_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _picLog_3.Click, _picLog_2.Click, _picLog_1.Click, _picLog_0.Click
		Dim Index As Integer = Array.IndexOf(picLog, eventSender)
		
		
		For iLoop1 As Integer = 0 To 3
			If Index = iLoop1 Then
				picLog(iLoop1).BackColor = Color.FromArgb(224, 224, 224)
				lblLog(iLoop1).BackColor = Color.FromArgb(224, 224, 224)
			Else
				picLog(iLoop1).BackColor = SystemColors.ControlLight
				lblLog(iLoop1).BackColor = SystemColors.ControlLight
			End If
		Next iLoop1
		
	End Sub
	
	Private Sub txtLogNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLogNumber.Enter
		
		txtLogNumber.SelectionStart = 0
		txtLogNumber.SelectionLength = Strings.Len(txtLogNumber.Text)
		
	End Sub
	
	'Private Sub txtRcpt_KeyDown(KeyCode As Integer, Shift As Integer)
	'
	' RDC 27092001 e-mail address comes from registry
	'    ' Lock or unlock the text
	'    If ((Shift And vbAltMask) And (KeyCode = vbKeyE)) Then
	'        If (txtRcpt.Locked = True) Then
	'            txtRcpt.Locked = False
	'            txtRcpt.BackColor = &H80000005
	'        Else
	'            txtRcpt.Locked = True
	'            txtRcpt.BackColor = &H80000013
	'        End If
	'    End If
	'
	'End Sub
End Class