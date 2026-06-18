using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateTaxesCommandBase : BaseRequestType
    {

        //[Range(1, int.MaxValue, ErrorMessage = "The InsuranceFileKey field is required")]
        //[DefaultValue(0)]
       
        public List<BaseUpdateTaxesRequestTypeRow> Row { get; set; }
    }
}
