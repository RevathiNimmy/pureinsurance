Public Class BaseQuoteRiskMsgTypeRisks
    ' From BaseRiskType
    Public Property BranchCode As String
    Public Property DataModelCode As String
    Public Property RiskDescription As String
    Public Property RiskTypeCode As String
    Public Property RunDefaultRules As Boolean
    Public Property ScreenCode As String
    Public Property XMLDataSet As String
    Public Property ProductBuilderDetail As List(Of BaseProductBuilderRiskType)
    Public Property Taxes As List(Of BaseTaxesType)
    ' BaseQuoteRiskMsgTypeRisks additions
    Public Property RatingSections As List(Of BaseRiskRatingSectionType)
    Public Property RiskFolderKey As Integer
    Public Property RiskFolderKeySpecified As Boolean
    Public Property RiskKey As Integer
    Public Property OriginalRiskKey As Integer
    Public Property OriginalRiskKeySpecified As Boolean
    Public Property SAMStagingRiskKey As Integer
End Class
