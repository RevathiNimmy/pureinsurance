using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCoverNoteRiskItemType
    {
        public System.DateTime CoverNoteFrom { get; set; } = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        public bool CoverNoteFromSpecified { get; set; }
        public string CoverNoteNumber { get; set; }
        public System.DateTime CoverNoteTo { get; set; } = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        public bool CoverNoteToSpecified { get; set; }
        public string RiskDesc { get; set; }
        public int RiskKey { get; set; }
        public byte[] ApiTimeStamp { get; set; }
    }
}
