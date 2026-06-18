Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Friend Partial Class frmEvent
	Inherits System.Windows.Forms.Form
	
	Private m_lIndex As Integer
	Private m_oParent As frmInterface
	
	Public WriteOnly Property EventIndex() As Integer
		Set(ByVal Value As Integer)
			m_lIndex = Value
		End Set
	End Property
	
	Public WriteOnly Property Parent_Renamed() As frmInterface
		Set(ByVal Value As frmInterface)
			m_oParent = Value
		End Set
	End Property
	
	Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdUp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdUp.Click
		
		Dim lReturn As DialogResult
		
		Dim lIndex As Integer = m_oParent.lvwEvents.FocusedItem.Index + 1
		
		If lIndex > 1 Then
			lIndex -= 1
		Else
			lReturn = MessageBox.Show("Reached top of event log" & Strings.Chr(13) & Strings.Chr(10) & "Start again at the bottom?", "PMEventLogViewer", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
			
			If lReturn = System.Windows.Forms.DialogResult.Yes Then
				lIndex = m_oParent.lvwEvents.Items.Count
			End If
		End If
		
        m_oParent.lvwEvents.Items.Item(lIndex - 1).Selected = True
        m_oParent.lvwEvents.Items.Item(lIndex - 1).Focused = True
		
		m_lIndex = CInt(Mid(m_oParent.lvwEvents.FocusedItem.Name, 2))
		
		ShowDetails()
		
	End Sub
	
	Private Sub cmdDown_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDown.Click
		
		Dim lReturn As DialogResult
		
		Dim lIndex As Integer = m_oParent.lvwEvents.FocusedItem.Index + 1
		
		If lIndex < m_oParent.lvwEvents.Items.Count Then
			lIndex += 1
		Else
			lReturn = MessageBox.Show("Reached bottom of event log" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Start again at the top?", "PMEventLogViewer", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
			
			If lReturn = System.Windows.Forms.DialogResult.Yes Then
				lIndex = 1
			End If
		End If
		
        m_oParent.lvwEvents.Items.Item(lIndex - 1).Selected = True
        m_oParent.lvwEvents.Items.Item(lIndex - 1).Focused = True

		m_lIndex = CInt(Mid(m_oParent.lvwEvents.FocusedItem.Name, 2))
		
		ShowDetails()
		
	End Sub
	

	Private Sub frmEvent_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		ShowDetails()
		
	End Sub
	
	Private Sub ShowDetails()
		
		With g_atypEventRecords(m_lIndex)
			
			Select Case .EventType
				Case "Information"
					imgInfo.Visible = True
					imgWarning.Visible = False
					imgError.Visible = False
				Case "Warning"
					imgInfo.Visible = False
					imgWarning.Visible = True
					imgError.Visible = False
				Case "Error"
					imgInfo.Visible = False
					imgError.Visible = True
					imgWarning.Visible = False
				Case Else
					imgInfo.Visible = False
					imgWarning.Visible = False
					imgError.Visible = False
			End Select
			
			lblTimestamp.Text = " " & DateTimeHelper.ToString(.EventTimeCreated)
			lblSource.Text = " " & .EventSourceName
			lblCategory.Text = " " & .EventCategory
			lblCategoryString.Text = " " & .EventCategoryString
			lblEventType.Text = " " & .EventType
			lblEventID.Text = " " & .EventID
			lblComputer.Text = " " & .EventComputerName
			txtDescription.Text = .EventDescription
			lblEventData.Text = " " & .EventData
			txtEventDataText.Text = .EventDataText
		End With
		
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub frmEvent_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		
		If VB6.PixelsToTwipsX(Me.Width) < 6570 Then
			Me.Width = VB6.TwipsToPixelsX(6570)
		End If
		
		If VB6.PixelsToTwipsY(Me.Height) < 7110 Then
			Me.Height = VB6.TwipsToPixelsY(7110)
		End If
		
		tabMain.Height = Me.ClientRectangle.Height - VB6.TwipsToPixelsY(720)
		tabMain.Width = Me.ClientRectangle.Width - VB6.TwipsToPixelsX(240)
		
		cmdOk.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6.PixelsToTwipsY(cmdOk.Height) - 120)
		cmdOk.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - VB6.PixelsToTwipsX(cmdOk.Width) - 120)
		
		cmdUp.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(tabMain.Width) - VB6.PixelsToTwipsX(cmdUp.Width) - 120)
		cmdDown.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(tabMain.Width) - VB6.PixelsToTwipsX(cmdDown.Width) - 120)
		
		txtDescription.Width = tabMain.Width - VB6.TwipsToPixelsX(480)
		lblEventData.Width = tabMain.Width - VB6.TwipsToPixelsX(480)
		txtEventDataText.Width = tabMain.Width - VB6.TwipsToPixelsX(480)
		
		txtEventDataText.Top = tabMain.Height - VB6.TwipsToPixelsY(1440)
		
		lblEventDataText.Top = txtEventDataText.Top - VB6.TwipsToPixelsY(210)
		
		lblEventData.Top = lblEventDataText.Top - VB6.TwipsToPixelsY(360)
		lblEventDataLabel.Top = lblEventData.Top - VB6.TwipsToPixelsY(210)
		
		txtDescription.Height = lblEventDataLabel.Top - txtDescription.Top
		
		
	End Sub
	
	Private Sub txtDescription_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtDescription.KeyPress
		Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
		
		KeyAscii = 0
		
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		eventArgs.KeyChar = Convert.ToChar(KeyAscii)
	End Sub
	
	Private Sub txtEventDataText_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtEventDataText.KeyPress
		Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
		
		KeyAscii = 0
		
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		eventArgs.KeyChar = Convert.ToChar(KeyAscii)
	End Sub
End Class