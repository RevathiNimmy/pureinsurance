<Serializable()> Public Class PaymentHubConfig
    Public Property SystemUserName() As String
    Public Property Password() As String
    Public Property ClientName() As String
    Public Property BrokerSCID() As String
    Public Property MerchantID() As String
    Public Property Customer() As String
    Public Property ReturnURL() As String
    Public Property SystemGUID() As String
    Public Property SystemPasscode() As String
    Public Property AccountID() As String
    Public Property TransactionIPAddress() As String
    Public Property RefundPasscode() As String
    Public Property CaptureMethod() As String
    Public Property RefundPremiumthroughInvoice() As String
    Public Property Donotuseoldcarddetailsforsubsequentpayments() As String
    Public Property MarkDefaultCreditCard() As String
    Public Property LanguageTemplateID() As String
    Public Property MerchantTemplateID() As String
    Public Property AccountPassCode() As String
    Public Property PaymentHubServiceUrl() As String

    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()


    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()

        Return sbPrint.ToString

    End Function

End Class
