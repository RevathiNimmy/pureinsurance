Option Strict Off
Option Explicit On

Imports System.Runtime.Serialization

Namespace SFI.SAMForInsuranceV2.WCF


    '''<remarks/>
    <System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseClaimProcessResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetDefaultRiskClausesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetDefaultRiskClausesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetFinancePlanDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetFinancePlanDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetFinancePlansResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetFinancePlansResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetNumberingSchemeNoResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetNumberingSchemeNoResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAttachCoverNoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AttachCoverNoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetTaxesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetTaxesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindBankResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(FindBankResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetBankGuaranteeResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetBankGuaranteeResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddBankGuaranteeResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddBankGuaranteeResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPolicyBankGuaranteeResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetPolicyBankGuaranteeResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseCreateReceiptCashListItemResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(CreateReceiptCashListItemResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseCreateReceiptCashListWithItemsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(CreateReceiptCashListWithItemsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAllocationDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetAllocationDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseMarkUnmarkTransactionResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(MarkUnmarkTransactionResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetTaxGroupsForClaimsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetTaxGroupsForClaimsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAgentDetailsForPolicyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetAgentDetailsForPolicyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetUserGroupsbyTaskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetUserGroupsbyTaskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPoliciesOnBankGuaranteeByKeyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetPoliciesOnBankGuaranteeByKeyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPoliciesOnBankGuaranteeForReceiptResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetPoliciesOnBankGuaranteeForReceiptResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetCoverNoteBookResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetCoverNoteBookResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddTaskGroupResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddTaskGroupResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddQuoteV2ResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddQuoteV2ResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetBalancesAndUnallocatedCreditsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetBalancesAndUnallocatedCreditsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseProductRiskOptionValueResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(ProductRiskOptionValueResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHeaderAndAgentCommissionByKeyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetHeaderAndAgentCommissionByKeyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetReceiptCashListItemsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetReceiptCashListItemsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPaymentCashListItemDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetPaymentCashListItemDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPaymentCashListDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetPaymentCashListDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindAccountsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(FindAccountsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetUserGroupUsersResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetUserGroupUsersResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateQuoteV2ResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateQuoteV2ResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateTaskGroupsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateTaskGroupsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateCoverNoteSheetResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateCoverNoteSheetResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateCoverNoteBookResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateCoverNoteBookResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseDeleteUndeleteUserGroupResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(DeleteUndeleteUserGroupResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRatingSectionTypesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetRatingSectionTypesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAccountDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetAccountDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetCoinsuranceDefaultsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetCoinsuranceDefaultsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateCoinsuranceValuesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateCoinsuranceValuesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetCoinsuranceValuesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetCoinsuranceValuesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseReAssignMultipleWmTasksResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(ReAssignMultipleWmTasksResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetReceiptCashListDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetReceiptCashListDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPaymentCashListItemsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetPaymentCashListItemsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateReceiptMediaTypeStatusResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateReceiptMediaTypeStatusResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindCashListReceiptsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(FindCashListReceiptsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPolicyStatusForMediaTypeStatusResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetPolicyStatusForMediaTypeStatusResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateAllocationResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateAllocationResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindCoverNoteBooksResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(FindCoverNoteBooksResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseApproveCashListItemResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(ApproveCashListItemResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetUnallocatedClaimPaymentsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetUnallocatedClaimPaymentsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseDeleteCoverNoteSheetResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(DeleteCoverNoteSheetResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetReferredPaymentsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetReferredPaymentsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddCoverNoteSheetResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddCoverNoteSheetResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddCoverNoteBookResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddCoverNoteBookResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetUserGroupTaskGroupsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetUserGroupTaskGroupsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetTaskGroupTasksResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetTaskGroupTasksResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetTaskGroupsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetTaskGroupsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateTaskGroupTasksResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateTaskGroupTasksResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateUserGroupUsersResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateUserGroupUsersResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateUserGroupResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateUserGroupResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetEventNoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetEventNoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddEventNoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddEventNoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddEventResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddEventResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetEventDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetEventDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindDocumentTemplatesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(FindDocumentTemplatesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateStandardPolicyWordingsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateStandardPolicyWordingsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetStandardPolicyWordingsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetStandardPolicyWordingsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetSubAgentsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetSubAgentsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetWmTaskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetWmTaskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetVersionsForClaimResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetVersionsForClaimResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHeaderAndPolicyTaxByKeyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetHeaderAndPolicyTaxByKeyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHeaderAndRiskTaxByKeyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetHeaderAndRiskTaxByKeyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateBankGuaranteeResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateBankGuaranteeResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAuthoriseClaimPaymentResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AuthoriseClaimPaymentResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetUserAuthorityValueResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetUserAuthorityValueResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddWriteOffResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddWriteOffResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetInsurerPaymentsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetInsurerPaymentsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindUsersResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(FindUsersResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseCopyRiskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(CopyRiskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetReceiptCashListItemDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetReceiptCashListItemDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetCurrencyExchangeRatesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetCurrencyExchangeRatesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetTransactionDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetTransactionDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetCoverNoteSheetResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetCoverNoteSheetResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateBankGuaranteeConditionallyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateBankGuaranteeConditionallyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseCheckUnpaidPremiumResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(CheckUnpaidPremiumResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetDMEFolderResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetDMEFolderResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindDMEDocumentsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(FindDMEDocumentsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseDeletePolicyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(DeletePolicyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateQuoteStatusResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateQuoteStatusResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseCopyQuoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(CopyQuoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetSharepointFileListResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetSharepointFileListResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetDocumentDefaultsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetDocumentDefaultsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateClaimReservesOrPaymentsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateClaimReservesOrPaymentsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetQuotesMarkedForCollectionResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetQuotesMarkedForCollectionResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddJournalResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddJournalResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetBalanceForCDResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetBalanceForCDResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAgentCommissionResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateAgentCommissionResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateAgentCommissionResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAgentCommissionResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetAgentCommissionResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGenerateDocumentsForEventResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GenerateDocumentsForEventResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateQuotePaymentMethodResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateQuotePaymentMethodResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAllLivePolicyVersionAmountsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetAllLivePolicyVersionAmountsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetBankAccountsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetBankAccountsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPartyPoliciesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetPartyPoliciesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetClaimPartyDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetClaimPartyDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAccountingPeriodResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetAccountingPeriodResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetEventNoteTypeResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetEventNoteTypeResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetProductRiskEventsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetProductRiskEventsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateFinancePlanDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateFinancePlanDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAgentSettingsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetAgentSettingsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseCalculateTaxForClaimsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(CalculateTaxForClaimsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetMediaTypeResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetMediaTypeResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetDatasetSchemaResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetDatasetSchemaResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetDatasetDefinitionResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetDatasetDefinitionResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetCurrenciesByBranchResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetCurrenciesByBranchResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetClaimsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetClaimRiskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetClaimRiskReadOnlyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddClaimRiskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetClaimRiskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetMIDFilesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetMIDFilesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetMIDFileDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetMIDFileDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetCurrencyToCurrencyExchangeRateResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetCurrencyToCurrencyExchangeRateResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseDeleteBackDatedVersionsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(DeleteBackDatedVersionsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddBackDatedMTAQuoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddBackDatedMTAQuoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetStandardWordingTemplateResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetStandardWordingTemplateResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateStandardWordingTemplateResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateStandardWordingTemplateResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindAddressResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(FindAddressResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetReportResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetReportResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseTransferQuoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(TransferQuoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetBrokerSummaryResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetBrokerSummaryResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseValidateBankAccountNumberResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(ValidateBankAccountNumberResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseActivatePartyBankResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(ActivatePartyBankResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseDeletePartyBankDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(DeletePartyBankDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdatePartyBankDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdatePartyBankDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddPartyBankDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddPartyBankDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPartyBankDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetPartyBankDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetProductClaimsWorkflowOptionsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetProductClaimsWorkflowOptionsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPoliciesForRenewalSelectionResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetPoliciesForRenewalSelectionResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetValidPrimaryCausesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetValidPrimaryCausesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateRenewalStatusResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateRenewalStatusResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseDeleteRenewalResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(DeleteRenewalResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseRunRenewalSelectionByPolicyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(RunRenewalSelectionByPolicyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGenerateInviteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GenerateInviteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetExistingInstalmentPlanPaymentDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetExistingInstalmentPlanPaymentDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPoliciesInRenewalResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetPoliciesInRenewalResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRenewalStatusResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetRenewalStatusResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseLapseRenewalResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(LapseRenewalResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseReplacePartyContactResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(ReplacePartyContactResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddAddressResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddAddressResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddMtaQuoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddMtaQuoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddPartyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddPartyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddQuoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddQuoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddReceiptResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddPayNowReceiptResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddAgentReceiptResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddRiskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddRiskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseCDTResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(ClaimDataImportResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseChangePasswordResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(ChangePasswordResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseClaimMTAResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(ClaimMTAResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseClaimResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseBindClaimResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BindClaimResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(OpenClaimResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(PayClaimResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(MaintainClaimResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(ClaimReceiptResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseCashListResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseClientDataImportResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(ClientDataImportResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseCreateEventResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseCreateWmTaskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(CreateWmTaskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseDeleteRiskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(DeleteRiskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindClaimResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(FindClaimResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindControlSearchResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(FindControlSearchResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindInsuranceFileResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(FindInsuranceFileForClaimsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindPartyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(FindPartyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseForgottenPasswordResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(ForgottenPasswordResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGenerateClaimsDocumentsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GenerateClaimsDocumentsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGenerateDocumentResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GenerateDocumentResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAccountBalanceResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetAccountBalanceResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAddressResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetAddressResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAllPolicyVersionsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetAllPolicyVersionsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetClaimDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetClaimDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetClaimPaymentTaxGroupsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetClaimReceiptTaxGroupsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetClaimReceiptTaxGroupsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetClaimPaymentTaxGroupsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetClaimPaymentTaxesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetClaimPaymentTaxesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetClaimReceiptTaxesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetClaimReceiptTaxesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetClaimRiskLinksResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetClaimRiskLinksResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetClaimRiskLinksResponse)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateQuoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateQuoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetDefaultDatasetResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetDefaultDatasetResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetDocumentListResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetDocumentListResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetDocumentResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetDocumentResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHeaderAndSummariesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetHeaderAndSummariesByRefResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetHeaderAndSummariesByKeyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHistoricalTransactionsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetInstalmentQuotesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetInstalmentQuotesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetListResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetListResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetOnlineClientListResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetOpenTransactionsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetOptionSettingResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetOptionSettingResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPartyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetPartyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPartySummaryResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetPartySummaryResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetProductByAgentResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetProductByAgentResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRatingDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetRatingDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRiskByProductResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetRiskByProductResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRiskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetRiskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetUserDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetUserDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseLogoffResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BasePostDocumentResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(PostDocumentResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseRunDefaultRulesAddResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(RunDefaultRulesAddResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseRunDefaultRulesEditResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(RunDefaultRulesEditResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseRunValidationRulesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(RunValidationRulesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseSaveRiskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(SaveRiskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseTransactResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseProcessPaymentAndBindQuoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(ProcessPaymentAndBindQuoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseBindQuoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BindQuoteResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateClaimRiskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateClaimRiskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdatePartyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdatePartyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdatePartyRiskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdatePartyRiskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateWmTaskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateWmTaskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateRiskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateRiskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetClaimPerilSummaryResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetClaimPerilSummaryResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHeaderAndPolicyFeesByKeyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetHeaderAndPolicyFeesByKeyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetClaimCoinsurerResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetClaimCoinsurerResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHeaderAndRiskFeesByKeyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetHeaderAndRiskFeesByKeyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHeaderAndRisksByKeyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetHeaderAndRisksByKeyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRecoveryReinsuranceResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetRecoveryReinsuranceResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetClaimReinsuranceBandsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetClaimReinsuranceBandsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetClaimReinsuranceArrangementsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetClaimReinsuranceArrangementsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetClaimReinsuranceArrangementLinesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetClaimReinsuranceArrangementLinesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRiskReinsuranceBandsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetRiskReinsuranceBandsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRiskReinsuranceArrangementsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetRiskReinsuranceArrangementsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRiskReinsuranceArrangementLinesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetRiskReinsuranceArrangementLinesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseDeleteWmTaskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(DeleteWmTaskResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddUserGroupResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddUserGroupResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetWorkManagerScheduledTasksResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetWorkManagerScheduledTasksResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetUserGroupsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetUserGroupsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateRiskSelectionResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateRiskSelectionResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindPolicyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(FindPolicyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseCreatePaymentCashListWithItemsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(CreatePaymentCashListWithItemsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseCreatePaymentCashListItemResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(CreatePaymentCashListItemResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPolicyDetailsForBouncedReceiptResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetPolicyDetailsForBouncedReceiptResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseCreateBackgroundJobResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(CreateBackgroundJobResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPeriodResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetPeriodResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetCashDepositResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetCashDepositResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateCashDepositResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateCashDepositResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetNextCashDepositRefResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetNextCashDepositRefResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddCashDepositResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddCashDepositResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAgentCommissionTaxResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetAgentCommissionTaxResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindCashDepositResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(FindCashDepositResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetLinkedCashDepositsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetLinkedCashDepositsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRatingSectionByRiskTypeResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetRatingSectionByRiskTypeResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddDocumentToDocumasterResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddDocumentToDocumasterResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAccountBalanceByAccountCodeResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetAccountBalanceByAccountCodeResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindBankGuaranteeResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(FindBankGuaranteeResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddWmTaskLogResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(AddWmTaskLogResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetWmTaskLogResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetWmTaskLogResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateRatingDetailsResponsetType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateRatingDetailsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetCashDepositsForPolicyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetCashDepositsForPolicyResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateSubAgentsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateSubAgentsResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRecoveryCoinsuranceResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(GetRecoveryCoinsuranceResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateTaxesResponseType)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(UpdateTaxesResponseType)), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"), _
     System.Runtime.Serialization.DataContractAttribute(Name:="SAMMethodResponseData", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20080429"), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(SAMMethodResponseData))> _
    Partial Public Class SAMMethodResponseData

        Private errorsField As System.Collections.Generic.List(Of SAMError)

        Private handlingInstanceIDField As System.Guid

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Errors() As System.Collections.Generic.List(Of SAMError)
            Get
                Return Me.errorsField
            End Get
            Set(value As System.Collections.Generic.List(Of SAMError))
                Me.errorsField = value
            End Set
        End Property

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property HandlingInstanceID() As System.Guid
            Get
                Return Me.handlingInstanceIDField
            End Get
            Set(value As System.Guid)
                Me.handlingInstanceIDField = value
            End Set
        End Property
    End Class


    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(SAMErrorInvalidData)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(SAMErrorBusinessRule)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(SAMErrorFatal)), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(WCFUnhandledError)), _
     System.Runtime.Serialization.DataContractAttribute(Name:="SAMError", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20080429"), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(SAMError))> _
    Partial Public MustInherit Class SAMError
    End Class

    <System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"), _
     System.Runtime.Serialization.DataContractAttribute(Name:="SAMErrorInvalidData", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20080429"), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(SAMErrorInvalidData))> _
    Partial Public Class SAMErrorInvalidData
        Inherits SAMError

        Private codeField As Integer

        Private reasonField As SAMErrorCode

        Private descriptionField As String

        Private fieldNameField As String

        Private suppliedValueField As String

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Code() As Integer
            Get
                Return Me.codeField
            End Get
            Set(value As Integer)
                Me.codeField = value
            End Set
        End Property

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Reason() As SAMErrorCode
            Get
                Return Me.reasonField
            End Get
            Set(value As SAMErrorCode)
                Me.reasonField = value
            End Set
        End Property

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(value As String)
                Me.descriptionField = value
            End Set
        End Property

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property FieldName() As String
            Get
                Return Me.fieldNameField
            End Get
            Set(value As String)
                Me.fieldNameField = value
            End Set
        End Property

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SuppliedValue() As String
            Get
                Return Me.suppliedValueField
            End Get
            Set(value As String)
                Me.suppliedValueField = value
            End Set
        End Property
    End Class

    <System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"), _
     System.Runtime.Serialization.DataContractAttribute(Name:="WCFUnhandledError", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20080429"), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(WCFUnhandledError))> _
    Partial Public Class WCFUnhandledError
        Inherits SAMError

        Private detailField As String

        Private reasonField As SAMErrorCode

        Private descriptionField As String

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(value As String)
                Me.descriptionField = value
            End Set
        End Property

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Detail() As String
            Get
                Return Me.detailField
            End Get
            Set(value As String)
                Me.detailField = value
            End Set
        End Property

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Reason() As SAMErrorCode
            Get
                Return Me.reasonField
            End Get
            Set(value As SAMErrorCode)
                Me.reasonField = value
            End Set
        End Property


    End Class

    <System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"), _
     System.Runtime.Serialization.DataContractAttribute(Name:="SAMErrorCode", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20080429"), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(SAMErrorCode))> _
    Public Enum SAMErrorCode

        <EnumMember> GeneralFailure

        <EnumMember>
        BackOfficeComponentFailed

        <EnumMember>
        MandatoryInputMissing

        <EnumMember>
        InvalidDateFormat

        <EnumMember>
        InvalidLookupListValue

        <EnumMember>
        InvalidFormat

        <EnumMember>
        RecordLockedByAnotherUser

        <EnumMember>
        RecordNotLockedByCurrentUser

        <EnumMember>
        BackOfficeUnavailable

        <EnumMember>
        UserNotAuthorisedToActOnData

        <EnumMember>
        SecurityCheckFailed

        <EnumMember>
        TokenExpired

        <EnumMember>
        RecordChanged

        <EnumMember>
        BranchCodeInvalid

        <EnumMember>
        BranchMismatch

        <EnumMember>
        PolicyMismatch

        <EnumMember>
        PartyDOBIsInFuture

        <EnumMember>
        PartyDOBIsTooOld

        <EnumMember>
        PolicyRiskLinkRecordNotFound

        <EnumMember>
        QuoteHeaderRecordNotFound

        <EnumMember>
        CoverStartDateIsInThePast

        <EnumMember>
        CoverEndDateIsBeforeCoverStartDate

        <EnumMember>
        PartyRecordNotFound

        <EnumMember>
        PolicyRecordNotFound

        <EnumMember>
        FailedToLoadRiskDB

        <EnumMember>
        FailedToRetrievePremiumDetails

        <EnumMember>
        ListTypeNotFound

        <EnumMember>
        AddressRecordNotFound

        <EnumMember>
        RiskRecordNotFound

        <EnumMember>
        DefaultXMLFileNotAvailable

        <EnumMember>
        DefaultXMLFilePathNotFound

        <EnumMember>
        DefaultXMLFileFailedToLoad

        <EnumMember>
        DefaultXMLFilePathTooLong

        <EnumMember>
        ConfigurationFileNotAvailable

        <EnumMember>
        ConfigurationFilePathNotFound

        <EnumMember>
        ConfigurationFileFailedToLoad

        <EnumMember>
        ConfigurationFilePathTooLong

        <EnumMember>
        XMLDocumentBadlyFormed

        <EnumMember>
        FailedToCreateBackofficeComponent

        <EnumMember>
        FailedToInitialiseBackofficeComponent

        <EnumMember>
        BackOfficeFailed

        <EnumMember>
        FailedToConnectToTheSiriusDatabase

        <EnumMember>
        SQLServerReturnedAnError

        <EnumMember>
        FailedToRetrieveDatamodelCodeFromXml

        <EnumMember>
        XMLDataSetBadlyFormed

        <EnumMember>
        DatasetPathRegistrySettingNotFound

        <EnumMember>
        FailedToAddRiskRecord

        <EnumMember>
        FailedToMergeRiskDataset

        <EnumMember>
        UserAuthorityLevelsCheckFailed

        <EnumMember>
        ValidationRulesFailed

        <EnumMember>
        FailedToQuoteTheRisk

        <EnumMember>
        FailedToSaveRiskToDatabase

        <EnumMember>
        SchemeVersionNumberMissing

        <EnumMember>
        SchemaForVersionMissing

        <EnumMember>
        FileNotFound

        <EnumMember>
        RecordNotFound

        <EnumMember>
        StatusOfRiskPreventsDeletion

        <EnumMember>
        AgentRecordNotFound

        <EnumMember>
        BackOfficeComponentReturnedRecordInUse

        <EnumMember>
        BackOfficeComponentReturnedNotFound

        <EnumMember>
        BrokerOrSchemeInvalid

        <EnumMember>
        ValidationRulesReferred

        <EnumMember>
        ValidationRulesDeclined

        <EnumMember>
        UALRulesReferred

        <EnumMember>
        UALRulesDeclined

        <EnumMember>
        RatingRulesReferred

        <EnumMember>
        RatingRulesDeclined

        <EnumMember>
        LoginFailureIncorrectUsername

        <EnumMember>
        LoginFailureIncorrectPassword

        <EnumMember>
        LoginFailureLoggedInElsewhere

        <EnumMember>
        LoginFailureNotLinkedToAgent
    End Enum

    <System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"), _
     System.Runtime.Serialization.DataContractAttribute(Name:="SAMErrorBusinessRule", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20080429"), _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(SAMErrorBusinessRule))> _
    Partial Public Class SAMErrorBusinessRule
        Inherits SAMError

        Private codeField As Integer

        Private descriptionField As String

        Private detailField As String

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Code() As Integer
            Get
                Return Me.codeField
            End Get
            Set(value As Integer)
                Me.codeField = value
            End Set
        End Property

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(value As String)
                Me.descriptionField = value
            End Set
        End Property

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Detail() As String
            Get
                Return Me.detailField
            End Get
            Set(value As String)
                Me.detailField = value
            End Set
        End Property
    End Class

    <System.Diagnostics.DebuggerStepThroughAttribute(), _
      System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"), _
      System.Runtime.Serialization.DataContractAttribute(Name:="SAMErrorFatal", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20080429"), _
      System.Runtime.Serialization.KnownTypeAttribute(GetType(SAMErrorFatal))> _
    Partial Public Class SAMErrorFatal
        Inherits SAMError

        Private typeField As String

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set(value As String)
                Me.typeField = value
            End Set
        End Property
    End Class

End Namespace