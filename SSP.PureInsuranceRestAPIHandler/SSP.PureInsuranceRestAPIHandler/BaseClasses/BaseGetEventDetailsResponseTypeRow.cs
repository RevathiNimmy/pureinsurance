namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetEventDetailsResponseTypeRow
    {

        //[DBCol("BGKey")]
        public int BGKey { get; set; }
        //[DBCol("CaseNumber")]
        public string CaseNumber { get; set; }
        //[DBCol("ClaimKey")]
        public int ClaimKey { get; set; }
        //[DBCol("ClaimNumber")]
        public string ClaimNumber { get; set; }
        //[DBCol("Description")]
        public string Description { get; set; }
        //[DBCol("DocumentKey")]
        public int DocumentKey { get; set; }
        //[DBCol("Document_Path")]
        public string Document_Path { get; set; }
        //[DBCol("EventDate")]
        public System.DateTime EventDate { get; set; }
        //[DBCol("EventDescription")]
        public string EventDescription { get; set; }
        //[DBCol("EventKey")]
        public int EventKey { get; set; }
        //[DBCol("EventNoteExist")]
        public string EventNoteExist { get; set; }
        //[DBCol("EventType")]
        public string EventType { get; set; }
        //[DBCol("EventTypeCode")]
        public string EventTypeCode { get; set; }
        //[DBCol("InsuranceFileKey")]
        public int InsuranceFileKey { get; set; }
        //[DBCol("InsuranceFolderKey")]
        public int InsuranceFolderKey { get; set; }
        //[DBCol("PolicyCode")]
        public string PolicyCode { get; set; }
        //[DBCol("Priority")]
        public string Priority { get; set; }
        //[DBCol("StatusKey")]
        public short StatusKey { get; set; }
        //[DBCol("TypeKey")]
        public int TypeKey { get; set; }
        //[DBCol("UserName")]
        public string UserName { get; set; }
    }
}
