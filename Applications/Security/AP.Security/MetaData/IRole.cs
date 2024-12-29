namespace AP.Security.MetaData
{
    public interface IRole
    {
        string Name { get; }
        string Description { get; set; }

        //ISetView<IUser> Users { get; }
        //IDictionaryView<IActivity, PermissionType> Permissions { get; }
    }
}
