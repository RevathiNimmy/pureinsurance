
Imports SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.WCF

<ServiceContract()>
Public Interface IPurePartyService

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
    ''' To Add a new party
    ''' </summary>
    ''' <param name="AddPartyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddParty(ByVal AddPartyRequest As AddPartyRequestType) As AddPartyResponseType

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
    ''' To search partied for given search criteria
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindParty(ByVal oRequest As FindPartyRequestType) As FindPartyResponseType


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
    ''' This web services method is used to Replace Party Contact.
    '''  </summary>
    '''<param name = "ReplacePartyContactRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ReplacePartyContact(ByVal ReplacePartyContactRequest As ReplacePartyContactRequestType) As ReplacePartyContactResponseType

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
    ''' This web services method is used to Get Address.
    '''  </summary>
    '''<param name = "GetAddressRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetAddress(ByVal GetAddressRequest As GetAddressRequestType) As GetAddressResponseType

    ''' <summary>
    ''' To Add an Event with details provided in request
    ''' </summary>
    ''' <param name="oAddEventRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="AddEventParty")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddEvent(ByVal oAddEventRequest As AddEventRequestType) As AddEventResponseType

    ''' <summary>
    ''' GetCurrenciesByBranch
    ''' </summary>
    ''' <param name="GetCurrenciesByBranchRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetCurrenciesByBranchParty")>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetCurrenciesByBranch(ByVal GetCurrenciesByBranchRequest As GetCurrenciesByBranchRequestType) As GetCurrenciesByBranchResponseType

    ''' <summary>
    ''' TO Get List  details provided in request
    ''' </summary>
    ''' <param name="GetListRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetListParty")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetList(ByVal GetListRequest As GetListRequestType) As GetListResponseType

    ''' <summary>
    ''' TO Get List  details from an ICCS SPU provided in request
    ''' </summary>
    ''' <param name="GetListRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetListSPUICCSParty")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetListSPUICCS(ByVal GetListRequest As GetListRequestType) As GetListResponseType

    ''' <summary>
    ''' To get logged-in userdetail
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetUserDetailsParty")> _
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserDetails(ByVal oRequest As GetUserDetailsRequestType) As GetUserDetailsResponseType

    ''' <summary>
    ''' TO Get User Group Users details provided in request
    ''' </summary>
    ''' <param name="GetUserGroupUsersRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetUserGroupUsersParty")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserGroupUsers(ByVal GetUserGroupUsersRequest As GetUserGroupUsersRequestType) As GetUserGroupUsersResponseType

    ''' <summary>
    ''' TO Get User Groups details provided in request 
    ''' </summary>
    ''' <param name="GetUserGroupsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetUserGroupsParty")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserGroups(ByVal GetUserGroupsRequest As GetUserGroupsRequestType) As GetUserGroupsResponseType

    ''' <summary>
    ''' This Web method is used to get all the work manager scheduled tasks
    ''' </summary>
    ''' <param name="GetWorkManagerScheduledTasksRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetWorkManagerScheduledTasksParty")>
      <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetWorkManagerScheduledTasks(ByVal GetWorkManagerScheduledTasksRequest As GetWorkManagerScheduledTasksRequestType) As GetWorkManagerScheduledTasksResponseType

    ''' <summary>
    ''' GetEventDetails
    ''' </summary>
    ''' <param name="oGetEventDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetEventDetailsParty")>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetEventDetails(ByVal oGetEventDetailsRequest As GetEventDetailsRequestType) As GetEventDetailsResponseType

    ''' <summary>
    ''' TO Get Option Setting details provided in request
    ''' </summary>
    ''' <param name="GetOptionSettingRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetOptionSettingParty")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetOptionSetting(ByVal GetOptionSettingRequest As GetOptionSettingRequestType) As GetOptionSettingResponseType


    ''' <summary>
    ''' This web services method is used to Add Address.
    '''  </summary>
    '''<param name = "GetUserAuthorityValueRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetUserAuthorityValueParty")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserAuthorityValue(ByVal GetUserAuthorityValueRequest As GetUserAuthorityValueRequestType) As GetUserAuthorityValueResponseType

    ''' <summary>
    ''' This web services method is used to Find Claim.
    ''' </summary>
    ''' <param name="FindClaimRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="FindClaimParty")>
   <FaultContract(GetType(SAMMethodResponseData))>
    Function FindClaim(ByVal FindClaimRequest As FindClaimRequestType) As FindClaimResponseType

    ''' <summary>
    ''' TO Get Account details 
    ''' </summary>
    ''' <param name="GetAccountDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetAccountDetailsParty")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetAccountDetails(ByVal GetAccountDetailsRequest As GetAccountDetailsRequestType) As GetAccountDetailsResponseType

    ''' <summary>
    ''' Update PartyBankDetails
    ''' </summary>
    ''' <param name="UpdatePartyBankDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdatePartyBankDetails(ByVal UpdatePartyBankDetailsRequest As UpdatePartyBankDetailsRequestType) As UpdatePartyBankDetailsResponseType

    ''' <summary>
    ''' ''' Get Client data Extract
    ''' </summary>
    ''' <param name="GetClientDataExtractRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetClientDataExtract")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetClientDataExtract(ByVal GetClientDataExtractRequest As GetClientDataExtractRequestType) As GetClientDataExtractResponseType


    ''' <summary>
    ''' ''' Check Retained Insurer exists in list
    ''' </summary>
    ''' <param name="CheckRetainedCoInsurerExists"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="CheckRetainedCoInsurerExists")>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function CheckRetainedCoInsurerExists(ByVal oGetRetainedCoInsurerRequest As GetRetainedCoInsurerRequestType) As GetRetainedCoInsurerResponseType
End Interface
