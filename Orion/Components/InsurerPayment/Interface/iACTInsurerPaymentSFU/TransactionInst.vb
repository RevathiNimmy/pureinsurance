Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("TransactionInst_NET.TransactionInst")> _
Public NotInheritable Class TransactionInst 
  
    ' Collection for all transaction entries
    Private m_cEntries As New Collection
    Private m_cInstalmentEntries As Collection
    ' Start - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc)
    Private Const ACClass As String = "TransactionInst"
    ' *******************************************************************************
    ' PUBLIC ENUMERATOR
    ' *******************************************************************************
    Public Enum MarkedStatusEnumInstalment
        acmseNotMarkedInstalment
        acmsePartMarkedInstalment
        acmseFullyMarkedInstalment
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
    Public CurrencyCode As String
    Public MarkedAmount As Decimal
    Public PaidAmount As Decimal

    ' Account Currency details
    Public CurrencyAccountAmount As Decimal
    Public AccountCurrencyID As Integer
    Public AccountCurrencyCode As String
    Public MarkedAccountAmount As Decimal
    Public PaidAccountAmount As Decimal

    ' Additional properties
    Public AccountingDate As Date
    Public CompanyID As Integer
    Public Month_Renamed As Integer
    Public Period As String
    Public Spare As String
    Public AltRef As String
    Public m_sComment As String
    Public InstalmentNumber As Integer
    Public DueDate As Date
    Public PremiumFinanceCnt As Integer
    Public PremiumFinanceVersion As Integer
    Public PFInstalmentsId As Integer
    ' *******************************************************************************
    ' PUBLIC FIELDS (VB WILL AUTOMATICALLY DERIVE THE PUBLIC PROPERTIES)
    ' *******************************************************************************


    ' *******************************************************************************
    ' PUBLIC PROPERTIES
    ' *******************************************************************************

    ' Returns the marked status for this entry
    Public ReadOnly Property IsMarked() As MarkedStatusEnumInstalment
        Get
            ' Determine 'how' marked out item is
            Select Case MarkedAmount
                Case 0
                    IsMarked = MarkedStatusEnumInstalment.acmseNotMarkedInstalment
                Case OutstandingAmount
                    IsMarked = MarkedStatusEnumInstalment.acmseFullyMarkedInstalment
                Case Else
                    IsMarked = MarkedStatusEnumInstalment.acmsePartMarkedInstalment
            End Select
        End Get
    End Property
   
    Public Property Comment() As String
        Get
            Comment = m_sComment
        End Get
        Set(ByVal Value As String)
            m_sComment = Value
        End Set
    End Property
    ' A listview compatible string key derived from the DetailID
    Public ReadOnly Property Key() As String
        Get
            Key = "td" & InstalmentNumber & DetailID & PremiumFinanceVersion
        End Get
    End Property

    ' The outstanding amount on this entry
    Public ReadOnly Property OutstandingAmount() As Decimal
        Get
            ' Outstanding is currency - paid
            OutstandingAmount = (CurrencyAmount - PaidAmount)
        End Get
    End Property

    ' The outstanding account amount on this entry
    Public ReadOnly Property OutstandingAccountAmount() As Decimal
        Get
            ' Outstanding is currency - paid
            OutstandingAccountAmount = (CurrencyAccountAmount - PaidAccountAmount)
        End Get
    End Property
    ' *******************************************************************************
    ' PUBLIC PROPERTIES
    ' *******************************************************************************
End Class
