using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCdtClaimType
    {
        public string CatastropheCode { get; set; }
        public string ClaimNumber { get; set; }
        public System.Collections.Generic.List<BaseCdtClaimReinsuranceTypeForDtu> ClaimReinsuranceForDTU { get; set; }
        public System.Collections.Generic.List<BaseCdtClaimPerilType> ClaimPeril { get; set; }
        public BaseCdtClaimReinsuranceType ClaimReinsurance { get; set; }
        public string ClaimVersionDescription { get; set; }
        public string Comments { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
        public string HandlerCode { get; set; }
        public bool InfoOnly { get; set; }
        public bool LikelyClaim { get; set; }
        public string Location { get; set; }
        public System.DateTime LossFromDate { get; set; }
        public System.DateTime LossToDate { get; set; }
        public bool LossToDateSpecified { get; set; }
        public bool LossToDateSpecified1 { get; set; }
        public string PrimaryCauseCode { get; set; }
        public string ProgressStatusCode { get; set; }
        public System.DateTime ReportedDate { get; set; }
        public int SAMStagingClaimKey { get; set; }
        public string SecondaryCauseCode { get; set; }
        public int SiriusInsuranceFileKey { get; set; }
        public int SiriusRiskKey { get; set; }
        public string TownCode { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public bool TransactionDateSpecified { get; set; }
        public string UnderwritingYearCode { get; set; }
        public int VersionNo { get; set; }
        public bool VersionNoSpecified { get; set; }
        public string XMLDATASET { get; set; }
        public int SiriusBaseClaimKey { get; set; }
        public int SiriusClaimKey { get; set; }
        public int VersionId { get; set; }
        public System.Collections.Generic.List<BaseClaimPerilRecoveryType> Recovery { get; set; }
        public System.Collections.Generic.List<BaseClaimPerilReserveType> Reserve { get; set; }
    }
}
