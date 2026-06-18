using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimType
    {
        // [DbColumn("insurance_file_cnt")]
        public int InsuranceFileKey { get; set; }

        // [DbColumn("product_id")]
        public int ProductId { get; set; }

        // [DbColumn("product_code")]
        public string ProductCode { get; set; }

        // [DbColumn("insurance_ref")]
        public string InsuranceRef { get; set; }

        // [DbColumn("lead_agent_cnt")]
        public int AgentCnt { get; set; }

        // [DbColumn("insurance_holder_cnt")]
        public int InsuranceHolderCnt { get; set; }

        // [DbColumn("insurance_folder_cnt")]
        public int InsuranceFolderCnt { get; set; }

        // [DbColumn("source_id")]
        public int SourceId { get; set; }

        // [DbColumn("insured_cnt")]
        public int InsuredCnt { get; set; }

        // [DbColumn("description")]
        public string SourceDescription { get; set; }

        // [DbColumn("suppress_reserves")]
        public int PaymentSuppressReserves { get; set; }

        // [DbColumn("suppress_payments")]
        public int PaymentSuppressPayments { get; set; }

        // [DbColumn("suppress_recoveries")]
        public int PaymentSuppressRecoveries { get; set; }

        // [DbColumn("underwriting_year_id")]
        public int UnderwritingYearId { get; set; }

        // [DbColumn("currency_code")]
        public string CurrencyCode { get; set; }

        // [DbColumn("currency_id")]
        public short CurrencyId { get; set; }

        // [DbColumn("inception_date")]
        public DateTime InceptionDate { get; set; }
        public int BaseCaseKey { get; set; }
        public string CatastropheCode { get; set; }
        public string ClaimStatus { get; set; }
        public DateTime ClaimStatusDate { get; set; }
        public int ClaimVersion { get; set; }
        public string ClaimVersionDescription { get; set; }
        public BaseClaimPartyClientType Client { get; set; }
        public string ClientEmail { get; set; }
        public string ClientFaxNo { get; set; }
        public string ClientMobileNo { get; set; }
        public string ClientShortName { get; set; }
        public string ClientTelNo { get; set; }
        public string ClientTelNoOff { get; set; }
        public string Comments { get; set; }
        public string Description { get; set; }
        public string HandlerCode { get; set; }
        public bool InfoOnly { get; set; }
        public BaseClaimPartyInsurerType Insurer { get; set; }
        public bool IsPolicyOutstanding { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool LikelyClaim { get; set; }
        public string Location { get; set; }
        public DateTime LossFromDate { get; set; }
        public DateTime LossToDate { get; set; }
        public bool LossToDateSpecified { get; set; }
        public bool LossToDateSpecified1 { get; set; }
        public string PrimaryCauseCode { get; set; }
        public int PrimaryCauseId { get; set; }
        public int ClientAddressTypeId { get; set; }
        public string ProgressStatusCode { get; set; }
        public DateTime ReportedDate { get; set; }
        public bool ReserveOnly { get; set; }
        public string SecondaryCauseCode { get; set; }
        public int TPA { get; set; }
        public string TownCode { get; set; }
        public string UserDefFldACode { get; set; }
        public string UserDefFldBCode { get; set; }
        public string UserDefFldCCode { get; set; }
        public string UserDefFldDCode { get; set; }
        public string UserDefFldECode { get; set; }
        public int TransactionTypeId { get; set; }
        public string TransactionTypeDescription { get; set; }
        public string ClientName { get; set; }
        public string InsurerTelNo { get; set; }
        public string InsurerEmail { get; set; }
        public string InsurerFaxNo { get; set; }
        public string ClientOtherContacts { get; set; }
        public string InsurerOtherContacts { get; set; }
        public int ClaimId { get; set; }
        public string InsurerContact { get; set; }
        public string InsurerMobileNo { get; set; }
        public string InsurerWebNo { get; set; }
        public string ClientWebNo { get; set; }
        public int? UserDefFldAId { get; set; }
        public int? UserDefFldBId { get; set; }
        public int? UserDefFldCId { get; set; }
        public int? UserDefFldDId { get; set; }
        public int? UserDefFldEId { get; set; }
        public string InsurerName { get; set; }
        public string InsurerShortName { get; set; }
        public int? ClaimStatusId { get; set; }
        public int ProgressStatusId { get; set; }
        public int HandlerId { get; set; }
        public int CatastropheId { get; set; }
        public int? PrimaryKey { get; set; }
        public int TownId { get; set; }
        public int SecondaryCauseId { get; set; }
        public int InsurerCountryId { get; set; }
        public int ClientCountryId { get; set; }

        public string UserDefFldATableCode { get; set; }
        public string UserDefFldBTableCode { get; set; }
        public string UserDefFldCTableCode { get; set; }
        public string UserDefFldDTableCode { get; set; }
        public string UserDefFldETableCode { get; set; }
        public bool IsDataTransferClaim { get; set; } = false;
        public bool DataTransferIsUsingFullClaimVersioning { get; set; } = false;

        public int? StatsFolderId { get; set; }
        public int PreviousSecondaryCauseId { get; set; }
        public int PreviousCatastropheCodeid { get; set; }
        public string PreviousLocation { get; set; }
        public int PreviousTownId { get; set; }
        public string PreviousClientTelNo { get; set; }
        public string PreviousClientFaxNo { get; set; }
        public string PreviousClientMobileNo { get; set; }
        public string PreviousClientEmail { get; set; }
        public string PreviousClientClaimNumber { get; set; }
        public string PreviousInsurerTelNo { get; set; }
        public string PreviousInsurerFaxNo { get; set; }
        public string PreviousInsurerEmail { get; set; }
        public string PreviousInsurerClaimNumber { get; set; }
        public string PreviousInsurerContact { get; set; }
        public string PreviousInsurerOtherContacts { get; set; }
        public string PreviousVatRegNo { get; set; }
        public string PreviousComments { get; set; }
        public string PreviousClientTelNoOff { get; set; }
        public string PreviousClientOtherContacts { get; set; }
        public int PreviousUserDefFldAId { get; set; }
        public int PreviousUserDefFldBId { get; set; }
        public int PreviousUserDefFldCId { get; set; }
        public int PreviousUserDefFldDId { get; set; }
        public int PreviousUserDefFldEId { get; set; }
        public bool PreviousClientTaxRegistered { get; set; }
        public DateTime PreviousLossFromDate { get; set; }
        public DateTime PreviousLossToDate { get; set; }
        public bool PreviousInfoOnly { get; set; }
        public string PreviousClaimStatusCode { get; set; }
        public int InsurerAddressId { get; set; }
        public string PolicyNumber { get; set; }
        public string ClaimNumber { get; set; }
        public int ClientAddressId { get; set; }
        public int RiskKey { get; set; }
        public int BaseClaimKey { get; set; }
        public int VersionId { get; set; }
        public bool DataTransferClaimHasClaimRiskDataSpecified { get; set; }
        public bool CloseClaimOnZeroReserveRecoveryBalance { get; set; }
        public string CoinsuranceTreatmentCode { get; set; }
    }
}

