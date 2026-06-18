<Serializable()> Public Class DoCurrencyConversion

    Private _baseCurrencyId As Integer
    Private _baseAmount As Decimal
    Private _accountCurrencyId As Integer
    Private _accountAmount As Decimal
    Private _systemCurrencyId As Integer
    Private _systemAmount As Decimal
    Private _currencyBaseXrate As Double
    Private _currencyBaseDate As DateTime
    Private _accountBaseXrate As Double
    Private _accountBaseDate As DateTime
    Private _systemBaseXrate As Double
    Private _systemBaseDate As DateTime

    Public Sub New()

    End Sub
    Public Property BaseCurrencyId As Integer
        Get
            Return _baseCurrencyId
        End Get
        Set(value As Integer)
            _baseCurrencyId = value
        End Set
    End Property

    Public Property BaseAmount As Decimal
        Get
            Return _baseAmount
        End Get
        Set(value As Decimal)
            _baseAmount = value
        End Set
    End Property

    Public Property AccountCurrencyId As Integer
        Get
            Return _accountCurrencyId
        End Get
        Set(value As Integer)
            _accountCurrencyId = value
        End Set
    End Property

    Public Property AccountAmount As Decimal
        Get
            Return _accountAmount
        End Get
        Set(value As Decimal)
            _accountAmount = value
        End Set
    End Property

    Public Property SystemCurrencyId As Integer
        Get
            Return _systemCurrencyId
        End Get
        Set(value As Integer)
            _systemCurrencyId = value
        End Set
    End Property

    Public Property SystemAmount As Decimal
        Get
            Return _systemAmount
        End Get
        Set(value As Decimal)
            _systemAmount = value
        End Set
    End Property

    Public Property CurrencyBaseXrate As Double
        Get
            Return _currencyBaseXrate
        End Get
        Set(value As Double)
            _currencyBaseXrate = value
        End Set
    End Property

    Public Property CurrencyBaseDate As DateTime
        Get
            Return _currencyBaseDate
        End Get
        Set(value As DateTime)
            _currencyBaseDate = value
        End Set
    End Property

    Public Property AccountBaseXrate As Double
        Get
            Return _accountBaseXrate
        End Get
        Set(value As Double)
            _accountBaseXrate = value
        End Set
    End Property

    Public Property AccountBaseDate As DateTime
        Get
            Return _accountBaseDate
        End Get
        Set(value As DateTime)
            _accountBaseDate = value
        End Set
    End Property

    Public Property SystemBaseXrate As Double
        Get
            Return _systemBaseXrate
        End Get
        Set(value As Double)
            _systemBaseXrate = value
        End Set
    End Property

    Public Property SystemBaseDate As DateTime
        Get
            Return _systemBaseDate
        End Get
        Set(value As DateTime)
            _systemBaseDate = value
        End Set
    End Property

End Class