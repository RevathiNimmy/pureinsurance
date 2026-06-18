<Serializable()> Public Class PaymentHubDetails
    Public Property CreditCard As CreditCard

    Public Property TransactionID As String
    Public Property IntegrationToken As String
    Public Property AuthCode As String
    Public Property TransactionAmount As String
    Public Property SchemeName As String
    Public Property CardNumber As String
    Public Property CardExpiry As String
    Public Property TokenID As String

    Public Property TokenExpirationDate As String
    Public Property ResultCode As String
    Public Property ResultDescription As String
    Public Property ProviderResponseMessage As String
    Public Property RequestType As Integer
    Public Property PartyBankKey As Integer
    Public Property TransactionCurrency As String
    Public Property ErrorCode As String
    Public Property ErrorDescription As String
    Public Property ReturnURL As String
	Public Property CashListItemIndex As Integer
    Public Property PartyKey As Integer
    Public Property PartyCode As String
    Public Property CustomerDetails As CustomerDetails

    Public Sub New()
        CreditCard = New CreditCard
        CustomerDetails = New CustomerDetails
    End Sub
End Class

<Serializable()> Public Class CustomerDetails
    Public Property Address1 As String
    Public Property Address2 As String
    Public Property Town As String
    Public Property County As String
    Public Property Country As String
    Public Property Postcode As String
    Public Property Email As String
    Public Property Firstname As String
    Public Property Lastname As String
End Class


