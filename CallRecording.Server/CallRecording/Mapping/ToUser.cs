using CallRecording.DAL.Models;
using CallRecording.ViewModels;

namespace CallRecording.Mapping
{
    public class ToUser
    {
        public static User CreateUserRequestToUser(CreateUserRequest createUserRequest)
        {
            return new User
            {
                Login = createUserRequest.Username,
                Password = createUserRequest.Password,
                Role = createUserRequest.Role
            };
        }
    }
}
