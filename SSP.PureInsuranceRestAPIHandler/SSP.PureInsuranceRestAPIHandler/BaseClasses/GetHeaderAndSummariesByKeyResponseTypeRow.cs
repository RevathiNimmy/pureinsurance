namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetHeaderAndSummariesByKeyResponseTypeRow
    {

        //[Column("PartyKey")]
        public int PartyKey { get; set; }

        //[Column("IsLead")]
        public bool IsLead { get; set; }

        //[Column("Correspondence")]
        public bool Correspondence { get; set; }
        //[Column("shortname")]
        public string shortname { get; set; }
        //[Column("Blank1")]
        public string Blank1 { get; set; }
        //[Column("Blank2")]
        public string Blank2 { get; set; }
    }
}
