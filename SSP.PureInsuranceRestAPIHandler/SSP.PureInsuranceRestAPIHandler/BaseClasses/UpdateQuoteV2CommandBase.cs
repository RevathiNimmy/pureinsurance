using System;
using System.Runtime.Serialization;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateQuoteV2CommandBase : BaseQuoteType
    {

        public System.DateTime AnniversaryDate { get; set; } = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        public bool AnniversaryDateSpecified { get; set; }

        public string BusinessTypeCode { get; set; }
        public string CoInsurancePlacement { get; set; }
        public int CollectionFrequency { get; set; }
        public bool CollectionFrequencySpecified { get; set; }
        public bool ConsolidatedLeadAgentCommission { get; set; }
        public bool ConsolidatedLeadAgentCommissionSpecified { get; set; }
        public bool ConsolidatedSubAgentCommission { get; set; }
        public bool ConsolidatedSubAgentCommissionSpecified { get; set; }
        public int ContactuserKey { get; set; }
        public bool ContactuserKeySpecified { get; set; }
        public string CorrespondenceType { get; set; }
        public string CoverNoteBookNumber { get; set; }
        public int CoverNoteSheetNumber { get; set; }
        public bool CoverNoteSheetNumberSpecified { get; set; }
        public string DefaultPreferredCorrespondence { get; set; }

        public string FrequencyCode { get; set; }
        public string HandlerCode { get; set; }

        public System.DateTime InceptionDate { get; set; } = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;

        public System.DateTime InceptionTPI { get; set; } = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;

        ////(1, int.MaxValue, ErrorMessage = "The InsuranceFileKey field is required")]
        //
        public int InsuranceFileKey { get; set; }

        ////(1, int.MaxValue, ErrorMessage = "The InsuranceFolderKey field is required")]
        //
        public int InsuranceFolderKey { get; set; }
        public bool IsAgentReceiveCorrespondence { get; set; }
        public bool IsMarketPlacePolicy { get; set; }
        public System.DateTime IssuedDate { get; set; } = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        public bool IssuedDateSpecified { get; set; }
        public System.DateTime LTUExpiryDate { get; set; } = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        public bool LTUExpiryDateSpecified { get; set; }
        public System.DateTime LapseCancelDate { get; set; } = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        public bool LapseCancelDateSpecified { get; set; }
        public string LapseCancelReasonCode { get; set; }
        public System.DateTime MarkedDate { get; set; } = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        public bool MarkedDateSpecified { get; set; }
        public bool MarkedForCollection { get; set; }
        public bool MarkedForCollectionSpecified { get; set; }
        public string OldPolicyNumber { get; set; }

        ////(1, int.MaxValue, ErrorMessage = "The PartyKey field is required")]
        //
        public int PartyKey { get; set; }
        public string PaymentMethod { get; set; }
        public int PaymentTerms { get; set; }
        public bool PaymentTermsSpecified { get; set; }
        public string PolicyStatusCode { get; set; }
        public System.DateTime ProposalDate { get; set; } = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        public bool ProposalDateSpecified { get; set; }
        public bool PutOnNextInstalmentRenewal { get; set; }
        public bool PutOnNextInstalmentRenewalSpecified { get; set; }

        public System.DateTime QuoteExpiryDate { get; set; } = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        public string ReceiverEmail { get; set; }
        public bool ReferredAtMTA { get; set; }
        public bool ReferredAtMTASpecified { get; set; }
        public bool ReferredAtRenewal { get; set; }
        public bool ReferredAtRenewalSpecified { get; set; }
        public string Regarding { get; set; }

        public System.DateTime RenewalDate { get; set; } = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        public int RenewalDayNo { get; set; }
        public bool RenewalDayNoSpecified { get; set; }
        public string RenewalMethodCode { get; set; }
        public string SenderEmail { get; set; }
        public string StopReasonCode { get; set; }

        public string SubBranchCode { get; set; }
        public string ApiTimeStamp { get; set; }
        public int UnderwritingYearId { get; set; }
        public bool UnderwritingYearIdSpecified { get; set; }
        [IgnoreDataMember]
        public int BranchKey { get; set; }
        [IgnoreDataMember]
        public int ProductKey { get; set; }
        [IgnoreDataMember]
        public int CurrencyKey { get; set; }
        [IgnoreDataMember]
        public int AnalysisKey { get; set; }
        [IgnoreDataMember]
        public int SubBranchKey { get; set; }
        [IgnoreDataMember]
        public int BusinessTypeKey { get; set; }
        [IgnoreDataMember]
        public int HandlerKey { get; set; }
        [IgnoreDataMember]
        public int PolicyStatusKey { get; set; }
        [IgnoreDataMember]
        public int FrequencyKey { get; set; }
        [IgnoreDataMember]
        public int RenewalMethodKey { get; set; }
        [IgnoreDataMember]
        public int LapseCancelReasonKey { get; set; }
        [IgnoreDataMember]
        public int StopReasonKey { get; set; }
        [IgnoreDataMember]
        public int UserKey { get; set; }
        [IgnoreDataMember]
        public int InsuranceFileStructureKey { get; set; }
        [IgnoreDataMember]
        public int InsuranceFileTypeKey { get; set; }
        [IgnoreDataMember]
        public int CorrespondenceTypeKey { get; set; }
        [IgnoreDataMember]
        public int DefaultPreferredCorrespondenceTypeKey { get; set; }
        [IgnoreDataMember]
        public System.DateTime IntialCoverStartDate { get; set; } = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        [IgnoreDataMember]
        public string InsuranceFileReference { get; set; }
    }
}
