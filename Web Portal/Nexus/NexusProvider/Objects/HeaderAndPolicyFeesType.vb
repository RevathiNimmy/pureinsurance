<Serializable()> Public Class HeaderAndPolicyFeesType

    Private sFeeName As String

    Private sCurrencyCode As String

    Private sAppliedTo As String

    Private dPremium As Double

    Private dRate As Double

    Private dFeeAmount As Double

    Private dTaxAmount As Double

    Private dTotalAmount As Double

    Private sTaxGroup As String

    Private iIncludeInInstallment As Integer

    Private iSpreadAcrossInstallment As Integer

    '''<remarks/>
    Public Property FeeName() As String
        Get
            Return Me.sFeeName
        End Get
        Set(ByVal value As String)
            Me.sFeeName = value
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
    Public Property AppliedTo() As String
        Get
            Return Me.sAppliedTo
        End Get
        Set(ByVal value As String)
            Me.sAppliedTo = value
        End Set
    End Property

    '''<remarks/>
    Public Property Premium() As Double
        Get
            Return Me.dPremium
        End Get
        Set(ByVal value As Double)
            Me.dPremium = value
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
    Public Property FeeAmount() As Double
        Get
            Return Me.dFeeAmount
        End Get
        Set(ByVal value As Double)
            Me.dFeeAmount = value
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
    Public Property TotalAmount() As Double
        Get
            Return Me.dTotalAmount
        End Get
        Set(ByVal value As Double)
            Me.dTotalAmount = value
        End Set
    End Property

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
    Public Property IncludeInInstallment() As Integer
        Get
            Return Me.iIncludeInInstallment
        End Get
        Set(ByVal value As Integer)
            Me.iIncludeInInstallment = value
        End Set
    End Property

    '''<remarks/>
    Public Property SpreadAcrossInstallment() As Integer
        Get
            Return Me.iSpreadAcrossInstallment
        End Get
        Set(ByVal value As Integer)
            Me.iSpreadAcrossInstallment = value
        End Set
    End Property
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("FeeName : " & sFeeName & "<br />")
        sbPrint.AppendLine("CurrencyCode: " & sCurrencyCode & "<br />")
        sbPrint.AppendLine("AppliedTo : " & sAppliedTo & "<br />")
        sbPrint.AppendLine("Premium : " & dPremium.ToString & "<br />")
        sbPrint.AppendLine("Rate : " & dRate.ToString & "<br />")
        sbPrint.AppendLine("FeeAmount : " & dFeeAmount.ToString & "<br />")
        sbPrint.AppendLine("TaxAmount : " & dTaxAmount.ToString & "<br />")
        sbPrint.AppendLine("TotalAmount : " & dTotalAmount.ToString & "<br />")
        sbPrint.AppendLine("TaxGroup : " & sTaxGroup & "<br />")
        sbPrint.AppendLine("IncludeInInstallment : " & iIncludeInInstallment.ToString & "<br />")
        sbPrint.AppendLine("SpreadAcrossInstallment : " & iSpreadAcrossInstallment.ToString & "<br />")


        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function
End Class
<Serializable()> Public Class HeaderAndPolicyFeesTypeCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oHeaderAndPolicyFeesType As HeaderAndPolicyFeesType In List
            sbPrint.AppendLine(oHeaderAndPolicyFeesType.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oHeaderAndPolicyFeesType As HeaderAndPolicyFeesType) As Integer
        Return List.Add(v_oHeaderAndPolicyFeesType)
    End Function

    Public Sub Remove(ByVal v_oHeaderAndPolicyFeesType As HeaderAndPolicyFeesType)
        List.Remove(v_oHeaderAndPolicyFeesType)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As HeaderAndPolicyFeesType
        Get
            Return List(i)
        End Get
        Set(ByVal value As HeaderAndPolicyFeesType)
            List(i) = value
        End Set
    End Property

End Class