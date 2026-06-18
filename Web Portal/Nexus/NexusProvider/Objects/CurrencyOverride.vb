<Serializable()> Public Class CurrencyOverride
    Private _dateOverrideAllowed As Boolean
    Private _rateOverrideAllowed As Boolean
    Private _prePolicdateOverrideAllowed As Boolean
    Private _prePolicyRateOverrideAllowed As Boolean

    Public Sub New()

    End Sub
    Public Property DateOverrideAllowed As Boolean
        Get
            Return _dateOverrideAllowed
        End Get
        Set(value As Boolean)
            _dateOverrideAllowed = value
        End Set
    End Property

    Public Property RateOverrideAllowed As Boolean
        Get
            Return _rateOverrideAllowed
        End Get
        Set(value As Boolean)
            _rateOverrideAllowed = value
        End Set
    End Property

    Public Property PrePolicyDateOverrideAllowed As Boolean
        Get
            Return _prePolicdateOverrideAllowed
        End Get
        Set(value As Boolean)
            _prePolicdateOverrideAllowed = value
        End Set
    End Property

    Public Property PrePolicyRateOverrideAllowed As Boolean
        Get
            Return _prePolicyRateOverrideAllowed
        End Get
        Set(value As Boolean)
            _prePolicyRateOverrideAllowed = value
        End Set
    End Property
End Class