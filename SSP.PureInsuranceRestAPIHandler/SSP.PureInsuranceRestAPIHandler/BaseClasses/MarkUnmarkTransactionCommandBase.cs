using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.MarkUnmarkTransaction
{
    public class MarkUnmarkTransactionCommandBase : BaseRequestType
    {

        public string CurrencyCode { get; set; }
        public MarkStatusType MarkStatus { get; set; }
        public decimal PaymentAmount { get; set; }

        //(1, int.MaxValue, ErrorMessage = "The TransactionKey field is required")]

        public int TransactionKey { get; set; }
    }
}
