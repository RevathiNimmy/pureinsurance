namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetBalancesAndUnallocatedCreditsResponseTypeRow
    {
        //[DBCol("AccountKey")]
        public int AccountKey { get; set; }
        //[DBCol("Amount")]
        public double Amount { get; set; }
        //[DBCol("CollectionDate")]
        public System.DateTime CollectionDate { get; set; }
        //[DBCol("")]
        public bool CollectionDateSpecified { get; set; }
        //[DBCol("DocumentRef")]
        public string DocumentRef { get; set; }
        //[DBCol("MediaType")]
        public string MediaType { get; set; }
        //[DBCol("Reference")]
        public string Reference { get; set; }
        //[DBCol("TransDetailKey")]
        public int TransDetailKey { get; set; }
    }
}
