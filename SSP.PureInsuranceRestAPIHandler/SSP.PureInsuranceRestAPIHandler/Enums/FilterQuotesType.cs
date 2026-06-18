namespace SSP.PureInsuranceRestAPIHandler.Enums
{
    public enum FilterQuotesType
    {
        All = 0,
        ExcludeExpiredQuotes = 1,
        ExpiredQuotesOnly = 2,
        ExcludeCancelledQuotes = 3,
        CancelledQuotesOnly = 4,
        ExcludeCancelledandExpiredQuotes = 5,
        NBQuotesOnly = 6,
        MTAQuotesOnly = 7,
        RenewalQuotesOnly = 8
    }
}
