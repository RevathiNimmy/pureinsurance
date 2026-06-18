Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports System
Imports System.Windows.Forms
Friend Partial Class frmDummy
	Inherits System.Windows.Forms.Form
	

	Private Sub frmDummy_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
	End Sub
	Private Sub frmDummy_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		MemoryHelper.ReleaseMemory()
	End Sub
End Class