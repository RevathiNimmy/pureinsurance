Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'developer guide no. 129 (guide)
Imports SharedFiles
Partial Friend Class frmCancelPlanReason
    Inherits System.Windows.Forms.Form
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "frmCancelPlanReason"
	
	
	Private m_lCancelReasonID As Integer
	Private m_lStatus As Integer
	'Public property declaration
	Public Property CancelReasonID() As Integer
		Get
			Return m_lCancelReasonID
		End Get
		Set(ByVal Value As Integer)
			m_lCancelReasonID = Value
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
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		CancelReasonID = 0
		Status = gPMConstants.PMEReturnCode.PMCancel
		Me.Hide()
	End Sub
	
	Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
		CancelReasonID = gPMFunctions.ToSafeLong(cboCancelReason.ItemId)
		Status = gPMConstants.PMEReturnCode.PMOK
		Me.Hide()
	End Sub

    Private Sub frmCancelPlanReason_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Developer Guide No 220
        cboCancelReason.FirstItem = "(None)"
    End Sub
End Class