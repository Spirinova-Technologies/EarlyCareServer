using System;
using System.Collections.Generic;
using System.Text;

namespace EarlyCare.Core.Models
{
   public class Ambulance
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
    }
}
