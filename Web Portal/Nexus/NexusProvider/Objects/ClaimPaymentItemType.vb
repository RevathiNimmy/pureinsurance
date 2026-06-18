<Serializable()> Public Class ClaimPaymentItemType

    Private iBaseReserveKey As Integer
    Private sTaxGroupCode As String
    Private dPaymentAmount, dLossPaymentAmount As Decimal
    Private bReverseExcess As Boolean

    Private dTaxAmount As Decimal
    Private dPaymentAdjustment As Decimal
    Private sCurrencyCode As String
    Private dCurrencyRate As Double
    'Private oClaimPaymentItemType As ClaimPaymentItemTypeCollection
    Private iPayQueue As Integer

    Public Property PayQueue() As Integer
        Get
            Return Me.iPayQueue
        End Get
        Set(ByVal value As Integer)
            Me.iPayQueue = value
        End Set
    End Property
    Public Property CurrencyRate() As Double
        Get
            Return Me.dCurrencyRate
        End Get
        Set(ByVal value As Double)
            Me.dCurrencyRate = value
        End Set
    End Property
    Public Property CurrencyCode() As String
        Get
            Return Me.sCurrencyCode
        End Get
        Set(ByVal value As String)
            Me.sCurrencyCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property TaxAmount() As Decimal
        Get
            Return Me.dTaxAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dTaxAmount = value
        End Set
    End Property

    '''<remarks/>
    Public Property PaymentAdjustment() As Decimal
        Get
            Return Me.dPaymentAdjustment
        End Get
        Set(ByVal value As Decimal)
            Me.dPaymentAdjustment = value
        End Set
    End Property

    '''<remarks/>
    Public Property BaseReserveKey() As Integer
        Get
            Return Me.iBaseReserveKey
        End Get
        Set(ByVal value As Integer)
            Me.iBaseReserveKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property TaxGroupCode() As String
        Get
            Return Me.sTaxGroupCode
        End Get
        Set(ByVal value As String)
            Me.sTaxGroupCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property PaymentAmount() As Decimal
        Get
            Return Me.dPaymentAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dPaymentAmount = value
        End Set
    End Property
    Public Property LossPaymentAmount() As Decimal
        Get
            Return Me.dLossPaymentAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dLossPaymentAmount = value
        End Set
    End Property

    '''<remarks/>
    Public Property ReverseExcess() As Boolean
        Get
            Return Me.bReverseExcess
        End Get
        Set(ByVal value As Boolean)
            Me.bReverseExcess = value
        End Set
    End Property

    Public Property IsTaxOverridden() As Boolean

    Public Property OverriddedTaxAmount() As Decimal

    'Public Property ClaimPaymentItemType() As ClaimPaymentItemTypeCollection
    '    Get
    '        Return Me.oClaimPaymentItemType
    '    End Get
    '    Set(ByVal value As ClaimPaymentItemTypeCollection)
    '        Me.oClaimPaymentItemType = value
    '    End Set
    'End Property
End Class
<Serializable()> Public Class ClaimPaymentItemTypeCollection : Inherits CollectionBase

    Public Function Add(ByVal v_oClaimPaymentItemType As ClaimPaymentItemType) As Integer
        Return List.Add(v_oClaimPaymentItemType)
    End Function

    Public Sub Remove(ByVal v_oClaimPaymentItemType As ClaimPaymentItemType)
        List.Remove(v_oClaimPaymentItemType)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As ClaimPaymentItemType
        Get
            Return List(i)
        End Get
        Set(ByVal value As ClaimPaymentItemType)
            List(i) = value
        End Set
    End Property

End Class