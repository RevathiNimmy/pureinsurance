
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CallNamedStoredProcedureCommandBaseResponse :BaseResponseType
    {
        public string Results { get; set; }
        public List<Dictionary<string, object>> ReportDataset { get; set; }
    }
}
