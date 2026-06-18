namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseAddBackDatedMtaQuoteResponseTypeRow
    {
        //[DBCol("CoverEndDate")]
        public System.DateTime CoverEndDate { get; set; }
        //[DBCol("CoverStartDate")]
        public System.DateTime CoverStartDate { get; set; }
        public string CurrencyCode { get; set; }
        public int GISScreenID { get; set; }
        //[DBCol("InsuranceFileCnt")]
        public int InsuranceFileCnt { get; set; }
        public int InsuranceFolderCnt { get; set; }
        //[DBCol("MTACommission")]
        public double MTACommission { get; set; }
        //[DBCol("MTAFee")]
        public double MTAFee { get; set; }
        //[DBCol("MTAPremium")]
        public double MTAPremium { get; set; }
        //[DBCol("OriginalCommission")]
        public double OriginalCommission { get; set; }
        //[DBCol("OriginalFee")]
        public double OriginalFee { get; set; }
        //[DBCol("OriginalMTAPremium")]
        public double OriginalMTAPremium { get; set; }
        public int PartyCnt { get; set; }
        public string PartyShortname { get; set; }
        //[DBCol("PolicyStatus")]
        public string PolicyStatus { get; set; }
        //[DBCol("PolicyType")]
        public string PolicyType { get; set; }
        public int PolicyVersion { get; set; }
        public int ProductID { get; set; }
        //[DBCol("QuoteStatus")]
        public string QuoteStatus { get; set; }
        //[DBCol("ReversedInsuranceFileCnt")]
        public int ReversedInsuranceFileCnt { get; set; }
        public int RiskCnt { get; set; }
        public string RiskDescription { get; set; }
        public string RiskTypeDescription { get; set; }
        public int RiskTypeID { get; set; }
        public string Status { get; set; }
    }
}
