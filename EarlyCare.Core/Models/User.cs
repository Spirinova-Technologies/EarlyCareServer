using System;

namespace EarlyCare.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
        public bool IsVerified { get; set; }
        public string ProfilePhoto { get; set; }
        public int CityId { get; set; }
        public bool IsActive { get; set; }
        public int UserType { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByUser { get; set; }
        public string HasApprovePermission { get; set; }
    }
}