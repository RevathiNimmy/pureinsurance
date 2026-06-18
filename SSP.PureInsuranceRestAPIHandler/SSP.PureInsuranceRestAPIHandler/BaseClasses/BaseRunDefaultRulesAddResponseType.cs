namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseRunDefaultRulesAddResponseType : BaseResponseType
    {
        public int RiskTypeID { get; set; }

        public int BranchID { get; set; }

        public string XMLDataSet { get; set; }
    }
}
