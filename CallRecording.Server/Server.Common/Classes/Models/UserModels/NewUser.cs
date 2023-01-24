using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Common.Classes.Models.UserModels
{
    public class NewUser
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required.")]
        [MinLength(3, ErrorMessage = "Username length can't be less than 3 characters.")]
        [MaxLength(50, ErrorMessage = "Username length can't be more than 50 characters.")]
        public string? Login { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required.")]
        public string? Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Role is required.")]
        [RegularExpression(@"(admin|user)", ErrorMessage = "Role does not match possible options.")]
        public string? Role { get; set; }
    }
}
