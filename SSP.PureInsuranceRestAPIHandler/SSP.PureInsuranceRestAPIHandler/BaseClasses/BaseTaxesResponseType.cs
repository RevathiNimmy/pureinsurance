namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseTaxesResponseType
    {
        public int TaxCalculationKey { get; set; } = 0;
        public int RiskKey { get; set; } = 0;
        public int TaxBandKey { get; set; } = 0;
        public decimal Premium { get; set; } = 0;
        public bool IsValue { get; set; } = false;
        public decimal TaxPercentage { get; set; } = 0;
        public decimal TaxValue { get; set; } = 0;
        public bool IsManuallyChanged { get; set; } = false;
        public int CalculationBasis { get; set; } = 0;
        public decimal BasisValue { get; set; } = 0;
        public decimal SumInsured { get; set; } = 0;
        public decimal SumInsuredRounded { get; set; } = 0;
        public int CurrencyKey { get; set; } = 0;
        public byte AllowTaxCredit { get; set; }
        public decimal OriginalSumInsured { get; set; } = 0;
        public int CountryKey { get; set; } = 0;
        public int StateKey { get; set; } = 0;
        public int ClassOfBusinessKey { get; set; } = 0;
        public int TaxGroupKey { get; set; } = 0;
        public int Sequence { get; set; } = 0;
        public int PolicyFeeUKey { get; set; } = 0;
        public int AgentCommissionKey { get; set; } = 0;
        public int RIPartyKey { get; set; } = 0;
        public int RIArrangementLineKey { get; set; } = 0;
        public int InsuranceSectionKey { get; set; } = 0;
        public int PolicyFeeKey { get; set; } = 0;
        public int PolicyAgentsKey { get; set; } = 0;
        public int InsurerPartyKey { get; set; } = 0;
        public int ClaimPerilKey { get; set; } = 0;
        public int ClaimPaymentKey { get; set; } = 0;
        public int ClaimReceiptKey { get; set; } = 0;
        public int ClaimPaymentItemKey { get; set; } = 0;
        public int ClaimReceiptItemKey { get; set; } = 0;
        public bool IsNotAppliedToClient { get; set; } = false;
        public bool IncludeTaxInInstalments { get; set; } = false;
        public bool SpreadTaxAcrossInstalments { get; set; } = false;
        public int BaseTaxCalculationKey { get; set; } = 0;
        public int VersionKey { get; set; } = 0;
        public int PfPremFinanceKey { get; set; } = 0;
        public int PfPremFinanceVersion { get; set; } = 0;
        public int PolicyCoinsurersSectionKey { get; set; } = 0;
        public bool IsCommissionTax { get; set; } = false;
        public int ApplyTaxBy { get; set; } = 0;
        public int TaxBandRateKey { get; set; } = 0;
        public bool IsSuspended { get; set; } = false;
        public string TransType { get; set; } = string.Empty;
        public string TaxBandCode { get; set; } = string.Empty;
        public string TaxBandDescription { get; set; } = string.Empty;
        public string TaxGroupCode { get; set; } = string.Empty;
        public string TaxGroupDescription { get; set; } = string.Empty;
        public string TaxBandRateDescription { get; set; } = string.Empty;
    }
}
