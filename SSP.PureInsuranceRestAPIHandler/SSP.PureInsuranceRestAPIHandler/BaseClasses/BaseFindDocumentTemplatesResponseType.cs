using System.Data;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseFindDocumentTemplatesResponseType
    {
        public System.Xml.XmlElement DocumentTemplatesDataset { get; set; }
        public DataSet ResultData { get; set; }
    }
}
