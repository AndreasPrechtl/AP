using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AP.Logging;

public abstract class LogSyncronizationContextUserBase
{
    public event LogEntryAddedEventHandler LogEntryAdded;

    /// <summary>
    /// Contains a reference to the underlying syncronizationContext
    /// </summary>
    protected readonly object SyncRoot;

    private LogSyncronizationContext _syncronizationContext;
    public LogSyncronizationContext SyncronizationContext => _syncronizationContext;

    internal LogSyncronizationContextUserBase(LogSyncronizationContext? syncronizationContext = null)
    {
        _syncronizationContext = syncronizationContext = syncronizationContext ?? new LogSyncronizationContext();
        SyncRoot = syncronizationContext.SyncRoot;
    }

    /// <summary>
    /// Raises the LogEntryAdded event
    /// </summary>
    /// <param name="entry"></param>
    protected virtual void OnLogEntryAdded(LogEntry entry)
    {
        LogEntryAddedEventHandler handler = LogEntryAdded;

        if (handler != null)
            handler(this, new LogEntryAddedEventArgs(entry));
    }
}

public abstract class LogReaderBase : LogSyncronizationContextUserBase
{
    protected LogReaderBase(LogSyncronizationContext? syncronizationContext = null)
        : base(syncronizationContext)
    { }

    public abstract IEnumerable<LogEntry> Read();
}

public class StreamingLogReader : LogReaderBase
{
    private readonly Activator<Stream> _streamCreator;
    private BufferedLogSyncronizationContext Buffer => (BufferedLogSyncronizationContext)SyncronizationContext;

    public StreamingLogReader(Activator<Stream> streamCreator, BufferedLogSyncronizationContext? buffer = null)
        : base(buffer ?? new BufferedLogSyncronizationContext(100))
    {
        ArgumentNullException.ThrowIfNull(streamCreator);

        _streamCreator = streamCreator;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
    public override IEnumerable<LogEntry> Read()
    {
        // ok here's what i need to do - read the buffer - inverted order, then the file also inverted? i donno yet...
        // todo: make it async .. ?? nouh?

        // read everything the buffer has to offer
        LogEntry[] snapshot = null;
        lock (SyncRoot)
            Buffer.GetSnapshot(out snapshot);

        List<LogEntry> entries = new(snapshot);

        const string begin = "<!-- entry begin -->";
        lock (SyncRoot)
        {
            Stream stream = null;

            try
            {
                stream = _streamCreator();

                using (StreamReader reader = new(stream))
                {
                    StringBuilder sb = new();

                    string current = reader.ReadLine();

                    while (current != null)
                    {
                        if (current == begin)
                        {
                            current = reader.ReadLine();

                            while (current != begin)
                            {
                                sb.Append(current);
                                current = reader.ReadLine();
                            }
                            entries.Add(Serialization.Deserialize.Xaml<LogEntry>(sb.ToString()));
                            sb.Clear();
                        }
                    }
                }
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }
        }
        return entries;
    }
}
