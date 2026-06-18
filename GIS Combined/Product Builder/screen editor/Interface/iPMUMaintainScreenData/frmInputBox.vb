Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
Friend Partial Class frmInputBox
	Inherits System.Windows.Forms.Form
	Private Sub frmInputBox_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	
	Private m_sValue As String = ""
	
	Public WriteOnly Property Prompt() As String
		Set(ByVal Value As String)
			lblPrompt.Text = Value
			txtValue.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(lblPrompt.Top) + VB6.PixelsToTwipsY(lblPrompt.Height) + 100)
			txtValue.Height = cmdOK.Top - VB6.TwipsToPixelsY(100) - txtValue.Top
		End Set
	End Property
	
	
	Public Property Value() As String
		Get
			Return m_sValue
		End Get
		Set(ByVal Value As String)
			m_sValue = Value
			txtValue.Text = Value
		End Set
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		Hide()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		m_sValue = txtValue.Text
		Hide()
		
	End Sub
End Class