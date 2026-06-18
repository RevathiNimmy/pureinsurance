namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetListOfManualJournalTransactionMasterResponse
    {
        //[DBCol("Branch")]
        public string Branch { get; set; }
        //[DBCol("Comment")]
        public string Comment { get; set; }
        //[DBCol("CreatedDate")]
        public System.DateTime CreatedDate { get; set; }
        //[DBCol("DocumentType")]
        public string DocumentType { get; set; }
        //[DBCol("IsReferred")]
        public int IsReferred { get; set; }
        //[DBCol("ReverseDate")]
        public System.DateTime ReversesOn { get; set; }
        //[DBCol("RecurringOccurs")]
        public int RecurringOccurs { get; set; }
        //[DBCol("PerPeriodOnDay")]
        public int PerPeriodOnDay { get; set; }
        //[DBCol("PerMonthOnDay")]
        public int PerMonthOnDay { get; set; }
        //[DBCol("PerQuarterOnDay")]
        public int PerQuarterOnDay { get; set; }
        //[DBCol("AuthorisationComment")]
        public string AuthorisationComment { get; set; }
    }
}
