
<Serializable()> Public Class RatingSectionTypes

    Private iRatingSectionTypeId As Integer
    Private sRatingSectionTypeCode As String
    Private sDescription As String
    Private iRateTypeId As Integer
    Private sRateTypeCode As String
    Private dRate As Decimal
    Private iCurrencyId As Integer
    Private sCurrencyCode As String
    Private iCountryId As Integer
    Private sCountryCode As String
    Private iStateId As Integer
    Private sStateCode As String
    Private sEarningPatternCode As String

    Public Sub New()
    End Sub

    Public Property RatingSectionTypeId() As Integer
        Get
            Return iRatingSectionTypeId
        End Get
        Set(ByVal value As Integer)
            iRatingSectionTypeId = value
        End Set
    End Property

    Public Property RatingSectionTypeCode() As String
        Get
            Return sRatingSectionTypeCode
        End Get
        Set(ByVal value As String)
            sRatingSectionTypeCode = value
        End Set
    End Property


    Public Property Description() As String
        Get
            Return sDescription
        End Get
        Set(ByVal value As String)
            sDescription = value
        End Set
    End Property

    Public Property RateTypeId() As Integer
        Get
            Return iRateTypeId
        End Get
        Set(ByVal value As Integer)
            iRateTypeId = value
        End Set
    End Property


    Public Property RateTypeCode() As String
        Get
            Return sRateTypeCode
        End Get
        Set(ByVal value As String)
            sRateTypeCode = value
        End Set
    End Property


    Public Property Rate() As Decimal
        Get
            Return dRate
        End Get
        Set(ByVal value As Decimal)
            dRate = value
        End Set
    End Property
    Public Property CurrencyId() As Integer
        Get
            Return iCurrencyId
        End Get
        Set(ByVal value As Integer)
            iCurrencyId = value
        End Set
    End Property


    Public Property CurrencyCode() As String
        Get
            Return sCurrencyCode
        End Get
        Set(ByVal value As String)
            sCurrencyCode = value
        End Set
    End Property

    Public Property CountryId() As Integer
        Get
            Return iCountryId
        End Get
        Set(ByVal value As Integer)
            iCountryId = value
        End Set
    End Property

    Public Property CountryCode() As String
        Get
            Return sCountryCode
        End Get
        Set(ByVal value As String)
            sCountryCode = value
        End Set
    End Property

    Public Property StateId() As Integer
        Get
            Return iStateId
        End Get
        Set(ByVal value As Integer)
            iStateId = value
        End Set
    End Property

    Public Property StateCode() As String
        Get
            Return sStateCode
        End Get
        Set(ByVal value As String)
            sStateCode = value
        End Set
    End Property
    Public Property EarningPatternCode() As String
        Get
            Return sEarningPatternCode
        End Get
        Set(ByVal value As String)
            sEarningPatternCode = value
        End Set
    End Property

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("RatingSectionTypeId : " & iRatingSectionTypeId.ToString & "<br />")
        sbPrint.AppendLine("RatingSectionTypeCode : " & sRatingSectionTypeCode & "<br />")
        sbPrint.AppendLine("Description : " & sDescription & "<br />")
        sbPrint.AppendLine("RateTypeId : " & iRateTypeId.ToString & "<br />")
        sbPrint.AppendLine("RateTypeCode : " & sRateTypeCode & "<br />")
        sbPrint.AppendLine("Rate : " & dRate.ToString & "<br />")
        sbPrint.AppendLine("CurrencyId : " & iCurrencyId.ToString & "<br />")
        sbPrint.AppendLine("CurrencyCode : " & sCurrencyCode & "<br />")
        sbPrint.AppendLine("CountryId : " & iCountryId.ToString & "<br />")
        sbPrint.AppendLine("CountryCode : " & CountryCode & "<br />")
        sbPrint.AppendLine("StateId : " & iStateId.ToString & "<br />")
        sbPrint.AppendLine("StateCode : " & sStateCode & "<br />")
        sbPrint.AppendLine("EarningPatternCode : " & sEarningPatternCode & "<br />")

        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function

End Class

<Serializable()> Public Class RatingSectionTypesCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oRatingSectionTypes As RatingSectionTypes In List
            sbPrint.AppendLine(oRatingSectionTypes.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oRatingSectionTypes As RatingSectionTypes) As Integer
        Return List.Add(v_oRatingSectionTypes)
    End Function

    Public Sub Remove(ByVal v_oRatingSectionTypes As RatingSectionTypes)
        List.Remove(v_oRatingSectionTypes)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As RatingSectionTypes
        Get
            Return List(i)
        End Get
        Set(ByVal value As RatingSectionTypes)
            List(i) = value
        End Set
    End Property

End Class
