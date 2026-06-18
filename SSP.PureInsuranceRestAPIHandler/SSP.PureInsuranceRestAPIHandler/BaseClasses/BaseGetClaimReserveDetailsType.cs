namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimReserveDetailsType
    {
        public int ClaimPerilId { get; set; }
        public int TypeKey { get; set; }
        public int BaseReserveKey { get; set; }
        public string TypeCode { get; set; }
        public decimal SumInsured { get; set; }
        public decimal InitialReserve { get; set; }
        public decimal RevisedReserve { get; set; }
        public decimal PaidAmount { get; set; }
        public bool IsExcess { get; set; }
        public bool IsIndemnity { get; set; }
        public bool IsExpense { get; set; }
        public bool CanDelete { get; set; }
        public decimal ThisRevision { get; set; }
        public string TypeDescription { get; set; }

        public decimal GrossReserve { get; set; }

        public decimal Tax { get; set; }

        public decimal RevisedGrossReserve { get; set; }

        public decimal RevisedTaxReserve { get; set; }

        public decimal PaidToDateTax { get; set; }
    }

}