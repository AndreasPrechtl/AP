namespace AP.Data;

public interface IEntityContext : 
    IEntityModelProvider, 
    IEntitySetProvider, 
    IEntityFactory, 
    IPersist, 
    AP.IContextDependentDisposable
{ }
