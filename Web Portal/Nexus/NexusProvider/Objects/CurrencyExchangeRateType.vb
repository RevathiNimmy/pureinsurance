Public Class CurrencyExchangeRateType

    Private iTransactionCurrencyKey As Integer

    Private sTransactionCurrencyDesc As String

    Private dTransactionCurrencyRate As Decimal

    Private iExchangeRateOverrideReasonKey As Integer

    Private bExchangeRateOverrideReasonKeySpecified As Boolean

    Private iBaseCurrency As Integer

    Private sBaseCurrencyDesc As String

    Private dBaseCurrencyRate As Decimal

    Private bBaseCurrencyRateSpecified As Boolean

    Private dBaseCurrencyDate As Date

    Private iAccountCurrencyKey As Integer

    Private sAccountCurrencyDesc As String

    Private dAccountCurrencyRate As Decimal

    Private bAccountCurrencyRateSpecified As Boolean

    Private dAccountCurrencyDate As Date

    Private dSystemCurrrencyRate As Decimal

    Private bSystemCurrrencyRateSpecified As Boolean

    Private dSystemCurrencyDate As Date

    Private iSystemCurrencyKey As Integer



    '''<remarks/>
    Public Property TransactionCurrencyRate() As Decimal
        Get
            Return Me.dTransactionCurrencyRate
        End Get
        Set(ByVal value As Decimal)
            Me.dTransactionCurrencyRate = value
        End Set
    End Property
    '''<remarks/>
    Public Property TransactionCurrencyKey() As Integer
        Get
            Return Me.iTransactionCurrencyKey
        End Get
        Set(ByVal value As Integer)
            Me.iTransactionCurrencyKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property TransactionCurrencyDesc() As String
        Get
            Return Me.sTransactionCurrencyDesc
        End Get
        Set(ByVal value As String)
            Me.sTransactionCurrencyDesc = value
        End Set
    End Property

    '''<remarks/>
    Public Property ExchangeRateOverrideReasonKey() As Integer
        Get
            Return Me.iExchangeRateOverrideReasonKey
        End Get
        Set(ByVal value As Integer)
            Me.iExchangeRateOverrideReasonKey = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property ExchangeRateOverrideReasonKeySpecified() As Boolean
        Get
            Return Me.bExchangeRateOverrideReasonKeySpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bExchangeRateOverrideReasonKeySpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property BaseCurrencyKey() As Integer
        Get
            Return Me.iBaseCurrency
        End Get
        Set(ByVal value As Integer)
            Me.iBaseCurrency = value
        End Set
    End Property

    '''<remarks/>
    Public Property BaseCurrencyDesc() As String
        Get
            Return Me.sBaseCurrencyDesc
        End Get
        Set(ByVal value As String)
            Me.sBaseCurrencyDesc = value
        End Set
    End Property

    '''<remarks/>
    Public Property BaseCurrencyRate() As Decimal
        Get
            Return Me.dBaseCurrencyRate
        End Get
        Set(ByVal value As Decimal)
            Me.dBaseCurrencyRate = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property BaseCurrencyRateSpecified() As Boolean
        Get
            Return Me.bBaseCurrencyRateSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bBaseCurrencyRateSpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property BaseCurrencyDate() As Date
        Get
            Return Me.dBaseCurrencyDate
        End Get
        Set(ByVal value As Date)
            Me.dBaseCurrencyDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property AccountCurrencyKey() As Integer
        Get
            Return Me.iAccountCurrencyKey
        End Get
        Set(ByVal value As Integer)
            Me.iAccountCurrencyKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property AccountCurrencyDesc() As String
        Get
            Return Me.sAccountCurrencyDesc
        End Get
        Set(ByVal value As String)
            Me.sAccountCurrencyDesc = value
        End Set
    End Property

    '''<remarks/>
    Public Property AccountCurrencyRate() As Decimal
        Get
            Return Me.dAccountCurrencyRate
        End Get
        Set(ByVal value As Decimal)
            Me.dAccountCurrencyRate = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property AccountCurrencyRateSpecified() As Boolean
        Get
            Return Me.bAccountCurrencyRateSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bAccountCurrencyRateSpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property AccountCurrencyDate() As Date
        Get
            Return Me.dAccountCurrencyDate
        End Get
        Set(ByVal value As Date)
            Me.dAccountCurrencyDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property SystemCurrrencyRate() As Decimal
        Get
            Return Me.dSystemCurrrencyRate
        End Get
        Set(ByVal value As Decimal)
            Me.dSystemCurrrencyRate = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property SystemCurrrencyRateSpecified() As Boolean
        Get
            Return Me.bSystemCurrrencyRateSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bSystemCurrrencyRateSpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property SystemCurrencyDate() As Date
        Get
            Return Me.dSystemCurrencyDate
        End Get
        Set(ByVal value As Date)
            Me.dSystemCurrencyDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property SystemCurrencyKey() As Integer
        Get
            Return Me.iSystemCurrencyKey
        End Get
        Set(ByVal value As Integer)
            Me.iSystemCurrencyKey = value
        End Set
    End Property
End Class
