namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetVersionsForClaimResponseTypeRow
    {
        //[DBCol("ClaimDescription")]
        public string ClaimDescription { get; set; }
        //[DBCol("ClaimKey")]
        public int ClaimKey { get; set; }
        //[DBCol("CurrentReserve")]
        public decimal CurrentReserve { get; set; }
        //[DBCol("InsuranceFileKey")]
        public int InsuranceFileKey { get; set; }
        //[DBCol("InsuranceFolderKey")]
        public int InsuranceFolderKey { get; set; }
        //[DBCol("InsuranceHolderShortName")]
        public string InsuranceHolderShortName { get; set; }
        //[DBCol("InsuranceRef")]
        public string InsuranceRef { get; set; }
        //[DBCol("LossCurrency")]
        public string LossCurrency { get; set; }
        //[DBCol("PolicyCurrency")]
        public string PolicyCurrency { get; set; }
        //[DBCol("RiskKey")]
        public int RiskKey { get; set; }
        //[DBCol("ThisPayment")]
        public decimal ThisPayment { get; set; }
        //[DBCol("ThisRevision")]
        public decimal ThisRevision { get; set; }
        //[DBCol("ThisSalvageRecovery")]
        public decimal ThisSalvageRecovery { get; set; }
        //[DBCol("ThisThirdPartyRecovery")]
        public decimal ThisThirdPartyRecovery { get; set; }
        //[DBCol("TotalIncurred")]
        public decimal TotalIncurred { get; set; }
        //[DBCol("TotalPaid")]
        public decimal TotalPaid { get; set; }
        //[DBCol("TransactionDate")]
        public System.DateTime TransactionDate { get; set; }
        //[DBCol("TransactionType")]
        public string TransactionType { get; set; }
        //[DBCol("User")]
        public string User { get; set; }
        //[DBCol("Version")]
        public int Version { get; set; }
        //[DBCol("VersionDescription")]
        public string VersionDescription { get; set; }
        //[DBCol("claim_number")]
        public string claim_number { get; set; }
        //[DBCol("client_short_name")]
        public string client_short_name { get; set; }
        //[DBCol("loss_from_date")]
        public System.DateTime loss_from_date { get; set; }
    }
}
