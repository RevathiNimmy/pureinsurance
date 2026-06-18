using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BindQuoteCommand : BindQuoteCommandBase//, IRequest<BindQuoteCommandResponse>
    {
        public int SourceId { get; set; }
        public int TransactionTypeId { get; set; }
        public bool SelectedInstalmentQuoteSpecified { get; set; }
        public int SelectedSchemeNo { get; set; }
        public int SelectedSchemeVersion { get; set; }
        public BaseCreditCardType CreditCard { get; set; }
        public int PFRF_ID { get; set; }
        public int WeekDay { get; set; }
        public int MonthDay { get; set; }
        public System.DateTime PreferredDate { get; set; }
        public System.DateTime QuoteDate { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public double OverrideInterestRate { get; set; }
        public BaseAddressType BankAddress { get; set; }
        public BaseBindQuoteRequestDataStore DataStore { get; set; }
        public bool SkipPolicyLevelTaxesRecalculation { get; set; }
        public bool IsMarketPlacePolicy { get; set; }
        public decimal MarketPlaceTotalPremium { get; set; }
        public string BankAccountNo { get; set; }
        public double AmountToFinance { get; set; }
        public bool SkipNewPolicyNumber { get; set; }
        public bool IsUseTransactionCurrency { get; set; }
        public bool PaymentProtection { get; set; }
        public double InstDepositAmount { get; set; }
        public bool DayOfMonthIsValid { get; set; }
        public bool IsCallingFromSAM { get; set; }
        public string BankName { get; set; }
        public string BankSortCode { get; set; }
        public string BankAccountName { get; set; }
        public string BankBranch { get; set; }
        public string BankAreaCode { get; set; }
        public string BankPhone { get; set; }
        public string BankExtn { get; set; }
        public string BankFaxCode { get; set; }
        public string BankFax { get; set; }
        public string BIC { get; set; }
        public string IBAN { get; set; }
        public double OverrideRate { get; set; }
        public double AmountPaid { get; set; }
        public double? CurrencyBaseRate { get; set; }
        public DateTime? CurrencyBaseDate { get; set; }
        public double? AccountBaseRate { get; set; }
        public DateTime? AccountBaseDate { get; set; }
        public double? SystemBaseRate { get; set; }
        public DateTime? SystemBaseDate { get; set; }
        public int? RateOverrideReasonID { get; set; }
    }
}
