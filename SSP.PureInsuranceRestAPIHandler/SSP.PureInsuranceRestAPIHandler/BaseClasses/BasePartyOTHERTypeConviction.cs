namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePartyOtherTypeConviction
    {
        public int ConvictionKey { get; set; }
        public string TypeCode { get; set; } = string.Empty;
        public string StatusCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public System.DateTime Date { get; set; }
        public decimal FineAmount { get; set; }
        public bool FineAmountSpecified { get; set; }
        public string SentenceTypeCode { get; set; } = string.Empty;
        public string SentenceDescription { get; set; } = string.Empty;
        public decimal SentenceDuration { get; set; }
        public bool SentenceDurationSpecified { get; set; }
        public string SentenceDurationQualifier { get; set; } = string.Empty;
        public System.DateTime SentenceEffectiveDate { get; set; }
        public bool SentenceEffectiveDateSpecified { get; set; }
        public decimal AlcoholLevel { get; set; }
        public bool AlcoholLevelSpecified { get; set; }
        public string AlcoholMeasurementMethod { get; set; } = string.Empty;
        public decimal DrivingLicencePenaltyPoints { get; set; }
        public bool DrivingLicencePenaltyPointsSpecified { get; set; }
        public int ProcessingStatus { get; set; }
    }
}
