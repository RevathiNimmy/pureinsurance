Public Class ProcessClaimCommandResponse
    ' From SamMethodResponseData via BaseResponseType
    Public Property Errors As List(Of SAMErrors)
    Public Property HandlingInstanceId As Guid?
    Public Property STSError As STSErrorType
    ' From BaseClaimResponseType
    Public Property BaseClaimKey As Integer
    Public Property ClaimKey As Integer
    Public Property ClaimNumber As String
    Public Property PaymentAuthorized As Boolean
    Public Property ResultingStatus As String
    Public Property ApiTimeStamp As Byte()
    Public Property Version As Integer
    Public Property Warnings As List(Of BaseClaimResponseTypeWarnings)
End Class
