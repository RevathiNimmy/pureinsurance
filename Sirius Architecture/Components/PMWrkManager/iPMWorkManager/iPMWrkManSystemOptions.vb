Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmSystemOptions
	Inherits System.Windows.Forms.Form
	'* Amendment History
	'*
	'* DAK190600 - Add WebTabCaption property
	'* DAK110700 - add MainFormCaption and PMSupportAddress properties
	'******************************************************************************
	
	Private Const ACClass As String = "frmSystemOptions"
	
	'DAK190600
	Private Const ACDefaultWebCaption As String = "&News"
	Private Const ACFixedExplanationPart1 As String = "Enter the "
	Private Const ACFixedExplanationPart2 As String = " Home Page. "
	Private Const ACFixedExplanationPart3 As String = "Clear the text box to disable the "
	Private Const ACFixedExplanationPart4 As String = " Tab for ALL users."
	
	Private m_bCancelled As Boolean
	
	Public Property Cancelled() As Boolean
		Get
			Return m_bCancelled
		End Get
		Set(ByVal Value As Boolean)
			m_bCancelled = Value
		End Set
	End Property
	
	Public Property PMNewsAddress() As String
		Get
			Return txtHomePage.Text.Trim()
		End Get
		Set(ByVal Value As String)
			txtHomePage.Text = Value.Trim()
		End Set
	End Property
	
	'DAK190600
	Public Property WebTabCaption() As String
		Get
			Return txtTabCaption.Text.Trim()
		End Get
		Set(ByVal Value As String)
			txtTabCaption.Text = Value.Trim()
		End Set
	End Property
	
	'DAK110700
	Public Property MainFormCaption() As String
		Get
			Return txtFormCaption.Text.Trim()
		End Get
		Set(ByVal Value As String)
			txtFormCaption.Text = Value.Trim()
		End Set
	End Property
	
	Public Property PMSupportAddress() As String
		Get
			Return txtPMSupport.Text.Trim()
		End Get
		Set(ByVal Value As String)
			txtPMSupport.Text = Value.Trim()
		End Set
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		Me.Hide()
	End Sub
	
	Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
		Cancelled = False
		Me.Hide()
	End Sub
	
	Private Sub frmSystemOptions_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			'DAK190600
			'DAK110700 - No longer required
			'    If WebTabCaption <> "" Then
			'        lblExplanation.Caption = ACFixedExplanationPart1 & _
			'WebTabCaption & _
			'ACFixedExplanationPart2 & _
			'ACFixedExplanationPart3 & _
			'WebTabCaption & _
			'ACFixedExplanationPart4
			'        SSTab1.TabCaption(0) = WebTabCaption
			'    End If
			
			txtHomePage.Focus()
			iPMFunc.SelectText(txtHomePage)
			
		End If
	End Sub
	

	Private Sub frmSystemOptions_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		Cancelled = True
		
	End Sub
	
	'DAK190600 - need to display caption changes when going back to tab 0
	Private Sub SSTab1_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles SSTab1.SelectedIndexChanged
		
		Select Case SSTabHelper.GetSelectedIndex(SSTab1)
			Case 0
				
				'DAK110700 - no longer required
				'            If WebTabCaption = "" Then
				
				'                lblExplanation = ACFixedExplanationPart1 & _
				'ACDefaultWebCaption & _
				'ACFixedExplanationPart2 & _
				'ACFixedExplanationPart3 & _
				'ACDefaultWebCaption & _
				'ACFixedExplanationPart4
				'                SSTab1.TabCaption(0) = ACDefaultWebCaption
				
				'            Else
				
				'                lblExplanation.Caption = ACFixedExplanationPart1 & _
				'WebTabCaption & _
				'ACFixedExplanationPart2 & _
				'ACFixedExplanationPart3 & _
				'WebTabCaption & _
				'ACFixedExplanationPart4
				'                SSTab1.TabCaption(0) = WebTabCaption
				
				'            End If
				
				txtHomePage.Focus()
				iPMFunc.SelectText(txtHomePage)
				
			Case 1
				
				'            txtTabCaption.SetFocus
				'            SelectText txtTabCaption
				
				txtFormCaption.Focus()
				iPMFunc.SelectText(txtFormCaption)
				
			Case Else
				' Do nothing
		End Select
		
		SSTab1PreviousTab = SSTab1.SelectedIndex
	End Sub

    
    Private Sub frmSystemOptions_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.Alt And e.KeyCode = Keys.W Then
            SSTab1.SelectedIndex = 0
            SSTab1.Focus()
        ElseIf e.Alt And e.KeyCode = Keys.A Then
            SSTab1.SelectedIndex = 1
            SSTab1.Focus()
        End If

    End Sub
End Class