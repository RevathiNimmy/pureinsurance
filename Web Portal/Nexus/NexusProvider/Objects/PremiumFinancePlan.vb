''' <summary>
''' To process the request of Premium finance plan
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class PremiumFinancePlan
    Private sPartyCodeField As String
    Private oPFTransactionField() As FinancePlanTransactions
    Private oTransTypeField As ProcessPFPlanType
    Private oTypeField As InstalmentType
    Private bSaveOnlyField As Boolean
    Private nPFPremFinanceKeyField As Integer
    Private nPFPremFinanceVersionField As Integer
    Private nPFCreditCardDetailsField As CreditCard
    Private oPFBankDetailsField As PFBankDetails
    Private oFinanceDetailsField As FinancePlanDetails
    Private oInstalmentField As InstalmentsCollection
    Private oTransactionsField As FinancePlanTransactionsCollection
    Private oPFHistoryField As FinancePlanHistoryCollection
    Private oPFBankHistoryField As FinancePlanBankHistoryCollection

    '''<remarks/>
    Public Property PFBankHistory() As FinancePlanBankHistoryCollection
        Get
            Return Me.oPFBankHistoryField
        End Get
        Set(ByVal value As FinancePlanBankHistoryCollection)
            Me.oPFBankHistoryField = value
        End Set
    End Property


    Public Property PartyCode() As String
        Get
            Return Me.sPartyCodeField
        End Get
        Set(ByVal value As String)
            Me.sPartyCodeField = value
        End Set
    End Property


    Public Property PFTransaction() As FinancePlanTransactions()
        Get
            Return Me.oPFTransactionField
        End Get
        Set(ByVal value As FinancePlanTransactions())
            Me.oPFTransactionField = value
        End Set
    End Property

    Public Property TransType() As ProcessPFPlanType
        Get
            Return Me.oTransTypeField
        End Get
        Set(ByVal value As ProcessPFPlanType)
            Me.oTransTypeField = value
        End Set
    End Property

    Public Property Type() As InstalmentType
        Get
            Return Me.oTypeField
        End Get
        Set(ByVal value As InstalmentType)
            Me.oTypeField = value
        End Set
    End Property

    Public Property SaveOnly() As Boolean
        Get
            Return Me.bSaveOnlyField
        End Get
        Set(ByVal value As Boolean)
            Me.bSaveOnlyField = value
        End Set
    End Property

    Public Property PFPremFinanceKey() As Integer
        Get
            Return Me.nPFPremFinanceKeyField
        End Get
        Set(ByVal value As Integer)
            Me.nPFPremFinanceKeyField = value
        End Set
    End Property

    Public Property PFPremFinanceVersion() As Integer
        Get
            Return Me.nPFPremFinanceVersionField
        End Get
        Set(ByVal value As Integer)
            Me.nPFPremFinanceVersionField = value
        End Set
    End Property

    Public Property PFCreditCardDetails() As CreditCard
        Get
            Return Me.nPFCreditCardDetailsField
        End Get
        Set(ByVal value As CreditCard)
            Me.nPFCreditCardDetailsField = value
        End Set
    End Property

    Public Property PFBankDetails() As PFBankDetails
        Get
            Return Me.oPFBankDetailsField
        End Get
        Set(ByVal value As PFBankDetails)
            Me.oPFBankDetailsField = value
        End Set
    End Property

    '''<remarks/>
    Public Property PremiumFinanceDetails() As FinancePlanDetails
        Get
            Return Me.oFinanceDetailsField
        End Get
        Set(ByVal value As FinancePlanDetails)
            Me.oFinanceDetailsField = value
        End Set
    End Property
    '''<remarks/>
    Public Property Instalments() As InstalmentsCollection
        Get
            Return Me.oInstalmentField
        End Get
        Set(ByVal value As InstalmentsCollection)
            Me.oInstalmentField = value
        End Set
    End Property
    '''<remarks/>
    Public Property Transactions() As FinancePlanTransactionsCollection
        Get
            Return Me.oTransactionsField
        End Get
        Set(ByVal value As FinancePlanTransactionsCollection)
            Me.oTransactionsField = value
        End Set
    End Property

    '''<remarks/>
    Public Property PFHistory() As FinancePlanHistoryCollection
        Get
            Return Me.oPFHistoryField
        End Get
        Set(ByVal value As FinancePlanHistoryCollection)
            Me.oPFHistoryField = value
        End Set
    End Property

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Overridable Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Base Class : PremiumFinancePlan <br />")
        
        Return sbPrint.ToString

    End Function
End Class
