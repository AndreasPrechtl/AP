namespace AP.UniformIdentifiers;

public interface IPasswordProtectableUri : IUriWithUserName
{
    string Password { get; }
    //string PasswordSeparator { get; }
}
