using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimPerilSummaryQueryResponse : BaseResponseType
    {
        public List<BaseGetClaimPerilSummaryResponseTypeRow> PerilTotals { get; set; }
        public List<BaseGetClaimPerilSummaryResponseTypeReserveType> ReserveType { get; set; }
        public List<BaseGetClaimPerilSummaryResponseTypeRow2> SalvageRecoveryPerils { get; set; }
        public List<BaseGetClaimPerilSummaryResponseTypeRow1> TPRecoveryPerils { get; set; }
        public BasePagedResponse PagedPerilTotals { get; set; }
        public BasePagedResponse PagedReserveType { get; set; }
        public BasePagedResponse PagedSalvageRecoveryPerils { get; set; }
        public BasePagedResponse PagedTPRecoveryPerils { get; set; }
    }
}
