<Serializable()> Public Class ReceiptType
#Region "PrivateFields"
    Private dListDate As DateTime

    Private sType As String

    Private bankAccountNameField As String

    Private currencyCodeField As String

    Private receiptTypeCodeField As String

    Private mediaTypeCodeField As String

    Private transactionDateField As Date

    Private amountField As Double

    Private cashListRefField As String

    Private mediaTypeIssuerCodeField As String

    Private mediaReferenceField As String

    Private ourReferenceField As String

    Private theirReferenceField As String

    Private contactNameField As String

    Private address1Field As String

    Private address2Field As String

    Private address3Field As String

    Private address4Field As String

    Private postalCodeField As String

    Private countryCodeField As String

    Private chequeNameField As String

    Private chequeDateField As Date

    Private chequeDateFieldSpecified As Boolean

    Private cCNameField As String

    Private cCNumberField As String

    Private cCExpiryDateField As String

    Private cCStartDateField As String

    Private cCIssueField As String

    Private cCPinField As String

    Private cCAuthCodeField As String

    Private cCManualAuthCodeField As String

    Private cCTransactionCodeField As String

    Private cCCustomerField As String

    Private collectionDateField As Date

    Private collectionDateFieldSpecified As Boolean

    Private commentsField As String
    Private sBankReference, sInstrumentNumber, sDraweeBankName, sDraweeBankLocation, sChequeType, sChequeClearingType, sDraweeBankBranch As String
    Private sCCTypeOfCard, sCCIssueBank, sCCSlipNumber As String
    Private iPartyBankkey As Integer
    Private sCCTrackingNumber As String
    Private sSubbranchCode As String
    Private dCurrencyBaseDate As Nullable(Of DateTime)
    Private dCurrencyBaseXrate As Decimal
    Private dAccountBaseDate As DateTime
    Private dAccountBaseXrate As Decimal
    Private dSystemBaseDate As DateTime
    Private dSystemBaseXrate As Decimal
    Private nOverrideReason As Integer

#End Region


    Public Sub New()

    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    '''
    Public Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("bankAccountNameField : " & bankAccountNameField & "<br />")
        sbPrint.AppendLine("currencyCodeField : " & currencyCodeField & "<br />")
        sbPrint.AppendLine("receiptTypeCodeField : " & receiptTypeCodeField & "<br />")
        sbPrint.AppendLine("mediaTypeCodeField : " & mediaTypeCodeField & "<br />")


        Return sbPrint.ToString

    End Function
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
    Public Property CCTrackingNumber() As String
        Get
            Return Me.sCCTrackingNumber
        End Get
        Set(ByVal value As String)
            Me.sCCTrackingNumber = value
        End Set
    End Property
    '''<remarks/>
    Public Property PartyBankKey() As Integer
        Get
            Return Me.iPartyBankkey
        End Get
        Set(ByVal value As Integer)
            Me.iPartyBankkey = value
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

    '''<remarks/>
    Public Property DraweeBankBranch() As String
        Get
            Return Me.sDraweeBankBranch
        End Get
        Set(ByVal value As String)
            Me.sDraweeBankBranch = value
        End Set
    End Property
    '''<remarks/>
    Public Property ChequeClearingType() As String
        Get
            Return Me.sChequeClearingType
        End Get
        Set(ByVal value As String)
            Me.sChequeClearingType = value
        End Set
    End Property
    '''<remarks/>
    Public Property ChequeType() As String
        Get
            Return Me.sChequeType
        End Get
        Set(ByVal value As String)
            Me.sChequeType = value
        End Set
    End Property
    '''<remarks/>
    Public Property DraweeBankLocation() As String
        Get
            Return Me.sDraweeBankLocation
        End Get
        Set(ByVal value As String)
            Me.sDraweeBankLocation = value
        End Set
    End Property
    '''<remarks/>
    Public Property DraweeBankName() As String
        Get
            Return Me.sDraweeBankName
        End Get
        Set(ByVal value As String)
            Me.sDraweeBankName = value
        End Set
    End Property
    '''<remarks/>
    Public Property InstrumentNumber() As String
        Get
            Return Me.sInstrumentNumber
        End Get
        Set(ByVal value As String)
            Me.sInstrumentNumber = value
        End Set
    End Property
    '''<remarks/>
    Public Property BankReference() As String
        Get
            Return Me.sBankReference
        End Get
        Set(ByVal value As String)
            Me.sBankReference = value
        End Set
    End Property

    '''<remarks/>
    Public Property BankAccountName() As String
        Get
            Return Me.bankAccountNameField
        End Get
        Set(ByVal value As String)
            Me.bankAccountNameField = value
        End Set
    End Property

    Public Property Type() As String
        Get
            Return Me.sType
        End Get
        Set(ByVal value As String)
            Me.sType = value
        End Set
    End Property

    Public Property ListDate() As DateTime
        Get
            Return Me.dListDate
        End Get
        Set(ByVal value As DateTime)
            Me.dListDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property CurrencyCode() As String
        Get
            Return Me.currencyCodeField
        End Get
        Set(ByVal value As String)
            Me.currencyCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ReceiptTypeCode() As String
        Get
            Return Me.receiptTypeCodeField
        End Get
        Set(ByVal value As String)
            Me.receiptTypeCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property MediaTypeCode() As String
        Get
            Return Me.mediaTypeCodeField
        End Get
        Set(ByVal value As String)
            Me.mediaTypeCodeField = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
    Public Property TransactionDate() As Date
        Get
            Return Me.transactionDateField
        End Get
        Set(ByVal value As Date)
            Me.transactionDateField = value
        End Set
    End Property

    '''<remarks/>
    Public Property Amount() As Double
        Get
            Return Me.amountField
        End Get
        Set(ByVal value As Double)
            Me.amountField = value
        End Set
    End Property

    '''<remarks/>
    Public Property CashListRef() As String
        Get
            Return Me.cashListRefField
        End Get
        Set(ByVal value As String)
            Me.cashListRefField = value
        End Set
    End Property

    '''<remarks/>
    Public Property MediaTypeIssuerCode() As String
        Get
            Return Me.mediaTypeIssuerCodeField
        End Get
        Set(ByVal value As String)
            Me.mediaTypeIssuerCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property MediaReference() As String
        Get
            Return Me.mediaReferenceField
        End Get
        Set(ByVal value As String)
            Me.mediaReferenceField = value
        End Set
    End Property

    '''<remarks/>
    Public Property OurReference() As String
        Get
            Return Me.ourReferenceField
        End Get
        Set(ByVal value As String)
            Me.ourReferenceField = value
        End Set
    End Property

    '''<remarks/>
    Public Property TheirReference() As String
        Get
            Return Me.theirReferenceField
        End Get
        Set(ByVal value As String)
            Me.theirReferenceField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ContactName() As String
        Get
            Return Me.contactNameField
        End Get
        Set(ByVal value As String)
            Me.contactNameField = value
        End Set
    End Property

    '''<remarks/>
    Public Property Address1() As String
        Get
            Return Me.address1Field
        End Get
        Set(ByVal value As String)
            Me.address1Field = value
        End Set
    End Property

    '''<remarks/>
    Public Property Address2() As String
        Get
            Return Me.address2Field
        End Get
        Set(ByVal value As String)
            Me.address2Field = value
        End Set
    End Property

    '''<remarks/>
    Public Property Address3() As String
        Get
            Return Me.address3Field
        End Get
        Set(ByVal value As String)
            Me.address3Field = value
        End Set
    End Property

    '''<remarks/>
    Public Property Address4() As String
        Get
            Return Me.address4Field
        End Get
        Set(ByVal value As String)
            Me.address4Field = value
        End Set
    End Property

    '''<remarks/>
    Public Property PostalCode() As String
        Get
            Return Me.postalCodeField
        End Get
        Set(ByVal value As String)
            Me.postalCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property CountryCode() As String
        Get
            Return Me.countryCodeField
        End Get
        Set(ByVal value As String)
            Me.countryCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ChequeName() As String
        Get
            Return Me.chequeNameField
        End Get
        Set(ByVal value As String)
            Me.chequeNameField = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
    Public Property ChequeDate() As Date
        Get
            Return Me.chequeDateField
        End Get
        Set(ByVal value As Date)
            Me.chequeDateField = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property ChequeDateSpecified() As Boolean
        Get
            Return Me.chequeDateFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.chequeDateFieldSpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property CCName() As String
        Get
            Return Me.cCNameField
        End Get
        Set(ByVal value As String)
            Me.cCNameField = value
        End Set
    End Property

    '''<remarks/>
    Public Property CCNumber() As String
        Get
            Return Me.cCNumberField
        End Get
        Set(ByVal value As String)
            Me.cCNumberField = value
        End Set
    End Property

    '''<remarks/>
    Public Property CCExpiryDate() As String
        Get
            Return Me.cCExpiryDateField
        End Get
        Set(ByVal value As String)
            Me.cCExpiryDateField = value
        End Set
    End Property

    '''<remarks/>
    Public Property CCStartDate() As String
        Get
            Return Me.cCStartDateField
        End Get
        Set(ByVal value As String)
            Me.cCStartDateField = value
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
    Public Property CCAuthCode() As String
        Get
            Return Me.cCAuthCodeField
        End Get
        Set(ByVal value As String)
            Me.cCAuthCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property CCManualAuthCode() As String
        Get
            Return Me.cCManualAuthCodeField
        End Get
        Set(ByVal value As String)
            Me.cCManualAuthCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property CCTransactionCode() As String
        Get
            Return Me.cCTransactionCodeField
        End Get
        Set(ByVal value As String)
            Me.cCTransactionCodeField = value
        End Set
    End Property

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
    Public Property CollectionDate() As Date
        Get
            Return Me.collectionDateField
        End Get
        Set(ByVal value As Date)
            Me.collectionDateField = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property CollectionDateSpecified() As Boolean
        Get
            Return Me.collectionDateFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.collectionDateFieldSpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property Comments() As String
        Get
            Return Me.commentsField
        End Get
        Set(ByVal value As String)
            Me.commentsField = value
        End Set
    End Property

    Public Property SubbranchCode() As String
        Get
            Return Me.sSubbranchCode
        End Get
        Set(ByVal value As String)
            Me.sSubbranchCode = value
        End Set
    End Property

    Public Property CurrencyBaseDate() As Nullable(Of DateTime)
        Get
            Return Me.dCurrencyBaseDate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
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

End Class
