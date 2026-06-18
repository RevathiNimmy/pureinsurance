Option Strict Off
Option Explicit On
Imports System
'Developer Guide No. 129
Imports SharedFiles
Module MainModule
	
	' for opening any file type
	Public Declare Function ShellExecute Lib "shell32.dll"  Alias "ShellExecuteA"(ByVal hWnd As Integer, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As Integer) As Integer
	
	' for ShellExecute
	Public Const SE_SHOW_NORMAL As Integer = 1
	
	Public Const ACApp As String = "ProductUpdateHistory"
	Public Const ACClass As String = "MainModule"
	
	
	' array constants
	Public Const UpdateUpdateID As Integer = 0
	Public Const UpdateProductID As Integer = 1
	Public Const UpdateDescription As Integer = 2
	Public Const UpdateNewProductVersion As Integer = 3
	Public Const UpdateInstallDate As Integer = 4
	Public Const UpdateReleaseNotesPath As Integer = 5
	Public Const UpdateUpdateDescription As Integer = 6
	
	Public Const ProductDescription As Integer = 0
	Public Const ProductCode As Integer = 1
	
	' ***************************************************************** '
	' Name: GetClientVersion
	'
	' Description: Gets the Client Version of the installed PM Product.
	'
	' ***************************************************************** '
	Public Function GetClientVersion(ByVal v_lPMEProductFamily As Integer, ByRef r_sVersion As String) As Integer
		
		Dim result As Integer = 0
		Dim sMsg As String = ""
		Dim lReturn As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			r_sVersion = ""
			
			' Get the Version from the Registry
			lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=v_lPMEProductFamily, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.PMRegKeyVersion, r_sSettingValue:=r_sVersion)
			
			If r_sVersion.Trim() = "" And v_lPMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusArchitecture Then
				
				Return result
			End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
End Module