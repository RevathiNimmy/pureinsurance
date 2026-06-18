Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Friend Partial Class frmCancelPoliciesFailed
	Inherits System.Windows.Forms.Form
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "frmCancelPoliciesFailed"
	Private m_sErrors As String = ""
	
	Public Property Errors() As String
		Get
			Return m_sErrors
		End Get
		Set(ByVal Value As String)
			m_sErrors = Value
		End Set
	End Property
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		Me.Hide()
	End Sub
	

	Private Sub frmCancelPoliciesFailed_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		ShowErrors()
	End Sub
	Private Function ShowErrors() As Integer
		
		Dim sErrors As String = Errors
		Dim vErrors As Object = sErrors.Split(";"c)

		Dim nLower As Integer = vErrors.GetLowerBound(0)

		Dim nUpper As Integer = vErrors.GetUpperBound(0)
		For nCount As Integer = nLower To nUpper

            txtPolicies.Text = txtPolicies.Text & CStr(vErrors(nCount)) & Strings.Chr(13) & Strings.Chr(10)
		Next 
	End Function
End Class