namespace EarlyCare.Infrastructure.SharedModels
{
    public class UpdateVerificationStatusModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool MarkVerified { get; set; }
    }
}