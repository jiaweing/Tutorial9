using System.ComponentModel.DataAnnotations;

namespace Web.Models.Api
{
    public class RegisterRequest
    {
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Required(ErrorMessage = "Email is required")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public int Role { get; set; }
    }
}
