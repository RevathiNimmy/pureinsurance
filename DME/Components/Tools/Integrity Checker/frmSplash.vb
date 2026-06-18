Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.IO
Imports System.Windows.Forms
Friend Partial Class frmSplash
	Inherits System.Windows.Forms.Form
	Private Sub frmSplash_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	

	Private Sub frmSplash_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		
		Me.Width = pnlMain.Width
		Me.Height = pnlMain.Height
		
		Dim oFSO As New Object
		

		Dim oFile As FileInfo = New FileInfo(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe")
		
		Dim iMajor As Integer = Math.Floor(My.Application.Info.Version.Major / 10)
		Dim iMinor As Integer = My.Application.Info.Version.Major - (iMajor * 10)
		
		lblVersion.Text = "Version: " & iMajor & "." & CStr(iMinor) & "." & CStr(My.Application.Info.Version.Minor) & "." & CStr(My.Application.Info.Version.Revision)
		lblDate.Text = "Build date: " & oFile.LastWriteTime.ToString("dd MMMM yyyy")
		
		oFile = Nothing
		oFSO = Nothing
		
		Application.DoEvents()
		
	End Sub
End Class