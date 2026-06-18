
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetFinancePlanDetailsQueryBaseResponse : BasePagedResponse
    {
        public double APRRate { get; set; }
        public double AdminCharge { get; set; }
        public string BIC { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountType { get; set; }
        public string BankAddress1 { get; set; }
        public string BankBranchCode { get; set; }
        public string BankBranchName { get; set; }
        public string BankName { get; set; }
        public BaseCreditCardType CreditCard { get; set; }
        public int DayOfWeekOrMonth { get; set; }
        public bool DayOfWeekOrMonthSpecified { get; set; }
        public double Deposit { get; set; }
        public DateTime EndDate { get; set; }
        public int FinacePlanKey { get; set; }
        public int FinacePlanVersion { get; set; }
        public double FinancedAmount { get; set; }
        public double FirstInstalmentAmount { get; set; }
        public DateTime FirstInstalmentDate { get; set; }
        public string Frequency { get; set; }
        public string IBAN { get; set; }
        public List<BaseGetFinancePlanDetailsResponseTypeRow> Instalments { get; set; }
        public double InterestAmount { get; set; }
        public double InterestRate { get; set; }
        public DateTime LastInstalmentDate { get; set; }
        public string MediaType { get; set; }
        public DateTime NextInstalmentDate { get; set; }
        public int NoOfInstalments { get; set; }
        public double OtherInstalmentAmount { get; set; }
        public int PartyBankKey { get; set; }
        public string PaymentMethod { get; set; }
        public string PlanReference { get; set; }
        public double ProtectionCharge { get; set; }
        public string SchemeName { get; set; }
        public DateTime StartDate { get; set; }
        public SSP.PureInsuranceRestAPIHandler.Enums.FinancePlanStatus? Status { get; set; }
        public string StatusDescription { get; set; }
        public double Taxes { get; set; }
        public double TotalAmount { get; set; }
    }
}
