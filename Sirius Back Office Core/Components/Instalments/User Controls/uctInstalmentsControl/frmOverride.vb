Option Strict Off
Option Explicit On
Imports System
Imports System.Drawing
Imports System.Windows.Forms

Imports SharedFiles
Friend Partial Class frmOverride
	Inherits System.Windows.Forms.Form
	Private m_bPaymentProtection As Boolean
	Private m_bOverride As Boolean
	Private m_dOverrideRate As Double
	Private m_sOverrideReference As String = ""
	Private m_dDepositOverride As Double
	Private m_sProductCode As String = ""
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Private m_bAllowRateOverride As Boolean
	Private m_bAllowDepositOverride As Boolean
	Private m_bAllowCommissionOverride As Boolean
    Private m_bAllowPaymentProtection As Boolean
    Private m_bSuppressDecimalValues As Boolean
	
	Public WriteOnly Property AllowPaymentProtection() As Boolean
		Set(ByVal Value As Boolean)
			m_bAllowPaymentProtection = Value
		End Set
	End Property
	
	Public WriteOnly Property AllowCommissionOverride() As Boolean
		Set(ByVal Value As Boolean)
			m_bAllowCommissionOverride = Value
		End Set
	End Property
	
	Public WriteOnly Property AllowDepositOverride() As Boolean
		Set(ByVal Value As Boolean)
			m_bAllowDepositOverride = Value
		End Set
	End Property
	
	Public WriteOnly Property AllowRateOverride() As Boolean
		Set(ByVal Value As Boolean)
			m_bAllowRateOverride = Value
		End Set
	End Property
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	Public WriteOnly Property ProductCode() As String
		Set(ByVal Value As String)
			m_sProductCode = Value
		End Set
	End Property
	
	
	Public Property OverrideReference() As String
		Get
			Return m_sOverrideReference
		End Get
		Set(ByVal Value As String)
			m_sOverrideReference = Value
		End Set
	End Property
	
	Public ReadOnly Property Override() As Boolean
		Get
			Return m_bOverride
		End Get
	End Property
	
	
	Public Property OverrideRate() As Double
		Get
			Return m_dOverrideRate
		End Get
		Set(ByVal Value As Double)
			m_dOverrideRate = Value
		End Set
	End Property
	
	
	Public Property PaymentProtection() As Boolean
		Get
			Return m_bPaymentProtection
		End Get
		Set(ByVal Value As Boolean)
			m_bPaymentProtection = Value
		End Set
	End Property
	
	
	Public Property DepositOverride() As Double
		Get
			Return m_dDepositOverride
		End Get
		Set(ByVal Value As Double)
			m_dDepositOverride = Value
		End Set
    End Property

    Public WriteOnly Property SuppressDecimalValues() As Boolean
        Set(ByVal Value As Boolean)
            m_bSuppressDecimalValues = Value
        End Set
    End Property
	
	Private Sub chkCommissionOverride_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkCommissionOverride.CheckStateChanged
		txtOverrideReference.Enabled = chkCommissionOverride.CheckState
		If chkCommissionOverride.CheckState Then
			txtOverrideReference.BackColor = SystemColors.Window
		Else
			txtOverrideReference.BackColor = SystemColors.Control
		End If
	End Sub
	
	Private Sub chkDepositOverride_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkDepositOverride.CheckStateChanged
		txtOverrideDeposit.Enabled = chkDepositOverride.CheckState
		If chkDepositOverride.CheckState Then
			txtOverrideDeposit.BackColor = SystemColors.Window
		Else
			txtOverrideDeposit.BackColor = SystemColors.Control
		End If
	End Sub
	
	Private Sub chkOverrideInterestRate_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkOverrideInterestRate.CheckStateChanged
		txtNewRate.Enabled = chkOverrideInterestRate.CheckState
		If chkOverrideInterestRate.CheckState Then
			txtNewRate.BackColor = SystemColors.Window
		Else
			txtNewRate.BackColor = SystemColors.Control
		End If
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		Hide()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		Dim sNewRate As String = ""
        m_lStatus = gPMConstants.PMEReturnCode.PMTrue
		
		'payment protection
		'TR - Get the new user input but don't recalculate unless they have
		'moved away from this tab
		m_bPaymentProtection = chkPaymentProtection.CheckState = CheckState.Checked
		
		m_bOverride = chkOverrideInterestRate.CheckState
		m_dOverrideRate = -1
		If m_bOverride Then
			'non pclsg rate
            m_dOverrideRate = ToSafeDouble(txtNewRate.Text)
		End If
		
		m_dDepositOverride = -1
		If chkDepositOverride.CheckState Then
			'deposit override

            m_dDepositOverride = ToSafeDouble(txtOverrideDeposit.Text)
		End If
		
		If chkCommissionOverride.CheckState Then
			'commission override
			m_sOverrideReference = txtOverrideReference.Text
		End If
		
		Hide()
	End Sub
	

	Private Sub frmOverride_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		txtOverrideReference.Text = m_sOverrideReference
        If m_dDepositOverride > 0 Then txtOverrideDeposit.Text = gPMFunctions.ToSafeString(m_dDepositOverride)
        If m_dOverrideRate > 0 Then txtNewRate.Text = gPMFunctions.ToSafeString(m_dOverrideRate)
		chkCommissionOverride.Enabled = m_bAllowCommissionOverride
		chkDepositOverride.Enabled = m_bAllowDepositOverride
		chkOverrideInterestRate.Enabled = m_bAllowRateOverride
		chkPaymentProtection.Enabled = m_bAllowPaymentProtection
	End Sub

    Private Sub txtOverrideDeposit_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtOverrideDeposit.KeyPress
        If m_bSuppressDecimalValues Then
            'Disallow the decimals
            gPMFunctions.NumPress(sender, e)
        End If
    End Sub
End Class