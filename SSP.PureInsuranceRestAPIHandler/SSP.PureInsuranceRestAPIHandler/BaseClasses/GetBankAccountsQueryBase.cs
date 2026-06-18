namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetBankAccounts
{
    public class GetBankAccountsQueryBase : BaseRequestType
    {
        public int BankAccountKey { get; set; }
        public bool BankAccountKeySpecified { get; set; }
    }
}
