Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmMain
	Inherits System.Windows.Forms.Form
	
	Private m_bMerged As Boolean
	
	Private m_lReturn As Integer
	
	Private m_sLogFile As String = ""
	Private m_sScriptPath As String = ""
	Private m_sKeyLog As String = ""
	
	Private m_sServer As String = ""
	Private m_sDatabase As String = ""
	
	 ' Private Const ACScript2 As String = "PN11317-2.sql" ### script not used:
	Private Const ACScript1 As String = "dmeh_script1.sql"
	Private Const ACScript2 As String = "dmeh_script2.sql"
	Private Const ACScript3 As String = "dmeh_script3.sql"
	Private Const ACScript4 As String = "dmeh_script4.sql"
	
	Private Const TAB_MAIN As Integer = 0
	Private Const TAB_ADVANCED As Integer = 1
	
	Private Const LOGFILE_NAME As String = "dme_harmoniser.txt"
	
	Private Const APP_TITLE As String = "Documaster Enterprise Harmoniser"
	 'end
	
	Private Sub frmMain_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			m_lReturn = ActivateForm()
		End If
	End Sub
	

	Private Sub frmMain_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		m_lReturn = LoadForm()
	End Sub
	
	Private Sub frmMain_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		m_lReturn = UnloadForm()
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExit.Click
		Me.Close()
	End Sub
	
	Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
		m_lReturn = CallHelp()
	End Sub
	
	Private Sub cmdRun_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRun.Click
		m_lReturn = RunHarmoniser()
	End Sub
	
	Private Sub cmdSetDefaults_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSetDefaults.Click
		m_lReturn = DefaultAdvancedSettings()
	End Sub
	
	Private Sub cmdSetMainDefaults_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSetMainDefaults.Click
		m_lReturn = DefaultMainSettings()
	End Sub
	
	Private Sub cmdServerRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdServerRefresh.Click
		PopulateServers()
	End Sub
	
	Private Sub cmdDatabaseRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDatabaseRefresh.Click
		PopulateDatabases()
	End Sub
	
	Private Sub txtLogFile_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLogFile.Enter
        iPMFunc.SelectText(txtLogFile)
	End Sub
	
	Private Sub txtPassword_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPassword.Enter
        iPMFunc.SelectText(txtPassword)
	End Sub
	
	Private Sub txtUsername_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtUsername.Enter
        iPMFunc.SelectText(txtUsername)
	End Sub
	
	Private Sub chkWindowsAuth_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkWindowsAuth.CheckStateChanged
		
		If chkWindowsAuth.CheckState = CheckState.Checked Then
			lblUsername.Enabled = False
			lblPassword.Enabled = False
			txtUsername.Enabled = False
			txtPassword.Enabled = False
		Else
			lblUsername.Enabled = True
			lblPassword.Enabled = True
			txtUsername.Enabled = True
			txtPassword.Enabled = True
		End If
		
	End Sub
	
	Private Sub cboVersion_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboVersion.SelectedIndexChanged
		
		 ' If its not auto detect then warn the user
		If cboVersion.SelectedIndex <> 0 Then
			If MessageBox.Show("It's advisable to leave this setting on (Auto Detect)." & Environment.NewLine & "Are you sure you wish to change it?", APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
				cboVersion.SelectedIndex = 0
				Exit Sub
			End If
		End If
		
		If cboVersion.SelectedIndex = 1 Then
			chkDB.CheckState = CheckState.Unchecked
		Else
			chkDB.CheckState = CheckState.Checked
		End If
		
	End Sub
	
	Private Sub chkDB_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkDB.CheckStateChanged
		
		Dim bEnabled As Boolean
		
		bEnabled = (chkDB.CheckState = CheckState.Checked)
		
		fraDB.Enabled = bEnabled
		chkScript1.Enabled = bEnabled
		chkScript2.Enabled = bEnabled
		chkScript3.Enabled = bEnabled
		chkScript4.Enabled = bEnabled
		
	End Sub
	
	Private Sub chkFiles_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkFiles.CheckStateChanged
		
		Dim bEnabled As Boolean
		
		bEnabled = (chkFiles.CheckState = CheckState.Checked)
		
		fraFile.Enabled = bEnabled
		chkLogExists.Enabled = bEnabled
		chkLogMissing.Enabled = bEnabled
		chkQuick.Enabled = bEnabled
		
	End Sub
	
	Private Sub txtLogFile_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLogFile.Leave
		
		If txtLogFile.Text.Trim().Substring(txtLogFile.Text.Trim().Length - 4).ToLower() <> ".txt" Then
			MessageBox.Show("logfile must end with .txt", APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
			txtLogFile.Focus()
			
			Exit Sub
		End If
		
		txtLogFile.Text = txtLogFile.Text.ToLower()
		
		txtOutputFile.Text = txtLogFile.Text.Replace(".txt", "_{scriptname}.txt")
		
		m_sLogFile = txtLogFile.Text
		
	End Sub
	
	 ' ##########################################################################
	 ' Method: tabMain_KeyDown
	 ' Desc:   detects "God Mode"
	 ' History:
	 '     RDC 25082004 created
	 ' ##########################################################################
	 'Private Sub tabMain_KeyDown(KeyCode As Integer, Shift As Integer)
	 '
	 '    On Error GoTo Err_tabMain_KeyDown
	 '
	 '    m_sKeyLog = m_sKeyLog & Chr(KeyCode)
	 '
	 '    If (Right$(UCase(m_sKeyLog), Len(ACMagicWord)) = ACMagicWord) And (Shift And vbAltMask) Then
	 '        tabMain.TabVisible(TAB_ADVANCED) = True
	 '        fmeAdvanced.Enabled = True
	 '        tabMain.Tab = TAB_ADVANCED
	 '    End If
	 '
	 '    ' Make sure it doesn't go on for too long
	 '    If Len(m_sKeyLog) > Len(ACMagicWord) Then
	 '        m_sKeyLog = Right$(m_sKeyLog, (Len(ACMagicWord)))
	 '    End If
	 '
	 '    Exit Sub
	 '
	 'Err_tabMain_KeyDown:
	 '
	 '    m_sKeyLog = ""
	 '
	 'End Sub
	
	 ' ##########################################################################
	 ' Method: ButtonSwitch
	 ' Desc:   Enable/disabled buttons
	 ' History:
	 '     RDC 25082004 created
	 ' ##########################################################################
	Private Sub ButtonSwitch(ByVal bMode As Boolean)
		
		cmdExit.Enabled = bMode
		cmdHelp.Enabled = bMode
		cmdRun.Enabled = bMode
		cmdSetDefaults.Enabled = bMode
		
	End Sub
	
	 ' ##########################################################################
	 ' Method: DefaultAdvancedSettings
	 ' Desc:   Returns properties to default settings on Advanced tab
	 ' History:
	 '     RDC 25082004 created
	 ' ##########################################################################
	Private Function DefaultAdvancedSettings() As Integer
		
		Dim result As Integer = 0
        Dim sSysDrive As String
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			 ' get the script path
			
			 ' TODO - change this
			If Not My.Application.Info.DirectoryPath.EndsWith("\") Then
				m_sScriptPath = My.Application.Info.DirectoryPath & "\Scripts\"
			Else
				m_sScriptPath = My.Application.Info.DirectoryPath & "Scripts\"
			End If
			
			 ' get SQL Server name from server registry
			m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="Server", r_sSettingValue:=m_sServer, v_sSubKey:="Databases\SiriusSolutions")
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_sServer = ""
			End If
			
			 ' get database name from server registry
			m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="Database", r_sSettingValue:=m_sDatabase, v_sSubKey:="Databases\SiriusSolutions")
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_sDatabase = ""
			End If
			
			 ' get the drive that the O/S is installed on. The log file and
			 ' script output files will be created in the sys drive root
			sSysDrive = GetSysDir()
			sSysDrive = sSysDrive.Substring(0, sSysDrive.IndexOf("\"c) + 1)
			
			 ' advanced tab properties
			
			chkWindowsAuth.CheckState = CheckState.Unchecked
			txtUsername.Enabled = True
			txtPassword.Enabled = True
			
			 ' set up the drop-down for versions
			cboVersion.Items.Clear()
			cboVersion.Items.Add("(Auto Detect)")
			cboVersion.Items.Add("Sirius 1.6.x <")
			cboVersion.Items.Add("Sirius 1.8.x >")
			cboVersion.SelectedIndex = 0
			
			txtLogFile.Text = sSysDrive & LOGFILE_NAME
            txtUsername.Text = ""
            txtPassword.Text = ""
			txtPath.Text = m_sScriptPath
			 ' each script generates a new output file in the system drive root
			txtOutputFile.Text = sSysDrive & LOGFILE_NAME.Replace(".txt", "_{scriptname}.txt")
			
			m_sLogFile = txtLogFile.Text
			
			PopulateServers()
			PopulateDatabases()
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	 ' ##########################################################################
	 ' Method: DefaultMainSettings
	 ' Desc:   Returns properties to default settings on Main tab
	 ' History:
	 '     RDC 25082004 created
	 ' ##########################################################################
	Private Function DefaultMainSettings() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			chkFiles.CheckState = CheckState.Checked
			chkLogMissing.CheckState = CheckState.Checked
			chkLogExists.CheckState = CheckState.Unchecked
			chkQuick.CheckState = CheckState.Checked
			
			chkDB.CheckState = CheckState.Checked
			chkScript1.CheckState = CheckState.Checked
			chkScript2.CheckState = CheckState.Checked
			chkScript3.CheckState = CheckState.Checked
			chkScript4.CheckState = CheckState.Checked
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	 ' ##########################################################################
	 ' Method: SQLQuery
	 ' Desc:   Run a SQL query via dPMDAO
	 ' History:
	 '
	 ' ##########################################################################
	Private Function SQLQuery(ByVal v_sSQL As String, ByRef r_vResultArray(,) As Object) As Integer 
		
		Dim result As Integer = 0 
		
		result = gPMConstants.PMEReturnCode.PMFalse
		
		Dim oDatabase As New dPMDAO.Database 
		
		 ' Open the database
		On Error GoTo Err_TryNextVersion
		
		 ' Try via DSN only
        m_lReturn = oDatabase.OpenDatabase(sSiriusUsername:="sirius", iSourceID:=1, iLanguageID:=1, sCallingAppName:=ACApp, vDSN:=PMDocumasterDSN)
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			MessageBox.Show("Failed to open database", APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error)
		End If
		
		GoTo Err_NoErr
		
Err_TryNextVersion: 
		
		On Error GoTo Err_SQLQuery
		
		m_lReturn = oDatabase.OpenDatabase(sSiriusUsername:="sirius", iSourceID:=1, iLanguageID:=1, sCallingAppName:=ACApp, vDSN:=PMDocumasterDSN)
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			MessageBox.Show("Failed to open database", APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error)
		End If
		
Err_NoErr: 
		
		 ' SQL Query
		 ' CTAF 20030528 - Changed to return all records
        m_lReturn = oDatabase.SQLSelect(sSQL:=v_sSQL, sSQLName:="DMESQLQuery", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)
		
		result = m_lReturn
		
		oDatabase = Nothing
		
		Return result
		
Err_SQLQuery: 
		
		LogMessageShort(v_sMessage:=Information.Err().Description, v_sTitle:="Error: " & Information.Err().Number)
		
		Return gPMConstants.PMEReturnCode.PMError
		
	End Function
	
	 ' ##########################################################################
	 ' Method: CheckVersion
	 ' Desc:   Check DME version
	 ' History:
	 '
	 ' ##########################################################################
	Private Function CheckVersion() As Integer
		
		Dim result As Integer = 0
		Dim sSQL As String = ""
        Dim vResultArray(,) As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			LogFile(v_sLogFile:=m_sLogFile, v_sText:="Checking for presence of DOC_annotation table...")
			
			 ' Work out if it's merged or not
			sSQL = "SELECT * FROM sysobjects WHERE NAME = 'DOC_annotation'"
			
			m_lReturn = SQLQuery(v_sSQL:=sSQL, r_vResultArray:=vResultArray)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return result
			End If
			
			If Information.IsArray(vResultArray) Then
				m_bMerged = True
				LogFile(v_sLogFile:=m_sLogFile, v_sText:="... Found. Determined database is merged (1.8.x)")
			Else
				m_bMerged = False
				LogFile(v_sLogFile:=m_sLogFile, v_sText:="... Not found. Determined database is not merged (1.6.x)")
				
			End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	 ' ##########################################################################
	 ' Method: RunFileCheck
	 ' Desc:   Check that files referenced on doc_page actually exist in DME
	 '         repository
	 ' History:
	 '
	 ' ##########################################################################
	Private Function RunFileCheck() As Integer
		
		Dim result As Integer = 0
		Dim lMissingCnt, lPresentCnt As Integer
		Dim sSQL, sTableDevice, sTablePage, sServerName, sShareName, sPath, sFilename As String
        Dim vResultArray(,) As Object
		
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			lMissingCnt = 0
			lPresentCnt = 0
			
			LogFile(v_sLogFile:=m_sLogFile, v_sText:="Beginning DME Integrity Check at " & DateTimeHelper.ToString(DateTime.Now))
			
			LogFile(v_sLogFile:=m_sLogFile, v_sText:="Using options:")
			LogFile(v_sLogFile:=m_sLogFile, v_sText:="     Log Missing Files         : " & (IIf(chkLogMissing.CheckState = CheckState.Checked, "Yes", "No")))
			LogFile(v_sLogFile:=m_sLogFile, v_sText:="     Log Existing Files        : " & (IIf(chkLogExists.CheckState = CheckState.Checked, "Yes", "No")))
			LogFile(v_sLogFile:=m_sLogFile, v_sText:="     Check 1 Page Per Document : " & (IIf(chkQuick.CheckState = CheckState.Checked, "Yes", "No")))
			
			 ' Construct the names of the tables
			If m_bMerged Then
				sTableDevice = ACDocTablePrefix & "device"
				sTablePage = ACDocTablePrefix & "page"
			Else
				sTableDevice = "device"
				sTablePage = "page"
			End If
			
			 ' Get the path to the shares
			sSQL = "SELECT server_unc, share_name FROM " & sTableDevice
			
			m_lReturn = SQLQuery(v_sSQL:=sSQL, r_vResultArray:=vResultArray)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return result
			End If
			
			If Not Information.IsArray(vResultArray) Then
				MessageBox.Show("There are no records in the " & sTableDevice & " table", APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information)
				Return result
			End If
			

			sServerName = CStr(vResultArray(0, 0))

			sShareName = CStr(vResultArray(1, 0))
			
			 ' Construct the path
			sPath = sServerName & sShareName & "\data"
			
			 ' Get the names of the files to check
			If chkQuick.CheckState = CheckState.Checked Then
				sSQL = "SELECT page_name, page_type, create_date FROM " & sTablePage & " WHERE page_num = 1"
			Else
				sSQL = "SELECT page_name, page_type, create_date FROM " & sTablePage
			End If
			
			m_lReturn = SQLQuery(v_sSQL:=sSQL, r_vResultArray:=vResultArray)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return result
			End If
			
			 ' Check we have some
			If Not Information.IsArray(vResultArray) Then
				MessageBox.Show("There are no records in the " & sTablePage & " table", APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information)
				Return result
			End If
			

			For lLoop1 As Integer = 0 To vResultArray.GetUpperBound(1)

				sFilename = CStr(vResultArray(0, lLoop1)).Trim() & "." & CStr(vResultArray(1, lLoop1)).Trim()
				
				If FileSystem.Dir(sPath & sFilename, FileAttribute.Normal) = "" Then
					 ' Missing
					If chkLogMissing.CheckState = CheckState.Checked Then

						LogFile(v_sLogFile:=m_sLogFile, v_sText:="Missing File : " & sPath & sFilename & " (date: " & CStr(vResultArray(2, lLoop1)) & ")")
					End If
					lMissingCnt += 1
				Else
					 ' Present
					If chkLogExists.CheckState = CheckState.Checked Then
						LogFile(v_sLogFile:=m_sLogFile, v_sText:="Present File : " & sPath & sFilename)
					End If
					lPresentCnt += 1
				End If
				
			Next 
			
			LogFile(v_sLogFile:=m_sLogFile, v_sText:="Summary:")
			

			LogFile(v_sLogFile:=m_sLogFile, v_sText:="     Files Checked  : " & vResultArray.GetUpperBound(1) + 1)
			
			LogFile(v_sLogFile:=m_sLogFile, v_sText:="     Missing Files  : " & lMissingCnt)
			
			LogFile(v_sLogFile:=m_sLogFile, v_sText:="     Existing Files : " & lPresentCnt)
			
			LogFile(v_sLogFile:=m_sLogFile, v_sText:="Finished DME Integrity Check at " & DateTimeHelper.ToString(DateTime.Now))
			
			LogFile(v_sLogFile:=m_sLogFile, v_sText:=New String("*", 90))
			
			lstStatus.Items.Add("There were : ")
			lstStatus.Items.Add(CStr(lMissingCnt) & " file(s) missing")
			lstStatus.Items.Add(CStr(lPresentCnt) & " file(s) present")
			lstStatus.Items.Add("Please refer to the log file for further information.")
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			Select Case Information.Err().Number
				Case 52  ' Bad path - Probably due to machine not existing in the database record
					LogFile(v_sLogFile:=m_sLogFile, v_sText:="Failed to check " & sPath & sFilename & ". Unable to access path or machine.")


				Case Else
					LogMessageShort(v_sMessage:=excep.Message, v_sTitle:="Error: " & Information.Err().Number)
			End Select
			
			Return gPMConstants.PMEReturnCode.PMError
			
		End Try
	End Function
	
	 ' ##########################################################################
	 ' Method: CallHelp
	 ' Desc:   Use CommonDialog to display help file
	 ' History:
	 '     RDC 25082004 created
	 ' ##########################################################################
	Private Function CallHelp() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			

             'dlgHelp.HelpFile = My.Application.Info.DirectoryPath & "\Help\DMEH.chm"
            System.Diagnostics.Process.Start(My.Application.Info.DirectoryPath & "\Help\DMEH.chm")

             'dlgHelp.HelpCommand = cdlHelpContents

             'dlgHelp.ShowHelp()
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	 ' ##########################################################################
	 ' Method: RunScript
	 ' Desc:   Run a SQL script using iSQL
	 ' History:
	 '
	 ' ##########################################################################
	Private Function RunScript(ByVal v_sScriptFile As String) As Integer
		
		Dim result As Integer = 0
		Dim sScriptName, sOutputFile, sFullScript, sCommand As String
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			sFullScript = m_sScriptPath & v_sScriptFile
			
			 ' Check if the script exists
			If FileSystem.Dir(sFullScript, FileAttribute.Normal) = "" Then
				MessageBox.Show("Unable to find the script file : " & Environment.NewLine & sFullScript, APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				Return result
			End If
			
			If txtUsername.Text.Trim() = "" Then
				MessageBox.Show("You must enter a username on the Advanced tab", APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				SSTabHelper.SetSelectedIndex(tabMain, TAB_ADVANCED)
				txtUsername.Focus()
				Return result
			End If
			
			 ' script filename without the path and suffix
			sScriptName = Mid(v_sScriptFile, (IIf(v_sScriptFile = "" And "\" = "", 0, (v_sScriptFile.LastIndexOf("\") + 1))) + 1)
			sScriptName = sScriptName.Substring(0, sScriptName.IndexOf("."c))
			
			 ' script output file
			sOutputFile = txtOutputFile.Text.Replace("{scriptname}", sScriptName)
			
			sCommand = ACSQLCommand
			
			sCommand = sCommand.Replace("{username}", txtUsername.Text)
			sCommand = sCommand.Replace("{password}", txtPassword.Text)
			sCommand = sCommand.Replace("{server}", cboServer.Text)
			sCommand = sCommand.Replace("{filename}", Strings.Chr(34).ToString() & sFullScript & Strings.Chr(34).ToString())
			sCommand = sCommand.Replace("{database}", IIf(m_bMerged, cboDatabase.Text, "DocuMaster"))
			sCommand = sCommand.Replace("{outputfile}", sOutputFile)
			
			 ' Call and wait...
			ShellWait(v_sCommandLine:=sCommand)
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	 ' ##########################################################################
	 ' Method: RunHarmoniser
	 ' Desc:   Runs all check & repair processes
	 ' History:
	 '
	 ' ##########################################################################
	Private Function RunHarmoniser() As Integer
		
		Dim result As Integer = 0
		Dim sMsg As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			sMsg = "Options:" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)
			sMsg = sMsg & "SQL Server: " & cboServer.Text & Strings.Chr(13) & Strings.Chr(10)
			sMsg = sMsg & "SQL Database: " & cboDatabase.Text & Strings.Chr(13) & Strings.Chr(10)
			sMsg = sMsg & "SQL Username: " & txtUsername.Text & Strings.Chr(13) & Strings.Chr(10)
			sMsg = sMsg & "Sirius Version: " & cboVersion.Text & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)
			sMsg = sMsg & "Log files: " & txtLogFile.Text & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)
			sMsg = sMsg & "Run Harmoniser?"
			
			m_lReturn = MessageBox.Show(sMsg, APP_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
			
			If m_lReturn <> System.Windows.Forms.DialogResult.OK Then
				Return result
			End If
			
			Me.Cursor = Cursors.WaitCursor
			
			ButtonSwitch(bMode:=False)
			
			m_sLogFile = txtLogFile.Text
			
			m_lReturn = gPMConstants.PMEReturnCode.PMTrue
			
			Select Case cboVersion.SelectedIndex
				Case 0  ' auto
					m_lReturn = CheckVersion()
				Case 1  ' 16
					m_bMerged = False
				Case 2
					m_bMerged = True
				Case Else  ' auto
					m_lReturn = CheckVersion()
			End Select
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Me.Cursor = Cursors.Default
				
				ButtonSwitch(bMode:=True)
				
				MessageBox.Show("Failed to auto detect version of DME installed", APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error)
				Return result
			End If
			
			If chkFiles.CheckState = CheckState.Checked Then
				lstStatus.Items.Add("Missing files: Running")
				Application.DoEvents()
				m_lReturn = RunFileCheck()
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					lstStatus.Items.Add("Failed to check missing files")
					ButtonSwitch(bMode:=True)
					Return result
				End If
				lstStatus.Items.Add("Missing files: Done")
			End If
			
			If chkDB.CheckState = CheckState.Checked Then
				
				If chkScript1.CheckState = CheckState.Checked Then
					lstStatus.Items.Add("Running script (" & chkScript1.Text & ")")
					m_lReturn = RunScript(ACScript1)
				End If
				
				If chkScript2.CheckState = CheckState.Checked Then
					lstStatus.Items.Add("Running script (" & chkScript2.Text & ")")
					m_lReturn = RunScript(ACScript2)
				End If
				
				If chkScript3.CheckState = CheckState.Checked Then
					lstStatus.Items.Add("Running script (" & chkScript3.Text & ")")
					m_lReturn = RunScript(ACScript3)
				End If
				
				If chkScript4.CheckState = CheckState.Checked Then
					lstStatus.Items.Add("Running script (" & chkScript4.Text & ")")
					m_lReturn = RunScript(ACScript4)
				End If
				
			End If
			
			 ' Select the last one
			ListBoxHelper.SetSelected(lstStatus, lstStatus.Items.Count - 1, True)
			
			ButtonSwitch(bMode:=True)
			
			Me.Cursor = Cursors.Default
			
			m_lReturn = MessageBox.Show("DME Harmoniser run complete" & Environment.NewLine & Environment.NewLine & "Do you wish to view the log file?", APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Information)
			
			If m_lReturn = System.Windows.Forms.DialogResult.Yes Then

				Process.Start("notepad " & txtLogFile.Text)
			End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			 ' pointless logging an error so just display a simple error message
			MessageBox.Show("DME Harmoniser was unable to process" & Environment.NewLine & Environment.NewLine & excep.Message, APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information)
			
			Return result
		End Try
	End Function
	
	 ' ##########################################################################
	 ' Method: PopulateServers
	 ' Desc:   Get list of SQL Servers
	 ' History:
	 '     RDC 26082004 created
	 ' ##########################################################################
	Private Sub PopulateServers()
		
		Dim oDMO As SQLDMO.Application  'SQLDMO.Application
		Dim oList As SQLDMO.NameList
		
		Try 
			
			Me.Cursor = Cursors.WaitCursor
			
			oDMO = New SQLDMO.Application()
			
			oList = oDMO.ListAvailableSQLServers()
			
			cboServer.Items.Clear()
			
			For iLoop As Integer = 1 To oList.Count
				cboServer.Items.Add(oList.Item(iLoop))
			Next 
			
			oList = Nothing
			oDMO = Nothing
			
			 ' from server registry
			cboServer.Text = m_sServer
			
			Me.Cursor = Cursors.Default
		
		Catch excep As System.Exception
			
			
			
			Me.Cursor = Cursors.Default
			
			lstStatus.Items.Add(CStr(Information.Err().Number) & " - " & excep.Message)
			LogFile(v_sLogFile:=m_sLogFile, v_sText:=CStr(Information.Err().Number) & " - " & excep.Message)
			
		End Try
		
	End Sub
	
	 ' ##########################################################################
	 ' Method: PopulateDatabases
	 ' Desc:   Get list of databases on server (cboServer.text)
	 ' History:
	 '     RDC 26082004 created
	 ' ##########################################################################
	Private Sub PopulateDatabases()
		
		Dim oServer As SQLDMO.SQLServer  ' SQLDMO.SQLServer
		
		Try 
			
			Me.Cursor = Cursors.WaitCursor
			
			oServer = New SQLDMO.SQLServer()
			
			oServer.LoginTimeout = -1  ' ODBC Default
			
			 'Connect to the Server
			If chkWindowsAuth.CheckState = CheckState.Checked Then
				With oServer
					.LoginSecure = True
					.AutoReConnect = False
					.Connect(cboServer.Text)
				End With
			Else
				With oServer
					.LoginSecure = False
					.AutoReConnect = False
					.Connect(cboServer.Text, txtUsername.Text, txtPassword.Text)
				End With
			End If
			
			cboDatabase.Items.Clear()
			
			For iLoop As Integer = 1 To oServer.Databases.Count
				cboDatabase.Items.Add(oServer.Databases.Item(iLoop).Name)
			Next 
			
			 ' from server registry
			cboDatabase.Text = m_sDatabase
			
			oServer.DisConnect()
			
			oServer = Nothing
			
			Me.Cursor = Cursors.Default
		
		Catch excep As System.Exception
			
			
			
			Me.Cursor = Cursors.Default
			
			lstStatus.Items.Add(CStr(Information.Err().Number) & " - " & excep.Message)
			LogFile(v_sLogFile:=m_sLogFile, v_sText:=CStr(Information.Err().Number) & " - " & excep.Message)
			
		End Try
		
	End Sub
	
	 ' ##########################################################################
	 ' Method: ActivateForm
	 ' Desc:   triggered by form_activate event
	 ' History:
	 '     RDC 26082004 created
	 ' ##########################################################################
	Private Function ActivateForm() As Integer
		
		Dim result As Integer = 0
		Static bActivated As Boolean
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			If bActivated Then
				Return gPMConstants.PMEReturnCode.PMTrue
			End If
			
			tabMain.Focus()
			
			bActivated = True
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	 ' ##########################################################################
	 ' Method: LoadForm
	 ' Desc:   triggered by form_load event
	 ' History:
	 '     RDC 26082004 created
	 ' ##########################################################################
	Private Function LoadForm() As Integer
		
		Dim result As Integer = 0
		Dim sngTime As Single
		Dim oSplash As frmSplash
		
		Const MIN_DISPLAY_TIME As Integer = 4
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			sngTime = DateTime.Now.TimeOfDay.TotalSeconds
			
			oSplash = New frmSplash()
			

             'Load(oSplash)

			
			oSplash.Show()
			
			Application.DoEvents()
			
			Me.Text = APP_TITLE
			
			 ' default settings on both tabs
			oSplash.lblMsg.Text = "Loading defaults ..."
			Application.DoEvents()
			
			m_lReturn = DefaultAdvancedSettings()
			m_lReturn = DefaultMainSettings()
			
			 ' hide advanced tab and disable controls
			 '    tabMain.TabVisible(TAB_ADVANCED) = False
			 '    fmeAdvanced.Enabled = False
			
			 ' select main tab
			SSTabHelper.SetSelectedIndex(tabMain, TAB_MAIN)
			
			oSplash.lblMsg.Text = "Initialisation complete"
			Application.DoEvents()
			
			Do While DateTime.Now.TimeOfDay.TotalSeconds < sngTime + MIN_DISPLAY_TIME
				Application.DoEvents()
			Loop 
			
			oSplash.Close()
			
			oSplash = Nothing
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	 ' ##########################################################################
	 ' Method: UnloadForm
	 ' Desc:   triggered by form_unload
	 ' History:
	 '     RDC 26082004 created
	 ' ##########################################################################
	Private Function UnloadForm() As Integer
		
		Dim result As Integer = 0
		Dim sngTime As Single
		Dim oSplash As frmSplash
		
		Const MIN_DISPLAY_TIME As Double = 1.5
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			Me.Hide()
			
			sngTime = DateTime.Now.TimeOfDay.TotalSeconds
			
			oSplash = New frmSplash()
			

             'Load(oSplash)
			
			oSplash.Show()
			
			oSplash.lblMsg.Text = "Shutting down"
			Application.DoEvents()
			
			Do While DateTime.Now.TimeOfDay.TotalSeconds < sngTime + MIN_DISPLAY_TIME
				Application.DoEvents()
			Loop 
			
			oSplash.Close()
			
			oSplash = Nothing
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
End Class
