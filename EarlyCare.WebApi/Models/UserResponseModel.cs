using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyCare.WebApi.Models
{
    public class UserResponseModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool IsVerified { get; set; }
        public string ProfilePhoto { get; set; }
        public string AccessToken { get; set; }
        public string SocialId { get; set; }
        public string Role { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }
}
