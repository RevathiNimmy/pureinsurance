using SSP.PureInsuranceRestAPIHandler.Enums;
using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddBackDatedMTAQuoteCommandBase : BaseRequestType
    {
        public DateTime EffectiveDate { get; set; }
        public int InsuranceFileKey { get; set; }
        public int InsuranceFolderKey { get; set; }
        public bool IsInteractive { get; set; }
        public int PartyCnt { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
