namespace CallRecording.Common.IUser
{
    public interface IUserValidation<TUser, TLoginUser> 
        where TUser : class 
        where TLoginUser : class
    {
        TUser? Authentificate(TLoginUser userLogin);
        string Generate(TUser user);
    }
}
