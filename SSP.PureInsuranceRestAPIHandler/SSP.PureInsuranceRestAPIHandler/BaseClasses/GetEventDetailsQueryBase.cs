using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetEventDetailsQueryBase : BaseRequestType
    {
        public int AccountKey { get; set; }
        public bool AccountKeySpecified { get; set; }
        public int BGKey { get; set; }
        public bool BGKeySpecified { get; set; }
        public int BaseCaseKey { get; set; }
        public bool BaseCaseKeySpecified { get; set; }
        public int BaseClaimKey { get; set; }
        public bool BaseClaimKeySpecified { get; set; }
        public int CaseKey { get; set; }
        public bool CaseKeySpecified { get; set; }
        public int ClaimKey { get; set; }
        public bool ClaimKeySpecified { get; set; }
        public string ClaimNumber { get; set; }
        public bool ClaimNumberSpecified { get; set; }
        public DateTime DateTo { get; set; }
        public bool DateToSpecified { get; set; }
        public int EventTypeKey { get; set; }
        public bool EventTypeKeySpecified { get; set; }
        public int FSAComplaintFolderKey { get; set; }
        public bool FSAComplaintFolderKeySpecified { get; set; }
        public DateTime FromDate { get; set; }
        public bool FromDateSpecified { get; set; }
        public int InsuranceFileKey { get; set; }
        public bool InsuranceFileKeySpecified { get; set; }
        public int InsuranceFolderKey { get; set; }
        public bool InsuranceFolderKeySpecified { get; set; }
        public int OldPartyTypeKey { get; set; }
        public bool OldPartyTypeKeySpecified { get; set; }
        public int PartyKey { get; set; }
        public int UserId { get; set; }
        public bool UserIdSpecified { get; set; }
    }
}
