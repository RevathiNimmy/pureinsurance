/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System;
using Thinktecture.IdentityServer.Protocols;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.ViewModels;
using Ssp.PureSts.Models;
using System.Web.SessionState;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;



namespace Thinktecture.IdentityServer.Web.ViewModels
{
    public class ChangePasswordModel
    {
        [Display(Name = "ReasonToForceChangePassword")]
        public String ReasonToForceChangePassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ChangePasswordModel), ErrorMessageResourceName = "PasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "OldPassword", ResourceType = typeof(Resources.ChangePasswordModel))]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(Resources.ChangePasswordModel), ErrorMessageResourceName = "NewPasswordRequired")]
        [PasswordValid()]
        [Display(Name = "NewPassword", ResourceType = typeof(Resources.ChangePasswordModel))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(Resources.ChangePasswordModel), ErrorMessageResourceName = "ConfirmPasswordRequired")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessageResourceType = typeof(Resources.ChangePasswordModel), ErrorMessageResourceName = "PasswordNotMatch")]
        [Display(Name = "ConfirmNewPassword", ResourceType = typeof(Resources.ChangePasswordModel))]
        public string ConfirmNewPassword { get; set; }
    }

    /// <summary>
    /// For clearing cache
    /// </summary>
    public class NoCache : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();

            base.OnResultExecuting(filterContext);
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class PasswordValidAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string sPwdRegex = HttpContext.Current.Session["PasswordRegularExpression"].ToString();
            Regex rgx = new Regex(sPwdRegex);
            
            if (value != null)
            {
                if (sPwdRegex != "")
                {
                    return rgx.IsMatch(value.ToString());
                }
                else
                {
                    if (value.ToString().Length >= 4)
                    {
                        return true;
                    }
                    else 
                    {
                        return false;
                    }
                }
                
            }
            else
            {
                return false;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            string sErrorMessage = HttpContext.Current.Session["InvalidPasswordMessage"].ToString(); ;
            return sErrorMessage;
        }
    }
}