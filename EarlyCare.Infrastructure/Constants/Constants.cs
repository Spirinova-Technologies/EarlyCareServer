namespace EarlyCare.Infrastructure.Constants
{
    public static class Constants
    {
        public static string EnableSMS = "EnableSMS";
        public static string DefaultOTP = "DefaultOTP";
        public static string FromEmail = "FromEmail";
        public static string BccEmails = "BccEmails";

        public static string IsolationBeds = "Isolation Beds";
        public static string ICUBeds = "ICU Beds";
        public static string OxigenBed = "Oxigen Bed";
        public static string VentilatorBeds = "VentilatorBeds";

        public const string NewUserEmailBody = "New account has beed created.";
        public const string NewUserEmailSubject = "Welcome to Earlycare";
        public const string PlasmaDetailsUpdatedEmailBody = "Your plasma details approved";
        public const string PlasmaDetailsUpdatedEmailSubject = "Plasma details has beed approved";
        public const string OxygenDetailsUpdatedEmailBody = "Your oxygen details approved";
        public const string OxygenUpdatedEmailSubject = "Oxygen details has beed approved";
        public const string FoodDetailsUpdatedEmailBody = "Your food details approved";
        public const string FoodDetailsUpdatedEmailSubject = "Oxygen details has beed approved";
    }

    public static class GoogleSheetNameConstants
    {
        public static string HospitalsBeds = "HospitalsBeds";
        public static string PlasmaDonors = "PlasmaDonors";
        public static string MedicalEquipment = "MedicalEquipment";
        public static string MedicalOxygenProvider = "MedicalOxygenProvider";
        public static string FoodPackets = "FoodPackets";
        public static string AmbulanceProvider = "AmbulanceProvider";
        public static string COVIDConsultancyDoc = "COVIDConsultancyDoc";
    }
}