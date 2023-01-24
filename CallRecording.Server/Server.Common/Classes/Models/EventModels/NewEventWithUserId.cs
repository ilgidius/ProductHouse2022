using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Common.Classes.Models.EventModels
{
    public class NewEventWithUserId : NewEvent
    {
        [Required(ErrorMessage = "User id is required.")]
        public long UserId { get; set; }
    }
}
