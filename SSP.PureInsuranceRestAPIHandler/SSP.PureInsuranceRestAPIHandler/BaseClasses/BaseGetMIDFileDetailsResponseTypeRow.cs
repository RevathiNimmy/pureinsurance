using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetMIDFileDetailsResponseTypeRow
    {
        public int BatchKey { get; set; }
        public string BatchRef { get; set; }
        public int ExpectedPPPC { get; set; }
        public int InsuranceFileKey { get; set; }
        public string InsuranceFileRef { get; set; }
        public int MidPolicyKey { get; set; }
        public string MidPolicyStatusCode { get; set; }
        public int PPPC { get; set; }
        public string RejectErrorCodes { get; set; }
        public string RejectReference { get; set; }
        public string StatusCode { get; set; }
        public string UpdateType { get; set; }
        public List<BaseGetMidFileDetailsResponseTypeRowRow> Vehicles { get; set; }
    }
}
