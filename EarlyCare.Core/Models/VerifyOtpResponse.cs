using System;
using System.Collections.Generic;
using System.Text;

namespace EarlyCare.Core.Models
{
    public class VerifyOtpResponse
    {
        public bool IsOtpVerified { get; set; }
        public bool IsNewUser { get; set; }
    }
}
