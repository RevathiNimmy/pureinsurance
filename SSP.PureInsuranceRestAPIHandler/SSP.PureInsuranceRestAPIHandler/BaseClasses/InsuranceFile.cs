namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class InsuranceFile
    {
        //[DBCol("insurance_file_type_id")]
        public int InsuranceFileTypeId { get; set; }

        //[DBCol("insurance_file_type_code")]
        public string InsuranceFileTypeCode { get; set; }

        //[DBCol("policy_version")]
        public int PolicyVersion { get; set; }

        //[DBCol("out_of_sequence_replaced")]
        public int OutOfSequenceReplaced { get; set; }
        public int OriginalInsuranceFileKey { get; set; }
        public int PFPremFinanceCnt { get; set; }
        public int PFPremFinanceVersion { get; set; }
        public string InsuranceFileStatus { get; set; }
        public int PFPartyBankKey { get; set; }
        public int InsuranceFileKey { get; set; }
    }
}
