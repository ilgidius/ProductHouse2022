using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class AddEventsWithId
    {
        public long UserId { get; set; }
        public string? AddedTime { get; set; }
        public string? SentTime { get; set; }
        public string? EventType { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
    }
}
