<Serializable()> Public Class Payment

    Private oPaymentMethod As PaymentTypes
    Private bPaymentMethodSpecified As Boolean
    Private dAmountPaid As Decimal
    Private dAmountToFinance As Decimal
    Private nSelectedSchemeNo As Integer
    Private nSelectedSchemeVersion As Integer
    Private dOverrideInterestRate As Decimal
    Private dOverrideRate As Decimal
    Private bPaymentProtection As Boolean
    Private dtQuoteDate As DateTime
    Private dtStartDate As DateTime
    Private dtEndDate As DateTime
    Private dtPreferredDate As DateTime
    Private nWeekDay As Integer
    Private nMonthDay As Integer
    Private sBankAccountName As String
    Private sBankAccountNo As String
    Private sBankSortCode As String
    Private oBankAddress As Address
    Private sBankName As String
    Private sBankBranch As String
    Private sBankAreaCode As String
    Private sBankExtn As String
    Private sBankFax As String
    Private sBankFaxCode As String
    Private sBankPhone As String
    Private nAccountHandlerCnt As Integer
    Private oCreditCard As CreditCardType
    Private oBankGuaranteeDetails As BankGuarantee
    Private oPayNowDetails As ReceiptType
    Private bPayTrueMonthlyPolicyMTAPremiumOnRenewal As Boolean
    Private nPFRF_ID As Integer
    Private dtCoverStartDate As DateTime
    Private oDebitAgainst As DebitAgainstType
    Private oCardHolder As CardHolder
    Private oCreditTransactionCollection As CreditTransactionCollection
    Private bAddToExistingFinancePlan As Boolean
    Private bInstallmentTypeSpecified As Boolean
    Private iPref_ID As Integer
    Private oSelectedCashDeposit As CashDeposit
    Private sDebitAgainstAccount As String
    Private sInstallmentType As String
    Private nPartyBankKey As Integer
    'MTA Cancellation
    Private oPayNowPaymentDetails As PaymentType
    Private dDaysDelayField As Integer
    Private sBIC As String
    Private sIBAN As String
    Private dInstDepositAmount As Double

    Public Sub New(ByVal v_oPaymentMethod As PaymentTypes, Optional ByVal v_dAmountPaid As Decimal = 0)

        oPaymentMethod = v_oPaymentMethod
        dAmountPaid = v_dAmountPaid
        oCreditTransactionCollection = New CreditTransactionCollection
        oBankGuaranteeDetails = New BankGuarantee
        oSelectedCashDeposit = New CashDeposit
    End Sub
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Bank Key : " & nPartyBankKey.ToString & "<br />")
        sbPrint.AppendLine("Bank Name : " & sBankName.ToString & "<br />")
        sbPrint.AppendLine("Bank SortCode : " & sBankSortCode.ToString & "<br />")
        sbPrint.AppendLine("Bank AccountNo : " & sBankAccountNo.ToString() & "<br />")
        sbPrint.AppendLine("Bank AccountName : " & sBankAccountName.ToString() & "<br />")
        sbPrint.AppendLine("Bank Branch : " & sBankBranch.ToString & "<br />")
        sbPrint.AppendLine("BankAddress AddressType : " & BankAddress.AddressType.ToString() & "<br />")
        sbPrint.AppendLine("BankAddress Address1 : " & BankAddress.Address1.ToString() & "<br />")
        sbPrint.AppendLine("BankAddress Address2 : " & BankAddress.Address2.ToString() & "<br />")
        sbPrint.AppendLine("BankAddress Address3 : " & BankAddress.Address3.ToString & "<br />")
        sbPrint.AppendLine("BankAddress Address4 : " & BankAddress.Address4.ToString & "<br />")
        sbPrint.AppendLine("BankAddress PostCode : " & BankAddress.PostCode.ToString & "<br />")
        sbPrint.AppendLine("BankAddress CountryCode : " & BankAddress.CountryCode.ToString & "<br />")

        sbPrint.AppendLine("Bank AreaCode : " & BankAreaCode.ToString & "<br />")
        sbPrint.AppendLine("Bank Phone : " & BankPhone.ToString & "<br />")
        sbPrint.AppendLine("Bank Extn : " & BankExtn.ToString & "<br />")
        sbPrint.AppendLine("Bank FaxCode : " & BankFaxCode.ToString & "<br />")
        sbPrint.AppendLine("Bank Fax : " & BankFax.ToString & "<br />")
        sbPrint.AppendLine("Pref_ID : " & Pref_ID.ToString & "<br />")

        Return sbPrint.ToString()

    End Function
    ''' <summary>
    ''' Installment Type Specified for selected Item
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InstallmentTypeSpecified() As Boolean
        Get
            Return bInstallmentTypeSpecified
        End Get
        Set(ByVal value As Boolean)
            bInstallmentTypeSpecified = value
        End Set
    End Property
    ''' <summary>
    ''' Installment Type for selected Item
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InstallmentType() As String
        Get
            Return sInstallmentType
        End Get
        Set(ByVal value As String)
            sInstallmentType = value
        End Set
    End Property

    Public Property DebitAgainstAccount() As String
        Get
            Return sDebitAgainstAccount
        End Get
        Set(ByVal value As String)
            sDebitAgainstAccount = value
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

    Public Property CreditTransaction() As CreditTransactionCollection
        Get
            Return oCreditTransactionCollection
        End Get
        Set(ByVal value As CreditTransactionCollection)
            oCreditTransactionCollection = value
        End Set
    End Property
    Public Property PaymentMethod() As PaymentTypes
        Get
            Return oPaymentMethod
        End Get
        Set(ByVal value As PaymentTypes)
            oPaymentMethod = value
        End Set
    End Property

    Public ReadOnly Property PaymentMethodSpecified() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Property AmountPaid() As Decimal
        Get
            Return dAmountPaid
        End Get
        Set(ByVal value As Decimal)
            dAmountPaid = value
        End Set
    End Property

    Public ReadOnly Property AmountPaidSpecified() As Boolean
        Get
            Return IIf(dAmountPaid > 0, True, False)
        End Get
    End Property

    Public Property AmountToFinance() As Decimal
        Get
            Return dAmountToFinance
        End Get
        Set(ByVal value As Decimal)
            dAmountToFinance = value
        End Set
    End Property

    Public Property SelectedSchemeNo() As Integer
        Get
            Return nSelectedSchemeNo
        End Get
        Set(ByVal value As Integer)
            nSelectedSchemeNo = value
        End Set
    End Property

    Public Property SelectedSchemeVersion() As Integer
        Get
            Return nSelectedSchemeVersion
        End Get
        Set(ByVal value As Integer)
            nSelectedSchemeVersion = value
        End Set
    End Property

    Public Property OverrideInterestRate() As Decimal
        Get
            Return dOverrideInterestRate
        End Get
        Set(ByVal value As Decimal)
            dOverrideInterestRate = value
        End Set
    End Property

    Public Property OverrideRate() As Decimal
        Get
            Return dOverrideRate
        End Get
        Set(ByVal value As Decimal)
            dOverrideRate = value
        End Set
    End Property

    Public Property PaymentProtection() As Boolean
        Get
            Return bPaymentProtection
        End Get
        Set(ByVal value As Boolean)
            bPaymentProtection = value
        End Set
    End Property

    Public Property QuoteDate() As DateTime
        Get
            Return dtQuoteDate
        End Get
        Set(ByVal value As DateTime)
            dtQuoteDate = value
        End Set
    End Property

    Public Property StartDate() As DateTime
        Get
            Return dtStartDate
        End Get
        Set(ByVal value As DateTime)
            dtStartDate = value
        End Set
    End Property

    Public Property EndDate() As DateTime
        Get
            Return dtEndDate
        End Get
        Set(ByVal value As DateTime)
            dtEndDate = value
        End Set
    End Property

    Public Property PreferredDate() As DateTime
        Get
            Return dtPreferredDate
        End Get
        Set(ByVal value As DateTime)
            dtPreferredDate = value
        End Set
    End Property

    Public Property WeekDay() As Integer
        Get
            Return nWeekDay
        End Get
        Set(ByVal value As Integer)
            nWeekDay = value
        End Set
    End Property

    Public Property MonthDay() As Integer
        Get
            Return nMonthDay
        End Get
        Set(ByVal value As Integer)
            nMonthDay = value
        End Set
    End Property

    Public Property BankAccountName() As String
        Get
            Return sBankAccountName
        End Get
        Set(ByVal value As String)
            sBankAccountName = value
        End Set
    End Property

    Public Property BankAccountNo() As String
        Get
            Return sBankAccountNo
        End Get
        Set(ByVal value As String)
            sBankAccountNo = value
        End Set
    End Property

    Public Property BankSortCode() As String
        Get
            Return sBankSortCode
        End Get
        Set(ByVal value As String)
            sBankSortCode = value
        End Set
    End Property

    Public Property BankAddress() As Address
        Get
            Return oBankAddress
        End Get
        Set(ByVal value As Address)
            oBankAddress = value
        End Set
    End Property

    Public Property BankName() As String
        Get
            Return sBankName
        End Get
        Set(ByVal value As String)
            sBankName = value
        End Set
    End Property

    Public Property BankBranch() As String
        Get
            Return sBankBranch
        End Get
        Set(ByVal value As String)
            sBankBranch = value
        End Set
    End Property

    Public Property BankAreaCode() As String
        Get
            Return sBankAreaCode
        End Get
        Set(ByVal value As String)
            sBankAreaCode = value
        End Set
    End Property

    Public Property BankExtn() As String
        Get
            Return sBankExtn
        End Get
        Set(ByVal value As String)
            sBankExtn = value
        End Set
    End Property

    Public Property BankFax() As String
        Get
            Return sBankFax
        End Get
        Set(ByVal value As String)
            sBankFax = value
        End Set
    End Property

    Public Property BankFaxCode() As String
        Get
            Return sBankFaxCode
        End Get
        Set(ByVal value As String)
            sBankFaxCode = value
        End Set
    End Property

    Public Property BankPhone() As String
        Get
            Return sBankPhone
        End Get
        Set(ByVal value As String)
            sBankPhone = value
        End Set
    End Property

    Public Property CreditCard() As CreditCardType
        Get
            Return oCreditCard
        End Get
        Set(ByVal value As CreditCardType)
            oCreditCard = value
        End Set
    End Property

    Public Property SelectedCashDeposit() As CashDeposit
        Get
            Return oSelectedCashDeposit
        End Get
        Set(ByVal value As CashDeposit)
            oSelectedCashDeposit = value
        End Set
    End Property

    Public Property BankGuaranteeDetails() As BankGuarantee
        Get
            Return oBankGuaranteeDetails
        End Get
        Set(ByVal value As BankGuarantee)
            oBankGuaranteeDetails = value
        End Set
    End Property

    Public Property PayNowPaymentDetails() As PaymentType
        Get
            Return oPayNowPaymentDetails
        End Get
        Set(ByVal value As PaymentType)
            oPayNowPaymentDetails = value
        End Set
    End Property

    Public Property PayNowDetails() As ReceiptType
        Get
            Return oPayNowDetails
        End Get
        Set(ByVal value As ReceiptType)
            oPayNowDetails = value
        End Set
    End Property

    Public Property PayTrueMonthlyPolicyMTAPremiumOnRenewal() As Boolean
        Get
            Return bPayTrueMonthlyPolicyMTAPremiumOnRenewal
        End Get
        Set(ByVal value As Boolean)
            bPayTrueMonthlyPolicyMTAPremiumOnRenewal = value
        End Set
    End Property

    Public Property CoverStartDate() As DateTime
        Get
            Return dtCoverStartDate
        End Get
        Set(ByVal value As DateTime)
            dtCoverStartDate = value
        End Set
    End Property

    Public Property DebitAgainst() As DebitAgainstType
        Get
            Return oDebitAgainst
        End Get
        Set(ByVal value As DebitAgainstType)
            oDebitAgainst = value
        End Set
    End Property

    Public Property PFRF_ID() As Integer
        Get
            Return nPFRF_ID
        End Get
        Set(ByVal value As Integer)
            nPFRF_ID = value
        End Set
    End Property

    Public Property AccountHandlerCnt() As Integer
        Get
            Return nAccountHandlerCnt
        End Get
        Set(ByVal value As Integer)
            nAccountHandlerCnt = value
        End Set
    End Property
    Public Property InstDepositAmount As Double
        Get
            Return dInstDepositAmount
        End Get
        Set(ByVal value As Double)
            dInstDepositAmount = value
        End Set
    End Property

#Region "Sachin"

    Public Property AddToExistingFinancePlan() As Boolean
        Get
            Return bAddToExistingFinancePlan
        End Get
        Set(ByVal value As Boolean)
            bAddToExistingFinancePlan = value
        End Set
    End Property

#End Region
#Region "Sanjay"
    Public Property Pref_ID() As Integer
        Get
            Return iPref_ID
        End Get
        Set(ByVal value As Integer)
            iPref_ID = value
        End Set
    End Property
#End Region

    ''' <summary>
    ''' Party Bank Key for selected payment
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PartyBankKey() As Integer
        Get
            Return nPartyBankKey
        End Get
        Set(ByVal value As Integer)
            nPartyBankKey = value
        End Set
    End Property
    Public Property DaysDelay() As Integer
        Get
            Return Me.dDaysDelayField
        End Get
        Set(ByVal value As Integer)
            Me.dDaysDelayField = value
        End Set
    End Property

    ''' <summary>
    ''' wpr10
    ''' </summary>
    ''' <remarks></remarks>
    Private bIsUseTransactionCurrency As Boolean
    Public Property IsUseTransactionCurrency() As Boolean
        Get
            Return bIsUseTransactionCurrency
        End Get
        Set(ByVal value As Boolean)
            bIsUseTransactionCurrency = value
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

    Public Property OverrideCommission() As Boolean

    Public Property OverrideDepositAmount() As Decimal
      
End Class

''' <summary>
''' Payment Types
''' </summary>
Public Enum PaymentTypes

    ''' <summary>
    ''' None
    ''' </summary>
    None = 0

    ''' <summary>
    ''' Bankers Draft
    ''' </summary>
    BankersDraft

    ''' <summary>
    ''' Cash
    ''' </summary>
    Cash

    ''' <summary>
    ''' Cheque
    ''' </summary>
    Cheque

    ''' <summary>
    ''' Credit Card
    ''' </summary>
    CreditCard

    ''' <summary>
    ''' Debit Card
    ''' </summary>
    DebitCard

    ''' <summary>
    ''' Agent Collection
    ''' </summary>
    AgentCollection

    ''' <summary>
    ''' Agent Overdraft
    ''' </summary>
    AgentOverdraft

    ''' <summary>
    ''' Agent Float Balance
    ''' </summary>
    AgentFloatBalanace

    '''<remarks/>
    BankGuarantee

    ''' <summary>
    ''' will be used for the payment type CASH DEPOSIT (WPR 85)
    ''' </summary>
    ''' <remarks></remarks>
    CashDeposit

    ''' <summary>
    ''' will be used for the payment via Payment Hub
    ''' </summary>
    ''' <remarks></remarks>
    PaymentHub



    ''' <summary>
    ''' Added to make the enum generic
    ''' </summary>
    ''' <remarks></remarks>
    AllOthers

End Enum
''' <summary>
''' DebitAgainstType
''' </summary>
Public Enum DebitAgainstType

    '''<remarks/>
    DebitAgainstCashListItem

    '''<remarks/>
    DebitAgainstOverDraft

    '''<remarks/>
    DebitAgainstFloatBalance

    '''<remarks/>
    DebitAgainstUnallocatedCredit

    ''' <summary>
    ''' DebitAgainstCashDeposit will be used for the payment type CASH DEPOSIT (WPR 85)
    ''' </summary>
    ''' <remarks></remarks>
    DebitAgainstCashDeposit
End Enum

