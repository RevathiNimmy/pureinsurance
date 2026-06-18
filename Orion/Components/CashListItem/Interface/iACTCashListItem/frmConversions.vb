Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Friend Partial Class frmConversions
	Inherits System.Windows.Forms.Form
	Private Sub frmConversions_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	
	Private Const ACClass As String = "frmConversions"
	
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	'Input
	Private m_lAccountID As Integer
	Private m_lSourceID As Integer
	Private m_lCurrencyID As Integer
	Private m_cAmount As Decimal
	Private m_dtEffectiveDate As Date
	Private m_bDoNotShow As Boolean
	
	'Output
	Private m_dtCurrencyBaseDate As Date
	Private m_dCurrencyBaseXrate As Double
	Private m_dtAccountBaseDate As Date
	Private m_dAccountBaseXrate As Double
	Private m_dtSystemBaseDate As Date
	Private m_dSystemBaseXrate As Double
	Private m_lOverrideReason As Integer
    Private m_cBaseAmount As Decimal
	
    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property
	
	Public Property AccountID() As Integer
		Get
			Return m_lAccountID
		End Get
		Set(ByVal Value As Integer)
			m_lAccountID = Value
		End Set
	End Property
	
	Public Property SourceID() As Integer
		Get
			Return m_lSourceID
		End Get
		Set(ByVal Value As Integer)
			m_lSourceID = Value
		End Set
	End Property
	
	Public Property CurrencyID() As Integer
		Get
			Return m_lCurrencyID
		End Get
		Set(ByVal Value As Integer)
			m_lCurrencyID = Value
		End Set
	End Property
	
	Public Property Amount() As Decimal
		Get
			Return m_cAmount
		End Get
		Set(ByVal Value As Decimal)
			m_cAmount = Value
		End Set
	End Property
	
	Public Property EffectiveDate() As Date
		Get
			Return m_dtEffectiveDate
		End Get
		Set(ByVal Value As Date)
			m_dtEffectiveDate = Value
		End Set
	End Property
	
	Public Property CurrencyBaseDate() As Date
		Get
			Return m_dtCurrencyBaseDate
		End Get
		Set(ByVal Value As Date)
			m_dtCurrencyBaseDate = Value
		End Set
	End Property
	
	Public Property CurrencyBaseXRate() As Double
		Get
			Return m_dCurrencyBaseXrate
		End Get
		Set(ByVal Value As Double)
			m_dCurrencyBaseXrate = Value
		End Set
	End Property
	
	Public Property AccountBaseDate() As Date
		Get
			Return m_dtAccountBaseDate
		End Get
		Set(ByVal Value As Date)
			m_dtAccountBaseDate = Value
		End Set
	End Property
	
	Public Property AccountBaseXrate() As Double
		Get
			Return m_dAccountBaseXrate
		End Get
		Set(ByVal Value As Double)
			m_dAccountBaseXrate = Value
		End Set
	End Property
	
	Public Property SystemBaseDate() As Date
		Get
			Return m_dtSystemBaseDate
		End Get
		Set(ByVal Value As Date)
			m_dtSystemBaseDate = Value
		End Set
	End Property
	
	Public Property SystemBaseXrate() As Double
		Get
			Return m_dSystemBaseXrate
		End Get
		Set(ByVal Value As Double)
			m_dSystemBaseXrate = Value
		End Set
	End Property
	
	Public Property OverrideReason() As Integer
		Get
			Return m_lOverrideReason
		End Get
		Set(ByVal Value As Integer)
			m_lOverrideReason = Value
		End Set
	End Property
	
	Public WriteOnly Property DoNotShow() As Boolean
		Set(ByVal Value As Boolean)
			m_bDoNotShow = Value
		End Set
	End Property
	
	Public ReadOnly Property BaseAmount() As Decimal
		Get
			Return m_cBaseAmount
		End Get
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		Me.Hide()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		m_lReturn = CType(GetReturnValues(), gPMConstants.PMEReturnCode)
		
		If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			Me.Hide()
		End If
	End Sub
	

    Public Sub frmConversions_Load()

        'developer guide no. 9
        uctConversions.Initialise()

        uctConversions.ShowAccountCurrency = True
        uctConversions.ShowSystemCurrency = True
        uctConversions.EnableTransactionGroup = True
        uctConversions.EnableBaseGroup = True
        uctConversions.EnableAccountGroup = True
        uctConversions.EnableSystemGroup = True
        uctConversions.AccountID = m_lAccountID
        uctConversions.SourceID = m_lSourceID
        uctConversions.TransactionCurrencyID = m_lCurrencyID
        uctConversions.TransactionAmount = m_cAmount
        uctConversions.EffectiveDateOfExchange = m_dtEffectiveDate

        uctConversions.LoadControl()

        'If transaction currency is same as base currency or the user doesn't want to see
        'the conversions then we don't want to show this form,
        'but we do still want the return values, hence the cmdOK_Click.
        If (m_lCurrencyID = uctConversions.BaseCurrencyID) Or m_bDoNotShow Then
            m_lReturn = CType(GetReturnValues(), gPMConstants.PMEReturnCode)
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Else
            m_lStatus = gPMConstants.PMEReturnCode.PMOK
        End If

    End Sub
	
	Private Function GetReturnValues() As Integer
		Dim result As Integer = 0
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_lReturn = uctConversions.OKClick()
		
		If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
			m_dtCurrencyBaseDate = uctConversions.EffectiveDateOfExchange
			m_dCurrencyBaseXrate = uctConversions.BaseExchangeRate
			m_dtAccountBaseDate = uctConversions.EffectiveDateOfExchange
			m_dAccountBaseXrate = uctConversions.AccountExchangeRate
			m_dtSystemBaseDate = uctConversions.EffectiveDateOfExchange
			m_dSystemBaseXrate = uctConversions.SystemExchangeRate
			m_lOverrideReason = uctConversions.RateOverrideReasonID
			m_cBaseAmount = uctConversions.BaseAmount
		End If
		
		Return m_lReturn
		
	End Function
End Class