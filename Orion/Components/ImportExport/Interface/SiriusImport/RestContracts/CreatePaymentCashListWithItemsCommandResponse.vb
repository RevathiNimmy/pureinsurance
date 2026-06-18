Public Class CreatePaymentCashListWithItemsCommandResponse
    Public Property CashListKey As Integer
    Public Property CashListItem As List(Of CreatePaymentCashListWithItemsResponseTypeCashListItem)
    Public Property Errors As List(Of SAMErrors) = New List(Of SAMErrors)()
    Public Property HandlingInstanceId As Guid?
    Public Property STSError As STSErrorType
End Class

Public Class CreatePaymentCashListWithItemsResponseTypeCashListItem
    Public Property CashListItemKey As Integer
    Public Property TransDetailKey As Integer
    Public Property AutoAllocatePaymentSuccessful As Boolean
    Public Property AccountShortCode As String
    Public Property DocumentRef As String
    Public Property DocumentCode As String
End Class
