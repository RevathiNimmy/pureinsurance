using System.Collections.Generic;
using System.Data;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseAdditionalClaimRelatedDetails
    {
        public System.Collections.Generic.List<BaseTaxCalculationItemType> ListOfTaxCalculationItem { get; set; }
        public DataView TaxGroupTaxBandDetails { get; set; }
        public BaseClaimAdditionalDetailsType Claim { get; set; } = new BaseClaimAdditionalDetailsType();
        public BaseClientAdditionalDetailsType Client { get; set; } = new BaseClientAdditionalDetailsType();
        public BaseAgentAdditionalDetailsType Agent { get; set; }
        public BaseAgentAdditionalDetailsType TransferAgent { get; set; }
        public BaseClaimPerilAdditionalDetailsType ClaimPeril { get; set; } = new BaseClaimPerilAdditionalDetailsType();
        public BaseInsuranceFileAdditionalDetailsType InsuranceFile { get; set; } = new BaseInsuranceFileAdditionalDetailsType();
        public BaseProductAdditionalDetailsType Product { get; set; } = new BaseProductAdditionalDetailsType();
        public BaseRiskAdditionalDetailsType Risk { get; set; } = new BaseRiskAdditionalDetailsType();
        public BaseSourceAdditionalDetailsType Source { get; set; } = new BaseSourceAdditionalDetailsType();
    }

}
