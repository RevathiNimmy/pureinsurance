using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;
namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetInstalmentQuotesQueryBase : BaseRequestType
    {

        public int InsuranceFileKey { get; set; }
        public System.DateTime EndDate { get; set; }
        public decimal AmountToFinance { get; set; }
        public bool InstalmentTypeSpecified { get; set; }
        public InstalmentType InstalmentType { get; set; }
        public double OverrideDepositAmount { get; set; }
        public double OverrideInterestRate { get; set; }
        public double OverrideRate { get; set; }
        public int MonthDay { get; set; }
        public System.DateTime PreferredDate { get; set; }
        public System.DateTime QuoteDate { get; set; }
        public System.DateTime StartDate { get; set; }
        public bool PaymentProtection { get; set; }
        public int WeekDay { get; set; }
        public string ProcessPFMode { get; set; }
        public int PFPremFinanceKey { get; set; }
        public int PFPremFinanceVersion { get; set; }
        public bool OverrideCommission { get; set; }
        public bool PreferredInstalmentDueDateonly { get; set; }
        public bool IsUseTransactionCurrency { get; set; }
        public List<BasePremiumFinancePlanTransactionsType> PFTransaction { get; set; }
    }
}
