namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.FindBank
{
    public class FindBankCommandBase : BaseRequestType
    {
        public string BankName { get; set; }
        public int MaxRowsToFetch { get; set; }
        public bool MaxRowsToFetchSpecified { get; set; }
        public string ShortCode { get; set; }
    }
}
