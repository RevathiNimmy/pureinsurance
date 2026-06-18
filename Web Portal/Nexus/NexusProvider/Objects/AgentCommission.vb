<Serializable()> Public Class AgentCommission

    Private sAgentField As String

    Private sAgentTypeField As String

    Private sRiskTypeField As String

    Private sCommissionBandField As String

    Private dPremiumField As Double

    Private dCommissionRateField As Double

    Private dCommissionValueField As Double

    Private bIsLeadAgentField As Boolean

    Private sTaxGroupField As String

    Private dTaxValueField As Double

    Private dMaximumRateField As Double

    Private bIsValueField As Boolean

    Private sOverRideReasonField As String

    Private dCalculatedCommissionValueField As Decimal

    Private bCalculatedCommissionValueFieldSpecified As Boolean

    Private bIsAmendedField, bIsTaxAmended As Boolean

    Private sTaxGroupDescriptionField As String

    Private perilTypeField As Integer

    Public Sub New()

    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Agent : " & sAgentField & "<br />")
        sbPrint.AppendLine("Agent Type : " & sAgentTypeField & "<br />")
        sbPrint.AppendLine("Risk Type : " & sRiskTypeField & "<br />")
        sbPrint.AppendLine("Commission Band : " & sCommissionBandField & "<br />")
        sbPrint.AppendLine("Premium : " & dPremiumField.ToString() & "<br />")
        sbPrint.AppendLine("Commission Rate : " & dCommissionRateField.ToString() & "<br />")
        sbPrint.AppendLine("Commission Value : " & dCommissionValueField.ToString() & "<br />")
        sbPrint.AppendLine("Lead Agent : " & bIsLeadAgentField.ToString() & "<br />")
        sbPrint.AppendLine("Tax Group : " & sTaxGroupField & "<br />")
        sbPrint.AppendLine("Tax Value : " & dTaxValueField.ToString() & "<br />")
        sbPrint.AppendLine("Maximum Rate : " & dMaximumRateField.ToString() & "<br />")
        sbPrint.AppendLine("Value : " & bIsValueField.ToString() & "<br />")

        Return sbPrint.ToString()

    End Function

    Public Property Agent() As String
        Get
            Return Me.sAgentField
        End Get
        Set(ByVal value As String)
            Me.sAgentField = value
        End Set
    End Property

    Public Property AgentType() As String
        Get
            Return Me.sAgentTypeField
        End Get
        Set(ByVal value As String)
            Me.sAgentTypeField = value
        End Set
    End Property

    Public Property RiskType() As String
        Get
            Return Me.sRiskTypeField
        End Get
        Set(ByVal value As String)
            Me.sRiskTypeField = value
        End Set
    End Property

    Public Property CommissionBand() As String
        Get
            Return Me.sCommissionBandField
        End Get
        Set(ByVal value As String)
            Me.sCommissionBandField = value
        End Set
    End Property

    Public Property Premium() As Double
        Get
            Return Me.dPremiumField
        End Get
        Set(ByVal value As Double)
            Me.dPremiumField = value
        End Set
    End Property

    Public Property CommissionRate() As Double
        Get
            Return Me.dCommissionRateField
        End Get
        Set(ByVal value As Double)
            Me.dCommissionRateField = value
        End Set
    End Property

    Public Property CommissionValue() As Double
        Get
            Return Me.dCommissionValueField
        End Get
        Set(ByVal value As Double)
            Me.dCommissionValueField = value
        End Set
    End Property

    Public Property IsLeadAgent() As Boolean
        Get
            Return Me.bIsLeadAgentField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsLeadAgentField = value
        End Set
    End Property

    Public Property TaxGroup() As String
        Get
            Return Me.sTaxGroupField
        End Get
        Set(ByVal value As String)
            Me.sTaxGroupField = value
        End Set
    End Property
    Public Property TaxGroupDescription() As String
        Get
            Return Me.sTaxGroupDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sTaxGroupDescriptionField = value
        End Set
    End Property
    Public Property TaxValue() As Double
        Get
            Return Me.dTaxValueField
        End Get
        Set(ByVal value As Double)
            Me.dTaxValueField = value
        End Set
    End Property

    Public Property MaximumRate() As Double
        Get
            Return Me.dMaximumRateField
        End Get
        Set(ByVal value As Double)
            Me.dMaximumRateField = value
        End Set
    End Property


    Public Property IsValue() As Boolean
        Get
            Return Me.bIsValueField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsValueField = value
        End Set
    End Property

    Public Property OverRideReason() As String
        Get
            Return Me.sOverRideReasonField
        End Get
        Set(ByVal value As String)
            Me.sOverRideReasonField = value
        End Set
    End Property

    Public Property CalculatedCommissionValue() As Decimal
        Get
            Return Me.dCalculatedCommissionValueField
        End Get
        Set(ByVal value As Decimal)
            Me.dCalculatedCommissionValueField = value
        End Set
    End Property

    Public Property CalculatedCommissionValueSpecified() As Boolean
        Get
            Return Me.bCalculatedCommissionValueFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bCalculatedCommissionValueFieldSpecified = value
        End Set
    End Property

    Public Property IsAmended() As Boolean
        Get
            Return Me.bIsAmendedField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsAmendedField = value
        End Set
    End Property
    Public Property IsTaxAmended() As Boolean
        Get
            Return Me.bIsTaxAmended
        End Get
        Set(ByVal value As Boolean)
            Me.bIsTaxAmended = value
        End Set
    End Property

    Public Property PerilType() As Integer
        Get
            Return Me.perilTypeField
        End Get
        Set(ByVal value As Integer)
            Me.perilTypeField = value
        End Set
    End Property
End Class

<Serializable()> Public Class AgentCommissionCollection : Inherits CollectionBase
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oAgentCommission As AgentCommission In List
            sbPrint.AppendLine(oAgentCommission.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oAgentCommission As AgentCommission) As Integer
        Return List.Add(v_oAgentCommission)
    End Function

    Public Sub Remove(ByVal v_oAgentCommission As AgentCommission)
        List.Remove(v_oAgentCommission)
    End Sub

    Public Shadows Sub RemoveAt(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As AgentCommission
        Get
            Return List(i)
        End Get
        Set(ByVal value As AgentCommission)
            List(i) = value
        End Set
    End Property
    Public ReadOnly Property Find(ByVal agent As String) As AgentCommission
        Get
            Dim oAgentCommission As AgentCommission = Nothing
            For Each oList As AgentCommission In List
                If oList.Agent = agent Then
                    oAgentCommission = oList
                End If
            Next
            Return oAgentCommission
        End Get
    End Property
End Class

' WPR 64 - Commission Maintenance - Objects
<Serializable()> Public Class EditAgentCommission

    Private dLeadAgentTotalCommissionField As Double

    Private dLeadAgentTotalTaxField As Double

    Private dLeadAgentNetField As Double

    Private dSubAgentTotalCommissionField As Double

    Private dSubAgentTotalTaxField As Double

    Private dSubAgentNetField As Double

    Private oAgentCommissionField As AgentCommissionCollection

    Private iInsuranceFileKeyField As Integer

    Public Sub New()
        oAgentCommissionField = New AgentCommissionCollection()
    End Sub


    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Insurance File Key : " & iInsuranceFileKeyField.ToString() & "<br />")
        sbPrint.AppendLine("Lead Agent Total Commission : " & dLeadAgentTotalCommissionField.ToString() & "<br />")
        sbPrint.AppendLine("Lead Agent Total Tax : " & dLeadAgentTotalTaxField.ToString() & "<br />")
        sbPrint.AppendLine("Lead Agent Net : " & dLeadAgentNetField.ToString() & "<br />")
        sbPrint.AppendLine("Sub Agent Total Commission : " & dSubAgentTotalCommissionField.ToString() & "<br />")
        sbPrint.AppendLine("Sub Agent Total Tax : " & dSubAgentTotalTaxField.ToString() & "<br />")
        sbPrint.AppendLine("Sub Agent Net : " & dSubAgentNetField.ToString() & "<br />")
        sbPrint.AppendLine(oAgentCommissionField.Print() & "<br />")

        Return sbPrint.ToString()

    End Function

    Public Property InsuranceFileKey() As Integer
        Get
            Return Me.iInsuranceFileKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iInsuranceFileKeyField = value
        End Set
    End Property

    Public Property LeadAgentTotalCommission() As Double
        Get
            Return Me.dLeadAgentTotalCommissionField
        End Get
        Set(ByVal value As Double)
            Me.dLeadAgentTotalCommissionField = value
        End Set
    End Property


    Public Property LeadAgentTotalTax() As Double
        Get
            Return Me.dLeadAgentTotalTaxField
        End Get
        Set(ByVal value As Double)
            Me.dLeadAgentTotalTaxField = value
        End Set
    End Property

    Public Property LeadAgentNet() As Double
        Get
            Return Me.dLeadAgentNetField
        End Get
        Set(ByVal value As Double)
            Me.dLeadAgentNetField = value
        End Set
    End Property

    Public Property SubAgentTotalCommission() As Double
        Get
            Return Me.dSubAgentTotalCommissionField
        End Get
        Set(ByVal value As Double)
            Me.dSubAgentTotalCommissionField = value
        End Set
    End Property

    Public Property SubAgentTotalTax() As Double
        Get
            Return Me.dSubAgentTotalTaxField
        End Get
        Set(ByVal value As Double)
            Me.dSubAgentTotalTaxField = value
        End Set
    End Property

    Public Property SubAgentNet() As Double
        Get
            Return Me.dsubAgentNetField
        End Get
        Set(ByVal value As Double)
            Me.dsubAgentNetField = value
        End Set
    End Property

    Public Property AgentCommission() As AgentCommissionCollection
        Get
            Return Me.oAgentCommissionField
        End Get
        Set(ByVal value As AgentCommissionCollection)
            Me.oAgentCommissionField = value
        End Set
    End Property
End Class
