using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseReceiptCashListItemType : BaseCoreCashListItemType
    {
        public AllocationType AllocationType { get; set; }
        public bool AllocationTypeSpecified { get; set; }
        public BaseBankReceiptType Bank { get; set; }
        public BaseCreditCardType CreditCard { get; set; }
        public System.Collections.Generic.List<BaseInstalmentPlanDetailsType> InstalmentPlanDetails { get; set; }
        public System.Collections.Generic.List<BaseReceiptCashListItemTypePolicies> Policies { get; set; }
        public string StatusCode { get; set; }

        public string TypeCode { get; set; }
        public int TypeKey { get; set; }
        public int StatusKey { get; set; }
        public int BankKey { get; set; }
        public BasePagedResponse PagedInstalmentPlanDetails { get; set; }
        public BasePagedResponse PagedPolicies { get; set; }
    }
}
