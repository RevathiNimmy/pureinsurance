Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports SharedFiles

Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  03/12/97
	'
	' Description: Main Module.
	'
	' Edit History:
	'
	'   MS 01/08/00
	'
	'   GetDiskSpace - New function added to calculate disk space
	'
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bDOCManager"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Username.
	Public g_sUsername As String = ""
	
	' Password.
	Public g_sPassword As New FixedLengthString(30)
	
	' User ID
	Public g_iUserID As Integer
	
	' Calling Application
	Public g_sCallingAppName As String = ""
	' Source ID
	Public g_iSourceID As Integer
	' Language ID
	Public g_iLanguageID As Integer
	' Currency ID
	Public g_iCurrencyID As Integer
	' LogLevel
	Public g_iLogLevel As Integer
	' AccessLevel
	Public g_iAccessLevel As Integer
	' AdminLevel
	Public g_iAdminLevel As Integer
	' Home Folder Number
	Public g_lHomeFolder As Integer
	
	'ND 081100 - Delete and move levels
	Public g_iFileDeleteLevel As Integer
	Public g_iFolderDeleteLevel As Integer
	Public g_iFileMoveLevel As Integer
	Public g_iFolderMoveLevel As Integer
	Public g_iFileCopyLevel As Integer
	Public g_iFolderCopyLevel As Integer
	
	'Check amount of free space
	Declare Function GetDiskFreeSpace Lib "kernel32"  Alias "GetDiskFreeSpaceA"(ByVal lpRootPathName As String, ByRef lpSectorsPerCluster As Integer, ByRef lpBytesPerSector As Integer, ByRef lpNumberOfFreeClusters As Integer, ByRef lpTotalNumberOfClusters As Integer) As Integer
	
	
	Public Function GetDiskSpace(ByRef sDriveLetter As String, ByRef dFreeDiskSpace As Double) As Integer
		
		Dim result As Integer = 0
		Dim lReturn, SxC, BxS, NOFC As Integer
		Dim dNOFC As Double
		Dim TNOC As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Select Case sDriveLetter.Length
				Case Is < 1
					sDriveLetter = "c"
				Case Is > 1
					sDriveLetter = sDriveLetter.Substring(0, 1)
				Case Else
					'We are ok.
			End Select
			
			lReturn = GetDiskFreeSpace(sDriveLetter & ":\", SxC, BxS, NOFC, TNOC)
			
			If Not lReturn Then
				Throw New Exception()
				Return result
			End If
			
			dNOFC = NOFC
			dFreeDiskSpace = BxS * SxC * dNOFC
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDiskSpace Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDiskSpace", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Module