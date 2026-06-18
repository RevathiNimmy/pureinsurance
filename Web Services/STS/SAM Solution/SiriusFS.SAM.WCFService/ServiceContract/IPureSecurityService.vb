Imports SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.WCF
' NOTE: You can use the "Rename" command on the context menu to change the interface name "IPureSecurityService" in both code and config file together.
'<ServiceContract(Name:="PureSecurityService", Namespace:="IPureSecurityService")> _
<ServiceContract(Namespace:="PureSecurityService")> _
Public Interface IPureSecurityService

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
    ''' Update user detail after validating it on client side. This will increase failure count, lock etc and will return user attributes in response
    ''' </summary>
    ''' <param name="UpdateUserDetailRequestType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()> _
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateUserDetail(ByVal UpdateUserDetailRequestType As UpdateUserDetailRequestType) As UpdateUserDetailResponseType

    ''' <summary>
    ''' Will update latest hashed password for an logged in user
    ''' </summary>
    ''' <param name="ChangePasswordRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ChangePassword(ByVal ChangePasswordRequest As ChangePasswordRequestType) As ChangePasswordResponseType

    ''' <summary>
    ''' TO get ForgottenPassword
    ''' </summary>
    ''' <param name="ForgottenPasswordRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ForgottenPassword(ByVal ForgottenPasswordRequest As ForgottenPasswordRequestType) As ForgottenPasswordResponseType

    ''' <summary>
    ''' TO Get Option Setting details provided in request
    ''' </summary>
    ''' <param name="GetOptionSettingRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetOptionSettingSecurity")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetOptionSetting(ByVal GetOptionSettingRequest As GetOptionSettingRequestType) As GetOptionSettingResponseType
    ''' <summary>
    ''' To return usergroups associated with user
    ''' </summary>
    ''' <param name="GetUserGroupsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetUserGroupsSecurity")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserGroups(ByVal GetUserGroupsRequest As GetUserGroupsRequestType) As GetUserGroupsResponseType

End Interface
