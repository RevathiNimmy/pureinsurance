Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("BankAccountDetail_NET.BankAccountDetail")> _
Public NotInheritable Class BankAccountDetail 

    Private m_lAccountId As Integer
    Private m_sDescription As String = ""
    Private m_sBankAccountName As String = ""
    Private m_sBankAccountNo As String = ""
    Private m_lCurrencyId As Integer
    Private m_lId As Integer
    Private m_sCode As String = ""
    Private m_iIsCashReceiveInThisCurrencyOnly As Integer


    Public Property AccountId() As Integer
        Get
            Return m_lAccountId
        End Get
        Set(ByVal Value As Integer)
            m_lAccountId = Value
        End Set
    End Property


    Public Property Description() As String
        Get
            Return m_sDescription
        End Get
        Set(ByVal Value As String)
            m_sDescription = Value
        End Set
    End Property


    Public Property BankAccountNo() As String
        Get
            Return m_sBankAccountNo
        End Get
        Set(ByVal Value As String)
            m_sBankAccountNo = Value
        End Set
    End Property


    Public Property Id() As Integer
        Get
            Return m_lId
        End Get
        Set(ByVal Value As Integer)
            m_lId = Value
        End Set
    End Property


    Public Property Code() As String
        Get
            Return m_sCode
        End Get
        Set(ByVal Value As String)
            m_sCode = Value
        End Set
    End Property


    Public Property BankAccountName() As String
        Get
            Return m_sBankAccountName
        End Get
        Set(ByVal Value As String)
            m_sBankAccountName = Value
        End Set
    End Property


    Public Property CurrencyId() As Integer
        Get
            Return m_lCurrencyId
        End Get
        Set(ByVal Value As Integer)
            m_lCurrencyId = Value
        End Set
    End Property


    Public Property IsCashReceiveInThisCurrencyOnly() As Integer
        Get
            Return m_iIsCashReceiveInThisCurrencyOnly
        End Get
        Set(ByVal Value As Integer)
            m_iIsCashReceiveInThisCurrencyOnly = Value
        End Set
    End Property
End Class
