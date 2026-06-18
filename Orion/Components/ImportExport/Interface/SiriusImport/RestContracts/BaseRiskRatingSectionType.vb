''' <summary>
''' Rating section data for a risk — maps to PureInsurance.REST.Common.Domain.Models.BaseRiskRatingSectionType
''' </summary>
Public Class BaseRiskRatingSectionType
    Public Property RatingSectionTypeCode As String
    Public Property RatingSectionTypeId As Integer
    Public Property SequenceNumber As Integer
    Public Property RateTypeCode As String
    Public Property RateTypeID As Integer
    Public Property AnnualRate As Double
    Public Property SumInsured As Double
    Public Property SumInsuredSpecified As Boolean
    Public Property AnnualPremium As Double
    Public Property AnnualPremiumSpecified As Boolean
    Public Property ThisPremium As Double
    Public Property ThisPremiumSpecified As Boolean
    Public Property CountryCode As String
    Public Property CountryID As Integer
    Public Property StateCode As String
    Public Property StateID As Integer
    Public Property OriginalFlag As Boolean
End Class
