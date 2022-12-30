using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallRecording.DAL.Models
{
    public class UserLogin
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
