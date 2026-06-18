Option Strict Off
Option Explicit On
Imports System
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Public Class frmLoadAVI
	Inherits System.Windows.Forms.Form
	

	Private Sub frmLoadAVI_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		' Call DisableFormCloseButton(Me.hwnd)
		PMAPIFunc.SetTopMostWindow(Me.Handle.ToInt32(), True)
		
		'    CenterForm Me
		
		Application.DoEvents()
	End Sub
End Class