Imports SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.WCF

<ServiceContract()>
Public Interface IPureClaimService
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
    ''' <param name="AddClaimRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddClaimRisk(ByVal AddClaimRiskRequest As AddClaimRiskRequestType) As AddClaimRiskResponseType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ApproveCashListItemRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
   <FaultContract(GetType(SAMMethodResponseData))> _
    Function ApproveCashListItem(ByVal ApproveCashListItemRequest As ApproveCashListItemRequestType) As ApproveCashListItemResponseType

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
    ''' This web services method is used to CaseLinkUnlink.
    ''' </summary>
    ''' <param name="oCaseLinkUnlinkRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CaseLinkUnlink(ByVal oCaseLinkUnlinkRequest As CaseLinkUnLinkRequestType) As CaseLinkUnLinkResponseType

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
    ''' TO Copy Risk details provided in request
    ''' </summary>
    ''' <param name="CopyRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CopyRisk(ByVal CopyRiskRequest As CopyRiskRequestType) As CopyRiskResponseType

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
    ''' TO CreateReceiptCashListItem details provided in request
    ''' </summary>
    ''' <param name="CreateReceiptCashListItemRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CreateReceiptCashListItem(ByVal CreateReceiptCashListItemRequest As CreateReceiptCashListItemRequestType) As CreateReceiptCashListItemResponseType

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
    ''' This web services method is used to Find Case.
    ''' </summary>
    ''' <param name="oFindCaseRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindCase(ByVal oFindCaseRequest As FindCaseRequestType) As FindCaseResponseType

    ''' <summary>
    ''' This web services method is used to Find Claim.
    ''' </summary>
    ''' <param name="FindClaimRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="FindClaim")>
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
    ''' <param name="oGetTaxTypesAndBandsRequestType"></param>
    ''' <returns></returns>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetTaxTypesAndBand(ByVal oGetTaxTypesAndBandsRequestType As GetTaxTypesAndBandsRequestType) As GetTaxTypesAndBandsResponseType


    <OperationContract>
 <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimCoinsurer(ByVal oRequest As GetClaimCoinsurerRequestType) As GetClaimCoinsurerResponseType

    <OperationContract>
 <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimDetails(ByVal oRequest As GetClaimDetailsRequestType) As GetClaimDetailsResponseType


    ''' <summary>
    ''' This web services method is used to Get Claim Party Details.
    '''  </summary>
    '''<param name = "GetClaimPartyDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimPartyDetails(ByVal GetClaimPartyDetailsRequest As GetClaimPartyDetailsRequestType) As GetClaimPartyDetailsResponseType


    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimPaymentTaxes(ByVal GetClaimPaymentTaxesRequest As GetClaimPaymentTaxesRequestType) As GetClaimPaymentTaxesResponseType

    ''' <summary>
    ''' TO Get ClaimPaymentTaxGroups with details provided in request
    ''' </summary>
    ''' <param name="GetClaimPaymentTaxGroupsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimPaymentTaxGroups(ByVal GetClaimPaymentTaxGroupsRequest As GetClaimPaymentTaxGroupsRequestType) As GetClaimPaymentTaxGroupsResponseType



    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimPerilSummary(ByVal GetClaimPerilSummaryRequest As GetClaimPerilSummaryRequestType) As GetClaimPerilSummaryResponseType


    <OperationContract>
   <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimReceiptTaxes(ByVal GetClaimReceiptTaxesRequest As GetClaimReceiptTaxesRequestType) As GetClaimReceiptTaxesResponseType

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
    ''' TO GetClaimReinsuranceBands details provided in request
    ''' </summary>
    ''' <param name="GetClaimReinsuranceBandsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimReinsuranceBands(ByVal GetClaimReinsuranceBandsRequest As GetClaimReinsuranceBandsRequestType) As GetClaimReinsuranceBandsResponseType


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
    ''' TO GetClaimReinsuranceArrangements details provided in request
    ''' </summary>
    ''' <param name="GetClaimReinsuranceArrangementsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimReinsuranceArrangements(ByVal GetClaimReinsuranceArrangementsRequest As GetClaimReinsuranceArrangementsRequestType) As GetClaimReinsuranceArrangementsResponseType


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetClaimRIArrangementLinesRI2007Request"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimRIArrangementLinesRI2007(ByVal oGetClaimRIArrangementLinesRI2007Request As GetClaimRIArrangementLinesRI2007RequestType) As GetClaimRIArrangementLinesRI2007ResponseType

    <OperationContract>
<FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimRisk(ByVal oRequest As GetClaimRiskRequestType) As GetClaimRiskResponseType

    ''' <summary>
    ''' TO Get Claim Risk Links details provided in request 
    ''' </summary>
    ''' <param name="GetClaimRiskLinksRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimRiskLinks(ByVal GetClaimRiskLinksRequest As GetClaimRiskLinksRequestType) As GetClaimRiskLinksResponseType

    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClaimRiskReadOnly(ByVal GetClaimRiskReadOnlyRequest As GetClaimRiskReadOnlyRequestType) As GetClaimRiskReadOnlyResponseType

  
    ''' <summary>
    ''' GetCurrencyExchangeRates
    ''' </summary>
    ''' <param name="oGetCurrencyExchangeRatesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetCurrencyExchangeRatesClaim")>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetCurrencyExchangeRates(ByVal oGetCurrencyExchangeRatesRequest As GetCurrencyExchangeRatesRequestType) As GetCurrencyExchangeRatesResponseType


    ''' <summary>
    ''' TO Get Product Claims Work flow Options details provided in request 
    ''' </summary>
    ''' <param name="GetProductClaimsWorkflowOptionsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetProductClaimsWorkflowOptions(ByVal GetProductClaimsWorkflowOptionsRequest As GetProductClaimsWorkflowOptionsRequestType) As GetProductClaimsWorkflowOptionsResponseType

 
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
    ''' TO GetRecoveryReinsurance details provided in request
    ''' </summary>
    ''' <param name="GetRecoveryReinsuranceRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetRecoveryReinsurance(ByVal GetRecoveryReinsuranceRequest As GetRecoveryReinsuranceRequestType) As GetRecoveryReinsuranceResponseType

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
    ''' TO Get Tax Groups For Claims details provided in request
    ''' </summary>
    ''' <param name="GetTaxGroupsForClaimsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetTaxGroupsForClaims(ByVal GetTaxGroupsForClaimsRequest As GetTaxGroupsForClaimsRequestType) As GetTaxGroupsForClaimsResponseType

    <OperationContract>
 <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUnallocatedClaimPayments(ByVal oRequest As GetUnallocatedClaimPaymentsRequestType) As GetUnallocatedClaimPaymentsResponseType

    ''' <summary>
    ''' This Web method is used to get all the Valid Primary Causes
    ''' </summary>
    ''' <param name="GetValidPrimaryCausesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetValidPrimaryCauses(ByVal GetValidPrimaryCausesRequest As GetValidPrimaryCausesRequestType) As GetValidPrimaryCausesResponseType


    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetVersionsForClaim(ByVal oRequest As GetVersionsForClaimRequestType) As GetVersionsForClaimResponseType

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


    ''' This web services method is used to Run Validation Rules.
    '''  </summary>
    '''<param name = "RunValidationRulesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="RunValidationRulesClaim")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function RunValidationRules(ByVal RunValidationRulesRequest As RunValidationRulesRequestType) As RunValidationRulesResponseType

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
    ''' UpdateClaimReservesOrPayments
    ''' </summary>
    ''' <param name="oUpdateClaimReservesOrPaymentsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateClaimReservesOrPayments(ByVal oUpdateClaimReservesOrPaymentsRequest As UpdateClaimReservesOrPaymentsRequestType) As UpdateClaimReservesOrPaymentsResponseType

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
    ''' UpdateClaimRisk
    ''' </summary>
    ''' <param name="UpdateClaimRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateClaimRisk(ByVal UpdateClaimRiskRequest As UpdateClaimRiskRequestType) As UpdateClaimRiskResponseType

    ''' <summary>
    ''' TO CheckUnpaidPremium details provided in request
    ''' </summary>
    ''' <param name="CheckUnpaidPremiumRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CheckUnpaidPremium(ByVal CheckUnpaidPremiumRequest As CheckUnpaidPremiumRequestType) As CheckUnpaidPremiumResponseType


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
    ''' To search partied for given search criteria
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="FindPartyClaim")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindParty(ByVal oRequest As FindPartyRequestType) As FindPartyResponseType

    ''' <summary>
    ''' To get a single party detail for given party id
    ''' </summary>
    ''' <param name="GetPartyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetPartyClaim")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetParty(ByVal GetPartyRequest As GetPartyRequestType) As GetPartyResponseType

    ''' <summary>
    ''' This web services method is used to Get Party Bank Details.
    '''  </summary>
    '''<param name = "GetPartyBankDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetPartyBankDetailsClaim")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPartyBankDetails(ByVal GetPartyBankDetailsRequest As GetPartyBankDetailsRequestType) As GetPartyBankDetailsResponseType

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
    ''' GetCurrenciesByBranch
    ''' </summary>
    ''' <param name="GetCurrenciesByBranchRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetCurrenciesByBranchClaim")>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetCurrenciesByBranch(ByVal GetCurrenciesByBranchRequest As GetCurrenciesByBranchRequestType) As GetCurrenciesByBranchResponseType

    ''' <summary>
    ''' GetDMEFolder
    ''' </summary>
    ''' <param name="GetDMEFolderRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetDMEFolderClaim")>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetDMEFolder(ByVal GetDMEFolderRequest As GetDMEFolderRequestType) As GetDMEFolderResponseType

    ''' <summary>
    ''' GetDatasetDefinition
    ''' </summary>
    ''' <param name="GetDatasetDefinitionRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetDatasetDefinitionClaim")>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetDatasetDefinition(ByVal GetDatasetDefinitionRequest As GetDatasetDefinitionRequestType) As GetDatasetDefinitionResponseType

    ''' <summary>
    ''' TO Get List  details provided in request
    ''' </summary>
    ''' <param name="GetListRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetListClaim")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetList(ByVal GetListRequest As GetListRequestType) As GetListResponseType

    ''' <summary>
    ''' TO Get List  details from an ICCS SPU provided in request
    ''' </summary>
    ''' <param name="GetListRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetListSPUICCSClaim")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetListSPUICCS(ByVal GetListRequest As GetListRequestType) As GetListResponseType

    ''' <summary>
    ''' TO GetProductRiskOptionValue details provided in request
    ''' </summary>
    ''' <param name="GetProductRiskOptionValueRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetProductRiskOptionValueClaim")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetProductRiskOptionValue(ByVal GetProductRiskOptionValueRequest As ProductRiskOptionValueRequestType) As ProductRiskOptionValueResponseType

    ''' <summary>
    ''' To get logged-in userdetail
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetUserDetailsClaim")> _
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserDetails(ByVal oRequest As GetUserDetailsRequestType) As GetUserDetailsResponseType

    ''' <summary>
    ''' TO Get User Group Users details provided in request
    ''' </summary>
    ''' <param name="GetUserGroupUsersRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetUserGroupUsersClaim")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserGroupUsers(ByVal GetUserGroupUsersRequest As GetUserGroupUsersRequestType) As GetUserGroupUsersResponseType

    ''' <summary>
    ''' TO Get User Groups details provided in request 
    ''' </summary>
    ''' <param name="GetUserGroupsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetUserGroupsClaim")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserGroups(ByVal GetUserGroupsRequest As GetUserGroupsRequestType) As GetUserGroupsResponseType

    ''' <summary>
    ''' This Web method is used to get all the work manager scheduled tasks
    ''' </summary>
    ''' <param name="GetWorkManagerScheduledTasksRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetWorkManagerScheduledTasksClaim")>
      <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetWorkManagerScheduledTasks(ByVal GetWorkManagerScheduledTasksRequest As GetWorkManagerScheduledTasksRequestType) As GetWorkManagerScheduledTasksResponseType

    ''' <summary>
    ''' TO Get Option Setting details provided in request
    ''' </summary>
    ''' <param name="GetOptionSettingRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetOptionSettingClaim")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetOptionSetting(ByVal GetOptionSettingRequest As GetOptionSettingRequestType) As GetOptionSettingResponseType


    ''' <summary>
    ''' This web services method is used to Add Address.
    '''  </summary>
    '''<param name = "GetUserAuthorityValueRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetUserAuthorityValueClaim")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserAuthorityValue(ByVal GetUserAuthorityValueRequest As GetUserAuthorityValueRequestType) As GetUserAuthorityValueResponseType

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
    ''' This method is used to find accounts in claim payment
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.FindAccountsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.FindAccountsResponseType</returns>  
    <OperationContract(Name:="FindAccountsClaims")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindAccounts(ByVal oRequest As FindAccountsRequestType) As FindAccountsResponseType


    ''' <summary>
    ''' This web services method is used to Get All Live Policy Version Amounts.
    '''  </summary>
    '''<param name = "GetAllLivePolicyVersionsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetAllPolicyVersionsClaim")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetAllPolicyVersions(ByVal GetAllLivePolicyVersionsRequest As GetAllPolicyVersionsRequestType) As GetAllPolicyVersionsResponseType

    ''' <summary>
    ''' GetHeaderAndSummariesByKey
    ''' </summary>
    ''' <param name="GetHeaderAndSummariesByKeyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetHeaderAndSummariesByKeyClaim")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetHeaderAndSummariesByKey(ByVal GetHeaderAndSummariesByKeyRequest As GetHeaderAndSummariesByKeyRequestType) As GetHeaderAndSummariesByKeyResponseType

    ''' <summary>
    ''' This Web method is used to run the default rules files explicitly
    ''' </summary>
    ''' <param name="RunDefaultRulesAddRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="RunDefaultRulesAddClaim")>
      <FaultContract(GetType(SAMMethodResponseData))> _
    Function RunDefaultRulesAdd(ByVal RunDefaultRulesAddRequest As RunDefaultRulesAddRequestType) As RunDefaultRulesAddResponseType

    ''' <summary>
    ''' This web services method is used to Run Default Rules Edit.
    '''  </summary>
    '''<param name = "RunDefaultRulesEditRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="RunDefaultRulesClaim")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function RunDefaultRulesEdit(ByVal RunDefaultRulesEditRequest As RunDefaultRulesEditRequestType) As RunDefaultRulesEditResponseType

    ''' <summary>
    ''' TO update Taxes details provided in request
    ''' </summary>
    ''' <param name="GetProductRiskEventsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetProductRiskEventsClaim")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetProductRiskEvents(ByVal GetProductRiskEventsRequest As GetProductRiskEventsRequestType) As GetProductRiskEventsResponseType

    <OperationContract(Name:="GenerateCashList")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GenerateCashList(ByVal oGenerateCashListRequest As GenerateCashListRequestType) As GenerateCashListResponseType

    <OperationContract(Name:="GetDefaultBankAccountWithCurrency")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetDefaultBankAccountWithCurrency(ByVal oGetDefaultBankAccountWithCurrencyRequest As GetDefaultBankAccountWithCurrencyRequestType) As GetDefaultBankAccountWithCurrencyResponseType

    ''' <summary>
    ''' UpdateRecommendStatus
    ''' </summary>
    ''' <param name="UpdateRecommendStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="UpdateRecommendStatusClaim")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateRecommendStatus(ByVal UpdateRecommendStatusRequest As UpdateRecommendStatusRequestType) As UpdateRecommendStatusResponseType

    ''' <summary>
    ''' GetProductDocuments
    ''' </summary>
    ''' <param name="oGetProductDocumentsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetProductDocuments")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetProductDocuments(ByVal oGetProductDocumentsRequest As GetProductDocumentsRequestType) As GetProductDocumentsResponseType

    ''' <summary>
    ''' CheckReserveRecovery
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="CheckReserveRecovery")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CheckReserveRecovery(ByVal oRequest As CheckReserveRecoveryRequestType) As CheckReserveRecoveryResponseType

    
    ''' <summary>
    ''' TO Delete claim provided in request
    ''' </summary>
    ''' <param name="oDeleteabandonClaimRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="DeleteAbandonClaim")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function DeleteAbandonClaim(ByVal oDeleteabandonClaimRequest As DeleteAbandonClaimRequestType) As DeleteAbandonClaimResponseType

End Interface
