using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetNumberingSchemeNoRequestType
    {
        public NumberingSchemeType SchemeType { get; set; }
        public int ProductKey { get; set; }
        public int AgentKey { get; set; }
        public int PartyKey { get; set; }
        public int ClaimKey { get; set; }
        public int SourceId { get; set; }

    }
}
