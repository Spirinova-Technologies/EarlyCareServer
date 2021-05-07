using System.Collections.Generic;

namespace EarlyCare.WebApi.Models
{
    public class CreateUserRequestModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
        public int CityId { get; set; }
        public string Otp { get; set; }
        public int UserType { get; set; }
        public List<int> Services { get; set; }
    }
}