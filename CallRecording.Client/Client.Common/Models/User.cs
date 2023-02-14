using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Common.Models
{
    public class User
    {
        public string? Username { get; private set; }
        public string? Password { get; private set; }
        public string? Role { get; private set; }

        public User(string username, string password, string role)
        {
            Username = username;
            Password = password;
            Role = role;
        }
        public User()
        {
            Console.Write("Enter username for a new user: ");
            Username = Console.ReadLine();
            Console.Write("Enter password for a new user: ");
            Password = Console.ReadLine();
            Console.Write("Enter role for a new user: ");
            Role = Console.ReadLine();
        }
        public User(string stubString)
        {
            Console.Write("Enter username for whom need a password changing: ");
            Username = Console.ReadLine();
            Console.Write("Enter new password: ");
            Password = Console.ReadLine();
        }
    }
}
