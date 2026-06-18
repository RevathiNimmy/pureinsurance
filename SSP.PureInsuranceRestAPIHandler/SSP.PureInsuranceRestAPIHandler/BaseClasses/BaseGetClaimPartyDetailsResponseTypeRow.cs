namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimPartyDetailsResponseTypeRow
    {
        //[DBCol("Address1")]
        public string Address1 { get; set; }
        //[DBCol("Address2")]
        public string Address2 { get; set; }
        //[DBCol("Address3")]
        public string Address3 { get; set; }
        //[DBCol("Address4")]
        public string Address4 { get; set; }
        //[DBCol("AddressKey")]
        public int AddressKey { get; set; }
        //[DBCol("CountryKey")]
        public int CountryKey { get; set; }
        //[DBCol("EMail")]
        public string EMail { get; set; }
        //[DBCol("Fax")]
        public string Fax { get; set; }
        //[DBCol("Mobile")]
        public string Mobile { get; set; }
        //[DBCol("PartyKey")]
        public int PartyKey { get; set; }
        //[DBCol("PostalCode")]
        public string PostalCode { get; set; }
        //[DBCol("ResolvedName")]
        public string ResolvedName { get; set; }
        //[DBCol("ShortName")]
        public string ShortName { get; set; }
        //[DBCol("TelHome")]
        public string TelHome { get; set; }
        //[DBCol("TelOff")]
        public string TelOff { get; set; }
    }
}
