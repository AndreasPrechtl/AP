namespace AP.Data;

public interface IPersistor : AP.IDisposable
{
    void Save();
    void Discard();

    SaveMode SaveMode { get; }
}
