Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Runtime.InteropServices
Module mShellExecute
	
	Private Declare Function ShellExecute Lib "shell32.dll"  Alias "ShellExecuteA"(ByVal hWnd As Integer, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As Integer) As Integer
	Private Declare Function ShellExecuteForExplore Lib "shell32.dll"  Alias "ShellExecuteA"(ByVal hWnd As Integer, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As Integer, ByVal lpDirectory As Integer, ByVal nShowCmd As Integer) As Integer
	Public Enum EShellShowConstants
		essSW_HIDE = 0
		essSW_MAXIMIZE = 3
		essSW_MINIMIZE = 6
		essSW_SHOWMAXIMIZED = 3
		essSW_SHOWMINIMIZED = 2
		essSW_SHOWNORMAL = 1
		essSW_SHOWNOACTIVATE = 4
		essSW_SHOWNA = 8
		essSW_SHOWMINNOACTIVE = 7
		essSW_SHOWDEFAULT = 10
		essSW_RESTORE = 9
		essSW_SHOW = 5
	End Enum
	Private Const ERROR_FILE_NOT_FOUND As Integer = 2
	Private Const ERROR_PATH_NOT_FOUND As Integer = 3
	Private Const ERROR_BAD_FORMAT As Integer = 11
	Private Const SE_ERR_ACCESSDENIED As Integer = 5 '  access denied
	Private Const SE_ERR_ASSOCINCOMPLETE As Integer = 27
	Private Const SE_ERR_DDEBUSY As Integer = 30
	Private Const SE_ERR_DDEFAIL As Integer = 29
	Private Const SE_ERR_DDETIMEOUT As Integer = 28
	Private Const SE_ERR_DLLNOTFOUND As Integer = 32
	Private Const SE_ERR_FNF As Integer = 2 '  file not found
	Private Const SE_ERR_NOASSOC As Integer = 31
	Private Const SE_ERR_PNF As Integer = 3 '  path not found
	Private Const SE_ERR_OOM As Integer = 8 '  out of memory
	Private Const SE_ERR_SHARE As Integer = 26
	
	Public Function ShellEx(ByVal sFile As String, Optional ByVal eShowCmd As EShellShowConstants = EShellShowConstants.essSW_SHOWDEFAULT, Optional ByVal sParameters As String = "", Optional ByVal sDefaultDir As String = "", Optional ByRef sOperation As String = "open", Optional ByRef Owner As Integer = 0) As Boolean
		Dim lR, lErr, sErr As Integer
		If sFile.ToUpper().IndexOf(".EXE") >= 0 Then
			eShowCmd = EShellShowConstants.essSW_HIDE
		End If

		Try 
			If (sParameters = "") And (sDefaultDir = "") Then
				Dim handle As GCHandle = GCHandle.Alloc(0, GCHandleType.Pinned)
				Dim handle2 As GCHandle = GCHandle.Alloc(0, GCHandleType.Pinned)
				Try 
					Dim tmpPtr2 As IntPtr = handle2.AddrOfPinnedObject()
					Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()
					lR = ShellExecuteForExplore(Owner, sOperation, sFile, tmpPtr, tmpPtr2, EShellShowConstants.essSW_SHOWNORMAL)
				Finally 
					handle.Free()
					handle2.Free()
				End Try
			Else
				lR = ShellExecute(Owner, sOperation, sFile, sParameters, sDefaultDir, eShowCmd)
			End If
			If (lR < 0) Or (lR > 32) Then
				Return True
			Else
				' raise an appropriate error:
				lErr = Constants.vbObjectError + 1048 + lR
				Select Case lR
					Case 0, SE_ERR_OOM
						lErr = 7 : sErr = CInt("Out of memory")
					Case ERROR_FILE_NOT_FOUND, SE_ERR_FNF
						lErr = 53 : sErr = CInt("File not found")
					Case ERROR_PATH_NOT_FOUND, SE_ERR_PNF
						lErr = 76 : sErr = CInt("Path not found")
					Case ERROR_BAD_FORMAT
						sErr = CInt("The executable file is invalid or corrupt")
					Case SE_ERR_ACCESSDENIED
						lErr = 75 : sErr = CInt("Path/file access error")
					Case SE_ERR_ASSOCINCOMPLETE
						sErr = CInt("This file type does not have a valid file association.")
					Case SE_ERR_DDEBUSY
						lErr = 285 : sErr = CInt("The file could not be opened because the target application is busy.  Please try again in a moment.")
					Case SE_ERR_DDEFAIL
						lErr = 285 : sErr = CInt("The file could not be opened because the DDE transaction failed.  Please try again in a moment.")
					Case SE_ERR_DDETIMEOUT
						lErr = 286 : sErr = CInt("The file could not be opened due to time out.  Please try again in a moment.")
					Case SE_ERR_DLLNOTFOUND
						lErr = 48 : sErr = CInt("The specified dynamic-link library was not found.")
					Case SE_ERR_NOASSOC
						sErr = CInt("No application is associated with this file type.")
					Case SE_ERR_SHARE
						lErr = 75 : sErr = CInt("A sharing violation occurred.")
					Case Else
						sErr = CInt("An error occurred occurred whilst trying to open or print the selected file.")
				End Select
				

				Throw New System.Exception(lErr.ToString() + ", " +   + ", " + My.Application.Info.AssemblyName & ".GShell" + ", " + sErr)
				Return False
			End If
		
		Catch exc As System.Exception
         
		End Try
		
	End Function
End Module