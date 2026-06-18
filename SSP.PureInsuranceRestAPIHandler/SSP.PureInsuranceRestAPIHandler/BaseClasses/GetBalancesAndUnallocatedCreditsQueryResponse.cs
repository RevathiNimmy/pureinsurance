using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetBalancesAndUnallocatedCredits
{
    public class GetBalancesAndUnallocatedCreditsQueryResponse
    {
        public GetBalancesAndUnallocatedCreditsQueryBaseResponse GetBalancesAndUnallocatedCreditsResponse { get; set; }
        public Uri UnallocatedCreditsForAgentsFirstPage { get; set; }
        public Uri UnallocatedCreditsForAgentsLastPage { get; set; }
        public Uri UnallocatedCreditsForAgentsPreviousPage { get; set; }
        public Uri UnallocatedCreditsForAgentsNextPage { get; set; }
        public int UnallocatedCreditsForAgentsPageNumber { get; set; }
        public int UnallocatedCreditsForAgentsPageSize { get; set; }
        public int UnallocatedCreditsForAgentsTotalPages { get; set; }
        public int UnallocatedCreditsForAgentsTotalRecords { get; set; }
        public Uri UnallocatedCreditsForClientsFirstPage { get; set; }
        public Uri UnallocatedCreditsForClientsLastPage { get; set; }
        public Uri UnallocatedCreditsForClientsPreviousPage { get; set; }
        public Uri UnallocatedCreditsForClientsNextPage { get; set; }
        public int UnallocatedCreditsForClientsPageNumber { get; set; }
        public int UnallocatedCreditsForClientsPageSize { get; set; }
        public int UnallocatedCreditsForClientsTotalPages { get; set; }
        public int UnallocatedCreditsForClientsTotalRecords { get; set; }
    }
}
