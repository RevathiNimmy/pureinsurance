using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetPolicyTransactionDetailsResponseType
    {
        public string DocumentReference { get; set; }
        public string DocumentType { get; set; }
        public System.DateTime EffectiveDate { get; set; }
        public System.Collections.Generic.List<BaseGetPolicyTransactionDetailsResponseTypeExtended> Extended { get; set; }
    }
}
