Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Friend Partial Class frmRiskCodeDetails
	Inherits System.Windows.Forms.Form
	Private Sub frmRiskCodeDetails_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_bRiskTransferAgreement As Boolean
	Private m_bDelegatedAuthority As Boolean
	Private m_sRiskCodeDesc As String = ""
	
	Public Property Status() As Integer
		Get
			Return m_lStatus
		End Get
		Set(ByVal Value As Integer)
			m_lStatus = Value
		End Set
	End Property
	
	Public Property RiskTransferAgreement() As Boolean
		Get
			Return m_bRiskTransferAgreement
		End Get
		Set(ByVal Value As Boolean)
			m_bRiskTransferAgreement = Value
		End Set
	End Property
	
	Public Property DelegatedAuthority() As Boolean
		Get
			Return m_bDelegatedAuthority
		End Get
		Set(ByVal Value As Boolean)
			m_bDelegatedAuthority = Value
		End Set
	End Property
	
	Public Property RiskCodeDesc() As String
		Get
			Return m_sRiskCodeDesc
		End Get
		Set(ByVal Value As String)
			m_sRiskCodeDesc = Value
		End Set
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		Me.Hide()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		
		m_bRiskTransferAgreement = chkRiskTransferAgreement.CheckState = CheckState.Checked
		
		m_bDelegatedAuthority = chkDelegatedAuthority.CheckState = CheckState.Checked
		
		Me.Hide()
	End Sub

    Private Sub frmRiskCodeDetails_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Me.Text = m_sRiskCodeDesc

        If m_bRiskTransferAgreement Then
            chkRiskTransferAgreement.CheckState = CheckState.Checked
        Else
            chkRiskTransferAgreement.CheckState = CheckState.Unchecked
        End If

        If m_bDelegatedAuthority Then
            chkDelegatedAuthority.CheckState = CheckState.Checked
        Else
            chkDelegatedAuthority.CheckState = CheckState.Unchecked
        End If

    End Sub
	
	Private Sub frmRiskCodeDetails_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		If UnloadMode = 0 Then
			Cancel = 1
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			Me.Hide()
		End If
		
		eventArgs.Cancel = Cancel <> 0
	End Sub
End Class