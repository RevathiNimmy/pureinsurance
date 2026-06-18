using System.Data;
using System.Xml;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public partial class BaseGetUnallocatedClaimPaymentsResponseType : BaseResponseType
    {
        public XmlElement ResultDataset { get; set; }

        public DataSet ResultData { get; set; } = new DataSet();
    }
}
