using System.Collections.Generic;

namespace AP.Logging;

public delegate void LogEntryAddedEventHandler(object sender, LogEntryAddedEventArgs e);

public class LogSynchronizationContext
{
    public readonly object SyncRoot = new();

    public LogSynchronizationContext()
    { }
}

public class BufferedLogSynchronizationContext : LogSynchronizationContext
{
    private List<LogEntry> _buffer;
    private int _minimumBufferSize;

    public BufferedLogSynchronizationContext(int minimumBufferSize = 100)
    {
        _buffer = new List<LogEntry>(minimumBufferSize);
        _minimumBufferSize = minimumBufferSize;
    }

    public void Add(LogEntry entry)
    {
        lock (SyncRoot)
            _buffer.Add(entry);
    }

    public int Count
    {
        get
        {
            lock (SyncRoot)
                return _buffer.Count;
        }
    }

    public void Clear()
    {
        lock (SyncRoot)
            _buffer.Clear();
    }

    public void GetSnapshot(out LogEntry[] entries)
    {
        lock (SyncRoot)
            entries = _buffer.ToArray();
    }

    public bool IsFilled
    {
        get
        {
            lock (SyncRoot)
                return _buffer.Count >= _minimumBufferSize;
        }
    }
}
