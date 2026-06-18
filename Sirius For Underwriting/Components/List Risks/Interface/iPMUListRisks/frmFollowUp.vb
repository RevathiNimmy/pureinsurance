Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports Sharedfiles
Friend Partial Class frmFollowUp
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	'
	' Name: frmFollowUp
	'
	' Description: Follow up form to capture the follow up details when
	' a quote is marked as "hot"
	'
	' Created: PW311002
	'
	' ***************************************************************** '
	
	Private Const ACClass As String = "frmFollowUp"
	
	' Private variables
	Private m_iStatus As gPMConstants.PMEReturnCode
	
	Public ReadOnly Property Status() As Integer
		Get
			
			Return m_iStatus
			
		End Get
	End Property
	
	
	
	Public Property FollowUpNote() As String
		Get
			
			Return txtFollowUpNote.Text
			
		End Get
		Set(ByVal Value As String)
			
			txtFollowUpNote.Text = Value.Substring(0, 255)
			
		End Set
	End Property
	
	
	Public Property ReferredTo() As String
		Get
			
			Return txtReferredTo.Text
			
		End Get
		Set(ByVal Value As String)
			
			txtReferredTo.Text = Value.Substring(0, 255)
			
		End Set
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		'
		' Set status to cancelled and hide form
		'
		m_iStatus = gPMConstants.PMEReturnCode.PMCancel
		Me.Hide()
		
	End Sub
	
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		'
		' Set status to OK and hide form
		'
		m_iStatus = gPMConstants.PMEReturnCode.PMOK
		Me.Hide()
		
	End Sub
	

	Private Sub frmFollowUp_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
	End Sub
End Class