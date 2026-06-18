Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
'Developer Guide No. 
Imports SharedFiles
Friend Partial Class frmEntry
	Inherits System.Windows.Forms.Form
	Private Sub frmEntry_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	
	'CLG 12/11/2003 : CQ2929 : Quick Quote Field Becomes Visible
    Private Const vbFormCode As Integer = 3
	Private m_sControlName As String = ""
	Private m_lPreQuote As Integer
	Private m_lPostQuote As Integer
	Private m_lPurchase As Integer
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Public Property ControlName() As String
		Get
			Return m_sControlName
		End Get
		Set(ByVal Value As String)
			m_sControlName = Value
		End Set
	End Property
	
	Public Property PreQuote() As Integer
		Get
			Return m_lPreQuote
		End Get
		Set(ByVal Value As Integer)
			m_lPreQuote = Value
		End Set
	End Property
	
	Public Property PostQuote() As Integer
		Get
			Return m_lPostQuote
		End Get
		Set(ByVal Value As Integer)
			m_lPostQuote = Value
		End Set
	End Property
	
	Public Property Purchase() As Integer
		Get
			Return m_lPurchase
		End Get
		Set(ByVal Value As Integer)
			m_lPurchase = Value
		End Set
	End Property
	
	Public Property Status() As Integer
		Get
			Return m_lStatus
		End Get
		Set(ByVal Value As Integer)
			m_lStatus = Value
		End Set
	End Property
	
	Private Sub cboPostQuote_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPostQuote.SelectedIndexChanged
		
		If cboPurchase.SelectedIndex < cboPostQuote.SelectedIndex Then
			cboPurchase.SelectedIndex = cboPostQuote.SelectedIndex
		End If
		
		If cboPreQuote.SelectedIndex > cboPostQuote.SelectedIndex Then
			cboPreQuote.SelectedIndex = cboPostQuote.SelectedIndex
		End If
		
	End Sub
	
	Private Sub cboPreQuote_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPreQuote.SelectedIndexChanged
		
		If cboPostQuote.SelectedIndex < cboPreQuote.SelectedIndex Then
			cboPostQuote.SelectedIndex = cboPreQuote.SelectedIndex
		End If
		
	End Sub
	
	Private Sub cboPurchase_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPurchase.SelectedIndexChanged
		
		If cboPostQuote.SelectedIndex > cboPurchase.SelectedIndex Then
			cboPostQuote.SelectedIndex = cboPurchase.SelectedIndex
		End If
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		Status = gPMConstants.PMEReturnCode.PMCancel
		Me.Close()
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		Status = gPMConstants.PMEReturnCode.PMOK
		Me.Close()
		
	End Sub
	

	Private Sub frmEntry_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		txtControlName.Text = m_sControlName
		cboPreQuote.SelectedIndex = m_lPreQuote - 1
		cboPostQuote.SelectedIndex = m_lPostQuote - 1
		cboPurchase.SelectedIndex = m_lPurchase - 1
		
	End Sub
	
	Private Sub frmEntry_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		


        If UnloadMode <> vbFormCode Then

            Status = gPMConstants.PMEReturnCode.PMCancel
        End If

        m_lPreQuote = cboPreQuote.SelectedIndex + 1
        m_lPostQuote = cboPostQuote.SelectedIndex + 1
        m_lPurchase = cboPurchase.SelectedIndex + 1

        eventArgs.Cancel = Cancel <> 0
	End Sub
End Class