Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Module MainModule
	
	' ********************************************************************************
	'
	' This project needs to be generic through all versions of SA and DME so we don't
	' use any of the shared modules (at present)
	'
	' ********************************************************************************
	
	'Public Const ACSQLCommand As String = "isql -S {server} -U {username} -P {password} -d {database} -i {filename}"
	Public Const ACSQLCommand As String = "isql -S {server} -U {username} -P {password} -d {database} -o {outputfile} -i {filename}"
	
	Public Const ACApp As String = "DMEHarmoniser"
	
	' Prefix on the front of merged tables
	Public Const ACDocTablePrefix As String = "DOC_"
	
	' Magic word that makes the second tab appear.
	Public Const ACMagicWord As String = "GOD"
	'end
	
	Public Sub LogFile(ByVal v_sLogFile As String, ByVal v_sText As String)
		
		
		Dim lFileHandle As Integer = FileSystem.FreeFile()
		
		' Open the file for writing
		FileSystem.FileOpen(lFileHandle, v_sLogFile, OpenMode.Append)
		
		' Output our text
		FileSystem.PrintLine(lFileHandle, DateTime.Now.ToString("HH:mm:ss") & "  " & v_sText)
		
		' Close the file
		FileSystem.FileClose(lFileHandle)
		
	End Sub
	
	
	Public Sub LogMessageShort(ByVal v_sMessage As String, ByVal v_sTitle As String)
		
		MessageBox.Show("Error occurred:" & Environment.NewLine & v_sMessage, v_sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
		
	End Sub
End Module