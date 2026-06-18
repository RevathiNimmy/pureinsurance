<Serializable()> Public Class Rating

    Private dAnnualPremium As Decimal
    Private dAnnualRate As Decimal
    Private dCalculatedPremium As Decimal
    Private sCountry As String
    Private nCountryID As Integer
    Private nCurrencyID As Integer
    Private sCurrencyCode As String
    Private iIsAmmended As Integer
    Private iOriginalFlag As Integer
    Private sOverrideReason As String
    Private sPolicySectionType As String
    Private nPolicySectionTypeID As Integer
    Private sRateType As String
    Private nRateTypeID As Integer
    Private sRateTypeCode As String
    Private nRateSectionID As Integer
    Private sRatingSectionType As String
    Private nRatingSectionTypeID As Integer
    Private sState As String
    Private nStateID As Integer
    Private dSumInsured As Decimal
    Private dThisPremium As Decimal
    Private sEarningPatternCode As String
    Private sRatingSectionTypeCode As String
    Private sRatingTypeCode As String
    Private sCountryCode As String
    Private sStateCode As String
    Private nEarningPatternId As Integer
    Private sEarningPattern As String

    Public Sub New()

    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("Annual Premium : " & dAnnualPremium.ToString() & "<br />")
        sbPrint.AppendLine("Annual Rate : " & dAnnualRate.ToString() & "<br />")
        sbPrint.AppendLine("Calculated Premium : " & dCalculatedPremium.ToString() & "<br />")
        sbPrint.AppendLine("Country : " & sCountry & "<br />")
        sbPrint.AppendLine("Country ID : " & nCountryID.ToString() & "<br />")
        sbPrint.AppendLine("Currency ID : " & nCurrencyID.ToString() & "<br />")
        sbPrint.AppendLine("Is Ammended : " & iIsAmmended.ToString() & "<br />")
        sbPrint.AppendLine("Original Flag : " & iOriginalFlag.ToString() & "<br />")
        sbPrint.AppendLine("Override Reason : " & sOverrideReason & "<br />")
        sbPrint.AppendLine("Policy Section Type : " & sPolicySectionType & "<br />")
        sbPrint.AppendLine("Policy Section Type ID : " & nPolicySectionTypeID.ToString() & "<br />")
        sbPrint.AppendLine("Rate Type : " & sRateType & "<br />")
        sbPrint.AppendLine("Rate Type ID : " & nRateTypeID.ToString() & "<br />")
        sbPrint.AppendLine("Rate Section ID : " & nRateSectionID.ToString() & "<br />")
        sbPrint.AppendLine("Rating Section Type : " & sRatingSectionType & "<br />")
        sbPrint.AppendLine("Rating Section Type ID : " & nRatingSectionTypeID.ToString() & "<br />")
        sbPrint.AppendLine("State : " & sState & "<br />")
        sbPrint.AppendLine("State ID : " & nStateID.ToString() & "<br />")
        sbPrint.AppendLine("Sum Insured : " & dSumInsured.ToString() & "<br />")
        sbPrint.AppendLine("This Premium : " & dThisPremium.ToString() & "<br />")

        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function
    Public Property EarningPatternCode() As String
        Get
            Return sEarningPatternCode
        End Get
        Set(ByVal value As String)
            sEarningPatternCode = value
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
    Public Property RatingTypeCode() As String
        Get
            Return sRatingTypeCode
        End Get
        Set(ByVal value As String)
            sRatingTypeCode = value
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
    Public Property StateCode() As String
        Get
            Return sStateCode
        End Get
        Set(ByVal value As String)
            sStateCode = value
        End Set
    End Property
    Public Property EarningPatternId() As Integer
        Get
            Return nEarningPatternId
        End Get
        Set(ByVal value As Integer)
            nEarningPatternId = value
        End Set
    End Property
    Public Property EarningPattern() As String
        Get
            Return sEarningPattern
        End Get
        Set(ByVal value As String)
            sEarningPattern = value
        End Set
    End Property
    Public Property AnnualPremium() As Decimal
        Get
            Return dAnnualPremium
        End Get
        Set(ByVal value As Decimal)
            dAnnualPremium = value
        End Set
    End Property
    Public Property AnnualRate() As Decimal
        Get
            Return dAnnualRate
        End Get
        Set(ByVal value As Decimal)
            dAnnualRate = value
        End Set
    End Property
    Public Property CalculatedPremium() As Decimal
        Get
            Return dCalculatedPremium
        End Get
        Set(ByVal value As Decimal)
            dCalculatedPremium = value
        End Set
    End Property
    Public Property Country() As String
        Get
            Return sCountry
        End Get
        Set(ByVal value As String)
            sCountry = value
        End Set
    End Property
    Public Property CountryID() As Integer
        Get
            Return nCountryID
        End Get
        Set(ByVal value As Integer)
            nCountryID = value
        End Set
    End Property
    Public Property CurrencyID() As Integer
        Get
            Return nCurrencyID
        End Get
        Set(ByVal value As Integer)
            nCurrencyID = value
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
    Public Property IsAmmended() As Integer
        Get
            Return iIsAmmended
        End Get
        Set(ByVal value As Integer)
            iIsAmmended = value
        End Set
    End Property
    Public Property OriginalFlag() As Integer
        Get
            Return iOriginalFlag
        End Get
        Set(ByVal value As Integer)
            iOriginalFlag = value
        End Set
    End Property
    Public Property OverrideReason() As String
        Get
            Return sOverrideReason
        End Get
        Set(ByVal value As String)
            sOverrideReason = value
        End Set
    End Property
    Public Property PolicySectionType() As String
        Get
            Return sPolicySectionType
        End Get
        Set(ByVal value As String)
            sPolicySectionType = value
        End Set
    End Property
    Public Property PolicySectionTypeID() As Integer
        Get
            Return nPolicySectionTypeID
        End Get
        Set(ByVal value As Integer)
            nPolicySectionTypeID = value
        End Set
    End Property
    Public Property RateType() As String
        Get
            Return sRateType
        End Get
        Set(ByVal value As String)
            sRateType = value
        End Set
    End Property
    Public Property RateTypeID() As Integer
        Get
            Return nRateTypeID
        End Get
        Set(ByVal value As Integer)
            nRateTypeID = value
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
    Public Property RateSectionID() As Integer
        Get
            Return nRateSectionID
        End Get
        Set(ByVal value As Integer)
            nRateSectionID = value
        End Set
    End Property
    Public Property RatingSectionType() As String
        Get
            Return sRatingSectionType
        End Get
        Set(ByVal value As String)
            sRatingSectionType = value
        End Set
    End Property
    Public Property RatingSectionTypeID() As Integer
        Get
            Return nRatingSectionTypeID
        End Get
        Set(ByVal value As Integer)
            nRatingSectionTypeID = value
        End Set
    End Property
    Public Property State() As String
        Get
            Return sState
        End Get
        Set(ByVal value As String)
            sState = value
        End Set
    End Property
    Public Property StateID() As Integer
        Get
            Return nStateID
        End Get
        Set(ByVal value As Integer)
            nStateID = value
        End Set
    End Property

    Public Property SumInsured() As Decimal
        Get
            Return dSumInsured
        End Get
        Set(ByVal value As Decimal)
            dSumInsured = value
        End Set
    End Property

    Public Property ThisPremium() As Decimal
        Get
            Return dThisPremium
        End Get
        Set(ByVal value As Decimal)
            dThisPremium = value
        End Set
    End Property
End Class

<Serializable()> Public Class RatingCollection : Inherits CollectionBase

    Public Function Add(ByVal v_oRating As Rating) As Integer
        Return List.Add(v_oRating)
    End Function

    Public Sub Remove(ByVal v_oRating As Rating)
        List.Remove(v_oRating)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As Rating
        Get
            Return List(i)
        End Get
        Set(ByVal value As Rating)
            List(i) = value
        End Set
    End Property
    Public Function GetValue(ByVal index As Integer, ByVal sSearchName As String) As String
        'To Store Properties and their values
        Dim sPropVal(10, 1) As String
        Dim sReturnStr As String = ""

        Dim iCounter As Integer
        sPropVal(0, 0) = "RateType"
        sPropVal(0, 1) = CType(List(index), Rating).RateType
        sPropVal(1, 0) = "SumInsured"
        sPropVal(1, 1) = CType(List(index), Rating).SumInsured
        sPropVal(2, 0) = "ThisPremium"
        sPropVal(2, 1) = CType(List(index), Rating).ThisPremium
        sPropVal(3, 0) = "AnnualRate"
        sPropVal(3, 1) = CType(List(index), Rating).AnnualRate
        sPropVal(4, 0) = "AnnualPremium"
        sPropVal(4, 1) = CType(List(index), Rating).AnnualPremium
        sPropVal(5, 0) = "CalculatedPremium"
        sPropVal(5, 1) = CType(List(index), Rating).CalculatedPremium
        sPropVal(6, 0) = "RatingSectionType"
        sPropVal(6, 1) = CType(List(index), Rating).RatingSectionType
        sPropVal(7, 0) = "RatingSectionTypeID"
        sPropVal(7, 1) = CType(List(index), Rating).RatingSectionTypeID
        sPropVal(8, 0) = "RateSectionID"
        sPropVal(8, 1) = CType(List(index), Rating).RateSectionID

        'Matching the properties with the requested sSearchName
        For iCounter = 0 To UBound(sPropVal)
            If Not sPropVal(iCounter, 0) Is Nothing Then
                If sPropVal(iCounter, 0).Trim.ToUpper = sSearchName.Trim.ToUpper Then
                    sReturnStr = sPropVal(iCounter, 1).ToString
                    Exit For
                End If
            End If
        Next
        Return sReturnStr
    End Function
    Public ReadOnly Property GetRatingItem(ByVal RatingSectionID As Integer) As Rating
        Get
            For Each oRating As Rating In List
                If oRating.RateSectionID = RatingSectionID And oRating.OriginalFlag = 0 Then
                    Return oRating
                End If
            Next
        End Get

    End Property
End Class
