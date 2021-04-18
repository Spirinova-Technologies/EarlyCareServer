using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyCare.WebApi.Models
{
    public class BedDetailsModel
    {
        public string BedType { get; set; }
        public int TotalBeds { get; set; }
        public int OccupiedBeds { get; set; }
        public int VacantBeds { get; set; }
    }
}
