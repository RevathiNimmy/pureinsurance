using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class ProductRiskOptionValueRequestType : BaseRequestType
    {
       public SSP.PureInsuranceRestAPIHandler.Enums.ProductConfigActionType ActionType { get; set; }
       
       public SSP.PureInsuranceRestAPIHandler.Enums.ProductRiskOptions ProducRiskOption { get; set; }

       public Boolean ProducRiskOptionSpecified { get;set; }

       public String ProductCode {get;set;}
       public String RiskTypeCode { get; set;}

       public SSP.PureInsuranceRestAPIHandler.Enums.RiskTypeOptions RiskTypeOption { get; set; }

       public Boolean RiskTypeOptionSpecified { get; set; }
    }
}
