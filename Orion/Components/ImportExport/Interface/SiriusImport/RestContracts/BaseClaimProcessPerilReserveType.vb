Public Class BaseClaimProcessPerilReserveType
    Public Property IsPaidToDate As Boolean
    Public Property IsReserveToDate As Boolean
    Public Property TypeCode As String
    Public Property Amount As Decimal
    Public Property TaxGroupCode As String
    Public Property PaymentAmount As Decimal
    Public Property PaymentAmountSpecified As Boolean
    Public Property ReverseExcess As Boolean = False
    Public Property PaymentDetails As BaseClaimProcessPaymentDetailsType
End Class
