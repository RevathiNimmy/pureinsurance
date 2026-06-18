Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.DB.DAO
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Data
Imports System.Data.Common
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Module PMAPI
	
	' Microsoft Windows 3.1 API routines.
	'Declare Function GetPrivateProfileInt% Lib "kernel" (ByVal lpAppName$, ByVal lpKeyName$, ByVal nDefault%, ByVal lpFileName$)
	Declare Function GetPrivateProfileInt Lib "kernel32"  Alias "GetPrivateProfileIntA"(ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal nDefault As Integer, ByVal lpFileName As String) As Integer
	'Declare Function GetPrivateProfileString% Lib "kernel" (ByVal lpApplicationName$, ByVal lpKeyName$, ByVal nDefault$, ByVal lpReturnString$, ByVal nSize%, ByVal lpFileName$)
	Declare Function GetPrivateProfileString Lib "kernel32"  Alias "GetPrivateProfileStringA"(ByVal lpApplicationName As String, ByVal lpKeyName As Integer, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
	
	Declare Function WritePrivateProfileInt Lib "kernel.dll" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpValue As Short, ByVal lpFileName As String) As Short
	'TODO: No Win32 API known for function WRITEPRIVATEPROFILEINT%.  Convert to different API function.
	Declare Function WritePrivateProfileStringByNum Lib "kernel.dll"  Alias "WritePrivateProfileString"(ByVal lpAppName As String, ByVal lpKeyName As Integer, ByVal lpString As Integer, ByVal lpFileName As String) As Short
	'TODO: No Win32 API known for function WRITEPRIVATEPROFILESTRINGBYNUM%.  Convert to different API function.
	'Declare Function WritePrivateProfileString% Lib "Kernel" (ByVal lpAppName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lplFileName As String)
	Declare Function WritePrivateProfileString Lib "kernel32"  Alias "WritePrivateProfileStringA"(ByVal lpApplicationName As String, ByVal lpKeyName As Integer, ByVal lpString As Integer, ByVal lpFileName As String) As Integer
	
	'Declare Function FindWindow% Lib "user" (ByVal lpClassName As Any, ByVal lpCaption As Any)
	Declare Function FindWindow Lib "user32"  Alias "FindWindowA"(ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
	'Declare Function ShowWindow% Lib "User" (ByVal Handle As Integer, ByVal Cmd As Integer)
	Declare Function ShowWindow Lib "user32" (ByVal hWnd As Integer, ByVal nCmdShow As Integer) As Integer
	Declare Function SFocus Lib "USER.dll"  Alias "SetFocus"(ByVal Handle As Short) As Short
	'TODO: No Win32 API known for function SFOCUS%.  Convert to different API function.
	
	'Declare Function GlobalFindAtom% Lib "User" (ByVal lpString$)
	Declare Function GlobalFindAtom Lib "kernel32"  Alias "GlobalFindAtomA"(ByVal lpString As String) As Short
	'Declare Function GlobalAddAtom% Lib "User" (ByVal lpString$)
	Declare Function GlobalAddAtom Lib "kernel32"  Alias "GlobalAddAtomA"(ByVal lpString As String) As Short
	'Declare Function GlobalDeleteAtom% Lib "User" (ByVal nAtom%)
	Declare Function GlobalDeleteAtom Lib "kernel32" (ByVal nAtom As Short) As Short
	
	Declare Function GetModuleUsage Lib "kernel.dll" (ByVal hModule As Short) As Short
	'TODO: No Win32 API known for function GETMODULEUSAGE%.  Convert to different API function.
	
    'Declare Function GetKeyState% Lib "User" (ByVal nVirtKey%)
	Declare Function GetKeyState Lib "user32" (ByVal nVirtKey As Integer ) As Short
	
	'Declare Function GetDriveType% Lib "Kernel" (ByVal nDrive%)
	Declare Function GetDriveType Lib "kernel32"  Alias "GetDriveTypeA"(ByVal nDrive As String) As Integer
	Declare Sub GetDiskInfo Lib "diskinfo.dll" (ByVal mydrive As String, ByVal myvolume As String, ByRef free As Integer)
	'TODO: WARNING Third-party DLL subroutine GETDISKINFO must be converted to 32-bit version.
	Declare Sub GetDiskFree Lib "diskinfo.dll" (ByVal drv As String, ByRef free As Integer, ByRef tot As Integer)
	'TODO: WARNING Third-party DLL subroutine GETDISKFREE must be converted to 32-bit version.
	
	' 3D form settings.
	'Global Const BUTTON_FACE = &H8000000F
	'Global Const FIXED_DOUBLE = 3
	Public Const DS_MODALFRAME As Integer = &H80
	Public Const GWL_STYLE As Integer = (-16)
	Public Const GWW_HINSTANCE As Integer = (-6)
	
	Declare Function Ctl3dAutoSubclass Lib "CTL3DV2.DLL" (ByVal hInst As Short) As Short
	'TODO: WARNING Third-party DLL function CTL3DAUTOSUBCLASS% must be converted to 32-bit version.
	Declare Function Ctl3dSubclassDlgEx Lib "CTL3DV2.DLL" (ByVal hWnd As Short, ByVal Flags As Integer) As Short
	'TODO: WARNING Third-party DLL function CTL3DSUBCLASSDLGEX% must be converted to 32-bit version.
	Declare Function Ctl3dRegister Lib "CTL3DV2.DLL" (ByVal hInst As Short) As Short
	'TODO: WARNING Third-party DLL function CTL3DREGISTER% must be converted to 32-bit version.
	Declare Function Ctl3dUnregister Lib "CTL3DV2.DLL" (ByVal hInst As Short) As Short
	'TODO: WARNING Third-party DLL function CTL3DUNREGISTER% must be converted to 32-bit version.
	'Declare Function GetWindowLong& Lib "User" (ByVal hWnd%, ByVal nIndex%)
	Declare Function GetWindowLong Lib "user32"  Alias "GetWindowLongA"(ByVal hWnd As Integer, ByVal nIndex As Integer) As Integer
	'Declare Function GetWindowWord% Lib "User" (ByVal hWnd%, ByVal nIndex%)
	Declare Function GetWindowWord Lib "user32" (ByVal hWnd As Integer, ByVal nIndex As Integer) As Short
	'Declare Function SetWindowLong& Lib "User" (ByVal hWnd%, ByVal nIndex%, ByVal dwNewLong&)
	Declare Function SetWindowLong Lib "user32"  Alias "SetWindowLongA"(ByVal hWnd As Integer, ByVal nIndex As Integer, ByVal dwNewLong As Integer) As Integer
	
	
	Const INI_FILENAME As String = "PMDMS.INI"
	
	Sub Ctl3DExit(ByRef hWnd As Integer)
		
		Exit Sub
		
		Dim hInst As Integer = GetWindowWord(hWnd, GWW_HINSTANCE)
		Dim iResult As Integer = Ctl3dUnregister(CShort(hInst))
		
	End Sub
	
	Sub Ctl3DForm(ByRef frm As Form)
		Dim iResult As Integer
		Dim lStyle As Integer
		
		Exit Sub
		
		Dim hWnd As Integer = frm.Handle.ToInt32()
		If frm.FormBorderStyle = FormBorderStyle.FixedDialog Then
			frm.BackColor = ColorTranslator.FromOle(BUTTON_FACE)
			lStyle = GetWindowLong(hWnd, GWL_STYLE)
			lStyle = lStyle Or DS_MODALFRAME
			lStyle = SetWindowLong(hWnd, GWL_STYLE, lStyle)
			iResult = Ctl3dSubclassDlgEx(CShort(hWnd), 0)
		End If
		
	End Sub
	
	Sub Ctl3DInit(ByRef hWnd As Integer)
		Dim hInst, iResult As Integer
		
		Exit Sub
		
		Try 
			
			hInst = GetWindowWord(hWnd, GWW_HINSTANCE)
			'TODO: Function/Sub GETWINDOWWORD% parameters should be checked against the calling convention on the next line.
			'TODO: -> Unknown WinAPI/Third-Party DLL Call -> GETWINDOWWORD%  Check Calling convention for compatibility with BSTR/UNICODE/ANSI Strings. See VB4DLL.TXT in VB directory.
			iResult = Ctl3dRegister(CShort(hInst))
			iResult = Ctl3dAutoSubclass(CShort(hInst))
		
		Catch 
		End Try
		
		
		
		'    If UCase(Right(VERSIONNUMBER, 1)) = "A" Then
		'        MsgBox "Inst:" & hInst% & "  RC:" & iResult%, , "Load CTL3DV2"
		'    End If
		
		Exit Sub
		
	End Sub
	
	Function GetControlFileVar(ByRef sSection As String, ByRef sVar As String, ByRef sReturn As String, ByRef sControlFile As String) As Integer
		
		Dim sTmp As String = ""
		Dim iDataLen As Integer
		
		Try 
			
			sTmp = New String(" "c, 129)
			Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi(sVar)
			Try 
				iDataLen = GetPrivateProfileString(sSection, tmpPtr, "", sTmp, 128, sControlFile)
				sVar = Marshal.PtrToStringAnsi(tmpPtr)
			Finally 
				Marshal.FreeHGlobal(tmpPtr)
			End Try
			sTmp = sTmp.Substring(0, iDataLen)
			
			
			If iDataLen > 0 Then
				sReturn = sTmp.Trim()
				Return PM_TRUE
			Else
				sReturn = "Error"
				Return PM_FALSE
			End If
		
		Catch 
		End Try
		
		
		
		sReturn = "Error"
		Return PM_FALSE
		
	End Function
	
	Function GetIniFileVar(ByRef sSection As String, ByRef sVar As String, ByRef sReturn As String, ByRef iFireForm As Integer) As Integer
		Dim sTmp As String = ""
		Dim iDataLen As Integer
		
		Try 
			
			sTmp = New String(" "c, 129)
			Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi(sVar)
			Try 
				iDataLen = GetPrivateProfileString(sSection, tmpPtr, "", sTmp, 128, INI_FILENAME)
				sVar = Marshal.PtrToStringAnsi(tmpPtr)
			Finally 
				Marshal.FreeHGlobal(tmpPtr)
			End Try
			sTmp = sTmp.Substring(0, iDataLen)
			
			
			If iDataLen > 0 Then
				sReturn = sTmp.Trim()
				Return True
			Else
				If iFireForm Then
					Interaction.MsgBox("Startup Settings for this application are missing" & Strings.Chr(10).ToString() & "Complete the following screen and save the settings", MB_ICONEXCLAMATION)
					frmAppSettings.ShowDialog()
					frmAppSettings.Close()
					
					sTmp = New String(" "c, 129)
					Dim tmpPtr2 As IntPtr = Marshal.StringToHGlobalAnsi(sVar)
					Try 
						iDataLen = GetPrivateProfileString(sSection, tmpPtr2, "", sTmp, 128, INI_FILENAME)
						sVar = Marshal.PtrToStringAnsi(tmpPtr2)
					Finally 
						Marshal.FreeHGlobal(tmpPtr2)
					End Try
					sTmp = sTmp.Substring(0, iDataLen)
					
					If iDataLen > 0 Then
						sReturn = sTmp.Trim()
						Return True
					Else
						sReturn = "Error"
						Return False
					End If
				Else
					sReturn = "Error"
					Return False
				End If
			End If
		
		Catch 
		End Try
		
		
		
		sReturn = "Error"
		Return False
		
	End Function
	
	Function PutControlFileVar(ByRef sSection As String, ByRef sVar As String, ByRef sString As String, ByRef sControlFile As String) As Integer
		
		Dim result As Integer = 0
		Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi(sVar)
		Dim tmpPtr2 As IntPtr = Marshal.StringToHGlobalAnsi(sString)
		Try 
			If Not WritePrivateProfileString(sSection, tmpPtr, tmpPtr2, sControlFile) Then
				sString = Marshal.PtrToStringAnsi(tmpPtr2)
				sVar = Marshal.PtrToStringAnsi(tmpPtr)
				result = PM_FALSE
			Else
				sString = Marshal.PtrToStringAnsi(tmpPtr2)
				sVar = Marshal.PtrToStringAnsi(tmpPtr)
				result = PM_TRUE
			End If
		Finally 
			Marshal.FreeHGlobal(tmpPtr)
			Marshal.FreeHGlobal(tmpPtr2)
		End Try
		
		Return result
	End Function
	
	Function PutIniFileVar(ByRef sSection As String, ByRef sVar As String, ByRef sString As String) As Integer
		
		Dim result As Integer = 0
		Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi(sVar)
		Dim tmpPtr2 As IntPtr = Marshal.StringToHGlobalAnsi(sString)
		Try 
			If Not WritePrivateProfileString(sSection, tmpPtr, tmpPtr2, INI_FILENAME) Then
				sString = Marshal.PtrToStringAnsi(tmpPtr2)
				sVar = Marshal.PtrToStringAnsi(tmpPtr)
				result = PM_FALSE
			Else
				sString = Marshal.PtrToStringAnsi(tmpPtr2)
				sVar = Marshal.PtrToStringAnsi(tmpPtr)
				result = PM_TRUE
			End If
		Finally 
			Marshal.FreeHGlobal(tmpPtr)
			Marshal.FreeHGlobal(tmpPtr2)
		End Try
		
		Return result
	End Function
End Module