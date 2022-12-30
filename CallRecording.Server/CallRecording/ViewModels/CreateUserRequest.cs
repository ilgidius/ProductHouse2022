using System.ComponentModel.DataAnnotations;

namespace CallRecording.ViewModels
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username length can't be more than 50.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Role is required.")]
        [RegularExpression(@"(admin|user)", ErrorMessage = "Role does not match possible options.")]
        public string Role { get; set; }
    }
}