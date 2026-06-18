using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimReceiptTaxesQueryResponse : BaseResponseType
    {
        public List<BaseClaimReceiptTaxItemType> TaxItems { get; set; }
        public List<BaseClaimPerilRecoveryReceiptType> Recoveries { get; set; }
        public List<BaseGetClaimReceiptTaxesResponseTypeReceiptItems> ReceiptItems { get; set; }
        public BasePagedResponse PagedTaxItems { get; set; }
        public BasePagedResponse PagedRecoveries { get; set; }
        public BasePagedResponse PagedReceiptItems { get; set; }
        public decimal ReceiptToLossExchangeRate { get; set; }
    }
}
