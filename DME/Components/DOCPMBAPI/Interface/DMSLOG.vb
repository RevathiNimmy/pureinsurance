Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles

Module DMSLOG
	
	'Global Const LMSG = 1
	'Global Const LERR = 2
	'Global Const LLOG = 3
	'Global Const LWRN = 4
	'Global Const LDBG = 5
	
	Public g_fhLogFile As Integer
	Public g_sLogFileName As String = ""
	
	Sub CloseLogFile(ByRef sRootName As String)
		
		Try 
			
			If g_sAppType.Value = "WAPI" Then
				Exit Sub
			End If
			
			If FileSystem.LOF(g_fhLogFile) = 0 Then
				FileSystem.FileClose(g_fhLogFile)
                If DOCGeneralFunc.KillFile(sRootName & "logs\" & g_sLogFileName) = PM_FALSE Then
                End If
			Else
				FileSystem.FileClose(g_fhLogFile)
			End If
		
		Catch 
			
			
			
			Exit Sub
		End Try
		
		
	End Sub
	
    'Sub iPMFunc.LogMessage(ilogType As Long, sUserName As String, sMessage() As String)
	'
	'Dim sMessageType As String
	'Dim iCntr As Integer
	'
	'    On Error GoTo ErrLogMessage
	'
	'    If g_sAppType = "WAPI" Then
	'        Exit Sub
	'    End If
	'
	'    Select Case (ilogType)
	'        Case LMSG
	'            sMessageType = "MSG"
	'        Case LERR
	'            sMessageType = "ERR"
	'        Case LLOG
	'            sMessageType = "LOG"
	'        Case LWRN
	'            sMessageType = "WRN"
	'        Case LDBG
	'            sMessageType = "DBG"
	'    End Select
	'
	'    Print #g_fhLogFile, Format$(Now, "hh:mm:ss am/pm") & "  " & sMessageType & " - " & sUserName & ", " & sMessage(1)
	'
	'    For iCntr = 2 To UBound(sMessage)
	'        If (Trim$(sMessage(iCntr)) <> "") Then
	'            Print #g_fhLogFile, Space$((Len(sUserName) + 21)) & sMessage(iCntr)
	'        End If
	'    Next iCntr
	'
	'ErrLogMessage:
	'
	'    Exit Sub
	'
	'End Sub
	
	Function OpenLogFile(ByRef sRootName As String) As Integer
		
		Dim result As Integer = 0
		Dim sLogName As String = ""
		
		result = True
		
		If g_sAppType.Value = "WAPI" Then
			Return result
		End If
		
		Try 
			
			' Contruct log file name
			g_sLogFileName = DateTime.Now.ToString("ddMMyy") & ".log"
			
			g_fhLogFile = FileSystem.FreeFile()
			
			sLogName = FileSystem.Dir(sRootName & "logs\" & g_sLogFileName, ATTR_NORMAL)
			
			If sLogName <> "" Then
				FileSystem.FileOpen(g_fhLogFile, sRootName & "logs\" & g_sLogFileName, OpenMode.Append)
			Else
				FileSystem.FileOpen(g_fhLogFile, sRootName & "logs\" & g_sLogFileName, OpenMode.Output)
			End If
			
			Return result
		
		Catch 
			
			
			
			If Information.Err().Number = 55 Then
				' File is already open
				Return True
			Else
				Interaction.MsgBox("ERROR! " & Information.Err().Number & ", " & Conversion.ErrorToString(), MB_ICONEXCLAMATION, "Open Log File Error")
				Return False
			End If
			
			Return result
		End Try
	End Function
End Module