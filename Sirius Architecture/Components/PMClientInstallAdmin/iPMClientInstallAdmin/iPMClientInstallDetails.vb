Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmDetails
	Inherits System.Windows.Forms.Form
	
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_bDisableCancel As Boolean
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	Public WriteOnly Property DisableQuit() As Boolean
		Set(ByVal Value As Boolean)
			m_bDisableCancel = Value
		End Set
	End Property
	
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		Me.Hide()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		Me.Hide()
	End Sub
	
	Private Sub frmDetails_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			cmdOK.Focus()
			If m_bDisableCancel Then
				cmdCancel.Enabled = False
			End If
		End If
	End Sub
	

	Private Sub frmDetails_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
	End Sub
End Class