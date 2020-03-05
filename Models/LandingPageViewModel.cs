using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kanopy.Models
{
    public class LandingPageViewModel
    {
        public LoginViewModel Login { get; set; }
        public RegisterViewModel Register { get; set; }
        public ForgotPasswordViewModel ForgotPassword { get; set; }
    }
}