Public Class PolicyProcessCommandResponse
    ' From SamMethodResponseData
    Public Property Errors As List(Of SAMErrors)
    Public Property HandlingInstanceId As Guid?
    ' From BaseResponseType
    Public Property STSError As STSErrorType
    ' From BaseNBQuoteResponseType
    Public Property Policy As List(Of PolicyProcessResponsePolicy)
End Class
