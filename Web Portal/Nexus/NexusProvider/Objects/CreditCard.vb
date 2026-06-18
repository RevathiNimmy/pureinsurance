
<Serializable()> Public Class CreditCard : Inherits CardHolder

    Private sNumber As String
    Private sExpiryDate As String
    Private sStartDate As String
    Private sNameOnCreditCard As String
    Private sTypeCode As String
    Private sIssue As String
    Private sPin As String
    Private oCardHolder As CardHolder
    Private sAuthCode As String
    Private sManualAuthCode As String
    Private sTransactionCode As String
    Private bCustomerPresent As Boolean
    Private sCCTypeOfCard, sCCIssueBank, sCCSlipNumber As String
    Private cCIssueField As String
    Private cCCustomerField As String
    Private cCPinField As String
    Private sAccountType As String
    Private sCashListItemBankCodeField As String
    Private nPartyBankKey As Integer
    Private sTrackingNumber As String
    Private bIsDefault As Boolean
    Private bVIAPaymentHub As Boolean
    Public Sub New()
        oCardHolder = New CardHolder
    End Sub
    '''<remarks/>
    Public Property CCCustomer() As String
        Get
            Return Me.cCCustomerField
        End Get
        Set(ByVal value As String)
            Me.cCCustomerField = value
        End Set
    End Property
    '''<remarks/>
    Public Property CCIssue() As String
        Get
            Return Me.cCIssueField
        End Get
        Set(ByVal value As String)
            Me.cCIssueField = value
        End Set
    End Property

    '''<remarks/>
    Public Property CCPin() As String
        Get
            Return Me.cCPinField
        End Get
        Set(ByVal value As String)
            Me.cCPinField = value
        End Set
    End Property
    '''<remarks/>
    Public Property CCSlipNumber() As String
        Get
            Return Me.sCCSlipNumber
        End Get
        Set(ByVal value As String)
            Me.sCCSlipNumber = value
        End Set
    End Property
    '''<remarks/>
    Public Property CCIssueBank() As String
        Get
            Return Me.sCCIssueBank
        End Get
        Set(ByVal value As String)
            Me.sCCIssueBank = value
        End Set
    End Property
    '''<remarks/>
    Public Property CCTypeOfCard() As String
        Get
            Return Me.sCCTypeOfCard
        End Get
        Set(ByVal value As String)
            Me.sCCTypeOfCard = value
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
    Public Property ExpiryDate() As String
        Get
            Return sExpiryDate
        End Get
        Set(ByVal value As String)
            sExpiryDate = value
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
    Public Property NameOnCreditCard() As String
        Get
            Return sNameOnCreditCard
        End Get
        Set(ByVal value As String)
            sNameOnCreditCard = value
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
    Public Property Issue() As String
        Get
            Return sIssue
        End Get
        Set(ByVal value As String)
            sIssue = value
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
    Public Property CardHolder() As CardHolder
        Get
            Return oCardHolder
        End Get
        Set(ByVal value As CardHolder)
            oCardHolder = value
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
    Public Property ManualAuthCode() As String
        Get
            Return sManualAuthCode
        End Get
        Set(ByVal value As String)
            sManualAuthCode = value
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


    Public Property CustomerPresent() As Boolean
        Get
            Return bCustomerPresent
        End Get
        Set(ByVal value As Boolean)
            bCustomerPresent = value
        End Set
    End Property
    Public Property IsDefaultCreditCard() As Boolean
        Get
            Return bIsDefault
        End Get
        Set(ByVal value As Boolean)
            bIsDefault = value
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
    ''' <summary>
    ''' Public Property to maintain AccountType
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AccountType() As String
        Get
            Return sAccountType
        End Get
        Set(ByVal value As String)
            sAccountType = value
        End Set
    End Property

    '''<remarks/>
    Public Property CashListItemBankCode() As String
        Get
            Return Me.sCashListItemBankCodeField
        End Get
        Set(ByVal value As String)
            Me.sCashListItemBankCodeField = value
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

    Public Property TrackingNumber() As String
        Get
            Return Me.sTrackingNumber
        End Get
        Set(ByVal value As String)
            Me.sTrackingNumber = value
        End Set
    End Property
End Class



<Serializable()> Public Class CreditCardCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface to the object
    ''' </summary>
    ''' <returns>An HTML string containining data held within the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oCreditCard As CreditCard In List
            sbPrint.AppendLine(oCreditCard.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a credit card object to the collection
    ''' </summary>
    ''' <param name="v_oCreditCard">Credit card added to a collection</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oCreditCard As CreditCard) As Integer
        Return List.Add(v_oCreditCard)
    End Function

    ''' <summary>
    ''' Remove a credit card object from the collection
    ''' </summary>
    ''' <param name="v_oCreditCard">Credit card object to be removed</param>
    Public Sub Remove(ByVal v_oCreditCard As CreditCard)
        List.Remove(v_oCreditCard)
    End Sub

    ''' <summary>
    ''' Remove an Address object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Address object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an Address object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Address object</param>
    ''' <value>The replacement Address object</value>
    ''' <returns>The Address object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As CreditCard
        Get
            Return List(i)
        End Get
        Set(ByVal value As CreditCard)
            List(i) = value
        End Set
    End Property

    ''' <summary>
    ''' Return the first credit card object in the collection with the specified CreditCardType
    ''' </summary>
    ''' <param name="v_oCreditCardType">The Credit card type of the credit card object to be returned</param>
    ''' <value>The AddressType the Address is to be retrieved by</value>
    ''' <returns>Matching Address object, if any</returns>
    Default Public ReadOnly Property Item(ByVal v_oCreditCardType As CreditCard) As CreditCard
        Get
            Return FindItemByCreditCardType(v_oCreditCardType)
        End Get
    End Property


    Public Function FindItemByCreditCardType(ByVal v_oCreditCardType As CreditCard) As CreditCard

        For Each oCreditCard As CreditCard In List
            If oCreditCard Is v_oCreditCardType Then
                Return oCreditCard
            End If
        Next

        Return Nothing

    End Function

End Class