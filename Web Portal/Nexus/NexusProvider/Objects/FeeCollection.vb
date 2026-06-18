<Serializable()> Partial Public Class FeeCollectionBase

    Private sFeeName As String
    Private sCurrencyCode As String
    Private sFeeAppliedTo As String
    Private dPremium As Double
    Private dRate As Double
    Private dFeeAmount As Double
    Private dTaxAmount As Double
    Private dTotalAmount As Double
    Private sTaxGroup As String
    Private iIncludeInInstallment As Integer
    Private iSpreadAcrossInstallment As Integer


    ''' <summary>
    ''' Currency Code
    ''' </summary>
    ''' <value>Currency Code</value>
    ''' <returns>Currency Code</returns>
    Public Property FeeName() As String
        Get
            Return sFeeName
        End Get
        Set(ByVal value As String)
            sFeeName = value
        End Set
    End Property



    ''' <summary>
    ''' Currency Code
    ''' </summary>
    ''' <value>Currency Code</value>
    ''' <returns>Currency Code</returns>
    Public Property CurrencyCode() As String
        Get
            Return sCurrencyCode
        End Get
        Set(ByVal value As String)
            sCurrencyCode = value
        End Set
    End Property



    ''' <summary>
    ''' Currency Code
    ''' </summary>
    ''' <value>Currency Code</value>
    ''' <returns>Currency Code</returns>
    Public Property FeeAppliedTo() As String
        Get
            Return sFeeAppliedTo
        End Get
        Set(ByVal value As String)
            sFeeAppliedTo = value
        End Set
    End Property



    ''' <summary>
    ''' Currency Code
    ''' </summary>
    ''' <value>Currency Code</value>
    ''' <returns>Currency Code</returns>
    Public Property Premium() As Double
        Get
            Return dPremium
        End Get
        Set(ByVal value As Double)
            dPremium = value
        End Set
    End Property


    ''' <summary>
    ''' Currency Code
    ''' </summary>
    ''' <value>Currency Code</value>
    ''' <returns>Currency Code</returns>
    Public Property Rate() As Double
        Get
            Return dRate
        End Get
        Set(ByVal value As Double)
            dRate = value
        End Set
    End Property

    ''' <summary>
    ''' Currency Code
    ''' </summary>
    ''' <value>Currency Code</value>
    ''' <returns>Currency Code</returns>
    Public Property FeeAmount() As Double
        Get
            Return dFeeAmount
        End Get
        Set(ByVal value As Double)
            dFeeAmount = value
        End Set
    End Property

    ''' <summary>
    ''' Currency Code
    ''' </summary>
    ''' <value>Currency Code</value>
    ''' <returns>Currency Code</returns>
    Public Property TaxAmount() As Double
        Get
            Return dTaxAmount
        End Get
        Set(ByVal value As Double)
            dTaxAmount = value
        End Set
    End Property

    ''' <summary>
    ''' Currency Code
    ''' </summary>
    ''' <value>Currency Code</value>
    ''' <returns>Currency Code</returns>
    Public Property TotalAmount() As Double
        Get
            Return dTotalAmount
        End Get
        Set(ByVal value As Double)
            dTotalAmount = value
        End Set
    End Property


    ''' <summary>
    ''' Currency Code
    ''' </summary>
    ''' <value>Currency Code</value>
    ''' <returns>Currency Code</returns>
    Public Property TaxGroup() As String
        Get
            Return sTaxGroup
        End Get
        Set(ByVal value As String)
            sTaxGroup = value
        End Set
    End Property



    ''' <summary>
    ''' BGKey
    ''' </summary>
    ''' <value>BGKey</value>
    ''' <returns>BGKey</returns>
    Public Property IncludeInInstallment() As Integer
        Get
            Return iIncludeInInstallment
        End Get
        Set(ByVal value As Integer)
            iIncludeInInstallment = value
        End Set
    End Property



    ''' <summary>
    ''' BGKey
    ''' </summary>
    ''' <value>BGKey</value>
    ''' <returns>BGKey</returns>
    Public Property SpreadAcrossInstallment() As Integer
        Get
            Return iSpreadAcrossInstallment
        End Get
        Set(ByVal value As Integer)
            iSpreadAcrossInstallment = value
        End Set
    End Property


    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Overridable Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Type : " & Me.GetType.Name & "<br />")
        sbPrint.AppendLine("FeeName   : " & sFeeName & "<br />")
        sbPrint.AppendLine("CurrencyCode   : " & sCurrencyCode & "<br />")
        sbPrint.AppendLine("FeeAppliedTo   : " & sFeeAppliedTo & "<br />")
        sbPrint.AppendLine("Premium    : " & dPremium.ToString() & "<br />")
        sbPrint.AppendLine("Rate    : " & dRate.ToString() & "<br />")
        sbPrint.AppendLine("Fee Amount    : " & dFeeAmount.ToString() & "<br />")
        sbPrint.AppendLine("Tax Amount    : " & dTaxAmount.ToString() & "<br />")
        sbPrint.AppendLine("TotalAmount    : " & dTotalAmount.ToString() & "<br />")
        sbPrint.AppendLine("TaxGroup    : " & sTaxGroup & "<br />")
        sbPrint.AppendLine("Include In Installment    : " & iIncludeInInstallment.ToString() & "<br />")
        sbPrint.AppendLine("Spread Across Installment   : " & iSpreadAcrossInstallment.ToString() & "<br />")
        sbPrint.AppendLine("<br />")
        Return sbPrint.ToString

    End Function
End Class

<Serializable()> Public Class FeeCollection : Inherits CollectionBase
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oFee As FeeCollection In List
            sbPrint.AppendLine(oFee.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function
    Public Function Add(ByVal v_oFeeCollectionBase As FeeCollectionBase) As Integer
        Return List.Add(v_oFeeCollectionBase)
    End Function

    Public Sub Remove(ByVal v_oFeeCollectionBase As FeeCollectionBase)
        List.Remove(v_oFeeCollectionBase)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As FeeCollectionBase
        Get
            Return List(i)
        End Get
        Set(ByVal value As FeeCollectionBase)
            List(i) = value
        End Set
    End Property

End Class
