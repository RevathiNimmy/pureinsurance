namespace SSP.PureInsuranceRestAPIHandler.Constants
{
    public static class TransactionTypeCode
    {
        public const string MaintainClaim = "C_CR";
        public const string OpenClaim = "C_CO";
        public const string PayClaim = "C_CP";
        public const string NewBusiness = "NB";
        public const string SalvageRecovery = "C_SA";
        public const string ThirdPartyRecovery = "C_RV";
        public const string CancelPolicy = "MTC";
        public const string EditPolicy = "EDIT";
        public const string MTA = "MTA";
        public const string Renewals = "REN";
        public const string PremiumFinanceCash = "PFCASH";
        public const string PremiumFinanceMidTermAdjustment = "PFMTA";
        public const string PremiumFinanceNewBusiness = "PFNB";
        public const string PremiumFinanceRenewal = "PFREN";
        public const string ReinstatePolicy = "MTR";
        public const string DeferredReinsurance = "DRI";
        public const string PortfolioTransfer = "PT";
    }
}
