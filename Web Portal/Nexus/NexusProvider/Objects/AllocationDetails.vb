''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class AllocationDetails

#Region "Private Variables"

    Private sDocRef As String
    Private dtTransDate As DateTime
    Private dtAllocatedDate As DateTime
    Private dtEffectiveDate As DateTime
    Private dAllocatedAmount As Double
    Private dOriginalAmount As Double
    Private dWriteOffAmount As Double
    Private dOutstandingAmount As Double
    Private sDocType As String
    Private sInsuranceRef As String
    Private sAltRef As String
    Private sAccount As String
    Private sUser As String
    Private sMediaType As String
    Private sMediaRef As String
    Private sAccountCode As String
    Private sCurrency As String

    Private iAllocationTransDetailKey As Integer
    'Newly Added Properties for Update Allocation
    Private iAccountKey As Integer
    Private dAmount As Double
    Private iCashListItemKey As Integer
    Private iTransdetailKey As Integer
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
    Private iAllocationKey As Integer
    Private sSpare As String
    Private bIsSplitReceipt As Boolean
    Private bIsLeadAgent As Boolean
    Private sTransactionCurrencyCode As String
    Private nInsuranceFileKey As Integer
    Private nDocumentTypeID As Integer
    Private sPeriod As String
    Private sPrimarySettled As String
    Private sDocGroup As String

#End Region

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        oAllocation = New AllocationCollection
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Doc Ref : " & sDocRef & "<br />")
        sbPrint.AppendLine("TransDate : " & dtTransDate.ToString() & "<br />")
        sbPrint.AppendLine("Allocated Date : " & dtAllocatedDate.ToString() & "<br />")
        sbPrint.AppendLine("Allocated Amount : " & dAllocatedAmount.ToString() & "<br />")
        sbPrint.AppendLine("AllocationAmount : " & dAllocatedAmount.ToString() & "<br />")
        sbPrint.AppendLine("Original Amount : " & dOriginalAmount.ToString() & "<br />")
        sbPrint.AppendLine("Write Off Amount : " & dWriteOffAmount.ToString() & "<br />")
        sbPrint.AppendLine("Doc Type : " & sDocType & "<br />")
        sbPrint.AppendLine("Insurance Ref : " & sInsuranceRef & "<br />")
        sbPrint.AppendLine("Account : " & sAccount & "<br />")
        sbPrint.AppendLine("sUser : " & sUser & "<br />")
        sbPrint.AppendLine("sCurrency : " & sUser & "<br />")

        Return sbPrint.ToString()

    End Function

#Region "Public Properties"
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

    Public Property DocType() As String
        Get
            Return Me.sDocType
        End Get
        Set(ByVal value As String)
            sDocType = value
        End Set
    End Property

    Public Property InsuranceRef() As String
        Get
            Return Me.sInsuranceRef
        End Get
        Set(ByVal value As String)
            sInsuranceRef = value
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
    Public Property Amount() As Double
        Get
            Return Me.dAmount
        End Get
        Set(ByVal value As Double)
            Me.dAmount = value
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
    Public Property TransdetailKey() As Integer
        Get
            Return Me.iTransdetailKey
        End Get
        Set(ByVal value As Integer)
            Me.iTransdetailKey = value
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
        Set(ByVal value As Byte())
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

    Public Property AllocationKey() As Integer
        Get
            Return Me.iAllocationKey
        End Get
        Set(ByVal value As Integer)
            Me.iAllocationKey = value
        End Set
    End Property

    Public Property InsuranceFileKey() As Integer
        Get
            Return Me.nInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            Me.nInsuranceFileKey = value
        End Set
    End Property

    Public Property Spare() As String
        Get
            Return Me.sSpare
        End Get
        Set(ByVal value As String)
            Me.sSpare = value
        End Set
    End Property

    Public Property IsSplitReceipt() As Boolean
        Get
            Return Me.bIsSplitReceipt
        End Get
        Set(ByVal value As Boolean)
            Me.bIsSplitReceipt = value
        End Set
    End Property

    Public Property IsLeadAgent() As Boolean
        Get
            Return Me.bIsLeadAgent
        End Get
        Set(ByVal value As Boolean)
            Me.bIsLeadAgent = value
        End Set
    End Property

    Public Property DocumentTypeID() As Integer
        Get
            Return Me.nDocumentTypeID
        End Get
        Set(ByVal value As Integer)
            Me.nDocumentTypeID = value
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

    Public Property TransactionCurrencyCode As String

    Public Property CurrencyCode As String

    Dim nSourceID As Integer
    Public Property SourceID() As Integer
        Get
            Return Me.nSourceID
        End Get
        Set(ByVal value As Integer)
            Me.nSourceID = value
        End Set
    End Property

#End Region

End Class

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class AllocationDetailsCollections : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oAllocationDetails As AllocationDetails In List
            sbPrint.AppendLine(oAllocationDetails.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oAllocationDetails As AllocationDetails) As Integer
        Return List.Add(v_oAllocationDetails)
    End Function

    Public Sub Remove(ByVal v_oAllocationDetails As AllocationDetails)
        List.Remove(v_oAllocationDetails)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As AllocationDetails
        Get
            Return List(i)
        End Get
        Set(ByVal value As AllocationDetails)
            List(i) = value
        End Set
    End Property

End Class
