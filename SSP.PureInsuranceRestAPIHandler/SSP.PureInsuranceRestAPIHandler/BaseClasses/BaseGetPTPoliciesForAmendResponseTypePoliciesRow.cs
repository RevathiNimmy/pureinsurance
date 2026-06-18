namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetPTPoliciesForAmendResponseTypePoliciesRow
    {
        //[DBCol("BranchCode")]
        public string BranchCode { get; set; }

        //[DBCol("Branch Desc")]
        public string BranchName { get; set; }

        //[DBCol("insurance_file_cnt")]
        public int InsuranceFileKey { get; set; }

        //[DBCol("ins_file_PT_RI_usage_id")]
        public int InsuranceFilePTRIUsageId { get; set; }

        //[DBCol("status")]
        public int InsuranceFileStatus { get; set; }

        //[DBCol("insurance_folder_cnt")]
        public int InsuranceFolderKey { get; set; }

        //[DBCol("new_insurance_file_cnt")]
        public int NewInsuranceFileKey { get; set; }

        //[DBCol("PT RI Status Desc")]
        public string PTRIStatus { get; set; }

        //[DBCol("Party Name")]
        public string PartyName { get; set; }

        //[DBCol("Party Shortname")]
        public string PartyShortname { get; set; }

        //[DBCol("Policy Number")]
        public string PolicyNumber { get; set; }

        //[DBCol("ProductCode")]
        public string ProductCode { get; set; }

        //[DBCol("Product Desc")]
        public string ProductName { get; set; }

        //[DBCol("TransferDate")]
        public System.DateTime TransactionDate { get; set; }
    }
}
