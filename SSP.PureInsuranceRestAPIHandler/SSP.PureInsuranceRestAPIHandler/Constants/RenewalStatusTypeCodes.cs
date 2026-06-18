namespace SSP.PureInsuranceRestAPIHandler.Constants
{
    public static class RenewalStatusTypeCodes
    {
        public const string AwaitingManualReview = "ManReview";
        public const string AwaitingRenewalNoticePrint = "AutoReview";
        public const string AwaitingManualRatingDueToFailure = "AutoFailed";
        public const string PolicyDetailsChanged = "PolicyChan";
        public const string AwaitingUpdate = "Update";
        public const string AwaitingManualRating = "ManRating";
        public const string AwaitingBrokerTransfer = "BROKERXFER";
        //'Start - (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.10)
        public const string AwaitingUpdateWritten = "Written";
        //'End -  (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.10)
    }
}
