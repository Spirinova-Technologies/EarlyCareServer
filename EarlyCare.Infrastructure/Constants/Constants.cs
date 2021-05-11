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

        public const string NewUserEmailBody = "Welcome to Earlycare. Your account has been created.";
        public const string NewUserEmailSubject = "Welcome to Earlycare";
        public const string PlasmaDetailsUpdatedEmailBody = "Thanks {0} for registering with us as a plasma donor. Your details have been verified by {1}. It will be shown to users.";
        public const string PlasmaDetailsUpdatedEmailSubject = "Earlycare - Plasma details verified";
        public const string OxygenDetailsUpdatedEmailBody = "Thanks {0} for registering with us as an oxygen supplier. Your details have been verified by {1}. It will be shown to users.";
        public const string OxygenUpdatedEmailSubject = "Earlycare - Oxygen details verified";
        public const string FoodDetailsUpdatedEmailBody = "Thanks {0} for registering with us as a food/tiffin supplier. Your details have been verified ny {1}. It will be shown to users.";
        public const string FoodDetailsUpdatedEmailSubject = "Earlycare - Food/tiffin supplier details verified";
        public const string VolunteerApprovedEmailSubject = "Earlycare - Volunteer approved";
        public const string VolunteerApprovedEmailBody = "Thanks {0} for registering with us as a volunteer. Your registration has been approved by {1}. As a volunteer you will be able to validate/verify service providers. We show only verified suppliers in our app. So please verify a service provider before marking it as verified in the app.";
        public const string VolunteerUnapprovedEmailSubject = "Earlycare - Volunteer has been unapproved.";
        public const string VolunteerUnapprovedEmailBody = "Thanks {0} for registering with us as a volunteer. Your registration has been rejected by {1}. Please contact us if this was a mistake.";
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