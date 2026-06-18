Public Class BasePaymentCashListItemType
    Inherits BaseCoreCashListItemType

    Public Property StatusCode As String
    Public Property TypeCode As String
    Public Property UserId As Integer
    Public Property StatusKey As Integer
    Public Property TypeKey As Integer
    Public Property UserName As String
    Public Property Policies As List(Of BasePaymentCashListItemTypePolicies)
End Class
