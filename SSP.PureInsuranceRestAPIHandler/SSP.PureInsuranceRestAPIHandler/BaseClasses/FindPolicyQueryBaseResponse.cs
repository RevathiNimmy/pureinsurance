using System.Collections.Generic;
using System.Xml;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindPolicyQueryBaseResponse : BasePagedResponse
    {
        public System.Collections.Generic.List<PolicyQueryResponse> Policies { get; set; }
        public XmlElement ResultDataset { get; set; }
    }
}
