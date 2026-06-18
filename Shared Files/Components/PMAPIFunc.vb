Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("PMAPIFunc_NET.PMAPIFunc")> _
 Public Module PMAPIFunc
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	' ********************************************************************************
	'
	' Name: PMAPIFunc
	'
	' Desc: Various API Functions used by PMB components.
	'
	' Edit History:
	'                   CTAF    21/05/99    Original.
	'                   RFC     17/09/99    PMGetTempPath added.
	'                   CTAF    05/10/99    PMShellWait added.
	' ********************************************************************************
	
	Private Const ACClass As String = "PMAPIFunc"
	
	' Declarations
	Declare Function FindWindow Lib "user32"  Alias "FindWindowA"(ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
	Declare Function ShowWindow Lib "user32" (ByVal hwnd As Integer, ByVal nCmdShow As Integer) As Integer
	Declare Function GetTempPath Lib "kernel32"  Alias "GetTempPathA"(ByVal nBufferLength As Integer, ByVal lpBuffer As String) As Integer
	
	Private Declare Function OpenProcess Lib "kernel32" (ByVal dwDesiredAccess As Integer, ByVal bInheritHandle As Integer, ByVal dwProcessId As Integer) As Integer
	
	Private Declare Function GetExitCodeProcess Lib "kernel32" (ByVal hProcess As Integer, ByRef lpExitCode As Integer) As Integer
	
	Private Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Integer) As Integer
	
	'sj 20/08/2001 - start
	Declare Function SetWindowPos Lib "user32" (ByVal hwnd As Integer, ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer
	
	Public Declare Function GetSystemMenu Lib "user32" (ByVal hwnd As Integer, ByVal bRevert As Integer) As Integer
	Public Declare Function DeleteMenu Lib "user32" (ByVal hMenu As Integer, ByVal nPosition As Integer, ByVal wFlags As Integer) As Integer
	'sj 20/08/2001 - end
	
	' Constants
	' Public
	Public Const SW_SHOWMAXIMIZED As Integer = 3
	' Private
	Private Const PROCESS_QUERY_INFORMATION As Integer = &H400s
	Private Const STATUS_PENDING As Integer = &H103
	
	' Commonly used window classes
	' VB MDI Form
	Public Const WNDCLSVBMDIForm As String = "ThunderRT5MDIForm"
	' VB Form
	Public Const WNDCLSVBForm As String = "ThunderRT5Form"
	' VB Command Button
	Public Const WNDCLSVBCommand As String = "ThunderRT5CommandButton"
	Public Const SWP_NOMOVE As Integer = 2
	
	'sj 20/08/2001 - start
	Public Const SWP_NOSIZE As Integer = 1
	Public Const FLAGS As Integer = SWP_NOMOVE Or SWP_NOSIZE
	Public Const HWND_TOPMOST As Integer = -1
	Public Const HWND_NOTOPMOST As Integer = -2
	
	Public Const SC_CLOSE As Integer = &HF060s
	Public Const MF_BYCOMMAND As Integer = &H0
	'sj 20/08/2001 - end
	
	' ***************************************************************** '
	'
	' Name: SetTopMostWindow
	'
	' Description: Make a Window always appear on top
	'
	' History: 20/08/2001 sj - Created.
	'
	' ***************************************************************** '
	Public Function SetTopMostWindow(ByVal v_hWnd As Integer, ByVal v_bTopmost As Boolean) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			
			If v_bTopmost Then 'Make the window topmost
				Return SetWindowPos(v_hWnd, HWND_TOPMOST, 0, 0, 0, 0, FLAGS)
			Else
				result = SetWindowPos(v_hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, FLAGS)
				Return False
			End If
		
		Catch 
		End Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMError
		
		' Log Error Message
        bPMFunc.LogMessage(susername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetTopMostWindow Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetTopMostWindow", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
	
	' ***************************************************************** '
	'
	' Name: DisableFormCloseButton
	'
	' Description: Disables 'X' button on the form using an API call
	'
	' History: 20/08/2001 sj - Created.
	'
	' ***************************************************************** '
	Public Function DisableFormCloseButton(ByVal v_hWnd As Integer) As Integer
		
		Dim result As Integer = 0
        Dim hMenuHandle, hClose As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			hMenuHandle = GetSystemMenu(v_hWnd, False)
			If hMenuHandle <> 0 Then
				hClose = DeleteMenu(hMenuHandle, SC_CLOSE, MF_BYCOMMAND)
			End If
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			bPMFunc.LogMessage(susername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisableFormCloseButton Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableFormCloseButton", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name: PMFindWindow
	'
	' Description: Searchs for a window with caption v_sCaption,
	'              if found sets r_bExists to true and puts the hWnd of
	'              the window in r_hWnd.
	'
	' ***************************************************************** '
	Public Function PMFindWindow(ByVal v_sCaption As String, ByRef r_bExists As Boolean, ByRef r_hWnd As Integer, Optional ByVal v_sClassName As String = "") As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If Not True Then
				v_sClassName = Nothing
			End If
			
			' Find the window
			r_hWnd = FindWindow(lpClassName:=v_sClassName, lpWindowName:=v_sCaption)
			
			' If we have a handle, then it exists
			If r_hWnd <> 0 Then
				r_bExists = True
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			bPMFunc.LogMessage(susername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PMFindWindow Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PMFindWindow", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: PMMaximizeWindow
	'
	' Description: Maximizes and activates the window.
	'
	' ***************************************************************** '
	Public Function PMMaximizeWindow(ByVal v_hWnd As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			ShowWindow(hwnd:=v_hWnd, nCmdShow:=SW_SHOWMAXIMIZED)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			bPMFunc.LogMessage(susername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PMMaximizeWindow Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PMMaximizeWindow", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: PMGetTempPath
	'
	' Description: Gets the Temp Folder Path and Creates it if it doesn't
	'              already exist.
	'
	' RFC170999
	' ***************************************************************** '
	Public Function PMGetTempPath(ByRef r_sTempPath As String) As Integer
		
		Dim result As Integer = 0
		Dim lResult As Integer
		Dim sPath As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			sPath = New String(" "c, 255)
			
			lResult = GetTempPath(255, sPath) ' Get system temp folder path
			
			sPath = sPath.Trim()
			
			If sPath.Length > 0 Then
				If sPath.EndsWith(Strings.Chr(0).ToString()) Then
					sPath = sPath.Substring(0, sPath.Length - 1) ' strip null char
				End If
			End If
			
			' Make sure the temp directory exists... if not create it
			
			If sPath.EndsWith("\") Then
				sPath = sPath.Substring(0, sPath.Length - 1) ' Chop off \ at end
			End If
			
			If Not Directory.Exists(sPath) Then
				Directory.CreateDirectory(sPath) ' Create it
			End If
			
			' Return the Temp Path
			r_sTempPath = sPath
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			bPMFunc.LogMessage(susername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PMGetTempPathFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="PMGetTempPath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: ShellWait
	'
	' Description: Starts a program and waits for it to finish.
	'
	' History: 05/10/1999 CTAF - Created.
	'
	' ***************************************************************** '
	Public Function PMShellWait(ByVal v_sCommandLine As String, ByRef WindowStyle As ProcessWindowStyle) As Integer
		
		Dim result As Integer = 0
		Dim lProcess, hProcess, lExitCode As Integer
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		Try 
			
			' Execute it

			Dim startInfo As ProcessStartInfo = New ProcessStartInfo(v_sCommandLine)
			startInfo.WindowStyle = WindowStyle
			lProcess = CInt(Process.Start(startInfo).Id)
			
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
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' CTAF 280100 - more useful message for file not found
			Select Case Information.Err().Number
				Case 53
					' Log Error Message
					bPMFunc.LogMessage(susername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to start : " & v_sCommandLine, vApp:=ACApp, vClass:=ACClass, vMethod:="PMShellWait", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
				Case Else
					' Log Error Message
					bPMFunc.LogMessage(susername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PMShellWait Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PMShellWait", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			End Select
			
			Return result
			
		End Try
	End Function
End Module