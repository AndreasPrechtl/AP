namespace AP.Data;

public interface IEntityContextUser
{
    IEntityContext EntityContext { get; }
}

public interface IEntityContextUser<out TDataContext> : IEntityContextUser
    where TDataContext : class, IEntityContext
{
    new TDataContext EntityContext { get; }
}
