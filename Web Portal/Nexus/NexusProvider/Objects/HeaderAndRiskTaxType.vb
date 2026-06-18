<Serializable()> Public Class HeaderAndRiskTaxType

    Private sTaxGroup As String

    Private iSequence As Integer

    Private sTaxBand As String

    Private dTaxAmount As Double

    Private sCalculationBasis As String

    Private dRate As Double

    Private sClassOfBusiness As String

    Private sCountry As String

    Private sState As String

    Private bIsNotAppliedToClient As Boolean

    Private bIncludeInInstallment As Boolean

    Private bSpreadAcrossInstallment As Boolean

    Private sApplyTaxBy As String


    '''<remarks/>
    Public Property TaxGroup() As String
        Get
            Return Me.sTaxGroup
        End Get
        Set(ByVal value As String)
            Me.sTaxGroup = value
        End Set
    End Property

    '''<remarks/>
    Public Property Sequence() As Integer
        Get
            Return Me.iSequence
        End Get
        Set(ByVal value As Integer)
            Me.iSequence = value
        End Set
    End Property

    '''<remarks/>
    Public Property TaxBand() As String
        Get
            Return Me.sTaxBand
        End Get
        Set(ByVal value As String)
            Me.sTaxBand = value
        End Set
    End Property

    '''<remarks/>
    Public Property TaxAmount() As Double
        Get
            Return Me.dTaxAmount
        End Get
        Set(ByVal value As Double)
            Me.dTaxAmount = value
        End Set
    End Property

    '''<remarks/>
    Public Property CalculationBasis() As String
        Get
            Return Me.sCalculationBasis
        End Get
        Set(ByVal value As String)
            Me.sCalculationBasis = value
        End Set
    End Property

    '''<remarks/>
    Public Property Rate() As Double
        Get
            Return Me.dRate
        End Get
        Set(ByVal value As Double)
            Me.dRate = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClassOfBusiness() As String
        Get
            Return Me.sClassOfBusiness
        End Get
        Set(ByVal value As String)
            Me.sClassOfBusiness = value
        End Set
    End Property

    '''<remarks/>
    Public Property Country() As String
        Get
            Return Me.sCountry
        End Get
        Set(ByVal value As String)
            Me.sCountry = value
        End Set
    End Property

    '''<remarks/>
    Public Property State() As String
        Get
            Return Me.sState
        End Get
        Set(ByVal value As String)
            Me.sState = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsNotAppliedToClient() As Boolean
        Get
            Return Me.bIsNotAppliedToClient
        End Get
        Set(ByVal value As Boolean)
            Me.bIsNotAppliedToClient = value
        End Set
    End Property

    '''<remarks/>
    Public Property IncludeInInstallment() As Boolean
        Get
            Return Me.bIncludeInInstallment
        End Get
        Set(ByVal value As Boolean)
            Me.bIncludeInInstallment = value
        End Set
    End Property

    '''<remarks/>
    Public Property SpreadAcrossInstallment() As Boolean
        Get
            Return Me.bSpreadAcrossInstallment
        End Get
        Set(ByVal value As Boolean)
            Me.bSpreadAcrossInstallment = value
        End Set
    End Property

    '''<remarks/>
    Public Property ApplyTaxBy() As String
        Get
            Return Me.sApplyTaxBy
        End Get
        Set(ByVal value As String)
            Me.sApplyTaxBy = value
        End Set
    End Property
    Public Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("TaxGroup : " & sTaxGroup & "<br />")
        sbPrint.AppendLine("Sequence : " & iSequence.ToString() & "<br />")
        sbPrint.AppendLine("TaxBand : " & sTaxBand & "<br />")
        sbPrint.AppendLine("TaxAmount : " & dTaxAmount.ToString() & "<br />")
        sbPrint.AppendLine("CalculationBasis : " & sCalculationBasis & "<br />")
        sbPrint.AppendLine("Rate : " & dRate.ToString() & "<br />")
        sbPrint.AppendLine("ClassOfBusiness : " & sClassOfBusiness & "<br />")
        sbPrint.AppendLine("Country : " & sCountry & "<br />")
        sbPrint.AppendLine("State : " & sState & "<br />")
        sbPrint.AppendLine("IsNotAppliedToClient : " & bIsNotAppliedToClient.ToString() & "<br />")
        sbPrint.AppendLine("IncludeInInstallment : " & bIncludeInInstallment.ToString() & "<br />")
        sbPrint.AppendLine("SpreadAcrossInstallment : " & bSpreadAcrossInstallment.ToString & "<br />")
        sbPrint.AppendLine("ApplyTaxBy : " & sApplyTaxBy & "<br />")
        Return sbPrint.ToString()
    End Function
End Class
<Serializable()> Public Class HeaderAndRiskTaxTypeCollection : Inherits CollectionBase
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oHeaderAndRiskTaxType As HeaderAndRiskTaxType In List
            sbPrint.AppendLine(oHeaderAndRiskTaxType.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function
    Public Function Add(ByVal v_oHeaderAndRiskTaxType As HeaderAndRiskTaxType) As Integer
        Return List.Add(v_oHeaderAndRiskTaxType)
    End Function

    Public Sub Remove(ByVal v_oHeaderAndRiskTaxType As HeaderAndRiskTaxType)
        List.Remove(v_oHeaderAndRiskTaxType)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As HeaderAndRiskTaxType
        Get
            Return List(i)
        End Get
        Set(ByVal value As HeaderAndRiskTaxType)
            List(i) = value
        End Set
    End Property

End Class