Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmDMSMain
	Inherits System.Windows.Forms.Form
	Private Sub frmDMSMain_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	' Edit History
	'
	' SP170898 - Add help about component
	'
	'
	'
	
	Dim f_iMinuteCounter As Integer
	Dim f_iCurrentTimerInterval As Integer
	
	Dim f_iLogoff As Integer
	
	'Added for Sirius Compliance
	Dim m_lReturn As Integer
	
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		f_iMinuteCounter = 1
		Me.WindowState = FormWindowState.Minimized
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
		Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
		Dim eProductFamily As gPMConstants.PMEProductFamily
		
		
		Try 
			
			If Conversion.Val(txtTimerInterval.Text) > 59 Or Conversion.Val(txtTimerInterval.Text) < 1 Then
				Interaction.MsgBox("Process interval must be between 1-59 minutes", MB_ICONINFORMATION, "Journal Processor")
				txtTimerInterval.Text = CStr(1)
			Else
				f_iCurrentTimerInterval = CInt(txtTimerInterval.Text)
				f_iMinuteCounter = 1
				Me.WindowState = FormWindowState.Minimized
				
				' Save the timer interval to the registry file
				eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
				eProductFamily = gPMConstants.PMEProductFamily.pmePFDocumaster
				eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLCommon
				
				m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCTimerIntervalKey, v_sSettingValue:=txtTimerInterval.Text, v_sSubKey:=DOCDaemonSection)
				
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					MessageBox.Show("ERROR: Failed to update 'TimerInterval' in registry. Error logged.", Application.ProductName)
				End If
				
			End If
		
		Catch 
			
			
			
			txtTimerInterval.Text = "1"


		End Try
		
		
	End Sub
	
	Private Sub cmdRunNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRunNow.Click
		
		Me.WindowState = FormWindowState.Minimized
		
		Application.DoEvents()
		
		f_iMinuteCounter = f_iCurrentTimerInterval
		tmrDMSAPI_Tick(tmrDMSAPI, New EventArgs())
		
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		Dim bDOCPMBLog As Object
		
		Try 
			
			'This all stuffed in so daemon is at least vaguely sirius compliant
			
			' Create an instance of the object manager.
#If PD_EARLYBOUND = 1 Then

			Set g_oObjectManager = New bObjectManager.ObjectManager
#Else
			g_oObjectManager = New bObjectManager.ObjectManager()
#End If
			
			' Call the initialise method.
			m_lReturn = g_oObjectManager.Initialise(sCallingAppName:="iDOCPMBAPI")
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to call the initialise method.
				
				' Set the object manager to nothing.
				g_oObjectManager = Nothing
				
				' Log Error.
				MessageBox.Show("Failed to initialise object manager", Application.ProductName)
				
				Environment.Exit(0)
			End If
			
			' Store the language ID from the object manager
			' to the public variables, to enable us to use
			' them throughout the object.
			With g_oObjectManager
				g_iLanguageID = .LanguageID
				g_iSourceID = .SourceID
				g_sUsername = .UserName
			End With
			
			' Get an instance of the business object via
			' the public object manager.
			Dim temp_g_oBusiness As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bDOCPMBAPI.Form", vInstanceManager:="ClientManager")
			g_oBusiness = temp_g_oBusiness
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Failed to get business via object manager", Application.ProductName)
				Environment.Exit(0)
			End If
			
			'Get instance of bDOCPMBLog
			g_oPMBLog = New bDOCPMBLog.Log()
			

			m_lReturn = g_oPMBLog.Initialise(sUsername:="DMSAPI")
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Failed to initialise DOCPMBLog", Application.ProductName)
				Environment.Exit(0)
			End If
		
		Catch excep As System.Exception
			
			
			
			MessageBox.Show("Error " & Information.Err().Number & ":" & excep.Message, "Form_Initialize")
			Environment.Exit(0)
			
		End Try
		
		
	End Sub
	

	Private Sub frmDMSMain_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Dim sLogMess(1) As String
		Dim iDataLen As Integer
		Dim sTmp1, sTmp2 As String
		
		'salvo(4/7/96) This is now declared in a generic constants file
		VERSIONNUMBER = "2.00.00"
		
		'who am I ???
		g_sAppType.Value = "APIT"
		
		g_iRunning = False
		
		Application.DoEvents()
		
		f_iMinuteCounter = 1
		
		' Get the timer interval and history root from the registry
		m_lReturn = GetDOCRegSettings(vTimerInterval:=sTmp1, vHistoryRoot:=sTmp2)
		
		Select Case m_lReturn
			Case gPMConstants.PMEReturnCode.PMTrue
				'how nice
				
			Case gPMConstants.PMEReturnCode.PMCancel
				'whats the point
				Environment.Exit(0)
				
			Case Else
				'oh dear
				MessageBox.Show("Failed in GetDOCRegSettings.", Application.ProductName)
				Environment.Exit(0)
				
		End Select
		
		f_iCurrentTimerInterval = CInt(sTmp1)
		g_sHistoryRoot = sTmp2
		
		' Append a log message to todays log file stating
		' that the API has started
		

		m_lReturn = g_oPMBLog.OpenLogFile(g_sHistoryRoot)
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			' Failed to log message
			ErrorLog("Form Load", "Failed to open log file")
		End If
		
		sLogMess(1) = "DocuMaster API Daemon Started"

		g_oPMBLog.DOCLogMessage(LLOG, "DMSAPI", sLogMess)
		

		g_oPMBLog.CloseLogFile(g_sHistoryRoot)
		
		f_iLogoff = False
		
		tmrDMSAPI.Enabled = True
		
	End Sub
	
	Private Sub frmDMSMain_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		If ShutMeDown() Then
			Cancel = 0 'shut down
			
			' Check if we have an instance of the Object Manager.
			If Not (g_oObjectManager Is Nothing) Then
				
				' Call the terminate method.
                g_oObjectManager.Dispose()
                ' Destroy the instance of the object manager
				' from memory.
                g_oObjectManager = Nothing
				
				' Terminate the business object

                g_oBusiness.Dispose()
				' Destroy the instance of the business object
                ' from memory.
                g_oBusiness = Nothing
				
			End If
			
		Else
			Cancel = 1 'stay alive
		End If
		
		eventArgs.Cancel = Cancel <> 0
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub frmDMSMain_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		
		If Me.WindowState <> FormWindowState.Minimized Then
			txtTimerInterval.Text = CStr(f_iCurrentTimerInterval)
		End If
		
	End Sub
	
	Private Sub frmDMSMain_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		
		Environment.Exit(0)
		
	End Sub
	
	Private Sub GetMeOutOfHere()
		
		Dim sLogMess(1) As String
		
		' Append a log message to todays log file stating
		' that the API has finished

		m_lReturn = g_oPMBLog.OpenLogFile(g_sHistoryRoot)
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			' Failed to log message
			MessageBox.Show("Failed to open log file", "GetMeOutOfHere")
		End If
		
		sLogMess(1) = "DocuMaster API Daemon Ended"

		g_oPMBLog.DOCLogMessage(LLOG, "DMSAPI", sLogMess)
		

		g_oPMBLog.CloseLogFile(g_sHistoryRoot)
		
	End Sub
	
	Public Sub mnuAccelerateAPI_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuAccelerateAPI.Click
		
		'User should not be able to select this willy-nilly
		If Not mnuAccelerateAPI.Checked Then
			
			m_lReturn = MessageBox.Show("This option should only be chosen under the " &  _
			            "guidance of Policy Master staff." & Strings.Chr(10).ToString() &  _
			            "Are you SURE you wish to select acceleration ?", "Accelerate API", MessageBoxButtons.YesNo)
			
			mnuAccelerateAPI.Checked = m_lReturn = System.Windows.Forms.DialogResult.Yes
		Else
			mnuAccelerateAPI.Checked = False
		End If
		
	End Sub
	
	Public Sub mnuErrorsRetryExport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuErrorsRetryExport.Click
		
		mnuErrorsRetryExport.Checked = Not (mnuErrorsRetryExport.Checked)
		
	End Sub
	
	Public Sub mnuErrorsRetryImport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuErrorsRetryImport.Click
		
		mnuErrorsRetryImport.Checked = Not (mnuErrorsRetryImport.Checked)
		
	End Sub
	
	Public Sub mnuFileExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileExit.Click
		
		Me.Close()
		
	End Sub
	
	Public Sub mnuFileOptions_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileOptions.Click
		
		Dim sMsg, sTmp1, sTmp2 As String
		
		Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
		Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
		Dim eProductFamily As gPMConstants.PMEProductFamily
		
		Try 
			
			'Allow user to change settings
			
			'JH260399 must check history root first as it might
			'have changed re: multiple history roots
			
			eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
			eProductFamily = gPMConstants.PMEProductFamily.pmePFDocumaster
			eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLCommon
			
			m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCHistoryRootKey, r_sSettingValue:=sTmp2, v_sSubKey:=DOCDaemonSection)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'error must have occurred
				Exit Sub
			End If
			
			sTmp1 = txtTimerInterval.Text
			
			If sTmp2 = "" Then
				sTmp2 = g_sHistoryRoot
			End If
			
			Static tt As String = ""

			m_lReturn = CInt(ChangeDOCRegSettings(vTimerInterval:=sTmp1, vHistoryRoot:=sTmp2))
			
			Select Case m_lReturn
				Case gPMConstants.PMEReturnCode.PMTrue
					'fine
				Case gPMConstants.PMEReturnCode.PMCancel
					'go
					Exit Sub
				Case Else
					'failed
					MessageBox.Show("Failed in ChangeDOCRegSettings", "mnuFileOptions_Click")
					Exit Sub
			End Select
			
			'Use new settings
			txtTimerInterval.Text = sTmp1
			f_iCurrentTimerInterval = CInt(txtTimerInterval.Text)
			f_iMinuteCounter = 1
			
			'close and reopen log file in case history root changed

			g_oPMBLog.CloseLogFile(g_sHistoryRoot)
			
			g_sHistoryRoot = sTmp2
			

			m_lReturn = g_oPMBLog.OpenLogFile(g_sHistoryRoot)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Interaction.MsgBox("Failed to Open New LogFile - Reverting to previous History Root.", "mnuFileOptions_Click")
				Exit Sub
			End If
		
		Catch 
			
			
			

            MessageBox.Show("Error: " & Information.Err().Number & " - " & Err.Description, "mnuFileOptions_Click")
			
			Exit Sub
		End Try
		
		
	End Sub
	
	
	' **********************************************************************
	'
	' Function    : mnuHelpAbout
	'
	' Description : Displays the standard Policy Master about screen modally
	'
	' SP170898 - Add help about component
	' **********************************************************************
	Public Sub mnuHelpAbout_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpAbout.Click
		
		' Code needed to display standard PM About screen
        'developer guide no.108
        Dim oSirAbout As iPMAbout.Interface_Renamed
		
		Dim sTitle, sVersionNumber, sVersionDate, sComponent As String
		
		
		Try 
			
			' Set the application title
			sTitle = "DocuMaster Enterprise"
			
			' Set the version number and date
			sVersionNumber = CStr(My.Application.Info.Version.Major) & "." & CStr(My.Application.Info.Version.Minor) & "." & CStr(My.Application.Info.Version.Revision)

			sVersionDate = DateTimeHelper.ToString((New FileInfo(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe")).LastWriteTime)

			sComponent = My.Application.Info.AssemblyName
			
            ' Create the object
            'developer guide no.108
            oSirAbout = New iPMAbout.Interface_Renamed()
			
			' Initialise it. No parameters
			m_lReturn = CType(oSirAbout, SSP.S4I.Interfaces.ILocalInterface).Initialise()
			
			' Display the about screen modally
			m_lReturn = oSirAbout.Show(sTitle:=sTitle, sVersionNumber:=sVersionNumber, sVersionDate:=sVersionDate, sComponent:=sComponent)
			
			' Terminate it, and...
            oSirAbout.Dispose()
			
            ' ...remove it from memory
            oSirAbout = Nothing
		
		Catch excep As System.Exception
			
			
			
			MessageBox.Show("Error : " & Information.Err().Number & " - " & excep.Message, Application.ProductName)
			'    LogMessage _
			''        iType:=PMLogError, _
			''        sMsg:=PMErrorText, _
			''        vApp:=ACApp, _
			''        vClass:=ACClass, _
			''        vMethod:="mnuHelpAbout_Click", _
			''        vErrNo:=Err.Number, _
			''        vErrDesc:=Err.description
			
			Exit Sub
			
			
			
		End Try
		
	End Sub
	
	Public Sub mnuLogsPurge_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuLogsPurge.Click
		
		frmPurgeLogs.ShowDialog()
		
	End Sub
	
	Public Sub mnuLogsView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuLogsView.Click
		
		frmViewLogFiles.ShowDialog()
		
	End Sub
	
	Private Function ShutMeDown() As Integer
		
		Dim result As Integer = 0
		Dim sMsg As String = ""
		
		' form first
		If g_iRunning Then
			sMsg = "The Daemon is currently processing data." & Strings.Chr(10).ToString()
			sMsg = sMsg & "It is highly recommended you cancel the Daemon first." & Strings.Chr(10).ToString() & Strings.Chr(10).ToString()
			sMsg = sMsg & "ARE YOU SURE YOU WANT TO SHUT DOWN?"
			
			If Interaction.MsgBox(sMsg, MB_ICONSTOP + MB_YESNO + MB_DEFBUTTON2, "Shut Down API Timer?") = System.Windows.Forms.DialogResult.No Then
				result = False 'do not close down
			Else
				result = True 'close down
				GetMeOutOfHere()
			End If
		Else
			result = True 'close down
			GetMeOutOfHere()
		End If
		
		Return result
	End Function
	
	
	
	Private Sub tmrDMSAPI_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tmrDMSAPI.Tick
		
		Dim sLogMess(1) As String
		Dim iInstance As Integer
		Dim iHandle As Integer
		Dim sComStr As String = ""
		Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
		Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
		Dim eProductFamily As gPMConstants.PMEProductFamily
		Dim sTmp As String = ""
		
		
		Try 
			
			If f_iLogoff Then
				' I've been killed off, sorry
				Me.Close()
			End If
			
			'turn timer off while proccessing
			tmrDMSAPI.Enabled = False
			
			If f_iMinuteCounter < f_iCurrentTimerInterval Then
				f_iMinuteCounter += 1
			Else
				g_iRunning = True
				cmdRunNow.Enabled = False
				
				f_iMinuteCounter = 1
				
				' Append a log message to todays log file,
				' saying that my little timer has kicked in!
				

				m_lReturn = g_oPMBLog.OpenLogFile(g_sHistoryRoot)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					MessageBox.Show("Failed to Open LogFile", Application.ProductName)
					Exit Sub
				End If
				
				sLogMess(1) = "DocuMaster API Daemon - Timer interval"

				g_oPMBLog.DOCLogMessage(LLOG, "DMSAPI", sLogMess)
				

				g_oPMBLog.CloseLogFile(g_sHistoryRoot)
				
				Application.DoEvents()
				
				If mnuAccelerateAPI.Checked Then
					
					m_lReturn = MessageBox.Show("You have chosen to accelerate this API run." & Strings.Chr(10).ToString() &  _
					            "This should only be selected for a system option 40 run." &  _
					            Strings.Chr(10).ToString() & "Would you like to continue this run with acceleration on ?", "Verify Acceleration", MessageBoxButtons.YesNo)
					
					If m_lReturn = System.Windows.Forms.DialogResult.No Then
						mnuAccelerateAPI.Checked = False
					End If
					
				End If
				
				'Check if business is already running, in case user has loaded
				'multiple timers
				eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
				eProductFamily = gPMConstants.PMEProductFamily.pmePFDocumaster
				eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLCommon
				
				m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCRunningKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCDaemonSection)
				
				
				If sTmp = "Y" Then
					
					MessageBox.Show("The daemon is already processing data." & Strings.Chr(10).ToString() &  _
					                "Please ensure you do not have multiple API Timers loaded.", "DocuMaster API Timer")
					
				Else
					
					'Indicate in registry that API is running
					m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCRunningKey, v_sSettingValue:="Y", v_sSubKey:=DOCDaemonSection)
					
					'Start the PMBAPI doing its work

                    g_oBusiness.Start(g_oPMBLog, mnuAccelerateAPI.Checked, mnuErrorsRetryImport.Checked, mnuErrorsRetryExport.Checked)
					
					Application.DoEvents()
					
					'Indicate in registry that API is not running
					m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCRunningKey, v_sSettingValue:="N", v_sSubKey:=DOCDaemonSection)
					
					'remove acceleration
					mnuAccelerateAPI.Checked = False
					
					mnuErrorsRetryImport.Checked = False
					mnuErrorsRetryExport.Checked = False
					
				End If
				
				g_iRunning = False
				cmdRunNow.Enabled = True
				
			End If
			
			If f_iLogoff Then
				' I've been killed off, sorry
				Me.Close()
			End If
			
			Application.DoEvents()
			
			'turn timer back on when proccessing is finished...
			tmrDMSAPI.Enabled = True
		
		Catch 
			
			
			
			MessageBox.Show("Error :" & Information.Err().Number & " " & Conversion.ErrorToString() & Strings.Chr(10).ToString(), "tmrDMSAPI_Timer")
			Exit Sub
		End Try
		
		
	End Sub
	
	Private Sub txtTimerInterval_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTimerInterval.Enter
		
		txtTimerInterval.SelectionStart = 0
		txtTimerInterval.SelectionLength = Strings.Len(txtTimerInterval.Text)
		
	End Sub
End Class