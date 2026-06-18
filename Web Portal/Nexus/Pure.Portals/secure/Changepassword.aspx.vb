Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Microsoft.SharePoint.Client
Imports System.Security.Cryptography
Imports System
Imports System.Web.Security


Namespace Nexus

    ' 26-11-07 - DH - Integrated with Nexus Provider

    ''' <summary>
    ''' Change password page
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class secure_Changepassword : Inherits Frontend.clsCMSPage

        Dim oWebService As NexusProvider.ProviderBase = Nothing

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not Page.IsPostBack Then
                If String.IsNullOrEmpty(Membership.PasswordStrengthRegularExpression) Then
                    vldPasswordRegex.Enabled = False
                Else
                    Dim oPasswordValidationMessage As NexusProvider.OptionTypeSetting = Nothing
                    Dim oOptionSettings As NexusProvider.OptionTypeSetting = Nothing
                    oWebService = New NexusProvider.ProviderManager().Provider
                    Dim sPasswordStrengthRegularExpression As String
                    Dim sPasswordValidationMessage As String

                    Try
                        oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5101)
                        sPasswordStrengthRegularExpression = oOptionSettings.OptionValue
                        oPasswordValidationMessage = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5113)
                        sPasswordValidationMessage = oPasswordValidationMessage.OptionValue
                    Catch
                        Throw New ArgumentNullException("Unable to retrieve password settings")
                    End Try

                    If Not String.IsNullOrEmpty(sPasswordStrengthRegularExpression) Then
                        vldPasswordRegex.ValidationExpression = sPasswordStrengthRegularExpression
                    Else
                        vldPasswordRegex.ValidationExpression = Membership.PasswordStrengthRegularExpression
                    End If

                    If Not String.IsNullOrEmpty(sPasswordValidationMessage) Then
                        vldPasswordRegex.ErrorMessage = sPasswordValidationMessage
                    Else
                        vldPasswordRegex.ErrorMessage = GetLocalResourceObject("msg_InvalidPasswordFormat")
                    End If

                    vldPasswordRegex.Enabled = True
                End If

                SetFocus(txtOldPassword)

            End If
        End Sub

        ''' <summary>
        ''' Change the users password, if an agent use the sam providers, otherwise use the .net membership
        ''' function to change the password.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

            If Page.IsValid Then
                oWebService = New NexusProvider.ProviderManager().Provider
                Try
                    'Check if existing password is correct
                    Dim bIsReusedPassword As Boolean = False
                    Dim oValidateUserResponse As NexusProvider.ValidateUserResponse
                    oValidateUserResponse = oWebService.ValidateUser(Session(CNBranchCode))
                    If oValidateUserResponse IsNot Nothing Then
                        'Check if existing password os correct
                        If CheckPassword(txtOldPassword.Text, oValidateUserResponse.HashedPassword) Then
                            'Check if password can be reused
                            If oValidateUserResponse.PasswordHistory IsNot Nothing Then
                                For iCt As Integer = 0 To oValidateUserResponse.PasswordHistory.Count - 1
                                    If CheckPassword(txtPassword.Text, oValidateUserResponse.PasswordHistory(iCt)) Then
                                        bIsReusedPassword = True
                                        Exit For
                                    End If
                                Next
                            End If

                            If bIsReusedPassword = False Then
                                'Dim sHashedPassword As String = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text, BCrypt.Net.BCrypt.GenerateSalt())
                                ''Encrypt New password with Old Encryption Logic
                                'Dim sEncryptedPassword As String = Utils.funcUtils.Encrypt(txtPassword.Text)
                                Dim sNewPassword As String = txtPassword.Text

                                oWebService.ChangePassword(sNewPassword, Session(CNBranchCode))
                                PnlConfirm.Visible = True
                                lblHeadingInfo.Visible = False
                                PnlPassword.Visible = False
                                SyncUser(Session(Nexus.Constants.CNLoginName).ToString(), txtPassword.Text)
                            Else
                                '"302" - "Not valid password for reuse"
                                vldPasswordChangeFailed.ErrorMessage = GetLocalResourceObject("302")
                                vldPasswordChangeFailed.IsValid = False
                            End If
                        Else
                            'sErrorCode = "291"-"Existing password is not correct";
                            vldPasswordChangeFailed.ErrorMessage = GetLocalResourceObject("291")
                            vldPasswordChangeFailed.IsValid = False
                        End If
                    Else
                        ' "290"-"User does not exist"
                        vldPasswordChangeFailed.ErrorMessage = GetLocalResourceObject("290")
                        vldPasswordChangeFailed.IsValid = False
                    End If

                Catch ex As NexusProvider.NexusException
                    Dim oError As NexusProvider.NexusError = CType(ex, NexusProvider.NexusException).Errors(0)
                    vldPasswordChangeFailed.ErrorMessage = GetLocalResourceObject(oError.Code)
                    vldPasswordChangeFailed.IsValid = False
                Catch ex As System.Exception
                    vldPasswordChangeFailed.IsValid = False
                End Try
            End If

        End Sub
        Public Function GetOVal(ByVal encryptedtext As String) As String
            Dim sRetVal As String = ""

            Dim TripleDes As New TripleDESCryptoServiceProvider
            Dim sKey As String = "!@$1R1U5"
            TripleDes.Key = TruncateHash(sKey, TripleDes.KeySize \ 8)
            TripleDes.IV = TruncateHash("", TripleDes.BlockSize \ 8)

            Try
                ' Convert the encrypted text string to a byte array. 
                Dim encryptedBytes() As Byte = Convert.FromBase64String(encryptedtext)

                ' Create the stream. 
                Dim ms As New System.IO.MemoryStream
                ' Create the decoder to write to the stream. 
                Dim decStream As New CryptoStream(ms,
                TripleDes.CreateDecryptor(),
                System.Security.Cryptography.CryptoStreamMode.Write)

                ' Use the crypto stream to write the byte array to the stream.
                decStream.Write(encryptedBytes, 0, encryptedBytes.Length)
                decStream.FlushFinalBlock()

                ' Convert the plaintext stream to a string. 
                sRetVal = System.Text.Encoding.Unicode.GetString(ms.ToArray)

                TripleDes = Nothing
            Catch ex As Exception
                Return ""
            End Try

            Return sRetVal
        End Function
        Private Function TruncateHash(ByVal key As String, ByVal length As Integer) As Byte()

            Dim sha1 As New SHA1CryptoServiceProvider

            ' Hash the key. 
            Dim keyBytes() As Byte =
            System.Text.Encoding.Unicode.GetBytes(key)
            Dim hash() As Byte = sha1.ComputeHash(keyBytes)

            ' Truncate or pad the hash. 
            ReDim Preserve hash(length - 1)
            Return hash
        End Function

        Private Sub SyncUser(ByVal userName As String, ByVal password As String)
            Dim oOptionSettings As NexusProvider.OptionTypeSetting = Nothing
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim keycloakModel As New Nexus.Library.KeycloakModel()
            Try
                oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5249)
                keycloakModel.EnableAuthentication = oOptionSettings.OptionValue
                oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5250)
                keycloakModel.Realm = oOptionSettings.OptionValue
                oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5251)
                keycloakModel.ClientID = oOptionSettings.OptionValue
                oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5252)
                keycloakModel.ClientSecret = oOptionSettings.OptionValue
                oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5253)
                keycloakModel.UserName = oOptionSettings.OptionValue
                oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5254)
                keycloakModel.Password = GetOVal(oOptionSettings.OptionValue)
                oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5255)
                keycloakModel.UserGroup = oOptionSettings.OptionValue
                oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5256)
                keycloakModel.TokenEndpoint = oOptionSettings.OptionValue
                keycloakModel.LoggedUserName = userName
                keycloakModel.LoggedPassword = password

            Catch
                Throw New ArgumentNullException("Unable to retrieve password settings")
            End Try

            If keycloakModel.EnableAuthentication = "1" Then
                Dim userDetails As NexusProvider.UserDetails
                userDetails = oWebService.GetUserDetails(userName)
                keycloakModel.FullUserName = userDetails.ResolvedName
                keycloakModel.EmailAddress = userDetails.EmailAddress
                Utils.FuncSecurity.SyncUser(keycloakModel)
            End If
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="password"></param>
        ''' <param name="hashedpassword"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckPassword(ByVal password As String, ByVal hashedpassword As String) As Boolean
            Try
                If BCrypt.Net.BCrypt.Verify(password, hashedpassword) = True Then
                    Return True
                Else
                    Return False
                End If
            Catch
                Return False
            End Try
        End Function

    End Class

End Namespace

