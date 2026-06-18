using System;
using System.Collections.Generic;
using System.Text;
using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetFinancePlanDetailsResponseType : BaseResponseType
    {
        public double aprRate{ get; set; }
        public double adminCharge { get; set; }
        public string bic { get; set; }
        public string bankAccountName { get; set; }
        public string bankAccountNumber { get; set; }
        public string bankAccountType { get; set; }
        public string bankAddress1 { get; set; }
        public string bankBranchCode { get; set; }
        public string bankBranchName { get; set; }
        public string bankName { get; set; }
        public BaseCreditCardType creditCard { get; set; }
        public int dayOfWeekOrMonth { get; set; }
        public bool dayOfWeekOrMonthSpecified { get; set; }
        public double deposit { get; set; }
        public DateTime endDate { get; set; }
        public int financePlanKey { get; set; }
        public int financePlanVersion { get; set; }
        public double financedAmount { get; set; }
        public double firstInstalmentAmount { get; set; }
        public DateTime firstInstalmentDate { get; set; }
        public string frequency { get; set; }
        public string iban { get; set; }
        public List<BaseGetFinancePlanDetailsResponseTypeRow> instalments { get; set; }
        public double interestAmount { get; set; }
        public double interestRate { get; set; }
        public DateTime lastInstalmentDate { get; set; }
        public string mediaType { get; set; }
        public DateTime nextInstalmentDate { get; set; }
        public int noOfInstalments { get; set; }
        public double otherInstalmentAmount { get; set; }
        public int partyBankKey { get; set; }
        public string paymentMethod { get; set; }
        public string planReference { get; set; }
        public double protectionCharge { get; set; }
        public string schemeName { get; set; }
        public DateTime startDate { get; set; }
        public FinancePlanStatus status { get; set; }
        public string statusDescription { get; set; }
        public double taxes { get; set; }
        public double totalAmount { get; set; }
    }
}
