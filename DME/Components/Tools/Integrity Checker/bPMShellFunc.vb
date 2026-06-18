Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
Module ShellFunc
	
	Public Const PROCESS_QUERY_INFORMATION As Integer = &H400s
	Public Const STATUS_PENDING As Integer = &H103
	
	Public Declare Function ShellExecute Lib "shell32.dll"  Alias "ShellExecuteA"(ByVal hwnd As Integer, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As Integer) As Integer
	
	Public Declare Function OpenProcess Lib "kernel32" (ByVal dwDesiredAccess As Integer, ByVal bInheritHandle As Integer, ByVal dwProcessId As Integer) As Integer
	
	Public Declare Function GetExitCodeProcess Lib "kernel32" (ByVal hProcess As Integer, ByRef lpExitCode As Integer) As Integer
	
	Public Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Integer) As Integer
	
	Public Sub ShellWait(ByVal v_sCommandLine As String)
		
		Dim lProcess, hProcess, lExitCode As Integer
		
		Try 
			
			' Execute it

			lProcess = CInt(Process.Start(v_sCommandLine).Id)
			
			' Get a handle on the process
			hProcess = OpenProcess(PROCESS_QUERY_INFORMATION, False, lProcess)
			
			Do 
				' Check if its exited yet
				GetExitCodeProcess(hProcess, lExitCode)
				' The fabulous DoEvents to stop things hanging
				Application.DoEvents()
			Loop While lExitCode = STATUS_PENDING
			
			' Get rid of the handle we created
			CloseHandle(hProcess)
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
			MessageBox.Show("Error : " & excep.Message, "Error in iPMBInstaller.ShellWait", MessageBoxButtons.OK, MessageBoxIcon.Error)
			
			Exit Sub
			
		End Try
		
	End Sub
End Module