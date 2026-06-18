Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		'Set cancelled flag
		g_bCancelProcessing = True
		
	End Sub
	
	Private Sub cmdPause_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPause.Click
		
		'Set paused flag
		g_bPauseProcessing = True
		
	End Sub
	
	'UPGRADE_NOTE: (7001) The following declaration (Form_QueryUnload) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub Form_QueryUnload(ByRef Cancel As Integer, ByRef UnloadMode As Integer)
		'
		'only allow form to close from cancel button request
		'If UnloadMode = 0 Then
			'g_bCancelProcessing = True
			'Cancel = 1
		'End If
		'
	'End Sub
	'UPGRADE_NOTE: (7001) The following declaration (Form_Unload) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub Form_Unload(ByRef Cancel As Integer)
	'End Sub
End Class