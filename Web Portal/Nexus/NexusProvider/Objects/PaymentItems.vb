Imports Microsoft.Web.Services3.Security.Tokens
''' <summary>
''' Nexus BasePerilClaim object, containing the common elements between the various perilclaim.
''' </summary>
''' <remarks></remarks>
''' 
<Serializable()> Public Class PaymentItems
    Private nCashListItemKey As Integer
    Private sAccountShortCode As String
    Private sAllocationStatusCode As String
    Private dAmount As Decimal
    Private dAmountTendered As Decimal
    Private dOriginalAmount As Decimal
    Private sBankReference As String
    Private sContactName As String
    Private sFurtherDetails As String
    Private bIsProduceDocument As Boolean
    Private sMediaReference As String
    Private sMediaTypeCode As String
    Private sOurReference As String
    Private sStatusCode As String
    Private sTheirReference As String
    Private dTransactionDate As Date
    Private sTypeCode As String
    Private sLetter As String
    Private oCreditCard As CreditCard
    Private oBank As Bank
    Private oAddress As Address
    Private sKey As String
    Private collectionDateField As Date
    <System.Runtime.Serialization.OptionalField()> Private dCurrencyBaseDate As DateTime
    <System.Runtime.Serialization.OptionalField()> Private dCurrencyBaseXrate As Decimal
    <System.Runtime.Serialization.OptionalField()> Private dAccountBaseDate As DateTime
    <System.Runtime.Serialization.OptionalField()> Private dAccountBaseXrate As Decimal
    <System.Runtime.Serialization.OptionalField()> Private dSystemBaseDate As DateTime
    <System.Runtime.Serialization.OptionalField()> Private dSystemBaseXrate As Decimal
    <System.Runtime.Serialization.OptionalField()> Private nOverrideReason As Integer


    Private oPolicies As ReceiptCashListItemTypePoliciesCollection
    Private sTaxBandCode As String
    Private nTaxBandKey As Integer
    Private dTaxAmount As Decimal
    Private bSkipPosting As Boolean
    Private sBIC As String
    Private sIBAN As String
    Private oPaymentHubDetails As PaymentHubDetails
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()
        oCreditCard = New CreditCard
        oBank = New Bank
        oAddress = New Address
        oPolicies = New ReceiptCashListItemTypePoliciesCollection
        oPaymentHubDetails = New PaymentHubDetails
    End Sub
    Public Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("Account ShortCode : " & sAccountShortCode & "<br />")
        sbPrint.AppendLine("AllocationStatus Code : " & sAllocationStatusCode & "<br />")
        sbPrint.AppendLine("Amount : " & dAmount & "<br />")
        sbPrint.AppendLine("ContactName : " & sContactName & "<br />")
        sbPrint.AppendLine("Further Details : " & sFurtherDetails & "<br />")
        sbPrint.AppendLine("Is ProduceDocument : " & bIsProduceDocument & "<br />")
        sbPrint.AppendLine("Media Reference  : " & sMediaReference & "<br />")
        Return sbPrint.ToString

    End Function

    ''' <summary>
    ''' CashListItem Key as Integer
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CashListItemKey() As Integer
        Get
            Return Me.nCashListItemKey
        End Get
        Set(ByVal value As Integer)
            Me.nCashListItemKey = value
        End Set
    End Property

    Public Property Policies() As ReceiptCashListItemTypePoliciesCollection
        Get
            Return Me.oPolicies
        End Get
        Set(ByVal value As ReceiptCashListItemTypePoliciesCollection)
            Me.oPolicies = value
        End Set
    End Property
    Public Property AccountShortCode() As String
        Get
            Return Me.sAccountShortCode
        End Get
        Set(ByVal value As String)
            Me.sAccountShortCode = value
        End Set
    End Property
    Public Property AllocationStatusCode() As String
        Get
            Return Me.sAllocationStatusCode
        End Get
        Set(ByVal value As String)
            Me.sAllocationStatusCode = value
        End Set
    End Property
    Public Property Amount() As Double
        Get
            Return Me.dAmount
        End Get
        Set(ByVal value As Double)
            Me.dAmount = value
        End Set
    End Property

    Public Property Amount_tendered() As Double
        Get
            Return Me.dAmountTendered
        End Get
        Set(ByVal value As Double)
            Me.dAmountTendered = value
        End Set
    End Property

    Public Property Original_amount() As Double
        Get
            Return Me.dOriginalAmount
        End Get
        Set(ByVal value As Double)
            Me.dOriginalAmount = value
        End Set
    End Property
    Public Property BankReference() As String
        Get
            Return Me.sBankReference
        End Get
        Set(ByVal value As String)
            Me.sBankReference = value
        End Set
    End Property
    Public Property ContactName() As String
        Get
            Return Me.sContactName
        End Get
        Set(ByVal value As String)
            Me.sContactName = value
        End Set
    End Property
    Public Property FurtherDetails() As String
        Get
            Return Me.sFurtherDetails
        End Get
        Set(ByVal value As String)
            Me.sFurtherDetails = value
        End Set
    End Property
    Public Property MediaReference() As String
        Get
            Return Me.sMediaReference
        End Get
        Set(ByVal value As String)
            Me.sMediaReference = value
        End Set
    End Property
    Public Property MediaTypeCode() As String
        Get
            Return Me.sMediaTypeCode
        End Get
        Set(ByVal value As String)
            Me.sMediaTypeCode = value
        End Set
    End Property
    Public Property OurReference() As String
        Get
            Return Me.sOurReference
        End Get
        Set(ByVal value As String)
            Me.sOurReference = value
        End Set
    End Property
    Public Property StatusCode() As String
        Get
            Return Me.sStatusCode
        End Get
        Set(ByVal value As String)
            Me.sStatusCode = value
        End Set
    End Property
    Public Property TheirReference() As String
        Get
            Return Me.sTheirReference
        End Get
        Set(ByVal value As String)
            Me.sTheirReference = value
        End Set
    End Property
    Public Property TypeCode() As String
        Get
            Return Me.sTypeCode
        End Get
        Set(ByVal value As String)
            Me.sTypeCode = value
        End Set
    End Property

    Public Property IsProduceDocument() As Boolean
        Get
            Return Me.bIsProduceDocument
        End Get
        Set(ByVal value As Boolean)
            Me.bIsProduceDocument = value
        End Set
    End Property

    Public Property TransactionDate() As Date
        Get
            Return Me.dTransactionDate
        End Get
        Set(ByVal value As Date)
            Me.dTransactionDate = value
        End Set
    End Property
    Public Property Bank() As Bank
        Get
            Return Me.oBank
        End Get
        Set(ByVal value As Bank)
            Me.oBank = value
        End Set
    End Property
    Public Property CreditCard() As CreditCard
        Get
            Return Me.oCreditCard
        End Get
        Set(ByVal value As CreditCard)
            Me.oCreditCard = value
        End Set
    End Property
    Public Property Address() As Address
        Get
            Return Me.oAddress
        End Get
        Set(ByVal value As Address)
            Me.oAddress = value
        End Set
    End Property
    Public Property Letter() As String
        Get
            Return Me.sLetter
        End Get
        Set(ByVal value As String)
            Me.sLetter = value
        End Set
    End Property
    Public Property Key() As String
        Get
            Return sKey
        End Get
        Set(ByVal value As String)
            sKey = value
        End Set
    End Property
    ''' <summary>
    ''' TaxBandCode
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TaxBandCode() As String
        Get
            Return sTaxBandCode
        End Get
        Set(ByVal value As String)
            Me.sTaxBandCode = value
        End Set
    End Property
    ''' <summary>
    ''' Tax BandKey as Integer
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TaxBandKey() As Integer
        Get
            Return nTaxBandKey
        End Get
        Set(ByVal value As Integer)
            Me.nTaxBandKey = value
        End Set
    End Property
    ''' <summary>
    ''' Tax Amount as Decimal
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TaxAmount() As Decimal
        Get
            Return dTaxAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dTaxAmount = value
        End Set
    End Property
    ''' <summary>
    ''' Skip Posting as Boolean
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SkipPosting() As Boolean
        Get
            Return Me.bSkipPosting
        End Get
        Set(ByVal value As Boolean)
            Me.bSkipPosting = value
        End Set
    End Property

    ''' <summary>
    ''' Business Identifier Codes(BIC) used in Party,Instalments,Claim,Cash/Cheque Payment and Receipt
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BIC() As String
        Get
            Return Me.sBIC
        End Get
        Set(ByVal value As String)
            Me.sBIC = value
        End Set
    End Property

    ''' <summary>
    ''' International Bank Account Number(IBAN) used in Party,Instalments,Claim,Cash/Cheque Payment and Receipt
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IBAN() As String
        Get
            Return Me.sIBAN
        End Get
        Set(ByVal value As String)
            Me.sIBAN = value
        End Set
    End Property

    Public Property Collection_Date() As Date
        Get
            Return Me.collectionDateField
        End Get
        Set(ByVal value As Date)
            Me.collectionDateField = value
        End Set
    End Property

    Public Property CurrencyBaseDate() As DateTime
        Get
            Return Me.dCurrencyBaseDate
        End Get
        Set(ByVal value As DateTime)
            Me.dCurrencyBaseDate = value
        End Set
    End Property

    Public Property CurrencyBaseXrate() As Decimal
        Get
            Return Me.dCurrencyBaseXrate
        End Get
        Set(ByVal value As Decimal)
            Me.dCurrencyBaseXrate = value
        End Set
    End Property

    Public Property AccountBaseDate() As DateTime
        Get
            Return Me.dAccountBaseDate
        End Get
        Set(ByVal value As DateTime)
            Me.dAccountBaseDate = value
        End Set
    End Property

    Public Property AccountBaseXrate() As Decimal
        Get
            Return Me.dAccountBaseXrate
        End Get
        Set(ByVal value As Decimal)
            Me.dAccountBaseXrate = value
        End Set
    End Property

    Public Property SystemBaseDate() As DateTime
        Get
            Return Me.dSystemBaseDate
        End Get
        Set(ByVal value As DateTime)
            Me.dSystemBaseDate = value
        End Set
    End Property

    Public Property SystemBaseXrate() As Decimal
        Get
            Return Me.dSystemBaseXrate
        End Get
        Set(ByVal value As Decimal)
            Me.dSystemBaseXrate = value
        End Set
    End Property

    Public Property OverrideReason() As Integer
        Get
            Return Me.nOverrideReason
        End Get
        Set(ByVal value As Integer)
            Me.nOverrideReason = value
        End Set
    End Property

    Public Property DocumentRef As String

    Public Property DocumentCode As String

    Public Property PaymentHubDetails() As PaymentHubDetails
        Get
            Return Me.oPaymentHubDetails
        End Get
        Set(ByVal value As PaymentHubDetails)
            Me.oPaymentHubDetails = value
        End Set
    End Property

    Public Property IsViaBulkClaimPayment As Boolean

    Public Property AutoAllocatePaymentSuccessful As Boolean

End Class
<Serializable()> Public Class PaymentItemsCollection : Inherits CollectionBase
    Public Function Add(ByVal v_oPaymentItems As PaymentItems) As Integer
        Return List.Add(v_oPaymentItems)
    End Function

    Public Sub Remove(ByVal v_oPaymentItems As PaymentItems)
        List.Remove(v_oPaymentItems)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As PaymentItems
        Get
            Return List(i)
        End Get
        Set(ByVal value As PaymentItems)
            List(i) = value
        End Set
    End Property
    Public Sub Update(ByVal v_oPaymentItems As PaymentItems)
        List.Item(v_oPaymentItems.Key) = v_oPaymentItems
    End Sub

    Public Sub Update(ByVal v_oPaymentItems As PaymentItems, ByVal index As Integer)
        List.Item(index) = v_oPaymentItems
    End Sub

End Class
