using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyCare.WebApi.Models
{
    public class VerifyOTPRequestModel
    {
        public string MobileNumber { get; set; }
        public string Otp { get; set; }
    }
}
