Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmNotesPreview
	Inherits System.Windows.Forms.Form
	
	Private m_sRTFText As String = ""
	Private m_lTask As gPMConstants.PMEComponentAction
	
	Private Sub cmdClose_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClose.Click
		Me.Close()
	End Sub
	
	
	Public Property RTFText() As String
		Get
			Return m_sRTFText
		End Get
		Set(ByVal Value As String)
			m_sRTFText = Value
		End Set
	End Property
	
	
	Public Property Task() As Integer
		Get
			Return m_lTask
		End Get
		Set(ByVal Value As Integer)
			m_lTask = Value
		End Set
	End Property
	

	Private Sub frmNotesPreview_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		If m_lTask = gPMConstants.PMEComponentAction.PMView Then
			uctRichTextBox1.Locked = True
		End If
		uctRichTextBox1.ShowToolbar = True
        uctRichTextBox1.Text = m_sRTFText
	End Sub
End Class