using System;
using System.Collections.Generic;
using System.Text;

namespace EarlyCare.Core.Models
{
    public class BedCountDetails
    {
        public int VacantIsolationBeds { get; set; }
        public int VacantWithOxygenBeds { get; set; }
        public int VacantWithICUBeds { get; set; }
        public int VacantWithICUVentilatorBeds { get; set; }
        public int TotalIsolationBeds { get; set; }
        public int TotalWithOxygenBeds { get; set; }
        public int TotalWithICUBeds { get; set; }
        public int TotalWithICUVentilatorBeds { get; set; }


    }
}
