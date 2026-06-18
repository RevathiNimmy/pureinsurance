Public Class PolicyProcessResponsePolicy
    Public Property PolicyID As Integer
    Public Property InsuranceFileKey As Integer
    Public Property InsuranceFolderKey As Integer
    Public Property QuoteRef As String
    Public Property PremiumDueNet As Decimal
    Public Property PremiumDueTax As Decimal
    Public Property PremiumDueGross As Decimal
    Public Property TotalAnnualTax As Decimal
    Public Property CommissionAmount As Decimal
    Public Property QuoteTimeStamp As Byte()
    Public Property STSError As STSErrorType
End Class
