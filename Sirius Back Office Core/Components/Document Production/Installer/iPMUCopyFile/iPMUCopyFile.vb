Option Strict Off
Option Explicit On
Module MainModule
	'*******************************************************************************
	' Module:   Copies a file to the Word\STARTUP directory of each profile
	' Shared:   No
	' Needs:    None
	'*******************************************************************************
	'
	' Conditional compiler constant for testing
	'
#Const kbTesting = False
	'
	' Constants for the file to copy from
	'
	Private Const ksFileName As String = "Sirius.dot"
	'
	' Constant required for the GetVersionEx API function
	'
	Private Const VER_PLATFORM_WIN32_NT As Short = 2
	'
	' Type required for the GetVersionEx API function
	'
	Private Structure OSVERSIONINFO
		Dim dwOSVersionInfoSize As Integer
		Dim dwMajorVersion As Integer
		Dim dwMinorVersion As Integer
		Dim dwBuildNumber As Integer
		Dim dwPlatformId As Integer
		'UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
		<VBFixedString(128),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray,SizeConst:=128)> Public szCSDVersion() As Char '  Maintenance string for PSS usage
	End Structure
	'
	' API function to return the version of Windows in use
	'
	'UPGRADE_WARNING: Structure OSVERSIONINFO may require marshalling attributes to be passed as an argument in this Declare statement. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"'
	Private Declare Function GetVersionEx Lib "kernel32"  Alias "GetVersionExA"(ByRef lpVersionInformation As OSVERSIONINFO) As Integer
	
	
	'***************************************************************** '
	' Name: GetAppPath
	'
	' Description: This subroutine returns the directory where the
	' application is running.
	'***************************************************************** '
	Private Function GetAppPath() As String
		
        GetAppPath = My.Application.Info.DirectoryPath
        'GetAppPath = System.Reflection.Assembly.GetExecutingAssembly.Location
		If Right(GetAppPath, 1) <> "\" Then
			GetAppPath = GetAppPath & "\"
		End If
		
	End Function
	
	'***************************************************************** '
	' Name: Main
	'
	' Description: This routine controls the main flow of the program.
	' A specified file is copied to the Word\STARTUP directory of each
	' User Profile found in Windows.
	'***************************************************************** '
	Public Sub Main()
		
        'On Error GoTo Err_Main
		
		Dim asUserDir() As String
		Dim sDir As String
		Dim sDirToSearch As String
		Dim sDirToCopyTo As String
		Dim sFileToCopy As String
		Dim nDirsCount As Short
		Dim iDir As Short
		Dim lpVersionInfo As OSVERSIONINFO
		Dim bReturn As Short
		' PW180203
		Dim iAttribute As Short
       
        Try
            ' Check if the file to copy from is available
            sFileToCopy = GetAppPath() & ksFileName
            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            If Dir(sFileToCopy) = "" Then
                MsgBox(sFileToCopy & " could not be found.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical)
                Exit Sub
            End If

            ' Find out version of windows being used
            lpVersionInfo.dwOSVersionInfoSize = 148
            bReturn = GetVersionEx(lpVersionInfo)

            If bReturn = 0 Then
                MsgBox("Unable to retrieve the version of Windows being used.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical)
                Exit Sub
            End If

            ' Set up the directories accordingly

            sDirToCopyTo = "\Application Data\Microsoft\Word\STARTUP\"

            If lpVersionInfo.dwMajorVersion = 5 And lpVersionInfo.dwMinorVersion = 0 And lpVersionInfo.dwPlatformId = VER_PLATFORM_WIN32_NT Then
                'Set up directories for Win2K
                sDirToSearch = "c:\Documents and Settings\"
            ElseIf lpVersionInfo.dwMajorVersion = 6 And lpVersionInfo.dwPlatformId = VER_PLATFORM_WIN32_NT Then
                sDirToSearch = "c:\users\"
                sDirToCopyTo = "\AppData\Roaming\Microsoft\Word\STARTUP\"
            Else
                'If not Win2K, assume NT
                sDirToSearch = "c:\WINNT\Profiles\"
            End If

            ' Check for available profile directories
            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            sDir = Dir(sDirToSearch, FileAttribute.Directory + FileAttribute.ReadOnly + FileAttribute.Hidden + FileAttribute.System)

            ' Store profile directories in an array
            Do While sDir <> ""
                ' make sure sdir is a valid directory.
                If sDir <> "." And sDir <> ".." Then
                    ' PW180203 - trap error 52 to ignore bad directory names
                    'Err.Clear()
                    'On Error Resume Next
                    iAttribute = GetAttr(sDirToSearch & sDir)
                    Select Case Err.Number
                        Case 0 ' No error, so add directory to array
                            ' On Error GoTo 0
                            If iAttribute = FileAttribute.Directory Then
                                nDirsCount = nDirsCount + 1
                                'UPGRADE_WARNING: Lower bound of array asUserDir was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
                                ReDim Preserve asUserDir(nDirsCount)
                                asUserDir(nDirsCount) = sDir
                            End If
                        Case 52 ' Bad file name - do nothing
                        Case Else ' Some other error so report it
                            'GoTo Err_Main
                    End Select
                    'On Error GoTo 0
                End If
                'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                sDir = Dir()
            Loop

            ' If none exist, inform user
            If nDirsCount = 0 Then
                MsgBox("No user profile directories found.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information)
                Exit Sub
            End If

            ' Process each of the stored profile directories
            For iDir = 1 To nDirsCount
                ' Create the target directory if it does not exist
                sDir = sDirToSearch & asUserDir(iDir) & sDirToCopyTo
                'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                If Dir(sDir, FileAttribute.Directory + FileAttribute.ReadOnly + FileAttribute.Hidden + FileAttribute.System) = "" Then
                    MakeDirPath(sDir)
                End If
                Try
                    ' Copy the file to the target directory
                    FileCopy(sFileToCopy, sDir & ksFileName)
                Catch
                End Try

            Next

            Exit Sub

            ' Error Handler
            'Err_Main:
        Catch
            MsgBox("An error has occurred:" & vbCrLf & Err.Number & " - " & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical)
            Exit Sub
        End Try
    End Sub
	
	'***************************************************************** '
	' Name: MakeDirPath
	'
	' Description: This subroutine checks if each directory in the
	' passed directory path exists. If a directory does not exist, it
	' is created.
	'***************************************************************** '
	Private Sub MakeDirPath(ByVal v_sDirPath As String)
		
		
		Dim iChar As Short
		Dim sBuildDirPath As String
		
		' Loop through the directory string
		For iChar = 1 To Len(v_sDirPath)
			' If the next sub directory has been reached, create it if applicable
			If Mid(v_sDirPath, iChar, 1) = "\" Then
				If sBuildDirPath <> "" Then
					'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					If Dir(sBuildDirPath, FileAttribute.Directory + FileAttribute.ReadOnly + FileAttribute.Hidden + FileAttribute.System) = "" Then
						MkDir(sBuildDirPath)
					End If
				End If
			End If
			' Keep building the directory path
			sBuildDirPath = sBuildDirPath & Mid(v_sDirPath, iChar, 1)
		Next 
		
		' Error Handler
		Exit Sub
		
	End Sub
End Module
