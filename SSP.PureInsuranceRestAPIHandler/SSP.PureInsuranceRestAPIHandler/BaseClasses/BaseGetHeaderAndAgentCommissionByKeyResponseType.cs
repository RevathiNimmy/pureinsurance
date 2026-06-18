using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetHeaderAndAgentCommissionByKeyResponseType : BaseResponseType
    {
        public string Agent { get; set; }
        public System.Collections.Generic.List<BaseGetHeaderAndAgentCommissionByKeyResponseTypeRow> AgentCommission { get; set; }
        public string ClientCode { get; set; }
        public System.DateTime CoverStartDate { get; set; }
        public string Currency { get; set; }
        public System.DateTime ExpiryDate { get; set; }
        public System.DateTime InceptionDate { get; set; }
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
