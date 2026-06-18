using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class SavePremiumFinanceDetailsCommandBase : BaseRequestType
    {
        public decimal AmountToFinance { get; set; }
        public int ClaimRecoveryTransactionId { get; set; }
        public string ProcessPFMode { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNo { get; set; }
        public BaseAddressType BankAddress { get; set; }
        public string BankAreaCode { get; set; }
        public string BankBranch { get; set; }
        public string BankExtn { get; set; }
        public string BankFax { get; set; }
        public string BankFaxCode { get; set; }
        public string BankName { get; set; }
        public string BankPhone { get; set; }
        public string BankSortCode { get; set; }
        public BaseCreditCardType CreditCard { get; set; }
        public decimal Deposit { get; set; }
        public DateTime EndDate { get; set; }
        public int InsuranceFileKey { get; set; }
        public int MonthDay { get; set; }
        public decimal NetAmount { get; set; }
        public bool OverrideCommission { get; set; }
        public decimal OverrideDepositAmount { get; set; }
        public decimal OverrideInterestRate { get; set; }
        public decimal OverrideRate { get; set; }
        public int PFRF_ID { get; set; }
        public int PartyBankKey { get; set; }
        public bool PaymentProtection { get; set; }
        public DateTime PreferredDate { get; set; }
        public DateTime QuoteDate { get; set; }
        public int SchemeNo { get; set; }
        public int SchemeVersion { get; set; }
        public DateTime StartDate { get; set; }
        public int WeekDay { get; set; }
        public int DaysDelay { get; set; }
        public double TotalCost { get; set; }
        public DateTime FirstInstalmentDate { get; set; }
        public DateTime LastInstalmentDate { get; set; }
        public DateTime NextInstalmentDate { get; set; }
        public double FirstInstallment { get; set; }
        public double LastInstalment { get; set; }
        public double OtherInstallments { get; set; }
        public double CostOfProtection { get; set; }
        public int NoOfInstallments { get; set; }
        public double OriginalAmount { get; set; }
        public double FinanceFee { get; set; }
    }
}
