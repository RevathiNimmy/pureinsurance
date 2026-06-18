namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetAllocationDetailsResponseTypeRow
    {
        //[DBCol("Account")]
        public string Account { get; set; }
        //[DBCol("AllocatedAmount")]
        public double AllocatedAmount { get; set; }
        //[DBCol("AllocatedDate")]
        public System.DateTime AllocatedDate { get; set; }
        //[DBCol("AllocationBatchKey")]
        public int AllocationBatchKey { get; set; }
        //[DBCol("AllocationDetailKey")]
        public int AllocationDetailKey { get; set; }
        //[DBCol("AllocationKey")]
        public int AllocationKey { get; set; }
        //[DBCol("CurrencyCode")]
        public string CurrencyCode { get; set; }
        //[DBCol("DocRef")]
        public string DocRef { get; set; }
        //[DBCol("DocType")]
        public string DocType { get; set; }
        //[DBCol("InsuranceRef")]
        public string InsuranceRef { get; set; }
        //[DBCol("IsReversed")]
        public bool IsReversed { get; set; }
        //[DBCol("OriginalAmount")]
        public double OriginalAmount { get; set; }
        //[DBCol("Spare")]
        public string Spare { get; set; }
        //[DBCol("TransDate")]
        public System.DateTime TransDate { get; set; }
        //[DBCol("TransDetailKey")]
        public int TransDetailKey { get; set; }
        //[DBCol("TransdetailExtendedKey")]
        public int TransdetailExtendedKey { get; set; }
        //[DBCol("User")]
        public string User { get; set; }
        //[DBCol("WriteOffAmount")]
        public double WriteOffAmount { get; set; }
    }
}
