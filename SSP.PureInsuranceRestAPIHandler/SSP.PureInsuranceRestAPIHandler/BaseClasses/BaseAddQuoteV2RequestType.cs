namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseAddQuoteV2RequestType : BaseQuoteType
    {
        public int AccountHandlerCnt { get; set; }
        public bool AccountHandlerCntSpecified { get; set; }
        public System.DateTime AnniversaryDate { get; set; }
        public bool AnniversaryDateSpecified { get; set; }

        public string BusinessTypeCode { get; set; }
        public string CoInsurancePlacement { get; set; }
        public bool ConsolidatedLeadAgentCommission { get; set; }
        public bool ConsolidatedLeadAgentCommissionSpecified { get; set; }
        public bool ConsolidatedSubAgentCommission { get; set; }
        public bool ConsolidatedSubAgentCommissionSpecified { get; set; }
        public string CoverNoteBookNumber { get; set; }
        public int CoverNoteSheetNumber { get; set; }
        public bool CoverNoteSheetNumberSpecified { get; set; }
        public string FrequencyCode { get; set; }
        public string HandlerCode { get; set; }

        public System.DateTime InceptionDate { get; set; }

        public System.DateTime InceptionTPI { get; set; }
        public bool IsMarketPlacePolicy { get; set; }
        public System.DateTime IssuedDate { get; set; }
        public bool IssuedDateSpecified { get; set; }
        public System.DateTime LTUExpiryDate { get; set; }
        public bool LTUExpiryDateSpecified { get; set; }
        public System.DateTime LapseCancelDate { get; set; }
        public bool LapseCancelDateSpecified { get; set; }
        public string LapseCancelReasonCode { get; set; }

        ////(1, int.MaxValue, ErrorMessage = "The PartyKey field is required")]
        //
        public int PartyKey { get; set; }
        public string PolicyStatusCode { get; set; }
        public System.DateTime ProposalDate { get; set; }
        public bool ProposalDateSpecified { get; set; }
        public bool PutOnNextInstalmentRenewal { get; set; }
        public bool PutOnNextInstalmentRenewalSpecified { get; set; }

        public System.DateTime QuoteExpiryDate { get; set; }
        public bool ReferredAtMTA { get; set; }
        public bool ReferredAtMTASpecified { get; set; }
        public bool ReferredAtRenewal { get; set; }
        public bool ReferredAtRenewalSpecified { get; set; }
        public string Regarding { get; set; }
        public int RenewalCount { get; set; }
        public bool RenewalCountSpecified { get; set; }

        public System.DateTime RenewalDate { get; set; }
        public int RenewalDayNo { get; set; }
        public bool RenewalDayNoSpecified { get; set; }
        public string RenewalMethodCode { get; set; }
        public string StopReasonCode { get; set; }

        public string SubBranchCode { get; set; }

        public string CollectionFrequencyCode { get; set; }

        public string PaymentTermCode { get; set; }

        public bool IsBDXRequest { get; set; }
    }
}
