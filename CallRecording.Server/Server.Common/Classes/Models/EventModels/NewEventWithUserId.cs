using System.ComponentModel.DataAnnotations;

namespace Server.Common.Classes.Models.EventModels
{
    public class NewEventWithUserId : NewEvent
    {
        [Required(ErrorMessage = "User id is required.")]
        public long UserId { get; set; }
    }
}
