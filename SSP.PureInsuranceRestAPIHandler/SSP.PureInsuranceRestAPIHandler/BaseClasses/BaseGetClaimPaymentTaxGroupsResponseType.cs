using System.Collections.Generic;
namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimPaymentTaxGroupsResponseType : BasePagedResponse
    {
        public System.Collections.Generic.List<BaseGetClaimPaymentTaxGroupsResponseTypeRow> TaxGroup { get; set; }
    }
}
