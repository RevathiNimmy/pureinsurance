using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateRatingSectionsCommandBase : BaseRequestType
    {

        ////(1, int.MaxValue, ErrorMessage = "The InsuranceFileKey field is required")]
        //
        public int InsuranceFileKey { get; set; }
        public System.Collections.Generic.List<BaseUpdateRatingDetailsRequestTypeRatingDetailsRow> RatingDetails { get; set; }

        ////(1, int.MaxValue, ErrorMessage = "The RiskKey field is required")]
        //
        public int RiskKey { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
        public string TransactionType { get; set; }
    }
}
