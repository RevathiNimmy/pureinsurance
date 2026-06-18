Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("TransactionEntry_NET.TransactionEntry")> _
Public NotInheritable Class TransactionEntry 
	
	' *******************************************************************************
	' PUBLIC ENUMERATOR
	' *******************************************************************************
	Public Enum MarkedStatusEnum
		acmseNotMarked
		acmsePartMarked
		acmseFullyMarked
	End Enum
	' *******************************************************************************
	' PUBLIC ENUMERATOR
	' *******************************************************************************
	
	
	' *******************************************************************************
	' PUBLIC FIELDS (VB WILL AUTOMATICALLY DERIVE THE PUBLIC PROPERTIES)
	' *******************************************************************************
	' Transdetail ID
	Public DetailID As Integer
	
	' Currency details
	Public CurrencyAmount As Decimal
	Public CurrencyID As Integer
	Public CurrencyCode As String = ""
	Public MarkedAmount As Decimal
	Public PaidAmount As Decimal
	
	' Account Currency details
	Public CurrencyAccountAmount As Decimal
	Public AccountCurrencyID As Integer
	Public AccountCurrencyCode As String = ""
	Public MarkedAccountAmount As Decimal
	Public PaidAccountAmount As Decimal
	
	' Additional properties
	Public AccountingDate As Date
	Public CompanyID As Integer
	Public Month As Integer
	Public Period As String = ""
	Public Spare As String = ""
	Public AltRef As String = ""
	
	' Start - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.3)
    Public m_sComment As String = ""
    Public AllocationPeriod As String = ""
	' End - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.3)
	
	' *******************************************************************************
	' PUBLIC FIELDS (VB WILL AUTOMATICALLY DERIVE THE PUBLIC PROPERTIES)
	' *******************************************************************************
	
	
	' *******************************************************************************
	' PUBLIC PROPERTIES
	' *******************************************************************************
	
	' Returns the marked status for this entry
	Public ReadOnly Property IsMarked() As MarkedStatusEnum
		Get
			' Determine 'how' marked out item is
			Select Case MarkedAmount
				Case 0
					Return MarkedStatusEnum.acmseNotMarked
				Case OutstandingAmount
					Return MarkedStatusEnum.acmseFullyMarked
				Case Else
					Return MarkedStatusEnum.acmsePartMarked
			End Select
		End Get
	End Property
	' Start - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.3)
	
	Public Property Comment() As String
		Get
			Return m_sComment
		End Get
		Set(ByVal Value As String)
			m_sComment = Value
		End Set
	End Property
	' End - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.3)
	' A listview compatible string key derived from the DetailID
	Public ReadOnly Property Key() As String
		Get
			Return "td" & DetailID
		End Get
	End Property
	
	' The outstanding amount on this entry
	Public ReadOnly Property OutstandingAmount() As Decimal
		Get
			' Outstanding is currency - paid
			Return CurrencyAmount - PaidAmount
		End Get
	End Property
	
	' The outstanding account amount on this entry
	Public ReadOnly Property OutstandingAccountAmount() As Decimal
		Get
			' Outstanding is currency - paid
			Return CurrencyAccountAmount - PaidAccountAmount
		End Get
	End Property
	' *******************************************************************************
	' PUBLIC PROPERTIES
	' *******************************************************************************
End Class
