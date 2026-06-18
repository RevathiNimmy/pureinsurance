Public Class BaseClaimProcessType
    ' Key fields from BaseClaimType used by Claim_BDX_Import
    Public Property InsuranceFileKey As Integer
    Public Property BaseClaimKey As Integer
    Public Property RiskKey As Integer
    Public Property TPA As Integer
    Public Property CurrencyCode As String
    Public Property Location As String
    Public Property LossFromDate As Date
    Public Property ReportedDate As Date
    Public Property PrimaryCauseCode As String
    Public Property Description As String
    Public Property Comments As String
    ' BaseClaimProcessType additions
    Public Property ClaimKey As Integer
    Public Property InsuranceFolderKey As Integer
    Public Property UnderwritingYearCode As String
    Public Property GisScreenCode As String
    Public Property ClaimPeril As List(Of BaseClaimProcessPerilType)
    Public Property IgnoreWarnings As Boolean
    Public Property ExternalHandler As Boolean
    Public Property ClaimBuilderDetail As List(Of BaseClaimProcessBuilderRiskType)
    Public Property Payee As BaseClaimPayeeType
    Public Property IsTPASettleDirectly As Boolean
    ' ReceiptPayee is [JsonIgnore] in REST — kept for code compatibility but won't be serialised
    Public Property ReceiptPayee As BaseClaimReceiptPayeeType
    Public Property CatastropheCode As String
    Public Property ClaimNumber As String
    Public Property ClaimStatus As String
    Public Property ClaimStatusDate As Date
    Public Property ClaimVersion As Integer
    Public Property ClaimVersionDescription As String
    Public Property ClientEmail As String
    Public Property ClientFaxNo As String
    Public Property ClientMobileNo As String
    Public Property ClientName As String
    Public Property ClientTelNo As String
    Public Property CloseClaimOnZeroReserveRecoveryBalance As Boolean
    Public Property ExclusiveLock As Boolean
    Public Property HandlerCode As String
    Public Property InfoOnly As Boolean
    Public Property InsuranceFileRef As String
    Public Property LastModifiedDate As Date
    Public Property LikelyClaim As Boolean
    Public Property LossToDate As Date
    Public Property LossToDateSpecified As Boolean
    Public Property ProgressStatusCode As String
    Public Property SecondaryCauseCode As String
    Public Property SessionValue As String
End Class
