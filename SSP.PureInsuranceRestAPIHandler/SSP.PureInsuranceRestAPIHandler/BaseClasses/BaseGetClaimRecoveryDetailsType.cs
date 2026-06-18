using System.Xml.Serialization;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimRecoveryDetailsType
    {
        public int ClaimPerilId { get; set; }
        public int TypeKey { get; set; }
        public int BaseRecoveryKey { get; set; }
        public string TypeCode { get; set; }
        public string CurrencyCode { get; set; }
        public decimal InitialRecovery { get; set; }
        public decimal RevisedRecovery { get; set; }
        public decimal ReceiptedAmount { get; set; }
        public decimal ReceiptedTaxAmount { get; set; }
        public int IsSalvage { get; set; }
        public string RecoveryPartyTypeCode { get; set; }
        public string RecoveryPartyCode { get; set; }
        public int RecoveryPartyTypeKey { get; set; }
        [XmlIgnore()]
        public bool RecoveryPartyTypeKeySpecified { get; set; }
        public int RecoveryPartyKey { get; set; }
        [XmlIgnore()]
        public bool RecoveryPartyKeySpecified { get; set; }
        public bool CanDelete { get; set; }
        public string PartyShortName { get; set; }
    }

}