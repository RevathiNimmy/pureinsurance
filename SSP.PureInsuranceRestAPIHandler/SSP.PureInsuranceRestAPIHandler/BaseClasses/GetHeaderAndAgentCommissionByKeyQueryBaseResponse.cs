
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetHeaderAndAgentCommissionByKeyQueryBaseResponse : BasePagedResponse
    {
        public string Agent { get; set; }
        public List<BaseGetHeaderAndAgentCommissionByKeyResponseTypeRow> AgentCommission { get; set; }
        public string ClientCode { get; set; }
        public DateTime CoverStartDate { get; set; }
        public string Currency { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime InceptionDate { get; set; }
        public int InsuranceFileKey { get; set; }
        public string InsuranceFileRef { get; set; }
        public System.Xml.XmlElement ResultDataset { get; set; }
        public double TotalCommissionLeadAgent { get; set; }
        public double TotalCommissionSubAgent { get; set; }
        public double TotalNetPremiumLeadAgent { get; set; }
        public double TotalNetPremiumSubAgent { get; set; }
        public double TotalTaxLeadAgent { get; set; }
        public double TotalTaxSubAgent { get; set; }
    }
}
