namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetRiskByProductResponseTypeRow
    {
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }

        //[DBCol("DataModel")]
        public string DataModelCode { get; set; }

        //[DBCol("caption")]
        public string Description { get; set; }

        //[DBCol("code")]
        public string RiskTypeCode { get; set; }

        //[DBCol("risk_type_id")]
        public int RiskTypeKey { get; set; }

        //[DBCol("Screen")]
        public string ScreenCode { get; set; }

        //[DBCol("gis_screen_id")]
        public int ScreenKey { get; set; }
    }
}
