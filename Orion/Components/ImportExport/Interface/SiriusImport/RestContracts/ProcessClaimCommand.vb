Public Class ProcessClaimCommand
    Inherits BaseRequestType
    ' From ProcessClaimCommandBase
    Public Property Claim As BaseClaimProcessType
    Public Property ExclusiveLock As Boolean
    Public Property IsMaintainClaim As Boolean
    Public Property SessionValue As String
    Public Property ClaimNumber As String
    Public Property ClaimKey As Integer
    Public Property IsPaymentRequired As Boolean
    Public Property ApiTimeStamp As Byte() = New Byte(-1) {}
    Public Property BaseClaimKey As Integer
    Public Property ClaimRiskXML As String
    Public Property ClaimVersion As Integer
    Public Property SourceId As Integer
End Class
