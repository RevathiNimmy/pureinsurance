using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePostDocumentRequestType
    {
        public string Comment { get; set; }
        public string DocumentReference { get; set; }
        public DocumentTypeType DocumentType { get; set; }
        public string DocumentTypeCode { get; set; }

        ////(1, int.MaxValue, ErrorMessage = "The InsuranceFileKey field is required")]
        //
        public int InsuranceFileKey { get; set; }
        public bool InsuranceFileKeySpecified { get; set; }
        public int SAMStagingPolicyKey { get; set; }
        public bool SAMStagingPolicyKeySpecified { get; set; }
        public System.Collections.Generic.List<BaseTransactionType> Transactions { get; set; }
    }
}
