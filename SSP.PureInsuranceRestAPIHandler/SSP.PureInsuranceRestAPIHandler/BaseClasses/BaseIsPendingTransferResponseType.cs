namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseIsPendingTransferResponseType : BaseResponseType
    {
        public bool IsPendingPortfolioTransfer { get; set; } = false;
        public bool IsPendingCloneTransfer { get; set; } = false;
    }

}
