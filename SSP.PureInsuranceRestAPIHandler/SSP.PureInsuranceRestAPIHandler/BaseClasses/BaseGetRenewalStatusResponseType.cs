namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetRenewalStatusResponseType
    {
        //[DBCol("renewal_status_type_code")]
        public string RenewalStatusTypeCode { get; set; }

        //[DBCol("renewal_status_type_description")]
        public string RenewalStatusTypeDescription { get; set; }

        //[DBCol("renewal_status_cnt")]
        public int RenewalStatusKey { get; set; }

        //[DBCol("insurance_holder_cnt")]
        public int InsuranceHolderKey { get; set; }

        //[DBCol("lead_agent_cnt")]
        public int LeadAgentKey { get; set; }

        //[DBCol("date_created")]
        public System.DateTime DateCreated { get; set; }

        //[DBCol("critical_date")]
        public System.DateTime CriticalDate { get; set; }

        //[DBCol("is_invite_printed")]
        public int IsInvitePrinted { get; set; }

        //[DBCol("insurance_file_cnt")]
        public int OriginalInsuranceFileKey { get; set; }

        //[DBCol("date_invite_printed")]
        public System.DateTime DateInvitePrinted { get; set; }

        //[DBCol("renewal_exception_notes")]
        public string RenewalExceptionNotes { get; set; }

        //[DBCol("email_sent")]
        public string EmailSent { get; set; }

        //[DBCol("email_sent_date")]
        public System.DateTime EmailSentDate { get; set; }

        //[DBCol("product_code")]
        public string ProductCode { get; set; }

        //[DBCol("renewal_exception_reason_code")]
        public string RenewalExceptionReasonCode { get; set; }

        //[DBCol("renewal_exception_reason_description")]
        public string RenewalExceptionReasonDescription { get; set; }

        public bool IsDuplicateRenewalExists { get; set; }
    }

}
