using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetInvalidDiscountRisksQuery : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
    }

    public class GetInvalidDiscountRisksQueryResponse
    {
        public List<InvalidDiscountRisk> InvalidRisks { get; set; }
    }

    public class InvalidDiscountRisk
    {
        public int RiskKey { get; set; }
        public decimal TotalBilledPremium { get; set; }
        public decimal TotalReturnPremium { get; set; }
        public string RiskDescription { get; set; }
        public int RiskNumber { get; set; }
    }
}
