Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmMessage
	Inherits System.Windows.Forms.Form
	
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	Private Sub cmdNo_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNo.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMFalse
		Me.Hide()
	End Sub
	
	Private Sub cmdYes_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdYes.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMTrue
		Me.Hide()
	End Sub
	

	Private Sub frmMessage_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		m_lStatus = gPMConstants.PMEReturnCode.PMFalse
	End Sub
End Class