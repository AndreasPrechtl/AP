namespace AP.Data;

public interface IEntitySetProvider
{
    IEntitySet<TEntity> GetEntitySet<TEntity>() where TEntity : class;
}
