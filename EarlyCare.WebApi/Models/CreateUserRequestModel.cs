using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyCare.WebApi.Models
{
    public class CreateUserRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string Otp { get; set; }
        public string SocialId { get; set; }
        public string Role { get; set; }
        public string Address { get; set; }
    }
}
