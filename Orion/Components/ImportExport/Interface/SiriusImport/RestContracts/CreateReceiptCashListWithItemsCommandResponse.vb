Public Class CreateReceiptCashListWithItemsCommandResponse
    Public Property CashListKey As Integer
    Public Property CashListItem As List(Of CreateReceiptCashListWithItemsResponseTypeCashListItem)
    Public Property Errors As List(Of SAMErrors) = New List(Of SAMErrors)()
    Public Property HandlingInstanceId As Guid?
    Public Property STSError As STSErrorType
End Class

Public Class CreateReceiptCashListWithItemsResponseTypeCashListItem
    Public Property AccountShortCode As String
    Public Property CashListItemKey As Integer
    Public Property DocumentCode As String
    Public Property DocumentRef As String
    Public Property TransDetailKey As Integer
End Class
