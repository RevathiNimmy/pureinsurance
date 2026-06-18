namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseFindBankResponseTypeRow
    {
        //[DBCol("bank_address1")]
        public string BankAddress { get; set; }
        //[DBCol("bank_id")]
        public int BankKey { get; set; }
        //[DBCol("bank_name")]
        public string BankName { get; set; }
        //[DBCol("branch_code")]
        public string BranchCode { get; set; }
        //[DBCol("code")]
        public string Code { get; set; }
        //[DBCol("bank_name")]
        public string HeadOffice { get; set; }
    }
}
