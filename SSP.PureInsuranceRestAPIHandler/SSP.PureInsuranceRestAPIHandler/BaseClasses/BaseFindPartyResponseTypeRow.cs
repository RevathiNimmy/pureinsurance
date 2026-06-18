namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseFindPartyResponseTypeRow
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int AgentKey { get; set; }
        public bool AgentKeySpecified { get; set; }
        public string AgentType { get; set; }
        public string AllowConsolidatedCommission { get; set; }
        public string ClientCode { get; set; }
        public string ContactTelephoneNumber { get; set; }
        public string CountryCode { get; set; }
        public System.DateTime DateCancelled { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public bool DateOfBirthSpecified { get; set; }
        public string FileCode { get; set; }
        public int IsProspect { get; set; }
        public int IsRIBroker { get; set; }
        public string Name { get; set; }
        public int PartyKey { get; set; }
        public string PartySourceDescription { get; set; }
        public string PartySourceId { get; set; }
        public int PartyTypeId { get; set; }
        public string PostCode { get; set; }
        public string ReinsuranceType { get; set; }
        public string ResolvedName { get; set; }
        public string ServiceLevelCode { get; set; }
        public string ServiceLevelDescription { get; set; }
        public string ShortName { get; set; }
        public string Status { get; set; }
        public string SwiftLink { get; set; }
        public string Type { get; set; }
    }
}
