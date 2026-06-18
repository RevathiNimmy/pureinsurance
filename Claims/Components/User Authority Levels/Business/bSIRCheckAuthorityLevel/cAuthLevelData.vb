Option Strict Off
Option Explicit On
Public NotInheritable Class cAuthLevelData

    Private Const ACClass As String = "cAuthLevelData"

    Private m_lAuthError As Integer 'Script Error Value
    Private m_lAuthUserID As Integer 'User who performed original action
    Private m_lCurrentUserID As Integer 'User Performing Operation
    Private m_bIsAuthorised As Boolean 'User Action Authorised (true / false)
    Private m_crPaymentAmount As Double 'Amount of Payment to be authorised
    Private m_lPaymentType As Integer 'Type of Payment
    Private m_lProductID As Integer 'Product Id
    Private m_lReference As Integer 'Reference Id (Claim Id)
    Private m_lTransType As Integer 'Transaction Type
    Private m_sCurrencyCode As String = "" 'Currency ISO Code of Amount
    Private m_crPaymentCurrencyAmount As Double 'Amount of Payment to be authorised in Payment Currency
    Private m_sPaymentCurrencyCode As String = "" 'Payment Currency ISO Code of Amount

    Public Property AuthError() As Integer
        Get
            Return m_lAuthError
        End Get
        Set(ByVal Value As Integer)
            m_lAuthError = Value
        End Set
    End Property
    Public Property AuthUserID() As Integer
        Get
            Return m_lAuthUserID
        End Get
        Set(ByVal Value As Integer)
            m_lAuthUserID = Value
        End Set
    End Property
    Public Property CurrentUserID() As Integer
        Get
            Return m_lCurrentUserID
        End Get
        Set(ByVal Value As Integer)
            m_lCurrentUserID = Value
        End Set
    End Property
    Public Property IsAuthorised() As Boolean
        Get
            Return m_bIsAuthorised
        End Get
        Set(ByVal Value As Boolean)
            m_bIsAuthorised = Value
        End Set
    End Property
    Public Property PaymentAmount() As Double
        Get
            Return m_crPaymentAmount
        End Get
        Set(ByVal Value As Double)
            m_crPaymentAmount = Value
        End Set
    End Property
    Public Property PaymentCurrencyAmount() As Double
        Get
            Return m_crPaymentCurrencyAmount
        End Get
        Set(ByVal Value As Double)
            m_crPaymentCurrencyAmount = Value
        End Set
    End Property
    Public Property PaymentType() As Integer
        Get
            Return m_lPaymentType
        End Get
        Set(ByVal Value As Integer)
            m_lPaymentType = Value
        End Set
    End Property
    Public Property ProductID() As Integer
        Get
            Return m_lProductID
        End Get
        Set(ByVal Value As Integer)
            m_lProductID = Value
        End Set
    End Property
    Public Property Reference() As Integer
        Get
            Return m_lReference
        End Get
        Set(ByVal Value As Integer)
            m_lReference = Value
        End Set
    End Property
    Public Property TransType() As Integer
        Get
            Return m_lTransType
        End Get
        Set(ByVal Value As Integer)
            m_lTransType = Value
        End Set
    End Property
    Public Property CurrencyCode() As String
        Get
            Return m_sCurrencyCode
        End Get
        Set(ByVal Value As String)
            m_sCurrencyCode = Value
        End Set
    End Property
    Public Property PaymentCurrencyCode() As String
        Get
            Return m_sPaymentCurrencyCode
        End Get
        Set(ByVal Value As String)
            m_sPaymentCurrencyCode = Value
        End Set
    End Property

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
