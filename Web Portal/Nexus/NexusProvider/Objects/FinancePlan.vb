<Serializable()> Public Class FinancePlan
    Private dFinanceAmount As Double
    Private dTotalInstalmentAmount As Double
    Private dAPRRate As Double
    Private dInterestRate As Double
    Private sMediaType As String
    Private nNoOfInstalments As Integer
    Private dtFirstInstalmentDate As Date
    Private dtNextInstalmentDate As Date
    Private dtLastInstalmentDate As Date
    Private dFirstInstalmentAmount As Double
    Private dOtherInstalmentAmount As Double
    Private dTaxAmount As Double
    Private dDeposit As Double
    Private dAdminCharge As Double
    Private dProtectionCharge As Double
    Private dInterestAmount As Double
    Private sFrequency As String
    Private sPaymentMethod As String
    Private sSchemeName As String
    Private cctCreditCardDetails As CreditCardType
    Private bdBankDetails As Bank
    Private idInstalmentDetails As InstalmentsCollection
    Private sStatusDescription As String
    Private nFinancePlanKey As Integer
    Private nFinancePlanVersion As Integer
    Private sFinanceProvider As String
    Private sInsuranceRef As String
    Private sPlanReference As String
    Private sAccountNumber As String
    Private dCRAmount As Double
    Private dtNextDueDate As Date
    Private sStatus As String
    Private nRemainingInstalments As Integer
    Private nPartyBankKey As Integer
    Private nDayOfWeekOrMonth As Integer

    Public Property FinanceAmount() As Double
        Get
            Return Me.dFinanceAmount
        End Get
        Set(ByVal value As Double)
            Me.dFinanceAmount = value
        End Set
    End Property
    Public Property TotalInstalmentAmount() As Double
        Get
            Return Me.dTotalInstalmentAmount
        End Get
        Set(ByVal value As Double)
            Me.dTotalInstalmentAmount = value
        End Set
    End Property
    Public Property APRRate() As Double
        Get
            Return Me.dAPRRate
        End Get
        Set(ByVal value As Double)
            Me.dAPRRate = value
        End Set
    End Property
    Public Property InterestRate() As Double
        Get
            Return Me.dInterestRate
        End Get
        Set(ByVal value As Double)
            Me.dInterestRate = value
        End Set
    End Property
    Public Property MediaType() As String
        Get
            Return Me.sMediaType
        End Get
        Set(ByVal value As String)
            Me.sMediaType = value
        End Set
    End Property
    Public Property NoOfInstalments() As Integer
        Get
            Return Me.nNoOfInstalments
        End Get
        Set(ByVal value As Integer)
            Me.nNoOfInstalments = value
        End Set
    End Property
    Public Property FirstInstalmentDate() As Date
        Get
            Return Me.dtFirstInstalmentDate
        End Get
        Set(ByVal value As Date)
            Me.dtFirstInstalmentDate = value
        End Set
    End Property
    Public Property NextInstalmentDate() As Date
        Get
            Return Me.dtNextInstalmentDate
        End Get
        Set(ByVal value As Date)
            Me.dtNextInstalmentDate = value
        End Set
    End Property
    Public Property LastInstalmentDate() As Date
        Get
            Return Me.dtLastInstalmentDate
        End Get
        Set(ByVal value As Date)
            Me.dtLastInstalmentDate = value
        End Set
    End Property
    Public Property FirstInstalmentAmount() As Double
        Get
            Return Me.dFirstInstalmentAmount
        End Get
        Set(ByVal value As Double)
            Me.dFirstInstalmentAmount = value
        End Set
    End Property
    Public Property OtherInstalmentAmount() As Double
        Get
            Return Me.dOtherInstalmentAmount
        End Get
        Set(ByVal value As Double)
            Me.dOtherInstalmentAmount = value
        End Set
    End Property
    Public Property TaxAmount() As Double
        Get
            Return Me.dTaxAmount
        End Get
        Set(ByVal value As Double)
            Me.dTaxAmount = value
        End Set
    End Property
    Public Property Deposit() As Double
        Get
            Return Me.dDeposit
        End Get
        Set(ByVal value As Double)
            Me.dDeposit = value
        End Set
    End Property
    Public Property AdminCharge() As Double
        Get
            Return Me.dAdminCharge
        End Get
        Set(ByVal value As Double)
            Me.dAdminCharge = value
        End Set
    End Property
    Public Property ProtectionCharge() As Double
        Get
            Return Me.dProtectionCharge
        End Get
        Set(ByVal value As Double)
            Me.dProtectionCharge = value
        End Set
    End Property
    Public Property InterestAmount() As Double
        Get
            Return Me.dInterestAmount
        End Get
        Set(ByVal value As Double)
            Me.dInterestAmount = value
        End Set
    End Property
    Public Property Frequency() As String
        Get
            Return Me.sFrequency
        End Get
        Set(ByVal value As String)
            Me.sFrequency = value
        End Set
    End Property
    Public Property PaymentMethod() As String
        Get
            Return Me.sPaymentMethod
        End Get
        Set(ByVal value As String)
            Me.sPaymentMethod = value
        End Set
    End Property
    Public Property SchemeName() As String
        Get
            Return Me.sSchemeName
        End Get
        Set(ByVal value As String)
            Me.sSchemeName = value
        End Set
    End Property
    Public Property CreditCardDetails() As CreditCardType
        Get
            Return Me.cctCreditCardDetails
        End Get
        Set(ByVal value As CreditCardType)
            Me.cctCreditCardDetails = value
        End Set
    End Property
    Public Property BankDetails() As Bank
        Get
            Return Me.bdBankDetails
        End Get
        Set(ByVal value As Bank)
            Me.bdBankDetails = value
        End Set
    End Property
    Public Property InstalmentDetails() As InstalmentsCollection
        Get
            Return Me.idInstalmentDetails
        End Get
        Set(ByVal value As InstalmentsCollection)
            Me.idInstalmentDetails = value
        End Set
    End Property

    Public Property StatusDescription() As String
        Get
            Return Me.sStatusDescription
        End Get
        Set(ByVal value As String)
            Me.sStatusDescription = value
        End Set
    End Property


    '''<remarks/>
    Public Property FinancePlanKey() As Integer
        Get
            Return Me.nFinancePlanKey
        End Get
        Set(ByVal value As Integer)
            Me.nFinancePlanKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property FinancePlanVersion() As Integer
        Get
            Return Me.nFinancePlanVersion
        End Get
        Set(ByVal value As Integer)
            Me.nFinancePlanVersion = value
        End Set
    End Property

    '''<remarks/>
    Public Property FinanceProvider() As String
        Get
            Return Me.sFinanceProvider
        End Get
        Set(ByVal value As String)
            Me.sFinanceProvider = value
        End Set
    End Property

    '''<remarks/>
    Public Property InsuranceRef() As String
        Get
            Return Me.sInsuranceRef
        End Get
        Set(ByVal value As String)
            Me.sInsuranceRef = value
        End Set
    End Property

    '''<remarks/>
    Public Property PlanReference() As String
        Get
            Return Me.sPlanReference
        End Get
        Set(ByVal value As String)
            Me.sPlanReference = value
        End Set
    End Property

    '''<remarks/>
    Public Property AccountNumber() As String
        Get
            Return Me.sAccountNumber
        End Get
        Set(ByVal value As String)
            Me.sAccountNumber = value
        End Set
    End Property

    '''<remarks/>
    Public Property Amount() As Double
        Get
            Return Me.dCRAmount
        End Get
        Set(ByVal value As Double)
            Me.dCRAmount = value
        End Set
    End Property

    '''<remarks/>
    Public Property NextDueDate() As Date
        Get
            Return Me.dtNextDueDate
        End Get
        Set(ByVal value As Date)
            Me.dtNextDueDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property Status() As String
        Get
            Return Me.sStatus
        End Get
        Set(ByVal value As String)
            Me.sStatus = value
        End Set
    End Property


    '''<remarks/>
    Public Property RemainingInstalments() As Integer
        Get
            Return Me.nRemainingInstalments
        End Get
        Set(ByVal value As Integer)
            Me.nRemainingInstalments = value
        End Set
    End Property

    Public Property PartyBankKey() As Integer
        Get
            Return Me.nPartyBankKey
        End Get
        Set(ByVal value As Integer)
            Me.nPartyBankKey = value
        End Set
    End Property

    Public Property DayOfWeekOrMonth() As Integer
        Get
            Return Me.nDayOfWeekOrMonth
        End Get
        Set(ByVal value As Integer)
            Me.nDayOfWeekOrMonth = value
        End Set
    End Property
End Class
'Make changes in this class to Convert it to sortable collection
<Serializable()> Public Class FinancePlanCollection : Inherits SortableCollectionBase

    Public Sub New()
        MyBase.SortObjectType = GetType(FinancePlan)
    End Sub

    ''' <summary>
    ''' Add Finance Plan List
    ''' </summary>
    ''' <param name="v_oFinancePlan"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Add(ByVal v_oFinancePlan As FinancePlan) As Integer
        Return List.Add(v_oFinancePlan)
    End Function

    ''' <summary>
    ''' Remove Finance Plan List
    ''' </summary>
    ''' <param name="v_oInstalmentQuote"></param>
    ''' <remarks></remarks>
    Public Sub Remove(ByVal v_oInstalmentQuote As InstalmentQuote)
        List.Remove(v_oInstalmentQuote)
    End Sub

    ''' <summary>
    ''' Remove Finance Plan List at a particular Index
    ''' </summary>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Public Shadows Sub RemoveAt(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Add Item Finance Plan Item List
    ''' </summary>
    ''' <param name="i"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Default Public Property Item(ByVal i As Integer) As FinancePlan
        Get
            Return List(i)
        End Get
        Set(ByVal value As FinancePlan)
            List(i) = value
        End Set
    End Property

End Class
