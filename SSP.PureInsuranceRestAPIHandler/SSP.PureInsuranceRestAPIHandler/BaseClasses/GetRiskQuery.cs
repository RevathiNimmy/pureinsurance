using SSP.PureInsuranceRestAPIHandler.BaseClasses;

namespace PureInsurance.REST.Policy.Application.Policy.Queries.GetRisk
{
    public class GetRiskQuery : GetRiskQueryBase// //GetRiskQueryResponse>
    {
        public int PolicyFeesPageNumber { get; set; }
        public int PolicyTaxesPageNumber { get; set; }
        public int RiskFeesPageNumber { get; set; }
        public int RiskTaxesPageNumber { get; set; }
        public int PolicyFeesPageSize { get; set; }
        public int PolicyTaxesPageSize { get; set; }
        public int RiskFeesPageSize { get; set; }
        public int RiskTaxesPageSize { get; set; }
        public string PolicyFeesSortyBy { get; set; }
        public string PolicyTaxesSortBy { get; set; }
        public string RiskTaxesSortBy { get; set; }
        public string RiskFeesSortBy { get; set; }
    }
}
