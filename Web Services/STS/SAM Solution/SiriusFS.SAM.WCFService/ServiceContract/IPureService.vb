Imports SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.WCF

<ServiceContract(Namespace:="PureService")> _
Public Interface IPureService

    ''' <summary>
    ''' To get logged-in userdetail
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()> _
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserDetails(ByVal oRequest As GetUserDetailsRequestType) As GetUserDetailsResponseType

    ''' <summary>
    ''' To search partied for given search criteria
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindParty(ByVal oRequest As FindPartyRequestType) As FindPartyResponseType

    ''' <summary>
    ''' To Add a new party
    ''' </summary>
    ''' <param name="AddPartyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddParty(ByVal AddPartyRequest As AddPartyRequestType) As AddPartyResponseType

    ''' <summary>
    ''' To get a single party detail for given party id
    ''' </summary>
    ''' <param name="GetPartyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetParty(ByVal GetPartyRequest As GetPartyRequestType) As GetPartyResponseType

    ''' <summary>
    ''' TO update a party with details provided in request
    ''' </summary>
    ''' <param name="UpdatePartyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateParty(ByVal UpdatePartyRequest As UpdatePartyRequestType) As UpdatePartyResponseType
    ''' <summary>  
    '''  This method calls core implementation layer and creates manual journal transactions.
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.AddJournalRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.AddJournalResponseType</returns>  
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddJournal(ByVal oRequest As AddJournalRequestType) As AddJournalResponseType

    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ApproveCashListItem(ByVal ApproveCashListItemRequest As ApproveCashListItemRequestType) As ApproveCashListItemResponseType

    ''' <summary>  
    ''' Find the Policy
    ''' </summary>  
    ''' <param name="oRequest">An object of type FindPolicyRequestType</param>  
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindPolicy(ByVal oRequest As FindPolicyRequestType) As FindPolicyResponseType

    ''' <summary>
    ''' This is webservice method for AddPayNowReceipt
    '''<param name="oRequest" type="AddPayNowReceiptRequestType"></param>   
    '''</summary>
    '''<returns>AddPayNowReceiptResponseType</returns>
    '''<remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddPayNowReceipt(ByVal oRequest As AddPayNowReceiptRequestType) As AddPayNowReceiptResponseType

    'Start (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR33 - Work Manager - Task  Log.doc)-(7.2.4.6)
    ''' <summary>
    ''' This is Business logic for  Add WorkManager Task Log
    '''<param name="oRequest" type="AddWmTaskLogRequestType"></param>   
    '''</summary>
    '''<remarks></remarks>  
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddWmTaskLog(ByVal oRequest As AddWmTaskLogRequestType) As AddWmTaskLogResponseType


    ''' <summary>
    '''This method passes the AddWriteOffRequestType request from web service layer to core implementation layer.
    '''It returns back the result from core implementation layer to web service layer.
    '''</summary>
    '''<param name="oRequest" type="SiriusFS.SAM.SFI.SAMForInsuranceV2.AddWriteOffRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.AddWriteOffResponseType</returns>
    '''<remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddWriteOff(ByVal oRequest As AddWriteOffRequestType) As AddWriteOffResponseType


    ''' <summary>  
    ''' This method is used to find accounts in claim payment
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.FindAccountsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.FindAccountsResponseType</returns>  
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindAccounts(ByVal oRequest As FindAccountsRequestType) As FindAccountsResponseType


    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="FindBankRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.FindBankResponseType</returns>  
    '''<remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindBank(ByVal oRequest As FindBankRequestType) As FindBankResponseType

    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="FindBankGuaranteeRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.FindBankGuaranteeResponseType</returns>  
    '''<remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindBankGuarantee(ByVal oRequest As FindBankGuaranteeRequestType) As FindBankGuaranteeResponseType

    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindControlSearch(ByVal oRequest As FindControlSearchRequestType) As FindControlSearchResponseType

    ''' <summary>  
    '''  This method calls the core implementation layer to find Cover Note Books.   
    ''' </summary>  
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindCoverNoteBooks(ByVal oRequest As FindCoverNoteBooksRequestType) As FindCoverNoteBooksResponseType

    ''' <summary>
    ''' TO update Taxes details provided in request
    ''' </summary>
    ''' <param name="UpdateTaxesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateTaxes(ByVal UpdateTaxesRequest As UpdateTaxesRequestType) As UpdateTaxesResponseType

    ''' <summary>
    ''' TO Update Standard WordingTemplate details provided in request
    ''' </summary>
    ''' <param name="UpdateStandardWordingTemplateRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateStandardWordingTemplate(ByVal UpdateStandardWordingTemplateRequest As UpdateStandardWordingTemplateRequestType) As UpdateStandardWordingTemplateResponseType

    ''' <summary>
    ''' TO Update Risk Selection details provided in request
    ''' </summary>
    ''' <param name="UpdateRiskSelectionRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateRiskSelection(ByVal UpdateRiskSelectionRequest As UpdateRiskSelectionRequestType) As UpdateRiskSelectionResponseType

    ''' <summary>
    ''' TO Update Risk details provided in request
    ''' </summary>
    ''' <param name="UpdateRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateRisk(ByVal UpdateRiskRequest As UpdateRiskRequestType) As UpdateRiskResponseType

    ''' <summary>
    ''' TO Update Renewal Status details provided in request
    ''' </summary>
    ''' <param name="UpdateRenewalStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateRenewalStatus(ByVal UpdateRenewalStatusRequest As UpdateRenewalStatusRequestType) As UpdateRenewalStatusResponseType

    ''' <summary>
    ''' TO Update Receipt Media Type Status details provided in request
    ''' </summary>
    ''' <param name="UpdateReceiptMediaTypeStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateReceiptMediaTypeStatus(ByVal UpdateReceiptMediaTypeStatusRequest As UpdateReceiptMediaTypeStatusRequestType) As UpdateReceiptMediaTypeStatusResponseType

    ''' <summary>
    ''' TO Update Rating Sections details provided in request
    ''' </summary>
    ''' <param name="UpdateRatingSectionsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateRatingSections(ByVal UpdateRatingSectionsRequest As UpdateRatingDetailsRequestType) As UpdateRatingDetailsResponseType

    ''' <summary>
    ''' TO Update QuoteV2 details provided in request
    ''' </summary>
    ''' <param name="UpdateQuoteV2Request"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateQuoteV2(ByVal UpdateQuoteV2Request As UpdateQuoteV2RequestType) As UpdateQuoteV2ResponseType

    ''' <summary>
    ''' TO Update Quote Status details provided in request
    ''' </summary>
    ''' <param name="UpdateQuoteStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateQuoteStatus(ByVal UpdateQuoteStatusRequest As UpdateQuoteStatusRequestType) As UpdateQuoteStatusResponseType

    ''' <summary>
    ''' TO Update Quote Payment Method details provided in request
    ''' </summary>
    ''' <param name="UpdateQuotePaymentMethodRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateQuotePaymentMethod(ByVal UpdateQuotePaymentMethodRequest As UpdateQuotePaymentMethodRequestType) As UpdateQuotePaymentMethodResponseType

    ''' <summary>
    ''' TO Update Quote  details provided in request
    ''' </summary>
    ''' <param name="UpdateQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateQuote(ByVal UpdateQuoteRequest As UpdateQuoteRequestType) As UpdateQuoteResponseType

    ''' <summary>
    ''' TO Update Finance Plan Details details provided in request
    ''' </summary>
    ''' <param name="UpdateFinancePlanDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateFinancePlanDetails(ByVal UpdateFinancePlanDetailsRequest As UpdateFinancePlanDetailsRequestType) As UpdateFinancePlanDetailsResponseType

    ''' <summary>
    ''' TO Update Coinsurance Values details provided in request
    ''' </summary>
    ''' <param name="UpdateCoinsuranceValuesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateCoinsuranceValues(ByVal UpdateCoinsuranceValuesRequest As UpdateCoinsuranceValuesRequestType) As UpdateCoinsuranceValuesResponseType


    ''' <summary>
    ''' TO Update Bank Guarantee Conditionally details provided in request
    ''' </summary>
    ''' <param name="UpdateBankGuaranteeConditionallyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateBankGuaranteeConditionally(ByVal UpdateBankGuaranteeConditionallyRequest As UpdateBankGuaranteeConditionallyRequestType) As UpdateBankGuaranteeConditionallyResponseType

    ''' <summary>
    ''' TO Update Bank Guarantee details provided in request
    ''' </summary>
    ''' <param name="UpdateBankGuaranteeRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateBankGuarantee(ByVal UpdateBankGuaranteeRequest As UpdateBankGuaranteeRequestType) As UpdateBankGuaranteeResponseType

    ''' <summary>
    ''' TO Update Allocation details provided in request
    ''' </summary>
    ''' <param name="UpdateAllocationRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateAllocation(ByVal UpdateAllocationRequest As UpdateAllocationRequestType) As UpdateAllocationResponseType

    ''' <summary>
    ''' TO Transfer Quote details provided in request
    ''' </summary>
    ''' <param name="TransferQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function TransferQuote(ByVal TransferQuoteRequest As TransferQuoteRequestType) As TransferQuoteResponseType

    ''' <summary>
    ''' TO Save Risk details provided in request
    ''' </summary>
    ''' <param name="SaveRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function SaveRisk(ByVal SaveRiskRequest As SaveRiskRequestType) As SaveRiskResponseType

    ''' <summary>
    ''' TO Process Payment And BindQuot edetails provided in request
    ''' </summary>
    ''' <param name="ProcessPaymentAndBindQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ProcessPaymentAndBindQuote(ByVal ProcessPaymentAndBindQuoteRequest As ProcessPaymentAndBindQuoteRequestType) As ProcessPaymentAndBindQuoteResponseType

    ''' <summary>
    ''' TO update Taxes details provided in request
    ''' </summary>
    ''' <param name="GetRiskByProductRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetRiskByProduct(ByVal GetRiskByProductRequest As GetRiskByProductRequestType) As GetRiskByProductResponseType


    ''' <summary>
    ''' TO Get Risk details provided in request
    ''' </summary>
    ''' <param name="GetRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetRisk(ByVal GetRiskRequest As GetRiskRequestType) As GetRiskResponseType

    ''' <summary>
    ''' TO update Taxes details provided in request
    ''' </summary>
    ''' <param name="GetRatingSectionByRiskTypeRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetRatingSectionByRiskType(ByVal GetRatingSectionByRiskTypeRequest As GetRatingSectionByRiskTypeRequestType) As GetRatingSectionByRiskTypeResponseType

    ''' <summary>
    ''' TO Get Quotes Marked For Collection details provided in request
    ''' </summary>
    ''' <param name="GetQuotesMarkedForCollectionRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetQuotesMarkedForCollection(ByVal GetQuotesMarkedForCollectionRequest As GetQuotesMarkedForCollectionRequestType) As GetQuotesMarkedForCollectionResponseType

    ''' <summary>
    ''' TO GetProductRiskOptionValue details provided in request
    ''' </summary>
    ''' <param name="GetProductRiskOptionValueRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetProductRiskOptionValue(ByVal GetProductRiskOptionValueRequest As ProductRiskOptionValueRequestType) As ProductRiskOptionValueResponseType

    ''' <summary>
    ''' TO update Taxes details provided in request
    ''' </summary>
    ''' <param name="GetProductRiskEventsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetProductRiskEvents(ByVal GetProductRiskEventsRequest As GetProductRiskEventsRequestType) As GetProductRiskEventsResponseType

    ''' <summary>
    ''' TO Get Instalment Quotes provided in request
    ''' </summary>
    ''' <param name="GetInstalmentQuotesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetInstalmentQuotes(ByVal GetInstalmentQuotesRequest As GetInstalmentQuotesRequestType) As GetInstalmentQuotesResponseType


    ''' <summary>
    ''' TO GetDefaultRiskClauses details provided in request
    ''' </summary>
    ''' <param name="GetDefaultRiskClausesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetDefaultRiskClauses(ByVal GetDefaultRiskClausesRequest As GetDefaultRiskClausesRequestType) As GetDefaultRiskClausesResponseType

    ''' <summary>
    ''' TO GetBackdatedMTARiskVersions details provided in request
    ''' </summary>
    ''' <param name="GetBackdatedMTARiskVersionsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetBackdatedMTARiskVersions(ByVal GetBackdatedMTARiskVersionsRequest As GetHeaderAndSummariesByKeyRequestType) As AddBackDatedMTAQuoteResponseType

    ''' <summary>
    ''' TO Delete Risk details provided in request
    ''' </summary>
    ''' <param name="DeleteRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function DeleteRisk(ByVal DeleteRiskRequest As DeleteRiskRequestType) As DeleteRiskResponseType

    ''' <summary>
    ''' TO Delete Policy details provided in request
    ''' </summary>
    ''' <param name="DeletePolicyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function DeletePolicy(ByVal DeletePolicyRequest As DeletePolicyRequestType) As DeletePolicyResponseType


    ''' <summary>
    ''' TO Copy Risk details provided in request
    ''' </summary>
    ''' <param name="CopyRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CopyRisk(ByVal CopyRiskRequest As CopyRiskRequestType) As CopyRiskResponseType

    ''' <summary>
    ''' TO Copy Quote details provided in request
    ''' </summary>
    ''' <param name="CopyQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CopyQuote(ByVal CopyQuoteRequest As CopyQuoteRequestType) As CopyQuoteResponseType

    ''' <summary>
    ''' TO Bind Quote provided in request
    ''' </summary>
    ''' <param name="BindQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function BindQuote(ByVal BindQuoteRequest As BindQuoteRequestType) As BindQuoteResponseType

    ''' <summary>
    ''' TO Add Risk details provided in request
    ''' </summary>
    ''' <param name="AddRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddRisk(ByVal AddRiskRequest As AddRiskRequestType) As AddRiskResponseType

    ''' <summary>
    ''' TO update Taxes details provided in request
    ''' </summary>
    ''' <param name="AddQuoteV2Request"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddQuoteV2(ByVal AddQuoteV2Request As AddQuoteV2RequestType) As AddQuoteV2ResponseType

    ''' <summary>
    ''' TO Add Quote details provided in request
    ''' </summary>
    ''' <param name="AddQuote"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddQuote(ByVal AddQuoteRequest As AddQuoteRequestType) As AddQuoteResponseType

    ''' <summary>
    ''' TO Add MtaQuote details provided in request
    ''' </summary>
    ''' <param name="AddMtaQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddMtaQuote(ByVal AddMtaQuoteRequest As AddMtaQuoteRequestType) As AddMtaQuoteResponseType

    ''' <summary>
    ''' TO Add Back Dated MTAQuote details provided in request
    ''' </summary>
    ''' <param name="AddBackDatedMTAQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddBackDatedMTAQuote(ByVal AddBackDatedMTAQuoteRequest As AddBackDatedMTAQuoteRequestType) As AddBackDatedMTAQuoteResponseType

    ''' <summary>
    '''This method passes the FindUsersRequestType request from web service layer to core implementation layer.
    '''It returns back the result from core implementation layer to web service layer.
    '''</summary>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindUsers(ByVal oRequest As FindUsersRequestType) As FindUsersResponseType

    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindDMEDocuments(ByVal oRequest As FindDMEDocumentsRequestType) As FindDMEDocumentsResponseType

    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindDocumentTemplates(ByVal oRequest As FindDocumentTemplatesRequestType) As FindDocumentTemplatesResponseType

    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimCoinsurer(ByVal oRequest As GetClaimCoinsurerRequestType) As GetClaimCoinsurerResponseType

    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimDetails(ByVal oRequest As GetClaimDetailsRequestType) As GetClaimDetailsResponseType

    ''' TO get document provided in request
    ''' </summary>
    ''' <param name="GetDocumentRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetDocument(ByVal GetDocumentRequest As GetDocumentRequestType) As GetDocumentResponseType

    ''' <summary>
    ''' TO get document defaults provided in request
    ''' </summary>
    ''' <param name="GetDocumentDefaultsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetDocumentDefaults(ByVal GetDocumentDefaultsRequest As GetDocumentDefaultsRequestType) As GetDocumentDefaultsResponseType

    ''' <summary>
    ''' TO Get Cover Note Sheet provided in request
    ''' </summary>
    ''' <param name="GetCoverNoteSheetRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetCoverNoteSheet(ByVal GetCoverNoteSheetRequest As GetCoverNoteSheetRequestType) As GetCoverNoteSheetResponseType

    ''' <summary>
    ''' TO Get Cover Note Book provided in request
    ''' </summary>
    ''' <param name="GetCoverNoteBookRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetCoverNoteBook(ByVal GetCoverNoteBookRequest As GetCoverNoteBookRequestType) As GetCoverNoteBookResponseType


    ''' <summary>
    ''' TO Generate Documents For Event provided in request
    ''' </summary>
    ''' <param name="GenerateDocumentsForEventRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GenerateDocumentsForEvent(ByVal GenerateDocumentsForEventRequest As GenerateDocumentsForEventRequestType) As GenerateDocumentsForEventResponseType

    ''' <summary>
    ''' </summary>
    ''' <param name="GenerateDocumentRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GenerateDocument(ByVal GenerateDocumentRequest As GenerateDocumentRequestType) As GenerateDocumentResponseType


    ''' <summary>
    ''' TO Get Cover Note Book provided in request
    ''' </summary>
    ''' <param name="ForgottenPasswordRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ForgottenPassword(ByVal ForgottenPasswordRequest As ForgottenPasswordRequestType) As ForgottenPasswordResponseType


    ''' <summary>
    ''' TO DeleteWmTask provided in request
    ''' </summary>
    ''' <param name="DeleteWmTaskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function DeleteWmTask(ByVal DeleteWmTaskRequest As DeleteWmTaskRequestType) As DeleteWmTaskResponseType


    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetVersionsForClaim(ByVal oRequest As GetVersionsForClaimRequestType) As GetVersionsForClaimResponseType

    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUnallocatedClaimPayments(ByVal oRequest As GetUnallocatedClaimPaymentsRequestType) As GetUnallocatedClaimPaymentsResponseType

    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimRisk(ByVal oRequest As GetClaimRiskRequestType) As GetClaimRiskResponseType

    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimRiskReadOnly(ByVal GetClaimRiskReadOnlyRequest As GetClaimRiskReadOnlyRequestType) As GetClaimRiskReadOnlyResponseType

    ''' <summary>
    ''' TO Update Sub Agents details provided in request
    ''' </summary>
    ''' <param name="UpdateSubAgentsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateSubAgents(ByVal UpdateSubAgentsRequest As UpdateSubAgentsRequestType) As UpdateSubAgentsResponseType

    ''' <summary>
    ''' TO Update Cash Deposit details provided in request
    ''' </summary>
    ''' <param name="UpdateCashDepositRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateCashDeposit(ByVal UpdateCashDepositRequest As UpdateCashDepositRequestType) As UpdateCashDepositResponseType

    ''' <summary>
    ''' TO UpdateAgentCommission details provided in request
    ''' </summary>
    ''' <param name="UpdateAgentCommissionRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateAgentCommission(ByVal UpdateAgentCommissionRequest As UpdateAgentCommissionRequestType) As UpdateAgentCommissionResponseType

    ''' <summary>
    ''' TO MarkUnmarkTransaction details provided in request
    ''' </summary>
    ''' <param name="MarkUnmarkTransactionRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function MarkUnmarkTransaction(ByVal MarkUnmarkTransactionRequest As MarkUnmarkTransactionRequestType) As MarkUnmarkTransactionResponseType

    ''' <summary>
    ''' TO LapseRenewal details provided in request
    ''' </summary>
    ''' <param name="LapseRenewalRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function LapseRenewal(ByVal LapseRenewalRequest As LapseRenewalRequestType) As LapseRenewalResponseType

    ''' <summary>
    ''' TO GetSubAgents details provided in request
    ''' </summary>
    ''' <param name="GetSubAgentsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetSubAgents(ByVal GetSubAgentsRequest As GetSubAgentsRequestType) As GetSubAgentsResponseType

    ''' <summary>
    ''' TO GetRiskReinsuranceBands details provided in request
    ''' </summary>
    ''' <param name="GetRiskReinsuranceBandsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetRiskReinsuranceBands(ByVal GetRiskReinsuranceBandsRequest As GetRiskReinsuranceBandsRequestType) As GetRiskReinsuranceBandsResponseType

    ''' <summary>
    ''' TO GetRiskReinsuranceArrangements details provided in request
    ''' </summary>
    ''' <param name="GetRiskReinsuranceArrangementsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetRiskReinsuranceArrangements(ByVal GetRiskReinsuranceArrangementsRequest As GetRiskReinsuranceArrangementsRequestType) As GetRiskReinsuranceArrangementsResponseType

    ''' <summary>
    ''' TO GetRiskReinsuranceArrangementLines details provided in request
    ''' </summary>
    ''' <param name="GetRiskReinsuranceArrangementLinesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetRiskReinsuranceArrangementLines(ByVal GetRiskReinsuranceArrangementLinesRequest As GetRiskReinsuranceArrangementLinesRequestType) As GetRiskReinsuranceArrangementLinesResponseType

    ''' <summary>
    ''' TO GetRecoveryReinsurance details provided in request
    ''' </summary>
    ''' <param name="GetRecoveryReinsuranceRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetRecoveryReinsurance(ByVal GetRecoveryReinsuranceRequest As GetRecoveryReinsuranceRequestType) As GetRecoveryReinsuranceResponseType

    ''' <summary>
    ''' TO GetReceiptCashListItems details provided in request
    ''' </summary>
    ''' <param name="GetReceiptCashListItemsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetReceiptCashListItems(ByVal GetReceiptCashListItemsRequest As GetReceiptCashListItemsRequestType) As GetReceiptCashListItemsResponseType

    ''' <summary>
    ''' TO GetReceiptCashListItemDetails details provided in request
    ''' </summary>
    ''' <param name="GetReceiptCashListItemDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetReceiptCashListItemDetails(ByVal GetReceiptCashListItemDetailsRequest As GetReceiptCashListItemDetailsRequestType) As GetReceiptCashListItemDetailsResponseType

    ''' <summary>
    ''' TO GetReceiptCashListDetails details provided in request
    ''' </summary>
    ''' <param name="GetReceiptCashListDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetReceiptCashListDetails(ByVal GetReceiptCashListDetailsRequest As GetReceiptCashListDetailsRequestType) As GetReceiptCashListDetailsResponseType

    ''' <summary>
    ''' TO GetProductByAgent details provided in request
    ''' </summary>
    ''' <param name="GetProductByAgentRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetProductByAgent(ByVal GetProductByAgentRequest As GetProductByAgentRequestType) As GetProductByAgentResponseType

    ''' <summary>
    ''' TO GetPaymentCashListItems details provided in request
    ''' </summary>
    ''' <param name="GetPaymentCashListItemsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPaymentCashListItems(ByVal GetPaymentCashListItemsRequest As GetPaymentCashListItemsRequestType) As GetPaymentCashListItemsResponseType

    ''' <summary>
    ''' TO GetPaymentCashListItemDetails details provided in request
    ''' </summary>
    ''' <param name="GetPaymentCashListItemDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPaymentCashListItemDetails(ByVal GetPaymentCashListItemDetailsRequest As GetPaymentCashListItemDetailsRequestType) As GetPaymentCashListItemDetailsResponseType

    ''' <summary>
    ''' TO GetPaymentCashListDetails details provided in request
    ''' </summary>
    ''' <param name="GetPaymentCashListDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPaymentCashListDetails(ByVal GetPaymentCashListDetailsRequest As GetPaymentCashListDetailsRequestType) As GetPaymentCashListDetailsResponseType

    ''' <summary>
    ''' TO GetNextCashDepositRef details provided in request
    ''' </summary>
    ''' <param name="GetNextCashDepositRefRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetNextCashDepositRef(ByVal GetNextCashDepositRefRequest As GetNextCashDepositRefRequestType) As GetNextCashDepositRefResponseType

    ''' <summary>
    ''' TO GetLinkedCashDeposits details provided in request
    ''' </summary>
    ''' <param name="GetLinkedCashDepositsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetLinkedCashDeposits(ByVal GetLinkedCashDepositsRequest As GetLinkedCashDepositsRequestType) As GetLinkedCashDepositsResponseType

    ''' <summary>
    ''' TO GetClaimReinsuranceBands details provided in request
    ''' </summary>
    ''' <param name="GetClaimReinsuranceBandsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimReinsuranceBands(ByVal GetClaimReinsuranceBandsRequest As GetClaimReinsuranceBandsRequestType) As GetClaimReinsuranceBandsResponseType

    ''' <summary>
    ''' TO GetClaimReinsuranceArrangements details provided in request
    ''' </summary>
    ''' <param name="GetClaimReinsuranceArrangementsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimReinsuranceArrangements(ByVal GetClaimReinsuranceArrangementsRequest As GetClaimReinsuranceArrangementsRequestType) As GetClaimReinsuranceArrangementsResponseType

    ''' <summary>
    ''' TO GetClaimReinsuranceArrangementLines details provided in request
    ''' </summary>
    ''' <param name="GetClaimReinsuranceArrangementLinesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimReinsuranceArrangementLines(ByVal GetClaimReinsuranceArrangementLinesRequest As GetClaimReinsuranceArrangementLinesRequestType) As GetClaimReinsuranceArrangementLinesResponseType

    ''' <summary>
    ''' TO GetCashDepositsForPolicy details provided in request
    ''' </summary>
    ''' <param name="GetCashDepositsForPolicyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetCashDepositsForPolicy(ByVal GetCashDepositsForPolicyRequest As GetCashDepositsForPolicyRequestType) As GetCashDepositsForPolicyResponseType

    ''' <summary>
    ''' TO GetCashDeposit details provided in request
    ''' </summary>
    ''' <param name="GetCashDepositRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetCashDeposit(ByVal GetCashDepositRequest As GetCashDepositRequestType) As GetCashDepositResponseType

    ''' <summary>
    ''' TO GetAgentSettings details provided in request
    ''' </summary>
    ''' <param name="GetAgentSettingsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetAgentSettings(ByVal GetAgentSettingsRequest As GetAgentSettingsRequestType) As GetAgentSettingsResponseType

    ''' <summary>
    ''' TO GetAgentCommissionTax details provided in request
    ''' </summary>
    ''' <param name="GetAgentCommissionTaxRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetAgentCommissionTax(ByVal GetAgentCommissionTaxRequest As GetAgentCommissionTaxRequestType) As GetAgentCommissionTaxResponseType

    ''' <summary>
    ''' TO GetAgentCommission details provided in request
    ''' </summary>
    ''' <param name="GetAgentCommissionRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetAgentCommission(ByVal GetAgentCommissionRequest As GetAgentCommissionRequestType) As GetAgentCommissionResponseType

    ''' <summary>
    ''' TO GenerateInvite details provided in request
    ''' </summary>
    ''' <param name="GenerateInviteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GenerateInvite(ByVal GenerateInviteRequest As GenerateInviteRequestType) As GenerateInviteResponseType

    ''' <summary>
    ''' TO FindCashListReceipts details provided in request
    ''' </summary>
    ''' <param name="FindCashListReceiptsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindCashListReceipts(ByVal FindCashListReceiptsRequest As FindCashListReceiptsRequestType) As FindCashListReceiptsResponseType

    ''' <summary>
    ''' TO FindCashDeposit details provided in request
    ''' </summary>
    ''' <param name="FindCashDepositRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindCashDeposit(ByVal FindCashDepositRequest As FindCashDepositRequestType) As FindCashDepositResponseType

    ''' <summary>
    ''' TO DeleteRenewal details provided in request
    ''' </summary>
    ''' <param name="DeleteRenewalRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function DeleteRenewal(ByVal DeleteRenewalRequest As DeleteRenewalRequestType) As DeleteRenewalResponseType

    ''' <summary>
    ''' TO DeleteBackDatedVersions details provided in request
    ''' </summary>
    ''' <param name="DeleteBackDatedVersionsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function DeleteBackDatedVersions(ByVal DeleteBackDatedVersionsRequest As DeleteBackDatedVersionsRequestType) As DeleteBackDatedVersionsResponseType

    ''' <summary>
    ''' TO CreateReceiptCashListWithItems details provided in request
    ''' </summary>
    ''' <param name="CreateReceiptCashListWithItemsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CreateReceiptCashListWithItems(ByVal CreateReceiptCashListWithItemsRequest As CreateReceiptCashListWithItemsRequestType) As CreateReceiptCashListWithItemsResponseType

    ''' <summary>
    ''' TO CreateReceiptCashListItem details provided in request
    ''' </summary>
    ''' <param name="CreateReceiptCashListItemRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CreateReceiptCashListItem(ByVal CreateReceiptCashListItemRequest As CreateReceiptCashListItemRequestType) As CreateReceiptCashListItemResponseType

    ''' <summary>
    ''' TO CreatePaymentCashListWithItems details provided in request
    ''' </summary>
    ''' <param name="CreatePaymentCashListWithItemsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CreatePaymentCashListWithItems(ByVal CreatePaymentCashListWithItemsRequest As CreatePaymentCashListWithItemsRequestType) As CreatePaymentCashListWithItemsResponseType

    ''' <summary>
    ''' TO CreatePaymentCashListItem details provided in request
    ''' </summary>
    ''' <param name="CreatePaymentCashListItemRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CreatePaymentCashListItem(ByVal CreatePaymentCashListItemRequest As CreatePaymentCashListItemRequestType) As CreatePaymentCashListItemResponseType

    ''' <summary>
    ''' TO CreateBackgroundJob details provided in request
    ''' </summary>
    ''' <param name="CreateBackgroundJobRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CreateBackgroundJob(ByVal CreateBackgroundJobRequest As CreateBackgroundJobRequestType) As CreateBackgroundJobResponseType

    ''' <summary>
    ''' TO ClientDataImport details provided in request
    ''' </summary>
    ''' <param name="ClientDataImportRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ClientDataImport(ByVal ClientDataImportRequest As ClientDataImportRequestType) As ClientDataImportResponseType

    ''' <summary>
    ''' TO CheckUnpaidPremium details provided in request
    ''' </summary>
    ''' <param name="CheckUnpaidPremiumRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CheckUnpaidPremium(ByVal CheckUnpaidPremiumRequest As CheckUnpaidPremiumRequestType) As CheckUnpaidPremiumResponseType
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddAgentReceipt(ByVal AddAgentReceiptRequest As AddAgentReceiptRequestType) As AddAgentReceiptResponseType

    ''' <summary>
    ''' To Add a Bank Guarantee with details provided in request
    ''' </summary>
    ''' <param name="oAddBankGuaranteeRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddBankGuarantee(ByVal oAddBankGuaranteeRequest As AddBankGuaranteeRequestType) As AddBankGuaranteeResponseType

    ''' <summary>
    ''' To Add a Cash Deposit with details provided in request
    ''' </summary>
    ''' <param name="oAddCashDepositRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddCashDeposit(ByVal oAddCashDepositRequest As AddCashDepositRequestType) As AddCashDepositResponseType

    ''' <summary>
    ''' To Add a Cover Notebook with details provided in request
    ''' </summary>
    ''' <param name="oAddCoverNoteBookRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddCoverNoteBook(ByVal oAddCoverNoteBookRequest As AddCoverNoteBookRequestType) As AddCoverNoteBookResponseType

    ''' <summary>
    ''' To Add a Cover NoteSheet with details provided in request
    ''' </summary>
    ''' <param name="oAddCoverNoteSheetRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddCoverNoteSheet(ByVal oAddCoverNoteSheetRequest As AddCoverNoteSheetRequestType) As AddCoverNoteSheetResponseType

    ''' <summary>
    ''' To Add a Documents to Documaster with details provided in request
    ''' </summary>
    ''' <param name="AddDocumentToDocumasterRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddDocumentToDocumaster(ByVal AddDocumentToDocumasterRequest As AddDocumentToDocumasterRequestType) As AddDocumentToDocumasterResponseType

    ''' <summary>
    ''' To Add an Event with details provided in request
    ''' </summary>
    ''' <param name="oAddEventRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddEvent(ByVal oAddEventRequest As AddEventRequestType) As AddEventResponseType

    ''' <summary>
    ''' This Web method is used to add the Event Notes details by passing the Request parameters as request type objects
    ''' and also the response object event key and EventPublicTextKey is being returned .
    '''</summary>
    '''<param name="oAddEventNoteRequest" type="AddEventNoteRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.AddEventNoteResponseType</returns>  
    '''<remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddEventNote(ByVal oAddEventNoteRequest As AddEventNoteRequestType) As AddEventNoteResponseType

    ''' <summary>
    ''' This is webservice method for GetPoliciesOnBankGuaranteeForReceipt
    '''<param name="oGetPoliciesOnBankGuaranteeForReceiptRequest" type="GetPoliciesOnBankGuaranteeForReceiptRequestType"></param>   
    '''<returns>GetPoliciesOnBankGuaranteeForReceiptResponseType</returns>
    '''</summary>
    '''<remarks></remarks> 
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPoliciesOnBankGuaranteeByKey(ByVal oGetPoliciesOnBankGuaranteeByKeyRequest As GetPoliciesOnBankGuaranteeByKeyRequestType) As GetPoliciesOnBankGuaranteeByKeyResponseType

    ''' <summary>
    ''' This is webservice method for GetPoliciesOnBankGuaranteeForReceipt
    '''<param name="oGetPoliciesOnBankGuaranteeForReceiptRequest" type="GetPoliciesOnBankGuaranteeForReceiptRequestType"></param>   
    '''<returns>GetPoliciesOnBankGuaranteeForReceiptResponseType</returns>
    '''</summary>
    '''<remarks></remarks> 
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPoliciesOnBankGuaranteeForReceipt(ByVal oGetPoliciesOnBankGuaranteeForReceiptRequest As GetPoliciesOnBankGuaranteeForReceiptRequestType) As GetPoliciesOnBankGuaranteeForReceiptResponseType

    ''' <summary>
    ''' This is webservice method definition for fetching the rating details
    ''' </summary>
    ''' <param name="GetRatingDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetRatingDetails(ByVal GetRatingDetailsRequest As GetRatingDetailsRequestType) As GetRatingDetailsResponseType

    ''' <summary>
    ''' This is webservice method definition for fetching the rating section types
    ''' </summary>
    ''' <param name="oGetRatingSectionTypesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
   <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetRatingSectionTypes(ByVal oGetRatingSectionTypesRequest As GetRatingSectionTypesRequestType) As GetRatingSectionTypesResponseType

    ''' <summary>
    ''' This Web method is used to get all the aithorise payment list by paddding branchcode as request type objects and getting the result as XML
    '''</summary>
    '''<param name="oGetReferredPaymentsRequest" type="GetReferredPaymentsRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetReferredPaymentsResponseType</returns>  
    '''<remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetReferredPayments(ByVal oGetReferredPaymentsRequest As GetReferredPaymentsRequestType) As GetReferredPaymentsResponseType

    ''' <summary>
    ''' This Web method is used to get the renewal status
    '''</summary>
    '''<param name="GetRenewalStatusRequest" type="GetReferredPaymentsRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetReferredPaymentsResponseType</returns>  
    '''<remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetRenewalStatus(ByVal GetRenewalStatusRequest As GetRenewalStatusRequestType) As GetRenewalStatusResponseType

    ''' <summary>
    ''' This Web method is used to get the report
    ''' </summary>
    ''' <param name="oGetReportRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
   <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetReport(ByVal oGetReportRequest As GetReportRequestType) As GetReportResponseType

    ''' <summary>
    ''' This Web method is used to get the sharepoint file list
    ''' </summary>
    ''' <param name="GetSharepointFileListRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
   <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetSharepointFileList(ByVal GetSharepointFileListRequest As GetSharepointFileListRequestType) As GetSharepointFileListResponseType

    ''' <summary>
    ''' This Web method is used to get the standard wording template
    ''' </summary>
    ''' <param name="oGetStandardWordingTemplateRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
   <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetStandardWordingTemplate(ByVal oGetStandardWordingTemplateRequest As GetStandardWordingTemplateRequestType) As GetStandardWordingTemplateResponseType

    ''' <summary>
    ''' This Web method is used to get the taxes 
    ''' </summary>
    ''' <param name="GetTaxesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
   <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetTaxes(ByVal GetTaxesRequest As GetTaxesRequestType) As GetTaxesResponseType

    ''' <summary>
    ''' This Web method is used to get all the transaction details
    ''' </summary>
    ''' <param name="oGetTransactionDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
   <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetTransactionDetails(ByVal oGetTransactionDetailsRequest As GetTransactionDetailsRequestType) As GetTransactionDetailsResponseType

    ''' <summary>
    ''' This Web method is used to get all the Valid Primary Causes
    ''' </summary>
    ''' <param name="GetValidPrimaryCausesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetValidPrimaryCauses(ByVal GetValidPrimaryCausesRequest As GetValidPrimaryCausesRequestType) As GetValidPrimaryCausesResponseType

    ''' <summary>
    ''' This Web method is used to get all the work manager scheduled tasks
    ''' </summary>
    ''' <param name="GetWorkManagerScheduledTasksRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
      <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetWorkManagerScheduledTasks(ByVal GetWorkManagerScheduledTasksRequest As GetWorkManagerScheduledTasksRequestType) As GetWorkManagerScheduledTasksResponseType

    ''' <summary>
    ''' This Web method is used to run the default rules files explicitly
    ''' </summary>
    ''' <param name="RunDefaultRulesAddRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
      <FaultContract(GetType(SAMMethodResponseData))> _
    Function RunDefaultRulesAdd(ByVal RunDefaultRulesAddRequest As RunDefaultRulesAddRequestType) As RunDefaultRulesAddResponseType
    ''' <summary>
    ''' TO Add a TaskGroup with details provided in request
    ''' </summary>
    ''' <param name="AddTaskGroupRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddTaskGroup(ByVal AddTaskGroupRequest As AddTaskGroupRequestType) As AddTaskGroupResponseType

    ''' <summary>
    ''' TO Add a UserGroup with details provided in request
    ''' </summary>
    ''' <param name="AddUserGroupRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddUserGroup(ByVal AddUserGroupRequest As AddUserGroupRequestType) As AddUserGroupResponseType

    ''' <summary>
    ''' TO Add a TaskGroup with details provided in request
    ''' </summary>
    ''' <param name="DeleteUndeleteUserGroupRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function DeleteUndeleteUserGroup(ByVal DeleteUndeleteUserGroupRequest As DeleteUndeleteUserGroupRequestType) As DeleteUndeleteUserGroupResponseType

    ''' <summary>
    ''' TO Add a TaskGroup with details provided in request
    ''' </summary>
    ''' <param name="GetAccountBalanceRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetAccountBalance(ByVal GetAccountBalanceRequest As GetAccountBalanceRequestType) As GetAccountBalanceResponseType

    ''' <summary>
    ''' TO Add a TaskGroup with details provided in request
    ''' </summary>
    ''' <param name="GetAccountBalanceByAccountCodeRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetAccountBalanceByAccountCode(ByVal GetAccountBalanceByAccountCodeRequest As GetAccountBalanceByAccountCodeRequestType) As GetAccountBalanceByAccountCodeResponseType

    ''' <summary>
    ''' TO Get Account details 
    ''' </summary>
    ''' <param name="GetAccountDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetAccountDetails(ByVal GetAccountDetailsRequest As GetAccountDetailsRequestType) As GetAccountDetailsResponseType

    ''' <summary>
    ''' TO Get Account Period with details provided in request
    ''' </summary>
    ''' <param name="GetAccountingPeriodRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetAccountingPeriod(ByVal GetAccountingPeriodRequest As GetAccountingPeriodRequestType) As GetAccountingPeriodResponseType

    ''' <summary>
    ''' TO Get bank account details provided in request
    ''' </summary>
    ''' <param name="GetBankAccountsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetBankAccounts(ByVal GetBankAccountsRequest As GetBankAccountsRequestType) As GetBankAccountsResponseType

    ''' <summary>
    ''' TO Get ClaimPaymentTaxGroups with details provided in request
    ''' </summary>
    ''' <param name="GetClaimPaymentTaxGroupsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimPaymentTaxGroups(ByVal GetClaimPaymentTaxGroupsRequest As GetClaimPaymentTaxGroupsRequestType) As GetClaimPaymentTaxGroupsResponseType

    ''' <summary>
    ''' TO Get ClaimReceiptTaxGroups with details provided in request
    ''' </summary>
    ''' <param name="GetClaimReceiptTaxGroupsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimReceiptTaxGroups(ByVal GetClaimReceiptTaxGroupsRequest As GetClaimReceiptTaxGroupsRequestType) As GetClaimReceiptTaxGroupsResponseType

    ''' <summary>
    ''' TO Get EventNote Type with details provided in request
    ''' </summary>
    ''' <param name="GetEventNoteTypeRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetEventNoteType(ByVal GetEventNoteTypeRequest As GetEventNoteTypeRequestType) As GetEventNoteTypeResponseType

    ''' <summary>
    ''' TO Get Existing Instalment Plan Payment Details with details provided in request
    ''' </summary>
    ''' <param name="GetExistingInstalmentPlanPaymentDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetExistingInstalmentPlanPaymentDetails(ByVal GetExistingInstalmentPlanPaymentDetailsRequest As GetExistingInstalmentPlanPaymentDetailsRequestType) As GetExistingInstalmentPlanPaymentDetailsResponseType

    ''' <summary>
    ''' TO Get the Finance plan details provided in request
    ''' </summary>
    ''' <param name="GetFinancePlanDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetFinancePlanDetails(ByVal GetFinancePlanDetailsRequest As GetFinancePlanDetailsRequestType) As GetFinancePlanDetailsResponseType

    ''' <summary>
    ''' TO Get the Finance plan details provided in request
    ''' </summary>
    ''' <param name="GetFinancePlansRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetFinancePlans(ByVal GetFinancePlansRequest As GetFinancePlansRequestType) As GetFinancePlansResponseType


    ''' <summary>
    ''' TO Get Insurer Payments details provided in request
    ''' </summary>
    ''' <param name="GetInsurerPaymentsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetInsurerPayments(ByVal GetInsurerPaymentsRequest As GetInsurerPaymentsRequestType) As GetInsurerPaymentsResponseType

    ''' <summary>
    ''' TO Get List  details provided in request
    ''' </summary>
    ''' <param name="GetListRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetList(ByVal GetListRequest As GetListRequestType) As GetListResponseType

    ''' <summary>
    ''' TO Get MIDFile details provided in request
    ''' </summary>
    ''' <param name="GetMIDFileDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetMIDFileDetails(ByVal GetMIDFileDetailsRequest As GetMIDFileDetailsRequestType) As GetMIDFileDetailsResponseType

    ''' <summary>
    ''' TO Get MID Files details provided in request
    ''' </summary>
    ''' <param name="GetMIDFilesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetMIDFiles(ByVal GetMIDFilesRequest As GetMIDFilesRequestType) As GetMIDFilesResponseType

    ''' <summary>
    ''' TO Get Media Type details provided in request
    ''' </summary>
    ''' <param name="GetMediaTypeRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetMediaType(ByVal GetMediaTypeRequest As GetMediaTypeRequestType) As GetMediaTypeResponseType

    ''' <summary>
    ''' TO Get Numbering Scheme No details provided in request
    ''' </summary>
    ''' <param name="GetNumberingSchemeNoRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetNumberingSchemeNo(ByVal GetNumberingSchemeNoRequest As GetNumberingSchemeNoRequestType) As GetNumberingSchemeNoResponseType

    ''' <summary>
    ''' TO Get Option Setting details provided in request
    ''' </summary>
    ''' <param name="GetOptionSettingRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetOptionSetting(ByVal GetOptionSettingRequest As GetOptionSettingRequestType) As GetOptionSettingResponseType

    ''' <summary>
    ''' TO Get Period details provided in request
    ''' </summary>
    ''' <param name="GetPeriodRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPeriod(ByVal GetPeriodRequest As GetPeriodRequestType) As GetPeriodResponseType

    ''' <summary>
    ''' TO Get Policies For Renewal Selection details provided in request
    ''' </summary>
    ''' <param name="GetPoliciesForRenewalSelectionRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPoliciesForRenewalSelection(ByVal GetPoliciesForRenewalSelectionRequest As GetPoliciesForRenewalSelectionRequestType) As GetPoliciesForRenewalSelectionResponseType

    ''' <summary>
    ''' TO Get Policies In Renewal details provided in request
    ''' </summary>
    ''' <param name="GetPoliciesInRenewalRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPoliciesInRenewal(ByVal GetPoliciesInRenewalRequest As GetPoliciesInRenewalRequestType) As GetPoliciesInRenewalResponseType

    ''' <summary>
    ''' TO Get Task Group Tasks details provided in request
    ''' </summary>
    ''' <param name="GetTaskGroupTasksRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetTaskGroupTasks(ByVal GetTaskGroupTasksRequest As GetTaskGroupTasksRequestType) As GetTaskGroupTasksResponseType

    ''' <summary>
    ''' TO Get Task Groups details provided in request
    ''' </summary>
    ''' <param name="GetTaskGroupsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetTaskGroups(ByVal GetTaskGroupsRequest As GetTaskGroupsRequestType) As GetTaskGroupsResponseType

    ''' <summary>
    ''' TO Get Tax Groups For Claims details provided in request
    ''' </summary>
    ''' <param name="GetTaxGroupsForClaimsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetTaxGroupsForClaims(ByVal GetTaxGroupsForClaimsRequest As GetTaxGroupsForClaimsRequestType) As GetTaxGroupsForClaimsResponseType

    ''' <summary>
    ''' TO Get User Group Task Groups details provided in request
    ''' </summary>
    ''' <param name="GetUserGroupTaskGroupsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserGroupTaskGroups(ByVal GetUserGroupTaskGroupsRequest As GetUserGroupTaskGroupsRequestType) As GetUserGroupTaskGroupsResponseType

    ''' <summary>
    ''' TO Get User Group Users details provided in request
    ''' </summary>
    ''' <param name="GetUserGroupUsersRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserGroupUsers(ByVal GetUserGroupUsersRequest As GetUserGroupUsersRequestType) As GetUserGroupUsersResponseType

    ''' <summary>
    ''' TO Attach CoverNote details provided in request
    ''' </summary>
    ''' <param name="AttachCoverNoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AttachCoverNote(ByVal AttachCoverNoteRequest As AttachCoverNoteRequestType) As AttachCoverNoteResponseType

    ''' <summary>
    ''' TO Change Password details 
    ''' </summary>
    ''' <param name="ChangePasswordRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ChangePassword(ByVal ChangePasswordRequest As ChangePasswordRequestType) As ChangePasswordResponseType

    ''' <summary>
    ''' TO CreateWmTask details provided in request 
    ''' </summary>
    ''' <param name="CreateWmTaskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CreateWmTask(ByVal CreateWmTaskRequest As CreateWmTaskRequestType) As CreateWmTaskResponseType

    ''' <summary>
    ''' TO Delete Cover Note Sheet details provided in request 
    ''' </summary>
    ''' <param name="DeleteCoverNoteSheetRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function DeleteCoverNoteSheet(ByVal DeleteCoverNoteSheetRequest As DeleteCoverNoteSheetRequestType) As DeleteCoverNoteSheetResponseType

    ''' <summary>
    ''' TO Get User Groups details provided in request 
    ''' </summary>
    ''' <param name="GetUserGroupsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserGroups(ByVal GetUserGroupsRequest As GetUserGroupsRequestType) As GetUserGroupsResponseType

    ''' <summary>
    ''' TO Get User Groups by Task details provided in request 
    ''' </summary>
    ''' <param name="GetUserGroupsbyTaskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserGroupsbyTask(ByVal GetUserGroupsbyTaskRequest As GetUserGroupsbyTaskRequestType) As GetUserGroupsbyTaskResponseType

    ''' <summary>
    ''' TO Update Task Groups details provided in request 
    ''' </summary>
    ''' <param name="UpdateTaskGroupsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateTaskGroups(ByVal UpdateTaskGroupsRequest As UpdateTaskGroupsRequestType) As UpdateTaskGroupsResponseType

    ''' <summary>
    ''' TO Update Task Group Tasks provided in request 
    ''' </summary>
    ''' <param name="UpdateTaskGroupTasksRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateTaskGroupTasks(ByVal UpdateTaskGroupTasksRequest As UpdateTaskGroupTasksRequestType) As UpdateTaskGroupTasksResponseType

    ''' <summary>
    ''' TO Update User Group provided in request 
    ''' </summary>
    ''' <param name="UpdateUserGroupRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateUserGroup(ByVal UpdateUserGroupRequest As UpdateUserGroupRequestType) As UpdateUserGroupResponseType

    ''' <summary>
    ''' TO Update User Group Users details provided in request 
    ''' </summary>
    ''' <param name="UpdateUserGroupUsersRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateUserGroupUsers(ByVal UpdateUserGroupUsersRequest As UpdateUserGroupUsersRequestType) As UpdateUserGroupUsersResponseType

    ''' <summary>
    ''' TO Validate Bank Account Number details provided in request 
    ''' </summary>
    ''' <param name="ValidateBankAccountNumberRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ValidateBankAccountNumber(ByVal ValidateBankAccountNumberRequest As ValidateBankAccountNumberRequestType) As ValidateBankAccountNumberResponseType

    ''' <summary>
    ''' TO Update Wm Taskr details provided in request 
    ''' </summary>
    ''' <param name="UpdateWmTaskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateWmTask(ByVal UpdateWmTaskRequest As UpdateWmTaskRequestType) As UpdateWmTaskResponseType

    ''' <summary>
    ''' TO UpdateCoverNoteSheet details provided in request 
    ''' </summary>
    ''' <param name="UpdateCoverNoteSheetRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateCoverNoteSheet(ByVal UpdateCoverNoteSheetRequest As UpdateCoverNoteSheetRequestType) As UpdateCoverNoteSheetResponseType

    ''' <summary>
    ''' TO UpdateCoverNoteBook details provided in request 
    ''' </summary>
    ''' <param name="UpdateCoverNoteBookRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateCoverNoteBook(ByVal UpdateCoverNoteBookRequest As UpdateCoverNoteBookRequestType) As UpdateCoverNoteBookResponseType

    ''' <summary>
    ''' TO ReAssignMultipleWmTasks details provided in request 
    ''' </summary>
    ''' <param name="ReAssignMultipleWmTasksrequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ReAssignMultipleWmTasks(ByVal ReAssignMultipleWmTasksRequest As ReAssignMultipleWmTasksRequestType) As ReAssignMultipleWmTasksResponseType

    ''' <summary>
    ''' TO PostDocument details provided in request 
    ''' </summary>
    ''' <param name="PostDocumentrequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function PostDocument(ByVal PostDocumentRequest As PostDocumentRequestType) As PostDocumentResponseType
    ''' <summary>
    ''' TO Get Document List details provided in request 
    ''' </summary>
    ''' <param name="GetDocumentListRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetDocumentList(ByVal GetDocumentListRequest As GetDocumentListRequestType) As GetDocumentListResponseType

    ''' <summary>
    ''' TO Get Recovery Coinsurance details provided in request 
    ''' </summary>
    ''' <param name="GetRecoveryCoinsuranceRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetRecoveryCoinsurance(ByVal GetRecoveryCoinsuranceRequest As GetRecoveryCoinsuranceRequestType) As GetRecoveryCoinsuranceResponseType

    ''' <summary>
    ''' TO Get Wm Task details provided in request 
    ''' </summary>
    ''' <param name="GetWmTaskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetWmTask(ByVal GetWmTaskRequest As GetWmTaskRequestType) As GetWmTaskResponseType

    ''' <summary>
    ''' TO Get Wm Task Log details provided in request 
    ''' </summary>
    ''' <param name="GetWmTaskLogRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetWmTaskLog(ByVal GetWmTaskLogRequest As GetWmTaskLogRequestType) As GetWmTaskLogResponseType

    ''' <summary>
    ''' TO Get Claim Risk Links details provided in request 
    ''' </summary>
    ''' <param name="GetClaimRiskLinksRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimRiskLinks(ByVal GetClaimRiskLinksRequest As GetClaimRiskLinksRequestType) As GetClaimRiskLinksResponseType

    ''' <summary>
    ''' TO Get Product Claims Work flow Options details provided in request 
    ''' </summary>
    ''' <param name="GetProductClaimsWorkflowOptionsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetProductClaimsWorkflowOptions(ByVal GetProductClaimsWorkflowOptionsRequest As GetProductClaimsWorkflowOptionsRequestType) As GetProductClaimsWorkflowOptionsResponseType

    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimPerilSummary(ByVal GetClaimPerilSummaryRequest As GetClaimPerilSummaryRequestType) As GetClaimPerilSummaryResponseType

    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimReceiptTaxes(ByVal GetClaimReceiptTaxesRequest As GetClaimReceiptTaxesRequestType) As GetClaimReceiptTaxesResponseType

    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimPaymentTaxes(ByVal GetClaimPaymentTaxesRequest As GetClaimPaymentTaxesRequestType) As GetClaimPaymentTaxesResponseType

    ''' <summary>
    ''' This web services method is used to Get Agent Details For Policy.
    '''  </summary>
    ''' <param name= "GetAgentDetailsForPolicyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetAgentDetailsForPolicy(ByVal GetAgentDetailsForPolicyRequest As GetAgentDetailsForPolicyRequestType) As GetAgentDetailsForPolicyResponseType


    ''' <summary>
    ''' This web services method is used to Get All Live Policy Version Amounts.
    '''  </summary>
    ''' <param name= "GetAllLivePolicyVersionAmountsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetAllLivePolicyVersionAmounts(ByVal GetAllLivePolicyVersionAmountsRequest As GetAllLivePolicyVersionAmountsRequestType) As GetAllLivePolicyVersionAmountsResponseType

    ''' <summary>
    ''' This web services method is used to Get All Live Policy Version Amounts.
    '''  </summary>
    '''<param name = "GetAllLivePolicyVersionsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetAllPolicyVersions(ByVal GetAllLivePolicyVersionsRequest As GetAllPolicyVersionsRequestType) As GetAllPolicyVersionsResponseType

    ''' <summary>
    ''' This web services method is used to Get Claim Party Details.
    '''  </summary>
    '''<param name = "GetClaimPartyDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimPartyDetails(ByVal GetClaimPartyDetailsRequest As GetClaimPartyDetailsRequestType) As GetClaimPartyDetailsResponseType

    ''' <summary>
    ''' This web services method is used to Get Party Bank Details.
    '''  </summary>
    '''<param name = "GetPartyBankDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPartyBankDetails(ByVal GetPartyBankDetailsRequest As GetPartyBankDetailsRequestType) As GetPartyBankDetailsResponseType


    ''' <summary>
    ''' This web services method is used to Get Party Policies.
    '''  </summary>
    '''<param name = "GetPartyPoliciesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPartyPolicies(ByVal GetPartyPoliciesRequest As GetPartyPoliciesRequestType) As GetPartyPoliciesResponseType

    ''' <summary>
    ''' This web services method is used to Get Party Summary.
    '''  </summary>
    '''<param name = "GetPartySummaryRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPartySummary(ByVal GetPartySummaryRequest As GetPartySummaryRequestType) As GetPartySummaryResponseType

    ''' <summary>
    ''' This web services method is used to Get Policy Bank Guarantee.
    '''  </summary>
    '''<param name = "GetPolicyBankGuaranteeRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPolicyBankGuarantee(ByVal GetPolicyBankGuaranteeRequest As GetPolicyBankGuaranteeRequestType) As GetPolicyBankGuaranteeResponseType

    ''' <summary>
    ''' This web services method is used to Get Policy Details For Bounced Receipt.
    '''  </summary>
    '''<param name = "GetPolicyDetailsForBouncedReceiptRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPolicyDetailsForBouncedReceipt(ByVal GetPolicyDetailsForBouncedReceiptRequest As GetPolicyDetailsForBouncedReceiptRequestType) As GetPolicyDetailsForBouncedReceiptResponseType

    ''' <summary>
    ''' This web services method is used to Get Policy Details For Bounced Receipt.
    '''  </summary>
    '''<param name = "GetPolicyStatusForMediaTypeStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPolicyStatusForMediaTypeStatus(ByVal GetPolicyStatusForMediaTypeStatusRequest As GetPolicyStatusForMediaTypeStatusRequestType) As GetPolicyStatusForMediaTypeStatusResponseType

    ''' <summary>
    ''' This web services method is used to Get Standard Policy Wordings.
    '''  </summary>
    '''<param name = "GetStandardPolicyWordingsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetStandardPolicyWordings(ByVal GetStandardPolicyWordingsRequest As GetStandardPolicyWordingsRequestType) As GetStandardPolicyWordingsResponseType

    ''' <summary>
    ''' This web services method is used to Replace Party Contact.
    '''  </summary>
    '''<param name = "ReplacePartyContactRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ReplacePartyContact(ByVal ReplacePartyContactRequest As ReplacePartyContactRequestType) As ReplacePartyContactResponseType

    ''' <summary>
    ''' This web services method is used to Run Default Rules Edit.
    '''  </summary>
    '''<param name = "RunDefaultRulesEditRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function RunDefaultRulesEdit(ByVal RunDefaultRulesEditRequest As RunDefaultRulesEditRequestType) As RunDefaultRulesEditResponseType

    ''' <summary>
    ''' This web services method is used to Run Renewal Accept.
    '''  </summary>
    '''<param name = "RunRenewalAcceptRequest"></param>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Sub RunRenewalAccept(ByVal RunRenewalAcceptRequest As RunRenewalAcceptRequestType)

    ''' <summary>  
    ''' This web services method is used to Run Renewal Invitation.
    '''  </summary>
    '''  <param name = "RunRenewalInviteRequest"></param>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Sub RunRenewalInvitation(ByVal RunRenewalInviteRequest As RunRenewalInviteRequestType)


    ''' <summary>
    ''' This web services method is used to  Run Renewal Selection.
    '''  </summary>
    '''<param name = "RunRenewalSelectionRequest"></param>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Sub RunRenewalSelection(ByVal RunRenewalSelectionRequest As RunRenewalSelectionRequestType)

    ''' <summary>
    ''' This web services method is used to Activate Party Bank Details.
    '''  </summary>
    '''<param name = "ActivatePartyBankDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ActivatePartyBankDetails(ByVal ActivatePartyBankDetailsRequest As ActivatePartyBankRequestType) As ActivatePartyBankResponseType


    ''' <summary>
    ''' This web services method is used to Run Renewal Selection By Policy.
    '''  </summary>
    '''<param name = "RunRenewalSelectionByPolicyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function RunRenewalSelectionByPolicy(ByVal RunRenewalSelectionByPolicyRequest As RunRenewalSelectionByPolicyRequestType) As RunRenewalSelectionByPolicyResponseType


    ''' This web services method is used to Run Validation Rules.
    '''  </summary>
    '''<param name = "RunValidationRulesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function RunValidationRules(ByVal RunValidationRulesRequest As RunValidationRulesRequestType) As RunValidationRulesResponseType



    ''' This web services method is used to Update Party Bank Details.
    '''  </summary>
    '''<param name = "UpdatePartyBankDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdatePartyBankDetails(ByVal UpdatePartyBankDetailsRequest As UpdatePartyBankDetailsRequestType) As UpdatePartyBankDetailsResponseType


    ''' <summary>
    ''' This web services method is used to Update Party Risk.
    '''  </summary>
    '''<param name = "UpdatePartyRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdatePartyRisk(ByVal UpdatePartyRiskRequest As UpdatePartyRiskRequestType) As UpdatePartyRiskResponseType

    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddClaimRisk(ByVal AddClaimRiskRequest As AddClaimRiskRequestType) As AddClaimRiskResponseType


    ''' <summary>
    ''' This web services method is used to Authorise Payment.
    ''' </summary>
    ''' <param name="oAuthoriseClaimPaymentRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AuthoriseClaimPayment(ByVal oAuthoriseClaimPaymentRequest As AuthoriseClaimPaymentRequestType) As AuthoriseClaimPaymentResponseType

    ''' <summary>
    ''' This web services method is used to bind claim.
    ''' </summary>
    ''' <param name="oBindClaimRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function BindClaim(ByVal oBindClaimRequest As BindClaimRequestType) As BindClaimResponseType

    ''' <summary>
    ''' This web services method is used to Calculate Tax For Claims.
    ''' </summary>
    ''' <param name="CalculateTaxForClaimsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function CalculateTaxForClaims(ByVal CalculateTaxForClaimsRequest As CalculateTaxForClaimsRequestType) As CalculateTaxForClaimsResponseType

    ''' <summary>
    ''' This web services method is used to Claim Data Import.
    ''' </summary>
    ''' <param name="ClaimDataImportRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function ClaimDataImport(ByVal ClaimDataImportRequest As ClaimDataImportRequestType) As ClaimDataImportResponseType

    ''' <summary>
    ''' This web services method is used to Claim Receipt.
    ''' </summary>
    ''' <param name="ClaimReceiptRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
   <FaultContract(GetType(SAMMethodResponseData))>
    Function ClaimReceipt(ByVal ClaimReceiptRequest As ClaimReceiptRequestType) As ClaimReceiptResponseType

    ''' <summary>
    ''' This web services method is used to Find Claim.
    ''' </summary>
    ''' <param name="FindClaimRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
   <FaultContract(GetType(SAMMethodResponseData))>
    Function FindClaim(ByVal FindClaimRequest As FindClaimRequestType) As FindClaimResponseType

    ''' <summary>
    ''' This web services method is used to Find InsuranceFile For CLaims Request.
    ''' </summary>
    ''' <param name="FindInsuranceFileForCLaimsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function FindInsuranceFileForClaims(ByVal FindInsuranceFileForCLaimsRequest As FindInsuranceFileForClaimsRequestType) As FindInsuranceFileForClaimsResponseType

    ''' <summary>
    ''' This web services method is used to Generate Claims Documents
    ''' </summary>
    ''' <param name="GenerateClaimsDocumentsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GenerateClaimsDocuments(ByVal GenerateClaimsDocumentsRequest As GenerateClaimsDocumentsRequestType) As GenerateClaimsDocumentsResponseType

    ''' <summary>
    ''' This web services method is used to Get Allocation Details
    ''' </summary>
    ''' <param name="oGetAllocationDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetAllocationDetails(ByVal oGetAllocationDetailsRequest As GetAllocationDetailsRequestType) As GetAllocationDetailsResponseType

    ''' <summary>
    ''' GetBalanceForCDResponseType
    ''' </summary>
    ''' <param name="oGetBalanceForCDRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetBalanceForCD(ByVal oGetBalanceForCDRequest As GetBalanceForCDRequestType) As GetBalanceForCDResponseType

    ''' <summary>
    ''' GetBalancesAndUnallocatedCredits
    ''' </summary>
    ''' <param name="GetBalancesAndUnallocatedCreditsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetBalancesAndUnallocatedCredits(ByVal GetBalancesAndUnallocatedCreditsRequest As GetBalancesAndUnallocatedCreditsRequestType) As GetBalancesAndUnallocatedCreditsResponseType

    ''' <summary>
    ''' GetBankGuarantee
    ''' </summary>
    ''' <param name="oGetBankGuaranteeRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetBankGuarantee(ByVal oGetBankGuaranteeRequest As GetBankGuaranteeRequestType) As GetBankGuaranteeResponseType

    ''' <summary>
    ''' GetBrokerSummary
    ''' </summary>
    ''' <param name="oGetBrokerSummaryRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetBrokerSummary(ByVal oGetBrokerSummaryRequest As GetBrokerSummaryRequestType) As GetBrokerSummaryResponseType

    ''' <summary>
    ''' GetCoinsuranceDefaults
    ''' </summary>
    ''' <param name="GetCoinsuranceDefaultsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetCoinsuranceDefaults(ByVal GetCoinsuranceDefaultsRequest As GetCoinsuranceDefaultsRequestType) As GetCoinsuranceDefaultsResponseType

    ''' <summary>
    ''' GetCoinsuranceValues
    ''' </summary>
    ''' <param name="GetCoinsuranceValuesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetCoinsuranceValues(ByVal GetCoinsuranceValuesRequest As GetCoinsuranceValuesRequestType) As GetCoinsuranceValuesResponseType

    ''' <summary>
    ''' GetCurrenciesByBranch
    ''' </summary>
    ''' <param name="GetCurrenciesByBranchRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetCurrenciesByBranch(ByVal GetCurrenciesByBranchRequest As GetCurrenciesByBranchRequestType) As GetCurrenciesByBranchResponseType

    ''' <summary>
    ''' GetCurrencyExchangeRates
    ''' </summary>
    ''' <param name="oGetCurrencyExchangeRatesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetCurrencyExchangeRates(ByVal oGetCurrencyExchangeRatesRequest As GetCurrencyExchangeRatesRequestType) As GetCurrencyExchangeRatesResponseType

    ''' <summary>
    ''' GetCurrencyToCurrencyExchangeRate
    ''' </summary>
    ''' <param name="oGetCurrencyToCurrencyExchangeRateRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetCurrencyToCurrencyExchangeRate(ByVal oGetCurrencyToCurrencyExchangeRateRequest As GetCurrencyToCurrencyExchangeRateRequestType) As GetCurrencyToCurrencyExchangeRateResponseType

    ''' <summary>
    ''' GetDMEFolder
    ''' </summary>
    ''' <param name="GetDMEFolderRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetDMEFolder(ByVal GetDMEFolderRequest As GetDMEFolderRequestType) As GetDMEFolderResponseType

    ''' <summary>
    ''' GetDatasetDefinition
    ''' </summary>
    ''' <param name="GetDatasetDefinitionRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetDatasetDefinition(ByVal GetDatasetDefinitionRequest As GetDatasetDefinitionRequestType) As GetDatasetDefinitionResponseType

    ''' <summary>
    ''' GetDatasetSchema
    ''' </summary>
    ''' <param name="GetDatasetSchemaRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetDatasetSchema(ByVal GetDatasetSchemaRequest As GetDatasetSchemaRequestType) As GetDatasetSchemaResponseType

    ''' <summary>
    ''' GetEventDetails
    ''' </summary>
    ''' <param name="oGetEventDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetEventDetails(ByVal oGetEventDetailsRequest As GetEventDetailsRequestType) As GetEventDetailsResponseType

    ''' <summary>
    ''' GetEventNote
    ''' </summary>
    ''' <param name="oGetEventNoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetEventNote(ByVal oGetEventNoteRequest As GetEventNoteRequestType) As GetEventNoteResponseType

    ''' <summary>
    ''' GetHeaderAndAgentCommissionByKey
    ''' </summary>
    ''' <param name="GetHeaderAndAgentCommissionByKeyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetHeaderAndAgentCommissionByKey(ByVal GetHeaderAndAgentCommissionByKeyRequest As GetHeaderAndAgentCommissionByKeyRequestType) As GetHeaderAndAgentCommissionByKeyResponseType

    ''' <summary>
    ''' GetHeaderAndPolicyFeesByKey
    ''' </summary>
    ''' <param name="GetHeaderAndPolicyFeesByKeyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetHeaderAndPolicyFeesByKey(ByVal GetHeaderAndPolicyFeesByKeyRequest As GetHeaderAndPolicyFeesByKeyRequestType) As GetHeaderAndPolicyFeesByKeyResponseType

    ''' <summary>
    ''' GetHeaderAndPolicyTaxByKey
    ''' </summary>
    ''' <param name="oGetHeaderAndPolicyTaxByKeyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetHeaderAndPolicyTaxByKey(ByVal oGetHeaderAndPolicyTaxByKeyRequest As GetHeaderAndPolicyTaxByKeyRequestType) As GetHeaderAndPolicyTaxByKeyResponseType

    ''' <summary>
    ''' GetHeaderAndRiskFeesByKey
    ''' </summary>
    ''' <param name="GetHeaderAndRiskFeesByKeyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetHeaderAndRiskFeesByKey(ByVal GetHeaderAndRiskFeesByKeyRequest As GetHeaderAndRiskFeesByKeyRequestType) As GetHeaderAndRiskFeesByKeyResponseType

    ''' <summary>
    ''' GetHeaderAndRiskTaxByKey
    ''' </summary>
    ''' <param name="GetHeaderAndRiskTaxByKeyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetHeaderAndRiskTaxByKey(ByVal GetHeaderAndRiskTaxByKeyRequest As GetHeaderAndRiskTaxByKeyRequestType) As GetHeaderAndRiskTaxByKeyResponseType

    ''' <summary>
    ''' GetHeaderAndRisksByKey
    ''' </summary>
    ''' <param name="GetHeaderAndRisksByKeyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetHeaderAndRisksByKey(ByVal GetHeaderAndRisksByKeyRequest As GetHeaderAndRisksByKeyRequestType) As GetHeaderAndRisksByKeyResponseType

    ''' <summary>
    ''' GetHeaderAndSummariesByKey
    ''' </summary>
    ''' <param name="GetHeaderAndSummariesByKeyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetHeaderAndSummariesByKey(ByVal GetHeaderAndSummariesByKeyRequest As GetHeaderAndSummariesByKeyRequestType) As GetHeaderAndSummariesByKeyResponseType
    ''' <summary>
    ''' GetHeaderAndSummariesByRef
    ''' </summary>
    ''' <param name="GetHeaderAndSummariesByRefRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetHeaderAndSummariesByRef(ByVal GetHeaderAndSummariesByRefRequest As GetHeaderAndSummariesByRefRequestType) As GetHeaderAndSummariesByRefResponseType

    ''' <summary>
    ''' MaintainClaim
    ''' </summary>
    ''' <param name="MaintainClaimRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function MaintainClaim(ByVal MaintainClaimRequest As MaintainClaimRequestType) As MaintainClaimResponseType

    ''' <summary>
    ''' OpenClaim
    ''' </summary>
    ''' <param name="OpenClaimRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function OpenClaim(ByVal OpenClaimRequest As OpenClaimRequestType) As OpenClaimResponseType

    ''' <summary>
    ''' PayClaim
    ''' </summary>
    ''' <param name="PayClaimRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function PayClaim(ByVal PayClaimRequest As PayClaimRequestType) As PayClaimResponseType

    ''' <summary>
    ''' ProcessClaim
    ''' </summary>
    ''' <param name="oClaimProcessRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function ProcessClaim(ByVal oClaimProcessRequest As BaseClaimProcessRequestType) As BaseClaimProcessResponseType

    ''' <summary>
    ''' UpdateClaimReservesOrPayments
    ''' </summary>
    ''' <param name="oUpdateClaimReservesOrPaymentsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateClaimReservesOrPayments(ByVal oUpdateClaimReservesOrPaymentsRequest As UpdateClaimReservesOrPaymentsRequestType) As UpdateClaimReservesOrPaymentsResponseType

    ''' <summary>
    ''' UpdateClaimRisk
    ''' </summary>
    ''' <param name="UpdateClaimRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateClaimRisk(ByVal UpdateClaimRiskRequest As UpdateClaimRiskRequestType) As UpdateClaimRiskResponseType


    ''' <summary>
    ''' This web services method is used to Update Fees.
    '''  </summary>
    '''<param name = "UpdateFeeRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateFee(ByVal UpdateFeeRequest As UpdateFeeRequestType) As UpdateFeeResponseType

    ''' <summary>
    ''' This web services method is used to Find Address.
    '''  </summary>
    '''<param name = "FindAddressRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindAddress(ByVal FindAddressRequest As FindAddressRequestType) As FindAddressResponseType

    ''' <summary>
    ''' This web services method is used to Get Address.
    '''  </summary>
    '''<param name = "GetAddressRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetAddress(ByVal GetAddressRequest As GetAddressRequestType) As GetAddressResponseType


    ''' <summary>
    ''' This web services method is used to Add Party Bank Details.
    '''  </summary>
    '''<param name = "AddPartyBankDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddPartyBankDetails(ByVal AddPartyBankDetailsRequest As AddPartyBankDetailsRequestType) As AddPartyBankDetailsResponseType


    ''' <summary>
    ''' This web services method is used to Delete Party Bank Details.
    '''  </summary>
    '''<param name = "DeletePartyBankDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function DeletePartyBankDetails(ByVal DeletePartyBankDetailsRequest As DeletePartyBankDetailsRequestType) As DeletePartyBankDetailsResponseType

    ''' <summary>
    ''' This web services method is used to Add Address.
    '''  </summary>
    '''<param name = "AddAddressRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddAddress(ByVal AddAddressRequest As AddAddressRequestType) As AddAddressResponseType

    ''' <summary>
    ''' This web services method is used to Add Address.
    '''  </summary>
    '''<param name = "UpdateStandardPolicyWordingsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateStandardPolicyWordings(ByVal UpdateStandardPolicyWordingsRequest As UpdateStandardPolicyWordingsRequestType) As UpdateStandardPolicyWordingsResponseType

    ''' <summary>
    ''' This web services method is used to Add Address.
    '''  </summary>
    '''<param name = "GetUserAuthorityValueRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserAuthorityValue(ByVal GetUserAuthorityValueRequest As GetUserAuthorityValueRequestType) As GetUserAuthorityValueResponseType

    ''' <summary>
    ''' This web services method is used to Add Address.
    '''  </summary>
    '''<param name = "ReverseAllocationRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ReverseAllocation(ByVal ReverseAllocationRequest As ReverseAllocationRequestType) As ReverseAllocationResponseType

    ''' <summary>
    ''' This web services method is used to Find Case.
    ''' </summary>
    ''' <param name="oFindCaseRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindCase(ByVal oFindCaseRequest As FindCaseRequestType) As FindCaseResponseType

    ''' <summary>
    ''' This web services method is used to GetCaseDetails.
    ''' </summary>
    ''' <param name="oGetCaseDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetCaseDetails(ByVal oGetCaseDetailsRequest As GetCaseDetailsRequestType) As GetCaseDetailsResponseType

    ''' <summary>
    ''' This web services method is used to Close Case.
    ''' </summary>
    ''' <param name="oCloseCaseRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CloseCase(ByVal oCloseCaseRequest As CloseCaseRequestType) As CloseCaseResponseType

    ''' <summary>
    ''' This web services method is used to SaveCase.
    ''' </summary>
    ''' <param name="oSaveCaseRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function SaveCase(ByVal oSaveCaseRequest As SaveCaseRequestType) As SaveCaseResponseType

    ''' <summary>
    ''' This web services method is used to CaseLinkUnlink.
    ''' </summary>
    ''' <param name="oCaseLinkUnlinkRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CaseLinkUnlink(ByVal oCaseLinkUnlinkRequest As CaseLinkUnLinkRequestType) As CaseLinkUnLinkResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oAddCashClaimLinkRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddCashClaimLink(ByVal oAddCashClaimLinkRequest As AddCashClaimLinkRequestType) As AddCashClaimLinkResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetCashClaimLinkRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetCashClaimLink(ByVal oGetCashClaimLinkRequest As GetCashClaimLinkRequestType) As GetCashClaimLinkResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oCalculateRITaxRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CalculateRITax(ByVal oCalculateRITaxRequest As CalculateRITaxRequestType) As CalculateRITaxResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oUpdateClaimRIArrangementLinesRI2007Request"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateClaimRIArrangementLinesRI2007(ByVal oUpdateClaimRIArrangementLinesRI2007Request As UpdateClaimRIArrangementLinesRI2007RequestType) As UpdateClaimRIArrangementLinesRI2007ResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oUpdateArrangementLinesRI2007Request"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateArrangementLinesRI2007(ByVal oUpdateArrangementLinesRI2007Request As UpdateArrangementLinesRI2007RequestType) As UpdateArrangementLinesRI2007ResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetClaimRIArrangementLinesRI2007Request"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimRIArrangementLinesRI2007(ByVal oGetClaimRIArrangementLinesRI2007Request As GetClaimRIArrangementLinesRI2007RequestType) As GetClaimRIArrangementLinesRI2007ResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetTreatyPartyDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetTreatyPartyDetails(ByVal oGetTreatyPartyDetailsRequest As GetTreatyPartyDetailsRequestType) As GetTreatyPartyDetailsResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oFindReinsurerRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindReinsurer(ByVal oFindReinsurerRequest As FindReinsurerRequestType) As FindReinsurerResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetRIModelLineDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetRIModelLineDetails(ByVal oGetRIModelLineDetailsRequest As GetRIModelLineDetailsRequestType) As GetRIModelLineDetailsResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetRIModelDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetRIModelDetails(ByVal oGetRIModelDetailsRequest As GetRIModelDetailsRequestType) As GetRIModelDetailsResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetRiskReinsuranceArrangementLinesRI2007Request"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetRiskReinsuranceArrangementLinesRI2007(ByVal oGetRiskReinsuranceArrangementLinesRI2007Request As GetRiskReinsuranceArrangementLinesRI2007RequestType) As GetRiskReinsuranceArrangementLinesRI2007ResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oUpdateRiskStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateRiskStatus(ByVal oUpdateRiskStatusRequest As UpdateRiskStatusRequestType) As UpdateRiskStatusResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetProductsForUserBranchRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetProductsForUserBranch(ByVal oGetProductsForUserBranchRequest As GetProductsForUserBranchRequestType) As GetProductsForUserBranchResponseType
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="request"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ClearCache(ByVal request As ClearCacheRequestType) As ClearCacheResponseType
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oSettleAllClaimPaymentsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function SettleAllClaimPayments(ByVal oSettleAllClaimPaymentsRequest As SettleAllClaimPaymentsRequestType) As SettleAllClaimPaymentsResponseType

    ''' <summary>
    ''' Use to return hashed password and password history for an user
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()> _
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ValidateUser(ByVal oRequest As ValidateUserRequestType) As ValidateUserResponseType

    ''' <summary>
    ''' this will used by DTU2
    ''' </summary>
    ''' <param name="oDocumentDataImportRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function DocumentDataImport(ByVal oDocumentDataImportRequest As DocumentDataImportRequestType) As DocumentDataImportResponseType

End Interface
