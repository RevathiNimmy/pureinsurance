<Serializable()> Public Class RITreatyParty
    Private iTreatyPartyKey As Integer

    Private iPartyKey As Integer

    Private sResolvedName As String

    Private iTreatyKey As Integer

    Private dSharePercentage As Decimal

    Private dCommissionPercentage As Decimal

    Private bIsDomiciledForTax As Boolean

    Private iTaxGroupKey As Integer

    Private sDescription, sReinsuranceTypeCode As String

    Private bIsReinsurerApproved As Boolean

    Private dCedingRate, dUpperLimit, dLowerLimit As Decimal

    '''<remarks/>
    Public Property TreatyPartyKey() As Integer
        Get
            Return Me.iTreatyPartyKey
        End Get
        Set(ByVal value As Integer)
            Me.iTreatyPartyKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property PartyKey() As Integer
        Get
            Return Me.iPartyKey
        End Get
        Set(ByVal value As Integer)
            Me.iPartyKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property ResolvedName() As String
        Get
            Return Me.sResolvedName
        End Get
        Set(ByVal value As String)
            Me.sResolvedName = value
        End Set
    End Property

    '''<remarks/>
    Public Property TreatyKey() As Integer
        Get
            Return Me.iTreatyKey
        End Get
        Set(ByVal value As Integer)
            Me.iTreatyKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property SharePercentage() As Decimal
        Get
            Return Me.dSharePercentage
        End Get
        Set(ByVal value As Decimal)
            Me.dSharePercentage = value
        End Set
    End Property

    '''<remarks/>
    Public Property CommissionPercentage() As Decimal
        Get
            Return Me.dCommissionPercentage
        End Get
        Set(ByVal value As Decimal)
            Me.dCommissionPercentage = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsDomiciledForTax() As Boolean
        Get
            Return Me.bIsDomiciledForTax
        End Get
        Set(ByVal value As Boolean)
            Me.bIsDomiciledForTax = value
        End Set
    End Property

    '''<remarks/>
    Public Property TaxGroupKey() As Integer
        Get
            Return Me.iTaxGroupKey
        End Get
        Set(ByVal value As Integer)
            Me.iTaxGroupKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property Description() As String
        Get
            Return Me.sDescription
        End Get
        Set(ByVal value As String)
            Me.sDescription = value
        End Set
    End Property

    '''<remarks/>
    Public Property CedingRate() As Decimal
        Get
            Return Me.dCedingRate
        End Get
        Set(ByVal value As Decimal)
            Me.dCedingRate = value
        End Set
    End Property

    '''<remarks/>
    Public Property ReinsuranceTypeCode() As String
        Get
            Return Me.sReinsuranceTypeCode
        End Get
        Set(ByVal value As String)
            Me.sReinsuranceTypeCode = value
        End Set
    End Property
    '''<remarks/>
    Public Property IsReinsurerApproved() As Boolean
        Get
            Return Me.bIsReinsurerApproved
        End Get
        Set(ByVal value As Boolean)
            Me.bIsReinsurerApproved = value
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

    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("TreatyPartyKey : " & iTreatyPartyKey & "<br />")
        sbPrint.AppendLine("PartyKey : " & iPartyKey & "<br />")
        sbPrint.AppendLine("ResolvedName : " & sResolvedName & "<br />")
        sbPrint.AppendLine("TreatyKey : " & iTreatyKey & "<br />")
        sbPrint.AppendLine("SharePercentage : " & dSharePercentage & "<br />")
        sbPrint.AppendLine("CommissionPercentage : " & dCommissionPercentage & "<br />")
        sbPrint.AppendLine("IsDomiciledForTax : " & bIsDomiciledForTax & "<br />")
        sbPrint.AppendLine("TaxGroupKey : " & iTaxGroupKey & "<br />")
        sbPrint.AppendLine("Description : " & sDescription & "<br />")
        sbPrint.AppendLine("IsReinsurerApproved : " & bIsReinsurerApproved & "<br />")
   
        Return sbPrint.ToString

    End Function
End Class

<Serializable()> Public Class RITreatyPartyCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oRITreatyParty As RiTreatyParty In List
            sbPrint.AppendLine(oRITreatyParty.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function
    Public Function Add(ByVal v_oRITreatyParty As RiTreatyParty) As Integer
        Return List.Add(v_oRITreatyParty)
    End Function

    Public Sub Remove(ByVal v_oRITreatyParty As RiTreatyParty)
        List.Remove(v_oRITreatyParty)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As RiTreatyParty
        Get
            Return List(i)
        End Get
        Set(ByVal value As RiTreatyParty)
            List(i) = value
        End Set
    End Property

End Class



