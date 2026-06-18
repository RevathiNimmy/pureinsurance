using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetHeaderAndRisksByKeyQueryBaseResponse : BasePagedResponse
    {
        public string Agent { get; set; }
        public string ClientCode { get; set; }
        public string CorrespondenceType { get; set; }
        public System.DateTime CoverStartDate { get; set; } = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        public string Currency { get; set; }
        public string DefaultPreferredCorrespondence { get; set; }
        public System.DateTime ExpiryDate { get; set; }
        public double FeeTotal { get; set; }
        public double GrossTotal { get; set; }
        public System.DateTime InceptionDate { get; set; }
        public int InsuranceFileKey { get; set; }
        public string InsuranceFileRef { get; set; }
        public bool IsAgentReceiveCorrespondence { get; set; }
        public string MailContacts { get; set; }
        public double NetTotal { get; set; }
        public System.Collections.Generic.List<BaseGetHeaderAndRisksByKeyResponseTypeRow> Risks { get; set; }
        public double TaxRateUnRounded { get; set; }
        public double TaxTotal { get; set; }
        public double TotalSumInsured { get; set; }
    }
}
