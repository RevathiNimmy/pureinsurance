Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'Developer Guide no. 129
Imports SharedFiles
Friend Partial Class frmEditWarning
	Inherits System.Windows.Forms.Form
	Private m_lDisplayMode As Integer
	
	Public ReadOnly Property DisplayMode() As Integer
		Get
			Return m_lDisplayMode
		End Get
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_lDisplayMode = gPMConstants.PMEReturnCode.PMCancel
		Me.Hide()
	End Sub
	
	Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
		m_lDisplayMode = gPMConstants.PMEComponentAction.PMEdit
		Me.Hide()
	End Sub
	
	Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click
		m_lDisplayMode = gPMConstants.PMEComponentAction.PMView
		Me.Hide()
	End Sub
End Class