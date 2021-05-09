using System.Collections.Generic;

namespace EarlyCare.WebApi.Models
{
    public class UpdateUserRequestModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int UserType { get; set; }
        public List<int> Services { get; set; }
    }
}