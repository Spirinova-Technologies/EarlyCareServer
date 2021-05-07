namespace EarlyCare.WebApi.Models
{
    public class OxygenDetailsRequestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Area { get; set; }
        public string Charges { get; set; }
        public string GovRegistraionNumber { get; set; }
        public string Type { get; set; }
        public bool IsVerified { get; set; }
        public int UserId { get; set; }
        public int CityId { get; set; }
    }
}