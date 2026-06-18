
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetProductRiskOptionValueQueryBase : BaseRequestType
    {
        public SSP.PureInsuranceRestAPIHandler.Enums.ProductConfigActionType ActionType { get; set; }
        public SSP.PureInsuranceRestAPIHandler.Enums.ProductRiskOptions ProducRiskOption { get; set; }
        public bool ProducRiskOptionSpecified { get; set; }
        public string ProductCode { get; set; }
        public string RiskTypeCode { get; set; }
        public SSP.PureInsuranceRestAPIHandler.Enums.RiskTypeOptions RiskTypeOption { get; set; }
        public bool RiskTypeOptionSpecified { get; set; }
        public bool UnitOfWorkDisposed { get; set; } = true;
    }
}
