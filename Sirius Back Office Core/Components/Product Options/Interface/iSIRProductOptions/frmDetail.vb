Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmDetail
	Inherits System.Windows.Forms.Form
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		'User cancel
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		txtPassword.Text = ""
		Me.Hide()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		Me.Hide()
	End Sub
	

	Private Sub frmDetail_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		'Remove hourglass
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		'Set caption
		lblCaption.Text = g_sPasswordMessage
		
		'Clear Password
		txtPassword.Text = ""
		
	End Sub
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	Private Sub frmDetail_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
        'User cancel
        'Modified,add the if condition
        If UnloadMode <> 0 Then
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            txtPassword.Text = ""
            Me.Hide()
            eventArgs.Cancel = Cancel <> 0
        End If

       
    End Sub
End Class