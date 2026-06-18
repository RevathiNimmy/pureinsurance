Imports System.ServiceModel
Imports System.Configuration.Provider
Imports System.Web
Imports System.Web.HttpContext
Imports System.Web.Configuration.WebConfigurationManager
Imports SiriusFS.SAM.Client.Security
Imports System.Xml.Serialization
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports System.Diagnostics
Imports System.Text
Imports System.Xml
Partial Public Class ProviderSAMForInsuranceV2

    ''' <summary>
    ''' This method will change the password for logged in user
    ''' </summary>
    ''' <param name="v_sNewPassword">New password with Old logic</param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function ChangePassword(ByVal v_sNewPassword As String, Optional ByVal v_sBranchCode As String = Nothing) As Boolean
        SyncLock oLock

            Dim oChangePasswordRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.ChangePasswordCommand
            Dim oChangePasswordResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.ChangePasswordCommandResponse
            Dim sbLogMessage As StringBuilder

            Try
                oChangePasswordRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.ChangePasswordCommand
                oChangePasswordResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.ChangePasswordCommandResponse
                sbLogMessage = New StringBuilder


                With oChangePasswordRequest
                    ' .WCFSecurityToken = SecurityToken()
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If String.IsNullOrEmpty(v_sNewPassword) Then
                        Throw New ArgumentNullException("v_sNewPassword")
                    Else
                        .NewPassword = v_sNewPassword
                    End If

                End With

                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Put(ApiMethods.ChangePassword, oChangePasswordRequest)
                    oChangePasswordResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.ChangePasswordCommandResponse)(result)
                End Using

                With oChangePasswordResponse
                    If 1 = 0 Then
                        'Process the error object if errors, and throw as a single exception
                        'Throw New NexusException(.Errors)
                        Return False
                    Else
                        Return True
                    End If

                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("ChangePassword executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)

                    sbLogMessage.AppendLine("v_sNewPassword = " & v_sNewPassword & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oChangePasswordRequest = Nothing
                oChangePasswordResponse = Nothing
            End Try


        End SyncLock

    End Function


    Public Overrides Sub ForgottenPassword(ByVal v_sUserName As String,
                                                Optional ByVal v_sBranchCode As String = Nothing)


        SyncLock oLock

            Dim oForgottenPasswordRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.ForgottenPasswordCommand
            Dim oForgottenPasswordResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.ForgottenPasswordCommandResponse
            Dim sbLogMessage As StringBuilder

            Try
                oForgottenPasswordRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.ForgottenPasswordCommand
                oForgottenPasswordResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.ForgottenPasswordCommandResponse
                sbLogMessage = New StringBuilder


                With oForgottenPasswordRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .Username = v_sUserName

                End With

                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Put(ApiMethods.ForgottenPassword, oForgottenPasswordRequest)
                    oForgottenPasswordResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.ForgottenPasswordCommandResponse)(result)
                End Using


                With oForgottenPasswordResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("ForgottenPassword executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_sUserName = " & v_sUserName.ToString & vbCrLf)


                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If
                    If Not IsNothing(v_sUserName) Then
                        sbLogMessage.AppendLine("v_sUserName = " & v_sUserName.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sUserName = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oForgottenPasswordRequest = Nothing
                oForgottenPasswordResponse = Nothing
            End Try


        End SyncLock

    End Sub

    ''' <summary>
    ''' This will return hashed password and password history for a valid user
    ''' </summary>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function ValidateUser(Optional ByVal v_sBranchCode As String = Nothing) As ValidateUserResponse
        SyncLock oLock

            Dim oRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.ValidateUserQuery
            Dim oResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.ValidateUserQueryResponse
            Dim oValidateUserResponse As New ValidateUserResponse
            Dim sbLogMessage As StringBuilder

            Try
                oRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.ValidateUserQuery
                oResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.ValidateUserQueryResponse
                sbLogMessage = New StringBuilder

                With oRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.ValidateUser, oRequest)
                    oResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.ValidateUserQueryResponse)(result)
                End Using

                With oResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If
                    oValidateUserResponse.HashedPassword = .PasswordHash
                    If .PasswordHistory IsNot Nothing Then
                        oValidateUserResponse.PasswordHistory = New List(Of String)
                        For iCt As Integer = 0 To .PasswordHistory.Count - 1
                            oValidateUserResponse.PasswordHistory.Add(.PasswordHistory(iCt))
                        Next
                    End If
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("ValidateUser executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oRequest = Nothing
                oResponse = Nothing
            End Try

            Return oValidateUserResponse

        End SyncLock

    End Function

End Class
