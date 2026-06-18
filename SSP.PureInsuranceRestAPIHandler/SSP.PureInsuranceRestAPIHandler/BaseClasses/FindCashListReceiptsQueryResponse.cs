using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindCashListReceiptsQueryResponse : BasePagedResponse
    {
        public System.Collections.Generic.List<BaseFindCashListReceiptsResponseTypeRow> CashListItems { get; set; }
    }
}
