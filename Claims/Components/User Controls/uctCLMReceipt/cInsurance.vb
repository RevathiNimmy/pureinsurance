Option Strict Off
Option Explicit On
Imports System

<System.Runtime.InteropServices.ProgId("cInsurance_NET.cInsurance")> _
Public NotInheritable Class cInsurance 
	
	Private m_lRecoveryId As Integer
	Private m_crThisRecoveryAmount As Decimal
	Private m_crThisRecoveryTaxAmount As Decimal
	Private m_sRecoveryTypeDescription As String = ""
	Private m_lPartyCnt As Integer
	Private m_sPartyName As String = ""
	Private m_crSharePercentage As Decimal
	Private m_crRecoveryToDateLC As Decimal ' IMPORTANT NB : RecoveryToDate is loaded in loss currency
	Private m_lIsTaxShared As Integer
	Private m_vRIArrangementLineId As Object
	Private m_vTreatyId As Object
	Private m_crThisRecoverySplitAmount As Decimal
	Private m_crThisRecoverySplitAmountLC As Decimal
	Private m_crThisRecoveryTaxAmountLC As Decimal
	Private m_crThisRecoveryAmountLC As Decimal
	Private m_crReceiptToLossXRate As Decimal
	Private m_crThisRecoverySplitTaxAmountLC As Decimal
	Private m_crThisRecoverySplitTaxAmount As Decimal
	'Gaurav
	Private m_cPaidToDate As Decimal
	Private m_sRIType As String = ""
	Private m_cLowerLimit As Decimal
	Private m_cUpperLimit As Decimal
	'**************************************************************
	
	Public ReadOnly Property ThisRecoverySplitTaxAmountLC() As Decimal
		Get
			Return m_crThisRecoverySplitTaxAmount * m_crReceiptToLossXRate
		End Get
	End Property
	
	'**************************************************************
	
	
	Public Property ThisRecoverySplitTaxAmount() As Decimal
		Get
			Return m_crThisRecoverySplitTaxAmount
		End Get
		Set(ByVal Value As Decimal)
			m_crThisRecoverySplitTaxAmount = Value
		End Set
	End Property
	
	'**************************************************************
	
	Public WriteOnly Property ReceiptToLossXRate() As Decimal
		Set(ByVal Value As Decimal)
			m_crReceiptToLossXRate = Value
		End Set
	End Property
	
	'**************************************************************
	
	Public ReadOnly Property ThisRecoveryTaxAmountLC() As Decimal
		Get
			Return m_crThisRecoveryTaxAmount * m_crReceiptToLossXRate
		End Get
	End Property
	
	'**************************************************************
	
	Public ReadOnly Property ThisRecoverySplitAmountLC() As Decimal
		Get
			Return m_crThisRecoverySplitAmount * m_crReceiptToLossXRate
		End Get
	End Property
	
	'**************************************************************
	
	Public ReadOnly Property ThisRecoveryAmountLC() As Decimal
		Get
			Return m_crThisRecoveryAmount * m_crReceiptToLossXRate
		End Get
	End Property
	
	'**************************************************************
	
	
	Public Property ThisRecoverySplitAmount() As Decimal
		Get
			Return m_crThisRecoverySplitAmount
		End Get
		Set(ByVal Value As Decimal)
			m_crThisRecoverySplitAmount = Value
		End Set
	End Property
	
	'**************************************************************
	
	
	Public Property TreatyId() As Object
		Get
			Return m_vTreatyId
		End Get
		Set(ByVal Value As Object)


			m_vTreatyId = Value
		End Set
	End Property
	
	'**************************************************************
	
	
	Public Property RIArrangementLineId() As Object
		Get
			Return m_vRIArrangementLineId
		End Get
		Set(ByVal Value As Object)


			m_vRIArrangementLineId = Value
		End Set
	End Property
	
	'**************************************************************
	
	
	Public Property IsTaxShared() As Integer
		Get
			Return m_lIsTaxShared
		End Get
		Set(ByVal Value As Integer)
			m_lIsTaxShared = Value
		End Set
	End Property
	
	'**************************************************************
	' NB : Recovery To Date is loaded in loss currency
	
	Public Property RecoveryToDateLC() As Decimal
		Get
			Return m_crRecoveryToDateLC
		End Get
		Set(ByVal Value As Decimal)
			m_crRecoveryToDateLC = Value
		End Set
	End Property
	
	'**************************************************************
	
	
	Public Property SharePercentage() As Decimal
		Get
			Return m_crSharePercentage
		End Get
		Set(ByVal Value As Decimal)
			m_crSharePercentage = Value
		End Set
	End Property
	
	'**************************************************************
	
	
	Public Property PartyName() As String
		Get
			Return m_sPartyName
		End Get
		Set(ByVal Value As String)
			m_sPartyName = Value
		End Set
	End Property
	
	'**************************************************************
	
	
	Public Property PartyCnt() As Integer
		Get
			Return m_lPartyCnt
		End Get
		Set(ByVal Value As Integer)
			m_lPartyCnt = Value
		End Set
	End Property
	
	'**************************************************************
	
	
	Public Property RecoveryTypeDescription() As String
		Get
			Return m_sRecoveryTypeDescription
		End Get
		Set(ByVal Value As String)
			m_sRecoveryTypeDescription = Value
		End Set
	End Property
	
	'**************************************************************
	
	
	Public Property ThisRecoveryTaxAmount() As Decimal
		Get
			Return m_crThisRecoveryTaxAmount
		End Get
		Set(ByVal Value As Decimal)
			m_crThisRecoveryTaxAmount = Value
		End Set
	End Property
	
	'**************************************************************
	
	
	Public Property ThisRecoveryAmount() As Decimal
		Get
			Return m_crThisRecoveryAmount
		End Get
		Set(ByVal Value As Decimal)
			m_crThisRecoveryAmount = Value
		End Set
	End Property
	
	'**************************************************************
	
	
	Public Property RecoveryId() As Integer
		Get
			Return m_lRecoveryId
		End Get
		Set(ByVal Value As Integer)
			m_lRecoveryId = Value
		End Set
	End Property
	
	'**************************************************************
	
	Public Property PaidToDate() As Decimal
		Get
			Return m_cPaidToDate
		End Get
		Set(ByVal Value As Decimal)
			m_cPaidToDate = Value
		End Set
	End Property
	
	
	Public Property RIType() As String
		Get
			Return m_sRIType
		End Get
		Set(ByVal Value As String)
			m_sRIType = Value
		End Set
	End Property
	
	
	Public Property LowerLimit() As Decimal
		Get
			Return m_cLowerLimit
		End Get
		Set(ByVal Value As Decimal)
			m_cLowerLimit = Value
		End Set
	End Property
	
	
	Public Property UpperLimit() As Decimal
		Get
			Return m_cUpperLimit
		End Get
		Set(ByVal Value As Decimal)
			m_cUpperLimit = Value
		End Set
	End Property
	
	'**************************************************************
End Class
