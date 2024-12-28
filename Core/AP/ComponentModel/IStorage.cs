namespace AP.ComponentModel;

public interface IStorage
{
    bool Commit();
    void Rollback();
}

public interface IStorageUser
{
    IStorage Storage { get; }
}
