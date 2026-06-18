Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no 129. 
Imports SharedFiles

Friend Partial Class frmReserve
	Inherits System.Windows.Forms.Form
	Private Sub frmReserve_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	Private m_sPaymentLine As String = ""
	Private m_sLossCurrency As String = ""
	Private m_crInitialReserve As Decimal
	Private m_crRevisedReserve As Decimal
	Private m_bOpenClaimNoTrans As Boolean
	Public status As gPMConstants.PMEReturnCode
    Private m_sTransactionType As String
    Private m_bRI2007Enabled As Boolean

    Public Property RI2007Enabled() As Boolean
        Get
            Return m_bRI2007Enabled
        End Get
        Set(ByVal Value As Boolean)
            m_bRI2007Enabled = Value
        End Set
    End Property
    Public Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property

    Public WriteOnly Property PaymentLine() As String
        Set(ByVal Value As String)
            m_sPaymentLine = Value
        End Set
    End Property

    Public WriteOnly Property LossCurrency() As String
        Set(ByVal Value As String)
            m_sLossCurrency = Value
        End Set
    End Property

    Public Property InitialReserve() As Decimal
        Get
            Return m_crInitialReserve
        End Get
        Set(ByVal Value As Decimal)
            m_crInitialReserve = Value
        End Set
    End Property

    Public Property RevisedReserve() As Decimal
        Get
            Return m_crRevisedReserve
        End Get
        Set(ByVal Value As Decimal)
            m_crRevisedReserve = Value
        End Set
    End Property

    Public WriteOnly Property IsOpenClaimNoTrans() As Boolean
        Set(ByVal Value As Boolean)
            m_bOpenClaimNoTrans = Value
        End Set
    End Property

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        status = gPMConstants.PMEReturnCode.PMCancel
        Me.Hide()
    End Sub

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
        status = gPMConstants.PMEReturnCode.PMOK
        Dim dbNumericTemp As Double
        If Not Double.TryParse(txtRevisionAmount.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            MessageBox.Show("Invalid Amount", "Reserve", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        m_crRevisedReserve = CDec(txtRevisionAmount.Text)
        If m_bOpenClaimNoTrans Then
            If InitialReserve <= 0 And m_crRevisedReserve < 0 Then
                MessageBox.Show("Revision Amount cannot be negative", "Reserve", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
        End If

        'PN 76954
        If m_sTransactionType = "C_CP" And m_bRI2007Enabled = True Then
            If m_crRevisedReserve < 0 And Math.Abs(m_crRevisedReserve) > InitialReserve Then
                MsgBox("Revision Amount should not make the Initial reserve negative", vbInformation, "Reserve")
                Exit Sub
            End If
        End If
        Me.Hide()
    End Sub


    Private Sub frmReserve_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        txtPaymentLine.Text = m_sPaymentLine
        txtLossCurrency.Text = m_sLossCurrency

        txtInitialReserve.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_crInitialReserve)
        If txtInitialReserve.Text = "" Then
            txtInitialReserve.Text = "0.00"
        End If

        'txtRevisionAmount.Text = FormatField(PMFormatCurrency, m_crRevisedReserve) 'PN 34567
        If txtRevisionAmount.Text = "" Then
            txtRevisionAmount.Text = "0.00"
        End If

    End Sub

    Private Sub txtRevisionAmount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRevisionAmount.Enter
        iPMFunc.SelectText(txtRevisionAmount)
    End Sub

    Private Sub txtRevisionAmount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRevisionAmount.Leave
        If txtRevisionAmount.Text.Trim() <> "" Then
            txtRevisionAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, txtRevisionAmount.Text)
        End If
    End Sub
End Class