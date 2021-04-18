using EarlyCare.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
   public interface IOtpService
    {
      //  Task<int> GenerateOTP();
      //  Task<OtpDetails> InsertOtpDetails(OtpDetails otpDetails);
        Task<bool> SendVerificationOtp(string mobileNumber);
        Task<OtpDetails> GetOtpDetails(string mobileNumber, string otp);
        Task UpdateOtpDetailsAsync(string phoneNumber);
    }
}
