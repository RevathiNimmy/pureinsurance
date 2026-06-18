<Serializable()> Public Class PaymentAdvancedTaxDetails
#Region "Private Fields"
    Private bInsuredDomiciledField As Boolean

    Private dInsuredPercentageField As Decimal

    Private sInsuranceTaxNumberField As String

    Private bPayeeDomiciledField As Boolean

    Private dPayeePercentageField As Decimal

    Private sPayeeTaxNumberField As String

    Private sSafeHarbourCodeField As String

    Private dSafeHarbourPercentageField As Decimal

    Private bIsTaxExemptField As Boolean

    Private bIsWHTExemptField As Boolean

    Private bIsSettlementField As Boolean
    Private sPaymentTo As String
    Private sPayeeName As String
    Private bIsExcess As Boolean
    Private bAdvancedTaxScriptOptionOn As Boolean
    Private nReserveKey As Integer
#End Region
#Region "Properties"
    Public Property ReserveKey() As Integer
        Get
            Return Me.nReserveKey
        End Get
        Set(ByVal value As Integer)
            Me.nReserveKey = value
        End Set
    End Property
    Public Property AdvancedTaxScriptOptionOn() As Boolean
        Get
            Return Me.bAdvancedTaxScriptOptionOn
        End Get
        Set(ByVal value As Boolean)
            Me.bAdvancedTaxScriptOptionOn = value
        End Set
    End Property
    '''<remarks/>
    Public Property PayeeName() As String
        Get
            Return Me.sPayeeName
        End Get
        Set(ByVal value As String)
            Me.sPayeeName = value
        End Set
    End Property
    '''<remarks/>
    Public Property IsExcess() As Boolean
        Get
            Return Me.bIsExcess
        End Get
        Set(ByVal value As Boolean)
            Me.bIsExcess = value
        End Set
    End Property
    '''<remarks/>
    Public Property InsuredDomiciled() As Boolean
        Get
            Return Me.bInsuredDomiciledField
        End Get
        Set(ByVal value As Boolean)
            Me.bInsuredDomiciledField = value
        End Set
    End Property

    '''<remarks/>
    Public Property InsuredPercentage() As Decimal
        Get
            Return Me.dInsuredPercentageField
        End Get
        Set(ByVal value As Decimal)
            Me.dInsuredPercentageField = value
        End Set
    End Property

    '''<remarks/>
    Public Property InsuranceTaxNumber() As String
        Get
            Return Me.sInsuranceTaxNumberField
        End Get
        Set(ByVal value As String)
            Me.sInsuranceTaxNumberField = value
        End Set
    End Property

    '''<remarks/>
    Public Property PayeeDomiciled() As Boolean
        Get
            Return Me.bPayeeDomiciledField
        End Get
        Set(ByVal value As Boolean)
            Me.bPayeeDomiciledField = value
        End Set
    End Property

    '''<remarks/>
    Public Property PayeePercentage() As Decimal
        Get
            Return Me.dPayeePercentageField
        End Get
        Set(ByVal value As Decimal)
            Me.dPayeePercentageField = value
        End Set
    End Property

    '''<remarks/>
    Public Property PayeeTaxNumber() As String
        Get
            Return Me.sPayeeTaxNumberField
        End Get
        Set(ByVal value As String)
            Me.sPayeeTaxNumberField = value
        End Set
    End Property

    '''<remarks/>
    Public Property SafeHarbourCode() As String
        Get
            Return Me.sSafeHarbourCodeField
        End Get
        Set(ByVal value As String)
            Me.sSafeHarbourCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property SafeHarbourPercentage() As Decimal
        Get
            Return Me.dSafeHarbourPercentageField
        End Get
        Set(ByVal value As Decimal)
            Me.dSafeHarbourPercentageField = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsTaxExempt() As Boolean
        Get
            Return Me.bIsTaxExemptField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsTaxExemptField = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsWHTExempt() As Boolean
        Get
            Return Me.bIsWHTExemptField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsWHTExemptField = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsSettlement() As Boolean
        Get
            Return Me.bIsSettlementField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsSettlementField = value
        End Set
    End Property
    ''' <remarks></remarks>
    Public Property PaymentTo() As String
        Get
            Return Me.sPaymentTo
        End Get
        Set(ByVal value As String)
            Me.sPaymentTo = value
        End Set
    End Property
#End Region
End Class

