Public Class BaseQuoteRiskMsgType
    ' From BaseQuoteType
    Public Property AgentKey As Integer
    Public Property AlternativeRef As String
    Public Property AnalysisCode As String
    Public Property CoverEndDate As Date
    Public Property CoverStartDate As Date
    Public Property CurrencyCode As String
    Public Property Description As String
    Public Property InsuredName As String
    Public Property ProductCode As String
    Public Property QuoteRef As String
    Public Property UnderwritingYearCode As String
    ' BaseQuoteRiskMsgType additions
    Public Property AccountExecutiveShortname As String
    Public Property AccountHandlerShortname As String
    Public Property AlternateReference As String
    Public Property BranchCode As String
    Public Property BusinessTypeCode As String
    Public Property CoInsurancePlacement As String
    Public Property CoInsurers As List(Of BaseUpdateCoinsuranceValuesRequestTypeRow)
    Public Property CollectionFrequencyCode As String
    Public Property CommissionRate As Double
    Public Property CommissionValue As Double
    Public Property DeletePolicyUnderRenewal As Integer
    Public Property DoNotCopyRiskAtRenSelection As Boolean
    Public Property IsBDXRequest As Boolean
    Public Property LastTransDescription As String
    Public Property MTAReasonCode As String
    Public Property NewQuoteRef As String
    Public Property OldPolicyNumber As String
    Public Property PartyKey As Integer
    Public Property PaymentTermCode As String
    Public Property PolicyProcessType As PolicyProcessTypes?
    Public Property PolicyStatusCode As String
    Public Property Risks As List(Of BaseQuoteRiskMsgTypeRisks)
    Public Property SAMStagingPolicyKey As Integer
    Public Property SkipGenerateRenewalPolicyNumber As Boolean
    Public Property Taxes As List(Of BaseTaxesType)
    Public Property TransactionDueDate As Date
    Public Property TransactionTypeCode As String
End Class
