Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.WCF
Imports Internal = SiriusFS.SAM.Structure.BaseImplementationTypes
Imports Sirius.Architecture.ExceptionHandling
Imports Sirius.Architecture.ExceptionHandling.Handler
Imports SiriusFS.SAM.CoreImplementation
Imports Sirius.Architecture.Data
Imports Sirius.Architecture.Utility
Imports Sirius.Architecture.Configuration.Database
Imports System.Xml.Serialization
Imports System.Linq
Imports Sirius.Architecture.Security
Imports System.ServiceModel.Activation
Imports System.Web.Services.Protocols

Partial Public Class PureService

    ''' <summary>
    ''' Use to return hashed password and password history for an user
    ''' </summary>
    ''' <param name="oValidateUserRequestType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ValidateUser(ByVal oValidateUserRequestType As ValidateUserRequestType) As ValidateUserResponseType Implements IPureSecurityService.ValidateUser

        Dim oValidateUserResponseType As New ValidateUserResponseType

        Try
            Dim sUserName As String = oValidateUserRequestType.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGUsrDet", iUserId)
            CommonFunctions.CheckSTSSecurityToken(oValidateUserRequestType.WCFSecurityToken)
            ' Implementation structures
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness()
            Dim oImpRequest As New BaseImplementationTypes.ValidateUserRequestType
            Dim oImpResponse As New BaseImplementationTypes.ValidateUserResponseType

            Try

                ' Call the implementation method
                oImpRequest.UserName = sUserName


                oImpResponse = DirectCast(oBusiness.ValidateUser(oImpRequest), Internal.ValidateUserResponseType)

                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                oValidateUserResponseType.UserId = oImpResponse.UserId
                oValidateUserResponseType.PasswordHash = oImpResponse.PasswordHash
                oValidateUserResponseType.SystemUpgradeChangePasswordRequired = oImpResponse.SystemUpgradeChangePasswordRequired

                If oImpResponse.PasswordHistory IsNot Nothing Then
                    oValidateUserResponseType.PasswordHistory = New List(Of String)
                    For iCt As Integer = 0 To oImpResponse.PasswordHistory.Count - 1
                        oValidateUserResponseType.PasswordHistory.Add(oImpResponse.PasswordHistory(iCt).ToString())
                    Next
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oValidateUserResponseType, ex, CommonFunctions.CreateDictionary(oValidateUserRequestType))
            End Try

            Return oValidateUserResponseType

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oValidateUserRequestType))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' Update user detail after validating it on client side. This will increase failure count, lock etc and will return user attributes in response
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateUserDetail(ByVal oRequest As UpdateUserDetailRequestType) As UpdateUserDetailResponseType Implements IPureSecurityService.UpdateUserDetail

        Try

            Dim oResponse As New UpdateUserDetailResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness()

            ' Implementation structures
            Dim oImpResponse As BaseImplementationTypes.UpdateUserDetailResponseType = Nothing

            Try
                Dim sUserName As String = oRequest.LoginUserName
                Dim iAgentKey As Integer
                Dim iUserId As Integer

                CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
                ' Call the implementation method
                Dim oImpRequest As New BaseImplementationTypes.UpdateUserDetailRequestType
                oImpRequest.UserName = oRequest.LoginUserName
                oImpRequest.BranchCode = oRequest.BranchCode
                oImpRequest.IsAuthenticated = oRequest.IsAuthenticated
                oImpRequest.UserId = iUserId

                oImpResponse = oBusiness.UpdateUserDetail(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.FullName = oImpResponse.FullName
                oResponse.EmailAddress = oImpResponse.EmailAddress
                oResponse.IsLocked = oImpResponse.IsLocked
                oResponse.IsTempPassword = oImpResponse.IsTempPassword
                oResponse.PasswordChangeDate = oImpResponse.PasswordChangeDate

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oRequest))
            Return Nothing
        End Try

    End Function

    Public Function ChangePassword(ByVal ChangePasswordRequest As ChangePasswordRequestType) As ChangePasswordResponseType Implements IPureSecurityService.ChangePassword

        Try

            Dim sUserName As String = ChangePasswordRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMChgPass", iUserId)
            CommonFunctions.CheckSTSSecurityToken(ChangePasswordRequest.WCFSecurityToken)
            Dim oResponse As New ChangePasswordResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, ChangePasswordRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New BaseImplementationTypes.BaseChangePasswordRequestType
            Dim oImpResponse As BaseImplementationTypes.BaseChangePasswordResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = ChangePasswordRequest.BranchCode
            oImpRequest.NewPasswordHashed = ChangePasswordRequest.NewPasswordHashed
            oImpRequest.NewEncryptedPassword = ChangePasswordRequest.NewEncryptedPassword
            oImpRequest.UserName = ChangePasswordRequest.LoginUserName

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.ChangePassword(oImpRequest)
                 oResponse.EmailAddress = oImpResponse.EmailAddress
                oResponse.FullUserName = oImpResponse.FullUserName
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(ChangePasswordRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(ChangePasswordRequest))
            Return Nothing
        End Try

    End Function

    Public Function ForgottenPassword(ByVal ForgottenPasswordRequest As ForgottenPasswordRequestType) As ForgottenPasswordResponseType Implements IPureSecurityService.ForgottenPassword

        Try
            Dim sUserName As String = ForgottenPasswordRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFrgtPwd", iUserId)
            CommonFunctions.CheckSTSSecurityToken(ForgottenPasswordRequest.WCFSecurityToken)
            Dim oResponse As New ForgottenPasswordResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, ForgottenPasswordRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.ForgottenPasswordRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.ForgottenPasswordResponseType = Nothing

            ' Pass the values to the implementation request structure

            oImpRequest.UserName = ForgottenPasswordRequest.Username
            oImpRequest.BranchCode = ForgottenPasswordRequest.BranchCode

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.ForgottenPassword(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(ForgottenPasswordRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(ForgottenPasswordRequest))
            Return Nothing
        End Try

    End Function

End Class
