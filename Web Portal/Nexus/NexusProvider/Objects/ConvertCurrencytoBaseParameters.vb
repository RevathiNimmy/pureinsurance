<Serializable()> Public Class ConvertCurrencytoBaseParameters

    Private _currencyID As Integer
    Private _companyID As Integer
    Private _baseAmount As Decimal
    Private _currencyAmount As Decimal
    Private _conversionDate As Object
    Private _conversionRate As Object
    Private _isMultiplier As Boolean
    Private _rounded As Object
    Private _baseRoundingDifference As Object
    Private _currencyRoundingDifference As Object
    Private _formattedBase As Object
    Private _formattedCurrency As Object
    Private _euro As Integer
    Private _euroAmount As Decimal
    Private _euroCCyXrate As Object
    Private _euroBaseXRate As Object
    Private _ccyAmountUnRounded As Object
    Private _baseAmountUnRounded As Decimal

    Public Sub New()
    End Sub

    Public Property CurrencyID() As Integer
        Get
            Return _currencyID
        End Get
        Set(ByVal value As Integer)
            _currencyID = value
        End Set
    End Property

    Public Property CompanyID() As Integer
        Get
            Return _companyID
        End Get
        Set(ByVal value As Integer)
            _companyID = value
        End Set
    End Property

    Public Property BaseAmount() As Decimal
        Get
            Return _baseAmount
        End Get
        Set(ByVal value As Decimal)
            _baseAmount = value
        End Set
    End Property

    Public Property CurrencyAmount() As Decimal
        Get
            Return _currencyAmount
        End Get
        Set(ByVal value As Decimal)
            _currencyAmount = value
        End Set
    End Property

    Public Property ConversionDate() As Object
        Get
            Return _conversionDate
        End Get
        Set(ByVal value As Object)
            _conversionDate = value
        End Set
    End Property

    Public Property ConversionRate() As Object
        Get
            Return _conversionRate
        End Get
        Set(ByVal value As Object)
            _conversionRate = value
        End Set
    End Property

    Public Property IsMultiplier() As Boolean
        Get
            Return _isMultiplier
        End Get
        Set(ByVal value As Boolean)
            _isMultiplier = value
        End Set
    End Property

    Public Property Rounded() As Object
        Get
            Return _rounded
        End Get
        Set(ByVal value As Object)
            _rounded = value
        End Set
    End Property

    Public Property BaseRoundingDifference() As Object
        Get
            Return _baseRoundingDifference
        End Get
        Set(ByVal value As Object)
            _baseRoundingDifference = value
        End Set
    End Property

    Public Property CurrencyRoundingDifference() As Object
        Get
            Return _currencyRoundingDifference
        End Get
        Set(ByVal value As Object)
            _currencyRoundingDifference = value
        End Set
    End Property

    Public Property FormattedBase() As Object
        Get
            Return _formattedBase
        End Get
        Set(ByVal value As Object)
            _formattedBase = value
        End Set
    End Property

    Public Property FormattedCurrency() As Object
        Get
            Return _formattedCurrency
        End Get
        Set(ByVal value As Object)
            _formattedCurrency = value
        End Set
    End Property

    Public Property Euro() As Integer
        Get
            Return _euro
        End Get
        Set(ByVal value As Integer)
            _euro = value
        End Set
    End Property

    Public Property EuroAmount() As Decimal
        Get
            Return _euroAmount
        End Get
        Set(ByVal value As Decimal)
            _euroAmount = value
        End Set
    End Property

    Public Property EuroCCyXrate() As Object
        Get
            Return _euroCCyXrate
        End Get
        Set(ByVal value As Object)
            _euroCCyXrate = value
        End Set
    End Property

    Public Property EuroBaseXRate() As Object
        Get
            Return _euroBaseXRate
        End Get
        Set(ByVal value As Object)
            _euroBaseXRate = value
        End Set
    End Property

    Public Property CcyAmountUnRounded() As Object
        Get
            Return _ccyAmountUnRounded
        End Get
        Set(ByVal value As Object)
            _ccyAmountUnRounded = value
        End Set
    End Property

    Public Property BaseAmountUnRounded() As Decimal
        Get
            Return _baseAmountUnRounded
        End Get
        Set(ByVal value As Decimal)
            _baseAmountUnRounded = value
        End Set
    End Property

End Class
