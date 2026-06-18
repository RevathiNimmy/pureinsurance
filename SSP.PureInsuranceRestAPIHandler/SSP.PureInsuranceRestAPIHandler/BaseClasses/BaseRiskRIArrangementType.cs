using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseRiskRIArrangementType
    {
        public string RIBandCode { get; set; }

        public int RIBandID { get; set; }

        public string RIModelCode { get; set; }

        public int RIModelID { get; set; }

        public double SumInsured { get; set; }

        public double Premium { get; set; }

        public bool OriginalFlag { get; set; }

        public System.Collections.Generic.List<BaseRiskRIArrangementLineType> RIArrangementLine { get; set; }
    }
}
