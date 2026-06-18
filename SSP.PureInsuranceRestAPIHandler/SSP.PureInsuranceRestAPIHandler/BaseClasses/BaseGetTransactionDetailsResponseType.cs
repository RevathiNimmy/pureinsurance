using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{

    public partial class BaseGetTransactionDetailsResponseType : BaseRequestType
    {
        public XmlElement ResultDataset { get; set; }
        public DataSet ResultData { get; set; }
        public byte[] AllocationTimeStamp { get; set; } = new byte[0];
        public System.Collections.Generic.List<BaseGetTransactionDetailsResponseTypeRow> Transactions { get; set; }
    }

}
