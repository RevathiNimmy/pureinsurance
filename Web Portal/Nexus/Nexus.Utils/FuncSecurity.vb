Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.Configuration
Imports Nexus.Library
Imports System.Web.Security
Imports CMS.Library


Public Module FuncSecurity
    Public Function UserIsInRoles(ByVal sRoles As String, Optional ByVal sUserName As String = "") As Boolean
        Dim RolesAssign() As String
        RolesAssign = sRoles.Split(",")

        If IsWindowsAuthentication() Then
            'need to set the username as this is not picked up automatically
            sUserName = HttpContext.Current.User.Identity.Name
        End If

        If RolesAssign.Length = 0 Then
            Return False
        Else
            For Each sRoleLoop As String In RolesAssign
                If sUserName = "" Then
                    If Roles.IsUserInRole(sRoleLoop) Or sRoleLoop = "*" Then
                        Return True
                        Exit For
                    End If
                Else
                    If Roles.IsUserInRole(sUserName, sRoleLoop) Or sRoleLoop = "*" Then
                        Return True
                        Exit For
                    End If
                End If
            Next
        End If
        Return False
    End Function

    Public Function SyncUser(ByRef keyCloakModel As KeycloakModel) As String
        Dim keyCloakConfigSettings As New KeyCloakConfiguration()
        keyCloakConfigSettings.Realm = keyCloakModel.Realm
        keyCloakConfigSettings.client_id = keyCloakModel.ClientID
        keyCloakConfigSettings.client_secret = keyCloakModel.ClientSecret
        keyCloakConfigSettings.username = keyCloakModel.UserName
        keyCloakConfigSettings.Password = keyCloakModel.Password
        keyCloakConfigSettings.AdminGroupName = keyCloakModel.UserGroup
        keyCloakConfigSettings.TokenEndpoint = keyCloakModel.TokenEndpoint

        keyCloakConfigSettings.grant_type = "password"

        If Not IsNothing(keyCloakConfigSettings) Then
            Dim adminGroup As String = ""
            adminGroup = keyCloakConfigSettings.AdminGroupName
            Dim usersSync As New SSP.Pure.UsersSync.Services.AuthenticationService(keyCloakConfigSettings)
            Dim firstName As String
            Dim lastName As String
            If String.IsNullOrEmpty(keyCloakModel.FullUserName) Then
                firstName = keyCloakModel.LoggedUserName
                lastName = keyCloakModel.LoggedUserName
            Else
                Dim str() = keyCloakModel.FullUserName.Split(" ")

                If str.Length > 0 Then

                    If str.Length = 1 Then
                        firstName = str(0)
                        lastName = keyCloakModel.LoggedUserName
                    Else
                        firstName = str(0)
                        lastName = str(str.Length - 1)
                    End If

                Else
                    firstName = keyCloakModel.FullUserName
                    lastName = keyCloakModel.LoggedUserName
                End If
            End If

            Dim user = New SSP.Pure.UsersSync.Contracts.UserRegisterRequestDTO(keyCloakModel.LoggedUserName, keyCloakModel.EmailAddress, keyCloakModel.LoggedPassword, adminGroup, "0", firstName, lastName, 0)
            Try
                Dim id = usersSync.GetUserAsync(keyCloakModel.LoggedUserName).Result
                If String.IsNullOrEmpty(id) = True Then
                    Dim response = usersSync.RegisterUserAsync(user)
                    If (response IsNot Nothing AndAlso response.Result IsNot Nothing) Then
                        If (response.Result.User IsNot Nothing AndAlso response.Result.User.Error IsNot Nothing AndAlso String.IsNullOrEmpty(response.Result.User.Error.ErrorResponseCode) = False) Then
                            Return "Cannot Add User Details To KeyCloak Server." & response.Result.User.Error.ErrorResponseCode & ":" & response.Result.User.Error.ErrorDetails
                        ElseIf (response.Result.Error IsNot Nothing) Then
                            Return "Cannot Add User Details To KeyCloak Server." & response.Result.User.Error.ErrorResponseCode & ":" & response.Result.User.Error.ErrorDetails
                        End If
                    End If
                Else
                    user = New SSP.Pure.UsersSync.Contracts.UserRegisterRequestDTO(keyCloakModel.LoggedUserName, keyCloakModel.EmailAddress, keyCloakModel.LoggedPassword, adminGroup, id, firstName, lastName, 0)
                    Dim response = usersSync.UpdateUserAsync(user)
                    If (response IsNot Nothing AndAlso response.Result IsNot Nothing) Then
                        If (response.Result.User IsNot Nothing AndAlso response.Result.User.Error IsNot Nothing AndAlso String.IsNullOrEmpty(response.Result.User.Error.ErrorResponseCode) = False) Then
                            Return "Cannot Update User Details To KeyCloak Server." & response.Result.User.Error.ErrorResponseCode & ":" & response.Result.User.Error.ErrorDetails
                        ElseIf (response.Result.Error IsNot Nothing) Then
                            Return "Cannot Update User Details To KeyCloak Server." & response.Result.User.Error.ErrorResponseCode & ":" & response.Result.User.Error.ErrorDetails
                        End If
                    End If
                End If
            Catch
                Dim response = usersSync.RegisterUserAsync(user)
                If (response IsNot Nothing AndAlso response.Result IsNot Nothing) Then
                    If (response.Result.User IsNot Nothing AndAlso response.Result.User.Error IsNot Nothing AndAlso String.IsNullOrEmpty(response.Result.User.Error.ErrorResponseCode) = False) Then
                        Return "Cannot Add User Details To KeyCloak Server." & response.Result.User.Error.ErrorResponseCode & ":" & response.Result.User.Error.ErrorDetails
                    ElseIf (response.Result.Error IsNot Nothing) Then
                        Return "Cannot Add User Details To KeyCloak Server." & response.Result.User.Error.ErrorResponseCode & ":" & response.Result.User.Error.ErrorDetails
                    End If
                End If
            End Try
        End If
        Return String.Empty
    End Function


    ''Function to check if user can perform as task as specified in config

    'Public Function UserCanDoTask(ByVal Task As String) As Boolean
    '    If CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Tasks.Task(Task) IsNot Nothing Then
    '        'get the list of roles that can do this task from config
    '        Dim sRoles As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Tasks.Task(Task).Role
    '        'call UserIsInRoles to see if the current user can carry out this ta
    '        Return UserIsInRoles(sRoles)

    '    Else
    '        Return False
    '    End If

    'End Function

    ''' <summary>
    ''' Method checks if impersonation is enabled in web.config
    ''' indicating that windows authentication is enabled in web.config
    ''' </summary>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Function IsWindowsAuthentication() As Boolean
        
        Dim oAnonymousIdentificationSection As AnonymousIdentificationSection = _
                CType(System.Web.Configuration.WebConfigurationManager.GetSection("system.web/anonymousIdentification"), AnonymousIdentificationSection)
        Return Not oAnonymousIdentificationSection.Enabled

    End Function
End Module
