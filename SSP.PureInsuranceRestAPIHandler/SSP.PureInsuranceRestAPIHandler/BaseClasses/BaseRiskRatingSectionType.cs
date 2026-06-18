using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseRiskRatingSectionType
    {
        public RowAction ActionType { get; set; }
        public string RatingSectionTypeCode { get; set; }

        public int RatingSectionTypeId { get; set; }

        public int SequenceNumber { get; set; }

        public string RateTypeCode { get; set; }

        public int RateTypeID { get; set; }

        public double AnnualRate { get; set; }

        public double SumInsured { get; set; }

        public bool SumInsuredSpecified { get; set; }

        public double AnnualPremium { get; set; }

        public bool AnnualPremiumSpecified { get; set; }

        public double ThisPremium { get; set; }

        public bool ThisPremiumSpecified { get; set; }

        public string CountryCode { get; set; }

        public int CountryID { get; set; }

        public string StateCode { get; set; }

        public int StateID { get; set; }

        public bool OriginalFlag { get; set; }
    }
}
