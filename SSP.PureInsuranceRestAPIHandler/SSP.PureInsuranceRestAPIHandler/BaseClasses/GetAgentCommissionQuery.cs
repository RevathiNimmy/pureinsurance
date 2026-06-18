using SSP.PureInsuranceRestAPIHandler.BaseClasses;

namespace PureInsurance.REST.Policy.Application.Policy.Queries.GetAgentCommission
{
    public class GetAgentCommissionQuery : GetAgentCommissionQueryBase// //GetAgentCommissionQueryResponse>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SortBy { get; set; }
    }
}
