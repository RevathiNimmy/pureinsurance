namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetRiskReinsuranceArrangementLinesResponseTypeRow
    {
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }
        //[DBCol("agreement_code")]
        public string Agreement { get; set; }
        //[DBCol("commission_value")]
        public decimal Commission { get; set; }
        public double CommissionPerc { get; set; }
        //[DBCol("commission_tax")]
        public decimal CommissionTax { get; set; }
        //[DBCol("DefaultLine")]
        public int DefaultLine { get; set; }
        public double DefaultPerc { get; set; }
        //[DBCol("is_obligatory")]
        public bool IsObligatory { get; set; }
        //[DBCol("ri_name")]
        public string Name { get; set; }
        //[DBCol("party_cnt")]
        public int PartyKey { get; set; }
        //[DBCol("premium_value")]
        public decimal Premium { get; set; }
        //[DBCol("ri_arrangement_line_id")]
        public int RIArrangementLineKey { get; set; }
        //[DBCol("sum_insured")]
        public decimal SumInsured { get; set; }
        //[DBCol("premium_tax")]
        public decimal Tax { get; set; }
        public double ThisPerc { get; set; }
        //[DBCol("treatycode")]
        public string TreatyCode { get; set; }
        //[DBCol("type")]
        public string Type { get; set; }
    }
}
