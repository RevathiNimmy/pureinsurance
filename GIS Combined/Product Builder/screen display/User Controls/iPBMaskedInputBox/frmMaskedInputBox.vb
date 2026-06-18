Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Friend Partial Class frmMaskedInputBox
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form:     Interface for iPBMaskedInputBox component
	' Shared:   No
	' Needs:    Standard Sirius Constant modules e.g. gPMConstants
	'
	' Form to capture generic data input, displaying the data entered as
	' '*' characters for protection. Captions for the title bar and input
	' caption need to be set by the calling application.
	'
	' Edit History: CJB 13/11/02 Created
	'
	' ***************************************************************** '
	
	
	Private Const ACClass As String = "frmMaskedInputBox"
	
	' TitleBar caption of form
	Private m_sTitleBarCaption As String = ""
	
	' Label on form to indicate what input is required
	Private m_sMaskedInputBoxCaption As String = ""
	
	' Holds the data captured
	Private m_sInputCaptured As String = ""
	
	Public Property TitleBarCaption() As String
		Get
			Return m_sTitleBarCaption
		End Get
		Set(ByVal Value As String)
			m_sTitleBarCaption = Value
		End Set
	End Property
	
	Public Property MaskedInputBoxCaption() As String
		Get
			Return m_sMaskedInputBoxCaption
		End Get
		Set(ByVal Value As String)
			m_sMaskedInputBoxCaption = Value
		End Set
	End Property
	
	Public Property InputCaptured() As String
		Get
			Return m_sInputCaptured
		End Get
		Set(ByVal Value As String)
			m_sInputCaptured = Value
		End Set
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		' Hide the form
		Me.Hide()
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		' Store the data and hide the form
		m_sInputCaptured = txtMaskedInput.Text
		Me.Hide()
		
	End Sub
	

	Private Sub frmMaskedInputBox_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		' Set caption of form and Masked Input Box Caption
		Me.Text = m_sTitleBarCaption
		lblCaption.Text = m_sMaskedInputBoxCaption
		
	End Sub
	
	Private Sub txtMaskedInput_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMaskedInput.Enter
		
		' Highlight the text
		txtMaskedInput.SelectionStart = 0
		txtMaskedInput.SelectionLength = Strings.Len(txtMaskedInput.Text)
		
	End Sub
End Class