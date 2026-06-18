using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseQuoteRiskMsgType : BaseQuoteType
    {
        public string AccountExecutiveShortname { get; set; }
        public string AccountHandlerShortname { get; set; }
        public string AlternateReference { get; set; }
        public double AmountToFinance { get; set; }
        public bool AmountToFinanceSpecified { get; set; }
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
        public System.Collections.Generic.List<BaseQuoteRiskMsgTypeClaim> Claim { get; set; }
        public string CoInsurancePlacement { get; set; }
        public int DayOfWeekOrMonth { get; set; }
        public bool DayOfWeekOrMonthSpecified { get; set; }
        public int FinanceCompanyNo { get; set; }
        public bool FinanceCompanyNoSpecified { get; set; }
        public System.DateTime FinanceEndDate { get; set; }
        public bool FinanceEndDateSpecified { get; set; }
        public System.DateTime FinancePreferredDate { get; set; }
        public bool FinancePreferredDateSpecified { get; set; }
        public System.DateTime FinanceQuoteDate { get; set; }
        public bool FinanceQuoteDateSpecified { get; set; }
        public int FinanceSchemeNo { get; set; }
        public bool FinanceSchemeNoSpecified { get; set; }
        public int FinanceSchemeVersion { get; set; }
        public bool FinanceSchemeVersionSpecified { get; set; }
        public System.DateTime FinanceStartDate { get; set; }
        public bool FinanceStartDateSpecified { get; set; }
        public System.DateTime InceptionDate { get; set; }
        public bool InceptionDateSpecified { get; set; }
        public System.DateTime InceptionDateTPI { get; set; }
        public bool InceptionDateTPISpecified { get; set; }
        public System.DateTime LapsedDate { get; set; }
        public bool LapsedDateSpecified { get; set; }
        public string LapsedReasonCode { get; set; }
        public string LapsedReasonDescription { get; set; }
        public string OldPolicyNumber { get; set; }
        public bool PaymentProtection { get; set; }
        public bool PaymentProtectionSpecified { get; set; }
        public string PolicyStatusCode { get; set; }
        public int PolicyVersion { get; set; }
        public string PolicyVersionTypeCode { get; set; }
        public System.DateTime RenewalDate { get; set; }
        public bool RenewalDateSpecified { get; set; }
        public BaseTaxesType[] Taxes { get; set; }
        public System.Collections.Generic.List<BaseQuoteRiskMsgTypeRisks> Risks { get; set; }
        public int SAMStagingPolicyKey { get; set; }
        public PolicyProcessTypes PolicyProcessType { get; set; }
        public string TransactionTypeCode { get; set; }
        public string NewQuoteRef { get; set; }
        public double CommissionRate { get; set; }
        public double CommissionValue { get; set; }
        public System.DateTime TransactionDueDate { get; set; }
        public string LastTransDescription { get; set; }
        public string BusinessTypeCode { get; set; }
        public string MTAReasonCode { get; set; }
        public bool IsMarketPlacePolicy { get; set; }
        public double OverrideNetPremium { get; set; }
        public int PartyKey { get; set; }
        public int SourceId { get; set; }
        public bool SkipGenerateRenewalPolicyNumber { get; set; }
        public int InsuranceFolderKey { get; set; }
        public int DeletePolicyUnderRenewal { get; set; }
        public string RenewalStatusCode { get; set; }
        public bool DoNotCopyRiskAtRenSelection { get; set; }
        public bool IsBDXRequest { get; set; }
        public System.DateTime AnniversaryDate { get; set; }
        public bool AnniversaryDateSpecified { get; set; }
        public string CollectionFrequencyCode { get; set; }
        public string PaymentTermCode { get; set; }
        public System.Collections.Generic.List<BaseUpdateCoinsuranceValuesRequestTypeRow> CoInsurers { get; set; }
    }
}
