namespace AP.Security.MetaData
{
    public interface IActivity
    {
        string Name { get; }
        string Description { get; set; }

        //IDictionaryView<IUser, PermissionType> UserPermissions { get; }
        //IDictionaryView<IRole, PermissionType> RolePermissions { get; }
    }
}
