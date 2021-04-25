namespace EarlyCare.Core.Models
{
    public class GoogleSheet
    {
        public int Id { get; set; }
        public GoogleSheetNames Name { get; set; }
        public string ToRange { get; set; }
    }

    public enum GoogleSheetNames
    {
        HospitalsBeds,
        PlasmaDonors,
        MedicalEquipment,
        MedicalOxygenProvider,
        FoodPackets,
        AmbulanceProvider,
        COVIDConsultancyDoc
    }
}