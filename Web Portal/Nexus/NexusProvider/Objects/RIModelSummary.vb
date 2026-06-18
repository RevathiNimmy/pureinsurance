Public Class RIModelSummary
    Private sRIModelDescriptiona As String
    Private dtEffectiveDate As Date
    Private dtExpiryDate As Date
    Private sClaimAllocation As String
    Private sRIModelCurrency As String
    Private oPriorityLine As PriorityLineCollections
    Public Property RIModelDescription() As String
        Get
            Return Me.sRIModelDescriptiona
        End Get
        Set(ByVal value As String)
            Me.sRIModelDescriptiona = value
        End Set
    End Property
    Public Property EffectiveDate() As Date
        Get
            Return Me.dtEffectiveDate
        End Get
        Set(ByVal value As Date)
            Me.dtEffectiveDate = value
        End Set
    End Property

    Public Property ExpiryDate() As Date
        Get
            Return Me.dtExpiryDate
        End Get
        Set(ByVal value As Date)
            Me.dtExpiryDate = value
        End Set
    End Property
    Public Property ClaimAllocation() As String
        Get
            Return Me.sClaimAllocation
        End Get
        Set(ByVal value As String)
            Me.sClaimAllocation = value
        End Set
    End Property
    Public Property RIModelCurrency() As String
        Get
            Return Me.sRIModelCurrency
        End Get
        Set(ByVal value As String)
            Me.sRIModelCurrency = value
        End Set
    End Property
    Public Property PriorityLine() As PriorityLineCollections
        Get
            Return Me.oPriorityLine
        End Get
        Set(ByVal value As PriorityLineCollections)
            Me.oPriorityLine = value
        End Set
    End Property

End Class
<Serializable()> Public Class PriorityLineCollections : Inherits CollectionBase

    Public Function Add(ByVal v_oPriorityLine As PriorityLine) As Integer
        Return List.Add(v_oPriorityLine)
    End Function

    Public Sub Remove(ByVal v_oPriorityLine As PriorityLine)
        List.Remove(v_oPriorityLine)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As PriorityLine
        Get
            Return List(i)
        End Get
        Set(ByVal value As PriorityLine)
            List(i) = value
        End Set
    End Property

End Class

Public Class PriorityLine
    Private sPriorityName As String
    Private iPriorityKey As Integer
    Private iNoOfLines As Integer
    Private lLineLimit As Long

    Public Property PriorityName() As String
        Get
            Return Me.sPriorityName
        End Get
        Set(ByVal value As String)
            Me.sPriorityName = value
        End Set
    End Property
    Public Property PriorityKey() As Integer
        Get
            Return Me.iPriorityKey
        End Get
        Set(ByVal value As Integer)
            Me.iPriorityKey = value
        End Set
    End Property
    Public Property NoOfLines() As Integer
        Get
            Return Me.iNoOfLines
        End Get
        Set(ByVal value As Integer)
            Me.iNoOfLines = value
        End Set
    End Property
    Public Property LineLimit() As Long
        Get
            Return Me.lLineLimit
        End Get
        Set(ByVal value As Long)
            Me.lLineLimit = value
        End Set
    End Property
End Class
<Serializable()> Public Class TreatyLineCollections : Inherits CollectionBase

    Public Function Add(ByVal v_oTreatyLine As TreatyLine) As Integer
        Return List.Add(v_oTreatyLine)
    End Function

    Public Sub Remove(ByVal v_oTreatyLine As TreatyLine)
        List.Remove(v_oTreatyLine)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As TreatyLine
        Get
            Return List(i)
        End Get
        Set(ByVal value As TreatyLine)
            List(i) = value
        End Set
    End Property

End Class
<Serializable()> Public Class TreatyLine
    Private sTreatyName As String
    Private iTreatyKey As Integer
    Private iTreatyPriorityKey As Integer
    Private dSharePercent As Decimal
    Private sDescription, sReinsuranceTypeCode As String
    Private dCedingRate, dLowerLimit, dUpperLimit As Decimal
    Private dtEffectiveDate, dtExpiryDate As Date
    'Private oTreatyParty As TreatyPartyLineCollection
    Public Property TreatyName() As String
        Get
            Return Me.sTreatyName
        End Get
        Set(ByVal value As String)
            Me.sTreatyName = value
        End Set
    End Property
    Public Property TreatyKey() As Integer
        Get
            Return Me.iTreatyKey
        End Get
        Set(ByVal value As Integer)
            Me.iTreatyKey = value
        End Set
    End Property
    Public Property TreatyPriorityKey() As Integer
        Get
            Return Me.iTreatyPriorityKey
        End Get
        Set(ByVal value As Integer)
            Me.iTreatyPriorityKey = value
        End Set
    End Property
    Public Property SharePercent() As Decimal
        Get
            Return Me.dSharePercent
        End Get
        Set(ByVal value As Decimal)
            Me.dSharePercent = value
        End Set
    End Property
    Public Property Description() As String
        Get
            Return Me.sDescription
        End Get
        Set(ByVal value As String)
            Me.sDescription = value
        End Set
    End Property
    Public Property CedingRate() As Decimal
        Get
            Return Me.dCedingRate
        End Get
        Set(ByVal value As Decimal)
            Me.dCedingRate = value
        End Set
    End Property
    Public Property EffectiveDate() As Date
        Get
            Return Me.dtEffectiveDate
        End Get
        Set(ByVal value As Date)
            Me.dtEffectiveDate = value
        End Set
    End Property
    Public Property ReinsuranceTypeCode() As String
        Get
            Return Me.sReinsuranceTypeCode
        End Get
        Set(ByVal value As String)
            Me.sReinsuranceTypeCode = value
        End Set
    End Property

    Public Property LowerLimit() As Decimal
        Get
            Return Me.dLowerLimit
        End Get
        Set(ByVal value As Decimal)
            Me.dLowerLimit = value
        End Set
    End Property
    Public Property UpperLimit() As Decimal
        Get
            Return Me.dUpperLimit
        End Get
        Set(ByVal value As Decimal)
            Me.dUpperLimit = value
        End Set
    End Property
    Public Property ExpiryDate() As Date
        Get
            Return Me.dtExpiryDate
        End Get
        Set(ByVal value As Date)
            Me.dtExpiryDate = value
        End Set
    End Property
End Class
<Serializable()> Public Class TreatyPartyLineCollections : Inherits CollectionBase

    Public Function Add(ByVal v_oTreatyPartyLine As TreatyPartyLine) As Integer
        Return List.Add(v_oTreatyPartyLine)
    End Function

    Public Sub Remove(ByVal v_oTreatyPartyLine As TreatyPartyLine)
        List.Remove(v_oTreatyPartyLine)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As TreatyPartyLine
        Get
            Return List(i)
        End Get
        Set(ByVal value As TreatyPartyLine)
            List(i) = value
        End Set
    End Property
    Public Function FindItemByKey(ByVal v_iKey As Integer) As TreatyPartyLine

        For Each oItem As TreatyPartyLine In List
            If oItem.TreatyKey = v_iKey Then
                Return oItem
            End If
        Next
        Return Nothing

    End Function
End Class
<Serializable()> Public Class TreatyPartyLine
    Private sTreatyPartyName As String
    Private iTreatyPartyKey As Integer
    Private iTreatyKey As Integer
    Private dSharePercent As Decimal
    Private sDescription, sReinsuranceTypeCode As String
    Private dtEffectiveDate As Date
    Private dtExpiryDate As Date
    Private dCedingRate, dLowerLimit, dUpperLimit As Decimal
    Public Property TreatyPartyName() As String
        Get
            Return Me.sTreatyPartyName
        End Get
        Set(ByVal value As String)
            Me.sTreatyPartyName = value
        End Set
    End Property
    Public Property TreatyPartyKey() As Integer
        Get
            Return Me.iTreatyPartyKey
        End Get
        Set(ByVal value As Integer)
            Me.iTreatyPartyKey = value
        End Set
    End Property
    Public Property TreatyKey() As Integer
        Get
            Return Me.iTreatyKey
        End Get
        Set(ByVal value As Integer)
            Me.iTreatyKey = value
        End Set
    End Property

    Public Property SharePercent() As Decimal
        Get
            Return Me.dSharePercent
        End Get
        Set(ByVal value As Decimal)
            Me.dSharePercent = value
        End Set
    End Property
    Public Property Description() As String
        Get
            Return Me.sDescription
        End Get
        Set(ByVal value As String)
            Me.sDescription = value
        End Set
    End Property
    Public Property EffectiveDate() As Date
        Get
            Return Me.dtEffectiveDate
        End Get
        Set(ByVal value As Date)
            Me.dtEffectiveDate = value
        End Set
    End Property

    Public Property ExpiryDate() As Date
        Get
            Return Me.dtExpiryDate
        End Get
        Set(ByVal value As Date)
            Me.dtExpiryDate = value
        End Set
    End Property
    Public Property CedingRate() As Decimal
        Get
            Return Me.dCedingRate
        End Get
        Set(ByVal value As Decimal)
            Me.dCedingRate = value
        End Set
    End Property
    Public Property ReinsuranceTypeCode() As String
        Get
            Return Me.sReinsuranceTypeCode
        End Get
        Set(ByVal value As String)
            Me.sReinsuranceTypeCode = value
        End Set
    End Property
    Public Property LowerLimit() As Decimal
        Get
            Return Me.dLowerLimit
        End Get
        Set(ByVal value As Decimal)
            Me.dLowerLimit = value
        End Set
    End Property
    Public Property UpperLimit() As Decimal
        Get
            Return Me.dUpperLimit
        End Get
        Set(ByVal value As Decimal)
            Me.dUpperLimit = value
        End Set
    End Property

End Class
