Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Friend Partial Class frmReceiptItem
	Inherits System.Windows.Forms.Form
	Private m_lClaimId As Integer
	Private m_lClaimPerilId As Integer
	Private m_lClaimReceiptId As Integer
	Private m_sTransactionType As String = ""
	Private m_iTask As Integer
	Private m_dtEffectiveDate As Date
	Private m_lRecoveryMode As Integer
	
	Public WriteOnly Property ClaimId() As Integer
		Set(ByVal Value As Integer)
			m_lClaimId = Value
		End Set
	End Property
	
	Public WriteOnly Property ClaimPerilId() As Integer
		Set(ByVal Value As Integer)
			m_lClaimPerilId = Value
		End Set
	End Property
	
	Public WriteOnly Property ClaimReceiptId() As Integer
		Set(ByVal Value As Integer)
			m_lClaimReceiptId = Value
		End Set
	End Property
	Public WriteOnly Property RecoveryMode() As Integer
		Set(ByVal Value As Integer)
			m_lRecoveryMode = Value
		End Set
	End Property
	
	Public WriteOnly Property TransactionType() As String
		Set(ByVal Value As String)
			m_sTransactionType = Value
		End Set
	End Property
	
	Public WriteOnly Property Task() As Integer
		Set(ByVal Value As Integer)
			m_iTask = Value
		End Set
	End Property
	
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			m_dtEffectiveDate = Value
		End Set
	End Property
	
	Private Sub cmdClose_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClose.Click
		Me.Close()
	End Sub
	

	Private Sub frmReceiptItem_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		Dim kMethodName As String = ""
		
		Dim ActionViewPayment As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
		
		'**********************************************
        'developer guide no.9
        Dim lReturn As gPMConstants.PMEReturnCode = uctCLMReceipt.Initialise()
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "uctCLMReceipt.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		'**********************************************
		
		uctCLMReceipt.RecoveryMode = m_lRecoveryMode
		
		'**********************************************
		
		lReturn = uctCLMReceipt.SetProcessModes(vTask:=m_iTask, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "uctCLMReceipt.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		'**********************************************
		
		uctCLMReceipt.ClaimID = m_lClaimId
		uctCLMReceipt.ClaimPerilId = m_lClaimPerilId
		uctCLMReceipt.ClaimReceiptId = m_lClaimReceiptId
		
		'**********************************************
        'developer guide no.68
        lReturn = uctCLMReceipt.Load_Renamed()
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "uctCLMPayment.Load Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
	End Sub

    Private Sub frmReceiptItem_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            DirectCast(uctCLMReceipt.Controls("SSTab1"), TabControl).SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            DirectCast(uctCLMReceipt.Controls("SSTab1"), TabControl).SelectedIndex = 1
        End If
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D3 Then
            DirectCast(uctCLMReceipt.Controls("SSTab1"), TabControl).SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D4 Then
            DirectCast(uctCLMReceipt.Controls("SSTab1"), TabControl).SelectedIndex = 1
        End If
    End Sub
End Class