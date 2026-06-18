namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class IsPendingTransferQueryBaseResponse : BaseResponseType
    {
        public bool IsPendingCloneTransfer { get; set; }
        public bool IsPendingPortfolioTransfer { get; set; }
    }
}
