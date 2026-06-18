namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseTransactionType
    {

        public string AccountCode { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public int PartyKey { get; set; }
        public int PeriodID { get; set; }
        public bool PeriodIDSpecified { get; set; }
        public string Reference { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public bool TransactionDateSpecified { get; set; }
        public string UnderwritingYearCode { get; set; }
        public string Username { get; set; }

        public int UnderwritingYearId { get; set; }

        public int AccountId { get; set; }

        public string InsuranceRef { get; set; }
    }
}
