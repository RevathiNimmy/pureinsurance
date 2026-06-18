namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseFindCaseResponseTypeCaseDetailsRow
    {
        //[DBCol("Analyst")]
        public string Analyst { get; set; }

        //[DBCol("Assistant")]
        public string Assistant { get; set; }

        //[DBCol("BaseCaseKey")]
        public int BaseCaseKey { get; set; }

        //[DBCol("CaseKey")]
        public int CaseKey { get; set; }

        //[DBCol("CaseNumber")]
        public string CaseNumber { get; set; }

        //[DBCol("CaseOpenDate")]
        public System.DateTime CaseOpenDate { get; set; }

        //[DBCol("CaseProgressDescription")]
        public string CaseProgressDescription { get; set; }

        //[DBCol("CurrencyCode")]
        public string CurrencyCode { get; set; }

        //[DBCol("TotalExcess")]
        public decimal TotalExcess { get; set; }

        //[DBCol("TotalExpense")]
        public decimal TotalExpense { get; set; }

        //[DBCol("TotalIndemnity")]
        public decimal TotalIndemnity { get; set; }
    }
}
