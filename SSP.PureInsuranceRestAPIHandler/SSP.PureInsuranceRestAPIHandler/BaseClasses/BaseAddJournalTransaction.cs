using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseAddJournalTransaction
    {
        public string AccountCode { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
        public string AltReference { get; set; }
        public string Comment { get; set; }
        public string UnderwritingYear { get; set; }
        public string CostCentreCode { get; set; }
        public string InsuranceRef { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string PurchaseInvoiceNumber { get; set; }
        public int ManualJournalDetailId { get; set; }

        public int AccountKey { get; set; }

        public int CurrencyKey { get; set; }

        public int CostCentreKey { get; set; }

        public int UnderwritingYearKey { get; set; }

        public decimal BaseAmount { get; set; }

        public Double CurrencyRate { get; set; }
    }
}
