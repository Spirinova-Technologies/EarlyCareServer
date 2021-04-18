using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarlyCare.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string Otp { get; set; }
        public bool IsVerified { get; set; }
        public string ProfilePhoto { get; set; }
        public string AccessToken { get; set; }
        public string SocialId { get; set; }
        public string Role { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
