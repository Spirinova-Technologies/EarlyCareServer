using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyCare.WebApi.Models
{
    public class VerifyOtpResponseModel : BaseResponseModel
    {
        public bool IsOtpVerified { get; set; }
        public bool IsNewUser { get; set; }

    }
}
