namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseBindQuoteRequestTypeCreditTransactionsRow
    {
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }
        public int AccountKey { get; set; }
        public double Amount { get; set; }
        public System.DateTime CollectionDate { get; set; }
        public int TransDetailKey { get; set; }
    }
}
