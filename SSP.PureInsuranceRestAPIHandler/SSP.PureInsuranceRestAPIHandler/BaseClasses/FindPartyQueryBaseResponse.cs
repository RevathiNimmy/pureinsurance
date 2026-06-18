using System.Collections.Generic;
using System.Xml;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindPartyQueryBaseResponse : BasePagedResponse
    {
        public System.Collections.Generic.List<BaseFindPartyResponseTypeRow> Parties { get; set; }
        public XmlElement ResultDataset { get; set; }

    }
}
