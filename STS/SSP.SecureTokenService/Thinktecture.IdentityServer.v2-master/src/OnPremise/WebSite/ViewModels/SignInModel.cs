/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Web.ViewModels
{
    public class SignInModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.SignInModel), ErrorMessageResourceName = "UsernameRequired")]
        [Display(Name = "UserName", ResourceType = typeof(Resources.SignInModel))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.SignInModel), ErrorMessageResourceName = "PasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.SignInModel))]
        public string Password { get; set; }

        [Display(Name = "EnableSSO", ResourceType = typeof(Resources.SignInModel))]
        public bool EnableSSO { get; set; }

        bool? isSigninRequest;
        [Display(Name = "IsSigninRequest", ResourceType = typeof(Resources.SignInModel))]
        public bool IsSigninRequest
        {
            get
            {
                if (isSigninRequest == null)
                {
                    isSigninRequest = !String.IsNullOrWhiteSpace(ReturnUrl);
                }
                return isSigninRequest.Value;
            }
            set
            {
                isSigninRequest = value;
            }
        }

        [Display(Name = "ReturnUrl", ResourceType = typeof(Resources.SignInModel))]
        public string ReturnUrl { get; set; }
    }
}