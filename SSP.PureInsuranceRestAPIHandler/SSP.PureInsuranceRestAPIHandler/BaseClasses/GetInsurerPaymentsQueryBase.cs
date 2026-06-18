using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetInsurerPayments
{
    public class GetInsurerPaymentsQueryBase : BaseRequestType
    {
        
        public int AccountKey { get; set; }
        public string AlternateReference { get; set; }
        public string CurrencyCode { get; set; }
        public InsurerPaymentsDateByType DateByTransaction { get; set; }
        public System.DateTime DateFrom { get; set; }
        public System.DateTime DateTo { get; set; }
        public bool GrossAgent { get; set; }
        public bool ExcludePendingAuth { get; set; }
        public bool OnlyPendingAuth { get; set; }
        public string InsurerPaymentBranchCode { get; set; }
        public InsurerPaymentsMarkedStatus? MarkedStatus { get; set; }
        public string MediaType { get; set; }
        public Month? Month { get; set; }
        public string PeriodName { get; set; }
        public string PolicyNumber { get; set; }
        public string Reference { get; set; }
        public string YearName { get; set; }
    }
}
