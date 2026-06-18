' WPR VB 64 - Media Type Status
<Serializable()> _
Public Class MediaTypeStatus
    Private iIsPolicyCanceledField As Integer

    Private bIsClaimPaymentInitiatedField As Boolean

    Private bIsClaimPaymentInitiatedOnLossDateField As Boolean

    Private bIsUnclearedCashListExistsField As Boolean

    Private iInsuranceFileKeyField As Integer

    Private dLossDateField As Date

    Private bLossDateFieldSpecified As Boolean

    Public Property InsuranceFileKey() As Integer
        Get
            Return Me.iInsuranceFileKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iInsuranceFileKeyField = value
        End Set
    End Property

    Public Property LossDate() As Date
        Get
            Return Me.dLossDateField
        End Get
        Set(ByVal value As Date)
            Me.dLossDateField = value
        End Set
    End Property

    Public Property LossDateSpecified() As Boolean
        Get
            Return Me.bLossDateFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bLossDateFieldSpecified = value
        End Set
    End Property

    Public Property IsPolicyCanceled() As Integer
        Get
            Return Me.iIsPolicyCanceledField
        End Get
        Set(ByVal value As Integer)
            Me.iIsPolicyCanceledField = value
        End Set
    End Property


    Public Property IsClaimPaymentInitiated() As Boolean
        Get
            Return Me.bIsClaimPaymentInitiatedField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsClaimPaymentInitiatedField = value
        End Set
    End Property

    Public Property IsClaimPaymentInitiatedOnLossDate() As Boolean
        Get
            Return Me.bIsClaimPaymentInitiatedOnLossDateField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsClaimPaymentInitiatedOnLossDateField = value
        End Set
    End Property

    Public Property IsUnclearedCashListExists() As Boolean
        Get
            Return Me.bIsUnclearedCashListExistsField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsUnclearedCashListExistsField = value
        End Set
    End Property
End Class
