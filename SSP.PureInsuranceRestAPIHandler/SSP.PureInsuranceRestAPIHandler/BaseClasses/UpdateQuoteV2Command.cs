using SSP.PureInsuranceRestAPIHandler.Enums;
using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateQuoteV2Command : UpdateQuoteV2CommandBase
    {
        public System.DateTime IntialCoverExpiryDate { get; set; } = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;

        public System.DateTime OldCoverEndDate { get; set; } = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;

        public TransactionType TransactionType { get; set; }
        public int UnderWritingYearKey { get; set; }
    }
}
