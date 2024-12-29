namespace AP.Data;

public partial interface IEntityModel :
    IQuery,
    ICreate,
    IUpdate,
    IDelete,
    IEntitySetProvider,
    IEntityFactory
{ }