namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetTreatyPartyDetailsResponseTypePartiesRow
    {
        //[DBCol("CommissionPercent")]
        public float CommissionPercent { get; set; }
        //[DBCol("Description")]
        public string Description { get; set; }
        //[DBCol("IsDomiciledForTax")]
        public bool IsDomiciledForTax { get; set; }
        //[DBCol("IsReinsurerApproved")]
        public bool IsReinsurerApproved { get; set; }
        //[DBCol("PartyKey")]
        public int PartyKey { get; set; }
        //[DBCol("ResolvedName")]
        public string ResolvedName { get; set; }
        //[DBCol("SharePercent")]
        public float SharePercent { get; set; }
        //[DBCol("TaxGroupKey")]
        public int TaxGroupKey { get; set; }
        //[DBCol("TreatyKey")]
        public int TreatyKey { get; set; }
        //[DBCol("TreatyPartyKey")]
        public int TreatyPartyKey { get; set; }
    }
}
