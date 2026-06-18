Public Class BaseReceiptCashListItemType
    Inherits BaseCoreCashListItemType

    Public Property TypeCode As String
    Public Property StatusCode As String
    Public Property TypeKey As Integer
    Public Property StatusKey As Integer
    Public Property BankKey As Integer
    Public Property Policies As List(Of BaseReceiptCashListItemTypePolicies)
End Class
