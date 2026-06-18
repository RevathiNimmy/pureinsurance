Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Windows.Forms
Friend Partial Class frmSQL
	Inherits System.Windows.Forms.Form
	
	Private Sub Command1_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Command1.Click
		
		Dim sSQL As String = ""
		Dim lFile As Integer
		
		Try 
			
			lFile = FileSystem.FreeFile()
			
			FileSystem.FileOpen(lFile, txtFilename.Text, OpenMode.Input)
			
			txtVBCode.Text = "Public Property Get ChangeMe() As String" & Environment.NewLine
			txtVBCode.Text = txtVBCode.Text & "ChangeMe = "
			
			Dim lCount As Integer
			
			lCount = 0
			
			While Not FileSystem.EOF(lFile)
				
				sSQL = FileSystem.LineInput(lFile)
				
				txtSQL.Text = txtSQL.Text & Environment.NewLine & sSQL
				
				If sSQL.Trim() <> "" Then
					
					' strip comments
					If sSQL.TrimStart().Substring(0, 2) <> "--" Then
						
						If lCount Mod 2 Then
							' start line
							txtVBCode.Text = txtVBCode.Text & Strings.Chr(34).ToString() & sSQL.Trim() & " "
						Else
							' end line
							txtVBCode.Text = txtVBCode.Text & sSQL.Trim() & Strings.Chr(34).ToString() & " & _" & Environment.NewLine
						End If
						
						lCount += 1
						
					End If
					
				End If
				
			End While
			
			FileSystem.FileClose(lFile)
			
			
			txtVBCode.Text = txtVBCode.Text & Strings.Chr(34).ToString() & Strings.Chr(34).ToString() & Environment.NewLine & "End Property"
		
		Catch excep As System.Exception
			
			
			
			MessageBox.Show("Failed to open file " & txtFilename.Text, excep.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
			
			Exit Sub
			
		End Try
		
	End Sub
End Class