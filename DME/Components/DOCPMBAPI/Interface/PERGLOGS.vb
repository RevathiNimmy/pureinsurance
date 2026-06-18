Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmPurgeLogs
	Inherits System.Windows.Forms.Form
	
	Private Sub cmbPurgeFrom_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbPurgeFrom.SelectedIndexChanged
		
		FillPurgeTo(VB6.GetItemString(cmbPurgeFrom, cmbPurgeFrom.SelectedIndex))
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		
		Dim sMsg As String = "Are you sure you wish to purge Log Files?"
		If Interaction.MsgBox(sMsg, MB_ICONQUESTION + MB_YESNO, "Purge Log Files") = System.Windows.Forms.DialogResult.No Then
			Exit Sub
		End If
		
		Me.Cursor = Cursors.WaitCursor
		
		If Not (PurgeLogFiles(VB6.GetItemString(cmbPurgeFrom, cmbPurgeFrom.SelectedIndex), VB6.GetItemString(cmbPurgeTo, cmbPurgeTo.SelectedIndex))) Then
			Interaction.MsgBox("Failed to purge log files", MB_ICONEXCLAMATION, "Purge Error")
		End If
		
		Me.Cursor = Cursors.Default
		Me.Close()
		
	End Sub
	
	Private Sub FillPurgeTo(ByRef sPurgeFrom As String)
		
		Dim DirName, sDate As String
		
		Try 
			
			Me.Cursor = Cursors.WaitCursor
			cmbPurgeTo.Items.Clear()
			
			DirName = FileSystem.Dir(g_sHistoryRoot & "logs\", ATTR_NORMAL)
			
			Do While DirName <> ""
				sDate = DirName.Substring(0, 2) & "/" & DirName.Substring(2, Math.Min(DirName.Length, 2)) & "/" & DirName.Substring(4, Math.Min(DirName.Length, 2))
				
				Dim TempDate3 As Date
				Dim TempDate2 As Date
				If (IIf(DateTime.TryParse(sDate, TempDate2), TempDate2.ToString("yyyyMMdd"), sDate)) >= (IIf(DateTime.TryParse(sPurgeFrom, TempDate3), TempDate3.ToString("yyyyMMdd"), sPurgeFrom)) Then
					'            cmbPurgeTo.AddItem Format$(sDate, "dd/mm/yyyy")
					Dim TempDate As Date
					lstPurgeToSorted.Items.Add(IIf(DateTime.TryParse(sDate, TempDate), TempDate.ToString("yyyy/MM/dd"), sDate))
				End If
				
				DirName = FileSystem.Dir()
			Loop 
			
			For iCntr As Integer = 0 To lstPurgeToSorted.Items.Count - 1
				cmbPurgeTo.Items.Add(VB6.GetItemString(lstPurgeToSorted, iCntr).Substring(8, Math.Min(VB6.GetItemString(lstPurgeToSorted, iCntr).Length, 2)) & "/" & VB6.GetItemString(lstPurgeToSorted, iCntr).Substring(5, Math.Min(VB6.GetItemString(lstPurgeToSorted, iCntr).Length, 2)) & "/" & VB6.GetItemString(lstPurgeToSorted, iCntr).Substring(0, Math.Min(VB6.GetItemString(lstPurgeToSorted, iCntr).Length, 4)))
			Next iCntr
			lstPurgeToSorted.Items.Clear()
			
			If cmbPurgeTo.Items.Count > 0 Then
				cmbPurgeTo.SelectedIndex = 0
			End If
			
			Me.Cursor = Cursors.Default
		
		Catch 
			
			
			
			Interaction.MsgBox("Unable to access server to retrieve files", MB_ICONSTOP, "Access Error")
			Exit Sub
		End Try
		
	End Sub
	

	Private Sub frmPurgeLogs_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Dim DirName, sDate As String
		
		Try 
			
            Me.Cursor = Cursors.WaitCursor
            'TODO
            'CenterForm(Me)
			
			DirName = FileSystem.Dir(g_sHistoryRoot & "logs\", ATTR_NORMAL)
			
			Do While DirName <> ""
				sDate = DirName.Substring(0, 2) & "/" & DirName.Substring(2, Math.Min(DirName.Length, 2)) & "/" & DirName.Substring(4, Math.Min(DirName.Length, 2))
				'        cmbPurgeFrom.AddItem Format$(sDate, "dd/mm/yyyy")
				Dim TempDate As Date
				lstPurgeFromSorted.Items.Add(IIf(DateTime.TryParse(sDate, TempDate), TempDate.ToString("yyyy/MM/dd"), sDate))
				
				DirName = FileSystem.Dir()
			Loop 
			
			For iCntr As Integer = 0 To lstPurgeFromSorted.Items.Count - 1
				cmbPurgeFrom.Items.Add(VB6.GetItemString(lstPurgeFromSorted, iCntr).Substring(8, Math.Min(VB6.GetItemString(lstPurgeFromSorted, iCntr).Length, 2)) & "/" & VB6.GetItemString(lstPurgeFromSorted, iCntr).Substring(5, Math.Min(VB6.GetItemString(lstPurgeFromSorted, iCntr).Length, 2)) & "/" & VB6.GetItemString(lstPurgeFromSorted, iCntr).Substring(0, Math.Min(VB6.GetItemString(lstPurgeFromSorted, iCntr).Length, 4)))
			Next iCntr
			lstPurgeFromSorted.Items.Clear()
			
			If cmbPurgeFrom.Items.Count > 0 Then
				cmbPurgeFrom.SelectedIndex = 0
			End If
			
			Me.Cursor = Cursors.Default
			
			FillPurgeTo(VB6.GetItemString(cmbPurgeFrom, cmbPurgeFrom.SelectedIndex))
		
		Catch 
			
			
			
			Interaction.MsgBox("Unable to access server to retrieve files", MB_ICONSTOP, "Access Error")
			Exit Sub
		End Try
		
		
	End Sub
	
	Private Function PurgeLogFiles(ByRef sPurgeFrom As String, ByRef sPurgeTo As String) As Integer
		
		Dim result As Integer = 0
		Dim DirName, sDate, sPurgeDateFrom, sPurgeDateTo As String
		
		result = True
		
		Try 
			
			DirName = FileSystem.Dir(g_sHistoryRoot & "logs\", ATTR_NORMAL)
			
			Do While DirName <> ""
				sDate = DirName.Substring(0, 2) & "/" & DirName.Substring(2, Math.Min(DirName.Length, 2)) & "/" & DirName.Substring(4, Math.Min(DirName.Length, 2))
				'        sPurgeDateFrom = Mid$(sPurgeFrom, 9, 2) & "/" & Mid$(sPurgeFrom, 4, 2) & "/" & Left$(sPurgeFrom, 2)
				'        sPurgeDateTo = Mid$(sPurgeTo, 9, 2) & "/" & Mid$(sPurgeTo, 4, 2) & "/" & Left$(sPurgeTo, 2)
				
				'        If (sDate >= sPurgeDateFrom And sDate <= sPurgeDateTo) Then
				
				Dim TempDate5 As Date
				Dim TempDate4 As Date
				Dim TempDate3 As Date
				Dim TempDate2 As Date
				If (IIf(DateTime.TryParse(sDate, TempDate2), TempDate2.ToString("yyyyMMdd"), sDate)) >= (IIf(DateTime.TryParse(sPurgeFrom, TempDate3), TempDate3.ToString("yyyyMMdd"), sPurgeFrom)) And (IIf(DateTime.TryParse(sDate, TempDate4), TempDate4.ToString("yyyyMMdd"), sDate)) <= (IIf(DateTime.TryParse(sPurgeTo, TempDate5), TempDate5.ToString("yyyyMMdd"), sPurgeTo)) Then
					Dim TempDate As Date
                    If DOCGeneralFunc.KillFile(g_sHistoryRoot & "logs\" & (IIf(DateTime.TryParse(sDate, TempDate), TempDate.ToString("ddMMyy"), sDate)) & ".log") = PM_FALSE Then
                        result = False
                    End If
				End If
				
				DirName = FileSystem.Dir()
			Loop 
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
End Class