Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
Friend Partial Class frmUserOptions
	Inherits System.Windows.Forms.Form
	'* Amendment History
	'*
	'******************************************************************************
	
	Private Const ACClass As String = "frmOptions"
	
	Private m_bCancelled As Boolean
	
	'DAK110100
	' IsAutoRefresh
	Private m_bIsAutoRefresh As Boolean
	' RefreshRate
	Private m_iRefreshRate As Integer
	
	Public Property Cancelled() As Boolean
		Get
			Return m_bCancelled
		End Get
		Set(ByVal Value As Boolean)
			m_bCancelled = Value
		End Set
	End Property
	
	'DAK231299
	Public Property ViewToolbar() As Integer
		Get
			
			
			Dim iViewToolbar As Integer = chkToolbar.CheckState
			If chkToolbar.Enabled Then
				iViewToolbar += ACChkEnabled
			End If
			
			Return iViewToolbar
			
		End Get
		Set(ByVal Value As Integer)
			
			If Value And ACChkEnabled Then
				chkToolbar.Enabled = True
				Value -= ACChkEnabled
			Else
				chkToolbar.Enabled = False
			End If
			
			chkToolbar.CheckState = Value
			
		End Set
	End Property
	
	Public Property ViewAvailableTasks() As Integer
		Get
			
			
			Dim iViewAvailableTasks As Integer = chkAvailableTasks.CheckState
			If chkAvailableTasks.Enabled Then
				iViewAvailableTasks += ACChkEnabled
			End If
			
			Return iViewAvailableTasks
			
		End Get
		Set(ByVal Value As Integer)
			
			If Value And ACChkEnabled Then
				chkAvailableTasks.Enabled = True
				Value -= ACChkEnabled
			Else
				chkAvailableTasks.Enabled = False
			End If
			
			chkAvailableTasks.CheckState = Value
			
		End Set
	End Property
	
	Public Property ViewStatusBar() As Integer
		Get
			
			
			Dim iViewStatusBar As Integer = chkStatusBar.CheckState
			If chkStatusBar.Enabled Then
				iViewStatusBar += ACChkEnabled
			End If
			
			Return iViewStatusBar
			
		End Get
		Set(ByVal Value As Integer)
			
			If Value And ACChkEnabled Then
				chkStatusBar.Enabled = True
				Value -= ACChkEnabled
			Else
				chkStatusBar.Enabled = False
			End If
			
			chkStatusBar.CheckState = Value
			
		End Set
	End Property
	
	Public Property ViewSplashScreen() As Integer
		Get
			
			
			Dim iViewSplashScreen As Integer = chkSplashScreen.CheckState
			If chkSplashScreen.Enabled Then
				iViewSplashScreen += ACChkEnabled
			End If
			
			Return iViewSplashScreen
			
		End Get
		Set(ByVal Value As Integer)
			
			If Value And ACChkEnabled Then
				chkSplashScreen.Enabled = True
				Value -= ACChkEnabled
			Else
				chkSplashScreen.Enabled = False
			End If
			
			chkSplashScreen.CheckState = Value
			
		End Set
	End Property
	
	Public Property ViewGridLines() As Integer
		Get
			
			
			Dim iViewGridLines As Integer = chkGridLines.CheckState
			If chkGridLines.Enabled Then
				iViewGridLines += ACChkEnabled
			End If
			
			Return iViewGridLines
			
		End Get
		Set(ByVal Value As Integer)
			
			If Value And ACChkEnabled Then
				chkGridLines.Enabled = True
				Value -= ACChkEnabled
			Else
				chkGridLines.Enabled = False
			End If
			
			chkGridLines.CheckState = Value
			
		End Set
	End Property
	
	Public Property ViewGraphics() As Integer
		Get
			
			
			Dim iViewGraphics As Integer = chkGraphics.CheckState
			If chkGraphics.Enabled Then
				iViewGraphics += ACChkEnabled
			End If
			
			Return iViewGraphics
			
		End Get
		Set(ByVal Value As Integer)
			
			If Value And ACChkEnabled Then
				chkGraphics.Enabled = True
				Value -= ACChkEnabled
			Else
				chkGraphics.Enabled = False
			End If
			
			chkGraphics.CheckState = Value
			
		End Set
	End Property
	
	'DAK110100
	Public Property IsAutoRefresh() As Boolean
		Get
			Return m_bIsAutoRefresh
		End Get
		Set(ByVal Value As Boolean)
			m_bIsAutoRefresh = Value
		End Set
	End Property
	
	Public Property RefreshRate() As Integer
		Get
			Return m_iRefreshRate
		End Get
		Set(ByVal Value As Integer)
			m_iRefreshRate = Value
		End Set
	End Property
	
	'DAK110100
	Private Sub chkAutoRef_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAutoRef.CheckStateChanged
		
		If chkAutoRef.CheckState = CheckState.Checked Then
			lblYN.Text = "Yes"
			fraRefresh.Enabled = True
			lblDsplMins.Enabled = True
			lblDsplMins.Text = CStr(sldRefresh.Value)
			lblMinutes.Enabled = True
			lblSlow.Enabled = True
			lblFast.Enabled = True
			IsAutoRefresh = True
		ElseIf chkAutoRef.CheckState = CheckState.Unchecked Then 
			lblYN.Text = "No"
			fraRefresh.Enabled = False
			lblDsplMins.Enabled = False
			lblDsplMins.Text = ""
			lblMinutes.Enabled = False
			lblSlow.Enabled = False
			lblFast.Enabled = False
			IsAutoRefresh = False
		End If
		
	End Sub
	
	Private Sub chkGraphics_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkGraphics.CheckStateChanged
		
		If chkGraphics.CheckState = CheckState.Unchecked Then
			chkSplashScreen.CheckState = CheckState.Unchecked
			chkSplashScreen.Enabled = False
		Else
			chkSplashScreen.Enabled = True
		End If
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		Me.Hide()
	End Sub
	
	Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
		Cancelled = False
		Me.Hide()
	End Sub
	
	Private Sub frmUserOptions_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			'DAK110100
			sldRefresh.Value = RefreshRate
            'Modified by Vijay Pal on 6/3/2010 12:50:57 PM line added as   lblDsplMins.Text was set before the value set in sldRefresh.Value 
            lblDsplMins.Text = sldRefresh.Value
			If IsAutoRefresh Then
				chkAutoRef.CheckState = CheckState.Checked
			Else
				chkAutoRef.CheckState = CheckState.Unchecked
			End If
			
		End If
	End Sub
	

	Private Sub frmUserOptions_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		Cancelled = True
		
	End Sub
	
	'DAK110100

	Private isInitializingComponent As Boolean
	Private Sub lblDsplMins_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lblDsplMins.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		If lblDsplMins.Text = "1" Then
			lblMinutes.Text = "Minute"
		Else
			lblMinutes.Text = "Minutes"
		End If
		
	End Sub
	
	'DAK110100

    'Modified by Vijay Pal on 6/2/2010 5:50:46 PM Add (ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sldRefresh.Scroll
    Private Sub sldRefresh_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sldRefresh.Scroll
		
		lblDsplMins.Text = CStr(sldRefresh.Value)
		RefreshRate = sldRefresh.Value
		
	End Sub

    Private Sub frmUserOptions_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.Alt And e.KeyCode = Keys.D Then
            SSTab1.SelectedIndex = 0
            SSTab1.Focus()
        ElseIf e.Alt And e.KeyCode = Keys.R Then
            SSTab1.SelectedIndex = 1
            SSTab1.Focus()
        End If
    End Sub
End Class