Imports SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.WCF

<ServiceContract()>
Public Interface IPurePolicyService
    ''' <summary>
    ''' This web services method is used to Add Address.
    '''  </summary>
    '''<param name = "AddAddressRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function AddAddress(ByVal AddAddressRequest As AddAddressRequestType) As AddAddressResponseType

    ''' <summary>
    ''' TO Add Back Dated MTAQuote details provided in request
    ''' </summary>
    ''' <param name="AddBackDatedMTAQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function AddBackDatedMTAQuote(ByVal AddBackDatedMTAQuoteRequest As AddBackDatedMTAQuoteRequestType) As AddBackDatedMTAQuoteResponseType

    ''' <summary>
    ''' TO Add MtaQuote details provided in request
    ''' </summary>
    ''' <param name="AddMtaQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function AddMtaQuote(ByVal AddMtaQuoteRequest As AddMtaQuoteRequestType) As AddMtaQuoteResponseType

    ''' <summary>
    ''' This is webservice method for AddPayNowReceipt
    '''<param name="oRequest" type="AddPayNowReceiptRequestType"></param>   
    '''</summary>
    '''<returns>AddPayNowReceiptResponseType</returns>
    '''<remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function AddPayNowReceipt(ByVal oRequest As AddPayNowReceiptRequestType) As AddPayNowReceiptResponseType

    ''' <summary>
    ''' TO Add Quote details provided in request
    ''' </summary>
    ''' <param name="AddQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function AddQuote(ByVal AddQuoteRequest As AddQuoteRequestType) As AddQuoteResponseType

    ''' <summary>
    ''' TO update Taxes details provided in request
    ''' </summary>
    ''' <param name="AddQuoteV2Request"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function AddQuoteV2(ByVal AddQuoteV2Request As AddQuoteV2RequestType) As AddQuoteV2ResponseType

    ''' <summary>
    ''' TO Add Risk details provided in request
    ''' </summary>
    ''' <param name="AddRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function AddRisk(ByVal AddRiskRequest As AddRiskRequestType) As AddRiskResponseType

    ''' <summary>
    ''' TO Bind Quote provided in request
    ''' </summary>
    ''' <param name="BindQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function BindQuote(ByVal BindQuoteRequest As BindQuoteRequestType) As BindQuoteResponseType

    ''' <summary>
    ''' TO Copy Quote details provided in request
    ''' </summary>
    ''' <param name="CopyQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function CopyQuote(ByVal CopyQuoteRequest As CopyQuoteRequestType) As CopyQuoteResponseType

    ''' <summary>
    ''' TO Clone Live Policy to Quote 
    ''' </summary>
    ''' <param name="CopyQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function CloneQuoteFromLivePolicy(ByVal CopyQuoteRequest As CopyQuoteRequestType) As CopyQuoteResponseType

    ''' <summary>
    ''' TO Delete Policy details provided in request
    ''' </summary>
    ''' <param name="DeletePolicyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function DeletePolicy(ByVal DeletePolicyRequest As DeletePolicyRequestType) As DeletePolicyResponseType

    ''' <summary>
    ''' TO DeleteRenewal details provided in request
    ''' </summary>
    ''' <param name="DeleteRenewalRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function DeleteRenewal(ByVal DeleteRenewalRequest As DeleteRenewalRequestType) As DeleteRenewalResponseType

    ''' <summary>
    ''' This method is used to find document templates
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function FindDocumentTemplates(ByVal oRequest As FindDocumentTemplatesRequestType) As FindDocumentTemplatesResponseType

    ''' <summary>  
    ''' Find the Policy
    ''' </summary>  
    ''' <param name="oRequest">An object of type FindPolicyRequestType</param>  
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function FindPolicy(ByVal oRequest As FindPolicyRequestType) As FindPolicyResponseType

    ''' <summary>
    ''' TO GenerateInvite details provided in request
    ''' </summary>
    ''' <param name="GenerateInviteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GenerateInvite(ByVal GenerateInviteRequest As GenerateInviteRequestType) As GenerateInviteResponseType


    ''' <summary>
    ''' TO GetAgentCommission details provided in request
    ''' </summary>
    ''' <param name="GetAgentCommissionRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetAgentCommission(ByVal GetAgentCommissionRequest As GetAgentCommissionRequestType) As GetAgentCommissionResponseType

    ''' <summary>
    ''' TO GetRenewalAmountToFinance details provided in request
    ''' </summary>
    ''' <param name="GetRenewalAmountToFinance"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetRenewalAmountToFinance(ByVal GetAgentCommissionTaxRequest As GetRenewalAmountToFinanceRequestType) As GetRenewalAmountToFinanceResponseType
    ''' <summary>
    ''' TO GetAgentCommissionTax details provided in request
    ''' </summary>
    ''' <param name="GetAgentCommissionTaxRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetAgentCommissionTax(ByVal GetAgentCommissionTaxRequest As GetAgentCommissionTaxRequestType) As GetAgentCommissionTaxResponseType

    ''' <summary>
    ''' This web services method is used to Get Agent Details For Policy.
    '''  </summary>
    ''' <param name= "GetAgentDetailsForPolicyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetAgentDetailsForPolicy(ByVal GetAgentDetailsForPolicyRequest As GetAgentDetailsForPolicyRequestType) As GetAgentDetailsForPolicyResponseType

    ''' <summary>
    ''' This web services method is used to Get All Live Policy Version Amounts.
    '''  </summary>
    '''<param name = "GetAllLivePolicyVersionsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetAllPolicyVersions(ByVal GetAllLivePolicyVersionsRequest As GetAllPolicyVersionsRequestType) As GetAllPolicyVersionsResponseType

    ''' <summary>
    ''' TO Get Backdated Versions for a policy
    ''' </summary>
    ''' <param name="GetBackdatedMTARiskVersionsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetBackdatedMTAPolicyVersions(ByVal GetBackdatedMTARiskVersionsRequest As GetHeaderAndSummariesByKeyRequestType) As AddBackDatedMTAQuoteResponseType

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
    ''' TO GetCashDepositsForPolicy details provided in request
    ''' </summary>
    ''' <param name="GetCashDepositsForPolicyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetCashDepositsForPolicy(ByVal GetCashDepositsForPolicyRequest As GetCashDepositsForPolicyRequestType) As GetCashDepositsForPolicyResponseType

    ''' <summary>
    ''' TO Get Existing Instalment Plan Payment Details with details provided in request
    ''' </summary>
    ''' <param name="GetExistingInstalmentPlanPaymentDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetExistingInstalmentPlanPaymentDetails(ByVal GetExistingInstalmentPlanPaymentDetailsRequest As GetExistingInstalmentPlanPaymentDetailsRequestType) As GetExistingInstalmentPlanPaymentDetailsResponseType

    ''' <summary>
    ''' TO Get the Finance plan details provided in request
    ''' </summary>
    ''' <param name="GetFinancePlanDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetFinancePlanDetails(ByVal GetFinancePlanDetailsRequest As GetFinancePlanDetailsRequestType) As GetFinancePlanDetailsResponseType

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
    ''' GetHeaderAndRisksByKey
    ''' </summary>
    ''' <param name="GetHeaderAndRisksByKeyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetHeaderAndRisksByKey(ByVal GetHeaderAndRisksByKeyRequest As GetHeaderAndRisksByKeyRequestType) As GetHeaderAndRisksByKeyResponseType

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
    ''' TO Get Instalment Quotes provided in request
    ''' </summary>
    ''' <param name="GetInstalmentQuotesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetInstalmentQuotes(ByVal GetInstalmentQuotesRequest As GetInstalmentQuotesRequestType) As GetInstalmentQuotesResponseType

    ''' <summary>
    ''' TO Get Policies For Renewal Selection details provided in request
    ''' </summary>
    ''' <param name="GetPoliciesForRenewalSelectionRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetPoliciesForRenewalSelection(ByVal GetPoliciesForRenewalSelectionRequest As GetPoliciesForRenewalSelectionRequestType) As GetPoliciesForRenewalSelectionResponseType

    ''' <summary>
    ''' TO Get Policies In Renewal details provided in request
    ''' </summary>
    ''' <param name="GetPoliciesInRenewalRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetPoliciesInRenewal(ByVal GetPoliciesInRenewalRequest As GetPoliciesInRenewalRequestType) As GetPoliciesInRenewalResponseType

    ''' <summary>
    ''' This is webservice method for GetPoliciesOnBankGuaranteeForReceipt
    '''<param name="oGetPoliciesOnBankGuaranteeForReceiptRequest" type="GetPoliciesOnBankGuaranteeForReceiptRequestType"></param>   
    '''<returns>GetPoliciesOnBankGuaranteeForReceiptResponseType</returns>
    '''</summary>
    '''<remarks></remarks> 
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetPoliciesOnBankGuaranteeByKey(ByVal oGetPoliciesOnBankGuaranteeByKeyRequest As GetPoliciesOnBankGuaranteeByKeyRequestType) As GetPoliciesOnBankGuaranteeByKeyResponseType


    ''' <summary>
    ''' This is webservice method for GetPoliciesOnBankGuaranteeForReceipt
    '''<param name="oGetPoliciesOnBankGuaranteeForReceiptRequest" type="GetPoliciesOnBankGuaranteeForReceiptRequestType"></param>   
    '''<returns>GetPoliciesOnBankGuaranteeForReceiptResponseType</returns>
    '''</summary>
    '''<remarks></remarks> 
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetPoliciesOnBankGuaranteeForReceipt(ByVal oGetPoliciesOnBankGuaranteeForReceiptRequest As GetPoliciesOnBankGuaranteeForReceiptRequestType) As GetPoliciesOnBankGuaranteeForReceiptResponseType

    ''' <summary>
    ''' This web services method is used to Get Policy Bank Guarantee.
    '''  </summary>
    '''<param name = "GetPolicyBankGuaranteeRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetPolicyBankGuarantee(ByVal GetPolicyBankGuaranteeRequest As GetPolicyBankGuaranteeRequestType) As GetPolicyBankGuaranteeResponseType

    ''' <summary>
    ''' This web services method is used to Get Policy Details For Bounced Receipt.
    '''  </summary>
    '''<param name = "GetPolicyStatusForMediaTypeStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetPolicyStatusForMediaTypeStatus(ByVal GetPolicyStatusForMediaTypeStatusRequest As GetPolicyStatusForMediaTypeStatusRequestType) As GetPolicyStatusForMediaTypeStatusResponseType


    ''' <summary>
    ''' TO Get Quotes Marked For Collection details provided in request
    ''' </summary>
    ''' <param name="GetQuotesMarkedForCollectionRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetQuotesMarkedForCollection(ByVal GetQuotesMarkedForCollectionRequest As GetQuotesMarkedForCollectionRequestType) As GetQuotesMarkedForCollectionResponseType

    ''' <summary>
    ''' This is webservice method definition for fetching the rating details
    ''' </summary>
    ''' <param name="GetRatingDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetRatingDetails(ByVal GetRatingDetailsRequest As GetRatingDetailsRequestType) As GetRatingDetailsResponseType

    ''' <summary>
    ''' TO update Taxes details provided in request
    ''' </summary>
    ''' <param name="GetRatingSectionByRiskTypeRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetRatingSectionByRiskType(ByVal GetRatingSectionByRiskTypeRequest As GetRatingSectionByRiskTypeRequestType) As GetRatingSectionByRiskTypeResponseType

    ''' <summary>
    ''' This is webservice method definition for fetching the rating section types
    ''' </summary>
    ''' <param name="oGetRatingSectionTypesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetRatingSectionTypes(ByVal oGetRatingSectionTypesRequest As GetRatingSectionTypesRequestType) As GetRatingSectionTypesResponseType

    ''' <summary>
    ''' This Web method is used to get the renewal status
    '''</summary>
    '''<param name="GetRenewalStatusRequest" type="GetReferredPaymentsRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetReferredPaymentsResponseType</returns>  
    '''<remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetRenewalStatus(ByVal GetRenewalStatusRequest As GetRenewalStatusRequestType) As GetRenewalStatusResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetRIModelDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetRIModelDetails(ByVal oGetRIModelDetailsRequest As GetRIModelDetailsRequestType) As GetRIModelDetailsResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetRIModelLineDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetRIModelLineDetails(ByVal oGetRIModelLineDetailsRequest As GetRIModelLineDetailsRequestType) As GetRIModelLineDetailsResponseType

    ''' <summary>
    ''' TO Get Risk details provided in request
    ''' </summary>
    ''' <param name="GetRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetRisk(ByVal GetRiskRequest As GetRiskRequestType) As GetRiskResponseType

    ''' <summary>
    ''' TO GetRiskReinsuranceArrangementLines details provided in request
    ''' </summary>
    ''' <param name="GetRiskReinsuranceArrangementLinesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetRiskReinsuranceArrangementLines(ByVal GetRiskReinsuranceArrangementLinesRequest As GetRiskReinsuranceArrangementLinesRequestType) As GetRiskReinsuranceArrangementLinesResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetRiskReinsuranceArrangementLinesRI2007Request"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetRiskReinsuranceArrangementLinesRI2007(ByVal oGetRiskReinsuranceArrangementLinesRI2007Request As GetRiskReinsuranceArrangementLinesRI2007RequestType) As GetRiskReinsuranceArrangementLinesRI2007ResponseType

    ''' <summary>
    ''' TO GetRiskReinsuranceArrangements details provided in request
    ''' </summary>
    ''' <param name="GetRiskReinsuranceArrangementsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetRiskReinsuranceArrangements(ByVal GetRiskReinsuranceArrangementsRequest As GetRiskReinsuranceArrangementsRequestType) As GetRiskReinsuranceArrangementsResponseType

    ''' <summary>
    ''' TO GetRiskReinsuranceBands details provided in request
    ''' </summary>
    ''' <param name="GetRiskReinsuranceBandsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetRiskReinsuranceBands(ByVal GetRiskReinsuranceBandsRequest As GetRiskReinsuranceBandsRequestType) As GetRiskReinsuranceBandsResponseType

    ''' <summary>
    ''' This web services method is used to Get Standard Policy Wordings.
    '''  </summary>
    '''<param name = "GetStandardPolicyWordingsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetStandardPolicyWordings(ByVal GetStandardPolicyWordingsRequest As GetStandardPolicyWordingsRequestType) As GetStandardPolicyWordingsResponseType

    ''' <summary>
    ''' TO GetSubAgents details provided in request
    ''' </summary>
    ''' <param name="GetSubAgentsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetSubAgents(ByVal GetSubAgentsRequest As GetSubAgentsRequestType) As GetSubAgentsResponseType

    ''' <summary>
    ''' This Web method is used to get the taxes 
    ''' </summary>
    ''' <param name="GetTaxesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetTaxes(ByVal GetTaxesRequest As GetTaxesRequestType) As GetTaxesResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetTreatyPartyDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetTreatyPartyDetails(ByVal oGetTreatyPartyDetailsRequest As GetTreatyPartyDetailsRequestType) As GetTreatyPartyDetailsResponseType


    ''' <summary>
    ''' TO LapseRenewal details provided in request
    ''' </summary>
    ''' <param name="LapseRenewalRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function LapseRenewal(ByVal LapseRenewalRequest As LapseRenewalRequestType) As LapseRenewalResponseType

    ''' <summary>
    ''' This web services method is used to Run Renewal Accept.
    '''  </summary>
    '''<param name = "RunRenewalAcceptRequest"></param>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Sub RunRenewalAccept(ByVal RunRenewalAcceptRequest As RunRenewalAcceptRequestType)

    ''' <summary>
    ''' This web services method is used to  Run Renewal Selection.
    '''  </summary>
    '''<param name = "RunRenewalSelectionRequest"></param>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Sub RunRenewalSelection(ByVal RunRenewalSelectionRequest As RunRenewalSelectionRequestType)

    ''' <summary>
    ''' This web services method is used to Run Renewal Selection By Policy.
    '''  </summary>
    '''<param name = "RunRenewalSelectionByPolicyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function RunRenewalSelectionByPolicy(ByVal RunRenewalSelectionByPolicyRequest As RunRenewalSelectionByPolicyRequestType) As RunRenewalSelectionByPolicyResponseType

    ''' This web services method is used to Run Validation Rules.
    '''  </summary>
    '''<param name = "RunValidationRulesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function RunValidationRules(ByVal RunValidationRulesRequest As RunValidationRulesRequestType) As RunValidationRulesResponseType

    ''' <summary>
    ''' TO Save Risk details provided in request
    ''' </summary>
    ''' <param name="SaveRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function SaveRisk(ByVal SaveRiskRequest As SaveRiskRequestType) As SaveRiskResponseType

    ''' <summary>
    ''' TO UpdateAgentCommission details provided in request
    ''' </summary>
    ''' <param name="UpdateAgentCommissionRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateAgentCommission(ByVal UpdateAgentCommissionRequest As UpdateAgentCommissionRequestType) As UpdateAgentCommissionResponseType

    ''' <summary>
    ''' To Update RI Arrangement line - enabled RI 2007
    ''' </summary>
    ''' <param name="oUpdateArrangementLinesRI2007Request"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateArrangementLinesRI2007(ByVal oUpdateArrangementLinesRI2007Request As UpdateArrangementLinesRI2007RequestType) As UpdateArrangementLinesRI2007ResponseType

    ''' <summary>
    ''' To Update RI Arrangement line - disabled RI 2007
    ''' </summary>
    ''' <param name="oUpdateArrangementLinesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateArrangementLines(ByVal oUpdateArrangementLinesRequest As UpdateArrangementLinesRequestType) As UpdateArrangementLinesResponseType

    ''' <summary>
    ''' This web services method is used to Update Fees.
    '''  </summary>
    '''<param name = "UpdateFeeRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateFee(ByVal UpdateFeeRequest As UpdateFeeRequestType) As UpdateFeeResponseType

    ''' <summary>
    ''' TO Update Quote  details provided in request
    ''' </summary>
    ''' <param name="UpdateQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateQuote(ByVal UpdateQuoteRequest As UpdateQuoteRequestType) As UpdateQuoteResponseType

    ''' <summary>
    ''' TO Update Quote Status details provided in request
    ''' </summary>
    ''' <param name="UpdateQuoteStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateQuoteStatus(ByVal UpdateQuoteStatusRequest As UpdateQuoteStatusRequestType) As UpdateQuoteStatusResponseType

    ''' <summary>
    ''' TO Update QuoteV2 details provided in request
    ''' </summary>
    ''' <param name="UpdateQuoteV2Request"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateQuoteV2(ByVal UpdateQuoteV2Request As UpdateQuoteV2RequestType) As UpdateQuoteV2ResponseType

    ''' <summary>
    ''' TO Update Rating Sections details provided in request
    ''' </summary>
    ''' <param name="UpdateRatingSectionsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateRatingSections(ByVal UpdateRatingSectionsRequest As UpdateRatingDetailsRequestType) As UpdateRatingDetailsResponseType

    ''' <summary>
    ''' TO Update Renewal Status details provided in request
    ''' </summary>
    ''' <param name="UpdateRenewalStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateRenewalStatus(ByVal UpdateRenewalStatusRequest As UpdateRenewalStatusRequestType) As UpdateRenewalStatusResponseType

    ''' <summary>
    ''' TO Update Risk details provided in request
    ''' </summary>
    ''' <param name="UpdateRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateRisk(ByVal UpdateRiskRequest As UpdateRiskRequestType) As UpdateRiskResponseType

    ''' <summary>
    ''' TO Update Risk Selection details provided in request
    ''' </summary>
    ''' <param name="UpdateRiskSelectionRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateRiskSelection(ByVal UpdateRiskSelectionRequest As UpdateRiskSelectionRequestType) As UpdateRiskSelectionResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oUpdateRiskStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateRiskStatus(ByVal oUpdateRiskStatusRequest As UpdateRiskStatusRequestType) As UpdateRiskStatusResponseType

    ''' <summary>
    ''' This web services method is used to Add Address.
    '''  </summary>
    '''<param name = "UpdateStandardPolicyWordingsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateStandardPolicyWordings(ByVal UpdateStandardPolicyWordingsRequest As UpdateStandardPolicyWordingsRequestType) As UpdateStandardPolicyWordingsResponseType

    ''' <summary>
    ''' TO update Taxes details provided in request
    ''' </summary>
    ''' <param name="UpdateTaxesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateTaxes(ByVal UpdateTaxesRequest As UpdateTaxesRequestType) As UpdateTaxesResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oCalculateRITaxRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function CalculateRITax(ByVal oCalculateRITaxRequest As CalculateRITaxRequestType) As CalculateRITaxResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oFindReinsurerRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function FindReinsurer(ByVal oFindReinsurerRequest As FindReinsurerRequestType) As FindReinsurerResponseType

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
    ''' TO GetProductByAgent details provided in request
    ''' </summary>
    ''' <param name="GetProductByAgentRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetProductByAgent(ByVal GetProductByAgentRequest As GetProductByAgentRequestType) As GetProductByAgentResponseType

    ''' <summary>
    ''' TO update Taxes details provided in request
    ''' </summary>
    ''' <param name="GetRiskByProductRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetRiskByProduct(ByVal GetRiskByProductRequest As GetRiskByProductRequestType) As GetRiskByProductResponseType

    ''' <summary>
    ''' TO Update Sub Agents details provided in request
    ''' </summary>
    ''' <param name="UpdateSubAgentsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateSubAgents(ByVal UpdateSubAgentsRequest As UpdateSubAgentsRequestType) As UpdateSubAgentsResponseType

    ''' <summary>
    ''' To search partied for given search criteria
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="FindPartyPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function FindParty(ByVal oRequest As FindPartyRequestType) As FindPartyResponseType

    ''' <summary>
    ''' To get a single party detail for given party id
    ''' </summary>
    ''' <param name="GetPartyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetPartyPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetParty(ByVal GetPartyRequest As GetPartyRequestType) As GetPartyResponseType

    ''' <summary>
    ''' TO update a party with details provided in request
    ''' </summary>
    ''' <param name="UpdatePartyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="UpdatePartyPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateParty(ByVal UpdatePartyRequest As UpdatePartyRequestType) As UpdatePartyResponseType

    ''' <summary>
    ''' This web services method is used to Get Party Bank Details.
    '''  </summary>
    '''<param name = "GetPartyBankDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetPartyBankDetailsPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetPartyBankDetails(ByVal GetPartyBankDetailsRequest As GetPartyBankDetailsRequestType) As GetPartyBankDetailsResponseType

    ''' <summary>
    ''' This web services method is used to Get Party Policies.
    '''  </summary>
    '''<param name = "GetPartyPoliciesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetPartyPoliciesPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetPartyPolicies(ByVal GetPartyPoliciesRequest As GetPartyPoliciesRequestType) As GetPartyPoliciesResponseType

    ''' <summary>
    ''' To Add an Event with details provided in request
    ''' </summary>
    ''' <param name="oAddEventRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="AddEventPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function AddEvent(ByVal oAddEventRequest As AddEventRequestType) As AddEventResponseType


    ''' <summary>
    ''' This web services method is used to Get Address.
    '''  </summary>
    '''<param name = "GetAddressRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetAddressPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetAddress(ByVal GetAddressRequest As GetAddressRequestType) As GetAddressResponseType

    ''' <summary>
    ''' GetCurrenciesByBranch
    ''' </summary>
    ''' <param name="GetCurrenciesByBranchRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetCurrenciesByBranchPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetCurrenciesByBranch(ByVal GetCurrenciesByBranchRequest As GetCurrenciesByBranchRequestType) As GetCurrenciesByBranchResponseType

    ''' <summary>
    ''' GetDMEFolder
    ''' </summary>
    ''' <param name="GetDMEFolderRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetDMEFolderPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetDMEFolder(ByVal GetDMEFolderRequest As GetDMEFolderRequestType) As GetDMEFolderResponseType

    ''' <summary>
    ''' GetDatasetDefinition
    ''' </summary>
    ''' <param name="GetDatasetDefinitionRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetDatasetDefinitionPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetDatasetDefinition(ByVal GetDatasetDefinitionRequest As GetDatasetDefinitionRequestType) As GetDatasetDefinitionResponseType

    ''' <summary>
    ''' TO Get List  details provided in request
    ''' </summary>
    ''' <param name="GetListRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetListPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetList(ByVal GetListRequest As GetListRequestType) As GetListResponseType

    ''' <summary>
    ''' TO Get List  details from an ICCS SPU provided in request
    ''' </summary>
    ''' <param name="GetListRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetListSPUICCSPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetListSPUICCS(ByVal GetListRequest As GetListRequestType) As GetListResponseType

    ''' <summary>
    ''' TO GetProductRiskOptionValue details provided in request
    ''' </summary>
    ''' <param name="GetProductRiskOptionValueRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetProductRiskOptionValuePolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetProductRiskOptionValue(ByVal GetProductRiskOptionValueRequest As ProductRiskOptionValueRequestType) As ProductRiskOptionValueResponseType

    ''' <summary>
    ''' To get logged-in userdetail
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetUserDetailsPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetUserDetails(ByVal oRequest As GetUserDetailsRequestType) As GetUserDetailsResponseType

    ''' <summary>
    ''' TO Get User Group Users details provided in request
    ''' </summary>
    ''' <param name="GetUserGroupUsersRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetUserGroupUsersPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetUserGroupUsers(ByVal GetUserGroupUsersRequest As GetUserGroupUsersRequestType) As GetUserGroupUsersResponseType

    ''' <summary>
    ''' TO Get User Groups details provided in request 
    ''' </summary>
    ''' <param name="GetUserGroupsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetUserGroupsPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetUserGroups(ByVal GetUserGroupsRequest As GetUserGroupsRequestType) As GetUserGroupsResponseType

    ''' <summary>
    ''' This Web method is used to get all the work manager scheduled tasks
    ''' </summary>
    ''' <param name="GetWorkManagerScheduledTasksRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetWorkManagerScheduledTasksPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetWorkManagerScheduledTasks(ByVal GetWorkManagerScheduledTasksRequest As GetWorkManagerScheduledTasksRequestType) As GetWorkManagerScheduledTasksResponseType

    ''' <summary>
    ''' GetEventDetails
    ''' </summary>
    ''' <param name="oGetEventDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetEventDetailsPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetEventDetails(ByVal oGetEventDetailsRequest As GetEventDetailsRequestType) As GetEventDetailsResponseType

    ''' <summary>
    ''' TO Get Option Setting details provided in request
    ''' </summary>
    ''' <param name="GetOptionSettingRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetOptionSettingPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetOptionSetting(ByVal GetOptionSettingRequest As GetOptionSettingRequestType) As GetOptionSettingResponseType


    ''' <summary>
    ''' This web services method is used to Add Address.
    '''  </summary>
    '''<param name = "GetUserAuthorityValueRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetUserAuthorityValuePolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetUserAuthorityValue(ByVal GetUserAuthorityValueRequest As GetUserAuthorityValueRequestType) As GetUserAuthorityValueResponseType

    ''' <summary>
    ''' This web services method is used to Find Claim.
    ''' </summary>
    ''' <param name="FindClaimRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="FindClaimPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function FindClaim(ByVal FindClaimRequest As FindClaimRequestType) As FindClaimResponseType

    ''' <summary>  
    ''' This method is used to find accounts in claim payment
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.FindAccountsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.FindAccountsResponseType</returns>  
    <OperationContract(Name:="FindAccountsPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function FindAccounts(ByVal oRequest As FindAccountsRequestType) As FindAccountsResponseType

    ''' <summary>
    ''' TO Get Account details 
    ''' </summary>
    ''' <param name="GetAccountDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetAccountDetailsAccount")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetAccountDetails(ByVal GetAccountDetailsRequest As GetAccountDetailsRequestType) As GetAccountDetailsResponseType

    ''' <summary>
    ''' This Web method is used to run the default rules files explicitly
    ''' </summary>
    ''' <param name="RunDefaultRulesAddRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="RunDefaultRulesAddPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function RunDefaultRulesAdd(ByVal RunDefaultRulesAddRequest As RunDefaultRulesAddRequestType) As RunDefaultRulesAddResponseType

    ''' <summary>
    ''' This web services method is used to Run Default Rules Edit.
    '''  </summary>
    '''<param name = "RunDefaultRulesEditRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="RunDefaultRulesPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function RunDefaultRulesEdit(ByVal RunDefaultRulesEditRequest As RunDefaultRulesEditRequestType) As RunDefaultRulesEditResponseType

    ''' <summary>
    ''' TO update Taxes details provided in request
    ''' </summary>
    ''' <param name="GetProductRiskEventsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetProductRiskEventsPolicy")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetProductRiskEvents(ByVal GetProductRiskEventsRequest As GetProductRiskEventsRequestType) As GetProductRiskEventsResponseType

    ''' <summary>
    ''' RunRenewalInvitation
    ''' </summary>
    ''' <param name="RunRenewalInviteRequest"></param>
    ''' <remarks></remarks>
    <OperationContract(Name:="RunRenewalInvitation")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Sub RunRenewalInvitation(ByVal RunRenewalInviteRequest As RunRenewalInviteRequestType)

    ''' <summary>
    ''' CreatePostingsForReinsurance
    ''' </summary>
    ''' <param name="CreatePostingsForReinsuranceRequest"></param>
    ''' <remarks></remarks>
    <OperationContract(Name:="CreatePostingsForReinsurance")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function CreatePostingsForReinsurance(ByVal CreatePostingsForReinsuranceRequest As CreatePostingsForReinsuranceRequestType) As CreatePostingsForReinsuranceResponseType

    ''' <summary>
    ''' CreatePostingsForReinsurance
    ''' </summary>
    ''' <param name="UpdateRIAmendmentStatusRequest"></param>
    ''' <remarks></remarks>
    <OperationContract(Name:="UpdateRIAmendmentStatus")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateRIAmendmentStatus(ByVal UpdateRIAmendmentStatusRequest As UpdateRIAmendmentStatusRequestType) As UpdateRIAmendmentStatusResponseType

    ''' <summary>
    ''' RecalculateRIForPortfolioTransfer
    ''' </summary>
    ''' <param name="RecalculateRIForPortfolioTransferRequest"></param>
    ''' <remarks></remarks>
    <OperationContract(Name:="RecalculateRIForPortfolioTransfer")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function RecalculateRIForPortfolioTransfer(ByVal RecalculateRIForPortfolioTransferRequest As RecalculateRIForPortfolioTransferRequestType) As RecalculateRIForPortfolioTransferResponseType

    ''' <summary>
    ''' RecalculateRIForCloneTransfer
    ''' </summary>
    ''' <param name="RecalculateRIForCloneTransferRequest"></param>
    ''' <remarks></remarks>
    <OperationContract(Name:="RecalculateRIForCloneTransfer")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function RecalculateRIForCloneTransfer(ByVal RecalculateRIForCloneTransferRequest As RecalculateRIForCloneTransferRequestType) As RecalculateRIForCloneTransferResponseType
    ''' <summary>
    ''' GetPTPoliciesForAmend
    ''' </summary>
    ''' <param name="GetPTPoliciesForAmendRequest"></param>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetPTPoliciesForAmend")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetPTPoliciesForAmend(ByVal GetPTPoliciesForAmendRequest As GetPTPoliciesForAmendRequestType) As GetPTPoliciesForAmendResponseType

    ''' <summary>
    ''' GetClonePoliciesForAmend
    ''' </summary>
    ''' <param name="GetClonePoliciesForAmendRequest"></param>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetClonePoliciesForAmend")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetClonePoliciesForAmend(ByVal GetClonePoliciesForAmendRequest As GetClonePoliciesForAmendRequestType) As GetClonePoliciesForAmendResponseType

    ''' <summary>
    ''' IsPendingTransfer
    ''' </summary>
    ''' <param name="IsPendingTransferRequest"></param>
    ''' <remarks></remarks>
    <OperationContract(Name:="IsPendingTransfer")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function IsPendingTransfer(ByVal IsPendingTransferRequest As IsPendingTransferRequestType) As IsPendingTransferResponseType

    ''' <summary>
    ''' IsAnniversaryDateEditable 
    ''' </summary>
    ''' <param name="IsAnniversaryDateEditableRequest"></param>
    ''' <remarks></remarks>
    <OperationContract(Name:="IsAnniversaryDateEditable")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function IsAnniversaryDateEditable(ByVal IsAnniversaryDateEditableRequest As IsAnniversaryDateEditableRequestType) As IsAnniversaryDateEditableResponseType

    ''' <summary>
    ''' GetRIVersion
    ''' </summary>
    ''' <param name="GetRIVersionRequest"></param>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetRIVersion")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetRIVersion(ByVal GetRIVersionRequest As GetRIVersionRequestType) As GetRIVersionResponseType


    ''' <summary>
    ''' RunPortfolioTransfer
    ''' </summary>
    ''' <param name="RunPortfolioTransferRequest"></param>
    ''' <remarks></remarks>
    <OperationContract(Name:="RunPortfolioTransfer")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function RunPortfolioTransfer(ByVal RunPortfolioTransferRequest As RunPortfolioTransferRequestType) As RunPortfolioTransferResponseType

    ''' <summary>
    ''' RunCloneRework
    ''' </summary>
    ''' <param name="RunCloneReworkRequest"></param>
    ''' <remarks></remarks>
    <OperationContract(Name:="RunCloneRework")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function RunCloneRework(ByVal RunCloneReworkRequest As RunCloneReworkRequestType) As RunCloneReworkResponseType
    ''' <summary>
    ''' TO RunBatchRenewal details provided in request
    ''' </summary>
    ''' <param name="RunBatchRenewalRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function RunBatchRenewal(ByVal RunBatchRenewalRequest As RunBatchRenewalRequestType) As RunBatchRenewalResponseType


    ''' <summary>
    ''' RunRenewalInvitation
    ''' </summary>
    ''' <param name="RunRenewalSelectionSyncRequest"></param>
    ''' <remarks></remarks>
    <OperationContract(Name:="RunRenewalSelectionSync")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function RunRenewalSelectionSync(ByVal RunRenewalSelectionSyncRequest As RunRenewalSelectionSyncRequestType)

    ''' <summary>
    ''' RunRenewalInvitation
    ''' </summary>
    ''' <param name="RunRenewalInviteSyncRequest"></param>
    ''' <remarks></remarks>
    <OperationContract(Name:="RunRenewalInvitationSync")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function RunRenewalInvitationSync(ByVal RunRenewalInviteSyncRequest As RunRenewalInviteSyncRequestType)

    ''' <summary>
    ''' RunRenewalInvitation
    ''' </summary>
    ''' <param name="RunRenewalAcceptSyncRequest"></param>
    ''' <remarks></remarks>
    <OperationContract(Name:="RunRenewalAcceptSync")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function RunRenewalAcceptSync(ByVal RunRenewalAcceptSyncRequest As RunRenewalAcceptSyncRequestType)
    ''' <summary>
    ''' To cancel (delete) a MTA quote
    ''' </summary>
    ''' <param name="CancelRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function CancelQuote(ByVal CancelRequest As CancelQuoteRequestType) As CancelQuoteResponseType

    <OperationContract(Name:="ProcessPfPlan")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ProcessPfPlan(ByVal oProcessPfPlanRequest As ProcessPFPlanRequestType) As ProcessPFPlanResponseType

    <OperationContract(Name:="UpdateInstalmentStatus")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateInstalmentStatus(ByVal oUpdateInstalmentStatusRequestType As UpdateInstalmentStatusRequestType) As UpdateInstalmentStatusResponseType

    <OperationContract(Name:="CancelPremiumFinancePlan")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CancelPremiumFinancePlan(ByVal oCancelPremiumFinancePlanRequestType As CancelPremiumFinancePlanRequestType) As CancelPremiumFinancePlanResponseType

    <OperationContract(Name:="CancelPFPolicies")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CancelPFPolicies(ByVal oCancelPFPoliciesRequestType As CancelPFPoliciesRequestType) As CancelPFPoliciesResponseType

    <OperationContract(Name:="GetHeaderAndSummariesPFPlanByKey")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetHeaderAndSummariesPFPlanByKey(ByVal oGetHeaderAndSummariesPFPlanByKeyRequestType As GetHeaderAndSummariesPFPlanByKeyRequestType) As GetHeaderAndSummariesPFPlanByKeyResponseType

    <OperationContract(Name:="GetFinancePlanInformation")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetFinancePlanInformation(ByVal oGetFinancePlanInformationRequestType As GetFinancePlanInformationRequestType) As GetFinancePlanInformationResponseType


    ''' <summary>
    ''' TO Update Market Place Policy Status details provided in request
    ''' </summary>
    ''' <param name="UpdateMarketplacePolicyStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="UpdateMarketplacePolicyStatus")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateMarketplacePolicyStatus(ByVal UpdateMarketplacePolicyStatusRequest As UpdateMarketplacePolicyStatusRequestType) As UpdateMarketplacePolicyStatusResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oFindPoliciesByRiskindexRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="FindPoliciesByRiskindex")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function FindPoliciesByRiskindex(ByVal oFindPoliciesByRiskindexRequest As FindPoliciesByRiskIndexRequestType) As FindPoliciesByRiskIndexResponseType

    ''' <summary>
    ''' Save Premium Finance Details
    ''' </summary>
    ''' <param name="SavePremiumFinanceDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function SavePremiumFinanceDetails(ByVal SavePremiumFinanceDetailsRequest As SavePremiumFinanceDetailsRequestType) As SavePremiumFinanceDetailsResponseType

    ''' <summary>
    ''' Update Policy Payment Method
    ''' </summary>
    ''' <param name="UpdatePolicyPaymentMethodRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdatePolicyPaymentMethod(ByVal UpdatePolicyPaymentMethodRequest As UpdatePolicyPaymentMethodRequestType) As UpdatePolicyPaymentMethodResponseType


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetDocumentTemplateStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="CheckDocumentTemplateExists")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function CheckDocumentTemplateExists(ByVal oGetDocumentTemplateStatusRequest As GetDocumentTemplateStatusRequestType) As GetDocumentTemplateStatusResponseType


    ''' <summary>
    ''' This method will return boolean flag depending on pending OOS version on policy
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>3
    <OperationContract(Name:="CheckPendingOOSVersions")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function CheckPendingOOSVersions(ByVal oRequest As CheckPendingOOSVersionsRequestType) As CheckPendingOOSVersionsResponseType


    ''' <summary>
    ''' To get latest policy versions
    ''' </summary>
    ''' <param name="oFindPolicyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function FindLatestPolicyVersions(ByVal oFindPolicyRequest As FindLatestPolicyVersionsRequestType) As FindLatestPolicyVersionsResponseType

    ''' <summary>
    ''' To get instalment settlement amount
    ''' </summary>
    ''' <param name="oGetInstalmentSettlementAmountRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetInstalmentSettlementAmount(ByVal oGetInstalmentSettlementAmountRequest As GetInstalmentSettlementAmountRequestType) As GetInstalmentSettlementAmountResponseType

    ''' <summary>
    ''' To Cancel MTA Quote
    ''' </summary>
    ''' <param name="oCancelMTAQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function CancelMTAQuote(ByVal oCancelMTAQuoteRequest As CancelMTAQuoteRequestType) As CancelMTAQuoteResponseType

    ''' <summary>
    ''' To Execute PRE ruleset
    ''' </summary>
    ''' <param name="oExecutePRERulesetRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function ExecutePRERuleset(ByVal oExecutePRERulesetRequest As ExecutePRERulesetRequestType) As ExecutePRERulesetResponseType

    ' <summary>
    ' TO Get Policy Associates details provided in request
    ' </summary>
    ' <param name="UpdateMarketplacePolicyStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetPolicyAssociates")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPolicyAssociates(ByVal PolicyAssociatesRequest As GetPolicyAssociatesRequestType) As GetPolicyAssociatesResponsType

    ' <summary>
    ' TO Update Policy Associates details provided in request
    ' </summary>
    ' <param name="UpdateMarketplacePolicyStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="UpdatePolicyAssociates")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdatePolicyAssociates(ByVal UpdatePolicyAssociatesRequest As UpdatePolicyAssociatesRequestType) As UpdatePolicyAssociatesResponsType


    ''' <summary>
    ''' Reverse Collected Instalment
    ''' </summary>
    ''' <param name="ReverseInstalmentRequestType"></param>
    ''' <returns>ReverseCollectedInstalmentResponseType</returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function ReverseCollectedInstalment(ByVal ReverseInstalmentRequestType As ReverseCollectedInstalmentRequestType) As ReverseCollectedInstalmentResponseType

    ''' <summary>
    ''' GetRITreatyPartyDetailsWithTax
    ''' </summary>
    ''' <param name="GetRITreatyPartyDetailsWithTaxType"></param>
    ''' <returns></returns>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetRITreatyPartyDetailsWithTax(ByVal GetRITreatyPartyDetailsWithTaxType As GetRITreatyPartyDetailsWithTaxRequestType) As GetRITreatyPartyDetailsWithTaxResponseType
    ''' <summary>
    ''' UpdateRiOverrideReasonInRiArrangement
    ''' </summary>
    ''' <param name="oUpdateRiOverrideReasonInRiArrangementRequest"></param>
    ''' <returns></returns>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateRiOverrideReasonInRiArrangement(ByVal oUpdateRiOverrideReasonInRiArrangementRequest As UpdateRiOverrideReasonInRiArrangementRequestType) As UpdateRiOverrideReasonInRiArrangementResponseType

    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetRIPropTreaties(ByVal oGetRIPropTreatiesRequest As GetRIPropTreatiesRequestType) As GetRIPropTreatiesResponseType

    ''' <summary>
    ''' Update Instalment Details
    ''' </summary>
    ''' <param name="oUpdateInstalmentDetailsRequestType"></param>
    ''' <returns></returns>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateInstalmentDetails(ByVal oUpdateInstalmentDetailsRequestType As UpdateInstalmentDetailsRequestType) As UpdateInstalmentDetailsResponseType

End Interface
