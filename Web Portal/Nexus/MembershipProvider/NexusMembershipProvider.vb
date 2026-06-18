Imports Microsoft.VisualBasic
Imports Nexus.Library
Imports Nexus.Library.Config
Imports NexusProvider.SAMForInsurance
Imports NexusProvider
Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports System.Web.Security
Imports System.Web.Profile
Imports System.Web
Imports Nexus
Imports CMS.Library.Portal
Imports System.Data.SqlClient
Imports System.Data
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Text.RegularExpressions

Public Class NexusMembershipProvider
    Inherits System.Web.Security.SqlMembershipProvider
    Private sApplicationName As String = Nothing
    Private bEnablePasswordReset As Boolean = False
    Private bEnablePasswordRetrieval As Boolean = False
    Private bRequiresQuestionAndAnswer As Boolean = False
    Private bRequiresUniqueEmail As Boolean = False
    Private sPasswordStrengthRegularExpression As String = String.Empty
    Private iMaxAllowedPasswordLength As Integer = 0
    Private iMaxInvalidPasswordAttempts As Integer = 0
    Private iMinRequiredNonAlphanumericCharacters As Integer = 0
    Private iMinRequiredPasswordLength As Integer = 0
    Private iPasswordAttemptWindow As Integer = 10
    Private ePasswordFormat As System.Web.Security.MembershipPasswordFormat = MembershipPasswordFormat.Clear

#Region "Initialize"

    Public Overrides Sub Initialize(ByVal name As String, ByVal config As System.Collections.Specialized.NameValueCollection)
        Dim strNewName As String

        If config Is Nothing Then _
           Throw New ArgumentNullException("MembershipProvider.NexusMembershipProvider: config is null.")

        If String.IsNullOrEmpty(name) Then
            strNewName = "NexusMembershipProvider"
        Else
            strNewName = name
        End If
        ' Set all MembershipProvider properties this way..... then call the base initialization
        sApplicationName = GetConfigValue(config("applicationName"), "/")
        bEnablePasswordReset = Convert.ToBoolean(GetConfigValue(config("enablePasswordReset"), "False"))
        bEnablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config("enablePasswordRetrieval"), "False"))
        bRequiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config("requiresQuestionAndAnswer"), "False"))
        bRequiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config("requiresUniqueEmail"), "False"))
        sPasswordStrengthRegularExpression = Convert.ToString(GetConfigValue(config("passwordStrengthRegularExpression"), ""))
        iMaxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config("maxInvalidPasswordAttempts"), "5"))
        iPasswordAttemptWindow = Convert.ToInt32(GetConfigValue(config("passwordAttemptWindow"), "10"))
        iMinRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config("minRequiredAlphaNumericCharacters"), "0"))
        iMinRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config("minRequiredPasswordLength"), "1"))
        iMaxAllowedPasswordLength = Convert.ToInt32(GetConfigValue(config("MaxAllowedPasswordLength"), "10"))
        Dim temp_format As String = config("passwordFormat")
        If String.IsNullOrEmpty(temp_format) Then
            temp_format = "clear"
        End If
        Select Case temp_format.ToLower
            Case "hashed"
                ePasswordFormat = MembershipPasswordFormat.Hashed
            Case "encrypted"
                ePasswordFormat = MembershipPasswordFormat.Encrypted
            Case "clear"
                ePasswordFormat = MembershipPasswordFormat.Clear
            Case Else
                Throw New Exception("Password format not supported.")
        End Select

        MyBase.Initialize(strNewName, config)

    End Sub
#End Region

#Region "Not Implemented"
    Public Overrides Function ChangePasswordQuestionAndAnswer(ByVal username As String, ByVal password As String, ByVal newPasswordQuestion As String, ByVal newPasswordAnswer As String) As Boolean
        Throw New Exception("not implemented")
    End Function



    Public Overrides Function FindUsersByEmail(ByVal emailToMatch As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef totalRecords As Integer) As System.Web.Security.MembershipUserCollection
        Throw New Exception("not implemented")
    End Function

    Public Overrides Function FindUsersByName(ByVal usernameToMatch As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef totalRecords As Integer) As System.Web.Security.MembershipUserCollection
        Throw New Exception("not implemented")
    End Function

    Public Overrides Function GetAllUsers(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef totalRecords As Integer) As System.Web.Security.MembershipUserCollection
        Throw New Exception("not implemented")
    End Function

    Public Overrides Function GetNumberOfUsersOnline() As Integer
        Throw New Exception("not implemented")
    End Function

    Public Overrides Function GetPassword(ByVal username As String, ByVal answer As String) As String
        Throw New Exception("not implemented")
    End Function

    Public Overrides Function UnlockUser(ByVal userName As String) As Boolean
        Throw New Exception("UnlockUser: This method is not implemented.")
    End Function

#End Region

#Region "properties"

    Public Overrides Property ApplicationName() As String
        Get
            Return sApplicationName
        End Get
        Set(ByVal value As String)
            sApplicationName = value
        End Set
    End Property
    Public Overrides ReadOnly Property EnablePasswordReset() As Boolean
        Get
            Return bEnablePasswordReset
        End Get
    End Property

    Public Overrides ReadOnly Property EnablePasswordRetrieval() As Boolean
        Get
            Return bEnablePasswordRetrieval
        End Get
    End Property

    Public Overrides ReadOnly Property MaxInvalidPasswordAttempts() As Integer
        Get
            Return iMaxInvalidPasswordAttempts
        End Get
    End Property

    Public Overrides ReadOnly Property MinRequiredNonAlphanumericCharacters() As Integer
        Get
            Return iMinRequiredNonAlphanumericCharacters
        End Get
    End Property

    Public Overrides ReadOnly Property MinRequiredPasswordLength() As Integer
        Get
            Return iMinRequiredPasswordLength
        End Get
    End Property

    Public Overrides ReadOnly Property PasswordAttemptWindow() As Integer
        Get
            Return iPasswordAttemptWindow
        End Get
    End Property

    Public Overrides ReadOnly Property PasswordFormat() As System.Web.Security.MembershipPasswordFormat
        Get
            Return ePasswordFormat
        End Get
    End Property

    Public Overrides ReadOnly Property PasswordStrengthRegularExpression() As String
        Get
            Return sPasswordStrengthRegularExpression
        End Get
    End Property

    Public Overrides ReadOnly Property RequiresQuestionAndAnswer() As Boolean
        Get
            Return bRequiresQuestionAndAnswer
        End Get
    End Property

    Public Overrides ReadOnly Property RequiresUniqueEmail() As Boolean
        Get
            Return bRequiresUniqueEmail
        End Get
    End Property
#End Region

#Region "Implemented methods"

    Public Overrides Function GetUser(ByVal providerUserKey As Object, ByVal userIsOnline As Boolean) As System.Web.Security.MembershipUser
        Return MyBase.GetUser(providerUserKey, userIsOnline)

    End Function

    Public Overrides Function GetUser(ByVal username As String, ByVal userIsOnline As Boolean) As System.Web.Security.MembershipUser
        Dim oMemUser As MembershipUser = MyBase.GetUser(username, userIsOnline)

        If oMemUser Is Nothing Then
            HttpContext.Current.Session.Add(CNIsAgent, LoginType.Agent)
            oMemUser = New MembershipUser("NexusMembershipProvider", username, username.GetHashCode, username, "", "", False, False, Today, Today, Today, Today, Today)
        Else
            If HttpContext.Current.Session(CNIsAgent) = LoginType.Agent Then
                HttpContext.Current.Session.Remove(CNIsAgent)
            End If
        End If

        Return oMemUser
    End Function

    Public Overrides Function GetUserNameByEmail(ByVal email As String) As String
        Return MyBase.GetUserNameByEmail(email)
    End Function

    Public Overrides Function ValidateUser(ByVal username As String, ByVal password As String) As Boolean
        Dim user As MembershipUser = Membership.GetUser(username, False)
        If Not user Is Nothing And MyBase.ValidateUser(username, password) Then
            Call ClientLogin(username)
            Return True
        End If

        Dim bAgentAuthenticate As Boolean = AgentLogin(username, password)
        Return bAgentAuthenticate
    End Function

    Public Overrides Function ResetPassword(ByVal username As String, ByVal answer As String) As String

        Dim sReturnPasswordStatus = String.Empty
        Dim sNewPassword As String = String.Empty
        Dim oMemUser As MembershipUser = Membership.GetUser(username, False)
        If HttpContext.Current.Session(CNIsAgent) = LoginType.Agent Then
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Try
                oWebService.ForgottenPassword(username)
            Catch ex As System.Exception
                sNewPassword = ex.Message
            Finally
                oWebService = Nothing
            End Try
        Else
            Try
                sNewPassword = Nexus.Utils.makePassword(7)
                Dim password As String
                password = MyBase.ResetPassword(username, "")
                MyBase.ChangePassword(username, password, sNewPassword)
            Catch ex As Exception
                sNewPassword = ex.Message
            End Try
        End If
        Return sNewPassword
    End Function

    Public Overrides Function ChangePassword(ByVal username As String, ByVal oldPassword As String, ByVal newPassword As String) As Boolean

        If CType(HttpContext.Current.Session.Item(CNLoginType), LoginType) = LoginType.Customer Then
            Dim oUser As MembershipUser = Membership.GetUser(HttpContext.Current.User.Identity.Name)
            If MyBase.ChangePassword(oUser.UserName, oldPassword, newPassword) Then
                Return True
            Else
                Return False
            End If
        Else
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Try
                oWebService.ChangePassword(oldPassword, newPassword)

                Return True
            Catch
                Return False
            Finally
                oWebService = Nothing
            End Try
        End If
    End Function

    Public Overrides Function CreateUser(ByVal username As String, ByVal password As String, ByVal email As String, ByVal passwordQuestion As String, ByVal passwordAnswer As String, ByVal isApproved As Boolean, ByVal providerUserKey As Object, ByRef status As System.Web.Security.MembershipCreateStatus) As System.Web.Security.MembershipUser
        Dim oUser As System.Web.Security.MembershipUser = MyBase.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, status)
        If oUser IsNot Nothing Then
            If CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AllowMultipleLogins = False Then
                InsertSessionIDforUser(oUser.UserName)
            Else
                System.Web.Security.FormsAuthentication.SetAuthCookie(username, False)
            End If
        End If
        Return oUser
    End Function

    Public Overrides Function DeleteUser(ByVal username As String, ByVal deleteAllRelatedData As Boolean) As Boolean
        MyBase.DeleteUser(username, deleteAllRelatedData)
    End Function


    Public Overrides Sub UpdateUser(ByVal user As System.Web.Security.MembershipUser)
        MyBase.UpdateUser(user)
    End Sub
#End Region

#Region "Private Methods"

    Private Function GetConfigValue(ByVal configValue As String, ByVal defaultValue As String) As String
        If String.IsNullOrEmpty(configValue) Then _
           Return defaultValue
        Return configValue
    End Function

    Private Function ClientLogin(ByVal username As String) As Boolean

        'Dim oProfile As CustomProfile = DirectCast(HttpContext.Current.Profile, CustomProfile)
        Dim oProfile As CustomProfile = CustomProfile.GetUserProfile(username)
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oParty As NexusProvider.BaseParty

        Dim oUserDetails As NexusProvider.UserDetails
        Dim oUserGroup, oUserGroupLoop As New NexusProvider.UserGroup
        Dim UserRoles As String

        Try
            oParty = oWebService.GetParty(oProfile.RegistrationDetails.PartyKey)
            HttpContext.Current.Session.Add(CNParty, oParty)
            HttpContext.Current.Session.Add(CNLoginName, username)
        Finally

            oParty = Nothing
        End Try

        If Not Roles.RoleExists("Customer") Then
            Roles.CreateRole("Customer")
        End If

        If Not Roles.IsUserInRole(username, "Customer") Then
            Roles.AddUserToRole(username, "Customer")
        End If

        oUserDetails = oWebService.GetUserDetails(username)
        'Membership.DeleteUser(username, True)

        oWebService = Nothing

        'check the Usergroups Agent belongs to and add all the roles he has been assigned
        If Not oUserDetails.AvailableUsergroups Is Nothing Then
            For Each oUserGroup In oUserDetails.AvailableUsergroups
                'check if role exists. if it doesn't then create it
                If Not Roles.RoleExists(oUserGroup.Code.Trim) Then
                    Roles.CreateRole(oUserGroup.Code)
                End If

                'check if user is already in role. if not then add them
                If Not Roles.IsUserInRole(username, oUserGroup.Code.Trim) Then
                    Roles.AddUserToRole(username, oUserGroup.Code.Trim)
                End If
            Next
        End If

        UserRoles = CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AllowRole

        If UserIsInRoles(UserRoles) = False Then
            Return False
        End If

        If CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AllowMultipleLogins = False Then
            InsertSessionIDforUser(username)
        Else
            System.Web.Security.FormsAuthentication.SetAuthCookie(username, False)
        End If
        HttpContext.Current.Session.Add(CNLoginType, LoginType.Customer)
        If String.IsNullOrEmpty(HttpContext.Current.Request.QueryString("ReturnUrl")) Then
            'No return url so go home
            Dim sClientStartPage As String = CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).ClientStartPage
            HttpContext.Current.Response.Redirect(sClientStartPage, True)
        Else
            FormsAuthentication.RedirectFromLoginPage(username, False)
        End If

        Return True

    End Function

    Private Function AgentLogin(ByVal username As String, ByVal password As String)

        'Login Agent to Nexus, login methods no longer exist in SAM, so authenticate credentials
        'by retrieving the user details with the credentials entered.

        HttpContext.Current.Session.Remove(CNAgentDetails)

        Dim oWebService As NexusProvider.ProviderBase
        Dim oUserDetails As NexusProvider.UserDetails
        Dim oUserGroup, oUserGroupLoop As New NexusProvider.UserGroup
        Dim UserRoles As String


        oWebService = New NexusProvider.ProviderManager().Provider



        'Retrieve the user details and authenticate
        Try
            oUserDetails = oWebService.GetUserDetails(username)
            If Not HttpContext.Current.Session.IsCookieless Then
                Membership.DeleteUser(username, True)
            End If
            If oUserDetails IsNot Nothing Then
                'oUserDetails.IsInvalidPassword = False
                oUserDetails.IsAuthenticated = True
                oUserDetails.IsWeakPassword = Not IsStrongPassword(password)
                oUserDetails.IsAuthenticated = True
                If oUserDetails.IsLocked Then
                    oUserDetails.IsAuthenticated = False
                ElseIf oUserDetails.IsTempPassword Then
                    Dim sTempPasswordExpiryDuration As String = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5109).OptionValue
                    Dim iTempPasswordExpiryDuration As Integer = 0
                    Dim dtTempPasswordExpiryDate As Nullable(Of DateTime)
                    If Not String.IsNullOrEmpty(sTempPasswordExpiryDuration) AndAlso oUserDetails.PasswordChange <> System.DateTime.MinValue Then
                        'If “Temporary Password Validity Duration (days)” is set as 0 then temporary password should never expired
                        iTempPasswordExpiryDuration = Convert.ToInt32(sTempPasswordExpiryDuration)
                        If iTempPasswordExpiryDuration > 0 Then
                            dtTempPasswordExpiryDate = oUserDetails.PasswordChange.AddDays(iTempPasswordExpiryDuration)
                            If dtTempPasswordExpiryDate.Value.Date <= DateTime.Now.Date Then
                                oUserDetails.IsAuthenticated = False
                                oUserDetails.IsLocked = True
                                Return False
                            End If
                        End If
                    End If
                End If
            End If

            Dim iPasswordExpiryDuration As Integer = 0
            Dim bForceToChangePassword As Boolean = False

            If oUserDetails.IsAuthenticated Then
                ' establishes a principal, set the session cookie and redirects
                ' you can also pass additional claims to signin, which will be embedded in the session token
                If oUserDetails.IsTempPassword OrElse oUserDetails.IsWeakPassword Then
                    bForceToChangePassword = True
                Else
                    'Get Password expiry days using system option 
                    Dim sPasswordExpiryDuration As String = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5103).OptionValue
                    iPasswordExpiryDuration = Integer.Parse(sPasswordExpiryDuration)

                    If Not String.IsNullOrEmpty(sPasswordExpiryDuration) AndAlso oUserDetails.PasswordChange <> Date.MinValue AndAlso iPasswordExpiryDuration > 0 Then

                        oUserDetails.PasswordExpiryDate = oUserDetails.PasswordChange.AddDays(iPasswordExpiryDuration)
                    End If

                    'Session("User") = objUserAttributes

                    If oUserDetails.PasswordExpiryDate.HasValue Then
                        'If CDate(oUserDetails.PasswordExpiryDate.Value.ToString("dd/MM/yyyy")) < CDate(DateTime.Now.ToString("dd/MM/yyyy")) Then
                        If oUserDetails.PasswordExpiryDate.Value.ToShortDateString() < DateTime.Now.ToShortDateString() Then
                            bForceToChangePassword = True
                        End If
                    End If
                End If



            End If

            'check the Usergroups Agent belongs to and add all the roles he has been assigned
            If Not oUserDetails.AvailableUsergroups Is Nothing Then
                For Each oUserGroup In oUserDetails.AvailableUsergroups
                    'check if role exists. if it doesn't then create it
                    If Not Roles.RoleExists(oUserGroup.Code.Trim) Then
                        Roles.CreateRole(oUserGroup.Code)
                    End If

                    'check if user is already in role. if not then add them
                    If Not Roles.IsUserInRole(username, oUserGroup.Code.Trim) Then
                        Roles.AddUserToRole(username, oUserGroup.Code.Trim)
                    End If
                Next
            End If

            UserRoles = CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AllowRole

            If UserIsInRoles(UserRoles, username) = False Then
                Return False
            End If


            HttpContext.Current.Session.Add(CNAgentDetails, oUserDetails)
            HttpContext.Current.Session.Add(CNLoginType, LoginType.Agent)
            HttpContext.Current.Session.Add(CNLoginName, Trim(username))
            HttpContext.Current.Session(CNClaimFlag) = "OFF"

            'Find that Enable Branch Selection At logon is enabled or not
            Dim bEnableBranchSelectionAtLogin As Boolean = False
            If CType(HttpContext.Current.Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches.Count > 1 Then
                Dim oOptionType As New NexusProvider.OptionTypeSetting
                oOptionType = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 37)
                If oOptionType.OptionValue = "1" Then
                    bEnableBranchSelectionAtLogin = True
                End If
            End If

            If oUserDetails.IsAuthenticated Then
                If bForceToChangePassword = True Then
                    HttpContext.Current.Response.Redirect("~/Changepassword.aspx")
                End If
            End If
            If String.IsNullOrEmpty(HttpContext.Current.Request.QueryString("ReturnUrl")) Then
                'No return url so go home

                Dim sAgentStartPage As String = CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AgentStartPage

                If CType(HttpContext.Current.Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches.Count > 1 And bEnableBranchSelectionAtLogin = True Then
                    'if an agent has more than 1 branches than go to select branch first
                    HttpContext.Current.Response.Redirect("~/SelectBranch.aspx")
                Else
                    'if only 1 branch than make it default selected and go to Agent Start page
                    ' Need to log In
                    If CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AllowMultipleLogins = False Then
                        InsertSessionIDforUser(username)
                    Else
                        FormsAuthentication.SetAuthCookie(username, False)
                    End If
                    HttpContext.Current.Session(CNBranchCode) = CType(HttpContext.Current.Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches(0).Code
                    HttpContext.Current.Response.Redirect(sAgentStartPage)
                End If

            Else
                If CType(HttpContext.Current.Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches.Count > 1 And bEnableBranchSelectionAtLogin = True Then
                    'if an agent has more than 1 branches than go to select branch first
                    HttpContext.Current.Response.Redirect("~/SelectBranch.aspx?ReturnUrl=" + HttpContext.Current.Request.QueryString("ReturnUrl").ToString())
                Else
                    HttpContext.Current.Session(CNBranchCode) = CType(HttpContext.Current.Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches(0).Code
                    FormsAuthentication.RedirectFromLoginPage(username, False)
                End If
            End If
            Return True
        Catch ex As System.Web.Services.Protocols.SoapHeaderException
            'Login failed with invalid credentials, this shouldn't be capturing any other
            'type of exception they should all be dealt with by the errorhandler module

            Return False
        Finally
            oWebService = Nothing
            oUserDetails = Nothing
        End Try

    End Function

    Sub InsertSessionIDforUser(ByVal userName As String)

        Dim authTicket As New FormsAuthenticationTicket(userName, False, 20)
        Dim encryptedTicket As String = FormsAuthentication.Encrypt(authTicket)
        Dim authCookie As New HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
        authCookie.SameSite = SameSiteMode.None
        authCookie.Secure = True
        authCookie.HttpOnly = True
        Dim command As New SqlCommand("usp_InsertSesionInfo")

        With command.Parameters
            .Add("@vSessionid", SqlDbType.VarChar, 400).Value = authCookie.Value
            .Add("@vUserName", SqlDbType.VarChar, 20).Value = userName
        End With

        funcDB.ExecNonQuery(command, "CMS")
        command.Dispose()
        HttpContext.Current.Response.Cookies.Add(authCookie)

    End Sub

    Private Function IsStrongPassword(ByVal sPassword As [String]) As [Boolean]

        Dim oWebService As NexusProvider.ProviderBase
        Dim sPasswordRegEx As String = String.Empty
        Try
            oWebService = New NexusProvider.ProviderManager().Provider

            sPasswordRegEx = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5101).OptionValue
        Catch ex As Exception
        Finally
            oWebService = Nothing

        End Try

        If Not String.IsNullOrEmpty(sPasswordRegEx) Then
            Dim regEx As New Regex(sPasswordRegEx)
            Return regEx.IsMatch(sPassword)
        Else
            If sPassword.Length >= 4 Then
                Return True
            Else
                Return False
            End If
        End If

    End Function
#End Region



End Class











