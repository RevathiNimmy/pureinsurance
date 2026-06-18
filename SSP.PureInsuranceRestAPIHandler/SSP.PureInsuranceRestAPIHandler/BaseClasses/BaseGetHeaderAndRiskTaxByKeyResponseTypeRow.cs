namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetHeaderAndRiskTaxByKeyResponseTypeRow
    {
        //[DBCol(nameof(ApplyTaxBy))]
        public string ApplyTaxBy { get; set; }

        //[DBCol(nameof(CalculationBasis))]
        public string CalculationBasis { get; set; }

        //[DBCol(nameof(ClassOfBusiness))]
        public string ClassOfBusiness { get; set; }

        //[DBCol(nameof(Country))]
        public string Country { get; set; }

        //[DBCol(nameof(CurrencyCode))]
        public string CurrencyCode { get; set; }

        //[DBCol(nameof(IncludeInInstallment))]
        public bool IncludeInInstallment { get; set; }

        //[DBCol(nameof(IsNotAppliedToClient))]
        public bool IsNotAppliedToClient { get; set; }

        //[DBCol(nameof(IsValue))]
        public bool IsValue { get; set; }

        //[DBCol(nameof(Rate))]
        public double Rate { get; set; }

        //[DBCol(nameof(Sequence))]
        public int Sequence { get; set; }

        //[DBCol(nameof(SpreadAcrossInstallment))]
        public bool SpreadAcrossInstallment { get; set; }

        //[DBCol(nameof(State))]
        public string State { get; set; }

        //[DBCol(nameof(TaxAmount))]
        public double TaxAmount { get; set; }

        //[DBCol(nameof(TaxBand))]
        public string TaxBand { get; set; }

        //[DBCol(nameof(TaxGroup))]
        public string TaxGroup { get; set; }
    }
}
