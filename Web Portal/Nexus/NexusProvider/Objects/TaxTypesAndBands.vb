<Serializable()> Public Class TaxTypesAndBands

    Public Property TaxTypeId() As Integer
    Public Property TaxTypeDescription() As String
    Public Property TaxBandId() As Integer
    Public Property TaxBandDescription() As String
    Public Property IsValue() As Boolean
    Public Property Rate() As Decimal
    Public Property CurrencyId() As Integer
    Public Property TaxTypeCode() As String
    Public Property Sequence() As Integer
    Public Property AllowTaxCredit() As Boolean
    Public Property TaxBandRateId() As Integer

End Class

<Serializable()> Public Class TaxTypesAndBandsCollection : Inherits CollectionBase


    Public Function Add(ByVal v_oAccountBalance As TaxTypesAndBands) As Integer

        Return List.Add(v_oAccountBalance)

    End Function

    Public Sub Remove(ByVal v_oAccountBalance As TaxTypesAndBands)
        List.Remove(v_oAccountBalance)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As TaxTypesAndBands
        Get
            Return List(i)
        End Get
        Set(ByVal value As TaxTypesAndBands)
            List(i) = value
        End Set
    End Property

End Class
