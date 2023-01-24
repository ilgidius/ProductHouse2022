using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Common.Classes.Models.Common
{
    public class UserModel
    {
        public long Id { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}
