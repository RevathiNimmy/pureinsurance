Option Strict Off
Option Explicit On
Imports System
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles

Friend Partial Class frmGISListCreator
	Inherits System.Windows.Forms.Form

	Private Sub frmGISListCreator_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		cmdOK.Enabled = False
	End Sub
	Private isInitializingComponent As Boolean
	Private Sub txtInputFile_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInputFile.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		cmdOK.Enabled = Not (txtInputFile.Text.Trim() = "")
	End Sub
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		Me.Close()
	End Sub
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		cmdOK.Enabled = False
		cmdCancel.Enabled = False
		Me.Cursor = Cursors.WaitCursor
		
		
        Dim lResult As gPMConstants.PMEReturnCode = ProcessList(txtInputFile.Text)
		Me.Cursor = Cursors.Default
		If lResult = gPMConstants.PMEReturnCode.PMTrue Then
			' Success - exit
			MessageBox.Show("List files created!", "GIS List Creator", MessageBoxButtons.OK)
			Me.Close()
		Else
			' Failed - return to allow select of other file
			MessageBox.Show("List files not created!", "GIS List Creator", MessageBoxButtons.OK)
			cmdOK.Enabled = True
			cmdCancel.Enabled = True
		End If
		
    End Sub

	Private Sub cmdSelect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelect.Click
		CommonDialog1Open.Title = "Select Input File"
		CommonDialog1Open.ShowDialog()
		
		If CommonDialog1Open.FileName <> "" Then
			txtInputFile.Text = CommonDialog1Open.FileName
		End If
	End Sub
End Class