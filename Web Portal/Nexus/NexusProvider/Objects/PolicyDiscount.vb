<Serializable()> Public Class PolicyDiscount

    Private bIsDiscountAppliedField As Boolean
    Private dDiscountPercentageField As Double
    Private crDiscountedPremiumField As Decimal
    Private crTotalPremiumField As Decimal
    Private iDiscountReasonIdField As Integer
    Private iRecurringTypeIdField As Integer
    Private iMatchDiscountedPremiumField As Integer

    Public Property IsDiscountApplied() As Boolean
        Get
            Return Me.bIsDiscountAppliedField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsDiscountAppliedField = value
        End Set
    End Property

    Public Property DiscountPercentage() As Double
        Get
            Return Me.dDiscountPercentageField
        End Get
        Set(ByVal value As Double)
            Me.dDiscountPercentageField = value
        End Set
    End Property

    Public Property DiscountedPremium() As Decimal
        Get
            Return Me.crDiscountedPremiumField
        End Get
        Set(ByVal value As Decimal)
            Me.crDiscountedPremiumField = value
        End Set
    End Property

    Public Property TotalPremium() As Decimal
        Get
            Return Me.crTotalPremiumField
        End Get
        Set(ByVal value As Decimal)
            Me.crTotalPremiumField = value
        End Set
    End Property

    Public Property DiscountReasonId() As Integer
        Get
            Return Me.iDiscountReasonIdField
        End Get
        Set(ByVal value As Integer)
            Me.iDiscountReasonIdField = value
        End Set
    End Property

    Public Property RecurringTypeId() As Integer
        Get
            Return Me.iRecurringTypeIdField
        End Get
        Set(ByVal value As Integer)
            Me.iRecurringTypeIdField = value
        End Set
    End Property

    Public Property MatchDiscountedPremium() As Integer
        Get
            Return Me.iMatchDiscountedPremiumField
        End Get
        Set(ByVal value As Integer)
            Me.iMatchDiscountedPremiumField = value
        End Set
    End Property

End Class
