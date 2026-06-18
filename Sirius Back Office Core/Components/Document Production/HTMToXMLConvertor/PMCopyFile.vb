Option Strict Off
Option Explicit On
Imports SharedFiles
Module PMCopyFile
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
		
		Try
		PMFileCopy = gPMConstants.PMEReturnCode.PMTrue
		
		Dim idx As Short
		Dim iLen As Short
		Dim sTarget As String
		Dim sTargetFile As String
		
		Dim iResult As Integer
		
		' TB 151102 Deal with null input
		If v_sSourceFile = "" Then
			r_sMessage = "Source file name is null"
			PMFileCopy = gPMConstants.PMEReturnCode.PMError
			Exit Function
		End If
		If v_sTarget = "" Then
			r_sMessage = "Target file name is null"
			PMFileCopy = gPMConstants.PMEReturnCode.PMError
			Exit Function
		End If
		
		If PathFileExists(v_sSourceFile) <> 1 Then
			r_sMessage = "Source file : " & v_sSourceFile & " does not exist."
			' Check for network path
			For idx = Len(v_sSourceFile) To 1 Step -1
				If Mid(v_sSourceFile, idx, 1) = "\" Then
					If PathFileExists(Left(v_sSourceFile, idx)) <> 1 Then
						If PathIsUNCServerShare(Left(v_sSourceFile, idx - 1)) = 1 Then
							' Its a valid network share, but we can't read the file
							r_sMessage = r_sMessage & vbCrLf & "Or Access to network share: " & Left(v_sSourceFile, idx) & " denied"
							PMFileCopy = gPMConstants.PMEReturnCode.PMMNoAccess
						Else
							' it may still be the server root
							If PathIsUNCServer(Left(v_sSourceFile, idx - 1)) = 1 Then
								r_sMessage = r_sMessage & vbCrLf & "No Directory supplied in Server path"
							Else
								' directory does not exist
								r_sMessage = r_sMessage & vbCrLf & "Source directory: " & Left(v_sSourceFile, idx) & " does not exist"
							End If
							PMFileCopy = gPMConstants.PMEReturnCode.PMNotFound
						End If
						Exit Function
					End If
					Exit For
				End If
			Next idx
			' Directory exists and is accessible, so file not found or unreadable
			PMFileCopy = gPMConstants.PMEReturnCode.PMNotFound
			Exit Function
		End If
		
		If PathFileExists(v_sTarget) = 1 Then
			If (GetAttr(v_sTarget) And FileAttribute.Directory) <> FileAttribute.Directory Then
				sTarget = v_sTarget
			Else ' Path exists (must be a directory)
				If Right(v_sTarget, 1) <> "\" Then
					sTarget = v_sTarget & "\"
				Else
					sTarget = v_sTarget
				End If
				' Check for write permissions on directory
				If (GetAttr(sTarget) And FileAttribute.ReadOnly) = FileAttribute.ReadOnly Then
					r_sMessage = ("Target Directory: " & sTarget & " is Read Only")
					PMFileCopy = gPMConstants.PMEReturnCode.PMMNoAccess
					Exit Function
				End If
				iLen = Len(v_sSourceFile)
				For idx = iLen To 1 Step -1
					If Mid(v_sSourceFile, idx, 1) = "\" Then
						sTarget = sTarget & Right(v_sSourceFile, iLen - idx)
						Exit For
					End If
				Next idx
			End If
			sTargetFile = sTarget
		Else
			' strip filename from right back to first "\"
			For idx = Len(v_sTarget) To 1 Step -1
				If Mid(v_sTarget, idx, 1) = "\" Then
					If PathFileExists(Left(v_sTarget, idx)) <> 1 Then
						' directory does not exist
						r_sMessage = ("Target directory: " & Left(v_sTarget, idx) & " does not exist")
						If PathIsUNCServerShare(Left(v_sTarget, idx - 1)) = 1 Then
							r_sMessage = r_sMessage & vbCrLf & "              Or Access to network share denied"
							PMFileCopy = gPMConstants.PMEReturnCode.PMMNoAccess
						Else
							' it may still be the server root
							If PathIsUNCServer(Left(v_sTarget, idx - 1)) = 1 Then
								r_sMessage = r_sMessage & vbCrLf & "No Directory supplied in Server path"
							Else
								' directory does not exist
								r_sMessage = r_sMessage & vbCrLf & "Source directory: " & Left(v_sTarget, idx) & " does not exist"
							End If
							PMFileCopy = gPMConstants.PMEReturnCode.PMNotFound
						End If
						Exit Function
					End If
					' Target is a directory
					sTarget = Left(v_sTarget, idx)
					Exit For
				End If
			Next idx
			sTargetFile = v_sTarget
		End If
		
		
		' Check for write permissions on target
		If (GetAttr(sTarget) And FileAttribute.ReadOnly) = FileAttribute.ReadOnly Then
			r_sMessage = ("Target: " & sTarget & " is Read Only")
			PMFileCopy = gPMConstants.PMEReturnCode.PMMNoAccess
			Exit Function
		End If
		
		
		' Got this far:  Do the filecopy
		
		FileCopy(v_sSourceFile, sTargetFile)
		
		
		
		Exit Function
		Catch ex As Exception
		
		r_sMessage = "Failed to copy " & v_sSourceFile & " to " & v_sTarget
		PMFileCopy = gPMConstants.PMEReturnCode.PMError
        iPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="PMFileCopy", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
		
	
		End Try
	End Function
	
	
	' End of $Workfile: PMCopyFile.bas $
End Module
