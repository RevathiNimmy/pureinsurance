namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetProductRiskEventsResponseTypeRow
    {
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }

        //[DBCol("EventCode")]
        public string EventCode { get; set; }

        //[DBCol("EventDescription")]
        public string EventDescription { get; set; }

        //[DBCol("EventKey")]
        public int EventKey { get; set; }

        //[DBCol("IsDefault")]
        public bool IsDefault { get; set; }
    }
}
