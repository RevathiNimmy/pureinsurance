/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Protocols;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.ViewModels;
using Ssp.PureSts.Models;
using System.Web.SessionState;
using System.Web.Configuration;

namespace Thinktecture.IdentityServer.Web.Controllers
{
    //[SessionState(SessionStateBehavior.Required)]
    public class AccountController : AccountControllerBase
    {
        public AccountController() : base()
        { }

        public AccountController(IUserRepository userRepository, IConfigurationRepository configurationRepository) : base(userRepository, configurationRepository)
        { }
        
        // shows the signin screen
        public ActionResult SignIn(string returnUrl, bool mobile=false)
        {
            // you can call AuthenticationHelper.GetRelyingPartyDetailsFromReturnUrl to get more information about the requested relying party
            Session["ReturnUrl"] = returnUrl;
            var vm = new SignInModel()
            {
                ReturnUrl = returnUrl
            };

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                vm.IsSigninRequest = true;
            }
            return View(vm);
        }

        public ActionResult Error()
        {
            
                ViewData["ValidateSignInUrlMessage"] = "Request url have been tampered.";
           
            return View();
        }

        // handles the signin
        [HttpPost]
        [Thinktecture.IdentityServer.Web.GlobalFilter.PreventFromUrlFilter]
        public ActionResult SignIn(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                int iPasswordExpiryDuration=0;
                bool bForceToChangePassword = false;

                 UserAttributes objUserAttributes = UserRepository.ValidateUser(model.UserName, model.Password);

                 Session["User"] = objUserAttributes;

                 if (objUserAttributes.IsAuthenticated)
                 {
                     // establishes a principal, set the session cookie and redirects
                     // you can also pass additional claims to signin, which will be embedded in the session token
                     if (objUserAttributes.IsTempPassword || objUserAttributes.IsWeakPassword || objUserAttributes.IsSystemUpgradeChangePasswordRequired)
                     {
                         bForceToChangePassword = true;
                     }
                     else
                     {
                         //Get Password expiry days using system option 
                         string sPasswordExpiryDuration = UserRepository.GetOptionSetting(model.UserName, 5103);
                         iPasswordExpiryDuration = int.Parse(sPasswordExpiryDuration);

                         if (!string.IsNullOrEmpty(sPasswordExpiryDuration) && objUserAttributes.PasswordChangeDate.HasValue && iPasswordExpiryDuration > 0)
                         {
                             
                             objUserAttributes.PasswordExpiryDate = objUserAttributes.PasswordChangeDate.Value.AddDays(iPasswordExpiryDuration);
                         }

                         Session["User"] = objUserAttributes;

                         if (objUserAttributes.PasswordExpiryDate.HasValue)
                         {
                             if (objUserAttributes.PasswordExpiryDate.Value < DateTime.Today)
                             {
                                 bForceToChangePassword = true;  
                             }
                         }
                     }

                     if (bForceToChangePassword == true)
                     {
                         Session["ReturnUrl"] = model.ReturnUrl;
                         return RedirectToAction("ChangePassword", "Account");
                     }
                     else
                     {
                         return SignIn(
                                 model.UserName,
                                 AuthenticationMethods.Password,
                                 model.ReturnUrl,
                                 model.EnableSSO,
                                 ConfigurationRepository.Global.SsoCookieLifetime);
                     }
                 }
                 else
                 {
                     if (objUserAttributes.IsLocked)
                     {
                         ModelState.AddModelError("", Resources.AccountController.IsLocked);                       
                     }
                     else if (objUserAttributes.IsInvalidPassword)
                     {
                         ModelState.AddModelError("", Resources.AccountController.IsInvalidPassword);
                     }
                     else if (!string.IsNullOrEmpty(objUserAttributes.ErrorMessage))
                     {
                         ModelState.AddModelError("", objUserAttributes.ErrorMessage);
                     }
                     else
                     {
                         ModelState.AddModelError("", Resources.AccountController.LoginError);
                     }
                     return View(model);
                 }
            }

            return View(model);
        }

        /// <summary>
        /// Shows Change password screen
        /// </summary>
        /// <returns></returns>
        //[NoCache]
        public ActionResult ChangePassword()
        {
            UserAttributes oUserAttributes = (UserAttributes)Session["User"];
            string sPwdRegex = UserRepository.GetOptionSetting(oUserAttributes.UserName, 5101);
            string sInvalidPwdMessage = UserRepository.GetOptionSetting(oUserAttributes.UserName, 5113);
            string sReasonToForceChangePassword = "";

            if (oUserAttributes.IsTempPassword)
            {
                sReasonToForceChangePassword = Resources.AccountController.ChangeTempPassword;
            }
            else if (oUserAttributes.IsWeakPassword)
            {
                sReasonToForceChangePassword = sInvalidPwdMessage;
            }
            else if (oUserAttributes.PasswordExpiryDate < DateTime.Today)
            {
                sReasonToForceChangePassword = Resources.AccountController.ChangeExpiredPassword;
            }
            else if (oUserAttributes.IsSystemUpgradeChangePasswordRequired )
            {
                sReasonToForceChangePassword = Resources.AccountController.SystemUpgradeChangePasswordRequired;
            }
            else
            {
                sReasonToForceChangePassword = Resources.AccountController.ChangeOtherPassword;
            }
            ViewBag.ReasonToForceChangePassword = sReasonToForceChangePassword;
            // you can call AuthenticationHelper.GetRelyingPartyDetailsFromReturnUrl to get more information about the requested relying party

            Session["PasswordRegularExpression"] = sPwdRegex;
            
            if (sInvalidPwdMessage != "")
            {
                Session["InvalidPasswordMessage"] = sInvalidPwdMessage;
            }
            else
            {
                Session["InvalidPasswordMessage"] = Resources.AccountController.ChangeWeakPassword;
            }

            var vm = new ChangePasswordModel()
            {
                ReasonToForceChangePassword = sReasonToForceChangePassword
            };

            return View(vm);
        }

        // handles the changepassword postback event
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.OldPassword == model.NewPassword)
                {
                    ModelState.AddModelError("", Resources.ChangePasswordModel.OldNewPasswordSame);
                    return View(model);
                }
                UserAttributes objUserAttributes = (UserAttributes)Session["User"];
                string sErrorCode = "";
                String sErrorMessage = "";
                bool bResult = UserRepository.ChangePassword(objUserAttributes.UserName, model.OldPassword, model.NewPassword, ref sErrorCode, ref sErrorMessage);
                if (bResult == true)
                {
                    string sReturnUrl = "";
                    if(Session["ReturnUrl"]!=null)
                    {
                        sReturnUrl = Session["ReturnUrl"].ToString();
                    }
                    return SignIn(
                                objUserAttributes.UserName,
                                AuthenticationMethods.Password,
                                sReturnUrl,
                                true,
                                ConfigurationRepository.Global.SsoCookieLifetime);
                }
                else
                {
                    //Show error message as per error code returned
                    switch (sErrorCode)
                    {
                        case "290": //Incorrect username
                            ModelState.AddModelError("", Resources.ChangePasswordModel.IncorrectUserName);
                            break;
                        case "291": //Incorrect existing Password
                            ModelState.AddModelError("", Resources.ChangePasswordModel.IncorrectPassword);
                            break;
                        case "301": //Weak Password
                            ModelState.AddModelError("", Resources.ChangePasswordModel.WeakPassword);
                            break;
                        case "302": //ReUsedPassword
                            ModelState.AddModelError("", Resources.ChangePasswordModel.ReusedPassword);
                            break;
                        default: //Unknow error
                            ModelState.AddModelError("", Resources.ChangePasswordModel.ChangePwdUnsuccessful);
                            break;
                    }

                    return View(model);
                }
            }

            return View(model);
        }

        // handles client certificate based signin - not in use
        public ActionResult CertificateSignIn(string returnUrl)
        {
            if (!ConfigurationRepository.Global.EnableClientCertificateAuthentication)
            {
                return new HttpNotFoundResult();
            }

            var clientCert = HttpContext.Request.ClientCertificate;

            if (clientCert != null && clientCert.IsPresent && clientCert.IsValid)
            {
                string userName;
                if (UserRepository.ValidateUser(new X509Certificate2(clientCert.Certificate), out userName))
                {
                    // establishes a principal, set the session cookie and redirects
                    // you can also pass additional claims to signin, which will be embedded in the session token

                    return SignIn(
                        userName, 
                        AuthenticationMethods.X509, 
                        returnUrl, 
                        false, 
                        ConfigurationRepository.Global.SsoCookieLifetime);
                }
            }

            return View("Error");
        }

        // shows the signin screen
    }
}