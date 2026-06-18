<Serializable()> Public Class Reinsurances
    Private iRecoveryKey As Integer

    Private sRecoveryType As String

    Private iPartyKey As Integer

    Private sReinsurer As String

    Private dSharePercent As Decimal

    Private dRecoveryToDate As Decimal

    Private dTotalThisRecovery As Decimal

    Private dThisRecovery As Decimal

    Private iRIModelKey As Integer

    Private sCode As String

    Private sDescription As String

    Private dtEffectiveDate As DateTime

    Private dtExpiryDate As DateTime

    Private sRIModelType As String

    Private sFACPremiums As String

    Private sCurrencyCode As String

    Private iRIModelLineKey As Integer

    Private oRIModelLineDetails As RIModelLineDetailsCollection

    Private oRITreatyParty As RITreatyPartyCollection
    Private dSalvage, dRecovery, dThisSalvage As Decimal

    '''<remarks/>
    Public Property ThisSalvage() As Decimal
        Get
            Return Me.dThisSalvage
        End Get
        Set(ByVal value As Decimal)
            Me.dThisSalvage = value
        End Set
    End Property
    '''<remarks/>
    Public Property Recovery() As Decimal
        Get
            Return Me.dRecovery
        End Get
        Set(ByVal value As Decimal)
            Me.dRecovery = value
        End Set
    End Property
    '''<remarks/>
    Public Property Salvage() As Decimal
        Get
            Return Me.dSalvage
        End Get
        Set(ByVal value As Decimal)
            Me.dSalvage = value
        End Set
    End Property
    '''<remarks/>
    Public Property RecoveryKey() As Integer
        Get
            Return Me.iRecoveryKey
        End Get
        Set(ByVal value As Integer)
            Me.iRecoveryKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property RecoveryType() As String
        Get
            Return Me.sRecoveryType
        End Get
        Set(ByVal value As String)
            Me.sRecoveryType = value
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
    Public Property Reinsurer() As String
        Get
            Return Me.sReinsurer
        End Get
        Set(ByVal value As String)
            Me.sReinsurer = value
        End Set
    End Property

    '''<remarks/>
    Public Property SharePercent() As Decimal
        Get
            Return Me.dSharePercent
        End Get
        Set(ByVal value As Decimal)
            Me.dSharePercent = value
        End Set
    End Property

    '''<remarks/>
    Public Property RecoveryToDate() As Decimal
        Get
            Return Me.dRecoveryToDate
        End Get
        Set(ByVal value As Decimal)
            Me.dRecoveryToDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property TotalThisRecovery() As Decimal
        Get
            Return Me.dTotalThisRecovery
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalThisRecovery = value
        End Set
    End Property

    '''<remarks/>
    Public Property ThisRecovery() As Decimal
        Get
            Return Me.dThisRecovery
        End Get
        Set(ByVal value As Decimal)
            Me.dThisRecovery = value
        End Set
    End Property

    Public Property Code() As String
        Get
            Return Me.sCode
        End Get
        Set(ByVal value As String)
            Me.sCode = value
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
    Public Property EffectiveDate() As DateTime
        Get
            Return Me.dtEffectiveDate
        End Get
        Set(ByVal value As DateTime)
            Me.dtEffectiveDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property ExpiryDate() As DateTime
        Get
            Return Me.dtExpiryDate
        End Get
        Set(ByVal value As DateTime)
            Me.dtExpiryDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property RIModelType() As String
        Get
            Return Me.sRIModelType
        End Get
        Set(ByVal value As String)
            Me.sRIModelType = value
        End Set
    End Property

    '''<remarks/>
    Public Property FACPremiums() As String
        Get
            Return Me.sFACPremiums
        End Get
        Set(ByVal value As String)
            Me.sFACPremiums = value
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
    Public Property RIModelLineKey() As Integer
        Get
            Return Me.iRIModelLineKey
        End Get
        Set(ByVal value As Integer)
            Me.iRIModelLineKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property RIModelLineDetails() As RIModelLineDetailsCollection
        Get
            Return oRIModelLineDetails
        End Get
        Set(ByVal value As RIModelLineDetailsCollection)
            oRIModelLineDetails = value
        End Set
    End Property

    '''<remarks/>
    Public Property RITreatyParty() As RITreatyPartyCollection
        Get
            Return oRITreatyParty
        End Get
        Set(ByVal value As RITreatyPartyCollection)
            oRITreatyParty = value
        End Set
    End Property

    Public Sub New()
        oRIModelLineDetails = New RIModelLineDetailsCollection
        oRITreatyParty = New RITreatyPartyCollection
    End Sub
End Class
<Serializable()> Public Class ReinsurancesCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oReinsurances As Reinsurances In List
            '   sbPrint.AppendLine(oReinsurances.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oReinsurances As Reinsurances) As Integer
        Return List.Add(v_oReinsurances)
    End Function

    Public Sub Remove(ByVal v_oReinsurances As Reinsurances)
        List.Remove(v_oReinsurances)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As Reinsurances
        Get
            Return List(i)
        End Get
        Set(ByVal value As Reinsurances)
            List(i) = value
        End Set
    End Property

End Class

<Serializable()> Public Class RIModelLineDetails
    Private iRIModelLineKey As Integer

    Private iRIModelKey As Integer

    Private iPriority As Integer

    Private dNoOfLines As Decimal

    Private dLineLimit As Decimal

    Private iTreatyKey As Integer

    Private sTreatyCode As String

    Private sDescription As String

    Private dSharePercentage As Decimal

    Private dLowerLimit As Decimal

    Private dCedingRate As Decimal

    Private iTreatyTypeKey As Integer

    Private sTreatyTypeCode As String

    Private iReinsuranceTypeKey As Integer

    Private sReinsuranceTypeCode As String

    Private bCedePremiumOnly As Boolean

    Private dtEffectivedate As Date

    Private dtExpiryDate As Date

    Private bManuallyAdded As Boolean

    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("RIModelLineKey  : " & iRIModelLineKey & "<br />")
        sbPrint.AppendLine("RIModelKey : " & iRIModelKey & "<br />")
        sbPrint.AppendLine("Priority : " & iPriority & "<br />")
        sbPrint.AppendLine("NoOfLines : " & dNoOfLines & "<br />")
        sbPrint.AppendLine("LineLimit : " & dLineLimit & "<br />")
        sbPrint.AppendLine("TreatyKey : " & iTreatyKey & "<br />")
        sbPrint.AppendLine("SharePercentage : " & dSharePercentage & "<br />")
        sbPrint.AppendLine("Description : " & sDescription & "<br />")
        sbPrint.AppendLine("LowerLimit : " & dLowerLimit & "<br />")
        sbPrint.AppendLine("CedingRate : " & dCedingRate & "<br />")
        sbPrint.AppendLine("TreatyTypeKey : " & iTreatyTypeKey & "<br />")
        sbPrint.AppendLine("ReinsuranceTypeKey : " & iReinsuranceTypeKey & "<br />")
        sbPrint.AppendLine("CedePremiumOnly : " & bCedePremiumOnly & "<br />")
        Return sbPrint.ToString

    End Function
    '''<remarks/>
    Public Property RIModelLineKey() As Integer
        Get
            Return Me.iRIModelLineKey
        End Get
        Set(ByVal value As Integer)
            Me.iRIModelLineKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property RIModelKey() As Integer
        Get
            Return Me.iRIModelKey
        End Get
        Set(ByVal value As Integer)
            Me.iRIModelKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property Priority() As Integer
        Get
            Return Me.iPriority
        End Get
        Set(ByVal value As Integer)
            Me.iPriority = value
        End Set
    End Property

    '''<remarks/>
    Public Property NoOfLines() As Decimal
        Get
            Return Me.dNoOfLines
        End Get
        Set(ByVal value As Decimal)
            Me.dNoOfLines = value
        End Set
    End Property
    '''<remarks/>
    Public Property LineLimit() As Decimal
        Get
            Return Me.dLineLimit
        End Get
        Set(ByVal value As Decimal)
            Me.dLineLimit = value
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
    Public Property TreatyCode() As String
        Get
            Return Me.sTreatyCode
        End Get
        Set(ByVal value As String)
            Me.sTreatyCode = value
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
    Public Property Description() As String
        Get
            Return Me.sDescription
        End Get
        Set(ByVal value As String)
            Me.sDescription = value
        End Set
    End Property

    '''<remarks/>
    Public Property LowerLimit() As Decimal
        Get
            Return Me.dLowerLimit
        End Get
        Set(ByVal value As Decimal)
            Me.dLowerLimit = value
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
    Public Property TreatyTypeKey() As Integer
        Get
            Return Me.iTreatyTypeKey
        End Get
        Set(ByVal value As Integer)
            Me.iTreatyTypeKey = value
        End Set
    End Property
    '''<remarks/>
    Public Property TreatyTypeCode() As String
        Get
            Return Me.sTreatyTypeCode
        End Get
        Set(ByVal value As String)
            Me.sTreatyTypeCode = value
        End Set
    End Property
    '''<remarks/>
    Public Property ReinsuranceTypeKey() As Integer
        Get
            Return Me.iReinsuranceTypeKey
        End Get
        Set(ByVal value As Integer)
            Me.iReinsuranceTypeKey = value
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
    Public Property CedePremiumOnly() As Boolean
        Get
            Return Me.bCedePremiumOnly
        End Get
        Set(ByVal value As Boolean)
            Me.bCedePremiumOnly = value
        End Set
    End Property
    '''<remarks/>
    Public Property EffectiveDate() As Date
        Get
            Return Me.dtEffectivedate
        End Get
        Set(ByVal value As Date)
            Me.dtEffectivedate = value
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
    '''<remarks/>
    Public Property ManuallyAdded() As Boolean
        Get
            Return Me.bManuallyAdded
        End Get
        Set(ByVal value As Boolean)
            Me.bManuallyAdded = value
        End Set
    End Property


End Class



<Serializable()> Public Class RIModelLineDetailsCollection : Inherits SortableCollectionBase
    Public Sub New()
        MyBase.SortObjectType = GetType(RIModelLineDetails)
    End Sub
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oRIModelLineDetails As RIModelLineDetails In List
            sbPrint.AppendLine(oRIModelLineDetails.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function
    Public Function Add(ByVal v_oRIModelLineDetails As RIModelLineDetails) As Integer
        Return List.Add(v_oRIModelLineDetails)
    End Function

    Public Sub Remove(ByVal v_oRIModelLineDetails As RIModelLineDetails)
        List.Remove(v_oRIModelLineDetails)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As RIModelLineDetails
        Get
            Return List(i)
        End Get
        Set(ByVal value As RIModelLineDetails)
            List(i) = value
        End Set
    End Property

End Class

