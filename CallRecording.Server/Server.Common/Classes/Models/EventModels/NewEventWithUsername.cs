using System.ComponentModel.DataAnnotations;

namespace Server.Common.Classes.Models.EventModels
{
    public class NewEventWithUsername : NewEvent
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required.")]
        [MinLength(3, ErrorMessage = "Username length can't be less than 3 characters.")]
        [MaxLength(50, ErrorMessage = "Username length can't be more than 50 characters.")]
        public string? Login { get; set; }
    }
}
