Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. :129
Imports SharedFiles
Friend Partial Class frmAddPeril
	Inherits System.Windows.Forms.Form
	Private m_lStatus As Integer
	
	

	'Private Sub Status(ByVal Value As Integer)
		' Set the interface exit status.
		'm_lStatus = Value
	'End Sub
	Public ReadOnly Property Status() As Integer
		Get
			' Return the interface exit status.
			Return m_lStatus
		End Get
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		Me.Hide()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		Me.Hide()
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		FileSystem.FileClose()
	End Sub
	

	Private Sub frmAddPeril_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		SetInterfaceDefaults()
		FileSystem.FileClose()
	End Sub
	
	Private Sub SetInterfaceDefaults()
		Dim g_iSourceID As Integer
		
		Dim vValue As String = ""
		'CMG/PB See if LossSchedule is enabled and set a private boolean
		iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTLossSchedule, g_iSourceID, vValue)
		Dim bLossSchedule As Boolean = (gPMFunctions.NullToString(vValue) = "1")
		lblDescription.Visible = bLossSchedule
		txtDescription.Visible = bLossSchedule
		'End CMG
	End Sub

    Private Sub frmAddPeril_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'developer guide no.293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabAddPeril.SelectedIndex = 0
        End If
    End Sub
End Class