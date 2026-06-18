using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddMtaQuoteCommandBase :BaseRequestType
    {
        public int AccountHandlerCnt { get; set; }
        public bool AccountHandlerCntSpecified { get; set; }
        public string AccountHandlerCode { get; set; }
        public string AlternateReference { get; set; }
        public string AnalysisCode { get; set; }
        public DateTime AnniversaryDate { get; set; }
        public bool AnniversaryDateSpecified { get; set; }
        public string BusinessTypeCode { get; set; }
        public string CoInsurancePlacement { get; set; }
        public string CorrespondenceType { get; set; }
        public string DefaultPreferredCorrespondence { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string FrequencyCode { get; set; }
        public int InsuranceFileKey { get; set; }
        public string InsuredName { get; set; }
        public bool IsAgentReceiveCorrespondence { get; set; }
        public bool IsReinstatement { get; set; }
        public DateTime IssueDate { get; set; }
        public bool IssueDateSpecified { get; set; }
        public DateTime LTUExpiryDate { get; set; }
        public bool LTUExpiryDateSpecified { get; set; }
        public DateTime LapseCancelDate { get; set; }
        public bool LapseCancelDateSpecified { get; set; }
        public string LapseCancelReasonCode { get; set; }
        public string MtaReason { get; set; }
        public string OldPolicyNumber { get; set; }
        public string PolicyKey { get; set; }
        public string PolicyStatusCode { get; set; }
        public string PolicyStyleCode { get; set; }
        public DateTime ProposalDate { get; set; }
        public bool ProposalDateSpecified { get; set; }
        public bool PutOnNextInstalmentRenewal { get; set; }
        public bool PutOnNextInstalmentRenewalSpecified { get; set; }
        public DateTime QuoteExpiryDate { get; set; }
        public bool QuoteExpiryDateSpecified { get; set; }
        public bool ReferredAtRenewal { get; set; }
        public bool ReferredAtRenewalSpecified { get; set; }
        public bool ReferredOnMTA { get; set; }
        public bool ReferredOnMTASpecified { get; set; }
        public string Regarding { get; set; }
        public DateTime RenewalDate { get; set; }
        public bool RenewalDateSpecified { get; set; }
        public int RenewalDayNo { get; set; }
        public bool RenewalDayNoSpecified { get; set; }
        public string RenewalMethodCode { get; set; }
        public string StopReasonCode { get; set; }
        public SSP.PureInsuranceRestAPIHandler.Enums.TransactionType TransactionType { get; set; }
        public string TypeOfMta { get; set; }
        
        public int SourceId { get; set; }
        
        public int MtaReasonID { get; set; }
        
        public int LapsedReasonID { get; set; }
        
        public int NewInsuranceFileKey { get; set; }
        
        public int MTAReasonKey { get; set; }
        
        public int CreatedByKey { get; set; }
        
        public string UnderWritingYearCode { get; set; }
   
        public int DefaultPreferredCorrespondenceTypeKey { get; set; }
       
        public int CorrespondenceTypeKey { get; set; }
        
        public bool IsBackdateMTA { get; set; }
    
        public string MTAReasonDescription { get; set; }
        
        public DateTime InceptionDate { get; set; }
       
        public int InsuranceHolderKey { get; set; }
       
        public int InsuranceFolderKey { get; set; }
        
        public string InsuranceRef { get; set; }
        
        public int ProductKey { get; set; }
        
        public int ErrorCode { get; set; }
        
        public int PolicyVersion { get; set; }
        
        public bool IsBDXRequest { get; set; }
      
        public DateTime CancellationDate { get; set; }

        public object CovertEndDate { get; set; }
    }
}
