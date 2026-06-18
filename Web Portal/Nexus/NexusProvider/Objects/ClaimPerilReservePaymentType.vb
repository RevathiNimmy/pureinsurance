<Serializable()> Public Class ClaimPerilReservePaymentType

    Private iBaseReserveKey As Integer
    Private sTypeCode As String
    Private dTotalReserve As Decimal
    Private dPaidToDate As Decimal
    Private dPaidToDateTax As Decimal
    Private dCurrentReserve As Decimal
    Private dThisPaymentINCLTax As Decimal
    Private dThisPaymentTax As Decimal
    Private dCostToClaim As Decimal
    Private dThisRevision As Decimal
    Private bIsExcess As Boolean
    Private bIsIndemnity As Boolean
    Private bIsExpense As Boolean
    Private dOldReserve As Decimal
    Private bIsLocked As Boolean
    Private sCurrencyCode, sDescription As String
    Private dCurrencyRate As Double
    Private iPayQueue As Integer

    Public Property PayQueue() As Integer
        Get
            Return Me.iPayQueue
        End Get
        Set(ByVal value As Integer)
            Me.iPayQueue = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return Me.sDescription
        End Get
        Set(ByVal value As String)
            Me.sDescription = value
        End Set
    End Property
    Public Property CurrencyRate() As Double
        Get
            Return Me.dCurrencyRate
        End Get
        Set(ByVal value As Double)
            Me.dCurrencyRate = value
        End Set
    End Property
    Public Property CurrencyCode() As String
        Get
            Return Me.sCurrencyCode
        End Get
        Set(ByVal value As String)
            Me.sCurrencyCode = value
        End Set
    End Property
    '''<remarks/>
    Public Property BaseReserveKey() As Integer
        Get
            Return Me.iBaseReserveKey
        End Get
        Set(ByVal value As Integer)
            Me.iBaseReserveKey = value
        End Set
    End Property

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
    Public Property TotalReserve() As Decimal
        Get
            Return Me.dTotalReserve
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalReserve = value
        End Set
    End Property

    '''<remarks/>
    Public Property PaidToDate() As Decimal
        Get
            Return Me.dPaidToDate
        End Get
        Set(ByVal value As Decimal)
            Me.dPaidToDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property PaidToDateTax() As Decimal
        Get
            Return Me.dPaidToDateTax
        End Get
        Set(ByVal value As Decimal)
            Me.dPaidToDateTax = value
        End Set
    End Property

    '''<remarks/>
    Public Property CurrentReserve() As Decimal
        Get
            Return Me.dCurrentReserve
        End Get
        Set(ByVal value As Decimal)
            Me.dCurrentReserve = value
        End Set
    End Property

    '''<remarks/>
    Public Property OldReserve() As Decimal
        Get
            Return Me.dOldReserve
        End Get
        Set(ByVal value As Decimal)
            Me.dOldReserve = value
        End Set
    End Property

    '''<remarks/>
    Public Property ThisPaymentINCLTax() As Decimal
        Get
            Return Me.dThisPaymentINCLTax
        End Get
        Set(ByVal value As Decimal)
            Me.dThisPaymentINCLTax = value
        End Set
    End Property

    '''<remarks/>
    Public Property ThisPaymentTax() As Decimal
        Get
            Return Me.dThisPaymentTax
        End Get
        Set(ByVal value As Decimal)
            Me.dThisPaymentTax = value
        End Set
    End Property

    '''<remarks/>
    Public Property CostToClaim() As Decimal
        Get
            Return Me.dCostToClaim
        End Get
        Set(ByVal value As Decimal)
            Me.dCostToClaim = value
        End Set
    End Property

    Public Property ThisRevision() As Decimal
        Get
            Return Me.dThisRevision
        End Get
        Set(ByVal value As Decimal)
            Me.dThisRevision = value
        End Set
    End Property
    Public Property IsLocked() As Boolean
        Get
            Return Me.bIsLocked
        End Get
        Set(ByVal value As Boolean)
            Me.bIsLocked = value
        End Set
    End Property

    Public Property IsExcess() As Boolean
        Get
            Return Me.bIsExcess
        End Get
        Set(ByVal value As Boolean)
            Me.bIsExcess = value
        End Set
    End Property

    Public Property IsIndemnity() As Boolean
        Get
            Return Me.bIsIndemnity
        End Get
        Set(ByVal value As Boolean)
            Me.bIsIndemnity = value
        End Set
    End Property

    Public Property IsExpense() As Boolean
        Get
            Return Me.bIsExpense
        End Get
        Set(ByVal value As Boolean)
            Me.bIsExpense = value
        End Set
    End Property

    Public Property GrossReserve() As Decimal

    Public Property Tax() As Decimal

    Public Property RevisedGrossReserve() As Decimal

    Public Property RevisedTaxReserve() As Decimal

End Class
<Serializable()> Public Class ClaimPerilReservePaymentTypeCollection : Inherits SortableCollectionBase

    Public Sub New()
        MyBase.SortObjectType = GetType(ClaimPerilReservePaymentType)
    End Sub

    Public Function Add(ByVal v_oClaimPerilReservePaymentType As ClaimPerilReservePaymentType) As Integer
        Return List.Add(v_oClaimPerilReservePaymentType)
    End Function

    Public Sub Remove(ByVal v_oClaimPerilReservePaymentType As ClaimPerilReservePaymentType)
        List.Remove(v_oClaimPerilReservePaymentType)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As ClaimPerilReservePaymentType
        Get
            Return List(i)
        End Get
        Set(ByVal value As ClaimPerilReservePaymentType)
            List(i) = value
        End Set
    End Property
End Class
