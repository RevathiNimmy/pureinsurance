<Serializable()> Public Class ProductClaimsWorkflowOptionsValue

    Private bCashPaymentProcess As Boolean
    Private bCheckDeferredReinsurances As Boolean
    Private bCheckUnpaidStatus As Boolean
    Private bClaimNotificationDocMessage As Boolean
    Private bClaimPaymentDocMessage As Boolean
    Private bClaimPaymentProcess As Boolean
    Private bDescriptionForChangeInPayment As Boolean
    Private bDescriptionForChangeInReserve As Boolean
    Private bExternalClaimHandling As Boolean
    Private bFastTrackClaims As Boolean
    Private bGenerateClaimNotificationDoc As Boolean
    Private bGenerateClaimPaymentDoc As Boolean
    Private bMakeFurtherPayments As Boolean
    Private bReinsurancePayment As Boolean
    Private bReinsuranceRecovery As Boolean
    Private bSalvageRecovery As Boolean
    Private bThirdPartyRecovery As Boolean

    Sub New()

    End Sub
    Public Property ThirdPartyRecovery() As Boolean
        Get
            Return Me.bThirdPartyRecovery
        End Get
        Set(ByVal value As Boolean)
            Me.bThirdPartyRecovery = value
        End Set
    End Property
    Public Property SalvageRecovery() As Boolean
        Get
            Return Me.bSalvageRecovery
        End Get
        Set(ByVal value As Boolean)
            Me.bSalvageRecovery = value
        End Set
    End Property
    Public Property ReinsuranceRecovery() As Boolean
        Get
            Return Me.bReinsuranceRecovery
        End Get
        Set(ByVal value As Boolean)
            Me.bReinsuranceRecovery = value
        End Set
    End Property
    Public Property ReinsurancePayment() As Boolean
        Get
            Return Me.bReinsurancePayment
        End Get
        Set(ByVal value As Boolean)
            Me.bReinsurancePayment = value
        End Set
    End Property
    Public Property MakeFurtherPayments() As Boolean
        Get
            Return Me.bMakeFurtherPayments
        End Get
        Set(ByVal value As Boolean)
            Me.bMakeFurtherPayments = value
        End Set
    End Property
    Public Property GenerateClaimPaymentDoc() As Boolean
        Get
            Return Me.bGenerateClaimPaymentDoc
        End Get
        Set(ByVal value As Boolean)
            Me.bGenerateClaimPaymentDoc = value
        End Set
    End Property
    Public Property GenerateClaimNotificationDoc() As Boolean
        Get
            Return Me.bGenerateClaimNotificationDoc
        End Get
        Set(ByVal value As Boolean)
            Me.bGenerateClaimNotificationDoc = value
        End Set
    End Property
    Public Property FastTrackClaims() As Boolean
        Get
            Return Me.bFastTrackClaims
        End Get
        Set(ByVal value As Boolean)
            Me.bFastTrackClaims = value
        End Set
    End Property
    Public Property ExternalClaimHandling() As Boolean
        Get
            Return Me.bExternalClaimHandling
        End Get
        Set(ByVal value As Boolean)
            Me.bExternalClaimHandling = value
        End Set
    End Property
    Public Property DescriptionForChangeInReserve() As Boolean
        Get
            Return Me.bDescriptionForChangeInReserve
        End Get
        Set(ByVal value As Boolean)
            Me.bDescriptionForChangeInReserve = value
        End Set
    End Property
    Public Property DescriptionForChangeInPayment() As Boolean
        Get
            Return Me.bDescriptionForChangeInPayment
        End Get
        Set(ByVal value As Boolean)
            Me.bDescriptionForChangeInPayment = value
        End Set
    End Property
    Public Property ClaimPaymentProcess() As Boolean
        Get
            Return Me.bClaimPaymentProcess
        End Get
        Set(ByVal value As Boolean)
            Me.bClaimPaymentProcess = value
        End Set
    End Property
    Public Property ClaimPaymentDocMessage() As Boolean
        Get
            Return Me.bClaimPaymentDocMessage
        End Get
        Set(ByVal value As Boolean)
            Me.bClaimPaymentDocMessage = value
        End Set
    End Property
    Public Property ClaimNotificationDocMessage() As Boolean
        Get
            Return Me.bClaimNotificationDocMessage
        End Get
        Set(ByVal value As Boolean)
            Me.bClaimNotificationDocMessage = value
        End Set
    End Property
    Public Property CheckUnpaidStatus() As Boolean
        Get
            Return Me.bCheckUnpaidStatus
        End Get
        Set(ByVal value As Boolean)
            Me.bCheckUnpaidStatus = value
        End Set
    End Property
    Public Property CheckDeferredReinsurances() As Boolean
        Get
            Return Me.bCheckDeferredReinsurances
        End Get
        Set(ByVal value As Boolean)
            Me.bCheckDeferredReinsurances = value
        End Set
    End Property
    Public Property CashPaymentProcess() As Boolean
        Get
            Return Me.bCashPaymentProcess
        End Get
        Set(ByVal value As Boolean)
            Me.bCashPaymentProcess = value
        End Set
    End Property

End Class
