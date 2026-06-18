namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetHeaderAndPolicyFeesByKeyResponseTypeRow
    {
        //[DBCol("AppliedTo")]
        public string AppliedTo { get; set; }
        //[DBCol("CalculationBasis")]
        public int CalculationBasis { get; set; }
        //[DBCol("CurrencyCode")]
        public string CurrencyCode { get; set; }
        //[DBCol("DoPaymentTermsId")]
        public int DoPaymentTermsId { get; set; }
        //[DBCol("FeeAmount")]
        public double FeeAmount { get; set; }
        //[DBCol("FeeName")]
        public string FeeName { get; set; }
        //[DBCol("IncludeInInstallment")]
        public int IncludeInInstallment { get; set; }
        //[DBCol("IsValue")]
        public bool IsValue { get; set; }
        //[DBCol("MakeLiveOptionsId")]
        public int MakeLiveOptionsId { get; set; }
        //[DBCol("PolicyFeeKey")]
        public int PolicyFeeKey { get; set; }
        //[DBCol("Premium")]
        public double Premium { get; set; }
        //[DBCol("ProRataRate")]
        public double ProRataRate { get; set; }
        //[DBCol("Rate")]
        public double Rate { get; set; }
        //[DBCol("SpreadAcrossInstallment")]
        public int SpreadAcrossInstallment { get; set; }
        //[DBCol("TaxAmount")]
        public double TaxAmount { get; set; }
        //[DBCol("TaxGroup")]
        public string TaxGroup { get; set; }
        //[DBCol("TotalAmount")]
        public double TotalAmount { get; set; }
        //[DBCol("bIsProrated")]
        public int bIsProrated { get; set; }
    }
}
