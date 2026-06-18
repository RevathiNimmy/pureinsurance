Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
<System.Runtime.InteropServices.ProgId("PMCopyFile_NET.PMCopyFile")> _
 Public Module PMCopyFile
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' $Author: Tom.brown $
	' $Revision: 2 $
	' $Modtime: 15/11/02 15:36 $
	' $Workfile: PMCopyFile.bas $
	' $Logfile: /Sirius Back Office Core/Components/Document Production/Document Template/Interface/iPMBDocTemplate/PMCopyFile.bas $
	' $History: PMCopyFile.bas $
	'
	' *****************  Version 2  *****************
	' User: Tom.brown    Date: 20/11/02   Time: 9:37
	' Updated in $/Sirius Back Office Core/Components/Document Production/Document Template/Interface/iPMBDocTemplate
	' Parallel Maintenance 1.6->1.8:  Replaced all FileCopy statements with
	' function call to PMFileCopy and test for /display error message
	'
	' *****************  Version 1  *****************
	' User: Tom.brown    Date: 23/10/02   Time: 11:16
	' Created in $/Sirius For Underwriting/Document Production/Document Manager/Interface/iPMBDocManager
	' New function to replace FileCopy and provide more useful error messages
	
	Private Const ACClass As String = "PMCopyFile"
	
	' Use API's to determine the cause of a file copy error
	Public Declare Function PathFileExists Lib "shlwapi"  Alias "PathFileExistsA"(ByVal pszPath As String) As Integer
	
	Declare Function PathIsUNCServerShare Lib "shlwapi"  Alias "PathIsUNCServerShareA"(ByVal pszPath As String) As Integer
	
	Declare Function PathIsUNCServer Lib "shlwapi"  Alias "PathIsUNCServerA"(ByVal pszPath As String) As Integer
	
	' ***************************************************************** '
	' Name: PMFileCopy
	'
	' Description: To provide checks on source & target
	'              replacing VB FileCopy command
	'
	' ***************************************************************** '
	Public Function PMFileCopy(ByVal v_sSourceFile As String, ByVal v_sTarget As String, ByRef r_sMessage As String) As Integer
		
		Dim result As Integer = 0
		Try 
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Dim iLen As Integer
            Dim sTarget As String = ""
            Dim sTargetFile As String = ""

			' TB 151102 Deal with null input
			If v_sSourceFile = "" Then
				r_sMessage = "Source file name is null"
				Return gPMConstants.PMEReturnCode.PMError
			End If
			If v_sTarget = "" Then
				r_sMessage = "Target file name is null"
				Return gPMConstants.PMEReturnCode.PMError
			End If
			
			If PathFileExists(v_sSourceFile) <> 1 Then
				r_sMessage = "Source file : " & v_sSourceFile & " does not exist."
				' Check for network path
				For idx As Integer = v_sSourceFile.Length To 1 Step -1
					If Mid(v_sSourceFile, idx, 1) = "\" Then
						If PathFileExists(v_sSourceFile.Substring(0, idx)) <> 1 Then
							If PathIsUNCServerShare(v_sSourceFile.Substring(0, idx - 1)) = 1 Then
								' Its a valid network share, but we can't read the file
								r_sMessage = r_sMessage & Strings.Chr(13) & Strings.Chr(10) & "Or Access to network share: " & v_sSourceFile.Substring(0, idx) & " denied"
								Return gPMConstants.PMEReturnCode.PMMNoAccess
							Else
								' it may still be the server root
								If PathIsUNCServer(v_sSourceFile.Substring(0, idx - 1)) = 1 Then
									r_sMessage = r_sMessage & Strings.Chr(13) & Strings.Chr(10) & "No Directory supplied in Server path"
								Else
									' directory does not exist
									r_sMessage = r_sMessage & Strings.Chr(13) & Strings.Chr(10) & "Source directory: " & v_sSourceFile.Substring(0, idx) & " does not exist"
								End If
								Return gPMConstants.PMEReturnCode.PMNotFound
							End If
						End If
						Exit For
					End If
				Next idx
				' Directory exists and is accessible, so file not found or unreadable
				Return gPMConstants.PMEReturnCode.PMNotFound
			End If
			
			If PathFileExists(v_sTarget) = 1 Then
				If (FileSystem.GetAttr(v_sTarget) And FileAttribute.Directory) <> FileAttribute.Directory Then
					sTarget = v_sTarget
				Else
					' Path exists (must be a directory)
					If Not v_sTarget.EndsWith("\") Then
						sTarget = v_sTarget & "\"
					Else
						sTarget = v_sTarget
					End If
					' Check for write permissions on directory
					If (FileSystem.GetAttr(sTarget) And FileAttribute.ReadOnly) = FileAttribute.ReadOnly Then
						r_sMessage = ("Target Directory: " & sTarget & " is Read Only")
						Return gPMConstants.PMEReturnCode.PMMNoAccess
					End If
					iLen = v_sSourceFile.Length
					For idx As Integer = iLen To 1 Step -1
						If Mid(v_sSourceFile, idx, 1) = "\" Then
							sTarget = sTarget & v_sSourceFile.Substring(v_sSourceFile.Length - (iLen - idx))
							Exit For
						End If
					Next idx
				End If
				sTargetFile = sTarget
			Else
				' strip filename from right back to first "\"
				For idx As Integer = v_sTarget.Length To 1 Step -1
					If Mid(v_sTarget, idx, 1) = "\" Then
						If PathFileExists(v_sTarget.Substring(0, idx)) <> 1 Then
							' directory does not exist
							r_sMessage = ("Target directory: " & v_sTarget.Substring(0, idx) & " does not exist")
							If PathIsUNCServerShare(v_sTarget.Substring(0, idx - 1)) = 1 Then
								r_sMessage = r_sMessage & Strings.Chr(13) & Strings.Chr(10) & "              Or Access to network share denied"
								Return gPMConstants.PMEReturnCode.PMMNoAccess
							Else
								' it may still be the server root
								If PathIsUNCServer(v_sTarget.Substring(0, idx - 1)) = 1 Then
									r_sMessage = r_sMessage & Strings.Chr(13) & Strings.Chr(10) & "No Directory supplied in Server path"
								Else
									' directory does not exist
									r_sMessage = r_sMessage & Strings.Chr(13) & Strings.Chr(10) & "Source directory: " & v_sTarget.Substring(0, idx) & " does not exist"
								End If
								Return gPMConstants.PMEReturnCode.PMNotFound
							End If
						End If
						' Target is a directory
						sTarget = v_sTarget.Substring(0, idx)
						Exit For
					End If
				Next idx
				sTargetFile = v_sTarget
			End If
			
			
			' Check for write permissions on target
			If (FileSystem.GetAttr(sTarget) And FileAttribute.ReadOnly) = FileAttribute.ReadOnly Then
				r_sMessage = ("Target: " & sTarget & " is Read Only")
				Return gPMConstants.PMEReturnCode.PMMNoAccess
			End If
			
			
			' Got this far:  Do the filecopy
			
			File.Copy(v_sSourceFile, sTargetFile)
			
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			r_sMessage = "Failed to copy " & v_sSourceFile & " to " & v_sTarget
			result = gPMConstants.PMEReturnCode.PMError
			iPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="PMFileCopy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result


			Return result
		End Try
	End Function
	
	
	' End of $Workfile: PMCopyFile.bas $
    'Modified by Deepak Sharma on 4/20/2010 4:42:07 PM refer developer guide no. 29(No Solutions)
    'Shared Sub New()
    '	MainModule.JustForInvokeMain()

End Module