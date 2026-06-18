Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Public Class frmChangeStatus
	Inherits System.Windows.Forms.Form
	Private Sub frmChangeStatus_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	
	Private Const ACClass As String = "frmFilterRenewal"
	
	Private m_lStatus As Integer
	
	Private m_lRenewalStatusTypeID As Integer
	Private m_sRenewalStatusType As String = ""
	
	
	Public Property Status() As Integer
		Get
			Return m_lStatus
		End Get
		Set(ByVal Value As Integer)
			m_lStatus = Value
		End Set
	End Property
	
	Public ReadOnly Property RenewalStatusTypeID() As Integer
		Get
			Return m_lRenewalStatusTypeID
		End Get
	End Property
	
	Public ReadOnly Property RenewalStatusType() As String
		Get
			Return m_sRenewalStatusType
		End Get
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		Me.Hide()
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		m_lRenewalStatusTypeID = cboRenewalStatusType.ItemId
		m_sRenewalStatusType = cboRenewalStatusType.ItemCaption
		
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		
		Me.Hide()
		
	End Sub
End Class