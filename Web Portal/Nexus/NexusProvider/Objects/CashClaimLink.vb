
<Serializable()> Public Class CashClaimLink

    Private iCashListKey As Integer

    Private iCashListItemKey As Integer

    Private sCurrencyCode As String

    Private sBranchCode As String

    Private dAmount As Decimal

    Private sMediaTypeCode As String

    '''<remarks/>
    Public Property CashListKey() As Integer
        Get
            Return Me.iCashListKey
        End Get
        Set(ByVal value As Integer)
            Me.iCashListKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property CashListItemKey() As Integer
        Get
            Return Me.iCashListItemKey
        End Get
        Set(ByVal value As Integer)
            Me.iCashListItemKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property CurrencyCode() As String
        Get
            Return Me.sCurrencyCode
        End Get
        Set(ByVal value As String)
            Me.sCurrencyCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property BranchCode() As String
        Get
            Return Me.sBranchCode
        End Get
        Set(ByVal value As String)
            Me.sBranchCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property Amount() As Decimal
        Get
            Return Me.dAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dAmount = value
        End Set
    End Property

    '''<remarks/>
    Public Property MediaTypeCode() As String
        Get
            Return Me.sMediaTypeCode
        End Get
        Set(ByVal value As String)
            Me.sMediaTypeCode = value
        End Set
    End Property

End Class

