
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetVersionsForClaimQueryResponse : BasePagedResponse
    {
        public bool IsPreviouslyLocked { get; set; }
        public string NewProperty { get; set; }
        public List<BaseGetVersionsForClaimResponseTypeRow> Versions { get; set; }
        
        public System.Xml.XmlElement ResultDataset { get; set; }
    }
}
