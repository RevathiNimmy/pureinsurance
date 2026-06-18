<Serializable()> Public Class GetInsuranceFileInformation

    Private _companyId As Integer
    Private _accountId As Integer
    Private _currencyId As Integer
    Private _premium As Decimal
    Private _currencyBaseXrate As Double
    Private _currencyBaseDate As DateTime
    Private _accountBaseXrate As Double
    Private _accountBaseDate As DateTime
    Private _systemBaseXrate As Double
    Private _systemBaseDate As DateTime
    Private _rateOverrideReasonId As Integer

    Public Sub New()

    End Sub

    Public Property CompanyId As Integer
        Get
            Return _companyId
        End Get
        Set(value As Integer)
            _companyId = value
        End Set
    End Property

    Public Property AccountId As Integer
        Get
            Return _accountId
        End Get
        Set(value As Integer)
            _accountId = value
        End Set
    End Property

    Public Property CurrencyId As Integer
        Get
            Return _currencyId
        End Get
        Set(value As Integer)
            _currencyId = value
        End Set
    End Property

    Public Property Premium As Decimal
        Get
            Return _premium
        End Get
        Set(value As Decimal)
            _premium = value
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

    Public Property RateOverrideReasonId As Integer
        Get
            Return _rateOverrideReasonId
        End Get
        Set(value As Integer)
            _rateOverrideReasonId = value
        End Set
    End Property
End Class
