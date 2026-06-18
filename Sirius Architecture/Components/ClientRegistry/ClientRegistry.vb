Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Module Module1
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	' ************************************************
	' Added to replace global variables 18/09/2003
	' Username.
	Private m_sUsername As String = ""
	
	' Password.
	Private m_sPassword As String = ""
	
	' User ID
	Private m_iUserID As Integer
	
	' Calling Application
	Private m_sCallingAppName As String = ""
	' Source ID
	Private m_iSourceID As Integer
	' Language ID
	Private m_iLanguageID As Integer
	' Currency ID
	Private m_iCurrencyID As Integer
	' LogLevel
	Private m_iLogLevel As Integer
	' ************************************************
	
	Public Const ACApp As String = "ClientRegistry"
	Public Const ACClass As String = "MainModule"
	
	'***********************************************
	' TO DO: GLOBAL VARIABLE IN BAS MODULE
	'Public g_iCountryID As Integer
	'***********************************************
	
	Private m_bUninstall As Boolean
	Private m_bVerbose As Boolean
	Private m_bHelp As Boolean
	Private m_sDebugSetting As String = ""
	Private m_sLocalEnabledSetting As String = ""
	Private m_sRemoteEnabledSetting As String = ""
	Private m_sLogMessageFile As String = ""
	Private m_sLogMessageLevel As String = ""
	Private m_sServerName As String = ""
	Private m_bUseRemoteAutomation As Boolean
	Private m_sUnixHost As String = ""
	Private m_sUnixPort As String = ""
	Private m_sQueryTimeout As String = ""
	
	Public Sub Main()
		
		Dim vCmdArgs As Object
		Dim sWholeCommand, sCommand, sValue As String
		
		Dim lErrorValue As Integer = bPMFunc.GetCommandLine(vArgArray:=vCmdArgs)
		
		' Check for errors.
		If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
			Exit Sub
		End If
		
		' Initialise Verbose and Uninstall flags
		m_bUninstall = False
		m_bVerbose = False
		m_bHelp = False
		m_sRemoteEnabledSetting = CStr(gPMConstants.PMEReturnCode.PMTrue)
		m_sDebugSetting = CStr(gPMConstants.PMEReturnCode.PMFalse)
		m_sLocalEnabledSetting = CStr(gPMConstants.PMEReturnCode.PMFalse)
		m_sLogMessageFile = gPMConstants.PMDefaultLogFile
		m_sLogMessageLevel = CStr(gPMConstants.PMELogLevel.PMLogInfo)
		m_sServerName = ""
		m_bUseRemoteAutomation = False
		m_sUnixHost = ""
		m_sUnixPort = ""
		m_sQueryTimeout = ""
		
		' If we found some commands
		If Information.IsArray(vCmdArgs) Then
			
			' For each command found

			For iSub As Integer = vCmdArgs.GetLowerBound(0) To vCmdArgs.GetUpperBound(0)
				

                sWholeCommand = CStr(vCmdArgs(iSub))
				' Get the first 2 characters of the command
				sCommand = sWholeCommand.Trim().ToUpper().Substring(0, 2)
				sValue = sWholeCommand.Substring(2).Trim()
				
				
				Select Case sCommand
					' Uninstall
					Case "-U", "/U"
						m_bUninstall = True
						
						' Verbose
					Case "-V", "/V"
						m_bVerbose = True
						
						' Help
					Case "-H", "/H", "-?", "/?"
						m_bHelp = True
						
						' Debug Setting
					Case "-D", "/D"
						If sValue = gPMConstants.PMEReturnCode.PMTrue Then
							m_sDebugSetting = CStr(gPMConstants.PMEReturnCode.PMTrue)
						End If
						
						' Local Enabled Setting
					Case "-L", "/L"
						If sValue = gPMConstants.PMEReturnCode.PMTrue Then
							m_sLocalEnabledSetting = CStr(gPMConstants.PMEReturnCode.PMTrue)
						End If
						
						' LogMessageFileSetting
					Case "-F", "/F"
						If sValue <> "" Then
							m_sLogMessageFile = sValue
						End If
						
						' Log Message Level
					Case "-M", "/M"
						Dim dbNumericTemp As Double
						If Double.TryParse(sValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
							m_sLogMessageLevel = sValue
						End If
						
						' Remote Enabled
					Case "-R", "/R"
						If sValue <> "" Then
							m_sRemoteEnabledSetting = sValue.Trim()
						End If
						
						' Server Name
					Case "-S", "/S"
						If sValue <> "" Then
							m_sServerName = sValue.Trim()
						End If
						
						' Use Remote Automation
					Case "-A", "/A"
						m_bUseRemoteAutomation = True
						
						' Unix Host
					Case "-X", "/X"
						If sValue <> "" Then
							m_sUnixHost = sValue.Trim()
						End If
						
						' Unix Port
					Case "-P", "/P"
						Dim dbNumericTemp2 As Double
						If Double.TryParse(sValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
							m_sUnixPort = sValue
						End If
						
						' Query Timeout
					Case "-T", "/T"
						Dim dbNumericTemp3 As Double
						If Double.TryParse(sValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
							m_sQueryTimeout = sValue
						End If
						
						' Unknown
					Case Else
						
				End Select
				
			Next iSub
			
		End If
		
		' If Help requested then display it
		If m_bHelp Then
			DisplayHelp()
		End If
		
		' Do we want to Install or Uninstall
		If m_bUninstall Then
			
			Uninstall()
			
		Else
			
			Install()
			
		End If
		
		' Display Message if in Verbose Mode
		If m_bVerbose Then
			
			If m_bUninstall Then
				MessageBox.Show("Uninstalled Client Registry Settings OK!", Application.ProductName)
			Else
				MessageBox.Show("Installed Client Registry Settings OK!", Application.ProductName)
			End If
			
		End If
		
	End Sub
	
	' ***************************************************************** '
	' Name: Install
	'
	' Description:
	'
	' ***************************************************************** '
	Private Sub Install()
		Dim lErrorValue As Integer
		Dim sCliRegString As String = ""
		Dim vShellRet As Double
		Dim sLocalSystemName As String = ""
		
		 
			
			' Log message level
			If m_bVerbose Then
				MessageBox.Show("Setting Log Message Level to - " & m_sLogMessageLevel, Application.ProductName)
			End If
			
            '    lErrorValue& = SaveRegSettings(m_sLogMessageLevel, PMRegAppName, _
            ''        PMRegSecSystem, PMRegKeyLogLevel)

			' Set the UserLogLevel setting in
			' HKEY_CURRENT_USER\software\PM\SiriusArchitecture\Common
			lErrorValue = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyLogLevel, v_sSettingValue:=m_sLogMessageLevel)
			
			' Check for errors.
			If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
				If m_bVerbose Then
					gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to save Default Log Level to the registry", vApp:=ACApp, vClass:=ACClass, vMethod:="Sub Main")
				End If
				Exit Sub
			End If
			
			' Log Message File
			If m_bVerbose Then
				MessageBox.Show("Setting Log Message File to - " & m_sLogMessageFile, Application.ProductName)
			End If
			
			'    lErrorValue& = SaveRegSettings(m_sLogMessageFile, PMRegAppName, _
			''        PMRegSecSystem, PMRegKeyLogFile)
			
			' Set the UserLogFile setting in
			' HKEY_CURRENT_USER\software\PM\SiriusArchitecture\Common
			'DAK130100 - Do not set user LogFileName registry entry
			'    lErrorValue& = SetPMRegSetting( _
			'v_lPMERegSettingRoot:=pmeRSRCurrentUser, _
			'v_lPMEProductFamily:=pmePFSiriusArchitecture, _
			'v_lPMERegSettingLevel:=pmeRSLCommon, _
			'v_sSettingName:=PMRegKeyLogFile, _
			'v_sSettingValue:=m_sLogMessageFile$)
			
			' Check for errors.
			If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
				If m_bVerbose Then
					gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to save Log File to the registry", vApp:=ACApp, vClass:=ACClass, vMethod:="Sub Main")
				End If
				Exit Sub
			End If
			
			' Set the UserLogFile setting in
			' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Common
			lErrorValue = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyLogFile, v_sSettingValue:=m_sLogMessageFile)
			
			' Check for errors.
			If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
				If m_bVerbose Then
					gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to save Log File to the registry", vApp:=ACApp, vClass:=ACClass, vMethod:="Sub Main")
				End If
				Exit Sub
			End If
			
			' Local Enabled
			
			If m_bVerbose Then
				MessageBox.Show("Setting Local Enabled to - " & m_sLocalEnabledSetting, Application.ProductName)
			End If

            '    lErrorValue& = SaveRegSettings(m_sLocalEnabledSetting, PMRegAppName, _
            ''        PMRegSecSystem, PMRegKeyArchitectureLocalEnabled)

			' Set the Local Enabled setting in
			' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Common
			lErrorValue = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyArchitectureLocalEnabled, v_sSettingValue:=m_sLocalEnabledSetting)
			
			' Check for errors.
			If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
				If m_bVerbose Then
					gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to save Local Enabled Setting to the registry", vApp:=ACApp, vClass:=ACClass, vMethod:="Sub Main")
				End If
				Exit Sub
			End If
			
			' Debug
			
			If m_bVerbose Then
				MessageBox.Show("Setting Debug to - " & m_sDebugSetting, Application.ProductName)
            End If

            '    lErrorValue& = SaveRegSettings(m_sDebugSetting, PMRegAppName, _
            ''        PMRegSecSystem, PMRegKeyArchitectureInDebug)

			' Set the Debug setting in
			' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Common
			lErrorValue = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyArchitectureInDebug, v_sSettingValue:=m_sDebugSetting)
			
			' Check for errors.
			If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
				If m_bVerbose Then
					gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to save Default Log File to the registry", vApp:=ACApp, vClass:=ACClass, vMethod:="Sub Main")
				End If
				Exit Sub
			End If
			
			m_sServerName = m_sServerName.Trim()
			
			' Get the local pc machine name
			lErrorValue = gPMFunctions.GetSystemName(sLocalSystemName)
			sLocalSystemName = sLocalSystemName.Trim()
			
			' If the server name supplied is the local system name
			' then there is no remote server so set it to LOCAL
			If sLocalSystemName.ToUpper() = m_sServerName.ToUpper() Then
				m_sServerName = "LOCAL"
			End If
			
			' If we have a server name
			If m_sServerName <> "" Then
				
				' RDC 15022002
				If (m_sServerName.ToUpper() <> "LOCAL") And (m_sServerName.ToUpper() <> "(LOCAL)") And (m_sServerName.ToUpper() <> "NONE") Then
                    '        If ((UCase(m_sServerName$) <> "LOCAL") _
                    ''        And (UCase(m_sServerName$) <> "NONE")) Then

                    ' Setup Remote Connection
					
					If m_bVerbose Then
						MessageBox.Show("Setting PMRemoteLicenceManager to SERVER - " & m_sServerName, Application.ProductName)
					End If
					
					sCliRegString = "Clireg32.exe bPMRemoteLicenceManager.vbr -s " & m_sServerName & " -p ncacn_ip_tcp"
					
					If Not m_bUseRemoteAutomation Then
						sCliRegString = sCliRegString & " -d"
					End If
					
					If Not m_bVerbose Then
						sCliRegString = sCliRegString & " -q"
					End If
					

					Dim startInfo As ProcessStartInfo = New ProcessStartInfo(sCliRegString)
					startInfo.WindowStyle = ProcessWindowStyle.Normal
					vShellRet = Process.Start(startInfo).Id
					
					' We have set up a REMOTE connection so force the Remote Setting to True
					m_sRemoteEnabledSetting = CStr(gPMConstants.PMEReturnCode.PMTrue)
					
				Else
					
					' No Server, there Remote = False
					m_sRemoteEnabledSetting = CStr(gPMConstants.PMEReturnCode.PMFalse)
					
				End If
				
			End If
			
			' Remote Enabled
			
			If m_bVerbose Then
				MessageBox.Show("Setting Remote Enabled to - " & m_sRemoteEnabledSetting, Application.ProductName)
			End If
			
            '    lErrorValue& = SaveRegSettings(m_sRemoteEnabledSetting, PMRegAppName, _
            ''        PMRegSecSystem, PMRegKeyArchitectureServerEnabled)

			' Set the Remote Enabled setting in
			' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Common
			lErrorValue = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyArchitectureServerEnabled, v_sSettingValue:=m_sRemoteEnabledSetting)
			
			' Check for errors.
			If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
				If m_bVerbose Then
					gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to save Remote Enabled to the registry", vApp:=ACApp, vClass:=ACClass, vMethod:="Sub Main")
				End If
				Exit Sub
			End If
			
			' If we have both a Host AND Port Setting
			If (m_sUnixHost.Trim() <> "") And (m_sUnixPort.Trim() <> "") Then
				
				If m_bVerbose Then
					MessageBox.Show("Setting Unix Host/Port to - " & m_sUnixHost & " - " & m_sUnixPort, Application.ProductName)
				End If
				
				' Set the HostName setting in
				' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Server\UnixLink
				lErrorValue = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="Host", v_sSettingValue:=m_sUnixHost, v_sSubKey:="UnixLink")
				
				' Set the HostName setting in
				' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Server\UnixLink
				lErrorValue = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="Port", v_sSettingValue:=m_sUnixPort, v_sSubKey:="UnixLink")
				
			End If
			
			' If we have a Query Timeout setting
			If m_sQueryTimeout.Trim() <> "" Then
				
				If m_bVerbose Then
					MessageBox.Show("Setting Database Query Timeout to - " & m_sQueryTimeout, Application.ProductName)
				End If
				
				' Set the Query Timeout setting in
				' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Server\
				lErrorValue = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=gPMConstants.PMRegKeyQueryTimeoutSeconds, v_sSettingValue:=m_sQueryTimeout)
				
			End If
		
		
		
	End Sub
	
	' ***************************************************************** '
	' Name: Uninstall
	'
	' Description:
	'
	' ***************************************************************** '
	Private Sub Uninstall()
		Dim lErrorValue As Integer
		Dim sCliRegString As String = ""
		Dim vShellRet As Double
		Dim sKey As String = ""
		
		 
			
			sKey = gPMConstants.ACRegRoot & gPMConstants.ACRegSiriusArchitecture
			
			' Current User Settings
			If m_bVerbose Then
				MessageBox.Show("Deleting Current User Setting", Application.ProductName)
			End If
			
			lErrorValue = gPMFunctions.DeleteKey(gPMConstants.HKEY_CURRENT_USER, sKey & gPMConstants.ACRegClient)
			lErrorValue = gPMFunctions.DeleteKey(gPMConstants.HKEY_CURRENT_USER, sKey & gPMConstants.ACRegCommon)
			lErrorValue = gPMFunctions.DeleteKey(gPMConstants.HKEY_CURRENT_USER, sKey & gPMConstants.ACRegServer)
			lErrorValue = gPMFunctions.DeleteKey(gPMConstants.HKEY_CURRENT_USER, sKey)
			
			' Local Machine Settings
			If m_bVerbose Then
				MessageBox.Show("Deleting Local Machine Setting", Application.ProductName)
			End If
			
			lErrorValue = gPMFunctions.DeleteKey(gPMConstants.HKEY_LOCAL_MACHINE, sKey & gPMConstants.ACRegClient)
			lErrorValue = gPMFunctions.DeleteKey(gPMConstants.HKEY_LOCAL_MACHINE, sKey & gPMConstants.ACRegCommon)
			lErrorValue = gPMFunctions.DeleteKey(gPMConstants.HKEY_LOCAL_MACHINE, sKey & gPMConstants.ACRegServer & "\UnixLink")
			lErrorValue = gPMFunctions.DeleteKey(gPMConstants.HKEY_LOCAL_MACHINE, sKey & gPMConstants.ACRegServer)
			lErrorValue = gPMFunctions.DeleteKey(gPMConstants.HKEY_LOCAL_MACHINE, sKey)
			
			'    ' Log Message Level
			'
			'    If (m_bVerbose = True) Then
			'        MsgBox "Deleting Log Message Level Setting"
			'    End If
			'
			'    lErrorValue = DeleteRegSettings(sAppName:=PMRegAppName, sSection:=PMRegSecSystem, _
			''        sKey:=PMRegKeyLogLevel)
			'
			'    ' Check for errors.
			'    If (lErrorValue& <> PMTrue) Then
			'        If (m_bVerbose = True) Then
			'            LogMessagePopup _
			''                iType:=PMLogError, _
			''                sMsg:="Failed to delete default Log Level from the registry", _
			''                vApp:=ACApp, _
			''                vClass:=ACClass, _
			''                vMethod:="Sub Main"
			'        End If
			'        Exit Sub
			'    End If
			'
			'    ' Log Message File
			'    If (m_bVerbose = True) Then
			'        MsgBox "Deleting Log Message File Setting"
			'    End If
			'
			'    lErrorValue = DeleteRegSettings(sAppName:=PMRegAppName, sSection:=PMRegSecSystem, _
			''        sKey:=PMRegKeyLogFile)
			'
			'    ' Check for errors.
			'    If (lErrorValue& <> PMTrue) Then
			'        If (m_bVerbose = True) Then
			'            LogMessagePopup _
			''                iType:=PMLogError, _
			''                sMsg:="Failed to delete Default Log File from the registry", _
			''                vApp:=ACApp, _
			''                vClass:=ACClass, _
			''                vMethod:="Sub Main"
			'        End If
			'        Exit Sub
			'    End If
			'
			'    ' Local Enabled
			'    If (m_bVerbose = True) Then
			'        MsgBox "Deleting Local Enabled Setting"
			'    End If
			'
			'    lErrorValue = DeleteRegSettings(sAppName:=PMRegAppName, sSection:=PMRegSecSystem, _
			''        sKey:=PMRegKeyArchitectureLocalEnabled)
			'
			'    ' Check for errors.
			'    If (lErrorValue& <> PMTrue) Then
			'        If (m_bVerbose = True) Then
			'            LogMessagePopup _
			''                iType:=PMLogError, _
			''                sMsg:="Failed to delete Default Log File from the registry", _
			''                vApp:=ACApp, _
			''                vClass:=ACClass, _
			''                vMethod:="Sub Main"
			'        End If
			'        Exit Sub
			'    End If
			'
			'    ' Debug
			'    If (m_bVerbose = True) Then
			'        MsgBox "Deleting Debug Setting"
			'    End If
			'
			'    lErrorValue = DeleteRegSettings(sAppName:=PMRegAppName, sSection:=PMRegSecSystem, _
			''        sKey:=PMRegKeyArchitectureInDebug)
			'
			'    ' Check for errors.
			'    If (lErrorValue& <> PMTrue) Then
			'        If (m_bVerbose = True) Then
			'            LogMessagePopup _
			''                iType:=PMLogError, _
			''                sMsg:="Failed to delete Default Log File from the registry", _
			''                vApp:=ACApp, _
			''                vClass:=ACClass, _
			''                vMethod:="Sub Main"
			'        End If
			'        Exit Sub
			'    End If
			
			If m_bVerbose Then
				MessageBox.Show("Uninstalling DCOM for PMRemoteLicenceManager", Application.ProductName)
			End If
			
			sCliRegString = "Clireg32.exe bPMRemoteLicenceManager.vbr -u"
			
			If Not m_bVerbose Then
				sCliRegString = sCliRegString & " -q"
			End If
			

			Dim startInfo As ProcessStartInfo = New ProcessStartInfo(sCliRegString)
			startInfo.WindowStyle = ProcessWindowStyle.Normal
			vShellRet = Process.Start(startInfo).Id
		
		
		
	End Sub
	
	' ***************************************************************** '
	' Name: DisplayHelp
	'
	' Description:
	'
	' ***************************************************************** '
	Private Sub DisplayHelp()
		Dim sHelp As String = ""
		
		 
			
			sHelp = "Usage = CLIENTREGISTRY [options]" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)
			sHelp = sHelp & "Options" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)
			sHelp = sHelp & "-U" & Strings.Chr(9) & Strings.Chr(9) & "Uninstall All Registry Settings" & Strings.Chr(13) & Strings.Chr(10)
			sHelp = sHelp & "-H/-?" & Strings.Chr(9) & Strings.Chr(9) & "Help" & Strings.Chr(13) & Strings.Chr(10)
			sHelp = sHelp & "-V" & Strings.Chr(9) & Strings.Chr(9) & "Verbose Mode" & Strings.Chr(13) & Strings.Chr(10)
			sHelp = sHelp & "-D[0/1]" & Strings.Chr(9) & Strings.Chr(9) & "Run Architecture In Debug (Default = 0)" & Strings.Chr(13) & Strings.Chr(10)
			sHelp = sHelp & "-M[Level]" & Strings.Chr(9) & Strings.Chr(9) & "Log Message Level (Default = LogInfo)" & Strings.Chr(13) & Strings.Chr(10)
			sHelp = sHelp & "-F[File]" & Strings.Chr(9) & Strings.Chr(9) & "Log message File" & Strings.Chr(13) & Strings.Chr(10)
			sHelp = sHelp & "-L[0/1]" & Strings.Chr(9) & Strings.Chr(9) & "Architecture Local Enabled (Default = 0)" & Strings.Chr(13) & Strings.Chr(10)
			sHelp = sHelp & "-R[0/1]" & Strings.Chr(9) & Strings.Chr(9) & "Architecture Remote Enabled (Default = 1)" & Strings.Chr(13) & Strings.Chr(10)
			sHelp = sHelp & "-S[Server]" & Strings.Chr(9) & Strings.Chr(9) & "Setup Remote Connection to point to this Server" & Strings.Chr(13) & Strings.Chr(10)
			sHelp = sHelp & "-A" & Strings.Chr(9) & Strings.Chr(9) & "Use Remote Automation instead of DCOM" & Strings.Chr(13) & Strings.Chr(10)
			sHelp = sHelp & "-X[UnixHost]" & Strings.Chr(9) & "Unix Host (Both Host and Port must be supplied.)" & Strings.Chr(13) & Strings.Chr(10)
			sHelp = sHelp & "-P[UnixPort]" & Strings.Chr(9) & "Unix Port - Numeric (Both Host and Port must be supplied.)" & Strings.Chr(13) & Strings.Chr(10)
			sHelp = sHelp & "-T[Seconds]" & Strings.Chr(9) & "Database Query Timeout setting in Seconds" & Strings.Chr(13) & Strings.Chr(10)
			sHelp = sHelp & Strings.Chr(13) & Strings.Chr(10)
			sHelp = sHelp & "Example, " & Strings.Chr(9) & Strings.Chr(9) & "CLIENTREGISTRY -D1 -SSIRIUS_NT_SRV -FC:\Temp\Sirius.Log"
			
			MessageBox.Show(sHelp, "Client Registry Help", MessageBoxButtons.OK, MessageBoxIcon.Information)
		
		
		
	End Sub
End Module

