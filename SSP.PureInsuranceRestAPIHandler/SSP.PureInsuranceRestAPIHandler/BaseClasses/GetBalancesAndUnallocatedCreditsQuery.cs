namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetBalancesAndUnallocatedCredits
{
    public class GetBalancesAndUnallocatedCreditsQuery : GetBalancesAndUnallocatedCreditsQueryBase //GetBalancesAndUnallocatedCreditsQueryResponse>
    {
        public int UnallocatedCreditsForAgentsPageNumber { get; set; }
        public int UnallocatedCreditsForAgentsPageSize { get; set; }
        public int UnallocatedCreditsForClientsPageNumber { get; set; }
        public int UnallocatedCreditsForClientsPageSize { get; set; }
        public string UnallocatedCreditsForAgentsSortBy { get; set; }
        public string UnallocatedCreditsForClientsSortBy { get; set; }
    }
}
