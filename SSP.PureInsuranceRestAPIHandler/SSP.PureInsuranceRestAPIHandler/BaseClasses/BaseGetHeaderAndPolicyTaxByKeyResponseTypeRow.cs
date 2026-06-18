namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetHeaderAndPolicyTaxByKeyResponseTypeRow
    {
        //[DBCol("ApplyTaxBy")]
        public string ApplyTaxBy { get; set; }

        //[DBCol("CalculationBasis")]
        public string CalculationBasis { get; set; }

        //[DBCol("ClassOfBusiness")]
        public string ClassOfBusiness { get; set; }

        //[DBCol("Country")]
        public string Country { get; set; }

        //[DBCol("IncludeInInstallment")]
        public bool IncludeInInstallment { get; set; }

        //[DBCol("IsNotAppliedToClient")]
        public bool IsNotAppliedToClient { get; set; }

        //[DBCol("IsValue")]
        public bool IsValue { get; set; }

        //[DBCol("Rate")]
        public double Rate { get; set; }

        //[DBCol("Sequence")]
        public int Sequence { get; set; }

        //[DBCol("SpreadAcrossInstallment")]
        public bool SpreadAcrossInstallment { get; set; }

        //[DBCol("State")]
        public string State { get; set; }

        //[DBCol("TaxAmount")]
        public double TaxAmount { get; set; }

        //[DBCol("TaxBand")]
        public string TaxBand { get; set; }

        //[DBCol("TaxGroup")]
        public string TaxGroup { get; set; }
    }
}
