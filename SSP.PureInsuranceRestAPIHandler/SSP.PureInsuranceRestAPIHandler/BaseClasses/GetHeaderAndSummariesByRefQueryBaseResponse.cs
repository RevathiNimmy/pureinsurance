using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetHeaderAndSummariesByRefQueryBaseResponse : BaseGetHeaderAndSummariesResponseType
    {
        public System.DateTime InceptionDate { get; set; }
        public int InsuranceFileVersion { get; set; }
        public new string InsuranceFileTypeCode { get; set; }
        public string InsuranceFileStatusCode { get; set; }
        public string PaymentMethodCode { get; set; }
        public new List<GetHeaderAndSummariesByKeyResponseTypeRow> InsuredParties { get; set; }
    }
}
