using Newtonsoft.Json;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class ApplyDiscountCommand : BaseRequestType
    {
        [JsonProperty("branchCode")]
        public new string BranchCode { get => base.BranchCode; set => base.BranchCode = value; }
        [JsonProperty("loginUserName")]
        public new string LoginUserName { get => base.LoginUserName; set => base.LoginUserName = value; }
        [JsonProperty("insuranceFileKey")]
        public int InsuranceFileKey { get; set; }
        [JsonProperty("productId")]
        public int ProductId { get; set; }
        [JsonProperty("transactionType")]
        public string TransactionType { get; set; }
        [JsonProperty("task")]
        public int Task { get; set; }
        [JsonProperty("appliedDiscountPremium")]
        public decimal AppliedDiscountPremium { get; set; }
        [JsonProperty("appliedDiscountPercentage")]
        public double AppliedDiscountPercentage { get; set; }
        [JsonProperty("appliedMatchDiscountPremium")]
        public int AppliedMatchDiscountPremium { get; set; }
        [JsonProperty("appliedDiscountReasonId")]
        public int AppliedDiscountReasonId { get; set; }
        [JsonProperty("appliedDiscountRecurringTypeId")]
        public int AppliedDiscountRecurringTypeId { get; set; }
    }
}
