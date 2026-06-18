Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
Friend Partial Class frmDebug
	Inherits System.Windows.Forms.Form
	'****
	' This is all debug code, and as such, it's not fit for human consumption.
	'
	' TopMost.Bas accompanies this form, and was stolen in broad daylight from
	' the Gemini source code.
	'
	'***********
	
	
	Private Sub frmDebug_Paint(ByVal eventSender As Object, ByVal eventArgs As PaintEventArgs) Handles MyBase.Paint
		
        Me.TopMost = True
		
	End Sub
	
	Private Sub tmrRefresh_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tmrRefresh.Tick
		
		Dim iLoop1 As Integer
		Dim NodeX As ListViewItem
		
		For iLoop1 = 1 To lvwCMs.Items.Count
			lvwCMs.Items.RemoveAt(0)
		Next iLoop1
		

		For iLoop1 = m_vCMArray.GetLowerBound(1) To m_vCMArray.GetUpperBound(1)
			NodeX = lvwCMs.Items.Add("K" & iLoop1, CStr(iLoop1), "")

            ListViewHelper.GetListViewSubItem(NodeX, 1).Text = CStr(m_vCMArray(ACArrayPartyCnt, iLoop1))

            Select Case CInt(m_vCMArray(ACArrayStatus, iLoop1))
                Case ACStatusLive
                    ListViewHelper.GetListViewSubItem(NodeX, 2).Text = "Live"
                Case ACStatusDead
                    ListViewHelper.GetListViewSubItem(NodeX, 2).Text = "Dead"
                Case ACStatusEmpty
                    ListViewHelper.GetListViewSubItem(NodeX, 2).Text = "Empty"
            End Select
		Next iLoop1
		
		Label1.Text = "Client managers running :" & iLoop1
		
	End Sub
End Class