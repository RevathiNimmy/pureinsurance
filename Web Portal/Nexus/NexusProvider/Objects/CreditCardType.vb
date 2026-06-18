<Serializable()> Public Class CreditCardType

    Private iKey As Integer
    Private oCreditCardTypeCardHolder As CardHolder
    Private sExpiryDate As String
    Private sIssue As String
    Private sNameOnCreditCard As String
    Private sNumber As String
    Private sPin As String
    Private sStartDate As String
    Private sTypeCode As String
    Private sAuthCode As String
    Private bCustomerPresent As Boolean
    Private sManualAuthCode As String
    Private sTransactionCode As String
    Private sTrackingNumber As String

    Private nPartyBankKey As Integer
    Private bVIAPaymentHub As Boolean

    Public Sub New()
    End Sub
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Key : " & iKey & "<br />")
        '        sbPrint.AppendLine("Address Type : " & oAddressType.GetName(GetType(AddressType), oAddressType) & "<br />")
        sbPrint.AppendLine("Address Line 1 : " & sExpiryDate & "<br />")
        sbPrint.AppendLine("Address Line 2 : " & sIssue & "<br />")
        sbPrint.AppendLine("Address Line 3 : " & sNameOnCreditCard & "<br />")
        sbPrint.AppendLine("Address Line 4 : " & sNumber & "<br />")
        sbPrint.AppendLine("Country Code : " & sPin & "<br />")
        sbPrint.AppendLine("Post Code : " & sStartDate & "<br />")
        sbPrint.AppendLine("Post Code : " & sTypeCode & "<br />")

        Return sbPrint.ToString

    End Function
    Public Property TrackingNumber() As String
        Get
            Return sTrackingNumber
        End Get
        Set(ByVal value As String)
            sTrackingNumber = value
        End Set
    End Property
    Public Property TransactionCode() As String
        Get
            Return sTransactionCode
        End Get
        Set(ByVal value As String)
            sTransactionCode = value
        End Set
    End Property
    Public Property ManualAuthCode() As String
        Get
            Return sManualAuthCode
        End Get
        Set(ByVal value As String)
            sManualAuthCode = value
        End Set
    End Property
    Public Property VIAPaymentHub() As Boolean
        Get
            Return bVIAPaymentHub
        End Get
        Set(ByVal value As Boolean)
            bVIAPaymentHub = value
        End Set
    End Property
    Public Property CustomerPresent() As Boolean
        Get
            Return bCustomerPresent
        End Get
        Set(ByVal value As Boolean)
            bCustomerPresent = value
        End Set
    End Property
    Public Property AuthCode() As String
        Get
            Return sAuthCode
        End Get
        Set(ByVal value As String)
            sAuthCode = value
        End Set
    End Property
    Public Property Key() As Integer
        Get
            Return iKey
        End Get
        Set(ByVal value As Integer)
            iKey = value
        End Set
    End Property
    Public Property CreditCardType_CardHolder() As CardHolder
        Get
            Return oCreditCardTypeCardHolder
        End Get
        Set(ByVal value As CardHolder)
            oCreditCardTypeCardHolder = value
        End Set
    End Property

    Public Property ExpiryDate() As String
        Get
            Return sExpiryDate
        End Get
        Set(ByVal value As String)
            sExpiryDate = value
        End Set
    End Property

    Public Property Issue() As String
        Get
            Return sIssue
        End Get
        Set(ByVal value As String)
            sIssue = value
        End Set
    End Property

    Public Property NameOnCreditCard() As String
        Get
            Return sNameOnCreditCard
        End Get
        Set(ByVal value As String)
            sNameOnCreditCard = value
        End Set
    End Property

    Public Property Number() As String
        Get
            Return sNumber
        End Get
        Set(ByVal value As String)
            sNumber = value
        End Set
    End Property

    Public Property Pin() As String
        Get
            Return sPin
        End Get
        Set(ByVal value As String)
            sPin = value
        End Set
    End Property

    Public Property StartDate() As String
        Get
            Return sStartDate
        End Get
        Set(ByVal value As String)
            sStartDate = value
        End Set
    End Property
    Public Property TypeCode() As String
        Get
            Return sTypeCode
        End Get
        Set(ByVal value As String)
            sTypeCode = value
        End Set
    End Property
    Public Property PartyBankKey() As Integer
        Get
            Return nPartyBankKey
        End Get
        Set(ByVal value As Integer)
            nPartyBankKey = value
        End Set
    End Property
End Class
<Serializable()> Public Class CreditCardTypeCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oCreditCardType As CreditCardType In List
            sbPrint.AppendLine(oCreditCardType.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oCreditCardType As CreditCardType) As Integer
        Return List.Add(v_oCreditCardType)
    End Function

    Public Sub Remove(ByVal v_oCreditCardType As CreditCardType)
        List.Remove(v_oCreditCardType)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As CreditCardType
        Get
            Return List(i)
        End Get
        Set(ByVal value As CreditCardType)
            List(i) = value
        End Set
    End Property
End Class



