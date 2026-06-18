
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CallNamedStoredProcedureCommandBase : BaseRequestType
    {
        public List<BaseParameterType> Parameters { get; set; }
        public string ProcedureName { get; set; }
        public bool? IsReportDataset { get; set; }
    }
}