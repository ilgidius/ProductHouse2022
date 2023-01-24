using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Common.Classes.Models.EventModels
{
    public class NewEvent
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Added time is required.")]
        [DataType(DataType.DateTime)]
        public string? AddedTime { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Sent time is required.")]
        [DataType(DataType.DateTime)]
        public string? SentTime { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Event type is required.")]
        [RegularExpression(@"(INIT|RINGING|START|VOICE|STOP)", ErrorMessage = "Event type does not match possible options.")]
        public string? EventType { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Key is required.")]
        [MaxLength(50, ErrorMessage = "Key length can't be more than 50 characters.")]
        public string? Key { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Value is required.")]
        [MaxLength(100, ErrorMessage = "Value length can't be more than 100 characters.")]
        public string? Value { get; set; }
    }
}
