Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Friend Partial Class frmViewLogFiles
	Inherits System.Windows.Forms.Form
	
	Dim f_FileBuff() As FixedLengthString = ArraysHelper.InitializeArray(Of FixedLengthString())(New Integer(){}, New Object(){120})
	
	Private Sub cmbLogFiles_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbLogFiles.SelectedIndexChanged
		
		Dim sLogName, sComStr As String
		Dim iHandle As Integer
		
		Dim dbNumericTemp As Double
		If Double.TryParse(VB6.GetItemString(cmbLogFiles, cmbLogFiles.SelectedIndex).Substring(0, 2), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
			Dim TempDate As Date
			sLogName = g_sHistoryRoot & "logs\" & (IIf(DateTime.TryParse(VB6.GetItemString(cmbLogFiles, cmbLogFiles.SelectedIndex), TempDate), TempDate.ToString("ddMMyy"), VB6.GetItemString(cmbLogFiles, cmbLogFiles.SelectedIndex))) & ".log"
		Else
			sLogName = g_sHistoryRoot & "logs\" & VB6.GetItemString(cmbLogFiles, cmbLogFiles.SelectedIndex)
		End If
		
		Dim lFileLen As Integer = CInt((New FileInfo(sLogName)).Length)
		
		'if greater than a meg(ish)
		If lFileLen > 1000000 Then
			sComStr = "WRITE " & sLogName
			Interaction.MsgBox("The Log file needs to be opened by 'WRITE' because of it size" & Strings.Chr(10).ToString() & "If you are asked to convert the file, reply 'NO'", MB_ICONINFORMATION, "Open Log File")

			Dim startInfo As ProcessStartInfo = New ProcessStartInfo(sComStr)
			startInfo.WindowStyle = ProcessWindowStyle.Normal
			iHandle = Process.Start(startInfo).Id
		Else
			ViewText(sLogName)
			vsbView_change(0)
		End If
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		Me.Close()
		
	End Sub
	

	Private Sub frmViewLogFiles_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Dim DirName, sDate As String
		
		Try 
			
            Me.Cursor = Cursors.WaitCursor
            'TODO
            'CenterForm(Me)
			
			DirName = FileSystem.Dir(g_sHistoryRoot & "logs\", ATTR_NORMAL)
			
			Do While DirName <> ""
				Dim dbNumericTemp As Double
				If Double.TryParse(DirName.Substring(0, 6), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
					sDate = DirName.Substring(0, 2) & "/" & DirName.Substring(2, Math.Min(DirName.Length, 2)) & "/" & DirName.Substring(4, Math.Min(DirName.Length, 2))
					Dim TempDate As Date
					lstLogFilesSorted.Items.Add(IIf(DateTime.TryParse(sDate, TempDate), TempDate.ToString("yyyy/MM/dd"), sDate))
				Else
					lstLogFilesSorted.Items.Add(DirName)
				End If
				
				DirName = FileSystem.Dir()
			Loop 
			
			For iCntr As Integer = 0 To lstLogFilesSorted.Items.Count - 1
				Dim dbNumericTemp2 As Double
				If Double.TryParse(VB6.GetItemString(lstLogFilesSorted, iCntr).Substring(0, 2), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
					cmbLogFiles.Items.Add(VB6.GetItemString(lstLogFilesSorted, iCntr).Substring(8, Math.Min(VB6.GetItemString(lstLogFilesSorted, iCntr).Length, 2)) & "/" & VB6.GetItemString(lstLogFilesSorted, iCntr).Substring(5, Math.Min(VB6.GetItemString(lstLogFilesSorted, iCntr).Length, 2)) & "/" & VB6.GetItemString(lstLogFilesSorted, iCntr).Substring(0, Math.Min(VB6.GetItemString(lstLogFilesSorted, iCntr).Length, 4)))
				Else
					cmbLogFiles.Items.Add(VB6.GetItemString(lstLogFilesSorted, iCntr))
				End If
			Next iCntr
			lstLogFilesSorted.Items.Clear()
			
			If cmbLogFiles.Items.Count > 0 Then
				cmbLogFiles.SelectedIndex = cmbLogFiles.Items.Count - 1
			End If
			
			Me.Cursor = Cursors.Default
		
		Catch 
			
			
			
			Interaction.MsgBox("Unable to access server to retrieve files", MB_ICONSTOP, "Access Error")
			Me.Cursor = Cursors.Default
			Exit Sub
		End Try
		
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub frmViewLogFiles_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		

		Try 
			
			pan3ViewLogFiles.Height = Me.ClientRectangle.Height
			pan3LogFile.Width = Me.ClientRectangle.Width - VB6.TwipsToPixelsX(480)
			pan3LogFile.Height = Me.ClientRectangle.Height - VB6.TwipsToPixelsY(1450)
			cmdOK.Left = Me.ClientRectangle.Width - VB6.TwipsToPixelsX(1200)
			cmdOK.Top = Me.ClientRectangle.Height - VB6.TwipsToPixelsY(570)
			
			vsbView.Height = pan3LogFile.Height - VB6.TwipsToPixelsY(120)
			vsbView.Top = pan3LogFile.Top + VB6.TwipsToPixelsY(60)
			vsbView.Left = pan3LogFile.Width - VB6.TwipsToPixelsX(80) 'vsbView.Width
			
			Application.DoEvents()
			
			vsbView_change(0)
			
			Me.Activate()
			vsbView.Focus()
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
		
	End Sub
	
	Private Sub ViewText(ByRef stxtFile As String)
		Dim s, st As String
		Dim ch, lines As Integer
		Dim fp, cap As Integer
		Dim m_Fnumber As Integer
		Dim sTmp As String = ""
		
		Try 
			
			Cursor = Cursors.WaitCursor
			
			m_Fnumber = FileSystem.FreeFile()
			
			FileSystem.FileOpen(m_Fnumber, stxtFile, OpenMode.Input, OpenAccess.Read)
			
			'    Me.Caption = Tag + " (" + Str(LOF(m_Fnumber)) + " bytes)"
			
			lines = 0
			While Not FileSystem.EOF(m_Fnumber)
				ReDim Preserve f_FileBuff(lines)
				
				sTmp = FileSystem.LineInput(m_Fnumber)
				If sTmp.Length > 120 Then
					f_FileBuff(lines).Value = sTmp.Substring(0, 120)
				Else
					f_FileBuff(lines).Value = sTmp
				End If
				'    txtLogFile = txtLogFile & f_FileBuff(lines%) & Chr(9) & Chr(10)
				
				lines += 1
				
			End While
			
			FileSystem.FileClose(m_Fnumber)
			
			If lines > 64000 Then
				vsbView.Maximum = (32000 + vsbView.LargeChange - 1)
				Interaction.MsgBox("Log file too big. First 64000 lines displayed", MB_ICONEXCLAMATION)
			Else
				vsbView.Maximum = (lines - 32000 + vsbView.LargeChange - 1)
			End If
			
			Cursor = Cursors.Default
			'f_iLoad = True
		
		Catch 
			
			
			
			Cursor = Cursors.Default
			Interaction.MsgBox("Unable to view log file", MB_ICONSTOP, "Open Log File")
			'f_iLoad = False
			Exit Sub
		End Try
		
	End Sub
	
	'UPGRADE_NOTE: (2010) vsbView.change was changed from an event to a procedure. More Information: http://www.vbtonet.com/ewis/ewi2010.aspx

	Private Sub vsbView_change(ByVal newScrollValue As Integer)
		Dim NewLargeChange, iLines, iMaxline As Integer
		Dim sTmp As New StringBuilder
		
		Try 
			
			txtLogFile.Text = ""
			
			'currentx = 0
			'currenty = 0
			
			iLines = VB6.PixelsToTwipsY(txtLogFile.Height) / VB6.PixelsToTwipsY(CreateGraphics().MeasureString("A", Font).Height)
			NewLargeChange = iLines
			vsbView.Maximum = vsbView.Maximum + NewLargeChange - vsbView.LargeChange
			vsbView.LargeChange = NewLargeChange
			iMaxline = newScrollValue + iLines
			
			If iMaxline >= (vsbView.Maximum - (vsbView.LargeChange + 1)) Then
				iMaxline = (vsbView.Maximum - (vsbView.LargeChange + 1)) - 1
			End If
			
			sTmp = New StringBuilder("")
			For iTmp As Integer = newScrollValue To iMaxline
                sTmp.Append(f_FileBuff(iTmp + 32000).Value.TrimEnd() & Strings.Chr(13).ToString() & Strings.Chr(10).ToString())
			Next iTmp
			
			txtLogFile.Text = sTmp.ToString()
		
		Catch 
			
			
			
			'    MsgBox "ERROR: - " & Err, MB_ICONEXCLAMATION, "Scroll Log"
			Exit Sub
		End Try
		
	End Sub
	Private Sub vsbView_Scroll(ByVal eventSender As Object, ByVal eventArgs As ScrollEventArgs) Handles vsbView.Scroll
		Select Case eventArgs.Type
			Case ScrollEventType.EndScroll
				vsbView_Change(eventArgs.NewValue)
		End Select
	End Sub
End Class