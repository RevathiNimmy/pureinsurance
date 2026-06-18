namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseFindReinsurerResponseTypeReinsurersRow
    {
        //[DBCol("AccountType")]
        public string AccountType { get; set; }
        //[DBCol("Address1")]
        public string Address1 { get; set; }
        //[DBCol("Address2")]
        public string Address2 { get; set; }
        //[DBCol("BranchCode")]
        public string BranchCode { get; set; }
        //[DBCol("BranchName")]
        public string BranchName { get; set; }
        //[DBCol("DefaultCommissionPercentage")]
        public double DefaultCommissionPercentage { get; set; }
        //[DBCol("IsBroker")]
        public bool IsBroker { get; set; }
        //[DBCol("IsBrokerSpecified")]
        public bool IsBrokerSpecified { get; set; }
        //[DBCol("IsDomiciledForTax")]
        public bool IsDomiciledForTax { get; set; }
        //[DBCol("IsDomiciledForTaxSpecified")]
        public bool IsDomiciledForTaxSpecified { get; set; }
        //[DBCol("IsRetained")]
        public bool IsRetained { get; set; }
        //[DBCol("IsRetainedSpecified")]
        public bool IsRetainedSpecified { get; set; }
        //[DBCol("IsTaxExempt")]
        public bool IsTaxExempt { get; set; }
        //[DBCol("IsTaxExemptSpecified")]
        public bool IsTaxExemptSpecified { get; set; }
        //[DBCol("PostalCode")]
        public string PostalCode { get; set; }
        //[DBCol("RIName")]
        public string RIName { get; set; }
        //[DBCol("ReinsuranceTypeCode")]
        public string ReinsuranceTypeCode { get; set; }
        //[DBCol("ReinsurerCode")]
        public string ReinsurerCode { get; set; }
        //[DBCol("ReinsurerKey")]
        public int ReinsurerKey { get; set; }
        //[DBCol("TaxGroupCode")]
        public string TaxGroupCode { get; set; }
        //[DBCol("TaxNumber")]
        public string TaxNumber { get; set; }
        //[DBCol("TaxPercentage")]
        public float TaxPercentage { get; set; }
        //[DBCol("TaxPercentageSpecified")]
        public bool TaxPercentageSpecified { get; set; }
    }
}
