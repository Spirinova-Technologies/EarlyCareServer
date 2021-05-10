using System.Collections.Generic;

namespace EarlyCare.Infrastructure.SharedModels
{
    public class UserResponseModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string ProfilePhoto { get; set; }
        public string AccessToken { get; set; }
        public int UserType { get; set; }
        public int CityId { get; set; }
        public List<ServiceData> Services { get; set; }
        public bool IsVerified { get; set; }
    }

    public class ServiceData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }
}