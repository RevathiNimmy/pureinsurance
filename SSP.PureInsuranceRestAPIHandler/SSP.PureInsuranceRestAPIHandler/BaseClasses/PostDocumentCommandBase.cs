using SSP.PureInsuranceRestAPIHandler.Enums;
using System.ComponentModel;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.PostDocument
{
    public class PostDocumentCommandBase : BaseRequestType
    {
        public string Comment { get; set; }
        public string DocumentReference { get; set; }
        public DocumentTypeType DocumentType { get; set; }
        public string DocumentTypeCode { get; set; }
        
        //(1, int.MaxValue, ErrorMessage = "The InsuranceFileKey field is required")]
        
        public int InsuranceFileKey { get; set; }
        public bool InsuranceFileKeySpecified { get; set; }
        public int SAMStagingPolicyKey { get; set; }
        public bool SAMStagingPolicyKeySpecified { get; set; }
        public System.Collections.Generic.List<BaseTransactionType> Transactions { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public int SourceId { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public int DocumentTypeId { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public int CurrencyId { get; set; }
    }
}
