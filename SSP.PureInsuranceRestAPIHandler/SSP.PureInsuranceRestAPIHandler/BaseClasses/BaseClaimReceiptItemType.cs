using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimReceiptItemType
    {
        public long BaseRecoveryKey { get; set; }
        public bool IsTaxOverridden { get; set; }
        public decimal OverriddedTaxAmount { get; set; }
        public decimal ReceiptAmount { get; set; }

        public decimal TotalTaxAmount
        {
            get
            {
                decimal TotalTaxValue = 0m;

                if (TaxCalculationItem is null)
                {
                    return TotalTaxValue;
                }

                foreach (BaseTaxCalculationItemType oItem in TaxCalculationItem)
                    TotalTaxValue = TotalTaxValue + oItem.TaxValue;


                return TotalTaxValue;
            }
        }

        public int RecoveryId { get; set; }

        private string _recoveryType;
        public string RecoveryTypeCode
        {
            get
            {
                return _recoveryType;
            }
            set
            {
                _recoveryType = value;

                if (TaxCalculationItem is null)
                {
                    return;
                }

                foreach (BaseTaxCalculationItemType taxCalculationItem in TaxCalculationItem)
                    taxCalculationItem.RecoveryType = _recoveryType;
            }
        }

        public int RecoveryTypeId { get; set; }

        public int TaxGroupId { get; set; }
        public string TaxGroupCode { get; set; }
        public string RecoveryPartyCode { get; set; }

        public string TaxGroupAdvancedTaxScript { get; set; }

        public bool RecoveryPartyTypeCodeSpecified { get; set; }

        public int ClaimReceiptItemId { get; set; }

        public decimal NetAmount
        {
            get
            {
                decimal _NetAmount = 0m;
                if (IsTPSalvageExcludeTax)
                {
                    _NetAmount = ReceiptAmount;
                }
                else
                {
                    _NetAmount = ReceiptAmount - TotalTaxAmount;
                }
                return _NetAmount;
            }
        }

        public bool IsTPSalvageExcludeTax { get; set; }

        public System.Collections.Generic.List<BaseTaxCalculationItemType> TaxCalculationItem { get; set; }
    }
}
