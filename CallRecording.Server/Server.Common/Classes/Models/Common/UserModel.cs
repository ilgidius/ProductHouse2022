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
