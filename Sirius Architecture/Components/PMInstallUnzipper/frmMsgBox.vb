Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Friend Partial Class frmMsgBox
	Inherits System.Windows.Forms.Form
	
	Private m_lMessageType As Integer ' uses msgbox icon constants eg. vbInformation
	Private m_sMessageText As String = "" ' main message text
	Private m_sMessageCaption As String = "" ' form caption
	Private m_lCountdown As Integer ' timeout in seconds
	Private m_lResponse As Integer ' key pressed
	
	Public WriteOnly Property MessageType() As Integer
		Set(ByVal Value As Integer)
			m_lMessageType = Value
		End Set
	End Property
	
	Public WriteOnly Property MessageText() As String
		Set(ByVal Value As String)
			m_sMessageText = Value
		End Set
	End Property
	
	Public WriteOnly Property MessageCaption() As String
		Set(ByVal Value As String)
			m_sMessageCaption = Value
		End Set
	End Property
	
	Public WriteOnly Property Countdown() As Integer
		Set(ByVal Value As Integer)
			m_lCountdown = Value
		End Set
	End Property
	
	Public ReadOnly Property Response() As Integer
		Get
			Return m_lResponse
		End Get
	End Property
	
	Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
		
		m_lResponse = System.Windows.Forms.DialogResult.OK
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_lResponse = System.Windows.Forms.DialogResult.Cancel
		
		Me.Close()
		
	End Sub
	
	Private Sub frmMsgBox_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			cmdCancel.Focus()
			
		End If
	End Sub
	

	Private Sub frmMsgBox_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Dim oImage As PictureBox
		
		m_lResponse = 0 ' user response, if any
		
		' message & caption
		lblMessage.Text = m_sMessageText
		Me.Text = m_sMessageCaption
		
		' position and display icon
		Select Case m_lMessageType
			Case MsgBoxStyle.Information
				oImage = imgInformation
			Case MsgBoxStyle.Exclamation
				oImage = imgExclamation
			Case MsgBoxStyle.Question
				oImage = imgQuestion
			Case MsgBoxStyle.Critical
				oImage = imgCritical
			Case Else
				oImage = Nothing
		End Select
		
		If Not (oImage Is Nothing) Then
			oImage.Visible = True
			oImage.Top = VB6.TwipsToPixelsY(180)
			oImage.Left = VB6.TwipsToPixelsX(180)
		End If
		
		oImage = Nothing
		
		' size and position form and buttons
		Me.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lblMessage.Left) + VB6.PixelsToTwipsX(lblMessage.Width) + 240)
		
		cmdOk.Left = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(Me.Width) / 2) - VB6.PixelsToTwipsX(cmdOk.Width) - 120)
		cmdOk.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(lblMessage.Top) + VB6.PixelsToTwipsY(lblMessage.Height) + 240)
		
		cmdCancel.Left = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(Me.Width) / 2) + 120)
		cmdCancel.Top = cmdOk.Top
		
		Me.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(cmdOk.Top) + VB6.PixelsToTwipsY(cmdOk.Height) + 540)
		
		m_lCountdown += 1
		
		If m_lCountdown = 1 Then
			m_lCountdown = 61
		End If
		
		' first tick
		tmrCountdown.Enabled = True
		tmrCountdown_Tick(tmrCountdown, New EventArgs())
		
	End Sub
	
	Private Sub tmrCountdown_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tmrCountdown.Tick
		
		m_lCountdown -= 1
		
		cmdOk.Text = "&Ok ... " & m_lCountdown
		
		If m_lCountdown = 0 Then
			Me.Close()
		End If
		
	End Sub
End Class