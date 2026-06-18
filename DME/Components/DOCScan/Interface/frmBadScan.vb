Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Friend Partial Class frmBadScan
	Inherits System.Windows.Forms.Form
	
	Private Sub cmdScanAgain_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdScanAgain.Click
		
		Me.Close()
        'TODO: Needs to be checked
        'frmInterface.cmdReScan_Click()

		
	End Sub
	
	Private Sub Command1_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Command1.Click
		
		Me.Close()
		
	End Sub
End Class