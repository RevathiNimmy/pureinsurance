using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CallNamedStoredProcedureRequestType :BaseRequestType
    {
        public List<BaseParameterType> Parameters { get; set; }

        public string ProcedureName { get; set; }
    }
}
