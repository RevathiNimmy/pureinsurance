using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetTaxesQueryResponse : BasePagedResponse
    {
        public System.Xml.XmlElement ResultDataset { get; set; }
        public System.Collections.Generic.List<BaseGetTaxesResponseTypeRow> Row { get; set; }
    }
}
