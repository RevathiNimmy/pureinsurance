namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetBankAccounts
{
    public class GetBankAccountsQueryBaseResponse : BasePagedResponse
    {
        public System.Collections.Generic.List<BaseGetBankAccountsResponseTypeRow> BankAccounts { get; set; }
    }
}
