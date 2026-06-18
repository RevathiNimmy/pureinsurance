using System.Collections.Generic;
using System.Xml;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindClaimQueryBaseResponse : BasePagedResponse
    {
        public System.Collections.Generic.List<BaseFindClaimResponseTypeRow> Claims { get; set; }
        public XmlElement ResultDataset { get; set; }
    }
}
