<Serializable()> Public Class ClaimReceiptAdvancedTaxDetails

    Private bIsSettlement As Boolean
    Private bIsTaxExempt As Boolean
    Private dReceivableTaxPercentage As Decimal
    Private bInsuredDomiciled As Boolean
    Private dInsuredPercentage As Decimal
    Private sInsuredTaxNumber As String

    Private bPayeeDomiciled As Boolean
    Private dPayeePercentage As Decimal
    Private sPayeeTaxNumber As String
    Private sSafeHarbourCode As String
    Private dSafeHarbourPercentage As Decimal
    Private bIsWHTExempt As Boolean
    Private sPayeeName As String
    Private bAdvancedTaxScriptOptionOn As Boolean

    Public Sub New()

    End Sub
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
    Public Property IsSettlement() As Boolean
        Get
            Return Me.bIsSettlement
        End Get
        Set(ByVal value As Boolean)
            Me.bIsSettlement = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsTaxExempt() As Boolean
        Get
            Return Me.bIsTaxExempt
        End Get
        Set(ByVal value As Boolean)
            Me.bIsTaxExempt = value
        End Set
    End Property

    '''<remarks/>
    Public Property ReceivableTaxPercentage() As Decimal
        Get
            Return Me.dReceivableTaxPercentage
        End Get
        Set(ByVal value As Decimal)
            Me.dReceivableTaxPercentage = value
        End Set
    End Property

    '''<remarks/>
    Public Property InsuredDomiciled() As Boolean
        Get
            Return Me.bInsuredDomiciled
        End Get
        Set(ByVal value As Boolean)
            Me.bInsuredDomiciled = value
        End Set
    End Property

    '''<remarks/>
    Public Property InsuredPercentage() As Decimal
        Get
            Return Me.dInsuredPercentage
        End Get
        Set(ByVal value As Decimal)
            Me.dInsuredPercentage = value
        End Set
    End Property

    '''<remarks/>
    Public Property InsuredTaxNumber() As String
        Get
            Return Me.sInsuredTaxNumber
        End Get
        Set(ByVal value As String)
            Me.sInsuredTaxNumber = value
        End Set
    End Property

    '''<remarks/>
    Public Property PayeeDomiciled() As Boolean
        Get
            Return Me.bPayeeDomiciled
        End Get
        Set(ByVal value As Boolean)
            Me.bPayeeDomiciled = value
        End Set
    End Property

    '''<remarks/>
    Public Property PayeePercentage() As Decimal
        Get
            Return Me.dPayeePercentage
        End Get
        Set(ByVal value As Decimal)
            Me.dPayeePercentage = value
        End Set
    End Property

    '''<remarks/>
    Public Property PayeeTaxNumber() As String
        Get
            Return Me.sPayeeTaxNumber
        End Get
        Set(ByVal value As String)
            Me.sPayeeTaxNumber = value
        End Set
    End Property

    '''<remarks/>
    Public Property SafeHarbourCode() As String
        Get
            Return Me.sSafeHarbourCode
        End Get
        Set(ByVal value As String)
            Me.sSafeHarbourCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property SafeHarbourPercentage() As Decimal
        Get
            Return Me.dSafeHarbourPercentage
        End Get
        Set(ByVal value As Decimal)
            Me.dSafeHarbourPercentage = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsWHTExempt() As Boolean
        Get
            Return Me.bIsWHTExempt
        End Get
        Set(ByVal value As Boolean)
            Me.bIsWHTExempt = value
        End Set
    End Property
End Class