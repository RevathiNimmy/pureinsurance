Imports Microsoft.Web.Services3.Security.Tokens
''' <summary>
''' Nexus BaseTax object, containing the common elements between the various perilclaim.
''' </summary>
''' <remarks></remarks>
''' 
<Serializable()> Public Class AllTaxes

    Private taxCalculationKey_Field As Integer
    Private insuranceFileKey_Field As Integer
    Private riskKey_Field As Integer
    Private taxBandKey_Field As Integer
    Private premium_field As Integer
    Private isValue_Field As Boolean
    Private isEdit_Field As Boolean
    Private taxPercentage_Field As Decimal
    Private taxValue_Field As Decimal
    Private isManuallyChanged_Field As Boolean
    Private calculationBasis_Field As Integer
    Private basisValue_Field As Decimal
    Private sumInsured_Field As Decimal
    Private sumInsuredRounded_Field As Decimal
    Private currencyKey_Field As Integer
    Private allowTaxCredit_Field As Byte
    Private originalSumInsured_Field As Decimal
    Private countryKey_Field As Integer
    Private stateKey_Field As Integer
    Private classOfBusinessKey_Field As Integer
    Private taxGroupKey_Field As Integer
    Private sequence_Field As Byte
    'Private transType_Field As TaxesType
    Private policyFeeUKey_Field As Integer
    Private agentCommissionKey_Field As Integer
    Private rIPartyKey_Field As Integer
    Private rIArrangementLineKey_Field As Integer
    Private insuranceSectionKey_Field As Integer
    Private policyFeeKey_Field As Integer
    Private policyAgentsKey_Field As Integer
    Private insurerPartyKey_Field As Integer
    Private claimPerilKey_Field As Integer
    Private claimPaymentKey_Field As Integer
    Private claimReceiptKey_Field As Integer
    Private claimPaymentItemKey_Field As Integer
    Private claimReceiptItemKey_Field As Integer
    Private isNotAppliedToClient_Field As Boolean
    Private includeTaxInInstalments_Field As Boolean
    Private spreadTaxAcrossInstalments_Field As Boolean
    Private baseTaxCalculationKey_Field As Integer
    Private versionKey_Field As Integer
    Private pfPremFinanceKey_Field As Integer
    Private pfPremFinanceVersion_Field As Integer
    Private policyCoinsurersSectionKey_Field As Integer
    Private isCommissionTax_Field As Boolean
    Private applyTaxBy_Field As Integer
    Private taxBandRateKey_Field As Integer
    Private isSuspended_Field As Boolean
    Private transType_Field As String
    Private taxBandCode_Field As String
    Private taxBandDescription_Field As String
    Private taxGroupCode_Field As String
    Private taxGroupDescription_Field As String
    Private taxBandRateDescription_Field As String
    Private oAllTaxesField As AllTaxesCollection


    Public Sub New()
        'initialize the collection
        oAllTaxesField = New AllTaxesCollection()
    End Sub
    Public Property TaxCalculationKey() As Integer
        Get
            Return Me.taxCalculationKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.taxCalculationKey_Field = value
        End Set
    End Property
    Public Property InsuranceFileKey() As Integer
        Get
            Return Me.insuranceFileKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.insuranceFileKey_Field = value
        End Set
    End Property

    Public Property RiskKey() As Integer
        Get
            Return Me.riskKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.riskKey_Field = value
        End Set
    End Property

    Public Property TaxBandKey() As Integer
        Get
            Return Me.taxBandKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.taxBandKey_Field = value
        End Set
    End Property
    Public Property Premium() As Integer
        Get
            Return Me.premium_field
        End Get
        Set(ByVal value As Integer)
            Me.premium_field = value
        End Set
    End Property

    Public Property IsValue() As Boolean
        Get
            Return Me.isValue_Field
        End Get
        Set(ByVal value As Boolean)
            Me.isValue_Field = value
        End Set
    End Property

    Public Property IsEdit() As Boolean
        Get
            Return Me.isEdit_Field
        End Get
        Set(ByVal value As Boolean)
            Me.isEdit_Field = value
        End Set
    End Property

    Public Property TaxPercentage() As Decimal
        Get
            Return Me.taxPercentage_Field
        End Get
        Set(ByVal value As Decimal)
            Me.taxPercentage_Field = value
        End Set
    End Property
    Public Property TaxValue() As Decimal
        Get
            Return Me.taxValue_Field
        End Get
        Set(ByVal value As Decimal)
            Me.taxValue_Field = value
        End Set
    End Property
    Public Property IsManuallyChanged() As Boolean
        Get
            Return Me.isManuallyChanged_Field
        End Get
        Set(ByVal value As Boolean)
            Me.isManuallyChanged_Field = value
        End Set
    End Property
    Public Property CalculationBasis() As Integer
        Get
            Return Me.calculationBasis_Field
        End Get
        Set(ByVal value As Integer)
            Me.calculationBasis_Field = value
        End Set
    End Property
    Public Property BasisValue() As Decimal
        Get
            Return Me.basisValue_Field
        End Get
        Set(ByVal value As Decimal)
            Me.basisValue_Field = value
        End Set
    End Property
    Public Property SumInsured() As Decimal
        Get
            Return Me.sumInsured_Field
        End Get
        Set(ByVal value As Decimal)
            Me.sumInsured_Field = value
        End Set
    End Property
    Public Property SumInsuredRounded() As Decimal
        Get
            Return Me.sumInsuredRounded_Field
        End Get
        Set(ByVal value As Decimal)
            Me.sumInsuredRounded_Field = value
        End Set
    End Property
    Public Property CurrencyKey() As Integer
        Get
            Return Me.currencyKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.currencyKey_Field = value
        End Set
    End Property
    Public Property AllowTaxCredit() As Byte
        Get
            Return Me.allowTaxCredit_Field
        End Get
        Set(ByVal value As Byte)
            Me.allowTaxCredit_Field = value
        End Set
    End Property
    Public Property OriginalSumInsured() As Decimal
        Get
            Return Me.originalSumInsured_Field
        End Get
        Set(ByVal value As Decimal)
            Me.originalSumInsured_Field = value
        End Set
    End Property
    Public Property CountryKey() As Integer
        Get
            Return Me.countryKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.countryKey_Field = value
        End Set
    End Property
    Public Property StateKey() As Integer
        Get
            Return Me.stateKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.stateKey_Field = value
        End Set
    End Property
    Public Property ClassOfBusinessKey() As Integer
        Get
            Return Me.classOfBusinessKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.classOfBusinessKey_Field = value
        End Set
    End Property
    Public Property TaxGroupKey() As Integer
        Get
            Return Me.taxGroupKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.taxGroupKey_Field = value
        End Set
    End Property
    Public Property Sequence() As Byte
        Get
            Return Me.sequence_Field
        End Get
        Set(ByVal value As Byte)
            Me.sequence_Field = value
        End Set
    End Property
    Public Property TransType() As String
        Get
            Return Me.transType_Field
        End Get
        Set(ByVal value As String)
            Me.transType_Field = value
        End Set
    End Property
    Public Property PolicyFeeUKey() As Integer
        Get
            Return Me.policyFeeUKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.policyFeeUKey_Field = value
        End Set
    End Property
    Public Property AgentCommissionKey() As Integer
        Get
            Return Me.agentCommissionKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.agentCommissionKey_Field = value
        End Set
    End Property
    Public Property RIPartyKey() As Integer
        Get
            Return Me.rIPartyKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.rIPartyKey_Field = value
        End Set
    End Property
    Public Property RIArrangementLineKey() As Integer
        Get
            Return Me.rIArrangementLineKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.rIArrangementLineKey_Field = value
        End Set
    End Property
    Public Property InsuranceSectionKey() As Integer
        Get
            Return Me.insuranceSectionKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.insuranceSectionKey_Field = value
        End Set
    End Property
    Public Property PolicyFeeKey() As Integer
        Get
            Return Me.policyFeeKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.policyFeeKey_Field = value
        End Set
    End Property
    Public Property PolicyAgentsKey() As Integer
        Get
            Return Me.policyAgentsKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.policyAgentsKey_Field = value
        End Set
    End Property
    Public Property InsurerPartyKey() As Integer
        Get
            Return Me.insurerPartyKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.insurerPartyKey_Field = value
        End Set
    End Property
    Public Property ClaimPerilKey() As Integer
        Get
            Return Me.claimPerilKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.claimPerilKey_Field = value
        End Set
    End Property

    Public Property ClaimPaymentKey() As Integer
        Get
            Return Me.claimPaymentKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.claimPaymentKey_Field = value
        End Set
    End Property
    Public Property ClaimReceiptKey() As Integer
        Get
            Return Me.claimReceiptKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.claimReceiptKey_Field = value
        End Set
    End Property

    Public Property ClaimPaymentItemKey() As Integer
        Get
            Return Me.claimPaymentItemKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.claimPaymentItemKey_Field = value
        End Set
    End Property
    Public Property IsNotAppliedToClient() As Boolean
        Get
            Return Me.isNotAppliedToClient_Field
        End Get
        Set(ByVal value As Boolean)
            Me.isNotAppliedToClient_Field = value
        End Set
    End Property
    Public Property IncludeTaxInInstalments() As Boolean
        Get
            Return Me.includeTaxInInstalments_Field
        End Get
        Set(ByVal value As Boolean)
            Me.includeTaxInInstalments_Field = value
        End Set
    End Property
    Public Property SpreadTaxAcrossInstalments() As Boolean
        Get
            Return Me.spreadTaxAcrossInstalments_Field
        End Get
        Set(ByVal value As Boolean)
            Me.spreadTaxAcrossInstalments_Field = value
        End Set
    End Property
    Public Property BaseTaxCalculationKey() As Integer
        Get
            Return Me.baseTaxCalculationKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.baseTaxCalculationKey_Field = value
        End Set
    End Property
    Public Property VersionKey() As Integer
        Get
            Return Me.versionKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.versionKey_Field = value
        End Set
    End Property

    Public Property PfPremFinanceKey() As Integer
        Get
            Return Me.pfPremFinanceKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.pfPremFinanceKey_Field = value
        End Set
    End Property

    Public Property PfPremFinanceVersion() As Integer
        Get
            Return Me.pfPremFinanceVersion_Field
        End Get
        Set(ByVal value As Integer)
            Me.pfPremFinanceVersion_Field = value
        End Set
    End Property
    Public Property PolicyCoinsurersSectionKey() As Integer
        Get
            Return Me.policyCoinsurersSectionKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.policyCoinsurersSectionKey_Field = value
        End Set
    End Property

    Public Property IsCommissionTax() As Boolean
        Get
            Return Me.isCommissionTax_Field
        End Get
        Set(ByVal value As Boolean)
            Me.isCommissionTax_Field = value
        End Set
    End Property

    Public Property ApplyTaxBy() As Integer
        Get
            Return Me.applyTaxBy_Field
        End Get
        Set(ByVal value As Integer)
            Me.applyTaxBy_Field = value
        End Set
    End Property
    Public Property TaxBandRateKey() As Integer
        Get
            Return Me.taxBandRateKey_Field
        End Get
        Set(ByVal value As Integer)
            Me.taxBandRateKey_Field = value
        End Set
    End Property
    Public Property IsSuspended() As Boolean
        Get
            Return Me.isSuspended_Field
        End Get
        Set(ByVal value As Boolean)
            Me.isSuspended_Field = value
        End Set
    End Property
    Public Property TaxBandCode() As String
        Get
            Return Me.taxBandCode_Field
        End Get
        Set(ByVal value As String)
            Me.taxBandCode_Field = value
        End Set
    End Property
    Public Property TaxBandDescription() As String
        Get
            Return Me.taxBandDescription_Field
        End Get
        Set(ByVal value As String)
            Me.taxBandDescription_Field = value
        End Set
    End Property

    Public Property TaxGroupCode() As String
        Get
            Return Me.taxGroupCode_Field
        End Get
        Set(ByVal value As String)
            Me.taxGroupCode_Field = value
        End Set
    End Property

    Public Property TaxGroupDescription() As String
        Get
            Return Me.taxGroupDescription_Field
        End Get
        Set(ByVal value As String)
            Me.taxGroupDescription_Field = value
        End Set
    End Property
    Public Property TaxBandRateDescription() As String
        Get
            Return Me.taxBandRateDescription_Field
        End Get
        Set(ByVal value As String)
            Me.taxBandRateDescription_Field = value
        End Set
    End Property

    Public Property AllTaxes() As AllTaxesCollection
        Get
            Return Me.oAllTaxesField
        End Get
        Set(ByVal value As AllTaxesCollection)
            Me.oAllTaxesField = value
        End Set
    End Property

End Class

<Serializable()> Public Class AllTaxesCollection : Inherits CollectionBase


    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        'For Each oDocument As Document In List
        '    sbPrint.AppendLine(oDocument.Print())
        '    sbPrint.AppendLine("<br />")
        'Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a AllTaxes object to the collection
    ''' </summary>
    Public Function Add(ByVal v_oAllTaxes As AllTaxes) As Integer
        Return List.Add(v_oAllTaxes)
    End Function

    ''' <summary>
    ''' Count a AllTaxes object to the collection
    ''' </summary>
    Public Shadows Function Count() As Integer
        Return List.Count
    End Function



    ''' <summary>
    ''' Remove an AllTaxes object from the collection
    ''' </summary>
    Public Sub Remove(ByVal v_oAllTaxes As AllTaxes)
        List.Remove(v_oAllTaxes)
    End Sub

    ''' <summary>
    ''' Remove an AllTaxes object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the ReceiptTaxGroup object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an AllTaxes object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the AllTaxes object</param>
    ''' <value>The replacement AllTaxes object</value>
    ''' <returns>The AllTaxes object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As AllTaxes
        Get
            Return List(i)
        End Get
        Set(ByVal value As AllTaxes)
            List(i) = value
        End Set
    End Property


End Class
