using EarlyCare.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IOtpRepository
    {
        Task<OtpDetails> InsertOtpDetails(OtpDetails otpDetails);
        Task<OtpDetails> GetOtpDetailsAsync(string mobileNumber, string otp);
        Task UpdateOtpDetailsAsync(string mobileNumber);
    }
}
