using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateQuoteV2CommandBaseResponse : BaseResponseType
    {

        public int BaseInsuranceFolderKey { get; set; }
        public string InsuranceFileRef { get; set; }
        public System.DateTime QuoteExpiryDate { get; set; }
        public QuoteStatusType QuoteStatusKey { get; set; }
        public int QuoteVersion { get; set; }
        public byte[] ApiTimeStamp { get; set; }
    }
}
