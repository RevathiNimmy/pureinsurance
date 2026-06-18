<Serializable()> Public Class FinancePlanTransactions
    Private nPFTransactionKeyField As Integer
    Private nInsuranceRefIndexField As String
    Private nAmountField As Double
    Private nInsuranceFileKeyField As Integer
    Private sSpareField As String
    Private nDocumentTypeIdField As Integer


    Private sDocRef As String
    Private dtTransDate As DateTime
    Private dtAllocatedDate As DateTime
    Private dtEffectiveDate As DateTime
    Private dAllocatedAmount As Double
    Private dOriginalAmount As Double
    Private dWriteOffAmount As Double
    Private dOutstandingAmount As Double
    Private sDocType As String
    Private sAltRef As String
    Private sAccount As String
    Private sUser As String
    Private sMediaType As String
    Private sMediaRef As String
    Private sAccountCode As String
    Private sCurrency As String
    Private nSourceID As Integer

    Private iAllocationTransDetailKey As Integer
    'Newly Added Properties for Update Allocation
    Private iAccountKey As Integer
    Private iCashListItemKey As Integer
    Private iWriteOffReason As Integer
    Private dCurrencyDiffField As Double
    Private bAllocationStatus As Boolean
    Private bWriteOffAmountSpecified As Boolean
    Private bWriteOffReasonSpecified As Boolean
    Private bCurrencyDiffSpecified As Boolean
    Private sBranchCode As String
    Private bAllactionTimeStamp() As Byte
    Private dAllocationAmount, dTransactionCurrencyAmount As Double
    Private oAllocation As AllocationCollection
    Private sTaxBand, sTransactionCurrency As String
    Private sTransactionCurrencyCode As String
    Private iInsuranceFileKey As Integer
    Private sPeriod As String
    Private sPrimarySettled As String
    Private sDocGroup As String

    '''<remarks/>
    Public Property PFTransactionKey() As Integer
        Get
            Return Me.nPFTransactionKeyField
        End Get
        Set(ByVal value As Integer)
            Me.nPFTransactionKeyField = value
        End Set
    End Property

    '''<remarks/>
    Public Property InsuranceRefIndex() As String
        Get
            Return Me.nInsuranceRefIndexField
        End Get
        Set(ByVal value As String)
            Me.nInsuranceRefIndexField = value
        End Set
    End Property
    '''<remarks/>
    Public Property Amount() As Double
        Get
            Return Me.nAmountField
        End Get
        Set(ByVal value As Double)
            Me.nAmountField = value
        End Set
    End Property
    '''<remarks/>
    Public Property InsuranceFileKey() As Integer
        Get
            Return Me.nInsuranceFileKeyField
        End Get
        Set(ByVal value As Integer)
            Me.nInsuranceFileKeyField = value
        End Set
    End Property

    Public Property Spare() As String
        Get
            Return Me.sSpareField
        End Get
        Set(ByVal value As String)
            Me.sSpareField = value
        End Set
    End Property

    Public Property DocumentTypeId() As Integer
        Get
            Return Me.nDocumentTypeIdField
        End Get
        Set(ByVal value As Integer)
            Me.nDocumentTypeIdField = value
        End Set
    End Property


    Public Property TransactionCurrency() As String
        Get
            Return Me.sTransactionCurrency
        End Get
        Set(ByVal value As String)
            Me.sTransactionCurrency = value
        End Set
    End Property
    Public Property TaxBand() As String
        Get
            Return Me.sTaxBand
        End Get
        Set(ByVal value As String)
            Me.sTaxBand = value
        End Set
    End Property
    Public Property AllocationTransDetailKey() As Integer
        Get
            Return Me.iAllocationTransDetailKey
        End Get
        Set(ByVal value As Integer)
            Me.iAllocationTransDetailKey = value
        End Set
    End Property

    Public Property DocRef() As String
        Get
            Return Me.sDocRef
        End Get
        Set(ByVal value As String)
            Me.sDocRef = value
        End Set
    End Property

    Public Property TransDate() As DateTime
        Get
            Return Me.dtTransDate
        End Get
        Set(ByVal value As DateTime)
            dtTransDate = value
        End Set
    End Property

    Public Property EffectiveDate() As DateTime
        Get
            Return Me.dtEffectiveDate
        End Get
        Set(ByVal value As DateTime)
            dtEffectiveDate = value
        End Set
    End Property

    Public Property AccountCode() As String
        Get
            Return Me.sAccountCode
        End Get
        Set(ByVal value As String)
            Me.sAccountCode = value
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

    Public Property MediaRef() As String
        Get
            Return Me.sMediaRef
        End Get
        Set(ByVal value As String)
            Me.sMediaRef = value
        End Set
    End Property
    Public Property TransactionCurrencyAmount() As Double
        Get
            Return Me.dTransactionCurrencyAmount
        End Get
        Set(ByVal value As Double)
            dTransactionCurrencyAmount = value
        End Set
    End Property
    Public Property TransactionCurrencyCode() As String
        Get
            Return Me.sTransactionCurrencyCode
        End Get
        Set(ByVal value As String)
            sTransactionCurrencyCode = value
        End Set
    End Property
    Public Property OutStandingamount() As Double
        Get
            Return Me.dOutstandingAmount
        End Get
        Set(ByVal value As Double)
            dOutstandingAmount = value
        End Set
    End Property
    Public Property AltRef() As String
        Get
            Return Me.sAltRef
        End Get
        Set(ByVal value As String)
            Me.sAltRef = value
        End Set
    End Property
    Public Property AllocatedDate() As DateTime
        Get
            Return Me.dtAllocatedDate
        End Get
        Set(ByVal value As DateTime)
            dtAllocatedDate = value
        End Set
    End Property

    Public Property AllocatedAmount() As Double
        Get
            Return Me.dAllocatedAmount
        End Get
        Set(ByVal value As Double)
            dAllocatedAmount = value
        End Set
    End Property
    Public Property AllocationAmount() As Double
        Get
            Return Me.dAllocationAmount
        End Get
        Set(ByVal value As Double)
            dAllocationAmount = value
        End Set
    End Property

    Public Property OriginalAmount() As Double
        Get
            Return Me.dOriginalAmount
        End Get
        Set(ByVal value As Double)
            dOriginalAmount = value
        End Set
    End Property

    Public Property WriteOffAmount() As Double
        Get
            Return Me.dWriteOffAmount
        End Get
        Set(ByVal value As Double)
            dWriteOffAmount = value
        End Set
    End Property

    Public Property Account() As String
        Get
            Return Me.sAccount
        End Get
        Set(ByVal value As String)
            sAccount = value
        End Set
    End Property

    Public Property User() As String
        Get
            Return Me.sUser
        End Get
        Set(ByVal value As String)
            sUser = value
        End Set
    End Property

    Public Property Allocation() As AllocationCollection
        Get
            Return Me.oAllocation
        End Get
        Set(ByVal value As AllocationCollection)
            Me.oAllocation = value
        End Set
    End Property
    'Newly Added Properties for Update Allocation
    '''<remarks/>
    Public Property AccountKey() As Integer
        Get
            Return Me.iAccountKey
        End Get
        Set(ByVal value As Integer)
            Me.iAccountKey = value
        End Set
    End Property
   
    '''<remarks/>
    Public Property CashListItemKey() As Integer
        Get
            Return Me.iCashListItemKey
        End Get
        Set(ByVal value As Integer)
            Me.iCashListItemKey = value
        End Set
    End Property
    
    '''<remarks/>
    Public Property WriteOffReason() As Integer
        Get
            Return Me.iWriteOffReason
        End Get
        Set(ByVal value As Integer)
            Me.iWriteOffReason = value
        End Set
    End Property
    '''<remarks/>
    Public Property CurrencyDiff() As Double
        Get
            Return Me.dCurrencyDiffField
        End Get
        Set(ByVal value As Double)
            Me.dCurrencyDiffField = value
        End Set
    End Property
    '''<remarks/>
    Public Property AllocationStatus() As Boolean
        Get
            Return Me.bAllocationStatus
        End Get
        Set(ByVal value As Boolean)
            Me.bAllocationStatus = value
        End Set
    End Property
    Public ReadOnly Property WriteOffAmountSpecified() As Boolean
        Get
            Return bWriteOffAmountSpecified
        End Get
    End Property
    Public ReadOnly Property WriteOffReasonSpecified() As Boolean
        Get
            Return bWriteOffReasonSpecified
        End Get
    End Property
    Public ReadOnly Property CurrencyDiffSpecified() As Boolean
        Get
            Return bCurrencyDiffSpecified
        End Get
    End Property

    Public Property BranchCode() As String
        Get
            Return Me.sBranchCode
        End Get
        Set(ByVal value As String)
            Me.sBranchCode = value
        End Set
    End Property

    Public Property AllocationTimeStamp() As Byte()
        Get
            Return bAllactionTimeStamp
        End Get
        Set(ByVal value() As Byte)
            bAllactionTimeStamp = value
        End Set
    End Property
    Public Property Currency() As String
        Get
            Return Me.sCurrency
        End Get
        Set(ByVal value As String)
            Me.sCurrency = value
        End Set
    End Property

    Public Property Period() As String
        Get
            Return Me.sPeriod
        End Get
        Set(ByVal value As String)
            sPeriod = value
        End Set
    End Property

    Public Property PrimarySettled() As String
        Get
            Return Me.sPrimarySettled
        End Get
        Set(ByVal value As String)
            sPrimarySettled = value
        End Set
    End Property
    Public Property DocGroup() As String
        Get
            Return Me.sDocGroup
        End Get
        Set(ByVal value As String)
            sDocGroup = value
        End Set
    End Property

    Public Property DocType() As String
        Get
            Return Me.sDocType
        End Get
        Set(ByVal value As String)
            sDocType = value
        End Set
    End Property
    'rajeev
    '''<remarks/>
    Public Property SourceID() As Integer
        Get
            Return Me.nSourceID
        End Get
        Set(ByVal value As Integer)
            Me.nSourceID = value
        End Set
    End Property
End Class

<Serializable()> Public Class FinancePlanTransactionsCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oFinancePlanTransaction As FinancePlanTransactions In List
            'sbPrint.AppendLine(oInstalments.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oFinancePlanTransactions As FinancePlanTransactions) As Integer
        Return List.Add(v_oFinancePlanTransactions)
    End Function

    Public Sub Remove(ByVal v_oFinancePlanTransactions As FinancePlanTransactions)
        List.Remove(v_oFinancePlanTransactions)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As FinancePlanTransactions
        Get
            Return List(i)
        End Get
        Set(ByVal value As FinancePlanTransactions)
            List(i) = value
        End Set
    End Property

End Class
