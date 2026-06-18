using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility.VB6;
using PureSecurityService;
using Ssp.PureSts.DataAccess.Interface;
using Ssp.PureSts.Models;
using SSP.Pure.UsersSync.Contracts;
using SSP.Pure.UsersSync.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Configuration;

namespace Ssp.PureSts.DataAccess.ADO
{
    public class UserService : IUserService
    {

        private TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
        private MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
        public const string PMEncryptionEntropy = "SiriusArchitecture";

        #region Public functions

        /// <summary>
        /// To validate an user and updating required attributes in database
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserAttributes ValidateUser(string userName, string password)
        {
            UserAttributes oUserAttributes = null;
            PureSecurityServiceClient oSecurityService = null;
            ValidateUserResponseType oValidateUserResponse = null;
            UpdateUserDetailResponseType oUpdateUserDetailResponse = null;
            UpdateUserDetailRequestType oUpdateUserDetailRequest = null;
            ValidateUserRequestType oValidateUserRequest = null;

            try
            {
                String sDefaultBranchCode = WebConfigurationManager.AppSettings["DefaultBranchCode"];
                oValidateUserRequest = new ValidateUserRequestType();
                oUserAttributes = new UserAttributes();
                oSecurityService = new PureSecurityServiceClient();
                oUpdateUserDetailRequest = new UpdateUserDetailRequestType();

                oValidateUserRequest.LoginUserName = userName;
                oValidateUserRequest.BranchCode = sDefaultBranchCode;
                oValidateUserRequest.WCFSecurityToken = SecurityToken();

                oValidateUserResponse = oSecurityService.ValidateUser(oValidateUserRequest);

                oUserAttributes.IsAuthenticated = false;
                oUserAttributes.IsInvalidPassword = true;
                oUpdateUserDetailRequest.IsAuthenticated = false;
                oUserAttributes.IsSystemUpgradeChangePasswordRequired = false;


                if (oValidateUserResponse != null)
                {

                    oUpdateUserDetailRequest.LoginUserName = userName;
                    oUpdateUserDetailRequest.BranchCode = sDefaultBranchCode;

                    //Check if password is correct
                    if (CheckPassword(password, oValidateUserResponse.PasswordHash, oValidateUserResponse.SystemUpgradeChangePasswordRequired))
                    {
                        oUserAttributes.IsInvalidPassword = false;
                        oUserAttributes.IsAuthenticated = true;
                        oUserAttributes.IsWeakPassword = !IsStrongPassword(userName, password);
                        oUpdateUserDetailRequest.IsAuthenticated = true;
                        oUserAttributes.IsSystemUpgradeChangePasswordRequired = oValidateUserResponse.SystemUpgradeChangePasswordRequired;
                    }
                    //Update failure count etc and get other attributes
                    oUpdateUserDetailResponse = oSecurityService.UpdateUserDetail(oUpdateUserDetailRequest);
                }


                if (oUpdateUserDetailResponse != null)
                {
                    oUserAttributes.UserName = userName;
                    oUserAttributes.IsLocked = oUpdateUserDetailResponse.IsLocked;
                    if (oUpdateUserDetailResponse.IsLocked)
                    {
                        oUserAttributes.IsAuthenticated = false;
                    }
                    else if (oUpdateUserDetailResponse.IsTempPassword)
                    {
                        string sTempPasswordExpiryDuration = GetOptionSetting(userName, 5109);
                        int iTempPasswordExpiryDuration = 0;
                        Nullable<DateTime> dtTempPasswordExpiryDate;
                        if (!string.IsNullOrEmpty(sTempPasswordExpiryDuration) && oUpdateUserDetailResponse.PasswordChangeDate.HasValue)
                        {
                            iTempPasswordExpiryDuration = int.Parse(sTempPasswordExpiryDuration);
                            //If “Temporary Password Validity Duration (days)” is set as 0 then temporary password should never expired
                            if (iTempPasswordExpiryDuration > 0)
                            {
                                dtTempPasswordExpiryDate = oUpdateUserDetailResponse.PasswordChangeDate.Value.AddDays(iTempPasswordExpiryDuration);

                                if (dtTempPasswordExpiryDate <= DateTime.Now)
                                {
                                    oUserAttributes.IsAuthenticated = false;
                                    oUserAttributes.IsLocked = true;
                                }
                            }
                        }

                    }
                    oUserAttributes.IsTempPassword = oUpdateUserDetailResponse.IsTempPassword;
                    oUserAttributes.PasswordChangeDate = oUpdateUserDetailResponse.PasswordChangeDate;
                }

                if (!oUserAttributes.IsAuthenticated)
                {
                    oUserAttributes.ErrorMessage = "User does not exist";
                }

            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    oUserAttributes.IsAuthenticated = false;
                    oUserAttributes.ErrorMessage = ex.Message;
                }
                else
                {
                    // log the system error 
                    throw ex;
                }
            }
            finally
            {
                oSecurityService.Close();
                oValidateUserRequest = null;
                oValidateUserResponse = null;
                oUpdateUserDetailRequest = null;
                oUpdateUserDetailResponse = null;
            }

            return oUserAttributes;
        }



        /// <summary>
        /// For changing the current password for logged in user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public bool ChangePassword(string userName, string oldPassword, string newPassword, ref string sErrorCode, ref string sErrorMessage)
        {
            PureSecurityServiceClient oSecurityService = null;

            ValidateUserResponseType oValidateUserResponse = null;
            ValidateUserRequestType oValidateUserRequest = null;
            ChangePasswordResponseType oChangePasswosResponse = null;
            ChangePasswordRequestType oChangePasswordRequest = null;

            try
            {
                oSecurityService = new PureSecurityServiceClient();
                oValidateUserRequest = new ValidateUserRequestType();

                String sDefaultBranchCode = WebConfigurationManager.AppSettings["DefaultBranchCode"];

                oValidateUserRequest.LoginUserName = userName;
                oValidateUserRequest.BranchCode = sDefaultBranchCode;
                oValidateUserRequest.WCFSecurityToken = SecurityToken();
                oValidateUserResponse = oSecurityService.ValidateUser(oValidateUserRequest);
                if (oValidateUserResponse != null)
                {
                    //Check existing password
                    if (CheckPassword(oldPassword, oValidateUserResponse.PasswordHash, oValidateUserResponse.SystemUpgradeChangePasswordRequired)) //Password is correct
                    {
                        //Check passord reuse logic
                        Boolean bIsValidPasswordToUse = true;
                        if (oValidateUserResponse.PasswordHistory != null)
                        {
                            for (int iCt = 0; iCt <= oValidateUserResponse.PasswordHistory.Count - 1; iCt++)
                            {
                                if (CheckPassword(newPassword, oValidateUserResponse.PasswordHistory[iCt], false))
                                {
                                    bIsValidPasswordToUse = false;
                                    break;
                                }
                            }
                        }
                        // check new password should not be temp password
                        if (CheckPassword(newPassword, oValidateUserResponse.PasswordHash, false))
                        {
                            bIsValidPasswordToUse = false;
                        }

                        if (bIsValidPasswordToUse == true)
                        {
                            string sSiriusEncrypt = string.Empty;
                            //change password
                            oChangePasswordRequest = new ChangePasswordRequestType();

                            oChangePasswordRequest.LoginUserName = userName;
                            oChangePasswordRequest.WCFSecurityToken = SecurityToken();
                            String sSalt = BCrypt.Net.BCrypt.GenerateSalt();
                            oChangePasswordRequest.NewPasswordHashed = BCrypt.Net.BCrypt.HashPassword(newPassword, sSalt);
                            oChangePasswordRequest.BranchCode = sDefaultBranchCode;
                            oChangePasswordRequest.NewEncryptedPassword = Encrypt(newPassword);
                            oChangePasswosResponse = oSecurityService.ChangePassword(oChangePasswordRequest); 
                            string emailAddress= oChangePasswosResponse.EmailAddress;
                            string fullUserName = oChangePasswosResponse.FullUserName;
                            if (oChangePasswosResponse.Errors != null)
                            {
                                sErrorCode = ((PureSecurityService.SAMErrorBusinessRule)((PureSecurityService.SAMMethodResponseData)(oChangePasswosResponse)).Errors[0]).Code.ToString();
                                sErrorMessage = ((PureSecurityService.SAMErrorBusinessRule)((PureSecurityService.SAMMethodResponseData)(oChangePasswosResponse)).Errors[0]).Description;
                                return false;
                            }
                            else
                            {
                                try
                                {
                                    UserModel userModel = new UserModel();
                                    userModel.EmailAddress = emailAddress;
                                    userModel.FullUserName = fullUserName;
                                    userModel.Password = newPassword;
                                    userModel.UserName = userName;
                                    SyncUser(userModel);
                                }
                                catch
                                {
                                    throw;
                                }
                                return true;
                            }
                        }
                        else
                        {
                            sErrorCode = "302";
                            sErrorMessage = "Not valid password for reuse";
                            return false;
                        }
                    }
                    else
                    {
                        sErrorCode = "291";
                        sErrorMessage = "Existing password is not correct";
                        return false;
                    }
                }
                else
                {
                    sErrorCode = "290";
                    sErrorMessage = "User does not exist";
                    return false;
                }

            }
            catch (Exception ex)
            {
                //logging will be done at interface layer
                throw ex;
            }
            finally
            {
                oSecurityService.Close();
                oValidateUserResponse = null;
                oValidateUserRequest = null;
                oChangePasswosResponse = null;
                oChangePasswordRequest = null;
            }
        }

        /// <summary>
        /// To check if hashed password is valid for given plain text password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hashedpassword"></param>
        /// <returns></returns>
        public bool CheckPassword(string password, string hashedpassword, bool bSystemUpgradeChangePasswordRequired)
        {
            string sSiriusEncrypt = string.Empty;

            if (bSystemUpgradeChangePasswordRequired)
            {
                SiriusEncrypt(ref password, ref sSiriusEncrypt);
                if (sSiriusEncrypt == hashedpassword)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    if (BCrypt.Net.BCrypt.Verify(password, hashedpassword) == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// To get value for any system option
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="optionNumber"></param>
        /// <returns></returns>
        public string GetOptionSetting(string userName, int optionNumber)
        {
            PureSecurityServiceClient oSecurityService = null;
            GetOptionSettingResponseType oResponse = null;
            GetOptionSettingRequestType oRequest = null;

            try
            {
                oSecurityService = new PureSecurityServiceClient();
                oRequest = new GetOptionSettingRequestType();

                oRequest.LoginUserName = userName;
                oRequest.OptionNumber = optionNumber;
                oRequest.OptionType = OptionType.SystemOption;
                oRequest.BranchCode = WebConfigurationManager.AppSettings["DefaultBranchCode"];
                //oRequest.WCFSecurityToken = SecurityToken();
                oResponse = oSecurityService.GetOptionSettingSecurity(oRequest);

                if (oResponse.Errors == null)
                {
                    return oResponse.OptionValue;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                //logging will be done at interface layer
                throw ex;
            }
            finally
            {
                oSecurityService.Close();
                oSecurityService = null;
                oResponse = null;
                oRequest = null;
            }
        }

        /// <summary>
        /// To get Task groups assigned to user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<UserRole> GetUserRoles(string userName)
        {
            List<UserRole> roles = null;
            PureSecurityServiceClient oSecurityService = null;
            GetUserGroupsResponseType oGetUserGroupsResponse = null;
            GetUserGroupsRequestType oGetUserGroupsRequest = null;

            try
            {
                roles = new List<UserRole>();
                oSecurityService = new PureSecurityServiceClient(); ;
                oGetUserGroupsRequest = new GetUserGroupsRequestType();
                oGetUserGroupsResponse = new GetUserGroupsResponseType();

                oGetUserGroupsRequest.LoginUserName = userName;
                oGetUserGroupsRequest.BranchCode = WebConfigurationManager.AppSettings["DefaultBranchCode"];
                oGetUserGroupsRequest.ShowForCurrentUserOnly = true;
                // oGetUserGroupsRequest.WCFSecurityToken = SecurityToken();
                oGetUserGroupsResponse = oSecurityService.GetUserGroupsSecurity(oGetUserGroupsRequest);

                if (oGetUserGroupsResponse != null)
                {
                    UserRole oUserGroupDetail = null;
                    foreach (BaseGetUserGroupsResponseTypeRow oUserGroup in oGetUserGroupsResponse.UserGroups)
                    {
                        oUserGroupDetail = new UserRole();
                        oUserGroupDetail.Code = oUserGroup.Code;
                        oUserGroupDetail.Description = oUserGroup.Description;
                        oUserGroupDetail.IsSystemAdmin = oUserGroup.IsSystemAdmin;
                        roles.Add(oUserGroupDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                //logging will be done at interface layer
                throw ex;
            }
            finally
            {
                oSecurityService.Close();
                oSecurityService = null;
                oGetUserGroupsResponse = null;
                oGetUserGroupsRequest = null;
            }

            return roles;
        }

        #endregion

        #region Private functions

        #region Private Security Token Function
        /// <summary>
        /// It will read WCFSecurityToken from config file then return encrypted token as string
        /// </summary>
        /// <returns></returns>

        private string SecurityToken()
        {
            string sSecurityToken = string.Empty;
            // System.Web.Caching.Cache oCache = System.Web.HttpContext.Current.Cache;
            if (WebConfigurationManager.AppSettings["WCFSecurityToken"] != null)
            {
                sSecurityToken = WebConfigurationManager.AppSettings["WCFSecurityToken"].ToString();
                if (sSecurityToken.Length > 0)
                {
                    return BCrypt.Net.BCrypt.HashPassword(sSecurityToken, 5);
                }
            }
            return string.Empty;
        }
        #endregion 
        /// <summary>
        /// To check if password is strong or not
        /// </summary>
        /// <param name="sUserName"></param>
        /// <param name="sPassword"></param>
        /// <returns></returns>
        private Boolean IsStrongPassword(String sUserName, String sPassword)
        {
            String sPasswordRegEx = GetOptionSetting(sUserName, 5101);

            if (!String.IsNullOrEmpty(sPasswordRegEx))
            {
                Regex regEx = new Regex(sPasswordRegEx);
                return regEx.IsMatch(sPassword);
            }
            else
            {
                if (sPassword.Length >= 4)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        #endregion

        public bool SiriusEncrypt(ref string sLicence, ref string sLicenceKey)
        {
            bool bResult = false;
            string sAString = "";
            StringBuilder sBString = new StringBuilder();
            int iCntr = 0;
            FixedLengthString sChar1 = new FixedLengthString(1);
            FixedLengthString sChar2 = new FixedLengthString(1);
            int iSn = 0;
            string sCodeString = "";
            int iClen = 0;

            try
            {
                bResult = true;

                // Encrypts the supplied string returning the encrypted
                // result. Encrypted string will always be 2 characters
                // longer than original (leave space!)
                //
                // Encrypted string contains only ASCII characters in
                // range 32-126

                sCodeString = "aPCXADneGgH7khIJpjKtBMzmQLrRcqSEsbUv6yuVFW9xYZ2T3fd4w5N8";
                iClen = sCodeString.Length;

                sAString = sLicence;
                iCntr = sAString.Length;

                if (iCntr < 1)
                {
                    bResult = false;

                    sLicenceKey = "";

                    return bResult;
                }


                sChar1.Value = sCodeString.Substring((Strings.Asc(sAString.Substring(0, 1)[0]) + iCntr) % iClen, 1);
                sChar2.Value = sCodeString.Substring(Strings.Asc(sAString.Substring(sAString.Length - 1)[0]) % iClen, 1);
                iSn = ((Strings.Asc(sChar1.Value[0]) + Strings.Asc(sChar2.Value[0])) % iClen) + 1;
                sBString = new StringBuilder(sChar2.Value);

                for (int iCntr2 = 1; iCntr2 <= iCntr; iCntr2++)
                {
                    sBString.Append(sCodeString.Substring((Strings.Asc(sAString.Substring(iCntr2 - 1, 1)[0]) + iSn + iCntr2) % iClen, 1));
                }

                sBString.Append(sChar1.Value);

                // Return the result.
                sLicenceKey = sBString.ToString().Trim();

                return bResult;


            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public string Encrypt(string sPassword)
        {
            string sEncryptedPassword = string.Empty;
            System.Text.StringBuilder sEncryptBuilder = new System.Text.StringBuilder();
            int nCntr = 0;
            string sChar1 = "\0";
            string sChar2 = "\0";
            int nSumAsc = 0;
            string sCodeString = "";
            int nClen = 0;

            try
            {
                // Encrypts the supplied string returning the encrypted
                // result. Encrypted string will always be 2 characters
                // longer than original (leave space!)
                //
                // Encrypted string contains only ASCII characters in
                // range 32-126

                sCodeString = "aPCXADneGgH7khIJpjKtBMzmQLrRcqSEsbUv6yuVFW9xYZ2T3fd4w5N8";
                nClen = sCodeString.Length;

                nCntr = sPassword.Length;

                if (nCntr < 1)
                {
                    sEncryptedPassword = "";
                    return sEncryptedPassword;
                }

                sChar1 = sCodeString.Substring((Strings.Asc(sPassword.Substring(0, 1)[0]) + nCntr) % nClen, 1);
                sChar2 = sCodeString.Substring(Strings.Asc(sPassword.Substring(sPassword.Length - 1)[0]) % nClen, 1);
                nSumAsc = ((Strings.Asc(sChar1) + Strings.Asc(sChar2)) % nClen) + 1;
                sEncryptBuilder = new System.Text.StringBuilder(sChar2);

                for (int iCntr2 = 1; iCntr2 <= nCntr; iCntr2++)
                {
                    sEncryptBuilder.Append(sCodeString.Substring((Strings.Asc(sPassword.Substring(iCntr2 - 1, 1)[0]) + nSumAsc + iCntr2) % nClen, 1));
                }

                sEncryptBuilder.Append(sChar1);

                // Return the result.
                sEncryptedPassword = sEncryptBuilder.ToString().Trim();
                return sEncryptedPassword;

            }
            catch (System.Exception ex)
            {
                sEncryptedPassword = "";
                return sEncryptedPassword;
            }
        }
        private void SyncUser(UserModel userDetails)
        {
            string userName = userDetails.UserName;
            KeyCloakConfiguration keyCloakConfiguration = new KeyCloakConfiguration();
            string authenticationIntegrationValue = GetOptionSetting(userName, 5249);
            string enableAuthenticationIntegration = (authenticationIntegrationValue == string.Empty || authenticationIntegrationValue == "0") ? "0" : "1";
            bool IsEnableAuthenticationIntegration = enableAuthenticationIntegration == "1" ? true : false;
            keyCloakConfiguration.Realm = GetOptionSetting(userName, 5250);
            keyCloakConfiguration.client_id = GetOptionSetting(userName, 5251);
            keyCloakConfiguration.client_secret = GetOptionSetting(userName, 5252);
            keyCloakConfiguration.username = GetOptionSetting(userName, 5253);
            keyCloakConfiguration.Password = GetOptionSetting(userName, 5254);
            string sDecryptedPassword = string.Empty;
            sDecryptedPassword = GetOVal(keyCloakConfiguration.Password);
            keyCloakConfiguration.Password = sDecryptedPassword;
            keyCloakConfiguration.AdminGroupName = GetOptionSetting(userName, 5255);
            keyCloakConfiguration.TokenEndpoint = GetOptionSetting(userName, 5256);
            keyCloakConfiguration.grant_type = "password";
            if (IsEnableAuthenticationIntegration)
            {
                string adminGroup = keyCloakConfiguration.AdminGroupName;
                var userSync = new AuthenticationService(keyCloakConfiguration);
                string firstName;
                string lastName;
                if (string.IsNullOrEmpty(userDetails.FullUserName))
                {
                    firstName = userDetails.UserName;
                    lastName = userDetails.UserName;
                }
                else
                {
                    object[] str = userDetails.FullUserName.Split(' ');

                    if (str.Length > 0)
                    {
                        if (str.Length == 1)
                        {
                            firstName = Convert.ToString(str[0]);
                            lastName = userDetails.UserName;
                        }
                        else
                        {
                            firstName = Convert.ToString(str[0]);
                            lastName = Convert.ToString(str[str.Length - 1]);
                        }
                    }
                    else
                    {
                        firstName = userDetails.FullUserName;
                        lastName = userDetails.UserName;
                    }
                }

                var user = new UserRegisterRequestDTO(userDetails.UserName, userDetails.EmailAddress, userDetails.Password, adminGroup, "0", firstName, lastName, 0);
                try
                {
                    var id = userSync.GetUserAsync(userDetails.UserName).Result;
                    if (string.IsNullOrEmpty(id) == true)
                    {
                        userSync.RegisterUserAsync(user);
                    }
                    else
                    {
                        user = new UserRegisterRequestDTO(userDetails.UserName, userDetails.EmailAddress, userDetails.Password, adminGroup, id, firstName, lastName, 0);
                        userSync.UpdateUserAsync(user);
                    }
                }
                catch
                {
                    userSync.RegisterUserAsync(user);
                }
            }
        }
        public string DecryptPassword(string sEncryptedString, string sKey)
        {
            string sDecryptedValue = string.Empty;

            try
            {
                DES.Key = MD5Hash(sKey);
                DES.Mode = CipherMode.ECB;
                byte[] Buffer = Convert.FromBase64String(sEncryptedString);
                sDecryptedValue = Encoding.ASCII.GetString(DES.CreateDecryptor().TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch (Exception excep)
            {
                // Log Error Message
                //LogMessage(sUsername: "", iType: gPMConstants.PMELogLevel.PMLogOnError, sMsg: "Decrypt Failed", vApp: ACApp, vClass: ACClass, vMethod: "Decrypt", Information.Err().Number, vErrDesc: excep);
            }
            return sDecryptedValue;
        }
        public byte[] MD5Hash(string value)
        {
            return MD5.ComputeHash(Encoding.ASCII.GetBytes(value));
        }
        public string GetOVal(string encryptedtext)
        {
            string sRetVal = "";

            var TripleDes = new TripleDESCryptoServiceProvider();
            string sKey = "!@$1R1U5";
            TripleDes.Key = TruncateHash(sKey, TripleDes.KeySize / 8);
            TripleDes.IV = TruncateHash("", TripleDes.BlockSize / 8);


            try
            {

                // Convert the encrypted text string to a byte array. 
                byte[] encryptedBytes = Convert.FromBase64String(encryptedtext);

                // Create the stream. 
                var ms = new MemoryStream();
                // Create the decoder to write to the stream. 
                var decStream = new CryptoStream(ms, TripleDes.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write);

                // Use the crypto stream to write the byte array to the stream.
                decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                decStream.FlushFinalBlock();

                // Convert the plaintext stream to a string. 
                sRetVal = Encoding.Unicode.GetString(ms.ToArray());

                TripleDes = default;
            }
            catch (Exception ex)
            {
                return "";
            }

            return sRetVal;
        }
        private byte[] TruncateHash(string key, int length)
        {

            var sha1 = new SHA1CryptoServiceProvider();

            // Hash the key. 
            byte[] keyBytes = Encoding.Unicode.GetBytes(key);
            byte[] hash = sha1.ComputeHash(keyBytes);

            // Truncate or pad the hash. 
            Array.Resize(ref hash, length);
            return hash;
        }

    }
}