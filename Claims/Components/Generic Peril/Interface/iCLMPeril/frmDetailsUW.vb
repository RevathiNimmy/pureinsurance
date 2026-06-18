Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
Friend Partial Class frmDetailsUW
	Inherits System.Windows.Forms.Form
	Private Sub frmDetailsUW_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
End Class