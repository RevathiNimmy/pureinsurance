using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetReportRequestType : BaseRequestType
    {

        public string ReportName { get; set; }
        public DocumentFormatTypes FormatType { get; set; }
        public System.Collections.Generic.List<BaseGetReportRequestTypeParameters> Parameters { get; set; }
    }
}
