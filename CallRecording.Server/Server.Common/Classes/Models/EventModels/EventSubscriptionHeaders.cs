using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Common.Classes.Models.EventModels
{
    public class EventSubscriptionHeaders
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Added time is required.")]
        public string? Login { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Added time is required.")]
        [RegularExpression(@"(INIT|RINGING|START|VOICE|STOP)", ErrorMessage = "Event type does not match possible options.")]
        public string? EventType { get; set; }
    }
}
