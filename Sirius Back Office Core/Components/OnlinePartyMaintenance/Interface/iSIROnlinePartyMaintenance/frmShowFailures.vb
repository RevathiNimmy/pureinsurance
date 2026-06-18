Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
Friend Partial Class frmShowFailures
	Inherits System.Windows.Forms.Form
	Private Sub frmShowFailures_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	
	'UPGRADE_NOTE: (7001) The following declaration (Command1_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub Command1_Click()
		'
	'End Sub
	
	
	Private Sub cmdExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExit.Click
		Me.Close()
	End Sub
End Class