﻿using System;

namespace EarlyCare.Core.Models
{
    public class OxygenProvider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Area { get; set; }
        public string Charges { get; set; }
        public string GovRegistraionNumber { get; set; }
        public string Type { get; set; }
        public bool IsVerified { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public int CityId { get; set; }
        public bool IsSynced { get; set; }
    }
}