namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseConvictionType
    {
        public int ConvictionKey { get; set; }
        public string TypeCode { get; set; }
        public string StatusCode { get; set; }
        public string Description { get; set; }
        public System.DateTime Date { get; set; }
        public decimal FineAmount { get; set; }
        public bool FineAmountSpecified { get; set; }
        public string SentenceTypeCode { get; set; }
        public string SentenceDescription { get; set; }
        public decimal SentenceDuration { get; set; }
        public bool SentenceDurationSpecified { get; set; }
        public string SentenceDurationQualifier { get; set; }
        public System.DateTime SentenceEffectiveDate { get; set; }
        public bool SentenceEffectiveDateSpecified { get; set; }
        public decimal AlcoholLevel { get; set; }
        public bool AlcoholLevelSpecified { get; set; }
        public string AlcoholMeasurementMethod { get; set; }
        public decimal DrivingLicensePenaltyPoints { get; set; }
        public bool DrivingLicensePenaltyPointsSpecified { get; set; }
        public int ProcessingStatus { get; set; }
    }
}
