Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
Module MigrateRegModule
	Public Const ACApp As String = "MigratedRegSettings"
	
	Public Sub Main()
		
		' Move the Architecture Settings
		ArchitectureSettings()
		
	End Sub
	
	Sub ArchitectureSettings()
		Dim lErrorValue, lReturnCode As gPMConstants.PMEReturnCode
		
		Dim sLogFile, sInDebug, sLocalEnabled, sUserLogLevel, sHostname, sPort As String
		
		Try 
			
			' ************************************************
			' ArchitectureInDebug
			
			' Get the Architecture In Debug Setting
			lErrorValue = CType(gPMFunctions.GetRegSettings(sResult:=sInDebug, sAppName:=gPMConstants.PMRegAppName, sSection:=gPMConstants.PMRegSecSystem, sKey:=gPMConstants.PMRegKeyArchitectureInDebug), gPMConstants.PMEReturnCode)
			
			If sInDebug.Trim() <> "" Then
				
				' Set the ArchitectureInDebug setting in
				' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Common
				lErrorValue = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyArchitectureInDebug, v_sSettingValue:=sInDebug), gPMConstants.PMEReturnCode)
				
				
				lErrorValue = CType(gPMFunctions.DeleteRegSettings(sAppName:=gPMConstants.PMRegAppName, sSection:=gPMConstants.PMRegSecSystem, sKey:=gPMConstants.PMRegKeyArchitectureInDebug), gPMConstants.PMEReturnCode)
				
			End If
			
			' ************************************************
			' ArchitectureLocalEnabled
			
			' Get the Architecture In Debug Setting
			lErrorValue = CType(gPMFunctions.GetRegSettings(sResult:=sLocalEnabled, sAppName:=gPMConstants.PMRegAppName, sSection:=gPMConstants.PMRegSecSystem, sKey:=gPMConstants.PMRegKeyArchitectureLocalEnabled), gPMConstants.PMEReturnCode)
			
			If sLocalEnabled.Trim() <> "" Then
				
				' Set the ArchitectureLocal Enabled setting in
				' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Client
				lErrorValue = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyArchitectureLocalEnabled, v_sSettingValue:=sLocalEnabled), gPMConstants.PMEReturnCode)
				
				lErrorValue = CType(gPMFunctions.DeleteRegSettings(sAppName:=gPMConstants.PMRegAppName, sSection:=gPMConstants.PMRegSecSystem, sKey:=gPMConstants.PMRegKeyArchitectureLocalEnabled), gPMConstants.PMEReturnCode)
				
			End If
			
			' ************************************************
			' User Log Level
			
			' Get the User Log Level Setting
			lErrorValue = CType(gPMFunctions.GetRegSettings(sResult:=sUserLogLevel, sAppName:=gPMConstants.PMRegAppName, sSection:=gPMConstants.PMRegSecSystem, sKey:=gPMConstants.PMRegKeyLogLevel), gPMConstants.PMEReturnCode)
			
			If sUserLogLevel.Trim() <> "" Then
				
				' Set the ArchitectureLocal Enabled setting in
				' HKEY_CURRENT_USER\software\PM\SiriusArchitecture\Client
				lErrorValue = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyLogLevel, v_sSettingValue:=sUserLogLevel), gPMConstants.PMEReturnCode)
				
				lErrorValue = CType(gPMFunctions.DeleteRegSettings(sAppName:=gPMConstants.PMRegAppName, sSection:=gPMConstants.PMRegSecSystem, sKey:=gPMConstants.PMRegKeyLogLevel), gPMConstants.PMEReturnCode)
				
			End If
			
			' ************************************************
			' LogFileName
			
			' Get the log file name from the registry
			lErrorValue = CType(gPMFunctions.GetRegSettings(sResult:=sLogFile, sAppName:=gPMConstants.PMRegAppName, sSection:=gPMConstants.PMRegSecSystem, sKey:=gPMConstants.PMRegKeyLogFile), gPMConstants.PMEReturnCode)
			
			If sLogFile.Trim() <> "" Then
				
				' Set the Log File Name setting in
				' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Common
				lErrorValue = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyLogFile, v_sSettingValue:=sLogFile), gPMConstants.PMEReturnCode)
				
				' Set the Log File Name setting in
				' HKEY_CURRENT_USER\software\PM\SiriusArchitecture\Common
				'DAK130100 - Do not set current user value for Log File Name
				'        lerrorvalue& = SetPMRegSetting( _
				'v_lPMERegSettingRoot:=pmeRSRCurrentUser, _
				'v_lPMEProductFamily:=pmePFSiriusArchitecture, _
				'v_lPMERegSettingLevel:=pmeRSLCommon, _
				'v_sSettingName:=PMRegKeyLogFile, _
				'v_sSettingValue:=sLogFile)
				
				lErrorValue = CType(gPMFunctions.DeleteRegSettings(sAppName:=gPMConstants.PMRegAppName, sSection:=gPMConstants.PMRegSecSystem, sKey:=gPMConstants.PMRegKeyLogFile), gPMConstants.PMEReturnCode)
				
			End If
			
			
			' ************************************************
			' Unix Link Settings
			
			' ************************************************
			' Host
			lErrorValue = CType(gPMFunctions.GetRegSettings(sResult:=sHostname, sAppName:=gPMConstants.PMProduct, sSection:="Linkage", sKey:="Host"), gPMConstants.PMEReturnCode)
			
			If sHostname.Trim() <> "" Then
				
				' Set the HostName setting in
				' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Server\UnixLink
				lErrorValue = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="Host", v_sSettingValue:=sHostname, v_sSubKey:="UnixLink"), gPMConstants.PMEReturnCode)
				
				' Delete existing setting
				lErrorValue = CType(gPMFunctions.DeleteRegSettings(sAppName:=gPMConstants.PMProduct, sSection:="Linkage", sKey:="Host"), gPMConstants.PMEReturnCode)
				
			End If
			
			
			' ************************************************
			' Port
			
			lReturnCode = CType(gPMFunctions.GetRegSettings(sResult:=sPort, sAppName:=gPMConstants.PMProduct, sSection:="Linkage", sKey:="Port"), gPMConstants.PMEReturnCode)
			
			If sPort.Trim() <> "" Then
				
				' Set the HostName setting in
				' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Server\UnixLink
				lErrorValue = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="Port", v_sSettingValue:=sPort, v_sSubKey:="UnixLink"), gPMConstants.PMEReturnCode)
				
				' Delete existing setting
				lErrorValue = CType(gPMFunctions.DeleteRegSettings(sAppName:=gPMConstants.PMProduct, sSection:="Linkage", sKey:="Port"), gPMConstants.PMEReturnCode)
				
			End If
			
			
			' ************************************************
			' Delete the whole Sirius Key
			Interaction.DeleteSetting("Sirius")
		
		Catch 
			
			
			
			Exit Sub
		End Try
		
		
	End Sub
End Module