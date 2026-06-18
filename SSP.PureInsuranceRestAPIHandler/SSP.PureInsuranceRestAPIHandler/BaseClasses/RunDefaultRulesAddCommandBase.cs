namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RunDefaultRulesAddCommandBase : BaseRequestType
    {

        public string ScreenCode { get; set; }


        public string XMLDataSet { get; set; }

        public string DataModelCode { get; set; }

        public bool SkipSaveToDB { get; set; }
    }
}
