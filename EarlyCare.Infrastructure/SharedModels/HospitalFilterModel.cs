using System;
using System.Collections.Generic;
using System.Text;

namespace EarlyCare.Infrastructure.SharedModels
{
    public class HospitalFilterModel
    {
        public List<int> BedType { get; set; }
        public int CityId { get; set; }
    }
}
