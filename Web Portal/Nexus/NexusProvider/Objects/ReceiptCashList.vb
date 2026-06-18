<Serializable()> Public Class ReceiptCashList : Inherits PaymentCashListItemType
    Private iInsuranceFileKey As System.Collections.Generic.List(Of Integer)
    Private sAllocationStatus As System.Collections.Generic.List(Of String)
    Private oReceiptCashList As ReceiptCashListType
    Private oCreditCard As CreditCardType
    Private oBank As BankReceiptType
    Private oAllocationType As AllocationType
    Private oPolicies As ReceiptCashListItemTypePoliciesCollection

    Public Sub New()

        oReceiptCashList = New ReceiptCashListType
        oCreditCard = New CreditCardType
        oBank = New BankReceiptType
        oAllocationType = New AllocationType
        oPolicies = New ReceiptCashListItemTypePoliciesCollection

    End Sub

    '''<remarks/>
    Public Property ReceiptCashList() As ReceiptCashListType
        Get
            Return Me.oReceiptCashList
        End Get
        Set(ByVal value As ReceiptCashListType)
            Me.oReceiptCashList = value
        End Set
    End Property

    Public Overloads Property CreditCard() As CreditCardType
        Get
            Return Me.oCreditCard
        End Get
        Set(ByVal value As CreditCardType)
            Me.oCreditCard = value
        End Set
    End Property

    '''<remarks/>
    Public Overloads Property Bank() As BankReceiptType
        Get
            Return Me.oBank
        End Get
        Set(ByVal value As BankReceiptType)
            Me.oBank = value
        End Set
    End Property

    '''<remarks/>
    Public Shadows Property AllocationType() As AllocationType
        Get
            Return Me.oAllocationType
        End Get
        Set(ByVal value As AllocationType)
            Me.oAllocationType = value
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
    '''<remarks/>

    Public Property InsuranceFileKey() As System.Collections.Generic.List(Of Integer)
        Get
            Return Me.iInsuranceFileKey
        End Get
        Set(ByVal value As System.Collections.Generic.List(Of Integer))
            Me.iInsuranceFileKey = value
        End Set
    End Property
    Public Property AllocationStatus() As System.Collections.Generic.List(Of String)
        Get
            Return Me.sAllocationStatus
        End Get
        Set(ByVal value As System.Collections.Generic.List(Of String))
            Me.sAllocationStatus = value
        End Set
    End Property


    Public Overrides Function Print() As String

        Dim sbPrint As New Text.StringBuilder


        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function

End Class
<Serializable()> Public Class ReceiptCashListCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oReceiptCashList As ReceiptCashList In List
            sbPrint.AppendLine(oReceiptCashList.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oReceiptCashList As ReceiptCashList) As Integer
        Return List.Add(v_oReceiptCashList)
    End Function

    Public Sub Remove(ByVal v_oReceiptCashList As ReceiptCashList)
        List.Remove(v_oReceiptCashList)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As ReceiptCashList
        Get
            Return List(i)
        End Get
        Set(ByVal value As ReceiptCashList)
            List(i) = value
        End Set
    End Property

End Class

<Serializable()> Public Class ReceiptCashListType : Inherits CoreCashListType

    Private oReceiptItem As ReceiptCashListItemType

    Public Sub New()

        oReceiptItem = New ReceiptCashListItemType


    End Sub


    Public Property ReceiptItem() As ReceiptCashListItemType
        Get
            Return Me.oReceiptItem
        End Get
        Set(ByVal value As ReceiptCashListItemType)
            Me.oReceiptItem = value
        End Set
    End Property
End Class

<Serializable()> Public Class ReceiptCashListItemType : Inherits CoreCashListItemType

    Private sTypeCode As String

    Private sStatusCode As String

    Private oCreditCard As CreditCardType

    Private oBank As BankReceiptType

    Private oAllocationType As AllocationType

    Private bAllocationTypeSpecified As Boolean

    Private oPolicies As ReceiptCashListItemTypePoliciesCollection
    Private oPaymentItems As PaymentItemsCollection
    Private oCoreCashList As CoreCashListType

    Private oInstalmentPlanDetails As InstalmentPlanDetailsCollection
    Private bAutoAllocateIfAble As Boolean
    Private bAutoAllocatePaymentSuccessful As Boolean

    Public Sub New()
        oCreditCard = New CreditCardType
        oBank = New BankReceiptType
        oAllocationType = New AllocationType
        oPolicies = New ReceiptCashListItemTypePoliciesCollection
        oPaymentItems = New PaymentItemsCollection
        oCoreCashList = New CoreCashListType
        InstalmentPlanCollection = New InstalmentPlanDetailsCollection
    End Sub

    '''<remarks/>
    Public Property TypeCode() As String
        Get
            Return Me.sTypeCode
        End Get
        Set(ByVal value As String)
            Me.sTypeCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property StatusCode() As String
        Get
            Return Me.sStatusCode
        End Get
        Set(ByVal value As String)
            Me.sStatusCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property CreditCard() As CreditCardType
        Get
            Return Me.oCreditCard
        End Get
        Set(ByVal value As CreditCardType)
            Me.oCreditCard = value
        End Set
    End Property

    '''<remarks/>
    Public Property Bank() As BankReceiptType
        Get
            Return Me.oBank
        End Get
        Set(ByVal value As BankReceiptType)
            Me.oBank = value
        End Set
    End Property

    '''<remarks/>
    Public Property AllocationType() As AllocationType
        Get
            Return Me.oAllocationType
        End Get
        Set(ByVal value As AllocationType)
            Me.oAllocationType = value
        End Set
    End Property
    Public Property AllocationTypeSpecified() As Boolean
        Get
            Return Me.bAllocationTypeSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bAllocationTypeSpecified = value
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
    Public Property ReceiptItems() As PaymentItemsCollection
        Get
            Return oPaymentItems
        End Get
        Set(ByVal value As PaymentItemsCollection)
            oPaymentItems = value
        End Set
    End Property
    Public Property CoreCashList() As CoreCashListType
        Get
            Return oCoreCashList
        End Get
        Set(ByVal value As CoreCashListType)
            oCoreCashList = value
        End Set
    End Property

    Public Property InstalmentPlanCollection() As InstalmentPlanDetailsCollection
        Get
            Return oInstalmentPlanDetails
        End Get
        Set(ByVal value As InstalmentPlanDetailsCollection)
            oInstalmentPlanDetails = value
        End Set
    End Property

    Public Property AutoAllocateIfAble() As Boolean
        Get
            Return bAutoAllocateIfAble
        End Get
        Set(ByVal value As Boolean)
            bAutoAllocateIfAble = value
        End Set
    End Property

    Public Property AutoAllocatePaymentSuccessful() As Boolean
        Get
            Return bAutoAllocatePaymentSuccessful
        End Get
        Set(ByVal value As Boolean)
            bAutoAllocatePaymentSuccessful = value
        End Set
    End Property

End Class


<Serializable()> Public Class CoreCashListItemType

    Private bIsProduceDocument As Boolean

    Private sBankReference As String

    Private sMediaTypeCode As String

    Private dTransactionDate As Date

    Private dChequeDate As Date

    Private sAccountShortCode As String

    Private dAmount As Decimal

    Private sAllocationStatusCode As String

    Private sMediaReference As String

    Private sOurReference As String

    Private sTheirReference As String

    Private sContactName As String

    Private oContactAddress As Address

    Private sFurtherDetails As String

    Private dtCurrencyBaseDate As Nullable(Of Date)
    Private dCurrencyBaseXrate As Nullable(Of Decimal)
    Private dtAccountBaseDate As Nullable(Of Date)
    Private dAccountBaseXrate As Nullable(Of Decimal)
    Private dtSystemBaseDate As Nullable(Of Date)
    Private dSystemBaseXrate As Nullable(Of Decimal)
    Private nOverrideReason As Integer

    '''<remarks/>
    Public Property IsProduceDocument() As Boolean
        Get
            Return Me.bIsProduceDocument
        End Get
        Set(ByVal value As Boolean)
            Me.bIsProduceDocument = value
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
    Public Property MediaTypeCode() As String
        Get
            Return Me.sMediaTypeCode
        End Get
        Set(ByVal value As String)
            Me.sMediaTypeCode = value
        End Set
    End Property

    '''<remarks/>

    Public Property ChequeDate() As Date
        Get
            Return Me.dChequeDate
        End Get
        Set(ByVal value As Date)
            Me.dChequeDate = value
        End Set
    End Property
    '''<remarks/>

    Public Property TransactionDate() As Date
        Get
            Return Me.dTransactionDate
        End Get
        Set(ByVal value As Date)
            Me.dTransactionDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property AccountShortCode() As String
        Get
            Return Me.sAccountShortCode
        End Get
        Set(ByVal value As String)
            Me.sAccountShortCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property Amount() As Decimal
        Get
            Return Me.dAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dAmount = value
        End Set
    End Property

    '''<remarks/>
    Public Property AllocationStatusCode() As String
        Get
            Return Me.sAllocationStatusCode
        End Get
        Set(ByVal value As String)
            Me.sAllocationStatusCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property MediaReference() As String
        Get
            Return Me.sMediaReference
        End Get
        Set(ByVal value As String)
            Me.sMediaReference = value
        End Set
    End Property

    '''<remarks/>
    Public Property OurReference() As String
        Get
            Return Me.sOurReference
        End Get
        Set(ByVal value As String)
            Me.sOurReference = value
        End Set
    End Property

    '''<remarks/>
    Public Property TheirReference() As String
        Get
            Return Me.sTheirReference
        End Get
        Set(ByVal value As String)
            Me.sTheirReference = value
        End Set
    End Property

    '''<remarks/>
    Public Property ContactName() As String
        Get
            Return Me.sContactName
        End Get
        Set(ByVal value As String)
            Me.sContactName = value
        End Set
    End Property

    '''<remarks/>
    Public Property ContactAddress() As Address
        Get
            Return Me.oContactAddress
        End Get
        Set(ByVal value As Address)
            Me.oContactAddress = value
        End Set
    End Property

    '''<remarks/>
    Public Property FurtherDetails() As String
        Get
            Return Me.sFurtherDetails
        End Get
        Set(ByVal value As String)
            Me.sFurtherDetails = value
        End Set
    End Property

    Public Property CurrencyBaseDate() As Nullable(Of Date)
        Get
            Return Me.dtCurrencyBaseDate
        End Get
        Set(ByVal value As Nullable(Of Date))
            Me.dtCurrencyBaseDate = value
        End Set
    End Property

    Public Property CurrencyBaseXrate() As Nullable(Of Decimal)
        Get
            Return Me.dCurrencyBaseXrate
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            Me.dCurrencyBaseXrate = value
        End Set
    End Property

    Public Property AccountBaseDate() As Nullable(Of Date)
        Get
            Return Me.dtAccountBaseDate
        End Get
        Set(ByVal value As Nullable(Of Date))
            Me.dtAccountBaseDate = value
        End Set
    End Property

    Public Property AccountBaseXrate() As Nullable(Of Decimal)
        Get
            Return Me.dAccountBaseXrate
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            Me.dAccountBaseXrate = value
        End Set
    End Property

    Public Property SystemBaseDate() As Nullable(Of Date)
        Get
            Return Me.dtSystemBaseDate
        End Get
        Set(ByVal value As Nullable(Of Date))
            Me.dtSystemBaseDate = value
        End Set
    End Property

    Public Property SystemBaseXrate() As Nullable(Of Decimal)
        Get
            Return Me.dSystemBaseXrate
        End Get
        Set(ByVal value As Nullable(Of Decimal))
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

    Public Sub New()
        oContactAddress = New Address()
    End Sub
End Class


<Serializable()> Public Class CoreCashListType

    Private sTypeCode As String

    Private dListDate As Date

    Private sBankAccountCode As String

    Private sCurrencyCode As String

    Private sReference As String

    Private sStatusCode As String

    Private sBankAccountName As String

    Private nCashListKey As Integer

    Private nbankAccountKeyField As Integer

    Private sSubBranchCodeField As String

    '''<remarks/>
    Public Property TypeCode() As String
        Get
            Return Me.sTypeCode
        End Get
        Set(ByVal value As String)
            Me.sTypeCode = value
        End Set
    End Property


    Public Property ListDate() As Date
        Get
            Return Me.dListDate
        End Get
        Set(ByVal value As Date)
            Me.dListDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property BankAccountCode() As String
        Get
            Return Me.sBankAccountCode
        End Get
        Set(ByVal value As String)
            Me.sBankAccountCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property CurrencyCode() As String
        Get
            Return Me.sCurrencyCode
        End Get
        Set(ByVal value As String)
            Me.sCurrencyCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property Reference() As String
        Get
            Return Me.sReference
        End Get
        Set(ByVal value As String)
            Me.sReference = value
        End Set
    End Property

    '''<remarks/>
    Public Property StatusCode() As String
        Get
            Return Me.sStatusCode
        End Get
        Set(ByVal value As String)
            Me.sStatusCode = value
        End Set
    End Property


    ''' <summary>
    ''' Bank Account Name for Reciept/Payment
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BankAccountName() As String
        Get
            Return Me.sBankAccountName
        End Get
        Set(ByVal value As String)
            Me.sBankAccountName = value
        End Set
    End Property

    ''' <summary>
    ''' CashList Key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CashListKey() As String
        Get
            Return Me.nCashListKey
        End Get
        Set(ByVal value As String)
            Me.nCashListKey = value
        End Set
    End Property

    ''' <summary>
    ''' Bank Account Key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BankAccountKey() As Integer
        Get
            Return Me.nbankAccountKeyField
        End Get
        Set(ByVal value As Integer)
            Me.nbankAccountKeyField = value
        End Set
    End Property

    Public Property SubBranchCode() As String
        Get
            Return sSubBranchCodeField
        End Get
        Set(ByVal value As String)
            sSubBranchCodeField = value
        End Set
    End Property
End Class


<Serializable()> Public Class ReceiptCashListItemTypePolicies

    Private iInsuranceFileKey As Integer

    Private sDocumentRef As String

    Private iWriteOffReasonKey As Integer

    Private dWriteOffAmount As Decimal

    Private bIsCurrencyWriteOff As Boolean

    Private dAmountTobeAllocated As Decimal

    Private iBGKey As Integer

    Private sPolicyRef As String
    '''<remarks/>
    Public Property InsuranceFileKey() As Integer
        Get
            Return Me.iInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            Me.iInsuranceFileKey = value
        End Set
    End Property
    Public Property PolicyRef() As String
        Get
            Return Me.sPolicyRef
        End Get
        Set(ByVal value As String)
            Me.sPolicyRef = value
        End Set
    End Property

    '''<remarks/>
    Public Property DocumentRef() As String
        Get
            Return Me.sDocumentRef
        End Get
        Set(ByVal value As String)
            Me.sDocumentRef = value
        End Set
    End Property

    '''<remarks/>
    Public Property WriteOffReasonKey() As Integer
        Get
            Return Me.iWriteOffReasonKey
        End Get
        Set(ByVal value As Integer)
            Me.iWriteOffReasonKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property WriteOffAmount() As Decimal
        Get
            Return Me.dWriteOffAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dWriteOffAmount = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsCurrencyWriteOff() As Boolean
        Get
            Return Me.bIsCurrencyWriteOff
        End Get
        Set(ByVal value As Boolean)
            Me.bIsCurrencyWriteOff = value
        End Set
    End Property

    '''<remarks/>
    Public Property AmountTobeAllocated() As Decimal
        Get
            Return Me.dAmountTobeAllocated
        End Get
        Set(ByVal value As Decimal)
            Me.dAmountTobeAllocated = value
        End Set
    End Property

    '''<remarks/>
    Public Property BGKey() As Integer
        Get
            Return Me.iBGKey
        End Get
        Set(ByVal value As Integer)
            Me.iBGKey = value
        End Set
    End Property
End Class

<Serializable()> Public Class BankReceiptType

    Private sBankCode As String

    Private sPayerName As String

    Private dChequeDate As Date

    '''<remarks/>
    Public Property BankCode() As String
        Get
            Return Me.sBankCode
        End Get
        Set(ByVal value As String)
            Me.sBankCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property PayerName() As String
        Get
            Return Me.sPayerName
        End Get
        Set(ByVal value As String)
            Me.sPayerName = value
        End Set
    End Property


    Public Property ChequeDate() As Date
        Get
            Return Me.dChequeDate
        End Get
        Set(ByVal value As Date)
            Me.dChequeDate = value
        End Set
    End Property
End Class
Public Enum AllocationType

    '''<remarks/>
    BankGuarantee
End Enum

<Serializable()> Public Class ReceiptCashListItemTypePoliciesCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oBaseReceiptCashListItemTypePolicies As ReceiptCashListItemTypePolicies In List
            'sbPrint.AppendLine(oBaseReceiptCashListItemTypePolicies.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oBaseReceiptCashListItemTypePolicies As ReceiptCashListItemTypePolicies) As Integer
        Return List.Add(v_oBaseReceiptCashListItemTypePolicies)
    End Function

    Public Sub Remove(ByVal v_oBaseReceiptCashListItemTypePolicies As ReceiptCashListItemTypePolicies)
        List.Remove(v_oBaseReceiptCashListItemTypePolicies)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As ReceiptCashListItemTypePolicies
        Get
            Return List(i)
        End Get
        Set(ByVal value As ReceiptCashListItemTypePolicies)
            List(i) = value
        End Set
    End Property

End Class
