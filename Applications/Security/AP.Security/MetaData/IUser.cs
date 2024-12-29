namespace AP.Security.MetaData
{
    public interface IUser
    {
        string Name { get; }
        string EmailAddress { get; }
        string DisplayName { get; }

        //ISetView<IRole> Roles { get; }
        //IDictionaryView<IActivity, PermissionType> Permissions { get; }
    }
}
