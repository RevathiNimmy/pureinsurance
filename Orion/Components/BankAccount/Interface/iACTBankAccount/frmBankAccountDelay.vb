Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no. 129 (guide)
Imports SharedFiles
Friend Partial Class frmBankAccountDelay
	Inherits System.Windows.Forms.Form
	Public Mode As gPMConstants.PMEComponentAction
	Public Status As gPMConstants.PMEReturnCode
	Public MediaTypeID As Integer
	Public DaysDelay As Integer
	Public MediaTypeDesc As String = ""
	'developer guide no. 7
	Private Const vbFormCode As Integer = 0
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		'Return Cancel
		Status = gPMConstants.PMEReturnCode.PMCancel
		Me.Hide()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		Dim dbNumericTemp As Double
		If Not Double.TryParse(txtDaysDelay.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
			MessageBox.Show("The Days Delay must be numeric.", "Invalid number", MessageBoxButtons.OK, MessageBoxIcon.Error)
		ElseIf Conversion.Val(txtDaysDelay.Text) <= 0 Or Conversion.Val(txtDaysDelay.Text) > 255 Then 
			MessageBox.Show("The Days Delay must be between 1 and 255.", "Invalid number", MessageBoxButtons.OK, MessageBoxIcon.Error)
		Else
			MediaTypeID = cboMediaTypeID.ItemId
			DaysDelay = CInt(txtDaysDelay.Text)
			MediaTypeDesc = cboMediaTypeID.ItemCaption.Trim()

			'Return OK
			Status = gPMConstants.PMEReturnCode.PMOK
			Me.Hide()
		End If
	End Sub
	

    Private Sub frmBankAccountDelay_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        'developer guide no. 220
        Me.cboMediaTypeID.FirstItem = ""
        If Mode = gPMConstants.PMEComponentAction.PMEdit Then
            cboMediaTypeID.ItemId = MediaTypeID
            txtDaysDelay.Text = CStr(DaysDelay)
        End If
    End Sub
	
	Private Sub frmBankAccountDelay_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        If UnloadMode <> vbFormCode Then
            Status = gPMConstants.PMEReturnCode.PMCancel
        End If
        eventArgs.Cancel = Cancel <> 0
    End Sub
End Class