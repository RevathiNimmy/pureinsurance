<Serializable()> Public Class HeaderAndRisk : Inherits HeaderAndRiskType
    Private iInsuranceFileKey As Integer

    Private sClientCode As String

    Private sInsuranceFileRef As String

    Private sAgent As String

    Private dInceptionDate As Date

    Private dCoverStartDate As Date

    Private dExpiryDate As Date

    Private sCurrency As String

    Private dNetTotal As Double

    Private dTaxTotal As Double

    Private dFeeTotal As Double

    Private dGrossTotal As Double

    Private dTotalRiskFeesEligibleForFinancing As Double

    Private dTotalRiskFeesExcludedFromFinancing As Double

    Private dTotalRiskFees As Double

    Private dTotalPolicyFeesEligibleForFinancing As Double

    Private dTotalPolicyFeesExcludedFromFinancing As Double
    Private sAppliedTo As String

    Private dTotalPolicyFees As Double
    Private oPolicyFees As HeaderAndPolicyFeesType
    Private oRisksTax As HeaderAndRiskTaxType
    Private oRisks As HeaderAndRiskType
    Private oRiskFees As FeeCollection

    Public Sub New()
        oRisks = New HeaderAndRiskType
        oRisksTax = New HeaderAndRiskTaxType
        oPolicyFees = New HeaderAndPolicyFeesType
        oRiskFees = New FeeCollection
    End Sub
    Public Property AppliedTo() As String
        Get
            Return Me.sAppliedTo
        End Get
        Set(ByVal value As String)
            Me.sAppliedTo = value
        End Set
    End Property
    Public Property RiskFees() As FeeCollection
        Get
            Return Me.oRiskFees
        End Get
        Set(ByVal value As FeeCollection)
            Me.oRiskFees = value
        End Set
    End Property
    '''<remarks/>
    Public Property InsuranceFileKey() As Integer
        Get
            Return Me.iInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            Me.iInsuranceFileKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClientCode() As String
        Get
            Return Me.sClientCode
        End Get
        Set(ByVal value As String)
            Me.sClientCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property InsuranceFileRef() As String
        Get
            Return Me.sInsuranceFileRef
        End Get
        Set(ByVal value As String)
            Me.sInsuranceFileRef = value
        End Set
    End Property

    '''<remarks/>
    Public Property Agent() As String
        Get
            Return Me.sAgent
        End Get
        Set(ByVal value As String)
            Me.sAgent = value
        End Set
    End Property

    '''<remarks/>
    Public Property InceptionDate() As Date
        Get
            Return Me.dInceptionDate
        End Get
        Set(ByVal value As Date)
            Me.dInceptionDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property CoverStartDate() As Date
        Get
            Return Me.dCoverStartDate
        End Get
        Set(ByVal value As Date)
            Me.dCoverStartDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property ExpiryDate() As Date
        Get
            Return Me.dExpiryDate
        End Get
        Set(ByVal value As Date)
            Me.dExpiryDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property Currency() As String
        Get
            Return Me.sCurrency
        End Get
        Set(ByVal value As String)
            Me.sCurrency = value
        End Set
    End Property

    '''<remarks/>
    Public Property NetTotal() As Double
        Get
            Return Me.dNetTotal
        End Get
        Set(ByVal value As Double)
            Me.dNetTotal = value
        End Set
    End Property

    '''<remarks/>
    Public Property TaxTotal() As Double
        Get
            Return Me.dTaxTotal
        End Get
        Set(ByVal value As Double)
            Me.dTaxTotal = value
        End Set
    End Property

    '''<remarks/>
    Public Property FeeTotal() As Double
        Get
            Return Me.dFeeTotal
        End Get
        Set(ByVal value As Double)
            Me.dFeeTotal = value
        End Set
    End Property

    '''<remarks/>
    Public Property GrossTotal() As Double
        Get
            Return Me.dGrossTotal
        End Get
        Set(ByVal value As Double)
            Me.dGrossTotal = value
        End Set
    End Property

    '''<remarks/>

    Public Property Risks() As HeaderAndRiskType
        Get
            Return Me.oRisks
        End Get
        Set(ByVal value As HeaderAndRiskType)
            Me.oRisks = value
        End Set
    End Property

    '''<remarks/>
    Public Property TotalRiskFeesEligibleForFinancing() As Double
        Get
            Return Me.dTotalRiskFeesEligibleForFinancing
        End Get
        Set(ByVal value As Double)
            Me.dTotalRiskFeesEligibleForFinancing = value
        End Set
    End Property

    '''<remarks/>
    Public Property TotalRiskFeesExcludedFromFinancing() As Double
        Get
            Return Me.dTotalRiskFeesExcludedFromFinancing
        End Get
        Set(ByVal value As Double)
            Me.dTotalRiskFeesExcludedFromFinancing = value
        End Set
    End Property

    '''<remarks/>
    Public Property TotalRiskFees() As Double
        Get
            Return Me.dTotalRiskFees
        End Get
        Set(ByVal value As Double)
            Me.dTotalRiskFees = value
        End Set
    End Property

    '''<remarks/>
    Public Property TotalPolicyFeesEligibleForFinancing() As Double
        Get
            Return Me.dTotalPolicyFeesEligibleForFinancing
        End Get
        Set(ByVal value As Double)
            Me.dTotalPolicyFeesEligibleForFinancing = value
        End Set
    End Property

    '''<remarks/>
    Public Property TotalPolicyFeesExcludedFromFinancing() As Double
        Get
            Return Me.dTotalPolicyFeesExcludedFromFinancing
        End Get
        Set(ByVal value As Double)
            Me.dTotalPolicyFeesExcludedFromFinancing = value
        End Set
    End Property

    '''<remarks/>
    Public Property TotalPolicyFees() As Double
        Get
            Return Me.dTotalPolicyFees
        End Get
        Set(ByVal value As Double)
            Me.dTotalPolicyFees = value
        End Set
    End Property
    '''<remarks/>

    Public Property RisksTax() As HeaderAndRiskTaxType
        Get
            Return Me.oRisksTax
        End Get
        Set(ByVal value As HeaderAndRiskTaxType)
            Me.oRisksTax = value
        End Set
    End Property
    '''<remarks/>

    Public Property PolicyFees() As HeaderAndPolicyFeesType
        Get
            Return Me.oPolicyFees
        End Get
        Set(ByVal value As HeaderAndPolicyFeesType)
            Me.oPolicyFees = value
        End Set
    End Property

End Class
