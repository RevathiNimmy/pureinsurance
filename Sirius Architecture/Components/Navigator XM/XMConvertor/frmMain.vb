Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Windows.Forms
Friend Partial Class frmMain
	Inherits System.Windows.Forms.Form
	
	Private Sub PopulateDatabases()
		
		Dim oConnect As SqlConnection
		Dim oCommand As SqlCommand

		Dim oResults As DataSet
		Dim lIndex As Integer
		Dim sDatabase As String = ""
		
		Try 
			
			oCommand = New SqlCommand()
			oConnect = New SqlConnection()
            'Developer Guide No.102
            'start
            'oConnect.Provider = "sqloledb"

            'oConnect.Properties("Data Source").Value = cboServer.Text

            'oConnect.Properties("User ID").Value = txtUser.Text

            'oConnect.Properties("Password").Value = txtPassword.Text

            'oConnect.Properties("Initial Catalog").Value = "master"
			

            'LogText("Connecting to " & oConnect.Properties("Data Source").Value)
			

			oConnect.Open()
			

            'LogText("Connected to " & oConnect.Properties("Data Source").Value)
            'End
			
			oCommand.Connection = oConnect
			oCommand.CommandType = CommandType.Text
			oCommand.CommandText = "SELECT name FROM sysdatabases ORDER BY name"
			Dim adap As SqlDataAdapter = New SqlDataAdapter(oCommand.CommandText, oCommand.Connection)
			oResults = New DataSet("dsl")
			adap.Fill(oResults)
			
			cboDatabase.Items.Clear()
			
			' Add the databases
			For	Each iteration_row As DataRow In oResults.Tables(0).Rows
				sDatabase = iteration_row("name")
				Dim cboDatabase_NewIndex As Integer = -1
				cboDatabase_NewIndex = cboDatabase.Items.Add(sDatabase)
				' Did we just add a likely Sirius database?
				If sDatabase.ToLower().IndexOf("sirius") >= 0 Then
					' Yep, snag it
					lIndex = cboDatabase_NewIndex
				End If
			Next iteration_row
			
			cboDatabase.SelectedIndex = lIndex
			cboDatabase.Focus()
		
		Catch excep As System.Exception
			
			
			
			MessageBox.Show("Error text: " & excep.Message & Environment.NewLine & "Error number: " & CStr(Information.Err().Number), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			
		End Try
		
	End Sub
	
	Private Sub PopulateProcesses()
		
		Dim oConnect As New SqlConnection
		Dim oCommand As New SqlCommand
		Dim oResults As DataSet

		Try 
            'Developer Guide No.102
            'start
            'oConnect.Provider = "sqloledb"

            'oConnect.Properties("Data Source").Value = cboServer.Text

            'oConnect.Properties("User ID").Value = txtUser.Text

            'oConnect.Properties("Password").Value = txtPassword.Text

            'oConnect.Properties("Initial Catalog").Value = cboDatabase.Text
			

            'LogText("Connecting to " & oConnect.Properties("Data Source").Value)
			

			oConnect.Open()
			

            'LogText("Connected to " & oConnect.Properties("Data Source").Value)
            'End
			
			oCommand.Connection = oConnect
			oCommand.CommandType = CommandType.Text
			oCommand.CommandText = "SELECT pmnav_process_id, description FROM pmnav_process ORDER BY description"
			Dim adap As SqlDataAdapter = New SqlDataAdapter(oCommand.CommandText, oCommand.Connection)
			oResults = New DataSet("dsl")
			adap.Fill(oResults)
			
			cboProcessID.Items.Clear()
			
			For	Each iteration_row As DataRow In oResults.Tables(0).Rows
				Dim cboProcessID_NewIndex As Integer = -1
				cboProcessID_NewIndex = cboProcessID.Items.Add((iteration_row("description")) & " [" & iteration_row("pmnav_process_id") & "]")
				VB6.SetItemData(cboProcessID, cboProcessID_NewIndex, iteration_row("pmnav_process_id"))
			Next iteration_row
			
			If cboProcessID.Items.Count > 0 Then
				cboProcessID.SelectedIndex = 0
			End If
			
			oResults = Nothing
			oConnect.Close()
			oConnect = Nothing
			
			cmdConvert.Enabled = True
			cmdConvertAll.Enabled = True
			VB6.SetDefault(cmdConvert, True)
		
		Catch excep As System.Exception
			
			
			
			
			Select Case Information.Err().Number
				Case -2147217843 ' bad username
					MessageBox.Show("The username or password is not valid for the current server" & Environment.NewLine & "Error text: " & excep.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
					txtUser.Focus()
					
				Case -2147467259 ' bad database
					If MessageBox.Show("The database '" & cboDatabase.Text & "' could not be found on the server." & Environment.NewLine & "Error text: " & excep.Message & Environment.NewLine & "Do you wish to get a list of databases?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) = System.Windows.Forms.DialogResult.Yes Then
						PopulateDatabases()
					End If
					
				Case Else ' your guess...
					MessageBox.Show("Unknown error : " & excep.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
					
			End Select
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub convert(Optional ByVal v_lProcessID As Integer = -1)
		
		Dim oConnection As New SqlConnection
		Dim oCommand As New SqlCommand
		Dim oRecord As DataSet
		Dim sFilename As String = ""
		Dim lHandle As Integer
		Dim sTemp, sStartMap As String
		Dim lProcessID As Integer
		Dim bDebug As Boolean
		
		Try 
			
			If v_lProcessID > 0 Then
				lProcessID = v_lProcessID
			Else
				lProcessID = VB6.GetItemData(cboProcessID, cboProcessID.SelectedIndex)
			End If
			
			bDebug = (chkDebug.CheckState = CheckState.Checked)
            'Developer Guide No.102
            'start
            'oConnection.Provider = "sqloledb"

            'oConnection.Properties("Data Source").Value = cboServer.Text

            'oConnection.Properties("User ID").Value = txtUser.Text

            'oConnection.Properties("Password").Value = txtPassword.Text

            'oConnection.Properties("Initial Catalog").Value = cboDatabase.Text
			

            'LogText("Connecting to " & oConnection.Properties("Data Source").Value)
			

			oConnection.Open()
			

            'LogText("Connected to " & oConnection.Properties("Data Source").Value)
            'End
			
			oCommand.Connection = oConnection
			oCommand.CommandType = CommandType.Text
			oCommand.CommandText = "EXECUTE spu_pm_generate_navigator_xml  @pmnav_process_id=" & lProcessID & ", @debug=" & (IIf(bDebug, "1", "0"))
			
			Dim adap As SqlDataAdapter = New SqlDataAdapter(oCommand.CommandText, oCommand.Connection)
			oRecord = New DataSet("dsl")
			adap.Fill(oRecord)
			
			If Not txtFolder.Text.EndsWith("\") Then txtFolder.Text = txtFolder.Text & "\"
			
			If Not Directory.Exists(txtFolder.Text) Then
				Directory.CreateDirectory(txtFolder.Text)
			End If
			
			'If (Dir(txtFolder.Text & lProcessID, vbDirectory) = "") Then
			'    MkDir txtFolder.Text & lProcessID
			'End If
			
			sStartMap = ""
			
			' Query format :
			' FILE:filename.xml
			' <XML CONTENT>
			' FILE:filename.xml
			' etc...
			lHandle = -1
			For	Each iteration_row As DataRow In oRecord.Tables(0).Rows
				
				If iteration_row(0).Substring(0, 5) = "FILE:" Then
					If lHandle <> -1 Then
						' close the file
						FileSystem.FileClose(lHandle)
					End If
					' Get the name
					'sTemp = txtFolder.Text & lProcessID & "\" & Right$(oRecord.Fields(0).Value, Len(oRecord.Fields(0).Value) - 5)
					sTemp = txtFolder.Text & CStr(iteration_row(0)).Substring(iteration_row(0).Length - (Strings.Len(iteration_row(0)) - 5))
					If sStartMap = "" Then
						sStartMap = sTemp
					End If
					If FileSystem.Dir(sTemp, FileAttribute.Normal) <> "" Then
						LogText("Duplicate Process: " & sTemp)
						sTemp = sTemp.Substring(0, sTemp.Length - 4) & "(" & CStr(lProcessID) & ").xml"
						'Kill sTemp
					End If
					' Open the file
					lHandle = FileSystem.FreeFile()
					FileSystem.FileOpen(lHandle, sTemp, OpenMode.Output)
					LogText("Saving to " & sTemp)
				Else
					sTemp = iteration_row(0)
					FileSystem.PrintLine(lHandle, sTemp)
				End If
				
				
			Next iteration_row
			
			FileSystem.FileClose(lHandle)
			
			oRecord = Nothing
			oCommand = Nothing
			LogText("Disconnecting")
			oConnection.Close()
			LogText("Disconnected.")
			oConnection = Nothing
			LogText("The start map is : " & sStartMap)
			LogText("Done.")
			
			If chkClipboard.CheckState = CheckState.Checked Then
				My.Computer.Clipboard.Clear()

				My.Computer.Clipboard.SetText(sStartMap)
			End If
		
		Catch excep As System.Exception
			
			
			
			MessageBox.Show("Error text: " & excep.Message & Environment.NewLine & "Error number: " & CStr(Information.Err().Number), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			'    Resume
		End Try
		
	End Sub
	
	Private Sub LogText(ByRef sMessage As String)
		
		lstLog.Items.Add(sMessage)
		ListBoxHelper.SetSelected(lstLog, lstLog.Items.Count - 1, True)
		
	End Sub
	
	Private Sub cmdConvert_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdConvert.Click
		
		convert()
		
	End Sub
	
	Private Sub cmdConvertAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdConvertAll.Click
		Dim oConnect As New SqlConnection
		Dim oCommand As New SqlCommand
        Dim lProcessID As Integer
		
		' On Error GoTo Err_PopulateProcesses
        'Developer Guide No.102
        'start
        'oConnect.Provider = "sqloledb"

        'oConnect.Properties("Data Source").Value = cboServer.Text

        'oConnect.Properties("User ID").Value = txtUser.Text

        'oConnect.Properties("Password").Value = txtPassword.Text

        'oConnect.Properties("Initial Catalog").Value = cboDatabase.Text
		

        'LogText("Connecting to " & oConnect.Properties("Data Source").Value)
		

		oConnect.Open()
		

        'LogText("Connected to " & oConnect.Properties("Data Source").Value)
        'End
		
		oCommand.Connection = oConnect
		oCommand.CommandType = CommandType.Text
		oCommand.CommandText = "SELECT  pmnav_process_id " &  _
		                       "From pmnav_process " &  _
		                       "WHERE pmnav_process_id IN (select ok_nav_process_id from PMNav_Step) " &  _
		                       "OR pmnav_process_id IN (select cancel_nav_process_id from PMNav_Step) " &  _
		                       "OR pmnav_process_id IN (select pmnav_process_id from PMWrk_Task) " &  _
		                       "ORDER BY pmnav_process_id desc"
		
		Dim adap As SqlDataAdapter = New SqlDataAdapter(oCommand.CommandText, oCommand.Connection)
		Dim oResults As DataSet = New DataSet("dsl")
		adap.Fill(oResults)
		
		' Delete existing XML RoadMaps
		File.Delete(txtFolder.Text & "\*.xml")
		
		For	Each iteration_row As DataRow In oResults.Tables(0).Rows
			lProcessID = iteration_row("pmnav_process_id")
			convert(lProcessID)
		Next iteration_row
		
		
		oResults = Nothing
		oConnect.Close()
		oConnect = Nothing
		
		
	End Sub
	
	Private Sub Command1_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Command1.Click
		
		LogText("Populating Processes.")
		PopulateProcesses()
		LogText("Ready...")
		
	End Sub
	
	Private Sub FindServers()
		
		Dim oNames As SQLDMO.NameList
		Dim oSQLApp As SQLDMO.Application
		
		Try 
			
			oSQLApp = New SQLDMO.Application()
			oNames = oSQLApp.ListAvailableSQLServers()
			
			For i As Integer = 1 To oNames.Count
				cboServer.Items.Add(oNames.Item(i))
			Next i
			
			If cboServer.Items.Count > 0 Then
				cboServer.SelectedIndex = 0
			End If
		
		Catch excep As System.Exception
			
			
			
			MessageBox.Show(CStr(CDbl("Error text: " & excep.Message & Environment.NewLine & "Error number: ") + Information.Err().Number), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			
		End Try
		
	End Sub
	
	Private Sub frmMain_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			Me.Refresh()
			LogText("Finding Servers. Please wait...")
			FindServers()
			cboProcessID.Items.Clear()
			cboProcessID.Items.Add("Please check database settings and click GetProcesses")
			cboProcessID.SelectedIndex = 0
			LogText("Ready...")
			
		End If
	End Sub
End Class