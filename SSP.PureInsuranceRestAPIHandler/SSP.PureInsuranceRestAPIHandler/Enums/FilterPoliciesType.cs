namespace SSP.PureInsuranceRestAPIHandler.Enums
{
    public enum FilterPoliciesType
    {
        All = 0,
        ExcludeLapsedPolicies = 1,
        LapsedPoliciesOnly = 2,
        ExcludeCancelledPolicies = 3,
        CancelledPoliciesOnly = 4,
        ExcludeCancelledandLapsedPolicies = 5
    }
}
