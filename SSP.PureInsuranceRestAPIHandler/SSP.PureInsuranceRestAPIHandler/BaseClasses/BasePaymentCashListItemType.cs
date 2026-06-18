
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePaymentCashListItemType : BaseCoreCashListItemType
    {
        public string StatusCode { get; set; }
        public string TypeCode { get; set; }
        public int UserId { get; set; }
        public int StatusKey { get; set; }
        public int TypeKey { get; set; }
        public string UserName { get; set; }
        public List<int> MarkedTransKeys { get; set; }
        public BaseBankPaymentType Bank { get; set; }
        public BaseCreditCardType CreditCard { get; set; }
        public System.Collections.Generic.List<BasePaymentCashListItemTypePolicies> Policies { get; set; }
    }
}
