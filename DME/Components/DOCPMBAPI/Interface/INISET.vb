Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmAppSettings
	Inherits System.Windows.Forms.Form
	
	Dim f_iChanged As Integer
	
	Const DIR_OK As Integer = 0
	Const DIR_NOTFOUND As Integer = -1
	Const DIR_INVAILD As Integer = -2
	Const DIR_NODRIVE As Integer = -3
	
	Private Function CheckPath(ByRef sPath As String) As Integer
		Dim sTmp As String = ""
		
		Try 
			
			
			If Mid(sPath, 2, 1) <> ":" Then
				Return DIR_NODRIVE
			Else
				
				sTmp = Directory.GetCurrentDirectory()
				
				Directory.SetCurrentDirectory(sPath.Substring(0, sPath.Length - 1))
				Directory.SetCurrentDirectory(sTmp)
				
				Return DIR_OK
			End If
		
		Catch 
		End Try
		
		
		
		
		Select Case Information.Err().Number
			Case 76 'path not found
				Return DIR_NOTFOUND
			Case Else
				Return Information.Err().Number
		End Select
		
	End Function
	
	Private Sub CheckScanDatabase()
		Dim sTmp, sTmp2 As String
		
		Try 
			
			If GetIniFileVar("Scan", "ScanRoot", sTmp, False) = PM_TRUE Then
				sTmp2 = FileSystem.Dir(sTmp & "DMSSDB.MDB", FileAttribute.Normal)
				
				If sTmp2.Trim().ToUpper() <> "DMSSDB.MDB" Then
					'no scan database, copy a blank one in...

                    File.Copy(lbl3ScanDBPath.Text.Trim().ToUpper() & "\DMSINIT\DMSSDB.MDB", sTmp.Trim().ToUpper() & "DMSSDB.MDB")
				End If
			End If
		
		Catch 
			
			
			
			Interaction.MsgBox("Error " & Information.Err().Number & ": " & Conversion.ErrorToString(), MB_ICONEXCLAMATION, "Copy ScanDatabase File")
			Exit Sub
		End Try
		
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		Me.Tag = CStr(False)
		Me.Hide()
		
	End Sub
	
	Private Sub cmdChange_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdChange.Click
		Dim sNewPath, sNewDB As String
		Dim iTmp As Integer
		
		Select Case g_sAppType.Value
			Case "VIEW", "LINK"
				
				'UPGRADE_NOTE: (6010) Variable cdbChange was renamed cdbChangeOpen. More Information: http://www.vbtonet.com/ewis/ewi6010.aspx
				cdbChangeOpen.Title = "ViewStation - DocuMaster Database"

				'UPGRADE_NOTE: (6010) Variable cdbChange was renamed cdbChangeOpen. More Information: http://www.vbtonet.com/ewis/ewi6010.aspx
				cdbChangeOpen.Filter = "DocuMaster DataBase (DMSDDB.MDB)|DMSDDB.MDB"
				'UPGRADE_NOTE: (6010) Variable cdbChange was renamed cdbChangeOpen. More Information: http://www.vbtonet.com/ewis/ewi6010.aspx
				cdbChangeOpen.FileName = ""

                'TODO
                'cdbChange.Flags = &H1804

                'TODO
                'cdbChange.Action = 1
				
				'UPGRADE_NOTE: (6010) Variable cdbChange was renamed cdbChangeOpen. More Information: http://www.vbtonet.com/ewis/ewi6010.aspx
				sNewPath = GetFilePath(cdbChangeOpen.FileName.Trim())
				'strip the trailing slash
				If sNewPath.Length > 0 Then
					sNewPath = sNewPath.Substring(0, sNewPath.Length - 1)
				End If
				
				'UPGRADE_NOTE: (6010) Variable cdbChange was renamed cdbChangeOpen. More Information: http://www.vbtonet.com/ewis/ewi6010.aspx
				sNewDB = GetFileName(cdbChangeOpen.FileName.Trim())
				

                If sNewPath.ToUpper() <> lbl3ViewDBPath.Text.ToUpper() And sNewPath.Length > 0 Then

                    lbl3ViewDBPath.Text = sNewPath
                    f_iChanged = True
                End If
				

                If sNewDB.ToUpper() <> lbl3ViewDBName.Text.ToUpper() And sNewDB.Length > 0 Then

                    lbl3ViewDBName.Text = sNewDB
                    f_iChanged = True
                End If
			Case "SCAN"
				
				'UPGRADE_NOTE: (6010) Variable cdbChange was renamed cdbChangeOpen. More Information: http://www.vbtonet.com/ewis/ewi6010.aspx
				cdbChangeOpen.Title = "ScanStation - DocuMaster Database"

				'UPGRADE_NOTE: (6010) Variable cdbChange was renamed cdbChangeOpen. More Information: http://www.vbtonet.com/ewis/ewi6010.aspx
				cdbChangeOpen.Filter = "DocuMaster DataBase (DMSDDB.MDB)|DMSDDB.MDB"
				'UPGRADE_NOTE: (6010) Variable cdbChange was renamed cdbChangeOpen. More Information: http://www.vbtonet.com/ewis/ewi6010.aspx
				cdbChangeOpen.FileName = ""

                'TODO
                'cdbChange.Flags = &H1804

                'TODO
                'cdbChange.Action = 1
				
				'UPGRADE_NOTE: (6010) Variable cdbChange was renamed cdbChangeOpen. More Information: http://www.vbtonet.com/ewis/ewi6010.aspx
				sNewPath = GetFilePath(cdbChangeOpen.FileName.Trim())
				'strip the trailing slash
				If sNewPath.Length > 0 Then
					sNewPath = sNewPath.Substring(0, sNewPath.Length - 1)
				End If
				
				'UPGRADE_NOTE: (6010) Variable cdbChange was renamed cdbChangeOpen. More Information: http://www.vbtonet.com/ewis/ewi6010.aspx
				sNewDB = GetFileName(cdbChangeOpen.FileName.Trim())
				

                If sNewPath.ToUpper() <> lbl3ScanDBPath.Text.ToUpper() And sNewPath.Length > 0 Then

                    lbl3ScanDBPath.Text = sNewPath
                    f_iChanged = True
                End If
				

                If sNewDB.ToUpper() <> lbl3ScanDBName.Text.ToUpper() And sNewDB.Length > 0 Then

                    lbl3ScanDBName.Text = sNewDB
                    f_iChanged = True
                End If
			Case "APIT"
				
				'UPGRADE_NOTE: (6010) Variable cdbChange was renamed cdbChangeOpen. More Information: http://www.vbtonet.com/ewis/ewi6010.aspx
				cdbChangeOpen.Title = "API - DocuMaster Database"

				'UPGRADE_NOTE: (6010) Variable cdbChange was renamed cdbChangeOpen. More Information: http://www.vbtonet.com/ewis/ewi6010.aspx
				cdbChangeOpen.Filter = "DocuMaster DataBase (DMSDDB.MDB)|DMSDDB.MDB"
				'UPGRADE_NOTE: (6010) Variable cdbChange was renamed cdbChangeOpen. More Information: http://www.vbtonet.com/ewis/ewi6010.aspx
                cdbChangeOpen.FileName = ""

                'TODO
                'cdbChange.Flags = &H1804

                'TODO
                'cdbChange.Action = 1
				
				'UPGRADE_NOTE: (6010) Variable cdbChange was renamed cdbChangeOpen. More Information: http://www.vbtonet.com/ewis/ewi6010.aspx
				sNewPath = GetFilePath(cdbChangeOpen.FileName.Trim())
				'strip the trailing slash
				If sNewPath.Length > 0 Then
					sNewPath = sNewPath.Substring(0, sNewPath.Length - 1)
				End If
				
				'UPGRADE_NOTE: (6010) Variable cdbChange was renamed cdbChangeOpen. More Information: http://www.vbtonet.com/ewis/ewi6010.aspx
				sNewDB = GetFileName(cdbChangeOpen.FileName.Trim())
				
				'Get remote path...
				frmSelectDir.ShowDialog()
				'add a trialing slash if it needs one...
                If Not frmSelectDir.lbl3Path.Text.EndsWith("\") Then

                    frmSelectDir.lbl3Path.Text = frmSelectDir.lbl3Path.Text & "\"
                End If


                If lbl3APIRemotePath.Text <> frmSelectDir.lbl3Path.Text Then


                    lbl3APIRemotePath.Text = frmSelectDir.lbl3Path.Text
                    f_iChanged = True
                End If
				frmSelectDir.Close()
				

                If sNewPath.ToUpper() <> lbl3APIDBPath.Text.ToUpper() And sNewPath.Length > 0 Then

                    lbl3APIDBPath.Text = sNewPath
                    f_iChanged = True
                End If
				

                If sNewDB.ToUpper() <> lbl3APIDBName.Text.ToUpper() And sNewDB.Length > 0 Then

                    lbl3APIDBName.Text = sNewDB
                    f_iChanged = True
                End If
				
		End Select
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		Dim sTmp As String = ""
		Dim iTmp As Integer
		
		If f_iChanged Then
			If Interaction.MsgBox("Do you want to save your changes?", MB_ICONQUESTION + MB_YESNO, "Save Changes") = System.Windows.Forms.DialogResult.No Then
				' We dont want to save ... Do nothing
			Else
				Select Case g_sAppType.Value
					Case "VIEW", "LINK"


                        If PutIniFileVar("Paths", "dbRoot", lbl3ViewDBPath.Text) = PM_FALSE Then
                            Interaction.MsgBox("ERROR: Failed to save new path setting", MB_ICONEXCLAMATION, "Save Settings")
                        ElseIf PutIniFileVar("Paths", "dbName", lbl3ViewDBName.Text) = PM_FALSE Then
                            Interaction.MsgBox("ERROR: Failed to save new database name", MB_ICONEXCLAMATION, "Save Settings")
                        End If
						
						If g_sAppType.Value = "VIEW" Then
							Interaction.MsgBox("In order for these changes to take affect" & Strings.Chr(10).ToString() & "you will need to restart the ViewStation", MB_ICONINFORMATION)
						End If
						
					Case "SCAN"
						


                        If lbl3ScanDBPath.Text.Trim() = "" Or lbl3ScanDBName.Text.Trim() = "" Then
                            Exit Sub
                        End If
						
						If txtScanDirectory.Text.Trim() = "" Then
							Interaction.MsgBox("A scan Directory must be specified", MB_ICONEXCLAMATION, "Startup Settings")
							Exit Sub
						End If
						
						iTmp = CheckPath(txtScanDirectory.Text)
						Select Case iTmp
							Case DIR_OK
							Case DIR_NOTFOUND
								If Interaction.MsgBox("The Scan Directory '" & txtScanDirectory.Text.Trim() & "' does not exist" & Strings.Chr(10).ToString() & "Do you want to create it now?", MB_ICONQUESTION + MB_YESNO, "Create Directory") = System.Windows.Forms.DialogResult.No Then
									txtScanDirectory.Focus()
									Exit Sub
								Else
									'make the scan path
                                    If Not DOCGeneralFunc.MakePath(txtScanDirectory.Text.Trim()) Then
                                        Interaction.MsgBox("Failed to create path '" & txtScanDirectory.Text.Trim() & "'", MB_ICONEXCLAMATION, "Create Path")
                                        txtScanDirectory.Focus()
                                        Exit Sub
                                    Else
                                        'make the data directory off the scan path
                                        If Not DOCGeneralFunc.MakePath(txtScanDirectory.Text.Trim() & "DATA\") Then
                                            Interaction.MsgBox("Failed to create path '" & txtScanDirectory.Text.Trim() & "DATA" & "'", MB_ICONEXCLAMATION, "Create Path")
                                        End If
                                    End If
								End If
							Case DIR_NODRIVE
								Interaction.MsgBox("A drive must be specified for the Scan Directory", MB_ICONEXCLAMATION, "Path error")
								txtScanDirectory.Focus()
								Exit Sub
							Case Else ' DIR_INVALID
								Interaction.MsgBox("The Scan Directory is invalid - " & iTmp, MB_ICONEXCLAMATION, "Invalid Path")
								txtScanDirectory.Focus()
								Exit Sub
						End Select
						


                        If PutIniFileVar("Paths", "dbRoot", lbl3ScanDBPath.Text) = PM_FALSE Then
                            Interaction.MsgBox("ERROR: Failed to save new path setting", MB_ICONEXCLAMATION, "Save Settings")
                        ElseIf PutIniFileVar("Paths", "dbName", lbl3ScanDBName.Text) = PM_FALSE Then
                            Interaction.MsgBox("ERROR: Failed to save new database name", MB_ICONEXCLAMATION, "Save Settings")
                        ElseIf PutIniFileVar("Scan", "ScanRoot", txtScanDirectory.Text) = PM_FALSE Then
                            Interaction.MsgBox("ERROR: Failed to save scan directory setting", MB_ICONEXCLAMATION, "Save Settings")
                        End If
						
						CheckScanDatabase()
						
					Case "APIT"
						
						If txtAPIInterval.Text = "" Then
							txtAPIInterval.Text = "15"
						Else
							Dim dbNumericTemp As Double
							If Not Double.TryParse(txtAPIInterval.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
								Interaction.MsgBox("Timer interval must be a numeric value", MB_ICONEXCLAMATION, "Invalid Number")
								Exit Sub
							End If
						End If
						



                        If PutIniFileVar("Paths", "dbRoot", lbl3APIDBPath.Text) = PM_FALSE Then
                            Interaction.MsgBox("ERROR: Failed to save new path setting", MB_ICONEXCLAMATION, "Save Settings")
                        ElseIf PutIniFileVar("Paths", "dbName", lbl3APIDBName.Text) = PM_FALSE Then
                            Interaction.MsgBox("ERROR: Failed to save new database name", MB_ICONEXCLAMATION, "Save Settings")
                        ElseIf PutIniFileVar("APIDaemon", "JournalRoot", lbl3APIRemotePath.Text) = PM_FALSE Then
                            Interaction.MsgBox("ERROR: Failed to save scan directory setting", MB_ICONEXCLAMATION, "Save Settings")
                        ElseIf PutIniFileVar("APIDaemon", "TimerInterval", txtAPIInterval.Text) = PM_FALSE Then
                            Interaction.MsgBox("ERROR: Failed to save API timer interval setting", MB_ICONEXCLAMATION, "Save Settings")
                        End If
						
				End Select
				
			End If
		End If
		
		Me.Tag = CStr(True)
		Me.Hide()
		
		
	End Sub
	

	Private Sub frmAppSettings_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		Dim iTmp As Integer
		Dim sTmp As String = ""
        'TODO
        'CenterForm(Me)
		f_iChanged = False
		
		Me.Cursor = Cursors.Default 'DEFAULT
		
		Select Case g_sAppType.Value
			Case "VIEW", "LINK"
				fra3ViewStation.Visible = True
				fra3ScanStation.Visible = False
				fra3API.Visible = False
				
				pan3Settings.Height = fra3ViewStation.Height + VB6.TwipsToPixelsY(360)
				
				Me.Text = "ViewStation Settings"
				
				If GetIniFileVar("Paths", "dbName", sTmp, False) Then

                    lbl3ViewDBName.Text = sTmp
				End If
				
				If GetIniFileVar("Paths", "dbRoot", sTmp, False) Then

                    lbl3ViewDBPath.Text = sTmp
				End If
				
			Case "SCAN"
				fra3ViewStation.Visible = False
				fra3ScanStation.Visible = True
				fra3API.Visible = False
				
				pan3Settings.Height = fra3ScanStation.Height + VB6.TwipsToPixelsY(360)
				
				Me.Text = "ScanStation Settings"
				
				If GetIniFileVar("Paths", "dbName", sTmp, False) = PM_TRUE Then

                    lbl3ScanDBName.Text = sTmp
				End If
				
				If GetIniFileVar("Paths", "dbRoot", sTmp, False) = PM_TRUE Then

                    lbl3ScanDBPath.Text = sTmp
				End If
				
				If GetIniFileVar("Scan", "ScanRoot", sTmp, False) = PM_TRUE Then
					txtScanDirectory.Text = sTmp
				Else
					txtScanDirectory.Text = "C:\SCAN\" 'default to local drive
				End If
				
			Case "APIT"
				fra3ViewStation.Visible = False
				fra3ScanStation.Visible = False
				fra3API.Visible = True
				
				pan3Settings.Height = fra3API.Height + VB6.TwipsToPixelsY(360)
				
				Me.Text = "DocuMaster API Settings"
				
				If GetIniFileVar("Paths", "dbName", sTmp, False) = PM_TRUE Then

                    lbl3APIDBName.Text = sTmp
				End If
				
				If GetIniFileVar("Paths", "dbRoot", sTmp, False) = PM_TRUE Then

                    lbl3APIDBPath.Text = sTmp
				End If
				
				If GetIniFileVar("APIDaemon", "JournalRoot", sTmp, False) = PM_TRUE Then

                    lbl3APIRemotePath.Text = sTmp
				End If
				
				If GetIniFileVar("APIDaemon", "TimerInterval", sTmp, False) = PM_TRUE Then
					txtAPIInterval.Text = sTmp
				End If
				
			Case Else
				Me.Close()
		End Select
		
		Me.Height = pan3Settings.Height + VB6.TwipsToPixelsY(400)
		
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub frmAppSettings_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		
		Me.Height = pan3Settings.Height + VB6.TwipsToPixelsY(400)
		Me.Width = pan3Settings.Width + VB6.TwipsToPixelsX(100)
		
	End Sub
	
	Private Function GetFileName(ByRef sPath As String) As String
		Dim result As String = String.Empty
		
		result = ""
		
		'find the file name
		For iTmp As Integer = sPath.Length To 1 Step -1
			
			If Mid(sPath, iTmp, 1) = "\" Then
				result = sPath.Substring(sPath.Length - (sPath.Length - iTmp))
				Exit For
			End If
			
		Next iTmp
		
		Return result
	End Function
	
	Private Function GetFilePath(ByRef sPath As String) As String
		Dim result As String = String.Empty
		
		result = ""
		
		'find the files path
		For iTmp As Integer = sPath.Length To 1 Step -1
			
			If Mid(sPath, iTmp, 1) = "\" Then
				result = sPath.Substring(0, iTmp)
				Exit For
			End If
			
		Next iTmp
		
		Return result
	End Function
	
	Private Sub txtScanDirectory_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtScanDirectory.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		f_iChanged = True
		
	End Sub
	
	Private Sub txtScanDirectory_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtScanDirectory.Leave
		
        If Not txtScanDirectory.Text.EndsWith("\") Then
            txtScanDirectory.Text = txtScanDirectory.Text & "\"
        End If
		
	End Sub
End Class