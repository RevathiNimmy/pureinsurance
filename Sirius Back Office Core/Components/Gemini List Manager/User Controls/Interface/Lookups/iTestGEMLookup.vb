Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
Friend Partial Class Form1
	Inherits System.Windows.Forms.Form
	Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
		
		
		Dim lItem As Integer = Lookup1.ItemId
		
		ControlHelper.Print(Me, CStr(lItem))
		
	End Sub
	

	Private Sub Form1_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		Lookup1.ItemId = 6
		
	End Sub
End Class