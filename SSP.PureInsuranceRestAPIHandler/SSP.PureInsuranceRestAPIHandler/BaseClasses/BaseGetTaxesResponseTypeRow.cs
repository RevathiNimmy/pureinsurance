using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetTaxesResponseTypeRow
    {
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }

        //[DBCol("agent_commission_cnt")]
        public int AgentCommissionKey { get; set; }

        //[DBCol("allow_tax_credit")]
        public Byte AllowTaxCredit { get; set; }

        //[DBCol("is_commission_tax")]
        public int ApplyTaxBy { get; set; }

        //[DBCol("base_tax_calculation_cnt")]
        public int BaseTaxCalculationKey { get; set; }

        //[DBCol("Basis_Value")]
        public decimal BasisValue { get; set; }

        //[DBCol("Calc_Basis")]
        public int CalculationBasis { get; set; }

        //[DBCol("claim_payment_item_id")]
        public int ClaimPaymentItemKey { get; set; }

        //[DBCol("claim_payment_id")]
        public int ClaimPaymentKey { get; set; }

        //[DBCol("claim_peril_id")]
        public int ClaimPerilKey { get; set; }

        //[DBCol("claim_receipt_item_id")]
        public int ClaimReceiptItemKey { get; set; }

        //[DBCol("claim_receipt_id")]
        public int ClaimReceiptKey { get; set; }

        //[DBCol("class_of_business_id")]
        public int ClassOfBusinessKey { get; set; }

        //[DBCol("country_id")]
        public int CountryKey { get; set; }

        //[DBCol("currency_id")]
        public int CurrencyKey { get; set; }

        //[DBCol("include_tax_in_instalments")]
        public bool IncludeTaxInInstalments { get; set; }

        //[DBCol("insurance_section_id")]
        public int InsuranceSectionKey { get; set; }

        //[DBCol("insurer_party_cnt")]
        public int InsurerPartyKey { get; set; }

        //[DBCol("is_commission_tax")]
        public bool IsCommissionTax { get; set; }

        //[DBCol("is_manually_changed")]
        public bool IsManuallyChanged { get; set; }

        //[DBCol("is_not_applied_to_client")]
        public bool IsNotAppliedToClient { get; set; }

        //[DBCol("is_suspended")]
        public bool IsSuspended { get; set; }

        //[DBCol("is_value")]
        public bool IsValue { get; set; }

        //[DBCol("original_sum_insured")]
        public decimal OriginalSumInsured { get; set; }

        //[DBCol("pfprem_finance_cnt")]
        public int PfPremFinanceKey { get; set; }

        //[DBCol("pfprem_finance_version")]
        public int PfPremFinanceVersion { get; set; }

        //[DBCol("policy_agents_id")]
        public int PolicyAgentsKey { get; set; }

        //[DBCol("policy_coinsurers_section_id")]
        public int PolicyCoinsurersSectionKey { get; set; }

        //[DBCol("policy_fee_id")]
        public int PolicyFeeKey { get; set; }

        //[DBCol("policy_fee_u_id")]
        public int PolicyFeeUKey { get; set; }

        //[DBCol("premium")]
        public decimal Premium { get; set; }

        //[DBCol("ri_arrangement_line_id")]
        public int RIArrangementLineKey { get; set; }

        //[DBCol("ri_party_cnt")]
        public int RIPartyKey { get; set; }

        //[DBCol("risk_cnt")]
        public int RiskKey { get; set; }

        //[DBCol("Sum_Insured_Rounded")]
        public int Sequence { get; set; }

        //[DBCol("Sum_Insured_Rounded")]
        public bool SpreadTaxAcrossInstalments { get; set; }

        //[DBCol("Sum_Insured_Rounded")]
        public int StateKey { get; set; }

        //[DBCol("Sum_Insured_Rounded")]
        public decimal SumInsured { get; set; }

        //[DBCol("Sum_Insured_Rounded")]
        public decimal SumInsuredRounded { get; set; }

        //[DBCol("taxbandcode")]
        public string TaxBandCode { get; set; }

        //[DBCol("taxbandDescription")]
        public string TaxBandDescription { get; set; }

        //[DBCol("tax_band_id")]
        public int TaxBandKey { get; set; }

        //[DBCol("TaxBandRateDescription")]
        public string TaxBandRateDescription { get; set; }

        //[DBCol("tax_band_rate_id")]
        public int TaxBandRateKey { get; set; }

        //[DBCol("tax_calculation_cnt")]
        public int TaxCalculationKey { get; set; }

        //[DBCol("taxgroupcode")]
        public string TaxGroupCode { get; set; }

        //[DBCol("taxgroupDescription")]
        public string TaxGroupDescription { get; set; }

        //[DBCol("tax_group_id")]
        public int TaxGroupKey { get; set; }

        //[DBCol("percentage")]
        public decimal TaxPercentage { get; set; }

        //[DBCol("value")]
        public decimal TaxValue { get; set; }

        //[DBCol("transtype")]
        public string TransType { get; set; }

        //[DBCol("version_id")]
        public int VersionKey { get; set; }
    }
}
