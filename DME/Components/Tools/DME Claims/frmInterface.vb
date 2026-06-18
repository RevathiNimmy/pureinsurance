Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	Private m_sDBConn As String = ""
	
	Private m_oConn As SqlConnection
	Private m_oCmd As SqlCommand
	
	Private Sub cmdExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdProcess_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdProcess.Click
		
		m_lReturn = OpenDatabase()
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			' failed to open database
			StatusMsg("Failed to open the database with the supplied parameters", True)
			StatusMsg("Please check values and try again")
			StatusMsg("If values are correct, please ensure that the database server is available and visible to this machine")
			
			Exit Sub
		End If
		
		cmdProcess.Enabled = False
		cmdSave.Enabled = False
		cmdExit.Enabled = False
		
		m_lReturn = ProcessClaimsFolders()
		
		cmdProcess.Enabled = True
		cmdSave.Enabled = True
		cmdExit.Enabled = True
		
		m_lReturn = CloseDatabase()
		
	End Sub
	
	Private Sub cmdSave_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSave.Click
		
		Dim lFH As Integer
        Dim sFilename As String
		
		Try 
			

            'TODO
            'CommonDialog1.CancelError = True
			
			'UPGRADE_NOTE: (6010) Variable CommonDialog1 was renamed CommonDialog1Open. More Information: http://www.vbtonet.com/ewis/ewi6010.aspx
			CommonDialog1Open.FileName = "dme_claims_fix.log"

            'TODO
            'CommonDialog1.Action = 2
			
			'UPGRADE_NOTE: (6010) Variable CommonDialog1 was renamed CommonDialog1Open. More Information: http://www.vbtonet.com/ewis/ewi6010.aspx
			sFilename = CommonDialog1Open.FileName
			
			lFH = FileSystem.FreeFile()
			
			FileSystem.FileOpen(lFH, sFilename, OpenMode.Output)
			FileSystem.PrintLine(1, txtMsg.Text)
			FileSystem.FileClose(1)
		
		Catch excep As System.Exception
			
			
			

			If Information.Err().Number = DialogResult.Cancel Then
				Exit Sub
			End If
			
			MessageBox.Show(CStr(Information.Err().Number) & " - " & excep.Message, ACAPP, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
			
		End Try
		
	End Sub
	

	Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		m_lReturn = GetDatabaseSettings()
		
	End Sub
	
	Private Function GetDatabaseSettings() As Integer
		
		Dim result As Integer = 0
		Dim sServer, sDatabase, sProvider As String
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			m_lReturn = CType(GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSubKey:="\Databases\SiriusSolutions", v_sSettingName:="Server", r_sSettingValue:=sServer), gPMConstants.PMEReturnCode)
			
			m_lReturn = CType(GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSubKey:="\Databases\SiriusSolutions", v_sSettingName:="Database", r_sSettingValue:=sDatabase), gPMConstants.PMEReturnCode)
			
			m_lReturn = CType(GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSubKey:="\Databases\SiriusSolutions", v_sSettingName:="Provider", r_sSettingValue:=sProvider), gPMConstants.PMEReturnCode)
			
			' not interested in return codes. User can fill in values if not in registry
			
			txtProvider.Text = "SQLOLEDB"
			txtUsername.Text = "sa"
			txtPassword.Text = ""
			
			If sServer <> "" Then
				txtServer.Text = sServer
			End If
			
			If sDatabase <> "" Then
				txtDatabase.Text = sDatabase
			End If
			
			If sProvider <> "" Then
				txtProvider.Text = sProvider
			End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	Private Function OpenDatabase() As Integer
		
		Dim result As Integer = 0
		Dim sConnString As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			StatusMsg("Opening database connection", False)
			
			' build the connection string from the form values
			sConnString = ""
			sConnString = sConnString & "Provider=" & txtProvider.Text & ";"
			sConnString = sConnString & "Data Source=" & txtServer.Text & ";"
			sConnString = sConnString & "Initial Catalog=" & txtDatabase.Text & ";"
			sConnString = sConnString & "User ID=" & txtUsername.Text & ";"
			sConnString = sConnString & "Password=" & txtPassword.Text & ";"
			
			' create connection and open database
#If PD_EARLYBOUND = 1 Then

			Set m_oConn = New ADODB.Connection
#Else
			m_oConn = New SqlConnection()
#End If
			
			m_oConn.ConnectionString = sConnString

            'm_oConn.CommandTimeout = 60

			m_oConn.Open()
			
			' create a command object
#If PD_EARLYBOUND = 1 Then

			Set m_oConn = New ADODB.Connection
			Set m_oCmd = New ADODB.Command
#Else
			m_oConn = New SqlConnection()
			m_oCmd = New SqlCommand()
#End If
			
			m_oCmd.Connection = m_oConn
            m_oCmd.CommandTimeout = 60
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			StatusMsg(CStr(Information.Err().Number) & " - " & excep.Message)
			
			Return result
		End Try
	End Function
	
	Private Function CloseDatabase() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' close the command and connection objects
			m_oCmd = Nothing
			
			m_oConn.Close()
			
			m_oConn = Nothing
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			StatusMsg(CStr(Information.Err().Number) & " - " & excep.Message)
			
			Return result
		End Try
	End Function
	
	Private Sub StatusMsg(ByVal sMsg As String, Optional ByVal bTime As Boolean = False)
		
		If bTime Then
			txtMsg.Text = txtMsg.Text & DateTime.Now.ToString("HH:mm:ss") & " " & sMsg & Strings.Chr(13) & Strings.Chr(10)
		Else
			txtMsg.Text = txtMsg.Text & sMsg & Strings.Chr(13) & Strings.Chr(10)
		End If
		
		txtMsg.SelectionStart = Strings.Len(txtMsg.Text)
		
		Application.DoEvents()
		
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		
		' move exit and save buttons, resize tab control and status msg box
		cmdExit.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - VB6.PixelsToTwipsX(cmdExit.Width) - 120)
		cmdExit.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6.PixelsToTwipsY(cmdExit.Height) - 360)
		cmdSave.Top = cmdExit.Top
		
		tabMain.Width = Me.ClientRectangle.Width - VB6.TwipsToPixelsX(240)
		tabMain.Height = Me.ClientRectangle.Height - VB6.TwipsToPixelsY(960)
		
		txtMsg.Width = tabMain.Width - VB6.TwipsToPixelsX(240)
		txtMsg.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(tabMain.Height) - VB6.PixelsToTwipsY(txtMsg.Top) - 120)
		
	End Sub
	
	Private Function ProcessClaimsFolders() As Integer
		
		Dim result As Integer = 0
		Dim lCount As Integer
		
		Dim sSQL As String = ""
		
		Dim sSourceFolderName, sSourceFolderNum, sSourceParentNum, sSourceExCode As String
		
		Dim sTargetFolderName, sTargetFolderNum As String
		
		Dim oSource, oTarget As DataSet
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			txtMsg.Text = ""
			
			StatusMsg("Processing started", True)
			
			' count claims folders where folder name starts with C00
			sSQL = "SELECT COUNT(folder_num) as row_count "
			sSQL = sSQL & "FROM doc_folder "
			sSQL = sSQL & "WHERE LTRIM(folder_name) LIKE 'c00%' "
			sSQL = sSQL & "  AND ex_code LIKE 'c%'"
			sSQL = sSQL & "  AND folder_level = 2"
			
			Dim TempCommand As SqlCommand
			TempCommand = m_oConn.CreateCommand()
			TempCommand.CommandText = sSQL
			Dim TempAdapter As SqlDataAdapter
			TempAdapter = New SqlDataAdapter()
			TempAdapter.SelectCommand = TempCommand
			Dim TempDataset As DataSet
			TempDataset = New DataSet()
			TempAdapter.Fill(TempDataset)
			oSource = TempDataset
			

			lCount = oSource.Tables(0).Rows(0)("row_count")
			
			' get claims folders where folder name starts with C00
			sSQL = "SELECT folder_num, folder_name, parent_num, ex_code "
			sSQL = sSQL & "FROM doc_folder "
			sSQL = sSQL & "WHERE LTRIM(folder_name) LIKE 'c00%' "
			sSQL = sSQL & "  AND ex_code LIKE 'c%'"
			sSQL = sSQL & "  AND folder_level = 2"
			
			Dim TempCommand_2 As SqlCommand
			TempCommand_2 = m_oConn.CreateCommand()
			TempCommand_2.CommandText = sSQL
			Dim TempAdapter_2 As SqlDataAdapter
			TempAdapter_2 = New SqlDataAdapter()
			TempAdapter_2.SelectCommand = TempCommand_2
			Dim TempDataset_2 As DataSet
			TempDataset_2 = New DataSet()
			TempAdapter_2.Fill(TempDataset_2)
			oSource = TempDataset_2
			
			If oSource Is Nothing Then
				Return result
			End If
			
			' check each one and see if there's a related folder without the C prefix
			For	Each iteration_row As DataRow In oSource.Tables(0).Rows
				
				stbStatus.Items.Item(0).Text = CStr(lCount) & " folders to process"
				
				lCount -= 1
				
				sSourceFolderName = iteration_row("folder_name")
				sSourceFolderNum = iteration_row("folder_num")
				sSourceParentNum = iteration_row("parent_num")
				sSourceExCode = iteration_row("ex_code")
				
				'proper' claims folder is format 9999999
				sTargetFolderName = "'" & Mid(sSourceFolderName, 4, 7) & "%'"
				
				sSQL = "SELECT folder_num, folder_name, parent_num, ex_code "
				sSQL = sSQL & "FROM doc_folder "
				sSQL = sSQL & "WHERE folder_name LIKE " & sTargetFolderName
				sSQL = sSQL & "  AND parent_num = " & sSourceParentNum
				sSQL = sSQL & "  AND RTRIM(ex_code) = '" & sSourceExCode & "'"
				sSQL = sSQL & "  AND folder_level = 2"
				
				Dim TempCommand_3 As SqlCommand
				TempCommand_3 = m_oConn.CreateCommand()
				TempCommand_3.CommandText = sSQL
				Dim TempAdapter_3 As SqlDataAdapter
				TempAdapter_3 = New SqlDataAdapter()
				TempAdapter_3.SelectCommand = TempCommand_3
				Dim TempDataset_3 As DataSet
				TempDataset_3 = New DataSet()
				TempAdapter_3.Fill(TempDataset_3)
				oTarget = TempDataset_3
				

                'TODO
                'If Not oTarget.BOF And Not (oTarget.Tables(0).Rows.Count = 0) Then
                If Not (oTarget.Tables(0).Rows.Count = 0) Then
                    ' there's a Sirius claims folder 9999999, so we need to move all
                    ' the documents from source folder to target folder
                    sTargetFolderNum = oTarget.Tables(0).Rows(0)("folder_num")
                    sTargetFolderName = oTarget.Tables(0).Rows(0)("folder_name")

                    sSQL = "UPDATE doc_document SET folder_num = " & sTargetFolderNum
                    sSQL = sSQL & " WHERE folder_num = " & sSourceFolderNum

                    m_oCmd.CommandText = sSQL
                    m_oCmd.ExecuteNonQuery()

                    ' then remove the source folder
                    sSQL = "DELETE FROM doc_folder WHERE folder_num = " & sSourceFolderNum

                    m_oCmd.CommandText = sSQL
                    m_oCmd.ExecuteNonQuery()

                    sSourceFolderName = "'" & sSourceFolderName & "'"
                    sSourceFolderName = (sSourceFolderName & New String(" "c, 72)).Substring(0, 72)

                    ' log it
                    StatusMsg("Docs in " & sSourceFolderName & " moved to '" & sTargetFolderName & "'")

                Else
                    ' a Sirius claims folder does not exist, so rename the C999999999 folder
                    ' to 9999999 so that subsequent Sirius claims don't create a 2nd folder
                    sTargetFolderName = Mid(sSourceFolderName, 4)
                    sTargetFolderName = sTargetFolderName.Replace("'", "''")

                    sSQL = "UPDATE doc_folder SET folder_name = '" & sTargetFolderName & "' WHERE folder_num = " & sSourceFolderNum

                    m_oCmd.CommandText = sSQL
                    m_oCmd.ExecuteNonQuery()

                    sSourceFolderName = "'" & sSourceFolderName & "'"
                    sSourceFolderName = (sSourceFolderName & New String(" "c, 72)).Substring(0, 72)

                    ' log it
                    StatusMsg("Folder  " & sSourceFolderName & " renamed '" & sTargetFolderName & "'")

                End If
				
				
				Application.DoEvents()
				
			Next iteration_row
			
			oTarget = Nothing
			oSource = Nothing
			
			stbStatus.Items.Item(0).Text = "Processing completed"
			
			StatusMsg("Processing completed", True)
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			stbStatus.Items.Item(0).Text = "Processing completed with errors"
			
			StatusMsg(CStr(Information.Err().Number) & " - " & excep.Message)
			StatusMsg(sSQL)
			
			StatusMsg("Processing halted by an error", True)
			
			Return result
		End Try
	End Function
	
	Private Sub txtDatabase_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDatabase.Enter
		m_lReturn = CType(SelectAll(txtDatabase), gPMConstants.PMEReturnCode)
		
	End Sub
	
	Private Sub txtPassword_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPassword.Enter
		m_lReturn = CType(SelectAll(txtPassword), gPMConstants.PMEReturnCode)
	End Sub
	
	Private Sub txtProvider_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtProvider.Enter
		m_lReturn = CType(SelectAll(txtProvider), gPMConstants.PMEReturnCode)
		
	End Sub
	
	Private Sub txtServer_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtServer.Enter
		m_lReturn = CType(SelectAll(txtServer), gPMConstants.PMEReturnCode)
	End Sub
	
	Private Sub txtUsername_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtUsername.Enter
		m_lReturn = CType(SelectAll(txtUsername), gPMConstants.PMEReturnCode)
		
	End Sub
	
	Private Function SelectAll(ByRef oText As TextBox) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			oText.SelectionStart = 0
			oText.SelectionLength = Strings.Len(oText.Text)
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
End Class