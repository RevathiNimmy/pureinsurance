
namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseAssociateType
    {
        public int ClientKey { get; set; }
        public int AssociateKey { get; set; }
        public string RelationshipCode { get; set; }
        public string RelationshipDescription { get; set; }
        public string AssociateCode { get; set; }
        public string AssociateName { get; set; }
        public decimal AccountBalance { get; set; }
        public decimal ClaimIncurred { get; set; }
        public string CurrencyCode { get; set; }
        public int ProcessingStatus { get; set; }
    }
}
