Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmPaymentItem
	Inherits System.Windows.Forms.Form
	Private m_lWorkClaimId As Integer
	Private m_lWorkClaimPerilId As Integer
	Private m_lWorkClaimPaymentId As Integer
	Private m_sTransactionType As String = ""
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_dtEffectiveDate As Date
	
	Public WriteOnly Property WorkClaimId() As Integer
		Set(ByVal Value As Integer)
			m_lWorkClaimId = Value
		End Set
	End Property
	
	Public WriteOnly Property WorkClaimPerilId() As Integer
		Set(ByVal Value As Integer)
			m_lWorkClaimPerilId = Value
		End Set
	End Property
	
	Public WriteOnly Property WorkClaimPaymentId() As Integer
		Set(ByVal Value As Integer)
			m_lWorkClaimPaymentId = Value
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
	

	Private Sub frmPaymentItem_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim kMethodName As String = ""
		
		Dim ActionViewPayment As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
		
		'**********************************************
		
        Dim lReturn As gPMConstants.PMEReturnCode = uctCLMPayment.Initialise()
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "uctCLMPayment.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		'**********************************************
		
		uctCLMPayment.ViewPaymentMode = True
		
		'**********************************************
		
		lReturn = uctCLMPayment.SetProcessModes(vTask:=m_iTask, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "uctCLMPayment.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		'**********************************************
		
		uctCLMPayment.WorkClaimID = m_lWorkClaimId
		uctCLMPayment.WorkClaimPerilId = m_lWorkClaimPerilId
		uctCLMPayment.WorkClaimPaymentId = m_lWorkClaimPaymentId
		
		'**********************************************
		
        lReturn = uctCLMPayment.Load_Renamed()
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "uctCLMPayment.Load Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub frmPaymentItem_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		'Dim lWidth As Long
		'    Dim lHeight As Long
		'
		'    If Me.Width < 13350 Then
		'        Me.Width = 13350
		'    End If
		'
		'    If Me.Height < 7770 Then
		'        Me.Height = 7770
		'    End If
		'
		'    lWidth = Me.ScaleWidth - (uctCLMPayment.Left * 2)
		'    lHeight = Me.ScaleHeight - (uctCLMPayment.Top * 3) - cmdClose.Height
		'
		'    uctCLMPayment.Width = lWidth
		'    uctCLMPayment.Height = lHeight
		'    'lvwPaymentDetails.Width = lWidth - (lvwPayments.Left * 2)
		'    'lvwPayments.Height = lHeight - 600
		'
		'    cmdClose.Left = Me.ScaleWidth - cmdClose.Width - uctCLMPayment.Left
		'    cmdClose.Top = Me.ScaleHeight - cmdClose.Height - uctCLMPayment.Top
	End Sub

   

   
End Class