Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Runtime.InteropServices
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 29/01/1999
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMClientInstallAdmin"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	' Menus
	
	
	' {* USER DEFINED CODE (End) *}
	Declare Function GetPrivateProfileString Lib "kernel32"  Alias "GetPrivateProfileStringA"(ByVal lpApplicationName As String, ByVal lpKeyName As Integer, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	Public Const ACArrayProductIDCol As Integer = 0
	Public Const ACArrayProductCodeCol As Integer = 1
	Public Const ACArrayProductCaptionCol As Integer = 2
	Public Const ACArrayServerVersionCol As Integer = 3
	Public Const ACArrayServerDateCol As Integer = 4
	Public Const ACArrayClientVersionCol As Integer = 5
	Public Const ACArrayClientDateCol As Integer = 6
	Public Const ACArrayMandatoryCol As Integer = 7
	Public Const ACArrayAutoInstallableCol As Integer = 8
	Public Const ACArrayInstallPathCol As Integer = 9
	Public Const ACArrayInstallProgramCol As Integer = 10
	Public Const ACArrayDescriptionCol As Integer = 11
	Public Const ACArrayRebootLevelCol As Integer = 12
	
	Public Const ACListProductCaptionCol As Integer = 0
	Public Const ACListClientVersionCol As Integer = 1
	Public Const ACListDescriptionCol As Integer = 2
	Public Const ACListMandatoryCol As Integer = 3
	Public Const ACListAutoInstallableCol As Integer = 4
	Public Const ACListServerVersionCol As Integer = 5
	
	Public Const ACMainKey As String = "Main"
	Public Const ACServerKey As String = "Server"
	Public Const ACClientKey As String = "Client"
	Public Const ACProductCodeIndex As String = "Product Code"
	Public Const ACRequiredVersionIndex As String = "Required Version"
	Public Const ACSoftwareDateIndex As String = "Software Date"
	Public Const ACLatestVersionIndex As String = "Latest Version"
	Public Const ACMandatoryIndex As String = "Mandatory"
	Public Const ACAutoInstallableIndex As String = "Auto Installable"
	Public Const ACInstallProgramIndex As String = "Install Program"
	Public Const ACDescriptionIndex As String = "Description"
	Public Const ACRebootLevelIndex As String = "Reboot Level"
	
    Sub Main()
        Dim objInterface As New Interface_Renamed
        objInterface.Initialise()
        objInterface.Start()
    End Sub
	
	' ***************************************************************** '
	' Name: GetDetailsFromINI
	'
	' Description: Get the details from the supplied INI file.
	'
	'
	' ***************************************************************** '
	Public Function GetDetailsFromINI(ByVal v_sINIFile As String, ByRef r_sProductCode As String, ByRef r_sRequiredServerVersion As String, ByRef r_dtServerSoftwareDate As Date, ByRef r_sLatestClientVersion As String, ByRef r_dtClientSoftwareDate As Date, ByRef r_iIsMandatory As Integer, ByRef r_iIsAutoInstallable As Integer, ByRef r_sPath As String, ByRef r_sInstallProgram As String, ByRef r_sDescription As String, ByRef r_iRebootLevel As Integer, ByRef r_sFieldInError As String, ByRef r_sErrorValue As String) As Integer
		
		Dim result As Integer = 0
		Dim sDefault, sKey, sIndex As String
		Dim lNoOfChars As Integer
		Dim sString As String = ""
		
		Dim iCharPos, iFoundAt As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			iCharPos = 1
			Do 
				iFoundAt = Strings.InStr(iCharPos, v_sINIFile, "\", CompareMethod.Text)
				If iFoundAt = 0 Then
					Exit Do
				Else
					iCharPos = iFoundAt + 1
				End If
			Loop 
			
			r_sPath = v_sINIFile.Substring(0, iCharPos - 1)
			
			' Initialise the Error fields
			r_sFieldInError = ""
			r_sErrorValue = ""
			
			' Product Code
			sDefault = ""
			sString = New String(" ", 128)
			sKey = ACMainKey
			sIndex = ACProductCodeIndex
			Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi(sIndex)
			Try 
				lNoOfChars = GetPrivateProfileString(sKey, tmpPtr, sDefault, sString, 128, v_sINIFile)
				sIndex = Marshal.PtrToStringAnsi(tmpPtr)
			Finally 
				Marshal.FreeHGlobal(tmpPtr)
			End Try
			
			r_sProductCode = sString.Substring(0, lNoOfChars)
			If r_sProductCode.Trim() = "" Then
				r_sFieldInError = ACProductCodeIndex
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Required Server Version
			sDefault = ""
			sString = New String(" ", 128)
			sKey = ACServerKey
			sIndex = ACRequiredVersionIndex
			Dim tmpPtr2 As IntPtr = Marshal.StringToHGlobalAnsi(sIndex)
			Try 
				lNoOfChars = GetPrivateProfileString(sKey, tmpPtr2, sDefault, sString, 128, v_sINIFile)
				sIndex = Marshal.PtrToStringAnsi(tmpPtr2)
			Finally 
				Marshal.FreeHGlobal(tmpPtr2)
			End Try
			
			r_sRequiredServerVersion = sString.Substring(0, lNoOfChars)
			If r_sRequiredServerVersion.Trim() = "" Then
				r_sFieldInError = ACRequiredVersionIndex
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Server Software Date
			sDefault = ""
			sString = New String(" ", 128)
			sKey = ACServerKey
			sIndex = ACSoftwareDateIndex
			Dim tmpPtr3 As IntPtr = Marshal.StringToHGlobalAnsi(sIndex)
			Try 
				lNoOfChars = GetPrivateProfileString(sKey, tmpPtr3, sDefault, sString, 128, v_sINIFile)
				sIndex = Marshal.PtrToStringAnsi(tmpPtr3)
			Finally 
				Marshal.FreeHGlobal(tmpPtr3)
			End Try
			
			sString = sString.Substring(0, lNoOfChars)
			If Information.IsDate(sString) Then
				r_dtServerSoftwareDate = CDate(sString)
			Else
				r_sFieldInError = ACSoftwareDateIndex
				r_sErrorValue = sString
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Latest Client Version
			sDefault = ""
			sString = New String(" ", 128)
			sKey = ACClientKey
			sIndex = ACLatestVersionIndex
			Dim tmpPtr4 As IntPtr = Marshal.StringToHGlobalAnsi(sIndex)
			Try 
				lNoOfChars = GetPrivateProfileString(sKey, tmpPtr4, sDefault, sString, 128, v_sINIFile)
				sIndex = Marshal.PtrToStringAnsi(tmpPtr4)
			Finally 
				Marshal.FreeHGlobal(tmpPtr4)
			End Try
			
			r_sLatestClientVersion = sString.Substring(0, lNoOfChars)
			If r_sLatestClientVersion.Trim() = "" Then
				r_sFieldInError = ACLatestVersionIndex
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Client Software Date
			sDefault = ""
			sString = New String(" ", 128)
			sKey = ACClientKey
			sIndex = ACSoftwareDateIndex
			Dim tmpPtr5 As IntPtr = Marshal.StringToHGlobalAnsi(sIndex)
			Try 
				lNoOfChars = GetPrivateProfileString(sKey, tmpPtr5, sDefault, sString, 128, v_sINIFile)
				sIndex = Marshal.PtrToStringAnsi(tmpPtr5)
			Finally 
				Marshal.FreeHGlobal(tmpPtr5)
			End Try
			
			sString = sString.Substring(0, lNoOfChars)
			If Information.IsDate(sString) Then
				r_dtClientSoftwareDate = CDate(sString)
			Else
				r_sFieldInError = ACSoftwareDateIndex
				r_sErrorValue = sString
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Is Mandatory
			sDefault = ""
			sString = New String(" ", 128)
			sKey = ACClientKey
			sIndex = ACMandatoryIndex
			Dim tmpPtr6 As IntPtr = Marshal.StringToHGlobalAnsi(sIndex)
			Try 
				lNoOfChars = GetPrivateProfileString(sKey, tmpPtr6, sDefault, sString, 128, v_sINIFile)
				sIndex = Marshal.PtrToStringAnsi(tmpPtr6)
			Finally 
				Marshal.FreeHGlobal(tmpPtr6)
			End Try
			
			Select Case sString.Substring(0, lNoOfChars).ToUpper()
				Case "Y", "YES"
					r_iIsMandatory = gPMConstants.PMEReturnCode.PMTrue
				Case "N", "NO"
					r_iIsMandatory = gPMConstants.PMEReturnCode.PMFalse
				Case Else
					r_sFieldInError = ACMandatoryIndex
					r_sErrorValue = sString
					Return gPMConstants.PMEReturnCode.PMFalse
			End Select
			
			' Is Auto Installable
			sDefault = ""
			sString = New String(" ", 128)
			sKey = ACClientKey
			sIndex = ACAutoInstallableIndex
			Dim tmpPtr7 As IntPtr = Marshal.StringToHGlobalAnsi(sIndex)
			Try 
				lNoOfChars = GetPrivateProfileString(sKey, tmpPtr7, sDefault, sString, 128, v_sINIFile)
				sIndex = Marshal.PtrToStringAnsi(tmpPtr7)
			Finally 
				Marshal.FreeHGlobal(tmpPtr7)
			End Try
			
			sString = sString.Substring(0, lNoOfChars).ToUpper()
			Select Case sString
				Case "Y", "YES"
					r_iIsAutoInstallable = gPMConstants.PMEReturnCode.PMTrue
				Case "N", "NO"
					r_iIsAutoInstallable = gPMConstants.PMEReturnCode.PMFalse
				Case Else
					r_sFieldInError = ACMandatoryIndex
					r_sErrorValue = sString
					Return gPMConstants.PMEReturnCode.PMFalse
			End Select
			
			' Install Program
			sDefault = ""
			sString = New String(" ", 128)
			sKey = ACClientKey
			sIndex = ACInstallProgramIndex
			Dim tmpPtr8 As IntPtr = Marshal.StringToHGlobalAnsi(sIndex)
			Try 
				lNoOfChars = GetPrivateProfileString(sKey, tmpPtr8, sDefault, sString, 128, v_sINIFile)
				sIndex = Marshal.PtrToStringAnsi(tmpPtr8)
			Finally 
				Marshal.FreeHGlobal(tmpPtr8)
			End Try
			
			r_sInstallProgram = sString.Substring(0, lNoOfChars)
			If r_sInstallProgram.Trim() = "" Then
				r_sFieldInError = ACInstallProgramIndex
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Description
			sDefault = ""
			sString = New String(" ", 128)
			sKey = ACClientKey
			sIndex = ACDescriptionIndex
			Dim tmpPtr9 As IntPtr = Marshal.StringToHGlobalAnsi(sIndex)
			Try 
				lNoOfChars = GetPrivateProfileString(sKey, tmpPtr9, sDefault, sString, 128, v_sINIFile)
				sIndex = Marshal.PtrToStringAnsi(tmpPtr9)
			Finally 
				Marshal.FreeHGlobal(tmpPtr9)
			End Try
			
			r_sDescription = sString.Substring(0, lNoOfChars)
			
			' Reboot Level
			sDefault = ""
			sString = New String(" ", 128)
			sKey = ACClientKey
			sIndex = ACRebootLevelIndex
			Dim tmpPtr10 As IntPtr = Marshal.StringToHGlobalAnsi(sIndex)
			Try 
				lNoOfChars = GetPrivateProfileString(sKey, tmpPtr10, sDefault, sString, 128, v_sINIFile)
				sIndex = Marshal.PtrToStringAnsi(tmpPtr10)
			Finally 
				Marshal.FreeHGlobal(tmpPtr10)
			End Try
			
			sString = sString.Substring(0, lNoOfChars)
			Select Case sString
				Case "0", "1", "2"
					r_iRebootLevel = CInt(sString)
				Case Else
					r_sFieldInError = ACRebootLevelIndex
					r_sErrorValue = sString
					Return gPMConstants.PMEReturnCode.PMFalse
			End Select
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetailsFromINIFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetailsFromINI", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Module
