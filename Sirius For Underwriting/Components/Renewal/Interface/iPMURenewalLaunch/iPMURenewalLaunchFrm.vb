Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' Status
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_bCanTransferBroker As Boolean
	Private m_lRenwalMode As Integer
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	Public ReadOnly Property RenewalMode() As Integer
		Get
			Return m_lRenwalMode
		End Get
	End Property
	
	Public WriteOnly Property CanTransferBroker() As Boolean
		Set(ByVal Value As Boolean)
			m_bCanTransferBroker = Value
		End Set
	End Property
	
	Private Sub cmdButton_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdButton_0.Click, _cmdButton_2.Click, _cmdButton_3.Click, _cmdButton_1.Click
		Dim Index As Integer = Array.IndexOf(cmdButton, eventSender)
		If Index = 3 Then
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		Else
			' 2 = transfer, 3 = amend, 4 = accept (1 is IAG)
			m_lRenwalMode = Index + 2
		End If
		
		Me.Hide()
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		iPMFunc.ShowFormInTaskBar_Attach()
	End Sub
	

	Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		iPMFunc.ShowFormInTaskBar_Detach()
		
		If Not m_bCanTransferBroker Then
			cmdButton(0).Visible = False
			Me.Width = VB6.TwipsToPixelsX(3855)
			cmdButton(1).Left = VB6.TwipsToPixelsX(120)
			cmdButton(2).Left = VB6.TwipsToPixelsX(1320)
			cmdButton(3).Left = VB6.TwipsToPixelsX(2520)
		End If
	End Sub
End Class