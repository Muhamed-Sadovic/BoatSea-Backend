using System.ComponentModel.DataAnnotations.Schema;

namespace BoatSea.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "User";
        [NotMapped]
        public IFormFile Image {  get; set; }
        public string ImageName { get; set; }
        public string VerificationCode { get; set; }
        public bool IsVerified { get; set; } = false;
        public ICollection<Rent> Rents { get; set; }

    }
    public class VerificationRequest
    {
        public string Code { get; set; }
    }
    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
    }
    public class ResetPasswordRequest
    {
        public string NewPassword { get; set; }
    }
}
