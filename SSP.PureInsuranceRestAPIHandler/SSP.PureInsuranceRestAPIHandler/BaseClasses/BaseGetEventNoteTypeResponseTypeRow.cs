namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetEventNoteTypeResponseTypeRow
    {
        //[DBCol("EventTypeCode")]
        public string EventTypeCode { get; set; }
        //[DBCol("EventTypeDescription")]
        public string EventTypeDescription { get; set; }
        //[DBCol("EventTypeKey")]
        public int EventTypeKey { get; set; }
    }
}
