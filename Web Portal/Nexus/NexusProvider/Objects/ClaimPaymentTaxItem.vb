<Serializable()> Public Class ClaimPaymentTaxItem

    Private sReserveType As String
    Private sTaxGroupCode As String
    Private sTaxBandCode As String
    Private dPercentage As Decimal
    Private dAmount As Decimal
    Private classOfBusinessIDField As Integer

    Private taxBandIdField As Integer

    Private taxGroupIdField As Integer

    Private sequenceField As Integer

    Private isManuallyChangesField As Integer
    Private oClaimPaymentTaxItem As ClaimPaymentTaxItemCollection
    '''<remarks/>
    Public Property ReserveType() As String
        Get
            Return Me.sReserveType
        End Get
        Set(ByVal value As String)
            Me.sReserveType = value
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
    Public Property TaxBandCode() As String
        Get
            Return Me.sTaxBandCode
        End Get
        Set(ByVal value As String)
            Me.sTaxBandCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property Percentage() As Decimal
        Get
            Return Me.dPercentage
        End Get
        Set(ByVal value As Decimal)
            Me.dPercentage = value
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
    Public Property ClassOfBusinessID() As Integer
        Get
            Return Me.classOfBusinessIDField
        End Get
        Set(ByVal value As Integer)
            Me.classOfBusinessIDField = value
        End Set
    End Property

    '''<remarks/>
    Public Property TaxBandId() As Integer
        Get
            Return Me.taxBandIdField
        End Get
        Set(ByVal value As Integer)
            Me.taxBandIdField = value
        End Set
    End Property

    '''<remarks/>
    Public Property TaxGroupId() As Integer
        Get
            Return Me.taxGroupIdField
        End Get
        Set(ByVal value As Integer)
            Me.taxGroupIdField = value
        End Set
    End Property

    '''<remarks/>
    Public Property Sequence() As Integer
        Get
            Return Me.sequenceField
        End Get
        Set(ByVal value As Integer)
            Me.sequenceField = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsManuallyChanges() As Integer
        Get
            Return Me.isManuallyChangesField
        End Get
        Set(ByVal value As Integer)
            Me.isManuallyChangesField = value
        End Set
    End Property
    Private sReserveTypeCode As String
    Public Property ReserveTypeCode() As String
        Get
            Return Me.sReserveTypeCode
        End Get
        Set(ByVal value As String)
            Me.sReserveTypeCode = value
        End Set
    End Property

    Public Property ClaimPaymentTaxItem() As ClaimPaymentTaxItemCollection
        Get
            Return Me.oClaimPaymentTaxItem
        End Get
        Set(ByVal value As ClaimPaymentTaxItemCollection)
            Me.oClaimPaymentTaxItem = value
        End Set
    End Property

End Class
<Serializable()> Public Class ClaimPaymentTaxItemCollection : Inherits CollectionBase

    Public Function Add(ByVal v_oClaimPaymentTaxItem As ClaimPaymentTaxItem) As Integer
        Return List.Add(v_oClaimPaymentTaxItem)
    End Function

    Public Sub Remove(ByVal v_oClaimPaymentTaxItem As ClaimPaymentTaxItem)
        List.Remove(v_oClaimPaymentTaxItem)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As ClaimPaymentTaxItem
        Get
            Return List(i)
        End Get
        Set(ByVal value As ClaimPaymentTaxItem)
            List(i) = value
        End Set
    End Property

End Class
